import { useState } from "react";
import MuiIcon from "src/_shares/_components/mui-icon/mui-icon.component";
const ProductListCatagoryLgSize = () => {
  const [isExpanded, setIsExpanded] = useState(false);
  const [isFeatherIconChanged, setFeatherIconChanged] = useState(false);

  const toggleExpand = () => {
    setIsExpanded(!isExpanded);
    setFeatherIconChanged(!isFeatherIconChanged);
  };
  return (
    <div className="products__menu">
      <div className="product__filter">
        <div className="filter__block">
          <h3 className="filter__title" onClick={toggleExpand}>
            .SubCategoryName
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
              className={`feather feather-chevron-right ${
                isFeatherIconChanged ? "svg-right" : ""
              }`}
            >
              <polyline points="9 18 15 12 9 6"></polyline>
            </svg> */}
            <MuiIcon
              name="CHEVRON_RIGHT"
              className={`feather feather-chevron-right ${
                isFeatherIconChanged ? "svg-right" : ""
              }`}
            />
          </h3>

          <ul
            className="filter__list filter--scrollbar"
            style={{
              maxHeight: isExpanded
                ? "calc(185px + 125 * ((100vw - 375px) / 1545))"
                : "0px",
            }}
          >
            <li className="filter__item">
              <p className="mb-0">
                <MuiIcon name="ARROW_DROP_DOWN" />
                OptionName
              </p>
              <ul className="filter__sub-list">
                <li className="filter__sub-item">
                  <div className="checkbox flex items-center">
                    <input
                      id="check__sub-item_@productOption.OptionValueId"
                      className="checkboxFilter"
                      type="checkbox"
                    />
                    <label
                      htmlFor="check__sub-item_@productOption.OptionValueId"
                      className="flex items-center mb-0"
                    >
                      <div className="sub-item-cate">OptionValueName</div>
                      <span className="sub-item-cate">TotalRecord</span>
                    </label>
                  </div>
                </li>
              </ul>
            </li>
          </ul>
        </div>
      </div>
    </div>
  );
};

export default ProductListCatagoryLgSize;
