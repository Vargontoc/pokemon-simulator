<template>
        <div class="form-row">
            <combo-multi-select 
                @update-selected="handleChangeSpecie" 
                :type="'only-text'" 
                :model="species" 
                :keys-selected="[]" 
                caption="Pokemon" />
            
            <div class="form-group">
                <label for="level" class="form-label" >Nivel</label>
                <input v-model="battler.level" class="form-input" id="level" type="number" min="1" max="100"  placeholder="Level.." />
            </div>
            
            <combo-multi-select 
                @update-selected="handleChangeNature" 
                :type="'only-text'" 
                :model="natures" 
                :keys-selected="[]"  
                caption="Naturaleza" />
            
            <combo-multi-select 
                @update-selected="handleChangeAbility" 
                :type="'only-text'" 
                :model="abilities" 
                :keys-selected="[]" 
                caption="Habilidad" />

            <combo-multi-select :type="'only-text'" :model="items" :keys-selected="[]" caption="Objeto" />
        </div>
        <div class="form-row">
            <iv-ev-form ref="iv-ev-form"></iv-ev-form>
            <div class="move-selection">
            <combo-multi-select @update-selected="(selected: string[]) => { handleChangeMove(selected, 0)}"   :type="'only-text'" :model="moves" :keys-selected="[]" />
            <combo-multi-select :type="'only-text'" :model="moves" :keys-selected="[]"  />
            <combo-multi-select :type="'only-text'" :model="moves" :keys-selected="[]" />
            <combo-multi-select :type="'only-text'" :model="moves" :keys-selected="[]" />
            </div>
        </div>

</template>
<script setup lang="ts">
import { LCombo } from '@/models/LCombo';
import client from '@/services/axios';
import { ComboService } from '@/services/CombosService';
import { ref } from 'vue';
import type BuilderBattler from '@/models/simulator/BuilderBattler';
import IvEvForm from '@/components/simulator/IvEvForm.vue';
import ComboMultiSelect from '@/components/ComboMultiSelect.vue';
const species = ref<LCombo>(new LCombo());
const natures = ref<LCombo>(new LCombo());
const abilities = ref<LCombo>(new LCombo());
const items = ref<LCombo>(new LCombo());
const moves = ref<LCombo>(new LCombo());

const comboService = new ComboService(client);

const battler = ref<BuilderBattler>({} as BuilderBattler);
const ivEvForm = ref<InstanceType<typeof IvEvForm>>();

comboService.getSpecies().then((response: LCombo) => {
    species.value = response;
}).catch((error: any) => {
    console.error('Error fetching species:', error);
});

comboService.getNatures().then((response: LCombo) => {
    natures.value = response;
}).catch((error: any) => {
    console.error('Error fetching natures:', error);
});

comboService.getAbilities().then((response: LCombo) => {
    abilities.value = response;
}).catch((error: any) => {
    console.error('Error fetching natures:', error);
});

comboService.getMoves().then((response: LCombo) => {
    moves.value = response;
    battler.value.moves = ['','','','']
}).catch((error: any) => {
    console.error('Error fetching natures:', error);
});

const handleChangeSpecie = (keys: string[]) => {
    if (keys.length > 0) {
        battler.value.specie = keys[0] ?? undefined;
    }
};

function handleChangeNature(keys: string[]) {
    if (keys.length > 0) {
        battler.value.nature = keys[0] ?? undefined;
    }else {
        battler.value.nature = undefined;
    }
}

function handleChangeAbility(keys: string[]) {
    if (keys.length > 0) {
        battler.value.ability = keys[0] ?? undefined;
    }else {
        battler.value.ability = undefined;
    }
}

function handleChangeMove(keys: string[], index: number) {
    if(!battler.value.moves) return;

    if (keys.length > 0) {
        battler.value.moves[index] = keys[0] ?? '';
    }else {
        battler.value.moves[index] = '';
    }
}

function reset() 
{
    if(ivEvForm.value)
        ivEvForm.value.reset();
}

function getIvsEvs() {
    if(ivEvForm.value) {
        let ivs = ivEvForm.value.ivArray
        let evs = ivEvForm.value.evArray;

        return {
            ivs: ivs,
            evs: evs
        }
    }
    return {};
}




defineExpose({
    battler, getIvsEvs
});
</script>
<style lang="scss" scoped></style>