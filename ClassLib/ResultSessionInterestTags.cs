using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


    
namespace CodeCampSV
{
    [DataObject(true)] // This attribute allows the ObjectDataSource wizard to see this class
    public class ResultSessionInterestTags
    {
        public string TagGroupName { get; set; }
        public int TotalAttendeesRegisteringInterest { get; set; }
        public int TotalAttendeesRegisteringInterestInTagGroup { get; set; }
        public int PercentInterest { get; set; }
    }

    public class ResultSessionInterestTagsClass
    {
        public ResultSessionInterestTagsClass()
        {
            
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<ResultSessionInterestTags> GetAll(int codeCampYearId,int attendeesId)
        {
            int totalAttendeesShowingInterestByYear;

            // 
            using (var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                var command =
                    new SqlCommand(SqlCountAttendeesShowingInterest, sqlConnection);
                command.Parameters.Add("@CodeCampYearId", SqlDbType.Int).Value = codeCampYearId;
                totalAttendeesShowingInterestByYear = (int)command.ExecuteScalar();
            }

            // get list of all the groups
            var attendeesTagListResults = AttendeesTagListManager.I.Get(new AttendeesTagListQuery
            {
                AttendeesId = attendeesId
            });


            var results = new List<ResultSessionInterestTags>();
            foreach (var attendeesTagListResult in attendeesTagListResults)
            {
                // count how many attendees for the current year have those tagsOfInterest in them
                using (var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                {
                    sqlConnection.Open();
                    var command =
                        new SqlCommand(SqlCountAttendeesForTagNameGroup, sqlConnection);
                    command.Parameters.Add("@CodeCampYearId", SqlDbType.Int).Value = codeCampYearId;
                    command.Parameters.Add("@AttendeesTagListId", SqlDbType.Int).Value = attendeesTagListResult.Id;

                    var totalByTagGroup = (int)command.ExecuteScalar();
                    results.Add(new ResultSessionInterestTags()
                                    {
                                        TagGroupName = attendeesTagListResult.TagListName,
                                        TotalAttendeesRegisteringInterest = totalAttendeesShowingInterestByYear,
                                        TotalAttendeesRegisteringInterestInTagGroup = totalByTagGroup,
                                        PercentInterest = Convert.ToInt32((Convert.ToDecimal(totalByTagGroup)*100/
                                                                           Convert.ToDecimal(
                                                                               totalAttendeesShowingInterestByYear)))
                                    });
                }
            }

            return results;
        }

        private const string SqlCountAttendeesShowingInterest = @"
            SELECT COUNT(*)
            FROM Attendees
            WHERE PKID IN (
                            SELECT DISTINCT SessionAttendee.Attendees_username
                            FROM dbo.SessionAttendee
                                 INNER JOIN dbo.Sessions ON (dbo.SessionAttendee.Sessions_id
                                 = dbo.Sessions.Id)
                            WHERE dbo.SessionAttendee.Interestlevel IN (2, 3) AND
                                  CodeCampYearId = @CodeCampYearId
                  )";

        private const string SqlCountAttendeesForTagNameGroup = @"
                SELECT COUNT(*)
                    FROM Attendees
                    WHERE PKID IN (
                                    SELECT distinct dbo.SessionAttendee.Attendees_username
                                    FROM dbo.SessionAttendee
                                    WHERE dbo.SessionAttendee.Interestlevel IN (2, 3) AND
                                          dbo.SessionAttendee.Sessions_id IN (
                                    SELECT Id
                                    From Sessions
                                    WHERE CodeCampYearId
                                    = @CodeCampYearId AND
                                            Id IN (
                                                    SELECT
                                                    SessionId
                                                    FROM
                                                    SessionTags
                                                    WHERE
                                                    TagId
                                                    IN (
                                                        SELECT
                                                        dbo.Tags.Id
                                                        FROM
                                                        dbo.AttendeesTagList
                                                        INNER JOIN
                                                        dbo.AttendeesTagListDetail
                                                        ON
                                                        (
                                                        dbo.AttendeesTagList.Id
                                                        =
                                                        dbo.AttendeesTagListDetail.AttendeesTagListId
                                                        )
                                                            INNER JOIN
                                                            dbo.Tags
                                                            ON
                                                            (dbo.AttendeesTagListDetail.TagsId = dbo.Tags.Id)
                                                            WHERE
                                                            dbo.AttendeesTagList.Id = @AttendeesTagListId
                                                                                   
                                                    )
                                                                                   
                                                )
                                                                                   
                                            )
                                                                                   
                                    )";
    }

   
}
