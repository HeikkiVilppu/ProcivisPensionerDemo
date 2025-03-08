<script setup lang="ts">
import { useApiForPensionCredentials } from './ResponseSite'
const { post } = useApiForPensionCredentials();
</script>

<template>
    <div class="weather-component">
        <h2>QR Response from API when QR is read:</h2>

        <div class="content">
          {{post}}
        </div>
    </div>
</template>

