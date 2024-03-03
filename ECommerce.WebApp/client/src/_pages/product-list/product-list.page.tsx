import ProductItem from "src/_shares/_components/product-item/product-item.component";
import { ProlCategory, ProlCategoryMb, ProlFilter, ProlPagination } from "./_components";
import { WebDirectional } from "src/_shares/_components";
import { useEffect } from "react";
import ProductService from "src/_cores/_services/product.service";
import { useHomeStore, useProductStore } from "src/_cores/_store/root-store";
import { useDispatch } from "react-redux";
import { setParam, setProductList, setSubCategories } from "src/_cores/_reducers/product.reducer";
import InventoryService from "src/_cores/_services/inventory.service";
import { IOption, IOptionValue, ISubCategory } from "src/_cores/_interfaces/inventory.interface";
import { setSelectedBrand } from "src/_cores/_reducers/home.reducer";

const ProductListPage = () => {
  const searchParams = new URLSearchParams(window.location.search);
  const _pageIndex = Number(searchParams.get('pageIndex'));
  const _brandId = Number(searchParams.get('brandId')) || -1;
  const _orderBy = searchParams.get('orderBy');
  const _subCategoryId = Number(searchParams.get('subCategoryId')) || -1;
  const _optionValueIds = searchParams.get('optionValueIds');

  const productStore = useProductStore();
  const homeStore = useHomeStore();
  const dispatch = useDispatch();

  useEffect(() => {
    const params = {
      pageIndex: _pageIndex,
      brandId: _brandId,
      orderBy: _orderBy ?? '',
      subCategoryId: _subCategoryId,
      optionValueIds: _optionValueIds ? _optionValueIds.split(',').map(id => Number(id)) : [],
    }
    getData(params);
    if (!homeStore.selectedBrand) {
      async function getBrand() {
        const res = await InventoryService.getBrand(_brandId) as any;
        if (res.isSucceed) {
          dispatch(setSelectedBrand(res.data));
        }
      }
      getBrand();
    }
  }, [_pageIndex, _orderBy, _subCategoryId, _optionValueIds]);

  useEffect(() => {
    getSubCategories({ brandId: _brandId });
  }, [_brandId])

  const getData = (params: any) => {
    if (_pageIndex > 0 && _brandId > -1) {
      ProductService.getProductList(params).then((res: any) => {
        if (res.data) {
          const _data = res.data;
          const param = {
            ...productStore.param,
            pageIndex: _data.currentPage,
            totalPage: _data.totalPage,
            currentRecord: _data.currentRecord,
            totalRecord: _data.totalRecord,
          }
          dispatch(setParam(param));
          dispatch(setProductList(_data.items));
        }
      }).catch((error: any) => {
        console.log(error);
      });
    }
  }

  const getSubCategories = (params: { brandId: number }) => {
    InventoryService.getSubCategories(params).then((res: any) => {
      if (res.data) {
        const list = res.data.map((item: ISubCategory) => {
          item.optionList?.forEach((option: IOption) => {
            option.values = option.values?.filter((value: IOptionValue) => (value.totalRecord ?? 0) > 0);
          });
          item.optionList = item.optionList?.filter((option: IOption) => (option.values?.length ?? 0) > 0);
          return item;
        });
        dispatch(setSubCategories(list));
      }
    }).catch(error => {
      console.log(error);
    });
  }

  return (
    <div className="custom-container">
      <div className="content__wrapper products__content-wrapper">
        <div className="content__inner w-full">
          <WebDirectional items={[
            { name: homeStore.selectedBrand?.name ?? '', path: `?pageIndex=${_pageIndex}&brandId=${homeStore.selectedBrand?.id}` }
          ]} />
          <div className="products__content flex justify-center">
            <div className="hidden md:block">
              <ProlCategory />
            </div>
            <div className="products__list ">
              <div className="filter__control-top">
                <ProlFilter />
                <ProlCategoryMb />
                <ProlPagination />
              </div>
              <div className="product__grid-wrapper">
                {productStore.productList.length > 0 ? (<>
                  <p className="product__grid-title text-center">Tất cả sản phẩm</p>
                  <div className="product__grid-inner w-full flex flex-wrap">
                    {productStore.productList.map((product: any) => (
                      <ProductItem key={product.id} grid={3} data={product} />
                    ))}
                  </div>
                </>) : (
                  <p className="text-center w-full">Chưa có sản phẩm</p>
                )}
              </div>
              <div className="filter__control-bottom">
                <ProlPagination />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProductListPage;
