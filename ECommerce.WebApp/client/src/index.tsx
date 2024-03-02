import React from 'react';
import ReactDOM from 'react-dom/client';
import App from 'src/App';
import reportWebVitals from 'src/reportWebVitals';
import 'src/_shares/_assets/_styles/main/global.css';
import 'src/_shares/_assets/_styles/main/header.css';
import 'src/_shares/_assets/_styles/main/footer.css';
import "src/_shares/_assets/_styles/main/product-detail.css";
import 'src/_shares/_assets/_styles/main/main.css';
import { Provider } from 'react-redux';
import { persistedStore, store } from './_cores/_store/root-store';
import { PersistGate } from 'redux-persist/integration/react';
import { MyAlert } from './_shares/_components';


const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);
root.render(
  <Provider store={store}>
    <PersistGate persistor={persistedStore}>
      <MyAlert />
      <App />
    </PersistGate>
  </Provider>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
