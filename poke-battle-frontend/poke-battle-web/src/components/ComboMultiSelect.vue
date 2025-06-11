<template>
    <div class="form-group">
         <div v-if="caption" class="form-label">{{ caption }}</div>
         <div class="form form-multi-combo" :class="{disabled: props.disabled, readonly: props.readonly}">
             <div class="multi-combo-header">
                 <input :disabled="props.disabled" @focus="openDropsown" type="text" v-model="inputModel" class="multi-combo-search" :placeholder="selectedLabels?.length ? '': 'Seleccionar...'" />
                 <button :disabled="props.disabled" type="button" class="multi-combo-toogle" @click="toogleDropdown">▼</button>
             </div>
     
             <div class="multi-combo-chips" v-if="isMultiple && selectedLabels?.length">
                 <span class="chip" v-for="i in selectedItems" :key="i.key">{{ i.value }}
                     <button :disabled="props.readonly || props.disabled" class="chip-remove" @click="removeOption(i.key)">x</button>
                 </span>
             </div>
     
             <div v-show="isOpen" class="multi-combo-dropdown">
                 <div @click.stop="toogleSelection(item.key)" class="multi-combo-option" :class="{selected: internalSelected.includes(item.key!)}" v-for="item in filteredItems" :key="item.key">
                     <template v-if="type === 'text-with-image'"> 
                         <span v-if="item.icon" class="option-icon">
                             <img :src="item.icon" :alt="item.value"/>
                         </span>
                         {{ item.value }}
                     </template>
                     <template v-else-if="type === 'only-image'">
                         <span v-if="item.icon" class="option-icon">
                             <img :src="item.icon" :alt="item.value"/>
                         </span>
                     </template>
                     <template v-else-if="type === 'only-text'">
                         {{ item.value }}
                     </template>
                 </div>
             </div>
         </div>
    </div>
</template>

<script setup lang="ts">
import type { LCombo } from '@/models/LCombo';
import type { TypeOption } from '@/models/LComboItem';
import { computed, ref } from 'vue';

const props = defineProps<{
    model: LCombo
    keysSelected: string[],
    type: TypeOption
    caption?: string,
    maxSelected?: number,
    disabled?: boolean,
    readonly?: boolean
}>()

const isOpen = ref<boolean>(false);
const filter = ref<string>('');
const internalSelected = ref<string[]>([...props.keysSelected])

const emit = defineEmits(['update-selected'])

const selectedLabels = computed(() => {
    return props.model.items?.filter(i => i.key !== undefined && internalSelected.value.includes(i.key!)).map(i => i.value)
});

const filteredItems = computed(() => {
    if(!filter.value) return props.model.items
    return props.model.items?.filter(i => i.value?.toLowerCase().includes(filter.value.toLowerCase()));
})

const selectedItems = computed(() => {
    return props.model.items?.filter(i => internalSelected.value.includes(i.key!))
})

const max = computed(() => Math.min(props.maxSelected ?? 1, props.model.items?.length))

const isMultiple = computed(() => max.value > 1)

const singleSelectedItem = computed(() => {
    if(!isMultiple.value)
        return props.model.items.find(i => internalSelected.value.includes(i.key!)) ?? null;
    return null;
})

const inputModel = computed({
    get() {
        return isMultiple.value ? filter.value : singleSelectedItem.value?.value ?? filter.value;
    },
    set(val: string) {
        filter.value = val;
    }
})

function toogleDropdown() {
    isOpen.value = !isOpen.value;
}

function openDropsown() {
    isOpen.value = true;
}

function removeOption(key: string) {
    if(props.readonly || props.disabled) return;
    const index = internalSelected.value.indexOf(key);
    if(index !== -1) {
        internalSelected.value.splice(index, 1);
        emitSelection();
    }
    
}

function toogleSelection(key: string) {
    if(props.readonly || props.disabled) return;

    if(isMultiple.value){
        const index = internalSelected.value.indexOf(key);
        if(index !== -1)
            internalSelected.value.splice(index, 1);
        else if(internalSelected.value.length < max.value)
            internalSelected.value.push(key);

    }else {
        internalSelected.value = [key]
        filter.value = ''
        isOpen.value = false 
    }

    emitSelection();
}

function emitSelection() {
    props.model.items.forEach(i => {
        i.checked = internalSelected.value.includes(i.key!);
    })
    emit('update-selected', internalSelected.value);
}

</script>

<style lang="scss" scoped>
.form.form-multi-combo 
{
    position: relative;
    width: 100%;
    border: 1px solid #ccc;
    border-radius: 6px;
    padding: .5rem;
    background-color: #fff;

    .multi-combo-header {
        display: flex;
        align-items: center;

        .multi-combo-search {
            flex: 1;
            border: none;
            outline: none;
            font-size: 1rem;
        }

        .multi-combo-toogle {
            background: none;
            border: none;
            font-size: 1.2rem;
            padding: 0 .5rem;
            cursor: pointer;
        }
    }

    .multi-combo-chips {
        margin-top: .5rem;
        display: flex;
        flex-wrap: wrap;
        gap: .4rem;

        .chip {
            background-color: #e0e0e0;
            padding: .25rem .5rem;
            border-radius: 1rem;
            color: black;
            display: flex;
            align-items: center;
            font-size: .9rem;

            .chip-remove {
                background: none;
                border: none;
                margin-left: .4rem;
                cursor: pointer;
                font-weight: bold;
            }

            .chip-remove:hover{
                color: red;
            }
        }
    }

    .multi-combo-dropdown {
        position: absolute;
        top: 100%;
        left: 0;
        right: 0;
        background: white;
        border: 1px solid #ccc;
        max-height: 200px;
        overflow-y: auto;
        z-index: 10;
        margin-top: .25rem;
        padding: .5rem;
        box-shadow: 0 4px 12px rgba(0,0,0, .1);

        .multi-combo-option {
            padding: .4rem;
            border-radius: 4px;
            cursor: pointer;
            display: flex;
            align-items: center;
            color: black;
            gap: .5rem;
            transition: background-color .2s ease;
        }

        .multi-combo-option:hover {
            background-color: #f0f0f0;
        }

        .multi-combo-option.selected {
            background-color: #dbeafe;
            font-weight: bold;
        }

        .multi-combo-option {

            .option-icon img {
                width: 1rem;
                height: 1rem;
            }
        }
    }
}

.form.form-multi-combo.disabled {
    background-color: #f5f5f5;
    pointer-events: none;
    opacity: .6;
}

.form.form-multi-combo.readonly {
    background-color: #fafafa;
}
</style>