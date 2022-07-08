import { reactive } from 'vue'

export const modalActionState = reactive({
    resetModalCounter: 0,
    submitModalCounter: 0,
    needReset() {
        this.resetModalCounter++
    },
    needSubmit() {
        this.submitModalCounter++
    },
})