using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages
{
    public class InformationModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}