var clientHub = new signalR.HubConnectionBuilder().withUrl("/client-hub").build();

// Connect to client server
clientHub.start().then(function () {
    console.log("clients connected!");
    // Call function from server
    // ...
}).catch(function (err) {
    return console.error(err.toString());
});

/*
 * Methods
 */
$(window).focus(function (e) {
    setOnline();
    // updateOnlineHistory();
});
$(window).blur(function (e) {
    setOffline();
    // updateOnlineHistory();
});
function setOffline() {
    clientHub.invoke("SetOffline").catch(function (err) {
        return console.error(err.toString());
    });
}
function setOnline() {
    clientHub.invoke("SetOnline").catch(function (err) {
        return console.error(err.toString());
    });
}
function updateOnlineHistory() {
    clientHub.invoke("UpdateOnlineHistory").catch(function (err) {
        return console.error(err.toString());
    })
}