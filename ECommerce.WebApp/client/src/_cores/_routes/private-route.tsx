import { Navigate } from "react-router-dom"
import { ROUTE_NAME } from "../_enums/route-config.enum";
import SessionService from "../_services/session.service";

export const PrivateRoute = ({ children, ...rest }: any) => {
    const isAuthorized = SessionService.getAccessToken();
    if (!isAuthorized) {
        return <Navigate to={ROUTE_NAME.LOGIN} />
    }
    return children;
}