<template>
    <div class="battle-events">
        <ul>
            <li v-for="(e, i) in events" :key="i">{{ e.message }}</li>
        </ul>
    </div>
</template>
<script lang="ts" setup>
import type { BattleEvent } from '@/models/simulator/BattleEvent';
defineProps<{
    events: BattleEvent[]
}>();
</script>