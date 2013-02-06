using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeCampSV;
using WebAPI.Code;
using WebAPI.ViewModels;

namespace WebAPI.Repositories
{
    public class Presenter : IRepositoryPresenter
    {
        public CommonViewModel GetDataForYear(string year)
        {
            int codeCampYearId;
            var commonViewModel = ControllerUtils.UpdateViewModel
                (new CommonViewModel(), ControllerUtils.GetCodeCampYearId(year), out codeCampYearId);

            UpdateCommonViewWithPresenters(commonViewModel, codeCampYearId);
            return commonViewModel;
        }

        public CommonViewModel Detail(string year, string speakername)
        {
            int codeCampYearId;
            var commonViewModel = ControllerUtils.UpdateViewModel
                (new CommonViewModel(), ControllerUtils.GetCodeCampYearId(year), out codeCampYearId);

            commonViewModel.Speakers =
                AttendeesManager.I.GetSpeakerResults(new AttendeesQuery
                                                         {
                                                             PresentersOnly = true,
                                                             IncludeSessions = true,
                                                             SpeakerNameWithId = speakername,
                                                             CodeCampYearId =
                                                                 Utils
                                                                 .ConvertCodeCampYearToCodeCampYearId
                                                                 (year)
                                                         });
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

    public interface IRepositoryPresenter
    {
        CommonViewModel GetDataForYear(string year);
        CommonViewModel Detail(string year, string speakername);
    }
}

