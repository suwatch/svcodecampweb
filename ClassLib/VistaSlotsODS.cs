// $LastChangedDate$
// $Rev$
// $Author$
// $Id$


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Web.Security;
using System.Web.Configuration;

using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Configuration;

namespace CodeCampSV
{
    [DataObject(true)]  // This attribute allows the ObjectDataSource wizard to see this class
    public class VistaSlotsODS
    {
        string connectionString;
        public VistaSlotsODS()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString;
        }
        public class DataObjectVistaSlots
        {
            public DataObjectVistaSlots() { }
            public DataObjectVistaSlots(string description, int id)
            {
                this.description = description;
                this.id = id;
                this.count = 0;
            }

            public DataObjectVistaSlots(string description, int id,int count)
            {
                this.description = description;
                this.id = id;
                this.count = count;
            }

            private string description;
            [DataObjectField(false, false, true)]
            public string Description
            {
                get { return description; }
                set { description = value; }
            }

            private int id;
            [DataObjectField(true, true, false)]
            public int Id
            {
                get { return id; }
                set { id = value; }
            }

            private int count;
            [DataObjectField(false,false,true)]
            public int Count
            {
                get { return count; }
                set { id = count; }
            }

        }

        /// <summary>
        /// get vista slots available (always show id=1 for no interested
        /// </summary>
        /// <param name="maxPerSlot">do not show if more than this number in attendees table</param>
        /// <param name="showId">show this id even if more than maxperslot. (-1 ignore)</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectVistaSlots> GetAvailableVistaSlots(int maxPerSlot,int showId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // First, make a quick list of counts for each id that is unavailable and show
                // not be shown
                string selectCount = @"select VistaSlotsId FROM attendees 
                                             WHERE VistaSlotsId >= 2 AND VistaSlotsId <= 5 Group By VistaSlotsId 
                                             HAVING COUNT(*) >= @MaxPerSlot  ";
                List<int> unavailableSlotsList = new List<int>();
                using (SqlCommand cmdUnAvailable = new SqlCommand(selectCount, conn))
                {
                    cmdUnAvailable.Parameters.Add("@MaxPerSlot", SqlDbType.Int).Value = maxPerSlot;
                    using (SqlDataReader readerUnavailable = cmdUnAvailable.ExecuteReader())
                    {
                        try
                        {
                            while (readerUnavailable.Read())
                            {
                                int id = readerUnavailable.GetInt32(0);
                                unavailableSlotsList.Add(id);
                            }
                        }
                        finally
                        {
                            if (readerUnavailable != null)
                                readerUnavailable.Close();
                        }
                    }
                }
                // check appsettings and if ShowVistaWaitingList is set false, kill entry for waiting list
                if (ConfigurationManager.AppSettings["ShowVistaWaitingList"].Equals("false") && showId != 6)
                    unavailableSlotsList.Add(6); // entry for waiting list
                List<DataObjectVistaSlots> DataTemplateODSList = new List<DataObjectVistaSlots>();
                
                string sqlSelectString = "SELECT description,id FROM [dbo].[VistaSlots]  ORDER BY id";
                using (SqlCommand cmd = new SqlCommand(sqlSelectString, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                string description = reader.IsDBNull(0) ? "" : reader.GetString(0);
                                int id = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                                if (!unavailableSlotsList.Contains(id) || id == showId || id == 1)
                                {
                                    DataObjectVistaSlots td = new DataObjectVistaSlots(description, id);
                                    DataTemplateODSList.Add(td);
                                }
                            }
                        }
                        finally
                        {
                            if (reader != null)
                                reader.Close();
                        }
                    }
                }
                conn.Close();
                // Finally, check and see if more than two items left.  If two means
                // first is do nothing, and second is waiting list.  If this is  more than
                // two entries on the list then don't need to show waiting list so clobber it.
                // (make sure person was not on waiting list before)
                if (DataTemplateODSList.Count > 2 && showId != 6 && ConfigurationManager.AppSettings["ShowVistaWaitingList"].Equals("true"))
                    DataTemplateODSList.RemoveAt(DataTemplateODSList.Count - 1);
                return DataTemplateODSList;
            }
        }



        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<DataObjectVistaSlots> GetAllVistaSlots()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            List<DataObjectVistaSlots> DataTemplateODSList = new List<DataObjectVistaSlots>();
            SqlDataReader reader = null;
            string sqlSelectString = "SELECT description,id FROM [dbo].[VistaSlots] ORDER BY id ";
            SqlCommand cmd = new SqlCommand(sqlSelectString, conn);
            reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    string description = reader.IsDBNull(0) ? "" : reader.GetString(0);
                    int id = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                    DataObjectVistaSlots td = new DataObjectVistaSlots(description, id);
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

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<DataObjectVistaSlots> GetAllVistaSlotsWithCount()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            // First, make a quick list of counts for each id that is unavailable and show
            // not be shown
            string selectCount = "select VistaSlotsId,COUNT(*) FROM attendees Group By VistaSlotsId ";
            Dictionary<int, int> slotsDictionary = new Dictionary<int, int>();
            SqlDataReader reader1 = null;
            SqlCommand cmd1 = new SqlCommand(selectCount, conn);
            reader1 = cmd1.ExecuteReader();
            try
            {
                while (reader1.Read())
                {
                    int id = reader1.IsDBNull(0) ? -1 : reader1.GetInt32(0);
                    int count = reader1.IsDBNull(1) ? -1 : reader1.GetInt32(1);
                    slotsDictionary.Add(id, count);
                }
            }
            finally
            {
                if (reader1 != null) reader1.Close();
            }


            List<DataObjectVistaSlots> DataTemplateODSList = new List<DataObjectVistaSlots>();
            SqlDataReader reader = null;
            string sqlSelectString = "SELECT description,id FROM [dbo].[VistaSlots] ORDER BY id ";
            SqlCommand cmd = new SqlCommand(sqlSelectString, conn);
            reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    string description = reader.IsDBNull(0) ? "" : reader.GetString(0);
                    int id = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                    DataObjectVistaSlots td = null;
                    if (slotsDictionary.ContainsKey(id))
                    {
                        td = new DataObjectVistaSlots(description, id, slotsDictionary[id]);
                    }
                    else
                    {
                        td = new DataObjectVistaSlots(description, id);
                    }
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


    }
}



