var commonHub = new signalR.HubConnectionBuilder().withUrl("/common-hub").build();
commonHub.start().then(function () {
    console.log("admin connected...!");
}).catch(function (err) {
    return console.error(err.toString());
});
commonHub.on("onUpdateUser", function (res) {
    if (res && res.data) {
        const user = res.data
        document.getElementById(`user-status-${user.userId}`).innerHTML = user.isOnline ? htmlOnline : htmlOffline;
        document.getElementById(`user-lastonline-${user.userId}`).innerHTML = user.isOnline === false ? user.lastOnlineLabel : "";
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