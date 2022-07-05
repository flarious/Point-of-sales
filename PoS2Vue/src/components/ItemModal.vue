<template>
    <h1>{{ mode }}</h1>
    <label for="form_code">รหัสสินค้า</label>
    <input :disabled="mode == 'แก้ไขสินค้า'" name="form_code" v-model="modalItem.code"><br>
    <label for="form_name">ชื่อสินค้า</label>
    <input name="form_name" v-model="modalItem.name"><br>
    <label for="form_price">ราคา</label>
    <input type="number" name="form_price" v-model="modalItem.price"><br>
    <label for="form_unit">หน่วย</label>
    <select name="form_unit" v-model="modalItem.unit_id">
        <option :value="0">Please Select</option>
        <option v-for="unit in units" :value="unit.id" :key="unit.id">{{ unit.name }}</option>
    </select><br>
    <button @click="onClose">close</button><br>
    <button @click="onSave">save changes</button>
    <button @click="defaultModal">default</button>
</template>

<script setup lang="ts">
import type { GlobalState } from '@/interfaces/State.js'
import { watch, ref, computed, inject } from 'vue'
import {
    getUnitsList,
    addItem,
    editItem,
} from '../api/api.js'

const props = defineProps({
    mode: {
        type: String,
    },
    editTarget: {
        type: Number,
    },
    item: {
        type: Object,
        default: () => ({
                code: "",
                name: "",
                price: 0,
                unit_id: 0,
        })
    }
})

const globalState = inject("state") as GlobalState
const isModalHidden = computed(() => globalState.isModalHidden)

watch(
    () => isModalHidden.value,
    () => {
        if(!isModalHidden.value) {
            modalItem.value = {...props.item}
        }
    }
)

const emits = defineEmits({
    ModalClose: null,
    ModalSave: null,
})

let modalItem = ref({...props.item})
let units = ref([])

getUnits()

async function getUnits() {
    const response = await getUnitsList()

    if (response.code != 200) {
        alert(response.message)
        return
    }

    units.value = response.data
}

function onClose() {
    emits("ModalClose")
}

async function onSave() {
    var response
    if (props.editTarget != undefined) {
        response = await editItem(props.editTarget, modalItem.value)
    }
    else {
        response = await addItem(modalItem.value)
    }

    if (response.code != 200) {
        alert(response.message)
        return
    }

    emits("ModalSave")
    onClose()
}

function defaultModal() {
    modalItem.value = {...props.item}
}
</script>

