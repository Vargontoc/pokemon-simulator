<template>
    <tr>
        <th v-for="c in columns" v-bind:key="c.key" @click="handleSort(c.key)">
            <span>{{ c.display }}
                <i v-if="c.key == propertySorted" class="text-xs">{{ direction === OrderDirection.asc ? '▲' : '▼' }}</i>
            </span>
        </th>
        <th></th>
    </tr>
</template>

<script setup lang="ts">
import { ref, toRefs } from 'vue';
import LColumn from '../models/LColumn';
import { OrderDirection } from '../models/Direction';
import { OrderBy } from '../models/OrderBy';

interface Props 
{
    columns: LColumn[],
}



const props = defineProps<Props>();
const { columns } = toRefs(props);
const emit = defineEmits(['sort'])
const propertySorted = ref<string>("");
const direction = ref<OrderDirection>(OrderDirection.asc);

function handleSort(key: string) : void
{   
 
    propertySorted.value = key;
    direction.value = direction.value === OrderDirection.asc ? OrderDirection.desc : OrderDirection.asc;

    const sendorder = new OrderBy();
    sendorder.Property = propertySorted.value;
    sendorder.Direction = direction.value

    emit('sort', sendorder);
        
    
}
</script>

<style scoped>
th {
    cursor: pointer;
    text-align: left;
    padding: 0.5rem 1rem;
    color: black;
}



</style>