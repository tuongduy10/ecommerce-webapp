import { Navigate } from "react-router-dom"
import { ADMIN_ROUTE_NAME } from "../_enums/route-config.enum";
import SessionService from "../_services/session.service";
import { useEffect } from "react";
import UserService from "../_services/user.service";
import { useUserStore } from "../_store/root-store";
import { useDispatch } from "react-redux";
import { setUser } from "../_reducers/user.reducer";

export const PrivateRoute = ({ children, ...rest }: any) => {
    const isAuthorized = SessionService.getAccessToken();
    if (!isAuthorized) {
        return <Navigate to={ADMIN_ROUTE_NAME.LOGIN} />
    }
    return children;
}