<template>
    <div class="paginator">
    <div class="paginator__total">Total: {{ total }}</div>
    
    <div class="paginator__controls">
        <div class="paginator__left">
        <button @click="prev" :disabled="page === 1" class="paginator__button">← Prev</button>
        <div class="paginator__center">
            <span>{{ page }} of {{ totalPages }}</span>
        </div>
        <button @click="next" :disabled="totalPages === page" class="paginator__button">Next →</button>
        </div>

        <select @change="handleChangeSize" v-model.number="pageSize" class="paginator__select">
        <option v-for="size in sizes" :key="size" :value="size">{{ size }} / Page</option>
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
  flex-direction: column;
  gap: 0.75rem;
  font-size: 0.9rem;
  margin-top: 1rem;

  @media (min-width: 480px) {
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
  }

  &__total {
    font-weight: 500;
    color: #333;
  }

  &__controls {
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 100%;
    flex-wrap: wrap;
    gap: 0.5rem;
  }

  &__left {
    display: flex;
    align-items: center;
    gap: 0.5rem;
  }

  &__center {
    font-weight: 500;
    color: #444;
    min-width: 80px;
    text-align: center;
  }

  &__button {
    padding: 0.4rem 0.8rem;
    background-color: #f5f5f5;
    border: 1px solid #ccc;
    border-radius: 5px;
    font-weight: 500;
    cursor: pointer;
    transition: all 0.2s ease;

    &:hover:not(:disabled) {
      background-color: #e0e0e0;
    }

    &:disabled {
      opacity: 0.5;
      cursor: not-allowed;
    }
  }

  &__select {
    margin-left: auto;
    padding: 0.4rem;
    font-size: 0.9rem;
    border-radius: 5px;
    border: 1px solid #ccc;
    background-color: white;
    cursor: pointer;

    &:focus {
      border-color: #2a6ebc;
      outline: none;
    }
  }
}
</style>