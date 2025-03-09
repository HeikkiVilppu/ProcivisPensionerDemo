<script setup lang="ts">
import { ref } from 'vue';
import QrSite from './components/QrSite.vue';
import ResponseSite from './components/ResponseSite.vue';
import * as signalR from "@microsoft/signalr";
const showQrSite = ref(true);
const response = ref<string>("");

const showResponseSite = () => {
  showQrSite.value = !showQrSite.value; // Toggle between QrSite and ResponseSite
};

const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:7089/qrcodehub")
  .withAutomaticReconnect()
  .build();

connection.start().catch(err => console.error(err));

connection.on("QrCodeApproved", (responseFromServer: string) => {
  console.log(`QR-koodi hyväksytty: ${responseFromServer}`);
  response.value = JSON.parse(responseFromServer);
  showResponseSite();
});

</script>

<template>
  <header>
    <div class="toolbar">
      <div class="toolbar-left">
        <button v-if="!showQrSite" @click="showResponseSite">{{"Back to Scanning" }}</button>
      </div>
      <div class="toolbar-title">Procivis Pensioner Swimming Hall Demo</div>
    </div>
  </header>

  <div class="wrapper">
    <QrSite v-if="showQrSite" />
    <ResponseSite :response="response" v-else />
  </div>
</template>

<style scoped src="./App.css"></style>
