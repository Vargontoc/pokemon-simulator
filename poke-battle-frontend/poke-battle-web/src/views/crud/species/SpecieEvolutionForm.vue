<template>
    <div class="form-view">
        <div class="form-row">
            <div class="form-col col-1-2">
                <label class="form-label">Evoluciona de: </label>
                <combo-multi-select :type="'only-text'"  :model="species" :max-selected="1" :keys-selected="[]" />
            </div>
        </div>

        <div class="form-row">
            <h4 class="form-label">Evoluciones</h4>
        </div>

        <div class="form-row form-evolution-row" v-for="(evo, i) in model.evolutions" :key="i">
            <div class="form-col col-1-3">
                <combo-multi-select :type="'only-text'"  :model="species" :max-selected="1" :keys-selected="[evo.id]" />
            </div>

            <div class="form-col col-1-3">
                <select class="form-input" :class="{ isInvalid: evo.method === '' }">
                    <option value=""></option>
                    <option value="level">Por nivel</option>
                    <option value="item">Por objeto</option>
                    <option value="trade">Por intercambio</option>
                    <option value="happiness">Por amistad</option>
                    <option value="other">Otro</option>
                </select>
            </div>

            <div class="form-col col-1-4">
                <input class="form-input" :type=" evo.method === 'level' ? 'number'  : 'text'" v-model="evo.condition" :class="{ isInvalid: !isValidEvolution }" />
            </div> 

            <div class="form-col col-1-12">
                <button class="btn btn-remove" @click="model.evolutions.splice(i, 1)">X</button>
            </div>
        </div>

        <div class="form-row" style="justify-content: flex-end;">
            <button class="btn btn-add" @click="onAddEvolution">+ Agregar evolución</button>
        </div>

    </div>
</template>

<script setup lang="ts">
import ComboMultiSelect from '@/components/ComboMultiSelect.vue';
import { LCombo } from '@/models/LCombo';
import client from '@/services/axios';
import { ComboService } from '@/services/CombosService';
import {  ref } from 'vue';

interface Evolution {
    id: string,
    name?: string,
    method: string,
    condition: string | number
}


const props = defineProps<{
    model: {
        evolvesFromId?: string,
        evolvesFromName?: string,
        evolutions: Evolution[]
    }
}>()

const isValidEvolution = (evo: Evolution): boolean => {
    if(!evo.id) return false;
    switch(evo.method) {
        case 'level': return !!evo.condition && Number(evo.condition) > 0 && Number(evo.condition) < 100;
        case 'item': return evo.condition === 'string' && evo.condition.trim().length > 0;
        default: return false;
    }

}

const service = new ComboService(client);
const species = ref<LCombo>(new LCombo());

service.getSpecies().then((r) => {
    species.value = r;
})

function onAddEvolution() {
    props.model.evolutions.push({
        id: '',
        method: 'level',
        condition: ''
    });
}
</script>