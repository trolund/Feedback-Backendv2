using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Services.Interfaces;
using Data.Contexts;
using Data.Models;
using Infrastructure.QueryParams;
using Infrastructure.Utils;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Http;
using QRCoder;

namespace Business.Services {
    public class MeetingService : IMeetingService {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static QRCodeGenerator qrGenerator = new QRCodeGenerator ();

        private string _baseURL = "https://localhost:5001";

        public MeetingService (ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork) {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MeetingDTO> GetMeeting (int id) {
            return _mapper.Map<MeetingDTO> (await _unitOfWork.Meetings.Get (id));
        }

        public async Task<MeetingDTO> GetMeeting (string id) {
            return _mapper.Map<MeetingDTO> (await _unitOfWork.Meetings.GetMeeting (MeetingIdHelper.GetId (id)));
        }

        public async Task<IEnumerable<MeetingDTO>> GetMeetings (MeetingResourceParameters parameters) {
            return _mapper.Map<IEnumerable<MeetingDTO>> (await _unitOfWork.Meetings.GetMeetings (parameters));
        }

        public async Task CreateMeeting (MeetingDTO meeting) {
            var m = _mapper.Map<Meeting> (meeting);

            _unitOfWork.Meetings.CreateMeeting (m);
            await _unitOfWork.SaveAsync ();
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

            await _unitOfWork.SaveAsync ();

            // var oldMeeting = await _unitOfWork.Meetings.GetMeeting (intId);
            var oldMeeting = await _unitOfWork.Meetings.Get (intId);

            // if (oldMeeting != null) {

            // var meetingId = oldMeeting.MeetingId;
            // var userId = oldMeeting.CreatedBy;

            _mapper.Map (meeting, oldMeeting);
            // oldMeeting.CreatedBy = userId;
            // oldMeeting.MeetingId = intId;
            // oldMeeting.meetingCategories = oldMeeting.meetingCategories.Where (item => item.CategoryId != null).ToList ();
            oldMeeting.MeetingCategoryId = Guid.NewGuid ();

            await _unitOfWork.SaveAsync ();

            return meeting;

        }

        public async Task DeleteMeeting (MeetingDTO meeting) {
            var model = _mapper.Map<Meeting> (meeting);
            model.MeetingId = MeetingIdHelper.GetId (meeting.ShortId);
            _unitOfWork.Meetings.DeleteMeeting (model);
            await _unitOfWork.SaveAsync ();
        }

        public async Task<IEnumerable<MeetingDTO>> GetMeetings (MeetingDateResourceParameters parameters) {
            return _mapper.Map<List<MeetingDTO>> (await _unitOfWork.Meetings.GetMeetings (parameters));
        }

        public byte[] GetQRCode (string shortCodeId) {
            QRCodeData qrCodeData = qrGenerator.CreateQrCode ($"{_baseURL}/{shortCodeId}", QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode (qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic (20);
            return BitmapToBytes (qrCodeImage);
        }

        private static byte[] BitmapToBytes (Bitmap img) {
            using (MemoryStream stream = new MemoryStream ()) {
                img.Save (stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray ();
            }
        }

        public Task<IEnumerable<CategoryDTO>> GetMeetingCategories (int CompanyId) {
            return _unitOfWork.Meetings.GetMeetingCategories (CompanyId);
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