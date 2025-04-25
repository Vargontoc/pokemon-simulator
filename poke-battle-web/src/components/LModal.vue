<template>
<div>
    <div v-if="show" class="modal-overlay" @click.self="$emit('close-modal')">
        <div class="modal">
            <header class="modal-header">
                <h3 v-if="title">{{ title }}</h3>
                <button class="btn-close" @click="$emit('close-modal')">x</button>
            </header>

            <div class="modal-body">
                <slot />
            </div>
        </div>
    </div>
</div>
</template>
<script setup lang="ts">
const props = defineProps<{
    show: boolean
    title?: string
}>()


</script>
<style lang="scss" scoped>
.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.6);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal {
  background: white;
  border-radius: 8px;
  width: 500px;
  max-width: 95%;
  padding: 20px;
  box-shadow: 0 0 15px rgba(0,0,0,0.2);
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 15px;
}

.close-btn {
  background: transparent;
  border: none;
  font-size: 1.5rem;
  cursor: pointer;
}

.modal-body {
  max-height: 70vh;
  overflow-y: auto;
}
</style>