<script setup lang="ts">


</script>

<template>
    <div class="app-layout">
        <main class="content">
            <RouterView />
        </main>
    </div>
</template>

<style lang="scss" scoped>
.app-layout {
  display: flex;
  min-height: 100vh;
}

.content {
    flex-grow: 1;
    padding: 30px;
    background: #f5f6fa;
}
</style>