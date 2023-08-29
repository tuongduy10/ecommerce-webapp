import { DataTable } from "src/_shares/_components";
import { ITableData, ITableHeader } from "src/_shares/_components/data-table/data-table";

export default function ProductList() {
    const header: ITableHeader[] = [
        { field: 'name', fieldName: 'Tên sản phẩm', align: 'left' },
        { field: 'category', fieldName: 'Loại sản phẩm', align: 'left' },
        { field: 'image', fieldName: 'Ảnh sản phẩm', align: 'left' },

        { field: 'price', fieldName: 'Giá sản phẩm', align: 'right' },
        { field: 'date', fieldName: 'Ngày', align: 'right' },
    ];
    const data: ITableData[] = [
        { name: 'Tên 1', category: 'Loại 1', image: 'Ảnh 1', price: '1.000 đ', date: '20/10/2010', },
        { name: 'Tên 2', category: 'Loại 2', image: 'Ảnh 2', price: '1.000 đ', date: '20/10/2010', },
        { name: 'Tên 3', category: 'Loại 3', image: 'Ảnh 3', price: '1.000 đ', date: '20/10/2010', },
        { name: 'Tên 4', category: 'Loại 4', image: 'Ảnh 4', price: '1.000 đ', date: '20/10/2010', },
        { name: 'Tên 5', category: 'Loại 5', image: 'Ảnh 5', price: '1.000 đ', date: '20/10/2010', },
        { name: 'Tên 6', category: 'Loại 6', image: 'Ảnh 6', price: '1.000 đ', date: '20/10/2010', },
    ]
    return (
        <DataTable header={header} data={data} />
    );
}