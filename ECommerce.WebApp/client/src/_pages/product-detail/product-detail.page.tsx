import { WebDirectional } from "src/_shares/_components";
import ProductDetailFooter from "./_components/layout/prod-suggestion";
import ProductDetailTab from "./_components/tab/prod-tab";
import ProductDetailInfo from "./_components/layout/prod-info";

import ProductDetailRealImages from "./_components/slide/prod-real-images";
import ProductDetailSlide from "./_components/slide/prod-slide";
import { useEffect, useState } from "react";
import { useProductStore } from "src/_cores/_store/root-store";
import { useDispatch } from "react-redux";
import ProductService from "src/_cores/_services/product.service";
import { setProductDetail } from "src/_cores/_reducers/product.reducer";
import InventoryService from "src/_cores/_services/inventory.service";
import { IBrand } from "src/_cores/_interfaces/inventory.interface";

const ProductDetailPage = () => {
  const [brand, setBrand] = useState<IBrand>();
  const searchParams = new URLSearchParams(window.location.search);
  const _id = Number(searchParams.get('id')) || -1;

  const productStore = useProductStore();
  const dispatch = useDispatch();

  useEffect(() => {
    getProductDetail();
  }, [_id]);

  const getProductDetail = () => {
    if (_id > -1) {
      ProductService.getProductDetail(_id).then((res) => {
        if (res.data) {
          dispatch(setProductDetail(res.data));
          getBrand();
        }
      }).catch((error) => {
        console.log(error);
      });
    }
  }

  const getBrand = () => {
    const brandId = productStore.productDetail?.brand.id ?? -1;
    if (brandId > -1) {
      InventoryService.getBrand(brandId).then((res) => {
        if (res.data) {
          setBrand(res.data);
        }
      }).catch((error) => {
        console.log(error);
      });
    }
  }

  return (
    <div className="custom-container">
      <div className="content__wrapper products__content-wrapper">
        <div className="content__inner w-full pb-0">
          <WebDirectional
            items={[
              { name: brand?.brandName ?? '', path: `?pageIndex=${1}&brandId=${brand?.brandId}` },
              { name: productStore.productDetail?.name ?? '', path: `?id=${_id}` },
            ]}
          />

          <div className="product__detail-content mt-4">
            <div className="md:flex gap-custom">
              <div className="product__detail-image md:w-5/12">
                <ProductDetailSlide />
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
