<template>
    <div>
        <h1>ดูใบเสร็จรับเงิน</h1>
        <label for="startDate">วันที่เริ่มต้น :</label>
        <input type="date" name="startDate" v-model="state.startDate">
        <label for="endDate">วันที่สิ้นสุด :</label>
        <input type="date" name="endDate" v-model="state.endDate">
        <button @click="filterReceipts">ค้นหา</button>
    </div>
    <Modal :showed="!isModalHidden" @ModalClose="onClose">
        <ReceiptDetailTable :receiptId="state.receiptId" />
    </Modal>
    <table class="table">
        <thead>
            <tr>
                <th>No.</th>
                <th>เลขที่เอกสาร</th>
                <th>วันที่</th>
                <th>Grand Total</th>
                <th class="center">ดำเนินการ</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(receipt, index) in state.receipts" :key="index"> 
                <td>{{ index + 1 }}</td>
                <td>{{ receipt.code }}</td>
                <td>{{ receipt.date }}</td>
                <td>{{ receipt.grand_total }}</td>
                <td class="center">
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

import Modal from '@/components/Modal.vue'
import ReceiptDetailTable from '@/components/ReceiptDetailTable.vue'

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
    globalState.isModalHidden = false

    state.receiptId = receipt.id
}

function onClose() {
    globalState.isModalHidden = true
}
</script>

<style scoped>
@import '@/assets/table.css';
</style>