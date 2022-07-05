<template>
    <div :class="{ not_modal: !isModalHidden }">
        <h1>ตั้งค่าหน่วยนับ</h1>
        <button @click="onAdd">เพิ่ม</button>
    </div>
    <div v-show="!isModalHidden" :class="{ modal: !isModalHidden }">
        <UnitModal :mode="state.mode" :editTarget="state.editTarget" :unit="state.unit" @ModalClose="onClose" @ModalSave="getUnits">
        </UnitModal>
    </div>
    <table :class="{ not_modal: !isModalHidden }"> 
        <tr>
            <td>
                <table>
                    <thead>
                        <tr>
                            <th>No.</th>
                            <th>ชื่อ</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr :class="{ selected: state.currentUnitIndex == index }" @click="selectItem(index)" v-for="(unit, index) in state.units" :key="index">
                            <td>{{ index + 1 }}</td>
                            <td>{{ unit.name }}</td>
                        </tr>
                    </tbody>
                </table>
            </td>
            <td>
                <DataCard :dataLength="state.units.length" :selectedIndex="state.currentUnitIndex" 
                @onIndexChange="(newIndex) => selectItem(newIndex)" @onCardEditPressed="onEdit(state.units[state.currentUnitIndex])" 
                @onCardDeletePressed="onDelete(state.units[state.currentUnitIndex])">
                    <template v-slot:detail>
                        <UnitDetail :object="state.currentUnitIndex != undefined ? state.units[state.currentUnitIndex] : undefined">
                        </UnitDetail>
                    </template>
                </DataCard>
            </td>
        </tr>
    </table>
    
</template>

<script setup lang="ts">
import { reactive, inject, computed } from 'vue'
import {
    getUnitsList,
    deleteUnit,
} from '../api/api.js'
import type { Unit } from '../interfaces/Unit' 
import type { UnitState, GlobalState } from '../interfaces/State'

import UnitModal from '@/components/UnitModal.vue'
import DataCard from '@/components/DataCard.vue'
import UnitDetail from '@/components/UnitDetail.vue'

const globalState = inject("state") as GlobalState
const isModalHidden = computed(() => globalState.isModalHidden)

const state: UnitState = reactive({
    unit: {
        name: "",
    },
    mode: "เพิ่มหน่วยนับ",
    editTarget: undefined,
    currentUnitIndex: undefined,
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
    document.body.style.backgroundColor = "lightgray"
    globalState.isModalHidden = false

    state.unit.name = ""
    state.editTarget = undefined
}

function onEdit(unit: Unit) {
    state.mode = "แก้ไขหน่วยนับ"
    document.body.style.backgroundColor = "lightgray"
    globalState.isModalHidden = false

    state.unit.name = unit.name
    state.editTarget = unit.id
}

function onClose() {
    document.body.style.backgroundColor = "black"
    globalState.isModalHidden = true
}

async function onDelete(unit: Unit) {
    const response = await deleteUnit(unit.id)

    if (response.code != 200) {
        alert(response.message)
        return
    }

    await getUnits()
}

function selectItem(index: number) {
    state.currentUnitIndex = index
}
</script>

