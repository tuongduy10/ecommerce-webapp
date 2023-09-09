import CookieHelper from "src/_shares/_helpers/cookie.helper";

export default class SessionService {
  private static accessTokenKey = "access_token";

  public static setAccessToken(token: string) {
    const _key = this.accessTokenKey;
    const _value = token;
    CookieHelper.setCookie(_key, _value);
  }

  public static getAccessToken() {
    return CookieHelper.getCookie(this.accessTokenKey);
  }

  public static deleteAccessToken() {
    CookieHelper.deleteCookie(this.accessTokenKey);
  }
}
