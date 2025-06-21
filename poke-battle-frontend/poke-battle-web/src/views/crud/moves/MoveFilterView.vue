<template>
    <div class="form-view">
        <div class="form-row">
            <div class="form-group form-col col-1-5">   
                <filter-input
                    ref="filterName"
                    :property="'DisplayName'"
                    :model-type="'string'"
                    v-model="displayName"
                    operator-value="contains"
                    caption="Nombre" @filter-changed="onFilterProperty" />
            </div>
            <div class="form-group form-col col-1-5">
                <filter-input
                        ref="filterName"
                        :property="'Power'"
                        :model-type="'number'"
                        v-model="power"
                        operator-value="equals"
                        caption="Potencia" @filter-changed="onFilterProperty" />
            </div>          
            
            <div class="form-group form-col col-1-5">
                <filter-input
                        ref="filterName"
                        :property="'Accuracy'"
                        :model-type="'number'"
                        v-model="accuracy"
                        operator-value="equals"
                        caption="Precsión" @filter-changed="onFilterProperty" />
            </div>   

            <div class="form-group form-col col-1-5">
                <filter-input
                        ref="filterName"
                        :property="'Priority'"
                        :model-type="'number'"
                        v-model="priority"
                        operator-value="equals"
                        caption="Prioridad" @filter-changed="onFilterProperty" />
            </div>  
            
            <div class="form-group form-col col-1-5">
                <filter-input
                    ref="filterType"
                    :property="'Type'"
                    :model-type="'list'"
                    v-model="types"
                    :combo="comboType"
                    operator-value="inList"
                    caption="Tipo" @filter-changed="onFilterProperty" />
            </div> 

            <div class="form-group form-col col-1-5">
                <filter-input
                    ref="filterCategory"
                    :property="'Category'"
                    :model-type="'list'"
                    v-model="categories"
                    :combo="comboCategory"
                    operator-value="inList"
                    caption="Categoria" @filter-changed="onFilterProperty" />
            </div>  

        </div>

    </div>
</template>

<script setup lang="ts">
import { LCombo } from '@/models/LCombo';
import type { MovesFilter } from '@/models/MovesFilter';
import client from '@/services/axios';
import { ComboService } from '@/services/CombosService';
import { ref } from 'vue';
import FilterInput from '@/components/FilterInput.vue';
const service = new ComboService(client);
const props = defineProps<{filter: MovesFilter}>();
const comboType = ref<LCombo>(new LCombo());
const comboCategory = ref<LCombo>(new LCombo());
const filterCategory = ref<InstanceType<typeof FilterInput>>();
const filterType = ref<InstanceType<typeof FilterInput>>();

const displayName = ref<string>("");
const power = ref<number | undefined>(undefined);
const accuracy = ref<number | undefined>(undefined);
const priority = ref<number | undefined>(undefined);

const categories = ref<string[]>([]);
const types = ref<string[]>([]);

service.getMoveCategories().then(c => {
    comboCategory.value = c;
});

service.getComboTypes().then(c => {
    comboType.value = c;
});

function onFilterProperty(data: { operator: string; value: string | number | string[], property: string}) {
    const updateEntries = [...props.filter.entries];
    const index = updateEntries.findIndex(e => e.property === data.property);
    
    const isEmpty = (
        data.value === null ||
        data.value === undefined ||
        (typeof data.value === 'string' && data.value.trim() === '') ||
        (Array.isArray(data.value) && data.value.length === 0)
    );

    if(isEmpty && index !== -1) {
        // If the value is empty and the entry exists, remove it
        updateEntries.splice(index, 1);
        props.filter.entries = updateEntries;
        return;

    }

    const newEntry = {
        property: data.property,
        type: data.operator,
        value: data.value
    };

    if(index !== -1) {
        updateEntries[index] = newEntry;
    } else {
        updateEntries.push(newEntry);
    }
    
   

    props.filter.entries = updateEntries;
}



</script>