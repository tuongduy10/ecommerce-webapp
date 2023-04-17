import { useState } from "react";
import { HEADER_MENU } from "src/_configs/web.config";
import { ICON_NAME } from "src/_shared/_components/mui-icon/_enums/mui-icon.enum";
import MuiIcon from "src/_shared/_components/mui-icon/mui-icon.component";

const Header = () => {
    const [isOpenedNav, setIsOpenedNav] = useState(false);
    const [isOpenedSearch, setIsOpenedSearch] = useState(false);

    const onToggleNav = () => {
        setIsOpenedNav(toggle => !toggle);
    }

    const onToggleSearch = () => {
        setIsOpenedSearch(toggle => !toggle);
    }

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
                                    <input type="text" placeholder="Thương hiệu, Dịch vụ, Sản phẩm cần tìm" />
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
                    <nav className="nav__mobile">
                        <div className={`nav__mobile-list flex flex-col ${!isOpenedNav ? 'nav-close' : 'nav-open'}`} id="nav__mobile-list-open">
                            <div className="nav__mobile-logo flex p-4">
                                <div className="w-full text-center relative">
                                    <span id="nav__mobile-list-close" className="absolute left-0" onClick={onToggleNav}>
                                        <MuiIcon name={ICON_NAME.FEATHER.X} />
                                    </span>
                                    <a href="/">
                                        <img className="mx-auto" src="https://hihichi.com/images/logo/logo_0052e058-a76f-46c6-ab29-0eaec8a3fc6c.png" alt="" style={{ maxHeight: '50px' }} />
                                    </a>
                                </div>
                            </div>
                            <div className="nav__list-mobile-wrapper py-4 pl-4">
                                <div className="nav__list-mobile h-full" style={{ overflow: 'hidden', overflowY: 'scroll' }}>
                                    {HEADER_MENU.map((item: any) => (
                                        <div key={`nav-mobile-${item.path}`} className="nav__link-mobile pb-4">
                                            <a href={item.path}>{item.name}</a>
                                        </div>
                                    ))}
                                </div>
                            </div>
                            <div className="nav__mobile-footer w-full">
                                <ul className="nav__mobile-account p-4">
                                    <li className="pb-4">
                                        <button className="flex w-full">
                                            <span className="icon">
                                                <MuiIcon name={ICON_NAME.FEATHER.HELP_CIRCLE} />
                                            </span>
                                            <span className="text">
                                                Tra cứu đơn hàng
                                            </span>
                                        </button>
                                    </li>
                                    <li className="pb-4">
                                        <button className="flex w-full">
                                            <span className="icon">
                                                <MuiIcon name={ICON_NAME.FEATHER.USER} />
                                            </span>
                                            <span className="text">
                                                Tài khoản của tôi
                                            </span>
                                        </button>
                                    </li>
                                </ul>
                                <div className="nav__mobile-info p-4">
                                    <p><a href="tel: 03979874403">Phone: 03979874403</a></p>
                                    <p><a href="mailto: mail@gmail.com">Mail: mail@gmail.com</a></p>
                                    <p><a href="/">Facebook: facebook</a></p>
                                </div>
                            </div>
                        </div>
                        <div className="nav__mobile-action flex justify-between py-2">
                            <label className={`${!isOpenedNav ? 'hidden' : ''} overlay m-0`} id="nav__overlay" onClick={onToggleNav}></label>
                            <label className="nav__menubar mb-0" id="nav__menubar-open" onClick={onToggleNav}>
                                <MuiIcon name={ICON_NAME.FEATHER.MENU} />
                            </label>

                            <a href="javascript:void(0);" className="header__mobile-searchicon" onClick={onToggleSearch}>
                                <MuiIcon name={ICON_NAME.FEATHER.SEARCH} style={{ stroke: !isOpenedSearch ? '' : '#3b99fc' }} />
                            </a>

                            <a href="/" className="profile-mobile">
                                <span className="quantity">12</span>
                                <MuiIcon name={ICON_NAME.FEATHER.USER} />
                            </a>

                            <a href="/" className="favorite-mobile">
                                <span className="quantity">1</span>
                                <MuiIcon name={ICON_NAME.FEATHER.HEART} />
                            </a>

                            <a className="cart-mobile minicart" href="/">
                                <span className="quantity">1</span>
                                <MuiIcon name={ICON_NAME.FEATHER.SHOPPING_BAG} />
                            </a>
                        </div>
                        <div className={`searchform__wrapper py-2 px-[20px] ${!isOpenedSearch ? 'hidden' : ''}`}>
                            <div className="searchform input-group rounded w-full">
                                <input type="search" id="search-input" className="form-control"
                                    placeholder="Thương hiệu, dịch vụ, sản phẩm cần tìm" aria-label="Search"
                                    aria-describedby="search-addon" />
                            </div>
                            <div className="searchresult hidden">
                                <p>
                                    <a href="/">Thế giới di động (Điện thoại di động)</a>
                                </p>
                                <p>
                                    <a href="/">FPT Shop (Điện thoại di động)</a>
                                </p>
                                <p>
                                    <a href="/">Samsung Galaxy S20 Ultra</a>
                                </p>
                                <p>
                                    <a href="/">Geforce RTX 3080TI Asus</a>
                                </p>
                                <div className="text-center w-100">
                                    <a href="/" className="search-viewmore">Xem thêm</a>
                                </div>
                            </div>
                        </div>
                    </nav>
                    <div className={`search__overlay ${!isOpenedSearch ? 'hidden' : ''}`} onClick={onToggleSearch}></div>
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
            </div >
        </header >
    );
}

export default Header;