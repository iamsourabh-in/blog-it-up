
function LikeBlog(blogId) {
    var url = window.location.protocol + '//' + window.location.host +"/api/Action/LikeBlog";
    var currentUser = getContextUser();
    if (currentUser.UserId) {
        var req = { BlogId: blogId, User: getContextUser() };
        Requestor(url, req);
    }
    else {
        alert("Please Login First");
    }

}

function DisLikeBlog(blogId) {
    var url = window.location.protocol + '//' + window.location.host +"/api/Action/DisLikeBlog";
    var currentUser = getContextUser();
    if (currentUser.UserId) {
        var req = { BlogId: blogId, User: getContextUser() };
        Requestor(url, req);
    }
    else {
        alert("Please Login First");
    }

}

function UnlikeBlog(blogId) {
    var url = window.location.protocol + '//' + window.location.host +"/api/Action/UnLikeBlog";
    var currentUser = getContextUser();
    if (currentUser.UserId) {
        var req = { BlogId: blogId, User: getContextUser() };
        Requestor(url, req);
    }
    else {
        alert("Please Login First");
    }

}




function getContextUser() {
    var username = getCookie("FullName");
    var userId = getCookie("Id");
    var userpic = getCookie("PicPath");


    var User = { UserName: username, UserId: userId, UserPicPath: userpic }

    return User;
}

function Requestor(URL, req) {

    $.ajax({
        url: URL,
        data: JSON.stringify(req),
        dataType: "json",
        type: "POST",
        async: false,
        cache: true,
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {

        },
        success: function (data) {
            window.location.reload();
        }
    });
}
function CommentRequestor(URL, req, callback) {

    $.ajax({
        url: URL,
        data: JSON.stringify(req),
        dataType: "json",
        type: "POST",
        async: false,
        cache: true,
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {

        },
        success: function (data) {
            callback(data);

        }
    });
}
function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}


//// EIDT


function UpdateContent(blogId) {
    var content = $("#edit-blog-div")[0].innerHTML;
    var url = window.location.protocol + '//' + window.location.host +"/api/Action/UpdateContent";
    EditRequestor(url, { "BlogId": blogId, Content: content });
}

function EditRequestor(URL, req) {

    $.ajax({
        url: URL,
        data: JSON.stringify(req),
        dataType: "json",
        type: "POST",
        async: false,
        cache: true,
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {

        },
        success: function (data) {
            window.location.reload();
        }
    });
}