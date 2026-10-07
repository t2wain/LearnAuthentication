# Overview of ASP.NET Core authentication

In ASP.NET Core, authentication is handled by the authentication service, **IAuthenticationService**, which is used by authentication **middleware**. The authentication service uses **registered authentication handlers** to complete authentication-related actions. Examples of **authentication-related actions** include:

- Authenticating a user.
- Responding when an unauthenticated user tries to access a restricted resource.

The **registered authentication handlers** and their **configuration options** are called
"**schemes**".

Authentication schemes are specified by registering authentication services in Program.cs :

- By calling a **scheme-specific extension method after a call to AddAuthentication**, such as **AddJwtBearer** or **AddCookie**. These extension methods use **AuthenticationBuilder.AddScheme** to register schemes with appropriate settings.
- Less commonly, by calling AuthenticationBuilder.AddScheme directly.
 
For example, the following code **registers authentication services and handlers** for
cookie and JWT bearer authentication schemes:

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
		options => builder.Configuration.Bind("JwtSettings", options))
	.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
		options => builder.Configuration.Bind("CookieSettings", options));
```

The AddAuthentication parameter JwtBearerDefaults.AuthenticationScheme is the **name of the scheme to use by default when a specific scheme isn't requested**.

If **multiple schemes** are used, **authorization policies** (or **authorization attributes**) can **specify the authentication scheme** (or schemes) they depend on to authenticate the user. In the example above, the cookie authentication scheme could be used by specifying its name (CookieAuthenticationDefaults.AuthenticationScheme by default, though a different name could be provided when calling AddCookie).

In some cases, the call to AddAuthentication is automatically made by other extension methods. For example, when using ASP.NET Core Identity, **AddAuthentication is called internally**.

The Authentication middleware is added in Program.cs by calling **UseAuthentication**. Calling UseAuthentication registers the **middleware that uses the previously registered authentication schemes**. Call UseAuthentication before any middleware that dependson users being authenticated.

## Authentication concepts

Authentication is responsible for providing the **ClaimsPrincipal** for **authorization** to make permission decisions against. There are multiple authentication scheme approaches **to select which authentication handler is responsible for generating the correct set of claims**:

- Authentication **scheme**
- The **default** authentication scheme, discussed in the next two sections.
- **Directly set HttpContext.User**

When there is only a **single** authentication scheme registered, it becomes the **default** scheme. If **multiple** schemes are registered and the **default scheme isn't specified, a scheme must be specified in the authorize attribute, otherwise, the following error is thrown:**

> InvalidOperationException: No authenticationScheme was specified, and there was no DefaultAuthenticateScheme found. The default schemes can be set using either AddAuthentication(string defaultScheme) or AddAuthentication(Action\<AuthenticationOptions> configureOptions).

#### DefaultScheme

When there is only a **single** authentication scheme registered, the single authenticationscheme:

- Is automatically used as the **DefaultScheme**
- Eliminates the need to **specify** the DefaultScheme in AddAuthentication(IServiceCollection) or AddAuthenticationCore(IServiceCollection)

**To disable automatically using the single authentication scheme as the DefaultScheme**, call AppContext.SetSwitch("Microsoft.AspNetCore.Authentication.SuppressAutoDefaultScheme").

## Authentication scheme

The **authentication scheme** can select which **authentication handler** is responsible for **generating the correct set of claims**. For more information, see **Authorize with a specific scheme.**

An authentication scheme is a **name** that **corresponds to**:

- An authentication **handler**.
- **Options** for configuring that specific instance of the handler.

Schemes are useful as a mechanism for referring to the **authentication**, **challenge**, and **forbid behaviors** of the associated handler. For example, an **authorization policy** can use **scheme names** to specify which authentication scheme (or schemes) should be used **to authenticate the user**. When configuring authentication, it's common to specify the **default** authentication scheme. **The default scheme is used unless a resource requests aspecific scheme**. It's also possible to:

- **Specify different default schemes to use for authenticate, challenge, and forbid actions**.
- **Combine multiple schemes into one using policy schemes**.

## Authentication handler

An authentication handler:

- Is a type that **implements the behavior** of a **scheme**.
- Is derived from **IAuthenticationHandler** or **AuthenticationHandler\<TOptions>**.
- **Has the primary responsibility to authenticate users**.

Based on the authentication scheme's configuration and the **incoming request context**, authentication handlers:

- **Construct AuthenticationTicket** objects representing the user's identity if authentication is successful.
- Return 'no result' or 'failure' if authentication is unsuccessful.
- Have methods for **challenge** and **forbid** actions for when users attempt to access resources:
	- They're unauthorized to access (forbid).
	- When they're unauthenticated (challenge).

#### RemoteAuthenticationHandler\<TOptions> vs AuthenticationHandler\<TOptions>

**RemoteAuthenticationHandler\<TOptions>** is the class for authentication that requires a remote authentication step. When the remote authentication step is finished, the handler calls back to the **CallbackPath** set by the handler. The handler **finishes the authentication step** using the information passed to the **HandleRemoteAuthenticateAsync** callback path. OAuth 2.0 and OIDC both use this pattern. JWT and cookies don't since they can directly use the bearer header and cookie to authenticate. The **remotely hosted provider** in this case:

- Is the **authentication provider**.
- Examples include Facebook, Twitter, Google, Microsoft, and any other OIDC provider that handles authenticating users using the handlers mechanism.

## Authenticate Action

An authentication scheme's **authenticate action** is responsible for **constructing** the user's identity based on **request context**. It returns an **AuthenticateResult** indicating whether authentication was successful and, if so, the user's identity in an authentication ticket. See **AuthenticateAsync**. Authenticate examples include:

- A cookie authentication scheme **constructing** the user's identity **from cookies**.
- A JWT bearer scheme **deserializing and validating a JWT bearer token** to **construct** the user's identity.

## Challenge Action

An authentication challenge is **invoked by Authorization** when an unauthenticated user requests an endpoint that requires authentication. An authentication challenge is issued, for example, when an anonymous user requests a restricted resource or follows a login link. **Authorization invokes a challenge** using the specified authentication scheme(s), or the default if none is specified. See **ChallengeAsync**. Authentication challenge examples include:

- A cookie authentication scheme **redirecting** the user to a login page.
- A JWT bearer scheme **returning a 401 result** with a www-authenticate: bearer header.

A challenge action should let the user know **what authentication mechanism to use** to access the requested resource.

## Forbid Action

An authentication scheme's **forbid action is called by Authorization** when anauthenticated user attempts to access a resource they're not permitted to access. See **ForbidAsync**. Authentication forbid examples include:

- A cookie authentication scheme **redirecting** the user to a page indicating accesswas forbidden.
- A JWT bearer scheme **returning a 403 result**.
- A **custom** authentication scheme **redirecting** to a page where the **user can request access** to the resource.

A forbid action can let the user know:

- They're authenticated.
- They're not permitted to access the requested resource.

See the following links for differences between challenge and forbid:

- Challenge and forbid with an operational resource handler.
- Differences between challenge and forbid.