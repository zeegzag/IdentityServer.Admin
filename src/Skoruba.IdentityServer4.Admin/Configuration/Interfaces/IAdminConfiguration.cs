﻿namespace Skoruba.IdentityServer4.Admin.Configuration.Interfaces
{
    public interface IAdminConfiguration
    {
        string IdentityAdminRedirectUri { get; }
        string IdentityServerBaseUrl { get; }
        string ClientId { get; }
        string ClientSecret { get; }
        string OidcResponseType { get; }
        string[] Scopes { get; }
        string AdministrationRole { get; }
        bool RequireHttpsMetadata { get; }
        string IdentityAdminCookieName { get; }
        double IdentityAdminCookieExpiresUtcHours { get; }
        string TokenValidationClaimName { get; }
        string TokenValidationClaimRole { get; }
    }
}