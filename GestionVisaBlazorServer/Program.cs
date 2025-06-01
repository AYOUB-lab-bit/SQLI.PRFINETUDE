using GestionVisaBlazorServer.Data;
using GestionVisaBlazorServer.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuration du DbContext avec SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services nécessaires pour Blazor Server
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();

// Ajout de ton service ExportService
builder.Services.AddScoped<ExportService>();

// *** IMPORTANT : forcer les urls d’écoute pour éviter problèmes de port HTTPS ***
builder.WebHost.UseUrls("http://localhost:5287", "https://localhost:7016");

var app = builder.Build();

// Pipeline Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ** Ici on retire totalement Authentication et Authorization **
// app.UseAuthentication();
// app.UseAuthorization();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
