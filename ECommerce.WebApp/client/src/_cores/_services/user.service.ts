import { IUserGetParam } from "src/_pages/admin/interfaces/user-interface";
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

  public static getUserList(param: IUserGetParam) {
    return api.post("/user/user-list", param);
  }

  public static getUsers() {
    return api.get<any>("/get-users");
  }

  public static getUser(id: any) {
    return api.get<any>(`/user/get-user/${id}`);
  }

  public static getUserInfo() {
    return api.post<any>(`/user/info`);
  }

  public static getShops() {
    return api.get(`/user/shops`);
  }

  public static save(param: any) { 
    return api.post(`/user/save`, param);
  }

  public static updateUser(param: any) {
    return api.post(`/user/update-user`, param);
  }

  public static updateUserStatus(param: any) {
    return api.post(`/user/update-status`, param);
  }
}
