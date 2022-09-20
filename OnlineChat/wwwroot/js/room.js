import { renderMessage } from './message.js';
import { joinRoom } from './chat.js';

var skip = 0; //initial index for pagination after each ajax call add value to skip msgs

export function enterRoom() {
	getUserRooms();
}

function getUserRooms() {
	$.ajax({
		url: "api/rooms/user/" + userName.value,
		method: "GET",
		dataType: "json",
		success: function (result) {
			var items = '';
			for (var i = 0; i < result.length; i++) {
				items += '<li><a href="#">' + result[i].roomName + '</a></li>';
			}
			$("#roomsList").append(items);
			chooseRoomShowMessages();
		}
	});
}

function chooseRoomShowMessages() {
	var rooms = document.querySelectorAll("ul#roomsList li a");
	for (var i = 0; i < rooms.length; i++) {
		$('#currentRoom').text('');

		let roomName = rooms[i].innerText;
		rooms[i].addEventListener('click', function () {
			$('#currentRoom').text(roomName).css('font-weight', 'bold');

			joinRoom($('#joinedUser').text(), roomName);

			document.getElementById('selectRoom').style.display = 'none';
			document.getElementById('messageArea').style.display = 'block';
			document.getElementById('enterMessageContent').style.display = 'block';

			loadDataOnScroll();

			$.ajax({
				url: "api/messages/rooms/" + roomName,
				method: "GET",
				dataType: "json",
				success: function (result) {
					skip += 3;
					if ($("#messagesList")) {
						$("#messagesList").remove()
					}
					$("<ul></ul>", { 'id': "messagesList" }).appendTo('.message-area');
					for (var i = 0; i < result.length; i++) {
						let items = renderMessage(result[i].fromUser, result[i].content, result[i].id, result[i].repliedContent);
						$("#messagesList").append(items);
					}
					$('#divRoom').css('display', 'block');
				}
			});
		});
	}
}

//doesn't work need to be replaced
function loadDataOnScroll() {
	$("#messageArea").scroll(function () {
		//ajax call to get msgs + skip parameter
		myFunc();
	}, false);

	function myFunc() {
		console.log("in myFUNC");
	};
}

