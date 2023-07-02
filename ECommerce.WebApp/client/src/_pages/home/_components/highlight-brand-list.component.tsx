import { useCallback, useEffect, useRef } from 'react';
import { ICON_NAME } from 'src/_shares/_components/mui-icon/_enums/mui-icon.enum';
import MuiIcon from 'src/_shares/_components/mui-icon/mui-icon.component';
import { Pagination, Navigation } from 'swiper';
import { Swiper, SwiperSlide } from 'swiper/react';
import { ENV } from 'src/_configs/enviroment.config';
import { useHomeStore } from 'src/_cores/_store/root-store';

const HomeBrandList = () => {
  const homeStore = useHomeStore();
  const navigationPrevRef = useRef<any>(null);
  const navigationNextRef = useRef<any>(null);
  const sliderRef = useRef<any>(null);

  const handlePrev = useCallback(() => {
    if (!sliderRef.current) return;
    sliderRef.current.swiper.slidePrev();
  }, []);

  const handleNext = useCallback(() => {
    if (!sliderRef.current) return;
    sliderRef.current.swiper.slideNext();
  }, []);

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
              navigation={{
                enabled: true,
                prevEl: navigationPrevRef.current,
                nextEl: navigationNextRef.current,
                disabledClass: 'pointer-events-none'
              }}
              onBeforeInit={(swiper) => {
                swiper.navigation.nextEl = navigationNextRef.current;
                swiper.navigation.prevEl = navigationPrevRef.current;
              }}
              modules={[Pagination, Navigation]}
              className='brand-slider my-4'
            >
              {homeStore.highLightBrands.map(item => (
                <>
                  <SwiperSlide key={`slc-${item.brandId}`}>
                    <a href="/" className="bran__logo p-4" style={{ cursor: 'pointer' }}>
                      <img src={ENV.IMAGE_URL + "/brand/" + item.brandImagePath} alt="" />
                    </a>
                  </SwiperSlide>
                  <SwiperSlide key={`slc-${item.brandId}`}>
                    <a href="/" className="bran__logo p-4" style={{ cursor: 'pointer' }}>
                      <img src={ENV.IMAGE_URL + "/brand/" + item.brandImagePath} alt="" />
                    </a>
                  </SwiperSlide>
                  <SwiperSlide key={`slc-${item.brandId}`}>
                    <a href="/" className="bran__logo p-4" style={{ cursor: 'pointer' }}>
                      <img src={ENV.IMAGE_URL + "/brand/" + item.brandImagePath} alt="" />
                    </a>
                  </SwiperSlide>
                </>
              ))}
            </Swiper>
            <button className='absolute left-[-50px] bottom-1/2 translate-y-1/2 hover:bg-red' ref={navigationPrevRef} onClick={handlePrev}>
              <MuiIcon
                name={ICON_NAME.FEATHER.CHEVRON_LEFT}
                className='w-[36px] h-[36px] border border-solid border-black hover:border-[#3b99fc] text-black hover:text-[#3b99fc]'
              />
            </button>
            <button className='absolute right-[-50px] bottom-1/2 translate-y-1/2 hover:bg-red' ref={navigationNextRef} onClick={handleNext}>
              <MuiIcon
                name={ICON_NAME.FEATHER.CHEVRON_RIGHT}
                className='w-[36px] h-[36px] border border-solid border-black hover:border-[#3b99fc] text-black hover:text-[#3b99fc]'
              />
            </button>
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
