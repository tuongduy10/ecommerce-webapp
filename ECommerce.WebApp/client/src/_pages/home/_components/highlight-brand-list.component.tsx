import { useRef } from 'react';
import { Pagination, Navigation } from 'swiper';
import { Swiper, SwiperSlide } from 'swiper/react';
import { ENV } from 'src/_configs/enviroment.config';
import { useHomeStore } from 'src/_cores/_store/root-store';

const HomeBrandList = () => {
  const homeStore = useHomeStore();
  const sliderRef = useRef<any>(null);

  return (
    <div className='hidden md:block'>
      {homeStore.highLightBrands && homeStore.highLightBrands.length > 0 ? (
        <>
          <div className="bran__logo-list">
            <div className="text-center">
              <div className="line my-4 mx-auto"></div>
              <h3 className="home-title mx-auto text-[1.75rem]">Thương hiệu nổi bật</h3>
            </div>
          </div>
          <div className='relative'>
            <Swiper
              ref={sliderRef}
              slidesPerView={5}
              spaceBetween={10}
              navigation={true}
              modules={[Pagination, Navigation]}
              className='brand-slider my-4'
            >
              {homeStore.highLightBrands.map(item => (
                <SwiperSlide key={`slc-${item.brandId}`}>
                  <a href="/" className="bran__logo p-4" style={{ cursor: 'pointer' }}>
                    <img src={ENV.IMAGE_URL + "/brand/" + item.brandImagePath} alt="" />
                  </a>
                </SwiperSlide>
              ))}
            </Swiper>
          </div>
          <div className="w-full text-center block">
            <a className="bran__viewmore inline-block" style={{ cursor: 'pointer' }} href="/">Xem thêm</a>
          </div>
        </>
      ) : null}
    </div>
  );
};

export default HomeBrandList;
