<template>
    <div class="card">
        <button @click="previous" :disabled="index == undefined || index <= 0">&lt;</button>
        <div>
            <slot name="detail"></slot>
            <button @click="onEdit" :disabled="index == undefined">แก้ไข</button>
            <button @click="onDelete" :disabled="index == undefined">ลบ</button>
        </div>
        <button @click="next" :disabled="index == undefined || index + 1 >= dataLength">&gt;</button>
    </div>
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

<style scoped>
@import '@/assets/card.css';
</style>
