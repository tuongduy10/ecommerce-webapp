import { api } from "../_api/api";
import { BLOG_API_URL, COMMON_API_URL } from "../_enums/api-url.enum";

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

}