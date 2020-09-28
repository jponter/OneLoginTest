using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace OneLoginTest.Pages
{
    [Authorize]
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private IOptions<OidcOptions> options;
        public bool isAdmin = false;

        public PrivacyModel(ILogger<PrivacyModel> logger , IOptions<OidcOptions> options)
        {
            _logger = logger;
            this.options = options;
        }

     


        public bool IsAdmin()
        {
            foreach (Claim claim in User.Claims)
            {
                if (claim.Type == "custom_fields")
                {
                    var details = JObject.Parse(claim.Value);
                    bool isAdmin = (bool)details["jp_funding_admin"];
                    if (isAdmin)
                    {
                        //bool arethey = User.IsInRole("admin");
                        
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
               
            }
            return false;
        }

        public void OnGet()
        {
            isAdmin = IsAdmin();
            //foreach (var group in User.Claims.Where(x => x.Type == "groups"))
            //{
            //    User.Claims.Append(new Claim(ClaimTypes.Role, group.Value));
            //    User.AddIdentities(new Claim(ClaimTypes.Role, group.Value));
            //}



        }

        public async Task<IActionResult> OnPost()
        {
            await LogoutUser(User.FindFirstValue("onelogin-access-token"));
            await HttpContext.SignOutAsync();
            return RedirectToPage("./Index");

        }

        private async Task<bool> LogoutUser(string accessToken)
        {
            using (var client = new HttpClient())
            {

                // The Token Endpoint Authentication Method must be set to POST if you
                // want to send the client_secret in the POST body.
                // If Token Endpoint Authentication Method then client_secret must be
                // combined with client_id and provided as a base64 encoded string
                // in a basic authorization header.
                // e.g. Authorization: basic <base64 encoded ("client_id:client_secret")>
                var formData = new FormUrlEncodedContent(new[]
                {
              new KeyValuePair<string, string>("token", accessToken),
              new KeyValuePair<string, string>("token_type_hint", "access_token"),
              new KeyValuePair<string, string>("client_id", options.Value.ClientId),
              new KeyValuePair<string, string>("client_secret", options.Value.ClientSecret)
          });

                var uri = String.Format("https://{0}.onelogin.com/oidc/token/revocation", options.Value.Region);

                var res = await client.PostAsync(uri, formData);

                return res.IsSuccessStatusCode;
            }
        }
    }
}
