<script setup></script>
<template>
    <div class="spinner"></div>
</template>

<style scoped>
.spinner{
    margin: 20px auto;
    width: 40px;
    height: 40px;
    border: 4px solid #ccc;
    border-top: 4px solid #333;
    border-radius: 50%;
    animation: spin 1s ease-in-out infinite;
    
}

@keyframes spin{
    to{
        transform: rotate(360deg);
    }
}
</style>