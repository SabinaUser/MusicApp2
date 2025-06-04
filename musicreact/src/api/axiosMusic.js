import axios from "axios";

const musicApi = axios.create({
    baseURL: "https://localhost:7117",
    headers: {
        "Content-Type": "application/json",
    },
});

musicApi.interceptors.request.use((config) => {
    const token = localStorage.getItem("token");
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

export default musicApi;
