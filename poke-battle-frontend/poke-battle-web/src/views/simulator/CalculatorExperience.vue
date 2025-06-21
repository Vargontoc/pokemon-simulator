<template>
    <div class="calculator-experience">
        <h2 class="section-title">Calculadora de experiencia</h2>
        
        <div class="calculator-section">
            <h3 class="calculator-subtitle">Experiencia por Nivel</h3>
            <div class="form-row">
                <div class="form-group form-col col-1-4">
                    <label class="form-label">Tipo de crecimiento</label>
                    <select class="form-input" v-model="growthRate" @change="calculateExpRequired">
                        <option v-for="growth in growths" :key="growth.key" :value="growth.key">{{ growth.value }}</option>
                    </select>
                </div>

                <div class="form-group form-col col-1-4">
                    <label for="level" class="form-label">Nivel</label>
                    <input id="level" class="form-input" v-model.number="targetLevel" @change="calculateExpRequired" type="number" min="1" max="100" placeholder="1" />
                </div>
                
            </div>
            <div class="exp-bar-wrapper">
                <div class="exp-bar" ></div>
            </div>
            <div class="result">
                <strong>Exp. requerida: </strong> {{  expRequired }}
            </div>
        </div>

        <div class="calculator-section">
            <h3 class="calculator-subtitle">Experiencia ganada en combate</h3>
            <div class="form-row">
                <div class="form-group form-col col-1-4">
                    <label class="form-label">Exp. Base</label>
                    <input @change="calculateExperienceGained" id="level" class="form-input" v-model.number="baseExp" type="number" min="1" max="100" placeholder="1" />
                </div>


                <div class="form-group form-col col-1-4">
                    <label for="level" class="form-label">Nivel rival</label>
                    <input  id="level" @change="calculateExperienceGained" class="form-input" v-model.number="foeLevel" type="number" min="1" max="100" placeholder="1" />
                </div>
                
                <div class="form-group">
                    <label class="form-label">Multiplicadores:</label>
                    <div class="form-checkboxes">
                        <label><input type="checkbox" @change="calculateExperienceGained" v-model="multipliers.trainer" />Entrenador</label>
                        <label><input type="checkbox" @change="calculateExperienceGained" v-model="multipliers.luckyEgg" />Huevo Suerte</label>
                    </div>
                </div>

                <div class="form-group form-col col-1-4">
                    <label class="form-label">Participantes</label>
                    <input  class="form-input" @change="calculateExperienceGained" v-model="participants" type="number" min="1" max="6" placeholder="1" />
                </div>
            </div>

            <div class="result">
                <strong>Exp. requerida: </strong> {{ expGained }}
            </div>
        </div>

    </div>
</template>
<script lang="ts" setup>
import client from '@/services/axios';
import { ComboService } from '@/services/CombosService';
import { SimulatorService } from '@/services/SimulatorService';
import { ref, watch } from 'vue';

const comboService = new ComboService(client);
const simulatorService = new SimulatorService(client);

const targetLevel = ref<number>(1);
const foeLevel = ref<number>(1);
const baseExp = ref<number>(1);
const participants = ref<number>(1);

const multipliers = ref<{
    trainer: false,
    luckyEgg: false
}>({
    trainer: false,
    luckyEgg: false
});

const growthRate = ref<string>(''); 

const growths = ref <{
    key: string;
    value: string;
}[]>([]);
comboService.getGrowth().then((r) => {
    growths.value.push({
        key: '',
        value: ''
    });
    r.items.forEach((item) => {
        growths.value.push({
            key: item.key ?? '',
            value: item.value ?? ''
        });
    });
});

const isAnimating = ref<boolean>(false);
const expRequired = ref<number>(0);
const expGained = ref<number>(0);
watch(expRequired, () => {
    isAnimating.value = true;
    requestAnimationFrame(() => { isAnimating.value = true; })   
});

function calculateExperienceGained() {
    if(foeLevel.value && baseExp.value && participants.value){
        simulatorService.getCalculateGainedExperience(foeLevel.value, baseExp.value, participants.value, multipliers.value.trainer, multipliers.value.luckyEgg).then((r) => {
            expGained.value = r;
        })
    }

}

function  calculateExpRequired() {
    if(growthRate.value && growthRate.value !== '' && targetLevel.value) 
    {
        simulatorService.getCalculateExperience(growthRate.value, targetLevel.value).then((r) => {
            expRequired.value = r;
        }).catch((err) => {
            console.log(err);
            expRequired.value = 0;
        })
    }else {
        expRequired.value = 0;
    }
}

</script> 
<style scoped lang="scss">
.calculator-experience {
    display: flex;
    flex-direction: column;
    gap: 2rem;
    padding: 1rem;

    .section-title {
        font-size: 1.5rem;
        font-weight: bold;
        margin-bottom: 1rem;
    }   

    .calculator-subtitle {
        font-size: 1.2rem;
        font-weight: 600;
        margin-bottom: 0.75rem;
    }

    .calculator-section {
        border: 1px solid #ccc;
        border-radius: 6px;
        padding: 1rem;
        background-color: #fafafa;
    }

    .result {
        margin-top: 1rem;
        font-size: 1.1rem;
        font-weight: bold;
        color: #2a6ebc;
    }   
}

.exp-bar-wrapper {
    height: 12px;
    background-color: #e0e0e0;
    border-radius: 6px;
    overflow: hidden;
    margin: 0.5rem 0;

    .exp-bar {
        height: 100%;
        transition: width 0.5s ease-in-out, background-color 0.3s ease-in-out;
    
        &.animated {
            transition: width 0.6s ease-in-out;
        }
    }
}

.form-checkboxes {
    display: flex;
    flex-wrap: wrap;
    gap: 1rem;
    padding-top: .5rem;

    label {
        display: flex;
        align-items: center;
        gap: 0.5rem;
        font-size: .9rem;
    }
}
</style>
<!-- This file is intentionally left blank as a placeholder for future implementation. -->