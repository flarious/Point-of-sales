<template>
    <h1>Unit detail</h1>
    <h3>ชื่อหน่วย</h3>
    <p>{{ object.name }}</p>
</template>

<script setup lang="ts">
import type { UnitModel } from "@/interfaces/Unit"
import type { PropType } from "vue"

const props = defineProps({
    object: {
        type: Object as PropType<UnitModel>,
        default: () => ({
            name: "-"
        })
    },
})
</script>

    
