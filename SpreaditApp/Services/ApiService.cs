using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Shared.Models;

namespace SpreaditApp.Services;

public class ApiService
{
    private HttpClient _http;
    private IConfiguration _configuration;
    private const string BaseApi = "https://localhost:7022";

    public ApiService(HttpClient http, IConfiguration configuration)
    {
        _http = http;
        this._configuration = configuration;
        //BaseAPI = configuration["base_api"];
    }

    public async Task<Post[]> GetPosts()
    {
        string url = $"{BaseApi}/posts";
        var posts = await _http.GetFromJsonAsync<Post[]>(url);
        return posts;
    }

    public async Task<Post> GetPost(int id)
    {
        string url = $"{BaseApi}/posts/{id}";
        return await _http.GetFromJsonAsync<Post>(url);
    }
    
    public async Task<Post> CreatePost(string title, string content, string author)
    {
        string url = $"{BaseApi}/posts";
        
        var user = new User(author);
        var post = new Post(user, title, content, DateTime.Now);
        
        HttpResponseMessage msg = await _http.PostAsJsonAsync(url, post);
        
        string json = await msg.Content.ReadAsStringAsync();
        
        Post? newPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        });

        return newPost;
    }

    public async Task<Comment> CreateComment(string content, string username, int postId)
    {
        string url = $"{BaseApi}/posts/{postId}/comments";
        
        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await _http.PostAsJsonAsync(url, new CreateCommentDto { Content = content, Username = username});
        
        // Get the JSON string from the response
        string json = await msg.Content.ReadAsStringAsync();

        // Deserialize the JSON string to a Comment object
        Comment? newComment = JsonSerializer.Deserialize<Comment>(json, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties 
        });

        // Return the new comment 
        return newComment;
    }

    public async Task<Post> UpvotePost(int id)
    {
        string url = $"{BaseApi}/posts/{id}/upvote/";

        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await _http.PutAsJsonAsync(url, "");

        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;

        // Deserialize the JSON string to a Post object
        Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
        });

        // Return the updated post (vote increased)
        return updatedPost;
    }

    public async Task<Post> DownvotePost(int id)
    {
        string url = $"{BaseApi}/posts/{id}/downvote/";

        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await _http.PutAsJsonAsync(url, "");

        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;

        // Deserialize the JSON string to a Post object
        Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
        });

        // Return the updated post (vote increased)
        return updatedPost;
    }
    
    public async Task<Comment> UpvoteComment(int id)
    {
        string url = $"{BaseApi}/comments/{id}/upvote/";

        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await _http.PutAsJsonAsync(url, "");

        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;

        // Deserialize the JSON string to a Post object
        Comment? updatedComment = JsonSerializer.Deserialize<Comment>(json, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
        });

        // Return the updated post (vote increased)
        return updatedComment;
    }
    
    public async Task<Comment> DownvoteComment(int id)
    {
        string url = $"{BaseApi}/comments/{id}/downvote/";

        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await _http.PutAsJsonAsync(url, "");

        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;

        // Deserialize the JSON string to a Post object
        Comment? updatedComment = JsonSerializer.Deserialize<Comment>(json, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
        });

        // Return the updated post (vote increased)
        return updatedComment;
    }
}
