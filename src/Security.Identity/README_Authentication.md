# Nuget Packages

- Microsoft.AspNetCore.Identity
	- Microsoft.Extensions.Identity.Core
	- Microsoft.AspNetCore.Authentication.Cookies
		- Microsoft.AspNetCore.Authentication
			- Microsoft.AspNetCore.Authentication.Core
				- Microsoft.AspNetCore.Authentication.Abstractions

# Assembly : Microsoft.AspNetCore.Authentication.Cookies

## Microsoft.Extensions.DependencyInjection

Specify to store, encrypte, decrypt, and parse data for an authenticted user from an identity cookie. Identity cookie is set by calling **HttpContext.SignInAsync(...)**. Calling **HttpConntext.SignOutAsync(...)** removes the cookie. It also parses the cookie to rebuild the PrincipalClaim object for each subsequent request.

You can setup multipe identity cookies to store data from various identity providers. A separate **authentication scheme** label is used to reference each identity provider such as Google and Microsoft.

To use external identity provider for sign-in but switching to a different application specific authentication scheme, then folow these steps:

1. Specify the **return URL** parameter (AuthenticationProperties) for an external identity provider that provides the logic to reconstruct the PrincialClaim
1. Access the identity data from each identity cookie with **HttpContext.Authenticated(...)**.
1. Construct a new application specific PrincipalClaim
1. Use **HttpConntext.SignOutAsync(...)** to remove the identity cookie from external provider
1. Use **HttpContext.SignInAsync(...)** to issue application specific identity cookie.
1. Be sure the **AddAuthentication()** is configured for the same application specific authentication scheme. 

```csharp
AuthenticationBuilder authBuilder = services
	// specify which authentication scheme to use
	// when multiple schemes are configured
    .AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie(
		// indicate this cookie came from local identity provider
		IdentityConstants.ApplicationScheme,
		(CookieAuthenticationOptions configureOptions) => { }
	);
    .AddCookie(
		// indicate this cookie came from external identity provider
		IdentityConstants.ExternalScheme,
		(CookieAuthenticationOptions configureOptions) => { }
	);
```

- CookieExtensions (**AuthenticationBuilder**)
	- **AddCookie()**
	- AddCookie(string authenticationScheme)
	- **AddCookie()**
		- string **authenticationScheme**, 
		- string displayName, 
		- Action\<**CookieAuthenticationOptions**> configureOptions
	- AddCookie()
		- string authenticationScheme, 
		- Action\<CookieAuthenticationOptions> configureOptions
	- AddCookie()
		- Action\<CookieAuthenticationOptions> configureOptions

## Microsoft.AspNetCore.Authentication.Cookies

- ChunkingCookieManager
- CookieAuthenticationDefaults (static)
	- AccessDeniedPath
	- CookiePrefix
	- LoginPath
	- LogoutPath
	- ReturnUrlParameter
	- **AuthenticationScheme**

Configure callback event to analyze the data stored in the identity cookie and potential reject the cookie.

- CookieAuthenticationEvents
	- **CookieAuthenticationEvents()**
	- RedirectToAccessDenied(RedirectContext\<CookieAuthenticationOptions> context)
	- RedirectToLogin(RedirectContext\<CookieAuthenticationOptions> context)
	- RedirectToLogout(RedirectContext\<CookieAuthenticationOptions> context)
	- RedirectToReturnUrl(RedirectContext\<CookieAuthenticationOptions> context)
	- SignedIn(CookieSignedInContext context)
	- SigningIn(CookieSigningInContext context)
	- SigningOut(CookieSigningOutContext context)
	- ValidatePrincipal(CookieValidatePrincipalContext context)
	- OnRedirectToAccessDenied : Func<RedirectContext\<CookieAuthenticationOptions>, Task>
	- OnRedirectToLogin : Func<RedirectContext\<CookieAuthenticationOptions>, Task>
	- OnRedirectToLogout : Func<RedirectContext\<CookieAuthenticationOptions>, Task>
	- OnRedirectToReturnUrl : Func<RedirectContext\<CookieAuthenticationOptions>, Task>
	- **OnSignedIn** : Func<CookieSignedInContext, Task>
	- **OnSigningIn** : Func<CookieSigningInContext, Task>
	- **OnSigningOut** : Func<CookieSigningOutContext, Task>
	- **OnValidatePrincipal** : Func<CookieValidatePrincipalContext, Task>
- CookieAuthenticationHandler : SignInAuthenticationHandler\<CookieAuthenticationOptions>
- **CookieAuthenticationOptions** : **AuthenticationSchemeOptions**
	- AccessDeniedPath 
	- Cookie : **CookieBuilder**
	- **CookieDomain** 
	- CookieHttpOnly 
	- **CookieManager** : ICookieManager 
	- CookieName 
	- **CookiePath** 
	- CookieSecure : CookieSecurePolicy
	- DataProtectionProvider : IDataProtectionProvider
	- Events : **CookieAuthenticationEvents**
	- ExpireTimeSpan : TimeSpan
	- **LoginPath** 
	- **LogoutPath** 
	- **ReturnUrlParameter** 
	- SessionStore : ITicketStore
	- **SlidingExpiration** : bool
	- TicketDataFormat : ISecureDataFormat\<**AuthenticationTicket**>
- PrincipalContext\<CookieAuthenticationOptions> : PropertiesContext\<CookieAuthenticationOptions>
	- Principal : **ClaimsPrincipal**
- PropertiesContext\<CookieAuthenticationOptions> : BaseContext\<CookieAuthenticationOptions>
	- Properties : **AuthenticationProperties**
- BaseContext\<CookieAuthenticationOptions>
	- HttpContext : **HttpContext**
	- Options : CookieAuthenticationOptions
	- Request : **HttpRequest**
	- Response : **HttpResponse**
	- Scheme : **AuthenticationScheme**
- CookieSignedInContext : PrincipalContext\<CookieAuthenticationOptions>
- CookieSigningInContext : PrincipalContext\<CookieAuthenticationOptions>
	- CookieOptions : CookieOptions
- CookieSigningOutContext : PropertiesContext\<CookieAuthenticationOptions>
	- CookieOptions : CookieOptions
- CookieValidatePrincipalContext : PrincipalContext\<CookieAuthenticationOptions>
	- ShouldRenew
	- RejectPrincipal()
- PostConfigureCookieAuthenticationOptions
	- PostConfigure(string name, CookieAuthenticationOptions options)

# Assembly : Microsoft.AspNetCore.Authentication

## Microsoft.Extensions.DependencyInjection

**AddAuthentication** is responsible build and populate the HttpContext.User. The identity data can come from various identity providers. Each identity provider is referenced by an **authentication scheme** label.

There are multiple scheme actions:
1. **Authenticate** scheme action
	- Configure how the ClaimPrincipal is reconstructed on every request.
	- It can be configured to get the identity data from the identity cookie.
1. **Challenge** scheme action
	- Configure what happen when an un-authenticatd user try to access resource that requires authentication.
	- Typically, the cookie scheme is configured to redirect user to a login page. The login page can be a local webapp or an external identity provider.
1. **Forbid** scheme action
	- Configure what happen an authenticated user try to access a resource that he is not authourized to access.
	- Typically, the cookie scheme will redirect user to an access-denied web page.

```csharp
AuthenticationBuilder authBuilder = services
    .AddAuthentication((AuthenticationOptions options) =>
    {
		// use idenity provided by this identity provider
        options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
        options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
        options.DefaultForbidScheme = IdentityConstants.ApplicationScheme;

		// configure which cooike to use for constructing PrincipalClaim for each request.
		options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
    })
	// save identity data to cookie to reconstruct the ClaimPrincipal
	// on subsequent request.
	.AddCookie(IdentityConstants.ApplicationScheme)
```

- AuthenticationServiceCollectionExtensions (IServiceCollection)
	- **AddAuthentication**() : **AuthenticationBuilder**
	- AddAuthentication(string defaultScheme) : AuthenticationBuilder
	- AddAuthentication(Action\<**AuthenticationOptions**> configure) : AuthenticationBuilder
	- AddAuthentication(string defaultScheme, Action\<AuthenticationOptions> configure) : AuthenticationBuilder
	- AddScheme\<THandler, TOptions>(...) : IServiceCollection
		- where THandler : AuthenticationHandler\<TOptions> 
		- where TOptions : **AuthenticationSchemeOptions**
		- string authenticationScheme, 
		- string displayName, 
		- Action\<**AuthenticationSchemeBuilder**> configureScheme
		- Action\<TOptions> configureOptions
	- AddScheme\<THandler, TOptions>(...) : IServiceCollection
		- where THandler : AuthenticationHandler\<TOptions> 
		- where TOptions : AuthenticationSchemeOptions
		- string authenticationScheme, 
		- string displayName, 
		- Action\<TOptions> configureOptions
	- AddScheme\<THandler, TOptions>(...) : IServiceCollection
		- where THandler : AuthenticationHandler\<TOptions> 
		- where TOptions : AuthenticationSchemeOptions
		- string authenticationScheme, 
		- Action\<TOptions> configureOptions

## Microsoft.AspNetCore.Authentication

- **AuthenticationBuilder**
	- AddPolicyScheme(...)
		- string authenticationScheme, 
		- string displayName, 
		- Action\<PolicySchemeOptions> configureOptions
	- AddRemoteScheme<TOptions, THandler>(...)
		- where TOptions : RemoteAuthenticationOptions,
		- where THandler : RemoteAuthenticationHandler\<TOptions>
		- string authenticationScheme, 
		- string displayName, 
		- Action\<TOptions> configureOptions
	- AddScheme<TOptions, THandler>(...)
		- where TOptions : AuthenticationSchemeOptions
		- where THandler : AuthenticationHandler\<TOptions>
		- string authenticationScheme, 
		- string displayName, 
		- Action\<TOptions> configureOptions
	- AddScheme<TOptions, THandler>(...)
		- where TOptions : AuthenticationSchemeOptions
		- where THandler : AuthenticationHandler\<TOptions>
		- string authenticationScheme, 
		- Action\<TOptions> configureOptions
	- AuthenticationBuilder(IServiceCollection services)
	- Services : IServiceCollection
- AuthenticationHandler\<TOptions> where TOptions : AuthenticationSchemeOptions
- AuthenticationMiddleware
- **AuthenticationSchemeOptions**
	- AuthenticationSchemeOptions()
	- Validate()
	- Validate(string scheme)
	- **ClaimsIssuer** 
	- **Events** : object
	- EventsType : System.Type
	- **ForwardAuthenticate** : string
	- ForwardChallenge : string 
	- ForwardDefault : string
	- ForwardDefaultSelector : Func<HttpContext, string> 
	- ForwardForbid : string
	- ForwardSignIn : string
	- ForwardSignOut : string
- Base64UrlTextEncoder
- BaseContext\<TOptions>
- HandleRequestContext\<TOptions> : BaseContext\<TOptions>
- HandleRequestResult : AuthenticateResult
- PolicySchemeHandler : SignInAuthenticationHandler\<PolicySchemeOptions> 
- PolicySchemeOptions : AuthenticationSchemeOptions
- PrincipalContext\<TOptions> : PropertiesContext\<TOptions>
- PropertiesContext\<TOptions> : BaseContext\<TOptions>
- PropertiesDataFormat : SecureDataFormat\<AuthenticationProperties>
- PropertiesSerializer
- RedirectContext\<TOptions> : PropertiesContext\<TOptions>
- RemoteAuthenticationContext\<TOptions> : HandleRequestContext\<TOptions>
- RemoteAuthenticationEvents
- RemoteAuthenticationHandler\<TOptions> : AuthenticationHandler\<TOptions>
- RemoteAuthenticationOptions : AuthenticationSchemeOptions
- RemoteFailureContext : HandleRequestContext\<RemoteAuthenticationOptions>
- ResultContext\<TOptions> : BaseContext\<TOptions>
- SecureDataFormat\<TData>
- SignInAuthenticationHandler\<TOptions> : SignOutAuthenticationHandler\<TOptions>
- SignOutAuthenticationHandler\<TOptions> : AuthenticationHandler\<TOptions>
- TicketDataFormat : SecureDataFormat\<AuthenticationTicket>
- TicketReceivedContext : RemoteAuthenticationContext\<RemoteAuthenticationOptions>

##  Microsoft.AspNetCore.Builder

- AuthAppBuilderExtensions
	- **UseAuthentication**()

# Assembly : Microsoft.AspNetCore.Authentication.Abstractions

## Microsoft.AspNetCore.Authentication

- **AuthenticateResult**
	- Fail(string failureMessage)
	- Fail(string failureMessage, AuthenticationProperties properties)
	- Fail(System.Exception failure)
	- Fail(System.Exception failure, AuthenticationProperties properties)
	- NoResult()
	- Success(**AuthenticationTicket** ticket)
	- Failure : Exeption
	- None : bool
	- Principal : **ClaimsPrincipal**
	- Properties : **AuthenticationProperties**
	- Succeeded : bool
	- Ticket : **AuthenticationTicket**
- AuthenticationHttpContextExtensions
- **AuthenticationOptions**
	- **AddScheme**(string name, Action\<AuthenticationSchemeBuilder> configureBuilder)
	- AddScheme\<THandler>(string name, string displayName)
		- where THandler : IAuthenticationHandler
	- AuthenticationOptions()
	- **DefaultAuthenticateScheme** : string
	- **DefaultChallengeScheme**
	- **DefaultForbidScheme**
	- **DefaultScheme**
	- **DefaultSignInScheme** 
	- **DefaultSignOutScheme** 
	- SchemeMap : IDictionary\<string, AuthenticationScheme>
	- Schemes : IEnumerable\<AuthenticationScheme>
- **AuthenticationProperties**
	- AuthenticationProperties()
	- AuthenticationProperties(IDictionary<string, string> items)
	- AuthenticationProperties(...)
		- IDictionary<string, string> items, 
		- IDictionary<string, object> parameters
	- GetParameter\<T>(string key) : T
	- GetString(string key) : string
	- SetParameter\<T>(string key, T value)
	- SetString(string key, string value)
	- AllowRefresh : bool
	- ExpiresUtc : DateTimeOffset?
	- **IsPersistent** : bool
	- IssuedUtc : DateTimeOffset?
	- **Items** : IDictionary\<string, string>
	- **Parameters** : IDictionary\<string, object>
	- **RedirectUri** : string
- AuthenticationScheme
	- AuthenticationScheme(...)
		- string name, 
		- string displayName, 
		- System.Type handlerType
	- DisplayName 
	- HandlerType : System.Type
	- Name 
- AuthenticationSchemeBuilder
	- AuthenticationSchemeBuilder(string name)
	- Build() : AuthenticationScheme
	- DisplayName 
	- HandlerType : System.Type
	- Name 
- **AuthenticationTicket**
	- AuthenticationTicket(...)
		- **ClaimsPrincipal** principal,
		- **AuthenticationProperties** properties, 
		- string authenticationScheme
	- AuthenticationTicket(...)
		- ClaimsPrincipal principal,
		- string authenticationScheme
	- AuthenticationScheme : string
	- Principal : ClaimsPrincipal
	- Properties : AuthenticationProperties
- AuthenticationToken
	- AuthenticationToken()
	- Name 
	- Value 
- AuthenticationTokenExtensions
	- GetTokenAsync(...) : Task\<string>
		- this IAuthenticationService auth
		- HttpContext context, 
		- string tokenName
	- GetTokenAsync(...)
		- this IAuthenticationService auth
		- HttpContext context, 
		- string scheme, 
		- string tokenName
	- GetTokens(this AuthenticationProperties properties) : IEnumerable\<AuthenticationToken>
	- GetTokenValue(this AuthenticationProperties properties, string tokenName) : string
	- StoreTokens(...)
		- this AuthenticationProperties properties, 
		- IEnumerable\<AuthenticationToken> tokens
	- UpdateTokenValue(...) : bool
		- this AuthenticationProperties properties, 
		- string tokenName, 
		- string tokenValue