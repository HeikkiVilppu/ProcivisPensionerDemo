import { ref, onMounted } from 'vue';

export function useApiQr() {
    const loading = ref<boolean>(false);
    const post = ref<string | undefined>("");

    const fetchData = async () => {
        loading.value = true;
        post.value = "";

        try {
          const response = await fetch('qrcode'); // Ensure correct casing
          const data = await response.json(); // Use .text() instead of .json() if string
          post.value = data.message;
          console.log('response', data.message);
        } catch (error) {
            console.error('Failed to fetch qrcode', error);
        } finally {
            loading.value = false;
        }

    };

    onMounted(fetchData);

    return { loading, post, fetchData };
}
