using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeCampSV
{
    public class UtilsDataAccess
    {
        public void UpdateAttendeesIdInSession()
        {
            AttendeesManager attendeesManager = new AttendeesManager();
            SessionsManager sessionsManager = new SessionsManager();

            List<SessionsResult> listSessions = sessionsManager.GetJustBaseTableColumns(new SessionsQuery());

            foreach (var sessionResult in listSessions)
            {
                List<AttendeesResult> listAttendees =
                    attendeesManager.GetJustBaseTableColumns(new AttendeesQuery() {Username = sessionResult.Username});
                sessionResult.Attendeesid = listAttendees[0].Id;
                sessionsManager.Update(sessionResult);
            }
        }


        public void SetAllAttendeesToYear2008()
        {
            AttendeesManager attendeesManager = new AttendeesManager();
           List<AttendeesResult> listAttendees = attendeesManager.GetJustBaseTableColumns(new AttendeesQuery());
            foreach (var attendee in listAttendees)
            {
                AttendeesCodeCampYearManager.I.Insert(new AttendeesCodeCampYearResult()
                                                          {
                                                              AttendeesId = attendee.Id,
                                                              CodeCampYearId = 3
                                                          });
            }

           
        }

    }
}
