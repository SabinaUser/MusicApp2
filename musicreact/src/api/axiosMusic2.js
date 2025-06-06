import axios from "axios";

const musicApi = axios.create({
    baseURL: "http://localhost:8000",
    // 👇 headers daxil etmə
});

musicApi.interceptors.request.use((config) => {
    const token = localStorage.getItem("token");
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

export default musicApi;
