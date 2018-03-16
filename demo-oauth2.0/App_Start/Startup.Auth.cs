using demoOAuth2.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace demoOAuth2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            ConfigureAuth(app);

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll);  // 允许跨域请求
            app.UseWebApi(config);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            var OAuthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                AuthenticationMode = AuthenticationMode.Active,
                TokenEndpointPath = new PathString("/token"), //获取 access_token 认证服务请求地址
                AuthorizeEndpointPath = new PathString("/authorize"), //获取 authorization_code 认证服务请求地址
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1), //access_token 过期时间

                Provider = new OpenAuthorizationServerProvider(), //access_token 相关认证服务
                AuthorizationCodeProvider = new OpenAuthorizationCodeProvider(), //authorization_code 认证服务
                RefreshTokenProvider = new OpenRefreshTokenProvider() //refresh_token 认证服务
            };
            app.UseOAuthBearerTokens(OAuthOptions); //表示 token_type 使用 bearer 方式
        }
    }
}