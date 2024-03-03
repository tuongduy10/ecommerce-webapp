import { IBrand, ICategory, ISubCategory } from "./inventory.interface";
import { IProduct, IProductInCart } from "./product.interface";

export interface IAction {
  type: string;
  payload?: any;
}
export interface ICommonInitState {
  data: any[];
  loading: boolean;
}

export interface IUserInitState {
  userApp: any
}

export interface IAlertInitState {
  message: string;
  open: boolean;
  type: 'info' | 'success' | 'error' | 'warning';
  duration: number;
  autoHide: boolean;
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
  selectedBrand: IBrand | null;
  [key: string]: any;
}

export interface IProductInitState { 
  productDetail: IProduct | undefined,
  productList: any[]
  param: {
    brandId: number,
    orderBy?: "asc" | "desc" | "newest" | "discount" | "",
    subCategoryId: number,
    optionValueIds: number[],

    // pagination
    pageIndex: number,
    totalPage: number,
    currentRecord: number,
    totalRecord: number
  },
  subCategories: ISubCategory[];
  [key: string]: any;
}

export interface ICartInitState {
  productsInCart: IProductInCart[];
  totalQty: number;
  totalPrice: number;
}