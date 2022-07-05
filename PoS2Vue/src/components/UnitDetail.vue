<template>
    <h1>Unit detail</h1>
    <h3>ชื่อหน่วย</h3>
    <p>{{ state.data.name }}</p>
</template>

<script setup lang="ts">
import { reactive, watch } from "vue"

const props = defineProps({
    object: {
        type: Object,
        default: () => ({
            name: "-"
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

    
