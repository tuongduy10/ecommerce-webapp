$.get("/js/json/provinces-open-api.json", function (data) {
    // Cities
    var htmlTinhThanh = `<option value="" disabled selected>Tỉnh/Thành...</option>`
    data.forEach(element => {
        htmlTinhThanh += `<option value="` + element.codename + `">` + element.name + `</option>`
    });
    $(".city-select")[0].innerHTML = htmlTinhThanh;
    // Districts
    $(".city-select").on('change', function () {
        var htmlQuanHuyen = `<option value="" disabled selected>Quận/Huyện...</option>`
        data.forEach(element => {
            if (element.codename == this.value) {
                element.districts.forEach(element1 => {
                    htmlQuanHuyen += `<option value="` + element1.codename + `">` + element1.name + `</option>`
                })
                $(".district-select")[0].innerHTML = htmlQuanHuyen
            }
        })
    });
    // Wards
    $(".district-select").on('change', function () {
        var valueTinhThanh = $(".city-select")[0].value
        var valueQuanHuyen = this.value
        var htmlPhuongXa = `<option value="" disabled selected>Phường/Xã...</option>`
        data.forEach(element => {
            if (element.codename == valueTinhThanh) {
                element.districts.forEach(element1 => {
                    if (element1.codename == valueQuanHuyen) {
                        element1.wards.forEach(element2 => {
                            htmlPhuongXa += `<option value="` + element2.codename + `">` + element2.name + `</option>`
                        })
                        $(".ward-select")[0].innerHTML = htmlPhuongXa
                    }
                })
            }
        })
    });
})