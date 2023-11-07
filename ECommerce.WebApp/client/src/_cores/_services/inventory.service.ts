import { api } from "../_api/api";
import { BRAND_API_URL, CATEGORY_API_URL, INVENTORY_API_URL } from "../_enums/api-url.enum";

export default class InventoryService {
    public static getBrands(params: any) { 
        return api.get(BRAND_API_URL.GET_ALL_AVAILABLE, params);
    }

    public static getBrand(id: number) { 
        return api.get(BRAND_API_URL.GET_BRAND + `/${id}`);
    }

    public static getBrandsInCategory(id: number)  {
        return api.get(BRAND_API_URL.GET_ALL_AVAILABLE_IN_CATEGORY + '/?id=' + id)
    }

    public static getCategories() {
        return api.get(CATEGORY_API_URL.GET_ALL);
    }

    public static getSubCategories(params: { brandId: number }) {
        return api.post(INVENTORY_API_URL.SUB_CATEGORIES, params);
    }
}