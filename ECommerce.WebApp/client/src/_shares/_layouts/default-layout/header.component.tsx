import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { HEADER_MENU } from "src/_configs/web.config";
import { ROUTE_NAME } from "src/_cores/_enums/route-config.enum";
import SessionService from "src/_cores/_services/session.service";
import UserService from "src/_cores/_services/user.service";
import HeaderNavMobile from "src/_shares/_components/header-nav/nav-mobile.component";
import { ICON_NAME } from "src/_shares/_components/mui-icon/_enums/mui-icon.enum";
import MuiIcon from "src/_shares/_components/mui-icon/mui-icon.component";

const Header = () => {
    const [fullName, setUserName] = useState<string>("");
    const token = SessionService.getAccessToken();

    useEffect(() => {
        UserService.getUserInfo().then((res: any) => {
            setUserName(res.fullName);
        }).catch(error => {
            console.log(error)
        });
    }, []);

    const leftHeader = [
        { path: '', field: 'findOrder', text: 'Tra cứu đơn hàng', icon: ICON_NAME.FEATHER.HELP_CIRCLE },
        { path: 'tel:0906035526', field: '', text: '0906035526', icon: ICON_NAME.FEATHER.SMARTPHONE },
        { path: token ? ROUTE_NAME.USER_PROFILE : ROUTE_NAME.LOGIN, field: 'profile', text: fullName ? `Hi, ${fullName}` : 'Tài khoản của tôi', icon: ICON_NAME.FEATHER.USER },
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
                                    <span className="quantity">12</span>
                                </span>
                            </Link>
                            <div className="minicart-content">
                                <div className="w-full minicart-quantity">
                                    <div className="header-title pb-4 pt-3 minicart-totalquantity">Giỏ hàng: 1</div>
                                </div>
                                <div className="minicart-products filter--scrollbar px-4">
                                    <div className="minicart-product flex py-4">
                                        <div className="minicart__product-image mr-2">
                                            <img className="h-full w-full" src="${img}" alt="" />
                                        </div>
                                        <div className="minicart__product-info flex flex-col">
                                            <div className="name">
                                                <strong>Name 01</strong>
                                            </div>
                                            <div className="detail mb-auto">Xanh, 1.5</div>
                                            <div className="amount">x12</div>
                                            <div className="price line-through">
                                                1100 ₫
                                            </div>
                                            <div className="price">1000 ₫</div>
                                            <div className="total-price product-totalprice">
                                                12000 ₫
                                            </div>
                                        </div>
                                        <span className="minicart-remove">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24"
                                                viewBox="0 0 24 24" fill="none" stroke="#333" strokeWidth="1"
                                                strokeLinecap="round" strokeLinejoin="round" className="feather feather-x">
                                                <line x1="18" y1="6" x2="6" y2="18"></line>
                                                <line x1="6" y1="6" x2="18" y2="18"></line>
                                            </svg>
                                        </span>
                                    </div>
                                </div>
                                <div className="minicart-totals px-4 pb-3">
                                    <div className="flex justify-between pt-3 pb-4">
                                        <div>Tạm tính</div>
                                        <div className="total-price cart-totalprice"><strong>120.000.000 ₫</strong></div>
                                    </div>
                                    <div className="text-center block">
                                        <a className="minicart-checkout inline-block w-full py-3 mb-4 btn-black" href={ROUTE_NAME.PAYMENT}>THANH TOÁN</a>
                                        <Link to={ROUTE_NAME.CART}><u>Xem / Chỉnh sửa</u></Link>
                                    </div>
                                </div>
                            </div>
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