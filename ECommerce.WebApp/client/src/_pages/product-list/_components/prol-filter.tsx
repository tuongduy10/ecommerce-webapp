import { useState } from "react";
import MuiIcon from "src/_shares/_components/mui-icon/mui-icon.component";

const ProductListFilter = () => {
  const items = [
    { text: 'Tất cả', value: '' },
    { text: 'Mẫu mới nhất', value: 'orderby-newest' },
    { text: 'Giá thấp - cao', value: 'orderby-ascending' },
    { text: 'Giá cao - thấp', value: 'orderby-descending' },
    { text: 'Đang giảm giá %', value: 'orderby-discount' },
  ];

  const [selectedItem, setSelectedItem] = useState(items[0]);
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
        {selectedItem.text}
        <MuiIcon
          name="CHEVRON_DOWN"
          className={`feather feather-chevron-down svg-order ${isOrderIconChanged ? "svg-up" : ""
            }`}
        />
      </div>
      <div
        onMouseEnter={mouseEnter}
        onMouseLeave={mouseLeave}
        className={`order__item-dropdown_menu ${isOpenOrderList ? "open" : ""}`}
      >
        <div className="order__item-dropdown_items">
          {items.map(item => (
            <button key={item.value} className="order__item text-left d-none" onClick={() => { setSelectedItem(item); toggleOrderList() }}>
              {item.text}
              <MuiIcon
                name="CHEVRON_RIGHT"
                className="feather feather-chevron-right"
              />
            </button>
          ))}
        </div>
      </div>
    </div>
  );
};

export default ProductListFilter;
