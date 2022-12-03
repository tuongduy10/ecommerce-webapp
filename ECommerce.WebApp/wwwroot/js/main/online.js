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
document.addEventListener("visibilitychange", function () {
    if (!document.hidden || document.visibilityState === "visible") {
        setOnline();
    } else {
        setOffline();
    }
}, false);
$(window).focus(function (e) {
    setOnline();
});
$(window).blur(function (e) {
    setOffline();
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