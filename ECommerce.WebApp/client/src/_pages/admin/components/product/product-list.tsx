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
import { Autocomplete, Button, Checkbox, Grid, Link, Menu, MenuItem, Pagination, TextField } from "@mui/material";
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
import { ADMIN_ROUTE_NAME } from "src/_cores/_enums/route-config.enum";
import { Link as RouterLink } from "react-router-dom";

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
    isSelected: boolean,
    onUpdateStatus: (id: number, status: number) => void,
    onDelete: (id: number) => void,
    onSelected: (id: number) => void,
}

function Row(props: TableRowProps) {
    const { rowData, isSelected, onUpdateStatus, onDelete, onSelected } = props;
    const [delAnchorEl, setDelAnchorEl] = useState<null | HTMLElement>(null);
    const [open, setOpen] = useState(false);
    const openDel = Boolean(delAnchorEl);

    const getFormatedPrice = (price: number) => ProductHelper.getFormatedPrice(price);
    const getProductStatus = (status: number) => ProductHelper.getProductStatus(status);

    const updateStatus = (id: number, status: number) => {
        onUpdateStatus(id, status);
    }

    const deleteProduct = (id: number) => {
        onDelete(id);
        handleCloseDel();
    }

    const handleClickDel = (event: React.MouseEvent<HTMLButtonElement>) => {
        setDelAnchorEl(event.currentTarget);
    };

    const handleCloseDel = () => {
        setDelAnchorEl(null);
    };

    return (
        <Fragment>
            <TableRow sx={{ '& > *': { borderBottom: 'unset' } }}>
                <TableCell>
                    <Checkbox
                        checked={isSelected}
                        onChange={() => onSelected(rowData.id)}
                    />
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
                <TableCell align="left">
                    <Link component={RouterLink} to={ADMIN_ROUTE_NAME.MANAGE_PRODUCT_DETAIL + '?id=' + rowData.id}>
                        {rowData.ppc}
                    </Link>
                </TableCell>
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
                        <MenuItem onClick={() => deleteProduct(rowData.id)}>Xác nhận xóa</MenuItem>
                        <MenuItem onClick={handleCloseDel}>Hủy</MenuItem>
                    </Menu>
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
    const [params, setParams] = useState<any>({
        keyword: "",
        shopId: -1,
        brandId: -1,
        subCategoryId: -1,
        categoryId: -1,
        pageIndex: 1,
        pageSize: GlobalConfig.MAX_PAGE_SIZE,
        totalPage: 1,
    });
    const [products, setProducts] = useState<any[]>([]);
    const [brands, setBrands] = useState<IBrand[]>([]);
    const [subCategories, setSubCategories] = useState<ISubCategory[]>([]);
    const [categories, setCategories] = useState<ICategory[]>([]);
    const [shops, setShops] = useState<IShop[]>([]);

    const [selectedProducts, setSelectedProducts] = useState<number[]>([]);
    const [delAnchorEl, setDelAnchorEl] = useState<null | HTMLElement>(null);
    const openDel = Boolean(delAnchorEl);

    useEffect(() => {
        search(params);
        getDataFilter();
    }, []);

    const isIndeterminate = (): boolean => selectedProducts.length > 0 && selectedProducts.length < products.length;
    const isSelectedAll = (): boolean => selectedProducts.length > 0 && selectedProducts.length === products.length;

    const getDataFilter = () => {
        getBrands();
        getSubCategories();
        getCategories();
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
            }
        }).catch(error => {
            alert(error);
        });
    }

    const getCategories = () => {
        InventoryService.getCategories().then(res => {
            if (res?.data) {
                setCategories(res?.data);
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
        params.pageIndex = pageIndex;
        setParams(params);
        search(params);
    }

    const onSearch = (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        params.keyword = form.get('keyword');
        setParams(params);
        search(params);
    }

    const search = (_params: any) => {
        ProductService.getProductManagedList(_params).then((res: any) => {
            if (res?.data) {
                setProducts(res.data.items || []);
                params.pageIndex = res.data.currentPage;
                params.totalPage = res.data.totalPage;
                setParams(params);
            }
        }).catch((error: any) => {
            console.log(error);
        });
        setSelectedProducts([]);
    }

    const updateStatus = (id: number, status: number) => {
        const _params = {
            ids: selectedProducts.length > 0 ? selectedProducts : [id],
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

    const deleteProduct = (id?: number) => {
        const _params = {
            ids: selectedProducts.length > 0 ? selectedProducts : [id ?? -1]
        }
        ProductService.deleteProduct(_params).then((res: any) => {
            if (res.isSucceed) {
                search(params);
            }
        }).catch((error: any) => {
            console.log(error);
        });
    }

    const selectAllProducts = () => {
        if (isIndeterminate() || isSelectedAll()) {
            setSelectedProducts([]);
            return;
        }
        const ids = [...products].map(_ => _.id);
        setSelectedProducts(ids);
    }

    const selectProduct = (id: number) => {
        if (!selectedProducts.includes(id)) {
            const addNewSelected = selectedProducts.concat(id);
            setSelectedProducts(addNewSelected);
        } else {
            const removeSelected = selectedProducts.filter(_ => _ !== id);
            setSelectedProducts(removeSelected);
        }
    }

    const handleClickDel = (event: React.MouseEvent<HTMLButtonElement>) => {
        setDelAnchorEl(event.currentTarget);
        deleteProduct();
    };

    const onChangeShop = (value: any) => {
        params.shopId = value?.id ?? -1
        setParams(params);
        search(params);
    }

    const onChangeBrand = (value: any) => {
        params.brandId = value?.id ?? -1;
        setParams(params);
        search(params);
    }

    const onChangeSubCategory = (value: any) => {
        params.subCategoryId = value?.id ?? -1
        setParams(params);
        search(params);
    }

    const onChangeCategory = (value: any) => {
        params.categoryId = value?.categoryId ?? -1;
        setParams(params);
        search(params);
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
                            onChange={(event, value) => onChangeShop(value)}
                        />
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        <Autocomplete
                            size="small"
                            disablePortal
                            options={brands.length > 0 ? brands.map((brand: IBrand) => ({ ...brand, label: brand.name })) : []}
                            renderInput={(params) => <TextField {...params} name="brand" label="Hãng" />}
                            onChange={(event, value) => onChangeBrand(value)}
                        />
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        <Autocomplete
                            size="small"
                            disablePortal
                            options={subCategories.length > 0 ? subCategories.map((subCategory: ISubCategory) => ({ ...subCategory, label: subCategory.name })) : []}
                            renderInput={(params) => <TextField {...params} name="subCategory" label="Loại sản phẩm" />}
                            onChange={(event, value) => onChangeSubCategory(value)}
                        />
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        <Autocomplete
                            size="small"
                            disablePortal
                            options={categories.length > 0 ? categories.map((category: ICategory) => ({ ...category, label: category.categoryName })) : []}
                            renderInput={(params) => <TextField {...params} name="category" label="Danh mục" />}
                            onChange={(event, value) => onChangeCategory(value)}
                        />
                    </Grid>
                    <Grid item xs={12} sm={12}>
                        <Button type="submit" variant="contained" sx={{ marginRight: 2 }}>Tìm kiếm</Button>
                        <Button
                            variant="outlined"
                            color="success"
                            disabled={selectedProducts.length === 0}
                            sx={{ marginRight: 2 }}
                            onClick={() => updateStatus(-1, PRODUCT_STATUS.AVAILABLE)}
                        >
                            Hiện
                        </Button>
                        <Button
                            variant="outlined"
                            sx={{ marginRight: 2 }}
                            disabled={selectedProducts.length === 0}
                            onClick={() => updateStatus(-1, PRODUCT_STATUS.DISABLED)}
                        >
                            Ẩn
                        </Button>
                        <Button
                            variant="outlined"
                            color="error"
                            aria-controls={openDel ? 'basic-menu' : undefined}
                            aria-expanded={openDel ? 'true' : undefined}
                            aria-haspopup="true"
                            onClick={handleClickDel}
                            disabled={selectedProducts.length === 0}
                        >
                            Xóa
                        </Button>
                        <Menu
                            anchorEl={delAnchorEl}
                            open={openDel}
                            onClose={() => setDelAnchorEl(null)}
                            MenuListProps={{
                                'aria-labelledby': 'basic-button',
                            }}
                        >
                            <MenuItem onClick={() => deleteProduct()}>Xác nhận xóa</MenuItem>
                            <MenuItem onClick={() => setDelAnchorEl(null)}>Hủy</MenuItem>
                        </Menu>
                    </Grid>
                </Grid>
            </Box>
            <TableContainer component={Paper} sx={{ marginBottom: 2 }}>
                <Table aria-label="collapsible table">
                    <TableHead>
                        <TableRow>
                            <TableCell>
                                <Checkbox
                                    indeterminate={isIndeterminate()}
                                    checked={isSelectedAll()}
                                    onChange={selectAllProducts}
                                />
                            </TableCell>
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
                                isSelected={selectedProducts.includes(item.id)}
                                onUpdateStatus={(id, status) => updateStatus(id, status)}
                                onDelete={(id) => deleteProduct(id)}
                                onSelected={(id) => selectProduct(id)}
                            />
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
            <Pagination
                variant="outlined"
                shape="rounded"
                page={params?.pageIndex ?? 1}
                count={params?.totalPage ?? 1}
                onChange={pageChange}
            />
        </Fragment>
    );
}