<template>
    <div class="calculator-damage">
        <h2 class="section-title">Calculadora de daño</h2>
        <div class="calculator-section">
            <h3 class="calculator-subtitle">Pokemons</h3>
            <div class="calculator-section-pokemon">
                <div class="calculator-section">
                    <battler-form :hide-move-section="true" ref="attacker" />
                </div>
                <div class="calculator-section">
                    <battler-form :hide-move-section="true" ref="defender" />
                </div>
            </div>


        </div>

        <div class="calculator-section">
            <h3 class="calculator-subtitle">Movimiento</h3>
            <div class="form-row">
                <div class="form-group form-col col-1-4">
                    <combo-multi-select :model="moves" :keys-selected="[]" :type="'only-text'" :max-selected="1"  />
                </div>
                <button class="btn btn-add">Calcular</button>
            </div>
        </div>

        <div class="calculator-section">
            <div class="result">
                <strong>Resultado: </strong> {{  }}
            </div>
        </div>


    </div>
</template>
<script setup lang="ts">
import BattlerForm from '@/components/simulator/BattlerForm.vue';
import { LCombo } from '@/models/LCombo';
import client from '@/services/axios';
import { ComboService } from '@/services/CombosService';
import ComboMultiSelect from '@/components/ComboMultiSelect.vue';
import { ref } from 'vue';
const service = new ComboService(client);
const moves = ref<LCombo>(new LCombo());

const attacker = ref<InstanceType<typeof BattlerForm>>();
const defender = ref<InstanceType<typeof BattlerForm>>();

service.getMoves().then((r) => { moves.value = r; })
</script>
<style lang="scss" scoped>
.calculator-damage {
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

        .calculator-section-pokemon {
            display: flex;
            flex-direction: row;
            gap: 2rem;
            padding: 1rem;
        }
    }

    .result {
        margin-top: 1rem;
        font-size: 1.1rem;
        font-weight: bold;
        color: #2a6ebc;
    }   
}
</style>