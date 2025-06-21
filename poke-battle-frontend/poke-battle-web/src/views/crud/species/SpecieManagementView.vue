<template>
    <div class="view">
        <l-filter-view :show="showFilter">

        </l-filter-view>
        <l-actions @create="() => { showEditionModal = true; }"/>
        
        <l-modal :show="showEditionModal" :title="'Nueva especie'" @close-modal="() => { showEditionModal = false }">
            <specie-edition-view ref="specieEditionView" :model="selected" />
                    <template #footer>
            <div class="actions" style="display: flex; gap: .5rem;">
                <button class="btn btn-add" > {{ 'Guardar' }}</button>
                <button class="btn btn-cancel" @click="() => { showEditionModal = false }">Cancelar</button>
            </div>
        </template>
        </l-modal>
    </div>

</template>

<script setup lang="ts">
import LFilterView from '@/components/LFilterView.vue';
import LActions from '@/components/LActions.vue';
import { ref } from 'vue';
import LModal from '@/components/LModal.vue';
import SpecieEditionView from '@/views/crud/species/SpecieEditionView.vue';
import type Specie from '@/models/Specie';

const selected = ref<Specie>({
    id: '',
    name: '',
    category: '',
    despcription: ''
})
const showFilter = ref(false);
const showEditionModal = ref(false);
const specieEditionView = ref<InstanceType<typeof SpecieEditionView>>()

</script>

