<template>
  <div v-if="confirmDialogState.show" class="modal-overlay" @click.self="cancel">
    <div class="modal">
      <header class="modal-header">
        <h3>{{ confirmDialogState.title }}</h3>
      </header>

      <div class="modal-body">
        <p>{{ confirmDialogState.message }}</p>
      </div>

      <div class="modal-footer">
        <button class="btn btn-cancel" @click="cancel">Cancelar</button>
        <button class="btn btn-confirm" @click="confirm">Confirmar</button>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { useConfirmDialog } from '@/composables/UseConfirmDialog';
const { confirmDialogState, confirm, cancel } = useConfirmDialog();
</script>
<style lang="scss" scoped>
.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.6);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9998;
}

.modal {
  background: white;
  border-radius: 10px;
  padding: 1.5rem;
  width: 400px;
  max-width: 90%;
  box-shadow: 0 8px 30px rgba(0,0,0,0.2);
}

.modal-header h3 {
  margin: 0;
  font-size: 1.2rem;
}

.modal-body {
  margin-top: 1rem;
}

.modal-footer {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  margin-top: 1.5rem;


}
</style>