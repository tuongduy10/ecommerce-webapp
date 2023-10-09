import { useProductStore } from "src/_cores/_store/root-store"

export default function DetailInfo(props: any) {
    const productStore = useProductStore();
    const productDetail = productStore.productDetail;
    return (
        <div className="product-detail">
            <ul className="detail-list flex mb-4 flex-wrap justify-between">
                {(productDetail?.attributes.length ?? 0) > 0 && productDetail?.attributes.map(
                    (attribute: any) => attribute.value ? (
                        <li key={`attr-${attribute.id}`} className="detail-item">
                            <span>
                                <b>{attribute.name}</b>
                            </span>
                            <span>{attribute.value}</span>
                        </li>
                    ) : null
                )}
            </ul>
            <div className="title">Mô tả ngắn</div>
            <div className="description"></div>
        </div>
    )
}