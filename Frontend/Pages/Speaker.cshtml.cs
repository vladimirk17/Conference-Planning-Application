using System.Threading.Tasks;
using ConferenceDTO;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages
{
    public class SpeakerModel : PageModel
    {
        private readonly IAPIClient _apiClient;

        public SpeakerModel(IAPIClient apiClient)
        {
            _apiClient = apiClient;
        }

        public SpeakerResponse Speaker { get; set; }
        
        public async Task<IActionResult> OnGet(int id)
        {
            Speaker = await _apiClient.GetSpeakerAsync(id);

            if (Speaker == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}