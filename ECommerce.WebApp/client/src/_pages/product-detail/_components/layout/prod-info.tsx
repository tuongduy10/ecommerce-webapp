/* eslint-disable jsx-a11y/anchor-has-content */
import { MenuItem, Select, SelectChangeEvent } from "@mui/material";
import { cloneDeep } from "lodash";
import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { ENV } from "src/_configs/enviroment.config";
import { INewProductInCart } from "src/_cores/_interfaces/product.interface";
import { showError, showSuccess, showWarning } from "src/_cores/_reducers/alert.reducer";
import { addToCart } from "src/_cores/_reducers/cart.reducer";
import { useCartStore, useProductStore } from "src/_cores/_store/root-store";
import { MuiIcon } from "src/_shares/_components";
import FullScreenDialog from "src/_shares/_components/dialog/fullscreen-dialog";
import { ICON_NAME } from "src/_shares/_components/mui-icon/_enums/mui-icon.enum";
import { ProductHelper } from "src/_shares/_helpers/product-helper";

const PRICE_TYPE = {
  'available': 'Có sẵn',
  'preOrder': 'Đặt trước',
}
const ProductDetailInfo = () => {
  const dispatch = useDispatch();
  const productStore = useProductStore();
  const cartStore = useCartStore();
  const productDetail = productStore.productDetail;

  const [selectedPriceType, setSelectedPriceType] = useState<string>('');
  const [quanity, setQuanity] = useState(1);
  const [options, setOptions] = useState<{ id: any, value: any }[]>([]);
  const [isShowCodeDis, setShowCodeDis] = useState(false);
  const [openBrandDes, setOpenBrandDes] = useState(false);

  useEffect(() => {
    if (productDetail) {
      const prices = [productDetail.priceAvailable, productDetail.pricePreOrder];
      if (prices.some(_ => !_)) {
        if (productDetail.priceAvailable) {
          setSelectedPriceType('available');
        }
        if (productDetail.pricePreOrder) {
          setSelectedPriceType('preOrder');
        }
      }
    }
  }, [productDetail]);

  const handleShowCodeDis = () => {
    setShowCodeDis(!isShowCodeDis);
  };

  const getFormatedPrice = (price: number) => {
    return ProductHelper.getFormatedPrice(price);
  }

  const onChangeOption = (event: SelectChangeEvent, option: any) => {
    const id = event.target.value;
    const idx = options.findIndex((_: any) => _.id === option.id);
    if (idx > -1) {
      setOptions(prevOptions => {
        [...prevOptions][idx].value = id;
        return [...prevOptions];
      });
    } else {
      const newOption = {
        id: option.id,
        value: id
      };
      setOptions(prevOptions => [...prevOptions, newOption]);
    }
  }

  const handleOnChangePriceType = (event: any) => {
    setSelectedPriceType(event.target.value);
  }

  const handleAddToCart = () => {
    if (productDetail) {
      const { id, name, priceAvailable, pricePreOrder, discountAvailable, discountPreOrder, imagePaths, shop } = productDetail;

      let price = null;
      let discount = null;
      if (selectedPriceType === 'available') {
        price = priceAvailable;
        discount = discountAvailable;
      } else if (selectedPriceType === 'preOrder') {
        price = pricePreOrder;
        discount = discountPreOrder;
      }

      const _options = cloneDeep(options).map((_: any) => {
        const idx = productDetail.options.findIndex(o => o.id === _.id);
        if (idx > -1) {
          const valueIdx = productDetail.options[idx].values.findIndex(v => v.id === _.value);
          if (valueIdx > -1) {
            _.valueName = productDetail.options[idx].values[valueIdx].name;
          }
        }
        return _;
      });

      // Check options selection
      if (productDetail.options.length > 0) {
        const optionNames:string[] = [];
        productDetail.options.forEach(_ => {
          const idx = cloneDeep(options).findIndex(o => o.id === _.id);
          if (idx === -1) {
            optionNames.push(_.name);
          }
        });
        if (optionNames.length > 0) {
          dispatch(showError(`Vui lòng chọn: ${optionNames.join(',')}`));
          return;
        }
      }
      // check prices selection
      if (!selectedPriceType) {
        dispatch(showError(`Vui lòng chọn giá`));
        return;
      }

      const newProductInCart = {
        id: id,
        name: name,
        shopName: shop?.name ?? '',
        qty: quanity,
        image: imagePaths[0],
        price: price,
        discount: discount,
        priceType: selectedPriceType,
        options: _options,
      } as INewProductInCart;
      dispatch(addToCart(newProductInCart));
      dispatch(showSuccess('Thêm vào giỏ hàng thành công'));
    }
  };

  const renderPrice = (type: 'available' | 'preOrder', price: number, discount?: number) => {
    return (
      <div className="option-price w-full flex mt-2 ">
        <div className="option-title">
          <input className="form-check-input" type="radio" value={type} onChange={handleOnChangePriceType} checked={selectedPriceType === type} />
          <label className="form-check-label">
            {PRICE_TYPE[type]}
          </label>
        </div>
        <label className="form-check-label flex items-center">
          {discount ? (<>
            <span className="price mr-2">{getFormatedPrice(discount)}</span>
            <span className="saleprice">
              <p className="line-through">{getFormatedPrice(price)}</p>
            </span>
          </>) : (
            <span className="price mr-2">{getFormatedPrice(price)}</span>
          )}
        </label>
      </div>
    )
  }

  const renderOptions = () => {
    return (
      productDetail && productDetail.options && productDetail.options.map((option) => (
        <div key={`option-${option.id}`} className="option-size flex mb-2 items-center">
          <div className="option-title">{option.name}</div>
          <div className="options-wrapper">
            {/* <select className="options form-select w-full pro-options md:h-[30px]" onChange={e => onChangeOption(e.target)}>
              <option disabled>- Chọn -</option>
              {option.values && option.values.map((value) => (
                <option key={`option-value-${value.id}`} value={value.id}>{value.name}</option>
              ))}
            </select> */}
            <Select
              displayEmpty
              className="options form-select w-full pro-options md:h-[30px]"
              inputProps={{ 'aria-label': 'Without label' }}
              sx={{
                boxShadow: "none",
                ".MuiOutlinedInput-notchedOutline": { border: 0 },
                "&.MuiOutlinedInput-root:hover .MuiOutlinedInput-notchedOutline":
                {
                  border: 0,
                },
                "&.MuiOutlinedInput-root.Mui-focused .MuiOutlinedInput-notchedOutline":
                {
                  border: 0,
                },
              }}
              value={options.find(_ => _.id === option.id)?.value || -1}
              onChange={e => onChangeOption(e, option)}
            >
              <MenuItem value={-1} disabled>
                <em>- Chọn -</em>
              </MenuItem>
              {option.values && option.values.map((value) => (
                <MenuItem key={`option-value-${value.id}`} value={value.id}>{value.name}</MenuItem>
              ))}
            </Select>
            {/* <MuiIcon
              name="CHEVRON_DOWN"
              className="feather feather-chevron-down"
            /> */}
          </div>
        </div>
      ))
    )
  }

  return (
    <div className="product__detail-card">
      {productDetail && (
        <div className="about">
          <div className="product__detail-id flex flex-wrap items-center">
            <a className="product-id flex">
              <span>Mã sản phẩm:</span>
              <span>{productDetail.code ?? ''}</span>
            </a>
            <button className="product-rating flex gap-2 mb-2 items-center">
              <div className="stars flex">
                {[...Array(5)].map((_, index) => (
                  <MuiIcon
                    key={index}
                    name={index < productDetail.review.avgValue ? ICON_NAME.FEATHER.STAR_FILLED : ICON_NAME.FEATHER.STAR}
                    sx={{ fontSize: '1.5rem important;' }}
                  />
                ))}
              </div>
              <u style={{ cursor: "pointer" }}>{productDetail.review.totalRating} Đánh giá</u>
            </button>
          </div>
          <div className="flex mb-4">
            <div className="w-6/12">
              <a className="product__detail-action" href="tel: 0935939401">
                <span className="icon mr-2">
                  <MuiIcon name={ICON_NAME.FEATHER.PHONE_PRODUCT_DETAIL} className="feather feather-phone" />
                </span>
                <span className="text text-[#707070]">0935939401</span>
              </a>
            </div>
            <div className="w-6/12">
              <a className="product__detail-action">
                <span className="icon mr-2">
                  <MuiIcon name={ICON_NAME.FEATHER.HEART} className="feather feather-heart" />
                </span>
                <span className="text text-[#707070]">Yêu Thích</span>
              </a>
            </div>
          </div>
          <div className="product__detail-name">{productDetail.name ?? ''}</div>
          <div className="product__detail-brand">
            <a>{productDetail.shop?.name ?? ''}</a>
          </div>
          <div className="product__detail-brand">
            <div className="flex items-center">
              <div className="mr-4" style={{ width: "130px", height: "auto" }}>
                <img
                  className="w-100 h-auto"
                  alt="Hình ảnh thương hiệu"
                  src={ENV.IMAGE_URL + "/brand/" + productDetail.brand.imagePath}
                />
              </div>
              <span
                className="hover-tag-a text-[#707070]"
                style={{
                  textDecoration: "underline",
                  cursor: "pointer",
                  fontSize: `calc(16px + (20 - 14) * ((100vw - 375px) / (1920 - 375)))`,
                }}
                onClick={() => setOpenBrandDes(true)}
              >
                {productDetail.brand?.descriptionTitle ?? ''}
              </span>
              <FullScreenDialog content={productDetail.brand?.description} isOpen={openBrandDes} onClose={() => setOpenBrandDes(false)} />
              {/* <span
                      style={{
                        fontSize:
                          "calc(16px + (20 - 14) * ((100vw - 375px)/ (1920 - 375)))",
                      }}
                    >
                      Tạm hết đến ngày ProductImportDate
                    </span> */}
            </div>
          </div>
          <div className="product__detail-desciption">
            <div className="bullets">
              {productDetail.isLegit ? (
                <div className="flex items-center mb-2">
                  <span className="mr-2">
                    <img src="/images/icon/medal.png" alt="" />
                  </span>
                  <span className="bullet-text">Sản phẩm chính hãng</span>
                </div>
              ) : ''}
              {productDetail.insurance && (
                <div className="flex items-center mb-2">
                  <span className="mr-2">
                    <img src="/images/icon/baohanh.png" alt="" />
                  </span>
                  <span className="bullet-text">{productDetail.insurance}</span>
                </div>
              )}
              {productDetail.repay && (
                <div className="flex items-center mb-2">
                  <span className="mr-2">
                    <img src="/images/icon/return.png" alt="" />
                  </span>
                  <span className="bullet-text">{productDetail.repay}</span>
                </div>
              )}
              {productDetail.delivery && (
                <div className="flex items-center mb-2">
                  <span className="mr-2">
                    <img src="/images/icon/giaohang.png" alt="" />
                  </span>
                  <span className="bullet-text">{productDetail.delivery}</span>
                </div>
              )}
            </div>
          </div>
          <hr />
          <div className="product__detail-bottom mt-4">
            {/* options */}
            <div className="product__detail-options">
              <div className="option-quality flex mb-2 items-center">
                <div className="option-title">Chọn số lượng</div>
                <input
                  className="text-center mr-2 md:h-[30px] pro-qty"
                  type="number"
                  value={quanity}
                  min="1"
                  max="999"
                  onChange={(e) => setQuanity(parseInt(e.target.value))}
                  step="1"
                />
              </div>
              {renderOptions()}
            </div>
            <div className="product__detail-price">
              {productDetail.discountPercent > 0 && (
                <span className="sales-value">Giảm {productDetail.discountPercent}%</span>
              )}

              {productDetail.pricePreOrder > 0 && (
                renderPrice('preOrder', productDetail.pricePreOrder, productDetail.discountPreOrder)
              )}

              {productDetail.priceAvailable > 0 && (
                renderPrice('available', productDetail.priceAvailable, productDetail.discountAvailable)
              )}

              {/* Discount */}
              {/*
                <div className="mt-2">
                  <div
                    className="product__sale-code w-full justify-between flex items-center"
                    onClick={handleShowCodeDis}
                  >
                    <span className="circle-left"></span>
                    <div className="product__sale-content">
                      <div className="text">
                        Nhập mã <strong>12345</strong> giảm 10%
                      </div>

                      <div className="text">
                        Nhập mã <strong>12345</strong> giảm {getFormatedPrice(10000)}
                      </div>
                    </div>
                    <MuiIcon
                      name="CHEVRON_DOWN"
                      className="feather feather-chevron-down"
                    />
                    <span className="circle-right"></span>
                  </div>
                  <div
                    className={`product__sale-bottom ${isShowCodeDis ? "" : "hidden"
                      }`}
                  >
                    <div className="flex items-center justify-between my-2 md:my-4">
                      <div className="text">
                        Nhập <strong className="salecode">code</strong> để giảm 10
                        % trên tổng hóa đơn
                      </div>

                      <div className="text">
                        Nhập{" "}
                        <strong className="salecode">
                          XINCHAO
                        </strong>{" "}
                        để giảm {getFormatedPrice(10000)} trên tổng hóa đơn
                      </div>
                      <button
                        type="button"
                        className="getcode"
                        style={{ whiteSpace: "nowrap", padding: "5px 4px" }}
                      >
                        Lấy mã
                      </button>
                    </div>
                  </div>
                </div>
                    */}

              <div className="product__detail-buttons flex justify-between w-full mt-4">
                <button className="btn-addtocart btn-black w-[49%] bg-[#333] text-[#fff]" onClick={handleAddToCart}>
                  Thêm vào giỏ
                </button>
                <button className="btn-buynow btn-black w-[49%] bg-[#333] text-[#fff]">
                  Mua ngay
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ProductDetailInfo;
