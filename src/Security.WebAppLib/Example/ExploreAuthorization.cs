using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Security.WebAppLib.Example
{
    public class ExploreAuthorization
    {
        #region Init

        IAuthorizationService _authorizationService;
        IAuthorizationHandlerProvider _handlerProvider;
        IAuthorizationPolicyProvider _policyProvider;
        IAuthorizationHandlerContextFactory _handlerContextFact;
        IAuthorizationEvaluator _handleEval;
        AuthorizationOptions _authOptions;

        /// <summary>
        /// Services available from DI container
        /// </summary>
        public ExploreAuthorization(
            IAuthorizationService authorizationService,
            IAuthorizationHandlerProvider handlerProvider,
            IAuthorizationPolicyProvider policyProvider,
            IAuthorizationHandlerContextFactory handlerContextFact,
            IAuthorizationEvaluator handleEval,
            IOptions<AuthorizationOptions> authOptions
        )
        {
            this._authorizationService = authorizationService;
            this._handlerProvider = handlerProvider;
            this._policyProvider = policyProvider;
            this._handlerContextFact = handlerContextFact;
            this._handleEval = handleEval;
            this._authOptions = authOptions.Value;
        }

        #endregion
        
        public async Task Run(int testNo = 0)
        {
            int? o = 3;
            var t = o ?? testNo;
            switch (t)
            {
                case 0:
                    await ExploreProvider();
                    break;
                case 1:
                    await AuthorizeUser();
                    break;
                case 2:
                    await AuthorizeResource();
                    break;
                case 3:
                    CheckUserClaim();
                    break;
            }
        }

        public async Task ExploreProvider()
        {
            AuthorizationPolicy p = await _policyProvider.GetDefaultPolicyAsync();
            AuthorizationPolicy? p2 = await _policyProvider.GetFallbackPolicyAsync();
            bool b = _policyProvider.AllowsCachingPolicies;

            p2 = await _policyProvider.GetPolicyAsync("policy1&2");
        }
        
        /// <summary>
        /// Check user claims against a policy
        /// </summary>
        public async Task AuthorizeUser()
        {
            var user = CreateUser();
            AuthorizationResult res = 
                await _authorizationService.AuthorizeAsync(user, "policy1&2");
            bool b = res.Succeeded;
            AuthorizationFailure? failure = res.Failure;
        }

        /// <summary>
        /// Check user claims against a requirement/polcy to
        /// perform an action on a resource
        /// </summary>
        public async Task AuthorizeResource()
        {
            var user = CreateUser();
            var resource = new AuthorizationService.AppData(1);
            AuthorizationResult res = 
                await _authorizationService.AuthorizeAsync(user, resource, "policy5");
            bool b = res.Succeeded;
            AuthorizationFailure? failure = res.Failure;
        }

        /// <summary>
        /// Manage access dynamically
        /// </summary>
        public void CheckUserClaim()
        {
            var user = CreateUser();
            bool b = user.HasClaim(ClaimTypes.Name, "Joe Smith");
            b = user.IsInRole("Admin1");
            Claim? c = user.FindFirst(ClaimTypes.Role);
        }

        public ClaimsPrincipal CreateUser(string? authenticationType = null)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, "Joe Smith"),
                new(ClaimTypes.Role, "Admin1"),
            };
            var identity = new ClaimsIdentity(claims, authenticationType);
            return new ClaimsPrincipal(identity);
        }

    }
}
