import { HomePage, LoginPage, ProductDetailPage, UserProfilePage } from 'src/_pages';
import { RouterProvider, createBrowserRouter } from 'react-router-dom';
import DefaultLayout from 'src/_shared/_layouts/default-layout/default-layout.component';

const Router = () => {
  const browserRoutes = createBrowserRouter([
    {
      path: '/',
      element: <DefaultLayout />,
      children: [
        { path: '/', element: <HomePage />  },
        { path: '/login', element: <LoginPage />  },
        { path: '/product/:id', element: <ProductDetailPage />  },
        { path: '/user-profile', element: <UserProfilePage />  }
      ]
    }
  ]);

  return (
    <RouterProvider router={browserRoutes}/>
  );
};

export default Router;