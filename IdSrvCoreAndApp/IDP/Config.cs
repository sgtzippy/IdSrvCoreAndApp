﻿using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IDP
{
    public static class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "ae128610-ee36-4443-8ba9-8ecc280705eb",
                    Username = "Admin",
                    Password = "Password",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Application"),
                        new Claim("family_name", "Admin"),
                        new Claim("address", "1234 Admin Blvd"),
                        new Claim("role", "Admin")
                    }
                },
                
                new TestUser
                {
                    SubjectId = "4abe534d-71f9-4927-8381-373a89ece735",
                    Username = "User",
                    Password = "Password",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Application"),
                        new Claim("family_name", "User"),
                        new Claim("address", "4321 User Way"),
                        new Claim("role", "User")
                    }
                }
            };
        }

        // Identity-related resources (scopes)
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResource(
                    "roles",
                    "Your role(s)",
                    new List<string> { "role" }
                )
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client
                {
                    ClientName = "Application1",
                    ClientId = "Application1Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44342/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:44342/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                }
            };
        }
    }
}
