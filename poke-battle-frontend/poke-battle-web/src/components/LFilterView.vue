<template>
    <div v-if="show" class="filter-view">
        <div class="filter-content">
            <slot />
        </div>
        <div class="filter-actions">
            <button @click="$emit('search-filter')">Buscar</button>
            <button @click="$emit('clean-filter')">Limpiar</button>
        </div>
    </div>
</template>
<script lang="ts" setup>
defineProps<{
    show: boolean
}>()
</script>

<style lang="scss" scoped>
.filter-view {
  border: 1px solid #ccc;
  border-radius: 6px;
  padding: 1rem;
  margin-bottom: 1rem;
  background-color: #f9f9f9;
}

.filter-content {
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
}

.filter-actions {
  margin-top: 1rem;
  display: flex;
  gap: 10px;
  justify-content: flex-end;
}

button {
  background-color: #3498db;
  color: white;
  padding: 6px 12px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

button:hover {
  background-color: #2980b9;
}
</style>