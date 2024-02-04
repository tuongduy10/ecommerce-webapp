import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { SLICE_NAME } from "src/_cores/_enums/state.enum";
import { IUserInitState } from "../_interfaces/state.interface";

const initialState: IUserInitState = {
  userApp: null,
};

const userSlice = createSlice({
  name: SLICE_NAME.USER,
  initialState: initialState,
  reducers: {
    setUser: (state, action: PayloadAction<any>) => {
      const user = action.payload;
      state.userApp = user;
    },
  },
});

export const { setUser } = userSlice.actions;

const userReducer = userSlice.reducer;
export default userReducer;
