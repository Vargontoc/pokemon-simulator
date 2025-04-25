<template>
    <div class="dropdown-menu">
        <button @click="toogle" class="dropdown-button">{{ title }}</button>
        <div v-show="open" class="dropdown-content">
            <slot />
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

defineProps<{
    title: string
}>();
const open = ref(false);
const toogle = () => (open.value = !open.value);
</script>