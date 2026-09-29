# Assembly : Microsoft.AspNetCore.Http.Abstractions

## Microsoft.AspNetCore.Http

- ConnectionInfo (abstract)
- CookieBuilder
- CookieSecurePolicy
- FragmentString
- HeaderDictionaryExtensions
- HostString
- HttpContext (abstract)
- HttpMethods (static)
- HttpRequest (abstract)
- HttpResponse (abstract)
- HttpResponseWritingExtensions
- IHttpContextAccessor
	- HttpContext : HttpContext
- IHttpContextFactory
	- Create(IFeatureCollection featureCollection) : HttpContext
	- Dispose(HttpContext httpContext)
- IMiddleware
- IMiddlewareFactory
- PathString
- QueryString
- RequestDelegate(HttpContext context) : Task (delegate)
- StatusCodes (static)
- WebSocketManager (static)

## Microsoft.AspNetCore.Http.Authentication

- AuthenticateInfo
	- AuthenticateInfo()
	- Description : AuthenticationDescription
	- Principal : ClaimsPrincipal
	- Properties : AuthenticationProperties
- AuthenticationDescription
	- AuthenticationDescription()
	- AuthenticationDescription(IDictionary<string, object> items)
	- AuthenticationScheme : string
	- DisplayName : string
	- Items : IDictionary<string, object> items
- AuthenticationManager (abstract)
- AuthenticationProperties
	- AuthenticationProperties()
	- AuthenticationProperties(IDictionary<string, string> items)
	- AllowRefresh : bool?
	- ExpiresUtc : System.DateTimeOffset?
	- IsPersistent : bool
	- IssuedUtc : System.DateTimeOffset?
	- Items : IDictionary<string, string>
	- RedirectUri : string

# Assembly : Microsoft.AspNetCore.Http

## Microsoft.AspNetCore.Http