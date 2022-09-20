using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineChat.Context;
using OnlineChat.ViewModels;

namespace OnlineChat.Hubs {
    public class ChatHub : Hub {
        private readonly ChatContext _chatContext;
        private readonly IMapper _mapper;
        private static Dictionary<string, string> _usersConnections = new Dictionary<string, string>();
        private static Dictionary<string, string> _usersInRooms= new Dictionary<string, string>();
        public ChatHub(ChatContext chatContext, IMapper mapper) {
            _chatContext = chatContext;
            _mapper = mapper;
        }
        public async Task PrivateReply(MessageViewModel mvm) {
            var receiverName = _chatContext.Messages.Where(m => m.Content == mvm.RepliedContent).Select(s => s.FromUser.UserName).SingleOrDefault();

            if (receiverName == null) {
                throw new BadHttpRequestException($"There is no receiver in chat");
            }

            if (_usersConnections.TryGetValue(receiverName, out string receiverId)) {
                await Clients.Client(receiverId).SendAsync("receivePrivateMessage", mvm);
                await Clients.Caller.SendAsync("receivePrivateMessage", mvm);
                return;
            }

            throw new BadHttpRequestException($"{receiverName} is not online");
        }

        public void Login(string login) {
            if (!_usersConnections.ContainsKey(login)) {
                _usersConnections.Add(login, Context.ConnectionId);
            } else {
                _usersConnections[login] = Context.ConnectionId;
            }
        }
        public async Task Join(string login, string roomName) {
            if (_usersInRooms.ContainsKey(login)) {
                await Leave(login, _usersInRooms[login]);
                _usersInRooms[login] = roomName;
            } else {
                _usersInRooms.Add(login, roomName);
            }

            await Groups.AddToGroupAsync(_usersConnections[login], roomName);
        }

        public async Task Leave(string login, string roomName) {
            await Groups.RemoveFromGroupAsync(_usersConnections[login], roomName);
        }

        public override Task OnConnectedAsync() {
            Console.WriteLine("Client connected to Hub");
            return base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception) {
            Console.WriteLine("Client disconneted from Hub");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
