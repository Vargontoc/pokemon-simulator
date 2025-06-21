<template>
    <div class="specie-view">
        <div class="tabs-header">
            <button v-for="(tab, index) in tabs" :key="tab" :class="{ active: activeTab === index }" @click="activeTab = index">{{ tab }}</button>
        </div>

        <div class="tabs-content">
            <div v-show="activeTab === 0">
                <specie-general-form />
            </div> 

            <div v-show="activeTab === 1">

            </div>

            <div v-show="activeTab === 2">
                <specie-move-form />
            </div>

            <div v-show="activeTab === 3">
                <specie-evolution-form :model="{
                    evolutions: []
                }" />
            </div>

            <div v-show="activeTab === 4">

            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import SpecieGeneralForm from '@/views/crud/species/SpecieGeneralForm.vue';
import SpecieMoveForm from '@/views/crud/species/SpecieMoveForm.vue';
import SpecieEvolutionForm from '@/views/crud/species/SpecieEvolutionForm.vue';
import { ref } from 'vue';
import type Specie from '@/models/Specie';

defineProps<{
    model: Specie
}>();
const tabs = ['Pokedex', 'Batalla', 'Movimientos', 'Lineas evolutivas', 'Gráficos' ]
const activeTab = ref(0);
</script>

<style lang="scss" scoped>

</style>