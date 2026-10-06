namespace Security.Identity.IdentityCore
{
    public interface IMyUser {   }

    public class MyUser : IMyUser
    {
        public string ID { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string? SecurityStamp { get; set; } = default!;
    }
}
