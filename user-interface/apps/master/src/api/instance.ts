import axios from "axios";

const instance = axios.create({
  baseURL: import.meta.env.VITE_LOCAL_API_URL + "/api",
  timeout: 5_000,
  headers: { "Content-Type": "application/json" },
});

export default instance;
