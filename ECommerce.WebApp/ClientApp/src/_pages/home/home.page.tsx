import HomeBanner from "./_components/home-banner/home-banner.component"
import HomeBrandList from "./_components/home-brand-list/home-brand-list.component";
import 'swiper/css';
import 'swiper/css/pagination';
import "swiper/css/navigation";
import 'src/_pages/home/_styles/banner.css';

const HomePage = () => {
    return (
        <>
            <HomeBanner />
            <div className="custom-container">
                <HomeBrandList />
                <div className="content__wrapper">
                    <div className="w-full my-4 p-4">
                        <div className="flex items-center flex-wrap">
                            <div className="bran__filter flex items-center">
                                <h3 className="mb-0">Hiển thị theo bởi</h3>
                                <div className="order__item-wrapper order__button">
                                    <div className="order__item-dropdown">
                                        Tất cả<svg xmlns="http://www.w3.org/2000/svg"
                                            width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#333"
                                            stroke-width="1" stroke-linecap="round" stroke-linejoin="round"
                                            className="feather feather-chevron-down svg-order">
                                            <polyline points="6 9 12 15 18 9"></polyline>
                                        </svg>
                                    </div>
                                </div>
                                <div className="bran__filter flex items-center">
                                    <h3 className="mb-0">Danh mục</h3>
                                    <div className="order__item-wrapper order__button">
                                        <div className="order__item-dropdown">
                                            Tất cả<svg xmlns="http://www.w3.org/2000/svg"
                                                width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#333"
                                                stroke-width="1" stroke-linecap="round" stroke-linejoin="round"
                                                className="feather feather-chevron-down svg-order">
                                                <polyline points="6 9 12 15 18 9"></polyline>
                                            </svg>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}

export default HomePage