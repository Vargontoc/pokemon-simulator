<template>
    <div class="form-row">
        <div class="form-group">
            <label class="form-label">Nombre: </label>
            <input  class="form-input" v-model="props.filter.search"/>
        </div>
        <div class="form-group">
            <label class="form-label">Tipo: </label>
            <l-combo-component :nullable="true" :model="comboType" :multi="false" :keys-selected="[]"></l-combo-component>
        </div>
        <div class="form-group">
            <label class="form-label">Categoria: </label>
            <l-combo-component :nullable="true" :model="comboCategory" :multi="false" :keys-selected="[]"></l-combo-component>
        </div>
    </div>

    <div class="form-row">
        <p-input-number-filter caption="Potencia"></p-input-number-filter>
        <p-input-number-filter caption="Precisión"></p-input-number-filter>
        <p-input-number-filter caption="Prioridad"></p-input-number-filter>
    </div>
</template>

<script setup lang="ts">
import { LCombo } from '@/models/LCombo';
import type { MovesFilter } from '@/models/MovesFilter';
import client from '@/services/axios';
import { ComboService } from '@/services/CombosService';
import { ref } from 'vue';
import LComboComponent from '@/components/LComboComponent.vue';
import PInputNumberFilter from '@/components/PInputNumberFilter.vue';

const service = new ComboService(client);
const props = defineProps<{filter: MovesFilter}>();
const comboType = ref<LCombo>(new LCombo());
const comboCategory = ref<LCombo>(new LCombo());

const loadCombos = async () => {

    // Combo types
    const rTypes = await  service.getComboTypes() as LCombo;
    comboType.value = rTypes;

    // Combo category
    const rCategory = await service.getMoveCategories() as LCombo;
    comboCategory.value = rCategory;
}

loadCombos();
</script>