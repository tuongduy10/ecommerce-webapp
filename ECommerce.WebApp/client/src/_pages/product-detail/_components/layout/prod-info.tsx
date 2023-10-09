/* eslint-disable jsx-a11y/anchor-has-content */
import { useState } from "react";
import { useDispatch } from "react-redux";
import { useProductStore } from "src/_cores/_store/root-store";
import { MuiIcon } from "src/_shares/_components";
import { ICON_NAME } from "src/_shares/_components/mui-icon/_enums/mui-icon.enum";

/* eslint-disable jsx-a11y/anchor-is-valid */
const ProductDetailInfo = () => {
  const dispatch = useDispatch();
  const productStore = useProductStore();
  const productDetail = productStore.productDetail;

  const price = 500000;
  const salePrice = 400000;
  const formattedPrice = new Intl.NumberFormat("vi-VN", {
    style: "currency",
    currency: "VND",
  }).format(price);
  const formattedSalePrice = new Intl.NumberFormat("vi-VN", {
    style: "currency",
    currency: "VND",
  }).format(salePrice);
  const [quanity, setQuanity] = useState(1);
  const [isShowCodeDis, setShowCodeDis] = useState(false);

  const handleShowCodeDis = () => {
    setShowCodeDis(!isShowCodeDis);
  };

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
                    name={ICON_NAME.FEATHER.STAR}
                    className="checked"
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
                  src="https://hihichi.com/images/brand/brand_d5d8fb28-c399-4259-8a60-3deb4511a810.png"
                />
              </div>
              <a
                className="hover-tag-a text-[#707070]"
                style={{
                  textDecoration: "underline",
                  cursor: "pointer",
                  fontSize: `calc(16px + (20 - 14) * ((100vw - 375px) / (1920 - 375)))`,
                }}
              >
                {productDetail.brand?.descriptionTitle ?? ''}
              </a>
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
            {/* t options */}

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

              <div className="option-size flex mb-2 items-center">
                <div className="option-title">Option Name</div>
                <div className="options-wrapper">
                  <select className="options form-select w-full pro-options md:h-[30px]">
                    <option disabled>
                      - Chọn -
                    </option>

                    <option value="value">Value</option>
                  </select>
                  <MuiIcon
                    name="CHEVRON_DOWN"
                    className="feather feather-chevron-down"
                  />
                </div>
              </div>
            </div>
            <div className="product__detail-price">
              {/*  <span className="sales-value">Giảm 5%</span>
                   */}

              {/*  Available &  Price */}

              <div className="option-price flex mt-2 w-full">
                <div className="option-title">
                  <input
                    className="form-check-input"
                    type="radio"
                    name="pro-type"
                    id="inlineRadio1"
                    value="1"
                  />
                  <label className="form-check-label" htmlFor="inlineRadio1">
                    Có sẵn
                  </label>
                </div>
                <label
                  className="form-check-label flex items-center"
                  htmlFor="inlineRadio1"
                >
                  <span className="price mr-2">{formattedPrice}</span>
                  <span className="saleprice">
                    <p className="line-through">{formattedSalePrice}</p>
                  </span>
                </label>
              </div>

              {/*  Order & Price */}

              <div className="option-price w-full flex mt-2 ">
                <div className="option-title">
                  <input
                    className="form-check-input"
                    type="radio"
                    name="pro-type"
                    id="inlineRadio2"
                    value="2"
                  />
                  <label className="form-check-label" htmlFor="inlineRadio2">
                    Đặt trước
                  </label>
                </div>
                <label
                  className="form-check-label flex items-center"
                  htmlFor="inlineRadio2"
                >
                  <span className="price mr-2">{formattedPrice}</span>
                  <span className="saleprice">
                    <p className="line-through">{formattedSalePrice}</p>
                  </span>
                </label>
              </div>

              {/* Discount */}

              {
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

                      {/*  <div className="text">
                              Nhập mã <strong>12345</strong> giảm {formattedSalePrice}
                            </div> */}
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

                      {/* <div className="text">
                              Nhập{" "}
                              <strong className="salecode">
                                @Model.discount.DiscountCode
                              </strong>{" "}
                              để giảm {formattedSalePrice} trên tổng hóa đơn
                            </div> */}
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
              }

              {/*  Button */}

              <div className="product__detail-buttons flex justify-between w-full mt-4">
                <button
                  className="btn-addtocart btn-black"
                  style={{
                    width: "49%",
                    backgroundColor: "#333",
                    color: "#fff",
                  }}
                >
                  Thêm vào giỏ
                </button>
                <button
                  className="btn-buynow btn-black"
                  style={{
                    width: "49%",
                    backgroundColor: "#333",
                    color: "#fff",
                  }}
                >
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
