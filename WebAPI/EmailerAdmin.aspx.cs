using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.UI.WebControls;
using aspNetEmail;
using CodeCampSV;
using System.Text.RegularExpressions;

public partial class EmailerAdmin : BaseContentPage
{
    //List<string> EmailAddressList;

    private Dictionary<string, string> _dictionaryOfEmailAll;
    private Dictionary<string, string> _dictionaryOfEmailSubscriber1;
    private Dictionary<string, string> _dictionaryOfEmailSubscriber2;

    private Dictionary<string, AttendeesResult> _dictionaryAllAttendeeResultByEmail;

    //cells[0, 0].Value = "Id";
    //cells[0, 1].Value = "FirstName";
    //cells[0, 2].Value = "LastName";
    //cells[0, 3].Value = "Website";
    //cells[0, 4].Value = "AddressLine1";
    //cells[0, 5].Value = "City";
    //cells[0, 6].Value = "State";
    //cells[0, 7].Value = "Zipcode";
    //cells[0, 8].Value = "Email";
    //cells[0, 9].Value = "PhoneNumber";

    /// <summary>
    /// used for xml output
    /// </summary>
    public class CodeCampAttendeesBarCodeInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WebSiteUrl { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }


    protected void Page_PreLoad(object sender, EventArgs e)
    {
        //CheckBoxListEmail.DataBind();
        //LabelCurrentStatus.Text = "Records Retrieved: " + CheckBoxListEmail.Items.Count;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //EmailAddressList = new List<string>();

        LabelCount.Text = string.Empty;
        if (HttpContext.Current.Cache[Utils.CacheMailSentStatusName] != null)
        {
            LabelCurrentStatus.Text = HttpContext.Current.Cache[Utils.CacheMailSentStatusName] as string;
        }

        if (!IsPostBack)
        {
            TextBoxSubject.Text = "PUT SUBJECT HERE!!!!";
            TextBoxFrom.Text = Utils.GetServiceEmailAddress();

            LabelCurrentCodeCampYearId.Text = Utils.CurrentCodeCampYear.ToString();

        }

        var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
        sqlConnection.Open();

       

        try
        {
            _dictionaryOfEmailAll =
                GetDictionaryFromSqlAndConnection(sqlConnection,
                                                  "SELECT Email,PKID FROM Attendees");
            _dictionaryOfEmailSubscriber1 =
                GetDictionaryFromSqlAndConnection(sqlConnection,
                                                  "SELECT Email,PKID FROM Attendees WHERE EmailSubscription = 1");
            _dictionaryOfEmailSubscriber2 =
                GetDictionaryFromSqlAndConnection(sqlConnection,
                                                  "SELECT Email,PKID FROM Attendees WHERE EmailSubscription = 2");

            var attendeeResultsList = AttendeesManager.I.GetAll();
            _dictionaryAllAttendeeResultByEmail = new Dictionary<string, AttendeesResult>();
            foreach (var attendee in attendeeResultsList)
            {
                if (!_dictionaryAllAttendeeResultByEmail.ContainsKey(attendee.Email))
                {
                    _dictionaryAllAttendeeResultByEmail.Add(attendee.Email, attendee);
                }
            }
        }
        catch (Exception eee1)
        {
            throw new ApplicationException(eee1.ToString());
        }


    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        // mark EmailSubscriber=1 as YELLOW (no mail and uncheck)
        // mark bouncing as RED (no mail and uncheck)





       



        // number checkboxes
        var itemList = CheckBoxListEmail.Items;
        int cnt = 0;
        foreach (ListItem checkItem in itemList)
        {
            if (_dictionaryOfEmailSubscriber1.ContainsKey(checkItem.Value))
            {
                checkItem.Attributes.Add("style", "background-color:yellow;");
            }

            if (_dictionaryOfEmailSubscriber2.ContainsKey(checkItem.Value))
            {
                checkItem.Attributes.Add("style", "background-color:red;");
            }


            checkItem.Text = cnt + ": " + checkItem.Text;
            cnt++;
        }



    }

    protected void ButtonSelectAll_Click(object sender, EventArgs e)
    {
        int start = Convert.ToInt32(TextBoxSelectStart.Text);
        int end = Convert.ToInt32(TextBoxSelectEnd.Text);
        int cnt = 0;
        int cntSelected = 0;
        foreach (ListItem cb in CheckBoxListEmail.Items)
        {
            cnt++;

            if (CheckBoxIncludeNoMailAndBouncing.Checked)
            {
                if (cnt >= start && cnt <= end)
                {
                    cb.Selected = true;
                }
                else
                {
                    cb.Selected = false;
                }
            }
            else
            {
                if (_dictionaryOfEmailSubscriber1.ContainsKey(cb.Value) ||
                    _dictionaryOfEmailSubscriber2.ContainsKey(cb.Value))
                {
                    cb.Selected = false;
                }
                else
                {
                    if (cnt >= start && cnt <= end)
                    {
                        cb.Selected = true;
                    }
                    else
                    {
                        cb.Selected = false;               
                    }
                }
            }

            if (cb.Selected)
            {
                cntSelected++;
            }

        }

        LabelCount.Text = cnt + " Selected: " + cntSelected;
    }

    protected void ButtonUnSelectAll_Click(object sender, EventArgs e)
    {
        foreach (ListItem cb in CheckBoxListEmail.Items)
        {
            cb.Selected = false;
        }
        LabelCount.Text = "Mail SendingThread Started.  Do not press again.";
    }

    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RadioButtonList1.SelectedValue.Equals("Custom"))
        {
           

            string sql; // secret trick here.
            if (TextBoxCustomSqlWhere.Text.StartsWith("..."))
            {
                string alphaWhereClause = string.Empty;
                if (!String.IsNullOrEmpty(TextBoxStartAlpha.Text) && !String.IsNullOrEmpty(TextBoxEndAlpha.Text))
                {
                    alphaWhereClause = String.Format(" AND LEFT(UserLastName, 1) BETWEEN '{0}' AND '{1}'", TextBoxStartAlpha.Text, TextBoxEndAlpha.Text);
                }

                // {1} === "AND LEFT(UserLastName, 1) BETWEEN 'A' AND 'F'
                sql = string.Format("SELECT Email,UserFirstName,UserLastName from ATTENDEES WHERE ID IN ({0}) {1} ORDER BY UserLastName ASC, UserFirstName ASC",
                                    TextBoxCustomSqlWhere.Text.Substring(3),alphaWhereClause);
            }
            else
            {
                // {1} === "AND LEFT(UserLastName, 1) BETWEEN 'A' AND 'F'
                sql = "SELECT EMAIL,UserFirstName,UserLastName FROM ATTENDEES WHERE ID IN (SELECT ID FROM ATTENDEES) ORDER BY UserLastName ASC, UserFirstName ASC";
            }

          

            CheckBoxListEmail.Items.Clear();
            List<AttendeesShort> attendeesShortList = Utils.GetEmailWithSql(sql);
            CheckBoxListEmail.Items.Clear();
            foreach (var rec in attendeesShortList)
            {
                string showString = rec.EmailAddress  + ": " + rec.LastName  + ", " + rec.FirstName ;
                CheckBoxListEmail.Items.Add(new ListItem(showString,rec.EmailAddress));
            }
        }
        if (RadioButtonList1.SelectedValue.Equals("All Attendees"))
        {
            ObjectDataSourceAttendees.SelectMethod = "GetAllUsers";
            CheckBoxListEmail.DataBind();
        }
        if (RadioButtonList1.SelectedValue.Equals("All Attendees Registered This Year"))
        {
            ObjectDataSourceAttendees.SelectMethod = "GetAllUsersByCodeCampYearId";
            CheckBoxListEmail.DataBind();
        }
        if (RadioButtonList1.SelectedValue.Equals("All Attendees Not Registered This Year"))
        {
            ObjectDataSourceAttendees.SelectMethod = "GetAttendeesNotRegisteredThisYear";
            CheckBoxListEmail.DataBind();
        }
        else if (RadioButtonList1.SelectedValue.Equals("All Presenters"))
        {
            ObjectDataSourceAttendees.SelectMethod = "GetAllPresenters";
            CheckBoxListEmail.DataBind();
        }
        else if (RadioButtonList1.SelectedValue.Equals("All Presenters This Year No Shirt Size"))
        {
            ObjectDataSourceAttendees.SelectMethod = "GetPresentersWithNoShirtSizeThisCCYear";
            CheckBoxListEmail.DataBind();
        }
        else if (RadioButtonList1.SelectedValue.Equals("All Presenters This Year With Shirt Size"))
        {
            ObjectDataSourceAttendees.SelectMethod = "GetPresentersWithShirtSizeThisCCYear";
            CheckBoxListEmail.DataBind();
        }
        else if (RadioButtonList1.SelectedValue.Equals("GetAllAttendeesWhereAttendeeVolunteerChecked"))
        {
            ObjectDataSourceAttendees.SelectMethod = "GetAllAttendeesWhereAttendeeVolunteerChecked";
            CheckBoxListEmail.DataBind();
        }
        else if (RadioButtonList1.SelectedValue.Equals("GetAllUsersByCodeCampYearIdWhoActuallyNotVolunteeredNotPresenter"))
        {
            ObjectDataSourceAttendees.SelectMethod = "GetAllUsersByCodeCampYearIdWhoActuallyNotVolunteeredNotPresenter";
            CheckBoxListEmail.DataBind();
        }
        else if (RadioButtonList1.SelectedValue.Equals("GetAllUsersByCodeCampYearIdWhoActuallyVolunteeredNotPresenter"))
        {
            ObjectDataSourceAttendees.SelectMethod = "GetAllUsersByCodeCampYearIdWhoActuallyVolunteeredNotPresenter";
            CheckBoxListEmail.DataBind();
        }
        else if (RadioButtonList1.SelectedValue.Equals("GetAllPresentersByCodeCampYearId"))
        {
            ObjectDataSourceAttendees.SelectMethod = "GetAllPresentersByCodeCampYearId";
            CheckBoxListEmail.DataBind();
        }
        else if (RadioButtonList1.SelectedValue.Equals("GetAllPresentersByCodeCampYearIdNotRegistered"))
        {
            ObjectDataSourceAttendees.SelectMethod = "GetAllPresentersByCodeCampYearIdNotRegistered";
            CheckBoxListEmail.DataBind();
        }
        else if (RadioButtonList1.SelectedValue.Equals("GetDataByPresentersAllYears"))
        {
            ObjectDataSourceAttendees.SelectMethod = "GetDataByPresentersAllYears";
            CheckBoxListEmail.DataBind();
        }
        else if (RadioButtonList1.SelectedValue.Equals("All Not Registed All CC Years"))
        {
            List<ListItem> checkBoxListItems = Utils.LoadAllUnRegisteredCodeCampUsers();
            CheckBoxListEmail.Items.Clear();
            foreach (ListItem checkListItem in checkBoxListItems)
            {
              CheckBoxListEmail.Items.Add(checkListItem);
            }
        }

        if (LiteralCount != null)
        {
            LiteralCount.Text = string.Format("Items Found: {0}", CheckBoxListEmail.Items.Count);
        }

        //<asp:ListItem>Vista Ultimatum</asp:ListItem>
        //<asp:ListItem>Vista Signed Up And Responded</asp:ListItem>
    }

    protected void ButtonProcessSelection_Click(object sender, EventArgs e)
    {
        int repeatEachEmail = Convert.ToInt32(TextBoxRepeatEachEmail.Text);
        int emailsPerHour = Convert.ToInt32(TextBoxEmailsPerHour.Text);
        //List<string> emailOptOut2008 = Utils.GetDoNotRemoveList("CodeCampSV06");

        var dictionaryOfAllPkiDsByEmail = new Dictionary<string, string>();

        var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
        sqlConnection.Open();
        
        try
        {
            dictionaryOfAllPkiDsByEmail = GetDictionaryFromSqlAndConnection(sqlConnection, "SELECT Email,PKID FROM Attendees");
        }
        catch (Exception eee1)
        {
            throw new ApplicationException(eee1.ToString());
        }
        

        sqlConnection.Close();

        var pet = new ProcessEmailThread
                      {
                          emailsPerHour = emailsPerHour,
                          logFile = (MapPath(string.Empty) + "\\App_Data\\EmailTest2.log"),
                          logFileStatus = (MapPath(string.Empty) + "\\App_Data\\EmailTestStatus.log"),
                          subject = TextBoxSubject.Text,
                          body = TextBoxNote.Text,
                          toEmailList = new List<string>(),
                          fromEmail = TextBoxFrom.Text,
                          cache = Cache,
                          DictionaryOfPKIDsByEmail = dictionaryOfAllPkiDsByEmail,
                          repeatEachEmail = repeatEachEmail
                      };


        Cache[Utils.CacheMailCancelFlag] = "false";
        var str = (string) HttpContext.Current.Cache[Utils.CacheMailSentStatusName];


        ThreadStart job = pet.GenerateMails;
        var thread = new Thread(job);


        var sbErrors = new StringBuilder();
        foreach (ListItem li in CheckBoxListEmail.Items)
        {
            if (li.Selected)
            {
                string emailAddress = li.Value;
                //if (!emailOptOut2008.Contains(emailAddress))
                //{
                    pet.toEmailList.Add(emailAddress);
                    ButtonProcessSelection.Enabled = false;
                    LabelSentStatus.Text = "Thread Launched To Send " + pet.toEmailList.Count +
                                           " Messages.  Do not click send again.";
                //}
            }
        }
        thread.Start();
    }


    // send all this to the EmailDetail for processing by the console program
    protected void ButtonProcessSelectionToEmailDetail_Click(object sender, EventArgs e)
    {
        int repeatEachEmail = Convert.ToInt32(TextBoxRepeatEachEmail.Text);
        int emailsPerHour = Convert.ToInt32(TextBoxEmailsPerHour.Text);
        //List<string> emailOptOut2008 = Utils.GetDoNotRemoveList("CodeCampSV06");

        var dictionaryOfAllPkiDsByEmail = new Dictionary<string, string>();
        var dictionaryOfAllIdsByEmail = new Dictionary<string, int>();

        using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
        {
            sqlConnection.Open();

            try
            {
                dictionaryOfAllPkiDsByEmail = GetDictionaryFromSqlAndConnection(sqlConnection,
                                                                                "SELECT Email,PKID FROM Attendees");
                dictionaryOfAllIdsByEmail = GetDictionaryFromSqlAndConnectionById(sqlConnection,
                                                                                  "SELECT Email,Id FROM Attendees");
            }
            catch (Exception eee1)
            {
                throw new ApplicationException(eee1.ToString());
            }


            //var pet = new ProcessEmailThread
            //              {
            //                  emailsPerHour = emailsPerHour,
            //                  logFile = (MapPath(string.Empty) + "\\App_Data\\EmailTest2.log"),
            //                  logFileStatus = (MapPath(string.Empty) + "\\App_Data\\EmailTestStatus.log"),
            //                  subject = TextBoxSubject.Text,
            //                  body = TextBoxNote.Text,
            //                  toEmailList = new List<string>(),
            //                  fromEmail = TextBoxFrom.Text,
            //                  cache = Cache,
            //                  DictionaryOfPKIDsByEmail = dictionaryOfAllPkiDsByEmail,
            //                  repeatEachEmail = repeatEachEmail
            //              };

            var sbErrors = new StringBuilder();
            //Guid sessionIdentifier = Guid.NewGuid();
            DateTime sentDateTime = DateTime.Now.Subtract(new TimeSpan(0, 3, 0, 0));
            foreach (ListItem li in CheckBoxListEmail.Items)
            {
                if (li.Selected)
                {

                    string emailAddress = li.Value;
                    int attendeesId = dictionaryOfAllIdsByEmail.ContainsKey(emailAddress)
                                          ? dictionaryOfAllIdsByEmail[emailAddress]
                                          : -1;

                    if (attendeesId > 0)
                    {
                        string bodyText = TextBoxNote.Text;

                        //// check for keywords
                        //var substituteWords = new List<string>()
                        //                                   {
                        //                                       "{Username}",
                        //                                       "{RegisteredPreviousYears}",
                        //                                       "{RegistrationStatusThisYear}",
                        //                                       "{PKID}"
                        //                                   };

                        //bool substituteWordFound = false;
                        //foreach (var search in substituteWords)
                        //{
                        //    if (bodyText.ToLower().Contains(search.ToLower()))
                        //    {
                        //        substituteWordFound = true;
                        //        break;
                        //    }
                        //}

                        //if (substituteWordFound)
                        //{
                        //   bodyText =  Utils.SubstituteWordsInLetter(bodyText, substituteWords,attendeesId);
                        //}


                        // do the insert statement here
                        const string sqlInsert = @"
                            INSERT INTO
                              EmailDetails(
                              EmailFrom,
                              EmailTo,
                              AttendeesId,
                              EmailSendStatus,
                              Subject,
                              BodyText,
                              SentDateTime)
                            VALUES(
                              @EmailFrom,
                              @EmailTo,
                              @AttendeesId,
                              @EmailSendStatus,
                              @Subject,
                              @BodyText,
                              @SentDateTime)
                            ";

                        var sqlCommandInsert = new SqlCommand(sqlInsert, sqlConnection);

                        sqlCommandInsert.Parameters.Add("@EMailFrom", SqlDbType.NVarChar).Value = TextBoxFrom.Text;
                        sqlCommandInsert.Parameters.Add("@EMailTo", SqlDbType.NVarChar).Value = emailAddress;
                        sqlCommandInsert.Parameters.Add("@EmailSendStatus", SqlDbType.NVarChar).Value = "NeedToSend";
                        sqlCommandInsert.Parameters.Add("@Subject", SqlDbType.NVarChar).Value = TextBoxSubject.Text;
                        sqlCommandInsert.Parameters.Add("@BodyText", SqlDbType.NVarChar).Value = bodyText;
                        sqlCommandInsert.Parameters.Add("@SentDateTime", SqlDbType.DateTime).Value = sentDateTime;
                        sqlCommandInsert.Parameters.Add("@AttendeesId", SqlDbType.Int).Value = attendeesId;

                        sqlCommandInsert.ExecuteScalar();
                    }
                }
            }
            sqlConnection.Close();
        }
    }




















    /// <summary>
    /// Take a sql connection plus a sql statement and create a dictionary of PKID's (as strings from email address)
    /// sql string must select first email, second pkid
    /// </summary>
    /// <param name="sqlConnection"></param>
    /// <param name="sql"></param>
    /// <returns></returns>
    private  Dictionary<string, string> GetDictionaryFromSqlAndConnection(SqlConnection sqlConnection, string sql)
    {
        var retDictionary = new Dictionary<string, string>();
        using (var command = new SqlCommand(sql, sqlConnection))
        {
            using (var reader = command.ExecuteReader())
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        string emailaddr = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                        var pkid1 = reader.IsDBNull(1) ? Guid.NewGuid() : reader.GetGuid(1);
                        if (!retDictionary.ContainsKey(emailaddr))
                        {
                            retDictionary.Add(emailaddr, pkid1.ToString());
                        }
                    }
                }
            }
        }
        return retDictionary;
    }

    private Dictionary<string, int> GetDictionaryFromSqlAndConnectionById(SqlConnection sqlConnection, string sql)
    {
        var retDictionary = new Dictionary<string, int>();
        using (var command = new SqlCommand(sql, sqlConnection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string emailaddr = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                    var id = reader.GetInt32(1);
                    if (!retDictionary.ContainsKey(emailaddr))
                    {
                        retDictionary.Add(emailaddr, id);
                    }
                }
            }
        }
        return retDictionary;
    }

    protected void ButtonResetSend_Click(object sender, EventArgs e)
    {
        ButtonProcessSelection.Enabled = true;
    }

    protected void ButtonCancel_Click(object sender, EventArgs e)
    {
        Cache[Utils.CacheMailCancelFlag] = "true";
    }

    protected void ButtonShowStatus_Click(object sender, EventArgs e)
    {
    }

    protected void ButtonPutEmailInTextBox_Click(object sender, EventArgs e)
    {
        TextBoxEmails.Visible = true;
        var sb = new StringBuilder();
        foreach (ListItem li in CheckBoxListEmail.Items)
        {
            if (li.Selected)
            {
                sb.Append(li.Value);
                sb.Append(";");
            }
        }
        TextBoxEmails.Text = sb.ToString();
    }
    protected void CheckBoxListEmail_DataBound(object sender, EventArgs e)
    {

    }
    protected void CheckBoxListEmail_DataBinding(object sender, EventArgs e)
    {

    }

    protected void XMLDownload0_Click(object sender, EventArgs e)
    {
        var attendeesResultsSelected = new List<AttendeesResult>();
        var emailListToSend = CheckBoxListEmail.Items;
        foreach (ListItem rec in emailListToSend)
        {
            if (rec.Selected)
            {
                if (_dictionaryAllAttendeeResultByEmail.ContainsKey(rec.Value))
                {
                    attendeesResultsSelected.Add(_dictionaryAllAttendeeResultByEmail[rec.Value]);
                }
            }
        }


        //// Create a new workbook.
        //SpreadsheetGear.IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook();
        //SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets["Sheet1"];
        //SpreadsheetGear.IRange cells = worksheet.Cells;
        //// Set the worksheet name.
        //worksheet.Name = "SiliconValleyCodeCamp " + Utils.GetCurrentCodeCampYear();

        //cells[0, 0].Value = "Id";
        //cells[0, 1].Value = "FirstName";
        //cells[0, 2].Value = "LastName";
        //cells[0, 3].Value = "Website";
        //cells[0, 4].Value = "AddressLine1";
        //cells[0, 5].Value = "City";
        //cells[0, 6].Value = "State";
        //cells[0, 7].Value = "Zipcode";
        //cells[0, 8].Value = "Email";
        //cells[0, 9].Value = "PhoneNumber";

        var dict = new Dictionary<string, ZIPCODEWORLDGOLDResult>();
        var recs1 = ZIPCODEWORLDGOLDManager.I.GetAll();
        foreach (var rec in recs1)
        {
            if (!dict.ContainsKey(rec.ZIP_CODE))
            {
                dict.Add(rec.ZIP_CODE, rec);
            }
        }

        //var dict = (from data in recs1 select new {data.ZIP_CODE, data}).ToDictionary(k => k.ZIP_CODE, v => v.data);




        Regex replacer = new Regex(@"[^A-Za-z0-9]");

        //Regex replacerPhone = new Regex(@"[\(\)\-\.\s]");

        Regex replacerPhone = new Regex(@"[^A-Za-z0-9\-]");




        int row = 1;
        var codeCampAttendeesBarCodeInfos = new List<CodeCampAttendeesBarCodeInfo>();
        foreach (var rec in attendeesResultsSelected)
        {
            if (String.IsNullOrEmpty(rec.UserFirstName)) rec.UserFirstName = "";
            if (String.IsNullOrEmpty(rec.UserLastName)) rec.UserLastName = "";
            if (String.IsNullOrEmpty(rec.UserWebsite)) rec.UserWebsite = "";
            if (String.IsNullOrEmpty(rec.UserZipCode)) rec.UserZipCode = "";
            if (String.IsNullOrEmpty(rec.City)) rec.City = "";
            if (String.IsNullOrEmpty(rec.State)) rec.State = "";
            if (String.IsNullOrEmpty(rec.Email)) rec.Email = "";
            if (String.IsNullOrEmpty(rec.PhoneNumber)) rec.PhoneNumber = "";

            if (rec.QRAddressLine1Allow == null) rec.QRAddressLine1Allow = false;
            if (rec.QREmailAllow == null) rec.QREmailAllow = false;
            if (rec.QRPhoneAllow == null) rec.QRPhoneAllow = false;
            if (rec.QRWebSiteAllow == null) rec.QRWebSiteAllow = false;
            if (rec.QRZipCodeAllow == null) rec.QRZipCodeAllow = false;
           

            string city = "";
            string state = "";
            if (dict.ContainsKey(rec.UserZipCode))
            {
                city = dict[rec.UserZipCode].CITY;
                state = dict[rec.UserZipCode].STATE;
            }
            codeCampAttendeesBarCodeInfos.Add(new CodeCampAttendeesBarCodeInfo()
                                                  {
                                                      Id = rec.Id,
                                                      FirstName =
                                                          replacer.Replace(rec.UserFirstName ?? string.Empty, " "),
                                                      LastName = replacer.Replace(rec.UserLastName ?? string.Empty, " "),
                                                      WebSiteUrl =
                                                          rec.QRWebSiteAllow != null && rec.QRWebSiteAllow.Value
                                                              ? rec.UserWebsite
                                                              : string.Empty,
                                                      AddressLine1 = rec.QRAddressLine1Allow.Value
                                                                         ? replacer.Replace(
                                                                             rec.AddressLine1 ?? string.Empty, " ")
                                                                         : string.Empty,
                                                      City = rec.QRZipCodeAllow.Value ? city : string.Empty,
                                                      State = rec.QRZipCodeAllow.Value ? state : string.Empty,
                                                      Zipcode = rec.QRZipCodeAllow.Value
                                                                    ? replacer.Replace(rec.UserZipCode ?? string.Empty,
                                                                                       " ")
                                                                    : string.Empty,
                                                      Email = rec.QREmailAllow.Value ? rec.Email : string.Empty,
                                                      PhoneNumber = rec.QRPhoneAllow.Value
                                                                        ? replacerPhone.Replace(
                                                                            rec.PhoneNumber ?? string.Empty, " ")
                                                                        : string.Empty
                                                  });
            row++;
        }

        string xmlString = Utils.SerializeToString(codeCampAttendeesBarCodeInfos);

        Response.Clear();
        Response.ContentType = "text/xml";

        const string headerStringTemplate = "attachment; filename={0}.xml";
        string dateTimeNow = String.Format("{0:yyMMddhhmmss}", DateTime.Now);

        Response.AddHeader("Content-Disposition", String.Format(headerStringTemplate, dateTimeNow));

        Response.Write(xmlString);

        Response.End();



    }
    protected void ExcelExcelDownload_Click(object sender, EventArgs e)
    {

        var attendeesResultsSelected = new List<AttendeesResult>();
        var emailListToSend = CheckBoxListEmail.Items;
        foreach (ListItem rec in emailListToSend)
        {
            if (rec.Selected)
            {
                if (_dictionaryAllAttendeeResultByEmail.ContainsKey(rec.Value))
                {
                    attendeesResultsSelected.Add(_dictionaryAllAttendeeResultByEmail[rec.Value]);
                }
            }
        }


        // Create a new workbook.
        SpreadsheetGear.IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook();
        SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets["Sheet1"];
        SpreadsheetGear.IRange cells = worksheet.Cells;
        // Set the worksheet name.
        worksheet.Name = "SiliconValleyCodeCamp " + Utils.GetCurrentCodeCampYear();

        cells[0, 0].Value = "Id";
        cells[0, 1].Value = "FirstName";
        cells[0, 2].Value = "LastName";
        cells[0, 3].Value = "Website";
        cells[0, 4].Value = "AddressLine1";
        cells[0, 5].Value = "City";
        cells[0, 6].Value = "State";
        cells[0, 7].Value = "Zipcode";
        cells[0, 8].Value = "Email";
        cells[0, 9].Value = "PhoneNumber";

        var dict = new Dictionary<string, ZIPCODEWORLDGOLDResult>();
        var recs1 = ZIPCODEWORLDGOLDManager.I.GetAll();
        foreach (var rec in recs1)
        {
            if (!dict.ContainsKey(rec.ZIP_CODE))
            {
                dict.Add(rec.ZIP_CODE,rec);
            }
        }

        //var dict = (from data in recs1 select new {data.ZIP_CODE, data}).ToDictionary(k => k.ZIP_CODE, v => v.data);



        
        Regex replacer = new Regex(@"[^A-Za-z0-9]");
 
         //Regex replacerPhone = new Regex(@"[\(\)\-\.\s]");

         Regex replacerPhone = new Regex(@"[^A-Za-z0-9\-]");

     


        int row = 1;
        foreach (var rec in attendeesResultsSelected)
        {
            string city = "";
            string state = "";
            if (dict.ContainsKey(rec.UserZipCode))
            {
                city = dict[rec.UserZipCode].CITY;
                state = dict[rec.UserZipCode].STATE;
            }

            //rec.UserFirstName = "First" + row + "First" + row + "First" + row + "First" + row;
            //rec.UserLastName = "Last" + row + " LAST " + row + " LAST " + row + " LAST " + row + " LAST " + row + " LAST " + row + " LAST " + row + " LAST " + row;
            //rec.AddressLine1 = "My Address Here " + row + " My Address Here" + row + " My Address Here" + row + " My Address Here" + row + " My Address Here";
            //rec.Email = "peter" + row + "@peterkellner.net";
            //rec.PhoneNumber = "999-123-" + row;

            cells[row, 0].Value = rec.Id;
            cells[row, 1].Value = replacer.Replace(rec.UserFirstName ?? string.Empty, " ");
            cells[row, 2].Value = replacer.Replace(rec.UserLastName ?? string.Empty, " ");
            cells[row, 3].Value = rec.QRWebSiteAllow != null && rec.QRWebSiteAllow.Value ? rec.UserWebsite : string.Empty;
            cells[row, 4].Value = rec.QRAddressLine1Allow.Value
                                      ? replacer.Replace(rec.AddressLine1 ?? string.Empty, " ")
                                      : string.Empty;
            cells[row, 5].Value = rec.QRZipCodeAllow.Value ? city : string.Empty;
            cells[row, 6].Value = rec.QRZipCodeAllow.Value ? state : string.Empty;
            cells[row, 7].Value = rec.QRZipCodeAllow.Value
                                      ? replacer.Replace(rec.UserZipCode ?? string.Empty, " ")
                                      : string.Empty;
            cells[row, 8].Value = rec.QREmailAllow.Value ? rec.Email : string.Empty;
            cells[row, 9].Value = rec.QRPhoneAllow.Value
                                      ? replacerPhone.Replace(rec.PhoneNumber ?? string.Empty, " ")
                                      : string.Empty;

            // "[\(\)\-\.\s]"
            row++;
        }


        Response.Clear();
        Response.ContentType = "application/vnd.ms-excel";

        const string headerStringTemplate = "attachment; filename={0}_.xls";
        string dateTimeNow = DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute;

        Response.AddHeader("Content-Disposition", String.Format(headerStringTemplate, dateTimeNow));
        workbook.SaveToStream(Response.OutputStream, SpreadsheetGear.FileFormat.Excel8);

        Response.End();

       
    }
    protected void CheckBoxIncludeNoMailAndBouncing_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void ButtonCustomSearch_Click(object sender, EventArgs e)
    {
        
    }
    protected void TextBoxCustomSqlWhere_TextChanged(object sender, EventArgs e)
    {

    }


    protected void ButtonClearEmailDetailRecordsAll_OnClick(object sender, EventArgs e)
    {
        string conn = ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString;
        Utils.ClearEmailDetailTable(conn);
    }
}

public class ProcessEmailThread
{
    public string body;
    public Cache cache;
    public Dictionary<string, string> DictionaryOfPKIDsByEmail;
    public string fromEmail;
    public string logFile;
    public string logFileStatus;
    public string subject;
    public int emailsPerHour;
    public int repeatEachEmail;
    public List<string> toEmailList;


    public void GenerateMails()
    {
        lock (Utils.MailLocker)
        {
            int delayPerEmailMilliSeconds = 0;
            if (emailsPerHour != 0)
            {
                delayPerEmailMilliSeconds = 3600000 / emailsPerHour;
            }

            DateTime startTime = DateTime.Now;
            var sbLog = new StringBuilder();
            sbLog.AppendLine("----------------START-------------------------------" + DateTime.Now);
            sbLog.AppendLine(String.Format("Email Delay in MilliSeconds {0}, Repeat Each Email: {1}",
                                           delayPerEmailMilliSeconds, repeatEachEmail));
            sbLog.AppendLine("Message Subject: " + subject);
            sbLog.AppendLine("Message From: " + fromEmail);
            sbLog.AppendLine("Message Body: " + body);


            var msg = new EmailMessage(true, false);
            int portNumber =  Convert.ToInt32(ConfigurationManager.AppSettings["EmailMessage.Port"].ToString());
            msg.Port = portNumber;

            if (msg.Server.Equals("smtp.gmail.com"))
            {
                var ssl = new AdvancedIntellect.Ssl.SslSocket();
                msg.LoadSslSocket(ssl);
                msg.Port = 587;
            }

            // using for instead of foreach cause it's easier to track start and end of list
            for (int i = 0; i < toEmailList.Count; i++)
            {
                try
                {
                    var cacheName = (string)cache[Utils.CacheMailCancelFlag];
                    if (!string.IsNullOrEmpty(cacheName))
                    {
                        if (((string)cache[Utils.CacheMailCancelFlag]).Equals("true"))
                        {
                            sbLog.AppendLine("Mail Send Cancelled By User");
                            cache[Utils.CacheMailSentStatusName] = "Mail Send Cancelled By User";
                            break;
                        }
                    }

                    msg.CharSet = "iso-8859-1";
                    msg.Logging = true;
                    msg.LogOverwrite = false;
                    msg.LogPath = logFile;
                    msg.FromAddress = fromEmail;
                    msg.To = toEmailList[i];
                    msg.Subject = subject;
                    string PKIDStr = string.Empty;
                    if (DictionaryOfPKIDsByEmail.ContainsKey(toEmailList[i]))
                    {
                        PKIDStr = DictionaryOfPKIDsByEmail[toEmailList[i]];
                    }

                    msg.Body = !String.IsNullOrEmpty(PKIDStr) ? body.Replace("{PKID}", PKIDStr) : body;
                }
                catch (Exception)
                {
                  // just skip any bad email here
                }

                for (int repeatCnt = 0; repeatCnt < repeatEachEmail;repeatCnt++ )
                {
                    try
                    {
                        if (i == 0 && toEmailList.Count == 1)
                        {
                            msg.Send();
                        }
                        else if (i == 0)
                        {
                            msg.Send(true, false);
                        }
                        else if (i == toEmailList.Count - 1)
                        {
                            msg.Send(false, true);
                        }
                        else
                        {
                            msg.Send(false, false);
                        }
                        string mailStatus = i + "Success Send To: " + msg.To;
                        // should really add with expiration but this is pretty small so not worring about it
                        cache[Utils.CacheMailSentStatusName] = mailStatus;
                        sbLog.AppendLine(mailStatus);
                        Thread.Sleep(delayPerEmailMilliSeconds);
                    }
                    catch (Exception eError)
                    {
                        sbLog.AppendLine(i + ":" + eError);
                    }
                }
            }

            DateTime stopTime = DateTime.Now;
            TimeSpan elapsedTime = stopTime - startTime;
            double milliSeconds = elapsedTime.TotalMilliseconds;

            sbLog.AppendLine();
            sbLog.AppendLine();
            sbLog.AppendLine(milliSeconds + " milliseconds");
            sbLog.AppendLine("----------------DONE-------------------------------" + DateTime.Now);

            var f = new FileInfo(logFileStatus);
            FileStream s = f.Open(FileMode.Append, FileAccess.Write);

            string str = sbLog.ToString();

            var ba = new byte[str.Length];
            ba = Encoding.ASCII.GetBytes(str);

            s.Write(ba, 0, ba.Length);
            s.Close();
        }
        cache[Utils.CacheMailSentStatusName] = "Mail Done";
    }
}