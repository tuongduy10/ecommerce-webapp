import "swiper/css";
import "swiper/css/pagination";
import "swiper/css/navigation";
import 'src/_pages/home/_styles/banner.css';
import { HighlightBrandList, HomeSorting, BrandList } from './_components';
import { useEffect, useState } from 'react';
import InventoryService from 'src/_cores/_services/inventory.service';
import { useDispatch } from 'react-redux';
import { IValueNameBase } from 'src/_cores/_interfaces/req-res.interface';
import { IBrand, ICategory } from 'src/_cores/_interfaces/inventory.interface';
import { ALPHABET_VALUES } from './_enums/sorting.enum';
import { setBrands, setCategories, setHighLightBrands } from "src/_cores/_reducers";

const HomePage = () => {
  const dispatch = useDispatch();
  const [categorySorting, setCategorySorting] = useState<IValueNameBase[]>([]);

  useEffect(() => {
    getCategories();
    getBrands();
  }, []);


  const getCategories = () => {
    InventoryService.getCategories().then(res => {
      if (res?.data) {
        const _list = res.data.map((item: ICategory) => {
          return {
            value: item.categoryId,
            name: item.categoryName,
            type: 'category'
          }
        });
        const sorting = [{
          value: 0,
          name: 'Tất cả',
          type: 'category'
        }].concat(_list) as any[];

        setCategorySorting(sorting);
        dispatch(setCategories(res.data))
      }
    }).catch(error => {
      alert(error);
    });
  }

  const getBrands = () => {
    InventoryService.getBrands().then(res => {
      if (res?.data) {
        const highLightList = res.data.filter((brand: IBrand) => brand.highlights);
        dispatch(setHighLightBrands(highLightList));
        dispatch(setBrands(res.data));
      }
    }).catch(error => {
      alert(error);
    });
  }

  return (
    <>
      <div className="custom-container">
        <HighlightBrandList />
        <div className="content__wrapper">
          <div className="w-full my-4 p-4">
            <div className="flex items-center flex-wrap">
              <div className="bran__filter flex items-center">
                <h3 className="mb-0">Hiển thị theo bởi</h3>
                <HomeSorting sortItems={ALPHABET_VALUES} sortType='alphabet' />
              </div>
              <div className="bran__filter flex items-center">
                <h3 className="mb-0">Danh mục</h3>
                <HomeSorting sortItems={categorySorting} sortType='category' />
              </div>
            </div>

            <div className="bran__list-line my-4"></div>
            <BrandList />
          </div>
        </div>
      </div>
    </>
  );
};

export default HomePage;
