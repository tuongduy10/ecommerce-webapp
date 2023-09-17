import { FOOTER_MENU_COL_3, HEADER_MENU } from "src/_configs/web.config";
import { ICON_NAME } from "../mui-icon/_enums/mui-icon.enum";
import MuiIcon from "../mui-icon/mui-icon.component";
import SearchForm from "../search-form/search-form.component";
import { Fragment, useEffect, useRef, useState } from "react";

const HeaderNavMobile = () => {
  const [isOpenedNav, setIsOpenedNav] = useState(false);
  const [isOpenedSearch, setIsOpenedSearch] = useState(false);
  const wrapperListRef = useRef<any>(null);
  const logoRef = useRef<any>(null);
  const footerRef = useRef<any>(null);
  const [menuHeight, setMenuHeight] = useState(0);

  useEffect(() => {
    if (wrapperListRef.current && logoRef.current && footerRef.current) {
      setMenuHeight(wrapperListRef.current.clientHeight - logoRef.current.clientHeight - footerRef.current.clientHeight);
    }
  }, []);

  const onToggleNav = () => {
    setIsOpenedNav((toggle) => !toggle);
    document.body.style.overflow = !isOpenedNav ? "hidden" : "";
  };

  const onToggleSearch = () => {
    setIsOpenedSearch((toggle) => !toggle);
    document.body.style.overflow = !isOpenedSearch ? "hidden" : "";
  };

  return (
    <Fragment>
      <nav className="nav__mobile">
        <div
          ref={wrapperListRef}
          className={`nav__mobile-list flex flex-col ${!isOpenedNav ? "nav-close" : "nav-open"
            }`}
          id="nav__mobile-list-open"
        >
          <div ref={logoRef} className="nav__mobile-logo flex p-4">
            <div className="w-full text-center relative">
              <span
                id="nav__mobile-list-close"
                className="absolute left-0"
                onClick={onToggleNav}
              >
                <MuiIcon name={ICON_NAME.FEATHER.X} />
              </span>
              <a href="/">
                <img
                  className="mx-auto max-h-[50px]"
                  src="https://hihichi.com/images/logo/logo_0052e058-a76f-46c6-ab29-0eaec8a3fc6c.png"
                  alt=""
                />
              </a>
            </div>
          </div>
          <div className="nav__list-mobile-wrapper py-4 pl-4" style={{ height: menuHeight + 'px' }}>
            <div className="nav__list-mobile h-full overflow-x-hidden overflow-y-auto">
              {HEADER_MENU.map((item: any) => (
                <div
                  key={`nav-mobile-${item.path}`}
                  className="nav__link-mobile pb-4"
                >
                  <a href={item.path}>{item.name}</a>
                </div>
              ))}
            </div>
          </div>
          <div ref={footerRef} className="nav__mobile-footer w-full mt-auto mb-0">
            <ul className="nav__mobile-account p-4">
              <li className="pb-4">
                <button className="flex w-full">
                  <span className="icon">
                    <MuiIcon name={ICON_NAME.FEATHER.HELP_CIRCLE} />
                  </span>
                  <span className="text">Tra cứu đơn hàng</span>
                </button>
              </li>
              <li>
                <button className="flex w-full">
                  <span className="icon">
                    <MuiIcon name={ICON_NAME.FEATHER.USER} />
                  </span>
                  <span className="text">Tài khoản của tôi</span>
                </button>
              </li>
            </ul>
            <div className="nav__mobile-info p-4">
              <p className="pb-[1rem]">
                <a href="tel: 0906035526">Phone: 0906035526</a>
              </p>
              <p className="pb-[1rem]">
                <a href="mailto: vantuanitc1989@gmail.com">Mail: vantuanitc1989@gmail.com</a>
              </p>
              {FOOTER_MENU_COL_3.map((item: any, _idx: number) => (
                <p key={`footer-col-3-${item.path}`} className={`${_idx !== FOOTER_MENU_COL_3.length - 1 ? 'pb-[1rem]' : ''}`}>
                  <a href={item.path} className="flex items-center">
                    <MuiIcon name={item.icon} className='mr-3' fontSize="small" /> {item.name}
                  </a>
                </p>
              ))}
            </div>
          </div >
        </div >
        <div className="nav__mobile-action flex justify-between py-2">
          <label
            className={`${!isOpenedNav ? "hidden" : ""} overlay block m-0`}
            id="nav__overlay"
            onClick={onToggleNav}
          ></label>
          <label
            className="nav__menubar mb-0"
            id="nav__menubar-open"
            onClick={onToggleNav}
          >
            <MuiIcon name={ICON_NAME.FEATHER.MENU} />
          </label>

          <button
            className="header__mobile-searchicon"
            onClick={onToggleSearch}
          >
            <MuiIcon
              name={ICON_NAME.FEATHER.SEARCH}
              style={{ stroke: !isOpenedSearch ? "" : "#3b99fc" }}
            />
          </button>

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
        {isOpenedSearch ? <SearchForm /> : null}
      </nav >
      <div
        className={`search__overlay ${!isOpenedSearch ? "hidden" : ""}`}
        onClick={onToggleSearch}
      ></div>
    </Fragment >
  );
};

export default HeaderNavMobile;
