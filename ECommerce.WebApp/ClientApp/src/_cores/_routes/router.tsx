import {
  HomePage,
  LoginPage,
  ProductDetailPage,
  UserProfilePage,
  CartPage,
  BlogPage,
  ExamplePage,
  ProductListPage
} from "src/_pages";
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import DefaultLayout from "src/_shared/_layouts/default-layout/default-layout.component";
import AdminLayout from "src/_shared/_layouts/admin-layout/admin-layout.component";
const Router = () => {
  const browserRoutes = createBrowserRouter([
    {
      path: "/",
      element: <DefaultLayout />,
      children: [
        { path: "/v2/", element: <HomePage /> },
        { path: '/v2/example', element: <ExamplePage /> },
        { path: "/v2/login", element: <LoginPage /> },
        { path: "/v2/product/:id", element: <ProductDetailPage /> },
        { path: "/v2/user-profile", element: <UserProfilePage /> },
        { path: "/v2/cart", element: <CartPage /> },
        { path: "/v2/blog", element: <BlogPage /> },
        { path: "/v2/product-list", element: <ProductListPage /> }
      ],
    },
    {
      element: <AdminLayout />,
      children: [
        { path: "/v2/admin", element: <div>admin</div> },
        { path: "/v2/admin/manage-product", element: <div>product</div> }
      ],
    }
  ]);

  return <RouterProvider router={browserRoutes} />;
};

export default Router;
