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
    <div v-else-if="state.modalItem.item_id != 0">
        <ItemDetail :object="state.selectedItem">
        </ItemDetail> 
    </div>
    <hr>
</template>

<script lang="ts">
import { reactive, defineComponent, watch } from 'vue'
import {
    getItemsList,
} from '@/api/api'
import type { Item } from '@/interfaces/Item'
import type { PropType } from 'vue'
import type { OrderModel } from '@/interfaces/Order'

import ItemDetail from '@/components/ItemDetail.vue'
import { modalActionState } from '@/states/modal'
import type { PosFormState } from '@/interfaces/State'

export default defineComponent({
    components: {
        ItemDetail,
    },
    props: {
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
        submitted: null,
    },
    setup(props, { emit }) {
        const state: PosFormState = reactive({
            modalItem: props.item,
            selectedItem: {
                code: '-',
                name: '-',
                price: 0,
            },
            items: [],
        })

        watch(
            () => modalActionState.submitModalCounter,
            (newValue, oldValue) => {
                if(newValue > oldValue) {
                    emit("submitted", state.modalItem)
                }
            }
        )

        watch(
            state.modalItem,
            () => {
                mapSelectedItem()
            }
        )

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

        function mapSelectedItem() {
            state.selectedItem.code = state.modalItem.item_code
            state.selectedItem.name = state.modalItem.item_name
            state.selectedItem.price = state.modalItem.item_price
        }
        
        return {
            state,
            getItems,
            selectItem,
        }
    },
})
</script>

<style scoped>
@import '@/assets/selection.css';
</style>