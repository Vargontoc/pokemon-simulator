
<template>
    <div class="errors">
        <ul v-if="errors.length" class="error-list"> 
            <li v-for="(e, i) in errors" :key="i">{{ e }}</li>
        </ul>
    </div>
   
    <div class="form">
        <h2>{{ isEdit ? 'Editar tipo' : 'Crear tipo' }}</h2>
        <div class="form-row">
          <div class="form-group">
            <label>Icono</label>
            <l-upload :model="props.item.icon" @change-image="handleChangeIcon" />
          </div>
          
          <div class="form-group">
              <label>Nombre Interno</label>
              <input
                :readonly="isEdit"
                :class="{ invalid: props.item.id == 0 }"
                v-model="props.item.internalName"/>
          </div>
          
          <div class="form-group">
              <label>Nombre</label>
              <input v-model="props.item.name" :class="{ invalid: !props.item.name }"/>
          </div>
        </div>

        <div class="form-row">           
             <div class="form-group">
                <label>Debil a: </label>
                <l-combo-component :model="comboWeakness" :multi="true" :keys-selected="weakSelected"
                ></l-combo-component>
            </div>
            <div class="form-group">
                <label>Resistente a: </label>
                <l-combo-component :model="comboResistences" :multi="true" :keys-selected="resistenceSelected"
                ></l-combo-component>
            </div>
            <div class="form-group">
                <label>Inmune a: </label>
                <l-combo-component :model="comboWeakness" :multi="true" :keys-selected="inmunitiesSelected"
                ></l-combo-component>
            </div>
        </div>
    </div>

   <div class="actions">
      <button @click="$emit('save')" :disabled="!isValid">Guardar</button>
      <button @click="$emit('cancel')">Cancelar</button>
    </div>
</template>
<script setup lang="ts">
import type { Type } from '@/models/Type';
import { computed, ref } from 'vue';
import LComboComponent from '@/components/LComboComponent.vue';
import { ComboService } from '@/services/CombosService';
import client from '@/services/axios';
import { LCombo } from '@/models/LCombo';
import LUpload from '@/components/LUpload.vue';
interface Props {
    item: Type,
    isEdit: boolean
    errors: string[]
}
const props = defineProps<Props>()

const service = new ComboService(client);

const comboWeakness = ref<LCombo>(new LCombo());
const comboResistences = ref<LCombo>(new LCombo());
const comboInmunities = ref<LCombo>(new LCombo());

const weakSelected = computed((): string[] => {
    const c = props.item.weakness as LCombo;
    if(c.items === undefined)
        return [];
    return c.items?.map(m => m.key?? '') ?? [];
    
})

const resistenceSelected = computed((): string[] => {
    const c = props.item.resistences as LCombo;
    if(c.items === undefined)
        return [];
    return c.items?.map(m => m.key?? '') ?? [];
    
})

const inmunitiesSelected = computed((): string[] => {
    const c = props.item.inmunities as LCombo;
    if(c.items === undefined)
        return [];
    return c.items?.map(m => m.key?? '') ?? [];
    
})
const loadCombo = async () => {
    const response = await service.getComboTypes() as LCombo;
    comboWeakness.value = response;
    comboResistences.value = response;
    comboInmunities.value = response;
}
const isValid = computed(() => {
  return props.item.name?.trim() !== '' && props.item.internalName?.trim() !== '';
});

const handleChangeIcon = (icon: string) => {
  props.item.icon = icon;
}
loadCombo();
</script>

<style lang="scss" scoped>
.form {
  max-width: 500px;
  margin: auto;

  h2 {
    color: black;
  }
}
.error-list{
  li {
    color: red;
  }
}
.form-row {
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
}
.form-group {
  margin-bottom: 16px;
  display: flex;
  flex-direction: column;

  label {
    color: black;
    text-align: left;
  }
}

.full-width {
  flex: 1 1 100%;
}

input,
textarea {
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
}

.invalid {
  border-color: red;
}

.actions {
  display: flex;
  gap: 10px;
  justify-content: flex-end;
}
</style>