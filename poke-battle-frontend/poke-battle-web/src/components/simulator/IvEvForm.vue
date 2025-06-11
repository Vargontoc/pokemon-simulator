<template>
  <div class="iv-ev-form">
    <div class="column">
      <h3>IVs</h3>
      <div v-for="stat in stats" :key="'iv-' + stat.item1" class="stat-row">
        <label>{{ stat.item2 }}</label>
        <input type="range" min="0" max="31" v-model.number="ivs[stat.item1]" />
        <span class="value">{{ ivs[stat.item1] }}</span>
      </div>
    </div>

    <div class="column">
      <h3>EVs <span :class="{ over: totalEvs > 510 }">({{ totalEvs }}/510)</span></h3>
      <div v-for="stat in stats" :key="'ev-' + stat.item1" class="stat-row">
        <label>{{ stat.item2 }}</label>
        <input
          type="range"
          min="0"
          max="252"
          v-model.number="evs[stat.item1]"
          @input="enforceEvLimit(stat.item1)"
        />
        <span class="value">{{ evs[stat.item1] }}</span>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import client from '@/services/axios';
import { ComboService } from '@/services/CombosService';
import {  reactive, computed, defineExpose, ref } from 'vue'

const service = new ComboService(client);
const stats = ref<{item1: number, item2: string}[]>([])

service.getStats().then(r => {
   
    r.forEach(d => {
        stats.value?.push({
            item1: d.item1,
            item2: d.item2
        });

    })

    reset();
})
// Props: array de objetos con índice y abreviatura


// Crear estructuras reactivas para ivs y evs
const ivs = reactive<Record<number, number>>({})
const evs = reactive<Record<number, number>>({})



// Calcular total de EVs
const totalEvs = computed(() =>
  Object.values(evs).reduce((sum, val) => sum + val, 0)
)

// Limitar EVs a 510
function enforceEvLimit(changedIndex: number) {
  const total = totalEvs.value
  if (total > 510) {
    const overflow = total - 510
    evs[changedIndex] = Math.max(0, evs[changedIndex] - overflow)
  }
}

function reset() {
      stats.value?.forEach(({item1}) => {
        ivs[item1] = 0
        evs[item1] = 0
    })
}

// Exponer valores como arrays de tuplas [index, value]
defineExpose({
  reset,
  get ivArray(): {item1: number, item2: number}[] {
    if(!stats.value)
        return [];
    return stats.value.map(stat => {
        return {
            item1: stat.item1,
            item2: ivs[stat.item1]
        }
    })
  },
  get evArray(): {item1: number, item2: number}[] {
    if(!stats.value)
        return [];
    return stats.value.map(stat => ({
        item1: stat.item1,
        item2: evs[stat.item1]
    }))
  }
  
})
</script>

<style scoped>
.iv-ev-form {
  display: flex;
  flex-wrap: wrap;
  gap: 2rem;
}

.column {
  flex: 1;
  min-width: 250px;
}

h3 {
  margin-bottom: 0.5rem;
  color: #444;
}

h3 span {
  font-size: 0.9rem;
  font-weight: normal;
  color: #666;
}

h3 span.over {
  color: red;
  font-weight: bold;
}

.stat-row {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 0.75rem;
}

label {
  width: 80px;
  font-weight: 500;
}

input[type='range'] {
  flex: 1;
}

.value {
  width: 30px;
  text-align: right;
  font-family: monospace;
}
</style>