const SearchForm = (props: any) => {
    return (
        <div className={`searchform__wrapper py-2 px-[20px]`}>
            <div className="searchform input-group rounded w-full">
                <input type="search" id="search-input" className="form-control w-full"
                    placeholder="Thương hiệu, dịch vụ, sản phẩm cần tìm" aria-label="Search"
                    aria-describedby="search-addon" />
            </div>
            <div className="searchresult hidden">
                <p>
                    <a href="/">Thế giới di động (Điện thoại di động)</a>
                </p>
                <p>
                    <a href="/">FPT Shop (Điện thoại di động)</a>
                </p>
                <p>
                    <a href="/">Samsung Galaxy S20 Ultra</a>
                </p>
                <p>
                    <a href="/">Geforce RTX 3080TI Asus</a>
                </p>
                <div className="text-center w-100">
                    <a href="/" className="search-viewmore">Xem thêm</a>
                </div>
            </div>
        </div>
    )
}

export default SearchForm;