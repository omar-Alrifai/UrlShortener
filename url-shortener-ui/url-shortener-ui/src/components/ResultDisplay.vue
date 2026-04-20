<script setup>
import { ref } from 'vue';
const copied=ref(false);
const props=defineProps({
    shortUrl:String
});

const copyToClipboard=async()=>{
   try {
    await navigator.clipboard.writeText(props.shortUrl);
    copied.value=true;

    setTimeout(()=>{
        copied.value=false;
    },2000);
   } catch (error) {
    console.error("Failed to copy!",error);
   }
}
</script>

<template>
 <div
      v-if="shortUrl"
      style="margin-top: 20px; font-family: courier; font-size: 125%"
    >
      <p>Short URL:</p>
      <a :href="shortUrl" target="_blank">{{ shortUrl }}</a>
      <br /><br/>
      <button @click="copyToClipboard" style="cursor: pointer;">
        {{ copied?"Copied":"Copy to clipboard" }}
      </button>
    </div>
</template> 