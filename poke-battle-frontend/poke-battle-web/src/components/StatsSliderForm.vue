<template>
    <div class="stats-form">
        <div class="stat-row" v-for="s in stats" :key="s.index">
            <label class="stat-label">{{ s.abbr }}</label>
            <input class="form-input stat-input" type="number" min="1" max="255" v-model.number="modelValue[s.index]" />
            <input class="stat-slider" type="range" min="1" max="255" v-model.number="modelValue[s.index]" :style="{
                '--fill-color' : getBarColor(modelValue[s.index]),
                '--fill-width': getBarWidth(modelValue[s.index]) + '%'
            }"/>
        </div>
    </div>
</template>

<script setup lang="ts">

defineProps<{
    modelValue: Record<number, number>
}>()

const stats = [
    { index: 0, abbr: 'PS'},
    { index: 1, abbr: 'At.'},
    { index: 2, abbr: 'Def.'},
    { index: 3, abbr: 'At. Esp.'},
    { index: 4, abbr: 'Def. Esp.'},
    { index: 5, abbr: 'Vel.'},
]

function getBarWidth(val: number) {
    return Math.min((val / 255)* 100, 100);
}

function getBarColor(val: number) {
    const pct = getBarWidth(val);
    if(pct > 75) return '#4caf50';
    if(pct > 50) return '#ffeb3b';
    if(pct > 30) return '#ff9880';
    return '#f44336';
}

</script>

<style lang="scss" scoped>

.stats-form {
    display: flex;
    flex-direction: column;
    gap: 1rem;
}

.stat-row {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.stat-label {
  width: 60px;
  text-align: right;
  font-weight: 600;
}

.stat-bar-container {
  background: #eee;
  height: 10px;
  width: 100px;
  border-radius: 4px;
  overflow: hidden;
  position: relative;
}

.stat-bar {
  height: 100%;
  transition: width 0.3s ease;
}

.stat-input {
  width: 60px;
  padding: 0.3rem;
  font-size: 0.9rem;
  text-align: center;
}

.stat-slider {
  flex: 1;
  height: 6px;
  border-radius: 4px;
  background: linear-gradient(to right, var(--fill-color, #4caf50) var(--fill-width, 0%), #ddd var(--fill-width, 0%));
  outline: none;
  appearance: none;

  &::-webkit-slider-thumb {
    appearance: none;
    width: 12px;
    height: 12px;
    background: #333;
    border-radius: 50%;
    cursor: pointer;
    position: relative;
    z-index: 2;
  }

  
  &::-moz-range-thumb {
    width: 12px;
    height: 12px;
    background: #333;
    border-radius: 50%;
    cursor: pointer;
    position: relative;
    z-index: 2;
  }

  &::-webkit-slider-runnable-track,
  &::-moz-range-track {
    background: transparent;
    height: 6px;
    border-radius: 4px;
  }
}

</style>