﻿using Microsoft.AspNetCore.Authorization;

namespace Application.Keycloak
{
    public class RptRequirement : IAuthorizationRequirement
    {
        public string Resource { get; }
        public string Scope { get; }

        public RptRequirement(string resource, string scope)
        {
            Resource = resource;
            Scope = scope;
        }
    }
}
