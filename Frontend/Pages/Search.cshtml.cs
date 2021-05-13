using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceDTO;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class SearchModel : PageModel
{
    private readonly IAPIClient _apiClient;

    public SearchModel(IAPIClient apiClient)
    {
        _apiClient = apiClient;
    }

    public string Term { get; set; }

    public List<SearchResult> SearchResults { get; set; }
    
    public async Task OnGetAsync(string term)
    {
        Term = term;
        SearchResults = await _apiClient.SearchAsync(term);
    }
}