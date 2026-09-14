using System.Security.Claims;
using System.Security.Principal;

namespace Security.ConsoleApp
{
    public static class ClaimSecurity
    {
        #pragma warning disable CA1416
        public static void ExploreWindowsSecurity()
        {
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            ClaimsIdentity claimsIdentity = windowsIdentity;
            IIdentity identity1 = windowsIdentity;

        }
        #pragma warning restore CA1416

        public static void ExploreClaimIdentity(ClaimsIdentity claimsIdentity)
        {

        }
    }
}
