using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using AspNetCoreOpenSearch.Options;
using Microsoft.AspNetCore.Routing;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreOpenSearch.Extensions
{
    public static class OpenSearchExtensions
    {
        public static IServiceCollection AddOpenSearch<TService>(this IServiceCollection services) where TService : class, IOpenSearchService
        {
            return services.AddSingleton<IOpenSearchService, TService>();
        }

        public static IApplicationBuilder UseOpenSearch(this IApplicationBuilder app, Action<OpenSearchOptions> options) 
        {
            #region Compile opensearch options
            var opt = new OpenSearchOptions();
            options(opt);
            #endregion

            #region Check OSDX endpoint format
            if (!opt.OsdxEndpoint.StartsWith('/'))
                throw new Exception(@"OpenSearch endpoint must start with ""/""");
            #endregion

            #region Get Service instance
            var service = app.ApplicationServices.GetService<IOpenSearchService>();
            #endregion

            #region Handle routes specific to this package
            var builder = new RouteBuilder(app);
            builder
                .MapVerb("GET", opt.OsdxEndpoint, async (context) =>
                {
                    #region Return OSDX
                    context.Response.ContentType = "application/opensearchdescription+xml; charset=utf-8";
                    context.Response.Headers["Content-Disposition"] = $"attachment; filename={opt.ShortName}.osdx";
                    await context.WriteModelAsync(new Data.OpenSearchDescription
                    {
                        ShortName = opt.ShortName,
                        Description = opt.Description,
                        InputEncoding = "UTF-8",
                        Image = new Data.Image
                        {
                            Width = 16,
                            Height = 16,
                            Url = $"{context.Request.Scheme}://{context.Request.Host.Value}{context.Request.PathBase}{opt.ImageUrl}",
                            Type = "image/png"
                        },
                        Urls = new[] {
                            new Data.Url { Type = "text/html", Method = "GET", Template = $"{context.Request.Scheme}://{context.Request.Host.Value}{context.Request.PathBase}{opt.SearchUrl}" },
                            new Data.Url { Type = "application/x-suggestions+json", Method = "GET", Template = $"{context.Request.Scheme}://{context.Request.Host.Value}{context.Request.PathBase}{opt.SuggestUrl}" },
                            new Data.Url { Type = "application/opensearchdescription+xml", Relation = "self", Template = $"{context.Request.Scheme}://{context.Request.Host.Value}{context.Request.PathBase}{opt.OsdxEndpoint}" }
                        }.ToList(),
                        Contact = opt.Contact,
                        SearchForm = $"{context.Request.Scheme}://{context.Request.Host.Value}{context.Request.PathBase}/"
                    });
                    #endregion
                })
                .MapVerb("GET", opt.SuggestUrl, async (context) =>
                {
                    var searchTerms = (string)context.GetRouteValue("searchTerms");
                    var suggestions = await service.ProvideSuggestions(searchTerms);
                    context.Response.Headers["Content-Type"] = "application/json";
                    await context.WriteModelAsync(new object[]
                    {
                        searchTerms,
                        suggestions.ToArray()
                    });
                })
                .MapVerb("GET", opt.SearchUrl, async (context) =>
                {
                    var searchTerms = (string)context.GetRouteValue("searchTerms");
                    var location = await service.PerformSearch(searchTerms);
                    if (location is Microsoft.AspNetCore.Mvc.RedirectResult)
                    {
                        context.Response.Redirect(((Microsoft.AspNetCore.Mvc.RedirectResult)location).Url);
                    }
                    else
                    {
                        //context.Response.(location);
                    }
                });
            var router = builder.Build();
            #endregion

            return app.UseRouter(router);
        }
    }
}
