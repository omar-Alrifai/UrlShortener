<script setup>
import { ref } from "vue";
import axios from "axios";
import ShortenForm from "./components/ShortenForm.vue";
import ResultDisplay from "./components/ResultDisplay.vue";
import LinkHistory from "./components/LinkHistory.vue";
import LoadingSpinner from "./components/LoadingSpinner.vue";


// reactive state
const shortUrl = ref("");
const error = ref("");
const loading = ref(false);
const history=ref([]);

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
    history.value.push({longUrl:url,shortUrl:shortUrl.value});
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

    <ResultDisplay v-if="!loading && shortUrl" :shortUrl="shortUrl"/>

    <LoadingSpinner v-if="loading"/>

    <!-- Error -->
    <div v-if="error" style="color: red; margin-top: 20px">
      {{ error }}
    </div>
    <LinkHistory :links="history"/>
  </div>
</template>
