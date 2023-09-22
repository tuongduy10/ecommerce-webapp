import * as React from 'react';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import UserService from 'src/_cores/_services/user.service';
import SessionService from 'src/_cores/_services/session.service';
import { FAIL_MESSAGE } from 'src/_cores/_enums/message.enum';
import { ADMIN_ROUTE_NAME } from 'src/_cores/_enums/route-config.enum';
import DataUsageRoundedIcon from '@mui/icons-material/DataUsageRounded';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

// TODO remove, this demo shouldn't need to reset the theme.
const defaultTheme = createTheme();

const Login = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  useEffect(() => {
    const token = SessionService.getAccessToken();
    if (token) {
      navigate(ADMIN_ROUTE_NAME.DASHBOARD)
    }
  }, []);

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const data = new FormData(event.currentTarget);
    const params = {
      userPhone: data.get('userName'),
      password: data.get('password')
    }
    setLoading(true);
    UserService.login(params).then(resp => {
      if (resp.data) {
        SessionService.setAccessToken(resp.data);
        navigate(ADMIN_ROUTE_NAME.DASHBOARD)
      }
      setLoading(false);
    }).catch(error => {
      alert(FAIL_MESSAGE.LOGIN_FAIL);
      setLoading(false);
    });
  };

  return (
    <ThemeProvider theme={defaultTheme}>
      <Container component="main" maxWidth="xs">
        <CssBaseline />
        <Box
          sx={{
            marginTop: 8,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
          }}
        >
          <Avatar sx={{ m: 1 }}>
            <LockOutlinedIcon />
          </Avatar>
          <Typography component="h1" variant="h5">
            Đăng nhập
          </Typography>
          <Box component="form" onSubmit={handleSubmit} noValidate sx={{ mt: 1 }}>
            <TextField
              margin="normal"
              required
              fullWidth
              id="userName"
              label="Tài khoản"
              name="userName"
              autoComplete="userName"
              autoFocus
            />
            <TextField
              margin="normal"
              required
              fullWidth
              name="password"
              label="Mật khẩu"
              type="password"
              id="password"
              autoComplete="current-password"
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3, mb: 2 }}
              disabled={loading}
            >
              {loading
                ? <DataUsageRoundedIcon className="animate-spin h-5 w-5 mr-3" />
                : 'Đăng nhập'}
            </Button>
            <Grid container>
              <Grid item xs>
                <Link href="#" variant="body2">
                  Quên mật khẩu?
                </Link>
              </Grid>
            </Grid>
          </Box>
        </Box>
      </Container>
    </ThemeProvider>
  );
}

export default Login;
