import { HEADER_MENU } from "src/_configs/web.config";
import { ICON_NAME } from "src/_shared/_components/mui-icon/_enums/mui-icon.enum";
import MuiIcon from "src/_shared/_components/mui-icon/mui-icon.component";

const Header = () => {
    return (
        <header className="header sticky top-0 z-[2]">
            <div className="header__top">
                <div className="header__top-content items-center flex pt-2">
                    <div className="logo__container text-center px-5 mx-auto">
                        <a href="https://hihichi.com/" className="header__logo">
                            <img className="logo img-fluid" src="https://hihichi.com/images/logo/logo_0052e058-a76f-46c6-ab29-0eaec8a3fc6c.png" alt="" />
                        </a>
                    </div>
                    <div className="header__top-info">
                        <a href="find-order.html" className="header__top-link">
                            <span className="icon mr-1">
                                <MuiIcon name={ICON_NAME.FEATHER.HELP_CIRCLE} />
                            </span>
                            <span className="text">
                                Tra cứu đơn hàng
                            </span>
                        </a>
                        <a href="tel:@Model.config.PhoneNumber" className="header__top-link">
                            <span className="icon mr-1">
                                <MuiIcon name={ICON_NAME.FEATHER.SMARTPHONE} />
                            </span>
                            <span className="text">
                                0397974403
                            </span>
                        </a>
                        <a href="/" className="header__top-link">
                            <span className="icon mr-1">
                                <MuiIcon name={ICON_NAME.FEATHER.USER} />
                            </span>
                            <span className="text">
                                Tài khoản của tôi
                            </span>
                        </a>
                    </div>
                    <ul className="header__top-action items-center">
                        <li>
                            <a href="/" className="header-searchform header__top-link">
                                <span className="text">
                                    <input type="text" placeholder="Thương hiệu/ Dịch vụ/ Sản phẩm cần tìm" />
                                </span>
                                <span className="icon">
                                    <MuiIcon name={ICON_NAME.FEATHER.SEARCH} />
                                </span>
                            </a>
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
                    <nav className="nav__pc">
                        <ul className="nav__pc-items nav__default flex">
                            {HEADER_MENU.map((item: any) => (
                                <li key={`nav-pc-${item.path}`} className="nav__link">
                                    <a href={item.path} className="py-3">{item.name}</a>
                                </li>
                            ))}
                        </ul>
                    </nav>
                </div>
            </div>
        </header>
    );
}

export default Header;