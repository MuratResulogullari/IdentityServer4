using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace IdentityServer4.AuthServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>() {
                new ApiResource("resource_api1")  {   // Basic Auth sağlarken Username
                    ApiSecrets=new[]{ new Secret("secretapi1".Sha256())}, // Basic Auth sağlarken  Password
                    Scopes={"api1.read", "api1.write", "api1.update" },
                },
                new ApiResource("resource_api2") { // Basic Auth sağlarken Username
                    ApiSecrets=new[]{ new Secret("secretapi2".Sha256())}, // Basic Auth sağlarken  Password
                    Scopes={"api2.read", "api2.write", "api2.update" },
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>() {
                new ApiScope("api1.read","API 1 için okuma izni"),
                new ApiScope("api1.write","API 1 için yazma izni"),
                new ApiScope("api1.update","API 1 için güncelleme izni"),
                new ApiScope("api2.read","API 2 için okuma izni"),
                new ApiScope("api2.write","API 2 için yazma izni"),
                new ApiScope("api2.update","API 2 için güncelleme izni"),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>() {
                new Client()
                {
                    ClientId="WebAppMVC1",
                    ClientName="Web MVC 1",
                    ClientSecrets=new [] {new  Secret("secret".ToSha256()) },
                    AllowedGrantTypes={GrantType.ClientCredentials},
                    AllowedScopes={"api1.read" }
                },
                 new Client()
                {
                    ClientId="WebAppMVC2",
                    ClientName="Web  MVC 2",
                    ClientSecrets=new [] {new  Secret("secret".ToSha256()) },
                    AllowedGrantTypes={GrantType.ClientCredentials},
                    AllowedScopes={"api1.read","api1.update","api2.write","api2.update"}
                }
            };
        }

        //    public static IEnumerable<IdentityResource> GetIdentityResources()
        //    {
        //        return new List<IdentityResource>
        //        {
        //            new IdentityResources.OpenId(),//subId
        //            new IdentityResources.Profile()// Kullanıcı hakkında claimleri tutar name,family_name,given_ame,midlle_name,nickname,preferred_username,profile,picture,website,gender,birthdate,zoneinfo,locale,updated_at
        //        };
        //    }

        //    public static IEnumerable<TestUser> GetUsers()
        //    {
        //        return new List<TestUser>()
        //        {
        //            new TestUser()
        //            {
        //                SubjectId="1",
        //                Username="murat.resulogullari",
        //                Password="123.Mr*",
        //                Claims=new List<Claim> {
        //                    new Claim("given_name","Murat"),
        //                    new Claim("family_name","Resuloğulları"),
        //                    new Claim("updated_at",DateTime.Now.ToString()),
        //                }
        //            },
        //             new TestUser()
        //            {
        //                SubjectId="2",
        //                Username="ali.veli",
        //                Password="ali123",
        //                Claims=new List<Claim> {
        //                    new Claim("given_name","Ali"),
        //                    new Claim("family_name","Veli"),
        //                    new Claim("updated_at",DateTime.Now.ToString()),
        //                }
        //            }
        //        };
        //    }
    }
}