$(document).ready(function(){
    $('.nav__list-mobile-wrapper').css('height', $('.nav__mobile-list').outerHeight() - $('.nav__mobile-logo').outerHeight() - $('.nav__mobile-footer').outerHeight())
    $(window).scroll(function(){
        $('.header').addClass('sticky-header')
        $('.main').css('margin-top', $('.header').height())
    })
    $(window).resize(function(){
        $('.header').addClass('sticky-header')
        $('.main').css('margin-top', $('.header').height())
        $('.nav__list-mobile-wrapper').css('height', $('.nav__mobile-list').outerHeight() - $('.nav__mobile-logo').outerHeight() - $('.nav__mobile-footer').outerHeight())
    })
})

document.getElementById('nav__menubar-open').onclick = function () {
    openNavMobileList()
    closeSearchForm()
    $('body').css('overflow', 'hidden')
    // $('html').css('overflow', 'hidden')
}
document.getElementById('nav__overlay').onclick = function () {
    closeNavMobileList()
    $('body').css('overflow', 'auto')
    // $('html').css('overflow','auto')
    $('.product__filter-dropdown-menu').removeClass('filter-open')
}
document.getElementById('nav__mobile-list-close').onclick = function () {
    closeNavMobileList()
    $('body').css('overflow', 'auto')
}

$(".header__mobile-searchicon").click(function () {
    if ($('.searchform__wrapper').hasClass('d-none')) {
        $('.searchform__wrapper').removeClass('d-none')
        $('.searchform__wrapper input').focus()
        $('.search__overlay').removeClass('d-none')
        $('.header__mobile-searchicon svg').addClass('icon-active')
        $('body').css('overflow', 'hidden')
    } else {
        closeSearchForm()
    }
    return false
})
$('.search__overlay').click(function () {
    closeSearchForm()
})
$('.cart-action').hover(
    function(){
        $(this).find('.minicart-content').addClass('open')
    },
    function(){
        $(this).find('.minicart-content').removeClass('open')
    }
)
$('.minicart-remove').click(function(){
    $(this).parent().remove();
})

function closeSearchForm() {
    $('.searchform__wrapper').addClass('d-none')
    $('.searchform__wrapper input').focusout()
    $('.search__overlay').addClass('d-none')
    $('body').css('overflow', 'auto')
    $('.header__mobile-searchicon svg').removeClass('icon-active')
}
function closeNavMobileList() {
    document.getElementById('nav__overlay').className = document.getElementById('nav__overlay').className.replace('d-block', 'd-none')
    document.getElementById('nav__mobile-list-open').className = document.getElementById('nav__mobile-list-open').className.replace('nav-open', 'nav-close')
}
function openNavMobileList() {
    document.getElementById('nav__overlay').className = document.getElementById('nav__overlay').className.replace('d-none', 'd-block')
    document.getElementById('nav__mobile-list-open').className = document.getElementById('nav__mobile-list-open').className.replace('nav-close', 'nav-open')
}

$(".order__button").hover(
    function () {
        $(this).find(".order__item-dropdown_menu").addClass("open");
        $(this).find(".svg-order").addClass("svg-up");
    }, function () {
        $(this).find(".order__item-dropdown_menu").removeClass("open");
        $(this).find(".svg-order").removeClass("svg-up");
    }
);
$(".order__item-wrapper").click(function () {
    if (!$(this).find(".order__item-dropdown_menu").hasClass('open')) {
        $(this).find(".order__item-dropdown_menu").addClass("open");
        $(this).find(".svg-order").addClass("svg-up");
    } else {
        $(this).find(".order__item-dropdown_menu").removeClass("open");
        $(this).find(".svg-order").removeClass("svg-up");
    }
})

$(".order__item").click(function () {
    let text = $(this).text()
    $(".order__item").removeClass('d-none')
    $(this).parent().parent().siblings()
        .html(text +
            `<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24"
                    viewBox="0 0 24 24" fill="none" stroke="#333" stroke-width="1"
                    stroke-linecap="round" stroke-linejoin="round"
                    class="feather feather-chevron-down svg-order">
                    <polyline points="6 9 12 15 18 9"></polyline>
                </svg>`)
    $(this).addClass('d-none')
    // $(".order__item-dropdown_menu").removeClass('open')
})

$(".filter__title").click(function (e) {
    $('.filter__list').css('max-height','0')  
    $('.filter__title').children('svg').removeClass('svg-right')
    if($(this).siblings('.filter__list').height() == 0){
        $(this).siblings('.filter__list').css('max-height','calc(185px + (310 - 185) * ((100vw - 375px)/ (1920 - 375)))')  
        $(this).children('svg').addClass('svg-right')
    
    }
})
$('.filter__item-title').click(function(){
    $('.filter__items-mobile').css('max-height','0')
    $('.filter__item-title').children('svg').removeClass('svg-right')
    if($(this).siblings('.filter__items-mobile').height() == 0){
        $(this).siblings('.filter__items-mobile').css('max-height','calc(185px + (310 - 185) * ((100vw - 375px)/ (1920 - 375)))')
        $(this).children('svg').addClass('svg-right')
    }
})
$('.filter__button').click(function(){
    // open nav
    $('#nav__overlay').removeClass('d-none')
    $('#nav__overlay').addClass('d-block')
    // 
    $('.product__filter-dropdown-menu').addClass('filter-open')
    $('body').css('overflow','hidden')
})
$('.filter-close').click(function(){
    $('#nav__overlay').removeClass('d-block')
    $('#nav__overlay').addClass('d-none')
    $('body').css('overflow', 'auto')
    $('.product__filter-dropdown-menu').removeClass('filter-open')
})
$('.add-to-wishlist').hover(
    function(){
        $(this).attr('fill','true')
    },
    function(){
        $(this).attr('fill','none')
    }
)
$('.add-to-wishlist').click(function(){
    if($(this).hasClass('fill')) {
        $(this).removeClass('fill')
        return
    }
    $(this).addClass('fill')
})
// $('.options-wrapper select').on('click',function(){
//     if(!$(this).siblings('svg').hasClass('svg-up')){
//         $(this).siblings('svg').addClass('svg-up') 
//         return
//     }
//     $(this).siblings('svg').removeClass('svg-up')
// })
// $('.options-wrapper select').on('focusout',function(){
//     if($(this).siblings('svg').hasClass('svg-up')){
//         $(this).siblings('svg').removeClass('svg-up') 
//     }
// })

$('.product__sale-code').click(function(){
    if($('.product__sale-bottom').hasClass('d-none')){
        $('.product__sale-bottom').removeClass('d-none');
    }else{
        $('.product__sale-bottom').addClass('d-none')
    }

    if($(this).find('svg').hasClass('svg-up')){
        $(this).find('svg').removeClass('svg-up')
    }else{
        $(this).find('svg').addClass('svg-up')
    }
})

$(document).ready(function(){
    $('.getcode').on('click', function(){
        var $temp = $("<input>");
        $("body").append($temp);
        $temp.val($('.salecode').text()).select();
        document.execCommand("copy");
        $temp.remove();
        $('.getcode').text('Đã lấy mã')
    })
    $('.control-tab').click(function () {
        if ($('.control-tab').hasClass('active')) {
            $('.control-tab').removeClass('active');
        }
        $(this).addClass('active')
    })
    $('.product-rating').click(function(){
        var node = document.querySelector(".product__detail-tab");
        var yourHeight = $('header').height();
        // scroll to your element
        node.scrollIntoView(true);
        var scrolledY = window.scrollY;
        if(scrolledY){
            window.scrollTo({top: scrolledY - yourHeight});
        }
        // Tab-control
        if ($('.control-tab').hasClass('active')) {
            $('.control-tab').removeClass('active');
        }
        $('.control__tab-comment').addClass('active');

        // Tab-content
        $('.content-tab').removeClass('active');
        if(!$('.tab__product-comment').hasClass('active')){
            $('.tab__product-comment').addClass('active');
            return;
        }
    
        $('.tab__product-comment').removeClass('active');
    })
    $('.control__tab-info').click(function(){
        $('.content-tab').removeClass('active');
        if(!$('.tab__product-info').hasClass('active')){
            $('.tab__product-info').addClass('active');
            return;
        }
    
        $('.tab__product-info').removeClass('active');
    })
    $('.control__tab-comment').click(function(){
        $('.content-tab').removeClass('active');
        if(!$('.tab__product-comment').hasClass('active')){
            $('.tab__product-comment').addClass('active');
            return;
        }
    
        $('.tab__product-comment').removeClass('active');
    })
    $('.copy-stk').on('click', function(){
        var $temp = $("<input>");
        $("body").append($temp);
        $temp.val($(this).siblings().children('.stk').text()).select();
        document.execCommand("copy");
        $temp.remove();
        $(this).children('.--tooltip-text').text('Đã sao chép');
    })
})
$('.cart-product__amount').on('change', function(){
    if(this.value == 0){
        $(this).parent().parent().parent().remove();
    }
    let totalvalue = this.value * $(this).parent().siblings('.cart-product--auto').children('.price').children('.value').text();
    $(this).parent().siblings('.totalprice').children('.total-value').text(totalvalue);
})
$('.cart-product__remove').click(function(){
    $(this).parent().remove();
})


$('.rating .stars span').click(function(){
    if(!$(this).hasClass('rated')){
        $('.rating .stars span').removeClass('checked');
        $('.rating .stars span').removeClass('rated');

        $(this).addClass('rated');
        $('.rating .stars span').each(function(index, element){
            $(element).addClass('checked')
            if($(this).hasClass('rated')){
                return false;
            }
        })        
    }else{
        $(this).removeClass('rated');
        $('.rating .stars span').each(function(index, element){
            $(element).removeClass('checked');
        })
    }
})

$('.write-comment').click(function(){
    if($('.tab__content-block').hasClass('d-none')){
        $('.tab__content-block').removeClass('d-none');
        return;
    }
    $('.tab__content-block').addClass('d-none');
})

$('.payment-method').change(function(){
    if($('#bank').is(':checked')){
        if($('.payment-expand').hasClass('d-none')){
            $('.payment-expand').removeClass('d-none');
        }
        return;
    }
    $('.payment-expand').addClass('d-none');
})


$.get("assets/js/json/provinces-open-api.json", function (data) {
    // Cities
    var htmlTinhThanh = `<option value="" disabled selected>Tỉnh/Thành...</option>`
    data.forEach(element => {
        htmlTinhThanh += `<option value="` + element.code + `">` + element.name + `</option>`
    });
    $(".city-select")[0].innerHTML = htmlTinhThanh;
    // Districts
    $(".city-select").on('change', function () {
        var htmlQuanHuyen = `<option value="" disabled selected>Quận/Huyện...</option>`
        data.forEach(element => {
            if (element.code == this.value) {
                element.districts.forEach(element1 => {
                    htmlQuanHuyen += `<option value="` + element1.code + `">` + element1.name + `</option>`
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
            if (element.code == valueTinhThanh) {
                element.districts.forEach(element1 => {
                    if (element1.code == valueQuanHuyen) {
                        element1.wards.forEach(element2 => {
                            htmlPhuongXa += `<option value="` + element2.code + `">` + element2.name + `</option>`
                        })
                        $(".ward-select")[0].innerHTML = htmlPhuongXa
                    }
                })
            }
        })
    });
})