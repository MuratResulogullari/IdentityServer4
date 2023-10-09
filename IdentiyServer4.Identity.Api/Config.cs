// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentiyServer4.Identity.Api
{
    //public static class Config
    //{
    //    public static IEnumerable<IdentityResource> IdentityResources =>
    //               new IdentityResource[]
    //               {
    //            new IdentityResources.OpenId(),
    //            new IdentityResources.Profile(),
    //               };

    //    public static IEnumerable<ApiScope> ApiScopes =>
    //        new ApiScope[]
    //        {
    //            new ApiScope("scope1"),
    //            new ApiScope("scope2"),
    //        };

    //    public static IEnumerable<Client> Clients =>
    //        new Client[]
    //        {
    //            // m2m client credentials flow client
    //            new Client
    //            {
    //                ClientId = "m2m.client",
    //                ClientName = "Client Credentials Client",

    //                AllowedGrantTypes = GrantTypes.ClientCredentials,
    //                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

    //                AllowedScopes = { "scope1" }
    //            },

    //            // interactive client using code flow + pkce
    //            new Client
    //            {
    //                ClientId = "interactive",
    //                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

    //                AllowedGrantTypes = GrantTypes.Code,

    //                RedirectUris = { "https://localhost:44300/signin-oidc" },
    //                FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
    //                PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

    //                AllowOfflineAccess = true,
    //                AllowedScopes = { "openid", "profile", "scope2" }
    //            },
    //        };
    //}

    public static class Config
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
                },
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName),
                new ApiResource("weather_api_resource")
                {
                    ApiSecrets=new Secret[] { new Secret("secret".Sha256()) },
                    Scopes={ 
                        //"weatherforecast.write", "weatherforecast.read", "weatherforecast.modify", "weatherforecast.remove" 
                         "IdentityServerApi",
                        "WeatherApi"
                    }
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>() {
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName,"Identity Server Scope"),
                new ApiScope("WeatherApi","Weather Api Scope"),
                //new ApiScope("weather_api_scope","weather api full izin"),
                new ApiScope("api1.read","API 1 için okuma izni"),
                new ApiScope("api1.write","API 1 için yazma izni"),
                new ApiScope("api1.update","API 1 için güncelleme izni"),
                new ApiScope("api2.read","API 2 için okuma izni"),
                new ApiScope("api2.write","API 2 için yazma izni"),
                new ApiScope("api2.update","API 2 için güncelleme izni"),
                //new ApiScope("weatherforecast.write","Weather Forecast yazma"),
                //new ApiScope("weatherforecast.read","Weather Forecast okuma"),
                //new ApiScope("weatherforecast.modify","Weather Forecast güncelleme"),
                //new ApiScope("weatherforecast.remove","Weather Forecast sileme"),
                 
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
                 }, new Client()
                    {
                        ClientId="spa-client",
                        //ClientSecrets= Spa ve mobilde burada belirtmiyoruz ve aşağıdakileri tanımlıyoruz
                        RequireClientSecret=false,// Özellikle istemediğini belirtiyorsun
                        RequirePkce=true, //Bunu tanımlayınca secret kendisi tutuyor js tarafına göndermiyor
                        AllowedGrantTypes=GrantTypes.Code,
                        ClientName="Spa Client (Angular)",
                        AllowedScopes={
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Email,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.OfflineAccess,//Aşağıda tanımını yaptık
                            "api1.read",
                            "CountryAndCity",
                            "Roles"
                        },
                        RedirectUris={"http://localhost:4200/callback"}, // http olarak ayarlıyoruz
                        PostLogoutRedirectUris={ "http://localhost:4200" },// callback ayarlaması
                        AllowedCorsOrigins={ "http://localhost:4200" },
                    },
                //new Client()
                //    {
                //    ClientId="Client1-ResourceOwner-Mvc",
                //    ClientName="Client 1 app mvc application",
                //    ClientSecrets=new [] {new  Secret("secret".ToSha256()) },
                //    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword, // Flow değiştiriyoruz
                //     AllowedScopes={
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile,
                //        IdentityServerConstants.StandardScopes.OfflineAccess,//Aşağıda tanımını yaptık
                //       "api1.read",
                //       "CountryAndCity"
                //       ,"Roles"
                //     },
                //      AccessTokenLifetime=2*60*60,//Access token için bir zaman verdik saniye cinsinde olacak
                //      AllowOfflineAccess=true,// Artık Bir refresh token yayınlayacaktır true yaparsak
                //      RefreshTokenUsage=TokenUsage.ReUse, // Birden fazla kullanılsın OneTimeOnly kullanarak bir kerelikte yappabilirsin
                //      AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddYears(6)-DateTime.Now).TotalSeconds, // 6 yıl sonra ömrü kesinlikle biter
                //      SlidingRefreshTokenLifetime=(int)(DateTime.Now.AddDays(30)-DateTime.Now).TotalSeconds, // 1 aylık süre içerisinde her kullanıldığında ömrünü 1 ay daha uzatır
                // }, 
                new Client()
                    {
                     ClientId="Client1-ResourceOwner-Mvc",
                     ClientName="Client 1 app mvc application",
                     ClientSecrets=new [] {new  Secret("secret".ToSha256()) },
                     AllowedGrantTypes=GrantTypes.ResourceOwnerPasswordAndClientCredentials, // Flow değiştiriyoruz
                     AllowedScopes={
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,//Aşağıda tanımını yaptık
                       "api1.read",
                       "api2.write",
                       "CountryAndCity"
                       ,"Roles",
                       IdentityServerConstants.LocalApi.ScopeName
                     },
                      AccessTokenLifetime=2*60*60,//Access token için bir zaman verdik saniye cinsinde olacak
                      AllowOfflineAccess=true,// Artık Bir refresh token yayınlayacaktır true yaparsak
                      RefreshTokenUsage=TokenUsage.ReUse, // Birden fazla kullanılsın OneTimeOnly kullanarak bir kerelikte yappabilirsin
                      AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddYears(6)-DateTime.Now).TotalSeconds, // 6 yıl sonra ömrü kesinlikle biter
                  },
                 new Client()
                    {
                        ClientId="weather_api_swagger",
                        ClientName="Weather API",
                        ClientSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                        AllowedGrantTypes=GrantTypes.Code,
                        RequireClientSecret=false,
                        RequirePkce=true,
                        AllowedScopes={
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Email,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.OfflineAccess,//Aşağıda tanımını yaptık
                            "Roles",
                            //"weatherforecast.write",
                            //"weatherforecast.read",
                            //"weatherforecast.modify",
                            //"weatherforecast.remove"
                            IdentityServerConstants.LocalApi.ScopeName,//IdentityServerApi
                            "WeatherApi"
                        },
                        RedirectUris={  "https://localhost:7277/swagger/oauth2-redirect.html"}, // http olarak ayarlıyoruz
                        PostLogoutRedirectUris={ "https://localhost:7277/signout-callback-oidc"},// callback ayarlaması
                        AllowedCorsOrigins={"https://localhost:7277"},
                        FrontChannelLogoutUri="https://localhost:7277/signout-oidc",
                       
                         
                    }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),     //subId
                new IdentityResources.Profile(),    // Kullanıcı hakkında claimleri tutar name,family_name,given_ame,midlle_name,nickname,preferred_username,profile,picture,website,gender,birthdate,zoneinfo,locale,updated_at
                new IdentityResources.Email(),
                new IdentityResource() {Name = IdentityServerConstants.StandardScopes.OfflineAccess},
                new IdentityResource(){Name="CountryAndCity",DisplayName="Country And City",Description="Kullanıcı ülke ve şehir bilgisi",UserClaims=new List<string> {"country","city"}},
                new IdentityResource(){ Name="Roles",DisplayName="Roles", Description="Kullanıcı rolleri", UserClaims=new [] { "role"} },
                new IdentityResource(){ Name="UserClaims", DisplayName="UserClaims",Description="Phone Number Email ve User Id bilgisini taşır", UserClaims= new [] { "UserId", "Phone","Email"}},
                new IdentityResource(){ Name="RoleClaims",DisplayName="RoleClaims", Description="Role Yetkileri", UserClaims=new [] { "instructorclaim" } },
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