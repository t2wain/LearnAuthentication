namespace Security.Identity.IdentityCore
{
    public interface IAppRole { }

    public class AppRole : IAppRole
    {
        public string ID { get; set; } = "";
        public string Name { get; set; } = string.Empty;
    }
}
