using Microsoft.AspNetCore.StaticAssets;
using Security.WebApp.Components;
using Security.WebAppLib;

namespace Security.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            IRazorComponentsBuilder b1 = builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            AppService appService = new(builder.Services, builder.Configuration);
            appService.AddMyIdentity();
            appService.AddMyOtherServices();
            appService.AddMyAuthorization();

            WebApplication? app = builder.Build();
            IApplicationBuilder app2 = app;
            IEndpointRouteBuilder app3 = app;

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app2.UseWebAssemblyDebugging();
            }
            else
            {
                app2 = app2.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app2 = app2.UseHsts();
            }

            app2 = app2.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app2 = app2.UseHttpsRedirection();

            app2 = app2.UseAntiforgery();

            app2 = app2.UseAuthentication();
            app2 = app2.UseAuthorization();

            StaticAssetsEndpointConventionBuilder b2 = app3.MapStaticAssets();
            RazorComponentsEndpointConventionBuilder b3 = app3.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Run();
        }
    }
}
