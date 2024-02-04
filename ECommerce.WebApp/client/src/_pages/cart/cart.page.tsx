import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { ROUTE_NAME } from "src/_cores/_enums/route-config.enum";
import { WebDirectional } from "src/_shares/_components";
import { ICON_NAME } from "src/_shares/_components/mui-icon/_enums/mui-icon.enum";
import MuiIcon from "src/_shares/_components/mui-icon/mui-icon.component";

const CartPage = () => {
  const navigate = useNavigate();
  const [quantity, setQuantity] = useState(1);

  const backToHome = () => {
    navigate({
      pathname: ROUTE_NAME.HOME
    })
  }

  return (
    <div className="custom-container">
      <div className="content__inner">
        <WebDirectional items={[
          { path: '/cart', name: 'Giỏ Hàng' }
        ]} />

        <div className="card-content">
          <div className="cart-title text-center">GIỎ HÀNG</div>
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
          <div className="cart-body flex flex-wrap">
            <div className="primary-cartcontent">
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
                    <MuiIcon name={ICON_NAME.FEATHER.X} />
                  </span>
                </div>
              </div>
              <button
                className="py-2 px-4 w-full lg:w-48 bg-[#4D4D4D] text-white
                       border border-solid border-black inline-block text-center
                       hover:bg-[#fff] hover:text-gray-700  transition duration-500"
                onClick={backToHome}
              >
                <strong>Tiếp tục mua sắm</strong>
              </button>
            </div>
            <div className="secondary-cartcontent">
              <div className="cart-head mb-0" style={{ visibility: 'hidden' }}>
                <span className="text-center">ĐƠN HÀNG CỦA BẠN</span>
              </div>
              <div className="order-summary">
                <div className="order-summary-title text-center">
                  <span>ĐƠN HÀNG CỦA BẠN</span>
                </div>
                <div className="payment-coupon">
                  <div className="flex">
                    <div className="input-coupon">
                      <input type="text" placeholder="MÃ GIẢM GIÁ" />
                    </div>
                    <button className="submit-coupon btn-black border-['1px solid #333']">
                      <strong>ÁP DỤNG</strong>
                    </button>
                  </div>
                </div>
                <div className="flex justify-between mb-2">
                  <span>Số lượng:</span>
                  <span>3</span>
                </div>
                <div className="flex justify-between mb-2">
                  <span>Tạm tính:</span>
                  <span>100000000 ₫</span>
                </div>
                <hr className="mb-4" />
                <div className="flex justify-between mb-4">
                  <span>TỔNG CỘNG:</span>
                  <span>
                    <strong>100000000 ₫</strong>
                  </span>
                </div>
                <button className="w-full checkout btn-black border-['1px solid #333']" onClick={() => navigate({ pathname: ROUTE_NAME.PAYMENT })}>
                  <strong>THANH TOÁN</strong>
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CartPage;
