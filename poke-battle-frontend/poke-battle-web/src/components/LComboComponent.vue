<template>
    <div class="form-group">
        <div v-if="label" class="form-label">{{ label }}</div>
        <select class="form-select" :multiple="multi" @change="handleChange">
            <option :value="undefined" v-if="nullable == true"></option>
            <option v-for="o in model.items" :key="o.key" :selected="keysSelected.includes(o.key!)" :value="o.key">
                {{ o.value }}
            </option>
        </select>
    </div>
</template>
<script lang="ts" setup>
import type { LCombo } from '@/models/LCombo';

const props = defineProps<{
    model: LCombo
    keysSelected: string[]
    multi?: boolean
    nullable?: boolean
    label?: string
}>()


const emit = defineEmits<{
    (e: 'update-selected', keys: string[]): void
}>()
const handleChange = (e: Event) => {
    const target = e.target as HTMLSelectElement;
    const selected = Array.from(target.selectedOptions).map(o => o.value);
    props.model.items?.forEach(item => { item.checked = selected.includes(item.key!)})

    emit('update-selected', selected)
}
</script>
<style lang="scss" scoped>
.form-control {
    padding: 0.5rem;
    border: 1px solid #ccc;
    width: 100%;
}
</style>