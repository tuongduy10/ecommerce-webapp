import { Box, Button, Checkbox, Container, CssBaseline, FormControlLabel, FormLabel, Grid, Input, TextField, ThemeProvider, Typography, createTheme } from "@mui/material";
import { Editor } from '@tinymce/tinymce-react';
import { useRef } from "react";

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
                        Sign up
                    </Typography>
                    <Box component="form" noValidate onSubmit={handleSubmit} sx={{ mt: 3 }}>
                        <Grid container spacing={2}>
                            <Grid item xs={12} sm={6}>
                                <TextField
                                    autoComplete="given-name"
                                    name="firstName"
                                    required
                                    fullWidth
                                    size="small"
                                    id="firstName"
                                    label="First Name"
                                    autoFocus
                                />
                            </Grid>
                            <Grid item xs={12} sm={6}>
                                <TextField
                                    required
                                    fullWidth
                                    size="small"
                                    id="lastName"
                                    label="Last Name"
                                    name="lastName"
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
                                {/* <Editor /> */}
                            </Grid>
                        </Grid>
                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            sx={{ mt: 3, mb: 2 }}
                        >
                            Sign Up
                        </Button>
                    </Box>
                </Box>
            </Container>
        </ThemeProvider>
    );
}

export default ProductDetail;
