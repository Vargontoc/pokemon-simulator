<template>
  <div class="filter-input">
    <label v-if="caption" class="form-label">{{ caption }}</label>

    <div class="filter-input-row">
      <select v-model="operator" class="filter-operator" @change="emitChange">
        <option v-for="op in filteredOperators" :key="op.value" :value="op.value">
          {{ op.label }}
        </option>
      </select>

      <template v-if="modelType === 'list' && combo">
        <div class="filter-combo-wrapper">
          <combo-multi-select
          ref="comboSelect"
            class="filter-combo"
            :model="combo"
            :type="'only-text'"
            :max-selected="combo.items?.length"
            :keys-selected="[]"
            @update-selected="onListChange"
          />
        </div>
      </template>

      <template v-else-if="modelType === 'number' || modelType === 'string'">
        <input
          class="form-input filter-input-right"
          :type="inputType"
          v-model="value"
          @input="emitChange"
        />
      </template>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ref, watch, computed } from 'vue';
import type { LCombo } from '@/models/LCombo';
import ComboMultiSelect from './ComboMultiSelect.vue';

const props = defineProps<{
  caption?: string,
  modelValue?: string | number | string[],
  operatorValue?: string,
  property: string,
  modelType: 'string' | 'number' | 'list',
  combo?: LCombo
}>();

const emit = defineEmits<{
  (e: 'update:modelValue', value: string | number | string[]): void,
  (e: 'update:operatorValue', value: string): void,
  (e: 'filter-changed', data: { operator: string, value: string | number | string[], property: string }): void
}>();

const value = ref<string | number>('');
const listValue = ref<string[]>([]);
const operator = ref<string>(props.operatorValue ?? 'equals');
const comboSelect = ref<InstanceType<typeof ComboMultiSelect>>();
const inputType = computed(() => props.modelType === 'number' ? 'number' : 'text');

const allOperators = [
  { label: 'Igual a', value: 'equals', types: ['string', 'number'] },
  { label: 'Distinto a', value: 'notEquals', types: ['string', 'number'] },
  { label: 'Mayor que', value: 'greaterThan', types: ['number'] },
  { label: 'Mayor o igual', value: 'greaterEqual', types: ['number'] },
  { label: 'Menor que', value: 'lessThan', types: ['number'] },
  { label: 'Menor o igual', value: 'lessEqual', types: ['number'] },
  { label: 'Empieza con', value: 'startsWith', types: ['string'] },
  { label: 'Contiene', value: 'contains', types: ['string'] },
  { label: 'Está en lista', value: 'inList', types: ['list'] },
  { label: 'No está en lista', value: 'notInList', types: ['list'] }
];

const filteredOperators = computed(() =>
  allOperators.filter(op => op.types.includes(props.modelType))
);

watch(() => props.modelValue, (val) => {
  if (Array.isArray(val)) {
    listValue.value = val;
  } else {
    value.value = val ?? '';
  }
});

watch(() => props.operatorValue, (val) => {
  operator.value = val ?? 'equals';
});

function emitChange() {
  const current = operator.value;
  const result = current === 'inList' || current === 'notInList' ? listValue.value : value.value;

  emit('update:modelValue', result);
  emit('update:operatorValue', current);
  emit('filter-changed', { operator: current, value: result, property: props.property });
}

function onListChange(newList: string[]) {
  listValue.value = newList;
  emitChange();
}

function clear() {
  if(comboSelect.value) {
    comboSelect.value.clearSelection();
  }
  value.value = '';
  listValue.value = [];
  operator.value = props.modelType == 'list' ? 'inList' : 'equals';
  emit('update:modelValue', '');
  emit('update:operatorValue', props.modelType === 'list' ? 'inList' : 'equals');

}

defineExpose({ clear });
</script>

<style scoped lang="scss">
.filter-operator {
  padding: .5rem .75rem;
  border: 1px solid #ccc;
  border-radius: 6px 0 0 6px;
  background-color: #f8f9fa;
  color: #333;
  height: 2.4rem;
  appearance: none;
  outline: none;
}

.filter-input {
  display: flex;
  flex-direction: column;
  width: 100%;
}

.filter-input-right {
  border-top-left-radius: 0;
  border-bottom-left-radius: 0;
  border-left: none;
}

.filter-combo-wrapper {
  flex: 1;

  .filter-combo {
    border-radius: 0 6px 6px 0;
    border-left: 0;
  }
}

.filter-input-row {
  display: flex;
}
</style>