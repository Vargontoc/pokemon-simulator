<template>
    <div 
        v-if="battler" class="battler-card" 
        :class="{
            'active': battler.isActive,
            'fainted' : battler.hp <= 0
        }"
        @mouseenter="hovered = true"  
        @mouseleave="hovered = false" 
        @click="$emit('switch-battler', battler.id)">
        <div class="battler-name">{{ battler.name }}</div>
        <div class="battler-hp">{{ battler.hp + ' / ' + battler.maxHp }}</div>
        <div class="battler-hp-bar" :class="computedLine" :style="computedWidth"></div>
        
        
        <div v-if="hovered && battler.hp > 0" class="battler-tooltip">
            <div class="battler-types" v-if="battler.types && battler.types.length">
                <div  class="battler-type" v-for="type in battler.types" :key="type.name">
                    <img v-if="type.icon" :src="type.icon" :alt="type.name" />
                    <span v-else>{{ type.name }}</span>
                </div>
            </div>

            <div><strong>Habilidad: </strong> Not Implemented</div>
            <div><strong>Item: </strong> Not Implemented</div>
            <div><strong>Naturaleza: </strong> Not Implemented </div>
            <div class="battler-stats">
                <div class="stat-row" v-for="stat in battler.stats" :key="stat.abbr">
                    <div class="stat-name">{{ stat.abbr }}</div>
                    <div class="stat-bar-container" :title="`Base: ${stat.value} |  IV: ${stat.iv} | EV: ${stat.ev} | Total: ${stat.raw}`">
                        <div  class="stat-bar" :class="getBarClass(stat.value)" :style="{width: computedBarWidth(stat.value) + '%'}"></div>
                    </div>
                    <div
                        class="mod-symbol"
                        v-if="stat.mod !== 0"
                        :style="{ color: getModifierColor(stat.mod) }"
                        >
                        {{ getModifierSymbol(stat.mod) }}
                        {{ getModifierMultiplier(stat.mod) }}%
                    </div>
                </div>
            </div>
        </div>


    </div>

    

</template>

<script lang="ts" setup>
import type BattlerInfo from '@/models/simulator/BattlerInfo';
import { computed, ref } from 'vue';
const props = defineProps<{ battler: BattlerInfo}>();
const hovered = ref<boolean>(false);
const computedWidth = computed(() => {
    return 'width: ' +  props.battler.hpPerentage + '%;';
});
const computedLine = computed(() => {
    return props.battler.hpPerentage > 0.5 ? 'line-green' : props.battler.hpPerentage > 0.25 ? 'orange' : 'line-red';
});

function computedBarWidth(total: number) : number {
    return Math.min((total /  255) * 100, 100);
}

function getBarClass(total: number): string {
    let percerntage = computedBarWidth(total);
    if(percerntage > 75) return 'bar-green'
    if(percerntage > 50) return 'bar-yellow'
    if(percerntage > 30) return 'bar-orange'
    return 'bar-red'
}

function getModifierMultiplier(mod: number): number {
  if (mod === 0) return 1
  const abs = Math.abs(mod)
  const multiplier = mod > 0
    ? (2 + abs) / 2
    : 2 / (2 + abs)
  return parseFloat(multiplier.toFixed(2))
}

function getModifierSymbol(mod: number) : string {
    if (mod > 0) return '↑'
    if (mod < 0) return '↓'
    return ''
}


function getModifierColor(mod: number): string {
  if (mod > 0) return '#4caf50' // verde
  if (mod < 0) return '#f44336' // rojo
  return '#666'
}

</script>
<style lang="scss" scoped>
.battler-card {
    position: relative;
    width: 15%;
    padding: .75rem;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    border-radius: 10px;
    cursor: pointer;
    background-color: #f0f4fA;
    transition: all .2s ease;
    box-shadow: 0 2px 4px rgba(0,0,0,.1);

    .battler-name {
        font-weight: bold;
        font-size: .95rem;
        margin-bottom: .25rem;
    }

    .battler-hp {
        font-size: .85rem;
        margin-bottom: .4rem;
    }

    .battler-hp-bar {
        height: 6px;
        width: 100%;
        border-radius: 3px;
    }

    &.active {
        background: #e0f7fa;
        border-color: #26c6da;
        box-shadow: 0 0 6px rgba(38, 198, 218, .6);
    }

    &.fainted {
        background: #fbe9e7;
        border-color: #ef5350;
        opacity: .6;
        cursor: not-allowed;

        .battler-hp-bar {
            background-color: #ef5350 !important;
        }
    }
}

.line-green {
    height: 5px;
    background-color: green;
}
.line-red {
    height: 5px;
    background-color: red;
}
.line-orange {
    height: 5px;
    background-color: orange;
}

.battler-tooltip {
    position: absolute;
    top: 100%;
    left: 0;
    width: 300px;
    background: white;
    border: 1px solid #ccc;
    padding: .75rem;
    box-shadow: 0 4px 12px rgba(0,0,0, .15);
    border-radius: 0.5rem;
    z-index: 10;
    font-size: .85rem;

    .battler-types {
        display: flex;
        gap: .4rem;
        margin-bottom: .5rem;
        justify-content: flex-start;
        flex-wrap: wrap;

        .battler-type {
            display: flex;
            align-items: center;
            gap: .25rem;
            font-size: .75rem;
            padding: .2rem .4rem;
            border-radius: 4px;
        }

        .battler-type img {
            width: 4rem;
        }
    }

    .battler-stats {
        display: flex;
        flex-direction: column;
        gap: .5rem;
        margin-top: .5rem;
        .stat-row {
            display: flex;
            align-items: center;
            gap: .5rem;

            .stat-name {
                width: 60px;
                font-weight: bold;
                text-align: right;
                font-size: .85rem;
            }

            .stat-bar-container {
                flex: 1;
                background: #eee;
                height: 10px;
                border-radius: 4px;
                overflow: hidden;
                position: relative;
                cursor: help;

                .stat-bar {
                    height: 100%;
                    transition: width .3s ease;
                    border-radius: 4px;

                    &.bar-green {
                        background-color: #4caf50;;
                    }
                    
                    &.bar-yellow {
                        background-color: #ffeb3b;
                    }

                    &.bar-orange {
                        background-color: #ff9800;
                    }


                    &.bar-red {
                        background-color: #f44336;
                    }
                }

                .mod-symbol {
                    font-size: .75rem;
                    font-weight: bold;
                    width: 60px;
                    text-align: left;
                }
            }

            .stat-meta {
                font-size: .75rem;
                color: #666;
            }
        }
    }
}

</style>