import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { Link } from "react-router-dom";
import { ENV } from "src/_configs/enviroment.config";
import { ROUTE_NAME } from "src/_cores/_enums/route-config.enum";
import ProductService from "src/_cores/_services/product.service";
import { useCartStore } from "src/_cores/_store/root-store";
import { ProductHelper } from "src/_shares/_helpers/product-helper";
import MuiIcon from "../mui-icon/mui-icon.component";
import { ICON_NAME } from "../mui-icon/_enums/mui-icon.enum";
import { removeItem } from "src/_cores/_reducers/cart.reducer";

export default function MiniCart() {
    const [products, setProducts] = useState<any>([]);
    const cartStore = useCartStore();
    const dispatch = useDispatch();

    useEffect(() => {
        getProducts();
    }, [cartStore.totalQty]);

    const getProducts = async () => {
        if (cartStore.productsInCart.length > 0) {
            const params = {
                ids: cartStore.productsInCart.map(_ => _.id)
            }
            const response = await ProductService.getProductList(params) as any;
            if (response.isSucceed) {
                setProducts(response.data?.items);
            }
        }
    }

    const removeCartItem = (uniqId: string) => {
        dispatch(removeItem(uniqId));
    }

    const getFormatedPrice = (price: number) => {
        return ProductHelper.getFormatedPrice(price);
    }

    const renderPrice = (price: number, discount?: number) => {
        return discount
            ? (<>
                <div className="price line-through">{getFormatedPrice(price)}</div>
                <div className="price">{getFormatedPrice(discount)}</div>
            </>) : (<>
                <div className="price line-through"></div>
                <div className="price">{getFormatedPrice(price)}</div>
            </>)
    }

    return (
        <div className="minicart-content">
            <div className="w-full minicart-quantity">
                <div className="header-title pb-4 pt-3 minicart-totalquantity">Giỏ hàng: {cartStore.totalQty}</div>
            </div>
            <div className="minicart-products filter--scrollbar px-4">
                {cartStore.productsInCart.length > 0 && (
                    cartStore.productsInCart.map(_ => (
                        <div key={_.uniqId} className="minicart-product flex py-4">
                            <div className="minicart__product-image mr-2">
                                <img className="h-full w-full" src={`${ENV.IMAGE_URL}/products/${_.image}`} alt="" />
                            </div>
                            <div className="minicart__product-info flex flex-col">
                                <div className="name">
                                    <strong>{_.name}</strong>
                                </div>
                                {_.options.length > 0 && (
                                    <div className="detail mb-auto">{_.options.map((o: any) => o.valueName).join(', ')}</div>
                                )}
                                <div className="amount">x{_.qty}</div>
                                {renderPrice(_.price, _.discount)}
                                <div className="total-price product-totalprice">
                                    {getFormatedPrice((_.discount ?? _.price) * _.qty)}
                                </div>
                            </div>
                            <span className="minicart-remove">
                                <MuiIcon
                                    name={ICON_NAME.FEATHER.X}
                                    onClick={() => removeCartItem(_.uniqId)}
                                />
                            </span>
                        </div>
                    ))
                )}
            </div>
            <div className="minicart-totals px-4 pb-3">
                <div className="flex justify-between pt-3 pb-4">
                    <div>Tạm tính</div>
                    <div className="total-price cart-totalprice"><strong>{getFormatedPrice(cartStore.totalPrice)}</strong></div>
                </div>
                <div className="text-center block">
                    <Link to={ROUTE_NAME.PAYMENT} className="minicart-checkout inline-block w-full py-3 mb-4 btn-black">THANH TOÁN</Link>
                    <Link to={ROUTE_NAME.CART}><u>Xem / Chỉnh sửa</u></Link>
                </div>
            </div>
        </div>
    )
}