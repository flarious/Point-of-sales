<template>
    <div :class="{ not_modal: !isModalHidden }">
        <h1>ดูใบเสร็จรับเงิน</h1>
        <label for="startDate">วันที่เริ่มต้น :</label>
        <input type="date" name="startDate" v-model="state.startDate">
        <label for="endDate">วันที่สิ้นสุด :</label>
        <input type="date" name="endDate" v-model="state.endDate">
        <button @click="filterReceipts">ค้นหา</button>
    </div>
    <div v-show="!isModalHidden" :class="{ modal: !isModalHidden }">
        <ReceiptDetailModal :receiptId="state.receiptId" @ModalClose="onClose">
        </ReceiptDetailModal>
    </div>
    <table :class="{ not_modal: !isModalHidden }">
        <thead>
            <tr>
                <th>No.</th>
                <th>เลขที่เอกสาร</th>
                <th>วันที่</th>
                <th>Grand Total</th>
                <th>ดำเนินการ</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(receipt, index) in state.receipts" :key="index"> 
                <td>{{ index + 1 }}</td>
                <td>{{ receipt.code }}</td>
                <td>{{ receipt.date }}</td>
                <td>{{ receipt.grand_total }}</td>
                <td>
                    <button @click="viewReceipt(receipt)">ดูรายละเอียด</button>
                </td>
            </tr>
        </tbody>
    </table>
</template>

<script setup lang="ts">
import { computed, inject, reactive } from 'vue'
import { 
    getReceiptsList,
    getReceiptByDate, 
} from '../api/api.js'
import type { Receipt } from '../interfaces/Receipt'
import type { GlobalState, ReceiptState } from '../interfaces/State'

import ReceiptDetailModal from '../components/ReceiptDetailModal.vue'

const globalState = inject("state") as GlobalState
const isModalHidden = computed(() => globalState.isModalHidden)

const state: ReceiptState = reactive({
    receipts: [],
    receiptId: undefined,
    startDate: "",
    endDate: "",
})

getReceipts()

async function getReceipts() {
    const response = await getReceiptsList()

    if (response.code != 200) {
        alert(response.message)
        return
    }

    state.receipts = response.data
}

async function filterReceipts() {
    const response = await getReceiptByDate(yyyyMMddToddMMyyyy(state.startDate), yyyyMMddToddMMyyyy(state.endDate))

    if (response.code != 200) {
        alert(response.message)
        return
    }

    state.receipts = response.data
}

function yyyyMMddToddMMyyyy(yyyyMMdd: string) {
    if (yyyyMMdd != "") {
        let split = yyyyMMdd.split("-")
        let ddMMyyyy = `${split[2]}/${split[1]}/${split[0]}`
        return ddMMyyyy
    }
    else {
        return ""
    }
}

function viewReceipt(receipt: Receipt) {
    document.body.style.backgroundColor = "lightgray"
    globalState.isModalHidden = false

    state.receiptId = receipt.id
}

function onClose() {
    document.body.style.backgroundColor = "black"
    globalState.isModalHidden = true
}
</script>