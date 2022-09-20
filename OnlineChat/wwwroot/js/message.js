import { privateReplyOnMessage } from './chat.js';

export function renderMessage(fromUser, content, messageId, repliedContent) {
	let li = $('<li></li>');
	let divMessageItem = $('<div></div>', { 'class': "message-item" });
	let divMessageContent = $('<div></div>', { 'class': "message-content" });
	let divMessageInsideContent = $('<div></div>', { 'class': "d-flex flex-wrap align-items-center" });
	let spanAuthor = $('<span></span>', { 'class': "author" }).text(fromUser);
	let divContent = $('<div></div>', { 'class': "content" }).text(content);
	let divActions = $('<div></div>', { 'class': "actions d-none" });
	let divMessageId = $('<input>', { 'type': "hidden" }).val(messageId);
	let divDropdownStart = $('<div></div>', { 'class': "dropdown dropstart" });
	let aText = $('<a></a>', { 'class': "text-secondary", 'role': "button", 'aria-expanded': "false" }).attr('data-bs-toggle', "dropdown");

	divMessageItem.on({
		mouseenter: function () {
			divActions.removeClass('d-none');
		},
		mouseleave: function () {
			divActions.addClass('d-none');
		}
	});

	const svgAttributes = {
		class: "feather feather-more-vertical",
		width: "16", height: "16", viewBox: "0 0 24 24",
		fill: "none", stroke: "currentColor",
		"stroke-width": "2", "stroke-linecap": "round",
		"stroke-linejoin": "round"
	};

	let svg = new svgBuilder(svgAttributes);

	let circle = new circleBuilder({ 'cx': "12", 'cy': "19", 'r': "1" });
	let circle2 = new circleBuilder({ 'cx': "12", 'cy': "12", 'r': "1" });
	let circle3 = new circleBuilder({ 'cx': "12", 'cy': "5", 'r': "1" });

	let ulDropMenu = $('<ul></ul>', { 'class': "dropdown-menu" });
	let liDropMenu = $('<li></li>');
	let aReply = $('<a></a>', { 'class': "dropdown-item reply-to-all", 'href': "#" }).text("Reply").on('click', function () {
		replyToMessage(messageId);
	});
	let aReplyPrivately = $('<a></a>', { 'class': "dropdown-item reply-privately", 'href': "#" }).text("Reply privately").on('click', function () {
		replyToMessagePrivately(messageId)
	});

	svg.append(circle, circle2, circle3);
	aText.append(svg);

	if ($('#joinedUser').text() == fromUser) {
		let aEdit = $('<a></a>', { 'class': "dropdown-item edit", 'href': "#" }).text("Edit").on('click', function () {
			editMessage(messageId);
		});
		let aDelete = $('<a></a>', { 'class': "dropdown-item delete", 'href': "#" }).text("Delete").on('click', function () {
			deleteMessage(messageId);
		});

		liDropMenu.append(aEdit, aReply, aReplyPrivately, aDelete);
	}
	else {
		liDropMenu.append(aReply, aReplyPrivately);
	}


	ulDropMenu.append(liDropMenu);
	divDropdownStart.append(aText, ulDropMenu);
	divActions.append(divDropdownStart);
	divMessageInsideContent.append(spanAuthor);

	if (repliedContent != '') {
		let divReplied = $('<div></div>', { 'class': "replied" }).text(repliedContent);
		divMessageContent.prepend(divReplied);
	}

	divMessageContent.append(divMessageInsideContent, divContent);
	divMessageItem.append(divMessageContent, divActions, divMessageId);
	li.append(divMessageItem);

	return li;
}

$('#sendButton').on('click', sendMessage);

function sendMessage() {

	let json = JSON.stringify({
		"roomName": $('#currentRoom').text(),
		"fromUser": $('#joinedUser').text(),
		"content": $('#messageInput').val()
	});

	$("#messageInput").val('');

	$.ajax({
		headers: {
			'Accept': 'application/json',
			'Content-Type': 'application/json'
		},
		type: "POST",
		url: "api/messages",
		data: json,
		success: function () {
			console.log("success message sent");
		},
		dataType: 'json'
	});
}

function editMessage(messageId) {
	var content = $("div.message-item:has('input[value=" + messageId + "]')").find('.content').text();
	$('#messageInput').val(content);



	$('#sendButton').val('Edit message').off().on('click', function () {
		let json = [{
			"op": "replace",
			"path": "/content",
			"value": $('#messageInput').val()
		}]

		$.ajax({
			type: "PATCH",
			url: "api/messages/" + messageId + "?room=" + $('#currentRoom').text(),
			data: JSON.stringify(json),
			success: function () {
				console.log("success patch message")
				$('#sendButton').val('Send message').off().on('click', sendMessage);
				$('#messageInput').val('');
			},
			contentType: 'application/json-patch+json',
			dataType: 'json'
		});

	});
}

function deleteMessage(messageId) {
	$.ajax({
		headers: {
			'Accept': 'application/json',
			'Content-Type': 'application/json'
		},
		type: "DELETE",
		url: "api/messages/" + $('#currentRoom').text() + "/" + messageId,
		success: function () {
			console.log("success delete message")
		},
		dataType: 'json'
	});
}

function replyToMessage(replyToId = '') {
	$('#sendButton').val('Reply to message').off().on('click', function () {
		let json = JSON.stringify({
			"roomName": $('#currentRoom').text(),
			"fromUser": $('#joinedUser').text(),
			"content": $('#messageInput').val()
		});

		$.ajax({
			headers: {
				'Accept': 'application/json',
				'Content-Type': 'application/json'
			},
			type: "POST",
			url: "api/messages" + "?replyToId=" + replyToId,
			data: json,
			success: function () {
				console.log("success reply to message");
				$('#sendButton').val('Send message').off().on('click', sendMessage);
				$("#messageInput").val('');
			},
			dataType: 'json'
		});
	});
}

function replyToMessagePrivately(replyToId) {
	var content = $("div.message-item:has('input[value=" + replyToId + "]')").find('.content').text();
	$('#sendButton').val('Reply to message privately').off().on('click', function () {
		let json = {
			"roomName": $('#currentRoom').text(),
			"fromUser": $('#joinedUser').text(),
			"content": $('#messageInput').val(),
			"repliedContent": content
		};
		privateReplyOnMessage(json);
		$('#sendButton').val('Send message').off().on('click', sendMessage);
	});


}

export function removeMessageLi(id) {
	$("li:has('input[value=" + id + "]')").remove();
}

export function editMessageContent(id, content) {
	$("div.message-item:has('input[value=" + id + "]')").find('.content').text(content);
}

function circleBuilder(obj) {
	var circleItem = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
	for (const prop in obj) {
		circleItem.setAttribute(prop, obj[prop])
	}
	return circleItem;
}

function svgBuilder(obj) {
	var svgItem = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
	for (const prop in obj) {
		svgItem.setAttribute(prop, obj[prop])
	}
	return svgItem;
}