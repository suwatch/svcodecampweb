<%@ WebHandler Language="C#" Class="DisplayImage" %>

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using CodeCampSV;

public class DisplayImage : IHttpHandler
{
    #region IHttpHandler Members

    public void ProcessRequest(HttpContext context)
    {
        SqlBytes imageArray = null;
        byte[] byteArray = null;

        try
        {
            string strParamHorizontalSize = context.Request.QueryString["sizex"];
            string strParam1 = context.Request.QueryString["PKID"];
            string strParam2 = context.Request.QueryString["roomid"];
            string strParam3 = context.Request.QueryString["pictureid"];
            string strParam4 = context.Request.QueryString["SessionIdVideo"];
            string cacheValueBoolean = context.Request.QueryString["Cache"] ?? "true";
            bool cacheImage = Convert.ToBoolean(cacheValueBoolean);
            if (!String.IsNullOrEmpty(strParam1) ||
                !String.IsNullOrEmpty(strParam2) ||
                !String.IsNullOrEmpty(strParam3) ||
                !String.IsNullOrEmpty(strParam4))
            {
                int paramHorizontalSize = 0;
                Int32.TryParse(strParamHorizontalSize, out paramHorizontalSize);

                string paramString = context.Request.RawUrl.Substring(context.Request.RawUrl.IndexOf('?') + 1);

                string cacheName = string.Empty;
                if (!String.IsNullOrEmpty(strParam1))
                {
                    cacheName = Utils.CacheDisplayImage + "?pkid=" + strParam1 + "&sizex=" + paramHorizontalSize;
                }
                else if (!String.IsNullOrEmpty(strParam2))
                {
                    cacheName = Utils.CacheDisplayImage + "?roomid=" + strParam2 + "&sizex=" + paramHorizontalSize;
                }
                else if (!String.IsNullOrEmpty(strParam3))
                {
                    cacheName = Utils.CacheDisplayImage + "?pictureid=" + strParam3 + "&sizex=" + paramHorizontalSize;
                }
                else if (!String.IsNullOrEmpty(strParam4))
                {
                    cacheName = Utils.CacheDisplayImage + "?SessionId=" + strParam4 + "&sizex=" + paramHorizontalSize;  // todo: make this right size
                }

                if (HttpContext.Current.Cache[cacheName] == null || !cacheImage)
                {
                    var sqlConnection =
                        new SqlConnection
                            (ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
                    sqlConnection.Open();

                    string sqlSelect = string.Empty;
                    if (!String.IsNullOrEmpty(strParam1))
                    {
                        sqlSelect = "SELECT UserImage FROM attendees WHERE PKID=@PKID";
                    }

                    if (!String.IsNullOrEmpty(strParam2))
                    {
                        sqlSelect = "SELECT picture FROM LectureRooms WHERE id=@id";
                    }

                    if (!String.IsNullOrEmpty(strParam3))
                    {
                        sqlSelect = "SELECT PictureBytes FROM Pictures WHERE id=@id";
                    }

                    if (!String.IsNullOrEmpty(strParam4))
                    {
                        sqlSelect =
                            @"SELECT 
                                  dbo.Video.PictureBytes
                                FROM
                                  dbo.SessionVideo
                                  INNER JOIN dbo.Video ON (dbo.SessionVideo.VideoId = dbo.Video.Id)
                                WHERE
                                  dbo.SessionVideo.SessionId = @id";
                    }

                    var sqlCommand = new SqlCommand(
                        sqlSelect, sqlConnection);

                    if (!String.IsNullOrEmpty(strParam1))
                    {
                        var userGuid = new Guid(strParam1);
                        sqlCommand.Parameters.Add("@PKID", SqlDbType.UniqueIdentifier).Value = userGuid;
                    }


                    if (!String.IsNullOrEmpty(strParam2))
                    {
                        int roomId = 0;
                        Int32.TryParse(strParam2, out roomId);
                        sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = roomId;
                    }

                    if (!String.IsNullOrEmpty(strParam3))
                    {
                        int pictureId = 0;
                        Int32.TryParse(strParam3, out pictureId);
                        sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = pictureId;
                    }

                    if (!String.IsNullOrEmpty(strParam4))
                    {
                        int sessionId = 0;
                        Int32.TryParse(strParam4, out sessionId);
                        sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = sessionId;
                    }

                    SqlDataReader sqlDataReader = null;

                    sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        if (!sqlDataReader.IsDBNull(0))
                        {
                            imageArray = sqlDataReader.GetSqlBytes(0);
                            byteArray = new byte[imageArray.Length];
                            imageArray.Read(0, byteArray, 0, Convert.ToInt32(imageArray.Length));
                        }
                        else
                        {
                            byteArray = GetEmptyImagePicture();
                        }
                    }
                    sqlDataReader.Close();
                    sqlDataReader.Dispose();
                    sqlConnection.Close();
                    sqlConnection.Dispose();
                    if (!String.IsNullOrEmpty(cacheName) && byteArray != null)
                    {
                        // this causes a lot of database access so x 10 the timeout
                        int cacheTimeout = Utils.RetrieveSecondsForSessionCacheTimeout()*10;
                        
                        HttpContext.Current.Cache.Insert(cacheName, byteArray,                
                                                         null,
                                                         DateTime.Now.Add(new TimeSpan(0, 0, cacheTimeout)), 
                                                         TimeSpan.Zero);
                    }
                    else
                    {
                    }
                }
                else
                {
                    byteArray = (byte[]) HttpContext.Current.Cache[cacheName];
                }

                context.Response.ContentType = "image/jpg";

                if (paramHorizontalSize != 0)
                {
                    byte[] ba = Utils.ResizeFromByteArray(string.Empty, paramHorizontalSize, byteArray);
                    if (byteArray != null)
                    {
                        context.Response.BinaryWrite(ba);
                        //context.Response.BinaryWrite(byteArray);
                    }
                }
                else
                {
                    if (byteArray != null)
                    {
                        context.Response.BinaryWrite(byteArray);
                    }
                }
                //context.Response.End();
            }
            else
            { // no params
                byteArray = GetEmptyImagePicture();
                context.Response.BinaryWrite(byteArray);
            }
        }
        catch (Exception eee)
        {
            string er = eee.ToString();
            throw new ApplicationException(eee.ToString());
        }
        finally
        {
        }

        //context.Response.ContentType = "image/jpg";
        //context.Response.BinaryWrite(byteArray);
        //context.Response.End();
    }


    public bool IsReusable
    {
        get { return false; }
    }

    #endregion

    private byte[] GetEmptyImagePicture()
    {
        string nopicFilename = HttpContext.Current.Server.MapPath("~/Images/") + "nopic.gif";
        var bitMap = new Bitmap(nopicFilename);
        var ms = new MemoryStream();
        bitMap.Save(ms, ImageFormat.Jpeg);
        var byteArray = new byte[ms.Length];
        ms.Position = 0;
        ms.Read(byteArray, 0, Convert.ToInt32(ms.Length));
        ms.Close();
        ms.Dispose();
        bitMap.Dispose();
        return byteArray;
    }
}


//int newSize = 100;
//System.Drawing.Bitmap bitMap = new System.Drawing.Bitmap(newSize, newSize);
//System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitMap);
//g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Honeydew), new System.Drawing.Rectangle(0, 0, newSize, newSize));

//System.Drawing.Font font = new System.Drawing.Font("Courier", 10);
//System.Drawing.SolidBrush solidBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
//g.DrawString("Image", font, solidBrush, 2, 10);
//g.DrawString("Not", font, solidBrush, 2, 35);
//g.DrawString("Provided", font, solidBrush, 2, 60);