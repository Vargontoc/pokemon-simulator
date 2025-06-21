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
        <div class="chart" v-if="experienceChartData.length > 0">
                <label class="form-label" for="chart-experience">Experiencia:</label>
                <Line :data="chartData" :options="chartOptions" />
        </div>

        <!-- Party members and combat experience -->
        <div class="row">
            <label>Combat Experience:</label>
            <span>{{ combatExperience }}</span>
            <button>Simulate Combat</button>
        </div>
    </div>
</template>

<script setup lang="ts">
import { LCombo } from '@/models/LCombo';
import {  ref } from 'vue';
import LComboComponent from '@/components/LComboComponent.vue';
import { ComboService } from '@/services/CombosService';
import client from '@/services/axios';
import { SimulatorService } from '@/services/SimulatorService';
import { Chart as ChartJS, LinearScale, PointElement, LineElement, Title, Tooltip, Legend, CategoryScale } from 'chart.js';
import { Line } from 'vue-chartjs';

ChartJS.register(Title, Tooltip, Legend, LineElement, LinearScale, PointElement, CategoryScale);
const comboService = new ComboService(client);  
const expService = new SimulatorService(client);

// Reactive data for experience calculation
const levelInput = ref(1);
const experienceResult = ref(0);
const visibleResult = ref(false);
const selectedGrowth = ref<string | undefined>(undefined);
const experienceChartData = ref<number[]>([]);

// Reactive data for combat experience simulation
const combatExperience = ref(0);
const chartData = {
    labels: Array.from({ length: experienceChartData.value.length}, (_, i) => i+ 1), 
    datasets: [
        {
            label: 'Ratio Experiencia',
            data: [0],
            borderWidth: 2,
            fill: true
        }
    ]
}
const chartOptions = {
    responsive: false,
    maintainAspectRatio: false,
    scales: {
        x: {
            title: {
                display: true,
                text: 'Nivel'
            }
        },
        y: {
            title: {
                display: true,
                text: 'Experiencia'
            }
        }
    },
}
// Hardcoded party data

const comboGrowth = ref<LCombo>(new LCombo());

// Method to handle growth selection
const handleGrowthSelected = async (keys: string[]) => {
    if(keys.length != 0) {
        selectedGrowth.value = keys[0] ?? undefined;       
        if(selectedGrowth.value !== undefined && selectedGrowth.value !== '' && levelInput.value  > 0 && levelInput.value <= 100) {
            await expService.getCalculateExperience(selectedGrowth.value, levelInput.value).then((response: number) => {
                experienceResult.value = response;
                visibleResult.value = true;
            }).catch((error) => {
                console.error('Error calculating experience:', error);
            });

            await expService.getGraphGrowth(selectedGrowth.value).then((response: number[]) => {
                experienceChartData.value = response;
                chartData.labels = Array.from({ length: experienceChartData.value.length}, (_, i) => i+ 1); 
                chartData.datasets[0].data = experienceChartData.value as number[];
            }).catch((error) => {
                experienceChartData.value = [];
                console.error('Error fetching experience chart:', error);
            }); 
        }else {
            visibleResult.value = false;
            experienceChartData.value = [];
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
    width: 420px;
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
