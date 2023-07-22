import { WebDirectional } from "src/_shares/_components";
import ProductDetailFooter from "./_components/layout/prod-suggestion";
import ProductDetailTab from "./_components/tab/prod-tab";
import ProductDetailInfo from "./_components/layout/prod-info";

import ProductDetailRealImages from "./_components/slide/prod-real-images";
import ProductDetailSlide from "./_components/slide/prod-slide";

const ProductDetailPage = () => {
  return (
    <div className="custom-container">
      <div className="content__wrapper products__content-wrapper">
        <div className="content__inner w-full pb-0">
          <WebDirectional
            items={[
              {
                path: "/product-list",
                name: "Tên nhãn hàng",
              },
              {
                path: "/product-detail",
                name: "Tên sản phẩm",
              },
            ]}
          />

          <div className="product__detail-content mt-4">
            <div className="md:flex gap-custom">
              <div className="product__detail-image md:w-5/12">
                {<ProductDetailSlide />}
              </div>
              <div className="product__detail-info md:mt-0 md:w-7/12">
                <ProductDetailInfo />
              </div>
            </div>
          </div>
          <ProductDetailRealImages />
          <ProductDetailTab />
          <ProductDetailFooter />
        </div>
      </div>
    </div>
  );
};

export default ProductDetailPage;
