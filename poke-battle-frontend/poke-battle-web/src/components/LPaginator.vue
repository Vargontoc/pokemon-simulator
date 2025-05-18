<template>
    <div class="paginator">
    
        <div class="paginator__total">Total: {{ total }}</div>
        
        <div class="paginator__controls">
            <button @click="prev" :disabled="page === 1" class="paginator__button"><- Prev</button>
            <div class="paginator__center">
                <span>{{ page }} of {{ totalPages }}</span>
            </div>
            <button @click="next" :disabled="totalPages === page" class="paginator__button">Next -></button>
        
            <select @change="handleChangeSize" v-model.number="pageSize" class="paginator__select">
                <option v-for="size in sizes" :key="size" :value="size">{{  size }}  / Page </option>
            </select>
        </div>

    </div>
</template>
<script setup lang="ts">
import { computed, toRefs } from 'vue';

const props = defineProps<{
    page:number
    pageSize: number
    total: number
}>()

const { page, pageSize, total } = toRefs(props);

const sizes: number[] = [5, 10, 25, 50, 100]
const totalPages = computed(():number => { 
    return Math.max(Math.ceil(props.total / props.pageSize), 1);
})
const emit = defineEmits<{
    (e:'update:page', value: number):void
    (e: 'update:pageSize', value: number): void
}>()

const prev = () => {
    if(props.page > 1) emit('update:page', props.page - 1)
}

const next = () => {
    if(props.page < totalPages.value) emit('update:page', props.page + 1)
}

const handleChangeSize = (event: Event) => {
    const selected = +(event.target as HTMLSelectElement).value;
    emit('update:pageSize', selected);
}

</script>
<style lang="scss">
.paginator {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
    gap: 1rem;
    margin-top: 1rem;

    &__total {
        font-size: 0.875rem;
        color: #4b5563;
        margin-top: 0.650rem;
    }

    &__center 
    {
        font-weight: 500;
        font-size: 0.9rem;
        color: #1f2937;
    }

    &__controls {
        display: flex;
        align-items: center;
        gap: 0.5rem;
    }

    &_button {
        padding: 0.35rem 0.75rem;
        border-radius: 0.375rem;
        background-color: #f3f4f6;
        border: none;
        cursor: pointer;
        transition: background 0.2s ease;

        &:hover {
            background-color: #e5e7eb;
        }

        &:disabled {
            opacity: 0.5;
            cursor: not-allowed;
        }
    }

    &__select {
        padding: 0.25rem 0.5rem;
        border-radius: 0.375rem;
        border: 1px solid #d1d5db;
        font-size: 0.875rem;
    }
}
</style>