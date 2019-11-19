using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FlowFitExample.Models;
using Microsoft.Extensions.Logging;

namespace FlowFitExample.Managers
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private ILogger<AuthenticationManager> _logger;
        

        public Task<bool> Authenticate(LoginRequest loginRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ClaimsPrincipal> GetClaimsPrincipalAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public string GetToken(ClaimsPrincipal principal)
        {
            throw new NotImplementedException();
        }
    }
}
