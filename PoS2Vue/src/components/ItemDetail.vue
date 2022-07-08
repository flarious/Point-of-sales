<template>
    <h1>Item detail</h1>
    <h3>รหัสสินค้า</h3>
    <p>{{ object.code }}</p><br>
    <h3>ชื่อสินค้า</h3>
    <p>{{ object.name }}</p><br>
    <h3>ราคา</h3>
    <p>{{ object.price }}</p>
</template>
    
<script setup lang="ts">
import type { ItemDetail } from "@/interfaces/Item"
import type { PropType } from 'vue'

const props = defineProps({
    object: {
        type: Object as PropType<ItemDetail>,
        default: () => ({
            code: "-",
            name: "-",
            price: "0",
        })
    },
})
</script>
