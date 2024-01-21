import IconButton from "@mui/material/IconButton";
import Paper from "@mui/material/Paper";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import { useState, useEffect } from "react";
import UserService from "src/_cores/_services/user.service";
import { IUserGetParam } from "../../interfaces/user-interface";
import { UserHelper } from "src/_shares/_helpers/user-helper";
import { DateTimeHelper } from "src/_shares/_helpers/datetime-helper";
import InfoOutlinedIcon from '@mui/icons-material/InfoOutlined';
import { Button, Menu, MenuItem } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { ADMIN_ROUTE_NAME } from "src/_cores/_enums/route-config.enum";
import { ITableHeader } from "src/_shares/_components/data-table/data-table";

function Row(props: any) {
    const { rowData } = props;
    const navigate = useNavigate();
    const [open, setOpen] = useState(false);
    const [delAnchorEl, setDelAnchorEl] = useState<null | HTMLElement>(null);
    const openDel = Boolean(delAnchorEl);

    const handleClickDel = (event: React.MouseEvent<HTMLButtonElement>) => {
        setDelAnchorEl(event.currentTarget);
    };

    const handleCloseDel = () => {
        setDelAnchorEl(null);
    };

    const getUserStatus = (status: boolean) => UserHelper.getUserStatus(status);

    const updateStatus = (id: number, status: boolean) => {

    }

    const deleteUser = (id: number) => {

    }

    const viewDetail = (id: number) => {
        navigate({
            pathname: ADMIN_ROUTE_NAME.MANAGE_USER_DETAIL,
            search: `?id=${id}`
        });
    }

    const userAddress = (user: any): string => {
        let address = user.userAddress ?? "";
        if (user.userWardName) {
            address += `, ${user.userWardName}`;
        }
        if (user.userDistrictName) {
            address += `, ${user.userDistrictName}`;
        }
        if (user.userCityName) {
            address += `, ${user.userCityName}`;
        }
        return address;
    }

    return (
        <TableRow sx={{ '& > *': { borderBottom: 'unset' } }}>
            <TableCell>
                <IconButton onClick={() => viewDetail(rowData.userId)}>
                    <InfoOutlinedIcon />
                </IconButton>
            </TableCell>
            <TableCell>
                {DateTimeHelper.getDateTimeFormated(rowData.userJoinDate)}
            </TableCell>
            <TableCell>{rowData.userFullName}</TableCell>
            <TableCell>{userAddress(rowData)}</TableCell>
            <TableCell>{rowData.userMail}</TableCell>
            <TableCell>{rowData.userPhone}</TableCell>
            <TableCell align="center" sx={{ color: getUserStatus(rowData.status).color }}>
                {getUserStatus(rowData.status).label}
            </TableCell>
            <TableCell align="center">
                <Button
                    onClick={() => updateStatus(rowData.id, true)}
                    variant="outlined"
                    color="success"
                >
                    Hiện
                </Button>
                <Button
                    onClick={() => updateStatus(rowData.id, false)}
                >
                    Ẩn
                </Button>
                <Button
                    variant="outlined"
                    color="error"
                    aria-controls={openDel ? 'basic-menu' : undefined}
                    aria-haspopup="true"
                    aria-expanded={openDel ? 'true' : undefined}
                    onClick={handleClickDel}
                >
                    Xóa
                </Button>
                <Menu
                    anchorEl={delAnchorEl}
                    open={openDel}
                    onClose={handleCloseDel}
                    MenuListProps={{
                        'aria-labelledby': 'basic-button',
                    }}
                >
                    <MenuItem onClick={() => deleteUser(rowData.id)}>Xác nhận xóa</MenuItem>
                    <MenuItem onClick={handleCloseDel}>Hủy</MenuItem>
                </Menu>
            </TableCell>
        </TableRow>
    );
}

export default function UserList() {
    const [users, setUsers] = useState([]);

    useEffect(() => {
        const param: IUserGetParam = {
            keyword: "",
            userId: -1,
            pageIndex: 1,
            pageSize: 50,
        }
        getData(param);
    }, []);

    const getData = (param: IUserGetParam) => {
        UserService.getUserList(param).then((res: any) => {
            if (res.isSucceed) {
                setUsers(res.data.items);
            }
        }).catch(error => {
            console.log(error);
        })
    }

    const header: ITableHeader[] = [
        { field: 'joinDate', fieldName: 'Ngày tham gia', align: 'left' },
        { field: 'name', fieldName: 'Tên', align: 'left' },
        { field: 'address', fieldName: 'Địa chỉ', align: 'left' },
        { field: 'email', fieldName: 'Email', align: 'left' },
        { field: 'phone', fieldName: 'Số điện thoại', align: 'left' },
        { field: 'status', fieldName: 'Trạng thái', align: 'center' },
        { field: '', fieldName: '', align: 'center' },
    ];

    return (
        <TableContainer component={Paper}>
            <Table aria-label="collapsible table">
                <TableHead>
                    <TableRow>
                        <TableCell />
                        {header.map((field) => (
                            <TableCell key={field.field} align={!field.align ? 'left' : field.align}>{field.fieldName}</TableCell>
                        ))}
                        <TableCell />
                    </TableRow>
                </TableHead>
                <TableBody>
                    {users.length > 0 && users.map((item: any, idx) => (
                        <Row
                            key={`row-${item.userId}`}
                            rowData={item}
                        />
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
}