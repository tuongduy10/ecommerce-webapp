import ProductItem from "src/_shared/_components/product-item/product-item.component";
import ProductListCatagory from "./_components/product-list-catagory.component";
import ProductListFilter from "./_components/product-list-filter.component";
import ProductListCatagoryLgSize from "./_components/product-list-catagory-lg-size.component";
import ProductListPageNumber from "./_components/product-list-page-number.component";

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
            <div className="product__grid-wrapper">
              <ProductItem />
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
