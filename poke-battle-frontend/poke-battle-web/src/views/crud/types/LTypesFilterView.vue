<template>
    <div>
        <div class="form-row">
            <div class="form-group">
                <label class="form-label">Nombre</label>
                <input class="form-input" v-model="props.filter.search"/>
            </div>
            <div class="form-group">
                <label class="form-label">Debil a: </label>
                <l-combo-component :nullable="true" :model="comboWeakness" :multi="false" :keys-selected="[]"
                @update-selected="handleWeak"></l-combo-component>
            </div>
            <div class="form-group">
                <label class="form-label">Resistente a: </label>
                <l-combo-component :nullable="true"  :model="comboResistences" :multi="false" :keys-selected="[]"
                @update-selected="handleResistence"></l-combo-component>
            </div>
            <div class="form-group">
                <label class="form-label">Inmune a: </label>
                <l-combo-component :nullable="true"  :model="comboWeakness" :multi="false" :keys-selected="[]"
                @update-selected="handleInmunity"></l-combo-component>
            </div>
        </div>
    </div>
</template>
<script lang="ts" setup>
import { LCombo } from '@/models/LCombo';
import type { TypesFilter } from '@/models/TypesFilter';
import client from '@/services/axios';
import { ComboService } from '@/services/CombosService';
import { ref } from 'vue';
import LComboComponent from '@/components/LComboComponent.vue';
const service = new ComboService(client);
const props = defineProps<{
    filter: TypesFilter
}>()


const loadCombo = async () => {
    const response = await service.getComboTypes() as LCombo;
    comboWeakness.value = response;
    comboResistences.value = response;
    comboInmunities.value = response;
}

const comboWeakness = ref<LCombo>(new LCombo());
const comboResistences = ref<LCombo>(new LCombo());
const comboInmunities = ref<LCombo>(new LCombo());

loadCombo();

const handleWeak = (keys: string[]) => 
{
    if(keys.length != 0) {
        props.filter.weakTo = keys[0] ?? "";
    }
}

const handleResistence = (keys: string[]) => 
{
    if(keys.length != 0) {
        props.filter.resistenceTo = keys[0] ?? "";
    }
}

const handleInmunity = (keys: string[]) => 
{
    if(keys.length != 0) {
        props.filter.inmunityTo = keys[0] ?? "";
    }
}
</script>