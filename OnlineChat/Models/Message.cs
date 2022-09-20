namespace OnlineChat.Models {
    public class Message {
        public int Id { get; set; }
        public string Content { get; set; }
        public User FromUser { get; set; }
        public ConversationRoom Room { get; set; }
        public int? ReplyToId { get; set; }
    }
}
