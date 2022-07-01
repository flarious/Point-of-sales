<!--
<template>
    <h1>{{ mode }}</h1>
    <label for="form_unit">ชื่อหน่วย</label>
    <input name="form_unit" v-model="modalUnit.name"><br>
    <button @click="onClose">close</button><br>
    <button @click="onSave">save changes</button>
</template>

<script>
import {
    addUnit,
    editUnit,
} from '../api/api.js'

export default {
    props: {
        mode: {
            type: String,
        },
        editTarget: {
            type: Number,
        },
        unit: {
            type: Object,
            default(raw) {
                return {
                    name: "",
                }
            }
        },
    }, 
    data() {
        return {
            modalUnit: this.unit,
        }
    },
    emits: {
        ModalClose: null,
        ModalSave: null,
    },
    methods: {
        onClose() {
            this.$emit("ModalClose", true)
            document.body.style.backgroundColor = "black"
        },
        async onSave() {
            var response
            if (this.editTarget != undefined) {
                response = await editUnit(this.editTarget, this.modalUnit)
            }
            else {
                response = await addUnit(this.modalUnit)
            }

            if (response.code != 200) {
                alert(response.message)
                return
            }

            this.$emit("ModalSave")
            this.onClose()
        },
    },
}
</script>
-->

<template>
    <h1>{{ props.mode }}</h1>
    <label for="form_unit">ชื่อหน่วย</label>
    <input name="form_unit" v-model="state.modalUnit.name"><br>
    <button @click="onClose">close</button><br>
    <button @click="onSave">save changes</button>
</template>

<script setup lang="ts">
import { reactive } from 'vue'
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
    showed: {
        type: Boolean,
    },
    unit: {
        type: Object,
        default: () => ({
                name: "",
        })
    },
})

const emit = defineEmits({
    ModalClose: null,
    ModalSave: null,
})

const state = reactive({
    modalUnit: props.unit
})

function onClose() {
    emit("ModalClose", true)
    document.body.style.backgroundColor = "black"
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
