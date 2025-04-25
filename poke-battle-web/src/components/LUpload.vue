<template>
    <div class="upload-image">
        <input type="file" accept="image/*" @change="onFileChange">

        <div v-if="imageSrc" class="preview">
            <img :src="imageSrc" alt="Icono" style="max-width: 80px" />
        </div>
    </div>
</template>

<script lang="ts" setup>
import { ref, watch } from 'vue';

const props = defineProps<{
    model: string | undefined
}>()

const emit = defineEmits(['change-image'])
const imageSrc = ref<string | undefined>(props.model);
watch(() => props.model, (newValue) => { imageSrc.value = newValue; })

const onFileChange = (e: Event) => {
    const target = e.target as HTMLInputElement;
    const file = target.files?.[0];
    if(!file) return;

    const reader = new FileReader();
    reader.onload = () => {
        const base64 = reader.result as string;
        imageSrc.value = base64;
        emit('change-image', imageSrc);

    }
    reader.readAsDataURL(file);
}

</script>