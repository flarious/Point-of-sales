<template>
    <button @click="previous" :disabled="state.currentUnitIndex == undefined || state.currentUnitIndex <= 0">&lt;</button>
    <div>
        <slot name="detail"></slot>
        <button @click="onEdit" :disabled="state.currentUnitIndex == undefined">แก้ไข</button>
        <button @click="onDelete" :disabled="state.currentUnitIndex == undefined">ลบ</button>
    </div>
    <button @click="next" :disabled="state.currentUnitIndex == undefined || state.currentUnitIndex + 1 >= dataLength">&gt;</button>
</template>

<script setup lang="ts">
import { reactive, watch } from 'vue'

const props = defineProps({
    dataLength: {
        type: Number,
    },
    selectedIndex: {
        type: Number,
    }
})

watch(
    () => props.selectedIndex,
    () => {
        state.currentUnitIndex = props.selectedIndex
    }
)

const emits = defineEmits({
    onIndexChange: null,
    onCardEditPressed: null,
    onCardDeletePressed: null
})

interface DataCardState {
    currentUnitIndex?: number,
}

const state: DataCardState = reactive({
    currentUnitIndex: props.selectedIndex,
})

function onIndexChange(index: number) {
    emits("onIndexChange", index)
}

function previous() {
    state.currentUnitIndex! -= 1
    onIndexChange(state.currentUnitIndex!)
}

function next() {
    state.currentUnitIndex! += 1
    onIndexChange(state.currentUnitIndex!)
}

function onEdit() {
    emits("onCardEditPressed")
}

function onDelete() {
    emits("onCardDeletePressed")
}
</script>
