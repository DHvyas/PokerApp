import axios from 'axios';

// Create an Axios instance
const api = axios.create({
    baseURL: 'https://localhost:7163', // Replace with your actual base URL
});

// Add a request interceptor to attach the token
api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

export default api;
