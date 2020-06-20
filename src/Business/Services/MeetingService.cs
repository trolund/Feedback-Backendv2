using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Business.Services.Interfaces;
using Data.Contexts;
using Data.Contexts.Roles;
using Data.Models;
using Infrastructure.QueryParams;
using Infrastructure.Utils;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QRCoder;

namespace Business.Services {
    public class MeetingService : IMeetingService {
        private IUnitOfWork _unitOfWork;
        private ILogger<MeetingService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MeetingService (ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, ILogger<MeetingService> logger, IUserService userService) {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
        }

        public async Task<MeetingDTO> GetMeeting (int id) {
            return _mapper.Map<MeetingDTO> (await _unitOfWork.Meetings.Get (id));
        }

        public async Task<MeetingDTO> GetMeeting (string id, bool requireRole) {
            var dto = _mapper.Map<MeetingDTO> (await _unitOfWork.Meetings.GetMeeting (MeetingIdHelper.GetId (id), requireRole));

            var user = await _userService.getUserAndCompany (dto.CreatedBy);

            dto.UserEmail = user.Email;
            dto.CompanyName = user.CompanyName;

            return dto;
        }

        public async Task<IEnumerable<MeetingDTO>> GetMeetings (MeetingResourceParameters parameters) {

            return _mapper.Map<IEnumerable<MeetingDTO>> (await _unitOfWork.Meetings.GetMeetings (parameters));
        }

        public async Task<IEnumerable<MeetingDTO>> GetMeetingsOneDay (DateTime date) {
            var userId = (_httpContextAccessor.HttpContext.User.Claims.Where (x => x.Type == ClaimTypes.NameIdentifier).First ().Value);
            return _mapper.Map<IEnumerable<MeetingDTO>> (await _unitOfWork.Meetings.GetMeetingsOneDay (date, userId, true));
        }

        public async Task CreateMeeting (MeetingDTO meeting) {
            try {
                // var meetingCategories = meeting.meetingCategories;
                // meeting.meetingCategories = null;

                // if (meeting.meetingCategories != null) {
                //     foreach (var cat in meeting.meetingCategories) {
                //         cat.MeetingId = MeetingIdHelper.GenerateShortId (meetingToSave.MeetingId);
                //     }
                // }

                var meetingToSave = _mapper.Map<Meeting> (meeting);
                meetingToSave.ApplicationUserId = (_httpContextAccessor.HttpContext.User.Claims.Where (x => x.Type == ClaimTypes.NameIdentifier).First ().Value); // TODO need fix?

                _unitOfWork.Meetings.CreateMeeting (meetingToSave);

                // await _unitOfWork.SaveAsync ();

                // if (meetingCategories != null) {
                //     foreach (var cat in meetingToSave.meetingCategories) {
                //         cat.MeetingId = MeetingIdHelper.GenerateShortId (meetingToSave.MeetingId);
                //     }
                // }

                // meetingToSave.meetingCategories = meetingCategories;

                // await _unitOfWork.MeetingCategories.AddRange (_mapper.Map<ICollection<MeetingCategory>> (meetingCategories));

                if (await _unitOfWork.SaveAsync ()) {
                    _logger.LogInformation ("meeting " + meetingToSave.MeetingId + "have been created.", meetingToSave);
                } else {
                    // clean up if categories was not add correctly
                    // _unitOfWork.Meetings.DeleteMeeting (meetingToSave);
                    throw new ArgumentException ("Meeting was not created.");
                }
            } catch (Exception e) {
                _logger.LogError ("meeting have NOT been created.");
                throw new SERLException ("Meeting was not created.", e);
            }
        }

        public bool MeetingExists (int id) {
            return _unitOfWork.Meetings.MeetingExists (id);
        }

        public async Task<MeetingDTO> UpdateMeeting (MeetingDTO meeting) {
            var intId = MeetingIdHelper.GetId (meeting.ShortId);
            meeting.MeetingId = intId;

            var oldMeetingCategory = await _unitOfWork.MeetingCategories.getMeetingCategoriesForMeeting (intId);
            if (oldMeetingCategory != null) {
                _unitOfWork.MeetingCategories.RemoveRange (oldMeetingCategory);
            }

            // await _unitOfWork.SaveAsync ();

            // var oldMeeting = await _unitOfWork.Meetings.GetMeeting (intId);
            var oldMeeting = await _unitOfWork.Meetings.Get (intId);

            // if (oldMeeting != null) {

            // var meetingId = oldMeeting.MeetingId;
            // var userId = oldMeeting.CreatedBy;

            _mapper.Map (meeting, oldMeeting);
            // oldMeeting.CreatedBy = userId;
            // oldMeeting.MeetingId = intId;
            // oldMeeting.meetingCategories = oldMeeting.meetingCategories.Where (item => item.CategoryId != null).ToList ();
            // oldMeeting.MeetingCategoryId = Guid.NewGuid ();

            if (await _unitOfWork.SaveAsync ()) {
                return meeting;
            }

            throw new SERLException ("meeting was not updated");
        }

        public async Task DeleteMeeting (MeetingDTO meeting) {
            try {
                var meetingId = MeetingIdHelper.GetId (meeting.ShortId);
                var model = await _unitOfWork.Meetings.GetMeeting (meetingId, false);
                _unitOfWork.Meetings.DeleteMeeting (model);
                await _unitOfWork.SaveAsync ();
            } catch (Exception e) {
                _logger.LogError ("meeting was not deleted", e);
                throw new SERLException ("Meeting was not deleted.", e);
            }
        }

        public async Task<IEnumerable<MeetingDTO>> GetMeetings (MeetingDateResourceParameters parameters) {
            string id = _httpContextAccessor.HttpContext.User.Claims.Where (c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault ().Value;
            return _mapper.Map<List<MeetingDTO>> (await _unitOfWork.Meetings.GetMeetings (parameters, id, _httpContextAccessor.HttpContext.User.IsInRole (Roles.VADMIN), _httpContextAccessor.HttpContext.User.IsInRole (Roles.ADMIN)));
        }

        // public byte[] GetQRCode (string shortCodeId) {
        //     QRCodeData qrCodeData = qrGenerator.CreateQrCode ($"{_baseURL}/{shortCodeId}", QRCodeGenerator.ECCLevel.Q);
        //     QRCode qrCode = new QRCode (qrCodeData);
        //     Bitmap qrCodeImage = qrCode.GetGraphic (20);
        //     return BitmapToBytes (qrCodeImage);
        // }

        // private static byte[] BitmapToBytes (Bitmap img) {
        //     using (MemoryStream stream = new MemoryStream ()) {
        //         img.Save (stream, System.Drawing.Imaging.ImageFormat.Png);
        //         return stream.ToArray ();
        //     }
        // }

        public Task<IEnumerable<CategoryDTO>> GetMeetingCategories (int CompanyId) {
            return _unitOfWork.Meetings.GetMeetingCategories (CompanyId);
        }

        public async Task<bool> IsMeetingOpenForFeedback (string id) {
            var meeting = await GetMeeting (id, false);
            if (meeting != null) {
                return TimeCheck (meeting);
            }
            return false;
        }

        public bool TimeCheck (MeetingDTO meeting) {
            var endOfFeedback = meeting.EndTime.AddHours (12);
            var d = DateTime.Compare (DateTime.Now, endOfFeedback);
            var openafter = d <= 0;

            var d2 = DateTime.Compare (DateTime.Now, meeting.StartTime);
            var notBeforeMeeting = d2 >= 0;

            return openafter && notBeforeMeeting;
        }

        // private List<MeetingDTO> addShortId(IEnumerable<MeetingDTO> DTOList){
        //     var  list = DTOList.ToList();
        //     for(int i = 0; i <= list.Count; i++){
        //         list[i].ShortId = MeetingIdHelper.GenerateShortId(list[i].MeetingId);
        //     }
        //     return list;
        // }
    }
}