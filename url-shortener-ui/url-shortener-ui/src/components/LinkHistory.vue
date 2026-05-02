<script setup>
const props=defineProps({
    links:Array
});
const isExpired = (expiresAt) => {
  if (!expiresAt) return false;
  return new Date(expiresAt) <= new Date();
};
</script>

<template>
  <div v-if="links.length" style="margin-top: 30px; text-align: left;">
    <h3>History</h3>
    <ul>
      <li v-for="(link, index) in links" :key="index" style="margin-bottom: 5px; width: 900px;">
        <small style="font-size: large;">{{ link.longUrl }}</small> --> 
        <a :href="link.shortUrl" target="_blank" @click="link.clicks">
          {{ link.shortUrl }}
        </a>
        <span class="click-badge">{{ link.clicks }} Clicks</span>
        <span v-if="isExpired(link.expiresAt)" class="expired-badge">Expired</span>
        <span v-else-if="link.expiresAt" class="expires-badge">
          Expires: {{ new Date(link.expiresAt).toLocaleString() }}
        </span>
      </li>
    </ul>
  </div>
</template>

<style scoped>
.click-badge {
  background-color: #e2e8f0;
  padding: 2px 8px;
  border-radius: 12px;
  font-size: 0.8em;
  margin-left: 10px;
  color: #4a5568;
}
.expired-badge {
  background-color: #fecaca;
  color: #991b1b;
  padding: 2px 8px;
  border-radius: 12px;
  font-size: 0.8em;
  margin-left: 10px;
}
.expires-badge {
  background-color: #dbeafe;
  color: #1e40af;
  padding: 2px 8px;
  border-radius: 12px;
  font-size: 0.8em;
  margin-left: 10px;
}
</style>