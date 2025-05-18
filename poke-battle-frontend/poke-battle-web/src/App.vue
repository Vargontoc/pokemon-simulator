<template>
  <div :class="{ 'dark-mode' :isDark}">
    <header class="header">
      <div class="left-buttons">
        <button class="icon-button" @click="toggleSidebar" title="Colapsar menú">
          ☰
        </button>
      </div>

      <div class="right-buttons">
        <button class="icon-button"  @click="toggleTheme">{{ isDark ? '🌙' : '☀️' }}</button>
      </div>
    </header>

    <div class="app-layout">
      <l-sidebar :collapsed="!sidebarOpen" />
      <main :class="['main',  { 'with-sidebar': sidebarOpen}]">
        <RouterView />
      </main>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import LSidebar from './components/LSidebar.vue';
import { RouterView } from 'vue-router';
const isDark = ref(false);
const sidebarOpen = ref(false);

const toggleTheme = () => {
  isDark.value = !isDark.value;
  localStorage.setItem('theme', isDark.value ? 'dark': 'light');
}

const toggleSidebar = () => {
  sidebarOpen.value = !sidebarOpen.value;
}
onMounted(() => {
  isDark.value = localStorage.getItem('theme') == 'dark';
})
</script>

<style lang="scss" scoped>
.header {
  padding: 1rem;
  background-color: var(--primary-color);
  color: var(--text-color);
  align-items: center;
  display: flex;
  justify-content: space-between;
} 

.left-buttons, .right-buttons {
  display: flex;
  align-items: center;
}

.icon-button {
  background: none;
  border: none;
  color: var(--text-color);
  font-size: 1.5rem;
  cursor: pointer;
  margin: 0 0.5rem;

  &:hover {
    color: var(--link-hover);
  }
}

.app-layout {
  display: flex;
}
.main {
  flex-grow: 1;
  transition:  margin-left 0.3s ease-in-out;
  padding: 1rem;
  background-color: var(--bg-color);
  color: var(--text-color);
}
</style>







