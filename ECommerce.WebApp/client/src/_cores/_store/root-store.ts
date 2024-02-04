import { configureStore } from "@reduxjs/toolkit";
import { useSelector } from "react-redux";
import { authReducer, exampleReducer, homeReducer, productReducer, userReducer } from "../_reducers";

export const store = configureStore({
  reducer: { 
    example: exampleReducer,
    home: homeReducer,
    auth: authReducer,
    product: productReducer,
    user: userReducer
  }
});

export const useExampleStore = () => useSelector((state: RootState) => state.example);
export const useHomeStore = () => useSelector((state: RootState) => state.home);
export const useAuthStore = () => useSelector((state: RootState) => state.auth);
export const useProductStore = () => useSelector((state: RootState) => state.product);
export const useUserStore = () => useSelector((state: RootState) => state.user);

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;