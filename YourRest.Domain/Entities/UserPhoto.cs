namespace YourRest.Domain.Entities
{
    public class UserPhoto : IntBaseEntity
    {
        public string FilePath { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
