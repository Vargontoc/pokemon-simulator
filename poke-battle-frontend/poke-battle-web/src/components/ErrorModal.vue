<template>
    <div v-if="state.show" class="error-modal-overlay" @click.self="close">
        <div class="error-modal">
            <header class="modal-header">
                <h2>Error</h2>
                <button class="btn btn-close" @click="close">✖️</button>
            </header>

            <div class="modal-body">
                <p> {{ state.message }}</p>
                <details v-if="state.details" class="modal-details">
                    <summary>Detalles del error</summary>
                    <pre>{{ state.details }}</pre>
                </details>
            </div>

            <footer class="modal-footer">
                <button class="btn btn-primary" @click="close">Cerrar</button>
            </footer>
        </div>

    </div>
</template>
<script setup lang="ts">
import { useErrorModal } from '@/composables/UseErrorModal';
const { errorState: state, close: close } = useErrorModal();
</script>

<style scoped lang="scss">
.error-modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.6);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
}

.error-modal {
  background: white;
  border-radius: 8px;
  padding: 1.5rem;
  width: 500px;
  max-width: 90%;
  box-shadow: 0 0 20px rgba(0,0,0,0.2);
  animation: fadeIn .2s ease-out;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;

  h2 {
    margin: 0;
    font-size: 1.25rem;
  }
}


.modal-body {
  margin-top: 1rem;
}

.modal-details {
  margin-top: 1rem;
  font-size: 0.85rem;

  pre {
    background: #f5f5f5;
    padding: 0.75rem;
    border-radius: 4px;
    white-space: pre-wrap;
    max-height: 200px;
    overflow-y: auto;
  }
}

.modal-footer {
  margin-top: 1.5rem;
  text-align: right;
}
</style>