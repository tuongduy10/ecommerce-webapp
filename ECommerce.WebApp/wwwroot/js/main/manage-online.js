var connection = new signalR.HubConnectionBuilder().withUrl("/onlineHub").build();
connection.start().then(function () {

}).catch(function (err) {
    return console.error(err.toString());
});
connection.on("ReceiveOnlineUsers", function (res) {
    if (res && res.data) {
        console.log(res.data);
        $(`.user-status`).html(htmlOffline);
        res.data.forEach(user => {
            document.getElementById(`user-status-${user.userId}`).innerHTML = htmlOnline;
        });
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