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
            </div>
        </>
    )
}

export default HomePage