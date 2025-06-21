<template>
    <div class="calculator-section">
        <div class="form-row">
            <div class="form-group form-col col-1-4">
                <combo-multi-select :model="types" :max-selected="isAttack ? 1: 2" :type="'only-text'" :keys-selected="selectedTypes" @update-selected="handleSelected" />
            </div>
        </div>

        <div class="effectiveness-display" v-if="effectivenessGroups">
            <div v-for="(types, multiplier) in effectivenessGroups" :key="multiplier" class="effectiveness-group">
                <div class="effectiveness-title"><strong>x {{ multiplier }} </strong>  {{ getLabel(Number(multiplier)) }} </div>
                <div class="type-icons">
                    <span v-for="type in types" :key="type.key" class="type-icon">
                        <img v-if="type.icon"  :src="type.icon"  :alt="type.value" />
                        <span v-else>{{ type.value }}</span>
                    </span>
                </div>
            </div>
        </div>
    </div>
</template>
<script setup lang="ts">
import { ComboService } from '@/services/CombosService';
import ComboMultiSelect from '../ComboMultiSelect.vue';
import client from '@/services/axios';
import { ref } from 'vue';
import { LCombo } from '@/models/LCombo';
import { SimulatorService } from '@/services/SimulatorService';
const props = defineProps<{
    isAttack?: boolean
}>();
const service = new ComboService(client);
const simulator = new SimulatorService(client);

const types = ref<LCombo>(new LCombo());
const selectedTypes = ref<string[]>([]);

const effectivenessGroups = ref<Record<number, { key: string, value: string,  icon?: string }[]>>({});
function getLabel(multiplier: number) {
  if (multiplier === 0) return 'No afecta';
  if (multiplier < 1) return 'No es muy eficaz';
  if (multiplier === 1) return 'Eficaz';
  if (multiplier > 1) return '¡Muy eficaz!';
  return '';
}


service.getComboTypes().then((r) => {
    types.value = r;
})

async function handleSelected(selected: string[]) {
    selectedTypes.value = selected;
    if(selected.length === 0)
    {
        effectivenessGroups.value = {};
        return;
    }

    if(props.isAttack) {
        await simulator.getMultipliersOnAttack(selected[0]).then((r) => {
            effectivenessGroups.value = r;
        })
    }else {
        
        await simulator.getMultipliersOnDefense(selected).then((r) => {
            effectivenessGroups.value = r;
        })
    }

}
</script>
<style lang="scss" scoped>
.effectiveness-display {
    display: flex;
    flex-direction: column;
    gap: 1rem;
    margin-top: 1rem;

    .effectiveness-group {
        background: #f8f8f8;
        border-radius: 8px;
        padding: .75rem 1rem;
        box-shadow: 0 2px 4px rgba(0,0,0,.1);

        .effectiveness-title {
            font-weight: bold;
            margin-bottom: .5rem;
        }

        .type-icons {
            display: flex;
            flex-wrap: wrap;
            gap: .5rem;

            .type-icon {
                display: flex;
                align-items: center;
                gap: .25rem;

                img {
                    width: 130px;
                    height: 30px;
                }
            }
        }
    }
}
</style>