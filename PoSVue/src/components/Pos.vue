<!--
<template>
    <nav>
        <router-link :to="{name: 'home'}" :class="{ not_modal: !isModalHidden }">กลับ</router-link>
    </nav>
    <h1 :class="{ not_modal: !isModalHidden }">ออกใบเสร็จรับเงิน</h1>
    <label :class="{ not_modal: !isModalHidden }">เลขที่เอกสาร</label>
    <input disabled value="TXXXX" :class="{ not_modal: !isModalHidden }"><br>
    <label :class="{ not_modal: !isModalHidden }">วันที่</label>
    <input disabled v-model="receipt.date" :class="{ not_modal: !isModalHidden }"><br>
    <div v-show="!isModalHidden" :class="{ modal: !isModalHidden }">
        <PosModal :showed="!isModalHidden" :item="selectedItem" @ModalClose="(state) => isModalHidden = state" @ModalSave="onModalSave">
        </PosModal>
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
            <tr v-for="(order, index) in receipt.orders" :key="index">
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
                <td>{{ receipt.orders.length + 1 }}</td>
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
        <input disabled type="number" v-model="receipt.total_amount"><br>
        <label>ยอดรวมส่วนลดสินค้า</label>
        <input disabled type="number" v-model="receipt.total_discount_amount"><br>
        <label>Subtotal</label>
        <input disabled type="number" v-model="receipt.subtotal"><br>
        <label>ส่วนลดการค้า</label>
        <input @change="onTradeDiscountChange($event.target.value)" type="number" :value="receipt.trade_discount"><br>
        <label>Grand total</label>
        <input disabled type="number" v-model="receipt.grand_total"><br>
        <button @click="onSubmit">บันทึก</button>
    </div>
</template>

<script>
import {
    addReceipt,
} from '../api/api.js'

import PosModal from '@/components/PosModal.vue'

export default {
    components: {
        PosModal,   
    },
    data () {
        return {
            receipt: {
                date: "",
                total_amount: 0,
                total_discount_amount: 0,
                subtotal: 0,
                trade_discount: 0,
                grand_total: 0,
                orders: [],
            },
            isModalHidden: true,
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
        }
    },
    methods: {
        setDate() {
            var today = new Date()
            var dd = today.getDate()
            var mm = today.getMonth() + 1
            var yyyy = today.getFullYear()

            if (dd < 10) {
                dd = "0" + dd
            }

            if (mm < 10) {
                mm = "0" + mm
            }

            this.receipt.date = dd + "/" + mm + "/" +yyyy
        },
        onQuantityChange(index, quantity) {
            this.receipt.orders[index].item_quantity = quantity

            this.calcItemTotalAmount(index)
            this.calcItemDiscountAmount(index)
            this.calcTotalAmount()
            this.calcTotalDiscountAmount()
            this.calcSubtotal()
            this.calcGrandTotal()
        },
        onDiscountPercentChange(index, discount_percent) {
            this.receipt.orders[index].item_discount_percent = discount_percent
            
            this.calcItemTotalAmount(index)
            this.calcItemDiscountAmount(index)
            this.calcTotalDiscountAmount()
            this.calcSubtotal()
            this.calcGrandTotal()
        },
        onTradeDiscountChange(discount) {
            this.receipt.trade_discount = discount

            this.calcGrandTotal()
        },
        calcItemTotalAmount(index) {
            let discount_percent
            if (this.receipt.orders[index].item_discount_percent > 100 ){
                discount_percent = 100
            }
            else {
                discount_percent = this.receipt.orders[index].item_discount_percent
            }

            this.receipt.orders[index].item_total_amount = this.receipt.orders[index].item_price * this.receipt.orders[index].item_quantity * (1 - (discount_percent / 100))
        },
        calcItemDiscountAmount(index) {
            let discount_percent
            if (this.receipt.orders[index].item_discount_percent > 100 ){
                discount_percent = 100
            }
            else {
                discount_percent = this.receipt.orders[index].item_discount_percent
            }
            this.receipt.orders[index].item_discount_amount = this.receipt.orders[index].item_price * this.receipt.orders[index].item_quantity * (discount_percent / 100)
        },
        calcTotalAmount() {
            this.receipt.total_amount = 0
            for (const order of this.receipt.orders) {
                this.receipt.total_amount += order.item_price * order.item_quantity
            }
        },
        calcTotalDiscountAmount() {
            let discount_percent
            this.receipt.total_discount_amount = 0
            for (const order of this.receipt.orders) {
                if (order.item_discount_percent > 100 ){
                    discount_percent = 100
                }
                else {
                    discount_percent = order.item_discount_percent
                }
                this.receipt.total_discount_amount += order.item_price * order.item_quantity * ( discount_percent / 100 )
            }
        },
        calcSubtotal() {
            this.receipt.subtotal = this.receipt.total_amount - this.receipt.total_discount_amount
        },
        calcGrandTotal() {
            let discount
            if (this.receipt.trade_discount > this.receipt.subtotal) {
                discount = this.receipt.subtotal
            }
            else {
                discount = this.receipt.trade_discount
            }

            this.receipt.grand_total = this.receipt.subtotal - discount
        },
        reCalcReceiptSummary() {
            this.calcTotalAmount()
            this.calcTotalDiscountAmount()
            this.calcSubtotal()
            this.calcGrandTotal()
        },
        openAdd() {
            this.isModalHidden = false
            document.body.style.backgroundColor = "lightgray"

            this.selectedItem.item_id = 0
            this.selectedItem.item_code = ""
            this.selectedItem.item_name = ""
            this.selectedItem.item_price = 0
            this.selectedItem.item_unit = ""
            this.selectedItem.item_unit_id = 0
            this.editTarget = undefined
        },
        openEdit(index, order) {
            this.isModalHidden = false
            document.body.style.backgroundColor = "lightgray"

            this.selectedItem.item_id = order.item_id
            this.selectedItem.item_code = order.item_code
            this.selectedItem.item_name = order.item_name
            this.selectedItem.item_price = order.item_price
            this.selectedItem.item_unit = order.item_unit
            this.selectedItem.item_unit_id = order.item_unit_id
            this.editTarget = index
        },
        onModalSave(selectedItem) {
            if (this.editTarget != undefined) {
                this.receipt.orders[this.editTarget] = {...selectedItem}
            }
            else {
                this.receipt.orders.push({...selectedItem})
            }

            this.reCalcReceiptSummary()
        },
        onDelete(index) {
            this.receipt.orders.splice(index, 1)

            this.reCalcReceiptSummary()
        },
        async onSubmit() {
            const response = await addReceipt(this.receipt)

            if (response.code != 200) {
                alert(response.message)
                return
            }

            this.receipt = {
                date: "",
                total_amount: 0,
                total_discount_amount: 0,
                subtotal: 0,
                trade_discount: 0,
                grand_total: 0,
                orders: [],
            }
            this.isModalHidden = true
            this.editTarget = undefined
            this.selectedItem = {
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
    },
    async created () {
        this.setDate()
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
    <h1 :class="{ not_modal: !state.isModalHidden }">ออกใบเสร็จรับเงิน</h1>
    <label :class="{ not_modal: !state.isModalHidden }">เลขที่เอกสาร</label>
    <input disabled value="TXXXX" :class="{ not_modal: !state.isModalHidden }"><br>
    <label :class="{ not_modal: !state.isModalHidden }">วันที่</label>
    <input disabled v-model="state.receipt.date" :class="{ not_modal: !state.isModalHidden }"><br>
    <div v-show="!state.isModalHidden" :class="{ modal: !state.isModalHidden }">
        <PosModal :showed="!state.isModalHidden" :item="state.selectedItem" @ModalClose="(isClose) => state.isModalHidden = isClose" @ModalSave="onModalSave">
        </PosModal>
    </div>
    <table :class="{ not_modal: !state.isModalHidden }">
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
    <div :class="{ not_modal: !state.isModalHidden }">
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
        <button @click="onSubmit">บันทึก</button>
    </div>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue'
import {
    addReceipt,
} from '../api/api.js'
import type { OrderModel } from '@/interfaces/Order'
import type { ReceiptModel } from '@/interfaces/Receipt'

import PosModal from '@/components/PosModal.vue'

interface PoSComponentState {
    receipt: ReceiptModel,
    isModalHidden: boolean,
    editTarget?: number,
    selectedItem: OrderModel,
}

const state: PoSComponentState = reactive({
    receipt: {
        date: "",
        total_amount: 0,
        total_discount_amount: 0,
        subtotal: 0,
        trade_discount: 0,
        grand_total: 0,
        orders: [],
    },
    isModalHidden: true,
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

<style scoped>
@import '../assets/modal.css';

</style>

