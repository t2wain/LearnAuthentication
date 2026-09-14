# Nuget Packages

- Microsoft.AspNetCore.Identity
	- Microsoft.Extensions.Identity.Core
	- Microsoft.AspNetCore.Authentication.Cookies
		- Microsoft.AspNetCore.Authentication
			- Microsoft.AspNetCore.Authentication.Core
				- Microsoft.AspNetCore.Authentication.Abstractions
- Microsoft.Extensions.Identity.Stores
	- Microsoft.Extensions.Identity.Core

# Assembly : System.Security.Claims

## System.Security.Claims

- Claim
	- Claim(string type, string value)
	- Claim(string type, string value, string valueType)
	- Claim(string type, string value, string valueType, string issuer)
	- Claim(...)
		- string type, 
		- string value, 
		- string valueType, 
		- string issuer,
		- string originalIssuer
	- Claim(...)
		- string type, 
		- string value, 
		- string valueType, 
		- string issuer, 
		- string originalIssuer, 
		- ClaimsIdentity subject
	- Type : string
	- Value 
	- ValueType 
	- Issuer 
	- OriginalIssuer 
	- Properties : IDictionary\<string, string>
	- Subject : ClaimsIdentity

# Assembly : Microsoft.Extensions.Identity.Core

## Microsoft.Extensions.DependencyInjection

```csharp
Services.Configure<IdentityOptions>(options =>
{
	// Password settings.
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequireUppercase = true;
	options.Password.RequiredLength = 6;
	options.Password.RequiredUniqueChars = 1;

	// Lockout settings.
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	options.Lockout.MaxFailedAccessAttempts = 5;
	options.Lockout.AllowedForNewUsers = true;

	// User settings.
	options.User.AllowedUserNameCharacters =
	"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
	options.User.RequireUniqueEmail = false;
});

// Identity services
IdentityBuilder identityBuilder = services
    //.AddIdentityCore<IAppUser>()
    .AddIdentityCore<IAppUser>((IdentityOptions setupOption) => {  })
    .AddSignInManager()
    .AddUserStore<UserStore>();
```

- IdentityServiceCollectionExtensions (IServiceCollection)
	- AddIdentityCore\<TUser>() : **IdentityBuilder** 
	- AddIdentityCore\<TUser>(Action\<**IdentityOptions**> setupAction) : **IdentityBuilder**

## Microsoft.AspNetCore.Identity

- IRoleClaimStore\<TRole>
	- GetClaimsAsync(TRole role) : Task\<IList\<Claim>>
- IRoleStore\<TRole>
	- GetNormalizedRoleNameAsync(TRole role) : Task\<string>
	- GetRoleIdAsync(TRole role) : Task\<string>
	- GetRoleNameAsync(TRole role) : Task\<string>
- **IUserClaimsPrincipalFactory\<TUser>**
- IUserClaimStore\<TUser>
	- GetClaimsAsync(TUser user) : Task\<IList\<Claim>>
- IUserPasswordStore\<TUser>
- IUserRoleStore\<TUser>
	- GetRolesAsync(TUser user) : Task\<IList\<string>>
	- IsInRoleAsync(TUser user, string roleName) : Task\<bool>
- IUserSecurityStampStore\<TUser>
	- GetSecurityStampAsync(TUser user) : Task\<string>
- IUserStore\<TUser>
	- FindByIdAsync(string userId) : Task\<TUser>
	- FindByNameAsync(string normalizedUserName) : Task\<TUser>
	- GetNormalizedUserNameAsync(TUser user) : Task\<string>
	- GetUserIdAsync(TUser user) : Task\<string>
	- GetUserNameAsync(TUser user) : Task\<string>
- **IdentityBuilder**
	- **AddClaimsPrincipalFactory\<TFactory>()**
	- AddErrorDescriber\<TDescriber>()
	- AddPasswordValidator\<TValidator>()
	- AddPersonalDataProtection<TProtector, TKeyRing>()
	- **AddRoleManager\<TRoleManager>()**
	- AddRoles\<TRole>()
	- **AddRoleStore\<TStore>()**
	- AddRoleValidator\<TRole>()
	- AddTokenProvider(string providerName, System.Type provider)
	- AddTokenProvider\<TProvider>(string providerName)
	- AddUserConfirmation\<TUserConfirmation>()
	- **AddUserManager\<TUserManager>()**
	- **AddUserStore\<TStore>()**
	- AddUserValidator\<TValidator>()
	- RoleType : System.Type
	- UserType : System.Type
- **IdentityOptions**
	- ClaimsIdentity : ClaimsIdentityOptions
	- Lockout : LockoutOptions
	- Password : PasswordOptions
	- SignIn : SignInOptions
	- Stores : StoreOptions
	- Tokens : TokenOptions
	- User : UserOptions
- IdentityResult
	- static Failed(params Microsoft.AspNetCore.Identity.IdentityError[] errors) : IdentityResult 
	- Errors : IEnumerable\<IdentityError>
	- Succeeded : bool
	- static Success : IdentityResult
- PersonalDataAttribute
- **RoleManager\<TRole>**
	- AddClaimAsync(TRole role, Claim claim) : Task\<IdentityResult>
	- GetClaimsAsync(TRole role) : Task\<IList\<Claim>>
	- GetRoleIdAsync(TRole role) : Task\<string>
	- GetRoleNameAsync(TRole role) : Task\<string>
	- RemoveClaimAsync(TRole role, Claim claim) : Task\<IdentityResult>
	- RoleManager(...)
		- **IRoleStore\<TRole>** store, 
		- IEnumerable<IRoleValidator\<TRole>> roleValidators, 
		- ILookupNormalizer keyNormalizer, 
		- IdentityErrorDescriber errors, 
		- ILogger<RoleManager\<TRole>> logger)
	- SupportsRoleClaims : bool
- SignInResult
- **UserClaimsPrincipalFactory<TUser, TRole>**
- **UserClaimsPrincipalFactory\<TUser>**
- UserLoginInfo
- **UserManager\<TUser>**
	- GetClaimsAsync(TUser user)
	- GetRolesAsync(TUser user)
	- GetSecurityStampAsync(TUser user)
	- GetUserAsync(ClaimsPrincipal principal)
	- GetUserId(ClaimsPrincipal principal)
	- GetUserIdAsync(TUser user)
	- GetUserName(ClaimsPrincipal principal)
	- GetUserNameAsync(TUser user)
	- GetUsersInRoleAsync(string roleName)
	- IsInRoleAsync(TUser user, string role)
	- NormalizeEmail(string email)
	- NormalizeName(string name)
	- ErrorDescriber : IdentityErrorDescriber
	- Options : IdentityOptions
	- SupportsQueryableUsers : bool
	- SupportsUserClaim : bool
	- SupportsUserEmail : bool
	- SupportsUserLockout : bool
	- SupportsUserLogin : bool
	- SupportsUserPasskey : bool
	- SupportsUserPassword : bool
	- SupportsUserPhoneNumber : bool
	- SupportsUserRole : bool
	- SupportsUserSecurityStamp : bool
	- SupportsUserTwoFactor : bool
	- SupportsUserTwoFactorRecoveryCodes : bool
	- **UserManager(...)**
		- **IUserStore\<TUser>** store, 
		- IOptions\<IdentityOptions> optionsAccessor, 
		- IPasswordHasher\<TUser> passwordHasher, 
		- IEnumerable<IUserValidator\<TUser>> userValidators,
		- IEnumerable<IPasswordValidator\<TUser>> passwordValidators, 
		- ILookupNormalizer keyNormalizer, 
		- IdentityErrorDescriber errors, 
		- IServiceProvider services, 
		- ILogger<UserManager\<TUser>> logger

# Assembly : Microsoft.AspNetCore.Identity

## Microsoft.Extensions.DependencyInjection

- IdentityServiceCollectionExtensions
	- **AddIdentity<TUser, TRole>()**
	- AddIdentity<TUser, TRole>(Action\<IdentityOptions> setupAction)
	- **ConfigureApplicationCookie**(Action\<**CookieAuthenticationOptions**> configure)

```csharp
Services.ConfigureApplicationCookie(options =>
{
	// Cookie settings
	options.Cookie.HttpOnly = true;
	options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
	options.LoginPath = "/Identity/Account/Login";
	options.AccessDeniedPath = "/Identity/Account/AccessDenied";
	options.SlidingExpiration = true;
});
```

## Microsoft.AspNetCore.Identity

- AspNetRoleManager\<TRole> : RoleManager\<TRole>
- AspNetUserManager\<TUser> : UserManager\<TUser>
- DataProtectionTokenProviderOptions
- DataProtectorTokenProvider\<TUser>
- ExternalLoginInfo : UserLoginInfo
- IdentityBuilderExtensions
	- **AddSignInManager()**
	- **AddSignInManager\<TSignInManager>()**

**IdentityConstants** is used to specify the authentication schemes.

- **IdentityConstants**
	- **ApplicationScheme**
	- ExternalScheme
	- TwoFactorRememberMeScheme
	- TwoFactorUserIdScheme
- IdentityCookieAuthenticationBuilderExtensions(this **AuthenticationBuilder**)
	- AddApplicationCookie() : OptionsBuilder\<CookieAuthenticationOptions>
	- AddExternalCookie() : OptionsBuilder\<CookieAuthenticationOptions>
	- AddIdentityCookies() : IdentityCookiesBuilder
	- AddIdentityCookies(Action\<**IdentityCookiesBuilder**> configureCookies) : IdentityCookiesBuilder
	- AddTwoFactorRememberMeCookie() : OptionsBuilder\<CookieAuthenticationOptions>
	- AddTwoFactorUserIdCookie() : OptionsBuilder\<CookieAuthenticationOptions>
- IdentityCookiesBuilder
	- ApplicationCookie : OptionsBuilder\<CookieAuthenticationOptions>
- **SignInManager\<TUser>**
	- CreateUserPrincipalAsync(TUser user) : Task\<ClaimsPrincipal>
	- IsSignedIn(ClaimsPrincipal principal)
	- RefreshSignInAsync(TUser user)
	- **SignInAsync()**
		- TUser user, 
		- bool isPersistent, 
		- [string authenticationMethod = null]
	- **SignInAsync()**
		- TUser user, 
		- **AuthenticationProperties** authenticationProperties, 
		- [string **authenticationMethod** = null]
	- **SignInManager(...)**
		- **UserManager\<TUser>** userManager, 
		- IHttpContextAccessor contextAccessor, 
		- **IUserClaimsPrincipalFactory\<TUser>** claimsFactory, 
		- IOptions\<IdentityOptions> optionsAccessor, 
		- ILogger<SignInManager\<TUser>> logger, 
		- IAuthenticationSchemeProvider schemes
	- SignOutAsync()
	- ValidateSecurityStampAsync(ClaimsPrincipal principal) : Task\<TUser> 
	- ValidateSecurityStampAsync(TUser user, string securityStamp) : Task\<bool>
	- ClaimsFactory : IUserClaimsPrincipalFactory\<TUser>
	- Context : HttpContext
	- Options : IdentityOptions
	- UserManager : UserManager\<TUser>

