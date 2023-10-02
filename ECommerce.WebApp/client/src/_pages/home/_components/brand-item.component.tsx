import { useNavigate } from "react-router-dom";
import { ENV } from "src/_configs/enviroment.config";
import { ROUTE_NAME } from "src/_cores/_enums/route-config.enum";

const BrandItem = (props: any) => {
    const navigate = useNavigate();
    const { data } = props;
    
    const goToProductList = () => {
        navigate({
            pathname: ROUTE_NAME.PRODUCT_LIST,
            search: `?pageIndex=1&brandId=${data.brandId}`
        });
    }

    return (
        <div
            className="bran__list-item rounded cursor-pointer" style={{ border: '.5px solid #ddd', padding: '12px' }}
            onClick={goToProductList}
        >
            <div className="border-default bran__list-img flex items-center justify-between py-2 px-4 mx-auto bg-white">
                <img src={ENV.IMAGE_URL + '/brand/' + data.brandImagePath} className="max-w-full max-h-full mx-auto" alt="name" />
            </div>
            <div className="card-body text-center p-2 items-center justify-center">
                <h5 className="bran__list-name card-title mb-0 leading-[1.2] font-medium text-[1.25rem]">{data.brandName}</h5>
                <h6 className="bran__list-type card-text leading-[1.2] font-medium text-[1rem]">{data.category}</h6>
            </div>
        </div>
    )
}

export default BrandItem;