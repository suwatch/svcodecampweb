using System;
using System.Collections.Generic;
using System.Linq;
using CodeCampSV;
using aspNetPOP3;
using ListNanny;




public class MailResult
{

    public MailResult(NDRType messageType, string messageTypeString, string bouncedEmailAddress, string helpMessage, NDR ndrMessage, bool willDelete, int newStatus)
    {
        MessageType = messageType;
        MessageTypeString = messageTypeString;
        BouncedEmailAddress = bouncedEmailAddress;
        HelpMessage = helpMessage;
        NdrMessage = ndrMessage;
        WillDelete = willDelete;
        NewStatus = newStatus;
    }

    public NDRType MessageType { get; set; }
    public string MessageTypeString { get; set; }
    public string BouncedEmailAddress { get; set; }
    public string HelpMessage { get; set; }
    public NDR NdrMessage { get; set; }
    public bool WillDelete { get; set; }
    public int NewStatus { get; set; }
}

public class MailResultGroups
{
    public string NDRTypeString { get; set; }
    public int Counter { get; set; }

}

namespace WebAPI
{
    public partial class ListNanny : System.Web.UI.Page
    {
        private List<MailResult> _mailResults;


        protected void Page_Load(object sender, EventArgs e)
        {
            _mailResults = new List<MailResult>();
        }

        protected void ButtonRun_Click(object sender, EventArgs e)
        {
            var pop = new POP3("mail.siliconvalley-codecamp.com", Utils.GetServiceEmailAddress(), "walnut95");

            // need to move this to appconfig at some point   todo
            //var pop = new POP3("pop.secureserver.net", "svcodecampservice@73rdstreet.net", "walnut95") { Port = 995 };

            //var ssl = new AdvancedIntellect.Ssl.SslSocket();
            //pop.LoadSslSocket(ssl);



            pop.LogInMemory = true;
            pop.Connect();
            int messageCount = pop.MessageCount();

            for (int i = 0; i < messageCount; i++)
            {
                if (i > Convert.ToInt32(TextBoxMaxCnt.Text)) break;
                var messageText = pop.GetMessageAsText(i);
                bool deleteMessage = ProcessMessage(messageText);
                if (deleteMessage)
                {
                    pop.Delete(i);
                }
            }
            pop.CommitDeletes();

            pop.Disconnect();

            var recs = from data in _mailResults
                       where data.WillDelete
                       group data by data.MessageType
                           into g
                           select new MailResultGroups()
                           {
                               Counter = g.Count(),
                               NDRTypeString = ConvertNDRTypeToString(g.Key)
                           };

            GridViewTotals.DataSource = recs.OrderBy(a => a.NDRTypeString).ToList();
            GridViewTotals.DataBind();

            GridViewResults.DataSource = _mailResults.Where(a => a.WillDelete);
            GridViewResults.DataBind();
        }

        private static string ConvertNDRTypeToString(NDRType nDrType)
        {
            return Enum.GetName(typeof(NDRType), nDrType);
        }

        /// <summary>
        /// return true if this is a message that should be deleted
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        bool ProcessMessage(string text)
        {
            var ndrMessage = NDR.Parse(text);

            bool customUnsubscribe = false;

            string subject = "";
            if (ndrMessage.BaseMessage.Subject != null)
            {
                subject = ndrMessage.BaseMessage.Subject.ToString().Trim().ToLower();
            }

            string body = "";
            if (ndrMessage.BaseMessage.MainBody != null)
            {
                body = ndrMessage.BaseMessage.MainBody.Trim().ToLower();
            }

            if (subject.Contains("unsubscribe") || body.StartsWith("unsubscribe"))
            {
                customUnsubscribe = true;
            }


            int newStatus = -1;
            bool deleteMessage = false;
            if (customUnsubscribe
                || ndrMessage.Type == NDRType.HardBounce
                || ndrMessage.Type == NDRType.SoftBounce
                || ndrMessage.Type == NDRType.DnsError
                || ndrMessage.Type == NDRType.ChallengeVerification
                //|| ndrMessage.Type == NDRType.Unsubscribe
                || ndrMessage.Type == NDRType.AutoResponder)
            {
                if (customUnsubscribe)
                {
                    ndrMessage.Type = NDRType.Unsubscribe;
                }

                int emailSubscriptionLevel = -1; // default is delete message but don't do anything to svcc record


                if (ndrMessage.BaseMessage.Subject != null && ndrMessage.BaseMessage.Subject.ToString().ToLower().Contains("unsubscribe"))
                {
                    emailSubscriptionLevel = 1; // person has unsubscribe in subject

                }
                else if (ndrMessage.BaseMessage.MainBody != null && ndrMessage.BaseMessage.MainBody.Contains("temporarily deferred due to user complaints"))
                {
                    // this is yahoo message about deferred bounce so don't mark there record  
                    ndrMessage.Type = NDRType.SoftBounce;
                    ndrMessage.HelpMessage = "Yahoo: temporarily deferred due to user complaints";
                }
                else if (ndrMessage.Type == NDRType.Unsubscribe)
                {
                    emailSubscriptionLevel = 1;
                }
                else if (ndrMessage.Type == NDRType.AutoResponder)
                {
                    emailSubscriptionLevel = -1;  // delete but don't touch persons record
                }
                else
                {
                    emailSubscriptionLevel = 2; // mark record as system disabled persons email
                }

                newStatus = emailSubscriptionLevel;

                if (emailSubscriptionLevel > 0 && CheckBoxUpdateAttendees.Checked)
                {
                    var attendee = AttendeesManager.I.Get(new AttendeesQuery()
                    {
                        Email = ndrMessage.BouncedEmailAddress
                    }).FirstOrDefault();
                    if (attendee != null)
                    {

                        attendee.EmailSubscription = emailSubscriptionLevel;
                        if (ndrMessage.BaseMessage.Subject != null)
                        {
                            attendee.EmailSubscriptionStatus = ndrMessage.HelpMessage +
                                                               ndrMessage.BaseMessage.Subject.ToString().Trim() +
                                                               ".  Message Received On:" +
                                                               ndrMessage.BaseMessage.Date;
                        }
                        else
                        {
                            attendee.EmailSubscriptionStatus = ndrMessage.HelpMessage;
                        }
                        AttendeesManager.I.Update(attendee);
                    }

                }
                deleteMessage = true; // all these should delete
            }

            _mailResults.Add(new MailResult(ndrMessage.Type, ndrMessage.Type.ToString(), ndrMessage.BouncedEmailAddress, ndrMessage.HelpMessage,
                                          ndrMessage, deleteMessage, newStatus));
            // never delete unless checkbox checked.
            return CheckBoxDeleteAfterProcessing.Checked && deleteMessage;
        }
    }
}