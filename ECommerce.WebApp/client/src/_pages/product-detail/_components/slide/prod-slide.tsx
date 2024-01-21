import { useState } from "react";
import SwiperCore, { Pagination, Navigation } from "swiper";
import { Swiper, SwiperSlide } from "swiper/react";
import GalleryDialog from "../dialog/prod-gallery";
import { useProductStore } from "src/_cores/_store/root-store";
import { ENV } from "src/_configs/enviroment.config";

const ProductDetailSlide = () => {
  const productStore = useProductStore();
  const [activeSlide, setActiveSlide] = useState(0);
  const [swiperInstance, setSwiperInstance] = useState<SwiperCore | null>(null);
  const [openDialog, setOpenDialog] = useState(false);
  const [selectedImage, setSelectedImage] = useState("");

  const handleClickOpenDialog = (index: number) => {
    const url = ENV.IMAGE_URL + '/products/' + (productStore.productDetail?.imagePaths[index] ?? '')
    setSelectedImage(url);
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
                {productStore.productDetail?.imagePaths?.map((image, index) => (
                  <SwiperSlide key={`main-img-${index}`}>
                    <img
                      className="cursor-grab"
                      src={ENV.IMAGE_URL + '/products/' + image}
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
              {productStore.productDetail?.imagePaths?.map((image, index) => (
                <li
                  key={`child-img-${index}`}
                  className={`${activeSlide === index ? "active" : ""} mr-2`}
                  onClick={() => handleGalleryItemClick(index)}
                  style={{ borderRadius: "5px", cursor: "pointer" }}
                >
                  <a>
                    <img src={ENV.IMAGE_URL + '/products/' + image} alt="Hình ảnh sản phẩm" />
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
