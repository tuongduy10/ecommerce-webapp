import axios from "axios";
import handleError from "./handler-exception";

const baseURL = "https://example.com/api";

const callApi = async (url: string, params: any) => {
  try {
    const api = axios.create({
      baseURL,
      headers: {
        "Content-Type": "application/json",
        "Accept": "application/json",
      },
    });
    api.interceptors.response.use(
      (response) => response,
      (error) => {
        throw new Error(handleError(error));
      }
    );
    const response = await api.post(url, params);
    return response.data;
  } catch (error) {
    console.log(error);
    throw error;
  }
};

export default callApi;
