# Learning ASP.NET Core Authentication

The purpose of this project is to document and to learn the authentication and authorization mechanisms in ASP.NET Core. This project will serve as a reference for implementing secure authentication and authorization in .NET applications. The security implementation is divided into 3 design layers: Identity, Authentication, and Authorization.

# Identity Layer

## Data Stores

This layer provides convenience classes and interfaces to implement data stores of user data. These classes and interfaces provide best practices for storing and retrieving common user data needed to implement authentication and authorization.

Some base classes and interface are:

- IRoleStore
- IUserStore
- RoleManager
- UserManager
- SignInManager

Once registered, these classes are available from the DI container.

Alternatively, the Microsoft.AspNetCore.Identity.EntityFrameworkCore nuget provides a full implementation of identity data stores using a relational database and EntityFrameworkCore for database data access.

## User Identity

The identity of the user is implemented as ClaimsPrincipal.  The user identity is stored in the HttpContext.User property for the purpose of authentication. Using the Identity framework, you can validate the user login information and retrieve the user data from the data store to construct the ClaimsPrincipal. 

After successful login, an encrypted HTTP Cookie or a signed JWT access token, that stores the authenticated user information, is sent back to the client application. On subsequent request, the client application can send the cookie or token back to server. An AuthenticationScheme is configured to reconstructed the cookie/token back to the ClaimsPrincipal.

The UserManager and SignInManager classes provide the core logic of Sign-In and Log-Out functionalities.

## Multiple Identities

A ClaimsPrincipal can have one or more ClaimsIdentities. Each ClaimsIdentity contains user information provided by a specific identity provider (ClaimsIdentity.AuthenticationType) such as your own application, Google, or Microsoft. A ClaimsIdentity without a provider information (AuthenticationType) is considered as unauthenticated anonymous user.

## Claim

Claims are any information about users provided and verified by various identity providers (OriginalIssuer). ClaimTypes enum specifies various standard claim types. The IClaimsTransformation converts various claim types to application-specific claim types.

# Authentication Layer

There are many activities in the authentication process such as:
- Sign-In (SignInManager)
- Log-Out (SignInManager)
- Redirect user to your app log-in web page (Challenge behavior)
- Redirect user to an external identity provider log-in web page (Challenge behavior)
- Redirect user to an unauthorized web page (Forbid behavior)
- Generate an HTTP Cookie (SignInManager)
- Generate an JWT access token
- Reconstruct ClaimsPrincipal from validated HTTP Cookie or JWT access token (Authenticate behavior)

## Authentication Scheme

In ASP.NET Core, an AuthenticationScheme encapsulates the following logics:

- Define all the redirect URI (Challenge / Forbid behavior)
- Validate HTTP Cookie or JWT access token (Authenticate behavior)
- Parse the HTTP Cookie or JWT access token to recreate the ClaimsPrincipal (Authenticate behavior)

Each AuthenticationScheme has a handler of type IAuthenticationHandler that implements these logics.

.NET provides these AuthenticationSchemes:

- CookieExtensions.AddCookie
- BearerTokenExtensions. AddBearerToken

Other external identity providers (Microsoft, Google) provide their own Nuget libraries

## Configuration

Multiple AuthenticationScheme can be defined in an application and each is identified by a unique Name. For example, the Identity framework configures 4 cookie-based AuthenticationScheme to facilitate the Sign-In logic with these default Name:

- IdentityConstants
	- ApplicationScheme
	- ExternalScheme
	- TwoFactorRememberMeScheme
	- TwoFactorUserIdScheme

An application is configured using AuthenticationOptions to specify the default authenticationScheme for various authentication behaviors:

- DefaultAuthenticateScheme
- DefaultChallengeScheme
- DefaultForbidScheme
- DefaultScheme
- DefaultSignInScheme 
- DefaultSignOutScheme 

The app.UseAuthentication() middleware in ASP.NET Core application will call the handler of the DefaultAuthenticateScheme to reconstruct the ClaimsPrincipal and set the HttpContext.User.

## Service Classes

The AuthenticationHttpContextExtensions class provides the main logic to utilize these AuthenticationScheme. Some methods allow override the configured default AuthenticationScheme.

Other service classes are registered with DI container to be available for use:

- AuthenticationService : IAuthenticationService
- AuthenticationSchemeProvider : IAuthenticationSchemeProvider

# Authorization

Authorization is about restricting Access to resources based on information (Claim) stored within the current user (ClaimsPrincipal). 

To manage access declaratively

- AuthorizeAttribute 
- AllowAnonymousAttribute .

To manage access dynamically, use

- properties, methods, and extension methods of the ClaimsPrincipal.
- DefaultAuthorizationService : IAuthorizationService (from DI container)

## Authorization Policy

Access management can be organized into various AuthorizationPolicy. An application can create a collection of AuthorizationPolicy to specify access criteria.

A policy (AuthorizationPolicyBuilder) can specify
- requirement for a certain set of claims
- complex logic based on claims and/or about access to specific resources
	- IAuthorizationRequirement and 
	- AuthorizationHandler<TRequirement, TResource> : IAuthorizationHandler
- dynamic logic using assertion (RequireAssertion) 

## Authorization Requirement

IAuthorizationRequirement is a marker interface that represent certain criteria and/or threshold. An application can create a collection of custom AuthorizationRequirement. A policy can specify these requirements to be evaluated. 

Create a custom class based on IAuthorizationRequirement and AuthorizeAttribute to accept additional parameters needed for the evaluation.

IAuthorizationHandler implements the custom logic to evaluate the user identity information against a specific IAuthorizationRequirement. IAuthorizationHandler is registered with the DI container to make it available to be called by the authorization framework.

.NET provide a set of AuthorizationRequirement classes with semantic meaning (Microsoft.AspNetCore.Authorization.Infrastructure). 

## Services

The following service classes are registered with the DI container for use in application:

- DefaultAuthorizationService : IAuthorizationService
- DefaultAuthorizationPolicyProvider : IAuthorizationPolicyProvider

# Examples

The following classes provide examples about these 3 layers:

- Security.ConsoleApp
	- [Program.cs](./src/Security.ConsoleApp/Program.cs)
- Security.WebAppLib
	- [AppService.cs](./src/Security.WebAppLib/AppService.cs)
- Security.WebAppLib.Example
	- [ExploreIdentity.cs](./src/Security.WebAppLib/Example/ExploreIdentity.cs)
	- [ExploreAuthentication.cs](./src/Security.WebAppLib/Example/ExploreAuthentication.cs)
	- [AuthorizationService.cs](./src/Security.WebAppLib/Example/AuthorizationService.cs)
	- [ExploreAuthorization.cs](./src/Security.WebAppLib/Example/ExploreAuthorization.cs)

# Additional Documents

**.NET Library API**

- Security.WebAppLib
	- [Api_Identity.md](./src/Security.WebAppLib/Docs/Api_Identity.md)
	- [Api_Authentication.md](./src/Security.WebAppLib/Docs/Api_Authentication.md)
	- [Api_Authorization.md](./src/Security.WebAppLib/Docs/Api_Authorization.md)
	- [Api_Web.md](./src/Security.WebAppLib/Docs/Api_Web.md)

**MSDN Articles**

- Security.WebAppLib
	- [Doc_Identity.md](./src/Security.WebAppLib/Docs/Doc_Identity.md)
	- [Doc_Authentication.md](./src/Security.WebAppLib/Docs/Doc_Authentication.md)
	- [Doc_Authorization.md](./src/Security.WebAppLib/Docs/Doc_Authorization.md)
	- [Doc_Passkey.md](./src/Security.WebAppLib/Docs/Doc_Passkey.md)

**Copilot Explanation**

- Security.WebAppLib
	- [Copilot_Prompt.md](./src/Security.WebAppLib/Docs/Copilot_Prompt.md)
	- [Copilot_Learn.md](./src/Security.WebAppLib/Docs/Copilot_Learn.md)