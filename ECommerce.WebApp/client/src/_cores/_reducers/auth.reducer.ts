import { createSlice } from "@reduxjs/toolkit";
import { SLICE_NAME } from "src/_cores/_enums/state.enum";
import { IAuthInitState } from "src/_cores/_interfaces/state.interface";

const initialState: IAuthInitState = {
  accessToken: '',
};

const authSlice = createSlice({
  name: SLICE_NAME.LOGIN,
  initialState: initialState,
  reducers: {
    login: (state, payload) => {

    },
    logout: (state, payload) => {

    }
  },
});

export const {
  login,
  logout,
} = authSlice.actions;

const loginReducer = authSlice.reducer;
export default loginReducer;