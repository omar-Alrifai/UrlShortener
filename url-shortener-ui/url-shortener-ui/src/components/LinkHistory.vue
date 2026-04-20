<script setup>
const props=defineProps({
    links:Array
});

</script>

<template>
  <div v-if="links.length" style="margin-top: 30px; text-align: left;">
    <h3>History</h3>
    <ul>
      <li v-for="(link, index) in links" :key="index" style="margin-bottom: 5px;">
        <small style="font-size: large;">{{ link.longUrl }}</small> --> 
        <a :href="link.shortUrl" target="_blank">{{ link.shortUrl }}</a>
      </li>
    </ul>
  </div>
</template>