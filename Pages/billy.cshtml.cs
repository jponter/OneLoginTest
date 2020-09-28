using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OneLoginTest.Pages
{
    [Authorize(Policy = "JPADMIN")]
    public class billyModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
