using Employee_Management.Utils;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Employee_Management
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=868025
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions() { ExpireTimeSpan = TimeSpan.FromSeconds(45) });
            
            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    // The `Authority` represents the v2.0 endpoint - https://login.microsoftonline.com/common/v2.0
                    Authority = AuthenticationConfig.Authority,
                    ClientId = AuthenticationConfig.ClientId,
                    RedirectUri = AuthenticationConfig.RedirectUri,
                    PostLogoutRedirectUri = AuthenticationConfig.RedirectUri,
                    Scope = AuthenticationConfig.BasicSignInScopes + " Mail.Read", // a basic set of permissions for user sign in & profile access "openid profile offline_access"
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        AudienceValidator = (audience, token, tvp) =>
                        {
                            if (MyCustomAppValidation(audience))
                            {
                                return true;
                            }
                            else
                            {
                                throw new SecurityTokenInvalidAudienceException("Invalid Audience");
                            }
                        },
                        ValidateAudience = true,
                        IssuerValidator = (issuer, token, tvp) =>
                        {
                            if (MyCustomTenantValidation(issuer))
                            {
                                return issuer;
                            }
                            else
                            {
                                throw new SecurityTokenInvalidIssuerException("Invalid issuer");
                            }
                        },
                        NameClaimType = "name",
                    },
                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {
                        AuthorizationCodeReceived = OnAuthorizationCodeReceived,
                        AuthenticationFailed = OnAuthenticationFailed,
                    }
                });
        }

        private bool MyCustomAppValidation(IEnumerable<string> audience)
        {
            return audience.ToList().Where(aud => aud == ConfigurationManager.AppSettings["ida:Audience"]
            || aud == "f2ef30f1-802f-4c2b-a40e-b8c4a02a7f3e")
            .ToList().Count > 0;
        }

        private bool MyCustomTenantValidation(string issuer)
        {
            return issuer == "https://login.microsoftonline.com/5f43b63f-274d-4af6-8d92-b05528850f33/v2.0";
        }

        private async Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedNotification context)
        {
            IConfidentialClientApplication clientApp = ConfidentialClientApplicationBuilder.Create(AuthenticationConfig.ClientId)
                 .WithClientSecret(AuthenticationConfig.ClientSecret)
                 .WithRedirectUri(AuthenticationConfig.RedirectUri)
                 .WithAuthority(new Uri(AuthenticationConfig.Authority))
                 .Build();

            AuthenticationResult result = await clientApp.AcquireTokenByAuthorizationCode(new[] { "Mail.Read" }
            , context.Code).ExecuteAsync();
        }

        private Task OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            notification.HandleResponse();
            notification.Response.Redirect("/Error?message=" + notification.Exception.Message);
            return Task.FromResult(0);
        }
    }
}
