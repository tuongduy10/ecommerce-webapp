import { Box, Button, Checkbox, Container, CssBaseline, FormControlLabel, FormLabel, Grid, Input, TextField, ThemeProvider, Typography, createTheme } from "@mui/material";
import { Editor } from '@tinymce/tinymce-react';
import { useRef } from "react";
import UploadInput from "src/_shares/_components/input/upload";

const defaultTheme = createTheme();
const ProductDetail = () => {
    const editorRef = useRef(null);

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        console.log({
            email: data.get('email'),
            password: data.get('password'),
        });
    };

    const onUpload = (e: any) => {
        console.log(e);
    }

    return (
        <ThemeProvider theme={defaultTheme}>
            <Container component="main" maxWidth="md">
                <CssBaseline />
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                    }}
                >
                    <Typography component="h1" variant="h5">
                        Thêm sản phẩm
                    </Typography>
                    <Box component="form" noValidate onSubmit={handleSubmit} sx={{ mt: 3 }}>
                        <Grid container spacing={2}>
                            <Grid item xs={12} sm={6}>
                                <TextField
                                    autoComplete="given-name"
                                    name="code"
                                    required
                                    fullWidth
                                    size="small"
                                    id="code"
                                    label="Mã sản phẩm"
                                    autoFocus
                                />
                            </Grid>
                            <Grid item xs={12} sm={6}>
                                <TextField
                                    required
                                    fullWidth
                                    size="small"
                                    id="productName"
                                    label="Tên sản phẩm"
                                    name="productName"
                                    autoComplete="family-name"
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <TextField
                                    required
                                    fullWidth
                                    size="small"
                                    id="email"
                                    label="Email Address"
                                    name="email"
                                    autoComplete="email"
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <FormControlLabel control={<Checkbox value="allowExtraEmails" color="primary" />} label="Mới" />
                                <FormControlLabel control={<Checkbox value="allowExtraEmails" color="primary" />} label="Nổi bật" />
                                <FormControlLabel control={<Checkbox value="allowExtraEmails" color="primary" />} label="Sản phẩm chính hãng" />
                            </Grid>
                            <Grid item xs={12} sm={4}>
                                <TextField
                                    autoComplete="given-name"
                                    name="repay"
                                    fullWidth
                                    size="small"
                                    label="Đổi trả"
                                    autoFocus
                                />
                            </Grid>
                            <Grid item xs={12} sm={4}>
                                <TextField
                                    autoComplete="given-name"
                                    name="delivery"
                                    fullWidth
                                    size="small"
                                    label="Giao hàng"
                                    autoFocus
                                />
                            </Grid>
                            <Grid item xs={12} sm={4}>
                                <TextField
                                    autoComplete="given-name"
                                    name="insurance"
                                    fullWidth
                                    size="small"
                                    label="Bảo hành"
                                    autoFocus
                                />
                            </Grid>
                            <Grid item xs={12} sm={12}>
                                <FormLabel>Mô tả</FormLabel>
                                <>
                                    <Editor
                                        apiKey='4ctr9pi6o8mlflu8si6gdszgl62tn4zl73knhf3tkqalszsx'
                                        init={{
                                            height: 500,
                                            plugins: ["image", "autolink", "lists", "media", "table"],
                                            toolbar: 'addcomment showcomments code image pageembed permanentpen table tableofcontents',
                                        }}
                                    />
                                </>
                            </Grid>

                            <Grid item xs={12} sm={12}>
                                <FormLabel>Ảnh sản phẩm</FormLabel>
                                <label htmlFor="imageSys-upload" className="input-tile mb-2">
                                    <div className="flex">
                                        <span className="upload mr-2 text-[#4e73df] cursor-pointer">Chọn ảnh</span>
                                        <FormLabel>(*Tối đa 10, nếu có)</FormLabel>
                                    </div>
                                </label>
                                <UploadInput
                                    id={`imageSys-upload`}
                                    multiple
                                    hidden
                                    onChangeFiles={onUpload}
                                />
                            </Grid>

                            <Grid item xs={12} sm={12}>
                                <FormLabel>Ảnh thực tế</FormLabel>
                                <label htmlFor="imageUser-upload" className="input-tile mb-2">
                                    <div className="flex">
                                        <span className="upload mr-2 text-[#4e73df] cursor-pointer">Chọn ảnh</span>
                                        <FormLabel>(*Tối đa 10, nếu có)</FormLabel>
                                    </div>
                                </label>
                                <UploadInput
                                    id={`imageUser-upload`}
                                    multiple
                                    hidden
                                    onChangeFiles={onUpload}
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <TextField
                                    required
                                    fullWidth
                                    size="small"
                                    id="note"
                                    label="Ghi chú"
                                    name="note"
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <TextField
                                    required
                                    fullWidth
                                    size="small"
                                    id="link"
                                    label="Link"
                                    name="link"
                                />
                            </Grid>
                        </Grid>
                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            sx={{ mt: 3, mb: 2 }}
                        >
                            Cập nhật
                        </Button>
                    </Box>
                </Box>
            </Container>
        </ThemeProvider>
    );
}

export default ProductDetail;
