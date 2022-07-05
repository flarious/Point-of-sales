<template>
    <h1>Item detail</h1>
    <h3>รหัสสินค้า</h3>
    <p>{{ state.data.code || state.data.item_code }}</p><br>
    <h3>ชื่อสินค้า</h3>
    <p>{{ state.data.name || state.data.item_name }}</p><br>
    <h3>ราคา</h3>
    <p>{{ state.data.price || state.data.item_price }}</p>
</template>
    
<script setup lang="ts">
import { reactive, watch } from "vue"

const props = defineProps({
    object: {
        type: Object,
        default: () => ({
            code: "-",
            name: "-",
            price: "0",
        })
    },
})

watch(
    () => props.object,
    () => {
        state.data = props.object
    }
)

const state = reactive({
    data: props.object
})
</script>
