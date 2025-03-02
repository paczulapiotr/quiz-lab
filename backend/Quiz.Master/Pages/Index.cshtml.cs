using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Quiz.Master.Pages;

public class IndexModel : PageModel
{

    public string StrapiUrl { get; init; }

    public IndexModel(IConfiguration configuration)
    {
        StrapiUrl = configuration["Razor:StrapiUrl"] ?? "";
    }

    public void OnGet()
    {
    }
}