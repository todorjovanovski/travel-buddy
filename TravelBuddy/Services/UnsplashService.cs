namespace TravelBuddy.Services;

using System.Net.Http.Json;

public class UnsplashPhoto
{
    public UrlsUrls Urls { get; set; }
}

public class UrlsUrls
{
    public string Raw     { get; set; }
    public string Full    { get; set; }
    public string Regular { get; set; }
    public string Small   { get; set; }
    public string Thumb   { get; set; }
}

public class UnsplashService
{
    private readonly HttpClient _http;

    // You can register HttpClient via DI in MauiProgram
    public UnsplashService(HttpClient httpClient)  
        => _http = httpClient;

    public async Task<string> GetRandomPhotoUrlAsync(string query)
    {
        // Make sure to URL‑encode your query (e.g. "New York" → "New%20York")
        var url = $"https://api.unsplash.com/photos/random?query={Uri.EscapeDataString(query)}";

        // Attach your access key in the Authorization header
        _http.DefaultRequestHeaders.Authorization 
            = new System.Net.Http.Headers.AuthenticationHeaderValue("Client-ID", "lBPL0W-vBZ-HuGVO__A-fYGK9QfUoB3wqy5idbkyZlo");

        // Call and deserialize
        var photo = await _http.GetFromJsonAsync<UnsplashPhoto>(url);
        return photo?.Urls?.Small;      // or .Regular/.Full depending on size you want
    }
}
