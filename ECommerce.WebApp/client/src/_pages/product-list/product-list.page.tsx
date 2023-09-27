import ProductItem from "src/_shares/_components/product-item/product-item.component";
import { ProlCategory, ProlCategoryMb, ProlFilter, ProlPagination } from "./_components";
import { WebDirectional } from "src/_shares/_components";
import { useEffect } from "react";
import ProductService from "src/_cores/_services/product.service";

const ProductListPage = () => {
  useEffect(() => {
    const params = {
      pageIndex: 1,
      brandId: 66,
    }
    ProductService.getProductList(params).then(res => {
      if (res.data) {
        console.log(res.data)
      }
    })
  }, []);
  return (
    <div className="custom-container">
      <div className="content__wrapper products__content-wrapper">
        <div className="content__inner w-full">
          <WebDirectional items={[
            { name: 'Bear', path: '/brand/bear' }
          ]} />
          <div className="products__content flex justify-center">
            <div className="hidden md:block">
              <ProlCategory />
            </div>
            <div className="products__list ">
              <div className="filter__control-top">
                <ProlFilter />
                <ProlCategoryMb />
                <ProlPagination />
              </div>
              <div className="product__grid-wrapper">
                <p className="product__grid-title text-center">Tất cả sản phẩm</p>
                <div className="product__grid-inner w-full flex flex-wrap">
                  <ProductItem grid={3} />
                  <ProductItem grid={3} />
                  <ProductItem grid={3} />
                </div>
              </div>
              <div className="filter__control-bottom">
                <ProlPagination />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProductListPage;
