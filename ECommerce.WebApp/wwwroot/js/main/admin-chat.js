var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start()
    .then(function () {
        //
    }).catch(function (err) {
        return console.error(err.toString());
    });

$(document).on("keypress", ".write_msg", function (e) {
    if (e.which === 13) {
        sendMessage();
    }
});
$(document).on("click", ".msg_send_btn", function (event) {
    event.preventDefault();
    sendMessage();
})

connection.on("ReceiveMessage", function (userId, message) {
    var div = document.createElement("div");
    document.getElementById(`msg_wrapper_${userId}`).appendChild(div);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    div.innerHTML = htmlInComing_msg(message);
});

function sendMessage() {
    var fromUserId = $(".fromUserId").val();
    var toUserId = $(".toUserId").val();
    var userName = $(".userInputName").val();
    var message = $(".write_msg").val();

    if (message && userName) {
        // Call function from server
        connection.invoke("SendToClient", fromUserId, toUserId, userName, message).catch(function (err) {
            return console.error(err.toString());
        });

        // msg behavior
        var div = document.createElement("div");
        document.getElementById(`msg_wrapper_${toUserId}`).appendChild(div);
        div.innerHTML = htmlOutGoing_msg(message);
        $(".write_msg").val("");
    }
}

function htmlInComing_msg(msg) {
    const currentDate = new Date();
    const _now = currentDate.getHours() + ":" + currentDate.getMinutes() + ", " + currentDate.getDate() + "/" + (currentDate.getMonth() + 1) + "/" + currentDate.getFullYear()
    return `
        <div class="incoming_msg">
            <div class="received_msg">
                <div class="received_withd_msg">
                    <p>
                        ${msg}
                    </p>
                    <span class="time_date"> ${_now} </span>
                </div>
            </div>
        </div>
    `;
}
function htmlOutGoing_msg(msg) {
    const currentDate = new Date();
    const _now = currentDate.getHours() + ":" + currentDate.getMinutes() + ", " + currentDate.getDate() + "/" + (currentDate.getMonth() + 1) + "/" + currentDate.getFullYear()
    return `
        <div class="outgoing_msg">
            <div class="sent_msg">
                <p>${msg}</p>
                <span class="time_date"> ${_now} </span>
            </div>
        </div>
    `;
}

