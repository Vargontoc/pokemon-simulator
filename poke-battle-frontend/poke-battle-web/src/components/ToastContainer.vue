<template>
    <div class="toast-container" >
       <div v-for="toast in toasts" :key="toast.id" class="toast" :class="toast.type">{{ toast.message }}</div>
    </div>
</template>

<script lang="ts" setup>
import { useToasts } from '@/composables/UseToats';
useToasts
const { toasts } = useToasts();
</script>

<style lang="scss" scoped>  
.toast-container {
    position: fixed;
    top: 1rem;
    right: 1rem;
    display: flex;
    flex-direction: column;
    gap: .75rem;
    z-index: 9999;

    .toast {
        padding: .75rem 1rem;
        border-radius: 6px;
        color: #fff;
        box-shadow: 0 4px 12px rgba(0,0,0,.1);
        animation:  fadeIn 0.3s ease-in-out;

        &.success { background-color: #4caf50; }
        &.error   { background-color: #f44336; }
        &.info    { background-color: #2196f3; }
        &.warning { background-color: #ff9800; }
    }
}

@keyframes fadeIn {
    from { opacity: 0; transform: translateY(-10px); }
    to { opacity: 1; transform: translateY(0); }
    
}
</style>