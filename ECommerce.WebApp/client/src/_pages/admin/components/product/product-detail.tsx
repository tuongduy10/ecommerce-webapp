import { Autocomplete, Box, Button, Checkbox, Chip, CssBaseline, FormControlLabel, FormLabel, Grid, Stack, TextField, ThemeProvider, Typography, createTheme } from "@mui/material";
import { Editor } from '@tinymce/tinymce-react';
import React, { Fragment, useEffect, useRef, useState } from "react";
import { GlobalConfig } from "src/_configs/global.config";
import UploadInput from "src/_shares/_components/input/upload-input";
import { FormatHelper } from "src/_shares/_helpers/format-helper";
import PriceInput from "src/_shares/_components/input/price-input";
import { IShop } from "src/_cores/_interfaces/user.interface";
import { IAttribute, IBrand, IOption, IOptionValue, ISubCategory } from "src/_cores/_interfaces/inventory.interface";
import UserService from "src/_cores/_services/user.service";
import InventoryService from "src/_cores/_services/inventory.service";
import ProductService from "src/_cores/_services/product.service";
import { Form, useNavigate } from "react-router-dom";
import { ADMIN_ROUTE_NAME } from "src/_cores/_enums/route-config.enum";

const defaultTheme = createTheme();
interface IDataDetail {
    [key: string]: any
}

const ProductDetail = () => {
    const navigate = useNavigate();
    const descriptionRef = useRef<any>(null);
    const sizeGuideRef = useRef<any>(null);
    const searchParams = new URLSearchParams(window.location.search);
    const productId = Number(searchParams.get('id')) || -1;
    const [selectedBrandId, setSelectedBrandId] = useState<number>(-1);
    const [selectedShopId, setSelectedShopId] = useState<number>(-1);
    const [selectedSubCategoryId, setSelectedSubCategoryId] = useState<number>(-1);
    const [systemFileNames, setSystemFileNames] = useState<string[]>([]);
    const [userFileNames, setUserFileNames] = useState<string[]>([]);
    const [dataDetail, setDataDetail] = useState<IDataDetail>({});
    const [shops, setShops] = useState<IShop[]>([]);
    const [brands, setBrands] = useState<IBrand[]>([]);
    const [subCategories, setSubCategories] = useState<ISubCategory[]>([]);
    const [attributes, setAttributes] = useState<IAttribute[]>([]);
    const [options, setOptions] = useState<IOption[]>([]);

    useEffect(() => {
        getShops();
        if (productId > -1) {
            getDetail();
        }
    }, []);

    const getDetail = () => {
        ProductService.getProductDetail(productId).then((res: any) => {
            if (res.isSucceed) {
                const _data = res.data;
                setDataDetail(_data);
                setSelectedShopId(_data.shopId);
                setSelectedBrandId(_data.brandId);
                getBrands(_data.shopId);
                setSelectedSubCategoryId(_data.subCategoryId);
                getSubCategories(_data.brandId, _data);
                // images
                setSystemFileNames(_data.imagePaths);
                setUserFileNames(_data.userImagePaths);
            }
        }).catch(error => {
            alert(error);
        })
    }

    const getShops = () => {
        UserService.getShops().then(res => {
            if (res?.data) {
                setShops(res.data);
            }
        }).catch(error => {
            console.log(error);
        });
    }

    const getBrands = (shopId: number) => {
        InventoryService.getBrands({ shopId: shopId }).then(res => {
            if (res?.data) {
                setBrands(res.data);
            }
        }).catch(error => {
            console.log(error);
        });
    }

    const getSubCategories = (brandId: number, dataDetail: any = null) => {
        InventoryService.getSubCategories({ brandId: brandId }).then(res => {
            if (res?.data) {
                const _result = res.data as ISubCategory[];
                setSubCategories(_result);
                if (dataDetail.subCategoryId) {
                    const sub = _result.find(_ => _.id === dataDetail.subCategoryId);
                    if (sub) {
                        // options
                        const _optionList = sub.optionList ?? [];
                        // for adding 
                        if (!dataDetail.id) {
                            const _optionListForAdding = _optionList;
                            _optionListForAdding.forEach((option) => {
                                option.values = option.values?.filter(value => value.isBase)
                            });
                            setOptions(_optionListForAdding);
                        } else {
                            // for updating
                            const _productOptions = _optionList;
                            _productOptions.forEach((option: IOption) => {
                                const optIdx = dataDetail.options.findIndex((_: IOption) => _.id === option.id);
                                option.values = optIdx > -1 ? dataDetail.options[optIdx].values : [];
                            });
                            setOptions(_productOptions);
                        }
                        // attributes
                        const _attributes = sub.attributes ?? [];
                        if (dataDetail.attributes?.length > 0) {
                            dataDetail.attributes.forEach((attribute: IAttribute) => {
                                const idx = _attributes.findIndex(_ => _.id === attribute.id);
                                if (idx > -1) {
                                    _attributes[idx].value = attribute.value;
                                }
                            });
                        }
                        setAttributes(_attributes);
                    }
                }
            }
        }).catch(error => {
            console.log(error);
        });
    }

    const onChangeShop = (value: IShop) => {
        setSelectedShopId(value?.id || -1);
        if (!value) {
            setSelectedShopId(-1);
            setBrands([]);
            setSubCategories([]);
            setAttributes([]);
            setOptions([]);
            return;
        }
        getBrands(value.id)
    }

    const onChangeBrand = (value: IBrand) => {
        setSelectedBrandId(value?.id || -1);
        if (!value) {
            setSelectedSubCategoryId(-1);
            setSubCategories([]);
            setAttributes([]);
            setOptions([]);
            return;
        }
        getSubCategories(value.id)
    }

    const onChangeSubCategory = (value: ISubCategory) => {
        setSelectedSubCategoryId(value?.id || -1);
        if (!value) {
            setAttributes([]);
            setOptions([]);
            return;
        }
        const _optionList = value.optionList ?? [];
        _optionList.forEach((option) => {
            option.values = option.values?.filter(value => value.isBase)
        });
        setOptions(_optionList);
        setAttributes(value.attributes || []);
    }

    const onBlurOption = (idx: number, value: string) => {
        if (options[idx]) {
            const _value = value ? value.trim() : "";
            if (_value && !options[idx].values.map(_ => _.name).includes(_value)) {
                const newOptionValue: IOptionValue = {
                    name: _value,
                }
                options[idx].values.push(newOptionValue);
                setOptions([...options]);
            }
        }
        // if (!newOptionValues[idx]) {
        //     newOptionValues[idx] = {
        //         id: options[idx].id,
        //         values: []
        //     }
        // }
        // if (value && !newOptionValues[idx].values.includes(value)) {
        //     newOptionValues[idx].values.push(value);
        // }
        // setNewOptionValues([...newOptionValues]);
    }

    const onChangeAttribute = (idx: number, value: string) => {
        if (attributes[idx]) {
            attributes[idx].value = value ?? '';
            setAttributes([...attributes]);
        }
    }

    const onChangeFieldValue = (field: string, value: any) => {
        setDataDetail({ ...dataDetail, [field]: value });
    }

    const deleteOptionValue = (idx: number, childIdx: number) => {
        if (options[idx]) {
            options[idx].values.splice(childIdx, 1);
        }
        setOptions([...options]);
    }

    const onChangeDiscountChecked = (event: any) => {
        setDataDetail({ ...dataDetail, isDiscountPercent: event.target.checked });
    }

    const onChangeIsNew = (event: any) => {
        setDataDetail({ ...dataDetail, isNew: event.target.checked });
    }

    const onChangeIsHighlight = (event: any) => {
        setDataDetail({ ...dataDetail, isHighlight: event.target.checked });
    }

    const onChangeIsLegit = (event: any) => {
        setDataDetail({ ...dataDetail, isLegit: event.target.checked });
    }

    const getNumber = (value: any) => FormatHelper.getNumber(value);

    const handleKeyPress = (event: React.KeyboardEvent<HTMLFormElement>) => {
        if (event.key === 'Enter') {
            event.preventDefault();
        }
    };

    const backToList = () => {
        navigate({
            pathname: ADMIN_ROUTE_NAME.MANAGE_PRODUCT
        });
    }

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        const param = {
            id: dataDetail['id'] ?? -1,
            code: form.get('code'),
            name: form.get('name'),
            stock: getNumber(form.get('stock')),
            priceImport: getNumber(form.get('priceImport')?.toString()),
            priceForSeller: getNumber(form.get('priceForSeller')?.toString()),
            priceAvailable: getNumber(form.get('priceAvailable')?.toString()),
            pricePreOrder: getNumber(form.get('pricePreOrder')?.toString()),
            discountPreOrder: !dataDetail['isDiscountPercent'] ? getNumber(form.get('discountPreOrder')?.toString()) : null,
            discountAvailable: !dataDetail['isDiscountPercent'] ? getNumber(form.get('discountAvailable')?.toString()) : null,
            discountPercent: dataDetail['isDiscountPercent'] ? getNumber(form.get('discountPercent')?.toString()) : null,
            isNew: form.get('isNew')?.toString() === 'on',
            isHighlight: form.get('isHighlight')?.toString() === 'on',
            isLegit: form.get('isLegit')?.toString() === 'on',
            repay: form.get('repay'),
            delivery: form.get('delivery'),
            insurance: form.get('insurance'),
            link: form.get('link'),
            note: form.get('note'),
            shopId: selectedShopId,
            brandId: selectedBrandId,
            subCategoryId: selectedSubCategoryId,
            attributes: attributes,
            description: descriptionRef.current.getContent(),
            sizeGuide: sizeGuideRef.current.getContent(),
            // options
            options: options,
            systemFileNames: systemFileNames,
            userFileNames: userFileNames
        }
        ProductService.save(param).then((res: any) => {
            if (res.isSucceed) {
                backToList();
            }
        }).catch(error => {
            console.log(error);
        })
    };

    return (
        <ThemeProvider theme={defaultTheme}>
            <Typography component="h1" variant="h5" sx={{ mb: 3 }}>
                {dataDetail.id ? 'Cập nhật sản phẩm' : 'Thêm mới sản phẩm'}
            </Typography>
            <Form noValidate onSubmit={handleSubmit} onKeyPress={handleKeyPress}>
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
                                        value={dataDetail['code']}
                                        onChange={(event) => onChangeFieldValue('code', event?.target.value)}
                                        InputLabelProps={{ shrink: true }}
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
                                        value={dataDetail['name']}
                                        onChange={(event) => onChangeFieldValue('name', event?.target.value)}
                                        InputLabelProps={{ shrink: true }}
                                        required
                                        fullWidth
                                        size="small"
                                        label="Tên sản phẩm"
                                        name="name"
                                        autoComplete='off'
                                    />
                                </Grid>
                                <Grid item xs={12} sm={2}>
                                    <TextField
                                        value={dataDetail['stock']}
                                        onChange={(event) => onChangeFieldValue('stock', event?.target?.value)}
                                        InputLabelProps={{ shrink: true }}
                                        type="number"
                                        name="stock"
                                        label="Kho"
                                        size="small"
                                        fullWidth
                                    />
                                </Grid>
                                <Grid item xs={12} sm={12}>
                                    <PriceInput
                                        value={dataDetail['priceImport']}
                                        onChange={(event) => onChangeFieldValue('priceImport', event)}
                                        InputLabelProps={{ shrink: true }}
                                        label="Giá nhập hàng"
                                        name='priceImport'
                                    />
                                </Grid>
                                <Grid item xs={12} sm={12}>
                                    <PriceInput
                                        value={dataDetail['priceForSeller']}
                                        onChange={(event) => onChangeFieldValue('priceForSeller', event)}
                                        InputLabelProps={{ shrink: true }}
                                        label="Giá cho Seller"
                                        name='priceForSeller'
                                    />
                                </Grid>
                                <Grid item xs={12} sm={12}>
                                    <PriceInput
                                        value={dataDetail['pricePreOrder']}
                                        onChange={(event) => onChangeFieldValue('pricePreOrder', event)}
                                        InputLabelProps={{ shrink: true }}
                                        label="Giá đặt trước"
                                        name='pricePreOrder'
                                    />
                                </Grid>
                                <Grid item xs={12} sm={12}>
                                    <PriceInput
                                        value={dataDetail['priceAvailable']}
                                        onChange={(event) => onChangeFieldValue('priceAvailable', event)}
                                        InputLabelProps={{ shrink: true }}
                                        label="Giá có sẵn"
                                        name='priceAvailable'
                                    />
                                </Grid>
                                <Grid item xs={12} sm={2}>
                                    <FormControlLabel
                                        control={<>
                                            <Checkbox
                                                onChange={onChangeDiscountChecked}
                                                checked={JSON.stringify(dataDetail['isDiscountPercent']) === 'true'}
                                                name="isDicountPercent"
                                                color="primary"
                                            />
                                            <TextField
                                                value={dataDetail['discountPercent']}
                                                disabled={JSON.stringify(dataDetail['isDiscountPercent']) !== 'true'}
                                                onChange={(event) => onChangeFieldValue('discountPercent', event?.target.value)}
                                                InputLabelProps={{ shrink: true }}
                                                type="number"
                                                name="discountPercent"
                                                label="% Giảm"
                                                size="small"
                                                fullWidth
                                            />
                                        </>}
                                        label=""
                                        sx={{ marginRight: 0 }}
                                    />
                                </Grid>
                                <Grid item xs={12} sm={5}>
                                    <PriceInput
                                        value={dataDetail['discountPreOrder']}
                                        disabled={JSON.stringify(dataDetail['isDiscountPercent']) === 'true'}
                                        onChange={(event) => onChangeFieldValue('discountPreOrder', event)}
                                        InputLabelProps={{ shrink: true }}
                                        label="Giá giảm đặt trước"
                                        name='discountPreOrder'
                                    />
                                </Grid>
                                <Grid item xs={12} sm={5}>
                                    <PriceInput
                                        value={dataDetail['discountAvailable']}
                                        disabled={JSON.stringify(dataDetail['isDiscountPercent']) === 'true'}
                                        onChange={(event) => onChangeFieldValue('discountAvailable', event)}
                                        InputLabelProps={{ shrink: true }}
                                        label="Giá giảm có sẵn"
                                        name='discountAvailable'
                                    />
                                </Grid>
                                <Grid item xs={12}>
                                    <FormControlLabel
                                        control={
                                            <Checkbox
                                                checked={JSON.stringify(dataDetail['isNew']) === 'true'}
                                                onChange={onChangeIsNew}
                                                name="isNew"
                                                color="primary"
                                            />}
                                        label="Mới"
                                    />
                                    <FormControlLabel
                                        control={
                                            <Checkbox
                                                checked={JSON.stringify(dataDetail['isHighlight']) === 'true'}
                                                onChange={onChangeIsHighlight}
                                                name="isHighlight"
                                                color="primary"
                                            />}
                                        label="Nổi bật"
                                    />
                                    <FormControlLabel
                                        control={
                                            <Checkbox
                                                checked={JSON.stringify(dataDetail['isLegit']) === 'true'}
                                                onChange={onChangeIsLegit}
                                                name="isLegit"
                                                color="primary"
                                            />}
                                        label="Sản phẩm chính hãng"
                                    />
                                </Grid>
                                <Grid item xs={12} sm={4}>
                                    <TextField
                                        value={dataDetail['repay']}
                                        onChange={(event) => onChangeFieldValue('repay', event?.target.value)}
                                        InputLabelProps={{ shrink: true }}
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
                                        value={dataDetail['delivery']}
                                        onChange={(event) => onChangeFieldValue('delivery', event?.target.value)}
                                        InputLabelProps={{ shrink: true }}
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
                                        value={dataDetail['insurance']}
                                        onChange={(event) => onChangeFieldValue('insurance', event?.target.value)}
                                        InputLabelProps={{ shrink: true }}
                                        name="insurance"
                                        fullWidth
                                        size="small"
                                        label="Bảo hành"
                                        autoFocus
                                        autoComplete='off'
                                    />
                                </Grid>
                                <Grid item xs={12} sm={12}>
                                    {shops.length > 0 ? (
                                        <Autocomplete
                                            size="small"
                                            disablePortal
                                            options={shops}
                                            value={shops.find(_ => _.id === selectedShopId) ?? null}
                                            onChange={(event, value) => value && onChangeShop(value)}
                                            getOptionLabel={(option) => `${option.name}`}
                                            renderOption={(props, option) => <li {...props}>{option.name}</li>}
                                            renderInput={(params) => <TextField {...params} name="shop" label="Cửa hàng" />}
                                        />
                                    ) : null}
                                </Grid>
                                <Grid item xs={12} sm={12}>
                                    {brands.length > 0 && (
                                        <Autocomplete
                                            size="small"
                                            disablePortal
                                            options={brands}
                                            value={brands.find(_ => _.id === selectedBrandId) ?? null}
                                            onChange={(event, value) => value && onChangeBrand(value)}
                                            getOptionLabel={(option) => `${option.name}`}
                                            renderOption={(props, option) => <li {...props}>{option.name}</li>}
                                            renderInput={(params) => <TextField {...params} label="Thương hiệu" />}
                                        />
                                    )}
                                </Grid>
                                <Grid item xs={12} sm={12}>
                                    {subCategories.length > 0 && (
                                        <Autocomplete
                                            size="small"
                                            disablePortal
                                            options={subCategories}
                                            value={subCategories.find(_ => _.id === selectedSubCategoryId) ?? null}
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
                                {attributes.length > 0 && (
                                    <Grid item xs={12} sm={12}>
                                        <Typography variant="subtitle1" gutterBottom>
                                            Thông tin chi tiết
                                        </Typography>
                                        <Grid container spacing={2}>
                                            {attributes.map((attribute, idx) => (
                                                <Grid key={`attribute-${attribute.id}`} item xs={12} sm={6}>
                                                    <TextField
                                                        value={attributes[idx].value}
                                                        onChange={(event) => onChangeAttribute(idx, event.target.value)}
                                                        fullWidth
                                                        size="small"
                                                        label={attribute.name}
                                                        autoComplete='off'
                                                    />
                                                </Grid>
                                            ))}
                                        </Grid>
                                    </Grid>
                                )}
                                {options.length > 0 && (
                                    <Grid item xs={12} sm={12}>
                                        <Typography variant="subtitle1" gutterBottom>
                                            Thông tin tùy chọn
                                        </Typography>
                                        <Grid item xs={12} sm={6}>
                                            {options.map((option, idx) => (
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
                                                                    key={`option-value-${value.id ?? childIdx}`}
                                                                    label={value.name}
                                                                    variant="outlined"
                                                                    onDelete={() => deleteOptionValue(idx, childIdx)}
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
                                    initialValue={dataDetail && dataDetail['description'] ? dataDetail['description'] : ''}
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
                                    initialValue={dataDetail && dataDetail['sizeGuide'] ? dataDetail['sizeGuide'] : ''}
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
                                    filesLimit={10}
                                    selectedFiles={systemFileNames}
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
                                    filesLimit={10}
                                    selectedFiles={userFileNames}
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <TextField
                                    value={dataDetail['note']}
                                    onChange={(event) => onChangeFieldValue('note', event?.target.value)}
                                    InputLabelProps={{ shrink: true }}
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
                                    value={dataDetail['link']}
                                    onChange={(event) => onChangeFieldValue('link', event?.target.value)}
                                    InputLabelProps={{ shrink: true }}
                                    fullWidth
                                    size="small"
                                    id="link"
                                    label="Link"
                                    name="link"
                                    autoComplete='off'
                                />
                            </Grid>
                            <Grid item xs={12} sm={12}>
                                <Button sx={{ marginRight: 2 }} variant="outlined" onClick={backToList}>
                                    Quay lại
                                </Button>
                                <Button type="submit" variant="contained">
                                    {dataDetail.id ? 'Cập nhật' : 'Thêm mới'}
                                </Button>
                            </Grid>
                        </Grid>
                    </Grid>
                </Grid>
            </Form>
        </ThemeProvider>
    );
}

export default ProductDetail;
