using Data;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Dodaj UniversityContext z konfiguracj� SQLite
builder.Services.AddDbContext<UniversityContext>(options =>
    options.UseSqlite("Data Source=university.db")); // Upewnij si�, �e plik bazy danych istnieje

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
