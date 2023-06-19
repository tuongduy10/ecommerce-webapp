import { api } from "../_api/api";

export default class UserService {

    public static addUser(param: any) {
        return api.post('/add-user', param);
    }

    public static getUsers() { 
        return api.get<any>('/get-users');
    }

    public static getUser(id: any) {
        return api.get<any>(`/get-user/${id}`);
    }
}