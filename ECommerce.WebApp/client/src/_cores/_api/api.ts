import axios from "axios";
import handleError from "./handler-exception";
import { ENV } from "src/_configs/enviroment.config";
import SessionService from "../_services/session.service";

const api = axios.create({
  baseURL: ENV.API_URL,
  headers: {
    "Content-Type": "application/json",
    Accept: "application/json",
    Authorization: SessionService.getAccessToken() ? `Bearer ${SessionService.getAccessToken()}` : '',
  },
});

api.interceptors.response.use(
  (response) => response.data,
  (error) => {
    throw new Error(handleError(error));
  }
);

export { api };
