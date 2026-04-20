<script setup>
import { ref } from "vue";
import axios from "axios";
import ShortenForm from "./components/ShortenForm.vue";
import ResultDisplay from "./components/ResultDisplay.vue";

// reactive state
const shortUrl = ref("");
const error = ref("");
const loading = ref(false);

const apiBase = "http://localhost:5021";

// function to call API
const handleShorten = async (url) => {
  error.value = "";
  shortUrl.value = "";
  loading.value = true;

  try {
    const response = await axios.post(`${apiBase}/shorten`, {
      longUrl: url,
    });

    shortUrl.value = `${apiBase}/${response.data.shortCode}`;
  } catch (err) {
    error.value = err.response?.data?.detail || "An unexpected error occurred.";
  } finally {
    loading.value = false;
  }
};
</script>

<template>
  <div
    style="
      max-width: 600px;
      margin: auto;
      padding: 50px;
      text-align: center;
      font-family: courier;
    "
  >
    <h1>URL Shortener</h1>
    <div
      style="color: grey; font-family: courier; font-size: 100%"
      class="desc"
    >
      <h4>Enter your LongUrl to make it short link</h4>
    </div>
<ShortenForm :isLoading="loading" @submit="handleShorten"/>

    <!-- Result -->
    <!-- <div
      v-if="shortUrl"
      style="margin-top: 20px; font-family: courier; font-size: 125%"
    >
      <p>Short URL:</p>
      <a :href="shortUrl" target="_blank">{{ shortUrl }}</a>
    </div> -->
    <ResultDisplay :shortUrl="shortUrl"/>

    <!-- Error -->
    <div v-if="error" style="color: red; margin-top: 20px">
      {{ error }}
    </div>
  </div>
</template>
