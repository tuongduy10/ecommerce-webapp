export default class CookieHelper {
  public static setCookie(name: string, value: string, exp: number = 0) {
    const d = new Date();
    d.setTime(d.getTime() + exp * 24 * 60 * 60 * 1000); // exp = days
    const expires = "expires=" + d.toUTCString();
    document.cookie = `${name}=${value};path=/`;
  }
  public static getCookie(name: string) {
    const decodedCookie = decodeURIComponent(document.cookie);
    const ca = decodedCookie.split(";");
    for (const element of ca) {
      let c = element;
      while (c.startsWith(" ")) {
        c = c.substring(1);
      }
      if (c.startsWith(name)) {
        return c.substring(name.length + 1, c.length);
      }
    }
    return "";
  }
  public static deleteCookie(name: string) {
    document.cookie = name +'=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
  }
}
