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

- AuthenticatorTokenProvider\<TUser> : IUserTwoFactorTokenProvider\<TUser>
- ClaimsIdentityOptions
- DefaultPersonalDataProtector : IPersonalDataProtector
- DefaultUserConfirmation\<TUser> : IUserConfirmation\<TUser>
- EmailTokenProvider\<TUser> : TotpSecurityStampBasedTokenProvider\<TUser>
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
- IdentityError
	- IdentityError()
	- Code : string
	- Description : string
- IdentityErrorDescriber
	- dentityErrorDescriber()
	- ConcurrencyFailure() : IdentityError
	- DefaultError() : IdentityError
	- DuplicateEmail(string email)
	- DuplicateRoleName(string role)
	- DuplicateUserName(string userName)
	- InvalidEmail(string email)
	- InvalidRoleName(string role)
	- InvalidToken()
	- InvalidUserName(string userName)
	- LoginAlreadyAssociated()
	- PasswordMismatch()
	- PasswordRequiresDigit()
	- PasswordRequiresLower()
	- PasswordRequiresNonAlphanumeric()
	- PasswordRequiresUniqueChars(int uniqueChars)
	- PasswordRequiresUpper()
	- PasswordTooShort(int length)
	- RecoveryCodeRedemptionFailed()
	- UserAlreadyHasPassword()
	- UserAlreadyInRole(string role)
	- UserLockoutNotEnabled()
	- UserNotInRole(string role)
- **IdentityOptions**
	- ClaimsIdentity : ClaimsIdentityOptions
	- Lockout : LockoutOptions
	- Password : PasswordOptions
	- SignIn : SignInOptions
	- Stores : StoreOptions
	- Tokens : TokenOptions
	- User : UserOptions
- IdentityResult
	- static Failed(params **IdentityError**[] errors) : IdentityResult 
	- Errors : IEnumerable\<**IdentityError**>
	- Succeeded : bool
	- static Success : IdentityResult
- IdentitySchemaVersions
- IRoleClaimStore\<TRole>
	- GetClaimsAsync(TRole role) : Task\<IList\<Claim>>
- IRoleStore\<TRole>
	- GetNormalizedRoleNameAsync(TRole role) : Task\<string>
	- GetRoleIdAsync(TRole role) : Task\<string>
	- GetRoleNameAsync(TRole role) : Task\<string>
- ILookupNormalizer
- ILookupProtector
- ILookupProtectorKeyRing
- IPasswordHasher\<TUser>
- IPasswordValidator\<TUser>
- IPersonalDataProtector
- IProtectedUserStore\<TUser>
- IQueryableRoleStore\<TRole>
- IQueryableUserStore\<TUser>
- IRoleClaimStore\<TRole>
- IRoleStore\<TRole>
- IRoleValidator\<TRole>
- IUserAuthenticationTokenStore\<TUser>
- IUserAuthenticatorKeyStore\<TUser>
- **IUserClaimsPrincipalFactory\<TUser>**
- IUserClaimStore\<TUser>
	- GetClaimsAsync(TUser user) : Task\<IList\<Claim>>
- IUserConfirmation\<TUser>
- IUserEmailStore\<TUser>
- IUserLockoutStore\<TUser>
- IUserLoginStore\<TUser>
- IUserPasskeyStore\<TUser>
- IUserPasswordStore\<TUser>
- IUserPhoneNumberStore\<TUser>
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
- IUserTwoFactorRecoveryCodeStore\<TUser>
- IUserTwoFactorStore\<TUser>
- IUserTwoFactorTokenProvider\<TUser>
- IUserValidator\<TUser>
- LockoutOptions
- PasswordHasher\<TUser> : IPasswordHasher\<TUser>
- PasswordHasherCompatibilityMode
- PasswordHasherOptions
- PasswordOptions
- PasswordValidator\<TUser> : IPasswordValidator\<TUser>
- PasswordVerificationResult
- PersonalDataAttribute
- PhoneNumberTokenProvider\<TUser> : TotpSecurityStampBasedTokenProvider\<TUser>
- ProtectedPersonalDataAttribute 
- **RoleManager\<TRole>** : IDisposable
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
- RoleValidator\<TRole> : IRoleValidator\<TRole>
- SignInOptions
- SignInResult
- StoreOptions
- TokenOptions
- TokenProviderDescriptor
- TotpSecurityStampBasedTokenProvider\<TUser> :  IUserTwoFactorTokenProvider\<TUser>
- UpperInvariantLookupNormalizer : ILookupNormalizer
- **UserClaimsPrincipalFactory<TUser, TRole>**
- **UserClaimsPrincipalFactory\<TUser>**
- UserLoginInfo
- **UserManager\<TUser>** : IDisposable
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
- UserOptions
- UserPasskeyInfo
- UserValidator\<TUser> : IUserValidator\<TUser>

## System.Security.Claims

- PrincipalExtensions (ClaimPrincipal)
	- FindFirstValue(string claimType) : string

# Assembly : Microsoft.AspNetCore.Identity

## Microsoft.Extensions.DependencyInjection

- IdentityServiceCollectionExtensions
	- **AddIdentity<TUser, TRole>()**
	- AddIdentity<TUser, TRole>(Action\<IdentityOptions> setupAction)
	- **ConfigureApplicationCookie**(Action\<**CookieAuthenticationOptions**> configure)
	- ConfigureExternalCookie(System.Action\<CookieAuthenticationOptions> configure)

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
- IdentityBuilderExtensions (**IdentityBuilder**)
	- AddDefaultTokenProviders()
	- **AddSignInManager()**
	- **AddSignInManager\<TSignInManager>()**

**IdentityConstants** is used to specify the authentication schemes.

- **IdentityConstants**
	- **ApplicationScheme**
	- ExternalScheme
	- TwoFactorRememberMeScheme
	- TwoFactorUserIdScheme
- IdentityCookieAuthenticationBuilderExtensions(this **AuthenticationBuilder**)
	- **AddApplicationCookie**() : OptionsBuilder\<CookieAuthenticationOptions>
	- AddExternalCookie() : OptionsBuilder\<CookieAuthenticationOptions>
	- **AddIdentityCookies**() : **IdentityCookiesBuilder**
	- AddIdentityCookies(Action\<**IdentityCookiesBuilder**> configureCookies) : IdentityCookiesBuilder
	- AddTwoFactorRememberMeCookie() : OptionsBuilder\<CookieAuthenticationOptions>
	- AddTwoFactorUserIdCookie() : OptionsBuilder\<CookieAuthenticationOptions>
- **IdentityCookiesBuilder**
	- ApplicationCookie : OptionsBuilder\<CookieAuthenticationOptions>
- ISecurityStampValidator
	- ValidateAsync(CookieValidatePrincipalContext context) : Task
- ITwoFactorSecurityStampValidator
- SecurityStampRefreshingPrincipalContext
	- SecurityStampRefreshingPrincipalContext()
	- CurrentPrincipal : ClaimPrincipal
	- NewPrincipal : ClaimPrincipal
- SecurityStampValidator
	- ValidateAsync\<TValidator>(CookieValidatePrincipalContext context) : Task
		- where TValidator : ISecurityStampValidator
	- ValidatePrincipalAsync(CookieValidatePrincipalContext context) : Task
- SecurityStampValidator\<TUser>
	- SecurityStampValidator(...)
		- IOptions\<SecurityStampValidatorOptions> options
		- SignInManager\<TUser> signInManager
		- ISystemClock clock
	- SecurityStampVerified(TUser user, CookieValidatePrincipalContext context) : Task
	- ValidateAsync(CookieValidatePrincipalContext context) : Task
	- VerifySecurityStamp(ClaimsPrincipal principal) : Task\<TUser>
	- Clock : ISystemClock
	- Options : SecurityStampValidatorOptions
	- SignInManager : SignInManager\<TUser>
- SecurityStampValidatorOptions
	- SecurityStampValidatorOptions()
	- OnRefreshingPrincipal : System.Func<SecurityStampRefreshingPrincipalContext, Task>
	- ValidationInterval : System.TimeSpan 
- **SignInManager\<TUser>**
	- CanSignInAsync(TUser user) : Task\<bool>
	- CheckPasswordSignInAsync(...) : Task\<SignInResult>
		- TUser user, 
		- string password, 
		- bool lockoutOnFailure
	- ConfigureExternalAuthenticationProperties(...) : AuthenticationProperties
		- string provider, 
		- string redirectUrl, 
		- [string userId = null]
	- CreateUserPrincipalAsync(TUser user) : Task\<ClaimsPrincipal>
	- ExternalLoginSignInAsync(...) : Task\<SignInResult>
		- string loginProvider, 
		- string providerKey, 
		- bool isPersistent
	- ExternalLoginSignInAsync(...) : Task\<SignInResult>
		- string loginProvider, 
		- string providerKey, 
		- bool isPersistent, 
		- bool bypassTwoFactor
	- ForgetTwoFactorClientAsync() : Task
	- GetExternalAuthenticationSchemesAsync() : Task<IEnumerable\<AuthenticationScheme>>
	- GetExternalLoginInfoAsync([string expectedXsrf = null]) : Task\<ExternalLoginInfo>
	- GetTwoFactorAuthenticationUserAsync() : Task\<TUser>
	- IsLockedOut(TUser user) : Task\<bool>
	- IsSignedIn(ClaimsPrincipal principal) : bool
	- IsTwoFactorClientRememberedAsync(TUser user) : Task\<bool>
	- LockedOut(TUser user) : Task\<SignInResult>
	- PasswordSignInAsync(...) : Task\<SignInResult>
		- string userName, 
		- string password, 
		- bool isPersistent, 
		- bool lockoutOnFailure
	- PasswordSignInAsync(...) : Task\<SignInResult>
		- TUser user, 
		- string password, 
		- bool isPersistent, 
		- bool lockoutOnFailure
	- PreSignInCheck(TUser user) : Task\<SignInResult>
	- RefreshSignInAsync(TUser user)
	- RememberTwoFactorClientAsync(TUser user) : Task
	- ResetLockout(TUser user) : Task
	- **SignInAsync()**
		- TUser user, 
		- bool isPersistent, 
		- [string **authenticationMethod** = null]
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
	- SignInOrTwoFactorAsync(...) : Task\<SignInResult>
		- TUser user, 
		- bool isPersistent, 
		- [string loginProvider = null], 
		- [bool bypassTwoFactor = False]
	- SignOutAsync()
	- TwoFactorAuthenticatorSignInAsync(...) : Task\<SignInResult>
		- string code, 
		- bool isPersistent, 
		- bool rememberClient
	- TwoFactorRecoveryCodeSignInAsync(string recoveryCode) : Task\<SignInResult>
	- TwoFactorSignInAsync(...) : Task\<SignInResult>
		- string provider, 
		- string code, 
		- bool isPersistent, 
		- bool rememberClient
	- UpdateExternalAuthenticationTokensAsync(...) : Task\<IdentityResult>
		- ExternalLoginInfo externalLogin)
	- ValidateSecurityStampAsync(ClaimsPrincipal principal) : Task\<TUser> 
	- ValidateSecurityStampAsync(TUser user, string securityStamp) : Task\<bool>
	- ClaimsFactory : IUserClaimsPrincipalFactory\<TUser>
	- Context : HttpContext
	- Options : IdentityOptions
	- UserManager : **UserManager\<TUser>**
- TwoFactorSecurityStampValidator\<TUser> : SecurityStampValidator\<TUser>
	- : ISecurityStampValidator
	- : ITwoFactorSecurityStampValidator
	- TwoFactorSecurityStampValidator(...)
		- IOptions\<SecurityStampValidatorOptions> options, 
		- SignInManager\<TUser> signInManager, 
		- ISystemClock clock
	- SecurityStampVerified(TUser user, CookieValidatePrincipalContext context) : Task
	- VerifySecurityStamp(ClaimsPrincipal principal) : Task\<TUser>
