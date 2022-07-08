<template>
    <div v-show="showed" class="modal">
        <div class="modal-content">
            <!-- <slot name="form"></slot>
            <slot name="form_default"></slot>
            <slot name="detail"></slot> -->
            <slot></slot>
            <slot name="footer">
                <div class="modal-button">
                    <button @click="onClose">close</button><br>
                </div>
            </slot>
            
            <!-- <button @click="onSave" v-if="modalFor !== 'detail'">save changes</button><br>
            <button @click="onDefault" v-if="modalFor === 'form_default'">default</button> -->
        </div>
    </div>
</template>

<script setup lang="ts">
import type { GlobalState } from '@/interfaces/State'
import { computed, inject } from 'vue'

// const globalState = inject("state") as GlobalState
// const isModalHidden = computed(() => globalState.isModalHidden)

const props = defineProps({
    showed: {
        type: Boolean,
    }
})

const emits = defineEmits({
    ModalClose: null,
})

function onClose() {
    emits("ModalClose")
}

// async function onSave() {
//     modalActionState.needSubmit()
//     onClose()
// }

// function onDefault() {
//     modalActionState.needReset()
// }
</script>

<style scoped>
@import '@/assets/modal.css';
</style>
