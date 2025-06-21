import { ref } from "vue";
interface ConfirmDialogState {
    show: boolean;
    title: string;
    message: string;
    resolve?: (value: boolean) => void;
}

const state = ref<ConfirmDialogState>({ show: false, title: '', message: '' });
export function useConfirmDialog() {
    const showDialog = (title: string, message: string): Promise<boolean> => {
        return new Promise((resolve) => {
            state.value = { show: true, title, message, resolve };
        });
    };

    const confirm = () => {
        if (state.value.resolve) {
            state.value.resolve(true);
        }
        state.value.show = false;
    };

    const cancel = () => {
        if (state.value.resolve) {
            state.value.resolve(false);
        }
        state.value.show = false;
    };

    return {
        confirmDialogState: state,
        showDialog,
        confirm,
        cancel
    };
}
