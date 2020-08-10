using AspNetCoreOpenSearch.Extensions;
using AspNetCoreOpenSearch.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MintPlayer.AspNetCore.OpenSearch.Test31
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllersWithViews(options =>
                {
                    options.RespectBrowserAcceptHeader = true;
                })
                .AddXmlSerializerFormatters();

            services.AddOpenSearch<Services.OpenSearchService>();
            services.Configure<OpenSearchOptions>(options =>
            {
                options.OsdxEndpoint = "/opensearch.xml";
                options.SearchUrl = "/api/Subject/opensearch/redirect/{searchTerms}";
                options.SuggestUrl = "/api/Subject/opensearch/suggest/{searchTerms}";
                options.ImageUrl = "/assets/logo/music_note_16.png";
                options.ShortName = "MintPlayer";
                options.Description = "Search music on MintPlayer";
                options.Contact = "email@example.com";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseOpenSearch();
            app.UseStaticFiles();

            app.UseRouting();

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
