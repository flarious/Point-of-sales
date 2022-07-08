<template>
    <div>
        <h1>ตั้งค่าสินค้า</h1>
    </div>
    <Modal :showed="!isModalHidden">
        <ItemForm :mode="state.mode" :item="state.item" :editTarget="state.editTarget" @submitted="getItems" />
        <template v-slot:footer>
            <div class="modal-button">
                <button @click="onClose">close</button>
                <button @click="onSave">save changes</button>
                <button @click="onDefault">default</button>
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
                            <th>รหัสสินค้า</th>
                            <th>ชื่อสินค้า</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr :class="{ selected: state.currentUnitIndex == index }" @click="selectItem(index)" v-for="(item, index) in state.items" :key="index">
                            <td>{{ index + 1 }}</td>
                            <td>{{ item.code }}</td>
                            <td class="important">{{ item.name }}</td>
                        </tr>
                    </tbody>
                </table>
            </td>
            <td>
                <DataCard :dataLength="state.items.length" :selectedIndex="state.currentUnitIndex" 
                @update:selectedIndex="newIndex => selectItem(newIndex)" @onCardEditPressed="onEdit" 
                @onCardDeletePressed="onDelete">
                    <template v-slot:detail>
                        <ItemDetail :object="currentItem">
                        </ItemDetail>
                    </template>
                </DataCard>
            </td>
        </tr>
    </table>
</template>

<script setup lang="ts">
import { computed, inject, reactive } from 'vue'
import {
    getItemsList,
    deleteItem,
} from '@/api/api'
import type { Item } from '@/interfaces/Item' 
import type { GlobalState, ItemState } from '@/interfaces/State'

import DataCard from '@/components/DataCard.vue'
import ItemDetail from '@/components/ItemDetail.vue'
import Modal from '@/components/Modal.vue'
import ItemForm from '@/components/ItemForm.vue'
import { modalActionState } from '@/states/modal.js'

const globalState = inject("state") as GlobalState
const isModalHidden = computed(() => globalState.isModalHidden)

const currentItem = computed((): Item | undefined => state.items[state.currentUnitIndex!])

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
    globalState.isModalHidden = false

    state.item.code = ""
    state.item.name = ""
    state.item.price = 0
    state.item.unit_id = 0
    state.editTarget = undefined
}

function editItem(item: Item) {
    state.mode = "แก้ไขสินค้า"
    globalState.isModalHidden = false

    state.item.code = item.code
    state.item.name = item.name
    state.item.price = item.price
    state.item.unit_id = item.unit_id
    state.editTarget = item.id
}

function onClose() {
    globalState.isModalHidden = true
}

function onSave() {
    modalActionState.needSubmit()
    onClose()
}

function onDefault() {
    modalActionState.needReset()
}

async function deleteItemData(item: Item) {
    const response = await deleteItem(item.id)

    if (response.code != 200) {
        alert(response.message)
        return
    }

    await getItems()
}

function onEdit() {
    editItem(state.items[state.currentUnitIndex!])
}

function onDelete() {
    deleteItemData(state.items[state.currentUnitIndex!])
}

function selectItem(index: number) {
    state.currentUnitIndex = index
}
</script>

<style scoped>
@import '@/assets/selection.css';
@import '@/assets/table.css';
@import '@/assets/modal.css';
</style>