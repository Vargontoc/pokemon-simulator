<template>
    <div class="error-view">
        <h1>Algo fue mal</h1>
        <p>{{ message }}</p>
        <router-link to="/" class="btn btn-back">Volver al inicio</router-link>
    </div>
</template>

<script lang="ts" setup>
import { useRoute } from 'vue-router';

const route = useRoute();
const message = route.query.message || 'Ha ocurrido un error inesperado'
</script>

<style lang="scss" scoped>
.error-view {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    height: 100vh;
    text-align: center;
    padding: 2rem;

    h1 {
        font-size: 2rem;
        margin-bottom: 1rem;
    }
}
</style>