# Using Asp.Net Core Identity without Entity Framework

# Basic Concept

The key thing to understand is that ASP.NET Core Identity is not tied to Entity Framework. Entity Framework is just the default persistence implementation. Identity itself is a collection of services such as:

- **UserManager\<TUser>**
- **SignInManager\<TUser>**
- **RoleManager\<TRole>**
- Password hashing
- Security stamp validation
- Cookie authentication integration

You can absolutely use:

- Your own database access layer
- Your own repository pattern / Dapper / stored procedures / APIs
- SignInManager
- Cookie authentication

without Entity Framework.

### 1. NuGet Packages

For a custom user store with cookie-based authentication, the minimum packages are:

1. Microsoft.AspNetCore.Identity
2. Microsoft.AspNetCore.Authentication.Cookies

### 2. Identity Architecture Without EF

The most important concept is:

1. SignInManager
2. -> UserManager
3. -> IUserStore
4. -> Your Custom Data Access

you build :

1. UserManager
2. -> CustomUserStore
3. -> Dapper / Repository / API

### 3. Create Your User Class

Identity requires a user type.

```csharp
public class ApplicationUser
{
    public string Id { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }
}
```

Many developers inherit from IdentityUser:

```csharp
public class ApplicationUser : IdentityUser
{
}
```

### 4. Create a Custom User Store

Identity needs a store implementation.

At minimum:

```csharp
public class CustomUserStore :
    IUserPasswordStore<ApplicationUser>
{
    private readonly IUserRepository _repository;

    public CustomUserStore(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApplicationUser?> FindByNameAsync(
        string normalizedUserName,
        CancellationToken cancellationToken)
    {
        return await _repository.GetByUserNameAsync(
            normalizedUserName);
    }

    public Task<string?> GetPasswordHashAsync(
        ApplicationUser user,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(
        ApplicationUser user,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(
            !string.IsNullOrEmpty(user.PasswordHash));
    }

    // Other required methods...
}
```

The full implementation usually includes:

1. IUserStore\<ApplicationUser>
2. IUserPasswordStore\<ApplicationUser>
3. IUserEmailStore\<ApplicationUser>

depending on the features you need.

### 5. Configure Authentication

Configure cookie authentication first.

```csharp
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme =
            IdentityConstants.ApplicationScheme;
    })
    .AddCookie(
        IdentityConstants.ApplicationScheme,
        options =>
        {
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";

            options.Cookie.Name = "MyApp.Auth";
        });
``
```

### 6. Register Identity Without Entity Framework

Most tutorials show:

```csharp
.AddEntityFrameworkStores<ApplicationDbContext>()
```

You will NOT use this.

Instead:

```csharp
builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.Password.RequiredLength = 8;
    })
    .AddSignInManager()
    .AddUserStore<CustomUserStore>();
```

Register your store:

```csharp
builder.Services.AddScoped<
    IUserStore<ApplicationUser>,
    CustomUserStore>();
```

### 7. Why SignInManager Works

SignInManager depends on:

- UserManager
- IAuthenticationService
- Cookie Scheme
- ClaimsPrincipal Factory

When you call:

```csharp
await _signInManager.SignInAsync(user, true);
```

the flow is:

1. SignInManager
2. -> Create ClaimsPrincipal
3. -> HttpContext.SignInAsync(...)
4. -> Cookie Authentication Handler
5. -> Authentication Cookie

No database access is needed during this step. The user has already been validated.

### 8. Password Login Flow

Typical login:

```csharp
public async Task<IActionResult> Login(LoginModel model)
{
    var result =
        await _signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                false);

    if (result.Succeeded)
    {
        return Redirect("/");
    }

    return View();
}
```

Internally:

1. PasswordSignInAsync
1. -> Find User
1. -> Verify Password Hash
1. -> Create Principal
1. -> Issue Cookie

The lookup happens through your:

1. CustomUserStore

not Entity Framework.

### 9. Manual Cookie Issuance Using SignInManager

If you already validated credentials yourself:

```charp
var user = await repository.GetUser(username);

await _signInManager.SignInAsync(
    user,
    isPersistent: true);
```

This immediately creates the auth cookie. Equivalent low-level code:

```charp
await HttpContext.SignInAsync(
    CookieAuthenticationDefaults.AuthenticationScheme,
    principal);
```

But when using Identity, prefer:

```csharp
_signInManager.SignInAsync(...)
```

because it also handles:

- security stamps
- claims generation
- Identity conventions

### 10. Required Middleware

In Program.cs:

``` csharp
var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
```

### 11. The Simplest Production-Ready Setup

If your goal is:

- Custom database access
- Cookie authentication
- UserManager
- SignInManager
- No Entity Framework

then use:

```csharp
builder.Services
    .AddIdentityCore<ApplicationUser>()
    .AddSignInManager()
    .AddUserStore<CustomUserStore>();

builder.Services
    .AddAuthentication(
        IdentityConstants.ApplicationScheme)
    .AddCookie(
        IdentityConstants.ApplicationScheme);
```

Then implement:


1. IUserStore\<ApplicationUser>
1. IUserPasswordStore\<ApplicationUser>

and call:

```charp
await _signInManager.PasswordSignInAsync(...)
```

or

```charp
await _signInManager.SignInAsync(...)
```

to issue the authentication cookie.

### Mental Model to Remember

Think of Identity as three separate layers:

- Layer 1
    - Cookie Authentication
    - (Creates/reads cookie)

- Layer 2
    - SignInManager + UserManager
    - (Identity business logic)

- Layer 3
    - UserStore
    - (Get user from DB)

- Layer 4
    - Your Database Access
  - (Dapper/Repository/SPs/API)

  Entity Framework only replaces Layer 4 + part of Layer 3. The rest of Identity continues to work unchanged.

A good next learning step is understanding exactly **which interfaces (IUserStore, IUserPasswordStore, IUserEmailStore, IUserSecurityStampStore) are required for PasswordSignInAsync() to work**, because that determines the minimum custom store implementation. Would you like me to walk through that dependency chain in detail?

# Data Stored in Authentication Cookies

The authentication cookie typically does not contain the user's password, password hash, or the entire user record.

Instead, it contains a serialized and encrypted ClaimsPrincipal, which includes things like:

- User Id
- Username
- Email (optional)
- Roles (optional)
- Claims
- Authentication metadata

Think of it as:

- Cookie
- -> Encrypted Identity Ticket
- -> ClaimsPrincipal
    - -> NameIdentifier = 123
    - -> Name = joe.smith
    - -> Role = Admin

## What Happens During Sign In?

When you call:

```csharp
await _signInManager.SignInAsync(user, true);
```

Identity performs roughly this process:

### Step 1

Create a ClaimsPrincipal

```csharp
var principal =
    await _claimsFactory.CreateAsync(user);
```

Result:

- ClaimsPrincipal
    - Claim(UserId)
    - Claim(UserName)
    - Claim(SecurityStamp)
    - Claim(Role)

### Step 2

Create an Authentication Ticket

- AuthenticationTicket

which contains

1. Principal
1. AuthenticationScheme
1. AuthenticationProperties

### Step 3

Serialize and Encrypt

The ticket is protected using ASP.NET Core Data Protection.

1. AuthenticationTicket
1. -> Protected Bytes
1. -> Cookie Value

The browser receives something that looks like:

- CfDJ8NvmhS8gJQ5...

You cannot read it directly.

## Typical Claims Stored

If you're using Identity defaults, you may see claims such as:

- NameIdentifier
- Name
- Email
- SecurityStamp
- Role

Example:

- ClaimTypes.NameIdentifier = "42"
- ClaimTypes.Name = "joe.smith"
- ClaimTypes.Email = "joe@company.com"
- ClaimTypes.Role = "Administrator"

## What Is NOT Stored?

Identity intentionally does NOT store:

- Password
- Password Hash
- Phone Number
- Address
- Entire User Record
- Database Connection Info

Example user record:

```csharp
public class ApplicationUser
{
    public string Id;
    public string UserName;
    public string Email;
    public string PasswordHash;
    public string Department;
    public string Address;
}
```

Cookie might store only:

- Id
- UserName
- Email
- Roles

but not:

- PasswordHash
- Department
- Address

unless you explicitly add them as claims.

## Where Do Claims Come From?

When using SignInManager, claims are created by:

- IUserClaimsPrincipalFactory\<TUser>

Default implementation:

- UserClaimsPrincipalFactory\<TUser>

You can customize it. Example:

```csharp
public class CustomClaimsFactory
    : UserClaimsPrincipalFactory<ApplicationUser>
{
    public override async Task<ClaimsPrincipal>
        CreateAsync(ApplicationUser user)
    {
        var principal =
            await base.CreateAsync(user);

        var identity =
            (ClaimsIdentity)principal.Identity!;

        identity.AddClaim(
            new Claim("Department", "IT"));

        return principal;
    }
}
```

Now the cookie contains:

- Department = IT

## The Security Stamp Claim

One of the most important Identity features is the security stamp.

Identity stores a claim similar to:

- AspNet.Identity.SecurityStamp

Purpose:

1. User changes password
1. -> Security Stamp changes
1. -> Old cookies become invalid

This allows Identity to force re-authentication without maintaining server-side session state.

## Can I See What's Inside the Cookie?

Not directly in the browser because it is encrypted.

However, after authentication you can inspect the claims:

```csharp
foreach(var claim in User.Claims)
{
    Console.WriteLine(
        $"{claim.Type}: {claim.Value}");
}
```

You'll see:

1. NameIdentifier: 42
1. Name: joe.smith
1. Email: joe@company.com
1. Role: Admin

## Important Misconception

Many developers think:

- Cookie contains User Id only

Not with ASP.NET Core Identity.

Instead:

- Cookie contains the entire ClaimsPrincipal

That means on most requests:

1. Browser sends cookie
1. -> Cookie middleware decrypts ticket
1. -> ClaimsPrincipal recreated
1. -> HttpContext.User available

No database lookup is required.

## What Happens on Every Request?

1. Incoming Request
1. -> Cookie Received
1. -> Cookie Middleware
1. -> Decrypt Ticket
1. -> Rebuild ClaimsPrincipal
1. -> Rebuild ClaimsPrincipal

This is why cookie authentication is fast.

The application already knows:

- UserId
- Username
- Roles
- Claims

## Best Practice for Custom Stores

Since you're implementing a custom IUserStore, store only stable identity information in the cookie:

- User ID
- Username
- Roles
- Permissions/Claims

Avoid:

- Address
- Department hierarchy
- Large profile data
- Frequently changing information

Reason:

If a claim value changes in the database, the cookie still contains the old value until the user signs in again (or security stamp validation refreshes the identity).

# About Security Stamp property

SecurityStamp is not a timestamp, despite its name.

Many developers initially assume it's something like:

- LastUpdated = 2026-09-08 13:45:00

but that's not how ASP.NET Core Identity uses it.

## What SecurityStamp Really Is

Think of it as a version number for a user's security state.

Typically it looks like:

- f4fcdcf0-fb9c-48aa-b311-f6c0e24d5a33

or another random unique value.

Example:

```csharp
user.SecurityStamp =
    "f4fcdcf0-fb9c-48aa-b311-f6c0e24d5a33";
```

It is:

- Random
- Opaque
- Meaningless to humans
- Not a date
- Not a timestamp

## Why Does Identity Need It?

Imagine this scenario:

#### User Signs In

Database:

- UserId = 42
- SecurityStamp = ABC123

Cookie contains:

- UserId = 42
- SecurityStamp = ABC123

The user remains authenticated.

#### User Changes Password

The password changes.

Identity updates:

- SecurityStamp = XYZ999

Database now:

- UserId = 42
- SecurityStamp = XYZ999

But the browser still has:

- UserId = 42
- SecurityStamp = ABC123

#### Next Validation

Identity compares:

- Cookie Stamp     = ABC123
- Database Stamp   = XYZ999

Mismatch.

Result:

- User is signed out
- Cookie rejected
- Must login again

## What Problem Does It Solve?

It solves the problem:
>"How do I invalidate already-issued cookies?"

Cookies are stateless.

Once a cookie is issued:

- Browser owns it

The server cannot physically delete it.

So Identity uses the SecurityStamp as a "revocation marker."

1. Cookie
2. -> stamp = ABC123

If the database stamp changes:

1. Database
1. -> stamp = XYZ999

all old cookies become invalid.

## When Should The Stamp Change?

A good rule:

> Change the stamp whenever existing logins should no longer be trusted.

**Change TimeStamp:**

- Password Changed
- Password Reset
- MFA Enabled
- User Disabled

These are security related.

**Don't change TimeStamp:**

- Change First Name
- Change Address

these are not security related

## How Does Validation Work?

Identity periodically re-checks the stamp.

Typical flow:

1. Request
1. -> Cookie Read
1. -> SecurityStampValidator
    1. -> Load User
    1. -> Compare Statmps  
    1. -> Match ?
        1. -> YES : Continue
        1. -> NO : Sign Out

## Why Doesn't Identity Check Every Request?

Because that would require:

- Database Lookup

on every request.

That defeats one of the main advantages of cookies.

Instead Identity usually validates periodically.

Example:

```csharp
builder.Services.Configure<SecurityStampValidatorOptions>(
    options =>
    {
        options.ValidationInterval =
            TimeSpan.FromMinutes(30);
    });
```

## If You're Building a Custom User Store

If you want full Identity functionality with SignInManager, implement:

- IUserSecurityStampStore\<ApplicationUser>

Example:

```csharp
public class ApplicationUser
{
    public string Id { get; set; }

    public string UserName { get; set; }

    public string PasswordHash { get; set; }

    public string SecurityStamp { get; set; }
}
```

Store implementation:

```csharp
Task<string?> GetSecurityStampAsync(
    ApplicationUser user,
    CancellationToken cancellationToken);

Task SetSecurityStampAsync(
    ApplicationUser user,
    string stamp,
    CancellationToken cancellationToken);
```

Then Identity can automatically invalidate old logins.

# About UserClaimsPrincipalFactory\<TUser>

UserClaimsPrincipalFactory<TUser> gets claims from multiple sources:

1. Directly from the TUser object
1. Through UserManager
1. Potentially through your custom store

So the answer is:

Indirectly, yes. The factory typically calls methods on UserManager, and UserManager then calls your custom store.

## Let's Follow the Actual Flow

Suppose you call:

```csharp
await _signInManager.SignInAsync(user, false);
```

Inside Identity, the flow is roughly:

1. SignInManager
1. -> CreateUserPrincipalAsync(user)
1. -> UserClaimsPrincipalFactory
1. -> Build ClaimsPrincipal
1. -> Authentication Cookie

The interesting part is:

- CreateUserPrincipalAsync(user)

which eventually executes:

- _claimsFactory.CreateAsync(user)

## Claims Taken Directly From TUser

The default implementation adds some claims from the user object itself.

Conceptually:

```csharp
identity.AddClaim(
    new Claim(
        ClaimTypes.NameIdentifier,
        await UserManager.GetUserIdAsync(user)));

identity.AddClaim(
    new Claim(
        ClaimTypes.Name,
        await UserManager.GetUserNameAsync(user)));
```

Notice something important:

It does **not** directly do:

- user.id

Instead it calls:

- UserManager.GetUserIdAsync()

## Why Use UserManager?

Because Identity doesn't know what your user object looks like.

You might have:

```csharp
public class ApplicationUser
{
    public int EmployeeNumber { get; set; }
}
```

Another application might have:

```csharp
public class ApplicationUser
{
    public Guid UserKey { get; set; }
}
```

Identity relies on its store interfaces rather than hardcoded properties.

## Does UserManager Call My Store?

Yes.

Example:

```csharp
await UserManager.GetUserIdAsync(user);
```

becomes

```csharp
_store.GetUserIdAsync(user);
```

where **_store** is your implementation of:

- IUserStore\<ApplicationUser>

So

1. ClaimsFactory
1. -> UserManager
1. -> CustomUserStore

## What About Roles?

Suppose the factory needs roles.

The default implementation does something like:

```csharp
var roles =
    await UserManager.GetRolesAsync(user);
```

Then

```csharp
foreach(var role in roles)
{
    identity.AddClaim(
        new Claim(
            ClaimTypes.Role,
            role));
}
```

Who supplies the roles?

Your store:

- IUserRoleStore\<ApplicationUser>

Flow:

1. ClaimsFactory
1. -> UserManager.GetRolesAsync()
1. -> CustomUserStore
1. -> Database

## What About Custom Claims?

This is another place where the factory indirectly calls your store.

The default factory checks:

- IUserClaimStore\<TUser>

through UserManager.

Conceptually:

```csharp
var claims =
    await UserManager.GetClaimsAsync(user);
```

which becomes:

1. UserManager
1. -> IUserClaimStore
1. -> Your DB

and those claims are added to the principal.

## What If My User Object Already Contains Everything?

Suppose your repository loads:

```csharp
public class ApplicationUser
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Department { get; set; }
    public string Region { get; set; }
}
```

The default factory will not automatically create claims for Department and Region.

It only knows about standard Identity claims.

You must extend the factory:

```csharp
public class CustomClaimsFactory
    : UserClaimsPrincipalFactory<ApplicationUser>
{
    public override async Task<ClaimsPrincipal>
        CreateAsync(ApplicationUser user)
    {
        var principal =
            await base.CreateAsync(user);

        var identity =
            (ClaimsIdentity)principal.Identity!;

        identity.AddClaim(
            new Claim(
                "Department",
                user.Department));

        identity.AddClaim(
            new Claim(
                "Region",
                user.Region));

        return principal;
    }
}
```

In this case:

1. ClaimsFactory
1. -> uses user.Department directly
1. -> no store call needed

## Performance Insight

A subtle but important point:

Consider this login flow:

```csharp
var user =
    await repository.GetUserAsync(username);

await _signInManager.SignInAsync(user, false);
```

You already have a fully loaded user.

However, the default UserClaimsPrincipalFactory may still call:

```csharp
UserManager.GetRolesAsync(user)
UserManager.GetClaimsAsync(user)
```

which can trigger additional database queries through your store.

That's why many custom Identity implementations create a custom factory that batches data efficiently.

## Mental Model

Think of UserClaimsPrincipalFactory as a claims assembler:

1. TUser
1. -> UserClaimsPrincipalFactory
    1. -> UserManager
        1. -> Custom Store
        1. -> Database
    1. -> User Properties

The factory does not usually talk to your database directly.

Instead:

1. ClaimsFactory
1. -> UserManager
1. -> Custom Store
1. -> Database

This separation is one of the core architectural patterns of ASP.NET Core Identity.

## Learning Check

Suppose your custom store implements only:

- IUserStore\<ApplicationUser>
- IUserPasswordStore\<ApplicationUser>

and **does not** implement:

- IUserRoleStore\<ApplicationUser>
- IUserClaimStore\<ApplicationUser>

What claims would you expect the authentication cookie to contain after SignInManager.SignInAsync()? Try to reason it out before looking at the source code. It is a great exercise for understanding the minimum Identity pipeline.

# Configure Custom Store for Authentication

There are two separate DI registrations:

## 1. Cookie Authentication

- Responsible for:
- Reading cookies
- Writing cookies
- Decrypting cookies
- Creating HttpContext.User

```csharp
builder.Services
    .AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme);
```

## 2. Identity Services

Responsible for:

- **UserManager**
- **SignInManager**
- **UserClaimsPrincipalFactory**
- Password validation
- Security stamp validation
- Calling your custom store

```csharp
builder.Services
    .AddIdentityCore<ApplicationUser>()
    .AddSignInManager();
```

## Registering a Custom Store

If your store implements all interfaces:

```csharp
public class CustomUserStore :
    IUserStore<ApplicationUser>,
    IUserPasswordStore<ApplicationUser>,
    IUserEmailStore<ApplicationUser>,
    IUserRoleStore<ApplicationUser>,
    IUserClaimStore<ApplicationUser>,
    IUserSecurityStampStore<ApplicationUser>
{
}
```

the cleanest registration is:

```csharp
builder.Services.AddScoped<CustomUserStore>();

builder.Services.AddScoped<
    IUserStore<ApplicationUser>,
    CustomUserStore>();
```

Then:

```csharp
builder.Services
    .AddIdentityCore<ApplicationUser>()
    .AddSignInManager()
    .AddUserStore<CustomUserStore>();
```

**AddUserStore\<CustomUserStore>()** allows Identity to resolve the store when UserManager needs it.

## What Happens at Runtime?

When you call:

```csharp
await _signInManager.PasswordSignInAsync(
    username,
    password,
    true,
    false);
```

Identity uses DI to build:

1. SignInManager
1. -> UserManager
1. -> CustomUserStore

and later:

1. SignInManager
1. -> UserClaimsPrincipalFactory
1. -> Cookie Handler

The cookie handler never knows your store exists.

The store is only involved when Identity needs data.

## Minimal Working Example

```csharp
builder.Services
    .AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme);

builder.Services.AddScoped<CustomUserStore>();

builder.Services
    .AddIdentityCore<ApplicationUser>()
    .AddSignInManager()
    .AddUserStore<CustomUserStore>();

builder.Services.AddScoped<
    IUserStore<ApplicationUser>,
    CustomUserStore>();
```

Middleware:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

## One Advanced Detail

Since you're implementing all standard store interfaces, Identity will attempt to cast the registered IUserStore\<ApplicationUser> into:

- IUserPasswordStore\<ApplicationUser>
- IUserRoleStore\<ApplicationUser>
- IUserClaimStore\<ApplicationUser>
- IUserSecurityStampStore\<ApplicationUser>

You **do not need** separate registrations like:

- IUserRoleStore\<ApplicationUser>
- IUserClaimStore\<ApplicationUser>

provided that the single registered store instance implements them.

For example:

```csharp
public class CustomUserStore :
    IUserStore<ApplicationUser>,
    IUserRoleStore<ApplicationUser>
{
}
```

is enough. Identity obtains those interfaces from the same store instance internally.

## Learning takeaway

Try to remember this separation:

1. Cookie Authentication
    1. ->  Handles cookies

1. Identity
    1. -> UserManager
    1. -> SignInManager
    1. -> Claims Factory

1. Custom Store
    1. -> Loads users, roles, claims, stamps

The **cookie scheme is not connected directly to your custom store**. The connection is:

1. Cookie
1. -> SignInManager
1. -> UserManager
1. -> CustomUserStore

A good next learning step is understanding **why AddIdentityCore() alone is not enough for SignInManager and cookie authentication**, and what additional services **AddIdentity()** adds behind the scenes. That's often where custom Identity implementations run into DI configuration problems.