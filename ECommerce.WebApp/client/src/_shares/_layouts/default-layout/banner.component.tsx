import { useEffect, useState } from "react";
import { Autoplay, Pagination, Navigation } from "swiper";
import { Swiper, SwiperSlide } from "swiper/react";
import { PaginationOptions } from "swiper/types/modules/pagination";

/* const autoplay = {
  delay: 2500,
  disableOnInteraction: false,
};  */

const pagination: PaginationOptions = {
  clickable: true,
  renderBullet: function (index: number, className: string) {
    return `<span class='w-[30px] h-[3px] bg-[#FFFFFF80] rounded-none ${className}'></span>`;
  },
};

const Banner = () => {
  const [hideBanner, setHideBanner] = useState(false);

  useEffect(() => {
<<<<<<< HEAD
    const path = [
      '/v2/product-detail'
=======
    const hiddenPath = [
      '/v2/product-detail',
      '/v2/cart',
      '/v2/login',
>>>>>>> 9cb11ae (JWT auth)
    ];
    if (hiddenPath.includes(window.location.pathname)) {
      setHideBanner(true);
    }
  }, []);

  return (
    <div className={`${hideBanner ? 'hidden' : ''}`}>
      <Swiper
        className={`banner-slider`}
        loop={true}
        slidesPerView={1}
        navigation={true}
        pagination={pagination}
        style={{ maxHeight: "500px", minHeight: "33vw" }}
        modules={[Autoplay, Pagination, Navigation]}
      >
        <SwiperSlide>
          <img
            className="w-full object-contain"
            src="https://hihichi.com/images/banner/banner_147f1f14-cf61-4808-b5cb-180a60d7fe8f.png"
            alt=""
          />
        </SwiperSlide>
        <SwiperSlide>
          <img
            className="w-full object-contain"
            src="https://hihichi.com/images/banner/banner_947667cf-bb07-4ab8-9248-9fd7428f7837.jpg"
            alt=""
          />
        </SwiperSlide>
      </Swiper>
    </div>
  );
};

export default Banner;
