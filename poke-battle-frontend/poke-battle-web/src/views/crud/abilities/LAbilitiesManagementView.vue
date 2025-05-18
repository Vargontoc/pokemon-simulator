<template>
<div class="view">
    <l-filter-view :show="showFilter" @search-filter="handleRefresh"  @clean-filter="handleClear">
        <l-abilities-filter-view :filter="filter"/>
    </l-filter-view>
    
    <l-actions 
        @refresh="handleRefresh"
        @edit="handleEdit"
        @create="handleCreate"
        @search="handleSearch" />

    <l-table :columns="columns" :rows="items" :pagination="pagination"
        @sort-column="handleSort"
        @change-page="handleChangePage"
        @change-page-size="handleChangePageSize"
        @selected-item="handleSelected"
    />
    
    <l-modal :show="showEditionModal" :title="!!selected.id ? 'Editar: ' +  selected.displayName : 'Nueva habilidad'" @close-modal="handleClose" >
        <l-abilities-edition-view :errors="errors" :item="selected" :is-edit="!!selected.id" 
        @save="handleSave"
        @cancel="handleClose"/>
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
import { Ability } from '@/models/Ability';
import LActions from '@/components/LActions.vue';
import LFilterView from '@/components/LFilterView.vue';
import LAbilitiesFilterView from './LAbilitiesFilterView.vue';

const service = new AbilitiesService(client);
const request = ref<PageRequest>(new PageRequest());
const filter = ref<AbilitiesFilter>(new AbilitiesFilter());
const pagination = ref<PaginationData>(new PaginationData());

const selected = ref<Ability>(new Ability());
const items = ref<LRow[]>([]);

const showEditionModal = ref(false);
const showFilter = ref(false);
const errors = ref<string[]>([]);

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

const handleEdit = () => {
    if(selected.value.id) {
        showEditionModal.value = true;
    }
}

const handleCreate = () => {
    selected.value = new Ability();
    showEditionModal.value = true;
}

const handleSave = async () => {


        if(selected.value.id === 0  || selected.value.id === undefined) {
            service.save(selected.value).then(() => {
                errors.value = [];
                showEditionModal.value = false;
            }).catch((err) => {
                errors.value = err;
            });
        }
    
}

const handleSearch = () => {
    showFilter.value = !showFilter.value
}

const handleClear = () => {
    filter.value.search = '';
}

const handleClose =() => {
    showEditionModal.value = false; 
    errors.value = [];
}
</script>