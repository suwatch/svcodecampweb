<%@ WebHandler Language="C#" Class="DisplayImage" %>

using System;
using System.Web;

public class DisplayImage : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        bool foundGoodImage = true;

        try
        {
            string strParam1 = context.Request.QueryString["pkid"];
            if (!String.IsNullOrEmpty(strParam1))
            {
                System.Data.SqlClient.SqlConnection sqlConnection =
                    new System.Data.SqlClient.SqlConnection
                    (System.Configuration.ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
                sqlConnection.Open();

                string sqlSelect = "SELECT UserImage FROM attendees WHERE PKID=@PKID";
                //string sqlSelect = "SELECT UserImage FROM attendees WHERE Username = 'xxx'";
                System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand(
                    sqlSelect, sqlConnection);

                Guid userGuid = new Guid(strParam1);
                sqlCommand.Parameters.Add("@PKID", System.Data.SqlDbType.UniqueIdentifier).Value = userGuid;

                System.Data.SqlClient.SqlDataReader sqlDataReader = null;

                sqlDataReader = sqlCommand.ExecuteReader();
                System.Data.SqlTypes.SqlBytes imageArray = null;
                while (sqlDataReader.Read())
                {
                    if (!sqlDataReader.IsDBNull(0))
                    {
                        imageArray = sqlDataReader.GetSqlBytes(0);
                    }
                    else
                    {
                        foundGoodImage = false;
                    }
                }
                sqlDataReader.Close();
                sqlDataReader.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
                if (foundGoodImage)
                {
                    byte[] byteArray = new byte[imageArray.Length];
                    imageArray.Read(0, byteArray, 0, Convert.ToInt32(imageArray.Length));
                    context.Response.ContentType = "image/jpg";
                    context.Response.BinaryWrite(byteArray);
                    //context.Response.End();
                }
            }
            else
            {
                foundGoodImage = false;
            }
        }
        catch (Exception)
        {
            foundGoodImage = false;
        }
        finally
        {
            
        }

        if (!foundGoodImage)
        {
            int newSize = 100;
            System.Drawing.Bitmap bitMap = new System.Drawing.Bitmap(newSize, newSize);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitMap);
            g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Honeydew), new System.Drawing.Rectangle(0, 0, newSize, newSize));

            System.Drawing.Font font = new System.Drawing.Font("Courier", 10);
            System.Drawing.SolidBrush solidBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            g.DrawString("No", font, solidBrush, 2, 10);
            g.DrawString("Image", font, solidBrush, 2, 35);
            g.DrawString("Provided", font, solidBrush, 2, 60);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] byteArray = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(byteArray, 0, Convert.ToInt32(ms.Length));
            
            context.Response.ContentType = "image/jpg";
            context.Response.BinaryWrite(byteArray);
            context.Response.End();
            ms.Close();
            ms.Dispose();
            g.Dispose();
            solidBrush.Dispose();
            font.Dispose();
            bitMap.Dispose();

        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}