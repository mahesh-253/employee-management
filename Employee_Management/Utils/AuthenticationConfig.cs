using System.Configuration;
using System.Globalization;

namespace Employee_Management.Utils
{
    public static class AuthenticationConfig
    {
        public static string IssuerClaim { get; } = "iss";

        public static string TenantIdClaimType { get; } = "http://schemas.microsoft.com/identity/claims/tenantid";

        public static string MicrosoftGraphGroupsApi { get; } = "https://graph.microsoft.com/v1.0/groups";

        public static string MicrosoftGraphUsersApi { get; } = "https://graph.microsoft.com/v1.0/users";

        public static string AdminConsentFormat { get; } = "https://login.microsoftonline.com/{0}/adminconsent?client_id={1}&state={2}&redirect_uri={3}";

        public static string BasicSignInScopes { get; } = "openid profile offline_access";

        public static string NameClaimType { get; } = "name";

        public static string ClientId { get; } = ConfigurationManager.AppSettings["ida:ClientId"];

        public static string ClientSecret { get; } = ConfigurationManager.AppSettings["ida:ClientSecret"];

        public static string RedirectUri { get; } = ConfigurationManager.AppSettings["ida:RedirectUri"];

        public static string AADInstance { get; } = ConfigurationManager.AppSettings["ida:AADInstance"];

        public static string Authority = string.Format(CultureInfo.InvariantCulture, AADInstance, "common", "/v2.0");
    }
}