using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Application1.Services.Contract.Interfaces;
using Application1.Services.Services;
using Application1.Repositories.Contract.Interfaces;
using Application1.Repositories.Repositories;

namespace Application1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.AddPolicy(
                    "CanViewSubscription",
                    policyBuilder =>
                    {
                        policyBuilder.RequireAuthenticatedUser();
                        policyBuilder.RequireClaim("company", "Big Corporate 1", "Small Startup 4");
                        policyBuilder.RequireClaim("subscriptionlevel", "PayingUser");
                    }
                );
            });
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IService1, Service1>();
            services.AddSingleton<IRepository1, Repository1>();
            services.AddSingleton<IRepository2, Repository2>();
            services.AddScoped<IApplication1HttpClient, Application1HttpClient>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies", options =>
                {
                    options.AccessDeniedPath = "/Authorization/AccessDenied";
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";
                    options.Authority = "https://localhost:44300/";
                    options.ClientId = "Application1Client";
                    options.ResponseType = "code id_token";
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("address");
                    options.Scope.Add("roles");
                    options.Scope.Add("company");
                    options.Scope.Add("subscriptionlevel");
                    options.Scope.Add("application1api");
                    options.Scope.Add("offline_access");
                    options.SaveTokens = true;
                    options.ClientSecret = "secret";
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.ClaimActions.Remove("amr");
                    options.ClaimActions.DeleteClaim("sid");
                    options.ClaimActions.DeleteClaim("idp");
                    options.ClaimActions.MapUniqueJsonKey("role", "role");
                    options.ClaimActions.MapUniqueJsonKey("company", "company");
                    options.ClaimActions.MapUniqueJsonKey("subscriptionlevel", "subscriptionlevel");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.GivenName,
                        RoleClaimType = JwtClaimTypes.Role
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
