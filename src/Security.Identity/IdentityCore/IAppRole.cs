namespace Security.Identity.IdentityCore
{
    public interface IAppRole 
    {
        string ID { get; set; }
        string Name { get; set; }
    }

    public class AppRole : IAppRole
    {
        public string ID { get; set; } = "";
        public string Name { get; set; } = string.Empty;
    }
}
