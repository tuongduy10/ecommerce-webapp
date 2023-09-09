import { IBrand, ICategory } from "./inventory.interface";

export interface IAction {
  type: string;
  payload?: any;
}
export interface ICommonInitState {
  data: any[];
  loading: boolean;
}

export interface IAuthInitState { 
  accessToken: string;
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