using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Contexts.Roles;
using Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Data.Contexts.Seeding {
    public class DBSeeding {

        public async static Task Seed (ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) {
            try {
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
                    context.SaveChanges ();
                }

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
                    var res = await context.SaveChangesAsync ();
                }

                if (context.QuestionSets.AsQueryable ().Where (c => c.Name.Contains ("Møder") || c.Name.Contains ("Undervisning") || c.Name.Contains ("Foredrag")).ToList ().Count == 0) {
                    // defining the question sets
                    var meetingSet = new QuestionSet ();
                    meetingSet.QuestionSetId = Guid.NewGuid ();
                    meetingSet.Name = "Møder";
                    meetingSet.Questions = new List<Question> (new Question[] {
                        new Question (Guid.NewGuid (), "Overordnet hvordan vurderer du mødet?"),
                            new Question (Guid.NewGuid (), "Hvordan vurderer du mødeledelsen?"),
                            new Question (Guid.NewGuid (), "Hvordan vurderer du din mulighed for at forberede dig?"),
                            new Question (Guid.NewGuid (), "Hvordan vurderer du indholdet?"),
                            new Question (Guid.NewGuid (), "Hvordan vurderer du kommunikationen på mødet?"),
                            new Question (Guid.NewGuid (), "Hvordan vurderer du værdien af mødet?"),
                            new Question (Guid.NewGuid (), "Hvordan vurderer du din indsats?")
                    });

                    var lectureSet = new QuestionSet ();
                    lectureSet.QuestionSetId = Guid.NewGuid ();
                    lectureSet.Name = "Undervisning";
                    lectureSet.Questions = new List<Question> (new Question[] {
                        new Question (Guid.NewGuid (), "Hvordan vurderer du overordnet undervisningen?"),
                            new Question (Guid.NewGuid (), "Hvordan vurderer du tidsstyringen?"),
                            new Question (Guid.NewGuid (), "Hvordan vurderer du relevansen af indholdet?"),
                            new Question (Guid.NewGuid (), "Hvordan vurderer du underviserens faglighed?"),
                            new Question (Guid.NewGuid (), "Hvordan vurderer du underviserens formidlingsevne?"),
                            new Question (Guid.NewGuid (), "Hvordan vurderer du dit udbytte af undervisningen?"),
                            new Question (Guid.NewGuid (), "Hvordan vurderer du din egen indsats?")
                    });

                    var talkSet = new QuestionSet ();
                    talkSet.QuestionSetId = Guid.NewGuid ();
                    talkSet.Name = "Foredrag";
                    talkSet.Questions = new List<Question> (new Question[] {
                        new Question (Guid.NewGuid (), "Hvordan vurderer du overordnet foredraget?"),
                            new Question (Guid.NewGuid (), "Hvordan vurderer du relevansen af indholdet?"),
                            new Question (Guid.NewGuid (), "Hvordan vurderer du foredragsholderens engagement?"),
                            new Question (Guid.NewGuid (), "Hvordan vurderer du foredragsholderens faglighed?"),
                            new Question (Guid.NewGuid (), "Hvordan vurderer du foredragsholderens formidlingsevne?"),
                            new Question (Guid.NewGuid (), "Hvordan vurderer du dit udbytte af foredraget?"),
                            new Question (Guid.NewGuid (), "Hvordan vurderer du tidsstyringen?")
                    });

                    // add them to the context
                    context.Add (meetingSet);
                    context.Add (lectureSet);
                    context.Add (talkSet);

                    // save it to det DB
                    context.SaveChanges ();
                }

                // create the roles 
                await CreateRoles (roleManager);

                if (await userManager.FindByEmailAsync ("admin@spinoff.com") == null) {

                    var userone = new ApplicationUser () {
                    CompanyId = 1,
                    CompanyConfirmed = true,
                    Email = "vadmin@spinoff.com",
                    UserName = "vadmin@spinoff.com",
                    Lastname = "Troels",
                    Firstname = "Lund",
                    EmailConfirmed = true,
                    PhoneNumber = "26456660",
                    };

                    var usertwo = new ApplicationUser () {
                        CompanyId = 1,
                        CompanyConfirmed = true,
                        Email = "admin@spinoff.com",
                        UserName = "admin@spinoff.com",
                        Lastname = "Troels",
                        Firstname = "Lund",
                        EmailConfirmed = true,
                        PhoneNumber = "26456660",
                    };

                    var userthree = new ApplicationUser () {
                        CompanyId = 1,
                        CompanyConfirmed = true,
                        Email = "Facilitator@spinoff.com",
                        UserName = "Facilitator@spinoff.com",
                        Lastname = "Troels",
                        Firstname = "Lund",
                        EmailConfirmed = true,
                        PhoneNumber = "26456660",
                    };

                    var otherFirmUser = new ApplicationUser () {
                        CompanyId = 2,
                        CompanyConfirmed = true,
                        Email = "Facilitator@firm.com",
                        UserName = "Facilitator@firm.com",
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

                }

            } catch (Exception e) {
                Console.WriteLine (e);
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
    }
}