import { useState } from "react";
import MuiIcon from "src/_shares/_components/mui-icon/mui-icon.component";

const ProlCategory = () => {
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
            Máy pha cà phê
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
                Màu sắc
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
                      <div className="sub-item-cate">Tím</div>
                      <span className="sub-item-cate">(10)</span>
                    </label>
                  </div>
                </li>
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
                      <div className="sub-item-cate">Tím</div>
                      <span className="sub-item-cate">(10)</span>
                    </label>
                  </div>
                </li>
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
                      <div className="sub-item-cate">Tím</div>
                      <span className="sub-item-cate">(10)</span>
                    </label>
                  </div>
                </li>
              </ul>
            </li>
            <li className="filter__item">
              <p className="mb-0">
                Kích thước
              </p>
              <ul className="filter__sub-list">
                <li className="filter__sub-item">
                  <div className="checkbox flex items-center">
                    <input
                      id="sub1"
                      className="checkboxFilter"
                      type="checkbox"
                    />
                    <label
                      htmlFor="sub1"
                      className="flex items-center mb-0"
                    >
                      <div className="sub-item-cate">To</div>
                      <span className="sub-item-cate">(10)</span>
                    </label>
                  </div>
                </li>
                <li className="filter__sub-item">
                  <div className="checkbox flex items-center">
                    <input
                      id="sub2"
                      className="checkboxFilter"
                      type="checkbox"
                    />
                    <label
                      htmlFor="sub2"
                      className="flex items-center mb-0"
                    >
                      <div className="sub-item-cate">Vừa</div>
                      <span className="sub-item-cate">(10)</span>
                    </label>
                  </div>
                </li>
                <li className="filter__sub-item">
                  <div className="checkbox flex items-center">
                    <input
                      id="sub3"
                      className="checkboxFilter"
                      type="checkbox"
                    />
                    <label
                      htmlFor="sub3"
                      className="flex items-center mb-0"
                    >
                      <div className="sub-item-cate">Nhỏ</div>
                      <span className="sub-item-cate">(10)</span>
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

export default ProlCategory;
