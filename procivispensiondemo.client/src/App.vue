<script setup lang="ts">
import { ref } from 'vue';
import QrSite from './components/QrSite.vue';
import ResponseSite from './components/ResponseSite.vue';

const showQrSite = ref(true);

const fakeQrResponse = () => {
  showQrSite.value = !showQrSite.value; // Toggle between QrSite and ResponseSite
};

</script>

<template>
  <header>
    <div class="toolbar">
      <div class="toolbar-left">
        <button @click="fakeQrResponse">{{ !showQrSite ? "Back to Scanning" : "Fake QR scan" }}</button>
      </div>
      <div class="toolbar-title">Procivis Pensioner Swimming Hall Demo</div>
    </div>
  </header>

  <div class="wrapper">
    <QrSite v-if="showQrSite" />
    <ResponseSite v-else />
  </div>
</template>

<style scoped src="./App.css"></style>
