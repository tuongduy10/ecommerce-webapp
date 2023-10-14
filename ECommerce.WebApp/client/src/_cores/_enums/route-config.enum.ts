export const ROUTE_PREFIX = '/v2';
export const ROUTE_NAME = {
    HOME: ROUTE_PREFIX + '',
    LOGIN: ROUTE_PREFIX + '/login',
    PRODUCT_LIST: ROUTE_PREFIX + '/product-list',
    PRODUCT_DETAIL: ROUTE_PREFIX + '/product-detail',
    EXAMPLE: ROUTE_PREFIX + '/example',
    USER_PROFILE: ROUTE_PREFIX + '/user-profile',
    CART: ROUTE_PREFIX + '/cart',
    BLOG: ROUTE_PREFIX + '/blog',
}

export const ADMIN_ROUTE_PREFIX = '/v2/admin';
export const ADMIN_ROUTE_NAME = {
    DASHBOARD: ADMIN_ROUTE_PREFIX + '/',
    LOGIN: ADMIN_ROUTE_PREFIX + '/login',
    MANAGE_PRODUCT: ADMIN_ROUTE_PREFIX + '/manage-product',
    MANAGE_USER: ADMIN_ROUTE_PREFIX + '/manage-user',
    MANAGE_USER_ADD: ADMIN_ROUTE_PREFIX + '/manage-user/add',
    MANAGE_USER_DETAIL: ADMIN_ROUTE_PREFIX + '/manage-user/detail',
}