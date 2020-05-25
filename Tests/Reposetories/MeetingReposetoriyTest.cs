using System;
using System.Linq;
using Business.Services.Interfaces;
using Data.Contexts;
using Data.Contexts.Seeding;
using Data.Models;
using Data.Repositories.Interface;
using Infrastructure.QueryParams;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Tests {
    public class MeetingReposetoriyTest : TestWithSqlite {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMeetingRepository _meetingR;
        private readonly IQuestionSetService _questionSetService;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public MeetingReposetoriyTest (IUnitOfWork u, IMeetingRepository meetingService, IQuestionSetService questionSetService, UserManager<ApplicationUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor) {
            _UnitOfWork = u;
            _meetingR = meetingService;
            _questionSetService = questionSetService;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;

            // run before each test
            DBSeeding.Seed (_context, _userManager, _roleManager).Wait ();
        }

        [Fact]
        public async void CreateMeetingTest () {

            var qset = _context.QuestionSets.First ();
            var user = _context.Users.First ();

            var newMeeting = new Meeting () { Name = "Test", StartTime = new DateTime (), EndTime = new DateTime (), QuestionsSetId = qset.QuestionSetId, ApplicationUserId = user.Id };
            _meetingR.CreateMeeting (newMeeting);

            Assert.Equal (true, await _UnitOfWork.SaveAsync ());
            Assert.NotNull (newMeeting);

            var meetingInDb = _context.Meetings.First ();
            Assert.NotNull (meetingInDb);
            Assert.Equal (newMeeting.Name, meetingInDb.Name);
        }

        [Fact]
        public async void GetMeetingTest () {
            var qset = _context.QuestionSets.First ();
            var user = _context.Users.First ();
            var newMeeting = new Meeting () { Name = "Test", StartTime = new DateTime (), EndTime = new DateTime (), QuestionsSetId = qset.QuestionSetId, ApplicationUserId = user.Id };
            _meetingR.CreateMeeting (newMeeting);

            Assert.Equal (true, await _UnitOfWork.SaveAsync ());

            var meeting = await _meetingR.GetMeeting (newMeeting.MeetingId);
            Assert.NotNull (meeting);
            Assert.Equal (newMeeting.Name, meeting.Name);
            Assert.Equal (newMeeting.MeetingId, meeting.MeetingId);
        }

        [Fact]
        public async void GetMeetingMultiTest () {
            var qset = _context.QuestionSets.First ();
            var user = _context.Users.First ();

            for (int i = 0; i < 10; i++) {
                var newMeeting = new Meeting () { Name = "Test" + i, StartTime = new DateTime (), EndTime = new DateTime (), QuestionsSetId = qset.QuestionSetId, ApplicationUserId = user.Id };
                _meetingR.CreateMeeting (newMeeting);
            }

            Assert.Equal (true, await _UnitOfWork.SaveAsync ());

            var col = await _UnitOfWork.Meetings.GetAll ();
            Assert.Equal (10, col.Count ());
        }

        [Fact]
        public async void UpdateMeetingTest () {
            var qset = _context.QuestionSets.First ();
            var user = _context.Users.First ();

            var newMeeting = new Meeting () { Name = "Test", StartTime = new DateTime (), EndTime = new DateTime (), QuestionsSetId = qset.QuestionSetId, ApplicationUserId = user.Id };
            _meetingR.CreateMeeting (newMeeting);

            Assert.Equal (true, await _UnitOfWork.SaveAsync ());
            Assert.Equal ("Test", newMeeting.Name);

            newMeeting.Name = "Test updated";

            _meetingR.UpdateMeeting (newMeeting);

            Assert.Equal (true, await _UnitOfWork.SaveAsync ());

            var updatedMeeting = await _meetingR.GetMeeting (newMeeting.MeetingId);

            Assert.Equal ("Test updated", updatedMeeting.Name);
        }

    }
}