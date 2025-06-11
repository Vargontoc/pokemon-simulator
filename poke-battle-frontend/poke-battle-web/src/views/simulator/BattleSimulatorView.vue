<template>
<div class="battle-simulator-view">
    <div class="battle-simulator-content">
        <h1>Team Builder</h1>
        <div class="form-battler">
            <battler-form ref="battlerForm"></battler-form>
            
          </div>
          <button class="btn btn-add" @click="onAddBattler">Add</button>

        <table>
            <thead>
                <tr>
                    <th></th>
                    <th>Specie</th>
                    <th>Level</th>
                    <th>Ability</th>
                    <th>Moves</th>
                    <th>Nature</th>
                    <th>Item</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(b, i) in battlers">
                    <td>
                        <button class="btn btn-remove"  @click="onRemoveBattler(i)">
                          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
    <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"/>
  </svg>
</button>
                    </td>
                    <td>{{ b.specie }}</td>
                    <td>{{ b.level }}</td>
                    <td>{{ b.ability }}</td>
                    <td>{{ b.moves }}</td>
                    <td>{{ b.nature }}</td>
                    <td>Not implemented</td>
                </tr>
            </tbody>
        </table>

    </div>
    <div>
        <button class="btn btn-start" :class="{disabled: battlers.length <= 0}" @click="onStartClick">Start Battle</button>
    </div>
</div>
</template>
<script lang="ts" setup>
import { LCombo } from '@/models/LCombo';
import client from '@/services/axios';
import { ComboService } from '@/services/CombosService';
import { ref } from 'vue';
import BattleService from '@/services/BattleService';
import { useRouter } from 'vue-router';
import BattlerForm from '@/components/simulator/BattlerForm.vue';
import type BuilderBattler from '@/models/simulator/BuilderBattler';

const specieService = new ComboService(client);
const battleService = new BattleService(client);

const species = ref<LCombo>(new LCombo());
const battlers  = ref<BuilderBattler[]>([]);
const battlerForm = ref<InstanceType<typeof BattlerForm>>();



const router = useRouter();
specieService.getSpecies().then((response: LCombo) => {
    species.value = response;
}).catch((error: any) => {
    console.error('Error fetching species:', error);
});

function onRemoveBattler(index: number) {
    battlers.value.splice(index, 1);
}

function onStartClick() {
    if(battlers.value && battlers.value.length != 0) {
        battleService.startBattle({
            battlers:  battlers.value
        }).then((response: string) => {
            router.push({name: 'Battle', params: { id: response }})
        }).catch((error: any) => {
            console.log('Error starting battle', error)
        })
    }
}

function onAddBattler() {
    if(battlerForm.value && battlers.value.length < 6 && battlerForm.value.battler.specie !== '')
    {   
        let b =  battlerForm.value.battler;
        b.ivs = battlerForm.value.getIvsEvs().ivs;
        b.evs = battlerForm.value.getIvsEvs().evs;

        battlers.value.push(Object.assign({},  b));
    }
}




</script>
<style lang="scss" scoped>
.form-battler {
  display: flex;
  align-items: center; // centra verticalmente
  gap: 1rem; // espacio entre elementos
  flex-wrap: wrap; // permite que se acomode bien en pantallas pequeñas (opcional)
}
table {
  width: 100%;
  border-collapse: collapse;
  margin: 1rem 0;
  font-family: 'Segoe UI', sans-serif;
  background-color: #fff;
  border-radius: 0.5rem;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);

  thead {
    background-color: #f5f5f5;

    th {
      text-align: left;
      padding: 1rem;
      font-weight: 600;
      color: #333;
    }
  }

  tbody {
    tr {
      border-top: 1px solid #eee;
      transition: background-color 0.3s;

      &:hover {
        background-color: #fafafa;
      }

      td {
        padding: 0.75rem 1rem;
        color: #444;

        &:first-child {
          text-align: center;
        }
      }
    }
  }
}





</style>