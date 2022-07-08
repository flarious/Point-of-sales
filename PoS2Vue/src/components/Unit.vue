<template>
    <h1>ตั้งค่าหน่วยนับ</h1>
    <Modal :showed="!isModalHidden">
        <UnitForm :mode="state.mode" :editTarget="state.editTarget" :unit="state.unit" @submitted="getUnits" />
        <template v-slot:footer>
            <div class="modal-button">
                <button @click="onClose">close</button>
                <button @click="onSave">save changes</button>
            </div>
        </template>
    </Modal>
    <table> 
        <tr>
            <td class="table">
                <div class="table-button"> 
                    <button @click="onAdd">เพิ่ม</button>
                </div>
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
                            <td class="important">{{ unit.name }}</td>
                        </tr>
                    </tbody>
                </table>
            </td>
            <td>
                <DataCard :dataLength="state.units.length" :selectedIndex="state.currentUnitIndex" 
                @update:selectedIndex="newIndex => selectItem(newIndex)" @onCardEditPressed="onEdit" 
                @onCardDeletePressed="onDelete">
                    <template v-slot:detail>
                        <UnitDetail :object="currentUnit">
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
} from '@/api/api'
import type { Unit } from '@/interfaces/Unit' 
import type { UnitState, GlobalState } from '@/interfaces/State'

import DataCard from '@/components/DataCard.vue'
import UnitDetail from '@/components/UnitDetail.vue'
import Modal from '@/components/Modal.vue'
import UnitForm from '@/components/UnitForm.vue'
import { modalActionState } from '@/states/modal'

const globalState = inject("state") as GlobalState
const isModalHidden = computed(() => globalState.isModalHidden)

const currentUnit = computed((): Unit | undefined => state.units[state.currentUnitIndex!])

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
    globalState.isModalHidden = false

    state.unit.name = ""
    state.editTarget = undefined
}

function editUnit(unit: Unit) {
    state.mode = "แก้ไขหน่วยนับ"
    globalState.isModalHidden = false

    state.unit.name = unit.name
    state.editTarget = unit.id
}

function onClose() {
    globalState.isModalHidden = true
}

function onSave() {
    modalActionState.needSubmit()
    onClose()
}

async function deleteUnitData(unit: Unit) {
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

function onEdit() {
    editUnit(state.units[state.currentUnitIndex!])
}

function onDelete() {
    deleteUnitData(state.units[state.currentUnitIndex!])
}



</script>

<style scoped>
@import '@/assets/selection.css';
@import '@/assets/table.css';
@import '@/assets/modal.css';
</style>