using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Security.Identity;
using Security.Identity.Example;
using System.Security.Claims;
using System.Security.Principal;

namespace Security.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            //WinSecurity();
            IHost host = CreateHost();
            CreateEx(host);
            RunEx1(host);
        }

        public static void CreateEx(IHost host)
        {
            var ex1 = host.Services.GetRequiredService<ExploreIdentity>();
            var ex2 = host.Services.GetRequiredService<ExploreAuthentication>();
            var ex3 = host.Services.GetRequiredService<ExploreAuthorization>();
        }

        public static IHost CreateHost()
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder([]);

            AppService appService = new(builder.Services, builder.Configuration);
            appService.AddIdentity();
            appService.AddAuthorization();
            appService.AddOtherServices();

            IHost app = builder.Build();
            return app;
        }

        public static void RunEx1(IHost host)
        {
            var ex = host.Services.GetRequiredService<ExploreAuthentication>();
            ex.Run().Wait();
        }


        #pragma warning disable CA1416
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
        #pragma warning restore CA1416
    }
}
