using IdentiyServer4.Identity.Api.Entities;
using IdentiyServer4.Identity.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace IdentiyServer4.Identity.Api.Services
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
        public override async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return await Users.Where(x => x.Id == userId).Include(x => x.UserClaims).Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefaultAsync();
        }
        public List<ClaimDto> GetUserProfileClaims(ClaimsPrincipal principal)
        {
            //var principal = await _claimsFactory.CreateAsync(user);


            var allClaims = principal.Claims.Select(a => new ClaimDto
            {
                Type = a.Type,
                Value = a.Value
            }).ToList();

            var roleClaims = allClaims.Where(x => x.Type.Contains("Controller")).Select(a => new
            {
                Type = a.Type,
                Value = JsonSerializer.Deserialize<RoleClaimValueDto>(a.Value)
            }).ToList();



            var userClaims = allClaims.Where(x => !x.Type.Contains("Controller")).ToList();

            var groupedRoleClaims = roleClaims.GroupBy(x => x.Type).ToList();

            if (groupedRoleClaims.Any(x => x.Count() > 1))
            {
                var uniqueRoleClaims = new List<ClaimDto>();

                var pluralRoleClaims = groupedRoleClaims.Where(x => x.Count() > 1).ToList();

                foreach (var item in pluralRoleClaims)
                {
                    if (item.Any(x => x.Value.SelectionType == RoleClaimTypes.SelectionAll))
                    {
                        uniqueRoleClaims.Add(new ClaimDto
                        {
                            Type = item.Key,
                            Value = JsonSerializer.Serialize(item.FirstOrDefault(x => x.Value.SelectionType == RoleClaimTypes.SelectionAll).Value)
                        });
                    }
                    else // all types partially
                    {
                        List<string> permittedValues = new List<string>();

                        foreach (var subItem in item)
                        {
                            permittedValues.AddRange(subItem.Value.PermittedValues);
                        }

                        var uniquePermittedValues = permittedValues.Distinct().ToList();

                        var roleClaim = new RoleClaimValueDto
                        {
                            SelectionType = RoleClaimTypes.SelectionPartially,
                            PermittedValues = uniquePermittedValues
                        };

                        uniqueRoleClaims.Add(new ClaimDto
                        {
                            Type = item.Key,
                            Value = JsonSerializer.Serialize(roleClaim)
                        });
                    }
                }

                var singleRoleClaims = groupedRoleClaims.Where(x => x.Count() == 1).ToList();

                foreach (var item in singleRoleClaims)
                {
                    uniqueRoleClaims.Add(new ClaimDto
                    {
                        Type = item.Key,
                        Value = JsonSerializer.Serialize<RoleClaimValueDto>(item.FirstOrDefault().Value)
                    });
                }

                return userClaims.Union(uniqueRoleClaims).ToList();
            }

            return allClaims;
        }

    }
}
