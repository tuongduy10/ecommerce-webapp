export class UserHelper {
  public static getUserStatus(_status: boolean) {
    if (_status === true) {
      return { label: "Đang xác thực", color: "#1cc88a" };
    }
    return { label: "Đã khóa", color: "" };
  }
  public static getUserAddress(user: any) {
    let address = user.userAddress ?? "";
    if (user.userWardName) {
      address += `, ${user.userWardName}`;
    }
    if (user.userDistrictName) {
      address += `, ${user.userDistrictName}`;
    }
    if (user.userCityName) {
      address += `, ${user.userCityName}`;
    }
    return address;
  }
}
