# Custom storage providers for ASP.NET Core Identity

By default, the ASP.NET Core Identity system stores user information in a SQL Server database using Entity Framework Core. For many apps, this approach works well. However, you may prefer to use a different persistence mechanism or data schema. For example:

- You use Azure Table Storage or another data store.
- Your database tables have a different structure.
- You may wish to use a different data access approach, such as Dapper.

In each of these cases, you can write a customized provider for your storage mechanism and plug that provider into your app.

ASP.NET Core Identity is included in project templates in Visual Studio with the "Individual User Accounts" option.

# The ASP.NET Core Identity architecture

ASP.NET Core Identity consists of classes called **managers** and **stores**. **Managers are high-level classes** which an app developer uses to perform operations, such as creating an Identity user. **Stores are lower-level classes** that specify how entities, such as users and roles, are persisted. Stores follow the repository pattern and are closely coupledwith the persistence mechanism. **Managers are decoupled from stores**, which means you can **replace the persistence mechanism without changing your application code** (except for configuration).

The following diagram shows how a **web app interacts with the managers**, while **stores interact with the data access layer**.

1. ASP.NET Core App
1. -> Identity Manager (**UserManager**, **RoleManager**)
1. -> Identity Store (**UserStore**, **RoleStore**)
1. -> Data Access Layer
1. -> Data Source

**To create a custom storage provider, create the data source, the data access layer, and the store classes** that interact with this data access layer. Y**ou don't need to customize the managers or your app code thatinteracts with them**.

When creating a new instance of UserManager or RoleManager you provide the type of the user class and pass an instance of the store class as an argument. This approach enables you to plug your customized classes into ASP.NET Core.

Reconfigure app to use new storage provider shows how to instantiate UserManager and RoleManager with a customized store.

# ASP.NET Core Identity stores data types

ASP.NET Core Identity data types are detailed in the following sections:

- **Users** - Registered users of your web site. The IdentityUser type may be extended or used as an example for your own custom type. You don't need to inherit from a particular type to implement your own custom identity storage solution.

- **User Claims** - A set of statements (or Claims) about the user that represent the user's identity. Can enable greater expression of the user's identity than can be achieved through roles.

- **User Logins** - Information about the external authentication provider (like Facebook or a Microsoft account) to use when logging in a user.

- **Roles** - Authorization groups for your site. Includes the role Id and role name (like "Admin" or "Employee").

## Model generic types

Identity defines default Common Language Runtime (CLR) types for each of the entity typeslisted above. These types are all prefixed with Identity:

- IdentityUser
- IdentityRole
- IdentityUserClaim
- IdentityUserToken
- IdentityUserLogin
- IdentityRoleClaim
- IdentityUserRole

Rather than using these types directly, the types can be used as base classes for the app's own types.

# The data access layer

You have a lot of freedom when designing the data access layer for a customized store provider. You only need to create persistence mechanisms for features that you intend to use in your app. For example, if you are not using roles in your app, you don't need to create storage for roles or user role associations. Your technology and existing infrastructure may require a structure that's very different from the default implementation of ASP.NET Core Identity. In your data access layer, you provide the logic to work with the structure of your storage implementation.

The data access layer provides the logic to save the data from ASP.NET Core Identity to a data source. The data access layer for your customized storage provider might include the following classes to store user and role information.

- **Context class** - Encapsulates the information to connect to your persistence mechanism and executequeries. Several data classes require an instance of this class, typically provided throughdependency injection.

- **User Storage** - Stores and retrieves user information (such as user name and password hash).

- **Role Storage** - Stores and retrieves role information (such as the role name).

- **UserClaims Storage** - Stores and retrieves user claim information (such as the claim type and value).

- **UserLogins Storage** - Stores and retrieves user login information (such as an external authentication provider).

- **UserRole Storage** - Stores and retrieves which roles are assigned to which users.

# Customize the user class

When implementing a storage provider, **create a user class** which is equivalent to the
**IdentityUser** class.

At a minimum, your user class must include an **Id** and a **UserName** property.

The **IdentityUser** class defines the properties that the **UserManager** calls when performing requested operations. The default type of the **Id** property is a **string**, but you can inherit from **IdentityUser<TKey, TUserClaim, TUserRole, TUserLogin,TUserToken>** and specify a different type. The framework expects the storage implementation to handle data type conversions.

# Customize the user store

**Create a UserStore class** that provides the methods **for all data operations on the user**. This class is equivalent to the **UserStore\<TUser>** class. **In your UserStore class, implement IUserStore\<TUser> and the optional interfaces required**. You select **which optional interfaces to implement** based on the functionality provided in your app.

## Optional interfaces

- IUserRoleStore
- IUserClaimStore
- IUserPasswordStore
- IUserSecurityStampStore
- IUserEmailStore
- IUserPhoneNumberStore
- IQueryableUserStore
- IUserLoginStore
- IUserTwoFactorStore
- IUserLockoutStore

Within the UserStore class, you use the data access classes that you created to perform operations. These are passed in using dependency injection. For example, in the SQLServer with Dapper implementation, the UserStore class has the CreateAsync method which uses an instance of DapperUsersTable to insert a new record:

```csharp
public async Task<IdentityResult> CreateAsync(ApplicationUser user)
{
	string sql = "INSERT INTO dbo.CustomUser " +
		"VALUES (@id, @Email, @EmailConfirmed, @PasswordHash, @UserName)";

	int rows = await _connection.ExecuteAsync(sql, new { user.Id, 
		user.Email, user.EmailConfirmed, user.PasswordHash, user.UserName });

	if (rows > 0) 
	{
		return IdentityResult.Success; 
	}

	return IdentityResult.Failed(new IdentityError { 
		Description = $"Could not insert user {user.Email}." });
}
```

## Interfaces to implement when customizing user store

- **IUserStore** - The IUserStore\<TUser> interface is the only interface you must implement in the user store. It defines methods for creating, updating, deleting, and retrieving users.

- **IUserClaimStore** - The IUserClaimStore\<TUser> interface defines the methods you implement toenable user claims. It contains methods for adding, removing and retrieving userclaims.

- **IUserLoginStore** - The IUserLoginStore\<TUser> defines the methods you implement to enable external authentication providers. It contains methods for adding, removing andretrieving user logins, and a method for retrieving a user based on the logininformation.

- **IUserRoleStore** - The IUserRoleStore\<TUser> interface defines the methods you implement to map a user to a role. It contains methods to add, remove, and retrieve a user's roles, and a method to check if a user is assigned to a role.

- **IUserPasswordStore** - The IUserPasswordStore\<TUser> interface defines the methods you implement to persist hashed passwords. It contains methods for getting and setting the hashedpassword, and a method that indicates whether the user has set a password.

- **IUserSecurityStampStore** - The IUserSecurityStampStor\<TUser> interface defines the methods you implement to use a security stamp for indicating whether the user's account information has changed. This stamp is updated when a user changes thepassword, or adds or removes logins. It contains methods for getting and settingthe security stamp.

- **IUserTwoFactorStore** - The IUserTwoFactorStore\<TUser> interface defines the methods you implement to support two factor authentication. It contains methods for getting and setting whether two factor authentication is enabled for a user.

- **IUserPhoneNumberStore** - The IUserPhoneNumberStore\<TUser> interface defines the methods you implement to store user phone numbers. It contains methods for getting and setting the phone number and whether the phone number is confirmed.

- **IUserEmailStore** - The IUserEmailStore\<TUser> interface defines the methods you implement to store user email addresses. It contains methods for getting and setting the email address and whether the email is confirmed.

- **IUserLockoutStore** - The IUserLockoutStore\<TUser> interface defines the methods you implement to store information about locking an account. It contains methods for tracking failed access attempts and lockouts.

- **IQueryableUserStore** - The IQueryableUserStore\<TUser> interface defines the members you implement to provide a queryable user store.

You implement only the interfaces that are needed in your app. For example:

```csharp
public class UserStore : 
	IUserStore<IdentityUser>,
	IUserClaimStore<IdentityUser>,
	IUserLoginStore<IdentityUser>,
	IUserRoleStore<IdentityUser>,
	IUserPasswordStore<IdentityUser>,
	IUserSecurityStampStore<IdentityUser>
{
	// interface implementations not shown
}
```

## IdentityUserClaim, IdentityUserLogin, and IdentityUserRole

The Microsoft.AspNet.Identity.EntityFramework namespace contains implementations of the IdentityUserClaim, IdentityUserLogin, and IdentityUserRole classes. If you are using these features, **you may want to create your own versions of these classes** and define the properties for your app. However, sometimes it's **more efficient to not load these entities into memory** when performing basic operations (such as adding or removing a user's claim). Instead, the **backend store classes can execute these operations directly on the data source**. For example, the UserStore.GetClaimsAsync method can call the userClaimTable.FindByUserId(user.Id) method to execute a query on that table directly and return a list of claims.

# Customize the role class

When implementing a role storage provider, you can create a custom role type. It need not implement a particular interface, but it must have an **Id** and typically it will have a **Name** property.

# Customize the role store

You can create a RoleStore class that provides the methods for all data operations on roles. This class is equivalent to the **RoleStore\<TRole>** class. In the RoleStore class, you implement the **IRoleStore\<TRole>** and optionally the **IQueryableRoleStore\<TRole>** interface.

- **IRoleStore\<TRole>** - The IRoleStore\<TRole> interface defines the methods to implement in the role store class. It contains methods for creating, updating, deleting, and retrieving roles.

- **RoleStore\<TRole>** - To customize RoleStore, create a class that implements the RoleStore\<TRole> interface.

# Reconfigure app to use a new storage provider

Once you have implemented a storage provider, you configure your app to use it. If your app used the default provider, replace it with your custom provider.

1. Remove the Microsoft.AspNetCore.EntityFramework.Identity NuGet package.
2. If the storage provider resides in a separate project or package, add a reference to it.
3. Replace all references to Microsoft.AspNetCore.EntityFramework.Identity with a using statement for the namespace of your storage provider.
4. Change the **AddIdentity** method to use the custom types. You can create your own extension methods for this purpose. See IdentityServiceCollectionExtensions for an example.
5. If you are using Roles, **update the RoleManager to use your RoleStore class**.
6. Update the connection string and credentials to your app's configuration.

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add identity types
builder.Services
	.AddIdentity<ApplicationUser, ApplicationRole>()
	.AddDefaultTokenProviders();

// Identity Services
builder.Services
	.AddTransient<IUserStore<ApplicationUser>, CustomUserStore>);
	.AddTransient<IRoleStore<ApplicationRole>, CustomRoleStore>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services
	.AddTransient<SqlConnection>(e => new SqlConnection(connectionString));
	.AddTransient<DapperUsersTable>();

// additional configuration
builder.Services.AddRazorPages();
var app = builder.Build();
```
# Using SignInManager and UserManager

```csharp
public AccountService 
{
	public AccountService (
		UserManager<IAppUser> userManager, 
		IUserStore<IAppUser> userStore,
		SignInManager<IAppUser> signInManager
	) 
	{
		_userManager = userManager; 
		_signInManager = signInManager;
		_userStore = userStore;
		if (!_userManager.SupportsUserEmail)
			_emailStore = (IUserEmailStore<IAppUser>)_userStore;
	}

	private async Task LoadAsync(IAppUser userInput)
	{
		var user = await _userManager.GetUserAsync(userInput);
		var userName = await _userManager.GetUserNameAsync(user);
		var phoneNumber = await _userManager.GetPhoneNumberAsync(user); 
	}

	private async Task UpdateAsync(IAppUser userInput)
	{
		var user = await _userManager.GetUserAsync(userInput);
		var userName = await _userManager.GetUserNameAsync(user);
		var phoneNumber = await _userManager.GetPhoneNumberAsync(user); 
		var setPhoneResult = 
			await _userManager.SetPhoneNumberAsync(user, userInput.PhoneNumber);
		await _userManager.UpdateAsync(user);
		await _signInManager.RefreshSignInAsync(user);
	}

	private async Task CreateAsync(IAppUser userInput)
	{
		var externalLogins = 
			(await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
		await _userStore.SetUserNameAsync(userInput, Input.Email, CancellationToken.None);
		await _emailStore.SetEmailAsync(userInput, Input.Email, CancellationToken.None);
		var result = await _userManager.CreateAsync(userInput, userInput.Password); 
		var code = await_userManager.GenerateEmailConfirmationTokenAsync(userInput);
		if (_userManager.Options.SignIn.RequireConfirmedAccount) {...}
		await _signInManager.SignInAsync(user, isPersistent: false);
	}

	private async Task SignInAsync(IAppUser userInput)
	{
		var externalLogins = 
			(await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
		var result = await _signInManager.PasswordSignInAsync(
			userInput.Email, userInput.Password, userInput.RememberMe, 
			lockoutOnFailure: false);
		if (result.Succeeded) {...}
		if (result.RequiresTwoFactor) {...}
		if (result.IsLockedOut) {...}
	}
```

# ISecurityStampValidator and SignOuteverywhere

Apps need to react to events involving security sensitive actions by regenerating the users ClaimsPrincipal. **For example, the ClaimsPrincipal should be regenerated when joining a role, changing the password, or other security sensitive events**. Identity uses the **ISecurityStampValidator interface to regenerate the ClaimsPrincipal**. The default implementation of Identity **registers a SecurityStampValidator with the main application cookie and the two-factor cookie**. **The validator hooks into the OnValidatePrincipal event of each cookie to call into Identity to verify that the user's security stamp claim is unchanged from what's stored in the cookie**. The validator calls in **at regular intervals**. The call interval is a **tradeoff** between hitting the datastore too frequently and not often enough. Checking with a long interval results in stale claims. Call **userManager.UpdateSecurityStampAsync(user)** to force existing cookies to be invalided the next time they are checked. Most of the Identity UI account and manage pages call userManager.UpdateSecurityStampAsync(user) after changing the password or adding alogin. **Apps can call userManager.UpdateSecurityStampAsync(user) to implement a signout everywhere action.**

Changing the validation interval is shown in the following highlighted code:

```csharp
// Force Identity's security stamp to be validated every minute.
builder.Services.Configure<SecurityStampValidatorOptions>(o => 
    o.ValidationInterval = TimeSpan.FromMinutes(1));
```

# Configure ASP.NET Core Identity

ASP.NET Core Identity uses default values for settings such as password policy, lockout,
and cookie configuration. These settings can be overridden at application startup.

## Identity options

The **IdentityOptions** class represents the options that can be used to configure the Identity system. IdentityOptions must be set after calling **AddIdentity** or AddDefaultIdentity

### Claims Identity

**IdentityOptions.ClaimsIdentity** specifies the **ClaimsIdentityOptions** with the properties shown in the following table.

- RoleClaimType
- SecurityStampClaimType
- UserIdClaimType
- UserNameClaimType

### Lockout

```csharp
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});
```

### Password

```csharp
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});
```

### Sign-in

```csharp
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default SignIn settings.
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});
```

### Tokens

**IdentityOptions.Tokens** specifies the **TokenOptions** with the properties shown in the
table.

- AuthenticatorTokenProvider -  to validate two-factor sign-ins with an authenticator
- ChangeEmailTokenProvider - to generate tokens used in email change confirmation emails.
- ChangePhoneNumberTokenProvider -  to generate tokens used when changing phone numbers.
- EmailConfirmationTokenProvider - to generate tokens used in account confirmation emails
- PasswordResetTokenProvider - to generate tokens used in password reset emails.
- ProviderMap - Used to construct a User Token Provider with the key used as the provider's name.

#### Change the email token lifespan

The default token lifespan of the Identity user tokens is one day . This section shows how to change the email token lifespan.

Add a custom DataProtectorTokenProvider\<TUser> and DataProtectionTokenProviderOptions:

```csharp
public class CustomEmailConfirmationTokenProvider<TUser>
    : DataProtectorTokenProvider<TUser> where TUser : class
{
    public CustomEmailConfirmationTokenProvider(
        IDataProtectionProvider dataProtectionProvider,
        IOptions<EmailConfirmationTokenProviderOptions> options,
        ILogger<DataProtectorTokenProvider<TUser>> logger)
        : base(dataProtectionProvider, options, logger)
    {

    }
}

public class EmailConfirmationTokenProviderOptions : 
DataProtectionTokenProviderOptions
{
    public EmailConfirmationTokenProviderOptions()
    {
        Name = "EmailDataProtectorTokenProvider";
        TokenLifespan = TimeSpan.FromHours(4);
    }
}
```
```csharp
builder.Services.AddDefaultIdentity<IdentityUser>(config =>
{
    config.Tokens.ProviderMap.Add("CustomEmailConfirmation",
        new TokenProviderDescriptor(
            typeof(CustomEmailConfirmationTokenProvider<IdentityUser>)));
    config.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
})

builder.Services.AddTransient<CustomEmailConfirmationTokenProvider<IdentityUser>>();
```

#### Change all data protection token lifespans

```csharp
builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
       o.TokenLifespan = TimeSpan.FromHours(3));
```

### User

```csharp
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default User settings.
    options.User.AllowedUserNameCharacters =
		"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});
```

### Cookie settings

Configure the app's cookie in Program.cs. ConfigureApplicationCookie must be called
after calling AddIdentity or AddDefaultIdentity.

```csharp
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.Cookie.Name = "YourAppCookieName";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Identity/Account/Login";
    // ReturnUrlParameter requires 
    //using Microsoft.AspNetCore.Authentication.Cookies;
    options.ReturnUrlParameter = 
		CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});
```

### Password Hasher options

**PasswordHasherOptions** gets and sets options for password hashing.

- **CompatibilityMode** 

The compatibility mode used when hashing new passwords. Defaults to **IdentityV3**. The first byte of a hashed password, called a format marker, specifies the **version of the hashing algorithm** used to hash the password. When verifying a password against a hash, the **VerifyHashedPassword** method selects the correct algorithm based on the first byte. **A client is able to authenticate regardless of which version of the algorithm was used to hash the password**. **Setting the compatibility mode affects the hashing of new passwords**.


- **Iteration**

The number of iterations used when hashing passwords using **PBKDF2**. This value is only used when the **CompatibilityMode** is set to **IdentityV3**. The value must be a positive integer and defaults to 100000.
