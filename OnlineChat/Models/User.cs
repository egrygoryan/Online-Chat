using System.ComponentModel.DataAnnotations;

namespace OnlineChat.Models {
	public class User {
        public int Id { get; set; }
        public string UserName { get; set; }
        public ICollection<ConversationRoom> Rooms { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
