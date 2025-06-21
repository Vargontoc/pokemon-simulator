<template>
    <form @click.prevent class="form-view" style="margin-top: -2rem;">
    <div class="tabs-header">
        <button v-for="(tab, index) in tabs" :key="tab" :class="{ active: activeTab === index }" @click="activeTab = index">{{ tab }}</button>
    </div>
    <div class="tabs-content">
            <div v-show="activeTab === 0">
                <move-table-section :moves="[]" :show-level="true" />
            </div>
            <div v-show="activeTab === 1">
                <move-table-section :moves="[]" />
            </div>
            <div v-show="activeTab === 2">
                <move-table-section :caption="tabs[activeTab]" :moves="[]" />
            </div>
            <div v-show="activeTab === 3">
                <move-table-section :caption="tabs[activeTab]" :moves="[]" />
            </div>
        </div>
    </form>
</template>

<script setup lang="ts">
import MoveTableSection from '@/components/MoveTableSection.vue';
import { ref } from 'vue';

const tabs = ['Por Nivel', 'Huevo', 'MT - MO', 'Tutor'];
const activeTab = ref<number>(0);
</script>