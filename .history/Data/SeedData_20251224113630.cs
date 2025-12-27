using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Models.Enums;
using TaskStatus = TaskManager.Models.Enums.TaskStatus;

namespace TaskManager.Data;

/// <summary>
/// Seed data for initializing roles and admin user
/// </summary>
public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Seed roles
        await SeedRoles(roleManager);

        // Seed users
        var adminUser = await SeedAdminUser(userManager);
        var managerUser = await SeedManagerUser(userManager);
        var memberUser1 = await SeedMemberUser(userManager, "member1@taskmanager.com", "Member123!");
        var memberUser2 = await SeedMemberUser(userManager, "member2@taskmanager.com", "Member123!");

        // Seed projects and tasks only if database is empty
        if (!await context.Projects.AnyAsync())
        {
            await SeedProjects(context, adminUser, managerUser);
            var projects = await context.Projects.Include(p => p.Tasks).ToListAsync();
            await SeedTasks(context, projects, memberUser1, memberUser2);
        }
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = { "ADMIN", "MANAGER", "MEMBER" };

        foreach (var roleName in roleNames)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    private static async Task<ApplicationUser?> SeedAdminUser(UserManager<ApplicationUser> userManager)
    {
        const string adminEmail = "admin@taskmanager.com";
        const string adminPassword = "Admin123!";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                Role = Role.ADMIN
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "ADMIN");
            }
        }
        return adminUser;
    }

    private static async Task<ApplicationUser?> SeedManagerUser(UserManager<ApplicationUser> userManager)
    {
        const string managerEmail = "manager@taskmanager.com";
        const string managerPassword = "Manager123!";

        var managerUser = await userManager.FindByEmailAsync(managerEmail);
        if (managerUser == null)
        {
            managerUser = new ApplicationUser
            {
                UserName = managerEmail,
                Email = managerEmail,
                EmailConfirmed = true,
                Role = Role.MANAGER
            };

            var result = await userManager.CreateAsync(managerUser, managerPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(managerUser, "MANAGER");
            }
        }
        return managerUser;
    }

    private static async Task<ApplicationUser?> SeedMemberUser(UserManager<ApplicationUser> userManager, string email, string password)
    {
        var memberUser = await userManager.FindByEmailAsync(email);
        if (memberUser == null)
        {
            memberUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                Role = Role.MEMBER
            };

            var result = await userManager.CreateAsync(memberUser, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(memberUser, "MEMBER");
            }
        }
        return memberUser;
    }

    private static async Task SeedProjects(ApplicationDbContext context, ApplicationUser? adminUser, ApplicationUser? managerUser)
    {
        if (adminUser == null || managerUser == null) return;

        var projects = new List<Project>
        {
            new Project
            {
                Name = "Website Redesign",
                Description = "Complete redesign of the company website with modern UI/UX",
                StartDate = DateTime.UtcNow.AddDays(-30),
                EndDate = DateTime.UtcNow.AddDays(60),
                Status = ProjectStatus.IN_PROGRESS,
                CreatorId = adminUser.Id,
                ManagerId = managerUser.Id
            },
            new Project
            {
                Name = "Mobile App Development",
                Description = "Develop a new mobile application for iOS and Android",
                StartDate = DateTime.UtcNow.AddDays(-15),
                EndDate = DateTime.UtcNow.AddDays(90),
                Status = ProjectStatus.IN_PROGRESS,
                CreatorId = adminUser.Id,
                ManagerId = managerUser.Id
            },
            new Project
            {
                Name = "API Integration",
                Description = "Integrate third-party APIs for payment processing",
                StartDate = DateTime.UtcNow.AddDays(-10),
                EndDate = DateTime.UtcNow.AddDays(45),
                Status = ProjectStatus.PLANNING,
                CreatorId = adminUser.Id,
                ManagerId = managerUser.Id
            },
            new Project
            {
                Name = "Database Migration",
                Description = "Migrate legacy database to new schema",
                StartDate = DateTime.UtcNow.AddDays(-60),
                EndDate = DateTime.UtcNow.AddDays(-5),
                Status = ProjectStatus.COMPLETED,
                CreatorId = adminUser.Id,
                ManagerId = managerUser.Id
            }
        };

        context.Projects.AddRange(projects);
        await context.SaveChangesAsync();
    }

    private static async Task SeedTasks(ApplicationDbContext context, List<Project> projects, ApplicationUser? memberUser1, ApplicationUser? memberUser2)
    {
        if (projects.Count == 0 || memberUser1 == null || memberUser2 == null) return;

        var tasks = new List<TaskItem>();

        // Tasks for Website Redesign project
        if (projects.Count > 0)
        {
            var project1 = projects[0];
            tasks.AddRange(new[]
            {
                new TaskItem
                {
                    Title = "Design Homepage Layout",
                    Description = "Create wireframes and mockups for the new homepage",
                    Priority = Priority.HIGH,
                    Status = TaskStatus.EN_COURS,
                    DueDate = DateTime.UtcNow.AddDays(10),
                    EstimatedHours = 16,
                    RealHours = 12,
                    ProjectId = project1.Id,
                    AssignedUserId = memberUser1.Id
                },
                new TaskItem
                {
                    Title = "Implement Responsive Navigation",
                    Description = "Build responsive navigation menu for all screen sizes",
                    Priority = Priority.MEDIUM,
                    Status = TaskStatus.A_FAIRE,
                    DueDate = DateTime.UtcNow.AddDays(15),
                    EstimatedHours = 8,
                    ProjectId = project1.Id,
                    AssignedUserId = memberUser2.Id
                },
                new TaskItem
                {
                    Title = "Optimize Images and Assets",
                    Description = "Compress and optimize all images for web",
                    Priority = Priority.LOW,
                    Status = TaskStatus.TERMINE,
                    DueDate = DateTime.UtcNow.AddDays(-5),
                    EstimatedHours = 4,
                    RealHours = 3,
                    ProjectId = project1.Id,
                    AssignedUserId = memberUser1.Id
                }
            });
        }

        // Tasks for Mobile App Development project
        if (projects.Count > 1)
        {
            var project2 = projects[1];
            tasks.AddRange(new[]
            {
                new TaskItem
                {
                    Title = "Setup React Native Project",
                    Description = "Initialize React Native project with necessary dependencies",
                    Priority = Priority.HIGH,
                    Status = TaskStatus.TERMINE,
                    DueDate = DateTime.UtcNow.AddDays(-10),
                    EstimatedHours = 6,
                    RealHours = 5,
                    ProjectId = project2.Id,
                    AssignedUserId = memberUser1.Id
                },
                new TaskItem
                {
                    Title = "Implement User Authentication",
                    Description = "Add login and signup functionality with JWT",
                    Priority = Priority.HIGH,
                    Status = TaskStatus.EN_COURS,
                    DueDate = DateTime.UtcNow.AddDays(20),
                    EstimatedHours = 20,
                    RealHours = 15,
                    ProjectId = project2.Id,
                    AssignedUserId = memberUser2.Id
                },
                new TaskItem
                {
                    Title = "Design App Icon",
                    Description = "Create app icon for iOS and Android",
                    Priority = Priority.MEDIUM,
                    Status = TaskStatus.A_FAIRE,
                    DueDate = DateTime.UtcNow.AddDays(25),
                    EstimatedHours = 8,
                    ProjectId = project2.Id,
                    AssignedUserId = memberUser1.Id
                }
            });
        }

        // Tasks for API Integration project
        if (projects.Count > 2)
        {
            var project3 = projects[2];
            tasks.AddRange(new[]
            {
                new TaskItem
                {
                    Title = "Research Payment APIs",
                    Description = "Evaluate different payment gateway options",
                    Priority = Priority.HIGH,
                    Status = TaskStatus.EN_COURS,
                    DueDate = DateTime.UtcNow.AddDays(5),
                    EstimatedHours = 12,
                    RealHours = 8,
                    ProjectId = project3.Id,
                    AssignedUserId = memberUser2.Id
                },
                new TaskItem
                {
                    Title = "Implement Stripe Integration",
                    Description = "Integrate Stripe payment processing",
                    Priority = Priority.HIGH,
                    Status = TaskStatus.A_FAIRE,
                    DueDate = DateTime.UtcNow.AddDays(20),
                    EstimatedHours = 16,
                    ProjectId = project3.Id,
                    AssignedUserId = memberUser1.Id
                }
            });
        }

        context.Tasks.AddRange(tasks);
        await context.SaveChangesAsync();
    }
}

