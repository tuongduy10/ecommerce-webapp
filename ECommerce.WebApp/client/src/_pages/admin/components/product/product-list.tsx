import IconButton from "@mui/material/IconButton";
import Paper from "@mui/material/Paper";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import { Fragment, useState, useEffect, FormEvent } from "react";
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import { ITableData, ITableHeader } from "src/_shares/_components/data-table/data-table";
import Collapse from "@mui/material/Collapse";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { Autocomplete, Button, Grid, TablePagination, TextField } from "@mui/material";
import ProductService from "src/_cores/_services/product.service";
import InventoryService from "src/_cores/_services/inventory.service";
import { IBrand, ICategory } from "src/_cores/_interfaces/inventory.interface";
import UserService from "src/_cores/_services/user.service";
import { IShop } from "src/_cores/_interfaces/user.interface";
import { IProduct } from "src/_cores/_interfaces/product.interface";
import { DateTimeHelper } from "src/_shares/_helpers/datetime-helper";
import { ProductHelper } from "src/_shares/_helpers/product-helper";
import { GlobalConfig } from "src/_configs/global.config";

const header: ITableHeader[] = [
    { field: 'createdDate', fieldName: 'Ngày tạo', align: 'left' },
    { field: 'ppc', fieldName: 'Mã tự động', align: 'left' },
    { field: 'code', fieldName: 'Mã sản phẩm', align: 'left' },
    { field: 'name', fieldName: 'Tên sản phẩm', align: 'left' },
    { field: 'shopName', fieldName: 'Shop', align: 'left' },
    { field: 'subCategory', fieldName: 'Loại sản phẩm', align: 'left' },
    { field: 'category', fieldName: 'Danh mục', align: 'center' },
    { field: 'stock', fieldName: 'Kho', align: 'center' },
    { field: 'status', fieldName: 'Trạng thái', align: 'center' },
    { field: 'action', fieldName: '', align: 'center' }
];

type TableRowProps = {
    rowData: IProduct,
    externalData?: any,
}

function Row(props: TableRowProps) {
    const { rowData, externalData } = props;
    const [open, setOpen] = useState(false);
    const getFormatedPrice = (price: number) => ProductHelper.getFormatedPrice(price);

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
                    {DateTimeHelper.getDateTimeFormated(rowData.createdDate)}
                </TableCell>
                <TableCell align="left">{rowData.ppc}</TableCell>
                <TableCell align="left">{rowData.code}</TableCell>
                <TableCell align="left">{rowData.name}</TableCell>
                <TableCell align="left">{rowData.shop?.name}</TableCell>

                <TableCell align="left">{rowData.subCategoryName}</TableCell>
                <TableCell align="center">{rowData.categoryName}</TableCell>

                <TableCell align="center">{rowData.stock}</TableCell>
                <TableCell align="center" sx={{ color: ProductHelper.getProductStatus(rowData.status).color }}>
                    {ProductHelper.getProductStatus(rowData.status).label}
                </TableCell>
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
                                Giá
                            </Typography>
                            <Table size="small" aria-label="purchases">
                                <TableHead>
                                    <TableRow>
                                        <TableCell>Giá đặt trước</TableCell>
                                        <TableCell>Giá có sẵn</TableCell>
                                        <TableCell>Giá nhập</TableCell>
                                        <TableCell>Giá cho Seller</TableCell>
                                        <TableCell>Lợi nhuận đặt trước</TableCell>
                                        <TableCell>Lợi nhuận có sẵn</TableCell>
                                        <TableCell>Lợi nhuận cho Seller</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    <TableRow>
                                        <TableCell>{getFormatedPrice(rowData.priceAvailable)}</TableCell>
                                        <TableCell>{getFormatedPrice(rowData.pricePreOrder)}</TableCell>
                                        <TableCell>{getFormatedPrice(rowData.priceImport)}</TableCell>
                                        <TableCell>{getFormatedPrice(rowData.priceForSeller)}</TableCell>
                                        <TableCell>{getFormatedPrice(rowData.priceForSeller)}</TableCell>
                                        <TableCell>{getFormatedPrice(rowData.priceForSeller)}</TableCell>
                                        <TableCell>{getFormatedPrice(rowData.priceForSeller)}</TableCell>
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
    const [params, setParams] = useState<{ [key: string]: any }>();
    const [products, setProducts] = useState<IProduct[]>([]);
    const [brands, setBrands] = useState<IBrand[]>([]);
    const [categories, setCategories] = useState<ICategory[]>([]);
    const [shops, setShops] = useState<IShop[]>([]);

    useEffect(() => {
        initData();
        getDataFilter();
    }, []);

    const initData = () => {
        const _params = {
            ...params,
            pageIndex: 1,
            pageSize: GlobalConfig.MAX_PAGE_SIZE
        }
        setParams(_params);
        search(_params);
    }

    const getDataFilter = () => {
        InventoryService.getBrands({}).then(res => {
            if (res?.data) {
                setBrands(res.data);
            }
        }).catch(error => {
            alert(error);
        });

        InventoryService.getCategories().then(res => {
            if (res?.data) {
                setCategories(res.data);
            }
        }).catch(error => {
            alert(error);
        });

        UserService.getShops().then(res => {
            if (res?.data) {
                setShops(res.data);
            }
        }).catch(error => {
            alert(error);
        });
    }

    const pageChange = (event: any, pageIndex: number) => {

    }

    const onSearch = (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        const _params = {
            ...params,
            keyword: form.get('keyword'),
        }
        setParams(_params);
        search(params);
    }

    const onChangeSearch = (event: any, value: any, type: 'shop' | 'subCategory' | 'category' | '') => {
        switch (type) {
            case 'shop': {
                setParams({ ...params, shopId: value?.id ?? -1 });
                break;
            }
            case 'subCategory': {
                setParams({ ...params, subCategoryId: value?.id ?? -1 });
                break;
            }
            case 'category': {
                setParams({ ...params, categoryId: value?.id ?? -1 });
                break;
            }
            default: {
                setParams({
                    ...params,
                    shopId: -1,
                    subCategoryId: -1,
                    categoryId: -1
                });
                break;
            }
        }
        search(params);
    }

    const search = (params: any) => {
        ProductService.getProductManagedList(params).then((res: any) => {
            if (res?.data) {
                setProducts(res?.data.items);
            }
        }).catch((error: any) => {
            console.log(error)
        });
    }

    return (
        <Fragment>
            <Box component='form' onSubmit={onSearch}>
                <Grid container spacing={2} sx={{ marginBottom: 2 }}>
                    <Grid item xs={12} sm={4}>
                        <TextField
                            autoComplete='off'
                            name="keyword"
                            fullWidth
                            size="small"
                            label="Mã / Mã tự động / Tên sản phẩm"
                            autoFocus
                        />
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        <Autocomplete
                            size="small"
                            disablePortal
                            options={shops.length > 0 ? shops.map((shop: IShop) => ({ ...shop, label: shop.name })) : []}
                            renderInput={(params) => <TextField {...params} name="shop" label="Cửa hàng" />}
                            onChange={(event, value) => onChangeSearch(event, value, 'shop')}
                        />
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        <Autocomplete
                            size="small"
                            disablePortal
                            options={brands.length > 0 ? brands.map((brand: IBrand) => ({ ...brand, label: brand.brandName, id: brand.brandId })) : []}
                            renderInput={(params) => <TextField {...params} name="subCategory" label="Loại sản phẩm" />}
                            onChange={(event, value) => onChangeSearch(event, value, 'subCategory')}
                        />
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        <Autocomplete
                            size="small"
                            disablePortal
                            options={categories.length > 0 ? categories.map((category: ICategory) => ({ ...category, label: category.categoryName, id: category.categoryId })) : []}
                            renderInput={(params) => <TextField {...params} name="category" label="Danh mục" />}
                            onChange={(event, value) => onChangeSearch(event, value, 'category')}
                        />
                    </Grid>
                    <Grid item xs={12} sm={12}>
                        <Button type="submit" variant="contained">Tìm kiếm</Button>
                    </Grid>
                </Grid>
            </Box>
            <TableContainer component={Paper} sx={{ marginBottom: 2 }}>
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
                        {products.length > 0 && products.map((item, idx) => (
                            <Row
                                key={`row-${item.id}`}
                                rowData={item}
                            // externalData={item?.externalData}
                            />
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
            <TablePagination
                count={20}
                rowsPerPage={params?.pageSize ?? 30}
                page={params?.pageIndex ?? 1}
                onPageChange={pageChange}
                labelRowsPerPage={'Sản phẩm trên trang'}
            />
        </Fragment>
    );
}