import axios from "axios";

const instance = axios.create({
  baseURL: import.meta.env.VITE_LOCAL_API_URL + "/api",
  timeout: 5_000_000,
  headers: { "Content-Type": "application/json" },
});

instance.interceptors.request.use(
  (config) => {
    const hostId = sessionStorage.getItem("hostId");
    const deviceId = sessionStorage.getItem("deviceId");
    const roomCode = sessionStorage.getItem("roomCode");
    
    if (hostId) {
      config.headers["hostId"] = hostId;
    }
    
    if (deviceId) {
      config.headers["deviceId"] = deviceId;
    }
    
    if (roomCode) {
      config.headers["roomCode"] = roomCode;
    }

    return config;
  }
);

export default instance;
