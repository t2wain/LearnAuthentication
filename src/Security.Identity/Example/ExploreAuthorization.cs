using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Security.Identity.Example
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

        public async Task ExploreAuthoriztion()
        {
            AuthorizationPolicy p = await _policyProvider.GetDefaultPolicyAsync();
            AuthorizationPolicy? p2 = await _policyProvider.GetFallbackPolicyAsync();
            bool b = _policyProvider.AllowsCachingPolicies;
        }
    }
}
