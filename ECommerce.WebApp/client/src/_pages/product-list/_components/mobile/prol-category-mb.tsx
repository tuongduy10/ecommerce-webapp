import { useState } from "react";
import { MuiIcon } from "src/_shares/_components";
import ProlCategory from "../prol-category";
import { ICON_NAME } from "src/_shares/_components/mui-icon/_enums/mui-icon.enum";

const ProlCategoryMb = () => {
    const [isOpenFilter, setOpenFilter] = useState("");
    return (
        <div className="product__filter-wrapper">
            <label
                htmlFor="nav__mobile-menu"
                className={`overlay ${isOpenFilter ? "block" : "hidden"}`}
                id="nav__overlay"
                style={{ margin: 0 }}
                onClick={() => { 
                    setOpenFilter("") 
                    document.body.style.overflow = ""
                }}
            ></label>
            <div
                className="product__filter-dropdown filter__button"
                onClick={() => { 
                    setOpenFilter("filter-open") 
                    document.body.style.overflow = "hidden"
                }}
            >
                Chọn lọc
                <MuiIcon name="CHEVRON_LEFT" />
            </div>
            <div className={`product__filter-dropdown-menu ${isOpenFilter}`}>
                <div className="product__filter-header text-center p-4">
                    <div className="w-100 px-4">
                        <img
                            className="max-w-full max-h-[55px] mx-auto"
                            src="https://hihichi.com/images/brand/brand_d5d8fb28-c399-4259-8a60-3deb4511a810.png"
                            alt=""
                        />
                    </div>
                    <p className="mb-0 mt-5">Bear</p>
                    <MuiIcon
                        name={ICON_NAME.FEATHER.X}
                        className="filter-close feather feather-x"
                        onClick={() => { 
                            setOpenFilter("") 
                            document.body.style.overflow = ""
                        }}
                    />
                </div>
                <div className="filter__menu-wrapper">
                    <div className="filter__menu-items">
                        <ProlCategory />
                    </div>
                </div>
            </div>
        </div>
    )
}

export default ProlCategoryMb;