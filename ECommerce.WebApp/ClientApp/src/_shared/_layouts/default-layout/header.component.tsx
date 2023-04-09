import { HEADER_MENU } from "src/_configs/web.config";

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
                            <span className="icon">
                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"
                                    fill="none" stroke="#333" strokeWidth="1" strokeLinecap="round"
                                    strokeLinejoin="round" className="feather feather-help-circle">
                                    <circle cx="12" cy="12" r="10"></circle>
                                    <path d="M9.09 9a3 3 0 0 1 5.83 1c0 2-3 3-3 3"></path>
                                    <line x1="12" y1="17" x2="12.01" y2="17"></line>
                                </svg>
                            </span>
                            <span className="text">
                                Tra cứu đơn hàng
                            </span>
                        </a>
                        <a href="tel:@Model.config.PhoneNumber" className="header__top-link">
                            <span className="icon">
                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"
                                    fill="none" stroke="#333" strokeWidth="1" strokeLinecap="round"
                                    strokeLinejoin="round" className="feather feather-smartphone">
                                    <rect x="5" y="2" width="14" height="20" rx="2" ry="2"></rect>
                                    <line x1="12" y1="18" x2="12.01" y2="18"></line>
                                </svg>
                            </span>
                            <span className="text">
                                0397974403
                            </span>
                        </a>
                        <a href="/" className="header__top-link">
                            <span className="icon">
                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"
                                    fill="none" stroke="#333" strokeWidth="1" strokeLinecap="round"
                                    strokeLinejoin="round" className="feather feather-user">
                                    <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path>
                                    <circle cx="12" cy="7" r="4"></circle>
                                </svg>
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
                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"
                                        fill="none" stroke="#333" strokeWidth="1" strokeLinecap="round"
                                        strokeLinejoin="round" className="feather feather-search">
                                        <circle cx="11" cy="11" r="8"></circle>
                                        <line x1="21" y1="21" x2="16.65" y2="16.65"></line>
                                    </svg>
                                </span>
                            </a>
                        </li>
                        <li className="wishlist-action">
                            <a href="/" className="header__top-link">
                                <span className="icon favorite">
                                    <span className="quantity">1</span>
                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24"
                                        viewBox="0 0 24 24" fill="none" stroke="#333" strokeWidth="1"
                                        strokeLinecap="round" strokeLinejoin="round" className="feather feather-heart">
                                        <path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z">
                                        </path>
                                    </svg>
                                </span>
                            </a>
                        </li>
                        <li className="cart-action">
                            <a href="/" className="header__top-link">
                                <span className="icon cart minicart">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"
                                        fill="none" stroke="#333" strokeWidth="1" strokeLinecap="round"
                                        strokeLinejoin="round" className="feather feather-shopping-bag">
                                        <path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"></path>
                                        <line x1="3" y1="6" x2="21" y2="6"></line>
                                        <path d="M16 10a4 4 0 0 1-8 0"></path>
                                    </svg>
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