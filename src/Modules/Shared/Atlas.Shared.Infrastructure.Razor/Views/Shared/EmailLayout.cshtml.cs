using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Atlas.Infrastructure.Razor.Views.Shared
{
    public class EmailLayoutModel : PageModel
    {
        public string EmailTitle { get; set; } = null!;

        public void OnGet()
        {
        }
    }
}
