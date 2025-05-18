<template>
    <div class="errors">
      <ul v-if="errors.length" class="error-list"> 
        <li v-for="(e, i) in errors" :key="i">{{ e }}</li>
      </ul>
    </div>
    <div class="form-row">
        <h2>{{ isEdit ? 'Editar habilidad' : 'Crear habilidad' }}</h2>
        <div class="form-row">
          <div class="form-group">
              <label class="form-label">Nombre Interno</label>
              <input class="form-input"
                :readonly="isEdit"
                :class="{ invalid: props.item.id == 0 }"
                v-model="props.item.name"/>
          </div>
          
          <div class="form-group">
              <label class="form-label">Nombre</label>
              <input class="form-input" v-model="props.item.displayName" :class="{ invalid: !props.item.name }"/>
          </div>
        </div>

        <div class="form-row">
          <div class="form-group">
              <label class="form-label">Descripción</label>
              <textarea class="form-input" v-model="props.item.description" rows="4"></textarea>
          </div>
        </div>
    </div>

    <div class="actions">
      <button @click="$emit('save')" :disabled="!isValid">Guardar</button>
      <button @click="$emit('cancel')">Cancelar</button>
    </div>
</template>

<script lang="ts" setup>
import  { Ability } from '@/models/Ability';
import { computed } from 'vue';


interface Props {
    item: Ability,
    isEdit: boolean
    errors: string[]
}
const props = defineProps<Props>()
const isValid = computed(() => {
  return props.item.name?.trim() !== '' && props.item.displayName?.trim() !== '';
});
</script>

