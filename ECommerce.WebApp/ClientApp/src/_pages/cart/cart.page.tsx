import { useState } from "react";
import { WebDirectional } from "src/_shared/_components";
import MuiIcon from "src/_shared/_components/mui-icon/mui-icon.component";

const CartPage = () => {
  const [quantity, setQuantity] = useState(1);

  return (
    <div className="custom-container">
      <div className="content__inner">
        <WebDirectional items={[
          { path: '/', name: 'Giỏ Hàng' }
        ]} />

        <div className="card-content">
          <div className="cart-title text-center ">GIỎ HÀNG</div>

          {/* No product */}
          {/* <div className="cart-body flex flex-wrap">
            <div className="primary-cartcontent">
              <div className="cart-bottom flex justify-center lg:justify-start">
                <button
                  className=" py-2 px-4 sm:w-full lg:w-48 bg-[#4D4D4D] text-white
                       border border-solid border-black inline-block text-center
                       hover:bg-[#fff] hover:text-gray-700  transition duration-500"
                >
                  <strong>Tiếp tục mua sắm</strong>
                </button>
              </div>
              <div className="text-center text-gray-500 mt-4">
                Chưa có sản phẩm
              </div>
            </div>
          </div> */}

          {/* has product */}

          <div className="cart-head">
            <span>
              <strong>Sản phẩm</strong>
            </span>
            <span className="">
              <strong>Số lượng</strong>
            </span>
            <span className="">
              <strong>Thành tiền</strong>
            </span>
          </div>
          <div className="cart-products">
            <div className="cart-product flex --tooltip-text flex-wrap w-full">
              <div className="cart-product__image">
                <img
                  src="https://hihichi.com/images/products/product_c048ab77-33c9-49e6-89e1-051dfb8a9671.jpg"
                  alt=""
                />
              </div>
              <div className="cart-product__info">
                <div className="cart-product--auto">
                  <div>
                    <strong>Name</strong>
                  </div>
                  <div>Shop Demo 02</div>
                  <div>()</div>
                  <div className="price flex gap-1">
                    <div className="value line-through">350.000</div>
                    <span>₫</span>
                  </div>
                  <div className="price flex gap-1">
                    <div className="value">350.000</div>
                    <span>₫</span>
                  </div>
                </div>
                <div className="cart-product--col">
                  <input
                    className="cart-product__amount"
                    type="number"
                    onChange={(e) => setQuantity(parseInt(e.target.value))}
                    value={quantity}
                    min="1"
                    step="1"
                  />
                </div>
                <div className="cart-product--col totalprice">
                  <span className="total-value" id="total">
                    100.000
                  </span>
                  <span>₫</span>
                </div>
              </div>
              <span className="cart-product__remove flex">
                <MuiIcon name="X" />
              </span>
            </div>
          </div>
          <button
            className="py-2 px-4 w-full lg:w-48 bg-[#4D4D4D] text-white
                       border border-solid border-black inline-block text-center
                       hover:bg-[#fff] hover:text-gray-700  transition duration-500"
          >
            <strong>Tiếp tục mua sắm</strong>
          </button>
        </div>
      </div>
    </div>
  );
};

export default CartPage;
