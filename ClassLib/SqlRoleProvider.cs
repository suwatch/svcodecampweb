using System.Web.Security;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Globalization;
using System.Data.SqlClient;
using System.Data;

/*

CREATE TABLE [dbo].[Roles] (
  [RoleName] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS,
  [ApplicationName] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS
)
ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [Roles_idx] ON [dbo].[Roles]
  ([RoleName], [ApplicationName])
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

CREATE TABLE [dbo].[UsersInRoles] (
  [Username] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS,
  [Rolename] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS,
  [ApplicationName] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS
)
ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [UsersInRoles_idx] ON [dbo].[UsersInRoles]
  ([Username], [Rolename], [ApplicationName])
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO
 
*/


namespace CodeCampSV
{

    public sealed class SqlRoleProvider : RoleProvider
    {

        //
        // Global connection string, generic exception message, event log info.
        //

        private string rolesTable = "RolesInMembership";
        private string usersInRolesTable = "UsersInRolesMembership";

        private string eventSource = "SqlRoleProvider";
        private string eventLog = "Application";
        private string exceptionMessage = "An exception occurred. Please check the Event Log.";

        private ConnectionStringSettings pConnectionStringSettings;
        private string connectionString;


        //
        // If false, exceptions are thrown to the caller. If true,
        // exceptions are written to the event log.
        //

        private bool pWriteExceptionsToEventLog = false;

        public bool WriteExceptionsToEventLog
        {
            get { return pWriteExceptionsToEventLog; }
            set { pWriteExceptionsToEventLog = value; }
        }



        //
        // System.Configuration.Provider.ProviderBase.Initialize Method
        //

        public override void Initialize(string name, NameValueCollection config)
        {

            //
            // Initialize values from web.config.
            //

            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "SqlRoleProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Sample Sql Role provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);


            if (config["applicationName"] == null || config["applicationName"].Trim() == "")
            {
                pApplicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            }
            else
            {
                pApplicationName = config["applicationName"];
            }


            if (config["writeExceptionsToEventLog"] != null)
            {
                if (config["writeExceptionsToEventLog"].ToUpper() == "TRUE")
                {
                    pWriteExceptionsToEventLog = true;
                }
            }


            //
            // Initialize SqlConnection.
            //

            pConnectionStringSettings = ConfigurationManager.
              ConnectionStrings[config["connectionStringName"]];

            if (pConnectionStringSettings == null || pConnectionStringSettings.ConnectionString.Trim() == "")
            {
                throw new ProviderException("Connection string cannot be blank.");
            }

            connectionString = pConnectionStringSettings.ConnectionString;
        }



        //
        // System.Web.Security.RoleProvider properties.
        //


        private string pApplicationName;


        public override string ApplicationName
        {
            get { return pApplicationName; }
            set { pApplicationName = value; }
        }

        //
        // System.Web.Security.RoleProvider methods.
        //

        //
        // RoleProvider.AddUsersToRoles
        //

        public override void AddUsersToRoles(string[] usernames, string[] rolenames)
        {
            foreach (string rolename in rolenames)
            {
                if (!RoleExists(rolename))
                {
                    throw new ProviderException("Role name not found.");
                }
            }

            foreach (string username in usernames)
            {
                if (username.IndexOf(',') > 0)
                {
                    throw new ArgumentException("User names cannot contain commas.");
                }

                foreach (string rolename in rolenames)
                {
                    if (IsUserInRole(username, rolename))
                    {
                        throw new ProviderException("User is already in role.");
                    }
                }
            }


            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO " + usersInRolesTable + 
                    " (Username, Rolename, ApplicationName) " +
                    " Values(@Username, @Rolename, @ApplicationName)", conn);

            SqlParameter userParm = cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255);
            SqlParameter roleParm = cmd.Parameters.Add("@Rolename", SqlDbType.VarChar, 255);
            cmd.Parameters.Add("@ApplicationName", SqlDbType.VarChar, 255).Value = ApplicationName;

            SqlTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                cmd.Transaction = tran;

                foreach (string username in usernames)
                {
                    foreach (string rolename in rolenames)
                    {
                        userParm.Value = username;
                        roleParm.Value = rolename;
                        cmd.ExecuteNonQuery();
                    }
                }

                tran.Commit();
            }
            catch (SqlException e)
            {
                try
                {
                    tran.Rollback();
                }
                catch { }


                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "AddUsersToRoles");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }
        }


        //
        // RoleProvider.CreateRole
        //

        public override void CreateRole(string rolename)
        {
            if (rolename.IndexOf(',') > 0)
            {
                throw new ArgumentException("Role names cannot contain commas.");
            }

            if (RoleExists(rolename))
            {
                throw new ProviderException("Role name already exists.");
            }

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO " + rolesTable + 
                    " (Rolename, ApplicationName) " +
                    " Values(@Rolename, @ApplicationName)", conn);

            cmd.Parameters.Add("@Rolename", SqlDbType.VarChar, 255).Value = rolename;
            cmd.Parameters.Add("@ApplicationName", SqlDbType.VarChar, 255).Value = ApplicationName;

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "CreateRole");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }
        }


        //
        // RoleProvider.DeleteRole
        //

        public override bool DeleteRole(string rolename, bool throwOnPopulatedRole)
        {
            if (!RoleExists(rolename))
            {
                throw new ProviderException("Role does not exist.");
            }

            if (throwOnPopulatedRole && GetUsersInRole(rolename).Length > 0)
            {
                throw new ProviderException("Cannot delete a populated role.");
            }

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("DELETE FROM " + rolesTable + 
                    " WHERE Rolename = @Rolename AND ApplicationName = @ApplicationName", conn);

            cmd.Parameters.Add("@Rolename", SqlDbType.VarChar, 255).Value = rolename;
            cmd.Parameters.Add("@ApplicationName", SqlDbType.VarChar, 255).Value = ApplicationName;


            SqlCommand cmd2 = new SqlCommand("DELETE FROM " + usersInRolesTable + 
                    " WHERE Rolename = @Rolename AND ApplicationName = @ApplicationName", conn);

            cmd2.Parameters.Add("@Rolename", SqlDbType.VarChar, 255).Value = rolename;
            cmd2.Parameters.Add("@ApplicationName", SqlDbType.VarChar, 255).Value = ApplicationName;

            SqlTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                cmd.Transaction = tran;
                cmd2.Transaction = tran;

                cmd2.ExecuteNonQuery();
                cmd.ExecuteNonQuery();

                tran.Commit();
            }
            catch (SqlException e)
            {
                try
                {
                    tran.Rollback();
                }
                catch { }


                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "DeleteRole");

                    return false;
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            return true;
        }


        //
        // RoleProvider.GetAllRoles
        //

        public override string[] GetAllRoles()
        {
            string tmpRoleNames = "";

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT Rolename FROM " + rolesTable + 
                      " WHERE ApplicationName = @ApplicationName", conn);

            cmd.Parameters.Add("@ApplicationName", SqlDbType.VarChar, 255).Value = ApplicationName;

            SqlDataReader reader = null;

            try
            {
                conn.Open();

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tmpRoleNames += reader.GetString(0) + ",";
                    }
                    reader.Close();
                }
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetAllRoles");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            if (tmpRoleNames.Length > 0)
            {
                // Remove trailing comma.
                tmpRoleNames = tmpRoleNames.Substring(0, tmpRoleNames.Length - 1);
                return tmpRoleNames.Split(',');
            }

            return new string[0];
        }


        //
        // RoleProvider.GetRolesForUser
        //

        public override string[] GetRolesForUser(string username)
        {
            string tmpRoleNames = "";

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT Rolename FROM " + usersInRolesTable + 
                    " WHERE Username = @Username AND ApplicationName = @ApplicationName", conn);

            cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;
            cmd.Parameters.Add("@ApplicationName", SqlDbType.VarChar, 255).Value = ApplicationName;

            SqlDataReader reader = null;

            try
            {
                conn.Open();

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tmpRoleNames += reader.GetString(0) + ",";
                    }
                    reader.Close();
                }
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetRolesForUser");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            if (tmpRoleNames.Length > 0)
            {
                // Remove trailing comma.
                tmpRoleNames = tmpRoleNames.Substring(0, tmpRoleNames.Length - 1);
                return tmpRoleNames.Split(',');
            }

            return new string[0];
        }


        //
        // RoleProvider.GetUsersInRole
        //

        public override string[] GetUsersInRole(string rolename)
        {
            string tmpUserNames = "";

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT Username FROM " + usersInRolesTable + 
                      " WHERE Rolename = @Rolename AND ApplicationName = @ApplicationName", conn);

            cmd.Parameters.Add("@Rolename", SqlDbType.VarChar, 255).Value = rolename;
            cmd.Parameters.Add("@ApplicationName", SqlDbType.VarChar, 255).Value = ApplicationName;

            SqlDataReader reader = null;

            try
            {
                conn.Open();

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tmpUserNames += reader.GetString(0) + ",";
                    }
                    reader.Close();
                }
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUsersInRole");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            if (tmpUserNames.Length > 0)
            {
                // Remove trailing comma.
                tmpUserNames = tmpUserNames.Substring(0, tmpUserNames.Length - 1);
                return tmpUserNames.Split(',');
            }

            return new string[0];
        }


        //
        // RoleProvider.IsUserInRole
        //

        public override bool IsUserInRole(string username, string rolename)
        {
            bool userIsInRole = false;

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM " + usersInRolesTable + 
                    " WHERE Username = @Username AND Rolename = @Rolename AND ApplicationName = @ApplicationName", conn);

            cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;
            cmd.Parameters.Add("@Rolename", SqlDbType.VarChar, 255).Value = rolename;
            cmd.Parameters.Add("@ApplicationName", SqlDbType.VarChar, 255).Value = ApplicationName;

            try
            {
                conn.Open();

                long numRecs = Convert.ToInt64(cmd.ExecuteScalar());

                if (numRecs > 0)
                {
                    userIsInRole = true;
                }
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "IsUserInRole");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            return userIsInRole;
        }


        //
        // RoleProvider.RemoveUsersFromRoles
        //

        public override void RemoveUsersFromRoles(string[] usernames, string[] rolenames)
        {
            foreach (string rolename in rolenames)
            {
                if (!RoleExists(rolename))
                {
                    throw new ProviderException("Role name not found.");
                }
            }

            foreach (string username in usernames)
            {
                foreach (string rolename in rolenames)
                {
                    if (!IsUserInRole(username, rolename))
                    {
                        throw new ProviderException("User is not in role.");
                    }
                }
            }


            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("DELETE FROM " + usersInRolesTable + 
                    " WHERE Username = @Username AND Rolename = @Rolename AND ApplicationName = @ApplicationName", conn);

            SqlParameter userParm = cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255);
            SqlParameter roleParm = cmd.Parameters.Add("@Rolename", SqlDbType.VarChar, 255);
            cmd.Parameters.Add("@ApplicationName", SqlDbType.VarChar, 255).Value = ApplicationName;

            SqlTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                cmd.Transaction = tran;

                foreach (string username in usernames)
                {
                    foreach (string rolename in rolenames)
                    {
                        userParm.Value = username;
                        roleParm.Value = rolename;
                        cmd.ExecuteNonQuery();
                    }
                }

                tran.Commit();
            }
            catch (SqlException e)
            {
                try
                {
                    tran.Rollback();
                }
                catch { }


                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "RemoveUsersFromRoles");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }
        }


        //
        // RoleProvider.RoleExists
        //

        public override bool RoleExists(string rolename)
        {
            bool exists = false;

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM " + rolesTable + 
                      " WHERE Rolename = @Rolename AND ApplicationName = @ApplicationName", conn);

            cmd.Parameters.Add("@Rolename", SqlDbType.VarChar, 255).Value = rolename;
            cmd.Parameters.Add("@ApplicationName", SqlDbType.VarChar, 255).Value = ApplicationName;

            try
            {
                conn.Open();

                long numRecs = Convert.ToInt64(cmd.ExecuteScalar());

                if (numRecs > 0)
                {
                    exists = true;
                }
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "RoleExists");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            return exists;
        }

        //
        // RoleProvider.FindUsersInRole
        //

        public override string[] FindUsersInRole(string rolename, string usernameToMatch)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT Username FROM " + usersInRolesTable + 
                      "WHERE Username LIKE @UsernameSearch AND Rolename = @Rolename AND ApplicationName = @ApplicationName", conn);
            cmd.Parameters.Add("@UsernameSearch", SqlDbType.VarChar, 255).Value = usernameToMatch;
            cmd.Parameters.Add("@RoleName", SqlDbType.VarChar, 255).Value = rolename;
            cmd.Parameters.Add("@ApplicationName", SqlDbType.VarChar, 255).Value = pApplicationName;

            string tmpUserNames = "";
            SqlDataReader reader = null;

            try
            {
                conn.Open();

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tmpUserNames += reader.GetString(0) + ",";
                    }
                    reader.Close();
                }
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "FindUsersInRole");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                conn.Close();
            }

            if (tmpUserNames.Length > 0)
            {
                // Remove trailing comma.
                tmpUserNames = tmpUserNames.Substring(0, tmpUserNames.Length - 1);
                return tmpUserNames.Split(',');
            }

            return new string[0];
        }

        //
        // WriteToEventLog
        //   A helper function that writes exception detail to the event log. Exceptions
        // are written to the event log as a security measure to avoid private database
        // details from being returned to the browser. If a method does not return a status
        // or boolean indicating the action succeeded or failed, a generic exception is also 
        // thrown by the caller.
        //

        private void WriteToEventLog(SqlException e, string action)
        {
            EventLog log = new EventLog();
            log.Source = eventSource;
            log.Log = eventLog;

            string message = exceptionMessage + "\n\n";
            message += "Action: " + action + "\n\n";
            message += "Exception: " + e.ToString();

            log.WriteEntry(message);
        }

    }
}