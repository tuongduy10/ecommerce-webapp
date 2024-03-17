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
    { field: 'name', fieldName: 'Tên chi tiết', align: 'left' },
    { field: 'action', fieldName: '', align: 'center' }
];

type TableRowProps = {
    rowData: any,
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
                        onChange={() => onSelected(rowData.id)}
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
                        onClick={() => viewDetail(rowData.id)}
                    >
                        <ModeEditIcon />
                    </IconButton>
                </TableCell>
                <TableCell align="left">{rowData.name}</TableCell>
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
                        <MenuItem onClick={() => deleteProduct(rowData.id)}>Xác nhận xóa</MenuItem>
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

export default function Attributes() {
    const [open, setOpen] = useState(false);
    const [attributes, setAttributes] = useState<any[]>([]);
    const [selectedItems, setSelectedItems] = useState<number[]>([]);
    const [formData, setFormData] = useState({
        id: -1,
        name: '',
    });

    useEffect(() => {
        search();
    }, []);

    const isIndeterminate = (): boolean => selectedItems.length > 0 && selectedItems.length < attributes.length;
    const isSelectedAll = (): boolean => selectedItems.length > 0 && selectedItems.length === attributes.length;

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
        setFormData({
            ...formData,
            id: -1,
            name: ''
        });
        setOpen(false);
    };

    const search = async (_params?: any) => {
        const response = await InventoryService.getAttributes() as any;
        if (response?.isSucceed) {
            setAttributes(response.data);
        }
        setSelectedItems([]);
    }

    const onSearch = (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
    }

    const selectAll = () => {
        if (isIndeterminate() || isSelectedAll()) {
            setSelectedItems([]);
            return;
        }
        const ids = [...attributes].map(_ => _.id);
        setSelectedItems(ids);
    }

    const updateStatus = (id: number, status: number) => {
        const _params = {
            ids: selectedItems.length > 0 ? selectedItems : [id],
            status: status,
        }
    }

    const deleteCategory = (id?: number) => {
        const _params = {
            ids: selectedItems.length > 0 ? selectedItems : [id ?? -1]
        }

    }

    const selectCategory = (id: number) => {
        if (!selectedItems.includes(id)) {
            const addNewSelected = selectedItems.concat(id);
            setSelectedItems(addNewSelected);
        } else {
            const removeSelected = selectedItems.filter(_ => _ !== id);
            setSelectedItems(removeSelected);
        }
    }

    const viewDetail = async (id: number) => {
        const response = await InventoryService.getAttributes({ id: id }) as any;
        if (response?.isSucceed) {
            const _data = response?.data[0];
            const _value = {
                ...formData,
                id: _data.id,
                name: _data.name,
            }
            setFormData(_value);
            if (_data)
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
            id: formData.id,
            name: formData.name
        }
        const res = await InventoryService.saveAttributes(param) as any;
        if (res?.isSucceed) {
            await returnToList();
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
                                />
                            </TableCell>
                            {header.map((field) => (
                                <TableCell key={field.field} align={!field.align ? 'left' : field.align}>{field.fieldName}</TableCell>
                            ))}
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {attributes.length > 0 && attributes.map((item, idx) => (
                            <Row
                                key={`row-${item.id}`}
                                rowData={item}
                                isSelected={selectedItems.includes(item.id)}
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
                    <DialogTitle>Chi tiết</DialogTitle>
                    <DialogContent>
                        <TextField
                            autoFocus
                            required
                            margin="dense"
                            name="name"
                            label="Tên chi tiết"
                            type="text"
                            fullWidth
                            variant="standard"
                            sx={{ marginBottom: 2 }}
                            value={formData.name || ''}
                            onChange={handleChange}
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