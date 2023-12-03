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
import ProductService from "src/_cores/_services/product.service";
import { useNavigate } from "react-router-dom";
import { ADMIN_ROUTE_NAME } from "src/_cores/_enums/route-config.enum";

const defaultTheme = createTheme();
type DataDetail = {
    shops: IShop[],
    brands: IBrand[],
    subCategories: ISubCategory[],
    attributes: IAttribute[],
    options: IOption[],
    [key: string]: any
}
type NewOptionValue = {
    id: number;
    values: string[]
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
    const [newOptionValues, setNewOptionValues] = useState<NewOptionValue[]>([]);
    const [systemFileNames, setSystemFileNames] = useState<string[]>([]);
    const [userFileNames, setUserFileNames] = useState<string[]>([]);
    const [dataDetail, setDataDetail] = useState<DataDetail>({
        shops: [],
        brands: [],
        subCategories: [],
        attributes: [],
        options: []
    });

    useEffect(() => {
        getShops();
        if (productId > -1) {
            getDetail();
        }
    }, [productId]);

    const getDetail = () => {
        ProductService.getProductDetail(productId).then((res: any) => {
            if (res.isSucceed) {
                const _data = res.data;
                setDataDetail({
                    ..._data,
                });
            }
        }).catch(error => {
            alert(error);
        })
    }

    const getShops = () => {
        UserService.getShops().then(res => {
            if (res?.data) {
                setDataDetail({ ...dataDetail, shops: res.data });
            }
        }).catch(error => {
            console.log(error);
        });
    }

    const getBrands = (shopId: number) => {
        InventoryService.getBrands({ shopId: shopId }).then(res => {
            if (res?.data) {
                setDataDetail({ ...dataDetail, brands: res.data });
            }
        }).catch(error => {
            console.log(error);
        });
    }

    const getSubCategories = (brandId: number) => {
        InventoryService.getSubCategories({ brandId: brandId }).then(res => {
            if (res?.data) {
                setDataDetail({ ...dataDetail, subCategories: res.data });
            }
        }).catch(error => {
            console.log(error);
        });
    }

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
        const _optionList = value.optionList ?? [];
        _optionList.forEach((option) => {
            option.values = option.values?.filter(value => value.isBase)
        });
        setDataDetail({
            ...dataDetail,
            options: _optionList,
            attributes: value.attributes || []
        });
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

    const onChangeAttribute = (idx: number, value: string) => {
        if (dataDetail.attributes[idx]) {
            dataDetail.attributes[idx].value = value ?? '';
            setDataDetail({
                ...dataDetail,
                attributes: [...dataDetail.attributes]
            });
        }
    }

    const onChangeFieldValue = (field: string, value: any) => {
        setDataDetail({ ...dataDetail, [field]: value });
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

    const onChangeDiscountChecked = (event: any) => {
        setDataDetail({ ...dataDetail, isDiscount: event.target.checked })
    }

    const getNumber = (value: any) => FormatHelper.getNumber(value);

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        const param = {
            code: form.get('code'),
            name: form.get('name'),
            stock: getNumber(form.get('stock')),
            priceImport: getNumber(form.get('priceImport')?.toString()),
            priceForSeller: getNumber(form.get('priceForSeller')?.toString()),
            priceAvailable: getNumber(form.get('priceAvailable')?.toString()),
            pricePreOrder: getNumber(form.get('pricePreOrder')?.toString()),
            discountPreOrder: !dataDetail['isDiscount'] ? getNumber(form.get('discountPreOrder')?.toString()) : null,
            discountAvailable: !dataDetail['isDiscount'] ? getNumber(form.get('discountAvailable')?.toString()) : null,
            discountPercent: dataDetail['isDiscount'] ? getNumber(form.get('discountPercent')?.toString()) : null,
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
            sizeGuide: sizeGuideRef.current.getContent(),
            // options
            currentOptions: dataDetail.options.length > 0 ? dataDetail.options.map(option => option.id) : [],
            newOptions: newOptionValues,
            systemFileNames: systemFileNames,
            userFileNames: userFileNames
        }
        ProductService.save(param).then((res: any) => {
            console.log(res);
            if (res.isSucceed) {
                navigate({
                    pathname: ADMIN_ROUTE_NAME.MANAGE_PRODUCT
                })
            }
        }).catch(error => {
            console.log(error);
        })
    };

    return (
        <TextField
            defaultValue={dataDetail['code']}
            value={dataDetail['code']}
            onChange={(event) => onChangeFieldValue('code', event?.target.value)}
            autoComplete='off'
            name="code"
            required
            fullWidth
            size="small"
            label="Mã sản phẩm"
            autoFocus
        />
    )

    // return (
    //     <ThemeProvider theme={defaultTheme}>
    //         <Typography component="h1" variant="h5">
    //             Thêm sản phẩm
    //         </Typography>
    //         <Box component="form" noValidate onSubmit={handleSubmit} sx={{ mt: 3 }}>
    //             <Grid container spacing={2}>
    //                 <Grid item xs={7}>
    //                     <CssBaseline />
    //                     <Box
    //                         sx={{
    //                             display: 'flex',
    //                             flexDirection: 'column',
    //                         }}
    //                     >
    //                         <Typography variant="subtitle1" gutterBottom>
    //                             Thông tin cơ bản
    //                         </Typography>
    //                         <Grid container spacing={2}>
    //                             <Grid item xs={12} sm={5}>
    //                                 <TextField
    //                                     defaultValue={dataDetail['code']}
    //                                     value={dataDetail['code']}
    //                                     onChange={(event) => onChangeFieldValue('code', event?.target.value)}
    //                                     autoComplete='off'
    //                                     name="code"
    //                                     required
    //                                     fullWidth
    //                                     size="small"
    //                                     label="Mã sản phẩm"
    //                                     autoFocus
    //                                 />
    //                             </Grid>
    //                             <Grid item xs={12} sm={5}>
    //                                 <TextField
    //                                     value={dataDetail['name']}
    //                                     onChange={(event) => onChangeFieldValue('name', event?.target.value)}
    //                                     required
    //                                     fullWidth
    //                                     size="small"
    //                                     label="Tên sản phẩm"
    //                                     name="name"
    //                                     autoComplete='off'
    //                                 />
    //                             </Grid>
    //                             <Grid item xs={12} sm={2}>
    //                                 <TextField
    //                                     value={dataDetail['stock']}
    //                                     onChange={(event) => onChangeFieldValue('stock', event?.target?.value)}
    //                                     type="number"
    //                                     name="stock"
    //                                     label="Kho"
    //                                     size="small"
    //                                     fullWidth
    //                                 />
    //                             </Grid>
    //                             <Grid item xs={12} sm={12}>
    //                                 <PriceInput
    //                                     value={dataDetail['priceImport']}
    //                                     onChange={(event) => onChangeFieldValue('priceImport', event)}
    //                                     label="Giá nhập hàng"
    //                                     name='priceImport'
    //                                 />
    //                             </Grid>
    //                             <Grid item xs={12} sm={12}>
    //                                 <PriceInput
    //                                     value={dataDetail['priceForSeller']}
    //                                     onChange={(event) => onChangeFieldValue('priceForSeller', event)}
    //                                     label="Giá cho Seller"
    //                                     name='priceForSeller'
    //                                 />
    //                             </Grid>
    //                             <Grid item xs={12} sm={12}>
    //                                 <PriceInput
    //                                     value={dataDetail['priceAvailable']}
    //                                     onChange={(event) => onChangeFieldValue('priceAvailable', event)}
    //                                     label="Giá có sẵn"
    //                                     name='priceAvailable'
    //                                 />
    //                             </Grid>
    //                             <Grid item xs={12} sm={12}>
    //                                 <PriceInput
    //                                     value={dataDetail['pricePreOrder']}
    //                                     onChange={(event) => onChangeFieldValue('pricePreOrder', event)}
    //                                     label="Giá đặt trước"
    //                                     name='pricePreOrder'
    //                                 />
    //                             </Grid>
    //                             <Grid item xs={12} sm={2}>
    //                                 <FormControlLabel
    //                                     control={<>
    //                                         <Checkbox
    //                                             onChange={onChangeDiscountChecked}
    //                                             checked={dataDetail['isDiscount'] ?? false}
    //                                             name="isDicountPercent"
    //                                             color="primary"
    //                                         />
    //                                         <TextField
    //                                             value={dataDetail['discountPercent']}
    //                                             disabled={!dataDetail['isDiscount'] ?? false}
    //                                             onChange={(event) => onChangeFieldValue('discountPercent', event?.target.value)}
    //                                             type="number"
    //                                             name="discountPercent"
    //                                             label="% Giảm"
    //                                             size="small"
    //                                             fullWidth
    //                                         />
    //                                     </>}
    //                                     label=""
    //                                     sx={{ marginRight: 0 }}
    //                                 />
    //                             </Grid>
    //                             <Grid item xs={12} sm={5}>
    //                                 <PriceInput
    //                                     value={dataDetail['discountPreOrder']}
    //                                     disabled={dataDetail['isDiscount'] ?? false}
    //                                     onChange={(event) => onChangeFieldValue('discountPreOrder', event)}
    //                                     label="Giá giảm đặt trước"
    //                                     name='discountPreOrder'
    //                                 />
    //                             </Grid>
    //                             <Grid item xs={12} sm={5}>
    //                                 <PriceInput
    //                                     value={dataDetail['discountAvailable']}
    //                                     disabled={dataDetail['isDiscount'] ?? false}
    //                                     onChange={(event) => onChangeFieldValue('discountAvailable', event)}
    //                                     label="Giá giảm có sẵn"
    //                                     name='discountAvailable'
    //                                 />
    //                             </Grid>
    //                             <Grid item xs={12}>
    //                                 <FormControlLabel control={<Checkbox name="isNew" color="primary" />} label="Mới" />
    //                                 <FormControlLabel control={<Checkbox name="isHighlight" color="primary" />} label="Nổi bật" />
    //                                 <FormControlLabel control={<Checkbox name="isLegal" color="primary" />} label="Sản phẩm chính hãng" />
    //                             </Grid>
    //                             <Grid item xs={12} sm={4}>
    //                                 <TextField
    //                                     value={dataDetail['repay']}
    //                                     onChange={(event) => onChangeFieldValue('repay', event?.target.value)}
    //                                     name="repay"
    //                                     fullWidth
    //                                     size="small"
    //                                     label="Đổi trả"
    //                                     autoFocus
    //                                     autoComplete='off'
    //                                 />
    //                             </Grid>
    //                             <Grid item xs={12} sm={4}>
    //                                 <TextField
    //                                     value={dataDetail['delivery']}
    //                                     onChange={(event) => onChangeFieldValue('delivery', event?.target.value)}
    //                                     name="delivery"
    //                                     fullWidth
    //                                     size="small"
    //                                     label="Giao hàng"
    //                                     autoFocus
    //                                     autoComplete='off'
    //                                 />
    //                             </Grid>
    //                             <Grid item xs={12} sm={4}>
    //                                 <TextField
    //                                     value={dataDetail['insurance']}
    //                                     onChange={(event) => onChangeFieldValue('insurance', event?.target.value)}
    //                                     name="insurance"
    //                                     fullWidth
    //                                     size="small"
    //                                     label="Bảo hành"
    //                                     autoFocus
    //                                     autoComplete='off'
    //                                 />
    //                             </Grid>
    //                             <Grid item xs={12} sm={12}>
    //                                 {/* {dataDetail.shops.length > 0 && (
    //                                     <Autocomplete
    //                                         size="small"
    //                                         disablePortal
    //                                         options={dataDetail.shops}
    //                                         onChange={(event, value) => value && onChangeShop(value)}
    //                                         getOptionLabel={(option) => `${option.name}`}
    //                                         renderOption={(props, option) => <li {...props}>{option.name}</li>}
    //                                         renderInput={(params) => <TextField {...params} name="shop" label="Cửa hàng" />}
    //                                     />
    //                                 )} */}
    //                             </Grid>
    //                             <Grid item xs={12} sm={12}>
    //                                 {/* {dataDetail.brands.length > 0 && (
    //                                     <Autocomplete
    //                                         size="small"
    //                                         disablePortal
    //                                         options={dataDetail.brands}
    //                                         onChange={(event, value) => value && onChangeBrand(value)}
    //                                         getOptionLabel={(option) => `${option.name}`}
    //                                         renderOption={(props, option) => <li {...props}>{option.name}</li>}
    //                                         renderInput={(params) => <TextField {...params} label="Thương hiệu" />}
    //                                     />
    //                                 )} */}
    //                             </Grid>
    //                             <Grid item xs={12} sm={12}>
    //                                 {/* {dataDetail.subCategories.length > 0 && (
    //                                     <Autocomplete
    //                                         size="small"
    //                                         disablePortal
    //                                         options={dataDetail.subCategories}
    //                                         onChange={(event, value) => value && onChangeSubCategory(value)}
    //                                         getOptionLabel={(option) => `${option.name}`}
    //                                         renderOption={(props, option) => <li {...props}>{option.name}</li>}
    //                                         renderInput={(params) => <TextField {...params} label="Loại sản phẩm" />}
    //                                     />
    //                                 )} */}
    //                             </Grid>
    //                         </Grid>
    //                     </Box>
    //                 </Grid>
    //                 <Grid item xs={5}>
    //                     <Box
    //                         sx={{
    //                             display: 'flex',
    //                             flexDirection: 'column',
    //                         }}
    //                     >
    //                         <Grid container spacing={2}>
    //                             {/* {dataDetail.attributes.length > 0 && (
    //                                 <Grid item xs={12} sm={12}>
    //                                     <Typography variant="subtitle1" gutterBottom>
    //                                         Thông tin chi tiết
    //                                     </Typography>
    //                                     <Grid container spacing={2}>
    //                                         {dataDetail.attributes.map((attribute, idx) => (
    //                                             <Grid key={`attribute-${attribute.id}`} item xs={12} sm={6}>
    //                                                 <TextField
    //                                                     value={dataDetail['attributes'][idx].value}
    //                                                     onChange={(event) => onChangeAttribute(idx, event.target.value)}
    //                                                     fullWidth
    //                                                     size="small"
    //                                                     label={attribute.name}
    //                                                     autoComplete='off'
    //                                                 />
    //                                             </Grid>
    //                                         ))}
    //                                     </Grid>
    //                                 </Grid>
    //                             )} */}
    //                             {/* {dataDetail.options.length > 0 && (
    //                                 <Grid item xs={12} sm={12}>
    //                                     <Typography variant="subtitle1" gutterBottom>
    //                                         Thông tin tùy chọn
    //                                     </Typography>
    //                                     <Grid item xs={12} sm={6}>
    //                                         {dataDetail.options.map((option, idx) => (
    //                                             <Fragment key={`option-${option.id}`}>
    //                                                 <TextField
    //                                                     sx={{ marginBottom: 1 }}
    //                                                     fullWidth
    //                                                     size="small"
    //                                                     label={option.name}
    //                                                     autoComplete='off'
    //                                                     onBlur={(event) => onBlurOption(idx, event.target.value)}
    //                                                 />
    //                                                 <Stack spacing={{ xs: 1, sm: 1 }} direction="row" useFlexGap flexWrap="wrap">
    //                                                     {option.values && option.values?.length > 0 && (
    //                                                         option.values.map((value, childIdx) => (
    //                                                             <Chip
    //                                                                 key={`option-value-${value.id}`}
    //                                                                 label={value.name}
    //                                                                 variant="outlined"
    //                                                                 onDelete={() => deleteCurrentOptionValue(idx, childIdx)}
    //                                                             />
    //                                                         ))
    //                                                     )}
    //                                                     {newOptionValues[idx] && newOptionValues[idx].values.length > 0 && (
    //                                                         newOptionValues[idx].values.map((value, childIdx) => (
    //                                                             <Chip
    //                                                                 key={`new-option-value-${childIdx}`}
    //                                                                 label={value}
    //                                                                 variant="outlined"
    //                                                                 onDelete={() => deleteNewOptionValue(idx, childIdx)}
    //                                                             />
    //                                                         ))
    //                                                     )}
    //                                                 </Stack>
    //                                             </Fragment>
    //                                         ))}
    //                                     </Grid>
    //                                 </Grid>
    //                             )} */}
    //                         </Grid>
    //                     </Box>
    //                 </Grid>
    //                 <Grid item xs={12}>
    //                     <Grid container spacing={2}>
    //                         <Grid item xs={12} sm={12}>
    //                             <Typography variant="subtitle1" gutterBottom>
    //                                 Mô tả
    //                             </Typography>
    //                             <Editor
    //                                 onInit={(evt, editor) => descriptionRef.current = editor}
    //                                 apiKey={GlobalConfig.TINY_KEY}
    //                                 init={{
    //                                     height: 500,
    //                                     plugins: GlobalConfig.TINY_PLUGINS,
    //                                     toolbar: GlobalConfig.TINY_TOOLBAR,
    //                                 }}
    //                             />
    //                         </Grid>
    //                         <Grid item xs={12} sm={12}>
    //                             <Typography variant="subtitle1" gutterBottom>
    //                                 Hướng dẫn chọn size
    //                             </Typography>
    //                             <Autocomplete
    //                                 sx={{ marginBottom: 1 }}
    //                                 size="small"
    //                                 disablePortal
    //                                 options={[]}
    //                                 renderInput={(params) => <TextField {...params} label="Chọn size theo loại" />}
    //                             />
    //                             <Editor
    //                                 onInit={(evt, editor) => sizeGuideRef.current = editor}
    //                                 apiKey={GlobalConfig.TINY_KEY}
    //                                 init={{
    //                                     height: 500,
    //                                     plugins: GlobalConfig.TINY_PLUGINS,
    //                                     toolbar: GlobalConfig.TINY_TOOLBAR,
    //                                 }}
    //                             />
    //                         </Grid>
    //                         <Grid item xs={12} sm={12}>
    //                             <Typography variant="subtitle1" gutterBottom>
    //                                 Ảnh sản phẩm
    //                             </Typography>
    //                             <div className="flex">
    //                                 <label htmlFor="imageSys-upload">
    //                                     <span className="upload mr-2 text-[#4e73df] cursor-pointer">Chọn ảnh</span>
    //                                     <FormLabel>(*Tối đa 10, nếu có)</FormLabel>
    //                                 </label>
    //                             </div>
    //                             <UploadInput
    //                                 id={`imageSys-upload`}
    //                                 name="imageSys-upload"
    //                                 multiple
    //                                 hidden
    //                                 onChangeFiles={(event) => setSystemFileNames(event)}
    //                                 uploadType="products"
    //                                 filesLimit={10}
    //                             />
    //                         </Grid>
    //                         <Grid item xs={12} sm={12}>
    //                             <Typography variant="subtitle1" gutterBottom>
    //                                 Ảnh thực tế
    //                             </Typography>
    //                             <div className="flex">
    //                                 <label htmlFor="imageUser-upload">
    //                                     <span className="upload mr-2 text-[#4e73df] cursor-pointer">Chọn ảnh</span>
    //                                     <FormLabel>(Tối đa 10, nếu có)</FormLabel>
    //                                 </label>
    //                             </div>
    //                             <UploadInput
    //                                 id={`imageUser-upload`}
    //                                 multiple
    //                                 hidden
    //                                 onChangeFiles={(event) => setUserFileNames(event)}
    //                                 uploadType="products"
    //                                 filesLimit={10}
    //                             />
    //                         </Grid>
    //                         <Grid item xs={12}>
    //                             <TextField
    //                                 fullWidth
    //                                 size="small"
    //                                 id="note"
    //                                 label="Ghi chú"
    //                                 name="note"
    //                                 autoComplete='off'
    //                             />
    //                         </Grid>
    //                         <Grid item xs={12}>
    //                             <TextField
    //                                 fullWidth
    //                                 size="small"
    //                                 id="link"
    //                                 label="Link"
    //                                 name="link"
    //                                 autoComplete='off'
    //                             />
    //                         </Grid>
    //                         <Grid item xs={12} sm={12}>
    //                             <Button sx={{ marginRight: 2 }} variant="outlined">
    //                                 Quay lại
    //                             </Button>
    //                             <Button type="submit" variant="contained">
    //                                 Cập nhật
    //                             </Button>
    //                         </Grid>
    //                     </Grid>
    //                 </Grid>
    //             </Grid>
    //         </Box>
    //     </ThemeProvider>
    // );
}

export default ProductDetail;
