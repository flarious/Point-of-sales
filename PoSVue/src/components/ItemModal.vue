<!--
<template>
    <h1>{{ mode }}</h1>
    <label for="form_code">รหัสสินค้า</label>
    <input name="form_code" v-model="modalItem.code"><br>
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

<script>
import {
    getUnitsList,
    addItem,
    editItem,
} from '../api/api.js'

export default {
    props: {
        mode: {
            type: String,
        },
        editTarget: {
            type: Number,
        },
        showed: {
            type: Boolean,
        },
        item: {
            type: Object,
            default(raw) {
                return {
                    code: "",
                    name: "",
                    price: 0,
                    unit_id: 0,
                }
            }
        },
    },
    data () {
        return {
            modalItem: {...this.item},
            units: [],
        }
    },
    watch: {
        showed: {
            handler() {
                if(showed) {
                    this.modalItem = {...this.item}
                }
            },
            deep: true,
        }
    },
    emits: {
        ModalClose: null,
        ModalSave: null,
    },
    methods: {
        async getUnits() {
            const response = await getUnitsList()

            if (response.code != 200) {
                alert(response.message)
                return
            }

            this.units = response.data
        },
        onClose() {
            this.$emit("ModalClose", true)
            document.body.style.backgroundColor = "black"
        },
        async onSave() {
            var response
            if (this.editTarget != undefined) {
                response = await editItem(this.editTarget, this.modalItem)
            }
            else {
                response = await addItem(this.modalItem)
            }

            if (response.code != 200) {
                alert(response.message)
                return
            }

            this.$emit("ModalSave")
            this.onClose()
        },
        defaultModal() {
            this.modalItem = {...this.item}
        }
    },
    async created () {
        await this.getUnits()
    }
}
</script>
-->

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
import { watch, ref } from 'vue'
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
    showed: {
        type: Boolean,
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

watch(
    () => props.showed,
    () => {
        if(props.showed) {
            modalItem.value = {...props.item}
        }
    }
)

const emit = defineEmits({
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
    emit("ModalClose", true)
    document.body.style.backgroundColor = "black"
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

    emit("ModalSave")
    onClose()
}

function defaultModal() {
    modalItem.value = {...props.item}
}
</script>

