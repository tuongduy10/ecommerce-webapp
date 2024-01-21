import { Autocomplete, Box, Button, Checkbox, CssBaseline, Grid, TextField, ThemeProvider, Typography, createTheme } from "@mui/material";
import CheckBoxOutlineBlankIcon from '@mui/icons-material/CheckBoxOutlineBlank';
import CheckBoxIcon from '@mui/icons-material/CheckBox';
import { useNavigate } from "react-router-dom";
import { ADMIN_ROUTE_NAME } from "src/_cores/_enums/route-config.enum";
import { useEffect, useState } from "react";
import CommonService from "src/_cores/_services/common.service";
import { ICity, IDistrict, IWard } from "src/_cores/_interfaces/common.interface";
import UserService from "src/_cores/_services/user.service";
import { IShop } from "src/_cores/_interfaces/user.interface";

const icon = <CheckBoxOutlineBlankIcon fontSize="small" />;
const checkedIcon = <CheckBoxIcon fontSize="small" />;

const defaultTheme = createTheme();

export default function UserDetail() {
    const searchParams = new URLSearchParams(window.location.search);
    const userId = Number(searchParams.get('id')) || -1;

    const [dataDetail, setDataDetail] = useState<{ [key: string]: any }>({});
    const [shops, setShops] = useState<IShop[]>([]);
    const [cities, setCitites] = useState<ICity[]>([]);
    const [districts, setDistricts] = useState<IDistrict[]>([]);
    const [wards, setWards] = useState<IWard[]>([]);

    const navigate = useNavigate();

    useEffect(() => {
        getCities();
        getShops();
        if (userId > -1) {
            getDetail(userId);
        }
    }, [userId]);

    const backToList = () => {
        navigate({
            pathname: ADMIN_ROUTE_NAME.MANAGE_USER
        });
    }

    const getDetail = (id: number) => {
        UserService.getUser(id).then((res: any) => {
            if (res.isSucceed) {
                const _dataDetail = res.data;
                setDataDetail({
                    ..._dataDetail,
                    shopIds: _dataDetail.shops.map((_: any) => _.shopId)
                });
                getDistricts(_dataDetail.userCityCode);
                getWards(_dataDetail.userDistrictCode);
            }
        }).catch(error => {
            console.log(error);
        });
    }

    const getShops = () => {
        UserService.getShops().then(res => {
            setShops(res.data);
        }).catch(error => {
            console.log(error);
        });
    }

    const getCities = () => {
        CommonService.getCities().then(res => {
            setCitites(res.data);
        }).catch(error => {
            console.log(error);
        });
    }

    const onChangeCity = (province: ICity | null) => {
        const provinceCode = province?.code ?? null;
        let _dataDetail = dataDetail;
        _dataDetail['cityCode'] = provinceCode;
        setDataDetail(_dataDetail);
        if (!provinceCode) {
            _dataDetail['districtCode'] = "";
            setDataDetail(_dataDetail);
            setDistricts([]);
        } else {
            getDistricts(provinceCode);
        }
    }

    const getDistricts = (code: string) => {
        CommonService.getDistrictsByCityCode(code).then(res => {
            setDistricts(res.data);
        }).catch(error => {
            console.log(error);
        });
    }

    const onChangeDistrict = (district: IDistrict | null) => {
        const districtCode = district?.code ?? "";
        let _dataDetail = dataDetail;
        _dataDetail['districtCode'] = districtCode;
        setDataDetail({ ...dataDetail, userDistrictCode: districtCode });
        if (!districtCode) {
            _dataDetail["wardCode"] = ""
            setDataDetail(_dataDetail);
            setWards([]);
            return;
        }
        getWards(districtCode);
    }

    const getWards = (code: string) => {
        CommonService.getWardsByDistrictCode(code).then(res => {
            setWards(res.data);
        }).catch(error => {
            console.log(error);
        });
    }

    const onChangeWard = (ward: IWard | null) => {
        const wardCode = ward?.code ?? "";
        setDataDetail({ ...dataDetail, wardCode: wardCode });
    }

    const onChangeFieldValue = (field: string, value: any) => {
        if (dataDetail[field]) {
            setDataDetail({ ...dataDetail, [field]: value });
        }
    }

    const onChangeShops = (shops: IShop[]) => {
        const _shopIds = shops.map(_ => _.id);
        setDataDetail({ ...dataDetail, shopIds: _shopIds });
    }

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        const _param = {
            id: dataDetail['userId'] ?? -1,
            fullName: form.get('userFullName'),
            email: form.get('userMail'),
            cityCode: dataDetail['userCityCode'] ?? "",
            districtCode: dataDetail['userDistrictCode'] ?? "",
            wardCode: dataDetail['userWardCode'] ?? "",
            address: form.get('userAddress'),
            phone: form.get('userPhone'),
            password: form.get('password'),
            rePassword: form.get('rePassword'),
            shopIds: dataDetail.shopIds ?? [],
        }
        UserService.saveSeller(_param).then(res => {
            backToList();
        }).catch(error => {
            console.log(error);
        });
    }

    return (
        <ThemeProvider theme={defaultTheme}>
            <Typography component="h1" variant="h5" sx={{ mb: 3 }}>
                Tài khoản bán hàng
            </Typography>
            <Box component="form" noValidate onSubmit={handleSubmit}>
                <Grid container spacing={2}>
                    <Grid item xs={7}>
                        <CssBaseline />
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'column',
                            }}
                        >
                            <Typography variant="subtitle1" gutterBottom>
                                Thông tin cơ bản
                            </Typography>
                            <Grid container spacing={2} sx={{ mb: 3 }}>
                                <Grid item xs={12} sm={8}>
                                    <TextField
                                        value={dataDetail['userFullName']}
                                        onChange={(event) => onChangeFieldValue('userFullName', event?.target.value)}
                                        InputLabelProps={{ shrink: true }}
                                        autoComplete='off'
                                        name="userFullName"
                                        required
                                        fullWidth
                                        size="small"
                                        label="Họ tên"
                                        autoFocus
                                    />
                                </Grid>
                                <Grid item xs={12} sm={8}>
                                    <TextField
                                        value={dataDetail['userMail']}
                                        onChange={(event) => onChangeFieldValue('userMail', event?.target.value)}
                                        InputLabelProps={{ shrink: true }}
                                        autoComplete='off'
                                        name="userMail"
                                        required
                                        fullWidth
                                        size="small"
                                        label="Email"
                                        autoFocus
                                    />
                                </Grid>
                                <Grid item xs={12} sm={8}>
                                    <Autocomplete
                                        size="small"
                                        disablePortal
                                        options={cities}
                                        value={dataDetail['userCityCode'] ? cities.find(_ => _.code === dataDetail['userCityCode']) : null}
                                        onChange={(event, city) => onChangeCity(city)}
                                        getOptionLabel={(city) => `${city.name}`}
                                        renderOption={(props, city) => <li {...props}>{city.name}</li>}
                                        renderInput={(params) => <TextField {...params} name="city" label="Thành phố" autoComplete="chrome-off" />}
                                    />
                                </Grid>
                                <Grid item xs={12} sm={8}>
                                    <Autocomplete
                                        size="small"
                                        disablePortal
                                        options={districts}
                                        value={dataDetail['userDistrictCode'] ? districts.find(_ => _.code === dataDetail['userDistrictCode']) : null}
                                        onChange={(event, district) => onChangeDistrict(district)}
                                        getOptionLabel={(district) => `${district.name}`}
                                        renderOption={(props, district) => <li {...props}>{district.name}</li>}
                                        renderInput={(params) => <TextField {...params} name="district" label="Quận/Huyện" autoComplete="off" />}
                                    />
                                </Grid>
                                <Grid item xs={12} sm={8}>
                                    <Autocomplete
                                        size="small"
                                        disablePortal
                                        options={wards}
                                        value={dataDetail['userWardCode'] ? wards.find(_ => _.code === dataDetail['userWardCode']) : null}
                                        onChange={(event, ward) => onChangeWard(ward)}
                                        getOptionLabel={(ward) => `${ward.name}`}
                                        renderOption={(props, ward) => <li {...props}>{ward.name}</li>}
                                        renderInput={(params) => <TextField {...params} name="ward" label="Phường xã" autoComplete="off" />}
                                    />
                                </Grid>
                                <Grid item xs={12} sm={8}>
                                    <TextField
                                        value={dataDetail['userAddress']}
                                        onChange={(event) => onChangeFieldValue('userAddress', event?.target.value)}
                                        InputLabelProps={{ shrink: true }}
                                        autoComplete='off'
                                        name="userAddress"
                                        required
                                        fullWidth
                                        size="small"
                                        label="Địa chỉ"
                                        autoFocus
                                    />
                                </Grid>
                            </Grid>

                            <Typography variant="subtitle1" gutterBottom>
                                Thông tin đăng nhập
                            </Typography>
                            <Grid container spacing={2} sx={{ mb: 3 }}>
                                <Grid item xs={12} sm={8}>
                                    <TextField
                                        value={dataDetail['userPhone']}
                                        onChange={(event) => onChangeFieldValue('userPhone', event?.target.value)}
                                        InputLabelProps={{ shrink: true }}
                                        autoComplete='off'
                                        name="userPhone"
                                        required
                                        fullWidth
                                        size="small"
                                        label="Số điện thoại"
                                        autoFocus
                                    />
                                </Grid>
                                <Grid item xs={12} sm={8}>
                                    <TextField
                                        value={dataDetail['password']}
                                        onChange={(event) => onChangeFieldValue('password', event?.target.value)}
                                        InputLabelProps={{ shrink: true }}
                                        autoComplete='off'
                                        name="password"
                                        required={!dataDetail['userId']}
                                        fullWidth
                                        size="small"
                                        label={`Mật khẩu ${dataDetail['userId'] ? "mới" : ""}`}
                                        autoFocus
                                    />
                                </Grid>
                                <Grid item xs={12} sm={8}>
                                    <TextField
                                        value={dataDetail['rePassword']}
                                        onChange={(event) => onChangeFieldValue('rePassword', event?.target.value)}
                                        InputLabelProps={{ shrink: true }}
                                        autoComplete='off'
                                        name="rePassword"
                                        required={!dataDetail['userId']}
                                        fullWidth
                                        size="small"
                                        label={`Nhập lại mật khẩu ${dataDetail['userId'] ? "mới" : ""}`}
                                        autoFocus
                                    />
                                </Grid>
                            </Grid>
                            <Typography variant="subtitle1" gutterBottom>
                                Thông tin bán hàng
                            </Typography>
                            <Grid container spacing={2} sx={{ mb: 3 }}>
                                <Grid item xs={12} sm={8}>
                                    <Autocomplete
                                        multiple
                                        id="checkboxes-tags-demo"
                                        options={shops}
                                        disableCloseOnSelect
                                        getOptionLabel={(shop) => `${shop.id} - ${shop.name}`}
                                        getOptionDisabled={(option) => option.user !== null && option.user.id !== dataDetail['userId']}
                                        value={shops.length > 0 && dataDetail['shopIds'] ? shops.filter((shop) => dataDetail['shopIds'].includes(shop.id)) : []}
                                        renderOption={(props, shop, { selected }) => (
                                            <li {...props}>
                                                <Checkbox
                                                    icon={icon}
                                                    checkedIcon={checkedIcon}
                                                    style={{ marginRight: 8 }}
                                                    checked={selected}
                                                    size="small"
                                                />
                                                {shop.name}
                                            </li>
                                        )}
                                        renderInput={(params) => (
                                            <TextField {...params} label="Cửa hàng" placeholder="Chọn cửa hàng" size="small" />
                                        )}
                                        onChange={(event, shops) => onChangeShops(shops)}
                                        style={{ width: 500 }}
                                        size="small"
                                    />
                                </Grid>
                            </Grid>
                            <Grid item xs={12} sm={12}>
                                <Button sx={{ marginRight: 2 }} variant="outlined" onClick={backToList}>
                                    Quay lại
                                </Button>
                                <Button type="submit" variant="contained">
                                    Cập nhật
                                </Button>
                            </Grid>
                        </Box>
                    </Grid>
                </Grid>
            </Box>
        </ThemeProvider>
    );
}