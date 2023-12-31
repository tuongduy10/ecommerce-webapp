export class UserHelper {
  public static getUserStatus(_status: boolean) {
    if (_status === true) {
        return { label: "Đang xác thực", color: "#1cc88a" };
    }
    return { label: "Đã khóa", color: "" };
  }
}
