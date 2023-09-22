import { useEffect } from "react";
import { HEADER_MENU } from "src/_configs/web.config";
import { ROUTE_NAME } from "src/_cores/_enums/route-config.enum";
import UserService from "src/_cores/_services/user.service";
import HeaderNavMobile from "src/_shares/_components/header-nav/nav-mobile.component";
import { ICON_NAME } from "src/_shares/_components/mui-icon/_enums/mui-icon.enum";
import MuiIcon from "src/_shares/_components/mui-icon/mui-icon.component";
//import SearchForm from "src/_shares/_components/search-form/search-form.component";

const leftHeader = [
    { path: '', field: 'findOrder', text: 'Tra cứu đơn hàng', icon: ICON_NAME.FEATHER.HELP_CIRCLE },
    { path: 'tel:0906035526', field: '', text: '0906035526', icon: ICON_NAME.FEATHER.SMARTPHONE },
    { path: ROUTE_NAME.LOGIN, field: 'profile', text: 'Tài khoản của tôi', icon: ICON_NAME.FEATHER.USER },
]

const Header = () => {

    useEffect(() => {
        UserService.getUserInfo().then(res => {
            console.log(res.data)
        }).catch(error => {
            console.log(error)
        });
    }, []);

    return (
        <header className="header sticky top-0 z-[2]">
            <div className="header__top">
                <div className="header__top-content items-center flex pt-2">
                    <div className="logo__container text-center px-5 mx-auto">
                        <a href={ROUTE_NAME.HOME} className="header__logo">
                            <img className="logo img-fluid" src="https://hihichi.com/images/logo/logo_0052e058-a76f-46c6-ab29-0eaec8a3fc6c.png" alt="" />
                        </a>
                    </div>
                    <div className="header__top-info">
                        {leftHeader.map((field) => (
                            <a key={field.field} href={field.path} className="header__top-link">
                                <span className="icon mr-1">
                                    <MuiIcon name={field.icon} />
                                </span>
                                <span className="text">
                                    {field.text}
                                </span>
                            </a>
                        ))}
                    </div>
                    <ul className="header__top-action items-center">
                        <li>
                            <div className="header-searchform header__top-link">
                                <span className="text">
                                    <input type="text" placeholder="Thương hiệu, Dịch vụ, Sản phẩm cần tìm" />
                                </span>
                                <span className="icon">
                                    <MuiIcon name={ICON_NAME.FEATHER.SEARCH} />
                                </span>
                            </div>
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
                            <a href="/" className="header__top-link">
                                <span className="icon cart minicart">
                                    <MuiIcon name={ICON_NAME.FEATHER.SHOPPING_BAG} />
                                </span>
                            </a>
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
                                    <a href={item.path} className="py-3 text-[#707070]">{item.name}</a>
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