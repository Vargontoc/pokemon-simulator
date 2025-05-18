<template>
    <div v-if="items.length != 0" class="dropdown-cell" @click="toggle">
        <div class="selected" >
            <img class="icon" v-if="selectedOption?.icon" :src="selectedOption.icon"/>
            <span v-else>{{ selectedOption?.value }}</span>
            <span class="arrow">▼</span>
        </div>

        <ul v-if="isOpen" class="dropdown"> 
            <li v-for="item in itemsList" :key="item.key" class="dropdown-item" :title="item.tooltip">
                <img v-if="item.icon" :src="item.icon" class="icon" style="width: 18px; height: 18px;"/>
                <span v-else>{{ item.value }}</span>
            </li>
        </ul>

    </div>
</template>

<script setup lang="ts">
import { LComboItem } from '@/models/LComboItem';
import { computed, ref, type PropType } from 'vue';
const props = defineProps({
    items: {
        type: Array as PropType<LComboItem[]>,
        required: true
    },
    selected: String
});

const isOpen = ref(false);
const itemsList = computed(() => props.items || [])
const toggle = () => {
    isOpen.value = !isOpen.value;
}

const selectedOption = computed(() => {
    return itemsList.value.find(i => i.key === props.selected)
})
</script>