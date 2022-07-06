<template>
    <slot name="form"></slot>
    <slot name="form_default"></slot>
    <slot name="detail"></slot>
    <button @click="onClose">close</button><br>
    <button @click="onSave" v-if="modalFor !== 'detail'">save changes</button><br>
    <button @click="onDefault" v-if="modalFor === 'form_default'">default</button>
</template>

<script setup lang="ts">
import { modalActionState } from '@/states/modal'

const props = defineProps({
    modalFor: {
        type: String,
    }
})

const emits = defineEmits({
    ModalClose: null,
})

function onClose() {
    emits("ModalClose", props.modalFor!)
}

async function onSave() {
    modalActionState.needSubmit()
    onClose()
}

function onDefault() {
    modalActionState.needReset()
}
</script>
