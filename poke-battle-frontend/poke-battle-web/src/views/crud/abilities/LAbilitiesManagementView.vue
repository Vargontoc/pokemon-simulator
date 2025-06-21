<template>
<div class="view">
    <l-filter-view :show="showFilter" @search-filter="handleRefresh"  @clean-filter="handleClear">
        <l-abilities-filter-view ref="filterView" :filter="filter"/>
    </l-filter-view>
    
    <l-actions 
        @refresh="handleRefresh"
        @create="handleCreate"
        @search="handleSearch" />

    <l-table :columns="columns" :rows="items" :pagination="pagination"
        @sort-column="handleSort"
        @change-page="handleChangePage"
        @change-page-size="handleChangePageSize"
        @selected-item="handleSelected"
        @edit="handleEdit"
        @remove="handleRemove"

    />
    
    <l-modal :show="showEditionModal" :title="!!selected.id ? 'Editar: ' +  selected.displayName : 'Nueva habilidad'" @close-modal="handleClose" >
        <l-abilities-edition-view ref="editionView" v-model:item="selected" :is-edit="!!selected.id" />
        <template #footer>
            <div class="actions" style="display: flex; gap: .5rem;" @click.self="handleGenerate">
                <button class="btn btn-generate">
                    <svg class="icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24">
                    <path d="M13 2v2a7 7 0 1 1-7 7H4a9 9 0 1 0 9-9z" fill="currentColor"/>
                    <path d="M12 6v6l4 2" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                </svg>Generar </button>
                    <button class="btn btn-add" @click.self="handleSave" :disabled="!editionView?.isValid">{{ !editionView ? '' : editionView.isEdit ?  'Actualizar' : 'Guardar' }}</button>
                    <button class="btn btn-cancel" @click.self="handleClose">Cancelar</button>
                </div>
        </template>
    </l-modal>
    
</div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import LTable from '@/components/LTable.vue';
import type LColumn from '@/models/LColumn';
import { AbilitiesService } from '@/services/AbilitiesService';
import client from '@/services/axios';
import type { LRow } from '@/models/LRow';
import type { PageResponse } from '@/models/PageResponse';
import { PageRequest } from '@/models/PageRequest';
import type { PageSize } from '@/models/PageSize';
import { AbilitiesFilter } from '@/models/AbilitiesFilter';
import type { OrderBy } from '@/models/OrderBy';
import { PaginationData } from '@/models/PaginationData';
import LModal from '@/components/LModal.vue';
import  LAbilitiesEditionView  from '@/views/crud/abilities/LAbilitiesEditionView.vue';
import LActions from '@/components/LActions.vue';
import LFilterView from '@/components/LFilterView.vue';
import LAbilitiesFilterView from './LAbilitiesFilterView.vue';
import type { Ability } from '@/models/Ability';
import { useToasts } from '@/composables/UseToats';
const service = new AbilitiesService(client);
const request = ref<PageRequest>(new PageRequest());
const filter = ref<AbilitiesFilter>(new AbilitiesFilter());
const pagination = ref<PaginationData>(new PaginationData());
const editionView = ref<InstanceType<typeof LAbilitiesEditionView>>();
const filterView = ref<InstanceType<typeof LAbilitiesFilterView>>();
const selected = ref<Ability>({});
const items = ref<LRow[]>([]);

const showEditionModal = ref(false);
const showFilter = ref(false);
const { showToast } = useToasts();  


const columns = ref<LColumn[]>([
    { key: "displayName", display: "Nombre" },
    { key: "description", display: "Descripción" }
])

const getItems = async () => {
    const response = await service.getAll(filter.value, request.value) as PageResponse
    pagination.value.page = response.page;
    pagination.value.total = response.total;
    pagination.value.pageSize = response.pageSize;

    const results = response.results as Ability[];
    items.value = [];
    results.forEach(d => {
        items.value.push({
            key: d.id,
            cells: [
                { value: d.displayName, type: 'text'},
                { value: d.description, type: 'text'}
            ]
        })
    })
}

getItems();

const handleChangePage = (page: number) => {
    request.value.Page = page;

    getItems();
}

const handleChangePageSize = (size: PageSize) => {
    request.value.Page = 1;
    request.value.PageSize = size

    getItems();
}

const handleSelected = async (id: number | undefined) => {
    if(id !== undefined) {
        selected.value = await  service.get(id) as Ability; 
    }else {
        selected.value.id = undefined;
    }
}
const handleSort = (event: OrderBy) =>{
    request.value.OrderBy = event;
    
    getItems();
}

const handleRefresh = () => {
    getItems()
}

const handleEdit = async  (id: number) => {
    selected.value = await service.get(id) as Ability;
    if(selected.value.id) {
        showEditionModal.value = true;
    }
}

const handleRemove = async (id: number) => {
    await service.delete(id).then(() => {
        getItems();
    });

}

const handleCreate = () => {
    selected.value = {
        name: '',
        displayName: '',
        id: undefined
    } as Ability;
    showEditionModal.value = true;
}

const handleSave = async () => {
    
    
    if(selected.value.id === 0  || selected.value.id === undefined) {
        service.save(selected.value).then(() => {
            showEditionModal.value = false;
            getItems();
            showToast('success', 'Habilidad creada correctamente');
        })
    }else {
        service.update(selected.value).then(() => {
            showEditionModal.value = false;
            getItems();
            showToast('success', 'Habilidad actualizada correctamente');
        })
    }
    
}

const handleSearch = () => {
    showFilter.value = !showFilter.value
}

const handleClear = () => {
    filterView.value?.clearFilter();
}

const handleClose =() => {
    showEditionModal.value = false; 
}

const handleGenerate = async () => {
    await service.generateAbilities().then((r) => {
        if(editionView.value) {
            editionView.value.generated(r);
        }
    })
}

</script>