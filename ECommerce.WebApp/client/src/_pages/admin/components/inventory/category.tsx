import { Autocomplete, Box, Button, Checkbox, Collapse, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Grid, IconButton, ListItemText, Menu, MenuItem, OutlinedInput, Paper, Popper, Select, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TextField, Typography } from "@mui/material";
import { FormEvent, Fragment, useEffect, useState } from "react";
import { ICategory } from "src/_cores/_interfaces/inventory.interface";
import { ITableHeader } from "src/_shares/_components/data-table/data-table";
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import CheckBoxOutlineBlankIcon from '@mui/icons-material/CheckBoxOutlineBlank';
import CheckBoxIcon from '@mui/icons-material/CheckBox';
import ModeEditIcon from '@mui/icons-material/ModeEdit';
import InventoryService from "src/_cores/_services/inventory.service";

const icon = <CheckBoxOutlineBlankIcon fontSize="small" />;
const checkedIcon = <CheckBoxIcon fontSize="small" />;

const header: ITableHeader[] = [
    { field: 'name', fieldName: 'Tên danh mục', align: 'left' },
    { field: 'action', fieldName: '', align: 'center' }
];

type TableRowProps = {
    rowData: ICategory,
    isSelected: boolean,
    onUpdateStatus: (id: number, status: number) => void,
    onDelete: (id: number) => void,
    onSelected: (id: number) => void,
    onViewDetail: (id: number) => void,
}

function Row(props: TableRowProps) {
    const { rowData, isSelected, onUpdateStatus, onDelete, onSelected, onViewDetail } = props;
    const [delAnchorEl, setDelAnchorEl] = useState<null | HTMLElement>(null);
    const [open, setOpen] = useState(false);
    const openDel = Boolean(delAnchorEl);


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

    const viewDetail = (id: number) => {
        onViewDetail(id)
    }

    return (
        <Fragment>
            <TableRow sx={{ '& > *': { borderBottom: 'unset' } }}>
                <TableCell>
                    <Checkbox
                        checked={isSelected}
                        onChange={() => onSelected(rowData.categoryId)}
                        sx={{ display: 'none' }}
                    />
                    <IconButton
                        aria-label="expand row"
                        size="small"
                        onClick={() => setOpen(!open)}
                        sx={{ display: 'none' }}
                    >
                        {open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
                    </IconButton>
                    <IconButton
                        size="small"
                        onClick={() => viewDetail(rowData.categoryId)}
                    >
                        <ModeEditIcon />
                    </IconButton>
                </TableCell>
                <TableCell align="left">{rowData.categoryName}</TableCell>
                <TableCell align="center">
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
                        <MenuItem onClick={() => deleteProduct(rowData.categoryId)}>Xác nhận xóa</MenuItem>
                        <MenuItem onClick={handleCloseDel}>Hủy</MenuItem>
                    </Menu>
                </TableCell>
            </TableRow>
            <TableRow sx={{ '& > *': { borderBottom: 'unset' } }}>
                <TableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={12}>
                    <Collapse in={open} timeout="auto" unmountOnExit>

                    </Collapse>
                </TableCell>
            </TableRow>
        </Fragment>
    );
}

export default function Category() {
    const [open, setOpen] = useState(false);
    const [categories, setCategories] = useState<any[]>([]);
    const [subCategories, setSubCategories] = useState<any[]>([]);
    const [selectedCategories, setSelectedCategories] = useState<number[]>([]);
    const [formData, setFormData] = useState({
        categoryId: -1,
        categoryName: '',
    });

    useEffect(() => {
        search();
    }, []);

    const isIndeterminate = (): boolean => selectedCategories.length > 0 && selectedCategories.length < categories.length;
    const isSelectedAll = (): boolean => selectedCategories.length > 0 && selectedCategories.length === categories.length;

    const handleChange = (e: any) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value,
        });
    };

    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
        setFormData({
            ...formData,
            categoryId: -1,
            categoryName: ''
        });
    };

    const search = async (_params?: any) => {
        const response = await InventoryService.getCategories() as any;
        if (response?.isSucceed) {
            setCategories(response.data);
        }
        const subResponse = await InventoryService.getSubCategories() as any;
        if (subResponse?.isSucceed) {
            setSubCategories(subResponse.data);
        }
        setSelectedCategories([]);
    }

    const onSearch = (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
    }

    const selectAll = () => {
        if (isIndeterminate() || isSelectedAll()) {
            setSelectedCategories([]);
            return;
        }
        const ids = [...categories].map(_ => _.id);
        setSelectedCategories(ids);
    }

    const updateStatus = (id: number, status: number) => {
        const _params = {
            ids: selectedCategories.length > 0 ? selectedCategories : [id],
            status: status,
        }
    }

    const deleteCategory = (id?: number) => {
        const _params = {
            ids: selectedCategories.length > 0 ? selectedCategories : [id ?? -1]
        }

    }

    const selectCategory = (id: number) => {
        if (!selectedCategories.includes(id)) {
            const addNewSelected = selectedCategories.concat(id);
            setSelectedCategories(addNewSelected);
        } else {
            const removeSelected = selectedCategories.filter(_ => _ !== id);
            setSelectedCategories(removeSelected);
        }
    }

    const viewDetail = async (id: number) => {
        const response = await InventoryService.getCategory(id) as any;
        if (response?.isSucceed) {
            const category = response?.data;
            const _value = {
                ...formData,
                categoryId: category.categoryId,
                categoryName: category.categoryName,
            }
            setFormData(_value);
            if (response.data)
                handleClickOpen();
        }
    }

    const handleSubmit = async (e: any) => {
        e.preventDefault();
        const returnToList = async () => {
            await search();
            handleClose();
        }
        const param = {
            id: formData.categoryId,
            name: formData.categoryName
        }
        if (formData.categoryId > -1) {
            const reqUpdate = await InventoryService.updateCategory(param) as any;
            if (reqUpdate?.isSucceed) {
                await returnToList();
            }
        } else {
            const reqAdd = await InventoryService.addCategory({ name: formData.categoryName }) as any;
            if (reqAdd?.isSucceed) {
                await returnToList();
            }
        }
    };

    return (
        <Fragment>
            <Box>
                <Grid container spacing={2} sx={{ marginBottom: 2 }}>
                    <Grid item xs={12} sm={4}>
                        <Button variant="contained" sx={{ marginRight: 2 }} onClick={handleClickOpen}>Thêm</Button>
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
                                    onChange={selectAll}
                                    sx={{ display: 'none' }}
                                />
                            </TableCell>
                            {header.map((field) => (
                                <TableCell key={field.field} align={!field.align ? 'left' : field.align}>{field.fieldName}</TableCell>
                            ))}
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {categories.length > 0 && categories.map((item, idx) => (
                            <Row
                                key={`row-${item.categoryId}`}
                                rowData={item}
                                isSelected={selectedCategories.includes(item.categoryId)}
                                onUpdateStatus={(id, status) => updateStatus(id, status)}
                                onDelete={(id) => deleteCategory(id)}
                                onSelected={(id) => selectCategory(id)}
                                onViewDetail={(id) => viewDetail(id)}
                            />
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>

            <Dialog
                fullWidth={true}
                open={open}
                onClose={handleClose}
                PaperProps={{
                    style: { height: '80vh' },
                }}
            >
                <Box component={'form'} onSubmit={handleSubmit}>
                    <DialogTitle>Danh mục</DialogTitle>
                    <DialogContent>
                        <TextField
                            autoFocus
                            required
                            margin="dense"
                            name="categoryName"
                            label="Tên danh mục"
                            type="text"
                            fullWidth
                            variant="standard"
                            sx={{ marginBottom: 2 }}
                            value={formData.categoryName || ''}
                            onChange={handleChange}
                        />
                        <Autocomplete
                            multiple
                            size="small"
                            disablePortal
                            options={subCategories}
                            getOptionLabel={(_) => _.name}
                            value={subCategories.length > 0 ? subCategories.filter(_ => _.categoryId === formData.categoryId) : []}
                            renderInput={(params) => <TextField {...params} name="subCategory" label="Loại sản phẩm" />}
                            renderOption={(props, _, { selected }) => (
                                <li {...props}>
                                    <Checkbox
                                        icon={icon}
                                        checkedIcon={checkedIcon}
                                        style={{ marginRight: 8 }}
                                        checked={selected}
                                        size="small"
                                    />
                                    {_.name}
                                </li>
                            )}
                        // onChange={(event, value) => onChangeShop(value)}
                        />
                    </DialogContent>
                    <DialogActions>
                        <Button onClick={handleClose}>Đóng</Button>
                        <Button type="submit">Xác nhận</Button>
                    </DialogActions>
                </Box>
            </Dialog>
        </Fragment>
    );
}