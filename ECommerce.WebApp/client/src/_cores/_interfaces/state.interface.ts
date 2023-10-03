import { IBrand, ICategory, ISubCategory } from "./inventory.interface";

export interface IAction {
  type: string;
  payload?: any;
}
export interface ICommonInitState {
  data: any[];
  loading: boolean;
}

export interface IAuthInitState { 
  loading?: boolean,
  accessToken: string;
  user: any;
  // userName: string;
  // phoneNumber: string;
  // password: string;
  // roles: string[];
  // loading: boolean;
  // resultMessage: string;
}

export interface IHomeInitState { 
  alphabetSelected: string[];
  categorySelected: string;
  highLightBrands: IBrand[];
  brands: IBrand[];
  categories: ICategory[];
}

export interface IProductInitState { 
  productList: any[]
  param: {
    brandId: number,
    orderBy?: "asc" | "desc" | "newest" | "discount" | "",

    // pagination
    pageIndex: number,
    totalPage: number,
    currentRecord: number,
    totalRecord: number
  },
  subCategories: ISubCategory[];
}