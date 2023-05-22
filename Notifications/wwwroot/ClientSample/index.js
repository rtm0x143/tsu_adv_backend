"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/api/hubs/notifications", {
        accessTokenFactory: () => document.getElementById("accessTokenArea").value
    })
    .configureLogging(signalR.LogLevel.Debug)
    .build();

var notifications = document.getElementById("notifications");

connection.on("ReceiveNotification", function (notification) {
    console.log("Received notification : ", notification)
    notifications.innerHTML += `<div>${JSON.stringify(notification)}</div>`

});

connection.start().then(function () {
    console.log("Connection done")

    connection.invoke("SubscribeToOrderTopic", "10")
        .then(() => console.log("Subscribed"))

    document.getElementById("subscribeToOrderEvents").onclick = () => {
        let orderNumber = document.getElementById("orderNameInput").value
        connection.invoke("SubscribeToOrderTopic", orderNumber)
            .then(() => {
                console.log("Subscribed to order with number " + orderNumber)
                document.getElementById("alreadySubscribed").innerHTML += `<span> ${orderNumber} </span>`
            })
    }

}).catch(function (err) {
    return console.error(err.toString());
});