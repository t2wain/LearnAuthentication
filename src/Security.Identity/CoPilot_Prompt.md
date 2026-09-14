I want to learn about Asp.Net Idendity Core business logic. Below is the topic I want to learn.

- I do not want to use EntityFramework for data access. I have my own custom logic to get data for a user.
- I want to use cookies authentication sheme. 
- I want to use SignInManager

Please explain to me about the following:

- What nuget packages I need to install.
- How to configure the application for authenticaion.
- How to use SignInManger to issue a authentication cookie
- Explain what data is stored in the authentication cookie
- Explain SecurityStamp concept. Is is just a time stamp about the last update to user profile?
- How does UserClaimsPrincipalFactory\<TUser> obtain the claims from the User object? Does it call the custom store?
- My custom store implement all standard store interfaces. How should I register the custom store with DI for the cookie authentication scheme?
- When sign-in with SignInManager, I can create a ClaimPrincipal object based
- Can Microsoft.Extensions.Identity.Core be used in Windows desktop application?
- Is there a corresponding concept similar to Microsoft.AspNetCore.Authorization for desktop application?
- Is there already a class th