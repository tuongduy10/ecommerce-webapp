export class AppUser {
  private accessToken: string;
  userName: string;
  userInfo?: IAppUserInfo | null = null;
  roleTypes: string[] = [];
  isAdmin: boolean = false;

  constructor(_userName: string, _accessToken: string, _userInfo?: any) {
    this.userName = _userName;
    this.accessToken = _accessToken;

    if (_userInfo) {
      if (_userInfo["roleTypes"]) {
        this.roleTypes = _userInfo["roleTypes"];
      }

      this.isAdmin = _userInfo["isAdmin"] || false;
      this.userInfo = { ..._userInfo };
    }
  }

  public get getAccessToken(): string {
    return this.accessToken;
  }

  public setToken(_token: string): string {
    this.accessToken = _token;
    return this.accessToken;
  }
}

export interface IAppUserInfo {
  id: number;
  fullName: string;
  username: string;
  address: string;
}
