using Personal_Blog;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run();

app.Run();

//Server server = new Server();

//app.MapGet("/new", async context =>
//{
//    context.Response.ContentType = "text/html; charset=utf-8";
//    await context.Response.SendFileAsync("html/new.html");
//});

//app.MapPost("/new", async context =>
//{
//    IFormCollection form = await context.Request.ReadFormAsync();

//    string title = form["title"];
//    string date = form["date"];
//    string content = form["content"];

//    await server.AddArticle(title, DateTime.Parse(date), content);

//    context.Response.Redirect("/new");
//});

//app.MapGet("/admin", async context =>
//{
//    context.Response.ContentType = "text/html; charset=utf-8";
//    await context.Response.SendFileAsync("html/admin.html");
//});


