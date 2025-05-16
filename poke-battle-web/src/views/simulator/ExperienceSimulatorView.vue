<template>
    <div class="experience-simulator">
        <h1>Experience Simulator</h1>
        <!-- Input for experience calculation -->
        <div class="row">
            <div class="form-group">
                <label class="form-label" for="experience-input">Tipo crecimiento:</label>
                <l-combo-component :nullable="true"  :model="comboGrowth" :multi="false" :keys-selected="[]" @update-selected="handleGrowthSelected"></l-combo-component>
            </div>
            <div class="form-group">
                <label class="form-label" for="experience-input">Nivel:</label>
                <input class="form-input" id="experience-input" min="1" max="100"  type="number" v-model="levelInput" />
            </div>
            <div v-if="visibleResult" class="form-group">
                <label class="form-label" for="result-experience">Resultado:</label>   
                <input class="form-input " id="result-experience" type="number" v-model="experienceResult" disabled />
            </div>
        </div>

        <!-- Chart for experience data -->
        <div class="chart">
            <h3>Experience Chart</h3>
            <ul>
            <li v-for="(data, index) in experienceChartData" :key="index">
                {{ data }}
            </li>
            </ul>
        </div>

        <!-- Party members and combat experience -->
        <div class="party">
            <h3>Party Members</h3>
            <div v-for="(member, index) in party" :key="index" class="party-member">
            <span>{{ member.name }}</span>
            <span>Experience: {{ member.experience }}</span>
            </div>
        </div>
        <div class="row">
            <label>Combat Experience:</label>
            <span>{{ combatExperience }}</span>
            <button>Simulate Combat</button>
        </div>
    </div>
</template>

<script setup lang="ts">
import type { LCombo } from '@/models/LCombo';
import {  ref } from 'vue';
import LComboComponent from '@/components/LComboComponent.vue';
import { ComboService } from '@/services/CombosService';
import client from '@/services/axios';
import { SimulatorService } from '@/services/SimulatorService';

const comboService = new ComboService(client);  
const expService = new SimulatorService(client);

// Reactive data for experience calculation
const levelInput = ref(1);
const experienceResult = ref(0);
const visibleResult = ref(false);
const selectedGrowth = ref<string | undefined>(undefined);
const experienceChartData = ref([]);

// Reactive data for combat experience simulation
const combatExperience = ref(0);

// Hardcoded party data
const party = ref([
    { name: 'Pikachu', experience: 1200 },
    { name: 'Charizard', experience: 3400 },
    { name: 'Bulbasaur', experience: 800 },
]);

const comboGrowth = ref<LCombo>({});

// Method to handle growth selection
const handleGrowthSelected = async (keys: string[]) => {
    if(keys.length != 0) {
        selectedGrowth.value = keys[0] ?? undefined;       
        if(selectedGrowth.value !== undefined && levelInput.value  > 0 && levelInput.value <= 100) {
            await expService.getCalculateExperience(selectedGrowth.value, levelInput.value).then((response: number) => {
                experienceResult.value = response;
                visibleResult.value = true;
            }).catch((error) => {
                console.error('Error calculating experience:', error);
            });
        }else {
            visibleResult.value = false;
        }
    }  
};

const loadView = async () => {
    const response = await comboService.getGrowth() as LCombo;
    comboGrowth.value = response;
};

loadView();

</script>

<style scoped>
.experience-simulator {
    display: flex;
    flex-direction: column;
    gap: 20px;
}

.row {
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.chart {
    width: 100%;
    height: 300px;
    background-color: #f5f5f5;
    border: 1px solid #ccc;
}

.party {
    display: flex;
    flex-direction: column;
    gap: 10px;
}

.party-member {
    display: flex;
    justify-content: space-between;
    padding: 10px;
    background-color: #e0e0e0;
    border-radius: 5px;
}
</style>
