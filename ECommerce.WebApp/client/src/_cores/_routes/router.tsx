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
  ProductList,
  UserList,
  ProductDetail
} from "src/_pages/admin/components";
import { PrivateRoute } from "./private-route";
import { ADMIN_ROUTE_NAME, ROUTE_NAME } from "../_enums/route-config.enum";
import { NotFoundLayout } from "src/_shares/_layouts/error-layout/notfound-layout";
const Router = () => {
  const browserRoutes = createBrowserRouter([
    {
      path: "/",
      element: <DefaultLayout />,
      errorElement: <NotFoundLayout />,
      children: [
        { path: ROUTE_NAME.HOME, element: <HomePage /> },
        { path: ROUTE_NAME.EXAMPLE, element: <ExamplePage /> },
        { path: ROUTE_NAME.LOGIN, element: <LoginPage /> },
        { path: ROUTE_NAME.USER_PROFILE, element: <UserProfilePage /> },
        { path: ROUTE_NAME.CART, element: <CartPage /> },
        { path: ROUTE_NAME.BLOG, element: <BlogPage /> },
        { path: ROUTE_NAME.PRODUCT_LIST, element: <ProductListPage /> },
        { path: ROUTE_NAME.PRODUCT_DETAIL, element: <ProductDetailPage /> },
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
        },
        { 
          path: ADMIN_ROUTE_NAME.MANAGE_PRODUCT_ADD, 
          element: 
            <PrivateRoute>
              <ProductDetail />
            </PrivateRoute>
        },
        { 
          path: ADMIN_ROUTE_NAME.MANAGE_USER, 
          element: 
            <PrivateRoute>
              <UserList />
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
