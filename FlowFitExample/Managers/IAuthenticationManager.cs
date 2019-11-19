using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FlowFitExample.Models;

namespace FlowFitExample.Managers
{
    interface IAuthenticationManager
    {
        Task<bool> Authenticate(LoginRequest loginRequest);
        Task<ClaimsPrincipal> GetClaimsPrincipalAsync(string userName);
        string GetToken(ClaimsPrincipal principal);
    }
}
