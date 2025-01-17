using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EventManagement.Models;

namespace EventManagement.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Create admin role if it doesn't exist
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                // Create admin user if it doesn't exist
                string adminEmail = "amine_admin@eventmanagement.com";
                string adminPassword = "Admin123!";

                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new IdentityUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(adminUser, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }

                // Return if data exists
                if (context.Events.Any() || context.Participants.Any() || context.Registrations.Any())
                {
                    return;
                }

                // Add Events
                var events = new List<Event>
                {
                    new Event
                    {
                        Name = "Conférence Tech 2025",
                        Description = "Conférence annuelle sur beaucoup de choses tech",
                        Date = DateTime.Now.AddMonths(2),
                        Location = "Salle A, Paris",
                        Price = 35,
                        MaxParticipants = 500
                    },
                    new Event
                    {
                        Name = "Atelier Marketing Digital",
                        Description = "Workshop sur comment vendre du vent",
                        Date = DateTime.Now.AddMonths(1),
                        Location = "Salle A, Lyon",
                        Price = 75,
                        MaxParticipants = 50
                    },
                    new Event
                    {
                        Name = "Soirée Startups",
                        Description = "Soirée de networking pour entrepreneurs",
                        Date = DateTime.Now.AddDays(15),
                        Location = "Salle A, Paris",
                        Price = 15,
                        MaxParticipants = 100
                    },
                    new Event
                    {
                        Name = "Formation ASP.NET Core",
                        Description = "Formation intensive de 3 jours sur le Framework ASP.NET Core",
                        Date = DateTime.Now.AddMonths(3),
                        Location = "Salle B, Paris",
                        Price = 120,
                        MaxParticipants = 30
                    }
                };

                context.Events.AddRange(events);
                await context.SaveChangesAsync();

                // Add Participants
                var participants = new List<Participant>
                {
                    new Participant
                    {
                        Name = "Amine Elm",
                        Email = "amine.elmdad@example.fr"
                    },
                    new Participant
                    {
                        Name = "Marie Laurent",
                        Email = "marie.laurent@example.fr"
                    },
                    new Participant
                    {
                        Name = "Nicolas Martin",
                        Email = "nicolas.martin@example.fr"
                    },
                    new Participant
                    {
                        Name = "Sophie Bernard",
                        Email = "sophie.bernard@example.fr"
                    },
                    new Participant
                    {
                        Name = "Pierre Petit",
                        Email = "pierre.petit@example.fr"
                    }
                };

                context.Participants.AddRange(participants);
                await context.SaveChangesAsync();

                // Add Registrations
                var registrations = new List<Registration>
                {
                    // Inscriptions Conférence Tech
                    new Registration
                    {
                        EventId = events[0].Id,
                        ParticipantId = participants[0].Id,
                        RegistrationDate = DateTime.Now.AddDays(-5)
                    },
                    new Registration
                    {
                        EventId = events[0].Id,
                        ParticipantId = participants[1].Id,
                        RegistrationDate = DateTime.Now.AddDays(-4)
                    },

                    // Inscriptions Atelier Marketing
                    new Registration
                    {
                        EventId = events[1].Id,
                        ParticipantId = participants[2].Id,
                        RegistrationDate = DateTime.Now.AddDays(-3)
                    },
                    new Registration
                    {
                        EventId = events[1].Id,
                        ParticipantId = participants[3].Id,
                        RegistrationDate = DateTime.Now.AddDays(-2)
                    },

                    // Inscriptions Soirée Networking
                    new Registration
                    {
                        EventId = events[2].Id,
                        ParticipantId = participants[4].Id,
                        RegistrationDate = DateTime.Now.AddDays(-1)
                    },
                    new Registration
                    {
                        EventId = events[2].Id,
                        ParticipantId = participants[0].Id,
                        RegistrationDate = DateTime.Now
                    }
                };

                context.Registrations.AddRange(registrations);
                await context.SaveChangesAsync();
            }
        }
    }
}
