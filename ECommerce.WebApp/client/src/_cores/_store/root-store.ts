import { configureStore } from "@reduxjs/toolkit";
import { useSelector } from "react-redux";
import exampleReducer from "../_reducers/example.reducer";
import homeReducer from "../_reducers/home.reducer";
import authReducer from "../_reducers/auth.reducer";

export const store = configureStore({
  reducer: { 
    example: exampleReducer,
    home: homeReducer,
    auth: authReducer,
  }
});

export const useExampleStore = () => useSelector((state: RootState) => state.example);
export const useHomeStore = () => useSelector((state: RootState) => state.home);
export const useAuthStore = () => useSelector((state: RootState) => state.auth);

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;