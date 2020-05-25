using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Contexts.Roles;
using Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Data.Contexts.Seeding {
    public class DBSeeding {

        private async static Task CompanySeed (ApplicationDbContext context) {
            if (context.Companies.AsQueryable ().Where (i => i.Name == "Test Aps").ToList ().Count == 0) {
                context.Companies.Add (
                    new Company () {
                        Name = "SpinOff"
                    }
                );
                context.Companies.Add (
                    new Company () {
                        Name = "Test Aps"
                    }
                );
                await context.SaveChangesAsync ();
            }
        }

        private async static Task CategoriesSeed (ApplicationDbContext context) {
            if (context.Categories.AsQueryable ().Where (c => c.CompanyId == 1).ToList ().Count == 0) {
                context.Categories.Add (new Category () {

                    CompanyId = 1,
                        Name = "Undervisning"
                });

                context.Categories.Add (new Category () {
                    CompanyId = 1,
                        Name = "Foredrag"
                });

                context.Categories.Add (new Category () {
                    CompanyId = 1,
                        Name = "Møder"
                });
                await context.SaveChangesAsync ();
            }
        }

        private async static Task QuestionSetSeed (ApplicationDbContext context) {
            if (context.QuestionSets.AsQueryable ().Where (c => c.Name.Contains ("Møder") || c.Name.Contains ("Undervisning") || c.Name.Contains ("Foredrag")).ToList ().Count == 0) {
                // defining the question sets
                var meetingSet = new QuestionSet ();
                meetingSet.QuestionSetId = Guid.NewGuid ();
                meetingSet.Name = "Møder";
                meetingSet.CompanyId = 1;
                meetingSet.active = true;
                meetingSet.Questions = new List<Question> (new Question[] {
                    new Question (Guid.NewGuid (), "Overordnet hvordan vurderer du mødet?", 0),
                        new Question (Guid.NewGuid (), "Hvordan vurderer du mødeledelsen?", 1),
                        new Question (Guid.NewGuid (), "Hvordan vurderer du din mulighed for at forberede dig?", 2),
                        new Question (Guid.NewGuid (), "Hvordan vurderer du indholdet?", 3),
                        new Question (Guid.NewGuid (), "Hvordan vurderer du kommunikationen på mødet?", 4),
                        new Question (Guid.NewGuid (), "Hvordan vurderer du værdien af mødet?", 5),
                        new Question (Guid.NewGuid (), "Hvordan vurderer du din indsats?", 6)
                });

                var lectureSet = new QuestionSet ();
                lectureSet.QuestionSetId = Guid.NewGuid ();
                lectureSet.Name = "Undervisning";
                lectureSet.CompanyId = 1;
                lectureSet.active = true;
                lectureSet.Questions = new List<Question> (new Question[] {
                    new Question (Guid.NewGuid (), "Hvordan vurderer du overordnet undervisningen?", 0),
                        new Question (Guid.NewGuid (), "Hvordan vurderer du tidsstyringen?", 1),
                        new Question (Guid.NewGuid (), "Hvordan vurderer du relevansen af indholdet?", 2),
                        new Question (Guid.NewGuid (), "Hvordan vurderer du underviserens faglighed?", 3),
                        new Question (Guid.NewGuid (), "Hvordan vurderer du underviserens formidlingsevne?", 4),
                        new Question (Guid.NewGuid (), "Hvordan vurderer du dit udbytte af undervisningen?", 5),
                        new Question (Guid.NewGuid (), "Hvordan vurderer du din egen indsats?", 6)
                });

                var talkSet = new QuestionSet ();
                talkSet.QuestionSetId = Guid.NewGuid ();
                talkSet.Name = "Foredrag";
                talkSet.CompanyId = 1;
                talkSet.active = true;
                talkSet.Questions = new List<Question> (new Question[] {
                    new Question (Guid.NewGuid (), "Hvordan vurderer du overordnet foredraget?", 0),
                        new Question (Guid.NewGuid (), "Hvordan vurderer du relevansen af indholdet?", 1),
                        new Question (Guid.NewGuid (), "Hvordan vurderer du foredragsholderens engagement?", 2),
                        new Question (Guid.NewGuid (), "Hvordan vurderer du foredragsholderens faglighed?", 3),
                        new Question (Guid.NewGuid (), "Hvordan vurderer du foredragsholderens formidlingsevne?", 4),
                        new Question (Guid.NewGuid (), "Hvordan vurderer du dit udbytte af foredraget?", 5),
                        new Question (Guid.NewGuid (), "Hvordan vurderer du tidsstyringen?", 6)
                });

                // add them to the context
                context.Add (meetingSet);
                context.Add (lectureSet);
                context.Add (talkSet);

                // save it to det DB
                await context.SaveChangesAsync ();
            }
        }

        private async static Task UsersSeed (ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) {
            if (await userManager.FindByEmailAsync ("admin@spinoff.com") == null) {

                var userone = new ApplicationUser () {
                CompanyId = 1,
                CompanyConfirmed = true,
                Email = "vadmin@spinoff.nu",
                UserName = "vadmin@spinoff.nu",
                Lastname = "Lund",
                Firstname = "Troels",
                EmailConfirmed = true,
                PhoneNumber = "26456660",
                };

                var usertwo = new ApplicationUser () {
                    CompanyId = 1,
                    CompanyConfirmed = true,
                    Email = "admin@spinoff.nu",
                    UserName = "admin@spinoff.nu",
                    Lastname = "Lund",
                    Firstname = "Troels",
                    EmailConfirmed = true,
                    PhoneNumber = "26456660",
                };

                var userthree = new ApplicationUser () {
                    CompanyId = 1,
                    CompanyConfirmed = true,
                    Email = "Facilitator@spinoff.com",
                    UserName = "Facilitator@spinoff.com",
                    Lastname = "Lund",
                    Firstname = "Troels",
                    EmailConfirmed = true,
                    PhoneNumber = "26456660",
                };

                var otherFirmUser = new ApplicationUser () {
                    CompanyId = 2,
                    CompanyConfirmed = true,
                    Email = "Facilitator@firm.nu",
                    UserName = "Facilitator@firm.nu",
                    Lastname = "Mr. I am a Facilitator",
                    Firstname = "Facilitator!",
                    EmailConfirmed = true,
                    PhoneNumber = "26456660",
                };

                // var userone = new ApplicationUser () { CompanyConfirmed = true, CompanyId = 1, UserName = "trolund@gmail.com", PhoneNumber = "29456660", Email = "trolund@gmail.com", EmailConfirmed = true };
                // var usertwo = new ApplicationUser () { CompanyConfirmed = true, CompanyId = 2, UserName = "spinoff@gmail.com", PhoneNumber = "29456660", Email = "spinoff@gmail.com", EmailConfirmed = true };
                // var userthree = new ApplicationUser () { CompanyConfirmed = true, CompanyId = 2, UserName = "Facilitator@gmail.com", PhoneNumber = "29456660", Email = "Facilitator@gmail.com", EmailConfirmed = true };
                await userManager.CreateAsync (userone, "Spinoff1234");
                await userManager.CreateAsync (usertwo, "Spinoff1234");
                await userManager.CreateAsync (userthree, "Spinoff1234");
                await userManager.CreateAsync (otherFirmUser, "Spinoff1234");

                await userManager.AddToRoleAsync (userone, Roles.Roles.VADMIN);
                await userManager.AddToRoleAsync (usertwo, Roles.Roles.ADMIN);
                await userManager.AddToRoleAsync (userthree, Roles.Roles.FACILITATOR);
                await userManager.AddToRoleAsync (otherFirmUser, Roles.Roles.FACILITATOR);
                await userManager.AddToRoleAsync (otherFirmUser, Roles.Roles.VADMIN);

            }
        }

        private static async Task CreateRoles (RoleManager<IdentityRole> roleManager) {

            string[] roles = { Roles.Roles.ADMIN, Roles.Roles.VADMIN, Roles.Roles.FACILITATOR };

            IdentityResult roleResult;

            foreach (string role in roles) {
                //here in this line we are adding Admin Role
                var roleCheck = await roleManager.RoleExistsAsync (role);
                if (!roleCheck) {
                    //here in this line we are creating admin role and seed it to the database
                    roleResult = await roleManager.CreateAsync (new IdentityRole (role));
                }
            }
        }

        public async static Task Seed (ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) {
            try {

                Console.WriteLine ("Seeding DB......");

                // creates test companies 
                await CompanySeed (context);

                // creates test categories 
                await CategoriesSeed (context);

                // creates test QuestionSets
                await QuestionSetSeed (context);

                // create the roles 
                await CreateRoles (roleManager);

                // creates some users to test with, most come after CreateRoles.
                await UsersSeed (context, userManager, roleManager);

            } catch (Exception e) {
                Console.WriteLine (e);
            }

        }
    }
}