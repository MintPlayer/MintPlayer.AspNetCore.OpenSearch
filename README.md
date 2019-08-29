# MintPlayer.AspNetCore.OpenSearch
Easily add OpenSearch to your ASP.NET Core website
## NuGet package
https://www.nuget.org/packages/MintPlayer.AspNetCore.OpenSearch/
## Installation
### NuGet package manager
Open the NuGet package manager and install `MintPlayer.AspNetCore.OpenSearch` in your project
### Package manager console
Install-Package MintPlayer.AspNetCore.OpenSearch
## Usage
### Adding OpenSearch middleware
Add OpenSearch before UseMVC in the middleware pipeline (Startup@Configure):

    app.UseOpenSearch(options =>
    {
        options.OsdxEndpoint =  "/opensearch.xml";
        options.SearchUrl = "/api/Subject/opensearch/redirect/{searchTerms}";
        options.SuggestUrl = "/api/Subject/opensearch/suggest/{searchTerms}";
        options.ImageUrl = "/assets/logo/music_note_16.png";
        options.ShortName = "MintPlayer";
        options.Description = "Search music on MintPlayer";
        options.Contact = "email@example.com";
    });
### Reference OpenSearchDescription from HTML
Open your index.html (angular app) or _ViewStart.cshtml (Razor) and add a link to your OpenSearchDescription:

    <link rel="search" type="application/opensearchdescription+xml" href="/opensearch.xml" title="Search through MintPlayer">