<template>
  <form class="form-view" @submit.prevent>
    <div class="form-row">

      <div class="form-group form-col col-1-3">
        <label for="name" class="form-label">Nombre Interno <span v-if="!isValidName" class="required-icon" title="Campo obligatorio">❗</span></label>
        <input id="name" class="form-input" :readonly="isEdit" :class="{'is-invalid' :  !isValidName }" v-model="item.name" placeholder="stench" @input="normalizeInternalName" />
      </div>

      <div class="form-group form-col col-1-2">
        <label for="displayName" class="form-label">Nombre <span v-if="!isValidDisplayName"  class="required-icon" title="Campo obligatorio">❗</span></label>
        <input id="displayName" class="form-input"  :class="{'is-invalid' : !isValidDisplayName }" v-model="item.displayName" placeholder="Hedor" @input="normalizeName" />
      </div>
    </div>


    <div class="form-row">
      <div class="form-group form-col col-1-1">
        <label for="description" class="form-label">Descripcion</label>
        <textarea id="description" class="form-input" v-model="item.description" placeholder="Descripción de la habilidad..." rows="4" ></textarea>
      </div>
    </div>
  </form>
</template>

<script lang="ts" setup>
import type { Ability } from '@/models/Ability';
import { computed, reactive, watch } from 'vue';


interface Props {
    item: Ability,
    isEdit: boolean
}
const props = defineProps<Props>()
const item = reactive({ ...props.item})

const emit = defineEmits<{
  (e: 'update:item', value: Ability): void
}>()

watch(item, (val) => { emit('update:item', val);}, { deep: true })

const isValid = computed(() => {
  if(!item.name || !item.displayName) return false;
  return item.name.trim() !== '' || item.displayName.trim() !== '';
});

const isValidName = computed((): boolean => {
  if(props.isEdit) return true;
  if(!item.name || item.name.trim() === '') return false;
  return true;
})

const isValidDisplayName = computed((): boolean => {
    if(!item.displayName || item.displayName.trim() === '') return false;
  return true;
})


function normalizeInternalName(e: Event) {
  const input = e.target as HTMLInputElement;
  props.item.name = input.value.toLowerCase().replace(/[^a-z\-]/g, '').trim();
}

function normalizeName(e: Event) {
  const input = e.target as HTMLInputElement;
  props.item.displayName = input.value.replace(/[^A-Za-zÁÉÍÓÚÜÑáéíóúüñ\s]/g, '');
}

function generated(generated: Ability) {
  
  emit('update:item', generated);
}

defineExpose({ isValid, generated })
</script>

