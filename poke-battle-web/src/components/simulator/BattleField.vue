<template>
    <div class="battle-field">
        <div class="pokemon-enemy">
            <battler-card :battler="enemy[0]"/>
        </div>
        <div class="pokemon-player">
            <battler-card :battler="player[0]"/>
        </div>
    </div>
</template>

<script setup lang="ts">
import type { PBattler } from '@/models/simulator/PBattler';
import BattlerCard from '@/components/simulator/BattlerCard.vue';
defineProps<{
    player: PBattler[],
    enemy: PBattler[]
}>();
</script>