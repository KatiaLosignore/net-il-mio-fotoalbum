using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.CustomLoggers;
using net_il_mio_fotoalbum.Database;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("FotoalbumContextConnection") ?? throw new InvalidOperationException("Connection string 'FotoalbumContextConnection' not found.");
//var connectionString = builder.Configuration.GetConnectionString("FotoalbumContextConnection") ?? throw new InvalidOperationException("Connection string 'FotoalbumContextConnection' not found.");

builder.Services.AddCors(o => // https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-7.0
{
    o.AddDefaultPolicy(policy =>
    {
        policy.WithMethods("POST", "PUT", "GET", "OPTIONS");
        policy.WithOrigins("http://localhost:5173");
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
    });
});


builder.Services.AddDbContext<FotoalbumContext>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<FotoalbumContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Codice di cofigurazione per il serializzatore JSON, in modo che ignori completamente le dipendenze cicliche di
// eventuali relazione N:N o 1:N presenti nel JSON risultante.
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


// Righe per la (Dependency Injection)
builder.Services.AddScoped<ICustomLogger, CustomFileLogger>();
builder.Services.AddScoped<FotoalbumContext, FotoalbumContext>();
builder.Services.AddScoped<IRepositoryPhotos, RepositoryPhotos>();


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

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Fotoalbum}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
