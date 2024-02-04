import { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
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
  const location = useLocation();
  const [hideBanner, setHideBanner] = useState(false);

  useEffect(() => {
    const hiddenPath = [
      '/product-detail',
      '/cart',
      '/login',
    ];
    if (hiddenPath.includes(location.pathname)) {
      setHideBanner(true);
    }
  }, [location.pathname]);

  return (
    <div className={`${hideBanner ? 'hidden' : ''}`}>
      <Swiper
        className={`banner-slider`}
        loop={true}
        slidesPerView={1}
        navigation={true}
        pagination={pagination}
        style={{ height: "calc(20vw*1.72)", minHeight: "33vw" }}
        modules={[Autoplay, Pagination, Navigation]}
      >
        <SwiperSlide>
          <img
            src="https://hihichi.com/images/banner/banner_147f1f14-cf61-4808-b5cb-180a60d7fe8f.png"
            alt=""
          />
        </SwiperSlide>
        <SwiperSlide>
          <img
            src="https://hihichi.com/images/banner/banner_947667cf-bb07-4ab8-9248-9fd7428f7837.jpg"
            alt=""
          />
        </SwiperSlide>
      </Swiper>
    </div>
  );
};

export default Banner;
