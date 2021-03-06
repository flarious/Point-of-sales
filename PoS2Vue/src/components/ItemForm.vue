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
</template>

<script setup lang="ts">
import type { GlobalState } from '@/interfaces/State'
import { ref, inject, computed, watch } from 'vue'
import {
    getUnitsList,
    addItem,
    editItem,
} from '@/api/api'
import { modalActionState } from '@/states/modal'
import type { ItemModel } from '@/interfaces/Item'
import type { PropType } from 'vue'
import type { Unit } from '@/interfaces/Unit'

const props = defineProps({
    mode: {
        type: String,
    },
    item: {
        type: Object as PropType<ItemModel>,
        default: () => ({
                code: "",
                name: "",
                price: 0,
                unit_id: 0,
        })
    },
    editTarget: {
        type: Number,
    },
})
const emit = defineEmits({
    submitted: null,
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

watch(
    () => modalActionState.resetModalCounter,
    (newValue, oldValue) => {
        if(newValue > oldValue) {
            modalItem.value = {...props.item}
        }
    }
)

watch(
    () => modalActionState.submitModalCounter,
    async (newValue, oldValue) => {
        if(newValue > oldValue) {
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

            emit("submitted")
        }
    }
)

let modalItem = ref({...props.item})
let units = ref<Unit[]>([])

getUnits()

async function getUnits() {
    const response = await getUnitsList()

    if (response.code != 200) {
        alert(response.message)
        return
    }

    units.value = response.data
}
</script>
