<!--
<template>
    <nav>
        <router-link :to="{name: 'home'}" :class="{ not_modal: !isModalHidden }">กลับ</router-link>
    </nav>
    <h1 :class="{ not_modal: !isModalHidden }">ตั้งค่าหน่วยนับ</h1>
    <button @click="onAdd" :class="{ not_modal: !isModalHidden }">เพิ่ม</button>
    <div v-show="!isModalHidden" :class="{ modal: !isModalHidden }">
        <UnitModal :mode="mode" :editTarget="editTarget" :showed="!isModalHidden" :unit="unit" @ModalClose="(state) => isModalHidden = state" @ModalSave="getUnits">
        </UnitModal>
    </div>
    <table :class="{ not_modal: !isModalHidden }">
        <thead>
            <tr>
                <th>No.</th>
                <th>ชื่อ</th>
                <th>ดำเนินการ</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(unit, index) in units" :key="index">
                <td>{{ index + 1 }}</td>
                <td>{{ unit.name }}</td>
                <td>
                    <button @click="onEdit(unit)">แก้ไข</button>
                    <button @click="onDelete(unit)">ลบ</button>
                </td> 
            </tr>
        </tbody>
    </table>
</template>

<script>
import {
    getUnitsList,
    deleteUnit,
} from '../api/api.js'

import UnitModal from '@/components/UnitModal.vue'

export default {
    components: {
        UnitModal,
    },
    data () {
        return {
            unit: {
                name: "",
            },
            mode: "เพิ่มหน่วยนับ",
            editTarget: undefined,
            isModalHidden: true,
            units: [],
        }
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
        onAdd() {
            this.mode = "เพิ่มหน่วยนับ"
            this.isModalHidden = false
            document.body.style.backgroundColor = "lightgray"

            this.unit.name = ""
            this.editTarget = undefined
        },
        onEdit(unit) {
            this.mode = "แก้ไขหน่วยนับ"
            this.isModalHidden = false
            document.body.style.backgroundColor = "lightgray"

            this.unit.name = unit.name
            this.editTarget = unit.id
        },
        async onDelete(unit) {
            const response = await deleteUnit(unit.id)

            if (response.code != 200) {
                alert(response.message)
                return
            }

            await this.getUnits()
        },
    },
    async created () {
        await this.getUnits()
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
    <h1 :class="{ not_modal: !state.isModalHidden }">ตั้งค่าหน่วยนับ</h1>
    <button @click="onAdd" :class="{ not_modal: !state.isModalHidden }">เพิ่ม</button>
    <div v-show="!state.isModalHidden" :class="{ modal: !state.isModalHidden }">
        <UnitModal :mode="state.mode" :editTarget="state.editTarget" :showed="!state.isModalHidden" :unit="state.unit" @ModalClose="(isClose) => state.isModalHidden = isClose" @ModalSave="getUnits">
        </UnitModal>
    </div>
    <table :class="{ not_modal: !state.isModalHidden }">
        <thead>
            <tr>
                <th>No.</th>
                <th>ชื่อ</th>
                <th>ดำเนินการ</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(unit, index) in state.units" :key="index">
                <td>{{ index + 1 }}</td>
                <td>{{ unit.name }}</td>
                <td>
                    <button @click="onEdit(unit)">แก้ไข</button>
                    <button @click="onDelete(unit)">ลบ</button>
                </td> 
            </tr>
        </tbody>
    </table>
</template>

<script setup lang="ts">
import { reactive } from 'vue'
import {
    getUnitsList,
    deleteUnit,
} from '../api/api.js'
import type { Unit, UnitModel } from '../interfaces/Unit' 

import UnitModal from '@/components/UnitModal.vue'

interface UnitComponentState {
    unit: UnitModel,
    mode: string,
    editTarget?: number,
    isModalHidden: boolean,
    units: Unit[],
}

const state: UnitComponentState = reactive({
    unit: {
        name: "",
    },
    mode: "เพิ่มหน่วยนับ",
    editTarget: undefined,
    isModalHidden: true,
    units: [],
})

getUnits()

async function getUnits() {
    const response = await getUnitsList()

    if (response.code != 200) {
        alert(response.message)
        return
    }

    state.units = response.data
}

function onAdd() {
    state.mode = "เพิ่มหน่วยนับ"
    state.isModalHidden = false
    document.body.style.backgroundColor = "lightgray"

    state.unit.name = ""
    state.editTarget = undefined
}

function onEdit(unit: Unit) {
    state.mode = "แก้ไขหน่วยนับ"
    state.isModalHidden = false
    document.body.style.backgroundColor = "lightgray"

    state.unit.name = unit.name
    state.editTarget = unit.id
}

async function onDelete(unit: Unit) {
    const response = await deleteUnit(unit.id)

    if (response.code != 200) {
        alert(response.message)
        return
    }

    await getUnits()
}
</script>

<style scoped>
@import '../assets/modal.css';

</style>