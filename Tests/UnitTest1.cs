using System;
using System.Linq;
using Business.Services.Interfaces;
using Data.Contexts;
using Data.Contexts.Seeding;
using Data.Models;
using Data.Repositories.Interface;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests {
    public class MeetingServiceTest {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMeetingService _meetingService;
        private readonly IQuestionSetService _questionSetService;

        public MeetingServiceTest (IUnitOfWork u, IMeetingService meetingService, IQuestionSetService questionSetService) {
            _UnitOfWork = u;
            _meetingService = meetingService;
            _questionSetService = questionSetService;
        }

        [Fact]
        public async void Test1 () {
            // var qlist = await _UnitOfWork.QuestionSet.GetAll ();
            // var qid = qlist.First ().QuestionSetId;

            // var newMeeting = new MeetingDTO () {
            //     Discription = "Discription",
            //     EndTime = new DateTime (),
            //     StartTime = new DateTime (),
            //     Name = "name",
            //     QuestionsSetId = qid.ToString ()
            // };

            // await _meetingService.CreateMeeting (newMeeting);
        }

        [Fact]
        public void IsMeetingOpenPosetiveTest () {

            var newMeeting = new MeetingDTO () {
                Discription = "Discription",
                EndTime = DateTime.Now.AddHours (1),
                StartTime = DateTime.Now,
                Name = "posetiveTest",
                QuestionsSetId = "1234"
            };
            var res = _meetingService.TimeCheck (newMeeting);

            // meeting shout be open for feedback from det start time.
            Assert.Equal (true, res);
        }

        [Fact]
        public void IsMeetingOpenNegativeTest () {
            var newMeeting = new MeetingDTO () {
                Discription = "Discription",
                EndTime = DateTime.Now.AddDays (2).AddHours (1),
                StartTime = DateTime.Now.AddDays (2),
                Name = "name",
                QuestionsSetId = "1234"
            };
            var res = _meetingService.TimeCheck (newMeeting);

            // meeting shout not be open because it first starts in two days.
            Assert.Equal (false, res);
        }

        [Fact]
        public void IsMeetingOpenNegativeTestTwo () {
            var newMeeting = new MeetingDTO () {
                Discription = "Discription",
                EndTime = DateTime.Now.AddHours (2),
                StartTime = DateTime.Now.AddDays (1),
                Name = "name",
                QuestionsSetId = "1234"
            };
            var res = _meetingService.TimeCheck (newMeeting);

            // meeting shout not be open because it first starts in one hour.
            Assert.Equal (false, res);
        }
    }
}