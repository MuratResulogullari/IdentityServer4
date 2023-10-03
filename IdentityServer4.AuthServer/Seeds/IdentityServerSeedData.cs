using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;

namespace IdentityServer4.AuthServer.Seeds
{
    public static class IdentityServerSeedData
    {
        public static void Seed(ConfigurationDbContext context)
        {
            if (!context.ApiResources.Any())
            {
                foreach (var apiResource in Config.GetApiResources())
                {
                    context.ApiResources.Add(apiResource.ToEntity());
                }
            }
            if (!context.ApiScopes.Any())
            {
                Config.GetApiScopes().ToList().ForEach(apiscope =>
                {
                    context.ApiScopes.Add(apiscope.ToEntity());
                });
            }
            if (!context.Clients.Any())
            {
                foreach (var client in Config.GetClients())
                {

                    context.Clients.Add(client.ToEntity());

                }
            }
            if (!context.IdentityResources.Any())
            {
                foreach (var identityResource in Config.GetIdentityResources())
                {
                    context.IdentityResources.Add(identityResource.ToEntity());
                }
            }

            context.SaveChanges();
        }
    }
}
