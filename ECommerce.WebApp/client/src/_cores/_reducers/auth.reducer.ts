import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { SLICE_NAME } from "src/_cores/_enums/state.enum";
import { IAuthInitState } from "src/_cores/_interfaces/state.interface";

const initialState: IAuthInitState = {
  loading: false,
  user: null,
  accessToken: '',
};

const authSlice = createSlice({
  name: SLICE_NAME.LOGIN,
  initialState: initialState,
  reducers: {
    login: (state, action: PayloadAction<{ user: null, accessToken: string }>) => {
      const { user, accessToken } = action.payload;
      state.user = user;
      state.accessToken = accessToken;
    },
    logout: (state, payload) => {
      state.user = null;
      state.accessToken = '';
    },
    loginSuccess: (state, action: PayloadAction<{ user: null, accessToken: string }>) => {
      const { user, accessToken } = action.payload;
      state.user = user;
      state.accessToken = accessToken;
    }
  },
});

export const {
  login,
  logout,
} = authSlice.actions;

const loginReducer = authSlice.reducer;
export default loginReducer;