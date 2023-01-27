var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start()
    .then(function () {
        //
    }).catch(function (err) {
        return console.error(err.toString());
    });

$("#messageInput").on("keypress", function (e) {
    if (e.which === 13) {
        sendMessage();
    }
})
$("#button-emoji").on("click", function () {
    if (!$(".emoji-wrapper").hasClass("d-none")) {
        $(".emoji-wrapper").addClass("d-none");
    } else {
        $(".emoji-wrapper").removeClass("d-none");
    }
    loadEmoji();
});
$(".emoji-list li").click(function () {
    var text = $("#messageInput").val() + $(this).html();
    $("#messageInput").val(text);
    $(".emoji-wrapper").addClass("d-none");
});

$("#button-send").on("click", function () {
    sendMessage();
});

// Receive from server
connection.on("ReceiveFromAdmin", function (userName, message) {
    var userId = document.getElementById("userInputId").value;
    var div = document.createElement("div");
    div.className = "message-wrapper message-left";
    document.getElementById(`message-list-${userId}`).appendChild(div);
    div.innerHTML = htmlMessageLeft(userName, message);
});


function sendMessage() {
    var userName = document.getElementById("userInputName").value
    var message = document.getElementById("messageInput").value;
    var userId = document.getElementById("userInputId").value; 
    var phone = document.getElementById("userInputPhoneNumber").value;

    const params = {
        userName: userName,
        message: message,
        userId: userId,
        phone: phone
    }
    // console.log(params)
    if (message) {
        // Call function from server
        connection.invoke("SendToAdmin",
            userId,
            phone,
            message
        ).then(function () {
            // msg behavior
            addMessageRight(userId, userName, message);
            $("#messageInput").val("");
        })
        .catch(function (err) {
            return console.error(err.toString());
        });
    }
};
function sendMessageToAdmin(message) {
    var request = {
        FromUserId: message.fromUserId,
        Message: message.message,
        UserName: message.userName
    }
    if (request.Message) {
        // Call function from server
        connection.invoke("SendToAdminNoService", request)
            .then(function () {
                addMessageRight(request.FromUserId, request.UserName, request.Message);
            })
            .catch(function (err) {
                return console.error(err.toString());
            });
    }
};

function loadEmoji() {
    if ($(".emoji-list li").length == 0) {
        $.get("/js/json/emoji.json", function (res) {
            if (res && res.length > 0) {
                $(".emoji-list").empty();
                res.forEach(emoji => {
                    let li = document.createElement('li');
                    li.textContent = emoji.character;
                    li.id = emoji.slug;
                    li.addEventListener('click', () => {
                        var text = $("#messageInput").val() + li.textContent;
                        $("#messageInput").val(text);
                        $(".emoji-wrapper").addClass("d-none");
                    })
                    $(".emoji-list").append(li);
                });
            }
        });
    }
}
function addMessageRight(userId, userName, message) {
    var div = document.createElement("div");
    div.className = "message-wrapper message-right";
    div.innerHTML = htmlMessageRight(userName, message);
    $(`#message-list-${userId}`).append(div);
}

// html
function htmlMessageRight(userName, message) {
    const currentDate = new Date();
    const _now = currentDate.getHours() + ":" + currentDate.getMinutes() + ", " + currentDate.getDate() + "/" + (currentDate.getMonth() + 1) + "/" + currentDate.getFullYear()
    return `
        <span class="user-name" style="height: 24px;">
            ${userName}
        </span>
        <div class="text msg-right" title="${_now}">
            ${message}
        </div>
    `;
}
function htmlMessageLeft(userName, message) {
    const currentDate = new Date();
    const _now = currentDate.getHours() + ":" + currentDate.getMinutes() + ", " + currentDate.getDate() + "/" + (currentDate.getMonth() + 1) + "/" + currentDate.getFullYear()
    return `
        <span class="user-name">
            <img src="https://hihichi.com//images/favicon/favicon_f31fb1e8-d495-4b29-8a3d-ebf12fb69910.png" alt="">
            <label class="mb-0" style="margin-top: 0.5px;">${userName}</label>
        </span>
        <div class="text msg-left" title="${_now}">
            ${message}
        </div>
    `;
}