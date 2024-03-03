import { Dialog, DialogActions, DialogContent, DialogTitle, Grid, IconButton } from "@mui/material";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { ENV } from "src/_configs/enviroment.config";
import { ICity, IDistrict, IWard } from "src/_cores/_interfaces";
import CommonService from "src/_cores/_services/common.service";
import { useCartStore } from "src/_cores/_store/root-store";
import { MuiIcon, WebDirectional } from "src/_shares/_components";
import { ICON_NAME } from "src/_shares/_components/mui-icon/_enums/mui-icon.enum";
import { ProductHelper } from "src/_shares/_helpers/product-helper";

const PaymentPage = () => {
    const navigate = useNavigate();
    const cartStore = useCartStore();
    const [open, setOpen] = useState(false);
    const [cities, setCitites] = useState<ICity[]>([]);
    const [districts, setDistricts] = useState<IDistrict[]>([]);
    const [wards, setWards] = useState<IWard[]>([]);
    const [dataDetail, setDataDetail] = useState<{ [key: string]: any }>({});

    useEffect(() => {
        getCities();
    }, []);

    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const getCities = async () => {
        const res = await CommonService.getCities() as any;
        if (res.isSucceed) {
            setCitites(res.data);
        }
    }

    const onChangeCity = async (code: string) => {
        const _dataDetail = {
            ...dataDetail,
            cityCode: code,
            districtCode: '',
            wardCode: ''
        }
        setDataDetail(_dataDetail);
        await getDistricts(code);
    }

    const getDistricts = async (code: string) => {
        const res = await CommonService.getDistrictsByCityCode(code) as any;
        if (res.isSucceed) {
            setDistricts(res.data);
        }
    }

    const onChangeDistrict = async (code: string) => {
        const _dataDetail = {
            ...dataDetail,
            districtCode: code,
            wardCode: ''
        }
        setDataDetail(_dataDetail);
        await getWards(code);
    }

    const getWards = async (code: string) => {
        const res = await CommonService.getWardsByDistrictCode(code) as any;
        if (res.isSucceed) {
            setWards(res.data);
        }
    }

    const onChangeWard = (code: string) => {
        onChangeFieldValue('wardCode', code);
    }

    const onChangeFieldValue = (field: string, value: any) => {
        if (dataDetail[field]) {
            setDataDetail({ ...dataDetail, [field]: value });
        }
    }

    const getFormatedPrice = (price: number) => {
        return ProductHelper.getFormatedPrice(price);
      }

    return (
        <div className="custom-container">
            <div className="content__inner">
                <WebDirectional items={[
                    { path: '/cart', name: 'Giỏ Hàng' },
                    { path: '/payment', name: 'Thanh toán' }
                ]} />
                <div className="billing-summary">
                    <div className="cart-title text-center">HÓA ĐƠN CỦA BẠN</div>
                    {cartStore.productsInCart.length === 0 ? (
                        <div className="cart-body flex flex-wrap">
                            <div className="primary-cartcontent">
                                <div className="cart-bottom flex justify-center lg:justify-start">
                                    <button
                                        className="py-2 px-4 sm:w-full lg:w-48 bg-[#4D4D4D] text-white
                                        border border-solid border-black inline-block text-center
                                        hover:bg-[#fff] hover:text-gray-700  transition duration-500"
                                        onClick={() => navigate(-1)}
                                    >
                                        <strong>Quay lại</strong>
                                    </button>
                                </div>
                                <div className="text-center text-gray-500 mt-4">
                                    Chưa có sản phẩm
                                </div>
                            </div>
                        </div>
                    ) : (<>
                        <div className="flex flex-wrap mb-5">
                            <div className="primary-cartcontent">
                                <div className="cart-head">
                                    <span>
                                        <strong>Sản phẩm</strong>
                                    </span>
                                    <span className="">
                                        <strong>Số lượng</strong>
                                    </span>
                                    <span className="text-right">
                                        <strong>Thành tiền</strong>
                                    </span>
                                </div>
                                <div className="cart-products">
                                    {cartStore.productsInCart.map(_ => (
                                        <div key={_.uniqId} className="cart-product flex flex-wrap w-full">
                                            <div className="cart-product__image">
                                                <img src={`${ENV.IMAGE_URL}/products/${_.image}`} alt="" />
                                            </div>
                                            <div className="cart-product__info">
                                                <div className="cart-product--auto">
                                                    <div><strong>{_.name}</strong></div>
                                                    <div>{_.shopName}</div>
                                                    {_.options.length > 0 && (
                                                        <div>({_.options.map((o: any) => o.valueName).join(', ')})</div>
                                                    )}
                                                    <div className="price">
                                                        <span className="value">{_.discount ? getFormatedPrice(_.discount) : getFormatedPrice(_.price)}</span>
                                                    </div>
                                                </div>
                                                <div className="cart-product--col">
                                                    x{_.qty}
                                                </div>
                                                <div className="cart-product--col totalprice text-right">
                                                    <span className="total-value">{_.discount ? getFormatedPrice(_.discount*_.qty) : getFormatedPrice(_.price*_.qty)}</span>
                                                </div>
                                            </div>
                                        </div>
                                    ))}
                                    <div className="cart-bottom billing-total">
                                        <div className="attr mb-2 flex justify-between ml-auto mr-0">
                                            <span>Số lượng:</span>
                                            <span className="">{cartStore.totalQty}</span>
                                        </div>
                                        <div className="attr mb-2 flex justify-between ml-auto mr-0">
                                            <span>Tạm tính:</span>
                                            <span className="">{getFormatedPrice(cartStore.totalPrice)}</span>
                                        </div>
                                        <div className="attr mb-2 flex justify-between ml-auto mr-0">
                                            <span>Giảm (-0%):</span>
                                            <span className="">0 ₫</span>
                                        </div>
                                        <div className="attr mb-2 flex justify-between ml-auto mr-0">
                                            <span className="text">
                                                TỔNG CỘNG:
                                            </span>
                                            <span className="value">
                                                <strong>{getFormatedPrice(cartStore.totalPrice)}</strong>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <Grid container spacing={3}>
                            <Grid className="user-info" item xs={7}>
                                <div className="title">
                                    <strong>THÔNG TIN GIAO HÀNG</strong>
                                </div>
                                <div className="payment-info__line w-full flex items-center relative">
                                    <span style={{ color: 'red', position: 'absolute', left: '8px', top: '3px' }}>*</span>
                                    <input className="w-full" type="text" placeholder="Họ tên" />
                                </div>
                                <div className="payment-info__line w-full flex items-center relative">
                                    <span style={{ color: 'red', position: 'absolute', left: '8px', top: '3px' }}>*</span>
                                    <input className="w-full" type="text" placeholder="Số điện thoại" />
                                </div>
                                <div
                                    className="payment-info__line payment-bank w-full flex items-center justify-between flex-wrap">
                                    <div className="location">
                                        <span style={{ color: 'red', position: 'absolute', left: '8px', top: '3px' }}>*</span>
                                        <select name="" id="" className="city-select w-full" onChange={e => onChangeCity(e.target.value)}>
                                            <option value="" disabled selected={!dataDetail || !dataDetail['cityCode']}>Tỉnh/Thành...</option>
                                            {cities.length > 0 && (
                                                cities.map((item) => (
                                                    <option key={item.code} value={item.code} selected={item.code === (dataDetail['cityCode'] ?? "")}>{item.name}</option>
                                                ))
                                            )}
                                        </select>
                                    </div>
                                    <div className="location">
                                        <span style={{ color: 'red', position: 'absolute', left: '8px', top: '3px' }}>*</span>
                                        <select name="" id="" className="district-select w-full" onChange={e => onChangeDistrict(e.target.value)}>
                                            <option value="" disabled selected={!dataDetail || !dataDetail['districtCode']}>Quận/Huyện...</option>
                                            {districts.length > 0 && (
                                                districts.map((item) => (
                                                    <option key={item.code} value={item.code} selected={item.code === (dataDetail['districtCode'] ?? "")}>{item.name}</option>
                                                ))
                                            )}
                                        </select>
                                    </div>
                                    <div className="location">
                                        <span style={{ color: 'red', position: 'absolute', left: '8px', top: '3px' }}>*</span>
                                        <select name="" id="" className="ward-select w-full" onChange={e => onChangeWard(e.target.value)}>
                                            <option value="" disabled selected={!dataDetail || !dataDetail['wardCode']}>Phường/Xã...</option>
                                            {wards.length > 0 && (
                                                wards.map((item) => (
                                                    <option key={item.code} value={item.code} selected={item.code === (dataDetail['wardCode'] ?? "")}>{item.name}</option>
                                                ))
                                            )}
                                        </select>
                                    </div>
                                </div>
                                <div className="payment-info__line w-full flex items-center relative">
                                    <span style={{ color: 'red', position: 'absolute', left: '8px', top: '3px' }}>*</span>
                                    <input className="w-full" type="text" placeholder="Địa chỉ" />
                                </div>
                                <div className="payment-info__line w-full flex items-center relative">
                                    <input className="w-full" type="text" placeholder="Email" />
                                </div>
                                <div className="payment-info__line w-full flex items-center relative">
                                    <input className="w-full" type="text" placeholder="Ghi chú" />
                                </div>
                            </Grid>
                            <Grid className="payment-method" item xs={5}>
                                <div className="title">
                                    <strong>PHƯƠNG THỨC THANH TOÁN</strong>
                                </div>
                                <div className="payment-info__line w-full items-center flex">
                                    <input name="payment-method" type="radio" style={{ height: 'calc(48px + (57 - 48) * ((100vw - 375px)/ (1920 - 375)))' }} />
                                    <label className="ml-2 mb-0 cursor-pointer">Thanh toán khi nhận hàng</label>
                                </div>
                                <div className="payment-info__line w-full items-center flex">
                                    <input name="payment-method" type="radio"
                                        style={{ height: 'calc(48px + (57 - 48) * ((100vw - 375px)/ (1920 - 375)))' }} />
                                    <label className="ml-2 mb-0 cursor-pointer">Chuyển khoản</label>
                                </div>
                                <div className="payment-checkout text-center block">
                                    <button className="inline-block btn-black" data-toggle="modal" data-target="#exampleModal" style={{ border: '1px solid #333' }}
                                        onClick={handleClickOpen}
                                    >
                                        Đặt hàng
                                    </button>
                                    <PaymentDialog
                                        open={open}
                                        onClose={handleClose}
                                    />
                                </div>
                            </Grid>
                        </Grid>
                    </>)}
                </div>
            </div>
        </div>
    )
}

interface DialogProps {
    open: boolean;
    onClose: () => void;
}

const PaymentDialog = (props: DialogProps) => {
    const { onClose, open } = props;

    const handleClose = () => {
        onClose();
    };

    return (
        <Dialog onClose={handleClose} open={open}>
            <DialogTitle>
                Thông tin chuyển khoản
            </DialogTitle>
            <IconButton
                aria-label="close"
                onClick={handleClose}
                sx={{
                    position: 'absolute',
                    right: 8,
                    top: 8,
                    color: (theme) => theme.palette.grey[500],
                }}
            >
                <MuiIcon name={ICON_NAME.FEATHER.X} />
            </IconButton>
            <DialogContent>
                <div className="payment-expand">
                    <div><strong>Mã đơn hàng: 11111</strong></div>
                    <div className="mb-4">
                        Cảm ơn quý khách đã mua hàng.
                        Quý khách vui lòng chuyển khoản để thanh toán đơn hàng với thông tin:
                    </div>
                    <hr className="mb-4" />
                    <ul className="bank-list">
                        <li className="bank">
                            <div className="flex">
                                <div className="bank-image">
                                    <img className="max-w-full max-h-full" src="assets/images/logo/bidv.png" alt="" />
                                </div>
                                <div className="bank-info flex items-center">
                                    <div className="">
                                        <div className="bank-name mb-2">Ngân hàng Thương mại cổ phần Đầu tư và
                                            Phát
                                            triển Việt Nam</div>
                                        <div className="bank-accnumber flex mb-2">
                                            <span className="mr-2">STK: <strong className="stk">987654321000</strong></span>
                                            <button className="--tooltip copy-stk" style={{ height: '22px', background: 'none', cursor: 'pointer' }}>
                                                <span className="--tooltip-text">Sao chép</span>
                                                <svg xmlns="http://www.w3.org/2000/svg" width="22"
                                                    height="22" viewBox="0 0 24 24" fill="none"
                                                    stroke="#333" strokeWidth="1"
                                                    strokeLinecap="round" strokeLinejoin="round"
                                                    className="feather feather-copy">
                                                    <rect x="9" y="9" width="13" height="13" rx="2" ry="2"></rect>
                                                    <path d="M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1"></path>
                                                </svg>
                                            </button>
                                        </div>
                                        <div className="bank-username">Tên: <strong>NGUYEN VAN A</strong></div>
                                    </div>
                                </div>
                            </div>
                        </li>
                        <li className="bank">
                            <div className="flex">
                                <div className="bank-image">
                                    <img className="max-w-full max-h-full" src="assets/images/logo/sacombank.png"
                                        alt="" />
                                </div>
                                <div className="bank-info flex items-center">
                                    <div className="">
                                        <div className="bank-name mb-2">Ngân hàng thương mại cổ phần Sài Gòn Thương Tín</div>
                                        <div className="bank-accnumber mb-2 flex items-center">
                                            <span className="mr-2">
                                                STK: <strong className="stk">000123456789</strong>
                                            </span>
                                            <button className="--tooltip copy-stk" style={{ height: '22px', background: 'none', cursor: 'pointer' }}>
                                                <span className="--tooltip-text">Sao chép</span>
                                                <svg xmlns="http://www.w3.org/2000/svg" width="22"
                                                    height="22" viewBox="0 0 24 24" fill="none"
                                                    stroke="#333" strokeWidth="1"
                                                    strokeLinecap="round" strokeLinejoin="round"
                                                    className="feather feather-copy">
                                                    <rect x="9" y="9" width="13" height="13" rx="2" ry="2"></rect>
                                                    <path d="M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1"></path>
                                                </svg>
                                            </button>
                                        </div>
                                        <div className="bank-username">Tên: <strong>NGUYEN VAN A</strong></div>
                                    </div>
                                </div>
                            </div>
                        </li>
                    </ul>
                    <hr className="mb-4" />
                    <div>Nội dung chuyển khoản: <strong>Thanh toan cho ma don hang: 11111</strong></div>
                </div>
            </DialogContent>
        </Dialog>
    )
}

export default PaymentPage;