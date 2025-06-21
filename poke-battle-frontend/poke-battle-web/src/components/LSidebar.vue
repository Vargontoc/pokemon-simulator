<template>
<aside :class="['sidebar', { collapsed }]">
    <nav class="nav" >
        <RouterLink to="/abilities" class="nav-link">Habilidades</RouterLink>
        <RouterLink to="/items" class="nav-link">Objetos</RouterLink>
        <RouterLink to="/types" class="nav-link">Tipos</RouterLink>
        <RouterLink to="/moves" class="nav-link">Movimientos</RouterLink>
        <RouterLink to="/species" class="nav-link">Pokedex</RouterLink>

        <l-dropdown title="Overworld">
            <RouterLink to="/trainers" class="dropdown-link">Entrenadores</RouterLink>
            <RouterLink to="/encounters" class="dropdown-link">Encuentros</RouterLink>
        </l-dropdown>

        <l-dropdown title="Simuladores">
            <RouterLink to="/calculators" class="dropdown-link">Calculadoras</RouterLink>
            <RouterLink to="/simulator-battle" class="dropdown-link">Batalla IA</RouterLink>
        </l-dropdown>
    </nav>
</aside>
</template>

<script setup lang="ts">
import { RouterLink } from 'vue-router';
import LDropdown from './LDropdown.vue';

defineProps({ collapsed: Boolean })

</script>