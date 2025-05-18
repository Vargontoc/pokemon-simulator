<template>
<div class="form-group">
    <label class="form-label">{{ caption }}</label>
    <div class="numeric-filter">
        <select v-model="selectedOperator" @change="handleChange">
            <option :value="undefined"></option>
            <option v-for="op in filterOperations" :key="op">{{ getCaptionByOperation(op)  }}</option>
        </select>
        <input class="form-input" type="number" v-model.number="value" @input="handleChange">
    </div>
</div>
</template>

<script setup lang="ts">
import { getCaptionByOperation, type OperationFilter } from '@/models/OperationFilter';
import { ref } from 'vue';



const emit = defineEmits(['change-value']);
defineProps<{
    caption: string
}>();

const filterOperations: OperationFilter[] = ['==', '!=',  '<', '<=',  '>', '>=']
const selectedOperator = ref<OperationFilter | undefined>('==')
const value = ref<number | undefined>(undefined)

const handleChange = () => {
    let send = {
        operator: selectedOperator.value,
        value: value.value
    }

    emit('change-value', send);
}


</script>

<style lang="scss" scoped>
.numeric-filter {
    display: flex;
    align-items: center;
    gap: 8px;
}
</style>