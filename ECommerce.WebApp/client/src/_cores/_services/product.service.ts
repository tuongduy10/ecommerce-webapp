import { api } from "../_api/api";
import { PRODUCT_API_URL } from "../_enums/api-url.enum";

export default class ProductService {

    public static getProductManagedList(param: any) { 
        return api.post(PRODUCT_API_URL.PRODUCT_MANAGED_LIST, param);
    }

    public static getProductList(param: any) { 
        return api.post(PRODUCT_API_URL.PRODUCT_LIST, param);
    }

    public static getProductDetail(_id: number) { 
        return api.get(PRODUCT_API_URL.PRODUCT_DETAIL + "/" + _id );
    }

}