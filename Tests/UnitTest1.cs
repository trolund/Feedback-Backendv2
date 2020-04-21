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
    public class UnitTest1 {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMeetingService _meetingService;
        private readonly IQuestionSetService _questionSetService;

        public UnitTest1 (IUnitOfWork u, IMeetingService meetingService, IQuestionSetService questionSetService) {
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
        public void Test2 () {

        }
    }
}