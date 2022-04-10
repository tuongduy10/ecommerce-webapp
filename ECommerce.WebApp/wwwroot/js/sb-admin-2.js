(function($) {
  "use strict"; // Start of use strict

  // Toggle the side navigation
  $("#sidebarToggle, #sidebarToggleTop").on('click', function(e) {
    $("body").toggleClass("sidebar-toggled");
    $(".sidebar").toggleClass("toggled");
    if ($(".sidebar").hasClass("toggled")) {
      $('.sidebar .collapse').collapse('hide');
    };
  });

  // Close any open menu accordions when window is resized below 768px
  // $(window).resize(function() {
  //   if ($(window).width() < 768) {
  //     $('.sidebar .collapse').collapse('hide');
  //   };
    
  //   // Toggle the side navigation when window is resized below 480px
  //   if ($(window).width() < 480 && !$(".sidebar").hasClass("toggled")) {
  //     $("body").addClass("sidebar-toggled");
  //     $(".sidebar").addClass("toggled");
  //     $('.sidebar .collapse').collapse('hide');
  //   };
  // });

  // Prevent the content wrapper from scrolling when the fixed side navigation hovered over
  $('body.fixed-nav .sidebar').on('mousewheel DOMMouseScroll wheel', function(e) {
    if ($(window).width() > 768) {
      var e0 = e.originalEvent,
        delta = e0.wheelDelta || -e0.detail;
      this.scrollTop += (delta < 0 ? 1 : -1) * 30;
      e.preventDefault();
    }
  });

  // Scroll to top button appear
  $(document).on('scroll', function() {
    var scrollDistance = $(this).scrollTop();
    if (scrollDistance > 100) {
      $('.scroll-to-top').fadeIn();
    } else {
      $('.scroll-to-top').fadeOut();
    }
  });

  // Smooth scrolling using jQuery easing
  $(document).on('click', 'a.scroll-to-top', function(e) {
    var $anchor = $(this);
    $('html, body').stop().animate({
      scrollTop: ($($anchor.attr('href')).offset().top)
    }, 1000, 'easeInOutExpo');
    e.preventDefault();
  });

})(jQuery); // End of use strict

function loadIamgesWithPreview(upload, preview, amount) {
  if (window.File && window.FileList && window.FileReader) {
      var filesInput = document.getElementById(upload);
      filesInput.addEventListener("change", function (event) {
          var files = event.target.files; //FileList object
          var currentFiles = $('.image__upload-item').length + files.length;
          if (currentFiles > amount) {
            alert("Vượt quá số ảnh cho phép")
            return;
          }
          var output = document.getElementById(preview);
          for (var i = 0; i < files.length; i++) {
            var file = files[i];      
            //Only pics
            if (!file.type.match('image')){
              continue;
            }
            var picReader = new FileReader();
            picReader.addEventListener("load", function (event) {
              var picFile = event.target;
              var div = document.createElement("div");
              div.classList.add(`image__upload-item`, `mr-2`);
              div.innerHTML = `
                <div class="border mb-2 mx-auto" style="height: 180px; width: 180px;">
                    <div class="image-wrapper w-100 h-100 position-relative d-flex align-items-center justify-content-center">
                        <img id="test" class="mw-100 mh-100" src="${picFile.result}" alt="${picFile.name}">
                        <span class="position-absolute remove-upload"
                          style="right: 0; top: 0; cursor: pointer;">
                          <svg xmlns="http://www.w3.org/2000/svg" width="18"
                              height="18" viewBox="0 0 24 24" fill="none"
                              stroke="red" stroke-width="1" stroke-linecap="round"
                              stroke-linejoin="round" class="feather feather-x">
                              <line x1="18" y1="6" x2="6" y2="18"></line>
                              <line x1="6" y1="6" x2="18" y2="18"></line>
                          </svg>
                        </span>
                    </div>
                </div>
              `;
              // Remove event
              var removeUpload = div.querySelector(".remove-upload");
              removeUpload.addEventListener("click", function (evt) {
                  div.remove();
              })
              output.insertBefore(div, null);
            });
            //Read the image
            picReader.readAsDataURL(file);
          }
      });
  }
}
