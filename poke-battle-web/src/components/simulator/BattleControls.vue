<template>
    <div class="battle-controls">
        <button @click="$emit('executeTurn')">Ejecutar turno</button>
    </div>
</template>

<script lang="ts" setup>
defineEmits(['executeTurn']);
</script>