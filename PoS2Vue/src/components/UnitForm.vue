<template>
    <h1>{{ props.mode }}</h1>
    <label for="form_unit">ชื่อหน่วย</label>
    <input name="form_unit" v-model="state.modalUnit.name"><br>
</template>

<script setup lang="ts">
import { reactive, watch } from 'vue'
import {
    addUnit,
    editUnit,
} from '@/api/api'
import { modalActionState } from '@/states/modal'
import type { UnitModel } from '@/interfaces/Unit.js'
import type { PropType } from 'vue'

const props = defineProps({
    mode: {
        type: String,
    },
    editTarget: {
        type: Number,
    },
    unit: {
        type: Object as PropType<UnitModel>,
        default: () => ({
                name: "",
        })
    },
})

const emit = defineEmits({
    submitted: null,
})

watch(
    () => modalActionState.submitModalCounter,
    async (newValue, oldValue) => {
        if (newValue > oldValue) {
            var response
            if (props.editTarget != undefined) {
                response = await editUnit(props.editTarget, state.modalUnit)
            }
            else {
                response = await addUnit(state.modalUnit)
            }

            if (response.code != 200) {
                alert(response.message)
                return
            }

            emit("submitted")
        }
    }
)

const state = reactive({
    modalUnit: props.unit
})

</script>