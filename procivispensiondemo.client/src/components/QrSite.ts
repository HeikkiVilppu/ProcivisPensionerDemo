import { ref, onMounted } from 'vue';

export function useApiQr() {
    const loading = ref<boolean>(false);
    const post = ref<string | undefined>("");
    const qrCode = ref<string | undefined>("");

    const fetchData = async () => {
        loading.value = true;
        post.value = "";
        try {
          const response = await fetch('qrcode'); // Ensure correct casing
          const blob = await response.blob(); // Get image as Blob
          qrCode.value = URL.createObjectURL(blob); // Convert to a local URL
          console.log('response', blob);
        } catch (error) {
            console.error('Failed to fetch qrcode', error);
        } finally {
            loading.value = false;
        }
    };

    onMounted(fetchData);

    return { loading, post, qrCode, fetchData };
}
