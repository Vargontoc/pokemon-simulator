<template>
    <div class="battle-move-card"
     @click="move.pp > 0 && $emit('select-move', move.name)"
     :class="{ disabled: move.pp === 0}"
     @mouseenter="hovered = true"  
        @mouseleave="hovered = false" 
     >
       <div class="move-name">{{ move.displayName }}</div>
       <div class="move-pp">{{ move.pp }} / {{ move.maxPP }}</div>
       
       <!--  Float card -->
       <div v-if="hovered && move.pp" class="move-tooltip">
           <div class="move-icons" >
               <div class="move-icon" v-if="move.type">
                   <img v-if="move.type.icon" :src="move.type.icon" :key="move.type.name" />
                   <span v-else> {{ move.type.name }}</span>
                </div>
                <div class="move-icon" v-if="move.category">
                    <img v-if="move.category.icon" :src="move.category.icon" :key="move.category.name" />
                    <span v-else> {{ move.category.name }}</span>
                </div>
            </div>
            <div class="move-separator"></div>
            <div><strong> Potencia: </strong> {{ move.power ? move.power : '-' }}</div>
            <div><strong> Precisión: </strong> {{ move.accuracy ? move.accuracy : '-' }}</div>
            <div><strong> Prioridad: </strong> {{ move.priority }}</div>
            <div class="move-separator"></div>
            <div><strong> Efecto: </strong> {{ move.effect }}</div>
        </div>
    </div>
        
</template>
<script lang="ts" setup>
import type BattlerMoveInfo from '@/models/simulator/BattleMoveInfo';
import { ref } from 'vue';

defineProps<{ move: BattlerMoveInfo}>();

const hovered = ref<boolean>(false);

</script>
<style lang="scss" scoped>
.battle-move-card {
    width: 25%;
    position: relative;
    padding: .5rem;
    border: 2px solid #2a6ebc;
    background: #e3f2fd;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    border-radius: 8px;

    &:hover {
        background: #bbdefb;
    }

    &.disabled {
        background: #eee;
        color: #888;
        border-color: #aaa;
        cursor: not-allowed;

        .move-pp {
            color: #aaa;
        }

        &:hover {
            background: #eee;
        }
    }
    .move-name {
        font-weight: bold;
        font-size: .95rem;
        margin-bottom: .25rem;
        text-align: center;
    }

    .move-pp {
        font-size: .75rem;
        color: #555;
    }
}

.move-tooltip {
    position: absolute;
    top: 100%;
    left: 0;
    width: 150px;
    background: white;
    border: 1px solid #ccc;
    padding: .75rem;
    box-shadow: 0 4px 12px rgba(0,0,0, .15);
    border-radius: .5rem;
    z-index: 10;
    font-size: .85rem;

    .move-separator {
        width: 100%;
        border: 1px solid black;
        margin-bottom: .5rem;
        margin-top: .5rem;
    }

    .move-icons {
        display: flex;
        gap: .4rem;
        margin-bottom: .5rem;
        justify-content: flex-start;
        flex-wrap: wrap;
        margin-bottom: .5rem;

        .move-icon {
            display: flex;
            align-items: center;
            gap: .25rem;
            font-size: .75rem;
            padding: .2rem .4rem;
            border-radius: 4px;
            img {
                width: 4rem;
            }
        }
    }
}
</style>