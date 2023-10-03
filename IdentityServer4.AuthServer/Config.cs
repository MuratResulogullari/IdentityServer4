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
                //new Client()
                //{
                //    ClientId="WebAppMVC1",
                //    ClientName="Web MVC 1",
                //    ClientSecrets=new [] {new  Secret("secret".ToSha256()) },
                //    AllowedGrantTypes={GrantType.ClientCredentials},
                //    AllowedScopes={"api1.read" }
                //},
                // new Client()
                //{
                //    ClientId="WebAppMVC2",
                //    ClientName="Web  MVC 2",
                //    ClientSecrets=new [] {new  Secret("secret".ToSha256()) },
                //    AllowedGrantTypes={GrantType.ClientCredentials},
                //    AllowedScopes={"api1.read","api1.update","api2.write","api2.update"}
                //}

                // Identity User Üyelik Sistemi olunca
                 new Client()
                    {
                    ClientId="WebAppMVC1",
                    ClientName="Web MVC 1",
                    ClientSecrets=new [] {new  Secret("secret".ToSha256()) },
                    AllowedGrantTypes=GrantTypes.Hybrid, // Response type code ve token_id istediğimiz için Hyprid seçiyorum
                    RedirectUris=new List<string>{"https://localhost:7118/signin-oidc"},//WebAppMVC1 client çalıştığı endpoint veriyoruz ve diyoruz ki Token almak için istekte bulunursa bu client
                    // Geri donuş ednpoint /sign-oidc bunu verek sağlıyoruz iki tarafta openId connect paketini iki tarfta kullanıyor ve geri dönüşü  AuthServer bu enpoint response atıyor
                    PostLogoutRedirectUris=new List<string>{"https://localhost:7118/signout-callback-oidc"},//Çıkış yapılınca dönecek yer
                    AllowedScopes={
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,//Aşağıda tanımını yaptık 
                       "api1.read",
                       "CountryAndCity"
                       ,"Roles"

                     },
                     // RequirePkce=true, // Bu Native Clients (mobile,spa,akıllı saat,devices)token saklıyamadıkları için  Prof Key for Code Exchangeönce code_challenge sonra code_verifier ile doğrulayarak token alır
                    RequirePkce=false, // Bizim Bir Server Side Client olduğu için token false çekiyoruz çünkü cookide saklıyacağız  
                    AccessTokenLifetime=2*60*60,//Access token için bir zaman verdik saniye cinsinde olacak
                    AllowOfflineAccess=true,// Artık Bir refresh token yayınlayacaktır true yaparsak
                    RefreshTokenUsage=TokenUsage.ReUse, // Birden fazla kullanılsın OneTimeOnly kullanarak bir kerelikte yappabilirsin
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddYears(6)-DateTime.Now).TotalSeconds, // 6 yıl sonra ömrü kesinlikle biter
                    SlidingRefreshTokenLifetime=(int)(DateTime.Now.AddDays(30)-DateTime.Now).TotalSeconds, // 1 aylık süre içerisinde her kullanıldığında ömrünü 1 ay daha uzatır
                    RequireConsent=false,// Onay  sayfasına yönlendirme yapar ve sen client login olduktan sonra nelere erieşceğini bilirtirsiniz
                    
                 },
                    new Client()
                    {
                    ClientId="WebAppMVC2",
                    ClientName="Web MVC 2",
                    ClientSecrets=new [] {new  Secret("secret".ToSha256()) },
                    AllowedGrantTypes=GrantTypes.Hybrid, // Response type code ve token_id istediğimiz için Hyprid seçiyorum
                    RedirectUris=new List<string>{"https://localhost:7161/signin-oidc"},//WebAppMVC2 client çalıştığı endpoint veriyoruz ve diyoruz ki Token almak için istekte bulunursa bu client
                    // Geri donuş ednpoint /sign-oidc bunu verek sağlıyoruz iki tarafta openId connect paketini iki tarfta kullanıyor ve geri dönüşü  AuthServer bu enpoint response atıyor
                    PostLogoutRedirectUris=new List<string>{"https://localhost:7161/signout-callback-oidc"},//Çıkış yapılınca dönecek yer
                    AllowedScopes={
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,//Aşağıda tanımını yaptık 
                      "api1.read","api2.read","api1.update","api2.write","api2.update",
                       "CountryAndCity"
                       ,"Roles"
                       ,IdentityServerConstants.StandardScopes.Email

                     },
                     // RequirePkce=true, // Bu Native Clients (mobile,spa,akıllı saat,devices)token saklıyamadıkları için  Prof Key for Code Exchangeönce code_challenge sonra code_verifier ile doğrulayarak token alır
                    RequirePkce=false, // Bizim Bir Server Side Client olduğu için token false çekiyoruz çünkü cookide saklıyacağız  
                    AccessTokenLifetime=2*60*60,//Access token için bir zaman verdik saniye cinsinde olacak
                    AllowOfflineAccess=true,// Artık Bir refresh token yayınlayacaktır true yaparsak
                    RefreshTokenUsage=TokenUsage.ReUse, // Birden fazla kullanılsın OneTimeOnly kullanarak bir kerelikte yappabilirsin
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddYears(6)-DateTime.Now).TotalSeconds, // 6 yıl sonra ömrü kesinlikle biter
                    SlidingRefreshTokenLifetime=(int)(DateTime.Now.AddDays(30)-DateTime.Now).TotalSeconds, // 1 aylık süre içerisinde her kullanıldığında ömrünü 1 ay daha uzatır
                    RequireConsent=false,// Onay  sayfasına yönlendirme yapar ve sen client login olduktan sonra nelere erieşceğini bilirtirsiniz
                    
                 },
                    
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),     //subId
                new IdentityResources.Profile(),    // Kullanıcı hakkında claimleri tutar name,family_name,given_ame,midlle_name,nickname,preferred_username,profile,picture,website,gender,birthdate,zoneinfo,locale,updated_at
                new IdentityResource(){Name="CountryAndCity",DisplayName="Country And City",Description="Kullanıcı ülke ve şehir bilgisi",UserClaims=new List<string> {"country","city"}},
                new IdentityResource(){ Name="Roles",DisplayName="Roles", Description="Kullanıcı rolleri", UserClaims=new [] { "role"} },
                //new IdentityResource() {Name = IdentityServerConstants.StandardScopes.OfflineAccess},
                //new IdentityResource(){ Name="UserClaims", DisplayName="UserClaims",Description="Phone Number Email ve User Id bilgisini taşır", UserClaims= new [] { "UserId", "Phone","Email"}},
                //new IdentityResource(){ Name="RoleClaims",DisplayName="RoleClaims", Description="Role Yetkileri", UserClaims=new [] { "instructorclaim" } },
                new IdentityResources.Email(),
            };
        }

        public static IEnumerable<TestUser> GetUsers()
        {
            return new List<TestUser>()
                {
                    new TestUser()
                    {
                        SubjectId="1",
                        Username="murat.resulogullari",
                        Password="123.Mr*",
                        Claims=new List<Claim> {
                            new Claim("given_name","Murat"),
                            new Claim("family_name","Resuloğulları"),
                            new Claim("updated_at",DateTime.Now.ToString()),
                            new Claim("country","Türkiye"),
                            new Claim("city","İzmir"),
                            new Claim("role","administrator")
                        }
                    },
                     new TestUser()
                    {
                        SubjectId="2",
                        Username="ali.veli",
                        Password="ali123",
                        Claims=new List<Claim> {
                            new Claim("given_name","Ali"),
                            new Claim("family_name","Veli"),
                            new Claim("updated_at",DateTime.Now.ToString()),
                            new Claim("country","Türkiye"),
                            new Claim("city","Konya"),
                             new Claim("role","manager")
                        }
                    }
                };
        }
    }
}