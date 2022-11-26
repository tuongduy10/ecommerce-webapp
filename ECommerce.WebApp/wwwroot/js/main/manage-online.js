var connection = new signalR.HubConnectionBuilder().withUrl("/onlineHub").build();
connection.start().then(function () {
    console.log("conected!");
}).catch(function (err) {
    return console.error(err.toString());
});
connection.on("ReceiveOnlineUsers", function (res) {
    if (res && res.data) {
        document.getElementById(`user-status-${res.data.userId}`).innerHTML = res.data.isOnline ? htmlOnline : htmlOffline;
        document.getElementById(`user-lastonline-${res.data.userId}`).innerHTML = !res.data.isOnline ? res.data.lastOnlineLabel : "";
    }
});

// html
const htmlOnline = `
    <div class="mr-2" style="height: 18px; width: 18px; background: #31A24C; border-radius: 50%"></div>
    <label class="mb-0" style="color: #31A24C">Đang hoạt động</label>
`;
const htmlOffline = `
    <div class="mr-2" style="height: 18px; width: 18px; background: #a1a3a6; border-radius: 50%"></div>
    <label class="mb-0" style="color: #a1a3a6">Không hoạt động</label>
`;