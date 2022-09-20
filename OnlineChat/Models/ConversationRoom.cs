using System.ComponentModel.DataAnnotations;

namespace OnlineChat.Models {
    public class ConversationRoom {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}