using Microsoft.AspNetCore.Identity;

namespace BibliotecaMVC.Data
{
    public static class IdentitySeeder
    {
        public static async Task InicializarAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            // 1. Crear los roles si no existen
            foreach (var rol in new[] { "Admin", "Usuario" })
            {
                if (!await roleManager.RoleExistsAsync(rol))
                {
                    await roleManager.CreateAsync(new IdentityRole(rol));
                }
            }

            // 2. Promover a Admin al usuario llamado "admin"
            var admin = await userManager.FindByNameAsync("admin");
            if (admin != null && !await userManager.IsInRoleAsync(admin, "Admin"))
            {
                if (await userManager.IsInRoleAsync(admin, "Usuario"))
                {
                    await userManager.RemoveFromRoleAsync(admin, "Usuario");
                }
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // 3. Asignar el rol "Usuario" a los usuarios que no tengan ningún rol
            foreach (var usuario in userManager.Users.ToList())
            {
                var roles = await userManager.GetRolesAsync(usuario);
                if (!roles.Any())
                {
                    await userManager.AddToRoleAsync(usuario, "Usuario");
                }
            }
        }
    }
}