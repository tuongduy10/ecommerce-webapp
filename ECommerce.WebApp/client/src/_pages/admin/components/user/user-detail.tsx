import { Box, CssBaseline, Grid, TextField, ThemeProvider, Typography, createTheme } from "@mui/material";

const defaultTheme = createTheme();

export default function UserDetail() {

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);

    }

    return (
        <ThemeProvider theme={defaultTheme}>
            <Typography component="h1" variant="h5">
                Người dùng
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
                                <Grid item xs={12} sm={8}>
                                    <TextField
                                        // value={dataDetail['code']}
                                        // onChange={(event) => onChangeFieldValue('code', event?.target.value)}
                                        InputLabelProps={{ shrink: true }}
                                        autoComplete='off'
                                        name="code"
                                        required
                                        fullWidth
                                        size="small"
                                        label="Họ tên"
                                        autoFocus
                                    />
                                </Grid>
                                <Grid item xs={12} sm={8}>
                                    <TextField
                                        // value={dataDetail['code']}
                                        // onChange={(event) => onChangeFieldValue('code', event?.target.value)}
                                        InputLabelProps={{ shrink: true }}
                                        autoComplete='off'
                                        name="code"
                                        required
                                        fullWidth
                                        size="small"
                                        label="Email"
                                        autoFocus
                                    />
                                </Grid>
                                <Grid item xs={12} sm={8}>
                                    <TextField
                                        // value={dataDetail['code']}
                                        // onChange={(event) => onChangeFieldValue('code', event?.target.value)}
                                        InputLabelProps={{ shrink: true }}
                                        autoComplete='off'
                                        name="code"
                                        required
                                        fullWidth
                                        size="small"
                                        label="Địa chỉ"
                                        autoFocus
                                    />
                                </Grid>
                            </Grid>
                        </Box>
                    </Grid>
                </Grid>
            </Box>
        </ThemeProvider>
    );
}