import { configureStore } from "@reduxjs/toolkit";
import { useSelector } from "react-redux";
import exampleReducer from "src/_pages/example/_store/example.reducer";
import homeReducer from "src/_pages/home/_store/home.reducer";

export const store = configureStore({
  reducer: { 
    example: exampleReducer,
    home: homeReducer,
  }
});

export const useExampleStore = () => useSelector((state: RootState) => state.example);
export const useHomeStore = () => useSelector((state: RootState) => state.home);

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;