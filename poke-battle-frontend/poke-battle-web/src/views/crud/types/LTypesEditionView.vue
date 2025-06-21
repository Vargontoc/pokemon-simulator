
<template>
    <form class="form-view" @submit.prevent>
      <div class="form-row">
          <div class="form-group form-col col-1-4">
            <l-upload :model="item.icon" @change-image="handleChangeIcon" />
          </div>
          <div class="form-group form-col col-1-4">
            <label for="name" class="form-label">Nombre Interno <span v-if="!isValidName" class="required-icon" title="Campo obligatorio">❗</span></label>
            <input id="name" class="form-input"
              :readonly="isEdit"
              :class="{'is-invalid' : !isValidName }"
              v-model="item.internalName" placeholder="fire"
              @input="normalizeInternalName"/>
          </div>


          <div class="form-group form-col col-1-4">
              <label for="displayName" class="form-label">Nombre<span v-if="!isValidDisplayName"  class="required-icon" title="Campo obligatorio">❗</span></label>
              <input id="displayName"
               class="form-input"
               v-model="item.name"
               @input="normalizeName" 
               :class="{ 'is-invalid': !isValidDisplayName }" placeholder="Fuego"/>
          </div>
      </div>

      <div class="form-row">
        <div class="form-group form-col col-1-4">
          <combo-multi-select :type="'only-text'" caption="Resistencias" 
            :model="comboResistences"  :max-selected="comboResistences.items.length"
            :keys-selected="resistenceSelected" @update-selected="handleResistenceSelected"
            
            ></combo-multi-select>

        </div>
        <div class="form-group form-col col-1-4">
          <combo-multi-select :type="'only-text'" caption="Debilidades" 
            :model="comboWeakness"  :max-selected="comboWeakness.items.length"
            :keys-selected="weakSelected" @update-selected="handleWeaknessSelected"
            ></combo-multi-select>
        </div>

        <div class="form-group form-col col-1-4">
          <combo-multi-select :type="'only-text'" caption="Inmunidades" 
              :model="comboInmunities"  :max-selected="comboInmunities.items.length"
              :keys-selected="inmunitiesSelected" @update-selected="handleInmunitiesSelected"
              ></combo-multi-select>
        </div>


      </div>

       
        </form>
</template>
<script setup lang="ts">
import type { Type } from '@/models/Type';
import { computed, reactive, ref, watch } from 'vue';
import { ComboService } from '@/services/CombosService';
import client from '@/services/axios';
import { LCombo } from '@/models/LCombo';
import ComboMultiSelect from '@/components/ComboMultiSelect.vue';
import LUpload from '@/components/LUpload.vue';
interface Props {
    item: Type,
    isEdit: boolean
}
const props = defineProps<Props>()
const item = reactive({ ...props.item})

const emit = defineEmits<{
  (e: 'update:item', value: Type): void
}>()
watch(item, (val) => { emit('update:item', val);}, { deep: true })


const comboWeakness = ref<LCombo>(new LCombo());
const comboResistences = ref<LCombo>(new LCombo());
const comboInmunities = ref<LCombo>(new LCombo());

const service = new ComboService(client);
service.getComboTypes().then((response) => {
    comboWeakness.value = response;
    comboResistences.value = response;
    comboInmunities.value = response;
}).catch((error) => {
    console.error('Error loading combo types:', error);
});

// Computed properties for selected items
const resistenceSelected = computed(() => props.item.resistences?.items?.map(i => i.key!) ?? []);
const weakSelected = computed(() => props.item.weakness?.items?.map(i => i.key!) ?? []);
const inmunitiesSelected = computed(() => props.item.inmunities?.items?.map(i => i.key!) ?? []);




// Computed properties for validation
const isValidName = computed(() => props.isEdit || !!item.internalName?.trim());
const isValidDisplayName = computed(() => !!item.name?.trim());
const isValid = computed(() => !!item.name?.trim() && !!item.internalName?.trim());
defineExpose({ isValid })


// Handlers
function handleChangeIcon(icon: string) {
  item.icon = icon;
}

function normalizeInternalName(e: Event) {
  const val = (e.target as HTMLInputElement).value;
  props.item.internalName = val.toLowerCase().replace(/[^a-z\-]/g, '').trim();
}

function normalizeName(e: Event) {
  const val = (e.target as HTMLInputElement).value;
  props.item.name = val.replace(/[^A-Za-zÁÉÍÓÚÜÑáéíóúüñ\s]/g, '');
}

function handleResistenceSelected(selected: string[]) {
  props.item.resistences = new LCombo();
  props.item.resistences.items = selected.map(s => ({ key: s, checked: true, value: s }));
}

function handleWeaknessSelected(selected: string[]) {
  props.item.weakness = new LCombo();
  props.item.weakness.items = selected.map(s => ({ key: s, checked: true, value: s }));
}

function handleInmunitiesSelected(selected: string[]) {
  props.item.inmunities = new LCombo();
  props.item.inmunities.items = selected.map(s => ({ key: s, checked: true,  value: s }));
}
</script>
