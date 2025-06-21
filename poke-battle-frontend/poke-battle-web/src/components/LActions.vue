<template>
    <div class="actions-bar">
        <div class="actions">
            <button @click="$emit('refresh')" title="Refrescar">
                <i class="fas fa-sync"></i>
            </button>
            <button @click="$emit('search')" title="Buscar">
                <i class="fas fa-search"></i>
            </button>
            <button @click="$emit('create')" title="Crear">
                <i class="fas fa-plus"></i>
            </button>
            
            <div v-if="hasMoreActions" class="more-options">
                <button @click="toggleMenu" title="Más opciones">
                    <i class="fas fa-ellipsis-v"></i>
                </button>
                <div v-if="menuOpen" class="dropdown" @click.outside="menuOpen = false">
                    <slot name="more-actions"/>
                </div>
            </div>
        </div>
    </div>
</template>
<script setup lang="ts">
import { computed, ref, useSlots } from 'vue';

const menuOpen = ref(false)
function toggleMenu() {
  menuOpen.value = !menuOpen.value
}

const slots = useSlots()
const hasMoreActions = computed(() => !!slots['more-actions'])
</script>
<style lang="scss" scoped>
.actions-bar {
  display: flex;
  justify-content: flex-end;
  margin-bottom: 1rem;
}

.actions {
  display: flex;
  gap: 10px;
  position: relative;
}

button {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 1.1rem;
  padding: 5px;
  transition: transform 0.1s;
}

button:hover {
  transform: scale(1.15);
}

.more-options {
  position: relative;
}

.dropdown {
  position: absolute;
  top: 120%;
  right: 0;
  background: white;
  border: 1px solid #ddd;
  border-radius: 5px;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.15);
  z-index: 10;
  padding: 0.5rem;
  display: flex;
  flex-direction: column;
  min-width: 150px;
}
</style>