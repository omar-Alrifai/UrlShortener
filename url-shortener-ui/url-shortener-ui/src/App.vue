<script setup>
import { useUrlShortener } from "./composables/useUrlShortener";
import ShortenForm from "./components/ShortenForm.vue";
import ResultDisplay from "./components/ResultDisplay.vue";
import LinkHistory from "./components/LinkHistory.vue";
import LoadingSpinner from "./components/LoadingSpinner.vue";
import ErrorMessage from "./components/ErrorMessage.vue";

const { shortUrl, error, loading, history, shortenUrl } = useUrlShortener();

const handleShorten = (url) => {
shortenUrl(url);
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
    <ErrorMessage :message="error"/>
    <LinkHistory :links="history"/>
  </div>
</template>
