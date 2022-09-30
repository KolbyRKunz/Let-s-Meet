// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function submit() {
    let url = "Auth/Login"
    let data = {
        "Username": $("#Username").val(),
        "Password": $("#Password").val()
    }

    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            location.reload()
        }
    })
}