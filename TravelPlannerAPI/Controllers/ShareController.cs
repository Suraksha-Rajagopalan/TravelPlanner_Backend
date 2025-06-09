using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/share")]
public class ShareController : ControllerBase
{
    private readonly IConfiguration _config;

    public ShareController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet("url")]
    public IActionResult GetAuthUrl()
    {
        var clientId = _config["GmailOAuth:ClientId"];
        var redirect = Uri.EscapeDataString(_config["GmailOAuth:RedirectUri"]);
        const string scope = "https://mail.google.com/";
        string url =
            $"https://accounts.google.com/o/oauth2/v2/auth?response_type=code&client_id={clientId}" +
            $"&redirect_uri={redirect}&scope={scope}&access_type=offline&prompt=consent";

        return Ok(new { url });
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string code)
    {
        using var http = new HttpClient();
        var tokenResult = await http.PostAsync(
            "https://oauth2.googleapis.com/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["code"] = code,
                ["client_id"] = _config["GmailOAuth:ClientId"],
                ["client_secret"] = _config["GmailOAuth:ClientSecret"],
                ["redirect_uri"] = _config["GmailOAuth:RedirectUri"],
                ["grant_type"] = "authorization_code"
            }));

        var tokenJson = await tokenResult.Content.ReadAsStringAsync();
        return Content($"<script>window.opener.postMessage({tokenJson}, '*');window.close();</script>", "text/html");
    }
}
