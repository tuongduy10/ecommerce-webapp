import { useState } from "react";
import MuiIcon from "src/_shared/_components/mui-icon/mui-icon.component";

const ProductListFilter = () => {
  const [isOpenOrderList, setOpenOrderList] = useState(false);
  const [isOrderIconChanged, setOrderIconChanged] = useState(false);

  const mouseEnter = () => {
    setOpenOrderList(true);
    setOrderIconChanged(true);
  };

  const mouseLeave = () => {
    setOpenOrderList(false);
    setOrderIconChanged(false);
  };

  const toggleOrderList = () => {
    setOpenOrderList(!isOpenOrderList);
    setOrderIconChanged(!isOrderIconChanged);
  };
  return (
    <div className="order__item-wrapper ">
      <div
        className="order__item-dropdown"
        onClick={toggleOrderList}
        onMouseEnter={mouseEnter}
        onMouseLeave={mouseLeave}
      >
        Tất cả
        <MuiIcon
          name="CHEVRON_DOWN"
          className={`feather feather-chevron-down svg-order ${
            isOrderIconChanged ? "svg-up" : ""
          }`}
        />
      </div>
      <div
        onMouseEnter={mouseEnter}
        onMouseLeave={mouseLeave}
        className={`order__item-dropdown_menu ${isOpenOrderList ? "open" : ""}`}
      >
        <div className="order__item-dropdown_items">
          <a href="/" className="order__item d-none">
            Tất cả
            <MuiIcon
              name="CHEVRON_RIGHT"
              className="feather feather-chevron-right"
            />
          </a>
          <a href="/" className="order__item orderby-newest">
            Mẫu mới nhất
            <MuiIcon
              name="CHEVRON_RIGHT"
              className="feather feather-chevron-right"
            />
          </a>
          <a href="/" className="order__item orderby-ascending">
            Giá thấp - cao
            <MuiIcon
              name="CHEVRON_RIGHT"
              className="feather feather-chevron-right"
            />
          </a>
          <a href="/" className="order__item orderby-descending">
            Giá cao - thấp
            <MuiIcon
              name="CHEVRON_RIGHT"
              className="feather feather-chevron-right"
            />
          </a>
          <a href="/" className="order__item orderby-discount">
            Đang giảm giá %
            <MuiIcon
              name="CHEVRON_RIGHT"
              className="feather feather-chevron-right"
            />
          </a>
        </div>
      </div>
    </div>
  );
};

export default ProductListFilter;
