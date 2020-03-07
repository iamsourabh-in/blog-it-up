"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/blogHub").build();

//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;

//connection.on("ReceiveMessage", function (user, message) {
//    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
//    var encodedMsg = user + " says " + msg;
//    var li = document.createElement("li");
//    li.textContent = encodedMsg;
//    document.getElementById("messagesList").appendChild(li);
//});

connection.start().then(function () {
    // document.getElementById("sendButton").disabled = false;
    alert("connected");
}).catch(function (err) {
    alert(err.toString());
});

document.getElementById("userInput").addEventListener("keyup", function (event) {
    var message = $("#userInput").html();
    connection.invoke("SendMessage", message).catch(function (err) {
        alert(err.toString());
    });
    event.preventDefault();
});