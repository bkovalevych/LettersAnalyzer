using LettersAnalyzer.Server.DataAccess;
using LettersAnalyzer.Server.Interfaces;
using LettersAnalyzer.Server.Settings;
using LettersAnalyzer.Server.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ArtWorkSettings>(
    builder.Configuration.GetSection("ArtWorkStoreDatabase"));

builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped<IArtWorkService, ArtWorkService>();
builder.Services.AddScoped<IFrequencyCounter, FrequencyCounter>();
builder.Services.AddScoped<SeedDataHelper>();

builder.Services.AddControllersWithViews();
//.AddJsonOptions(opts =>
//{
//    opts.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
//});
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
