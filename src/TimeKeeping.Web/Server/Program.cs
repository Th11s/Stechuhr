using Microsoft.AspNetCore.Identity;
using Th11s.TimeKeeping;
using Th11s.TimeKeeping.Data;
using Th11s.TimeKeeping.Data.Entities;
using TimeKeeping.Web.Server.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreServices(builder.Configuration);

//TODO: Identity benötigt mehr setup
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>(); ;

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 14;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.HttpOnly = true;
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(15);

    opt.LoginPath = "/Identity/Account/Login";
    opt.AccessDeniedPath = "/Identity/Account/AccessDenied";
    opt.SlidingExpiration = true;
});

//builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapMinimalApi();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
