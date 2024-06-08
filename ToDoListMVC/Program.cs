using System.Xml;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Factory;
using ToDoList.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<ToDoListDbContext>();
builder.Services.AddSingleton<XmlStorageContext>();

builder.Services.AddSingleton<StorageChanger>();

builder.Services.AddSingleton<ToDoListDbPepository>();
builder.Services.AddSingleton<ToDoListXmlRepository>();

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

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();