namespace WebClinic.Data.Models
{
    public class Notification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public required string UserId { get; set; }
        
        public User? User { get; set; }
        
        public required string Message { get; set; }
        
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public required bool IsRead { get; set; } = false;
    }
}
