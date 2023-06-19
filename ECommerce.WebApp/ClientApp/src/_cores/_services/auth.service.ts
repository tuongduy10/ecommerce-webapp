import { api } from "../_api/api";

export default class AuthService { 

    public static login(phoneNumber: string, password: string ) {
        const params = {
            phoneNumber: phoneNumber,
            password: password
        }
        return api.post<string>('/login', params)
    }
}