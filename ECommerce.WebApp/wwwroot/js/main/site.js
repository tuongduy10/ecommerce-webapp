// Write your Javascript code.
function PostApi(url, params) {
    return $.post(url, params)
        .fail(function (xhr, status, error) {
           switch (xhr.status) {
               case 401:
                   if (xhr.getAllResponseHeaders().includes('Manage')) {
                       window.location.href = "/Admin/SignIn";
                   } else {
                       var url = "/Account/SignIn";
                       if (window.location.pathname) {
                           url = url + "?CurrentUrl=" + window.location.pathname;
                           if (window.location.search) {
                               url = url + window.location.search
                           }
                       }
                       window.location.href = url;
                   }
                   break;
               case 400:
                   console.log("error---", xhr);
                   if (xhr.responseText) {
                       alert(xhr.responseText);
                   }
                   break;
               case 404:
                   console.log("error---", xhr);
                   if (xhr.responseText) {
                       alert(xhr.responseText);
                   }
                   break;
               case 500:
                   alert('Lỗi');
                   break;
           }
        })
}

function AjaxPost(_url, _params, errorMessage = true) {
    return $.ajax({
        url: _url,
        data: JSON.stringify(_params),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        type: 'POST',
    }).fail(function (error) {
        switch (error.status) {
           case 401:
                if (error.getAllResponseHeaders().includes("Manage")) {
                    window.location.href = "/Admin/SignIn";
                } else {
                    var url = "/Account/SignIn";
                    if (window.location.pathname) {
                        url = url + "?CurrentUrl=" + window.location.pathname;
                        if (window.location.search) {
                            url = url + window.location.search
                        }
                    }
                    window.location.href = url;
                }
               break;
           case 400:
                console.log('Ajax Post error--- ', error);
                if (errorMessage && error.responseText) {
                    alert(error.responseText);
                }
               break;
           case 500:
               alert('Lỗi');
               break;
        }
    });
}

function AjaxPostForm(_url, _formData, errorMessage = true) {
    return $.ajax({
        type: "POST",
        url: _url,
        data: _formData,
        processData: false,
        contentType: false,
    }).fail(function (error) {
        switch (error.status) {
           case 401:
                if (error.getAllResponseHeaders().includes("Manage")) {
                    window.location.href = "/Admin/SignIn";
                } else {
                    var url = "/Account/SignIn";
                    if (window.location.pathname) {
                        url = url + "?CurrentUrl=" + window.location.pathname;
                        if (window.location.search) {
                            url = url + window.location.search
                        }
                    }
                    window.location.href = url;
                }
                break;
           case 400:
                console.log('Ajax Post Form error--- ', error);
                if (errorMessage && error.responseText) {
                    alert(error.responseText);
                }
                break;
           case 404:
                console.log("error---", xhr);
                if (xhr.responseText) {
                    alert(xhr.responseText);
                }
                break;
           case 500:
                alert('Lỗi');
                break;
        }
    });
}

