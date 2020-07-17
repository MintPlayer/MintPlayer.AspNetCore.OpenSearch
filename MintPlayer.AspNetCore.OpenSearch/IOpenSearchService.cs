using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreOpenSearch
{
    public interface IOpenSearchService
    {
        Task<IEnumerable<string>> ProvideSuggestions(string searchTerms);
        Task<RedirectResult> PerformSearch(string searchTerms);
    }
}
