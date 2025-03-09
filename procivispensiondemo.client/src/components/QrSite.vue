<script setup lang="ts">
import { useApiQr } from './QrSite'
const { loading, post, qrCode } = useApiQr();
</script>

<template>
    <div class="weather-component">
        <!--<img alt="qrCode" class="logo" src="../assets/issuerQrCode.png" width="256" height="256" />-->


        <img v-if="qrCode" :src="qrCode" alt="Scan QR Code">


        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See
        </div>
        <h2>Scan the QR code using your digital wallet</h2>

        <div v-if="post" class="content">
            <table>
                <thead>
                    <tr>
                        <th>Test Response from API: {{ post }}</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<style scoped src="./QrSite.css"></style>
