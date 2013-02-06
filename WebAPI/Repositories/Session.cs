using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeCampSV;
using WebAPI.Code;
using WebAPI.ViewModels;

namespace WebAPI.Repositories
{
    public class Session : IRepositorySession
    {
        public CommonViewModel GetDataForYear(string year)
        {
            int codeCampYearId;
            var commonViewModel = ControllerUtils.UpdateViewModel
                (new CommonViewModel(), ControllerUtils.GetCodeCampYearId(year), out codeCampYearId);

            UpdateCommonViewWithSessions(commonViewModel, codeCampYearId);

            // not needed in production, just needed for generating a full commonview model for development
            ////UpdateCommonViewWithSpeakers(commonViewModel, codeCampYearId);
            ////if (false)
            ////{
            ////    // serialize commonViewModel
            ////    var ser = new XmlSerializer(typeof(CommonViewModel));
            ////    using (var ms = new MemoryStream())
            ////    {
            ////        ser.Serialize(ms, commonViewModel);
            ////        var bytes = ms.ToArray();
            ////        var xmlString = System.Text.Encoding.UTF8.GetString(bytes);
            ////        // put xmlString in /App_Data/CommonViewModel.xml with debugger (make sure to close file in vs or it locks it)
            ////    }
            ////}

            return commonViewModel;
        }

        public CommonViewModel Detail(string year, string session)
        {
            int codeCampYearId;
            var commonViewModel = ControllerUtils.UpdateViewModel
                (new CommonViewModel(), ControllerUtils.GetCodeCampYearId(year),out codeCampYearId);
            UpdateCommonViewWithSessions(commonViewModel, codeCampYearId);
            return ControllerUtils.GetCommonViewModelOneSession(session, commonViewModel);
        }

      

        private void UpdateCommonViewWithPresenters(CommonViewModel commonViewModel, int codeCampYearId)
        {
            commonViewModel.Speakers =
              AttendeesManager.I.GetSpeakerResults(new AttendeesQuery()
              {
                  CodeCampYearId = codeCampYearId,
                  PresentersOnly = true,
                  IncludeSessions = true
              });
        }

        private void UpdateCommonViewWithSpeakers(CommonViewModel commonViewModel, int codeCampYearId)
        {
            commonViewModel.Speakers =
            AttendeesManager.I.GetSpeakerResults(new AttendeesQuery()
            {
                CodeCampYearId = codeCampYearId,
                PresentersOnly = true,
                IncludeSessions = true
            });
        }

        private void UpdateCommonViewWithSessions(CommonViewModel commonViewModel, int codeCampYearId)
        {
            var sessions = SessionsManager.I.Get(new SessionsQuery
            {
                CodeCampYearId = codeCampYearId,
                WithInterestOrPlanToAttend = true,
                WithLectureRoom = true,
                WithSpeakers = true,
                WithTags = true,
                //Attendeesid = 1164 // nima
            });

            commonViewModel.Sessions = sessions;
            commonViewModel.SessionsByTime = ControllerUtils.SessionTimesResultsWithSessionInfo(codeCampYearId, sessions);
            commonViewModel.SessionTimeResults = SessionTimesManager.I.Get(new SessionTimesQuery
            {
                CodeCampYearId = codeCampYearId
            });
            commonViewModel.TagsResults = TagsManager.I.Get(new TagsQuery
            {
                CodeCampYearId = codeCampYearId
            });
        }
    }

    public interface IRepositorySession
    {
        CommonViewModel GetDataForYear(string year);
        CommonViewModel Detail(string year, string speakername);
    }
}

