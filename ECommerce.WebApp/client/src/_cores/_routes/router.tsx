import {
  HomePage,
  LoginPage,
  ProductDetailPage,
  UserProfilePage,
  CartPage,
  BlogPage,
  ExamplePage,
  ProductListPage,
} from "src/_pages";
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import DefaultLayout from "src/_shares/_layouts/default-layout/default-layout.component";
import AdminLayout from "src/_shares/_layouts/admin-layout/admin-layout.component";
import {
  Login,
  Dashboard,
  ProductList
} from "src/_pages/admin/components";
import { PrivateRoute } from "./private-route";
import { ADMIN_ROUTE_NAME } from "../_enums/route-config.enum";
import { NotFoundLayout } from "src/_shares/_layouts/error-layout/notfound-layout";
const Router = () => {
  const browserRoutes = createBrowserRouter([
    {
      path: "/",
      element: <DefaultLayout />,
      errorElement: <NotFoundLayout />,
      children: [
        { path: "/v2", element: <HomePage /> },
        { path: "/v2/example", element: <ExamplePage /> },
        { path: "/v2/login", element: <LoginPage /> },
        { path: "/v2/user-profile", element: <UserProfilePage /> },
        { path: "/v2/cart", element: <CartPage /> },
        { path: "/v2/blog", element: <BlogPage /> },
        { path: "/v2/product-list", element: <ProductListPage /> },
        { path: "/v2/product-detail", element: <ProductDetailPage /> },
      ],
    },
    { path: ADMIN_ROUTE_NAME.LOGIN, element: <Login /> },
    {
      element: <AdminLayout />,
      children: [
        { 
          path: ADMIN_ROUTE_NAME.DASHBOARD, 
          element: 
            <PrivateRoute>
              <Dashboard />
            </PrivateRoute>
        },
        { 
          path: ADMIN_ROUTE_NAME.MANAGE_PRODUCT, 
          element: 
            <PrivateRoute>
              <ProductList />
            </PrivateRoute>
        }
      ],
    }
  ]);

  return <RouterProvider router={browserRoutes} />;
};

export default Router;
