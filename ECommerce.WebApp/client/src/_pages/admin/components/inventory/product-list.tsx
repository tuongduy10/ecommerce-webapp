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
import { useEffect, useState } from 'react';

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
                <TableCell>{rowData.category}</TableCell>
                <TableCell>{rowData.image}</TableCell>
                <TableCell align="right">{rowData.price}</TableCell>
                <TableCell align="right">{rowData.date}</TableCell>
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

    const header: ITableHeader[] = [
        { field: 'name', fieldName: 'Tên sản phẩm', align: 'left' },
        { field: 'category', fieldName: 'Loại sản phẩm', align: 'left' },
        { field: 'image', fieldName: 'Ảnh sản phẩm', align: 'left' },
        { field: 'price', fieldName: 'Giá sản phẩm', align: 'right' },
        { field: 'date', fieldName: 'Ngày', align: 'right' },
    ];
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