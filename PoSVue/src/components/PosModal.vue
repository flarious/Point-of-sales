<!--
<template>
    <h3>เลือกสินค้า</h3>
    <hr>
    <table>
        <tr @click="selectItem(item)" v-for="(item, index) in items" :key="index">
            <td :class="{ selected: modalItem.item_id == item.id }">{{ item.name }}</td>
        </tr>
    </table>
    <hr>
    <p v-if="modalItem.item_id == 0">No Item Selected</p>
    <div v-if="modalItem.item_id != 0">
        <h1>Item detail</h1>
        <h3>รหัสสินค้า</h3>
        <p>{{ modalItem.item_code }}</p>
        <h3>ชื่อสินค้า</h3>
        <p>{{ modalItem.item_name }}</p>
        <h3>ราคา</h3>
        <p>{{ modalItem.item_price }}</p>   
    </div>
    <hr>
    <button @click="onClose">close</button><br>
    <button @click="onSave">save changes</button>
</template>

<script>
import {
    getItemsList,
} from '../api/api.js'

export default {
    props: {
        item: {
            type: Object,
            default(raw) {
                return {
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
    },
    data () {
        return {
            modalItem: this.item,
            items: [],
        }
    },
    emits: {
        ModalClose: null,
        ModalSave: null,
    },  
    methods: {
        async getItems() {
            const response = await getItemsList()

            if (response.code != 200) {
                alert(response.message)
                return
            }

            this.items = response.data
        },
        selectItem(item) {
            this.modalItem.item_id = item.id
            this.modalItem.item_code = item.code
            this.modalItem.item_name = item.name
            this.modalItem.item_price = item.price
            this.modalItem.item_unit = item.unit
            this.modalItem.item_unit_id = item.unit_id
        },
        onClose() {
            this.$emit("ModalClose", true)
            document.body.style.backgroundColor = "black"
        },
        onSave() {
            this.$emit("ModalSave", this.modalItem)
            this.onClose()
        },
    },
    async created () {
        await this.getItems()
    }
}
</script>

<style scoped>
@import '../assets/modal.css';

</style>
-->

<template>
    <h3>เลือกสินค้า</h3>
    <hr>
    <table>
        <tr @click="selectItem(item)" v-for="(item, index) in state.items" :key="index">
            <td :class="{ selected: state.modalItem.item_id == item.id }">{{ item.name }}</td>
        </tr>
    </table>
    <hr>
    <p v-if="state.modalItem.item_id == 0">No Item Selected</p>
    <div v-if="state.modalItem.item_id != 0">
        <h1>Item detail</h1>
        <h3>รหัสสินค้า</h3>
        <p>{{ state.modalItem.item_code }}</p>
        <h3>ชื่อสินค้า</h3>
        <p>{{ state.modalItem.item_name }}</p>
        <h3>ราคา</h3>
        <p>{{ state.modalItem.item_price }}</p>   
    </div>
    <hr>
    <button @click="onClose">close</button><br>
    <button @click="onSave">save changes</button>
</template>

<script lang="ts">
import { reactive, defineComponent } from 'vue'
import {
    getItemsList,
} from '../api/api.js'
import type { Item } from '@/interfaces/Item.js'
import type { PropType } from 'vue'
import type { OrderModel } from '@/interfaces/Order.js'

export default defineComponent({
    props: {
        showed: {
            type: Boolean,
        },
        item: {
            type: Object as PropType<OrderModel>,
            default: () => ({
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
            })
        },
    },
    emits: {
        ModalClose: null,
        ModalSave: null,
    },
    setup(props, { emit }) {
        const state = reactive({
            modalItem: props.item,
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

        function selectItem(item: Item) {
            state.modalItem.item_id = item.id
            state.modalItem.item_code = item.code
            state.modalItem.item_name = item.name
            state.modalItem.item_price = item.price
            state.modalItem.item_unit = item.unit
            state.modalItem.item_unit_id = item.unit_id
        }

        function onClose() {
            emit("ModalClose", true)
            document.body.style.backgroundColor = "black"
        }

        function onSave() {
            emit("ModalSave", state.modalItem)
            onClose()
        }
        
        return {
            state,
            getItems,
            selectItem,
            onClose,
            onSave,
        }
    },
})
</script>

<style scoped>
@import '../assets/modal.css';

</style>