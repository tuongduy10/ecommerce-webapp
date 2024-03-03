import { useState } from "react";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { ENV } from "src/_configs/enviroment.config";
import { ROUTE_NAME } from "src/_cores/_enums/route-config.enum";
import { changeItemQty, removeItem } from "src/_cores/_reducers/cart.reducer";
import { useCartStore } from "src/_cores/_store/root-store";
import { WebDirectional } from "src/_shares/_components";
import { ICON_NAME } from "src/_shares/_components/mui-icon/_enums/mui-icon.enum";
import MuiIcon from "src/_shares/_components/mui-icon/mui-icon.component";
import { ProductHelper } from "src/_shares/_helpers/product-helper";

const CartPage = () => {
  const cartStore = useCartStore();
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const backToHome = () => {
    navigate(-1)
  }

  const removeCartItem = (uniqId: string) => {
    dispatch(removeItem(uniqId));
  }

  const changeQty = (uniqId: string, qty: number) => {
    dispatch(changeItemQty({ uniqId: uniqId, qty: qty }));
  }

  const getFormatedPrice = (price: number) => {
    return ProductHelper.getFormatedPrice(price);
  }

  const renderPrice = (price: number, discount?: number) => {
    return discount
      ? (<>
        <div className="price flex gap-1">
          <div className="value line-through">{getFormatedPrice(price)}</div>
        </div>
        <div className="price flex gap-1">
          <div className="value">{getFormatedPrice(discount)}</div>
        </div>
      </>) : (<>
        <div className="price flex gap-1">
          <div className="value line-through"></div>
        </div>
        <div className="price flex gap-1">
          <div className="value">{getFormatedPrice(price)}</div>
        </div>
      </>);
  }

  return (
    <div className="custom-container">
      <div className="content__inner">
        <WebDirectional items={[
          { path: '/cart', name: 'Giỏ Hàng' }
        ]} />

        <div className="card-content">
          <div className="cart-title text-center">GIỎ HÀNG</div>
          {cartStore.productsInCart.length === 0 ? (
            <div className="cart-body flex flex-wrap">
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
            </div>
          ) : (
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
                  {cartStore.productsInCart.map(_ => (
                    <div key={_.uniqId} className="cart-product flex --tooltip-text flex-wrap w-full">
                      <div className="cart-product__image">
                        <img
                          src={`${ENV.IMAGE_URL}/products/${_.image}`}
                          alt=""
                        />
                      </div>
                      <div className="cart-product__info">
                        <div className="cart-product--auto">
                          <div>
                            <strong>{_.name}</strong>
                          </div>
                          <div>{_.shopName}</div>
                          {_.options.length > 0 && (
                            <div>({_.options.map((o: any) => o.valueName).join(', ')})</div>
                          )}
                          {renderPrice(_.price, _.discount)}
                        </div>
                        <div className="cart-product--col">
                          <input
                            className="cart-product__amount"
                            type="number"
                            onChange={(e) => changeQty(_.uniqId, parseInt(e.target.value))}
                            value={_.qty}
                            min="1"
                            step="1"
                          />
                        </div>
                        <div className="cart-product--col totalprice">
                          <span className="total-value" id="total">
                            {getFormatedPrice((_.discount ?? _.price) * _.qty)}
                          </span>
                        </div>
                      </div>
                      <span className="cart-product__remove flex">
                        <MuiIcon
                          name={ICON_NAME.FEATHER.X}
                          onClick={() => removeCartItem(_.uniqId)}
                        />
                      </span>
                    </div>
                  ))}
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
                    <span>{cartStore.totalQty}</span>
                  </div>
                  <div className="flex justify-between mb-2">
                    <span>Tạm tính:</span>
                    <span>{getFormatedPrice(cartStore.totalPrice)}</span>
                  </div>
                  <hr className="mb-4" />
                  <div className="flex justify-between mb-4">
                    <span>TỔNG CỘNG:</span>
                    <span>
                      <strong>{getFormatedPrice(cartStore.totalPrice)}</strong>
                    </span>
                  </div>
                  <button className="w-full checkout btn-black border-['1px solid #333']" onClick={() => navigate({ pathname: ROUTE_NAME.PAYMENT })}>
                    <strong>THANH TOÁN</strong>
                  </button>
                </div>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default CartPage;
