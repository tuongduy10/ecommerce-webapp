"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/onlineHub").build();
// Connect to server
connection.start()
    .then(function () {
    
    })
    .catch(function (err) {
        return console.error(err.toString());
    });