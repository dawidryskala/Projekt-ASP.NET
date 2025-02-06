using Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using WebApplication1.Middleware;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Dodaj UniversityContext z konfiguracj� SQLite
builder.Services.AddDbContext<UniversityContext>(options =>
    options.UseSqlite("Data Source=university1.db")); // Upewnij si�, �e plik bazy danych istnieje

// Rejestracja serwis�w
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<LastVisitMiddleware>(); // Middleware do zapisu daty wizyty

// ?? Middleware rejestruj�ce logi HTTP ??
app.Use(async (context, next) =>
{
    Console.WriteLine("? Middleware dzia�a!"); // ?? Testowy log

    var requestContent = new StringBuilder();
    requestContent.AppendLine("=== Request Info ===");
    requestContent.AppendLine($"method = {context.Request.Method.ToUpper()}");
    requestContent.AppendLine($"path = {context.Request.Path}");
    requestContent.AppendLine("-- headers");

    foreach (var (headerKey, headerValue) in context.Request.Headers)
    {
        requestContent.AppendLine($"header = {headerKey} value = {headerValue}");
    }

    requestContent.AppendLine("-- body");
    context.Request.EnableBuffering();
    using var requestReader = new StreamReader(context.Request.Body, leaveOpen: true);
    var content = await requestReader.ReadToEndAsync();
    requestContent.AppendLine($"body = {content}");

    Console.WriteLine(requestContent.ToString()); // ?? Powinno by� w konsoli
    await File.AppendAllTextAsync("log.txt", requestContent.ToString() + Environment.NewLine);

    context.Request.Body.Position = 0;
    await next();
});




app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseDeveloperExceptionPage();


app.Run();
