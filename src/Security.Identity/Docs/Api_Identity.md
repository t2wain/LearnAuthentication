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
	- AccessFailedAsync(TUser user) : Task\<IdentityResult>
	- AddClaimAsync(TUser user, Claim claim) : Task\<IdentityResult>
	- AddClaimsAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- IEnumerable\<Claim> claims
	- AddLoginAsync(TUser user, UserLoginInfo login) : Task\<IdentityResult>
	- AddOrUpdatePasskeyAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- UserPasskeyInfo passkey
	- AddPasswordAsync(TUser user, string password) : Task\<IdentityResult>
	- AddToRoleAsync(TUser user, string role) : Task\<IdentityResult>
	- AddToRolesAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- IEnumerable\<string> roles
	- ChangeEmailAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- string newEmail, 
		- string token
	- ChangePasswordAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- string currentPassword, 
		- string newPassword
	- ChangePhoneNumberAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- string phoneNumber, 
		- string token
	- CheckPasswordAsync(TUser user, string password) : Task\<bool>
	- ConfirmEmailAsync(TUser user, string token) : Task\<IdentityResult>
	- CountRecoveryCodesAsync(TUser user) : Task\<int>
	- CreateAsync(TUser user) : Task\<IdentityResult>
	- CreateAsync(TUser user, string password) : Task\<IdentityResult>
	- CreateSecurityTokenAsync(TUser user) : Task\<byte[]>
	- CreateTwoFactorRecoveryCode() : string
	- DeleteAsync(TUser user) : Task\<byte[]>
	- Dispose()
	- FindByEmailAsync(string email) : Task\<TUser>
	- FindByIdAsync(string userId) : Task\<TUser>
	- FindByLoginAsync(string loginProvider, string providerKey) : Task\<TUser>
	- FindByNameAsync(string userName) : Task\<TUser>
	- FindByPasskeyIdAsync(byte[] credentialId) : Task\<TUser>
	- GenerateChangeEmailTokenAsync(TUser user, string newEmail) : Task\<string>
	- GenerateConcurrencyStampAsync(TUser user) : Task\<string>
	- GenerateEmailConfirmationTokenAsync(TUser user) : Task\<string>
	- GenerateNewAuthenticatorKey() : string
	- GenerateNewTwoFactorRecoveryCodesAsync(...) : Task<IEnumerable\<string>>
		- TUser user, 
		- int number
	- GeneratePasswordResetTokenAsync(TUser user) : Task\<string>
	- GenerateTwoFactorTokenAsync(...) : Task\<string>
		- TUser user, 
		- string tokenProvider
	- GenerateUserTokenAsync(...) : Task\<string>
		- TUser user, 
		- string tokenProvider, 
		- string purpose
	- GetAccessFailedCountAsync(TUser user) : Task\<int>
	- GetAuthenticationTokenAsync(...) : Task\<string>
		- TUser user, 
		- string loginProvider, 
		- string tokenName
	- GetAuthenticatorKeyAsync(TUser user) : Task\<string>
	- GetChangeEmailTokenPurpose(string newEmail) : Task\<string>
	- GetClaimsAsync(TUser user) : Task<IList\<Claim>>
	- GetEmailAsync(TUser user) : Task\<string>
	- GetLockoutEnabledAsync(TUser user) : Task\<bool>
	- GetLockoutEndDateAsync(TUser user) : Task<System.DateTimeOffset?>
	- GetLoginsAsync(TUser user) : Task<IList\<UserLoginInfo>>
	- GetPasskeyAsync(TUser user, byte[] credentialId) : Task\<UserPasskeyInfo>
	- GetPasskeysAsync(TUser user) : Task<IList\<UserPasskeyInfo>>
	- GetPhoneNumberAsync(TUser user) : Task\<string>
	- GetRolesAsync(TUser user) : Task<IList\<string>>
	- GetSecurityStampAsync(TUser user) : Task\<string>
	- GetTwoFactorEnabledAsync(TUser user) : Task\<bool>
	- GetUserAsync(ClaimsPrincipal principal) : Task\<TUser>
	- GetUserId(ClaimsPrincipal principal) : string
	- GetUserIdAsync(TUser user) : Task\<string>
	- GetUserName(ClaimsPrincipal principal) : string
	- GetUserNameAsync(TUser user) : Task\<string>
	- GetUsersForClaimAsync(Claim claim) : Task<IList\<TUser>> 
	- GetUsersInRoleAsync(string roleName) : Task<IList\<TUser>> 
	- GetValidTwoFactorProvidersAsync(TUser user) : Task<IList\<string>>
	- HasPasswordAsync(TUser user) : Task\<bool>
	- IsEmailConfirmedAsync(TUser user) : Task\<bool>
	- IsInRoleAsync(TUser user, string role) : Task\<bool>
	- IsLockedOutAsync(TUser user) : Task\<bool>
	- IsPhoneNumberConfirmedAsync(TUser user) : Task\<bool>
	- NormalizeEmail(string email) : string
	- NormalizeName(string name) : string
	- RedeemTwoFactorRecoveryCodeAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- string code
	- RegisterTokenProvider(...)
		- string providerName, 
		- IUserTwoFactorTokenProvider\<TUser> provider
	- RemoveAuthenticationTokenAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- string loginProvider, 
		- string tokenName
	- RemoveClaimAsync(TUser user, Claim claim) : Task\<IdentityResult>
	- RemoveClaimsAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- IEnumerable\<Claim> claims
	- RemoveFromRoleAsync(TUser user, string role) : Task\<IdentityResult>
	- RemoveFromRolesAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- IEnumerable\<string> roles
	- RemoveLoginAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- string loginProvider, 
		- string providerKey
	- RemovePasskeyAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- byte[] credentialId
	- RemovePasswordAsync(TUser user) : Task\<IdentityResult>
	- ReplaceClaimAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- Claim claim, 
		- Claim newClaim
	- ResetAccessFailedCountAsync(TUser user) : Task\<IdentityResult>
	- ResetAuthenticatorKeyAsync(TUser user) : Task\<IdentityResult>
	- ResetPasswordAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- string token, 
		- string newPassword
	- SetAuthenticationTokenAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- string loginProvider, 
		- string tokenName, 
		- string tokenValue
	- SetEmailAsync(TUser user, string email) : Task\<IdentityResult>
	- SetLockoutEnabledAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- bool enabled
	- SetLockoutEndDateAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- System.DateTimeOffset? lockoutEnd
	- SetPhoneNumberAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- string phoneNumber
	- SetTwoFactorEnabledAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- bool enabled
	- SetUserNameAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- string userName
	- UpdateAsync(TUser user) : Task\<IdentityResult>
	- UpdateNormalizedEmailAsync(TUser user)
	- UpdateNormalizedUserNameAsync(TUser user)
	- UpdatePasswordHash(...) : Task\<IdentityResult>
		- TUser user, 
		- string newPassword, 
		- bool validatePassword
	- UpdateSecurityStampAsync(TUser user) : Task\<IdentityResult>
	- UpdateUserAsync(TUser user) : Task\<IdentityResult>
	- ValidatePasswordAsync(...) : Task\<IdentityResult>
		- TUser user, 
		- string password
	- ValidateUserAsync(TUser user) : Task\<IdentityResult>
	- VerifyChangePhoneNumberTokenAsync(...) : Task\<bool>
		- TUser user, 
		- string token, 
		- string phoneNumber
	- VerifyPasswordAsync(...) " Task\<PasswordVerificationResult> 
		- IUserPasswordStore\<TUser> store, 
		- TUser user, 
		- string password
	- VerifyTwoFactorTokenAsync(...) : Task\<bool>
		- TUser user, 
		- string tokenProvider, 
		- string token
	- VerifyUserTokenAsync(...) : Task\<bool>
		- TUser user, 
		- string tokenProvider, 
		- string purpose, 
		- string token
	- ErrorDescriber : IdentityErrorDescriber
	- KeyNormalizer : ILookupNormalizer
	- Options : IdentityOptions
	- PasswordHasher : IPasswordHasher\<TUser>
	- PasswordValidators : IList<IPasswordValidator\<TUser>> 
	- ServiceProvider : IServiceProvider 
	- Store : IUserStore\<TUser>
	- SupportsQueryableUsers : bool
	- SupportsUserAuthenticationTokens : bool
	- SupportsUserAuthenticatorKey : bool
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
	- Users : IQueryable\<TUser>
	- UserValidators : IList<IUserValidator\<TUser>>
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
	- AllowedUserNameCharacters : string
	- RequireUniqueEmail : bool
- UserPasskeyInfo
	- UserPasskeyInfo(...)
		- byte[] credentialId, 
		- byte[] publicKey, 
		- System.DateTimeOffset createdAt, 
		- uint signCount, 
		- string[] transports, 
		- bool isUserVerified, 
		- bool isBackupEligible, 
		- bool isBackedUp, 
		- byte[] attestationObject, 
		- byte[] clientDataJson
	- AttestationObject : bool
	- ClientDataJson : byte[]
	- CreatedAt : System.DateTimeOffset
	- CredentialId : byte[]
	- IsBackedUp : bool
	- IsBackupEligible : bool
	- IsUserVerified : bool
	- Name : string
	- PublicKey : byte[]
	- SignCount : int
	- Transports : string[]
- UserValidator\<TUser> : IUserValidator\<TUser>
	- UserValidator(IdentityErrorDescriber errors = null)
	- ValidateAsync(...) : Task\<IdentityResult>
		- UserManager\<TUser> manager, 
		- TUser user
	- Describer : IdentityErrorDescriber

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
	- **ExternalLoginSignInAsync**(...) : Task\<SignInResult>
		- string loginProvider, 
		- string providerKey, 
		- bool isPersistent
	- **ExternalLoginSignInAsync**(...) : Task\<SignInResult>
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
	- **PasswordSignInAsync**(...) : Task\<SignInResult>
		- string userName, 
		- string password, 
		- bool isPersistent, 
		- bool lockoutOnFailure
	- **PasswordSignInAsync**(...) : Task\<SignInResult>
		- TUser user, 
		- string password, 
		- bool isPersistent, 
		- bool lockoutOnFailure
	- PreSignInCheck(TUser user) : Task\<SignInResult>
	- RefreshSignInAsync(TUser user)
	- **RememberTwoFactorClientAsync**(TUser user) : Task
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
	- **TwoFactorAuthenticatorSignInAsync**(...) : Task\<SignInResult>
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
