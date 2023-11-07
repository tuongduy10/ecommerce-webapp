import BrandItem from "./brand-item.component";
import InventoryService from 'src/_cores/_services/inventory.service';
import { useDispatch } from 'react-redux';
import { useEffect } from "react";
import { IBrand } from "src/_cores/_interfaces/inventory.interface";
import React from "react";
import { useHomeStore } from "src/_cores/_store/root-store";
import { setBrands } from "src/_cores/_reducers";

const BrandList = () => {
    const homeStore = useHomeStore();
    const dispatch = useDispatch();

    useEffect(() => {
        getBrands(Number(homeStore.categorySelected));
    }, [homeStore.categorySelected]);

    const getBrands = (categoryId: number) => {
        const req = categoryId
            ? InventoryService.getBrandsInCategory(categoryId)
            : InventoryService.getBrands({});
        req.then(res => {
            if (res?.data) {
                dispatch(setBrands(res.data));
            }
        }).catch(error => {
            alert(error);
        });
    }

    const listDist = () => {
        const _list: any = [];
        if (homeStore.brands.length > 0) {
            homeStore.brands.forEach((item: IBrand) => {
                _list.push(item.brandName.substring(0, 1).toUpperCase());
            });
            return _list.filter((value: any, index: number, array: any) => array.indexOf(value) === index);
        }
        return _list;
    }

    const BrandListResult = () => {
        if (listDist().length > 0) {
            return listDist().map((dist: any) => (
                homeStore.alphabetSelected.map(alp => (
                    alp === dist ? (
                        <React.Fragment key={`alp-${alp}`}>
                            <h4>{alp}</h4>
                            <div className="flex flex-wrap">
                                 {homeStore.brands && homeStore.brands.length > 0 ?
                                    homeStore.brands.map((brand, idx) => {
                                        if (alp === brand.brandName.substring(0, 1).toUpperCase() && idx < 10) {
                                            return <BrandItem key={`home-brand-${brand.brandId}`} data={brand} />
                                        }
                                        return null;
                                    }) : null}
                            </div>
                        </React.Fragment>
                    ) : null
                ))
            ));
        }
        return null;
    }

    return (
        <div className="bran__list-items mb-4">
            <BrandListResult />
            <div className="w-100 text-center d-block">
                <button className="bran__viewmore d-inline-block" style={{ cursor: 'pointer' }}>Xem thêm</button>
            </div>
        </div>
    )
}

export default BrandList