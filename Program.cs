using Personal_Blog;
using System.Text;


var builder = WebApplication.CreateBuilder(args);



var app = builder.Build();

Server server = new Server();

app.MapGet("/home", async (context) =>
{
    List<Article> articles = server.GetArticles();
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("<h1>Personal Blog</h1>");
    stringBuilder.Append("<table>");
    foreach (Article article in articles)
    {
        stringBuilder.Append("<tr>");
        stringBuilder.Append($"<td><a href='/article/{article.Id}'>{article.Title}</a></td>");
        stringBuilder.Append($"<td>{article.Date.ToShortDateString()}</td>");
        stringBuilder.Append("</tr>");
    }
    stringBuilder.Append("</table>");
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(stringBuilder.ToString());
});

app.MapGet("/article/{id:int}", async (int id, HttpContext context) =>
{
    Article article = server.GetArticleById(id);

    StringBuilder sb = new StringBuilder();

    sb.Append($"<h1>{article.Title}</h1>");
    sb.Append($"<p>{article.Date.ToShortDateString()}</p>");
    sb.Append($"<p>{article.Content}</p>");

    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(sb.ToString());
});

app.MapGet("/admin", async (context) =>
{
    List<Article> articles = server.GetArticles();
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("<h1>Personal Blog</h1>");
    stringBuilder.Append("<table>");
    foreach (Article article in articles)
    {
        stringBuilder.Append("<tr>");
        stringBuilder.Append($"<td>{article.Title}</td>");
        stringBuilder.Append($"<td><a href='/edit/{article.Id}'>Edit</a></td>");
        stringBuilder.Append($"<td><a href='/delete/{article.Id}'>Delete</a></td>");
        stringBuilder.Append("</tr>");
    }
    stringBuilder.Append("</table>");
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(stringBuilder.ToString());

});

app.MapGet("/edit/{id:int}", async (int id, HttpContext context) =>
{
    StringBuilder sb = new StringBuilder();

    sb.Append("<h1>Update Article</h1>");
    sb.Append($"<form method='post' action='/edit/{id}'>");

    sb.Append("<div>");
    sb.Append("<label for='title'>Article Title</label><br>");
    sb.Append("<input type='text' id='title' name='title' placeholder='Article Title'>");
    sb.Append("</div>");

    sb.Append("<div>");
    sb.Append("<label for='date'>Publishing Date</label><br>");
    sb.Append("<input type='date' id='date' name='date'>");
    sb.Append("</div>");

    sb.Append("<div>");
    sb.Append("<label for='content'>Content</label><br>");
    sb.Append("<textarea id='content' name='content' rows='12' placeholder='Write your article here...'></textarea>");
    sb.Append("</div>");

    sb.Append("<input type='submit' value='Publish'>");

    sb.Append("</form>");

    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(sb.ToString());
});

app.MapPost("/edit/{id:int}", async (int id, HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();

    string title = form["title"];
    string content = form["content"];
    DateTime date = DateTime.Parse(form["date"]);

    await server.EditArticle(id, title, date, content);

    context.Response.Redirect("/admin");
});

app.MapGet("/delete/{id:int}", async (int id, HttpContext context) =>
{
    await server.DeleteArticle(id);

    context.Response.Redirect("/admin");
});


app.MapGet("/new", async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync("html/new.html");
});

app.MapPost("/new", async context =>
{
    IFormCollection form = await context.Request.ReadFormAsync();

    string title = form["title"];
    string date = form["date"];
    string content = form["content"];

    await server.AddArticle(title, DateTime.Parse(date), content);

    context.Response.Redirect("/admin");
});



app.Run();


