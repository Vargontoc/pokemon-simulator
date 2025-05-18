<template>
    <div v-if="battler" class="battler-card">
        <div>{{ battler.nickname }}</div>
        <div>{{ battler.currentHp }} / {{ battler.getStat('hp')?.rawValue }}</div>
    </div>
</template>

<script lang="ts" setup>
import type { PBattler } from '@/models/simulator/PBattler';
defineProps<{ battler: PBattler}>();
</script>