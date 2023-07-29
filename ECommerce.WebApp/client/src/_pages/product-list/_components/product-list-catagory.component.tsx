import { useState } from "react";
import MuiIcon from "src/_shares/_components/mui-icon/mui-icon.component";
const ProductListCatagory = () => {
  const [isOpenFilter, setOpenFilter] = useState("");
  const [isExpanded, setIsExpanded] = useState(false);
  const [isFeatherIconChanged, setFeatherIconChanged] = useState(false);
  const toggleFilterItemMobile = () => {
    setIsExpanded(!isExpanded);
    setFeatherIconChanged(!isFeatherIconChanged);
  };

  return (
    <div className="product__filter-wrapper">
      <label
        htmlFor="nav__mobile-menu"
        className={`overlay ${isOpenFilter ? "block" : "hidden"}`}
        id="nav__overlay"
        style={{ margin: 0 }}
        onClick={() => setOpenFilter("")}
      ></label>
      <div
        className="product__filter-dropdown filter__button"
        onClick={() => setOpenFilter("filter-open")}
      >
        Chọn lọc
        <MuiIcon name="CHEVRON_LEFT" />
      </div>
      <div className={` product__filter-dropdown-menu ${isOpenFilter}`}>
        <div className="product__filter-header text-center p-4">
          <div className="w-100 px-4">
            <img
              style={{ maxHeight: "55px", maxWidth: "100%" }}
              className=""
              src=""
              alt=""
            />
          </div>
          <p className="mb-0 mt-5">Brand img</p>
          {/* <svg
            xmlns="http://www.w3.org/2000/svg"
            width="24"
            height="24"
            viewBox="0 0 24 24"
            fill="none"
            stroke="#333"
            stroke-width="1"
            stroke-linecap="round"
            stroke-linejoin="round"
            className="filter-close feather feather-x"
            onClick={() => setOpenFilter("")}
          >
            <line x1="18" y1="6" x2="6" y2="18"></line>
            <line x1="6" y1="6" x2="18" y2="18"></line>
          </svg> */}
          <MuiIcon
            name="X"
            className="filter-close feather feather-x"
            onClick={() => setOpenFilter("")}
          />
        </div>
        <div className="filter__menu-wrapper">
          <div className="filter__menu-items">
            <div className="filter__block-mobile">
              <p
                className="filter__item-title mb-0 flex justify-between items-center"
                onClick={toggleFilterItemMobile}
              >
                Nồi nấu điện đa năng
                <MuiIcon
                  name="CHEVRON_RIGHT"
                  className={`feather feather-chevron-right ${
                    isFeatherIconChanged ? "svg-right" : ""
                  }`}
                />
              </p>

              <ul
                className="filter__items-mobile"
                style={{
                  maxHeight: isExpanded
                    ? "calc(185px + 125 * ((100vw - 375px) / 1545))"
                    : "0px",
                }}
              >
                <li className="filter__item-mobile">
                  <p className="mb-0">
                    <MuiIcon name="ARROW_DROP_DOWN" />
                    Màu sắc
                  </p>
                  <ul className="filter__sub-items-mobile">
                    <li className="flex items-center">
                      <input
                        className="checkboxFilter mr-2"
                        id="checkbox__mobile"
                        type="checkbox"
                      />
                      <label
                        htmlFor="checkbox__mobile-@productOption.OptionValueId"
                        className="flex items-center mb-0"
                      >
                        <div className="mr-1">a</div>
                        <span>(1)</span>
                      </label>
                    </li>
                  </ul>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProductListCatagory;
