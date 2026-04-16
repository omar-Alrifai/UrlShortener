<script setup>
import { ref } from "vue";
import axios from "axios";

// reactive state
const longUrl = ref("");
const shortUrl = ref("");
const error = ref("");
const loading = ref(false);

// function to call API
const shortenUrl = async () => {
  error.value = "";
  shortUrl.value = "";
  loading.value = true;

  try {
    const apiBase = "http://localhost:5021";
    const response = await axios.post(`${apiBase}/shorten`, {
      longUrl: longUrl.value,
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
    <!-- Input -->
    <input
      v-model="longUrl"
      type="text"
      placeholder="Enter long URL"
      style="width: 100%; padding: 10px; margin-bottom: 10px"
      @keyup.enter="shortenUrl"
    />

    <!-- Button -->
    <button @click="shortenUrl" :disabled="loading">
      {{ loading ? "Loading..." : "Shorten" }}
    </button>

    <!-- Result -->
    <div
      v-if="shortUrl"
      style="margin-top: 20px; font-family: courier; font-size: 125%"
    >
      <p>Short URL:</p>
      <a :href="shortUrl" target="_blank">{{ shortUrl }}</a>
    </div>

    <!-- Error -->
    <div v-if="error" style="color: red; margin-top: 20px">
      {{ error }}
    </div>
  </div>
</template>
