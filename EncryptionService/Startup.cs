using Encryption.Filters;
using Encryption.Helpers;
using Encryption.Interfaces;
using Encryption.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace Encryption
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            IConfigurationSection encryptionSettings = Configuration.GetSection("EncryptionSettings");
            services.Configure<EncryptionSettings>(encryptionSettings);
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<IEncryptionHelperService, EncryptionHelperService>();
            services.AddSingleton<IKeyProvider, KeyProvider>();

            services.AddMvc();
            services.AddScoped<ITokenValidator, TokenVaidator>();
            services.AddScoped<EncryptionServiceAuthorizationFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "text/html";

                        await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
                        await context.Response.WriteAsync("ERROR!<br><br>\r\n");

                        var exceptionHandlerPathFeature =
                            context.Features.Get<IExceptionHandlerPathFeature>();

                        if (exceptionHandlerPathFeature?.Error is CryptographicException)
                        {
                            await context.Response.WriteAsync(@"An error occurred during the data decryption.<br>
                                Please contact the original data provider and try to re-encrypt the data.<br><br>\r\n");
                        }
                        else
                        {
                            var message = exceptionHandlerPathFeature?.Error.Message;
                            await context.Response.WriteAsync($@"An error occurred while processing the request.<br>
                                The message is: '{message}'.<br><br>\r\n");
                        }

                        await context.Response.WriteAsync("</body></html>\r\n");
                        await context.Response.WriteAsync(new string(' ', 512)); // IE padding
                    });
                });
            }

            app.UseStatusCodePages();
            app.UseMvc();
        }
    }
}
