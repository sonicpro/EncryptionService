using Encryption.Filters;
using Encryption.Helpers;
using Encryption.Interfaces;
using Encryption.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            app.UseStatusCodePages();
            app.UseMvc();
        }
    }
}
