using AutoMapper;
using Data.Models;
using Infrastructure.Utils;
using Infrastructure.ViewModels;

namespace Data.Contexts_access {
    public class Profiles : Profile {
        public Profiles () {
            CreateMap<Meeting, MeetingDTO> ()
                .ForMember (d => d.ShortId, map => map.MapFrom (s => MeetingIdHelper.GenerateShortId (s.MeetingId)));

            CreateMap<FeedbackBatchDTO, FeedbackBatch> ()
                .ForMember (s => s.MeetingId, map => map.MapFrom (s => MeetingIdHelper.GetId (s.MeetingId)));
            CreateMap<FeedbackBatch, FeedbackBatchDTO> ()
                .ForMember (s => s.MeetingId, map => map.MapFrom (s => MeetingIdHelper.GenerateShortId (s.MeetingId)));

            CreateMap<FeedbackDTO, Feedback> ().ReverseMap ();

            CreateMap<Question, QuestionDTO> ().ReverseMap ();

            CreateMap<ApplicationUser, UserDTO> ();
            CreateMap<ApplicationUser, UserAdminDTO> ().ReverseMap ();

            CreateMap<QuestionSet, QuestionSetDTO> ().ReverseMap ();

            CreateMap<Company, CompanyDTO> ().ReverseMap ();
            CreateMap<CompanyDTO, CompanyDTO> ();

            CreateMap<MeetingDTO, Meeting> ()
                .ForMember (s => s.MeetingId, map => map.MapFrom (s => (s.ShortId != null ? MeetingIdHelper.GetId (s.ShortId) : 0)));

            CreateMap<Category, CategoryDTO> ().ReverseMap ();

            CreateMap<MeetingCategoryDTO, MeetingCategory> ().ForMember (s => s.MeetingId, map => map.MapFrom (s => s.MeetingId != null ? MeetingIdHelper.GetId (s.MeetingId) : 0));
            CreateMap<MeetingCategory, MeetingCategoryDTO> ().ForMember (d => d.MeetingId, map => map.MapFrom (s => s.MeetingId != 0 ? MeetingIdHelper.GenerateShortId (s.MeetingId) : null));

            CreateMap<ApplicationUser, UserDTO> ().ReverseMap ();
        }

    }

}