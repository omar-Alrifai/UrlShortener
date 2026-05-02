<script setup>
import { ref } from 'vue';

const inputUrl = ref("");
const customAlias = ref("");
const expiresAt = ref("");


const props = defineProps({ 
  isLoading: Boolean 
});
const emit = defineEmits(["submit"]);

const handleSubmit = () => {
  if (props.isLoading) return;
  emit("submit", inputUrl.value, customAlias.value, expiresAt.value);
  inputUrl.value = "";
  customAlias.value = "";
  expiresAt.value="";
};
</script>

<template>
  <div class="url-shortener">
    <div class="input-group">
      <div class="input-wrapper">
        <input
          v-model="inputUrl"
          type="url"
          placeholder="Paste your long URL here..."
          :disabled="isLoading"
          @keyup.enter="handleSubmit"
        />
        <label class="input-label">Long URL</label>
      </div>

      <div class="input-wrapper">
        <input
          v-model="customAlias"
          type="text"
          placeholder="Custom short code (optional)"
          :disabled="isLoading"
          @keyup.enter="handleSubmit"
        />
        <label class="input-label">Custom Alias</label>
      </div>

      <div class="input-wrapper">
        <input
          v-model="expiresAt"
          type="datetime-local"
          :disabled="isLoading"
        />
        <label class="input-label">Expires At (optional) </label>
      </div>

    </div>

    <button 
      class="shorten-btn"
      @click="handleSubmit" 
      :disabled="isLoading"
    >
      <span v-if="isLoading"></span>
      {{ isLoading ? "Shortening..." : "Create Short URL" }}
    </button>
  </div>
</template>

<style scoped>
.url-shortener {
  max-width: 500px;
  margin: 0 auto;
  padding: 2rem;
  background: #fff;
  border-radius: 16px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
}

.input-group {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.input-wrapper {
  position: relative;
  display: flex;
  flex-direction: column;
}

.input-wrapper input {
  width: 100%;
  padding: 16px 20px;
  font-size: 16px;
  border: 2px solid #e5e7eb;
  border-radius: 12px;
  background: #fafbfc;
  transition: all 0.3s ease;
  outline: none;
  box-sizing: border-box;
}

.input-wrapper input:focus {
  border-color: #3b82f6;
  background: #fff;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

.input-wrapper input:disabled {
  background: #f9fafb;
  color: #9ca3af;
  cursor: not-allowed;
}

.input-label {
  position: absolute;
  top: -16px;
  left: 20px;
  font-size: 14px;
  font-weight: 500;
  color: #6b7280;
  pointer-events: none;
  transition: all 0.3s ease;
  background: transparent;
}

.input-wrapper input:focus + .input-label,
.input-label
.input-wrapper input:not(:placeholder-shown) + .input-label {
  top: -8px;
  left: 12px;
  font-size: 12px;
  color: #3b82f6;
  background: #fff;
  padding: 0 6px;
}

.shorten-btn {
  width: 100%;
  padding: 16px 24px;
  font-size: 16px;
  font-weight: 600;
  color: white;
  background: linear-gradient(135deg, #3b82f6 0%, #1d4ed8 100%);
  border: none;
  border-radius: 12px;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  box-shadow: 0 4px 14px rgba(59, 130, 246, 0.4);
}

.shorten-btn:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 8px 25px rgba(59, 130, 246, 0.5);
}

.shorten-btn:active:not(:disabled) {
  transform: translateY(0);
}

.shorten-btn:disabled {
  background: #d1d5db;
  cursor: not-allowed;
  box-shadow: none;
  transform: none;
}
</style>