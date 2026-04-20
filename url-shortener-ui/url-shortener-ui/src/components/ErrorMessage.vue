<script setup>
const props=defineProps({message:String});

</script>
<template>
    <div v-if="message" class="error-box">
        <strong>Error:</strong>{{ message }}
    </div>
</template>

<style scoped>
.error-box{
    margin-top: 20px;
    padding: 12px;
    background-color: #fff5f5;
    color: #c53030;
    border: 1px solid #feb2b2;
    border-radius: 6px;
    font-size: 0.95em;
    text-align: center;
}
</style>