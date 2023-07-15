import { WebDirectional } from "src/_shares/_components";
import ProductDetailFooter from "./_components/layout/product-detail-footer.component";
import ProductDetailTab from "./_components/tab/main/product-detail-tab.component";
import ProductDetailInfo from "./_components/product-detail-info.component";

import ProductDetailRealImages from "./_components/silde/product-detail-real-images.component";
import ProductDetailSlide from "./_components/silde/product-detail-slide.component";

const ProductDetailPage = () => {
  return (
    <div className="custom-container">
      <div className="content__wrapper products__content-wrapper">
        <div className="content__inner w-full pb-0">
          <WebDirectional
            items={[
              {
                path: "/product-detail",
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
