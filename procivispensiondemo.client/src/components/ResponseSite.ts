import { ref, onMounted } from "vue";

export function useApiForPensionCredentials() {
    const loading = ref<boolean>(false);
    const post = ref<string | undefined>("");

    const fetchData = async () => {
        loading.value = true;
        post.value = "";
        try {
          const response = await fetch("/qrcode/pension"); // Correct route
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.json();
            post.value = data;
        } catch (error) {
            console.error("Failed to fetch pension credential", error);
        } finally {
            loading.value = false;
        }
    };

    onMounted(fetchData);

    return { loading, post, fetchData };
}
