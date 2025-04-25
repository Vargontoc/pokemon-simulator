<template>
<div class="view">
    <l-actions @refresh="handleRefresh" />

    <l-table :columns="columns" :rows="items" :pagination="pagination"
        @sort-column="handleSort"
        @change-page="handleChangePage"
        @change-page-size="handleChangePageSize"
        @selected-item="handleSelected"
    />
</div>
</template>
<script setup lang="ts">
import type LColumn from '@/models/LColumn';
import type { LRow } from '@/models/LRow';
import { Move } from '@/models/Move';
import { MovesFilter } from '@/models/MovesFilter';
import { PageRequest } from '@/models/PageRequest';
import type { PageResponse } from '@/models/PageResponse';
import { PaginationData } from '@/models/PaginationData';
import client from '@/services/axios';
import { MovesService } from '@/services/MovesService';
import { ref } from 'vue';
import LTable from '@/components/LTable.vue';
import LActions from '@/components/LActions.vue';
import type { PageSize } from '@/models/PageSize';
import type { OrderBy } from '@/models/OrderBy';
const service = new MovesService(client);
const request = ref<PageRequest>(new PageRequest());
const filter = ref<MovesFilter>(new MovesFilter());
const pagination = ref<PaginationData>(new PaginationData())

const selected = ref<Move>(new Move());
const items = ref<LRow[]>([]);

const columns = ref<LColumn[]>([
    { key: "displayName", display: "Nombre"},
    { key: "type", display: "Tipo", canOrder: false},
    { key: "category", display: "Categoria", canOrder: false },
    { key: "power", display: "Potencia" },
    { key: "accuracy", display: "Precisión" },
    { key: "priority", display: "Prioridad" },
    { key: "description", display: "Descripción" }
])

const getItems = async () => {
    const response = await service.getAll(filter.value, request.value) as PageResponse;
    pagination.value.page = response.page;
    pagination.value.total = response.total;
    pagination.value.pageSize = response.pageSize;

    const results = response.results as Move[];
    items.value = [];
    results.forEach(d => {
        console.log(d);
        items.value.push({
            key: d.id,
            cells: [
                { type: 'text', value: d.name },
                { type: 'icon', value: d.type?.icon },
                { type: 'icon', value: d.iconCategory },
                { type: 'number', value: d.power },
                { type: 'number', value: d.accuracy },
                { type: 'number', value: d.priority },
                { type: 'text', value: d.description },

            ]
        });
    })
}

getItems();

const handleSelected = async (id: number | undefined) => {
    if(id !== undefined) {
        selected.value = await  service.get(id) as Move; 
    }else {
        selected.value.id = undefined;
    }
}

const handleChangePage = (page: number) => {
    request.value.Page = page;

    getItems();
}

const handleChangePageSize = (size: PageSize) => {
    request.value.Page = 1;
    request.value.PageSize = size

    getItems();
}

const handleSort = (event: OrderBy) =>{
    request.value.OrderBy = event;
    
    getItems();
}

const handleRefresh = () => {
    getItems()
}

</script>
