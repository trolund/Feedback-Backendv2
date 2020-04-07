using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Data.Contexts.Seeding {
    public class DBSeeding {
        public async static void Seed (ApplicationDbContext context, UserManager<ApplicationUser> userManager) {
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

                if (userManager.FindByEmailAsync ("trolund@gmail.com") == null) {

                    var userone = new ApplicationUser () { CompanyConfirmed = true, CompanyId = 1, UserName = "trolund@gmail.com", PhoneNumber = "29456660", Email = "trolund@gmail.com", EmailConfirmed = true };
                    var usertwo = new ApplicationUser () { CompanyConfirmed = true, CompanyId = 2, UserName = "spinoff@gmail.com", PhoneNumber = "29456660", Email = "spinoff@gmail.com", EmailConfirmed = true };
                    var userthree = new ApplicationUser () { CompanyConfirmed = true, CompanyId = 2, UserName = "Facilitator@gmail.com", PhoneNumber = "29456660", Email = "Facilitator@gmail.com", EmailConfirmed = true };

                    await userManager.CreateAsync (userone, "Spinoff1234");
                    await userManager.CreateAsync (usertwo, "Spinoff1234");
                    await userManager.CreateAsync (userthree, "Spinoff1234");

                    await userManager.AddToRoleAsync (userone, Roles.Roles.VADMIN);
                    await userManager.AddToRoleAsync (usertwo, Roles.Roles.ADMIN);
                    await userManager.AddToRoleAsync (usertwo, Roles.Roles.FACILITATOR);

                    context.SaveChanges ();
                }

                if (context.Categories.AsQueryable ().Where (c => c.CompanyId == 1).ToList ().Count == 0) {
                    context.Categories.Add (new Category () {
                        CompanyId = 1,
                            Name = "Undervisning"
                    });

                    context.Categories.Add (new Category () {
                        CompanyId = 1,
                            Name = "konference"
                    });

                    context.Categories.Add (new Category () {
                        CompanyId = 1,
                            Name = "Møder"
                    });
                    context.SaveChanges ();
                }

                if (context.QuestionSets.AsQueryable ().Where (c => c.Name.Contains ("test")).ToList ().Count == 0) {
                    // var list = new List<string> (new string[] {
                    //     "1 - Undervisning",
                    //     "2 - Klar komenikation",
                    //     "3 - Stop tidsspild",
                    //     "4 - DTU",
                    //     "5 - SpinOff custom",
                    //     "6 - Test Sæt"
                    // });

                    // list.ForEach (item => {
                    //     var qSet = new QuestionSet ();
                    //     qSet.QuestionSetId = Guid.NewGuid ();
                    //     qSet.Name = item;
                    //     qSet.Questions = new List<Question> ();
                    //     qSet.Questions = new List<Question> (new Question[] {
                    //         new Question (Guid.NewGuid (), "Spørgsmål 1"),
                    //             new Question (Guid.NewGuid (), "Spørgsmål 2"),
                    //             new Question (Guid.NewGuid (), "Spørgsmål 3"),
                    //             new Question (Guid.NewGuid (), "Spørgsmål 4"),
                    //             new Question (Guid.NewGuid (), "Spørgsmål 5")
                    //     });

                    //     context.QuestionSets.Add (qSet);
                    // });

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

                    context.SaveChanges ();
                }
            } catch (Exception e) {
                Console.WriteLine (e);
            }

        }
    }
}