import { Link } from "react-router-dom";
import { ROUTE_NAME } from "src/_cores/_enums/route-config.enum";

interface IWebDirectionalItem {
    path: string,
    name: string
}

const WebDirectional = (props: { items: IWebDirectionalItem[] }) => {

    return (
        <ul className="web__directional">
            <li>
                <Link to={ROUTE_NAME.HOME}>Trang chủ</Link>
            </li>
            {props.items && props.items.length > 0 ? (
                props.items.map(item => (
                    item.name && (
                        <li key={`direct-${item.path}`}>
                            <Link to={item.path}>{item.name}</Link>
                        </li>
                    )
                ))
            ) : null}
        </ul>
    )
}

export default WebDirectional;