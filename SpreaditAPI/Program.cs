using Microsoft.EntityFrameworkCore;
using Shared.Models;
using SpreaditAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SpreaditContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => 
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
        );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();

using var dbContext = new SpreaditContext();

app.MapGet("/posts", async () =>
{
    var result = await dbContext.Posts
        .Include(post => post.Comments)
        .ThenInclude(comment => comment.User)
        .ToListAsync();
    
    return result;
});

app.MapGet("/posts/{id}", (int id) =>
{
    var result = dbContext.Posts.Find(id);
    return result; 
});

//Get comments by post id   
app.MapGet("/posts/{id}/comments", (int id) =>
{
    var result = dbContext.Comments
    .Where(comment => comment.Id == id);
    return result.ToList();
});

// post post 
app.MapPost("/posts", async (Post post) =>
{
    await dbContext.Posts.AddAsync(post);
    await dbContext.SaveChangesAsync();

    return Results.Ok(post);
});

// post comment
app.MapPost("/posts/{postId}/comments/", async (CreateCommentDto createCommentDto, int postId) =>
{
    var postToComment = await dbContext.Posts.FindAsync(postId);
    var user = await dbContext.Users.FirstOrDefaultAsync(user => user.Username == createCommentDto.Username);

    if (user == null)
    {
        user = new User(createCommentDto.Username);
    }
    
    var comment = new Comment()
    {
        User = user,
        Content = createCommentDto.Content
    };
    
    postToComment.Comments.Add(comment);
    
    await dbContext.SaveChangesAsync();

    return Results.Ok(comment);
});

// post upvote
app.MapPut("/posts/{id}/upvote", (int id) =>
{
    var postToUpvote = dbContext.Posts.Find(id);
    postToUpvote.Upvotes += 1;
    dbContext.SaveChanges();
    return postToUpvote; 
});

// post downvote
app.MapPut("/posts/{id}/downvote/", (int id) =>
{
    var postToDownvote = dbContext.Posts.Find(id);
    postToDownvote.Downvotes -= 1;
    dbContext.SaveChanges();
    return postToDownvote; 
});

//  comment upvote
app.MapPut("/comments/{id}/upvote/", (int id) =>
{
    var commentToUpvote = dbContext.Comments.Find(id);
    commentToUpvote.Upvotes += 1;
    dbContext.SaveChanges();
    return commentToUpvote; 
});

// comment downvote
app.MapPut("/comments/{id}/downvote/", (int id) =>
{
    var commentToDownvote = dbContext.Comments.Find(id);
    commentToDownvote.Downvotes -= 1;
    dbContext.SaveChanges();
    return commentToDownvote; 
});

app.Run();


