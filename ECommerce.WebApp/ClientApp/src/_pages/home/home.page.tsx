import HomeBanner from "./_components/home-banner/home-banner.component"
import HomeBrandList from "./_components/home-brand-list/home-brand-list.component";
import 'swiper/css';
import 'swiper/css/pagination';
import "swiper/css/navigation";
import 'src/_pages/home/_styles/banner.css';
import HomeSorting from "./_components/home-sorting/home-sorting.component";

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
                                <HomeSorting />
                            </div>
                            <div className="bran__filter flex items-center">
                                <h3 className="mb-0">Danh mục</h3>
                                <HomeSorting />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}

export default HomePage