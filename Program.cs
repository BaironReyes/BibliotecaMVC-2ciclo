using BibliotecaMVC.Services;
using BibliotecaMVC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<BibliotecaContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BibliotecaDB"))
);

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Lockout.MaxFailedAccessAttempts = 5; // Número máximo de intentos fallidos antes de bloquear la cuenta
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // Tiempo de bloqueo
    options.Lockout.AllowedForNewUsers = true; // Permitir bloqueo para nuevos usuarios
}).AddEntityFrameworkStores<BibliotecaContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Ruta de inicio de sesión
    options.AccessDeniedPath = "/Account/AccessDenied"; // Ruta de acceso denegado
});



// Actividad 3: Registro de la Inyección de Dependencias.
// Se asocia la interfaz IAutorService con su implementación AutorService,
// usando el ciclo de vida Scoped (una instancia por cada petición HTTP).
builder.Services.AddScoped<IAutorService, AutorService>();

var app = builder.Build();

// Crear roles y promover al usuario admin al arrancar la aplicación
using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.InicializarAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();