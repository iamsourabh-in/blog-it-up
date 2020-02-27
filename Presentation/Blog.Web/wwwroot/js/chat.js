"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/blogHub").build();


connection.on("ReceiveMessage", function (message) {
    //document.getElementById("editor").value = message;
    CKEDITOR.instances.editor.setData(message);
});

connection.start().then(function () {
    // document.getElementById("sendButton").disabled = false;
    console.log("asdasd");
}).catch(function (err) {
    return console.error(err.toString());
});

