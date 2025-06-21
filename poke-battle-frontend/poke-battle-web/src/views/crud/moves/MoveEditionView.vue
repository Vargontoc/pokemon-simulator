<template>
    <div class="form-view">
        <div class="form-row">
            <div class="form-group form-col col-1-4">
                <label for="name" class="form-label">Nombre Interno</label>
                <input id="name" class="form-input" :readonly="isEdit" placeholder="tackle" />
            </div>

            <div class="form-group form-col col-1-4">
                <label for="displayName" class="form-label">Nombre</label>
                <input id="displayName" class="form-input" placeholder="Placaje" />
            </div>
            
            <div class="form-group form-col col-1-4">
                <combo-multi-select :type="'only-text'" caption="Tipo" 
                    :model="comboTypes"  :max-selected="1"
                    :keys-selected="[]" @update-selected="handleType"
                ></combo-multi-select>
            </div>

            <div class="form-group form-col col-1-4">
                <combo-multi-select :type="'text-with-image'" caption="Categoria" 
                    :model="comboCategory"  :max-selected="1"
                    :keys-selected="[]" @update-selected="handleCategory"
                ></combo-multi-select>
            </div>
        </div>
        
        <div class="form-row">
            <div class="form-group form-col col-1-5">
                <label for="power" class="form-label">Potencia</label>
                <input id="power" class="form-input" type="number" min="0" max="255" placeholder="90" />
            </div>

            <div class="form-group form-col col-1-5">
                <label for="accuracy" class="form-label">Precision</label>
                <input id="accuracy" class="form-input" type="number" min="0" max="100" placeholder="100" />
            </div>

            <div class="form-group form-col col-1-5">
                <label for="priority" class="form-label">Prioridad</label>
                <input id="priority" class="form-input" type="number" min="-6" max="6" placeholder="0" />
            </div>

            <div class="form-froum">

            </div>

            <div class="form-group form-col col-1-5">
                <label for="priority" class="form-label">Efectos</label>
                <input id="effects" class="btn btn-search" type="button" title="Efectos" />
            </div>
        </div>

        <div class="form-row">
            <div class="form-group form-col col-1-5">
                <label for="description" class="form-label">Descripcion</label>
                <textarea id="description" class="form-input"  placeholder="Descripción del movimiento..." rows="4" ></textarea>
            </div>
        </div>
    </div>
</template>

<script lang="ts" setup>
import ComboMultiSelect from '@/components/ComboMultiSelect.vue';
import { LCombo } from '@/models/LCombo';
import client from '@/services/axios';
import { ComboService } from '@/services/CombosService';
import { ref } from 'vue';
defineProps<{
    isEdit: boolean
}>()


const comboService = new ComboService(client);
const comboTypes = ref<LCombo>(new LCombo());
const comboCategory = ref<LCombo>(new LCombo());
comboService.getComboTypes().then((r) => {
    comboTypes.value = r;
})

comboService.getMoveCategories().then((r) => {
    comboCategory.value = r;
}) 


function handleType(keys: string[]) {
    // Empty method
} 

function handleCategory(keys: string[]) {
    // Empty method
} 
</script>