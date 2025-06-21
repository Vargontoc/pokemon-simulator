<template>
    <div class="table-container">
        <table class="table">
            <thead>
               <l-columns @sort="$emit('sort-column', $event)" :columns="columns"/>
            </thead>
            <tbody>
                <l-rows :rows="rows" @edit="$emit('edit', $event)" @remove="$emit('remove', $event)" />
            </tbody>
            <tfoot v-if="rows.length != 0">
                <l-paginator
                @update:page="$emit('change-page', $event)"
                @update:page-size="$emit('change-page-size', $event)"
                :page="pagination.page"
                :page-size="pagination.pageSize"
                 :total="pagination.total" />
            </tfoot>
        </table>
    </div>
</template>

<script setup lang="ts">
import LColumn from '../models/LColumn';
import type { LRow } from '../models/LRow';
import LColumns from './LColumns.vue';
import LRows from './LRows.vue';
import LPaginator from './LPaginator.vue';
import type { PaginationData } from '../models/PaginationData';
interface Props {
    columns: LColumn[],
    rows: LRow[],
    pagination: PaginationData
}
defineProps<Props>()
</script>

<style lang="scss" scoped>
.table {
    width: 100%;
    border: 1px solid #d1d5db;
    box-shadow: 0 1px 2px rgba(0, 0, 0, 0.1);
    border-radius: 0.25rem;
    overflow: hidden;
    
    thead { 
        background-color: #e5e7eb;
    }

    th, td {
        text-align: left;
        padding: 0.5rem 1rem;
    }

    tr:hover {
        background-color: #f9fafb;
    }
}
</style>