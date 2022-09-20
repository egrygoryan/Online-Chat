import { renderMessage, removeMessageLi, editMessageContent } from './message.js';
import { setUsername } from './provideName.js';

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start().then(function () {
	sendButton.prop('disabled', false);
}).catch(function (err) {
	return console.error(err.toString());
});

export function joinRoom(login, roomName) {
	connection.invoke("Join", login, roomName).catch(function (err) {
		return console.error(err.toString() + "JOIN METHOD")
	});
}

connection.on("newMessage", function (message) {
	var items = renderMessage(message.fromUser, message.content, message.id, message.repliedContent);
	$('#messagesList').append(items);
});

connection.on("deleteMessage", function (id) {
	removeMessageLi(id);
	console.log("message deleted succesfully")
});

connection.on("editMessage", function (message) {
	editMessageContent(message.id, message.content);
	console.log("message edited succesfully")
});

var sendButton = $("#sendButton").prop('disabled', true);
var userName = $("#userName");

$("#joinButton").on('click', login);
$(document).on('keypress', function (e) {
	if (e.which == 13) {
		login();
	}
});

export function privateReplyOnMessage(message) {
	connection.invoke("PrivateReply", message).catch(function (err) {
		return console.error(err.toString() + "Private Reply METHOD");
	});
	$("#messageInput").val('');
	console.log("message replied privately");
}

connection.on("receivePrivateMessage", function (message) {
	var items = renderMessage(message.fromUser, message.content, message.id, message.repliedContent);
	$('#messagesList').append(items);
	console.log("received private message");
});

function login() {
	if (userName.val()) {
		connection.invoke("Login", userName.val()).catch(function (err) {
			return console.error(err.toString() + "LOGIN METHOD");
		});
		setUsername();
	}
	else {
		alert('Provide your User Name');
	}
}





