<template>
    <button @click="previous" :disabled="selectedIndex == undefined || selectedIndex <= 0">&lt;</button>
    <div>
        <slot name="detail"></slot>
        <button @click="onEdit" :disabled="selectedIndex == undefined">แก้ไข</button>
        <button @click="onDelete" :disabled="selectedIndex == undefined">ลบ</button>
    </div>
    <button @click="next" :disabled="selectedIndex == undefined || selectedIndex + 1 >= dataLength">&gt;</button>
</template>

<script setup lang="ts">
import { computed } from 'vue'

const props = defineProps({
    dataLength: {
        type: Number,
    },
    selectedIndex: {
        type: Number,
    }
})

const emit = defineEmits({
    onCardEditPressed: null,
    onCardDeletePressed: null,
    'update:selectedIndex': null,
})

const index = computed({
    get(): number | undefined{
        return props.selectedIndex 
    },
    set(value?: number) {
        emit("update:selectedIndex", value)
    }
})

function previous() {
    index.value = index.value! - 1
}

function next() {
    index.value = index.value! + 1
}

function onEdit() {
    emit("onCardEditPressed")
}

function onDelete() {
    emit("onCardDeletePressed")
}
</script>
