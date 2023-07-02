import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { useSelector } from "react-redux";
import { SLICE_NAME } from "src/_cores/_enums/state.enum";
import { RootState } from "src/_cores/_store/root-store";

const initialState = {
  data: [] as any[],
  loading: false,
};

const exampleSlice = createSlice({
  name: SLICE_NAME.EXAMPLE,
  initialState: initialState,
  reducers: {
    addData: (state, action: PayloadAction<any>) => {
      const item = action.payload;
      state.data.push(item);
    },
    deleteData: (state, action: PayloadAction<any>) => {
      state.data = [];
    },
    updateData: (state, action: PayloadAction<any>) => {
      state.data = [action.payload];
    },
    setData: (state, action: PayloadAction<any>) => {
      state.data = action.payload;
    },
  }
});
export const { addData, deleteData, updateData, setData } = exampleSlice.actions;

const exampleReducer = exampleSlice.reducer;
export default exampleReducer;
