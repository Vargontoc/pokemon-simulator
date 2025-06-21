<template>
    <div class="move-section">
        <div class="form-row">
            <div class="form-col col-1-5">
                <combo-multi-select :type="'text-with-image'" 
                    :model="moveCombo"  :max-selected="1"
                    :keys-selected="[]" @update-selected="onSelectMove"
                ></combo-multi-select>
            </div>
            <div class="form-col col-1-5"></div>

            <div class="form-col col-1-5" v-if="showLevel">
                <input type="number" min="1" max="100" v-model.number="level" class="form-input" placeholder="1" />
            </div>

            <div class="form-col col-1-5">
                <button  class="btn btn-add" @click="onAddMove">Agregar</button>
            </div>
            <div class="form-col col-1-5"></div>
        </div>

        <table class="moves-table" >
            <thead>
                <tr>
                    <th></th>
                    <th>Movimiento</th>
                    <th v-if="showLevel">Nivel</th>
                </tr>
            </thead>

            <tbody>
                <tr v-for="(entry, i) in moves" :key="entry.id + '-' + i">
                    <td>
                        <button class="btn btn-remove"></button>
                    </td>
                    <td>{{ entry.name }}</td>
                    <td v-if="showLevel">{{ entry.level }}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script setup lang="ts">
import { LCombo } from '@/models/LCombo';
import ComboMultiSelect from './ComboMultiSelect.vue'
import { ref } from 'vue';
import { ComboService } from '@/services/CombosService';
import client from '@/services/axios';

interface MoveEntry {
    id: string,
    name: string,
    level?: number
}

const props = defineProps<{
    showLevel?: boolean,
    moves: MoveEntry[]
}>()

const emit = defineEmits(['add', 'remove']);

const comboService =  new ComboService(client);
const moveCombo = ref<LCombo>(new LCombo());
const level = ref(1);
const selectedMove = ref<string | null>(null);

comboService.getMoves().then((r) => { moveCombo.value = r; })

function onSelectMove(moves: string[]) {
    selectedMove.value =  moves[0] ?? null;
}

function onAddMove() {
    if(!selectedMove.value) return;
    const move = moveCombo.value.items.find(m  => m.key === selectedMove.value);
    
    if(!move) return;
    emit('add', {
        id: move.key,
        name: move.value,
        level: props.showLevel ? level.value : undefined
    });

    selectedMove.value = null;
    level.value = 1;
}
</script>

<style lang="scss" scoped>
.move-section {
    margin-bottom: 2rem;

    .move-caption {
        font-weight: bold;
        margin-bottom: .75rem;
    }

    .moves-table {
        width: 100%;
        border-collapse: collapse;
        margin-top: 1rem;

        th, td {
            padding: .5rem;
            border-bottom: 1px solid #ddd;
            text-align: left;
        }

        th {
            background: #f9f9f9;
        }

        td:last-child {
            text-align: right;
        }
    }
}
</style>