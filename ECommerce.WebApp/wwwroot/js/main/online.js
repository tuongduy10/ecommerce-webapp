"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/onlineHub").build();
// Connect to server
connection.start().then(function () {
    // Call function from server
    connection.invoke("SendOnlineUsers").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});