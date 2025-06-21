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
                    ref="filterWeakness"
                    :property="'Resistences'"
                    :model-type="'list'"
                    v-model="resistences"
                    :combo="comboResistences"
                    operator-value="inList"
                    caption="Debil a" @filter-changed="onFilterProperty" />
            </div>

            <div class="form-group form-col col-1-5">
                <filter-input
                    ref="filterResistences"
                    :property="'weakness'"
                    :model-type="'list'"
                    v-model="weakness"
                    :combo="comboWeakness"
                    operator-value="inList"
                    caption="Debil a" @filter-changed="onFilterProperty" />
            </div>

            <div class="form-group form-col col-1-5">
                <filter-input
                    ref="filterInmunities"
                    :property="'Inmunities'"
                    :model-type="'list'"
                    v-model="inmunities"
                    :combo="comboInmunities"
                    operator-value="inList"
                    caption="Inmune a" @filter-changed="onFilterProperty" />
            </div>

        </div>


    </div>
</template>
<script lang="ts" setup>
import { LCombo } from '@/models/LCombo';
import type { TypesFilter } from '@/models/TypesFilter';
import client from '@/services/axios';
import { ComboService } from '@/services/CombosService';
import { ref } from 'vue';
import FilterInput from '@/components/FilterInput.vue';
const service = new ComboService(client);
const props = defineProps<{
    filter: TypesFilter
}>()

const filterName = ref<InstanceType<typeof FilterInput>>();
const filterResistences = ref<InstanceType<typeof FilterInput>>();
const filterWeakness = ref<InstanceType<typeof FilterInput>>();
const filterInmunities = ref<InstanceType<typeof FilterInput>>();

const comboWeakness = ref<LCombo>(new LCombo());
const comboResistences = ref<LCombo>(new LCombo());
const comboInmunities = ref<LCombo>(new LCombo());

const displayName = ref<string>("");
const weakness = ref<string[]>([]);
const resistences = ref<string[]>([]);
const inmunities = ref<string[]>([]);

service.getComboTypes().then((r) => {
    comboWeakness.value = r;
    comboResistences.value = r;
    comboInmunities.value = r;
})

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


function clearFilter() {
    props.filter.entries = [];
    if(filterName.value)
        filterName.value.clear();
    if(filterWeakness.value)
        filterWeakness.value.clear();
    if(filterResistences.value)
        filterResistences.value.clear();
    if(filterInmunities.value)
        filterInmunities.value.clear();
}

defineExpose({ clearFilter})

</script>