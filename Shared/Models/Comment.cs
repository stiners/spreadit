namespace Shared.Models;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int Upvotes { get; set; }
    public int Downvotes { get; set; }
    public User User { get; set; }

    public DateTime? Date { get; set; }
    public Comment(string content = "", int upvotes = 0, int downvotes = 0, User user = null, DateTime? date = null)
    {
        Content = content;
        Upvotes = upvotes;
        Downvotes = downvotes;
        Date = date; 
        User = user;
    }
    public Comment() {
        Id = 0;
        Content = "";
        Upvotes = 0;
        Downvotes = 0;
        Date = DateTime.Now; 
    }
}