import axios from "axios";
import { ENV } from "src/_configs/enviroment.config";
import SessionService from "../_services/session.service";
import { ADMIN_ROUTE_NAME } from "../_enums/route-config.enum";

const api = axios.create({
  baseURL: ENV.API_URL,
  headers: {
    "Content-Type": "application/json",
    Accept: "application/json",
  },
});

api.interceptors.request.use(
  (config) => {
    const accessToken = SessionService.getAccessToken();
    if (accessToken) {
      config.headers.Authorization = `Bearer ${accessToken}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

api.interceptors.response.use(
  (response) => response.data,
  async (error) => {
    if (error.response) {
      if (error.response.status === 401) {
        if (window.location.pathname.includes("admin")) {
          window.location.replace(ADMIN_ROUTE_NAME.LOGIN);
        }
      }

      // The request was made, but the server responded with a status code
      console.error(
        "Response Error:",
        error.response.status,
        error.response.data
      );
    } else if (error.request) {
      console.error("Request Error:", error.request);
    } else {
      console.error("Error:", error.message);
    }
    return Promise.reject(error);
  }
);

export { api };
