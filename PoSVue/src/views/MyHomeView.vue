<template>
    <header>
        <h1>Point of Sales</h1>
    </header>

    <main>
        <nav>
            <button><router-link v-bind:to="{name: 'unit'}">ตั้งค่าหน่วยนับ</router-link></button>
            <button><router-link v-bind:to="{name: 'item'}">ตั้งค่าสินค้า</router-link></button>
            <button><router-link v-bind:to="{name: 'receipt'}">ดูใบเสร็จรับเงิน</router-link></button>
            <button><router-link v-bind:to="{name: 'pos'}">ออกใบเสร็จรับเงิน</router-link></button>
        </nav>
    </main>
    
</template>

<script setup lang="ts">
</script>