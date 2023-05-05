using AspControler.Repositories;
using AspControler.Services;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

Console.Clear();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<UserRepository>();
builder.Services.AddTransient<TicketRepository>();
builder.Services.AddTransient<QuestionsService>();
//builder.Services.AddDbContext<MvcMovieContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("MvcMovieContext")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapControllers();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

