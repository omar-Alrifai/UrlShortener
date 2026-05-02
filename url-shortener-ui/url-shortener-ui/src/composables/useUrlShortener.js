import { ref } from "vue";
import axios from "axios";

export function useUrlShortener() {
  const shortUrl = ref("");
  const error = ref("");
  const loading = ref(false);
  const history = ref([]);
  const currentLink = ref(null);


  const apiBase = "http://localhost:5021";

  const toUtcIso = (localDateTime) => {
    if (!localDateTime) return null;
    const date = new Date(localDateTime);
    if (isNaN(date.getTime())) return null;
    return date.toISOString();
  };


  const shortenUrl = async (url, customAlias = null, localExpiry = null) => {
    error.value = "";
    shortUrl.value = "";
    loading.value = true;
    currentLink.value = null;
    try {
      const payload = { longUrl: url };
      if (customAlias && customAlias.trim() !== "") {
        payload.customCode = customAlias.trim();
      }
        const expiryUtc = toUtcIso(localExpiry);
      if (expiryUtc) {
        payload.ExpiresAt = expiryUtc;
      }

      const response = await axios.post(`${apiBase}/shorten`, payload);
      const link = response.data.shortLink;

      const generatedLink = `${apiBase}/${link.code}`;
      shortUrl.value = generatedLink;
      currentLink.value = { ...link, shortUrl: generatedLink };

      history.value.unshift({
        longUrl: link.longUrl,
        shortUrl: generatedLink,
        clicks: link.clicks,
        expiresAt: link.expiresAt,
        code: link.code,
      });
    }  catch (err) {
      if (err.response?.status === 409) {
        error.value = err.response?.data?.detail || "Custom code already taken. Please choose another.";
      } else if (err.response?.status === 410) {
        error.value = err.response?.data?.detail || "This link has expired.";
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
    currentLink
  };
}