import type ToastMessage from "@/models/ToastMessage";
import { ref } from "vue";

const toasts = ref<ToastMessage[]>([]);

export function useToasts() {
    const showToast = (
        type: ToastMessage['type'],
        message: string,
        duration: number = 3000
    ) => {
        const id = Date.now() + Math.random();
        toasts.value.push({ id, type, message, duration });
        setTimeout(() => {
            toasts.value = toasts.value.filter(toast => toast.id !== id);
        }, duration);
    }

    return { toasts, showToast };
}