# Assembly : Microsoft.Net.Http.Headers

## Microsoft.Net.Http.Headers

- CookieHeaderValue
- HeaderNames
	- Cookie
	- SetCookie
- SetCookieHeaderValue

# Assembly : Microsoft.AspNetCore.Http.Abstractions

## Microsoft.AspNetCore.Http

- ConnectionInfo (abstract)
- CookieBuilder
- CookieSecurePolicy
- FragmentString
- HeaderDictionaryExtensions
- HostString
- **HttpContext** (abstract)
	- Abort()
	- **Authentication** : AuthenticationManager
	- Connection : ConnectionInfo
	- Features : IFeatureCollection 
	- Items : IDictionary<object, object>
	- **Request** : HttpRequest
	- RequestAborted : CancellationToken
	- RequestServices : IServiceProvider
	- **Response** : HttpResponse
	- Session : ISession
	- TraceIdentifier : string
	- **User** : ClaimsPrincipal
	- WebSockets : WebSocketManager
- HttpMethods (static)
- **HttpRequest** (abstract)
	- ReadFormAsync() : Task\<IFormCollection>
	- Body : Stream
	- ContentLength : long?
	- ContentType : string
	- **Cookies** : **IRequestCookieCollection**
	- Form : IFormCollection
	- HasFormContentType : bool
	- Headers : IHeaderDictionary
	- Host : HostString
	- **HttpContext** : HttpContext
	- IsHttps : bool
	- Method : string
	- Path : PathString
	- PathBase : PathString
	- Protocol : string
	- Query : IQueryCollection
	- QueryString : string
	- Scheme : string
- **HttpResponse** (abstract)
	- OnCompleted(System.Func<object, Task> callback, object state)
	- OnCompleted(System.Func\<Task> callback)
	- OnStarting(System.Func<object, Task> callback, object state)
	- OnStarting(System.Func\<Task> callback)
	- Redirect(string location)
	- Redirect(string location, bool permanent)
	- RegisterForDispose(IDisposable disposable)
	- Body : Stream
	- ContentLength : bool
	- ContentType : string
	- **Cookies** : **IResponseCookies**
	- HasStarted : bool
	- Headers : IHeaderDictionary
	- **HttpContext** : HttpContext
	- StatusCode : int
- HttpResponseWritingExtensions
- **IHttpContextAccessor**
	- HttpContext : HttpContext
- **IHttpContextFactory**
	- Create(IFeatureCollection featureCollection) : HttpContext
	- Dispose(HttpContext httpContext)
- IMiddleware
- IMiddlewareFactory
- PathString
- QueryString
- **RequestDelegate**(HttpContext context) : Task (delegate)
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
- **AuthenticationManager** (abstract)
	- AuthenticateAsync(AuthenticateContext context) : Task
	- **AuthenticateAsync**(string authenticationScheme) : Task\<ClaimsPrincipal>
	- ChallengeAsync() : Task
	- ChallengeAsync(AuthenticationProperties properties) : Task
	- ChallengeAsync(string authenticationScheme) : Task
	- ChallengeAsync(...) : Task
		- string authenticationScheme, 
		- AuthenticationProperties properties
	-  ChallengeAsync(...) : Task
		- string authenticationScheme, 
		- AuthenticationProperties properties, 
		- ChallengeBehavior behavior
	- ForbidAsync() : Task
	- ForbidAsync(AuthenticationProperties properties) : Task
	- ForbidAsync(string authenticationScheme) : Task
	- ForbidAsync(...) : Task
		- string authenticationScheme, 
		- AuthenticationProperties properties
	- **GetAuthenticateInfoAsync**(string authenticationScheme): Task\<**AuthenticateInfo**>
	- **GetAuthenticationSchemes**() : IEnumerable\<**AuthenticationDescription**>
	- SignInAsync(string authenticationScheme, ClaimsPrincipal principal) : Task
	- **SignInAsync**(...) : Task
		- string authenticationScheme, 
		- ClaimsPrincipal principal, 
		- AuthenticationProperties properties
	- SignOutAsync(string authenticationScheme) : Task
	- **SignOutAsync**(...) : Task
		- string authenticationScheme, 
		- AuthenticationProperties properties
	- HttpContext : HttpContext 
	- AutomaticScheme : string
- **AuthenticationProperties**
	- AuthenticationProperties()
	- AuthenticationProperties(IDictionary<string, string> items)
	- AllowRefresh : bool?
	- ExpiresUtc : System.DateTimeOffset?
	- IsPersistent : bool
	- IssuedUtc : System.DateTimeOffset?
	- Items : IDictionary<string, string>
	- RedirectUri : string

## Microsoft.AspNetCore.Builder

- IApplicationBuilder
	- Build() : RequestDelegate
	- New() : IApplicationBuilder
	- Use(...) : IApplicationBuilder
		- System.Func<RequestDelegate, 
		- RequestDelegate> middleware
	- ApplicationServices : IServiceProvider
	- Properties : IDictionary<string, object>
	- ServerFeatures : IFeatureCollection
- MapExtensions
	- Map(...) : IApplicationBuilder
		- PathString pathMatch, 
		- System.Action\<IApplicationBuilder> configuration
- MapWhenExtensions
	- MapWhen(...) : IApplicationBuilder
		- System.Func<HttpContext, bool> predicate, 
		- System.Action\<IApplicationBuilder> configuration
- RunExtensions
	- Run(RequestDelegate handler) : void
- UseExtensions
	- Use(...) : IApplicationBuilder
		- System.Func<HttpContext, System.Func\<Task>, Task> middleware
- UseMiddlewareExtensions
	- UseMiddleware(...) : IApplicationBuilder
		- System.Type middleware, params object[] args
	- UseMiddleware\<TMiddleware>(params object[] args) : IApplicationBuilder
- UsePathBaseExtensions
	- UsePathBase(MPathString pathBase) : IApplicationBuilder
- UseWhenExtensions
	- UseWhen(...) : IApplicationBuilder
		- System.Func<HttpContext, bool> predicate, 
		- System.Action\<IApplicationBuilder> configuration

## Microsoft.AspNetCore.Builder.Extensions

- MapMiddleware
	- MapMiddleware(RequestDelegate next, MapOptions options)
	- Invoke(HttpContext context)
- MapOptions
	- MapOptions()
	- Branch : RequestDelegate
	- PathMatch : PathString
- MapWhenMiddleware
	- MapWhenMiddleware(RequestDelegate next, MapWhenOptions options)
	- Invoke(HttpContext context)
- MapWhenOptions
	- MapWhenOptions()
	- Branch : RequestDelegate
	- Predicate : System.Func<HttpContext, bool>
- UsePathBaseMiddleware
	- UsePathBaseMiddleware(RequestDelegate next, PathString pathBase)
	- Invoke(HttpContext context)

# Assembly : Microsoft.AspNetCore.Http

## Microsoft.AspNetCore.Http

- **DefaultHttpContext** : HttpContext
	- DefaultHttpContext()
	- DefaultHttpContext(**IFeatureCollection** features)
	- Initialize(IFeatureCollection features)
	- InitializeAuthenticationManager() : AuthenticationManager
	- InitializeConnectionInfo() : ConnectionInfo
	- InitializeHttpRequest() : HttpRequest 
	- InitializeHttpResponse() : HttpResponse
	- InitializeWebSocketManager() : WebSocketManager 
	- Uninitialize()
	- UninitializeAuthenticationManager(AuthenticationManager instance)
	- UninitializeConnectionInfo(ConnectionInfo instance)
	- UninitializeHttpRequest(HttpRequest instance)
	- UninitializeHttpResponse(HttpResponse instance)
	- UninitializeWebSocketManager(WebSocketManager instance)
- FormCollection
	- : Enumerable<KeyValuePair<String, StringValues>>
	- : IFormCollection
- FormCollection.Enumerator
- HeaderDictionary 
	- : IHeaderDictionary
	- : ICollection<KeyValuePair<String, StringValues>>
	- : IDictionary<String, StringValues>
	- : IEnumerable<KeyValuePair<String, StringValues>>
- HeaderDictionary.Enumerator
- **HttpContextAccessor** : IHttpContextAccessor
	- HttpContextAccessor()
	- HttpContext : HttpContext 
- **HttpContextFactory** : IHttpContextFactory
	- HttpContextFactory(IOptions\<FormOptions> formOptions)
	- HttpContextFactory(...)
		- IOptions\<FormOptions> formOptions, 
		- IHttpContextAccessor httpContextAccessor
- HttpRequestRewindExtensions
- MiddlewareFactory : IMiddlewareFactory
	- MiddlewareFactory(IServiceProvider serviceProvider)
- RequestFormReaderExtensions

## Microsoft.AspNetCore.Builder.Internal

- **ApplicationBuilder** : IApplicationBuilder
	- ApplicationBuilder(IServiceProvider serviceProvider)
	- ApplicationBuilder(IServiceProvider serviceProvider, object server)

## Microsoft.AspNetCore.Http.Authentication.Internal

- **DefaultAuthenticationManager** : AuthenticationManager
	- DefaultAuthenticationManager(HttpContext context)
	- AuthenticateAsync(AuthenticateContext context) : Task
	- ChallengeAsync(...) : Task
		- string authenticationScheme, 
		- AuthenticationProperties properties, 
		- ChallengeBehavior behavior
	- GetAuthenticateInfoAsync(string authenticationScheme) : Task\<AuthenticateInfo>
	- GetAuthenticationSchemes() : IEnumerable\<AuthenticationDescription>
	- Initialize(HttpContext context)
	- **SignInAsync**(...) : Task
		- string authenticationScheme, 
		- ClaimsPrincipal principal, 
		- AuthenticationProperties properties
	- **SignOutAsync**(...) : Task
		- string authenticationScheme, 
		- AuthenticationProperties properties
	- Uninitialize()
	- HttpContext : HttpContext 

## Microsoft.AspNetCore.Http.Features.Authentication

- **HttpAuthenticationFeature** : IHttpAuthenticationFeature
	- HttpAuthenticationFeature()
	- Handler : IAuthenticationHandler
	- User : ClaimsPrincipal

## Microsoft.AspNetCore.Http.Features

- DefaultSessionFeature : ISessionFeature
- FormFeature : IFormFeature
- FormOptions
- HttpConnectionFeature : IHttpConnectionFeature
- HttpRequestFeature : IHttpRequestFeature
- HttpRequestIdentifierFeature : IHttpRequestIdentifierFeature
- HttpRequestLifetimeFeature : IHttpRequestLifetimeFeature
- HttpResponseFeature : IHttpResponseFeature
- ItemsFeature : IItemsFeature
- QueryFeature : IQueryFeature
- RequestCookiesFeature : IRequestCookiesFeature
- ResponseCookiesFeature : IResponseCookiesFeature
- ServiceProvidersFeature : IServiceProvidersFeature
- TlsConnectionFeature : ITlsConnectionFeature

## Microsoft.AspNetCore.Http.Features.Authentication

- HttpAuthenticationFeature : IHttpAuthenticationFeature

## Microsoft.AspNetCore.Http.Internal

- BindingAddress
- BufferingHelper
- DefaultConnectionInfo : ConnectionInfo
	- DefaultConnectionInfo(IFeatureCollection features)
	- GetClientCertificateAsync() : Task\<X509Certificate2>
	- Initialize(IFeatureCollection features)
	- Uninitialize()
	- ClientCertificate : X509Certificate2
	- Id : string
	- LocalIpAddress : IPAddress
	- LocalPort : int
	- RemoteIpAddress : IPAddress
	- RemotePort : int
- **DefaultHttpRequest** : HttpRequest
	- DefaultHttpRequest(HttpContext context)
	- Initialize(HttpContext context)
	- Uninitialize()
- **DefaultHttpResponse** : HttpResponse
	- DefaultHttpResponse(HttpContext context)
	- Initialize(HttpContext context)
	- Uninitialize()
- DefaultWebSocketManager : WebSocketManager
	- DefaultWebSocketManager(IFeatureCollection features)
- FormFile : IFormFile
- FormFileCollection : List\<IFormFile>
- ItemsDictionary
- QueryCollection : IQueryCollection
- QueryCollection.Enumerator
- **RequestCookieCollection** : IRequestCookieCollection
	- RequestCookieCollection()
	- RequestCookieCollection(int capacity)
	- RequestCookieCollection(Dictionary<string, string> store)
	- Parse(IList\<string> values) : RequestCookieCollection
	- Empty : RequestCookieCollection
- RequestCookieCollection.Enumerator
- **ResponseCookies** : IResponseCookies
	- ResponseCookies(...)
		- IHeaderDictionary headers, 
		- ObjectPool\<StringBuilder> builderPool

## Microsoft.Extensions.DependencyInjection

- HttpServiceCollectionExtensions
	- **AddHttpContextAccessor**()

# Assembly: Microsoft.AspNetCore.Http.Features

## Microsoft.AspNetCore.Http

- **CookieOptions**
	- CookieOptions()
	- Domain : string
	- Expires : System.DateTimeOffset?
	- HttpOnly : bool
	- IsEssential : bool
	- MaxAge : System.TimeSpan?
	- Path : string
	- SameSite : SameSiteMode
	- Secure : bool
- IFormCollection
	- ContainsKey(string key)
	- TryGetValue(string key, out StringValues value)
	- Count : int
	- Files : IFormFileCollection
	- Keys : ICollection\<string>
	- this[string key] : StringValues
- IFormFile
	- CopyTo(Stream target)
	- CopyToAsync(Stream target)
	- OpenReadStream() : Stream
	- ContentDisposition : string
	- ContentType : string
	- FileName : string
	- Headers : IHeaderDictionary
	- Length : long
	- Name : string
- IFormFileCollection
- IHeaderDictionary
- IQueryCollection
- **IRequestCookieCollection**
	- ContainsKey(string key) : bool
	- TryGetValue(string key, out string value) : bool
	- Count : int
	- Keys : ICollection\<string>
	- this[string key] : string
- **IResponseCookies**
	- Append(string key, string value)
	- Append(string key, string value, CookieOptions options)
	- Delete(string key)
	- Delete(string key, CookieOptions options)
- ISession
- SameSiteMode
	- Lax
	- None
	- Strict
- WebSocketAcceptContext

## Microsoft.AspNetCore.Http.Features

- FeatureCollection()
	- FeatureCollection(IFeatureCollection defaults)
	- Get\<TFeature>() : TFeature
	- GetEnumerator() : IEnumerator<KeyValuePair<System.Type, object>>
	- Set\<TFeature>(TFeature instance)
	- IsReadOnly : bool
	- Revision : int
	- this[System.Type key] : object
- FeatureReference\<T>
- FeatureReferences\<TCache>
- IFeatureCollection
- IFormFeature
- IHttpBodyControlFeature
- IHttpBufferingFeature
- IHttpConnectionFeature
- IHttpMaxRequestBodySizeFeature
- IHttpRequestFeature
- IHttpRequestIdentifierFeature
- IHttpRequestLifetimeFeature
- IHttpResponseFeature
- IHttpSendFileFeature
- IHttpUpgradeFeature
- IHttpWebSocketFeature
- IItemsFeature
- IQueryFeature
- IRequestCookiesFeature
- IResponseCookiesFeature
- IServiceProvidersFeature
- ISessionFeature
- ITlsConnectionFeature
- ITlsTokenBindingFeature
- ITrackingConsentFeature

## Microsoft.AspNetCore.Http.Features.Authentication

- **AuthenticateContext**
	- AuthenticateContext(string authenticationScheme)
	- Authenticated(...)
		- ClaimsPrincipal principal, 
		- IDictionary<string, string> properties, 
		- IDictionary<string, object> description
	- Failed(System.Exception error)
	- NotAuthenticated()
	- Accepted : bool
	- AuthenticationScheme : string
	- Description : IDictionary<string, object>
	- Error : System.Exception
	- Principal : ClaimsPrincipal
	- Properties : IDictionary<string, string>
- ChallengeBehavior
	- Automatic
	- Forbidden
	- Unauthorized
- **ChallengeContext**
	- ChallengeContext(string authenticationScheme)
	- ChallengeContext(...)
		- string authenticationScheme, 
		- IDictionary<string, string> properties, 
		- ChallengeBehavior behavior
	- Accept()
	- Accepted : bool
	- AuthenticationScheme : string
	- Behavior : ChallengeBehavior
	- Properties : IDictionary<string, string>
- DescribeSchemesContext
	- DescribeSchemesContext()
	- Accept(IDictionary<string, object> description)
	- Results : IEnumerable<IDictionary<string, object>>
- **IAuthenticationHandler**
	- AuthenticateAsync(**AuthenticateContext** context) : Task
	- ChallengeAsync(**ChallengeContext** context) : Task
	- GetDescriptions(**DescribeSchemesContext** context)
	- SignInAsync(**SignInContext** context) : Task
	- SignOutAsync(**SignOutContext** context) : Task
- **IHttpAuthenticationFeature**
	- Handler : IAuthenticationHandler
	- User : ClaimsPrincipal
- **SignInContext**
	- SignInContext(...)
		- string authenticationScheme, 
		- ClaimsPrincipal principal, 
		- IDictionary<string, string> properties
	- Accept()
	- Accepted : bool
	- AuthenticationScheme : string
	- Principal : ClaimsPrincipal
	- Properties : IDictionary<string, string>
- **SignOutContext**
	- SignOutContext(...)
		- string authenticationScheme, 
		- IDictionary<string, string> properties
	- Accept()
	- Accepted : bool
	- AuthenticationScheme : string
	- Properties : IDictionary<string, string>

# Assembly: Microsoft.AspNetCore.Http.Extensions

## Microsoft.AspNetCore.Http

- HeaderDictionaryTypeExtensions
	- AppendList\<T>(...)
		- this IHeaderDictionary Headers, 
		- string name, 
		- IList\<T> values
	- GetTypedHeaders(this HttpRequest request) : RequestHeaders
	- GetTypedHeaders(this HttpResponse response) : ResponseHeaders
- ResponseExtensions
	- Clear(this HttpResponse response)
- SendFileResponseExtensions
	- SendFileAsync(...) : Task
		- this HttpResponse response, 
		- IFileInfo file, 
		- long offset, 
		- long? count, 
		- [CancellationToken cancellationToken = null]
	- SendFileAsync(...) : Task
		- this HttpResponse response, 
		- IFileInfo file, 
		- [CancellationToken cancellationToken = null]
	- SendFileAsync(...) : Task
		- this HttpResponse response, 
		- string fileName, 
		- long offset, 
		- long? count, 
		- [CancellationToken cancellationToken = null]
	- SendFileAsync(...) : Task
		- this HttpResponse response, 
		- string fileName, 
		- [CancellationToken cancellationToken = null]
- SessionExtensions
	- Get(this ISession session, string key) : byte[]
	- GetInt32(this ISession session, string key) : int?
	- GetString(this ISession session, string key) : string
	- SetInt32(this ISession session, string key, int value)
	- SetString(this ISession session, string key, string value)

## Microsoft.AspNetCore.Http.Extensions

- HttpRequestMultipartExtensions
	- GetMultipartBoundary(this HttpRequest request) : string
- QueryBuilder
	- QueryBuilder()
	- QueryBuilder(IEnumerable<KeyValuePair<string, string>> parameters)
	- Add(string key, string value)
	- Add(string key, IEnumerable\<string> values)
	- Equals(object obj) : bool
	- GetEnumerator() : IEnumerator<KeyValuePair<string, string>>
	- GetHashCode() : int
	- ToQueryString() : QueryString
	- ToString() : string
- StreamCopyOperation
	- CopyToAsync(...) : Task
		- Stream source, 
		- Stream destination, 
		- long? count, 
		- int bufferSize, 
		- CancellationToken cancel
	- CopyToAsync(...) : Task
		- Stream source, 
		- Stream destination, 
		- long? count, 
		- CancellationToken cancel
- UriHelper
	- BuildAbsolute(...) : string
		- string scheme, 
		- HostString host, 
		- [PathString pathBase = null], 
		- [PathString path = null], 
		- [QueryString query = null], 
		- [FragmentString fragment = null]
	- BuildRelative(...) : string
		- [PathString pathBase = null], 
		- [PathString path = null], 
		- [QueryString query = null], 
		- [FragmentString fragment = null]
	- Encode(System.Uri uri) : string
	- FromAbsolute(...)
		- string uri, 
		- out string scheme, 
		- out HostString host, 
		- out PathString path, 
		- out QueryString query, 
		- out FragmentString fragment

## Microsoft.AspNetCore.Http.Headers

- RequestHeaders
	- RequestHeaders(IHeaderDictionary headers)
	- Append(string name, object value)
	- AppendList\<T>(string name, IList\<T> values)
	- Get\<T>(string name) : T
	- GetList\<T>(string name) : IList\<T>
	- Set(string name, object value)
	- SetList<T>(string name, IList\<T> values)
	- Accept : IList\<MediaTypeHeaderValue>
	- AcceptCharset : IList\<StringWithQualityHeaderValue>
	- AcceptEncoding : IList\<StringWithQualityHeaderValue>
	- AcceptLanguage : IList\<StringWithQualityHeaderValue>
	- CacheControl : CacheControlHeaderValue
	- ContentDisposition : ContentDispositionHeaderValue
	- ContentLength : long?
	- ContentRange : ContentRangeHeaderValue
	- ContentType : MediaTypeHeaderValue
	- Cookie : IList\<CookieHeaderValue>
	- Date : System.DateTimeOffset?
	- Expires : System.DateTimeOffset?
	- Headers : IHeaderDictionary
	- Host : HostString
	- IfMatch : IList\<EntityTagHeaderValue>
	- IfModifiedSince : System.DateTimeOffset?
	- IfNoneMatch : IList\<EntityTagHeaderValue>
	- IfRange : RangeConditionHeaderValue
	- IfUnmodifiedSince : System.DateTimeOffset?
	- LastModified : System.DateTimeOffset?
	- Range : RangeItemHeaderValue
	- Referer : Uri
- ResponseHeaders
	- ResponseHeaders(IHeaderDictionary headers)
	- Append(string name, object value)
	- AppendList\<T>(string name, IList\<T> values)
	- Get\<T>(string name) : T
	- GetList\<T>(string name) : IList\<T>
	- Set(string name, object value)
	- SetList<T>(string name, IList\<T> values)
	- CacheControl : CacheControlHeaderValue
	- ContentDisposition : ContentDispositionHeaderValue
	- ContentLength : long?
	- ContentRange : ContentRangeHeaderValue
	- ContentType : MediaTypeHeaderValue
	- Date : System.DateTimeOffset?
	- ETag : EntityTagHeaderValue
	- Expires : System.DateTimeOffset?
	- Headers : IHeaderDictionary
	- LastModified : System.DateTimeOffset?
	- Location : Uri
	- SetCookie : IList\<SetCookieHeaderValue>








