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
        <ItemDetail :object="state.modalItem">
        </ItemDetail> 
    </div>
    <hr>
</template>

<script lang="ts">
import { reactive, defineComponent, inject, computed, watch } from 'vue'
import {
    getItemsList,
} from '../api/api.js'
import type { Item } from '@/interfaces/Item.js'
import type { PropType } from 'vue'
import type { OrderModel } from '@/interfaces/Order.js'

import ItemDetail from '@/components/ItemDetail.vue'
import type { GlobalState } from '@/interfaces/State.js'
import { modalActionState } from '@/states/modal'

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
        const globalState = inject("state") as GlobalState
        const isModalHidden = computed(() => globalState.isModalHidden)
        
        watch(
            () => modalActionState.shouldSubmitModal,
            async () => {
                if(modalActionState.shouldSubmitModal) {
                    modalActionState.submitted()
                    emit("submitted", state.modalItem)
                }
            }
        )

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

        // function onClose() {
        //     emit("ModalClose", "modal")
        // }

        // function onSave() {
        //     emit("ModalSave", state.modalItem)
        //     onClose()
        // }
        
        return {
            state,
            getItems,
            selectItem,
            // onClose,
            // onSave,
        }
    },
})
</script>