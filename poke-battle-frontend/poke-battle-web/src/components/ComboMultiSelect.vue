<template>
    <div class="combo-wrapper" >
        <div v-if="caption" class="form-label">{{ caption }} <span v-if="invalid" class="required-icon" title="Campo obligatorio">❗</span>  </div>
        <div class="form form-multi-combo" :class="{disabled: props.disabled, readonly: props.readonly, 'is-invalid': invalid}">
            <!-- Combo -->
            <div class="multi-combo-header">
                <input :disabled="props.disabled" @focus="() => { isOpen = true }" type="text" v-model="filter" class="multi-combo-search" placeholder="Seleccionar..." />
                <button v-if="!props.disabled && (filter || internalSelected.length > 0)" type="button" class="multi-combo-clear" @click.self="clearSelection" title="Limpiar seleccion">✖</button>
                <button :disabled="props.disabled" type="button" class="multi-combo-toogle" @click="toogleDropdown" title="Desplegar">▼</button>
            </div>
    
            <!-- Chips-->
            <div class="multi-combo-chips-wrapper" v-if="visibleChips.length != 0">
                <div ref="chipContainer" class="multi-combo-chips">
                    <span class="chip" v-for="(item, index) in visibleChips" :key="item.key">
                        {{ item.value }} <button class="chip-remove" @click="removeOption(item.key)">X</button>
                    </span>
                    <span v-if="hiddenChipsCount > 0" class="chip more-chip" @click="showAll = !showAll">
                        +{{ hiddenChipsCount }}
                    </span>
                </div>
            </div>
            
            <!-- Dropdown -->
            <div v-show="isOpen" class="multi-combo-dropdown">
                <div @click.stop="toogleSelection(item.key)" class="multi-combo-option" :class="{selected: internalSelected.includes(item.key!)}" v-for="item in filteredItems" :key="item.key">
                    <template v-if="type === 'text-with-image'"> 
                        <span v-if="item.icon" class="option-icon">
                            <img :src="item.icon" :alt="item.value"/>
                        </span>
                        <span class="option-label"> {{ item.value }} </span> 
                    </template>
                    <template v-else-if="type === 'only-image'">
                        <span v-if="item.icon" class="option-icon">
                            <img :src="item.icon" :alt="item.value"/>
                        </span>
                    </template>
                    <template v-else-if="type === 'only-text'">
                        <span class="option-label"> {{ item.value }} </span> 
                    </template>
                </div>
            </div>

        </div>

    </div>
</template>

<script setup lang="ts">
import type { LCombo } from '@/models/LCombo';
import type { TypeOption } from '@/models/LComboItem';
import { computed, nextTick, onMounted, onUnmounted, ref, watch } from 'vue';

const props = defineProps<{
    model: LCombo
    keysSelected: string[],
    type: TypeOption
    caption?: string,
    maxSelected?: number,
    disabled?: boolean,
    readonly?: boolean,

    required?: boolean,
    isInvalid?: boolean,
    maxVisibleChips?: number,
}>()

const isOpen = ref<boolean>(false);
const filter = ref<string>('');
const internalSelected = ref<string[]>([...props.keysSelected])
const chipContainer = ref<HTMLElement | null>(null);
const showAll = ref(false);
const chipWidths = ref<number[]>([]);


const emit = defineEmits(['update-selected'])

const visibleChips = computed(() => {
    if(showAll.value || !chipContainer.value) return selectedItems.value;
    let totalWidth = 0;
    const maxWidth = chipContainer.value.clientWidth;
    const result: typeof selectedItems.value = [];

    chipWidths.value.forEach((width, i) => {
        const chip = selectedItems.value[i];
        if(!chip) return;
        if(totalWidth + width + 60 < maxWidth) {
            result.push(chip);
            totalWidth += width;
        }
    })

    return result;
});

const hiddenChipsCount = computed(() => {
    return selectedItems.value.length - visibleChips.value.length;
})

const filteredItems = computed(() => {
    if(!filter.value) return props.model.items
    return props.model.items?.filter(i => i.value?.toLowerCase().includes(filter.value.toLowerCase()));
})

const selectedItems = computed(() => {
    return props.model.items?.filter(i => internalSelected.value.includes(i.key!))
})

const max = computed(() => Math.min(props.maxSelected ?? 1, props.model.items?.length))

const isMultiple = computed(() => max.value > 1)

const invalid = computed(() => {
    if(props.isInvalid) return true;
    if(props.required) return internalSelected.value.length === 0;
    return false;
})

function updateChipsWidths() {
    nextTick(() => {
        if(!chipContainer.value) return;
        const elements = chipContainer.value.querySelectorAll('.chip');
        chipWidths.value = Array.from(elements).map(el => (el as HTMLElement).offsetWidth);
    });
}

function toogleDropdown() {
    console.log('toogleDropdown', isOpen.value);
    isOpen.value = !isOpen.value;
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

function clearSelection() {
    internalSelected.value = [];
    filter.value = '';
    isOpen.value = false;
    emitSelection();
}

defineExpose({
    clearSelection
})

onMounted(() => {
    window.addEventListener('resize', updateChipsWidths);
})
onUnmounted(() => { window.removeEventListener('resize', updateChipsWidths)});
</script>

<style lang="scss" scoped>
.combo-wrapper{ display: contents;}
.form.form-multi-combo 
{
    position: relative;
    border: 1px solid #ccc;
    border-radius: 6px;
    padding: .5rem;
    background-color: #fff;

    &.is-invalid {
        border-color: #e53935;
        background-color: #fff5f5;
    }
}



// Header
.multi-combo-header {
    display: flex;
    align-items: center;
    gap: .25rem;

    .multi-combo-search {
        flex: 1;
        border: none;
        outline: none;
        font-size: 1rem;
        padding: .25rem;
        background-color: transparent;
    }

    .multi-combo-toogle, .multi-combo-clear {
        background: none;
        border: none;
        font-size: 1.1rem;
        cursor: pointer;
        padding: .25rem .5rem;
        color: #555;
        transition: color .2s ease;

        &:hover {
            color: #000;
        }

        &:disabled {
            cursor: not-allowed;
            color: #aaa;
        }
    }
}


// Chip container
.multi-combo-chips-wrapper {
  display: flex;
  align-items: center;
  overflow: hidden;
  position: relative;
  gap: 0.5rem;
  padding: 0.25rem 0.5rem;
  border-radius: 6px;
  background-color: #f8f9fa;
  border: 1px dashed #d0d0d0;
  min-height: 40px;
  max-width: 100%;
  white-space: nowrap;
}

.chip {
  display: inline-flex;
  align-items: center;
  background-color: #e0f2ff;
  color: #0366d6;
  border: 1px solid #a4d4f5;
  padding: 0.35rem 0.75rem;
  border-radius: 999px;
  font-size: 0.85rem;
  font-weight: 500;
  white-space: nowrap;
  flex-shrink: 0;
  max-width: 100%;
  overflow: hidden;
  text-overflow: ellipsis;
}

.chip-remove {
  background: none;
  border: none;
  color: #0366d6;
  font-weight: bold;
  margin-left: 0.5rem;
  cursor: pointer;
  font-size: 0.9rem;
  padding: 0;
  display: flex;
  align-items: center;

  &:hover {
    color: #d93025;
  }
}

.more-chip {
  display: inline-flex;
  align-items: center;
  background-color: #e9ecef;
  color: #495057;
  border: 1px solid #ced4da;
  padding: 0.35rem 0.75rem;
  border-radius: 999px;
  font-size: 0.85rem;
  font-weight: 500;
  cursor: pointer;
  flex-shrink: 0;
  white-space: nowrap;

  &:hover {
    background-color: #dee2e6;
  }
}


// Dropdown 
.multi-combo-dropdown {
    position: absolute;
    top: calc(100% + 4px);
    left: 0;
    right: 0;
    background-color: #fff;
    border: 1px solid #ccc;
    border-radius: 6px;
    max-height: 240px;
    overflow-y: auto;
    padding: .25rem 0;
    z-index: 9999;
    box-shadow: 0 8px 30px rgba(0,0,0, .15);
    animation: dropdownFadeIn 0.15s ease-out;
    .multi-combo-option {
        display: flex;
        align-items: center;
        gap: .5rem;
        padding: .5rem .75rem;
        cursor: pointer;
        font-size: .95rem;
        transition: background-color .2s ease, color .2 ease;
        color: #333;

        .option-icon img {
            width: 1.2rem;
            height: 1.2rem;
            object-fit: contain;
        }

        .option-label {
            flex: 1;
        }

        &:hover {
            background-color: #f0f0f0;
        }

        &.selected {
            background-color: #dbeafe;
            font-weight: bold;
        }
    }
}

@keyframes dropdownFadeIn {
  from {
    opacity: 0;
    transform: translateY(-4px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>