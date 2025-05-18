<template>
    <div class="battle-simulator">
        <battle-field :enemy="enemy" :player="player" />
        <BattleEvents :events="events" />
        <battle-controls  @execute-turn="handleTurn" :player="player" />
    </div>
</template>

<script setup lang="ts">
import BattleField from '@/components/simulator/BattleField.vue';
import BattleEvents from '@/components/simulator/BattleEvents.vue';
import BattleControls from '@/components/simulator/BattleControls.vue';
import type { BattleEvent } from '@/models/simulator/BattleEvent';
import type { PBattler } from '@/models/simulator/PBattler';
import { onMounted, ref } from 'vue';
import client from '@/services/axios';
import { SimulatorService } from '@/services/SimulatorService';

const player = ref<PBattler[]>([])
const enemy = ref<PBattler[]>([])
const service = new SimulatorService(client);
const events = ref<BattleEvent[]>([])

const handleTurn = async () => {
    // TODO: Aplicar logica
}

onMounted(async () => {
const result = await service.startBattle();
player.value = result.player;
enemy.value = result.player;
})
</script>