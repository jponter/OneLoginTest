using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace OneLoginTest.Pages
{
    [Authorize(Policy ="JPADMIN")]
    public class NopeModel : PageModel
    {
        private readonly ILogger<NopeModel> _logger;

        public NopeModel(ILogger<NopeModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
