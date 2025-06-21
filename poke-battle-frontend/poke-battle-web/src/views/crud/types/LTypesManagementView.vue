<template>
    <div class="view">
        <l-filter-view :show="showFilter" @search-filter="handleRefresh"  @clean-filter="handleClear">
            <l-types-filter-view ref="filterView" :filter="filter"/>
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
        ></l-table>

        <l-modal :show="showEditionModal" :title="!!selected.id ? 'Editar: ' +  selected.name : 'Nuevo tipo'" @close-modal="handleClose">
            <l-types-edition-view ref="editionView"  :item="selected" :is-edit="!!selected.id" />
                <template #footer>
                    <div class="actions" style="display: flex; gap: .5rem;">
                        <button class="btn btn-add" @click.self="handleSave" :disabled="editionView && editionView.isValid == false">{{ !editionView ? '' : editionView.isEdit ?  'Actualizar' : 'Guardar' }}</button>
                        <button class="btn btn-cancel" @click.self="handleClose">Cancelar</button>
                    </div>
                </template>
        </l-modal>
    </div>
</template>

<script lang="ts" setup>
import type LColumn from '@/models/LColumn';
import type { LRow } from '@/models/LRow';
import { PageRequest } from '@/models/PageRequest';
import { PaginationData } from '@/models/PaginationData';
import { Type } from '@/models/Type';
import { TypesFilter } from '@/models/TypesFilter';
import client from '@/services/axios';
import { TypesService } from '@/services/TypesService'
import { ref } from 'vue';
import LTable from '@/components/LTable.vue';
import type { PageResponse } from '@/models/PageResponse';
import type { PageSize } from '@/models/PageSize';
import type { OrderBy } from '@/models/OrderBy';
import LActions from '@/components/LActions.vue';
import LFilterView from '@/components/LFilterView.vue';
import LTypesFilterView from './LTypesFilterView.vue';
import LModal from '@/components/LModal.vue';
import LTypesEditionView from '@/views/crud/types/LTypesEditionView.vue';
import { LComboItem } from '@/models/LComboItem';

// Service
const service = new TypesService(client);
const request = ref<PageRequest>(new PageRequest());
const filter = ref<TypesFilter>(new TypesFilter());
const pagination = ref<PaginationData>(new PaginationData());


// Table data
const columns = ref<LColumn[]>([
    { key: "i", display: 'Icono'},
    { key: "displayName", display: "Nombre" },
    { key: "weakness", display: "Debil a...", canOrder: false},
    { key: "resistences", display: "Resistente a...", canOrder: false},
    { key: "inmunities", display: "Inmune a...", canOrder: false}
])
const items = ref<LRow[]>([]); 
const selected = ref<Type>(new Type());

// Formularios
const showEditionModal = ref(false);
const showFilter = ref(false);
const editionView = ref<InstanceType<typeof LTypesEditionView>>();
const filterView = ref<InstanceType<typeof LTypesFilterView>>();
// Metodos
const getItems = async () => {
    const response = await service.getAll(filter.value, request.value) as PageResponse
    pagination.value.page = response.page;
    pagination.value.total = response.total;
    pagination.value.pageSize = response.pageSize;

    const results = response.results as Type[];

    items.value = [];
    results.forEach(d => {

        let w = d.weakness !== undefined ? d.weakness.items as LComboItem[]: [];
        let r = d.resistences !== undefined ? d.resistences.items  as LComboItem[]: [];
        let i = d.inmunities !== undefined ? d.inmunities.items as LComboItem[]: [];

        items.value.push({
            key: d.id,
            cells: [
                { type: 'icon', value: d.icon ?? '' },
                { type: 'text', value: d.name ?? '' },
                { type: 'combo', value: w},
                { type: 'combo', value: r },
                { type: 'combo', value: i }
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
        selected.value = await  service.get(id) as Type; 
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

const handleEdit = async (id:number) => {
    selected.value = await service.get(id) as Type;
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
    selected.value = new Type();
    showEditionModal.value = true;
}

const handleSave = async () => {
    if(selected.value.id === 0  || selected.value.id === undefined) {
        service.save(selected.value).then(() => {
            showEditionModal.value = false;
            getItems();
        })
    }else {
        service.update(selected.value).then(() => {
            showEditionModal.value = false;
            getItems();
        })
        
    }
    
}

const handleSearch = () => {
    showFilter.value = !showFilter.value
}

const handleClear = () => {
    if(filterView.value) {
        filterView.value.clearFilter();
    }
}

const handleClose =() => {
    showEditionModal.value = false; 
}
</script>