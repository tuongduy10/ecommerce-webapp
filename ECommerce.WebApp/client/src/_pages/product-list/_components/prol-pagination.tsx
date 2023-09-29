import { useProductStore } from "src/_cores/_store/root-store";
import { MuiIcon } from "src/_shares/_components";
import { ICON_NAME } from "src/_shares/_components/mui-icon/_enums/mui-icon.enum";

const ProlPagination = () => {
    const productStore = useProductStore();

    return (<>
        <div className="filter__number">
            <div className="text">Hiển thị: </div>
            <div className="number">
                <span>{productStore.param.currentRecord}</span>/<span className="totalItems">{productStore.param.totalRecord}</span>
            </div>
        </div>
        <div className="filter__pagination">
            <ul className="pagination">
                {productStore.param.totalPage > 3 && productStore.param.pageIndex > 2 && (
                    <li className="pagination-item pagination-item__page previous">
                        <button className="pagination-link pagination-item__page h-[24px]">
                            <MuiIcon name={ICON_NAME.FEATHER.CHEVRON_LEFT} />
                        </button>
                    </li>
                )}
                <li className={`pagination-item pagination-item__page`}>
                    <button className="pagination-link">{productStore.param.pageIndex === 1 ? 1 : productStore.param.pageIndex - 1}</button>
                </li>
                <li className={`pagination-item pagination-item__page ${productStore.param.pageIndex && 'active'}`}>
                    <button className="pagination-link">{productStore.param.pageIndex === 1 ? 2 : productStore.param.pageIndex}</button>
                </li>
                <li className={`pagination-item pagination-item__page`}>
                    <button className="pagination-link">{productStore.param.pageIndex === 1 ? 3 : productStore.param.pageIndex + 1}</button>
                </li>
                {productStore.param.totalPage > 3 && productStore.param.totalPage - productStore.param.pageIndex > 1 && (
                    <li className="pagination-item pagination-item__page next">
                        <button className="pagination-link h-[24px]">
                            <MuiIcon name={ICON_NAME.FEATHER.CHEVRON_RIGHT} />
                        </button>
                    </li>
                )}
            </ul>
        </div>
    </>)
}

export default ProlPagination;
