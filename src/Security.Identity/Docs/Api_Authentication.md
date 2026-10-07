# Nuget Packages

- Microsoft.AspNetCore.Identity
	- Microsoft.Extensions.Identity.Core
	- Microsoft.AspNetCore.Authentication.Cookies
		- Microsoft.AspNetCore.Authentication
			- Microsoft.AspNetCore.Authentication.Core
				- Microsoft.AspNetCore.Authentication.Abstractions
					- Microsoft.AspNetCore.Http.Abstractions
				- Microsoft.AspNetCore.Http
				- Microsoft.AspNetCore.Http.Extensions
			- Microsoft.AspNetCore.DataProtection
			- Microsoft.AspNetCore.Http
			- Microsoft.AspNetCore.Http.Extensions


# Assembly : Microsoft.AspNetCore.DataProtection

## Microsoft.AspNetCore.DataProtection

- DataProtectionOptions
	- DataProtectionOptions()
	- ApplicationDiscriminator : string

## Microsoft.Extensions.DependencyInjection

- DataProtectionServiceCollectionExtensions
	- AddDataProtection()
	- **AddDataProtection**(Action\<DataProtectionOptions> configure)

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
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(
		// indicate this cookie came from local identity provider
		CookieAuthenticationDefaults.AuthenticationScheme,
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
- **CookieAuthenticationDefaults** (static)
	- AccessDeniedPath : PathString
	- CookiePrefix : string
	- LoginPath : PathString
	- LogoutPath : PathString
	- ReturnUrlParameter : string
	- **AuthenticationScheme** : string

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
- **CookieAuthenticationHandler** 
	- : **SignInAuthenticationHandler\<CookieAuthenticationOptions>**
	- CookieAuthenticationHandler(...)
		- IOptionsMonitor\<CookieAuthenticationOptions> options
		- ILoggerFactory logger
		- UrlEncoder encoder
		- ISystemClock clock
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

## Microsoft.AspNetCore.Authentication.Internal

- RequestPathBaseCookieBuilder : CookieBuilder
	- Build(HttpContext context, System.DateTimeOffset expiresFrom) : CookieOptions
	- AdditionalPath : string

## Microsoft.AspNetCore.Builder

- AuthAppBuilderExtensions (IApplicationBuilder)
	- UseAuthentication()

## Microsoft.AspNetCore.Authentication

- **AuthenticationBuilder**
	- AddPolicyScheme(...)
		- string authenticationScheme, 
		- string displayName, 
		- Action\<PolicySchemeOptions> configureOptions
	- AddRemoteScheme<TOptions, THandler>(...)
		- where TOptions : RemoteAuthenticationOptions,
		- where **THandler** : **RemoteAuthenticationHandler\<TOptions>**
		- string authenticationScheme, 
		- string displayName, 
		- Action\<TOptions> configureOptions
	- AddScheme<TOptions, THandler>(...)
		- where TOptions : AuthenticationSchemeOptions
		- where **THandler** : **AuthenticationHandler\<TOptions>**
		- string authenticationScheme, 
		- string displayName, 
		- Action\<TOptions> configureOptions
	- AddScheme<TOptions, THandler>(...)
		- where TOptions : AuthenticationSchemeOptions
		- where **THandler** : **AuthenticationHandler\<TOptions>**
		- string authenticationScheme, 
		- Action\<TOptions> configureOptions
	- AuthenticationBuilder(IServiceCollection services)
	- Services : IServiceCollection
- **AuthenticationHandler\<TOptions>** (abstract) : **IAuthenticationHandler**
	- where TOptions : AuthenticationSchemeOptions
	- **AuthenticateAsync**() : Task\<AuthenticateResult>
	- ChallengeAsync(AuthenticationProperties properties)) : Task
	- ForbidAsync(AuthenticationProperties properties)) : Task
	- InitializeAsync(AuthenticationScheme scheme, HttpContext context) : Task
	- Options : TOptions
	- Scheme : **AuthenticationScheme**
	- AuthenticationHandler(...)
		- OptionsMonitor\<TOptions> options
		- ILoggerFactory logger
		- UrlEncoder encoder
		- ISystemClock clock
	- BuildRedirectUri(string targetPath) : string
	- CreateEventsAsync() : Task\<object>
	- HandleAuthenticateAsync() : Task\<AuthenticateResult>
	- HandleAuthenticateOnceAsync() : Task\<AuthenticateResult>
	- HandleAuthenticateOnceSafeAsync() : Task\<AuthenticateResult>
	- HandleChallengeAsync(AuthenticationProperties properties) : Task
	- HandleForbiddenAsync(AuthenticationProperties properties) : Task
	- InitializeEventsAsync() : Task
	- InitializeHandlerAsync() : Task
	- ResolveTarget(string scheme) : string
	- ClaimsIssuer : string
	- Clock  : ISystemClock
	- Context : HttpContext
	- CurrentUri : string
	- Events : object
	- Logger : ILogger
	- OptionsMonitor : IOptionsMonitor\<TOptions>
	- OriginalPath : PathString
	- OriginalPathBase : PathString
	- Request : HttpRequest
	- Response : HttpResponse
	- UrlEncoder : UrlEncoder
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
	- where TOptions : AuthenticationSchemeOptions
	- BaseContext(MHttpContext context, AuthenticationScheme scheme, TOptions options)
	- HttpContext : HttpContext 
	- Options : TOptions
	- Request : HttpRequest
	- Response : HttpResponse
	- Scheme : AuthenticationScheme
- HandleRequestContext\<TOptions> : BaseContext\<TOptions>
	- HandleRequestContextHttpContext context, AuthenticationScheme scheme, TOptions options)
	- HandleResponse()
	- SkipHandler()
	- Result : HandleRequestResult
- HandleRequestResult : AuthenticateResult
	- Fail(string failureMessage) : AuthenticateResult
	- Fail(string failureMessage, AuthenticationProperties properties) : AuthenticateResult
	- Fail(System.Exception failure) : AuthenticateResult
	- Fail(System.Exception failure, AuthenticationProperties properties) : AuthenticateResult
	- NoResult() : AuthenticateResult
	- Success(AuthenticationTicket ticket) : AuthenticateResult
	- Failure : System.Exception
	- None : bool
	- Principal : ClaimPrincipal
	- Properties : AuthenticationProperties
	- Succeeded : bool
	- Ticket : AuthenticationTicket
- IDataSerializer\<TModel>
- ISecureDataFormat\<TData>
- ISystemClock
- PolicySchemeHandler : **SignInAuthenticationHandler\<PolicySchemeOptions>** 
	- PolicySchemeHandler(...)
		- IOptionsMonitor\<PolicySchemeOptions> options
		- ILoggerFactory logger
		- UrlEncoder encoder
		- ISystemClock clock
	- HandleAuthenticateAsync() : Task\<AuthenticateResult>
	- HandleChallengeAsync(AuthenticationProperties properties) : Task
	- HandleForbiddenAsync(AuthenticationProperties properties) : Task
	- HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties properties) : Task
	- HandleSignOutAsync(AuthenticationProperties properties) : Task
- PolicySchemeOptions : AuthenticationSchemeOptions
- PrincipalContext\<TOptions> : PropertiesContext\<TOptions> (abstract)
	- PrincipalContext(...)
		- HttpContext context, 
		- AuthenticationScheme scheme, 
		- TOptions options, 
		- AuthenticationProperties properties
	- Principal : PrincipalClaim
- PropertiesContext\<TOptions> : BaseContext\<TOptions> (abstract)
	- PropertiesContext(...)
		- HttpContext context, 
		- AuthenticationScheme scheme, 
		- TOptions options, 
		- AuthenticationProperties properties
	- Properties : AuthenticationProperties
- PropertiesDataFormat : SecureDataFormat\<AuthenticationProperties>
	- PropertiesDataFormat(IDataProtector protector)
- PropertiesSerializer
	- Deserialize(byte[] data) : AuthenticationProperties
	- PropertiesSerializer()
	- Read(System.IO.BinaryReader reader) : AuthenticationProperties
	- Serialize(AuthenticationProperties model) : byte[]
	- Write(BinaryWriter writer,AuthenticationProperties properties)
	- Default : PropertiesSerializer
- RedirectContext\<TOptions> : PropertiesContext\<TOptions>
	- RedirectContext(...)
		- HttpContext context, 
		- AuthenticationScheme scheme, 
		- TOptions options, 
		- AuthenticationProperties properties, 
		- string redirectUri
	- RedirectUri : string
- RemoteAuthenticationContext\<TOptions> : HandleRequestContext\<TOptions> (abstract)
	- RemoteAuthenticationContext(...)
		- HttpContext context, 
		- AuthenticationScheme scheme, 
		- TOptions options, 
		- AuthenticationProperties properties
	- Fail(string failureMessage)
	- Fail(System.Exception failure)
	- Success()
	- Principal : ClaimPrincipal
	- Properties : AuthenticationProperties
- RemoteAuthenticationEvents
	- RemoteAuthenticationEvents()
	- RemoteFailure(RemoteFailureContext context) : Task
	- TicketReceived(TicketReceivedContext context) : Task
	- OnRemoteFailure : System.Func<RemoteFailureContext, Task> 
	- OnTicketReceived : System.Func<TicketReceivedContext, Task>
- RemoteAuthenticationHandler\<TOptions> : AuthenticationHandler\<TOptions> (abstract)
	- RemoteAuthenticationHandler(...)
		- IOptionsMonitor\<TOptions> options, 
		- ILoggerFactory logger, 
		- UrlEncoder encoder, 
		- ISystemClock clock
	- CreateEventsAsync() : Task\<object>
	- GenerateCorrelationId(AuthenticationProperties properties)
	- HandleAuthenticateAsync() : Task\<AuthenticationProperties>
	- HandleForbiddenAsync(AuthenticationProperties properties) : Task
	- HandleRemoteAuthenticateAsync() : Task\<HandleRequestResult>
	- HandleRequestAsync() : Task\<bool>
	- ShouldHandleRequestAsync() : Task\<bool>
	- ValidateCorrelationId(AuthenticationProperties properties) : bool
	- Events : RemoteAuthenticationEvents
	- SignInScheme : string
- RemoteAuthenticationOptions : AuthenticationSchemeOptions
	- RemoteAuthenticationOptions()
	- Validate()
	- Validate(string scheme)
	- Backchannel : HttpClient
	- BackchannelHttpHandler : HttpMessageHandler
	- BackchannelTimeout : System.TimeSpan
	- CallbackPath : PathString
	- CorrelationCookie : CookieBuilder
	- DataProtectionProvider : IDataProtectionProvider
	- Events : RemoteAuthenticationEvents
	- RemoteAuthenticationTimeout : System.TimeSpan
	- SaveTokens : bool
	- SignInScheme : string
- RemoteFailureContext : HandleRequestContext\<RemoteAuthenticationOptions>
	- RemoteFailureContext(...)
		- Http.HttpContext context
		- AuthenticationScheme scheme
		- RemoteAuthenticationOptions options
		- System.Exception failure
	- Failure : System.Exception
	- Properties : AuthenticationProperties
- ResultContext\<TOptions> : BaseContext\<TOptions> (abstract)
	- ResultContext(...)
		- HttpContext context, 
		- AuthenticationScheme scheme, 
		- TOptions options
	- Fail(string failureMessage)
	- Fail(System.Exception failure)
	- NoResult()
	- Success()
	- Principal : ClaimPrincipal
	- Properties : AuthenticationProperties
	- Result : AuthenticateResult
- SecureDataFormat\<TData>
	- SecureDataFormat(...)
		- IDataSerializer\<TData> serializer, 
		- IDataProtector protector
	- Protect(TData data) : string
	- Protect(TData data, string purpose) : string
	- Unprotect(string protectedText) : TData
	- Unprotect(string protectedText, string purpose) : TData
- **SignInAuthenticationHandler\<TOptions>** (abstract)
	- : IAuthenticationHandler
	- : IAuthenticationSignInHandler
	- : IAuthenticationSignOutHandler
	- : **SignOutAuthenticationHandler\<TOptions>**
	- SignInAuthenticationHandler(...)
		- IOptionsMonitor\<TOptions> options, 
		- ILoggerFactory logger, 
		- UrlEncoder encoder, 
		- ISystemClock clock
	- HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties properties) : Task
	- SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties) : Task
- **SignOutAuthenticationHandler\<TOptions>** (abstract)
	- : IAuthenticationHandler
	- : IAuthenticationSignOutHandler
	- : **AuthenticationHandler\<TOptions>**
	- SignOutAuthenticationHandler(...)
		- IOptionsMonitor\<TOptions> options, 
		- ILoggerFactory logger, 
		- UrlEncoder encoder, 
		- ISystemClock clock
	- HandleSignOutAsync(AuthenticationProperties properties) : Task
	- SignOutAsync(AuthenticationProperties properties) : Task
- SystemClock
	- UtcNow : System.DateTimeOffset
- TicketDataFormat : SecureDataFormat\<AuthenticationTicket>
	- TicketDataFormat(IDataProtector protector)
- TicketReceivedContext : RemoteAuthenticationContext\<RemoteAuthenticationOptions>
	- TicketReceivedContext(...)
		- HttpContext context
		- AuthenticationScheme scheme
		- RemoteAuthenticationOptions options
		- AuthenticationTicket ticket
	- ReturnUri : string
- TicketSerializer
	- Deserialize(byte[] data) : AuthenticationTicket
	- Read(BinaryReader reader) : AuthenticationTicket
	- ReadClaim(BinaryReader reader, ClaimsIdentity identity) : Claim
	- ReadIdentity(BinaryReader reader) : ClaimsIdentity
	- Serialize(AuthenticationTicket ticket) : byte[]
	- Write(BinaryWriter writer, AuthenticationTicket ticket)
	- WriteClaim(BinaryWriter writer, Claim claim)
	- WriteIdentity(BinaryWriter writer, ClaimsIdentity identity)
	- Default : TicketSerializer

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
- **AuthenticationHttpContextExtensions** (HttpContext)
	- AuthenticateAsync() : Task\<AuthenticateResult>
	- **AuthenticateAsync**(string scheme) : Task\<AuthenticateResult>
	- ChallengeAsync() : Task
	- ChallengeAsync(**AuthenticationProperties** properties) : Task
	- ChallengeAsync(string scheme) : Task
	- **ChallengeAsync**(string scheme, AuthenticationProperties properties) : Task
	- ForbidAsync() : Task
	- ForbidAsync(AuthenticationProperties properties) : Task
	- ForbidAsync(string scheme) : Task
	- **ForbidAsync**(string scheme, AuthenticationProperties properties) : Task
	- GetTokenAsync(string tokenName) : Task\<string>
	- GetTokenAsync(string scheme, string tokenName) : Task\<string>
	- SignInAsync(string scheme) : Task
	- **SignInAsync**(...) : Task
		- string scheme, 
		- ClaimsPrincipal principal, 
		- AuthenticationProperties properties
	- SignInAsync(ClaimsPrincipal principal) : Task
	- SignInAsync(ClaimsPrincipal principal, AuthenticationProperties properties) : Task
	- SignOutAsync() : Task
	- SignOutAsync(AuthenticationProperties properties) : Task
	- SignOutAsync(string scheme) : Task
	- **SignOutAsync**(string scheme, AuthenticationProperties properties) : Task
- **AuthenticationOptions**
	- **AddScheme**(string name, Action\<**AuthenticationSchemeBuilder**> configureBuilder)
	- AddScheme\<THandler>(string name, string displayName)
		- where THandler : **IAuthenticationHandler**
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
		- IDictionary<string, string> **items**, 
		- IDictionary<string, object> **parameters**
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
- **AuthenticationSchemeBuilder**
	- AuthenticationSchemeBuilder(string name)
	- Build() : **AuthenticationScheme**
	- DisplayName 
	- **HandlerType** : System.Type
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
- IAuthenticationFeature
	- OriginalPath : PathString
	- OriginalPathBase : PathString
- **IAuthenticationHandler**
	- AuthenticateAsync() : Task\<AuthenticateResult>
	- ChallengeAsync() : Task\<AuthenticationProperties>
	- ForbidAsync(AuthenticationProperties properties) : Task
	- InitializeAsync(AuthenticationScheme sheme, HttpContext context) : Task
- **IAuthenticationHandlerProvider**
	- GetHandlerAsync(...) : Task\<IAuthenticationHandler>
		- HttpContext context
		- string authenticationSheme
- **IAuthenticationRequestHandler**
	- HandleRequestAsync() : Task\<bool>
- **IAuthenticationSchemeProvider**
	- AddScheme(AuthenticationScheme scheme)
	- GetAllSchemesAsync() : Task\<IEnumerable\<AuthenticationScheme>>
	- GetDefaultAuthenticateSchemeAsync() : Task\<AuthenticationScheme>
	- GetDefaultChallengeSchemeAsync() : Task\<AuthenticationScheme>
	- GetDefaultForbidSchemeAsync() : Task\<AuthenticationScheme>
	- GetDefaultSignInSchemeAsync() : Task\<AuthenticationScheme>
	- GetDefaultSignOutSchemeAsync() : Task\<AuthenticationScheme>
	- GetRequestHandlerSchemesAsync() : Task\<IEnumerable\<AuthenticationScheme>>
	- GetSchemeAsync(string name) : Task\<AuthenticationScheme>
	- RemoveScheme(string name)
- **IAuthenticationService**
	- AuthenticateAsync(HttpContext context, string scheme) : Task\<AuthenticateResult>
	- ChallengeAsync(...) : Task
		- HttpContext context, 
		- string scheme, 
		- AuthenticationProperties properties
	- ForbidAsync(...) : Task
		- HttpContext context, 
		- string scheme, 
		- AuthenticationProperties properties
	- SignInAsync(...) : Task
		- HttpContext context, 
		- string scheme, 
		- ClaimsPrincipal principal,
		- AuthenticationProperties properties
	- SignOutAsync(...) : Task
		- HttpContext context, 
		- string scheme, 
		- AuthenticationProperties properties
- **IAuthenticationSignInHandler**
	- SignInAsync(ClaimPrincipal user, AuthenticationProperties properties) : Task
- **IAuthenticationSignOutHandler**
	- SignOutAsync(AuthenticationProperties properties) : Task
- **IClaimsTransformation**
	- TransformAsync(ClaimPrincipal principal) : Task\<ClaimPrincipal>

# Assembly : Microsoft.AspNetCore.Authentication.Core

## Microsoft.Extensions.DependencyInjection

- AuthenticationCoreServiceCollectionExtensions
	- AddAuthenticationCore()
	- AddAuthenticationCore(System.Action\<AuthenticationOptions> configureOptions))

## Microsoft.AspNetCore.Authentication

- AuthenticationFeature : IAuthenticationFeature
	- OriginalPath : PathString
	- OriginalPathBase : PathString
- AuthenticationHandlerProvider : **IAuthenticationHandlerProvider**
	- AuthenticationHandlerProvider(IAuthenticationSchemeProvider schemes)
	- GetHandlerAsync(HttpContext context, string authenticationScheme) : Task\<IAuthenticationHandler>
	- Schemes : IAuthenticationSchemeProvider 
- AuthenticationSchemeProvider : **IAuthenticationSchemeProvider**
	- AuthenticationSchemeProvider(IOptions\<AuthenticationOptions> options)
	- AuthenticationSchemeProvider(...)
		- IOptions\<AuthenticationOptions> options, 
		- IDictionary<string, AuthenticationScheme> schemes
	- AddScheme(AuthenticationScheme scheme)
	- GetAllSchemesAsync() : Task\<IEnumerable\<AuthenticationScheme>>
	- GetDefaultAuthenticateSchemeAsync() : Task\<AuthenticationScheme>
	- GetDefaultChallengeSchemeAsync() : Task\<AuthenticationScheme>
	- GetDefaultForbidSchemeAsync() : Task\<AuthenticationScheme>
	- **GetDefaultSignInSchemeAsync**() : Task\<AuthenticationScheme>
	- GetDefaultSignOutSchemeAsync() : Task\<AuthenticationScheme>
	- GetRequestHandlerSchemesAsync() : Task<IEnumerable\<AuthenticationScheme>>
	- GetSchemeAsync(string name) : Task\<AuthenticationScheme>
	- RemoveScheme(string name)
- AuthenticationService : **IAuthenticationService**
	- AuthenticationService(...)
		- IAuthenticationSchemeProvider schemes, 
		- IAuthenticationHandlerProvider handlers, 
		- IClaimsTransformation transform
	- **AuthenticateAsync**(HttpContext context, string scheme) : Task\<AuthenticateResult>
	- **ChallengeAsync**(HttpContext context, string scheme, AuthenticationProperties properties) : Task
	- **ForbidAsync**(HttpContext context, string scheme, AuthenticationProperties properties) : Task
	- **SignInAsync**(...) : Task
		- HttpContext context, 
		- string scheme, 
		- ClaimsPrincipal principal, 
		- AuthenticationProperties properties
	- **SignOutAsync**(HttpContext context, string scheme, AuthenticationProperties properties) : Task
	- Handlers : **IAuthenticationHandlerProvider**
	- Schemes : **IAuthenticationSchemeProvider**
	- Transform : **IClaimsTransformation**
- NoopClaimsTransformation : **IClaimsTransformation**
	- TransformAsync(ClaimsPrincipal principal) : Task\<ClaimsPrincipal> 
