<script setup>
import { ref } from 'vue';
const inputUrl=ref("");

const props=defineProps({isLoading:Boolean});
const emit=defineEmits(["submit"]);

const handleSubmit=()=>{
    if(props.isLoading) return;
    emit("submit",inputUrl.value);
    inputUrl.value="";
};

</script>
<template>
    <div>
        <input
      v-model="inputUrl"
      type="text"
      placeholder="Enter long URL"
      style="width: 100%; padding: 10px; margin-bottom: 10px"
      :disabled="isLoading"
      @keyup.enter="handleSubmit"
    />

    <!-- Button -->
    <button @click="handleSubmit" :disabled="isLoading">
        {{isLoading?"Processing...":"Shorten"}}
    </button>
    </div>
</template>