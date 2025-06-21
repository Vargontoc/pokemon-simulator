<template>
    <div class="form-upload" >
        <label class="upload-preview" @click="triggerUpload" :class="{ 'has-image': imageSrc }">
            <img v-if="imageSrc" :src="imageSrc" alt="Preview" />
            <span v-else>Select an image</span>
        </label>
        <input ref="fileInput" type="file" accept=" image/*" @change="onFileChange" hidden />
    </div>
</template>

<script lang="ts" setup>
import { ref, watch } from 'vue';

const props = defineProps<{
    model: string | undefined
}>()

const emit = defineEmits(['change-image'])
const imageSrc = ref<string | undefined>(props.model);
const fileInput = ref<InstanceType<typeof HTMLInputElement>>()
watch(() => props.model, (newValue) => { imageSrc.value = newValue; })

const onFileChange = (e: Event) => {
    if(fileInput.value && fileInput.value.files && fileInput.value.files[0])
    {
        const reader = new FileReader();
        reader.onload = () => {
            const base64 = reader.result as string;
            imageSrc.value = base64;
            emit('change-image', imageSrc);
    
        }
        reader.readAsDataURL(fileInput.value.files[0]);

    }

}

const triggerUpload = () => {
    if(fileInput.value)
        fileInput.value.click();
 }

</script>