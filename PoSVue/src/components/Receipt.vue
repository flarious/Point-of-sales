<!--
<template>
    <nav>
        <router-link :to="{name: 'home'}">กลับ</router-link>
    </nav>

    <h1>ดูใบเสร็จรับเงิน</h1>
    <label for="start_date">วันที่เริ่มต้น :</label>
    <input type="date" name="start_date" v-model="start_date">
    <label for="end_date">วันที่สิ้นสุด :</label>
    <input type="date" name="end_date" v-model="end_date">
    <button @click="filterReceipts">ค้นหา</button>
    <table>
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
            <tr v-for="(receipt, index) in receipts" :key="index"> 
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

<script>
import {
    getReceiptsList,
    getReceiptByDate,
} from '../api/api.js'

export default {
    name: 'receipt',
    data () {
        return {
            receipts: [],
            start_date: "",
            end_date: "",
        }
    },
    methods: {
        async getReceipts() {
            const response = await getReceiptsList()

            if (response.code != 200) {
                alert(response.message)
                return
            }

            this.receipts = response.data
        },
        async filterReceipts() {
            const response = await getReceiptByDate(this.yyyyMMddToddMMyyyy(this.start_date), this.yyyyMMddToddMMyyyy(this.end_date))

            if (response.code != 200) {
                alert(response.message)
                return
            }

            this.receipts = response.data
        },
        yyyyMMddToddMMyyyy(yyyyMMdd) {
            if (yyyyMMdd != "") {
                let split = yyyyMMdd.split("-")
                let ddMMyyyy = `${split[2]}/${split[1]}/${split[0]}`
                return ddMMyyyy
            }
            else {
                return yyyyMMdd
            }
            
        },
        viewReceipt(receipt) {
            window.location.href = `/receipt/${receipt.id}`
        },

    },
    async created () {
        await this.getReceipts()
    },
}
</script>
-->

<template>
    <nav>
        <router-link :to="{name: 'home'}">กลับ</router-link>
    </nav>

    <h1>ดูใบเสร็จรับเงิน</h1>
    <label for="start_date">วันที่เริ่มต้น :</label>
    <input type="date" name="start_date" v-model="state.start_date">
    <label for="end_date">วันที่สิ้นสุด :</label>
    <input type="date" name="end_date" v-model="state.end_date">
    <button @click="filterReceipts">ค้นหา</button>
    <table>
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
import { reactive } from 'vue'
import { 
    getReceiptsList,
    getReceiptByDate, 
} from '../api/api.js'
import type { Receipt } from '../interfaces/Receipt'

const state = reactive({
    receipts: [],
    start_date: "",
    end_date: "",
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
    const response = await getReceiptByDate(yyyyMMddToddMMyyyy(state.start_date), yyyyMMddToddMMyyyy(state.end_date))

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
    window.location.href = `/receipt/${receipt.id}`
}
</script>