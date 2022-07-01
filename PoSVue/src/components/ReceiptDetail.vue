<!--
<template>
    <nav>
        <router-link :to="{name: 'receipt'}">กลับ</router-link>
    </nav>

    <label>เลขที่เอกสาร</label><br>
    <input v-model="receipt.code" disabled><br>
    <label>วันที่</label><br>
    <input v-model="receipt.date" disabled><br>
    <table>
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
            </tr>
        </thead>
        <tbody>
            <tr v-for="(order, index) in receipt.orders" :key="index">
                <td>{{ index + 1 }}</td>
                <td>{{ order.item_code }}</td>
                <td>{{ order.item_name }}</td>
                <td>{{ order.item_unit }}</td>
                <td>{{ order.item_quantity }}</td>
                <td>{{ order.item_price }}</td>
                <td>{{ order.item_discount_percent }}</td>
                <td>{{ order.item_discount_amount }}</td>
                <td>{{ order.item_total_amount }}</td>
            </tr>
        </tbody>
    </table>

    <label>ยอดรวมสินค้าก่อนส่วนลด</label>
    <input v-model="receipt.total_amount" disabled type="number"><br>
    <label>ยอดรวมส่วนลดสินค้า</label>
    <input v-model="receipt.total_discount_amount" disabled type="number"><br>
    <label>Subtotal</label>
    <input v-model="receipt.subtotal" disabled type="number"><br>
    <label>ส่วนลดการค้า</label>
    <input v-model="receipt.trade_discount" disabled type="number"><br>
    <label>Grand total</label>
    <input v-model="receipt.grand_total" disabled type="number"><br>
</template>

<script>
import {
    getOneReceipt,
} from '../api/api.js'

export default {
    data () {
        return {
            receipt: {
                id: 0,
                code: "",
                date: "",
                total_amount: 0,
                total_discount_amount: 0,
                subtotal: 0,
                trade_discount: 0,
                grand_total: 0,
                orders: [],
            }
        }
    },
    methods: {
        async getReceipt(id) {
            const response = await getOneReceipt(id)

            if (response.code != 200) {
                alert(response.message)
                return
            }

            this.receipt = response.data
        }
    },
    async created () {
        await this.getReceipt(this.$route.params.id)
    }
}
</script>
-->

<template>
    <nav>
        <router-link :to="{name: 'receipt'}">กลับ</router-link>
    </nav>

    <label>เลขที่เอกสาร</label><br>
    <input v-model="state.receipt.code" disabled><br>
    <label>วันที่</label><br>
    <input v-model="state.receipt.date" disabled><br>
    <table>
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
            </tr>
        </thead>
        <tbody>
            <tr v-for="(order, index) in state.receipt.orders" :key="index">
                <td>{{ index + 1 }}</td>
                <td>{{ order.item_code }}</td>
                <td>{{ order.item_name }}</td>
                <td>{{ order.item_unit }}</td>
                <td>{{ order.item_quantity }}</td>
                <td>{{ order.item_price }}</td>
                <td>{{ order.item_discount_percent }}</td>
                <td>{{ order.item_discount_amount }}</td>
                <td>{{ order.item_total_amount }}</td>
            </tr>
        </tbody>
    </table>

    <label>ยอดรวมสินค้าก่อนส่วนลด</label>
    <input v-model="state.receipt.total_amount" disabled type="number"><br>
    <label>ยอดรวมส่วนลดสินค้า</label>
    <input v-model="state.receipt.total_discount_amount" disabled type="number"><br>
    <label>Subtotal</label>
    <input v-model="state.receipt.subtotal" disabled type="number"><br>
    <label>ส่วนลดการค้า</label>
    <input v-model="state.receipt.trade_discount" disabled type="number"><br>
    <label>Grand total</label>
    <input v-model="state.receipt.grand_total" disabled type="number"><br>
</template>

<script setup lang="ts">
import { reactive } from 'vue'
import { useRoute } from 'vue-router'
import {
    getOneReceipt,
} from '../api/api.js'

const route = useRoute()

const state = reactive({
    receipt: {
        id: 0,
        code: "",
        date: "",
        total_amount: 0,
        total_discount_amount: 0,
        subtotal: 0,
        trade_discount: 0,
        grand_total: 0,
        orders: [],
    },
})

getReceipt(+route.params.id)

async function getReceipt(id: number) {
    const response = await getOneReceipt(id)

    if (response.code != 200) {
        alert(response.message)
        return
    }

    state.receipt = response.data
}
</script>
