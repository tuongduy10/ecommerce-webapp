import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { SLICE_NAME } from "src/_cores/_enums/state.enum";
import { IAlertInitState } from "../_interfaces/state.interface";

const initialState: IAlertInitState = {
  message: "",
  open: false,
  type: "info",
  duration: 3000,
  autoHide: true,
};

const alertSlice = createSlice({
  name: SLICE_NAME.ALERT,
  initialState: initialState,
  reducers: {
    show: (state, action: PayloadAction<string>) => {
      state.open = true;
      state.duration = 6000;
      state.message = action.payload;
      state.type = "info";
    },
    showSuccess: (state, action: PayloadAction<string>) => {
      state.open = true;
      state.message = action.payload;
      state.type = "success";
    },
    showError: (state, action: PayloadAction<string>) => {
      state.open = true;
      state.duration = 3000;
      state.message = action.payload;
      state.type = "error";
    },
    showWarning: (state, action: PayloadAction<string>) => {
      state.open = true;
      state.duration = 6000;
      state.message = action.payload;
      state.type = "warning";
    },
    hideAlert: (state) => {
      state.open = false;
      state.message = "";
    },
  },
});

export const { show, showSuccess, showError, showWarning, hideAlert } =
  alertSlice.actions;

const alerReducer = alertSlice.reducer;
export default alerReducer;
