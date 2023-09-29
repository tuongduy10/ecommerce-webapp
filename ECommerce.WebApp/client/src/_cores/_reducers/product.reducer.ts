import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { SLICE_NAME } from "../_enums/state.enum";
import { IProductInitState } from "../_interfaces/state.interface";

const initialState: IProductInitState = {
  productList: [],
  param: {
    pageIndex: 0,
    totalPage: 0,
    currentRecord: 0,
    totalRecord: 0,
  },
};

const productSlice = createSlice({
  name: SLICE_NAME.PRODUCT,
  initialState: initialState,
  reducers: {
    setProductList: (state, action: PayloadAction<any>) => {
      state.productList = action.payload;
    },
    setParam: (state, action: PayloadAction<any>) => {
      state.param = action.payload;
    },
  },
});

export const { setParam, setProductList } = productSlice.actions;

const productReducer = productSlice.reducer;
export default productReducer;
