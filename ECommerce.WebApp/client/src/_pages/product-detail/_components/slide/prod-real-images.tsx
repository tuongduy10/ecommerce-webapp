import { Swiper, SwiperSlide } from "swiper/react";
import "swiper/css";
import "swiper/css/free-mode";
import "swiper/css/pagination";
import { Autoplay, FreeMode, Pagination } from "swiper";

const ProductDetailRealImages = () => {
  return (
    <div className="product__images-slider">
      <h1 className="detail-title mb-0">
        <strong>HÌNH ẢNH THỰC TẾ</strong>
      </h1>
      <hr className="mt-0" />
      <Swiper
        slidesPerView={4}
        spaceBetween={30}
        freeMode={true}
        pagination={{
          clickable: true,
        }}
        autoplay={{
          delay: 1500,
          disableOnInteraction: false,
        }}
        modules={[Autoplay, FreeMode, Pagination]}
        className="mySwiper cursor-grab"
      >
        <SwiperSlide>
          <img
            src="https://hihichi.com/images/products/product_be147d8e-feb3-4c0f-9a0b-c1a779568d52.jpeg"
            alt=""
          />
        </SwiperSlide>
        <SwiperSlide>
          <img
            src="https://hihichi.com/images/products/product_be147d8e-feb3-4c0f-9a0b-c1a779568d52.jpeg"
            alt=""
          />
        </SwiperSlide>
        <SwiperSlide>
          <img
            src="https://hihichi.com/images/products/product_be147d8e-feb3-4c0f-9a0b-c1a779568d52.jpeg"
            alt=""
          />
        </SwiperSlide>
        <SwiperSlide>
          <img
            src="https://hihichi.com/images/products/product_be147d8e-feb3-4c0f-9a0b-c1a779568d52.jpeg"
            alt=""
          />
        </SwiperSlide>
        <SwiperSlide>
          <img
            src="https://hihichi.com/images/products/product_be147d8e-feb3-4c0f-9a0b-c1a779568d52.jpeg"
            alt=""
          />
        </SwiperSlide>
        <SwiperSlide>
          <img
            src="https://hihichi.com/images/products/product_be147d8e-feb3-4c0f-9a0b-c1a779568d52.jpeg"
            alt=""
          />
        </SwiperSlide>
        <SwiperSlide>
          <img
            src="https://hihichi.com/images/products/product_be147d8e-feb3-4c0f-9a0b-c1a779568d52.jpeg"
            alt=""
          />
        </SwiperSlide>
        <SwiperSlide>
          <img
            src="https://hihichi.com/images/products/product_be147d8e-feb3-4c0f-9a0b-c1a779568d52.jpeg"
            alt=""
          />
        </SwiperSlide>
        <SwiperSlide>
          <img
            src="https://hihichi.com/images/products/product_be147d8e-feb3-4c0f-9a0b-c1a779568d52.jpeg"
            alt=""
          />
        </SwiperSlide>
        <SwiperSlide>
          <img
            src="https://hihichi.com/images/products/product_be147d8e-feb3-4c0f-9a0b-c1a779568d52.jpeg"
            alt=""
          />
        </SwiperSlide>
      </Swiper>
    </div>
  );
};

export default ProductDetailRealImages;
