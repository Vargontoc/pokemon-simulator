<template>
<div class="icon-cell">
    <img v-if="icon" :src="icon" />
    <span v-else> - </span>
</div>
</template>

<script setup lang="ts">
defineProps({
    icon: String
})
</script>