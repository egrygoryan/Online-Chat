using Microsoft.EntityFrameworkCore;
using OnlineChat.Models;

namespace OnlineChat.Context {
	public class ChatContext : DbContext {
		public ChatContext(DbContextOptions<ChatContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<ConversationRoom> ConversationRooms { get; set; }
        public DbSet<Message> Messages { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }
    }
}