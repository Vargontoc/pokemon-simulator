<template>
    <div v-if="show" class="chatbot-view">
        <div class="chatbot-view-content">
            <div v-for="(message, index) in chatMessages" :key="index" class="chat-message" >
                <div class="message-text" v-html="message"></div>
            </div>
        </div>

        <div class="chatbot-view-input">
            <input v-model="messageTyped" type="text" max="1000" placeholder="Type a message..." />
            <button @click="handleClick">Send</button>
        </div>
    </div>
</template>

<script setup lang="ts">
import aiClient from '@/services/axios-ia';
import { GenAIService } from '@/services/GenAIService';
import { ref } from 'vue';

defineProps({
  show: Boolean
})

const iaService = new GenAIService(aiClient)
const messageTyped = ref<string>('');
const chatMessages = ref<string[]>([]);

const handleClick = async () => {
    if(messageTyped.value.trim() === '') return;

    await iaService.talkIA(messageTyped.value)
        .then((response: any) => {
            if(response.content) {
                chatMessages.value.push(messageTyped.value);
                chatMessages.value.push(response.content);

            }
            messageTyped.value = '';
        })
        .catch((error: any) => {
            console.error('Error:', error);
        });
    
}
</script>

<style lang="scss" scoped>
.chatbot-view {
    position: fixed;
    margin-top: 4.4rem;
    top: 0;
    right: 0;
    width: 460px;
    height: 600px;
    background-color: var(--primary-color);
    border-radius: 10px 10px 0 0;
    box-shadow: 0 -2px 10px rgba(0, 0, 0, 0.1);
    transition: transform 0.3s ease-in-out;
}

.chatbot-view-content {
    padding: 1rem;
    height: calc(100% - 50px);
    overflow-y: auto;

    &::-webkit-scrollbar {
        width: 8px;
    }
}

.chatbot-view-input {
    display: flex;
    align-items: center;
    padding: 0.5rem;
    background-color: var(--primary-color);
    border-top: 1px solid var(--border-color);

    input {
        flex-grow: 1;
        padding: 0.5rem;
        border-radius: 5px;
        border: 1px solid var(--border-color);
        margin-right: 0.5rem;
    }

    button {
        background-color: var(--primary-color);
        color: var(--text-color);
        border: none;
        padding: 0.5rem 1rem;
        border-radius: 5px;
        cursor: pointer;

        &:hover {
            background-color: var(--link-hover);
        }
    }
}

.chat-message {
    margin-bottom: 1rem;
    padding: 0.5rem;
    background-color: var(--bg-color);
    border-radius: 5px;

    .message-text {
        color: var(--text-color);
    }
}

</style>