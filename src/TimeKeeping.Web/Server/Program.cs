using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Th11s.TimeKeeping;
using Th11s.TimeKeeping.Auth;
using Th11s.TimeKeeping.Data;
using Th11s.TimeKeeping.Data.Entities;
using Th11s.TimeKeeping.Services;
using TimeKeeping.Web.Server.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreServices(builder.Configuration);

//TODO: Identity Setup weiter prüfen.
builder.Services.AddIdentity<User, IdentityRole>(opt =>
    {
        opt.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.Sub;
        opt.ClaimsIdentity.UserNameClaimType = JwtRegisteredClaimNames.Name;
        opt.ClaimsIdentity.EmailClaimType = JwtRegisteredClaimNames.Email;
        opt.ClaimsIdentity.RoleClaimType = "role";
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;

    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 12;
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

if (app.Configuration.GetValue<string?>("SetAdminPassword", null) is string password and not null)
{
    using(var scope = app.Services.CreateScope())
    {
        var eaas = scope.ServiceProvider.GetRequiredService<IAdminBenutzerService>();
        await eaas.ExecuteAsync(password, default);

        return;
    }
}



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
