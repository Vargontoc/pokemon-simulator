import { ref } from "vue";

interface ErrorModalData {
    show: boolean;
    message: string;
    title: string;
    details?: string;
}

const state = ref<ErrorModalData>({
    show: false, title: '', message: '', details: ''});

export function useErrorModal() {
    const showErrorModal = (title: string, message: string, details?: string) => {
        state.value = { show: true, title, message, details: details || '' };
    };

    const close = () => {
        state.value.show = false;
    };

    return {
        errorState: state,
        showErrorModal,
        close
    };
}