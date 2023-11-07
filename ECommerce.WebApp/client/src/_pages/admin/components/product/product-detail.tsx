import { Autocomplete, Box, Button, Checkbox, Chip, CssBaseline, FormControlLabel, FormLabel, Grid, Stack, TextField, ThemeProvider, Typography, createTheme } from "@mui/material";
import { Editor } from '@tinymce/tinymce-react';
import React, { Fragment, useEffect, useRef, useState } from "react";
import { GlobalConfig } from "src/_configs/global.config";
import UploadInput from "src/_shares/_components/input/upload-input";
import { FormatHelper } from "src/_shares/_helpers/format-helper";
import PriceInput from "src/_shares/_components/input/price-input";
import { IShop } from "src/_cores/_interfaces/user.interface";
import { IAttribute, IBrand, IOption, ISubCategory } from "src/_cores/_interfaces/inventory.interface";
import UserService from "src/_cores/_services/user.service";
import InventoryService from "src/_cores/_services/inventory.service";

const _tempAttributes = [
    {
        "id": 14,
        "name": "Chất liệu",
        "value": null
    },
    {
        "id": 33,
        "name": "Trọng lượng",
        "value": null
    },
    {
        "id": 40,
        "name": "Hãng",
        "value": null
    },
    {
        "id": 42,
        "name": "Dung tích",
        "value": null
    },
    {
        "id": 43,
        "name": "Công suất",
        "value": null
    },
    {
        "id": 44,
        "name": "Điện áp",
        "value": null
    },
    {
        "id": 45,
        "name": "Kích thước",
        "value": null
    },
    {
        "id": 46,
        "name": "Phiên bản",
        "value": null
    },
    {
        "id": 47,
        "name": "Hướng dẫn sử dụng",
        "value": null
    },
    {
        "id": 48,
        "name": "Màn hình hiển thị",
        "value": null
    },
    {
        "id": 49,
        "name": "Đi kèm",
        "value": null
    },
    {
        "id": 50,
        "name": "Chân cắm điện",
        "value": null
    }
]

const defaultTheme = createTheme();
type DataDetail = {
    shops: IShop[],
    brands: IBrand[],
    subCategories: ISubCategory[],
    attributes: IAttribute[],
    options: IOption[],
}
type NewOptionValue = {
    id: number;
    values: string[]
}

const ProductDetail = () => {
    const [dataDetail, setDataDetail] = useState<DataDetail>({
        shops: [],
        brands: [],
        subCategories: [],
        attributes: [],
        options: []
    });
    const [selectedBrandId, setSelectedBrandId] = useState<number>(-1);
    const [selectedShopId, setSelectedShopId] = useState<number>(-1);
    const [selectedSubCategoryId, setSelectedSubCategoryId] = useState<number>(-1);
    const descriptionRef = useRef<any>(null);
    const sizeGuideRef = useRef<any>(null);
    const [newOptionValues, setNewOptionValues] = useState<NewOptionValue[]>([]);
    const [systemFileNames, setSystemFileNames] = useState<string[]>([]);
    const [userFileNames, setUserFileNames] = useState<string[]>([]);

    useEffect(() => {
        getShops();
    }, []);

    const getShops = () => {
        UserService.getShops().then(res => {
            if (res?.data) {
                setDataDetail({ ...dataDetail, shops: res.data });
            }
        }).catch(error => {
            alert(error);
        });
    }

    const getBrands = (shopId: number) => {
        InventoryService.getBrands({ shopId: shopId }).then(res => {
            if (res?.data) {
                setDataDetail({ ...dataDetail, brands: res.data });
            }
        }).catch(error => {
            alert(error);
        });
    }

    const getSubCategories = (brandId: number) => {
        InventoryService.getSubCategories({ brandId: brandId }).then(res => {
            if (res?.data) {
                setDataDetail({ ...dataDetail, subCategories: res.data });
            }
        }).catch(error => {
            alert(error);
        });
    }

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        const param = {
            code: form.get('code'),
            productName: form.get('productName'),
            priceImport: FormatHelper.getNumber(form.get('priceImport')?.toString()),
            priceForSeller: FormatHelper.getNumber(form.get('priceForSeller')?.toString()),
            priceForAvailable: FormatHelper.getNumber(form.get('priceForAvailable')?.toString()),
            pricePreOrder: FormatHelper.getNumber(form.get('pricePreOrder')?.toString()),
            discountPreOrder: FormatHelper.getNumber(form.get('discountPreOrder')?.toString()),
            discountAvailable: FormatHelper.getNumber(form.get('discountAvailable')?.toString()),
            discountPercent: form.get('discountPercent'),
            isNew: form.get('isNew')?.toString() === 'on',
            isHighlight: form.get('isHighlight')?.toString() === 'on',
            isLegal: form.get('isLegal')?.toString() === 'on',
            repay: form.get('repay'),
            delivery: form.get('delivery'),
            insurance: form.get('insurance'),
            link: form.get('link'),
            note: form.get('note'),
            shopId: selectedShopId,
            brandId: selectedBrandId,
            subCategoryId: selectedSubCategoryId,
            attributes: dataDetail.attributes,
            description: descriptionRef.current.getContent(),
            // options
            currentOptions: dataDetail.options.length > 0 ? dataDetail.options.map(option => option.id) : [],
            newOptions: newOptionValues,
            systemFileNames: systemFileNames,
            userFileNames: userFileNames
        }
        console.log(param);
    };

    const onChangeShop = (value: IShop) => {
        setSelectedShopId(value?.id || -1);
        if (!value) {
            setSelectedShopId(-1);
            setDataDetail({
                ...dataDetail,
                brands: [],
                subCategories: [],
                attributes: [],
                options: [],
            });
            return;
        }
        getBrands(value.id)
    }

    const onChangeBrand = (value: IBrand) => {
        setSelectedBrandId(value?.id || -1);
        if (!value) {
            setSelectedSubCategoryId(-1);
            setDataDetail({
                ...dataDetail,
                subCategories: [],
                attributes: [],
                options: [],
            });
            return;
        }
        getSubCategories(value.id)
    }

    const onChangeSubCategory = (value: ISubCategory) => {
        setSelectedSubCategoryId(value?.id || -1);
        if (!value) {
            setDataDetail({
                ...dataDetail,
                options: [],
                attributes: []
            });
            return;
        }
        if (value.optionList) {
            const _optionList = value.optionList;
            _optionList.forEach((option) => {
                option.values = option.values?.filter(value => value.isBase)
            });
            setDataDetail({
                ...dataDetail,
                options: _optionList,
                attributes: _tempAttributes
            });
        }
    }

    const onBlurOption = (idx: number, value: string) => {
        if (!newOptionValues[idx]) {
            newOptionValues[idx] = {
                id: dataDetail.options[idx].id,
                values: []
            }
        }
        if (value && !newOptionValues[idx].values.includes(value)) {
            newOptionValues[idx].values.push(value);
        }
        setNewOptionValues([...newOptionValues]);
    }

    const onBlurAttribute = (idx: number, value: string) => {
        if (dataDetail.attributes[idx]) {
            dataDetail.attributes[idx].value = value ?? '';
            setDataDetail({
                ...dataDetail,
                attributes: [...dataDetail.attributes]
            });
        }
    }

    const deleteCurrentOptionValue = (idx: number, childIdx: number) => {
        if (dataDetail.options[idx]) {
            dataDetail.options[idx].values.splice(childIdx, 1);
        }
        setDataDetail({
            ...dataDetail,
            options: [...dataDetail.options]
        })
    }

    const deleteNewOptionValue = (idx: number, childIdx: number) => {
        if (newOptionValues[idx]) {
            newOptionValues[idx].values.splice(childIdx, 1);
        }
        setNewOptionValues([...newOptionValues]);
    }

    return (
        <ThemeProvider theme={defaultTheme}>
            <Typography component="h1" variant="h5">
                Thêm sản phẩm
            </Typography>
            <Box component="form" noValidate onSubmit={handleSubmit} sx={{ mt: 3 }}>
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
                            <Grid container spacing={2}>
                                <Grid item xs={12} sm={5}>
                                    <TextField
                                        autoComplete='off'
                                        name="code"
                                        required
                                        fullWidth
                                        size="small"
                                        label="Mã sản phẩm"
                                        autoFocus
                                    />
                                </Grid>
                                <Grid item xs={12} sm={5}>
                                    <TextField
                                        required
                                        fullWidth
                                        size="small"
                                        label="Tên sản phẩm"
                                        name="productName"
                                        autoComplete='off'
                                    />
                                </Grid>
                                <Grid item xs={12} sm={2}>
                                    <TextField type="number" name="stock" label="Kho" size="small" fullWidth />
                                </Grid>
                                <Grid item xs={12} sm={12}>
                                    <PriceInput label="Giá nhập hàng" name='priceImport' />
                                </Grid>
                                <Grid item xs={12} sm={12}>
                                    <PriceInput label="Giá cho Seller" name='priceForSeller' />
                                </Grid>
                                <Grid item xs={12} sm={12}>
                                    <PriceInput label="Giá có sẵn" name='priceAvailable' />
                                </Grid>
                                <Grid item xs={12} sm={12}>
                                    <PriceInput label="Giá đặt trước" name='pricePreOrder' />
                                </Grid>
                                <Grid item xs={12} sm={2}>
                                    <FormControlLabel
                                        control={<>
                                            <Checkbox name="isDicountPercent" color="primary" />
                                            <TextField type="number" name="discountPercent" label="% Giảm" size="small" fullWidth />
                                        </>}
                                        label=""
                                        sx={{ marginRight: 0 }}
                                    />
                                </Grid>
                                <Grid item xs={12} sm={5}>
                                    <PriceInput label="Giá giảm đặt trước" name='discountPreOrder' />
                                </Grid>
                                <Grid item xs={12} sm={5}>
                                    <PriceInput label="Giá giảm có sẵn" name='discountAvailable' />
                                </Grid>
                                <Grid item xs={12}>
                                    <FormControlLabel control={<Checkbox name="isNew" color="primary" />} label="Mới" />
                                    <FormControlLabel control={<Checkbox name="isHighlight" color="primary" />} label="Nổi bật" />
                                    <FormControlLabel control={<Checkbox name="isLegal" color="primary" />} label="Sản phẩm chính hãng" />
                                </Grid>
                                <Grid item xs={12} sm={4}>
                                    <TextField
                                        name="repay"
                                        fullWidth
                                        size="small"
                                        label="Đổi trả"
                                        autoFocus
                                        autoComplete='off'
                                    />
                                </Grid>
                                <Grid item xs={12} sm={4}>
                                    <TextField
                                        name="delivery"
                                        fullWidth
                                        size="small"
                                        label="Giao hàng"
                                        autoFocus
                                        autoComplete='off'
                                    />
                                </Grid>
                                <Grid item xs={12} sm={4}>
                                    <TextField
                                        name="insurance"
                                        fullWidth
                                        size="small"
                                        label="Bảo hành"
                                        autoFocus
                                        autoComplete='off'
                                    />
                                </Grid>
                                <Grid item xs={12} sm={12}>
                                    {dataDetail.shops.length > 0 && (
                                        <Autocomplete
                                            size="small"
                                            disablePortal
                                            options={dataDetail.shops}
                                            onChange={(event, value) => value && onChangeShop(value)}
                                            getOptionLabel={(option) => `${option.name}`}
                                            renderOption={(props, option) => <li {...props}>{option.name}</li>}
                                            renderInput={(params) => <TextField {...params} name="shop" label="Cửa hàng" />}
                                        />
                                    )}
                                </Grid>
                                <Grid item xs={12} sm={12}>
                                    {dataDetail.brands.length > 0 && (
                                        <Autocomplete
                                            size="small"
                                            disablePortal
                                            options={dataDetail.brands}
                                            onChange={(event, value) => value && onChangeBrand(value)}
                                            getOptionLabel={(option) => `${option.name}`}
                                            renderOption={(props, option) => <li {...props}>{option.name}</li>}
                                            renderInput={(params) => <TextField {...params} label="Thương hiệu" />}
                                        />
                                    )}
                                </Grid>
                                <Grid item xs={12} sm={12}>
                                    {dataDetail.subCategories.length > 0 && (
                                        <Autocomplete
                                            size="small"
                                            disablePortal
                                            options={dataDetail.subCategories}
                                            onChange={(event, value) => value && onChangeSubCategory(value)}
                                            getOptionLabel={(option) => `${option.name}`}
                                            renderOption={(props, option) => <li {...props}>{option.name}</li>}
                                            renderInput={(params) => <TextField {...params} label="Loại sản phẩm" />}
                                        />
                                    )}
                                </Grid>
                            </Grid>
                        </Box>
                    </Grid>
                    <Grid item xs={5}>
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'column',
                            }}
                        >
                            <Grid container spacing={2}>
                                {dataDetail.attributes.length > 0 && (
                                    <Grid item xs={12} sm={12}>
                                        <Typography variant="subtitle1" gutterBottom>
                                            Thông tin chi tiết
                                        </Typography>
                                        <Grid container spacing={2}>
                                            {dataDetail.attributes.map((attribute, idx) => (
                                                <Grid key={`attribute-${attribute.id}`} item xs={12} sm={6}>
                                                    <TextField
                                                        fullWidth
                                                        size="small"
                                                        label={attribute.name}
                                                        autoComplete='off'
                                                        onBlur={(event) => onBlurAttribute(idx, event.target.value)}
                                                    />
                                                </Grid>
                                            ))}
                                        </Grid>
                                    </Grid>
                                )}
                                {dataDetail.options.length > 0 && (
                                    <Grid item xs={12} sm={12}>
                                        <Typography variant="subtitle1" gutterBottom>
                                            Thông tin tùy chọn
                                        </Typography>
                                        <Grid item xs={12} sm={6}>
                                            {dataDetail.options.map((option, idx) => (
                                                <Fragment key={`option-${option.id}`}>
                                                    <TextField
                                                        sx={{ marginBottom: 1 }}
                                                        fullWidth
                                                        size="small"
                                                        label={option.name}
                                                        autoComplete='off'
                                                        onBlur={(event) => onBlurOption(idx, event.target.value)}
                                                    />
                                                    <Stack spacing={{ xs: 1, sm: 1 }} direction="row" useFlexGap flexWrap="wrap">
                                                        {option.values && option.values?.length > 0 && (
                                                            option.values.map((value, childIdx) => (
                                                                <Chip
                                                                    key={`option-value-${value.id}`}
                                                                    label={value.name}
                                                                    variant="outlined"
                                                                    onDelete={() => deleteCurrentOptionValue(idx, childIdx)}
                                                                />
                                                            ))
                                                        )}
                                                        {newOptionValues[idx] && newOptionValues[idx].values.length > 0 && (
                                                            newOptionValues[idx].values.map((value, childIdx) => (
                                                                <Chip
                                                                    key={`new-option-value-${childIdx}`}
                                                                    label={value}
                                                                    variant="outlined"
                                                                    onDelete={() => deleteNewOptionValue(idx, childIdx)}
                                                                />
                                                            ))
                                                        )}
                                                    </Stack>
                                                </Fragment>
                                            ))}
                                        </Grid>
                                    </Grid>
                                )}
                            </Grid>
                        </Box>
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container spacing={2}>
                            <Grid item xs={12} sm={12}>
                                <Typography variant="subtitle1" gutterBottom>
                                    Mô tả
                                </Typography>
                                <Editor
                                    onInit={(evt, editor) => descriptionRef.current = editor}
                                    apiKey={GlobalConfig.TINY_KEY}
                                    init={{
                                        height: 500,
                                        plugins: GlobalConfig.TINY_PLUGINS,
                                        toolbar: GlobalConfig.TINY_TOOLBAR,
                                    }}
                                />
                            </Grid>
                            <Grid item xs={12} sm={12}>
                                <Typography variant="subtitle1" gutterBottom>
                                    Hướng dẫn chọn size
                                </Typography>
                                <Autocomplete
                                    sx={{ marginBottom: 1 }}
                                    size="small"
                                    disablePortal
                                    options={[]}
                                    renderInput={(params) => <TextField {...params} label="Chọn size theo loại" />}
                                />
                                <Editor
                                    onInit={(evt, editor) => sizeGuideRef.current = editor}
                                    apiKey={GlobalConfig.TINY_KEY}
                                    init={{
                                        height: 500,
                                        plugins: GlobalConfig.TINY_PLUGINS,
                                        toolbar: GlobalConfig.TINY_TOOLBAR,
                                    }}
                                />
                            </Grid>
                            <Grid item xs={12} sm={12}>
                                <Typography variant="subtitle1" gutterBottom>
                                    Ảnh sản phẩm
                                </Typography>
                                <div className="flex">
                                    <label htmlFor="imageSys-upload">
                                        <span className="upload mr-2 text-[#4e73df] cursor-pointer">Chọn ảnh</span>
                                        <FormLabel>(*Tối đa 10, nếu có)</FormLabel>
                                    </label>
                                </div>
                                <UploadInput
                                    id={`imageSys-upload`}
                                    name="imageSys-upload"
                                    multiple
                                    hidden
                                    onChangeFiles={(event) => setSystemFileNames(event)}
                                    uploadType="products"
                                />
                            </Grid>
                            <Grid item xs={12} sm={12}>
                                <Typography variant="subtitle1" gutterBottom>
                                    Ảnh thực tế
                                </Typography>
                                <div className="flex">
                                    <label htmlFor="imageUser-upload">
                                        <span className="upload mr-2 text-[#4e73df] cursor-pointer">Chọn ảnh</span>
                                        <FormLabel>(Tối đa 10, nếu có)</FormLabel>
                                    </label>
                                </div>
                                <UploadInput
                                    id={`imageUser-upload`}
                                    multiple
                                    hidden
                                    onChangeFiles={(event) => setUserFileNames(event)}
                                    uploadType="products"
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <TextField
                                    fullWidth
                                    size="small"
                                    id="note"
                                    label="Ghi chú"
                                    name="note"
                                    autoComplete='off'
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <TextField
                                    fullWidth
                                    size="small"
                                    id="link"
                                    label="Link"
                                    name="link"
                                    autoComplete='off'
                                />
                            </Grid>
                            <Grid item xs={12} sm={12}>
                                <Button sx={{ marginRight: 2 }} variant="outlined">
                                    Quay lại
                                </Button>
                                <Button type="submit" variant="contained">
                                    Cập nhật
                                </Button>
                            </Grid>
                        </Grid>
                    </Grid>
                </Grid>
            </Box>
        </ThemeProvider>
    );
}

export default ProductDetail;
