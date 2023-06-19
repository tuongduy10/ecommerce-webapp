interface IWebDirectionalItem {
    path: string,
    name: string
}

const WebDirectional = (props: { items: IWebDirectionalItem[] }) => {

    return (
        <ul className="web__directional">
            <li>
                <a href="/">Trang chủ</a>
            </li>
            {props.items && props.items.length > 0 ? (
                props.items.map(item => (
                    <li key={`direct-${item.path}`}>
                        <a href={item.path}>{item.name}</a>
                    </li>
                ))
            ) : null}
        </ul>
    )
}

export default WebDirectional;