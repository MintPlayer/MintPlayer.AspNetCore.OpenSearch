using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreOpenSearch.Test.Services
{
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
            return new[] { searchTerms, new string(searchTerms.Reverse().ToArray()) };
        }
    }
}
