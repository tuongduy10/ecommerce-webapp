export interface IShop {
  id: number;
  name: string;
  phoneNumber: string;
  mail: string;
  address: string;
  wardCode: string;
  districtCode: string;
  cityCode: string;
  joinDate: string;
  tax: number;
  status: number;
  user: null | any;
}

export interface IUser {
  id: number;
  mail: string | null;
  fullName: string;
  joinDate: string;
  address: string | null;
  wardCode: string | null;
  districtCode: string | null;
  cityCode: string | null;
  phone: string | null;
  roles: string[] | null;
  status: boolean;
  isSystemAccount: boolean;
  isOnline: boolean;
  lastOnline: string;
}
