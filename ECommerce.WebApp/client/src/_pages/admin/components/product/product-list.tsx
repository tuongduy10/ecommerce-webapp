import IconButton from "@mui/material/IconButton";
import Paper from "@mui/material/Paper";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import { Fragment, useState, useEffect } from "react";
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import { ITableData, ITableHeader } from "src/_shares/_components/data-table/data-table";
import Collapse from "@mui/material/Collapse";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";

const header: ITableHeader[] = [
    { field: 'createdDate', fieldName: 'Ngày tạo', align: 'left' },
    { field: 'ppc', fieldName: 'Mã tự động', align: 'left' },
    { field: 'code', fieldName: 'Mã sản phẩm', align: 'left' },
    { field: 'name', fieldName: 'Tên sản phẩm', align: 'left' },
    { field: 'shopName', fieldName: 'Shop', align: 'left' },

    { field: 'pricePreOrder', fieldName: 'Giá đặt trước', align: 'right' },
    { field: 'priceAvailable', fieldName: 'Giá có sẵn', align: 'right' },
    { field: 'subCategory', fieldName: 'Loại sản phẩm', align: 'left' },

    { field: 'category', fieldName: 'Danh mục', align: 'center' },
    { field: 'stock', fieldName: 'Kho', align: 'center' },
    { field: 'status', fieldName: 'Trạng thái', align: 'center' },
    { field: 'action', fieldName: '', align: 'center' }
];

function Row(props: any) {
    const { rowData, externalData } = props;
    const [open, setOpen] = useState(false);

    return (
        <Fragment>
            <TableRow sx={{ '& > *': { borderBottom: 'unset' } }}>
                <TableCell>
                    <IconButton
                        aria-label="expand row"
                        size="small"
                        onClick={() => setOpen(!open)}
                    >
                        {open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
                    </IconButton>
                </TableCell>
                <TableCell component="th" scope="row">
                    {rowData.name}
                </TableCell>
                <TableCell align="left">{rowData.category}</TableCell>
                <TableCell align="left">{rowData.image}</TableCell>
                <TableCell align="left">{rowData.price}</TableCell>
                <TableCell align="left">{rowData.date}</TableCell>

                <TableCell align="right">123</TableCell>
                <TableCell align="right">456</TableCell>
                <TableCell align="left">789</TableCell>


                <TableCell align="center">zxc</TableCell>
                <TableCell align="center">qwe</TableCell>
                <TableCell align="center">asd</TableCell>
                <TableCell align="center">
                    <button>Hiện</button>
                    <button>Ẩn</button>
                    <button>Xóa</button>
                </TableCell>
            </TableRow>
            <TableRow>
                <TableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={6}>
                    <Collapse in={open} timeout="auto" unmountOnExit>
                        <Box sx={{ margin: 1 }}>
                            <Typography variant="h6" gutterBottom component="div">
                                History
                            </Typography>
                            <Table size="small" aria-label="purchases">
                                <TableHead>
                                    <TableRow>
                                        <TableCell>Date</TableCell>
                                        <TableCell>Customer</TableCell>
                                        <TableCell align="right">Amount</TableCell>
                                        <TableCell align="right">Total price ($)</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    <TableRow>
                                        <TableCell component="th" scope="row">10/11/2000</TableCell>
                                        <TableCell>1</TableCell>
                                        <TableCell align="right">12</TableCell>
                                        <TableCell align="right">100</TableCell>
                                    </TableRow>
                                </TableBody>
                            </Table>
                        </Box>
                    </Collapse>
                </TableCell>
            </TableRow>
        </Fragment>
    );
}

export default function ProductList() {

    useEffect(() => {
        // your logic
    }, []);

    const data: ITableData[] = [
        { id: "1", name: 'Tên 1', category: 'Loại 1', image: 'Ảnh 1', price: '1.000 đ', date: '20/10/2010', },
        { id: "2", name: 'Tên 2', category: 'Loại 2', image: 'Ảnh 2', price: '1.000 đ', date: '20/10/2010', },
        { id: "3", name: 'Tên 3', category: 'Loại 3', image: 'Ảnh 3', price: '1.000 đ', date: '20/10/2010', },
        { id: "4", name: 'Tên 4', category: 'Loại 4', image: 'Ảnh 4', price: '1.000 đ', date: '20/10/2010', },
        { id: "5", name: 'Tên 5', category: 'Loại 5', image: 'Ảnh 5', price: '1.000 đ', date: '20/10/2010', },
        { id: "6", name: 'Tên 6', category: 'Loại 6', image: 'Ảnh 6', price: '1.000 đ', date: '20/10/2010', },
        { id: "7", name: 'Tên 6', category: 'Loại 6', image: 'Ảnh 6', price: '1.000 đ', date: '20/10/2010', },
        { id: "8", name: 'Tên 6', category: 'Loại 6', image: 'Ảnh 6', price: '1.000 đ', date: '20/10/2010', },
        { id: "9", name: 'Tên 6', category: 'Loại 6', image: 'Ảnh 6', price: '1.000 đ', date: '20/10/2010', },
        { id: "10", name: 'Tên 6', category: 'Loại 6', image: 'Ảnh 6', price: '1.000 đ', date: '20/10/2010', },
        { id: "11", name: 'Tên 6', category: 'Loại 6', image: 'Ảnh 6', price: '1.000 đ', date: '20/10/2010', },
    ]
    return (
        <TableContainer component={Paper}>
            <Table aria-label="collapsible table">
                <TableHead>
                    <TableRow>
                        <TableCell />
                        {header.map((field) => (
                            <TableCell key={field.field} align={!field.align ? 'left' : field.align}>{field.fieldName}</TableCell>
                        ))}
                    </TableRow>
                </TableHead>
                <TableBody>
                    {data.length > 0 && data.map((item, idx) => (
                        <Row
                            key={`row-${item.id}`}
                            rowData={item}
                            externalData={item?.externalData}
                        />
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
}