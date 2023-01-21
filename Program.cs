using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "UserAuth";
})
        .AddCookie("UserAuth", x =>
         {
             x.LoginPath = "/User/Login";
             x.SlidingExpiration = true;
             //  x.ExpireTimeSpan = TimeSpan.FromSeconds(10);
         })
        .AddCookie("AdminAuth", x =>
        {
            x.LoginPath = "/Admin/Login";
        });
builder.Services.AddDataProtection().SetApplicationName("hanmudo")
                .PersistKeysToFileSystem(new DirectoryInfo(@"wwwroot\MyWebSite-keys")); ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseExceptionHandler("/Home/Error");
    //app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    //app.UseExceptionHandler("/Home/Error");

    //app.UseHsts();
}

app.UseHttpsRedirection();
//app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles(new StaticFileOptions()
{
    OnPrepareResponse = s =>
    {
        if ((s.Context.Request.Path.StartsWithSegments(new PathString("/galery/contentTeknik")) ||
        s.Context.Request.Path.StartsWithSegments(new PathString("/galery/darian")) ||
         s.Context.Request.Path.StartsWithSegments(new PathString("/galery/roleContent"))
        ) &&
           !s.Context.User.Identity.IsAuthenticated)
        {
            s.Context.Response.StatusCode = 401;
            s.Context.Response.Body = Stream.Null;
            s.Context.Response.ContentLength = 0;
        }
    }
});
var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
};
app.UseCookiePolicy(cookiePolicyOptions);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
