# MintPlayer.AspNetCore.OpenSearch
[![NuGet Version](https://img.shields.io/nuget/v/MintPlayer.AspNetCore.OpenSearch.svg?style=flat)](https://www.nuget.org/packages/MintPlayer.AspNetCore.OpenSearch/)

Easily add OpenSearch to your ASP.NET Core website
## NuGet package
https://www.nuget.org/packages/MintPlayer.AspNetCore.OpenSearch/
## Installation
### NuGet package manager
Open the NuGet package manager and install `MintPlayer.AspNetCore.OpenSearch` in your project
### Package manager console
Install-Package MintPlayer.AspNetCore.OpenSearch
## Usage
### Register the OpenSearch services and configure the options

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

Don't forget to add the XML serializer formatters

### Adding OpenSearch middleware
Add OpenSearch before UseMVC in the middleware pipeline (Startup@Configure):

    app.UseOpenSearch();

### Build your OpenSearch service
This is an example implementation of the IOpenSearchService:

    public class OpenSearchService : IOpenSearchService
    {
        private readonly IServiceProvider serviceProvider;
        public OpenSearchService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<RedirectResult> PerformSearch(string searchTerms)
        {
            //using (var scope = serviceProvider.CreateScope())
            //{
            //    var subjectRepository = scope.ServiceProvider.GetService<ISubjectRepository>();
            //}
            return new RedirectResult($"/search/{searchTerms}");
        }

        public async Task<IEnumerable<string>> ProvideSuggestions(string searchTerms)
        {
            //using (var scope = serviceProvider.CreateScope())
            //{
            //    var subjectRepository = scope.ServiceProvider.GetService<ISubjectRepository>();
            //}
            return new[] { searchTerms + 'o', new string(searchTerms.Reverse().ToArray()) };
        }
    }

### Reference OpenSearchDescription from HTML
Open your index.html (angular app) or _ViewStart.cshtml (Razor) and add a link to your OpenSearchDescription:

    <link rel="search" type="application/opensearchdescription+xml" href="/opensearch.xml" title="Search through MintPlayer">
