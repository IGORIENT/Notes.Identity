using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityModel;


namespace Notes.Identity
{
    public static class Configuration
    {
        //тут нужно хранить информацию о клиентах, которым разрешено использовать наш IdentityServer
        //Скоуп представляет то, что клиентскому приложению можно использовать.
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("NotesWebAPI", "Web API")
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> ApiResiurces => new List<ApiResource>
        {
            new ApiResource("NotesWebAPI", "Web API", new []
                { JwtClaimTypes.Name})
            {
                Scopes = {"NotesWebAPI"}
            }
        };

        // клиенты - списки приложений, которые могут использовать нашу систему
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    //идентификатор клиента на сервере. (должен быть такой же, как в конфигурации самого клиента.)
                    ClientId = "notes-web-api",

                    //имя клиента
                    ClientName = "Notes Web",

                    //гранд тип определяет как клиент взаимодействует с сервисом токена.
                    //CODE (autorization Code) самый распространенный для обмена кода авторизации на токен доступа
                    AllowedGrantTypes = GrantTypes.Code, 

                    //секрет клиента - постоянный пароль, который должен совпадать на клиенте и на сервере, чтобы выдать токен
                    RequireClientSecret = false,

                    //одноразовый код, для проверки клиента.
                    RequirePkce = true,

                    //набор адресов для перенаправления после аутентификации.
                    RedirectUris =
                    {
                        "http:// .../signin-oidc"  //набор адресов, куда может быть выполнено перенаправление после аутентификации клиентского приложения
                    },

                    //набор Uri адресов, кому позволено использовать Identity Server
                    AllowedCorsOrigins =
                    {
                        "http:// ..."
                    },
                    //набор адресов куда может переходить направление после выхода клиентского приложения
                    PostLogoutRedirectUris =
                    {
                        "http:/ .../signout-oidc"
                    },
                    //области доступные клиенту
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "NotesWebAPI"
                    },
                    //управляет передачей токена через браузер
                    AllowAccessTokensViaBrowser = true
                }
            };
    }
}
