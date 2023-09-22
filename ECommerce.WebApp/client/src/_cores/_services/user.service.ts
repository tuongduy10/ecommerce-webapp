import { api } from "../_api/api";
import SessionService from "./session.service";

export default class UserService {
  public static logout() {
    SessionService.deleteAccessToken();
  }
  
  public static login(param: any) { 
    return api.post("/user/login", param);
  }

  public static addUser(param: any) {
    return api.post("/add-user", param);
  }

  public static getUsers() {
    return api.get<any>("/get-users");
  }

  public static getUser(id: any) {
    return api.get<any>(`/get-user/${id}`);
  }

  public static getUserInfo() {
    return api.post<any>(`/user/info`);
  }
}
