<template>
    <div>
        <div class="form-group">
            <label class="form-label">Nombre</label>
            <input class="form-input" v-model="props.filter.search"/>
        </div>
    </div>
</template>
<script lang="ts" setup>
import { AbilitiesFilter } from '@/models/AbilitiesFilter';

const props = defineProps<{
    filter: AbilitiesFilter
}>()
</script>
