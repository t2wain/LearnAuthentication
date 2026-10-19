using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;

namespace Security.WebAppLib.Example
{
    /// <summary>
    /// Examples of how to configure Authorization
    /// </summary>
    public class MyAuthorizationService
    {
        #region Requirements

        /// <summary>
        /// An example of appplication-specific resource
        /// that needs be proctected.
        /// </summary>
        public record AppData(int ID);

        /// <summary>
        /// An example of creating a cusom handler to evaluate
        /// specific requirement.
        /// </summary>
        public class AppDataAuthorizationHandler :
            AuthorizationHandler<OperationAuthorizationRequirement, AppData>
        {
            public const string DeleteOperationName = "Delete";
            public const string AddOperationName = "Add";

            protected override Task HandleRequirementAsync(
                AuthorizationHandlerContext context,
                OperationAuthorizationRequirement requirement,
                AppData resource)
            {
                Explore(context);

                if (context.User == null || resource == null)
                {
                    // indicate this handler is NOT involve
                    // with the evaluation.
                    return Task.CompletedTask;
                }

                // If not asking for delete, return.
                if (requirement.Name != DeleteOperationName)
                {
                    return Task.CompletedTask;
                }

                // Managers can approve or reject.
                if (context.User.IsInRole("Admin1"))
                {
                    // indicated this handler authorize access
                    // to this resource
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

        public static void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            // register custom handler to evaluate authorization base on requirement
            services.AddSingleton<IAuthorizationHandler, AppDataAuthorizationHandler>();
        }

        /// <summary>
        /// Provide examples of registering Authorization components with DI container
        /// </summary>
        public static void ConfigureAuthorizationOptions(AuthorizationOptions options)
        {
            // build authorization policy
            options
                .AddPolicy("policy1&2", (AuthorizationPolicyBuilder configurePolicy) => 
                {
                    var b = BuildPolicy(configurePolicy, 1);
                    b = BuildPolicy(b, 2);
                });

            // build authorization policy
            options.AddPolicy("policy2", CreatePolicy(2));

            // build authorization policy
            options.AddPolicy("policy5", CreatePolicy(5));
        }

        /// <summary>
        /// Provide examples of building AuthorizationPolicy collection
        /// </summary>
        public static AuthorizationPolicyBuilder BuildPolicy(
            AuthorizationPolicyBuilder builder, int policyNo)
        {
            var b = builder;
            switch (policyNo)
            {
                case 1:
                    // evaluation based on Claims
                    b = b.RequireClaim(ClaimTypes.Name, "Joe Smith", "Jane Smith");
                    break;
                case 2:
                    // evaluation based on role
                    b = b.RequireRole("Admin1", "Admin2");
                    break;
                case 3:
                    // convenience method
                    b = b.RequireAuthenticatedUser();
                    break;
                case 4:
                    // convenience method
                    b = b.RequireUserName("joey");
                    break;
                case 5:
                    // evaluation based on requirements
                    b = b.AddRequirements(
                        new OperationAuthorizationRequirement() { Name = "Delete" });
                    break;
            }
            return b;
        }

        /// <summary>
        /// Another API to create policy
        /// </summary>
        public static AuthorizationPolicy CreatePolicy(int policyNo)
        {
            var builder = new AuthorizationPolicyBuilder();
            var b = BuildPolicy(builder, policyNo);
            return b.Build();
        }

        #endregion
    }
}
