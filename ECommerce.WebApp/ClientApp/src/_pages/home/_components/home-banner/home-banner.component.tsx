import { Autoplay, Pagination, Navigation } from 'swiper';
import { Swiper, SwiperSlide } from 'swiper/react';
import { PaginationOptions } from 'swiper/types/modules/pagination';

const autoplay = {
    delay: 2500,
    disableOnInteraction: false,
}

const pagination: PaginationOptions = {
    clickable: true,
    renderBullet: function (index: number, className) {
        return `<span class='w-[30px] h-[3px] bg-[#FFFFFF80] rounded-none ${className}'></span>`;
    }
}

const HomeBanner = () => {
    return (
        <Swiper
            className='banner-slider'
            autoplay={autoplay}
            loop={true}
            slidesPerView={1}
            navigation={true}
            pagination={pagination}
            style={{ maxHeight: '500px', minHeight: '33vw' }}
            modules={[Autoplay, Pagination, Navigation]}
        >
            <SwiperSlide>
                <img className='h-auto my-auto' src="/assets/images/banner/2d83236a-01eb-4195-ab86-a4b12af64f79.jpg" alt="" />
            </SwiperSlide>
            <SwiperSlide>
                <img className='h-auto' src="/assets/images/banner/a9c4f5ca-0786-468d-b51c-68f9b856569b.jpg" alt="" />
            </SwiperSlide>
        </Swiper>
    )
}

export default HomeBanner;