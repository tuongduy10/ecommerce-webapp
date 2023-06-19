/* eslint-disable jsx-a11y/anchor-is-valid */
import { useState } from "react";
import MuiIcon from "../mui-icon/mui-icon.component";
const ProductItem = () => {
  const [isFillHeart, setFillHeart] = useState(false);
  const toggleFill = () => {
    setFillHeart(!isFillHeart);
  };
  return (
    <>
      <p className="product__grid-title text-center">Tất cả sản phẩm</p>
      <div className="product__grid-inner w-100 flex flex-wrap">
        <div className="product product-3 ">
          <div
            className="product-label lg:mt-1 flex justify-end items-center w-full bg-[#fff]"
            style={{ padding: "3px 0", position: "unset" }}
          >
            {/* <span className="label-sales">-@product.DiscountPercent%</span> */}

            <span className="label-new">Mới</span>

            <span className="label-hot">Hot</span>
            <a className="product-heart ">
              <MuiIcon
                name="HEART"
                onClick={toggleFill}
                className={`add-to-wishlist feather feather-heart cursor-pointer ${
                  isFillHeart ? "fill" : ""
                }`}
              />
            </a>
          </div>
          <div className="product-img cursor-pointer sm:mt-6">
            <img
              src="https://hihichi.com/images/products/product_be147d8e-feb3-4c0f-9a0b-c1a779568d52.jpeg"
              alt=""
            />
          </div>
          <hr
            style={{
              width: "50%",
              height: "1px",
              margin: "0 auto",
              backgroundColor: "#ddd",
            }}
          />
          <div className="product__info">
            <div className="product__info-detail text-center">
              <div className="product-brand align-items-center">ShopName</div>
              <div className="product-name mb-0">ProductName</div>

              {/* <div className="product-name">
                Tạm hết đến @product.ProductImportDate.ToString("dd/MM/yyyy")
              </div> */}
              {<div className="product-name">ProductTypeName</div>}
              {/*  <div className="product-subprice" style="visibility: hidden">@String.Format("{0:0,0} ₫", product.Price)</div>
                                                        <div className="product-price">@String.Format("{0:0,0} ₫", product.Price)</div> */}
              <div className="product-subprice">600.000</div>
              <div className="product-price">400.000</div>
              <div className="product-btn">
                <a href="/">Xem nhanh</a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default ProductItem;
