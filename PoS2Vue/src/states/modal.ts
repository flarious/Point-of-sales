import { reactive } from 'vue'

export const modalActionState = reactive({
    shouldResetModal: false,
    shouldSubmitModal: false,
    needReset() {
        this.shouldResetModal = true
    },
    needSubmit() {
        this.shouldSubmitModal = true
    },
    resetted() {
        this.shouldResetModal = false
    },
    submitted() {
        this.shouldSubmitModal = false
    },
})