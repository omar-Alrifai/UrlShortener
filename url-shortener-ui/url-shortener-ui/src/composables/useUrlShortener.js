import { ref } from "vue";
import axios from "axios";

export function useUrlShortener() {
  const shortUrl = ref("");
  const error = ref("");
  const loading = ref(false);
  const history = ref([]);

  const apiBase = "http://localhost:5021";

  const shortenUrl = async (url, customAlias = null) => {
    error.value = "";
    shortUrl.value = "";
    loading.value = true;

    try {
      const payload = { longUrl: url };
      if (customAlias && customAlias.trim() !== "") {
        payload.customCode = customAlias.trim();
      }
      

      const response = await axios.post(`${apiBase}/shorten`, payload);
      const link = response.data.shortLink;

      const generatedLink = `${apiBase}/${link.code}`;
      shortUrl.value = generatedLink;

      history.value.unshift({
        longUrl: link.longUrl,
        shortUrl: generatedLink,
        clicks: link.clicks,
      });
    } catch (err) {
      if (err.response?.status === 409) {
        error.value = err.response?.data?.detail || "Custom code already taken. Please choose another.";
      } else {
        error.value = err.response?.data?.detail || "An unexpected error occurred.";
      }
    } finally {
      loading.value = false;
    }
  };

  return {
    shortUrl,
    error,
    loading,
    history,
    shortenUrl,
  };
}