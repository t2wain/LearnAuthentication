namespace Security.Identity.IdentityCore
{
    public interface IMyRole 
    {
        string ID { get; set; }
        string Name { get; set; }
    }

    public class MyRole : IMyRole
    {
        public string ID { get; set; } = "";
        public string Name { get; set; } = string.Empty;
    }
}
