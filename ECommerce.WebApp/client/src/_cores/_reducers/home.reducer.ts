import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { SLICE_NAME } from "src/_cores/_enums/state.enum";
import { IHomeInitState } from "src/_cores/_interfaces/state.interface";
import { IBrand, ICategory } from "src/_cores/_interfaces/inventory.interface";
import { ALPHABET_VALUES } from "src/_pages/home/_enums/sorting.enum";

const initialState: IHomeInitState = {
  alphabetSelected: ALPHABET_VALUES[0].data,
  categorySelected: "",
  highLightBrands: [],
  brands: [],
  categories: [],
};

const homeSlice = createSlice({
  name: SLICE_NAME.HOME,
  initialState: initialState,
  reducers: {
    setHighLightBrands: (state, action: PayloadAction<IBrand[]>) => {
      state.highLightBrands = action.payload;
    },
    setBrands: (state, action: PayloadAction<IBrand[]>) => {
      state.brands = action.payload;
    },
    setCategories: (state, action: PayloadAction<ICategory[]>) => {
      state.categories = action.payload;
    },
    selectAlphabet: (state, action) => {
      const values = ALPHABET_VALUES.find(
        (item) => item.value === action.payload
      );
      state.alphabetSelected = values!.data || ALPHABET_VALUES[0].data;
    },
    selectCategory: (state, action) => {
      state.categorySelected = action.payload;
    },
  },
});

export const {
  setHighLightBrands,
  setBrands,
  setCategories,
  selectAlphabet,
  selectCategory,
} = homeSlice.actions;

const homeReducer = homeSlice.reducer;
export default homeReducer;
