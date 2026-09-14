using System.Security.Claims;
using System.Security.Principal;

namespace Security.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            WinSecurity();
        }

        public static void WinSecurity()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            IIdentity identity1 = identity;

            Console.WriteLine($"User: {identity.Name}");
            Console.WriteLine($"IsAuthenticated: {identity.IsAuthenticated}");
            foreach (var claim in identity.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
            }

            ClaimsPrincipal? principal = ClaimsPrincipal.Current;
            principal = WindowsPrincipal.Current;
            IPrincipal? principal1 = principal;
        }
    }
}
