import { MuiIcon } from "src/_shares/_components";
import { ICON_NAME } from "src/_shares/_components/mui-icon/_enums/mui-icon.enum";

const ProlPagination = () => {
    return (<>
        <div className="filter__number">
            <div className="text">Hiển thị: </div>
            <div className="number">
                <span>9</span>/<span className="totalItems">9</span>
            </div>
        </div>
        <div className="filter__pagination">
            <ul className="pagination">
                <li value="@prevIndex" className="pagination-item pagination-item__page previous">
                    <button className="pagination-link pagination-item__page h-[24px]">
                        <MuiIcon name={ICON_NAME.FEATHER.CHEVRON_LEFT} />
                    </button>
                </li>
                <li value="@prevIndex" className="pagination-item pagination-item__page">
                    <button className="pagination-link">5</button>
                </li>
                <li className="pagination-item pagination-item__page active">
                    <button className="pagination-link">6</button>
                </li>
                <li value="@nextIndex" className="pagination-item pagination-item__page">
                    <button className="pagination-link">7</button>
                </li>
                <li value="@nextIndex" className="pagination-item pagination-item__page next">
                    <button className="pagination-link h-[24px]">
                        <MuiIcon name={ICON_NAME.FEATHER.CHEVRON_RIGHT} />
                    </button>
                </li>
            </ul>
        </div>
    </>)
}

export default ProlPagination;
