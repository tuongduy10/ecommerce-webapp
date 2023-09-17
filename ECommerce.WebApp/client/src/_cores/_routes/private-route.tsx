import { Navigate } from "react-router-dom"
import { ADMIN_ROUTE_NAME } from "../_enums/route-config.enum";
import SessionService from "../_services/session.service";

export const PrivateRoute = ({ children, ...rest }: any) => {
    const isAuthorized = SessionService.getAccessToken();
    if (!isAuthorized) {
        return <Navigate to={ADMIN_ROUTE_NAME.LOGIN} />
    }
    return children;
}