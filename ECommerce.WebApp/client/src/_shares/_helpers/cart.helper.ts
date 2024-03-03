import { INewProductInCart } from "src/_cores/_interfaces/product.interface";

export class CartHelper {
    public static getUniqId(product: INewProductInCart) {
        const _options = product.options.map(_ => ({
            id: _.id,
            value: _.value
        }))
        return `${product.id}_${product.price}_${product.discount}_${product.priceType}_${JSON.stringify(_options)}`;
    }
}