<template>
<tr v-for="r in rows" v-bind:key="r.key" @click="handleSelected(r.key)" :class="{ 'active-row': r.key === selectedRow }">
    <td v-for="c in r.cells">
        <!-- Texto o numero-->
        <span v-if="c.type ==='text'  || c.type === 'number'">{{ c.value }}</span>
        
        <!-- Booleano -->
        <input v-else-if="c.type == 'boolean'" type="checkbox" v-model="(c.value as boolean)" disabled/>

        <!-- Combo -->
        <l-dropdown-cell v-else-if="c.type === 'combo'" :items="(c.value as LComboItem[])" :selected="(c.value as LComboItem[]).length != 0 ? (c.value as LComboItem[])[0].key : '-'" />
     
        <!-- Icon -->
        <l-icon-cell v-else-if="c.type == 'icon'" :icon="(c.value as string)"></l-icon-cell>
        <span v-else> (Unkwon value) </span>
    </td>
</tr>
</template>

<script setup lang="ts">
import { ref, toRefs } from 'vue';
import type { LRow } from '../models/LRow';
import LDropdownCell from './LDropdownCell.vue';
import LIconCell from './LIconCell.vue';
import type { LComboItem } from '@/models/LComboItem';
interface Props 
{
    rows: LRow[],
}

const props = defineProps<Props>();
const { rows } = toRefs(props);
const selectedRow = ref<number | undefined>(undefined)

const emit = defineEmits(['selected-item'])
function handleSelected(id: number | undefined)
{
    if(id === selectedRow.value) {
        selectedRow.value = undefined
    }else {
        selectedRow.value = id;
        emit('selected-item', selectedRow.value);
    }
}


</script>
<style lang="scss" scoped>
td {
    text-align: left;
    padding: 0.5rem 1rem;
}

tr:hover, tr.active-row {
        background-color: #f9fafb;
        cursor: pointer;

        td {
            color: black
        }
    }
</style>