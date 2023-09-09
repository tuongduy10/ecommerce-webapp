import * as React from 'react';
import { styled, createTheme, ThemeProvider } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import MuiDrawer from '@mui/material/Drawer';
import Box from '@mui/material/Box';
import MuiAppBar, { AppBarProps as MuiAppBarProps } from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import List from '@mui/material/List';
import Typography from '@mui/material/Typography';
import Divider from '@mui/material/Divider';
import IconButton from '@mui/material/IconButton';
import Container from '@mui/material/Container';
import Grid from '@mui/material/Grid';
import Paper from '@mui/material/Paper';
import MenuIcon from '@mui/icons-material/Menu';
import ChevronLeftIcon from '@mui/icons-material/ChevronLeft';
import { ListItemButton, ListItemIcon, ListItemText, Menu, MenuItem } from '@mui/material';
import ListSubheader from '@mui/material/ListSubheader';
import HomeRoundedIcon from '@mui/icons-material/HomeRounded';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';
import PeopleIcon from '@mui/icons-material/People';
import BarChartIcon from '@mui/icons-material/BarChart';
import AssignmentIcon from '@mui/icons-material/Assignment';
import InventoryIcon from '@mui/icons-material/Inventory';
import AccountCircle from '@mui/icons-material/AccountCircle';
import { Outlet } from 'react-router-dom';
import SessionService from 'src/_cores/_services/session.service';
import { ROUTE_NAME } from 'src/_cores/_enums/route-config.enum';

const mainListItems = [
    { name: "dashboard", icon: <HomeRoundedIcon />, label: "Trang chủ", path: "/" },
    { name: "inventory", icon: <InventoryIcon />, label: "Kho", path: "" },
    { name: "order", icon: <ShoppingCartIcon />, label: "Đơn hàng", path: "" },
    { name: "user", icon: <PeopleIcon />, label: "Người dùng", path: "" },
    { name: "statistical", icon: <BarChartIcon />, label: "Thống kê", path: "" },
];

const secondListItems = [
    { name: "statistical", icon: <AssignmentIcon />, label: "Tháng hiện tại", path: "" },
    { name: "statisticalLastQuarter", icon: <AssignmentIcon />, label: "Quý trước", path: "" },
    { name: "statisticalYearEnd", icon: <AssignmentIcon />, label: "Tổng kết năm", path: "" },
];

const drawerWidth: number = 240;

interface AppBarProps extends MuiAppBarProps {
    open?: boolean;
}

const AppBar = styled(MuiAppBar, {
    shouldForwardProp: (prop) => prop !== 'open',
})<AppBarProps>(({ theme, open }) => ({
    zIndex: theme.zIndex.drawer + 1,
    transition: theme.transitions.create(['width', 'margin'], {
        easing: theme.transitions.easing.sharp,
        duration: theme.transitions.duration.leavingScreen,
    }),
    ...(open && {
        marginLeft: drawerWidth,
        width: `calc(100% - ${drawerWidth}px)`,
        transition: theme.transitions.create(['width', 'margin'], {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.enteringScreen,
        }),
    }),
}));

const Drawer = styled(MuiDrawer, { shouldForwardProp: (prop) => prop !== 'open' })(
    ({ theme, open }) => ({
        '& .MuiDrawer-paper': {
            position: 'relative',
            whiteSpace: 'nowrap',
            width: drawerWidth,
            transition: theme.transitions.create('width', {
                easing: theme.transitions.easing.sharp,
                duration: theme.transitions.duration.enteringScreen,
            }),
            boxSizing: 'border-box',
            ...(!open && {
                overflowX: 'hidden',
                transition: theme.transitions.create('width', {
                    easing: theme.transitions.easing.sharp,
                    duration: theme.transitions.duration.leavingScreen,
                }),
                width: theme.spacing(7),
                [theme.breakpoints.up('sm')]: {
                    width: theme.spacing(9),
                },
            }),
        },
    }),
);

// TODO remove, this demo shouldn't need to reset the theme.
const defaultTheme = createTheme();

export default function Dashboard() {
    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
    const [open, setOpen] = React.useState(true);
    const toggleDrawer = () => {
        setOpen(!open);
    };

    return (
        <ThemeProvider theme={defaultTheme}>
            <Box className="flex">
                <CssBaseline />
                <AppBar position="absolute" open={open}>
                    <Toolbar className='pr-[24px]'>
                        <IconButton
                            edge="start"
                            color="inherit"
                            aria-label="open drawer"
                            onClick={toggleDrawer}
                            sx={{
                                marginRight: '36px',
                                ...(open && { display: 'none' }),
                            }}
                        >
                            <MenuIcon />
                        </IconButton>
                        <Typography
                            component="h1"
                            variant="h6"
                            color="inherit"
                            noWrap
                            sx={{ flexGrow: 1 }}
                        >
                            Quản trị
                        </Typography>
                        <IconButton
                            size="large"
                            aria-label="account of current user"
                            aria-controls="menu-appbar"
                            aria-haspopup="true"
                            onClick={(event) => setAnchorEl(event.currentTarget)}
                            color="inherit"
                        >
                            <AccountCircle />
                        </IconButton>
                        <Menu
                            id="menu-appbar"
                            anchorEl={anchorEl}
                            keepMounted
                            open={Boolean(anchorEl)}
                            onClose={() => setAnchorEl(null)}
                        >
                            <MenuItem onClick={() => setAnchorEl(null)}>Hồ sơ</MenuItem>
                            <MenuItem onClick={() => { 
                                setAnchorEl(null);
                                SessionService.deleteAccessToken();
                                window.location.href = ROUTE_NAME.LOGIN;
                            }}>Đăng xuất</MenuItem>
                        </Menu>
                    </Toolbar>
                </AppBar>
                <Drawer variant="permanent" open={open}>
                    <Toolbar
                        className="flex items-center justify-end"
                        sx={{ px: [1] }}
                    >
                        <IconButton onClick={toggleDrawer}>
                            <ChevronLeftIcon />
                        </IconButton>
                    </Toolbar>
                    <Divider />
                    <List component="nav">
                        <React.Fragment>
                            {mainListItems.map(item => (
                                <ListItemButton key={item.name}>
                                    <ListItemIcon>
                                        {item.icon}
                                    </ListItemIcon>
                                    <ListItemText primary={item.label} />
                                </ListItemButton>
                            ))}
                        </React.Fragment>
                        <Divider sx={{ my: 1 }} />
                        <React.Fragment>
                            <ListSubheader component="div" inset>
                                Báo cáo
                            </ListSubheader>
                            {secondListItems.map(item => (
                                <ListItemButton key={item.name}>
                                    <ListItemIcon>
                                        {item.icon}
                                    </ListItemIcon>
                                    <ListItemText primary={item.label} />
                                </ListItemButton>
                            ))}
                        </React.Fragment>
                    </List>
                </Drawer>
                <Box
                    className='overflow-auto grow h-screen'
                    component="main"
                    sx={{ backgroundColor: (theme) => theme.palette.grey[100] }}
                >
                    <Toolbar />
                    <Container maxWidth={false} sx={{ mt: 4, mb: 4 }}>
                        <Paper className='flex flex-col h-full' sx={{ p: 2 }}>
                            <Outlet />
                        </Paper>
                    </Container>
                </Box>
            </Box>
        </ThemeProvider>
    );
}