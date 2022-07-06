<template>
    <div :class="{ not_modal: !isModalHidden }">
        <h1>ตั้งค่าสินค้า</h1>
        <button @click="onAdd">เพิ่ม</button>
    </div>
    <div v-show="!isModalHidden" :class="{ modal: !isModalHidden }">
        <!-- <ItemModal :mode="state.mode" :editTarget="state.editTarget" :item="state.item" @ModalClose="onClose" @ModalSave="getItems">
        </ItemModal> -->
        <Modal :modalFor="'form_default'" @ModalClose="onClose">
            <template v-slot:form_default>
                <ItemForm :mode="state.mode" :item="state.item" :editTarget="state.editTarget" @submitted="getItems">
                </ItemForm>
            </template>
        </Modal>
    </div>
    <table :class="{ not_modal: !isModalHidden }">
        <tr>
            <td>
                <table>
                    <thead>
                        <tr>
                            <th>No.</th>
                            <th>รหัสสินค้า</th>
                            <th>ชื่อสินค้า</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr :class="{ selected: state.currentUnitIndex == index }" @click="selectItem(index)" v-for="(item, index) in state.items" :key="index">
                            <td>{{ index + 1 }}</td>
                            <td>{{ item.code }}</td>
                            <td>{{ item.name }}</td>
                        </tr>
                    </tbody>
                </table>
            </td>
            <td>
                <DataCard :dataLength="state.items.length" :selectedIndex="state.currentUnitIndex" 
                @update:selectedIndex="newIndex => selectItem(newIndex)" @onCardEditPressed="onEdit(state.items[state.currentUnitIndex])" 
                @onCardDeletePressed="onDelete(state.items[state.currentUnitIndex])">
                    <template v-slot:detail>
                        <ItemDetail :object="state.currentUnitIndex != undefined ? state.items[state.currentUnitIndex] : undefined">
                        </ItemDetail>
                    </template>
                </DataCard>
            </td>
        </tr>
    </table>
</template>

<script setup lang="ts">
import { computed, inject, reactive, ref } from 'vue'
import {
    getItemsList,
    deleteItem,
} from '../api/api.js'
import type { Item } from '../interfaces/Item' 
import type { GlobalState, ItemState } from '../interfaces/State'

import ItemModal from '@/components/ItemModal.vue'
import DataCard from '@/components/DataCard.vue'
import ItemDetail from '@/components/ItemDetail.vue'
import Modal from '@/components/Modal.vue'
import ItemForm from '@/components/ItemForm.vue'

const globalState = inject("state") as GlobalState
const isModalHidden = computed(() => globalState.isModalHidden)

const state: ItemState = reactive({
    item: {
        code: "",
        name: "",
        price: 0,
        unit_id: 0,
    },
    mode: "เพิ่มสินค้า",
    editTarget: undefined,
    currentUnitIndex: undefined,
    items: [],
})

const shouldResetModal = ref(false)
const shouldSubmitModal = ref(false)

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
    document.body.style.backgroundColor = "lightgray"
    globalState.isModalHidden = false

    state.item.code = ""
    state.item.name = ""
    state.item.price = 0
    state.item.unit_id = 0
    state.editTarget = undefined
}

function onEdit(item: Item) {
    state.mode = "แก้ไขสินค้า"
    document.body.style.backgroundColor = "lightgray"
    globalState.isModalHidden = false

    state.item.code = item.code
    state.item.name = item.name
    state.item.price = item.price
    state.item.unit_id = item.unit_id
    state.editTarget = item.id
}

function onClose() {
    document.body.style.backgroundColor = "black"
    globalState.isModalHidden = true
}

async function onDelete(item: Item) {
    const response = await deleteItem(item.id)

    if (response.code != 200) {
        alert(response.message)
        return
    }

    await getItems()
}

function selectItem(index: number) {
    state.currentUnitIndex = index
}
</script>