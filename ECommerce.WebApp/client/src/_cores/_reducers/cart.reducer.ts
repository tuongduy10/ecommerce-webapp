import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { SLICE_NAME } from "../_enums/state.enum";
import { ICartInitState } from "../_interfaces/state.interface";
import {
  INewProductInCart,
  IProductInCart,
} from "../_interfaces/product.interface";
import { CartHelper } from "src/_shares/_helpers/cart.helper";
import { sumBy } from "lodash";

const initialState: ICartInitState = {
  productsInCart: [],
  totalQty: 0,
  totalPrice: 0,
};

const cartSlice = createSlice({
  name: SLICE_NAME.CART,
  initialState: initialState,
  reducers: {
    addToCart: (state, action: PayloadAction<INewProductInCart>) => {
      const _product = action.payload;
      const _uniqId = CartHelper.getUniqId(_product);
      const idx = state.productsInCart.findIndex((_) => _.uniqId === _uniqId);
      if (idx > -1) {
        state.productsInCart[idx].qty += _product.qty;
        state.productsInCart[idx].uniqId = CartHelper.getUniqId(
          state.productsInCart[idx]
        );
      } else {
        const _productInCart = {
          ..._product,
          uniqId: _uniqId,
        } as IProductInCart;
        state.productsInCart.push(_productInCart);
      }
      state.totalQty = sumBy(state.productsInCart, "qty");
      state.totalPrice = sumBy(
        state.productsInCart,
        (_) => (_.discount ?? _.price) * _.qty
      );
    },
    removeItem: (state, action: PayloadAction<string>) => {
      const _uniqId = action.payload;
      const idx = state.productsInCart.findIndex((_) => _.uniqId === _uniqId);
      if (idx > -1) {
        state.productsInCart = state.productsInCart.filter((_) => _.uniqId !== _uniqId);
        state.totalQty = sumBy(state.productsInCart, "qty");
        state.totalPrice = sumBy(
          state.productsInCart,
          (_) => (_.discount ?? _.price) * _.qty
        );
      }
    },
    changeItemQty: (state, action: PayloadAction<{ uniqId: string, qty: number }>) => {
      const { uniqId, qty } = action.payload;
      const idx = state.productsInCart.findIndex((_) => _.uniqId === uniqId);
      if (idx > -1) {
        state.productsInCart[idx].qty = qty;
        state.totalQty = sumBy(state.productsInCart, "qty");
        state.totalPrice = sumBy(
          state.productsInCart,
          (_) => (_.discount ?? _.price) * _.qty
        );
      }
    }
  },
});

export const { addToCart, removeItem, changeItemQty } = cartSlice.actions;

const cartReducer = cartSlice.reducer;
export default cartReducer;
