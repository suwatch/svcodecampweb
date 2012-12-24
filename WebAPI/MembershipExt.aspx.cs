/*
Copyright © 2005, Peter Kellner
All rights reserved.
http://peterkellner.net

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:

- Redistributions of source code must retain the above copyright
notice, this list of conditions and the following disclaimer.

- Neither Peter Kellner, nor the names of its
contributors may be used to endorse or promote products
derived from this software without specific prior written 
permission. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE 
COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES INCLUDING,
BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

public partial class MembershipExt : BaseContentPage
{
	protected void Page_Load(object sender, EventArgs e)
	{
        if (!IsPostBack)
        {

        }
	}


    protected void ButtonAssignPresentersRolePresenter_Click(object sender, EventArgs e)
    {
        // select distinct username from sessions;
        string sqlSelect =
            @"SELECT DISTINCT dbo.Attendees.Username
                 FROM dbo.SessionPresenter
                 INNER JOIN dbo.Attendees ON (dbo.SessionPresenter.AttendeeId =
                 dbo.Attendees.Id)";

        var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
        sqlConnection.Open();
        SqlDataReader reader1 = null;
        try
        {
            var command1 = new SqlCommand(sqlSelect, sqlConnection);
            reader1 = command1.ExecuteReader();
            while (reader1.Read())
            {
                string username = reader1.IsDBNull(0) ? "" : reader1.GetString(0);
                if (!Roles.IsUserInRole(username, "presenter"))
                {
                    Roles.AddUserToRole(username, "presenter");
                }
            }
        }
        catch (Exception eee1)
        {
            throw new ApplicationException(eee1.ToString());
        }
        finally
        {
            if (reader1 != null) reader1.Dispose();
        }
        sqlConnection.Close();
        sqlConnection.Dispose();



    }
}
