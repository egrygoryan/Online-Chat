import { enterRoom } from './room.js';

export function setUsername() {
	let userNameInput = $('#userName').val();
	
	$(document).off('keypress');
	$('#userInfo').css('display', 'none');
	$('#appArea').css('display', 'block');
	$('#joinedUser').text(userNameInput);
	enterRoom();
}