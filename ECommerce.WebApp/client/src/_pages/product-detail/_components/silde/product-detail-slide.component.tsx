/* eslint-disable jsx-a11y/anchor-has-content */
/* eslint-disable jsx-a11y/anchor-is-valid */
import { useState } from "react";
import SwiperCore, { Pagination, Navigation } from "swiper";
import { Swiper, SwiperSlide } from "swiper/react";
import GalleryDialog from "../dialog/product-detail-gallery.dialog";

export const imagesSlide = [
  "https://hihichi.com/images/products/product_b0214a36-2678-428d-b589-fc451905e4a6.jpg",
  "https://hihichi.com/images/products/product_5cf18ba2-cf25-4301-8925-fdc6aac87374.jpg",
  "https://hihichi.com/images/products/product_ea42cf0b-685c-42fc-90d3-aa1a8cdf438b.jpg",
  "https://hihichi.com/images/products/product_ea42cf0b-685c-42fc-90d3-aa1a8cdf438b.jpg",
  "https://hihichi.com/images/products/product_ea42cf0b-685c-42fc-90d3-aa1a8cdf438b.jpg",
  "https://hihichi.com/images/products/product_ea42cf0b-685c-42fc-90d3-aa1a8cdf438b.jpg",
  "https://hihichi.com/images/products/product_ea42cf0b-685c-42fc-90d3-aa1a8cdf438b.jpg",
];

const ProductDetailSlide = () => {
  const [activeSlide, setActiveSlide] = useState(0);
  const [swiperInstance, setSwiperInstance] = useState<SwiperCore | null>(null);

  const [openDialog, setOpenDialog] = useState(false);

  const [selectedImage, setSelectedImage] = useState("");

  const handleClickOpenDialog = (index: number) => {
    setSelectedImage(imagesSlide[index]);
    setOpenDialog(true);
  };

  const handleClickCloseDialog = () => {
    setOpenDialog(false);
  };

  const handleGalleryItemClick = (index: number) => {
    if (swiperInstance) {
      setActiveSlide(index);
      swiperInstance.slideTo(index);
    }
  };

  const handleSwiperSlideChange = (swiper: SwiperCore) => {
    setActiveSlide(swiper.realIndex);
  };

  return (
    <>
      <GalleryDialog
        selectedImage={selectedImage}
        openDialog={openDialog}
        handleClickCloseDialog={handleClickCloseDialog}
      />
      <div className="product__detail-card">
        <div className="product__detail-slider mb-2">
          <div className="lSSlideOuter">
            <div className="lSSlideWrapper mb-2">
              <Swiper
                className="product-slider lightSlider h-full lSSlide"
                // autoplay={autoplay}
                loop={true}
                slidesPerView={1}
                navigation={true}
                modules={[Pagination, Navigation]}
                onSwiper={(swiper) => setSwiperInstance(swiper)}
                onSlideChange={handleSwiperSlideChange}
              >
                {imagesSlide.map((image, index) => (
                  <SwiperSlide key={image}>
                    <img
                      className="cursor-grab"
                      src={image}
                      alt=""
                      onClick={() => {
                        handleClickOpenDialog(index);
                      }}
                    />
                  </SwiperSlide>
                ))}
              </Swiper>
            </div>
            <ul
              className="lSPager lSGallery flex"
              style={{
                marginTop: "5px",
                transitionDuration: "400ms",
                height: "59.3281px",
                transform: "translate3d(0px, 0px, 0px)",
              }}
            >
              {imagesSlide.map((image, index) => (
                <li
                  className={`${activeSlide === index ? "active" : ""} mr-2`}
                  key={index}
                  onClick={() => handleGalleryItemClick(index)}
                  style={{ borderRadius: "5px", cursor: "pointer" }}
                >
                  <a>
                    <img src={image} alt="Hình ảnh sản phẩm" />
                  </a>
                </li>
              ))}
            </ul>
          </div>
        </div>
      </div>
    </>
  );
};

export default ProductDetailSlide;
