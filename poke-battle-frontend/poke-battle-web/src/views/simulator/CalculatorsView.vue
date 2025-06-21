<template>
    <div class="calculators-view">
        <div class="calculator-tabs">
            <button v-for="tab in tabs" :key="tab.key" @click="() => { currentTab = tab.key}" :class="[ 'calculator-tab', { active: tab.key === currentTab }]">{{ tab.label }}</button>
        </div>

        <div class="calculator-content">
            <calculator-experience v-if="currentTab === 'exp'" />
            <calculator-type v-if="currentTab === 'types'" />
            <calculator-damage v-if="currentTab === 'dmg'" />
        </div>
            
    </div>
</template>
<script lang="ts" setup>
import { ref } from 'vue';
import CalculatorExperience from './CalculatorExperience.vue';
import CalculatorType from './CalculatorType.vue';
import CalculatorDamage from './CalculatorDamage.vue';
const tabs = [
    { key: 'exp', label: 'Experiencia' },
    { key: 'dmg', label: 'Calculo de daño' },
    { key: 'types', label: 'Tabla de tipos' },
    
];
const currentTab = ref('exp');

</script>
<style scoped lang="scss">
.calculatosrs-view {
    display: flex;
    flex-direction: column;
    gap: 1rem;
}

.calculatos-tabs {
    display: flex;
    gap: 0.5rem;
    border-bottom: 1px solid #ccc;

    .calculator-tab {
        padding: 0.5rem 1rem;
        background: none;
        border: none;
        cursor: pointer;
        font-weight: bold;
        border-bottom: 3px solid transparent;

        &:hover {
            color: #1e4e89;

        }

        &.active {
            border-bottom-color: #2a6ebc;
            color: #2a6ebc;
        }   
    }
}

.calculator-content {
  background: white;
  padding: 1rem;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0,0,0,.1);
}
</style>
<!-- This file is intentionally left blank as a placeholder for future implementation. -->
