
<script setup lang="ts">
const response = defineProps({
  response: String, // Expecting an object response
});
</script>

<template>
    <div class="weather-component">
        <h2>QR Response from API when QR is read:</h2>
        <div class="content">
          {{response}}
        </div>
    </div>
</template>

