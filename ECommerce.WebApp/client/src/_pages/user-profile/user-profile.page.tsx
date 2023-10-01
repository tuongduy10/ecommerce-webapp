import { WebDirectional } from "src/_shares/_components";

const UserProfilePage = () => {
    return (
        <div className="custom-container">
            <div className="content__wrapper products__content-wrapper">
                <div className="content__inner w-full">
                    <WebDirectional items={[{ path: ' ', name: 'hồ sơ cá nhân' }]} />
                    <hr className="my-4" />
                    <div className="user__profile-wrapper grid grid-cols-6 gap-4">
                        <div className="col-span-6 md:col-span-1 mb-4">
                            <div className="tabs-control">
                                <div className="control-selectlist">
                                    <select name="">
                                        <option>Thông báo</option>
                                        <option selected>Thông tin cá nhân</option>
                                        <option>Lịch sử đơn hàng</option>
                                        <option>Đổi mật khẩu</option>
                                        <option>Đăng xuất</option>
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
                                        <a className="flex items-center active" href="" style={{ whiteSpace: 'nowrap' }}>
                                            <span className="mr-2 h-[24px]">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1" stroke-linecap="round" stroke-linejoin="round" className="feather feather-bell"><path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9"></path><path d="M13.73 21a2 2 0 0 1-3.46 0"></path></svg>
                                                <span className="point-red-quantity" style={{ top: '12px', left: '30px', }}>12</span>
                                            </span>
                                            Thông báo
                                        </a>
                                    </li>
                                    <li className="mb-2">
                                        <a className="flex items-center" href="" style={{ whiteSpace: 'nowrap' }}>
                                            <span className="mr-2 h-[24px]">
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
                                        <a className="flex items-center" href=""
                                            style={{ whiteSpace: 'nowrap' }}>
                                            <span className="mr-2 h-[24px]">
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
                                        <a className="flex items-center" href="" style={{ whiteSpace: 'nowrap' }}>
                                            <span className="mr-2 mb-0 h-[24px]">
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
                                        <a className="flex items-center" href="" style={{ whiteSpace: 'nowrap' }}>
                                            <span className="mr-2 mb-0 h-[24px]">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1" stroke-linecap="round" stroke-linejoin="round" className="feather feather-log-out"><path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"></path><polyline points="16 17 21 12 16 7"></polyline><line x1="21" y1="12" x2="9" y2="12"></line></svg>
                                            </span>
                                            Đăng xuất
                                        </a>
                                    </li>
                                    <hr />
                                </ul>
                            </div>
                        </div>
                        <div className="col-span-6 md:col-span-5 mb-4">
                            <div className="tabs-content">
                                <div className="tab-content">
                                    <div className="text-center">
                                        <div className="title"><strong>THÔNG TIN CÁ NHÂN</strong></div>
                                    </div>
                                    <div className="user__info-form mx-auto">
                                        <div className="mb-4">
                                            <label className="block mb-2">Họ tên</label>
                                            <input className="bg-gray-50 border border-gray-300 text-gray-900 rounded-lg focus:ring-[--blue-dior] focus:border-[--blue-dior] block w-full p-2" />
                                        </div>
                                        <div className="mb-4">
                                            <label className="block mb-2">Số điện thoại</label>
                                            <div className="flex items-center">
                                                <input className="bg-gray-50 border border-gray-300 text-gray-900 rounded-lg focus:ring-[--blue-dior] focus:border-[--blue-dior] block w-full p-2 mr-2" disabled />
                                                <a href="" className="" style={{ whiteSpace: 'nowrap', cursor: 'pointer' }}><u>Thay đổi</u></a>
                                            </div>
                                        </div>
                                        <div className="mb-4">
                                            <label className="block mb-2">Email</label>
                                            <input className="bg-gray-50 border border-gray-300 text-gray-900 rounded-lg focus:ring-[--blue-dior] focus:border-[--blue-dior] block w-full p-2" />
                                        </div>
                                        <div className="mb-4">
                                            <label className="block mb-2">Địa chỉ</label>
                                            <div className="d-flex flex-wrap">
                                                <input className="bg-gray-50 border border-gray-300 text-gray-900 rounded-lg focus:ring-[--blue-dior] focus:border-[--blue-dior] block w-full p-2" />
                                                <div className="select-form w-full mb-2">
                                                    <select name="city" className="user-city form-control">
                                                        <option disabled selected>Tỉnh/Thành...</option>
                                                    </select>
                                                    <span className="flex items-center">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width={24} height={24} viewBox="0 0 24 24" fill="none" stroke="#333" strokeWidth={1} strokeLinecap="round" strokeLinejoin="round" className="feather feather-chevron-down">
                                                            <polyline points="6 9 12 15 18 9" />
                                                        </svg>
                                                    </span>
                                                </div>
                                                <div className="select-form w-full mb-2">
                                                    <select name="district" className="user-district form-control">
                                                        <option disabled selected>Quận/Huyện...</option>
                                                    </select>
                                                    <span className="flex items-center">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width={24} height={24} viewBox="0 0 24 24" fill="none" stroke="#333" strokeWidth={1} strokeLinecap="round" strokeLinejoin="round" className="feather feather-chevron-down">
                                                            <polyline points="6 9 12 15 18 9" />
                                                        </svg>
                                                    </span>
                                                </div>
                                                <div className="select-form w-full mb-2">
                                                    <select name="ward" className="user-ward form-control">
                                                        <option disabled selected>Phường/Xã...</option>
                                                    </select>
                                                    <span className="flex items-center">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width={24} height={24} viewBox="0 0 24 24" fill="none" stroke="#333" strokeWidth={1} strokeLinecap="round" strokeLinejoin="round" className="feather feather-chevron-down">
                                                            <polyline points="6 9 12 15 18 9" />
                                                        </svg>
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
                </div >
            </div >
        </div >
    )
}

export default UserProfilePage;