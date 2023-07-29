import ProductItem from "src/_shares/_components/product-item/product-item.component";
import ProductListCatagory from "./_components/product-list-catagory.component";
import ProductListFilter from "./_components/product-list-filter.component";
import ProductListCatagoryLgSize from "./_components/product-list-catagory-lg-size.component";
import ProductListPageNumber from "./_components/product-list-page-number.component";
import { WebDirectional } from "src/_shares/_components";

const ProductListPage = () => {
  return (
    <div className="custom-container content__wrapper products__content-wrapper">
      <div>
        <div className="products__content flex justify-center  mt-4">
          <div className="">
            <ProductListCatagoryLgSize />
          </div>
          <div className="products__list ">
            <div className="filter__control-top grid grid-cols-2">
              <ProductListFilter />
              <ProductListCatagory />
              <ProductListPageNumber />
            </div>
            <p className="product__grid-title text-center">Tất cả sản phẩm</p>
            <div className="product__grid-wrapper">
              <div className="product__grid-inner w-full flex flex-wrap">
                <ProductItem grid={3} />
                <ProductItem grid={3} />
                <ProductItem grid={3} />
              </div>
            </div>
            <div className="filter__control-bottom">
              <ProductListPageNumber />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProductListPage;
