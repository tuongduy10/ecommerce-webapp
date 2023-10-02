import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { setPageIndex, setParam } from "src/_cores/_reducers/product.reducer";
import { useProductStore } from "src/_cores/_store/root-store";
import { MuiIcon } from "src/_shares/_components";
import { ICON_NAME } from "src/_shares/_components/mui-icon/_enums/mui-icon.enum";

const ProlPagination = () => {
  const dispatch = useDispatch();
  const productStore = useProductStore();

  const [showPrev, setShowPrev] = useState(false);
  const [showNext, setShowNext] = useState(false);
  const [isFirstIndexes, setIsFirstIndexes] = useState(false);
  const [isLastIndexes, setIsLastIndexes] = useState(false);

  useEffect(() => {
    setShowPrev(productStore.param.pageIndex > 2);
    setShowNext(productStore.param.pageIndex < productStore.param.totalPage - 1);
    setIsFirstIndexes(productStore.param.pageIndex < 3);
    setIsLastIndexes(productStore.param.totalPage - productStore.param.pageIndex < 3);
  }, [productStore.param.pageIndex]);

  const changePage = (pageIndex: number) => {
    dispatch(setPageIndex(pageIndex));
  }

  const generatePaginationItems = (start: number, end: number) => {
    const items = [];
    for (let i = start; i <= end; i++) {
      items.push(
        <li key={i} className={`pagination-item pagination-item__page ${productStore.param.pageIndex === i && 'active'}`}>
          <button className="pagination-link" onClick={() => changePage(i)}>
            {i}
          </button>
        </li>
      );
    }
    return items;
  };

  const renderPagination = () => {
    if (!isFirstIndexes && !isLastIndexes) {
      return generatePaginationItems(productStore.param.pageIndex - 1, productStore.param.pageIndex + 1);
    } else if (isFirstIndexes) {
      return generatePaginationItems(1, 3);
    } else {
      const start = productStore.param.totalPage - 2;
      const end = productStore.param.totalPage;
      return generatePaginationItems(start, end);
    }
  };

  return (<>
    <div className="filter__number">
      <div className="text">Hiển thị: </div>
      <div className="number">
        <span>{productStore.param.currentRecord}</span>/<span className="totalItems">{productStore.param.totalRecord}</span>
      </div>
    </div>
    <div className="filter__pagination">
      <ul className="pagination">
        {showPrev && (
          <li className="pagination-item pagination-item__page previous">
            <button className="pagination-link pagination-item__page h-[24px]">
              <MuiIcon name={ICON_NAME.FEATHER.CHEVRON_LEFT} />
            </button>
          </li>
        )}
        {productStore.param.totalPage > 1 && renderPagination()}
        {showNext && (
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
