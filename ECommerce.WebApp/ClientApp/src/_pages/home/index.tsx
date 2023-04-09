import { Autoplay, Pagination, Navigation } from 'swiper';
import { Swiper, SwiperSlide } from 'swiper/react';
import 'swiper/css';
import 'swiper/css/pagination';
import "swiper/css/navigation";

const HomePage = () => {
    return (
        <>
            <Swiper
                autoplay={{
                    delay: 10000,
                    disableOnInteraction: false,
                }}
                loop={true}
                slidesPerView={1}
                navigation={true}
                pagination={{
                    clickable: true,
                }}
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
            <div className="custom-container">

            </div>
        </>
    )
}

export default HomePage