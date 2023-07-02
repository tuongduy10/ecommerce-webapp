import axios from "axios";
import handleError from "./handler-exception";
import { ENV } from "src/_configs/enviroment.config";

const api = axios.create({
  baseURL: ENV.API_URL,
  headers: {
    "Content-Type": "application/json",
    Accept: "application/json",
    Authorization: "Bearer access_token",
  },
});

api.interceptors.response.use(
  (response) => response.data,
  (error) => {
    throw new Error(handleError(error));
  }
);

export { api };
