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
    name: "Máy xay sinh tố",
    categoryId: 1,
    optionList: [
      {
        id: 11,
        name: "Màu sắc",
        values: [
          {
            id: 111,
            name: "Trắng",
            totalRecord: 100 // total product record
          },
          {
            id: 112,
            name: "Đen",
            totalRecord: 100 // total product record
          }
        ]
      },
      {
        id: 12,
        name: "Kích thước",
        values: [
          {
            id: 121,
            name: "100mm",
            totalRecord: 100 // total product record
          },
          {
            id: 122,
            name: "110mm",
            totalRecord: 100 // total product record
          }
        ]
      }
    ]
  },
  {
    id: 2,
    name: "Máy pha cà phê",
    categoryId: 2,
    optionList: [
      {
        id: 21,
        name: "Màu sắc",
        values: [
          {
            id: 211,
            name: "Đen",
            totalRecord: 100 // total product record
          },
          {
            id: 212,
            name: "Trắng",
            totalRecord: 100 // total product record
          },
        ]
      },
      {
        id: 22,
        name: "Kích thước",
        values: [
          {
            id: 221,
            name: "120mm",
            totalRecord: 100 // total product record
          },
          {
            id: 222,
            name: "130mm",
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
  const _brandId = Number(searchParams.get('brandId')) || -1;
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
            { name: productStore.productList.length > 0 ? productStore.productList[0].brandName : '', path: `?pageIndex=${_pageIndex}&brandId=${_brandId}` }
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
