import { useState } from "react";
import { useDispatch } from "react-redux";
import { useSearchParams } from "react-router-dom";
import { setParam } from "src/_cores/_reducers/product.reducer";
import { useProductStore } from "src/_cores/_store/root-store";
import MuiIcon from "src/_shares/_components/mui-icon/mui-icon.component";

const ProductListFilter = () => {
  const items = [
    { text: 'Tất cả', value: '' },
    { text: 'Mẫu mới nhất', value: 'newest' },
    { text: 'Giá thấp - cao', value: 'asc' },
    { text: 'Giá cao - thấp', value: 'desc' },
    { text: 'Đang giảm giá %', value: 'discount' },
  ];

  const [selectedItem, setSelectedItem] = useState(items[0]);
  const [isOpenOrderList, setOpenOrderList] = useState(false);
  const [isOrderIconChanged, setOrderIconChanged] = useState(false);
  const [searchParams, setSearchParams] = useSearchParams();
  const updatedSearchParams = new URLSearchParams(searchParams.toString());

  const dispatch = useDispatch();
  const productStore = useProductStore();

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
            <button key={item.value} className="order__item text-left d-none" onClick={() => {
              setSelectedItem(item); 
              toggleOrderList();
              updatedSearchParams.set('orderBy', item.value);
              setSearchParams(updatedSearchParams);
              dispatch(setParam({
                ...productStore.param,
                orderBy: item.value
              }));
            }}>
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
