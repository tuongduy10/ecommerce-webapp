import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { SLICE_NAME } from "src/_cores/_enums/state.enum";
import { IAuthInitState } from "src/_cores/_interfaces/state.interface";
import SessionService from "../_services/session.service";

const initialState: IAuthInitState = {
  loading: false,
  user: null,
  accessToken: '',
};

const authSlice = createSlice({
  name: SLICE_NAME.LOGIN,
  initialState: initialState,
  reducers: {
    setAccessToken: (state, action: PayloadAction<string>) => {
      const accessToken = action.payload;
      state.accessToken = accessToken;
      SessionService.setAccessToken(accessToken);
    },
    setUser: (state, action: PayloadAction<any>) => {
      const user = action.payload;
      state.user = user;
    },
    logout: (state, payload) => {
      state.user = null;
      state.accessToken = '';
      SessionService.deleteAccessToken();
    },
    loginSuccess: (state, action: PayloadAction<{ user: null, accessToken: string }>) => {
      const { user, accessToken } = action.payload;
      state.user = user;
      state.accessToken = accessToken;
    }
  },
});

export const {
  setAccessToken,
  setUser,
  logout,
} = authSlice.actions;

const loginReducer = authSlice.reducer;
export default loginReducer;