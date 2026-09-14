namespace Security.Identity.IdentityCore
{
    public interface IAppUser {   }

    public class AppUser : IAppUser
    {
        public string ID { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Name { get; set; } = "";
        public string? SecurityStamp { get; set; } = default!;
    }
}
`