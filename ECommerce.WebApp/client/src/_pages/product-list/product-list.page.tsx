import ProductItem from "src/_shares/_components/product-item/product-item.component";
import { ProlCategory, ProlCategoryMb, ProlFilter, ProlPagination } from "./_components";
import { WebDirectional } from "src/_shares/_components";
import { useEffect } from "react";
import ProductService from "src/_cores/_services/product.service";
import { useProductStore } from "src/_cores/_store/root-store";
import { useDispatch } from "react-redux";
import { setParam, setProductList, setSubCategories } from "src/_cores/_reducers/product.reducer";
import InventoryService from "src/_cores/_services/inventory.service";

const dummData = [
  {
    id: 1,
    name: "sub category 1",
    categoryId: 1,
    optionList: [
      {
        id: 1,
        name: "option 1",
        values: [
          {
            id: 11,
            name: "option value 11",
            totalRecord: 100 // total product record
          },
          {
            id: 12,
            name: "option value 12",
            totalRecord: 100 // total product record
          }
        ]
      },
      {
        id: 1,
        name: "option 1",
        values: [
          {
            id: 11,
            name: "option value 11",
            totalRecord: 100 // total product record
          },
          {
            id: 12,
            name: "option value 12",
            totalRecord: 100 // total product record
          }
        ]
      }
    ]
  },
  {
    id: 2,
    name: "sub category 2",
    categoryId: 2,
    optionList: [
      {
        id: 2,
        name: "option",
        values: [
          {
            id: 21,
            name: "option value 21",
            totalRecord: 100 // total product record
          },
          {
            id: 22,
            name: "option value 22",
            totalRecord: 100 // total product record
          },
        ]
      },
      {
        id: 1,
        name: "option 1",
        values: [
          {
            id: 11,
            name: "option value 11",
            totalRecord: 100 // total product record
          },
          {
            id: 12,
            name: "option value 12",
            totalRecord: 100 // total product record
          }
        ]
      }
    ]
  }
];

const ProductListPage = () => {
  const searchParams = new URLSearchParams(window.location.search);
  const _pageIndex = Number(searchParams.get('pageIndex'));
  const _brandId = Number(searchParams.get('brandId'));
  const _orderBy = searchParams.get('orderBy');

  const productStore = useProductStore();
  const dispatch = useDispatch();

  useEffect(() => {
    const params = {
      pageIndex: _pageIndex,
      brandId: _brandId,
      orderBy: _orderBy ?? '',
    }
    getData(params);
  }, [_pageIndex, _orderBy]);
  
  useEffect(() => {
    getSubCategories(_brandId);
  }, [_brandId])

  const getData = (params: any) => {
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
    });
  }

  const getSubCategories = (brandId: number) => {
    InventoryService.getSubCategories(brandId).then((res: any) => {
      if (res.data) {
        
      }
    }).catch(error => {
      console.log(error);
      dispatch(setSubCategories(dummData))
    });
  }

  return (
    <div className="custom-container">
      <div className="content__wrapper products__content-wrapper">
        <div className="content__inner w-full">
          <WebDirectional items={[
            { name: 'Bear', path: '/brand/bear' }
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
                    {productStore.productList.map((product) => (
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
