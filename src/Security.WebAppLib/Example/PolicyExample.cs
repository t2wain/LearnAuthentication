using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;

namespace Security.WebAppLib.Example
{
    public class PolicyExample
    {
        #region Requirements

        public record AppData(int ID);

        public class AppDataAuthorizationHandler :
            AuthorizationHandler<OperationAuthorizationRequirement, AppData>
        {
            public const string DeleteOperationName = "delete";
            public const string AddOperationName = "add";

            protected override Task HandleRequirementAsync(
                AuthorizationHandlerContext context,
                OperationAuthorizationRequirement requirement,
                AppData resource)
            {
                Explore(context);

                if (context.User == null || resource == null)
                {
                    return Task.CompletedTask;
                }

                // If not asking for approval/reject, return.
                if (requirement.Name != DeleteOperationName &&
                    requirement.Name != AddOperationName)
                {
                    return Task.CompletedTask;
                }

                // Managers can approve or reject.
                if (context.User.IsInRole("admin"))
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }

        public static void Explore(AuthorizationHandlerContext context)
        {
            IEnumerable<IAuthorizationRequirement> requirements = context.Requirements;
            requirements = context.PendingRequirements;
            object? resource = context.Resource;
            bool b = context.HasSucceeded;
            b = context.HasFailed;
            IEnumerable<AuthorizationFailureReason> reasons =  context.FailureReasons;
        }

        #endregion

        #region Configure Authorization

        public static void ConfigureAuthorization(
            IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IAuthorizationHandler, AppDataAuthorizationHandler>();
            services.AddAuthorization((AuthorizationOptions configure) =>
            {
                configure
                    .AddPolicy("policy1", (AuthorizationPolicyBuilder configurePolicy) => 
                    {
                        var b = BuildPolicy(configurePolicy, 1);
                        b = BuildPolicy(b, 2);
                    });

                configure.AddPolicy("policy2", CreatePolicy(2));

            });
        }

        public static AuthorizationPolicyBuilder BuildPolicy(
            AuthorizationPolicyBuilder builder, int policyNo)
        {
            var b = builder;
            switch (policyNo)
            {
                case 1:
                    b = b.RequireClaim(ClaimTypes.Name, "Joe Smith", "Jane Smith");
                    break;
                case 2:
                    b = b.RequireRole("Admin1", "Admin2");
                    break;
                case 3:
                    b = b.RequireAuthenticatedUser();
                    break;
                case 4:
                    b = b.RequireUserName("joey");
                    break;
                case 5:
                    b = b.AddRequirements(new OperationAuthorizationRequirement() { Name = "Delete" });
                    break;
            }
            return b;
        }

        public static AuthorizationPolicy CreatePolicy(int policyNo)
        {
            var builder = new AuthorizationPolicyBuilder();
            var b = BuildPolicy(builder, policyNo);
            return b.Build();
        }

        #endregion
    }
}
