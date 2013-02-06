using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeCampSV;
using WebAPI.Code;
using WebAPI.ViewModels;

namespace WebAPI.Repositories
{
    public class PresenterTest : IRepositoryPresenter
    {
        public CommonViewModel GetDataForYear(string year)
        {
            return ControllerUtils.CommonViewModelTestData();
        }

        public CommonViewModel Detail(string year, string speakername)
        {
            var commonViewModel = ControllerUtils.CommonViewModelTestData();

            var parts = speakername.Split(new[] { '-' }).ToList();
            if (parts.Count == 3)
            {
                int attendeeId;
                if (Int32.TryParse(parts[2], out attendeeId))
                {
                    commonViewModel.Speakers =
                        commonViewModel.Speakers.Where(a => a.AttendeeId == attendeeId).ToList();
                }
            }
            return commonViewModel;
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
    }

  
}

