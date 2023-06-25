import { ENV } from "src/_configs/enviroment.config";

const BrandItem = (props: any) => {
    const { data } = props;
    return (
        <div className="bran__list-item card" style={{ border: '.5px solid #ddd', padding: '12px' }}>
            <a href="/">
                <div className="border-default bran__list-img flex items-center justify-between py-2 px-4 mx-auto bg-white">
                    <img src={ENV.IMAGE_URL + '/brand/' + data.brandImagePath} className="max-w-full max-h-full mx-auto" alt="name" />
                </div>
                <div className="card-body text-center p-2 items-center justify-center">
                    <h5 className="bran__list-name card-title mb-0">{data.brandName}</h5>
                    <h6 className="bran__list-type card-text">{data.category}</h6>
                </div>
            </a>
        </div>
    )
}

export default BrandItem;