<template>
    <div :class="{ not_modal: !isModalHidden }">
        <h1>ออกใบเสร็จรับเงิน</h1>
        <label>เลขที่เอกสาร</label><br>
        <input disabled v-model="state.receipt.code"><br>
        <label>วันที่</label><br>
        <input disabled v-model="state.receipt.date"><br>
    </div>
    <div v-show="!state.isModalHidden" :class="{ modal: !state.isModalHidden }">
        <!-- <PosModal :item="state.selectedItem" @ModalClose="onClose" @ModalSave="onModalSave">
        </PosModal> -->
        <Modal :modalFor="'form'" @ModalClose="onClose">
            <template v-slot:form>
                <PosForm :item="state.selectedItem" @submitted="onModalSave">
                </PosForm>
            </template>
        </Modal>
    </div>
    <div v-show="!state.isPreviewHidden" :class="{ modal: !state.isPreviewHidden}">
        <!-- <ReceiptDetailModal :receipt="state.receipt" @ModalClose="onClose">
        </ReceiptDetailModal> -->
        <Modal :modalFor="'detail'" @ModalClose="onClose">
            <template v-slot:detail>
                <ReceiptDetailTable :receipt="state.receipt">
                </ReceiptDetailTable>
            </template>
        </Modal>
    </div>
    <table :class="{ not_modal: !isModalHidden }">
        <thead>
            <tr>
                <th>No.</th>
                <th>รหัสสินค้า</th>
                <th>ชื่อสินค้า</th>
                <th>หน่วย</th>
                <th>จำนวน</th>
                <th>ราคา</th>
                <th>ส่วนลด (%)</th>
                <th>ส่วนลด (บาท)</th>
                <th>รวมเงิน</th>
                <th>ดำเนินการ</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(order, index) in state.receipt.orders" :key="index">
                <td>{{ index + 1 }}</td>
                <td>
                    <button @click="openEdit(index, order)">{{ order.item_code }}</button>
                </td>
                <td>{{ order.item_name }}</td>
                <td>{{ order.item_unit }}</td>
                <td>
                    <input @change="onQuantityChange(index, $event.target.value)" type="number" :value="order.item_quantity">
                </td>
                <td>{{ order.item_price }}</td>
                <td>
                    <input @change="onDiscountPercentChange(index, $event.target.value)" type="number" :value="order.item_discount_percent">
                </td>
                <td>{{ order.item_discount_amount }}</td>
                <td>{{ order.item_total_amount }}</td>
                <td>
                    <button @click="onDelete(index)">ลบ</button>
                </td>
            </tr>
            <tr>
                <td>{{ state.receipt.orders.length + 1 }}</td>
                <td>
                    <button @click="openAdd()">เลือกสินค้า</button>
                </td>
                <td></td>
                <td></td>
                <td>
                    <input disabled type="number" value="0">
                </td>
                <td>0</td>
                <td>
                    <input disabled type="number" value="0">
                </td>
                <td>
                    <input disabled type="number" value="0">
                </td>
                <td>0</td>
                <td>
                    <button disabled>ลบ</button>
                </td>
            </tr>
        </tbody>
    </table>
    <div :class="{ not_modal: !isModalHidden }">
        <label>ยอดรวมสินค้าก่อนส่วนลด</label>
        <input disabled type="number" v-model="state.receipt.total_amount"><br>
        <label>ยอดรวมส่วนลดสินค้า</label>
        <input disabled type="number" v-model="state.receipt.total_discount_amount"><br>
        <label>Subtotal</label>
        <input disabled type="number" v-model="state.receipt.subtotal"><br>
        <label>ส่วนลดการค้า</label>
        <input @change="onTradeDiscountChange($event.target.value)" type="number" :value="state.receipt.trade_discount"><br>
        <label>Grand total</label>
        <input disabled type="number" v-model="state.receipt.grand_total"><br>
        <button @click="onPreview">ดูตัวอย่าง</button>
        <button @click="onSubmit">บันทึก</button>
    </div>
</template>

<script setup lang="ts">
import { computed, inject, reactive, ref } from 'vue'
import {
    addReceipt,
} from '../api/api.js'
import type { OrderModel } from '@/interfaces/Order'
import type { GlobalState, PoSState } from '@/interfaces/State'

import PosModal from '@/components/PosModal.vue'
import ReceiptDetailModal from '../components/ReceiptDetailModal.vue'
import Modal from '@/components/Modal.vue'
import PosForm from '@/components/PosForm.vue'
import ReceiptDetailTable from '@/components/ReceiptDetailTable.vue'

const globalState = inject("state") as GlobalState
const isModalHidden = computed(() => globalState.isModalHidden)

const state: PoSState = reactive({
    receipt: {
        code: "TXXXX",
        date: "",
        total_amount: 0,
        total_discount_amount: 0,
        subtotal: 0,
        trade_discount: 0,
        grand_total: 0,
        orders: [],
    },
    isModalHidden: true,
    isPreviewHidden: true,
    editTarget: undefined,
    selectedItem: {
        item_id: 0,
        item_code: "",
        item_name: "",
        item_unit: "",
        item_unit_id: 0,
        item_quantity: 0,
        item_price: 0,
        item_discount_percent: 0,
        item_discount_amount: 0,
        item_total_amount: 0,
    },
})

// let getEditTarget: number
// getEditTarget = state.editTarget!

setDate()

function setDate() {
    var today = new Date()
    var dd = (today.getDate()).toString()
    var mm = (today.getMonth() + 1).toString()
    var yyyy = (today.getFullYear()).toString()

    if (+dd < 10) {
        dd = "0" + dd
    }

    if (+mm < 10) {
        mm = "0" + mm
    }

    state.receipt.date = dd + "/" + mm + "/" + yyyy
}

function onQuantityChange(index: string, quantity: string) {
    state.receipt.orders[+index].item_quantity = +quantity

    calcItemTotalAmount(index)
    calcItemDiscountAmount(index)
    calcTotalAmount()
    calcTotalDiscountAmount()
    calcSubtotal()
    calcGrandTotal()
}

function onDiscountPercentChange(index: string, discount_percent: string) {
    state.receipt.orders[+index].item_discount_percent = +discount_percent
    
    calcItemTotalAmount(index)
    calcItemDiscountAmount(index)
    calcTotalDiscountAmount()
    calcSubtotal()
    calcGrandTotal()
}

function onTradeDiscountChange(discount: string) {
    state.receipt.trade_discount = +discount

    calcGrandTotal()
}

function calcItemTotalAmount(index: string) {
    let discount_percent
    if (state.receipt.orders[+index].item_discount_percent > 100 ){
        discount_percent = 100
    }
    else {
        discount_percent = state.receipt.orders[+index].item_discount_percent
    }

    state.receipt.orders[+index].item_total_amount = state.receipt.orders[+index].item_price * state.receipt.orders[+index].item_quantity * (1 - (discount_percent / 100))
}

function calcItemDiscountAmount(index: string) {
    let discount_percent
    if (state.receipt.orders[+index].item_discount_percent > 100 ){
        discount_percent = 100
    }
    else {
        discount_percent = state.receipt.orders[+index].item_discount_percent
    }
    state.receipt.orders[+index].item_discount_amount = state.receipt.orders[+index].item_price * state.receipt.orders[+index].item_quantity * (discount_percent / 100)
}

function calcTotalAmount() {
    state.receipt.total_amount = 0
    for (const order of state.receipt.orders) {
        state.receipt.total_amount += order.item_price * order.item_quantity
    }
}

function calcTotalDiscountAmount() {
    let discount_percent
    state.receipt.total_discount_amount = 0
    for (const order of state.receipt.orders) {
        if (order.item_discount_percent > 100 ){
            discount_percent = 100
        }
        else {
            discount_percent = order.item_discount_percent
        }
        state.receipt.total_discount_amount += order.item_price * order.item_quantity * ( discount_percent / 100 )
    }
}

function calcSubtotal() {
    state.receipt.subtotal = state.receipt.total_amount - state.receipt.total_discount_amount
}

function calcGrandTotal() {
    let discount
    if (state.receipt.trade_discount > state.receipt.subtotal) {
        discount = state.receipt.subtotal
    }
    else {
        discount = state.receipt.trade_discount
    }

    state.receipt.grand_total = state.receipt.subtotal - discount
}

function reCalcReceiptSummary() {
    calcTotalAmount()
    calcTotalDiscountAmount()
    calcSubtotal()
    calcGrandTotal()
}

function openAdd() {
    state.isModalHidden = false
    document.body.style.backgroundColor = "lightgray"
    globalState.isModalHidden = false

    state.selectedItem.item_id = 0
    state.selectedItem.item_code = ""
    state.selectedItem.item_name = ""
    state.selectedItem.item_price = 0
    state.selectedItem.item_unit = ""
    state.selectedItem.item_unit_id = 0
    state.editTarget = undefined
}

function openEdit(index: string, order: OrderModel) {
    state.isModalHidden = false
    document.body.style.backgroundColor = "lightgray"
    globalState.isModalHidden = false

    state.selectedItem.item_id = order.item_id
    state.selectedItem.item_code = order.item_code
    state.selectedItem.item_name = order.item_name
    state.selectedItem.item_price = order.item_price
    state.selectedItem.item_unit = order.item_unit
    state.selectedItem.item_unit_id = order.item_unit_id
    state.editTarget = +index
}
function onModalSave(selectedItem: OrderModel) {
    if (state.editTarget != undefined) {
        state.receipt.orders[state.editTarget] = {...selectedItem}
    }
    else {
        state.receipt.orders.push({...selectedItem})
    }

    reCalcReceiptSummary()
}
function onDelete(index: string) {
    state.receipt.orders.splice(+index, 1)

    reCalcReceiptSummary()
}

function onPreview() {
    let containEmptyQuantity = state.receipt.orders.some(order => order.item_quantity == 0)

    if (!containEmptyQuantity) {
        state.isPreviewHidden = false
        document.body.style.backgroundColor = "lightgray"
        globalState.isModalHidden = false
    }
    else {
        alert("Item's quantity is empty")
    }
}

function onClose(closeFrom: string) {
    if (closeFrom == "form") {
        state.isModalHidden = true
    }
    else if (closeFrom == "detail") {
        state.isPreviewHidden = true
    }

    document.body.style.backgroundColor = "black"
    globalState.isModalHidden = true
}

async function onSubmit() {
    const response = await addReceipt(state.receipt)

    if (response.code != 200) {
        alert(response.message)
        return
    }

    state.receipt = {
        date: "",
        total_amount: 0,
        total_discount_amount: 0,
        subtotal: 0,
        trade_discount: 0,
        grand_total: 0,
        orders: [],
    }
    state.isModalHidden = true
    state.editTarget = undefined
    state.selectedItem = {
        item_id: 0,
        item_code: "",
        item_name: "",
        item_unit: "",
        item_unit_id: 0,
        item_quantity: 0,
        item_price: 0,
        item_discount_percent: 0,
        item_discount_amount: 0,
        item_total_amount: 0,
    }
}
</script>

