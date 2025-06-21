<template>
    <form class="form-view" @click.prevent>
        <div class="form-row">
            <div class="form-group form-col col-1-5">
                <label for="name" class="form-label">Nombre Interno</label>
                <input id="name" class="form-input" v-model="model.id"  placeholder="bulbasaur" />
            </div>

            <div class="form-group form-col col-1-5">
                <label for="displayName" class="form-label">Nombre</label>
                <input id="displayName" class="form-input" v-model="model.name"  placeholder="Bulbasaur" />
            </div>
            
            <div class="form-group form-col col-1-5">
                <label for="category" class="form-label">Categoria</label>
                <input id="category" class="form-input" v-model="model.category" placeholder="Semilla" />
            </div>

            <div class="form-group form-col col-1-5">
                <combo-multi-select :type="'only-text'" caption="Tipo" :model="comboTypes" :max-selected="2" :keys-selected="[]" />
            </div>
        </div>

        <div class="form-row">
            <div class="form-group form-col col-1-1">
                <label for="description" class="form-label">Descripcion</label>
                <textarea id="description" class="form-input" v-model="model.despcription" placeholder="Descripción del pokemon..." rows="4" ></textarea>
            </div>
        </div>

        <div class="form-row"> 
            <div class="form-group form-col col-1-2">
                <stats-slider-form v-model="baseStats" />
            </div>
        </div>

        <div class="form-row">
            <div class="form-group form-col col-1-6">
                <combo-multi-select :type="'only-text'" caption="Habilidades" :model="comboAbilities" :max-selected="2" :keys-selected="[]" />
            </div>
            
            <div class="form-group form-col col-1-6">
                <combo-multi-select :type="'only-text'" caption="Habilidad oculta" :model="comboAbilities" :max-selected="1" :keys-selected="[]" />
            </div>

            <div class="form-group form-col col-1-6">
                <combo-multi-select :type="'only-text'" caption="Grupo huevo" :model="comboEggGroup" :max-selected="comboEggGroup.items.length" :keys-selected="[]" />
            </div>
            
            <div class="form-group form-col col-1-6">
                <combo-multi-select :type="'only-text'" caption="Color" :model="comboColor" :max-selected="1" :keys-selected="[]" />
            </div>

            <div class="form-group form-col col-1-6">
                <combo-multi-select :type="'only-text'" caption="Habitat" :model="comboHabitat" :max-selected="comboHabitat.items.length" :keys-selected="[]" />
            </div>
        </div>

        <div class="form-row">
            <div class="form-group form-col col-1-5">
                <label for="height" class="form-label">Altura</label>
                <input id="height" class="form-input" type="number" min="0"  placeholder="0.7" />
            </div>

            <div class="form-group form-col col-1-5">
                <label for="weight" class="form-label">Peso</label>
                <input id="weigth" class="form-input" type="number" min="0"  placeholder="6.9" />
            </div>

            
            <div class="form-group form-col col-1-5">
                <label for="gender" class="form-label">Ratio hembra</label>
                <input id="gender" class="form-input" type="number" min="0" max="100"  placeholder="6.9" />
            </div>
            
            <div class="form-group form-col col-1-5">
                <label for="happiness" class="form-label">Felicidad base</label>
                <input id="happiness" class="form-input" type="number" min="0" max="255"  placeholder="25" />
            </div>

            <div class="form-group form-col col-1-5">
                <label for="catch" class="form-label">Ratio captura</label>
                <input id="catch" class="form-input" type="number" min="1" max="255"  placeholder="45" />
            </div>

            
            <div class="form-group form-col col-1-5">
                <label for="exp" class="form-label">Experiencia base</label>
                <input id="exp" class="form-input" type="number" min="1" max="255"  placeholder="45" />
            </div>

            <div class="form-group form-col col-1-5">
                <label for="hatch" class="form-label">Pasos eclosion</label>
                <input id="hatch" class="form-input" type="number" min="0"    />
            </div>
        </div>


    </form>
</template>

<script setup lang="ts">
import { LCombo } from '@/models/LCombo';
import client from '@/services/axios';
import { ComboService } from '@/services/CombosService';
import { ref } from 'vue';
import ComboMultiSelect from '@/components/ComboMultiSelect.vue';
import StatsSliderForm from '@/components/StatsSliderForm.vue';
import type Specie from '@/models/Specie';
defineProps<{
    model:  Specie
}>()


const comboService = new ComboService(client);
const comboTypes = ref<LCombo>(new LCombo());
const comboAbilities = ref<LCombo>(new LCombo());
const comboHiddenAbility = ref<LCombo>(new LCombo());
const comboEggGroup = ref<LCombo>(new LCombo());
const comboColor = ref<LCombo>(new LCombo());
const comboHabitat = ref<LCombo>(new LCombo());
comboService.getComboTypes().then((r) => comboTypes.value = r);
comboService.getAbilities().then((r) => {
    comboAbilities.value = r;
    comboHiddenAbility.value = r;
})

const baseStats = ref<Record<number, number>>({
    0: 45,
    1: 49,
    2: 45,
    3: 45,
    4: 30,
    5: 60
})

</script>
<style lang="scss" scoped>

</style>