import { api } from "../_api/api";
import {
  BRAND_API_URL,
  CATEGORY_API_URL,
  INVENTORY_API_URL,
} from "../_enums/api-url.enum";

export default class InventoryService {
  public static getBrands(params: any) {
    return api.post(BRAND_API_URL.GET_BRANDS, params);
  }

  public static getBrand(id: number) {
    return api.get(INVENTORY_API_URL.GET_BRAND + `/${id}`);
  }

  public static getBrandsInCategory(id: number) {
    return api.get(BRAND_API_URL.GET_ALL_AVAILABLE_IN_CATEGORY + "/?id=" + id);
  }

  public static getCategories() {
    return api.post(INVENTORY_API_URL.CATEGORIES);
  }

  public static getSubCategories(params?: { brandId: number }) {
    const _params = params ?? {};
    return api.post(INVENTORY_API_URL.SUB_CATEGORIES, _params);
  }
}
