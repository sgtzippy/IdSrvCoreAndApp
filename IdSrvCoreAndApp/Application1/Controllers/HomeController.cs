using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using IdentityModel.Client;
using Application1.Services;
using Newtonsoft.Json;

namespace Application1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApplication1HttpClient _application1HttpClient;

        public HomeController(IApplication1HttpClient application1HttpClient)
        {
            _application1HttpClient = application1HttpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> About()
        {
            var discoveryClient = new DiscoveryClient("https://localhost:44300/");
            var metaDataResponse = await discoveryClient.GetAsync();

            var userInfoClient = new UserInfoClient(metaDataResponse.UserInfoEndpoint);

            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var response = await userInfoClient.GetAsync(accessToken);

            if (response.IsError)
            {
                throw new Exception("Problem accessing the UserInfo endpoint.", response.Exception);
            }

            var address = response.Claims.FirstOrDefault(c => c.Type == "address")?.Value;

            await WriteOutIdentityInformation();

            var httpClient = await _application1HttpClient.GetClient();

            var valuesResponse = await httpClient.GetAsync("api/values").ConfigureAwait(false);

            if (valuesResponse.IsSuccessStatusCode)
            {
                var dataAsString = await valuesResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var data = JsonConvert.DeserializeObject<string>(dataAsString);

                return View(new AboutViewModel(address, data));
            }
            else if (valuesResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                valuesResponse.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {valuesResponse.ReasonPhrase}");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        [Authorize(Policy = "CanViewSubscription")]
        public IActionResult Subscription()
        {
            return View();
        }

        public async Task Logout()
        {
            var discoveryClient = new DiscoveryClient("https://localhost:44300/");
            var metaDataResponse = await discoveryClient.GetAsync();

            var revocationClient = new TokenRevocationClient(
                metaDataResponse.RevocationEndpoint,
                "Application1Client",
                "secret"
            );

            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                var revokeAccessTokenResponse = await revocationClient.RevokeAccessTokenAsync(accessToken);

                if (revokeAccessTokenResponse.IsError)
                {
                    throw new Exception("A problem was encountered while revoking the access token.", revokeAccessTokenResponse.Exception);
                }
            }

            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var revokeRefreshTokenResponse = await revocationClient.RevokeRefreshTokenAsync(refreshToken);

                if (revokeRefreshTokenResponse.IsError)
                {
                    throw new Exception("A problem was encountered while revoking the refresh token.", revokeRefreshTokenResponse.Exception);
                }
            }
            
            // Clears the local cookie ("Cookies" must match name from scheme)
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task WriteOutIdentityInformation()
        {
            // Get the saved identity token
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            // Write it out
            Debug.WriteLine($"Identity token: {identityToken}");

            // Write out the user claims
            foreach(var claim in User.Claims)
            {
                Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }
        }
    }
}
