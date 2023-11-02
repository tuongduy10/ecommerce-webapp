import { Autocomplete, Box, Button, Checkbox, Chip, CssBaseline, FormControlLabel, FormLabel, Grid, Input, Stack, TextField, ThemeProvider, Typography, createTheme } from "@mui/material";
import { Editor } from '@tinymce/tinymce-react';
import { useRef } from "react";
import { GlobalConfig } from "src/_configs/global.config";
import UploadInput from "src/_shares/_components/input/upload-input";
import NumberInput from "src/_shares/_components/input/number-input";
import { FormatHelper } from "src/_shares/_helpers/format-helper";
import PriceInput from "src/_shares/_components/input/price-input";


const top100Films = [
    { label: 'The Shawshank Redemption', value: 1994 },
    { label: 'The Godfather', value: 1972 },
    { label: 'The Godfather: Part II', value: 1974 },
    { label: 'The Dark Knight', value: 2008 },
    { label: '12 Angry Men', value: 1957 },
    { label: "Schindler's List", value: 1993 },
    { label: 'Pulp Fiction', value: 1994 },
];

const defaultTheme = createTheme();
const ProductDetail = () => {
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
            files: form.getAll('imageSys-upload'),
            isNew: form.get('isNew')?.toString() === 'on',
            isHighlight: form.get('isHighlight')?.toString() === 'on',
            isLegal: form.get('isLegal')?.toString() === 'on',
            repay: form.get('repay'),
            delivery: form.get('delivery'),
            insurance: form.get('insurance'),
            link: form.get('link'),
            note: form.get('note'),
            shop: form.get('shop'),
        }
        console.log(param);
    };

    const onUpload = (e: any) => {
        console.log(e);
    }

    const handleDeleteChip = () => {

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
                                        id="code"
                                        label="Mã sản phẩm"
                                        autoFocus
                                    />
                                </Grid>
                                <Grid item xs={12} sm={5}>
                                    <TextField
                                        required
                                        fullWidth
                                        size="small"
                                        id="productName"
                                        label="Tên sản phẩm"
                                        name="productName"
                                        autoComplete='off'
                                    />
                                </Grid>
                                <Grid item xs={12} sm={2}>
                                    <Input type="number" placeholder="Kho" />
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
                                <Grid item xs={12} sm={4}>
                                    <FormControlLabel
                                        control={<>
                                            <Checkbox name="isDicountPercent" color="primary" />
                                            <Input type="number" name="discountPercent" placeholder="% Giảm" />
                                        </>}
                                        label=""
                                        sx={{ marginRight: 0 }}
                                    />
                                </Grid>
                                <Grid item xs={12} sm={4}>
                                    <PriceInput label="Giá giảm đặt trước" name='discountPreOrder' />
                                </Grid>
                                <Grid item xs={12} sm={4}>
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
                                    <Autocomplete
                                        size="small"
                                        disablePortal
                                        options={top100Films}
                                        renderInput={(params) => <TextField {...params} name="shop" label="Cửa hàng" />}
                                    />
                                </Grid>
                                <Grid item xs={12} sm={12}>
                                    <Autocomplete
                                        size="small"
                                        disablePortal
                                        options={top100Films}
                                        renderInput={(params) => <TextField {...params} label="Thương hiệu" />}
                                    />
                                </Grid>
                                <Grid item xs={12} sm={12}>
                                    <Autocomplete
                                        size="small"
                                        disablePortal
                                        options={top100Films}
                                        renderInput={(params) => <TextField {...params} label="Danh mục" />}
                                    />
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
                                <Grid item xs={12} sm={12}>
                                    <Typography variant="subtitle1" gutterBottom>
                                        Thông tin chi tiết
                                    </Typography>
                                    <Grid container spacing={2}>
                                        <Grid item xs={12} sm={6}>
                                            <TextField
                                                fullWidth
                                                size="small"
                                                label="Chất liệu"
                                                autoComplete='off'
                                            />
                                        </Grid>
                                        <Grid item xs={12} sm={6}>
                                            <TextField
                                                fullWidth
                                                size="small"
                                                label="Trọng lượng"
                                                autoComplete='off'
                                            />
                                        </Grid>
                                        <Grid item xs={12} sm={6}>
                                            <TextField
                                                fullWidth
                                                size="small"
                                                label="Dung tích"
                                                autoComplete='off'
                                            />
                                        </Grid>
                                        <Grid item xs={12} sm={6}>
                                            <TextField
                                                fullWidth
                                                size="small"
                                                label="Kích thước"
                                                autoComplete='off'
                                            />
                                        </Grid>
                                        <Grid item xs={12} sm={6}>
                                            <TextField
                                                fullWidth
                                                size="small"
                                                label="Công suất"
                                                autoComplete='off'
                                            />
                                        </Grid>
                                        <Grid item xs={12} sm={6}>
                                            <TextField
                                                fullWidth
                                                size="small"
                                                label="Điện áp"
                                                autoComplete='off'
                                            />
                                        </Grid>
                                    </Grid>
                                </Grid>
                                <Grid item xs={12} sm={12}>
                                    <Typography variant="subtitle1" gutterBottom>
                                        Thông tin tùy chọn
                                    </Typography>
                                    <Grid item xs={12} sm={6}>
                                        <TextField
                                            sx={{ marginBottom: 1 }}
                                            fullWidth
                                            size="small"
                                            label="Màu sắc"
                                            autoComplete='off'
                                        />
                                        <Stack spacing={{ xs: 1, sm: 1 }} direction="row" useFlexGap flexWrap="wrap">
                                            <Chip label="Vàng" variant="outlined" onDelete={handleDeleteChip} />
                                            <Chip label="Hồng" variant="outlined" onDelete={handleDeleteChip} />
                                            <Chip label="Trắng" variant="outlined" onDelete={handleDeleteChip} />
                                            <Chip label="Đen" variant="outlined" onDelete={handleDeleteChip} />
                                        </Stack>
                                    </Grid>
                                </Grid>
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
                                    options={top100Films}
                                    renderInput={(params) => <TextField {...params} label="Chọn size theo loại" />}
                                />
                                <Editor
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
                                    onChangeFiles={onUpload}
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
                                    onChangeFiles={onUpload}
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
