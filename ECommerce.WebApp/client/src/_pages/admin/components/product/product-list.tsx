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

const tempData = [
    {
        "id": 146,
        "ppc": null,
        "code": null,
        "name": "Máy xay ăn dặm Bear",
        "discountPercent": null,
        "stock": null,
        "status": 1,
        "isNew": true,
        "isHighlights": true,
        "isLegit": null,
        "subCategoryId": 24,
        "subCategoryName": null,
        "categoryName": null,
        "shop": null,
        "brand": {
            "id": 66,
            "name": "Bear",
            "imagePath": "brand_d5d8fb28-c399-4259-8a60-3deb4511a810.png",
            "createdDate": "0001-01-01T00:00:00",
            "isActive": false,
            "isHighlights": null,
            "isNew": null,
            "description": "<h1><span style=\"font-size: 14pt; color: rgb(0, 0, 0);\"><img style=\"display: block; margin-left: auto; margin-right: auto;\" src=\"https://digiviet.com/wp-content/uploads/2022/01/Thuong-hieu-bear-cua-nuoc-nao-1.png\" alt=\"Thương hiệu bear của nước n&agrave;o? C&oacute; tốt kh&ocirc;ng? - Digi Việt\" width=\"191\" height=\"112\"></span></h1>\n<h1><span style=\"font-size: 14pt; color: rgb(0, 0, 0);\">Bear l&agrave; thương hiệu của nước n&agrave;o? Những d&ograve;ng sản phẩm từ Bear</span></h1>\n<h2><span style=\"font-size: 14pt; color: rgb(0, 0, 0);\">Thương hiệu&nbsp;<a style=\"color: rgb(0, 0, 0);\" title=\"Bear\" href=\"https://bearvietnam.vn/\" target=\"_blank\" rel=\"nofollow noopener\">Bear</a>&nbsp;chuy&ecirc;n kinh doanh về c&aacute;c thiết bị điện gia dụng như&nbsp;nồi nấu chậm,&nbsp;bếp lẩu nướng,&nbsp;<a style=\"color: rgb(0, 0, 0);\" title=\"m&aacute;y nhồi bột, đ&aacute;nh trứng\" href=\"https://www.dienmayxanh.com/may-danh-trung-bear\" target=\"_blank\" rel=\"dofollow noopener\">m&aacute;y nhồi bột, đ&aacute;nh trứng</a>,&nbsp;<a style=\"color: rgb(0, 0, 0);\" title=\"m&aacute;y nướng b&aacute;nh m&igrave;\" href=\"https://www.dienmayxanh.com/lo-nuong-bear?g=may-nuong-banh-mi\" target=\"_blank\" rel=\"dofollow noopener\">m&aacute;y nướng b&aacute;nh m&igrave;</a>,... Bear hấp dẫn nhiều kh&aacute;ch h&agrave;ng chọn lựa bởi thiết kế bắt mắt, hiện đại v&agrave; chất lượng tốt. Qua b&agrave;i viết sau, HihiChi chia sẻ đến bạn Bear l&agrave; thương hiệu của nước n&agrave;o? Những d&ograve;ng sản phẩm từ Bear nh&eacute;!</span></h2>\n<div class=\"bannerAdNews\">\n<div class=\"bannerAdNews__content owl-carousel owl-loaded owl-drag\">\n<div class=\"owl-stage-outer owl-height\">\n<div class=\"owl-stage\">\n<div class=\"owl-item cloned\">\n<div class=\"item\">\n<h3 id=\"hmenuid1\"><span style=\"font-size: 14pt; color: rgb(0, 0, 0);\">Thương hiệu gia dụng uy t&iacute;n từ Trung Quốc</span></h3>\n<p><span style=\"font-size: 14pt; color: rgb(0, 0, 0);\">Bear l&agrave; thương hiệu điện gia dụng uy t&iacute;n v&agrave; chất lượng từ Trung Quốc, th&agrave;nh lập năm 2006, thuộc<strong>&nbsp;c&ocirc;ng ty Bear Electric Appliance</strong>. C&ocirc;ng ty chuy&ecirc;n nghi&ecirc;n cứu, ph&aacute;t triển, thiết kế sản phẩm gia dụng th&ocirc;ng minh, mang đến người ti&ecirc;u d&ugrave;ng những sản phẩm nhỏ gọn, dễ sử dụng, chất lượng ổn định.</span></p>\n<p><span style=\"font-size: 14pt; color: rgb(0, 0, 0);\"><img style=\"display: block; margin-left: auto; margin-right: auto;\" title=\"Thương hiệu gia dụng uy t&iacute;n từ Trung Quốc\" src=\"https://cdn.tgdd.vn/Files/2021/08/21/1376850/bear-la-thuong-hieu-cua-nuoc-nao-nhung-dong-san-p-12.jpg\" alt=\"Thương hiệu gia dụng uy t&iacute;n từ Trung Quốc\" width=\"730\" height=\"410\" data-original=\"https://cdn.tgdd.vn/Files/2021/08/21/1376850/bear-la-thuong-hieu-cua-nuoc-nao-nhung-dong-san-p-12.jpg\"></span></p>\n<p><span style=\"font-size: 14pt; color: rgb(0, 0, 0);\">Hiện tại thương hiệu đ&atilde; c&oacute; mặt tr&ecirc;n khắp thế giới với gần&nbsp;<strong>30 quốc gia</strong>&nbsp;như&nbsp;c&aacute;c nước Đ&ocirc;ng Nam &Aacute;, H&agrave;n Quốc, Nhật Bản, Bắc Mỹ,...&nbsp;C&ocirc;ng ty c&oacute; 4 cơ sở sản xuất với hơn<strong>&nbsp;4.000 nh&acirc;n vi&ecirc;n v&agrave; kỹ sư</strong>&nbsp;trẻ tuổi, t&agrave;i năng v&agrave; c&oacute; kinh nghiệm.</span></p>\n<p><span style=\"font-size: 14pt; color: rgb(0, 0, 0);\"><img style=\"display: block; margin-left: auto; margin-right: auto;\" title=\"C&ocirc;ng ty c&oacute; 4 cơ sở sản xuất với hơn 4.000 nh&acirc;n vi&ecirc;n \" src=\"https://cdn.tgdd.vn/Files/2021/08/21/1376850/bear-la-thuong-hieu-cua-nuoc-nao-nhung-dong-san-p-2.jpg\" alt=\"C&ocirc;ng ty c&oacute; 4 cơ sở sản xuất với hơn 4.000 nh&acirc;n vi&ecirc;n \" data-original=\"https://cdn.tgdd.vn/Files/2021/08/21/1376850/bear-la-thuong-hieu-cua-nuoc-nao-nhung-dong-san-p-2.jpg\"></span></p>\n<p><span style=\"font-size: 14pt; color: rgb(0, 0, 0);\">Thương hiệu chủ yếu sản xuất c&aacute;c sản phẩm về&nbsp;nồi nấu chậm,&nbsp;bếp lẩu nướng,&nbsp;<a style=\"color: rgb(0, 0, 0);\" title=\"m&aacute;y nhồi bột, đ&aacute;nh trứng\" href=\"https://www.dienmayxanh.com/may-danh-trung-bear\" target=\"_blank\" rel=\"dofollow noopener\">m&aacute;y nhồi bột, đ&aacute;nh trứng</a>,&nbsp;<a style=\"color: rgb(0, 0, 0);\" title=\"m&aacute;y nướng b&aacute;nh m&igrave;\" href=\"https://www.dienmayxanh.com/lo-nuong-bear?g=may-nuong-banh-mi\" target=\"_blank\" rel=\"dofollow noopener\">m&aacute;y nướng b&aacute;nh m&igrave;</a>,&nbsp;<a style=\"color: rgb(0, 0, 0);\" title=\"nồi chi&ecirc;n kh&ocirc;ng dầu\" href=\"https://www.dienmayxanh.com/noi-chien-khong-dau-bear\" target=\"_blank\" rel=\"dofollow noopener\">nồi chi&ecirc;n kh&ocirc;ng dầu</a>,... được rất nhiều kh&aacute;ch h&agrave;ng tin d&ugrave;ng v&agrave; ưa chuộng. Điểm nổi bật c&aacute;c d&ograve;ng sản phẩm đến từ thương hiệu Bear l&agrave; c&oacute; chất lượng tốt, bền bỉ, thiết kế dễ thương, sang trọng v&agrave; đ&aacute;p ứng tốt, an to&agrave;n cho người ti&ecirc;u d&ugrave;ng.<br></span></p>\n<p><span style=\"font-size: 14pt; color: rgb(0, 0, 0);\"><img style=\"display: block; margin-left: auto; margin-right: auto;\" title=\"Thương hiệu chủ yếu sản xuất c&aacute;c sản phẩm về gia dụng\" src=\"https://cdn.tgdd.vn/Files/2021/08/21/1376850/bear-la-thuong-hieu-cua-nuoc-nao-nhung-dong-san-p-4.jpg\" alt=\"Thương hiệu chủ yếu sản xuất c&aacute;c sản phẩm về gia dụng\" data-original=\"https://cdn.tgdd.vn/Files/2021/08/21/1376850/bear-la-thuong-hieu-cua-nuoc-nao-nhung-dong-san-p-4.jpg\"></span></p>\n<p><span style=\"font-size: 14pt; color: rgb(0, 0, 0);\">Bear kh&ocirc;ng ngừng nổ lực v&agrave; cố gắng trong nhiều năm qua, n&ecirc;n đ&atilde; mang về cho m&igrave;nh một số giải thưởng nổi bật như sau:</span></p>\n<ul>\n<li style=\"font-size: 14pt; color: rgb(0, 0, 0);\"><span style=\"font-size: 14pt; color: rgb(0, 0, 0);\">Top 10 Thương hiệu H&agrave;ng h&oacute;a Trực tuyến To&agrave;n cầu năm 2010.</span></li>\n<li style=\"font-size: 14pt; color: rgb(0, 0, 0);\"><span style=\"font-size: 14pt; color: rgb(0, 0, 0);\">Top 100 Thương hiệu b&aacute;n lẻ Trực tuyến h&agrave;ng đầu Trung Quốc năm 2010.</span></li>\n<li style=\"font-size: 14pt; color: rgb(0, 0, 0);\"><span style=\"font-size: 14pt; color: rgb(0, 0, 0);\">Đơn vị hợp t&aacute;c to&agrave;n vẹn truyền h&igrave;nh vệ tinh Quảng Đ&ocirc;ng năm 2011.</span></li>\n<li style=\"font-size: 14pt; color: rgb(0, 0, 0);\"><span style=\"font-size: 14pt; color: rgb(0, 0, 0);\">Top 10 sản phẩm tốt của năm 2011 tại Trung Quốc.</span></li>\n<li style=\"font-size: 14pt; color: rgb(0, 0, 0);\"><span style=\"font-size: 14pt; color: rgb(0, 0, 0);\">Giải nhất cuộc thi thiết kế sản phẩm điện kỳ diệu \"My Time Of I\" tại Bắc Kinh năm 2013 v&agrave; nhiều giải thưởng nổi bật kh&aacute;c.<br><br></span></li>\n</ul>\n<p><span style=\"font-size: 14pt; color: rgb(0, 0, 0);\"><img style=\"display: block; margin-left: auto; margin-right: auto;\" title=\"C&aacute;c th&agrave;nh t&iacute;ch của Bear đạt được trong năm 2010 v&agrave; 2011\" src=\"https://cdn.tgdd.vn/Files/2021/08/21/1376850/bear-la-thuong-hieu-cua-nuoc-nao-nhung-dong-san-p-3.jpg\" alt=\"C&aacute;c th&agrave;nh t&iacute;ch của Bear đạt được trong năm 2010 v&agrave; 2011\" data-original=\"https://cdn.tgdd.vn/Files/2021/08/21/1376850/bear-la-thuong-hieu-cua-nuoc-nao-nhung-dong-san-p-3.jpg\"></span></p>\n</div>\n</div>\n<div class=\"owl-item cloned\">\n<div class=\"item\">&nbsp;</div>\n</div>\n<div class=\"owl-item\">\n<div class=\"item\">&nbsp;</div>\n</div>\n<div class=\"owl-item\">\n<div class=\"item\">&nbsp;</div>\n</div>\n<div class=\"owl-item active\">\n<div class=\"item\">&nbsp;</div>\n</div>\n</div>\n</div>\n</div>\n</div>",
            "descriptionTitle": "Thương hiệu Bear của nước nào? Có tốt không?"
        },
        "createdDate": "2023-04-15T16:46:50.383",
        "importDate": null,
        "imagePaths": [
            "product_c048ab77-33c9-49e6-89e1-051dfb8a9671.jpg",
            "product_5cf18ba2-cf25-4301-8925-fdc6aac87374.jpg",
            "product_ea42cf0b-685c-42fc-90d3-aa1a8cdf438b.jpg",
            "product_b0214a36-2678-428d-b589-fc451905e4a6.jpg",
            "product_7c421ebf-6bae-4736-9ea6-a50ebbbda7f9.jpg"
        ],
        "userImagePaths": null,
        "description": null,
        "size": null,
        "link": null,
        "note": null,
        "repay": null,
        "delivery": null,
        "insurance": null,
        "priceAvailable": 750000,
        "discountAvailable": 579000,
        "pricePreOrder": null,
        "discountPreOrder": null,
        "priceImport": null,
        "priceForSeller": null,
        "profitAvailable": null,
        "profitPreOrder": null,
        "profitForSeller": null,
        "options": null,
        "attributes": null,
        "review": null
    }
]

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
}

function Row(props: TableRowProps) {
    const { rowData } = props;
    const [open, setOpen] = useState(false);

    const getFormatedPrice = (price: number) => ProductHelper.getFormatedPrice(price);
    const getProductStatus = (status: number) => ProductHelper.getProductStatus(status);

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
                <TableCell align="center" sx={{ color: getProductStatus(rowData.status).color }}>
                    {getProductStatus(rowData.status).label}
                </TableCell>
                <TableCell align="center">
                    <Button>Hiện</Button>
                    <Button>Ẩn</Button>
                    <Button>Xóa</Button>
                </TableCell>
            </TableRow>
            <TableRow sx={{ '& > *': { borderBottom: 'unset' } }}>
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
    const [products, setProducts] = useState<any[]>(tempData);
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
        getBrands();
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

    const getCategories = () => {
        InventoryService.getCategories().then(res => {
            if (res?.data) {
                setCategories(res.data);
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
                            options={brands.length > 0 ? brands.map((brand: IBrand) => ({ ...brand, label: brand.name })) : []}
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