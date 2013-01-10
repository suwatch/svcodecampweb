//  Copyright  2006, Peter Kellner, 73rd Street Associates
//  All rights reserved.
//  http://PeterKellner.net
//
//
//  - Neither Peter Kellner, nor the names of its
//  contributors may be used to endorse or promote products
//  derived from this software without specific prior written
//  permission.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
//  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
//  FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
//  COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
//  INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES INCLUDING,
//  BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
//  CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
//  LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
//  ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
//  POSSIBILITY OF SUCH DAMAGE.


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Web.Security;
using System.Web.Configuration;

using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace CodeCampSV
{
    [DataObject(true)]  // This attribute allows the ObjectDataSource wizard to see this class
    public class CodeCampEvalsODS
    {
        string connectionString;
        public CodeCampEvalsODS()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString;
        }
        public class DataObjectCodeCampEvals
        {
            public DataObjectCodeCampEvals() { }
            public DataObjectCodeCampEvals(Guid attendeepkid, int metexpectations, int plantoattendagain, int enjoyedfreefood, int sessionsvariedenough, int enoughsessionsatmylevel, int foothillgoodvenue, int wishtoldmorefriends, int eventwellplanned, int wirelessaccessimportant, int wiredaccessimportant, int likereceivingupdatebyemail, int likereceivingupdatebybyrssfeed, int rathernosponsorandnofreefood, bool attendedvistafaironly, bool attendedvistafairandcc, bool attendedcconly, string bestpartofevent, string whatwouldyouchange, string notsatisfiedwhy, string whatfoothillclassestoadd, bool interesteinlongtermplanning, bool interesteinwebbackend, bool interestedinwebfrontend, bool interesteinlongsessionreviewpanel, bool interesteincontributorsolicitation, bool interesteinbeingcontributor, bool interesteinbeforeevent, bool interesteindayofevent, bool interesteineventteardown, bool interesteinafterevent, string forvolunteeringbestwaytocontactemail, string forvolunteeringbestwaytocontactphone, DateTime datesubmitted, DateTime dateupdated, int id)
            {
                this.attendeepkid = attendeepkid;
                this.metexpectations = metexpectations;
                this.plantoattendagain = plantoattendagain;
                this.enjoyedfreefood = enjoyedfreefood;
                this.sessionsvariedenough = sessionsvariedenough;
                this.enoughsessionsatmylevel = enoughsessionsatmylevel;
                this.foothillgoodvenue = foothillgoodvenue;
                this.wishtoldmorefriends = wishtoldmorefriends;
                this.eventwellplanned = eventwellplanned;
                this.wirelessaccessimportant = wirelessaccessimportant;
                this.wiredaccessimportant = wiredaccessimportant;
                this.likereceivingupdatebyemail = likereceivingupdatebyemail;
                this.likereceivingupdatebybyrssfeed = likereceivingupdatebybyrssfeed;
                this.rathernosponsorandnofreefood = rathernosponsorandnofreefood;
                this.attendedvistafaironly = attendedvistafaironly;
                this.attendedvistafairandcc = attendedvistafairandcc;
                this.attendedcconly = attendedcconly;
                this.bestpartofevent = bestpartofevent;
                this.whatwouldyouchange = whatwouldyouchange;
                this.notsatisfiedwhy = notsatisfiedwhy;
                this.whatfoothillclassestoadd = whatfoothillclassestoadd;
                this.interesteinlongtermplanning = interesteinlongtermplanning;
                this.interesteinwebbackend = interesteinwebbackend;
                this.interestedinwebfrontend = interestedinwebfrontend;
                this.interesteinlongsessionreviewpanel = interesteinlongsessionreviewpanel;
                this.interesteincontributorsolicitation = interesteincontributorsolicitation;
                this.interesteinbeingcontributor = interesteinbeingcontributor;
                this.interesteinbeforeevent = interesteinbeforeevent;
                this.interesteindayofevent = interesteindayofevent;
                this.interesteineventteardown = interesteineventteardown;
                this.interesteinafterevent = interesteinafterevent;
                this.forvolunteeringbestwaytocontactemail = forvolunteeringbestwaytocontactemail;
                this.forvolunteeringbestwaytocontactphone = forvolunteeringbestwaytocontactphone;
                this.datesubmitted = datesubmitted;
                this.dateupdated = dateupdated;
                this.id = id;
            }

            private Guid attendeepkid;
            [DataObjectField(false, false, false)]
            public Guid Attendeepkid
            {
                get { return attendeepkid; }
                set { attendeepkid = value; }
            }

            private int metexpectations;
            [DataObjectField(false, false, true)]
            public int Metexpectations
            {
                get { return metexpectations; }
                set { metexpectations = value; }
            }

            private int plantoattendagain;
            [DataObjectField(false, false, true)]
            public int Plantoattendagain
            {
                get { return plantoattendagain; }
                set { plantoattendagain = value; }
            }

            private int enjoyedfreefood;
            [DataObjectField(false, false, true)]
            public int Enjoyedfreefood
            {
                get { return enjoyedfreefood; }
                set { enjoyedfreefood = value; }
            }

            private int sessionsvariedenough;
            [DataObjectField(false, false, true)]
            public int Sessionsvariedenough
            {
                get { return sessionsvariedenough; }
                set { sessionsvariedenough = value; }
            }

            private int enoughsessionsatmylevel;
            [DataObjectField(false, false, true)]
            public int Enoughsessionsatmylevel
            {
                get { return enoughsessionsatmylevel; }
                set { enoughsessionsatmylevel = value; }
            }

            private int foothillgoodvenue;
            [DataObjectField(false, false, true)]
            public int Foothillgoodvenue
            {
                get { return foothillgoodvenue; }
                set { foothillgoodvenue = value; }
            }

            private int wishtoldmorefriends;
            [DataObjectField(false, false, true)]
            public int Wishtoldmorefriends
            {
                get { return wishtoldmorefriends; }
                set { wishtoldmorefriends = value; }
            }

            private int eventwellplanned;
            [DataObjectField(false, false, true)]
            public int Eventwellplanned
            {
                get { return eventwellplanned; }
                set { eventwellplanned = value; }
            }

            private int wirelessaccessimportant;
            [DataObjectField(false, false, true)]
            public int Wirelessaccessimportant
            {
                get { return wirelessaccessimportant; }
                set { wirelessaccessimportant = value; }
            }

            private int wiredaccessimportant;
            [DataObjectField(false, false, true)]
            public int Wiredaccessimportant
            {
                get { return wiredaccessimportant; }
                set { wiredaccessimportant = value; }
            }

            private int likereceivingupdatebyemail;
            [DataObjectField(false, false, true)]
            public int Likereceivingupdatebyemail
            {
                get { return likereceivingupdatebyemail; }
                set { likereceivingupdatebyemail = value; }
            }

            private int likereceivingupdatebybyrssfeed;
            [DataObjectField(false, false, true)]
            public int Likereceivingupdatebybyrssfeed
            {
                get { return likereceivingupdatebybyrssfeed; }
                set { likereceivingupdatebybyrssfeed = value; }
            }

            private int rathernosponsorandnofreefood;
            [DataObjectField(false, false, true)]
            public int Rathernosponsorandnofreefood
            {
                get { return rathernosponsorandnofreefood; }
                set { rathernosponsorandnofreefood = value; }
            }

            private bool attendedvistafaironly;
            [DataObjectField(false, false, true)]
            public bool Attendedvistafaironly
            {
                get { return attendedvistafaironly; }
                set { attendedvistafaironly = value; }
            }

            private bool attendedvistafairandcc;
            [DataObjectField(false, false, true)]
            public bool Attendedvistafairandcc
            {
                get { return attendedvistafairandcc; }
                set { attendedvistafairandcc = value; }
            }

            private bool attendedcconly;
            [DataObjectField(false, false, true)]
            public bool Attendedcconly
            {
                get { return attendedcconly; }
                set { attendedcconly = value; }
            }

            private string bestpartofevent;
            [DataObjectField(false, false, true)]
            public string Bestpartofevent
            {
                get { return bestpartofevent; }
                set { bestpartofevent = value; }
            }

            private string whatwouldyouchange;
            [DataObjectField(false, false, true)]
            public string Whatwouldyouchange
            {
                get { return whatwouldyouchange; }
                set { whatwouldyouchange = value; }
            }

            private string notsatisfiedwhy;
            [DataObjectField(false, false, true)]
            public string Notsatisfiedwhy
            {
                get { return notsatisfiedwhy; }
                set { notsatisfiedwhy = value; }
            }

            private string whatfoothillclassestoadd;
            [DataObjectField(false, false, true)]
            public string Whatfoothillclassestoadd
            {
                get { return whatfoothillclassestoadd; }
                set { whatfoothillclassestoadd = value; }
            }

            private bool interesteinlongtermplanning;
            [DataObjectField(false, false, true)]
            public bool Interesteinlongtermplanning
            {
                get { return interesteinlongtermplanning; }
                set { interesteinlongtermplanning = value; }
            }

            private bool interesteinwebbackend;
            [DataObjectField(false, false, true)]
            public bool Interesteinwebbackend
            {
                get { return interesteinwebbackend; }
                set { interesteinwebbackend = value; }
            }

            private bool interestedinwebfrontend;
            [DataObjectField(false, false, true)]
            public bool Interestedinwebfrontend
            {
                get { return interestedinwebfrontend; }
                set { interestedinwebfrontend = value; }
            }

            private bool interesteinlongsessionreviewpanel;
            [DataObjectField(false, false, true)]
            public bool Interesteinlongsessionreviewpanel
            {
                get { return interesteinlongsessionreviewpanel; }
                set { interesteinlongsessionreviewpanel = value; }
            }

            private bool interesteincontributorsolicitation;
            [DataObjectField(false, false, true)]
            public bool Interesteincontributorsolicitation
            {
                get { return interesteincontributorsolicitation; }
                set { interesteincontributorsolicitation = value; }
            }

            private bool interesteinbeingcontributor;
            [DataObjectField(false, false, true)]
            public bool Interesteinbeingcontributor
            {
                get { return interesteinbeingcontributor; }
                set { interesteinbeingcontributor = value; }
            }

            private bool interesteinbeforeevent;
            [DataObjectField(false, false, true)]
            public bool Interesteinbeforeevent
            {
                get { return interesteinbeforeevent; }
                set { interesteinbeforeevent = value; }
            }

            private bool interesteindayofevent;
            [DataObjectField(false, false, true)]
            public bool Interesteindayofevent
            {
                get { return interesteindayofevent; }
                set { interesteindayofevent = value; }
            }

            private bool interesteineventteardown;
            [DataObjectField(false, false, true)]
            public bool Interesteineventteardown
            {
                get { return interesteineventteardown; }
                set { interesteineventteardown = value; }
            }

            private bool interesteinafterevent;
            [DataObjectField(false, false, true)]
            public bool Interesteinafterevent
            {
                get { return interesteinafterevent; }
                set { interesteinafterevent = value; }
            }

            private string forvolunteeringbestwaytocontactemail;
            [DataObjectField(false, false, true)]
            public string Forvolunteeringbestwaytocontactemail
            {
                get { return forvolunteeringbestwaytocontactemail; }
                set { forvolunteeringbestwaytocontactemail = value; }
            }

            private string forvolunteeringbestwaytocontactphone;
            [DataObjectField(false, false, true)]
            public string Forvolunteeringbestwaytocontactphone
            {
                get { return forvolunteeringbestwaytocontactphone; }
                set { forvolunteeringbestwaytocontactphone = value; }
            }

            private DateTime datesubmitted;
            [DataObjectField(false, false, true)]
            public DateTime Datesubmitted
            {
                get { return datesubmitted; }
                set { datesubmitted = value; }
            }

            private DateTime dateupdated;
            [DataObjectField(false, false, true)]
            public DateTime Dateupdated
            {
                get { return dateupdated; }
                set { dateupdated = value; }
            }

            private int id;
            [DataObjectField(true, true, false)]
            public int Id
            {
                get { return id; }
                set { id = value; }
            }


        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectCodeCampEvals> GetAll(string sortData)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            List<DataObjectCodeCampEvals> DataTemplateODSList = new List<DataObjectCodeCampEvals>();
            SqlDataReader reader = null;
            string sqlSelectString = "SELECT AttendeePKID,MetExpectations,PlanToAttendAgain,EnjoyedFreeFood,SessionsVariedEnough,EnoughSessionsAtMyLevel,FoothillGoodVenue,WishToldMoreFriends,EventWellPlanned,WirelessAccessImportant,WiredAccessImportant,LikeReceivingUpdateByEmail,LikeReceivingUpdateByByRSSFeed,RatherNoSponsorAndNoFreeFood,AttendedVistaFairOnly,AttendedVistaFairAndCC,AttendedCCOnly,BestPartOfEvent,WhatWouldYouChange,NotSatisfiedWhy,WhatFoothillClassesToAdd,InteresteInLongTermPlanning,InteresteInWebBackEnd,InterestedInWebFrontEnd,InteresteInLongSessionReviewPanel,InteresteInContributorSolicitation,InteresteInBeingContributor,InteresteInBeforeEvent,InteresteInDayOfEvent,InteresteInEventTearDown,InteresteInAfterEvent,ForVolunteeringBestWayToContactEmail,ForVolunteeringBestWayToContactPhone,DateSubmitted,DateUpdated,id FROM [dbo].[CodeCampEvals] ";
            SqlCommand cmd = new SqlCommand(sqlSelectString, conn);
            reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Guid attendeepkid = reader.IsDBNull(0) ? Guid.NewGuid() : reader.GetGuid(0);
                    int metexpectations = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                    int plantoattendagain = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                    int enjoyedfreefood = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                    int sessionsvariedenough = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                    int enoughsessionsatmylevel = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                    int foothillgoodvenue = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                    int wishtoldmorefriends = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);
                    int eventwellplanned = reader.IsDBNull(8) ? 0 : reader.GetInt32(8);
                    int wirelessaccessimportant = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                    int wiredaccessimportant = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                    int likereceivingupdatebyemail = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                    int likereceivingupdatebybyrssfeed = reader.IsDBNull(12) ? 0 : reader.GetInt32(12);
                    int rathernosponsorandnofreefood = reader.IsDBNull(13) ? 0 : reader.GetInt32(13);
                    bool attendedvistafaironly = reader.IsDBNull(14) ? false : reader.GetBoolean(14);
                    bool attendedvistafairandcc = reader.IsDBNull(15) ? false : reader.GetBoolean(15);
                    bool attendedcconly = reader.IsDBNull(16) ? false : reader.GetBoolean(16);
                    string bestpartofevent = reader.IsDBNull(17) ? "" : reader.GetString(17);
                    string whatwouldyouchange = reader.IsDBNull(18) ? "" : reader.GetString(18);
                    string notsatisfiedwhy = reader.IsDBNull(19) ? "" : reader.GetString(19);
                    string whatfoothillclassestoadd = reader.IsDBNull(20) ? "" : reader.GetString(20);
                    bool interesteinlongtermplanning = reader.IsDBNull(21) ? false : reader.GetBoolean(21);
                    bool interesteinwebbackend = reader.IsDBNull(22) ? false : reader.GetBoolean(22);
                    bool interestedinwebfrontend = reader.IsDBNull(23) ? false : reader.GetBoolean(23);
                    bool interesteinlongsessionreviewpanel = reader.IsDBNull(24) ? false : reader.GetBoolean(24);
                    bool interesteincontributorsolicitation = reader.IsDBNull(25) ? false : reader.GetBoolean(25);
                    bool interesteinbeingcontributor = reader.IsDBNull(26) ? false : reader.GetBoolean(26);
                    bool interesteinbeforeevent = reader.IsDBNull(27) ? false : reader.GetBoolean(27);
                    bool interesteindayofevent = reader.IsDBNull(28) ? false : reader.GetBoolean(28);
                    bool interesteineventteardown = reader.IsDBNull(29) ? false : reader.GetBoolean(29);
                    bool interesteinafterevent = reader.IsDBNull(30) ? false : reader.GetBoolean(30);
                    string forvolunteeringbestwaytocontactemail = reader.IsDBNull(31) ? "" : reader.GetString(31);
                    string forvolunteeringbestwaytocontactphone = reader.IsDBNull(32) ? "" : reader.GetString(32);
                    DateTime datesubmitted = reader.IsDBNull(33) ? DateTime.Now : reader.GetDateTime(33);
                    DateTime dateupdated = reader.IsDBNull(34) ? DateTime.Now : reader.GetDateTime(34);
                    int id = reader.IsDBNull(35) ? 0 : reader.GetInt32(35);
                    DataObjectCodeCampEvals td = new DataObjectCodeCampEvals(attendeepkid, metexpectations, plantoattendagain, enjoyedfreefood, sessionsvariedenough, enoughsessionsatmylevel, foothillgoodvenue, wishtoldmorefriends, eventwellplanned, wirelessaccessimportant, wiredaccessimportant, likereceivingupdatebyemail, likereceivingupdatebybyrssfeed, rathernosponsorandnofreefood, attendedvistafaironly, attendedvistafairandcc, attendedcconly, bestpartofevent, whatwouldyouchange, notsatisfiedwhy, whatfoothillclassestoadd, interesteinlongtermplanning, interesteinwebbackend, interestedinwebfrontend, interesteinlongsessionreviewpanel, interesteincontributorsolicitation, interesteinbeingcontributor, interesteinbeforeevent, interesteindayofevent, interesteineventteardown, interesteinafterevent, forvolunteeringbestwaytocontactemail, forvolunteeringbestwaytocontactphone, datesubmitted, dateupdated, id);
                    DataTemplateODSList.Add(td);
                }
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            conn.Close();

            if (sortData == null)
            {
                sortData = "Id";
            }
            if (sortData.Length == 0)
            {
                sortData = "Id";
            }
            string sortDataBase = sortData;
            string descString = " DESC";
            if (sortData.EndsWith(descString))
            {
                sortDataBase = sortData.Substring(0, sortData.Length - descString.Length);
            }
            Comparison<DataObjectCodeCampEvals> comparison = null;
            switch (sortDataBase)
            {
                case "Metexpectations":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Metexpectations.CompareTo(rhs.Metexpectations);
                       }
                     );
                    break;
                case "Plantoattendagain":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Plantoattendagain.CompareTo(rhs.Plantoattendagain);
                       }
                     );
                    break;
                case "Enjoyedfreefood":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Enjoyedfreefood.CompareTo(rhs.Enjoyedfreefood);
                       }
                     );
                    break;
                case "Sessionsvariedenough":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Sessionsvariedenough.CompareTo(rhs.Sessionsvariedenough);
                       }
                     );
                    break;
                case "Enoughsessionsatmylevel":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Enoughsessionsatmylevel.CompareTo(rhs.Enoughsessionsatmylevel);
                       }
                     );
                    break;
                case "Foothillgoodvenue":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Foothillgoodvenue.CompareTo(rhs.Foothillgoodvenue);
                       }
                     );
                    break;
                case "Wishtoldmorefriends":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Wishtoldmorefriends.CompareTo(rhs.Wishtoldmorefriends);
                       }
                     );
                    break;
                case "Eventwellplanned":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Eventwellplanned.CompareTo(rhs.Eventwellplanned);
                       }
                     );
                    break;
                case "Wirelessaccessimportant":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Wirelessaccessimportant.CompareTo(rhs.Wirelessaccessimportant);
                       }
                     );
                    break;
                case "Wiredaccessimportant":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Wiredaccessimportant.CompareTo(rhs.Wiredaccessimportant);
                       }
                     );
                    break;
                case "Likereceivingupdatebyemail":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Likereceivingupdatebyemail.CompareTo(rhs.Likereceivingupdatebyemail);
                       }
                     );
                    break;
                case "Likereceivingupdatebybyrssfeed":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Likereceivingupdatebybyrssfeed.CompareTo(rhs.Likereceivingupdatebybyrssfeed);
                       }
                     );
                    break;
                case "Rathernosponsorandnofreefood":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Rathernosponsorandnofreefood.CompareTo(rhs.Rathernosponsorandnofreefood);
                       }
                     );
                    break;
                case "Attendedvistafaironly":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Attendedvistafaironly.CompareTo(rhs.Attendedvistafaironly);
                       }
                     );
                    break;
                case "Attendedvistafairandcc":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Attendedvistafairandcc.CompareTo(rhs.Attendedvistafairandcc);
                       }
                     );
                    break;
                case "Attendedcconly":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Attendedcconly.CompareTo(rhs.Attendedcconly);
                       }
                     );
                    break;
                case "Bestpartofevent":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Bestpartofevent.CompareTo(rhs.Bestpartofevent);
                       }
                     );
                    break;
                case "Whatwouldyouchange":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Whatwouldyouchange.CompareTo(rhs.Whatwouldyouchange);
                       }
                     );
                    break;
                case "Notsatisfiedwhy":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Notsatisfiedwhy.CompareTo(rhs.Notsatisfiedwhy);
                       }
                     );
                    break;
                case "Whatfoothillclassestoadd":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Whatfoothillclassestoadd.CompareTo(rhs.Whatfoothillclassestoadd);
                       }
                     );
                    break;
                case "Interesteinlongtermplanning":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Interesteinlongtermplanning.CompareTo(rhs.Interesteinlongtermplanning);
                       }
                     );
                    break;
                case "Interesteinwebbackend":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Interesteinwebbackend.CompareTo(rhs.Interesteinwebbackend);
                       }
                     );
                    break;
                case "Interestedinwebfrontend":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Interestedinwebfrontend.CompareTo(rhs.Interestedinwebfrontend);
                       }
                     );
                    break;
                case "Interesteinlongsessionreviewpanel":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Interesteinlongsessionreviewpanel.CompareTo(rhs.Interesteinlongsessionreviewpanel);
                       }
                     );
                    break;
                case "Interesteincontributorsolicitation":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Interesteincontributorsolicitation.CompareTo(rhs.Interesteincontributorsolicitation);
                       }
                     );
                    break;
                case "Interesteinbeingcontributor":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Interesteinbeingcontributor.CompareTo(rhs.Interesteinbeingcontributor);
                       }
                     );
                    break;
                case "Interesteinbeforeevent":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Interesteinbeforeevent.CompareTo(rhs.Interesteinbeforeevent);
                       }
                     );
                    break;
                case "Interesteindayofevent":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Interesteindayofevent.CompareTo(rhs.Interesteindayofevent);
                       }
                     );
                    break;
                case "Interesteineventteardown":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Interesteineventteardown.CompareTo(rhs.Interesteineventteardown);
                       }
                     );
                    break;
                case "Interesteinafterevent":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Interesteinafterevent.CompareTo(rhs.Interesteinafterevent);
                       }
                     );
                    break;
                case "Forvolunteeringbestwaytocontactemail":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Forvolunteeringbestwaytocontactemail.CompareTo(rhs.Forvolunteeringbestwaytocontactemail);
                       }
                     );
                    break;
                case "Forvolunteeringbestwaytocontactphone":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Forvolunteeringbestwaytocontactphone.CompareTo(rhs.Forvolunteeringbestwaytocontactphone);
                       }
                     );
                    break;
                case "Datesubmitted":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Datesubmitted.CompareTo(rhs.Datesubmitted);
                       }
                     );
                    break;
                case "Dateupdated":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Dateupdated.CompareTo(rhs.Dateupdated);
                       }
                     );
                    break;
                case "Id":
                    comparison = new Comparison<DataObjectCodeCampEvals>(
                       delegate(DataObjectCodeCampEvals lhs, DataObjectCodeCampEvals rhs)
                       {
                           return lhs.Id.CompareTo(rhs.Id);
                       }
                     );
                    break;
            }
            if (comparison != null)
            {
                DataTemplateODSList.Sort(comparison);
                if (sortData.ToLower().EndsWith("desc"))
                {
                    DataTemplateODSList.Reverse();
                }
            }
            return DataTemplateODSList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectCodeCampEvals> GetByPKID(Guid searchattendeepkid)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            List<DataObjectCodeCampEvals> DataTemplateODSList = new List<DataObjectCodeCampEvals>();
            SqlDataReader reader = null;
            string sqlSelectString = "SELECT AttendeePKID,MetExpectations,PlanToAttendAgain,EnjoyedFreeFood,SessionsVariedEnough,EnoughSessionsAtMyLevel,FoothillGoodVenue,WishToldMoreFriends,EventWellPlanned,WirelessAccessImportant,WiredAccessImportant,LikeReceivingUpdateByEmail,LikeReceivingUpdateByByRSSFeed,RatherNoSponsorAndNoFreeFood,AttendedVistaFairOnly,AttendedVistaFairAndCC,AttendedCCOnly,BestPartOfEvent,WhatWouldYouChange,NotSatisfiedWhy,WhatFoothillClassesToAdd,InteresteInLongTermPlanning,InteresteInWebBackEnd,InterestedInWebFrontEnd,InteresteInLongSessionReviewPanel,InteresteInContributorSolicitation,InteresteInBeingContributor,InteresteInBeforeEvent,InteresteInDayOfEvent,InteresteInEventTearDown,InteresteInAfterEvent,ForVolunteeringBestWayToContactEmail,ForVolunteeringBestWayToContactPhone,DateSubmitted,DateUpdated,id FROM [dbo].[CodeCampEvals] WHERE attendeePKID = @searchattendeepkid ";
            SqlCommand cmd = new SqlCommand(sqlSelectString, conn);
            cmd.Parameters.Add("@searchattendeepkid", SqlDbType.UniqueIdentifier).Value = searchattendeepkid; ;
            reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Guid attendeepkid = reader.IsDBNull(0) ? Guid.NewGuid() : reader.GetGuid(0);
                    int metexpectations = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                    int plantoattendagain = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                    int enjoyedfreefood = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                    int sessionsvariedenough = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                    int enoughsessionsatmylevel = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                    int foothillgoodvenue = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                    int wishtoldmorefriends = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);
                    int eventwellplanned = reader.IsDBNull(8) ? 0 : reader.GetInt32(8);
                    int wirelessaccessimportant = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                    int wiredaccessimportant = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                    int likereceivingupdatebyemail = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                    int likereceivingupdatebybyrssfeed = reader.IsDBNull(12) ? 0 : reader.GetInt32(12);
                    int rathernosponsorandnofreefood = reader.IsDBNull(13) ? 0 : reader.GetInt32(13);
                    bool attendedvistafaironly = reader.IsDBNull(14) ? false : reader.GetBoolean(14);
                    bool attendedvistafairandcc = reader.IsDBNull(15) ? false : reader.GetBoolean(15);
                    bool attendedcconly = reader.IsDBNull(16) ? false : reader.GetBoolean(16);
                    string bestpartofevent = reader.IsDBNull(17) ? "" : reader.GetString(17);
                    string whatwouldyouchange = reader.IsDBNull(18) ? "" : reader.GetString(18);
                    string notsatisfiedwhy = reader.IsDBNull(19) ? "" : reader.GetString(19);
                    string whatfoothillclassestoadd = reader.IsDBNull(20) ? "" : reader.GetString(20);
                    bool interesteinlongtermplanning = reader.IsDBNull(21) ? false : reader.GetBoolean(21);
                    bool interesteinwebbackend = reader.IsDBNull(22) ? false : reader.GetBoolean(22);
                    bool interestedinwebfrontend = reader.IsDBNull(23) ? false : reader.GetBoolean(23);
                    bool interesteinlongsessionreviewpanel = reader.IsDBNull(24) ? false : reader.GetBoolean(24);
                    bool interesteincontributorsolicitation = reader.IsDBNull(25) ? false : reader.GetBoolean(25);
                    bool interesteinbeingcontributor = reader.IsDBNull(26) ? false : reader.GetBoolean(26);
                    bool interesteinbeforeevent = reader.IsDBNull(27) ? false : reader.GetBoolean(27);
                    bool interesteindayofevent = reader.IsDBNull(28) ? false : reader.GetBoolean(28);
                    bool interesteineventteardown = reader.IsDBNull(29) ? false : reader.GetBoolean(29);
                    bool interesteinafterevent = reader.IsDBNull(30) ? false : reader.GetBoolean(30);
                    string forvolunteeringbestwaytocontactemail = reader.IsDBNull(31) ? "" : reader.GetString(31);
                    string forvolunteeringbestwaytocontactphone = reader.IsDBNull(32) ? "" : reader.GetString(32);
                    DateTime datesubmitted = reader.IsDBNull(33) ? DateTime.Now : reader.GetDateTime(33);
                    DateTime dateupdated = reader.IsDBNull(34) ? DateTime.Now : reader.GetDateTime(34);
                    int id = reader.IsDBNull(35) ? 0 : reader.GetInt32(35);
                    DataObjectCodeCampEvals td = new DataObjectCodeCampEvals(attendeepkid, metexpectations, plantoattendagain, enjoyedfreefood, sessionsvariedenough, enoughsessionsatmylevel, foothillgoodvenue, wishtoldmorefriends, eventwellplanned, wirelessaccessimportant, wiredaccessimportant, likereceivingupdatebyemail, likereceivingupdatebybyrssfeed, rathernosponsorandnofreefood, attendedvistafaironly, attendedvistafairandcc, attendedcconly, bestpartofevent, whatwouldyouchange, notsatisfiedwhy, whatfoothillclassestoadd, interesteinlongtermplanning, interesteinwebbackend, interestedinwebfrontend, interesteinlongsessionreviewpanel, interesteincontributorsolicitation, interesteinbeingcontributor, interesteinbeforeevent, interesteindayofevent, interesteineventteardown, interesteinafterevent, forvolunteeringbestwaytocontactemail, forvolunteeringbestwaytocontactphone, datesubmitted, dateupdated, id);
                    DataTemplateODSList.Add(td);
                }
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            conn.Close();

            return DataTemplateODSList;
        }


        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void UpdateAllCodeCampEvals(Guid PKID,int metexpectations, int plantoattendagain, int enjoyedfreefood, int sessionsvariedenough, int enoughsessionsatmylevel, int foothillgoodvenue, int wishtoldmorefriends, int eventwellplanned, int wirelessaccessimportant, int wiredaccessimportant, int likereceivingupdatebyemail, int likereceivingupdatebybyrssfeed, int rathernosponsorandnofreefood, bool attendedvistafaironly, bool attendedvistafairandcc, bool attendedcconly, string bestpartofevent, string whatwouldyouchange, string notsatisfiedwhy, string whatfoothillclassestoadd, bool interesteinlongtermplanning, bool interesteinwebbackend, bool interestedinwebfrontend, bool interesteinlongsessionreviewpanel, bool interesteincontributorsolicitation, bool interesteinbeingcontributor, bool interesteinbeforeevent, bool interesteindayofevent, bool interesteineventteardown, bool interesteinafterevent, string forvolunteeringbestwaytocontactemail, string forvolunteeringbestwaytocontactphone,  DateTime dateupdated)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string updateString = "UPDATE [dbo].[CodeCampEvals] SET CodeCampYearId = @CodeCampYearId, MetExpectations = @metexpectations,PlanToAttendAgain = @plantoattendagain,EnjoyedFreeFood = @enjoyedfreefood,SessionsVariedEnough = @sessionsvariedenough,EnoughSessionsAtMyLevel = @enoughsessionsatmylevel,FoothillGoodVenue = @foothillgoodvenue,WishToldMoreFriends = @wishtoldmorefriends,EventWellPlanned = @eventwellplanned,WirelessAccessImportant = @wirelessaccessimportant,WiredAccessImportant = @wiredaccessimportant,LikeReceivingUpdateByEmail = @likereceivingupdatebyemail,LikeReceivingUpdateByByRSSFeed = @likereceivingupdatebybyrssfeed,RatherNoSponsorAndNoFreeFood = @rathernosponsorandnofreefood,AttendedVistaFairOnly = @attendedvistafaironly,AttendedVistaFairAndCC = @attendedvistafairandcc,AttendedCCOnly = @attendedcconly,BestPartOfEvent = @bestpartofevent,WhatWouldYouChange = @whatwouldyouchange,NotSatisfiedWhy = @notsatisfiedwhy,WhatFoothillClassesToAdd = @whatfoothillclassestoadd,InteresteInLongTermPlanning = @interesteinlongtermplanning,InteresteInWebBackEnd = @interesteinwebbackend,InterestedInWebFrontEnd = @interestedinwebfrontend,InteresteInLongSessionReviewPanel = @interesteinlongsessionreviewpanel,InteresteInContributorSolicitation = @interesteincontributorsolicitation,InteresteInBeingContributor = @interesteinbeingcontributor,InteresteInBeforeEvent = @interesteinbeforeevent,InteresteInDayOfEvent = @interesteindayofevent,InteresteInEventTearDown = @interesteineventteardown,InteresteInAfterEvent = @interesteinafterevent,ForVolunteeringBestWayToContactEmail = @forvolunteeringbestwaytocontactemail,ForVolunteeringBestWayToContactPhone = @forvolunteeringbestwaytocontactphone,DateUpdated = @dateupdated WHERE attendeePKID = @attendeePKID";
            SqlCommand cmd = new SqlCommand(updateString, connection);
            cmd.Parameters.Add("@CodeCampYearId", SqlDbType.Int, 4).Value = Utils.GetCurrentCodeCampYear();
            cmd.Parameters.Add("@attendeePKID", SqlDbType.UniqueIdentifier).Value = PKID;
            cmd.Parameters.Add("@metexpectations", SqlDbType.Int, 4).Value = metexpectations;
            cmd.Parameters.Add("@plantoattendagain", SqlDbType.Int, 4).Value = plantoattendagain;
            cmd.Parameters.Add("@enjoyedfreefood", SqlDbType.Int, 4).Value = enjoyedfreefood;
            cmd.Parameters.Add("@sessionsvariedenough", SqlDbType.Int, 4).Value = sessionsvariedenough;
            cmd.Parameters.Add("@enoughsessionsatmylevel", SqlDbType.Int, 4).Value = enoughsessionsatmylevel;
            cmd.Parameters.Add("@foothillgoodvenue", SqlDbType.Int, 4).Value = foothillgoodvenue;
            cmd.Parameters.Add("@wishtoldmorefriends", SqlDbType.Int, 4).Value = wishtoldmorefriends;
            cmd.Parameters.Add("@eventwellplanned", SqlDbType.Int, 4).Value = eventwellplanned;
            cmd.Parameters.Add("@wirelessaccessimportant", SqlDbType.Int, 4).Value = wirelessaccessimportant;
            cmd.Parameters.Add("@wiredaccessimportant", SqlDbType.Int, 4).Value = wiredaccessimportant;
            cmd.Parameters.Add("@likereceivingupdatebyemail", SqlDbType.Int, 4).Value = likereceivingupdatebyemail;
            cmd.Parameters.Add("@likereceivingupdatebybyrssfeed", SqlDbType.Int, 4).Value = likereceivingupdatebybyrssfeed;
            cmd.Parameters.Add("@rathernosponsorandnofreefood", SqlDbType.Int).Value = rathernosponsorandnofreefood;
            cmd.Parameters.Add("@attendedvistafaironly", SqlDbType.Bit, 1).Value = attendedvistafaironly;
            cmd.Parameters.Add("@attendedvistafairandcc", SqlDbType.Bit, 1).Value = attendedvistafairandcc;
            cmd.Parameters.Add("@attendedcconly", SqlDbType.Bit, 1).Value = attendedcconly;
            cmd.Parameters.Add("@bestpartofevent", SqlDbType.VarChar, 2147483647).Value = bestpartofevent == null ? String.Empty : bestpartofevent;
            cmd.Parameters.Add("@whatwouldyouchange", SqlDbType.VarChar, 2147483647).Value = whatwouldyouchange == null ? String.Empty : whatwouldyouchange;
            cmd.Parameters.Add("@notsatisfiedwhy", SqlDbType.VarChar, 2147483647).Value = notsatisfiedwhy == null ? String.Empty : notsatisfiedwhy;
            cmd.Parameters.Add("@whatfoothillclassestoadd", SqlDbType.VarChar, 2147483647).Value = whatfoothillclassestoadd == null ? String.Empty : whatfoothillclassestoadd;
            cmd.Parameters.Add("@interesteinlongtermplanning", SqlDbType.Bit, 1).Value = interesteinlongtermplanning;
            cmd.Parameters.Add("@interesteinwebbackend", SqlDbType.Bit, 1).Value = interesteinwebbackend;
            cmd.Parameters.Add("@interestedinwebfrontend", SqlDbType.Bit, 1).Value = interestedinwebfrontend;
            cmd.Parameters.Add("@interesteinlongsessionreviewpanel", SqlDbType.Bit, 1).Value = interesteinlongsessionreviewpanel;
            cmd.Parameters.Add("@interesteincontributorsolicitation", SqlDbType.Bit, 1).Value = interesteincontributorsolicitation;
            cmd.Parameters.Add("@interesteinbeingcontributor", SqlDbType.Bit, 1).Value = interesteinbeingcontributor;
            cmd.Parameters.Add("@interesteinbeforeevent", SqlDbType.Bit, 1).Value = interesteinbeforeevent;
            cmd.Parameters.Add("@interesteindayofevent", SqlDbType.Bit, 1).Value = interesteindayofevent;
            cmd.Parameters.Add("@interesteineventteardown", SqlDbType.Bit, 1).Value = interesteineventteardown;
            cmd.Parameters.Add("@interesteinafterevent", SqlDbType.Bit, 1).Value = interesteinafterevent;
            cmd.Parameters.Add("@forvolunteeringbestwaytocontactemail", SqlDbType.VarChar, 128).Value = forvolunteeringbestwaytocontactemail == null ? String.Empty : forvolunteeringbestwaytocontactemail;
            cmd.Parameters.Add("@forvolunteeringbestwaytocontactphone", SqlDbType.VarChar, 128).Value = forvolunteeringbestwaytocontactphone == null ? String.Empty : forvolunteeringbestwaytocontactphone;
            cmd.Parameters.Add("@dateupdated", SqlDbType.DateTime, 8).Value = dateupdated.CompareTo(new DateTime(1800, 1, 1)) < 0 ? new DateTime(1800, 1, 1) : dateupdated;
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException err)
            {
                throw new ApplicationException("TestODS Update Error." + err.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void Delete(int id, int original_id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string deleteString = "DELETE FROM [dbo].[CodeCampEvals] WHERE id = @original_id";
            SqlCommand cmd = new SqlCommand(deleteString, connection);

            cmd.Parameters.Add("@original_id", SqlDbType.Int, 4).Value = id == 0 ? original_id : id;
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException err)
            {
                throw new ApplicationException("TestODS Delete Error." + err.ToString());
            }
            finally
            {
                connection.Close();
            }
        }
    }
}





