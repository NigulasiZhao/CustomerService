"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {

}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var connectionId = document.getElementById("connectionId").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", connectionId, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("LoginButton").addEventListener("click", function (event) {
    var UserName = document.getElementById("UserName").value;
    connection.invoke("Login", UserName).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
connection.on("LoginResult", function (list) {
    var json = JSON.parse(list);
    $("#UserList").html(" ");
    for (var i = 0; i < json.length; i++) {
        var html = '<li>用户名:' + json[i].UserName + '<input type="button" connectionId="' + json[i].ConnectionID + '" id="' + json[i].ConnectionID + '" value="聊天"  onclick="userChat(this)" />';
        $("#UserList").append(html);
    }

});
function userChat(obj) {
    var connectionId = $(obj).attr('connectionId');
    $('#connectionId').val(connectionId);
    document.getElementById("sendButton").disabled = false;
}