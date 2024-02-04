import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { ROUTE_NAME } from "src/_cores/_enums/route-config.enum";
import { ICity, IDistrict, IWard } from "src/_cores/_interfaces";
import CommonService from "src/_cores/_services/common.service";
import UserService from "src/_cores/_services/user.service";
import { useUserStore } from "src/_cores/_store/root-store";
import { MuiIcon, WebDirectional } from "src/_shares/_components";
import { ICON_NAME } from "src/_shares/_components/mui-icon/_enums/mui-icon.enum";

const PROFILE_MENU_ITEMS = [
    {
        code: 'notifications',
        name: 'Thông báo',
        icon: ICON_NAME.FEATHER.BELL
    },
    {
        code: 'userInfo',
        name: 'Thông tin cá nhân',
        icon: ICON_NAME.FEATHER.INFO
    },
    {
        code: 'orderHistory',
        name: 'Lịch sử đơn hàng',
        icon: ICON_NAME.FEATHER.LIST
    },
    {
        code: 'changePassword',
        name: 'Đổi mật khẩu',
        icon: ICON_NAME.FEATHER.EDIT_3
    },
    {
        code: 'logout',
        name: 'Đăng xuất',
        icon: ICON_NAME.FEATHER.LOG_OUT
    },
]

const UserProfilePage = () => {
    const [selectedMenu, setSelectedMenu] = useState('userInfo');
    const [cities, setCitites] = useState<ICity[]>([]);
    const [districts, setDistricts] = useState<IDistrict[]>([]);
    const [wards, setWards] = useState<IWard[]>([]);
    const [dataDetail, setDataDetail] = useState<{ [key: string]: any }>({});
    const userStore = useUserStore();
    const dispatch = useDispatch();

    useEffect(() => {
        getCities();
        getUserInfo();
    }, []);

    const getUserInfo = async () => {
        const result = await UserService.getUserInfo() as any;
        if (result) {
            setDataDetail(result);
            if (result.cityCode) {
                getDistricts(result.cityCode);
            }
            if (result.wardCode) {
                getWards(result.districtCode);
            }
        }
    }

    const getCities = () => {
        CommonService.getCities().then(res => {
            setCitites(res.data);
        }).catch(error => {
            console.log(error);
        });
    }

    const onChangeCity = (code: string) => {
        const _dataDetail = {
            ...dataDetail,
            cityCode: code,
            districtCode: '',
            wardCode: ''
        }
        setDataDetail(_dataDetail);
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
        const _dataDetail = {
            ...dataDetail,
            districtCode: code,
            wardCode: ''
        }
        setDataDetail(_dataDetail);
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
        onChangeFieldValue('wardCode', code);
    }

    const onChangeFieldValue = (field: string, value: any) => {
        setDataDetail({ ...dataDetail, [field]: value });
    }

    const update = async () => {
        const _param = {
            ...dataDetail,
            email: dataDetail['mail']
        }
        const res = await UserService.updateUser(_param) as any;
        if (res.isSucceed) {
            alert("cập nhật thành công");
        }
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
                                        {PROFILE_MENU_ITEMS.map(_ => (
                                            <option key={_.code} value="">{_.name}</option>
                                        ))}
                                    </select>
                                    <span className="flex items-center">
                                        <MuiIcon name={ICON_NAME.FEATHER.CHEVRON_DOWN} />
                                    </span>
                                </div>
                                <ul className="control-list">
                                    {PROFILE_MENU_ITEMS.map(_ => (
                                        <li key={_.code} className="mb-2">
                                            <a className={`flex items-center ${_.code === selectedMenu && 'active'}`} href="/"
                                                style={{ whiteSpace: 'nowrap' }}>
                                                <span className="mr-2 relative" style={{ height: '24px' }}>
                                                    <MuiIcon name={_.icon} />
                                                    {_.code === 'notifications' && (
                                                        <span className="point-red-quantity" style={{ top: '12px', left: '20px' }}>1</span>
                                                    )}
                                                </span>
                                                {_.name}
                                            </a>
                                        </li>
                                    ))}
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
                                            <input type="text" className="w-full" value={dataDetail ? dataDetail['fullName'] : ""} onChange={(e) => onChangeFieldValue('fullName', e.target.value)} />
                                        </div>
                                        <div className="form-basic">
                                            <label>Số điện thoại</label>
                                            <div className="flex items-center">
                                                <input type="tel" className="mr-2 w-full" value={dataDetail ? dataDetail['phone'] : ""} disabled />
                                                <a href="/" className="" style={{ whiteSpace: 'nowrap', cursor: 'pointer' }}><u>Thay đổi</u></a>
                                            </div>
                                        </div>
                                        <div className="form-basic">
                                            <label>Email</label>
                                            <input type="text" className="w-full" value={dataDetail ? dataDetail['mail'] : ""} onChange={(e) => onChangeFieldValue('mail', e.target.value)} />
                                        </div>
                                        <div className="form-basic">
                                            <label>Địa chỉ</label>
                                            <input type="text" className="w-full mb-2" value={dataDetail ? dataDetail['address'] : ""} />
                                            <div className="flex flex-wrap">
                                                <div className="select-form w-full mb-2">
                                                    <select className="w-full" onChange={e => onChangeCity(e.target.value)}>
                                                        <option value="" disabled selected={!dataDetail || !dataDetail['cityCode']}>Tỉnh/Thành...</option>
                                                        {cities.length > 0 && (
                                                            cities.map((city) => (
                                                                <option key={city.code} value={city.code} selected={city.code === (dataDetail ? dataDetail['cityCode'] : "")}>{city.name}</option>
                                                            ))
                                                        )}
                                                    </select>
                                                    <span className="flex items-center">
                                                        <MuiIcon name={ICON_NAME.FEATHER.CHEVRON_DOWN} />
                                                    </span>
                                                </div>
                                                <div className="select-form w-full mb-2">
                                                    <select className="user-district w-full" onChange={e => onChangeDistrict(e.target.value)}>
                                                        <option value="" disabled selected={!dataDetail || !dataDetail['districtCode']}>Quận/Huyện...</option>
                                                        {districts.length > 0 && (
                                                            districts.map((item) => (
                                                                <option key={item.code} value={item.code} selected={item.code === (dataDetail ? dataDetail['districtCode'] : "")}>{item.name}</option>
                                                            ))
                                                        )}
                                                    </select>
                                                    <span className="flex items-center">
                                                        <MuiIcon name={ICON_NAME.FEATHER.CHEVRON_DOWN} />
                                                    </span>
                                                </div>
                                                <div className="select-form w-full mb-2">
                                                    <select className="user-ward w-full" onChange={e => onChangeWard(e.target.value)}>
                                                        <option value="" disabled selected={!dataDetail || !dataDetail['wardCode']}>Phường/Xã...</option>
                                                        {wards.length > 0 && (
                                                            wards.map((item) => (
                                                                <option key={item.code} value={item.code} selected={item.code === (dataDetail ? dataDetail['wardCode'] : "")}>{item.name}</option>
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
                                            <button className="update-userprofile btn-black" onClick={update}>Cập nhật</button>
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