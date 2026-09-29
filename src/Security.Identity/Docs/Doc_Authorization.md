# Introduction to authorization in ASP.NET Core

Authorization is separate and distinct from authentication. However, **authorization relies on an authentication mechanism**. Authentication is the process of verifying a user's identity, which may result in the creation of **one or more identity objects** for the user.

# Authorization types

ASP.NET Core authorization provides a simple, **declarative role and a rich policy-based model**. **Authorization is expressed in requirements, and handlers evaluate a user's claims against requirements**. **Imperative checks** can be based on simple policies or policies which evaluate both the user identity and properties of the resource that the user is attempting to access.

# Require authenticated users

```csharp
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
```

The preceding highlighted code sets the **fallback authorization policy**. The fallback authorization policy requires all users to be authenticated, except for Razor Pages, controllers, or action methods with an authorization attribute. For example, Razor Pages, controllers, or action methods with **[AllowAnonymous] or [Authorize(PolicyName="MyPolicy")] use the applied authorization attribute rather than the fallback authorization policy**.

**RequireAuthenticatedUser** adds **DenyAnonymousAuthorizationRequirement** to the current instance, which enforces that the current user is authenticated.

The fallback authorization policy:

- **Is applied to all requests that don't explicitly specify an authorization policy**. For requests served by endpoint routing, this includes any endpoint that doesn't specify an authorization attribute. **For requests served by other middleware after the authorization middleware, such as static files, this applies the policy to all requests.**

Setting the fallback authorization policy to require users to be authenticated protects newly added Razor Pages and controllers. Having authorization required by default is more secure than relying on new controllers and Razor Pages to include the **[Authorize]** attribute.

The AuthorizationOptions class also contains AuthorizationOptions.DefaultPolicy. The **DefaultPolicy is the policy used with the [Authorize] attribute when no policy is specified**. [Authorize] doesn't contain a named policy, unlike [Authorize(PolicyName="MyPolicy")].

For more information on policies, see Policy-based authorization in ASP.NET Core.

An alternative way for MVC controllers and Razor Pages to require all users be authenticated is adding an **authorization filter**:

```csharp
builder.Services.AddControllers(config =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});
```

The preceding code uses an authorization filter, setting the fallback policy uses endpoint routing. Setting the fallback policy is the preferred way to require all users be authenticated.

Add AllowAnonymous to the Index and Privacy pages so anonymous users can get information about the site before they register.

# Create owner, manager, and administrator authorization handlers

Create a **ContactIsOwnerAuthorizationHandler** class in the Authorization folder. The ContactIsOwnerAuthorizationHandler verifies that the user acting on a resource owns the resource.

```csharp
public class ContactIsOwnerAuthorizationHandler
    : AuthorizationHandler<OperationAuthorizationRequirement, Contact>
{
    UserManager<IdentityUser> _userManager;

    public ContactIsOwnerAuthorizationHandler(
        UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OperationAuthorizationRequirement requirement,
        Contact resource)
    {
        if (context.User == null || resource == null)
        {
            return Task.CompletedTask;
        }

        // If not asking for CRUD permission, return.
        if (requirement.Name != Constants.CreateOperationName &&
            requirement.Name != Constants.ReadOperationName   &&
            requirement.Name != Constants.UpdateOperationName &&
            requirement.Name != Constants.DeleteOperationName )
        {
            return Task.CompletedTask;
        }

        if (resource.OwnerID == _userManager.GetUserId(context.User))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
```

The ContactIsOwnerAuthorizationHandler calls context.Succeed if the current authenticated user is the contact owner. Authorization handlers generally:

- **Call context.Succeed when the requirements are met**.
- **Return Task.CompletedTask when requirements aren't met. Returning Task.CompletedTask without a prior call to context.Success or context.Fail, is not a success or failure, it allows other authorization handlers to run.**

If you need to explicitly fail, call **context.Fail**.

The app allows contact owners to edit/delete/create their own data. ContactIsOwnerAuthorizationHandler doesn't need to check the operation passed in the requirement parameter.

## Create a manager authorization handler

The ContactManagerAuthorizationHandler verifies the user acting on the resource is a manager. Only managers can approve or reject content changes (new or changed)

```csharp
public class ContactManagerAuthorizationHandler :
    AuthorizationHandler<OperationAuthorizationRequirement, Contact>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OperationAuthorizationRequirement requirement,
        Contact resource)
    {
        if (context.User == null || resource == null)
        {
            return Task.CompletedTask;
        }

        // If not asking for approval/reject, return.
        if (requirement.Name != Constants.ApproveOperationName &&
            requirement.Name != Constants.RejectOperationName)
        {
            return Task.CompletedTask;
        }

        // Managers can approve or reject.
        if (context.User.IsInRole(Constants.ContactManagersRole))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
```

## Create an administrator authorization handler

The ContactAdministratorsAuthorizationHandler verifies the user acting on the resource is an administrator. Administrator can do all operations.

```csharp
public class ContactAdministratorsAuthorizationHandler
    : AuthorizationHandler<OperationAuthorizationRequirement, Contact>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OperationAuthorizationRequirement requirement, 
        Contact resource)
    {
        if (context.User == null)
        {
            return Task.CompletedTask;
        }

        // Administrators can do anything.
        if (context.User.IsInRole(Constants.ContactAdministratorsRole))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
```

# Register the authorization handlers

Services using Entity Framework Core must be registered for dependency injection using AddScoped. The ContactIsOwnerAuthorizationHandler uses ASP.NET Core Identity, which is built on Entity Framework Core. Register the handlers with the service collection so they're available to the ContactsController through dependency injection. Add the following code to the end of ConfigureServices:

```csharp
// Authorization handlers.
builder.Services.AddScoped<IAuthorizationHandler,
    ContactIsOwnerAuthorizationHandler>();

builder.Services.AddSingleton<IAuthorizationHandler,
    ContactAdministratorsAuthorizationHandler>();

builder.Services.AddSingleton<IAuthorizationHandler,
    ContactManagerAuthorizationHandler>();
```

ContactAdministratorsAuthorizationHandler and ContactManagerAuthorizationHandler are added as **singletons**. They're singletons because they **don't use EF and all the information needed is in the Context parameter of the HandleRequirementAsync method**.

# Support authorization

## Review the contact operations requirements class

```csharp
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ContactManager.Authorization
{
    public static class ContactOperations
    {
        public static OperationAuthorizationRequirement Create =   
          new OperationAuthorizationRequirement {Name=Constants.CreateOperationName};

        public static OperationAuthorizationRequirement Read = 
          new OperationAuthorizationRequirement {Name=Constants.ReadOperationName};  

        public static OperationAuthorizationRequirement Update = 
          new OperationAuthorizationRequirement {Name=Constants.UpdateOperationName}; 

        public static OperationAuthorizationRequirement Delete = 
          new OperationAuthorizationRequirement {Name=Constants.DeleteOperationName};

        public static OperationAuthorizationRequirement Approve = 
          new OperationAuthorizationRequirement {Name=Constants.ApproveOperationName};

        public static OperationAuthorizationRequirement Reject = 
          new OperationAuthorizationRequirement {Name=Constants.RejectOperationName};
    }

    public class Constants
    {
        public static readonly string CreateOperationName = "Create";
        public static readonly string ReadOperationName = "Read";
        public static readonly string UpdateOperationName = "Update";
        public static readonly string DeleteOperationName = "Delete";
        public static readonly string ApproveOperationName = "Approve";
        public static readonly string RejectOperationName = "Reject";
        public static readonly string ContactAdministratorsRole =   
            "ContactAdministrators";
        public static readonly string ContactManagersRole = "ContactManagers";
    }
}
```
```csharp
using Microsoft.AspNetCore.Authorization;
namespace ContactManager.Pages.Contacts
{
    public class DI_BasePageModel : PageModel
    {
        protected ApplicationDbContext Context { get; }
        protected IAuthorizationService AuthorizationService { get; }
        protected UserManager<IdentityUser> UserManager { get; }

        public DI_BasePageModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager) : base()
        {
            Context = context;
            UserManager = userManager;
            AuthorizationService = authorizationService;
        } 
    }
}
```

The preceding code:

- Adds the **IAuthorizationService** service to access to the **authorization handlers**.
- Adds the Identity UserManager service.
- Add the ApplicationDbContext.

### Create authorization

```csharp
namespace ContactManager.Pages.Contacts
{
    public class CreateModel : DI_BasePageModel
    {
        public CreateModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
                : base(context, authorizationService, userManager) { }

        [BindProperty]
        public Contact Contact { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            ...
            Contact.OwnerID = UserManager.GetUserId(User);

            var isAuthorized = 
                await AuthorizationService.AuthorizeAsync(
                    User, Contact, ContactOperations.Create);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            Context.Contact.Add(Contact);
            ...
        }
    }
}
```

### Update authorization

```csharp
public class IndexModel : DI_BasePageModel
{
    public IndexModel(
        ApplicationDbContext context,
        IAuthorizationService authorizationService,
        UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager) {  }

    public IList<Contact> Contact { get; set; }

    public async Task OnGetAsync()
    {
        var contacts = getAllContacts(...);

        // general authorization check, not per resource
        var isAuthorized = User.IsInRole(Constants.ContactManagersRole) ||
            User.IsInRole(Constants.ContactAdministratorsRole);

        var currentUserId = UserManager.GetUserId(User);

        // Only approved contacts are shown UNLESS you're 
        // authorized to see them or you are the owner.
        if (!isAuthorized)
        {
            contacts = contacts.Where(c => 
                c.Status == ContactStatus.Approved 
                    || c.OwnerID == currentUserId);
        }
        ...
    }
}
```

Add an **authorization handler** to verify the user owns the contact. Because **resource authorization** is being validated, the **[Authorize] attribute** is not enough. The app doesn't have access to the resource when attributes are evaluated. **Resource-based authorization must be imperative**. Checks must be performed once the app has access to the resource, either by loading it in the page model or by loading it within the handler itself. You frequently access the resource by passing in the resource key.

```csharp
public class EditModel : DI_BasePageModel
{
    public EditModel(
        ApplicationDbContext context,
        IAuthorizationService authorizationService,
        UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager) {  }

    [BindProperty]
    public Contact Contact { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Contact? contact = getContact(...);

        // imperative authorization check per resource
        var isAuthorized = await AuthorizationService.AuthorizeAsync(
                User, contact, ContactOperations.Update);

        if (!isAuthorized.Succeeded)
        {
            return Forbid();
        }
        ...
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        ...
        // Fetch Contact from DB to get OwnerID.
        var contact = getContact(...);

        // imperative authorization check per resource
        var isAuthorized = await AuthorizationService.AuthorizeAsync(
            User, contact, ContactOperations.Update);

        if (!isAuthorized.Succeeded)
        {
            return Forbid();
        }

        // imperative authorization check per resource
        var canApprove = await AuthorizationService.AuthorizeAsync(User,
            contact, ContactOperations.Approve);

        if (!canApprove.Succeeded)
        {
            ...
        }
        ...
    }
}
```

# Differences between Challenge and Forbid

This app sets the default policy to require authenticated users. The following code allows anonymous users. Anonymous users are allowed **to show the differences between Challenge vs Forbid.**

```csharp
[AllowAnonymous]
public class Details2Model : DI_BasePageModel
{
    public Details2Model(
        ApplicationDbContext context,
        IAuthorizationService authorizationService,
        UserManager<IdentityUser> userManager)
        : base(context, authorizationService, userManager) {  }

    public Contact Contact { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        ...
        if (!User.Identity!.IsAuthenticated)
        {
            return Challenge();
        }

        var isAuthorized = User.IsInRole(Constants.ContactManagersRole) ||
            User.IsInRole(Constants.ContactAdministratorsRole);

        var currentUserId = UserManager.GetUserId(User);

        if (!isAuthorized
            && currentUserId != Contact.OwnerID
            && Contact.Status != ContactStatus.Approved)
        {
            return Forbid();
        }
        ...
    }
}
```

In the preceding code:

- When the user is **not authenticated**, a **ChallengeResult** is returned. When a **ChallengeResult** is returned, the user is **redirected** to the sign-in page.

- When the user is **authenticated, but not authorized**, a **ForbidResult** is returned. When a **ForbidResult** is returned, the user is **redirected** to the access denied page.

# Razor Pages authorization conventions in ASP.NET Core

One way to control access in your Razor Pages app is to use authorization conventions at startup. These conventions allow you to authorize users and allow anonymous users to access individual pages or folders of pages. The conventions described in this topic automatically apply authorization filters to control access.

The sample app uses cookie authentication without ASP.NET Core Identity. The concepts and examples shown in this topic apply equally to apps that use ASP.NET Core Identity. To use ASP.NET Core Identity, follow the guidance in Introduction to Identity on ASP.NET Core.

## Require authorization to access a page or folder

- Use the **AuthorizePage** convention to add an **AuthorizeFilter** to the page at the specified path. To specify an authorization policy, use an AuthorizePage. The specified path is the View Engine path, which is the Razor Pages root relative path without an extension and containing only forward slashes.

- Use the **AuthorizeFolder** convention to add an AuthorizeFilte* to all of the pages in a folder at the specified path. To specify an authorization policy, use an AuthorizeFolder overload

- Use the **AuthorizeAreaPage** convention to add an AuthorizeFilter to the area page at the specified path. The page name is the path of the file without an extension relative to the pages root directory for the specified area. For example, the page name for the file Areas/Identity/Pages/Manage/Accounts.cshtml is /Manage/Accounts. To specify an authorization policy, use an AuthorizeAreaPage overload.

- Use the **AuthorizeAreaFolder** convention to add an AuthorizeFilter to all of the areas in a folder at the specified path. The folder path is the path of the folder relative to the pages root directory for the specified area. For example, the folder path for the files under Areas/Identity/Pages/Manage/ is /Manage. To specify an authorization policy, use an AuthorizeAreaFolder overload

- Use the **AllowAnonymousToPage** convention to add an **AllowAnonymousFilter** to a page at the specified path. The specified path is the View Engine path, which is the Razor Pages root relative path without an extension and containing only forward slashes

- Use the **AllowAnonymousToFolder** convention to add an AllowAnonymousFilter to all of the pages in a folder at the specified path. The specified path is the View Engine path, which is the Razor Pages root relative path.

```csharp
services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/Contact");
    options.Conventions.AuthorizeFolder("/Private");
    options.Conventions.AllowAnonymousToPage("/Private/PublicPage");
    options.Conventions.AllowAnonymousToFolder("/Private/PublicPages");
    options.Conventions.AuthorizeAreaPage("Identity", "/Manage/Accounts");
    options.Conventions.AuthorizeAreaFolder("Identity", "/Manage");
    options.Conventions.AllowAnonymousToPage("/Private/PublicPage");
    options.Conventions.AllowAnonymousToFolder("/Private/PublicPages");
});
```

```csharp
options.Conventions.AuthorizePage("/Contact", "AtLeast21");
options.Conventions.AuthorizeFolder("/Private", "AtLeast21");
options.Conventions.AuthorizeAreaPage("Identity", "/Manage/Accounts", "AtLeast21");
options.Conventions.AuthorizeAreaFolder("Identity", "/Manage", "AtLeast21");
```

> An AuthorizeFilter can be applied to a page model class with the [Authorize] filter
attribute. For more information, see Authorize filter attribute.

## Note on combining authorized and anonymous access

It's valid to specify that a folder of pages requires authorization and then specify that a page within that folder allows anonymous access.

```csharp
// This works.
.AuthorizeFolder("/Private").AllowAnonymousToPage("/Private/Public")
```

The reverse, however, isn't valid. You can't declare a folder of pages for anonymous access and then specify a page within that folder that requires authorization:

```csharp
// This doesn't work!
.AllowAnonymousToFolder("/Public").AuthorizePage("/Public/Private")
```

Requiring authorization on the Private page fails. When both the AllowAnonymousFilter and AuthorizeFilter are applied to the page, the AllowAnonymousFilter takes precedence and controls access

# Simple authorization in ASP.NET Core

Authorization in ASP.NET Core is controlled with the **[Authorize] attribute** and its various parameters. In its most basic form, applying the [Authorize] attribute to a controller, action, or Razor Page, limits access to that component to authenticated users

# Use the [Authorize] attribute

```csharp
[Authorize]
public class AccountController : Controller
{
    public ActionResult Login() {   }
    public ActionResult Logout() {   }
}
```
```csharp
public class AccountController : Controller
{
    public ActionResult Login() {   }

    [Authorize]
    public ActionResult Logout() {   }
}

```
```csharp
[Authorize]
public class AccountController : Controller
{
    [AllowAnonymous]
    public ActionResult Login() {   }

    public ActionResult Logout() {   }
}
```

[AllowAnonymous] bypasses authorization statements. If you combine [AllowAnonymous] and an [Authorize] attribute, the [Authorize] attributes are ignored. For example if you apply [AllowAnonymous] at the controller level:

- Any authorization requirements from [Authorize] attributes on the same controller or action methods on the controller are ignored.
- Authentication middleware is not short-circuited but doesn't need to succeed.

# Authorize attribute and Razor Pages

**The AuthorizeAttribute can not be applied to Razor Page handlers**. For example, [Authorize] can't be applied to **OnGet, OnPost, or any other page handler**. Consider using an ASP.NET Core MVC controller for pages **with different authorization requirements for different handlers**. Using an MVC controller when different authorization requirements are required:

- Is the least complex approach.
- Is the approach recommended by Microsoft.

If you decide not to use an MVC controller, the following two approaches can be used to apply authorization to Razor Page handler methods:

- **Use separate pages for page handlers requiring different authorization**. Move shared content into one or more partial views. When possible, this is the recommended approach.

- For content that must share a common page, **write a filter that performs authorization as part of IAsyncPageFilter.OnPageHandlerSelectionAsync**. The PageHandlerAuth GitHub project demonstrates this approach:

    - The **AuthorizeIndexPageHandlerFilter** implements the authorization filter:
    - The **[AuthorizePageHandler]** attribute is applied to the page handler:

```csharp
[TypeFilter(typeof(AuthorizeIndexPageHandlerFilter))]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet() {   }
    public void OnPost() {   }

    [AuthorizePageHandler]
    public void OnPostAuthorized() {  }
}
```
```csharp
public class AuthorizeIndexPageHandlerFilter 
    : IAsyncPageFilter, IOrderedFilter
{
    private readonly IAuthorizationPolicyProvider policyProvider;
    private readonly IPolicyEvaluator policyEvaluator;

    public AuthorizeIndexPageHandlerFilter(
        IAuthorizationPolicyProvider policyProvider,
        IPolicyEvaluator policyEvaluator)
    {
        this.policyProvider = policyProvider;
        this.policyEvaluator = policyEvaluator;
    }

    // Run late in the selection pipeline
    public int Order => 10000;

    public Task OnPageHandlerExecutionAsync(
        PageHandlerExecutingContext context, 
        PageHandlerExecutionDelegate next) => next();

    public async Task OnPageHandlerSelectionAsync(
        PageHandlerSelectedContext context)
    {
        // get decorated Attribute for this handler method.
        var attribute = context.HandlerMethod?.MethodInfo?
            .GetCustomAttribute<AuthorizePageHandlerAttribute>();

        if (attribute is null)
        {
            return;
        }

        var policy = await AuthorizationPolicy
            .CombineAsync(policyProvider, new[] { attribute });

        if (policy is null)
        {
            return;
        }

        await AuthorizeAsync(context, policy);
    }

    #region AuthZ - do not change
    private async Task AuthorizeAsync(ActionContext actionContext, 
        AuthorizationPolicy policy)
    {
        var httpContext = actionContext.HttpContext;

        var authenticateResult = 
            await policyEvaluator.AuthenticateAsync(policy, httpContext);

        var authorizeResult = 
            await policyEvaluator.AuthorizeAsync(policy, 
                authenticateResult, httpContext, actionContext.ActionDescriptor);

        if (authorizeResult.Challenged)
        {
            if (policy.AuthenticationSchemes.Count > 0)
            {
                foreach (var scheme in policy.AuthenticationSchemes)
                {
                    await httpContext.ChallengeAsync(scheme);
                }
            }
            else
            {
                await httpContext.ChallengeAsync();
            }
            return;
        }
        else if (authorizeResult.Forbidden)
        {
            if (policy.AuthenticationSchemes.Count > 0)
            {
                foreach (var scheme in policy.AuthenticationSchemes)
                {
                    await httpContext.ForbidAsync(scheme);
                }
            }
            else
            {
                await httpContext.ForbidAsync();
            }
            return;
        }
    }
}
```

The PageHandlerAuth sample approach **does not**:

- Compose with authorization attributes applied to the page, page model, or globally. Composing authorization attributes results in authentication and authorization executing multiple times when you have one more AuthorizeAttribute or AuthorizeFilter instances also applied to the page.

- Work in conjunction with the rest of ASP.NET Core authentication and authorization system. You must verify using this approach works correctly for your application.

**There are no plans to support the AuthorizeAttribute on Razor Page handlers.**

# Custom authorization policies with IAuthorizationRequirementData

## Minimum age authorize attribute The 

**MinimumAgeAuthorizeAttribute** implementation of **IAuthorizationRequirementData** sets an authorization age:

```csharp
using Microsoft.AspNetCore.Authorization;

class MinimumAgeAuthorizeAttribute(int age) : AuthorizeAttribute, 
    IAuthorizationRequirement, IAuthorizationRequirementData
{
    public int Age { get; set; } = age;

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}
```

## Minimum age authorization handler

The **MinimumAgeAuthorizationHandler** class handles the single **IAuthorizationRequirement** provided by **MinimumAgeAuthorizeAttribute**, as specified by the generic parameter MinimumAgeAuthorizeAttribute.

The **HandleRequirementAsync** method:

- Gets the user's birth date claim.
- Obtains the user's age from the claim.
- Adjusts age if the user hasn't had a birthday this year.
- Marks the authorization requirement succeeded if the user meets the age requirement.
- Implements logging for demonstration purposes.

```csharp
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

class MinimumAgeAuthorizationHandler(ILogger<MinimumAgeAuthorizationHandler> logger) 
    : AuthorizationHandler<MinimumAgeAuthorizeAttribute>
{
    // Check whether a given minimum age requirement is satisfied.
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        MinimumAgeAuthorizeAttribute requirement)
    {
        // Get the user's birth date claim.
        var dateOfBirthClaim = 
            context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth);

        if (dateOfBirthClaim != null)
        {
            var age = calcAge(...);

            // If the user meets the age requirement, mark the authorization
            // requirement succeeded.
            if (age >= requirement.Age)
            {
                context.Succeed(requirement);
            }
        }
        return Task.CompletedTask;
    }
}
```

The **MinimumAgeAuthorizationHandler is registered as a scoped IAuthorizationHandler** service in the app's Program file.

```csharp
builder.Services.AddSingleton<IAuthorizationHandler,
    MinimumAgeAuthorizationHandler>();
```

The GreetingsController displays the user's name when they satisfy the minimum age policy,
using an age of 21 years old with the **[MinimumAgeAuthorize({AGE})] attribute**, where the {AGE} placeholder is the age

```csharp
[ApiController]
[Route("api/[controller]")]
public class GreetingsController : Controller
{
    [MinimumAgeAuthorize(21)]
    [HttpGet("hello")]
    public string Hello() => 
        $"Hello {HttpContext.User.Identity?.Name}!";
}
```

If the user's birth date claim indicates that they're at least 21 years old, the controller displays the greeting string, issuing a 200 (OK) status code. If the user is missing the birth date claim or the claim indicates that they aren't at least 21 years old, the greeting isn't displayed **and a 403 (Forbidden) status code is issued**.

# Add Role services to Identity

```csharp
builder.Services.AddDefaultIdentity<IdentityUser>( ... )
    .AddRoles<IdentityRole>()
    ...
```

## Adding role checks

Role based authorization checks

- Are declarative and specify roles which the current user must be a member of to access the requested resource.
- Are applied to Razor Pages, controllers, or actions within a controller.
- Can not be applied at the Razor Page handler level, they must be applied to the Page.

For example, the following code limits access to any actions on the AdministrationController to users who are a member of the Administrator role.

```csharp
[Authorize(Roles = "Administrator")]
public class AdministrationController : Controller
{
    public IActionResult Index() =>
        Content("Administrator");
}
```

Multiple roles can be specified as a comma separated list:

```csharp
[Authorize(Roles = "HRManager,Finance")]
public class SalaryController : Controller
{
    public IActionResult Payslip() =>
        Content("HRManager || Finance");
}
```

The SalaryController is only accessible by users who are members of the HRManager role **or** the Finance role.

**When multiple attributes are applied**, an accessing user must be a **member of all the roles specified**. The following sample requires that a user must be a member of **both** the PowerUser and ControlPanelUser role:

```csharp
[Authorize(Roles = "PowerUser")]
[Authorize(Roles = "ControlPanelUser")]
public class ControlPanelController : Controller
{
    public IActionResult Index() =>
        Content("PowerUser && ControlPanelUser");
}
```

Access to an action can be limited by applying additional role authorization attributes at the action level:

```csharp
[Authorize(Roles = "Administrator, PowerUser")]
public class ControlAllPanelController : Controller
{
    public IActionResult SetTime() =>
        Content("Administrator || PowerUser");

    [Authorize(Roles = "Administrator")]
    public IActionResult ShutDown() =>
        Content("Administrator only");
}
```

In the preceding ControlAllPanelController controller:

- Members of the Administrator role or the PowerUser role can access the controller and the SetTime action.
- Only members of the Administrator role can access the ShutDown action.

A controller can be secured but allow anonymous, unauthenticated access to individual actions:

```csharp
[Authorize]
public class Control3PanelController : Controller
{
    public IActionResult SetTime() =>
        Content("[Authorize]");

    [AllowAnonymous]
    public IActionResult Login() =>
        Content("[AllowAnonymous]");
}
```

For Razor Pages, [Authorize] can be applied by either:

- Using a convention, or
- Applying the [Authorize] to the PageModel instance:

## Policy based role checks

Role requirements can also be expressed using the Policy syntax, where a developer registers a policy at application startup as part of the Authorization service configuration. This typically occurs in the Program.cs file:

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole",
         policy => policy.RequireRole("Administrator"));
});
```

Policies are applied using the Policy property on the [Authorize] attribute:

```csharp
[Authorize(Policy = "RequireAdministratorRole")]
public IActionResult Shutdown()
{
    return View();
}
```

To specify multiple allowed roles in a requirement, specify them as parameters to the RequireRole method

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ElevatedRights", policy =>
        policy.RequireRole("Administrator", "PowerUser", "BackupAdministrator"));
});
```

The preceding code authorizes users who belong to the Administrator, PowerUser **or** BackupAdministrator roles.

# Claims-based authorization in ASP.NET Core

## Adding claims checks

Claim-based authorization checks:

- Are declarative.
- Are applied to Razor Pages, controllers, or actions within a controller.
- Can not be applied at the Razor Page handler level, they must be applied to the Page.

Claims in code specify claims which the current user must possess, and optionally the value the claim must hold to access the requested resource. **Claims requirements are policy based; the developer must build and register a policy expressing the claims requirements.**

**The simplest type of claim policy looks for the presence of a claim and doesn't check the value.**

**Build and register the policy and call UseAuthorization. Registering the policy takes place as part of the Authorization service configuration, typically in the Program.cs file:**

```csharp
builder.Services.AddAuthorization(options =>
{
   options.AddPolicy("EmployeeOnly", policy => 
        policy.RequireClaim("EmployeeNumber"));
});

...

app.UseAuthorization();
```

In this case the EmployeeOnly policy checks for the presence of an EmployeeNumber claim on the current identity.

Apply the policy using the Policy property on the [Authorize] attribute to specify the policy name.

```csharp
[Authorize(Policy = "EmployeeOnly")]
public IActionResult VacationBalance()
{
    return View();
}

```

Most claims come with a value. You can specify a list of allowed values when creating the policy. The following example would only succeed for employees whose employee number was 1, 2, 3, 4, **or** 5.


```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Founders", policy =>
        policy.RequireClaim("EmployeeNumber", "1", "2", "3", "4", "5"));
});
```

### Add a generic claim check

If the claim value **isn't a single value** or a **transformation is required**, use **RequireAssertion**. For more information, see **Use a func to fulfill a policy**.

## Multiple Policy Evaluation

If multiple policies are applied at the controller and action levels, all policies must pass before access is granted:

```csharp
[Authorize(Policy = "EmployeeOnly")]
public class SalaryController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Payslip()
    {
        return View();
    }

    [Authorize(Policy = "HumanResources")]
    public IActionResult UpdateSalary()
    {
        return View();
    }
}
```

In the preceding example any identity which fulfills the EmployeeOnly policy can access the Payslip action as that policy is enforced on the controller. However, in order to call the UpdateSalary action the identity must fulfill **both** the EmployeeOnly policy and the HumanResources policy.

If you want more complicated policies, such as taking a date of birth claim, calculating an age from it, and then checking that the age is 21 or older, then you need **to write custom policy handlers**.

In the following sample, both page handler methods must fulfill both the EmployeeOnly policy and the HumanResources policy

```csharp
[Authorize(Policy = "EmployeeOnly")]
[Authorize(Policy = "HumanResources")]
public class SalaryModel : PageModel
{
    public ContentResult OnGetPayStub()
    {
        return Content("OnGetPayStub");
    }

    public ContentResult OnGetSalary()
    {
        return Content("OnGetSalary");
    }
}
```

# Policy-based authorization in ASP.NET Core

Underneath the covers, role-based authorization and claims-based authorization **use a requirement, a requirement handler, and a preconfigured policy**. These **building blocks** support the expression of authorization evaluations in code. The result is a richer, reusable, testable authorization structure.

**An authorization policy consists of one or more requirements. Register it as part of the authorization service configuration, in the app's Program.cs file:**

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AtLeast21", policy =>
        policy.Requirements.Add(new MinimumAgeRequirement(21)));
});
```

In the preceding example, an "AtLeast21" policy is created. It has a single requirement — that of a minimum age, which is supplied as a parameter to the requirement.

## IAuthorizationService

The primary service that determines if authorization is successful is IAuthorizationService:

```csharp
/// <summary>
/// Checks policy based permissions for a user
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Checks if a user meets a specific set of requirements for the specified resource
    /// </summary>
    Task<AuthorizationResult> AuthorizeAsync(
        ClaimsPrincipal user, object resource, 
        IEnumerable<IAuthorizationRequirement> requirements);

    /// <summary>
    /// Checks if a user meets a specific authorization policy
    /// </summary>
    Task<AuthorizationResult> AuthorizeAsync(
        ClaimsPrincipal user, object resource, string policyName);
}
```

The preceding code highlights the two methods of the **IAuthorizationService**.

**IAuthorizationRequirement is a marker service with no methods, and the mechanism for tracking whether authorization is successful.**

**Each IAuthorizationHandler is responsible for checking if requirements are met:**

```csharp
/// <summary>
/// Classes implementing this interface are able to make a decision if authorization
/// is allowed.
/// </summary>
public interface IAuthorizationHandler
{
    /// <summary>
    /// Makes a decision if authorization is allowed.
    /// </summary>
    Task HandleAsync(AuthorizationHandlerContext context);
}
```

The **AuthorizationHandlerContext** class is what the handler uses **to mark whether requirements have been met**:

```csharp
context.Succeed(requirement)
```

The following code shows the simplified (and annotated with comments) default implementation of the authorization service:

```csharp
public async Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, 
    object resource, IEnumerable<IAuthorizationRequirement> requirements)
{
    // Create a tracking context from the authorization inputs.
    var authContext = _contextFactory.CreateContext(requirements, user, resource);

    // By default this returns an IEnumerable<IAuthorizationHandler> from DI.
    var handlers = await _handlers.GetHandlersAsync(authContext);

    // Invoke all handlers.
    foreach (var handler in handlers)
    {
        await handler.HandleAsync(authContext);
    }

    // Check the context, by default success is when all requirements have been met.
     return _evaluator.Evaluate(authContext);
}
```

The following code shows a typical authorization service configuration:

```csharp
// Add all of your handlers to DI.
builder.Services.AddSingleton<IAuthorizationHandler, MyHandler1>();

// MyHandler2, ...
builder.Services.AddSingleton<IAuthorizationHandler, MyHandlerN>();

// Configure your policies
builder.Services.AddAuthorization(options =>
      options.AddPolicy("Something",
        policy => policy.RequireClaim("Permission", 
            "CanViewPage", "CanViewAnything")));
```

Use **IAuthorizationService**, **[Authorize(Policy = "Something")]**, or **RequireAuthorization("Something")** for authorization.

## Apply policies to endpoints

Apply policies to endpoints by using **RequireAuthorization** with the policy name. For example:

```csharp
app.MapGet("/helloworld", () => "Hello World!")
    .RequireAuthorization("AtLeast21");
```

# Requirements

An **authorization requirement** is a **collection of data parameters** that **a policy can use to evaluate the current user principal**. In our "AtLeast21" policy, **the requirement is** a single parameter—the minimum age. **A requirement implements IAuthorizationRequirement, which is an empty marker interface**. A parameterized minimum age requirement could be implemented as follows

```csharp
public class MinimumAgeRequirement : IAuthorizationRequirement
{
    public MinimumAgeRequirement(int minimumAge) =>
        MinimumAge = minimumAge;

    public int MinimumAge { get; }
}
```

**If an authorization policy contains multiple authorization requirements, all requirements must pass in order for the policy evaluation to succeed. In other words, multiple authorization requirements added to a single authorization policy are treated on an AND basis.**

> A requirement doesn't need to have data or properties

# Authorization handlers

**An authorization handler is responsible for the evaluation of a requirement's properties. The authorization handler evaluates the requirements against a provided AuthorizationHandlerContext to determine if access is allowed.**

**A requirement can have multiple handlers. A handler may inherit AuthorizationHandler\<TRequirement>, where TRequirement is the requirement to be handled. Alternatively, a handler may implement IAuthorizationHandler directly to handle more than one type of requirement.**

## Use a handler for one requirement

The following example shows a one-to-one relationship in which a minimum age handler handles a single requirement

```csharp
public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        MinimumAgeRequirement requirement)
    {
        var dateOfBirthClaim = context.User.FindFirst(
            c => c.Type == ClaimTypes.DateOfBirth && c.Issuer == "http://contoso.com");

        if (dateOfBirthClaim is null)
        {
            return Task.CompletedTask;
        }

        int calculatedAge = calcAge(...);

        if (calculatedAge >= requirement.MinimumAge)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
```

The preceding code determines if the current user principal has a date of birth claim that has been issued by a known and trusted Issuer. Authorization can't occur when the claim is missing, in which case a completed task is returned. When a claim is present, the user's age is calculated. If the user meets the minimum age defined by the requirement, authorization is considered successful. When authorization is successful, context.Succeed is invoked with the satisfied requirement as its sole parameter.

## Use a handler for multiple requirements

The following example shows a one-to-many relationship in which a permission handler can handle three different types of requirements

```csharp
public class PermissionHandler : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        var pendingRequirements = context.PendingRequirements.ToList();
        foreach (var requirement in pendingRequirements)
        {
            if (requirement is ReadPermission)
            {
                if (IsOwner(context.User, context.Resource)
                    || IsSponsor(context.User, context.Resource))
                {
                    context.Succeed(requirement);
                }
            }
            else if (requirement is EditPermission || requirement is DeletePermission)
            {
                if (IsOwner(context.User, context.Resource))
                {
                    context.Succeed(requirement);
                }
            }
        }
        return Task.CompletedTask;
    }

    private static bool IsOwner(ClaimsPrincipal user, object? resource)
    {
        // Code omitted for brevity
        return true;
    }

    private static bool IsSponsor(ClaimsPrincipal user, object? resource)
    {
        // Code omitted for brevity
        return true;
    }
}
```

The preceding code traverses **PendingRequirements — a property containing requirements not marked as successful**. For a **ReadPermission** requirement, the user must be either an owner or a sponsor to access the requested resource. For an **EditPermission** or **DeletePermission** requirement, they must be an owner to access the requested resource.

## Handler registration

Register handlers in the services collection during configuration. For example:

```csharp
builder.Services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
```

The preceding code registers MinimumAgeHandler as a singleton. Handlers can be registered using any of the built-in service lifetimes.

**It's possible to bundle both a requirement and a handler into a single class implementing both IAuthorizationRequirement and IAuthorizationHandler. This bundling creates a tight coupling between the handler and requirement and is only recommended for simple requirements and handlers. Creating a class that implements both interfaces removes the need to register the handler in DI because of the built-in PassThroughAuthorizationHandler that allows requirements to handle themselves.**

**See the AssertionRequirement class for a good example where the AssertionRequirement is both a requirement and the handler in a fully self-contained class.**

## What should a handler return?

Note that the Handle method in the handler example returns no value. How is a status of either success or failure indicated?

- **A handler indicates success by calling context.Succeed(IAuthorizationRequirement requirement), passing the requirement that has been successfully validated.**
- **A handler doesn't need to handle failures generally, as other handlers for the same requirement may succeed.**
- **To guarantee failure, even if other requirement handlers succeed, call context.Fail.**

If a handler calls context.Succeed or context.Fail, **all other handlers are still called**. This allows requirements to produce side effects, such as logging, which takes place even if another handler has successfully validated or failed a requirement. **When set to false, the InvokeHandlersAfterFailure property short-circuits the execution of handlers when context.Fail is called. InvokeHandlersAfterFailure defaults to true, in which case all handlers are called.**

**Authorization handlers are called even if authentication fails. Also handlers can execute in any order, so do not depend on them being called in any particular order.**


## Why would I want multiple handlers for a requirement?

**In cases where you want evaluation to be on an OR basis, implement multiple handlers for a single requirement**. For example, Microsoft has doors that only open with key cards. If you leave your key card at home, the receptionist prints a temporary sticker and opens the door for you. In this scenario, you'd have a single requirement, BuildingEntry, but multiple handlers, each one examining a single requirement.

```csharp
public class BuildingEntryRequirement : IAuthorizationRequirement { }
```

```csharp
public class BadgeEntryHandler : AuthorizationHandler<BuildingEntryRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        BuildingEntryRequirement requirement)
    {
        if (context.User.HasClaim(
            c => c.Type == "BadgeId" && c.Issuer == "https://microsoftsecurity"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
```

```csharp
public class TemporaryStickerHandler : AuthorizationHandler<BuildingEntryRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        BuildingEntryRequirement requirement)
    {
        if (context.User.HasClaim(
            c => c.Type == "TemporaryBadgeId" && c.Issuer == "https://microsoftsecurity"))
        {
            // Code to check expiration date omitted for brevity.
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
```

Ensure that both handlers are registered. If either handler succeeds when a policy evaluates the BuildingEntryRequirement, the policy evaluation succeeds.

## Use a func to fulfill a policy

There may be situations in which fulfilling a policy is simple to express in code. It's possible to supply a **Func<AuthorizationHandlerContext, bool>** when configuring a policy with the **RequireAssertion** policy builder.

For example, the previous BadgeEntryHandler could be rewritten as follows:

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BadgeEntry", policy =>
        policy.RequireAssertion(context => 
            context.User.HasClaim(c => (c.Type == "BadgeId" || c.Type == "TemporaryBadgeId")
                && c.Issuer == "https://microsoftsecurity")));
});
```

# Access MVC request context in handlers

The **HandleRequirementAsync** method has two parameters: an **AuthorizationHandlerContext** and the **TRequirement** being handled. **Frameworks such as MVC or SignalR are free to add any object to the Resource property on the AuthorizationHandlerContext to pass extra information**.

**When using endpoint routing, authorization is typically handled by the Authorization Middleware. In this case, the Resource property is an instance of HttpContext. The context can be used to access the current endpoint, which can be used to probe the underlying resource to which you're routing. For example:**.

```csharp
if (context.Resource is HttpContext httpContext)
{
    var endpoint = httpContext.GetEndpoint();
    var actionDescriptor = 
        endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
    ...
}
```

**With traditional routing, or when authorization happens as part of MVC's authorization filter, the value of Resource is an AuthorizationFilterContext instance. This property provides access to HttpContext, RouteData, and everything else provided by MVC and Razor Pages.**

**The use of the Resource property is framework-specific. Using information in the Resource property limits your authorization policies to particular frameworks. Cast the Resource property using the is keyword, and then confirm the cast has succeeded to ensure your code doesn't crash with an frameworks:**

```csharp
if (context.Resource is AuthorizationFilterContext mvcContext)
{
    // Examine MVC-specific things like routing data.
}
```

# Custom Authorization Policy Providers using IAuthorizationPolicyProvider in ASP.NET Core

Typically when using policy-based authorization, **policies are registered by calling AuthorizationOptions.AddPolicy** as part of authorization service configuration. In some scenarios, it may not be possible (or desirable) to register all authorization policies in this way. In those cases, you can use a custom **IAuthorizationPolicyProvider** to control how authorization policies are supplied.

Examples of scenarios where a custom IAuthorizationPolicyProvider may be useful include:

- Using an external service to provide policy evaluation.

- Using a large range of policies (for different room numbers or ages, for example), so it doesn't make sense to add each individual authorization policy with an AuthorizationOptions.AddPolicy call.

- Creating policies at runtime based on information in an external data source (like a database) or determining authorization requirements dynamically through another mechanism

## Customize policy retrieval

**ASP.NET Core apps use an implementation of the IAuthorizationPolicyProvider interface to retrieve authorization policies**. By default, **DefaultAuthorizationPolicyProvider** is registered and used. **DefaultAuthorizationPolicyProvider returns policies from the  provided in an IServiceCollection.AddAuthorization call**.

Customize this behavior by **registering a different IAuthorizationPolicyProvider implementation** in the app's dependency injection container.

The IAuthorizationPolicyProvider interface contains three APIs:

- **GetPolicyAsync** returns an authorization policy for a given name.

- **GetDefaultPolicyAsync** returns the default authorization policy (the policy used for **[Authorize] attributes** without a policy specified).

- **GetFallbackPolicyAsync** returns the fallback authorization policy (the policy used by the Authorization Middleware **when no policy is specified**).

**By implementing these APIs, you can customize how authorization policies are provided**

## Parameterized authorize attribute example

One scenario where **IAuthorizationPolicyProvider** is useful is **enabling custom [Authorize] attributes whose requirements depend on a parameter**. For example, in policy-based authorization documentation, an age-based (“AtLeast21”) policy was used as a sample. If different controller actions in an app should be made available to users of different ages, it might be useful to have many different age-based policies. **Instead of registering all the different age-based policies that the application will need in AuthorizationOptions, you can generate the policies dynamically with a custom IAuthorizationPolicyProvider. To make using the policies easier, you can annotate actions with custom authorization attribute like [MinimumAgeAuthorize(20)].**

## Custom Authorization attributes

**Authorization policies are identified by their names**. The custom MinimumAgeAuthorizeAttribute described previously needs **to map arguments into a string that can be used to retrieve the corresponding authorization policy**. You can do this by **deriving from AuthorizeAttribute** and **making the Age property wrap the AuthorizeAttribute.Policy property**.

```csharp
internal class MinimumAgeAuthorizeAttribute : AuthorizeAttribute
{
    const string POLICY_PREFIX = "MinimumAge";

    public MinimumAgeAuthorizeAttribute(int age) => Age = age;

    // Get or set the Age property by manipulating the underlying Policy property
    public int Age
    {
        get
        {
            if (int.TryParse(Policy.Substring(POLICY_PREFIX.Length), out var age))
            {
                return age;
            }
            return default(int);
        }
        set
        {
            Policy = $"{POLICY_PREFIX}{value.ToString()}";
        }
    }
}
```

This attribute type has a Policy string based on the hard-coded prefix (and an integer passed in via the constructor.

You can apply it to actions in the same way as other takes an integer as a parameter.

```csharp
[MinimumAgeAuthorize(10)]
public IActionResult RequiresMinimumAge10()
```

## Custom IAuthorizationPolicyProvider

The custom MinimumAgeAuthorizeAttribute makes it easy to request authorization policies for any minimum age desired. **The next problem to solve is making sure that authorization policies are available for all of those different ages. This is where an IAuthorizationPolicyProvider is useful.**

When using MinimumAgeAuthorizationAttribute, **the authorization policy names will follow the pattern "MinimumAge" + Age, so the custom IAuthorizationPolicyProvider should generate authorization policies by:**

- Parsing the age from the policy name.

- **Using AuthorizationPolicyBuilder to create a new AuthorizationPolicy**

- In this and following examples it will be assumed that the user is authenticated via a cookie. The AuthorizationPolicyBuilder should either be constructed with at least one authorization scheme name or always succeed. Otherwise there is no information on how to provide a challenge to the user and an exception will be thrown.

- Adding requirements to the policy based on the age with **AuthorizationPolicyBuilder.AddRequirements**. In other scenarios, you might use RequireClaim, RequireRole, or RequireUserName instead

```csharp
internal class MinimumAgePolicyProvider : IAuthorizationPolicyProvider
{
    const string POLICY_PREFIX = "MinimumAge";

    // Policies are looked up by string name, so expect 'parameters' (like age)
    // to be embedded in the policy names. This is abstracted away from developers
    // by the more strongly-typed attributes derived from AuthorizeAttribute
    // (like [MinimumAgeAuthorize()] in this sample)
    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase) 
            && int.TryParse(policyName.Substring(POLICY_PREFIX.Length), out var age))
        {
            var policy = new AuthorizationPolicyBuilder(
                CookieAuthenticationDefaults.AuthenticationScheme);
            policy.AddRequirements(new MinimumAgeRequirement(age));
            return Task.FromResult(policy.Build());
        }
        return Task.FromResult<AuthorizationPolicy>(null);
    }
}
```

## Multiple authorization policy providers

**When using custom IAuthorizationPolicyProvider implementations, keep in mind that ASP.NET Core only uses one instance of IAuthorizationPolicyProvider. If a custom provider isn't able to provide authorization policies for all policy names that will be used, it should defer to a backup provider.**

For example, consider an application that needs both custom age policies and more traditional role-based policy retrieval. Such an app could use a custom authorization policy provider that:

- Attempts to parse policy names.
- C**alls into a different policy provider** (like DefaultAuthorizationPolicyProvider) if the policy name doesn't contain an age.

The example IAuthorizationPolicyProvider implementation shown above can be updated to use the DefaultAuthorizationPolicyProvider by creating a backup policy provider in its constructor (to be used in case the policy name doesn't match its expected pattern of 'MinimumAge' + age).

```csharp
private DefaultAuthorizationPolicyProvider BackupPolicyProvider { get; }

public MinimumAgePolicyProvider(IOptions<AuthorizationOptions> options)
{
    // ASP.NET Core only uses one authorization policy provider, 
    // so if the custom implementation doesn't handle all policies 
    // it should fall back to an alternate provider.
    BackupPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
}
```

Then, the GetPolicyAsync method can be updated to use the BackupPolicyProvider instead of returning null.

```csharp
...
return BackupPolicyProvider.GetPolicyAsync(policyName);
```

## Default policy

In addition to providing named authorization policies, a custom IAuthorizationPolicyProvider needs to **implement GetDefaultPolicyAsync to provide an authorization policy for [Authorize] attributes without a policy name specified**.

**In many cases, this authorization attribute only requires an authenticated user, so you can make the necessary policy with a call to RequireAuthenticatedUser:**

```csharp
public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => 
    Task.FromResult(
        new AuthorizationPolicyBuilder(
            CookieAuthenticationDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build()
        );
```

As with all aspects of a custom IAuthorizationPolicyProvider, you can customize this, as needed. In some cases, it may be desirable to retrieve the default policy from a fallback IAuthorizationPolicyProvider.

## Fallback policy

A custom IAuthorizationPolicyProvider can **optionally** implement GetFallbackPolicyAsync to provide a policy that's **used when combining policies and when no policies are specified. If GetFallbackPolicyAsync returns a non-null policy, the returned policy is used by the Authorization Middleware when no policies are specified for the request.**

If no fallback policy is required, the provider can return null or defer to the fallback provider:

```csharp
public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => 
    Task.FromResult<AuthorizationPolicy>(null);
```

## Use a custom IAuthorizationPolicyProvider

To use custom policies from an IAuthorizationPolicyProvider, you must:

- Register the appropriate AuthorizationHandler types with dependency injection (described in policy-based authorization), as with all policy-based authorization scenarios.

- Register the custom IAuthorizationPolicyProvider type in the app's dependency injection service collection in Startup.ConfigureServices to replace the default policy provider.

```csharp
services.AddSingleton<IAuthorizationPolicyProvider, MinimumAgePolicyProvider>();
```

# Customize the behavior of AuthorizationMiddleware

Apps can register an **IAuthorizationMiddlewareResultHandler** to customize how **AuthorizationMiddleware** handles authorization results. Apps can use IAuthorizationMiddlewareResultHandler to:

- Return customized responses.
- Enhance the default challenge or forbid responses.

The following code shows an example implementation of IAuthorizationMiddlewareResultHandler that returns a custom response for specific authorization failures:

```csharp
public class SampleAuthorizationMiddlewareResultHandler : 
    IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        // If the authorization was forbidden and the resource 
        // had a specific requirement, provide a custom 404 response.
        if (authorizeResult.Forbidden
            && authorizeResult.AuthorizationFailure!.FailedRequirements
                .OfType<Show404Requirement>().Any())
        {
            // Return a 404 to make it appear as if the resource doesn't exist.
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }
        // Fall back to the default implementation.
        await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}

public class Show404Requirement : IAuthorizationRequirement { }
```
Register this implementation of IAuthorizationMiddlewareResultHandler in Program.cs

```csharp
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, SampleAuthorizationMiddlewareResultHandler>();
```

# Dependency injection in requirement handlers in ASP.NET Core