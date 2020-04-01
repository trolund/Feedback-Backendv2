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

            // CreateMap<string, ApplicationUser> ().ConvertUsing (s => new ApplicationUser () { Id = s });
            CreateMap<Question, QuestionDTO> ().ReverseMap ();
            CreateMap<ApplicationUser, UserDTO> ();
            CreateMap<QuestionSet, QuestionSetDTO> ().ReverseMap ();
            CreateMap<Company, CompanyDTO> ();
            CreateMap<CompanyDTO, CompanyDTO> ();

            CreateMap<MeetingDTO, Meeting> ();

            CreateMap<Category, CategoryDTO> ().ReverseMap ();
            CreateMap<MeetingCategoryDTO, MeetingCategory> ().ForMember (s => s.MeetingId, map => map.MapFrom (s => MeetingIdHelper.GetId (s.MeetingId)));
            CreateMap<MeetingCategory, MeetingCategoryDTO> ().ForMember (d => d.MeetingId, map => map.MapFrom (s => MeetingIdHelper.GenerateShortId (s.MeetingId)));

            CreateMap<ApplicationUser, UserDTO> ().ReverseMap ();
            // .ForMember (m => m.CreatedBy, dto => dto.MapFrom (u => u.CreatedBy));
            /*
            .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.Id))
            .ReverseMap()
            .ForPath(dst => dst.CreatedBy.Id, opt => opt.MapFrom(src => src.CreatedBy));
            */
        }

    }

}