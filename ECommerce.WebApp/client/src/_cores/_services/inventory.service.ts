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

  public static getCategory(id: number) {
    return api.get(INVENTORY_API_URL.CATEGORY + `/${id}`);
  }

  public static updateCategory(param: { id: number; name: string }) {
    return api.post(INVENTORY_API_URL.UPDATE_CATEGORY, param);
  }

  public static addCategory(param: { name: string }) {
    return api.post(INVENTORY_API_URL.ADD_CATEGORY, param);
  }

  public static getSubCategories(params?: {
    brandId?: number;
    subCategoryId?: number;
  }) {
    const _params = params ?? {};
    return api.post(INVENTORY_API_URL.SUB_CATEGORIES, _params);
  }

  public static updateSubCategory(param: any) {
    return api.post(INVENTORY_API_URL.UPDATE_SUB_CATEGORY, param);
  }

  public static addSubCategory(param: any) {
    return api.post(INVENTORY_API_URL.ADD_SUB_CATEGORY, param);
  }

  public static getAttributes(param?: any) {
    return api.post(INVENTORY_API_URL.ATTRIBUTES, { id: param?.id });
  }

  public static saveAttributes(params: any) {
    return api.post(INVENTORY_API_URL.SAVE_ATTRIBUTES, params);
  }

  public static getOptions(params?: any) {
    return api.post(INVENTORY_API_URL.OPTIONS, { id: params?.id });
  }

  public static saveOptions(params: any) {
    return api.post(INVENTORY_API_URL.SAVE_OPTIONS, params);
  }
}
