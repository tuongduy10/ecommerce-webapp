import { ProductHelper } from "./../../_shares/_helpers/product-helper";
import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { SLICE_NAME } from "../_enums/state.enum";
import { IProductInitState } from "../_interfaces/state.interface";
import { useSearchParams } from "react-router-dom";
import { ISubCategory } from "../_interfaces/inventory.interface";

const initialState: IProductInitState = {
  productList: [],
  param: {
    brandId: 0,
    orderBy: "",
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

export const { setParam, setProductList, setPageIndex, setSubCategories } =
  productSlice.actions;

const productReducer = productSlice.reducer;
export default productReducer;
