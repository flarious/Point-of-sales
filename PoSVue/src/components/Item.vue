<!--
<template>
    <nav>
        <router-link :to="{name: 'home'}" :class="{ not_modal: !isModalHidden }">กลับ</router-link>
    </nav>

    <h1 :class="{ not_modal: !isModalHidden }">ตั้งค่าสินค้า</h1>
    <button @click="onAdd" :class="{ not_modal: !isModalHidden }">เพิ่ม</button>
    <div v-show="!isModalHidden" :class="{ modal: !isModalHidden }">
        <ItemModal :mode="mode" :editTarget="editTarget" :showed="!isModalHidden" :item="item" @ModalClose="(state) => isModalHidden = state" @ModalSave="getItems">
        </ItemModal>
    </div>
    <table :class="{ not_modal: !isModalHidden }">
        <thead>
            <tr>
                <th>No.</th>
                <th>รหัสสินค้า</th>
                <th>ชื่อสินค้า</th>
                <th>หน่วย</th>
                <th>ราคา</th>
                <th>ดำเนินการ</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(item, index) in items" :key="index">
                <td>{{ index + 1 }}</td>
                <td>{{ item.code }}</td>
                <td>{{ item.name }}</td>
                <td>{{ item.unit }}</td>
                <td>{{ item.price }}</td>
                <td>
                    <button @click="onEdit(item)">แก้ไข</button>
                    <button @click="onDelete(item)">ลบ</button>
                </td>
            </tr>
        </tbody>
    </table>
</template>

<script>
import {
    getItemsList,
    deleteItem,
} from '../api/api.js'

import ItemModal from '@/components/ItemModal.vue'

export default {
    components: {
        ItemModal,
    },
    data () {
        return {
            editTarget: undefined,
            isModalHidden: true,
            item: {
                    code: "",
                    name: "",
                    price: 0,
                    unit_id: 0,
            },
            mode: "เพิ่มสินค้า",
            items: [],
        }
    },
    methods: {
        async getItems() {
            const response = await getItemsList()

            if (response.code != 200) {
                alert(response.message)
                return
            }

            this.items = response.data
        },
        onAdd() {
            this.mode = "เพิ่มสินค้า"
            this.isModalHidden = false
            document.body.style.backgroundColor = "lightgray"

            this.item.code = ""
            this.item.name = ""
            this.item.price = ""
            this.item.unit_id = 0
            this.editTarget = undefined
        },
        onEdit(item) {
            this.mode = "แก้ไขสินค้า"
            this.isModalHidden = false
            document.body.style.backgroundColor = "lightgray"

            this.item.code = item.code
            this.item.name = item.name
            this.item.price = item.price
            this.item.unit_id = item.unit_id
            this.editTarget = item.id
        },
        async onDelete(item) {
            const response = await deleteItem(item.id)

            if (response.code != 200) {
                alert(response.message)
                return
            }

            await this.getItems()
        },
    },
    async created () {
        await this.getItems()
    },
}
</script>

<style scoped>
@import '../assets/modal.css';

</style>
-->

<template>
    <nav>
        <router-link :to="{name: 'home'}" :class="{ not_modal: !state.isModalHidden }">กลับ</router-link>
    </nav>

    <h1 :class="{ not_modal: !state.isModalHidden }">ตั้งค่าสินค้า</h1>
    <button @click="onAdd" :class="{ not_modal: !state.isModalHidden }">เพิ่ม</button>
    <div v-show="!state.isModalHidden" :class="{ modal: !state.isModalHidden }">
        <ItemModal :mode="state.mode" :editTarget="state.editTarget" :showed="!state.isModalHidden" :item="state.item" @ModalClose="(isClose) => state.isModalHidden = isClose" @ModalSave="getItems">
        </ItemModal>
    </div>
    <table :class="{ not_modal: !state.isModalHidden }">
        <thead>
            <tr>
                <th>No.</th>
                <th>รหัสสินค้า</th>
                <th>ชื่อสินค้า</th>
                <th>หน่วย</th>
                <th>ราคา</th>
                <th>ดำเนินการ</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(item, index) in state.items" :key="index">
                <td>{{ index + 1 }}</td>
                <td>{{ item.code }}</td>
                <td>{{ item.name }}</td>
                <td>{{ item.unit }}</td>
                <td>{{ item.price }}</td>
                <td>
                    <button @click="onEdit(item)">แก้ไข</button>
                    <button @click="onDelete(item)">ลบ</button>
                </td>
            </tr>
        </tbody>
    </table>
</template>

<script setup>
import { reactive } from 'vue'
import {
    getItemsList,
    deleteItem,
} from '../api/api.js'

import ItemModal from '@/components/ItemModal.vue'

const state = reactive({
    item: {
        code: "",
        name: "",
        price: 0,
        unit_id: 0,
    },
    mode: "เพิ่มสินค้า",
    editTarget: undefined,
    isModalHidden: true,
    items: [],
})

getItems()

async function getItems() {
    const response = await getItemsList()

    if (response.code != 200) {
        alert(response.message)
        return
    }

    state.items = response.data
}

function onAdd() {
    state.mode = "เพิ่มสินค้า"
    state.isModalHidden = false
    document.body.style.backgroundColor = "lightgray"

    state.item.code = ""
    state.item.name = ""
    state.item.price = ""
    state.item.unit_id = 0
    state.editTarget = undefined
}

function onEdit(item) {
    state.mode = "แก้ไขสินค้า"
    state.isModalHidden = false
    document.body.style.backgroundColor = "lightgray"

    state.item.code = item.code
    state.item.name = item.name
    state.item.price = item.price
    state.item.unit_id = item.unit_id
    state.editTarget = item.id
}

async function onDelete(item) {
    const response = await deleteItem(item.id)

    if (response.code != 200) {
        alert(response.message)
        return
    }

    await this.getItems()
}
</script>

<style scoped>
@import '../assets/modal.css';

</style>