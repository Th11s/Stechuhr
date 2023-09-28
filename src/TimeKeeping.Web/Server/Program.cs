using Microsoft.EntityFrameworkCore;
using Th11s.TimeKeeping.Data;

var builder = WebApplication.CreateBuilder(args);

var dbProvider = builder.Configuration.GetValue("DatabaseProvider", "npgsql");
switch(dbProvider)
{
    case "Npgsql":
        builder.Services.AddDbContext<NpgsqlDbContext>(
        dbContext =>
        {
            dbContext.UseNpgsql(builder.Configuration.GetConnectionString("npgsql"));
        });

        builder.Services.AddScoped<ApplicationDbContext, NpgsqlDbContext>();
        break;

    case "SqlServer":
        builder.Services.AddDbContext<SqlServerDbContext>(
        dbContext =>
        {
            dbContext.UseSqlServer(builder.Configuration.GetConnectionString("sqlserver"));
        });

        builder.Services.AddScoped<ApplicationDbContext, SqlServerDbContext>();
        break;
}

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
