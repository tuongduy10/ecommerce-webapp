import { combineReducers, configureStore } from "@reduxjs/toolkit";
import { useSelector } from "react-redux";
import {
  alertReducer,
  authReducer,
  cartReducer,
  exampleReducer,
  homeReducer,
  productReducer,
  userReducer,
} from "../_reducers";
import { persistStore, persistReducer } from "redux-persist";
import storage from "redux-persist/lib/storage";

const reducers = combineReducers({
  example: exampleReducer,
  home: homeReducer,
  auth: authReducer,
  product: productReducer,
  user: userReducer,
  cart: cartReducer,
  alert: alertReducer,
});

const persistConfig = {
  key: "root",
  storage,
  whitelist: ["auth", "cart"],
};

const persistedReducer = persistReducer(persistConfig, reducers);
const store = configureStore({ 
  reducer: persistedReducer,
  middleware: (getDefaultMiddleware) => getDefaultMiddleware({
    serializableCheck: false
  }),
});
const persistedStore = persistStore(store);

export { store, persistedStore };

export const useExampleStore = () => useSelector((state: RootState) => state.example);
export const useHomeStore = () => useSelector((state: RootState) => state.home);
export const useAuthStore = () => useSelector((state: RootState) => state.auth);
export const useProductStore = () => useSelector((state: RootState) => state.product);
export const useUserStore = () => useSelector((state: RootState) => state.user);
export const useCartStore = () => useSelector((state: RootState) => state.cart);
export const useAlertStore = () => useSelector((state: RootState) => state.alert);

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;