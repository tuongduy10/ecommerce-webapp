import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { ROUTE_NAME } from "src/_cores/_enums/route-config.enum";
import { ICity, IDistrict, IWard } from "src/_cores/_interfaces";
import CommonService from "src/_cores/_services/common.service";
import { useUserStore } from "src/_cores/_store/root-store";
import { MuiIcon, WebDirectional } from "src/_shares/_components";
import { ICON_NAME } from "src/_shares/_components/mui-icon/_enums/mui-icon.enum";

const UserProfilePage = () => {
    const [cities, setCitites] = useState<ICity[]>([]);
    const [districts, setDistricts] = useState<IDistrict[]>([]);
    const [wards, setWards] = useState<IWard[]>([]);
    const [dataDetail, setDataDetail] = useState<{ [key: string]: any }>();
    const userStore = useUserStore();
    const dispatch = useDispatch();

    useEffect(() => {
        getCities();
        setDataDetail(userStore.userApp);
        console.log(userStore.userApp)
    }, []);

    const getCities = () => {
        CommonService.getCities().then(res => {
            setCitites(res.data);
        }).catch(error => {
            console.log(error);
        });
    }

    const onChangeCity = (code: string) => {
        getDistricts(code);
    }

    const getDistricts = (code: string) => {
        CommonService.getDistrictsByCityCode(code).then(res => {
            setDistricts(res.data);
        }).catch(error => {
            console.log(error);
        });
    }

    const onChangeDistrict = (code: string) => {
        getWards(code);
    }

    const getWards = (code: string) => {
        CommonService.getWardsByDistrictCode(code).then(res => {
            setWards(res.data);
        }).catch(error => {
            console.log(error);
        });
    }

    const onChangeWard = (code: string) => {

    }

    return (
        <div className="custom-container">
            <div className="content__wrapper products__content-wrapper">
                <div className="content__inner w-full">
                    <WebDirectional items={[
                        { path: ROUTE_NAME.USER_PROFILE, name: 'Hồ sơ cá nhân' }
                    ]} />
                    <div className="user__profile-wrapper grid grid-cols-12 gap-2">
                        <div className="col-span-12 md:col-span-2 mb-4">
                            <div className="tabs-control">
                                <div className="control-selectlist">
                                    <select name="" id="" className="w-full">
                                        <option value="" selected>Thông báo</option>
                                        <option value="" selected>Thông tin cá nhân</option>
                                        <option value="" selected>Lịch sử đơn hàng</option>
                                        <option value="" selected>Đổi mật khẩu</option>
                                        <option value="" hidden>Đăng ký bán hàng</option>
                                        <option value="">Đăng xuất</option>
                                    </select>
                                    <span className="flex items-center">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24"
                                            viewBox="0 0 24 24" fill="none" stroke="#333" stroke-width="1"
                                            stroke-linecap="round" stroke-linejoin="round"
                                            className="feather feather-chevron-down">
                                            <polyline points="6 9 12 15 18 9"></polyline>
                                        </svg>
                                    </span>
                                </div>
                                <ul className="control-list">
                                    <li className="mb-2">
                                        <a className="flex items-center active" href="/"
                                            style={{ whiteSpace: 'nowrap' }}>
                                            <span className="mr-2" style={{ height: '24px' }}>
                                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1" stroke-linecap="round" stroke-linejoin="round" className="feather feather-bell"><path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9"></path><path d="M13.73 21a2 2 0 0 1-3.46 0"></path></svg>
                                                <span className="point-red-quantity" style={{ top: '12px', left: '30px' }}>1</span>
                                            </span>
                                            Thông báo
                                        </a>
                                    </li>
                                    <li className="mb-2">
                                        <a className="flex items-center"
                                            href="/"
                                            style={{ whiteSpace: 'nowrap' }}>
                                            <span className="mr-2" style={{ height: '24px' }}>
                                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24"
                                                    viewBox="0 0 24 24" fill="none" stroke="#333" stroke-width="1"
                                                    stroke-linecap="round" stroke-linejoin="round"
                                                    className="feather feather-info">
                                                    <circle cx="12" cy="12" r="10"></circle>
                                                    <line x1="12" y1="16" x2="12" y2="12"></line>
                                                    <line x1="12" y1="8" x2="12.01" y2="8"></line>
                                                </svg>
                                            </span>
                                            Thông tin cá nhân
                                        </a>
                                    </li>
                                    <li className="mb-2">
                                        <a className="flex items-center" href="/"
                                            style={{ whiteSpace: 'nowrap' }}>
                                            <span className="mr-2" style={{ height: '24px' }}>
                                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24"
                                                    viewBox="0 0 24 24" fill="none" stroke="#333" stroke-width="1"
                                                    stroke-linecap="round" stroke-linejoin="round"
                                                    className="feather feather-list">
                                                    <line x1="8" y1="6" x2="21" y2="6"></line>
                                                    <line x1="8" y1="12" x2="21" y2="12"></line>
                                                    <line x1="8" y1="18" x2="21" y2="18"></line>
                                                    <line x1="3" y1="6" x2="3.01" y2="6"></line>
                                                    <line x1="3" y1="12" x2="3.01" y2="12"></line>
                                                    <line x1="3" y1="18" x2="3.01" y2="18"></line>
                                                </svg>
                                            </span>
                                            Lịch sử đơn hàng
                                        </a>
                                    </li>
                                    <li className="mb-2">
                                        <a className="flex items-center"
                                            href="/"
                                            style={{ whiteSpace: 'nowrap' }}>
                                            <span className="mr-2 mb-0" style={{ height: '24px' }}>
                                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24"
                                                    viewBox="0 0 24 24" fill="none" stroke="currentColor"
                                                    stroke-width="1" stroke-linecap="round" stroke-linejoin="round"
                                                    className="feather feather-edit-3">
                                                    <path d="M12 20h9"></path>
                                                    <path d="M16.5 3.5a2.121 2.121 0 0 1 3 3L7 19l-4 1 1-4L16.5 3.5z">
                                                    </path>
                                                </svg>
                                            </span>
                                            Đổi mật khẩu
                                        </a>
                                    </li>
                                    <li className="mb-2">
                                        <a className="flex items-center" href="/"
                                            style={{ whiteSpace: 'nowrap' }}>
                                            <span className="mr-2 mb-0" style={{ height: '24px' }}>
                                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1" stroke-linecap="round" stroke-linejoin="round" className="feather feather-log-out"><path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"></path><polyline points="16 17 21 12 16 7"></polyline><line x1="21" y1="12" x2="9" y2="12"></line></svg>
                                            </span>
                                            Đăng xuất
                                        </a>
                                    </li>
                                    <hr />
                                </ul>
                            </div>
                        </div>
                        <div className="col-span-12 md:col-span-10 mb-4">
                            <div className="tabs-content">
                                <div className="tab-content">
                                    <div className="text-center">
                                        <div className="title"><strong>THÔNG TIN CÁ NHÂN</strong></div>
                                    </div>
                                    <div className="user__info-form mx-auto">
                                        <div className="form-basic">
                                            <label>Họ tên</label>
                                            <input type="text" className="w-full" value={dataDetail ? dataDetail['fullName'] : ""} />
                                        </div>
                                        <div className="form-basic">
                                            <label>Số điện thoại</label>
                                            <div className="flex items-center">
                                                <input type="tel" className="mr-2 w-full" value="0397974403" disabled />
                                                <a href="/" className="" style={{ whiteSpace: 'nowrap', cursor: 'pointer' }}><u>Thay đổi</u></a>
                                            </div>
                                        </div>
                                        <div className="form-basic">
                                            <label>Email</label>
                                            <input type="text" className="w-full" />
                                        </div>
                                        <div className="form-basic">
                                            <label>Địa chỉ</label>
                                            <input type="text" className="w-full mb-2" value="" />
                                            <div className="flex flex-wrap">
                                                <div className="select-form w-full mb-2">
                                                    <select className="w-full" onChange={e => onChangeCity(e.target.value)}>
                                                        <option value="" disabled selected>Tỉnh/Thành...</option>
                                                        {cities.length > 0 && (
                                                            cities.map((city) => (
                                                                <option key={city.code} value={city.code}>{city.name}</option>
                                                            ))
                                                        )}
                                                    </select>
                                                    <span className="flex items-center">
                                                        <MuiIcon name={ICON_NAME.FEATHER.CHEVRON_DOWN} />
                                                    </span>
                                                </div>
                                                <div className="select-form w-full mb-2">
                                                    <select className="user-district w-full" onChange={e => onChangeDistrict(e.target.value)}>
                                                        <option value="" disabled selected>Quận/Huyện...</option>
                                                        {districts.length > 0 && (
                                                            districts.map((item) => (
                                                                <option key={item.code} value={item.code}>{item.name}</option>
                                                            ))
                                                        )}
                                                    </select>
                                                    <span className="flex items-center">
                                                        <MuiIcon name={ICON_NAME.FEATHER.CHEVRON_DOWN} />
                                                    </span>
                                                </div>
                                                <div className="select-form w-full mb-2">
                                                    <select className="user-ward w-full" onChange={e => onChangeWard(e.target.value)}>
                                                        <option value="" disabled selected>Phường/Xã...</option>
                                                        {wards.length > 0 && (
                                                            wards.map((item) => (
                                                                <option key={item.code} value={item.code}>{item.name}</option>
                                                            ))
                                                        )}
                                                    </select>
                                                    <span className="flex items-center">
                                                        <MuiIcon name={ICON_NAME.FEATHER.CHEVRON_DOWN} />
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="text-right">
                                            <button className="update-userprofile btn-black">Cập nhật</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default UserProfilePage;