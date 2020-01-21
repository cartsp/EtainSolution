using System;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using React.AspNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using WeatherForecastEtain.Core.Interfaces;
using WeatherForecastEtain.Infrastructure.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WeatherForecastEtain.Infrastructure.Data;

namespace WeatherForecastEtain.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(5));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddJsEngineSwitcher(options => options.DefaultEngineName = ChakraCoreJsEngine.EngineName)
                .AddChakraCore();
            services.AddReact();

            
            services.AddTransient<IAccountService, AccountService>()
                    .AddSingleton<IUserRepository, InMemoryUserRepository>()
                    .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                    .AddHttpClient<IWeatherForcastService, MetaWeatherForcastService>()
                    .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[] //adding transient fault resilience
                    {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4)
                    })).AddPolicyHandler(timeoutPolicy);

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseReact(config =>
            {
                // If you want to use server-side rendering of React components,
                // add all the necessary JavaScript files here. This includes
                // your components as well as all of their dependencies.
                // See http://reactjs.net/ for more information. Example:
                config
                  .AddScript("~/js/Components/WeatherList.jsx")
                  //.AddScript("~/js/Components/WeatherItem.jsx")
                  .SetJsonSerializerSettings(new JsonSerializerSettings
                  {
                      StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                      ContractResolver = new CamelCasePropertyNamesContractResolver()
                  });
                // If you use an external build too (for example, Babel, Webpack,
                // Browserify or Gulp), you can improve performance by disabling
                // ReactJS.NET's version of Babel and loading the pre-transpiled
                // scripts. Example:
                //config
                //  .SetLoadBabel(false)
                //  .AddScriptWithoutTransform("~/js/bundle.server.js");
            });

            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
