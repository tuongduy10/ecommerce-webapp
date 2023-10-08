import { api } from "../_api/api";
import { BRAND_API_URL, CATEGORY_API_URL, INVENTORY_API_URL } from "../_enums/api-url.enum";

export default class InventoryService {
    public static getBrands() { 
        return api.get(BRAND_API_URL.GET_ALL_AVAILABLE);
    }

    public static getBrandsInCategory(id: number)  {
        return api.get(BRAND_API_URL.GET_ALL_AVAILABLE_IN_CATEGORY + '/?id=' + id)
    }

    public static getCategories() {
        return api.get(CATEGORY_API_URL.GET_ALL);
    }

    public static getSubCategories(id: number) {
        return api.get(INVENTORY_API_URL.GET_SUB_CATEGORIES + `/${id}`);
    }
}