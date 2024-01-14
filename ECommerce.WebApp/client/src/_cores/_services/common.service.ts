import { api } from "../_api/api";
import { BLOG_API_URL, COMMON_API_URL } from "../_enums/api-url.enum";
import { ICity, IDistrict, IWard } from "../_interfaces/common.interface";

export default class CommonService { 

    public static getData() {
        return api.get(COMMON_API_URL.GET_DATA);
    }

    public static postData(params: any) {
        return api.post('url', params);
    }

    public static getBlog(param: any) {
        return api.post(BLOG_API_URL.GET_BLOG, param);
    }

    public static uploadFiles(params: any) {
        return api.postForm(COMMON_API_URL.UPLOAD_FILES, params)
    }

    public static removeFiles(params: { fileNames: string[], uploadType: "products" | "brand" | "rates" }) {
        return api.post(COMMON_API_URL.REMOVE_FILES, params);
    }

    public static getCities() {
        return api.get<ICity[]>(COMMON_API_URL.PROVINCES);
    }

    public static getDistrictsByCityCode(code: string) {
        const params = {
            provinceCode: code
        }
        return api.post<IDistrict[]>(COMMON_API_URL.DISTRICTS, params);
    }

    public static getWardsByDistrictCode(code: string) {
        const params = {
            districtCode: code
        }
        return api.post<IWard[]>(COMMON_API_URL.WARDS, params);
    }
}