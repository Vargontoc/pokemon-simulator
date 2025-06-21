<template>
<div>
    <div v-if="show" class="modal-overlay" @click.self="$emit('close-modal')" tabindex="-1">
        <div class="modal" :style="{width: width}" role="dialog" aria-modal="true">
            <header class="modal-header">
                <h3 v-if="title" class="modal-title">{{ title }}</h3>
                <button class="btn btn-close-modal" aria-label="Cerrar" @click="$emit('close-modal')">x</button>
            </header>

            <div class="modal-body">
                <slot />
            </div>

            <footer class="modal-footer" v-if="$slots.footer">
              <slot name="footer" />
            </footer>
        </div>
    </div>
</div>
</template>
<script setup lang="ts">
defineProps<{
    show: boolean
    title?: string
    width?: string
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
  animation: fadeIn .2 ease-out;

  .modal {
    background: white;
    border-radius: 10px;
    padding: 20px;
    box-shadow: 0 8px 30px rgba(0, 0, 0, .25);
    max-height: 90vh;
    overflow: hidden;
    display: flex;
    flex-direction: column;
    animation: slideIn .2s ease-out;

    .modal-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 1rem;

      .modal-title {
        font-size: 1.25rem;
        font-weight: bold;
        margin: 0;
      }
    }

    .modal-body {
      overflow-y: auto;
      flex: 1;
      font-size: .95rem;
    }

    .modal-footer {
      margin-top: 1rem;
      display: flex;
      justify-content: flex-end;
      gap: .5rem;
    }
  }
}

@keyframes fadeIn {
  from { opacity: 0;}
  to { opacity: 1;}
}

@keyframes slideIn {
  from { transform: translateY(20px); opacity: 0; }
  to { transform: translateY(0); opacity: 1; }
}
</style>