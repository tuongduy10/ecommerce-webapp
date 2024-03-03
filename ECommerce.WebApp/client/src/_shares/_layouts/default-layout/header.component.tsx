import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { Link, useLocation } from "react-router-dom";
import { HEADER_MENU } from "src/_configs/web.config";
import { ROUTE_NAME } from "src/_cores/_enums/route-config.enum";
import { setAccessToken, setUser } from "src/_cores/_reducers/auth.reducer";
import SessionService from "src/_cores/_services/session.service";
import UserService from "src/_cores/_services/user.service";
import { useAuthStore, useCartStore } from "src/_cores/_store/root-store";
import { MiniCart } from "src/_shares/_components";
import HeaderNavMobile from "src/_shares/_components/header-nav/nav-mobile.component";
import { ICON_NAME } from "src/_shares/_components/mui-icon/_enums/mui-icon.enum";
import MuiIcon from "src/_shares/_components/mui-icon/mui-icon.component";

const Header = () => {
    const authStore = useAuthStore();
    const cartStore = useCartStore();
    const dispatch = useDispatch();
    const location = useLocation();
    const [hideCart, setHideCart] = useState(false);

    useEffect(() => {
        async function getUserInfo() {
            const response = await UserService.getUserInfo() as any;
            if (response) {
                dispatch(setUser(response));
            }
        }
        if (!authStore.user) {
            getUserInfo();
        }
    }, [authStore.accessToken]);


    useEffect(() => {
        const hiddenPath = [
            '/cart',
            '/payment',
        ];
        setHideCart(hiddenPath.includes(location.pathname));
    }, [location.pathname]);

    const leftHeader = [
        { path: '', field: 'findOrder', text: 'Tra cứu đơn hàng', icon: ICON_NAME.FEATHER.HELP_CIRCLE },
        { path: 'tel:0906035526', field: '', text: '0906035526', icon: ICON_NAME.FEATHER.SMARTPHONE },
        { path: authStore.accessToken ? ROUTE_NAME.USER_PROFILE : ROUTE_NAME.LOGIN, field: 'profile', text: authStore.user?.fullName ? `Hi, ${authStore.user?.fullName}` : 'Tài khoản của tôi', icon: ICON_NAME.FEATHER.USER },
    ];

    return (
        <header className="header sticky top-0 z-[2]">
            <div className="header__top">
                <div className="header__top-content items-center flex pt-2">
                    <div className="logo__container text-center px-5 mx-auto">
                        <Link to={ROUTE_NAME.HOME} className="header__logo">
                            <img className="logo img-fluid" src="https://hihichi.com/images/logo/logo_0052e058-a76f-46c6-ab29-0eaec8a3fc6c.png" alt="" />
                        </Link>
                    </div>
                    <div className="header__top-info">
                        {leftHeader.map((field) => (
                            <Link key={field.field} to={field.path} className="header__top-link">
                                <span className="icon mr-1">
                                    <MuiIcon name={field.icon} />
                                </span>
                                <span className="text">
                                    {field.text}
                                </span>
                            </Link>
                        ))}
                    </div>
                    <ul className="header__top-action items-center">
                        <li>
                            <span className="header-searchform header__top-link">
                                <span className="text">
                                    <input type="text" placeholder="Thương hiệu, Dịch vụ, Sản phẩm cần tìm" />
                                </span>
                                <span className="icon">
                                    <MuiIcon name={ICON_NAME.FEATHER.SEARCH} />
                                </span>
                            </span>
                        </li>
                        <li className="wishlist-action">
                            <a href="/" className="header__top-link">
                                <span className="icon favorite">
                                    <span className="quantity">1</span>
                                    <MuiIcon name={ICON_NAME.FEATHER.HEART} />
                                </span>
                            </a>
                        </li>
                        <li className="cart-action">
                            <Link to={ROUTE_NAME.CART} className="header__top-link">
                                <span className="icon cart minicart">
                                    <MuiIcon name={ICON_NAME.FEATHER.SHOPPING_BAG} />
                                    {cartStore.totalQty > 0 && (
                                        <span className="quantity">{cartStore.totalQty}</span>
                                    )}
                                </span>
                            </Link>
                            {cartStore.productsInCart.length > 0 && !hideCart && (
                                <MiniCart />
                            )}
                        </li>
                    </ul>
                </div>
            </div>
            <div className="header__inner">
                <div className="custom-container">
                    <HeaderNavMobile />
                    <nav className="nav__pc">
                        <ul className="nav__pc-items nav__default flex">
                            {HEADER_MENU.map((item: any) => (
                                <li key={`nav-pc-${item.path}`} className="nav__link">
                                    <Link to={item.path} className="py-3 text-[#707070]">{item.name}</Link>
                                </li>
                            ))}
                        </ul>
                    </nav>
                </div>
            </div >
        </header >
    );
}

export default Header;