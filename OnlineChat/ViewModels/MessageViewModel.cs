using System.ComponentModel.DataAnnotations;

namespace OnlineChat.ViewModels {
    public class MessageViewModel {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public string FromUser { get; set; }
        [Required]
        public string RoomName { get; set; }
        public string? RepliedContent { get; set; }
    }
}
