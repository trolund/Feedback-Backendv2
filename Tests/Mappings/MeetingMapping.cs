using System;
using AutoMapper;
using Data.Models;
using Infrastructure.Utils;
using Infrastructure.ViewModels;
using Xunit;

namespace Tests {
    public class MeetingMapping {
        private readonly IMapper _mapper;

        public MeetingMapping (IMapper mapper) {
            _mapper = mapper;
        }

        [Fact]
        public void DaoToDto () {
            var dao = new Meeting () { Name = "Test", StartTime = new DateTime (), EndTime = new DateTime (), QuestionsSetId = Guid.NewGuid (), ApplicationUserId = "userID", MeetingId = 1 };

            var dto = _mapper.Map<MeetingDTO> (dao);

            Assert.Equal (dao.Name, dto.Name);
            Assert.Equal (dto.ShortId, MeetingIdHelper.GenerateShortId (dao.MeetingId));
            Assert.Equal (dao.StartTime, dto.StartTime);
            Assert.Equal (dao.EndTime, dto.EndTime);
        }

        [Fact]
        public void DtoToDao () {
            var meetingId = MeetingIdHelper.GenerateShortId (1);
            var dto = new MeetingDTO () { Name = "Test", StartTime = new DateTime (), EndTime = new DateTime (), QuestionsSetId = Guid.NewGuid ().ToString (), ShortId = meetingId };

            var dao = _mapper.Map<Meeting> (dto);

            Console.WriteLine (dao.ToString () + dao.MeetingId + " " + MeetingIdHelper.GetId (meetingId));

            Assert.Equal (dto.Name, dao.Name);
            Assert.Equal (dao.MeetingId, MeetingIdHelper.GetId (meetingId));
            Assert.Equal (dto.StartTime, dao.StartTime);
            Assert.Equal (dto.EndTime, dao.EndTime);
        }

    }
}