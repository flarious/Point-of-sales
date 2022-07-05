<template>
    <h1>{{ props.mode }}</h1>
    <label for="form_unit">ชื่อหน่วย</label>
    <input name="form_unit" v-model="state.modalUnit.name"><br>
    <button @click="onClose">close</button><br>
    <button @click="onSave">save changes</button>
</template>

<script setup lang="ts">
import type { GlobalState } from '@/interfaces/State.js'
import { computed, inject, reactive } from 'vue'
import {
    addUnit,
    editUnit,
} from '../api/api.js'

const props = defineProps({
    mode: {
        type: String,
    },
    editTarget: {
        type: Number,
    },
    unit: {
        type: Object,
        default: () => ({
                name: "",
        })
    },
})

const globalState = inject("state") as GlobalState
const isModalHidden = computed(() => globalState.isModalHidden)

const emit = defineEmits({
    ModalClose: null,
    ModalSave: null,
})

const state = reactive({
    modalUnit: props.unit
})

function onClose() {
    emit("ModalClose")
}

async function onSave() {
    var response
    if (props.editTarget != undefined) {
        response = await editUnit(props.editTarget, state.modalUnit)
    }
    else {
        response = await addUnit(state.modalUnit)
    }

    if (response.code != 200) {
        alert(response.message)
        return
    }

    emit("ModalSave")
    onClose()
}
</script>
