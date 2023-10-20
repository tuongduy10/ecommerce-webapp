/* eslint-disable jsx-a11y/anchor-is-valid */
import ProductItem from "src/_shares/_components/product-item/product-item.component";

const ProductDetailFooter = () => {
  return (
    <div className="products__list mx-auto w-full hidden">
      <h1 className="detail-title mb-0">
        <strong>GỢI Ý CHO BẠN</strong>
      </h1>
      <hr className="my-0" />
      <div className="product__grid-wrapper pb-0">
        <div className="product__grid-inner w-full flex flex-wrap">
          {/* <ProductItem grid={4} data={null} />
          <ProductItem grid={4} data={null} />
          <ProductItem grid={4} data={null} />
          <ProductItem grid={4} data={null} /> */}
        </div>
      </div>
      <div className="w-100 text-center d-block cursor-pointer">
        <a className="bran__viewmore inline-block">Xem thêm</a>
      </div>

      {/* <div className="w-full text-center block mt-2">
        <a>Chưa có sản phẩm nào phù hợp</a>
      </div> */}
    </div>
  );
};

export default ProductDetailFooter;
