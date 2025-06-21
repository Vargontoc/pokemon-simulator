<template>
    <div class="form-view">
        <div class="form-row">
            <div class="form-group form-col col-1-1">
                <filter-input
                    ref="filterName"
                    :property="'DisplayName'"
                    :model-type="'string'"
                    v-model="displayName"
                    operator-value="contains"
                    caption="Nombre"
                    @filter-changed="onFilterProperty" />
            </div>
        </div>
    </div>
</template>
<script lang="ts" setup>
import { AbilitiesFilter } from '@/models/AbilitiesFilter';
import FilterInput from '@/components/FilterInput.vue';
import { ref } from 'vue';
const props = defineProps<{
    filter: AbilitiesFilter
}>()

const filterName = ref<InstanceType<typeof FilterInput>>();
const displayName = ref('');



function onFilterProperty(data: { operator: string; value: string | number | string[], property: string}) {
    const updateEntries = [...props.filter.entries];
    const index = updateEntries.findIndex(e => e.property === data.property);

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
    if(filterName.value) filterName.value.clear();
    
}

defineExpose({ clearFilter})

</script>


