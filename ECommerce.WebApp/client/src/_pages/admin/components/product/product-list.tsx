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
import { ITableHeader } from "src/_shares/_components/data-table/data-table";
import Collapse from "@mui/material/Collapse";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { Autocomplete, Button, ButtonGroup, Grid, TablePagination, TextField } from "@mui/material";
import ProductService from "src/_cores/_services/product.service";
import InventoryService from "src/_cores/_services/inventory.service";
import { IBrand, ICategory, ISubCategory } from "src/_cores/_interfaces/inventory.interface";
import UserService from "src/_cores/_services/user.service";
import { IShop } from "src/_cores/_interfaces/user.interface";
import { IProduct } from "src/_cores/_interfaces/product.interface";
import { DateTimeHelper } from "src/_shares/_helpers/datetime-helper";
import { ProductHelper } from "src/_shares/_helpers/product-helper";
import { GlobalConfig } from "src/_configs/global.config";
import { ENV } from "src/_configs/enviroment.config";
import { PRODUCT_STATUS } from "src/_cores/_enums/product.enum";

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
    onUpdateStatus: (id: number, status: number) => void,
    onDelete: (id: number) => void,
}

function Row(props: TableRowProps) {
    const { rowData, onUpdateStatus, onDelete } = props;
    const [open, setOpen] = useState(false);

    const getFormatedPrice = (price: number) => ProductHelper.getFormatedPrice(price);
    const getProductStatus = (status: number) => ProductHelper.getProductStatus(status);

    const updateStatus = (id: number, status: number) => {
        onUpdateStatus(id, status);
    }

    const deleteProduct = (id: number) => {
        onDelete(id);
    }

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
                <TableCell align="left">
                    <Box className="flex">
                        <Box className="mr-2 items-center justify-between h-[100px] w-[100px]">
                            <img
                                src={`${ENV.IMAGE_URL}/products/${rowData.imagePaths[0]}`}
                                alt={rowData.name}
                                loading="lazy"
                            />
                        </Box>
                        <Box>
                            <Box>{rowData.name}</Box>
                            <Box>{rowData.brand?.name ?? ''}</Box>
                        </Box>
                    </Box>
                </TableCell>
                <TableCell align="left">{rowData.shop?.name}</TableCell>
                <TableCell align="left">{rowData.subCategoryName}</TableCell>
                <TableCell align="center">{rowData.categoryName}</TableCell>
                <TableCell align="center">{rowData.stock}</TableCell>
                <TableCell align="center" sx={{ color: getProductStatus(rowData.status).color }}>
                    {getProductStatus(rowData.status).label}
                </TableCell>
                <TableCell align="center">
                    <ButtonGroup size="small" aria-label="small button group">
                        <Button
                            onClick={() => updateStatus(rowData.id, PRODUCT_STATUS.AVAILABLE)}
                            variant="outlined"
                            color="success"
                        >
                            Hiện
                        </Button>
                        <Button
                            onClick={() => updateStatus(rowData.id, PRODUCT_STATUS.DISABLED)}
                        >
                            Ẩn
                        </Button>
                        <Button
                            onClick={() => deleteProduct(rowData.id)}
                            variant="outlined"
                            color="error"
                        >
                            Xóa
                        </Button>
                    </ButtonGroup>
                </TableCell>
            </TableRow>
            <TableRow sx={{ '& > *': { borderBottom: 'unset' } }}>
                <TableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={12}>
                    <Collapse in={open} timeout="auto" unmountOnExit>
                        <Box sx={{ margin: 1 }}>
                            <Typography variant="h6" gutterBottom component="div">
                                Giá và lợi nhuận
                            </Typography>
                            <Table size="small" aria-label="purchases">
                                <TableHead>
                                    <TableRow>
                                        <TableCell>Giá đặt trước</TableCell>
                                        <TableCell>Giảm giá đặt trước</TableCell>
                                        <TableCell>Giá có sẵn</TableCell>
                                        <TableCell>Giảm giá có sẵn</TableCell>
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
                                        <TableCell>{getFormatedPrice(rowData.discountAvailable)}</TableCell>
                                        <TableCell>{getFormatedPrice(rowData.pricePreOrder)}</TableCell>
                                        <TableCell>{getFormatedPrice(rowData.discountPreOrder)}</TableCell>
                                        <TableCell>{getFormatedPrice(rowData.priceImport)}</TableCell>
                                        <TableCell>{getFormatedPrice(rowData.priceForSeller)}</TableCell>
                                        <TableCell>{getFormatedPrice(rowData.profitAvailable)}</TableCell>
                                        <TableCell>{getFormatedPrice(rowData.profitPreOrder)}</TableCell>
                                        <TableCell>{getFormatedPrice(rowData.profitForSeller)}</TableCell>
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
    const [products, setProducts] = useState<any[]>([]);
    const [brands, setBrands] = useState<IBrand[]>([]);
    const [subCategories, setSubCategories] = useState<ISubCategory[]>([]);
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
        getBrands();
        getSubCategories();
        getShops();
    }

    const getBrands = () => {
        InventoryService.getBrands({}).then(res => {
            if (res?.data) {
                setBrands(res.data);
            }
        }).catch(error => {
            alert(error);
        });
    }

    const getSubCategories = () => {
        InventoryService.getSubCategories().then(res => {
            if (res?.data) {
                setSubCategories(res.data);
                console.log(subCategories)
            }
        }).catch(error => {
            alert(error);
        });
    }

    const getShops = () => {
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

    const search = (params: any) => {
        ProductService.getProductManagedList(params).then((res: any) => {
            if (res?.data) {
                setProducts(res?.data.items || []);
            }
        }).catch((error: any) => {
            console.log(error);
        });
    }

    const updateStatus = (id: number, status: number) => {
        const _params = {
            ids: [id],
            status: status,
        }
        ProductService.updateStatus(_params).then((res: any) => {
            if (res.isSucceed) {
                search(params);
            }
        }).catch((error: any) => {
            console.log(error);
        });
    }

    const deleteProduct = (id: number) => {
        const _params = {
            ids: [id]
        }
        // ProductService.deleteProduct(_params).then((res: any) => {
        //     if (res.isSucceed) {
        //         search(params);
        //     }
        // }).catch((error: any) => {
        //     console.log(error);
        // });
    }

    return (
        <Fragment>
            <Box component='form' onSubmit={onSearch}>
                <Grid container spacing={2} sx={{ marginBottom: 2 }}>
                    <Grid item xs={12} sm={4}>
                        <TextField
                            onChange={(event) => setParams({ ...params, keyword: event?.target.value ?? '' })}
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
                            onChange={(event, value) => search({ ...params, shopId: value?.id ?? -1 })}
                        />
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        <Autocomplete
                            size="small"
                            disablePortal
                            options={brands.length > 0 ? brands.map((brand: IBrand) => ({ ...brand, label: brand.name })) : []}
                            renderInput={(params) => <TextField {...params} name="brand" label="Hãng" />}
                            onChange={(event, value) => search({ ...params, brandId: value?.id ?? -1 })}
                        />
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        <Autocomplete
                            size="small"
                            disablePortal
                            options={subCategories.length > 0 ? subCategories.map((subCategory: ISubCategory) => ({ ...subCategory, label: subCategory.name })) : []}
                            renderInput={(params) => <TextField {...params} name="subCategory" label="Loại sản phẩm" />}
                            onChange={(event, value) => search({ ...params, subCategoryId: value?.id ?? -1 })}
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
                                onUpdateStatus={(id, status) => updateStatus(id, status)}
                                onDelete={(id) => deleteProduct(id)}
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