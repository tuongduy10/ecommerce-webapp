import { ProductHelper } from "./../../_shares/_helpers/product-helper";
import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { SLICE_NAME } from "../_enums/state.enum";
import { IProductInitState } from "../_interfaces/state.interface";
import { ISubCategory } from "../_interfaces/inventory.interface";
import { IProduct } from "../_interfaces/product.interface";

const initialState: IProductInitState = { 
  productDetail: undefined,
  productList: [],
  param: {
    brandId: -1,
    orderBy: "",
    subCategoryId: -1,
    optionValueIds: [],

    pageIndex: 0,
    totalPage: 0,
    currentRecord: 0,
    totalRecord: 0,
  },
  subCategories: [],
};

const productSlice = createSlice({
  name: SLICE_NAME.PRODUCT,
  initialState: initialState,
  reducers: {
    setProductDetail: (state, action: PayloadAction<IProduct>) => {
      state.productDetail = action.payload;
    },
    setProductList: (state, action: PayloadAction<any>) => {
      state.productList = ProductHelper.getProductListFormated(action.payload);
    },
    setParam: (state, action: PayloadAction<any>) => {
      state.param = action.payload;
    },
    setPageIndex: (state, action: PayloadAction<number>) => {
      state.param.pageIndex = action.payload;
    },
    setSubCategories: (state, action: PayloadAction<ISubCategory[]>) => {
      state.subCategories = action.payload;
    },
  },
});

export const {
  setParam,
  setProductDetail,
  setProductList,
  setPageIndex,
  setSubCategories,
} = productSlice.actions;

const productReducer = productSlice.reducer;
export default productReducer;
