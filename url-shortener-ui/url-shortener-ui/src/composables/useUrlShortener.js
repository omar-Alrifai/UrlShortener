import {ref} from "vue";
import axios from "axios";

export function useUrlShortener() {
const shortUrl=ref("");
const error=ref("");
const loading=ref(false);
const history=ref([]);

const apiBase="http://localhost:5021";

const shortenUrl=async(url)=>{
    error.value ="";
    shortUrl.value="";
    loading.value=true;

    try {
        const response=await axios.post(`${apiBase}/shorten`,{
            longUrl:url,
        });
        const generatedLink=`${apiBase}/${response.data.shortCode}`;
        shortUrl.value=generatedLink;

        history.value.push({
            longUrl: url,
            shortUrl: generatedLink
        });
    } catch (err) {
        error.value = err.response?.data?.detail || "An unexpected error occurred.";
    }finally{
        loading.value = false;
    }
};

return {
   shortUrl,
    error,
    loading,
    history,
    shortenUrl
};
}