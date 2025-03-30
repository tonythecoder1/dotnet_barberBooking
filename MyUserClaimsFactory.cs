using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using dotidentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace dotidentity
{
    public class MyUserClaimsFactory : UserClaimsPrincipalFactory<MyUser>
    {
        public MyUserClaimsFactory(UserManager<MyUser> userManager, IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor)
        {

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(MyUser user)
        {
            var Identity = await base.GenerateClaimsAsync(user);
            Identity.AddClaim(new Claim("Member", user.Member));
            return Identity;
        }
        
    }
}