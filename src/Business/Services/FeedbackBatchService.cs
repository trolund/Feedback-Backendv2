using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Business.Services.Interfaces;
using Data.Contexts;
using Data.Contexts.Roles;
using Data.Models;
using Infrastructure.Utils;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Business.Services {

    public class FeedbackBatchService : IFeedbackBatchService {

        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IMeetingService _meetingService;

        public FeedbackBatchService (ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMeetingService meetingService) {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _meetingService = meetingService;
        }

        public FeedbackBatchService () { }

        public async Task<bool> Create (FeedbackBatchDTO Feedback) {
            var model = _mapper.Map<FeedbackBatch> (Feedback);
            await _unitOfWork.FeedbackBatch.Add (model);
            return await _unitOfWork.SaveAsync ();
        }

        public Task Delete (FeedbackBatchDTO Feedback) {
            throw new NotImplementedException ();
        }

        public Task<IEnumerable<FeedbackBatchDTO>> GetAllFeedbackBatch (string meetingId) {
            throw new NotImplementedException ();
        }

        public async Task<IEnumerable<FeedbackBatchDTO>> GetAllFeedbackBatchByMeetingId (string meetingShortId) {
            var list = await _unitOfWork.FeedbackBatch.getAllFeedbackByMeetingId (MeetingIdHelper.GetId (meetingShortId));
            return list;
        }

        public Task<FeedbackBatchDTO> GetFeedbackBatch (string meetingId) {
            throw new NotImplementedException ();
        }

        public Task<IEnumerable<FeedbackBatchDTO>> GetFeedbackBatchByMeetingId (string meetingId) {
            throw new NotImplementedException ();
        }

        public Task<FeedbackBatchDTO> Update (FeedbackBatchDTO Feedback) {
            throw new NotImplementedException ();
        }

        public Task<IEnumerable<FeedbackBatchDTO>> GetAllFeedbackBatch () {
            throw new NotImplementedException ();
        }

        public async Task<IEnumerable<FeedbackBatchDTO>> OwnFeedback (DateTime start, DateTime end, string[] categories, string searchWord) {
            string companyId = null;
            string userId = null;

            if (_httpContextAccessor.HttpContext.User.IsInRole (Roles.VADMIN)) {
                companyId = _httpContextAccessor.HttpContext.User.Claims.Where (x => x.Type == "CID").First ().Value;
            }

            if (_httpContextAccessor.HttpContext.User.IsInRole (Roles.FACILITATOR)) {
                userId = _httpContextAccessor.HttpContext.User.Claims.Where (x => x.Type == ClaimTypes.NameIdentifier).First ().Value;
            }

            return await _unitOfWork.FeedbackBatch.OwnFeedback (start, end, categories, searchWord, userId, companyId);
        }

        public async Task<IEnumerable<FeedbackMonthDTO>> OwnFeedbackMonth (DateTime start, DateTime end, string[] categories, string searchWord, bool onlyOwnData) {
            string companyId = null;
            string userId = null;

            if (_httpContextAccessor.HttpContext.User.IsInRole (Roles.VADMIN)) {
                companyId = _httpContextAccessor.HttpContext.User.Claims.Where (x => x.Type == "CID").First ().Value;
            }

            if (_httpContextAccessor.HttpContext.User.IsInRole (Roles.FACILITATOR)) {
                userId = _httpContextAccessor.HttpContext.User.Claims.Where (x => x.Type == ClaimTypes.NameIdentifier).First ().Value;
                onlyOwnData = true;
            }

            return await _unitOfWork.FeedbackBatch.OwnFeedbackMonth (start, end, categories, searchWord, userId, companyId, onlyOwnData);
        }

        public async Task<IEnumerable<FeedbackDateDTO>> OwnFeedbackDate (DateTime start, DateTime end, string[] categories, string searchWord, bool onlyOwnData) {
            string companyId = null;
            string userId = null;

            if (!_httpContextAccessor.HttpContext.User.IsInRole (Roles.ADMIN)) {
                companyId = _httpContextAccessor.HttpContext.User.Claims.Where (x => x.Type == "CID").First ().Value;
                if (companyId == null) throw new ArgumentException ("Company ID was not precent in token.");
            }

            if ((_httpContextAccessor.HttpContext.User.IsInRole (Roles.FACILITATOR) && !_httpContextAccessor.HttpContext.User.IsInRole (Roles.VADMIN)) ||  onlyOwnData) {
                userId = _httpContextAccessor.HttpContext.User.Claims.Where (x => x.Type == ClaimTypes.NameIdentifier).First ().Value;
                onlyOwnData = true;
            }

            return await _unitOfWork.FeedbackBatch.OwnFeedbackDate (start, end, categories, searchWord, userId, companyId, onlyOwnData);
        }

        public async Task<double> GetUserRating () {
            var userId = _httpContextAccessor.HttpContext.User.Claims.Where (x => x.Type == ClaimTypes.NameIdentifier).First ().Value;

            return await _unitOfWork.FeedbackBatch.GetUserRating (userId);
        }

        public async Task<bool> HaveAlreadyGivenFeedback (string meetingId, string fingerprint) {
            var list = await _unitOfWork.FeedbackBatch.getFeedbackByFingerprintandMeetingId (MeetingIdHelper.GetId (meetingId), fingerprint);
            return list.Count > 0;
        }

    }
}