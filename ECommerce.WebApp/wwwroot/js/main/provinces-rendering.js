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

function hello() {
    alert("hello");
}

function renderAddressSelect(cityCode, districtCode, wardCode) {
    // Get and display provinces data
    $.get('/js/json/provinces-open-api.json', function (data) {
        // City
        var userCity = cityCode;
        // District
        var userDistrict = districtCode;
        // Ward
        var userWard = wardCode;

        var htmlTinhThanh = `<option value="" disabled>Tỉnh/Thành...</option>`
        data.forEach(city => {
            if (city.codename == userCity) {
                htmlTinhThanh += `<option value="` + city.codename + `" selected>` + city.name + `</option>`
                let htmlQuanHuyen = `<option value="" disabled selected>Quận/Huyện...</option>`
                city.districts.forEach(dist => {
                    if (dist.codename == userDistrict) {
                        htmlQuanHuyen += `<option value="` + dist.codename + `" selected>` + dist.name + `</option>`
                        let htmlPhuongXa = `<option value="" disabled selected>Phường/Xã...</option>`
                        dist.wards.forEach(ward => {
                            if (ward.codename == userWard) {
                                htmlPhuongXa += `<option value="` + ward.codename + `" selected>` + ward.name + `</option>`
                            } else {
                                htmlPhuongXa += `<option value="` + ward.codename + `">` + ward.name + `</option>`
                            }
                        })
                        $(".user-ward")[0].innerHTML = htmlPhuongXa;
                    } else {
                        htmlQuanHuyen += `<option value="` + dist.codename + `">` + dist.name + `</option>`
                    }
                })
                $(".user-district")[0].innerHTML = htmlQuanHuyen;
            } else {
                htmlTinhThanh += `<option value="` + city.codename + `">` + city.name + `</option>`
            }
        });
        $(".user-city")[0].innerHTML = htmlTinhThanh;

        // Select Districts
        $(".user-city").on('change', function () {
            let htmlQuanHuyen = `<option value="" disabled selected>Quận/Huyện...</option>`
            data.forEach(city => {
                if (city.codename == this.value) {
                    city.districts.forEach(dist => {
                        htmlQuanHuyen += `<option value="` + dist.codename + `">` + dist.name + `</option>`
                    })
                    $(".user-district")[0].innerHTML = htmlQuanHuyen
                    $(".user-ward")[0].innerHTML = `<option value="" disabled selected>Phường/Xã...</option>`
                }
            })
        });
        // Select Wards
        $(".user-district").on('change', function () {
            let valueTinhThanh = $(".user-city")[0].value
            let valueQuanHuyen = this.value
            let htmlPhuongXa = `<option value="" disabled selected>Phường/Xã...</option>`
            data.forEach(city => {
                if (city.codename == valueTinhThanh) {
                    city.districts.forEach(dist => {
                        if (dist.codename == valueQuanHuyen) {
                            dist.wards.forEach(ward => {
                                htmlPhuongXa += `<option value="` + ward.codename + `">` + ward.name + `</option>`
                            })
                            $(".user-ward")[0].innerHTML = htmlPhuongXa
                        }
                    })
                }
            })
        });
    })
}

function renderAddressString(element) {
    $(element).each(function () {
        let address = $(this).text().split('|')[0];
        let ward = $(this).text().split('|')[1];
        let district = $(this).text().split('|')[2];
        let city = $(this).text().split('|')[3];
        var element = $(this);

        $.get("/js/json/provinces-open-api.json", function (data) {
            // Cities
            data.forEach(c => {
                if (city == c.codename) {
                    c.districts.forEach(d => {
                        if (district == d.codename) {
                            d.wards.forEach(w => {
                                if (ward == w.codename) {
                                    address += ', ' + w.name + ', ' + d.name + ', ' + c.name;
                                    element.text(address);
                                    return;
                                }
                            })
                        }
                    })
                }
            });
        })
    })
}