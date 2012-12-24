<%@ WebHandler Language="C#" Class="DisplayAd"  %>

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Web;
using CodeCampSV;

public class DisplayAd : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {

        try
        {
            //bool showSessions = true;
            bool showTime = false;
            //int cacheExpireMinutes = 1;
            //int cacheExpireSeconds = 1;
            //int cacheExpireMinutes = 0;
            //int cacheExpireSeconds = 1;

            string threshHoldString = System.Configuration.ConfigurationManager.AppSettings["ShowRegThreshHold"].ToString();
            int threshHoldToShowRegisteredNumber = Convert.ToInt32(threshHoldString);

            // the image we display depends on e ImageType
            System.Drawing.Bitmap bitMap = null;
            System.Drawing.Graphics g = null;
            System.Drawing.Pen blackPen = new System.Drawing.Pen(System.Drawing.Color.Gray);
            System.Drawing.SolidBrush solidBrush = null;
            // System.Drawing.SolidBrush solidBrushWhite = null;

            string strParam1 = context.Request["ImageType"];
            if (String.IsNullOrEmpty(strParam1))
            {
                strParam1 = "2";
            }

            // force 1 if not choice.
            if (!strParam1.Equals("1") && !strParam1.Equals("2") &&
                !strParam1.Equals("3") && !strParam1.Equals("4"))
            {
                strParam1 = "1";
            }

            const int bitmap150_letterY = 105;
            const int bitmap450_letterY = 299;

            string cacheKey = "DisplayjAd_" + strParam1;
            byte[] byteArray = null;
            if (HttpContext.Current.Cache[cacheKey] == null)
            {
                //strParam1 = "5"; //$$$ always show 2008 logo

                if (strParam1.Equals("1"))
                { // this is the case of strParam not 2,3 or 4
                    strParam1 = "1";  // just in case, we force the issue for cache below
                    string fileName = context.Server.MapPath("App_Themes/default/svcc_badge_150.png");
                    bitMap = new System.Drawing.Bitmap(fileName);
                    g = System.Drawing.Graphics.FromImage(bitMap);
                    blackPen.Dispose();
                }
                else if (strParam1.Equals("2"))
                {
                    string fileName = context.Server.MapPath("App_Themes/default/svcc_badge_150_spacer.png");
                    bitMap = new System.Drawing.Bitmap(fileName);
                    g = System.Drawing.Graphics.FromImage(bitMap);
                    
                    

                    System.Drawing.Font font = new System.Drawing.Font("Courier", 8);  // changed from size 11 9/2009
                    solidBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

                    //   

                    if (CodeCampSV.Utils.IsSqlServerOnLine())
                    {
                        int numberRegistered = CodeCampSV.Utils.GetNumberRegistered();
                        int numberSessions = CodeCampSV.Utils.GetNumberSessions();

                        if (numberRegistered > threshHoldToShowRegisteredNumber)
                        {
                            const string showStrTemplate = "{0} Sessions,{1} Registered";
                            g.DrawString(String.Format(showStrTemplate, numberSessions, numberRegistered), font, solidBrush, 4, bitmap150_letterY);

                            //if (!showTime)
                            //{
                            //    g.DrawString(CodeCampSV.Utils.GetNumberRegistered().ToString() + " People Registered", font, solidBrush, 5, bitmap150_letterY);
                            //}
                            //else
                            //{
                            //    g.DrawString(DateTime.Now.ToLongTimeString() + " People Registered", font, solidBrush, 5, bitmap150_letterY);
                            //}
                        }
                        else
                        {
                            string messageOut = "Call for Speakers!";
                            if (!showTime)
                            {
                                g.DrawString(messageOut, font, solidBrush, 5, bitmap150_letterY);
                            }
                            else
                            {
                                g.DrawString(DateTime.Now.ToLongTimeString() + messageOut, font, solidBrush, 5, bitmap150_letterY);
                            }
                        }
                    }
                    solidBrush.Dispose();
                    blackPen.Dispose();
                    font.Dispose();
                }
                else if (strParam1.Equals("3"))
                {
                    string fileName = context.Server.MapPath("App_Themes/default/svcc_badge_450.png");
                    bitMap = new System.Drawing.Bitmap(fileName);
                    g = System.Drawing.Graphics.FromImage(bitMap);                    
                    blackPen.Dispose();
                }
                else if (strParam1.Equals("4"))
                {
                    string fileName = context.Server.MapPath("App_Themes/default/svcc_badge_450_spacer.png");
                    bitMap = new System.Drawing.Bitmap(fileName);
                    g = System.Drawing.Graphics.FromImage(bitMap);

                    Font font = new System.Drawing.Font("Courier", 13);
                    solidBrush = new SolidBrush(System.Drawing.Color.Black);

                    int xEdge = 3;
                    g.FillRectangle(Brushes.White, xEdge, bitmap450_letterY, bitMap.Width - 2 - (xEdge * 2), bitMap.Height - 2 - bitmap450_letterY - 0);


                    string messageOut;
                    if (CodeCampSV.Utils.IsSqlServerOnLine())
                    {

                        int numberRegistered = Utils.GetNumberRegistered();
                        if (numberRegistered > threshHoldToShowRegisteredNumber)
                        {
                            if (!showTime)
                            {
                                messageOut = CodeCampSV.Utils.GetNumberRegistered().ToString(CultureInfo.InvariantCulture) + " People Registered";
                            }
                            else
                            {
                                messageOut = DateTime.Now.ToLongTimeString() +
                                    CodeCampSV.Utils.GetNumberRegistered().ToString(CultureInfo.InvariantCulture) + " People Registered";
                            }
                        }
                        else
                        {
                            if (!showTime)
                            {
                                messageOut = "Call for Speakers!  (click here)";
                            }
                            else
                            {
                                messageOut = DateTime.Now.ToLongTimeString() + " Call for Speakers!  (click here)";
                            }
                        }
                    }
                    else
                    {
                        messageOut = ".";
                    }

                    g.DrawString(messageOut, font, solidBrush, 3, bitmap450_letterY);
                    blackPen.Dispose();
                    solidBrush.Dispose();
                    font.Dispose();
                }

                else if (strParam1.Equals("5"))
                {
                    string fileName = context.Server.MapPath("Images/logo-2008.jpg");
                    bitMap = new System.Drawing.Bitmap(fileName);
                    g = System.Drawing.Graphics.FromImage(bitMap);
                    g.DrawRectangle(blackPen, 1, 1, bitMap.Width - 2, bitMap.Height - 2);
                    blackPen.Dispose();
                }



                var ms = new System.IO.MemoryStream();


                ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

                // Create an Encoder object based on the GUID
                // for the Quality parameter category.
                var myEncoder = Encoder.Quality;
                var myEncoderParameters = new EncoderParameters(1);

                // the second parameter is the percent quality.  10L is 10%   90L would be 90%
                var myEncoderParameter = new EncoderParameter(myEncoder, 95L);

                myEncoderParameters.Param[0] = myEncoderParameter;
                //bmp1.Save(@"c:\TestPhotoQualityFifty.jpg", jgpEncoder, myEncoderParameters);
                bitMap.Save(ms, jgpEncoder, myEncoderParameters);

                byteArray = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(byteArray, 0, Convert.ToInt32(ms.Length));


                //DateTime expireTime = DateTime.Now.Add(new TimeSpan(0, cacheExpireMinutes, cacheExpireSeconds));
                HttpContext.Current.Cache.Insert(cacheKey, byteArray, null,
                //    DateTime.Now.Add(new TimeSpan(0, 0, 0)), TimeSpan.Zero);
                 DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())), TimeSpan.Zero);


                ms.Close();
                ms.Dispose();
                g.Dispose();

                bitMap.Dispose();

            }
            else
            {
                byteArray = (Byte[])HttpContext.Current.Cache[cacheKey];
            }

            context.Response.ContentType = "image/jpeg";
            context.Response.BinaryWrite(byteArray);


        }
        catch (Exception ee)
        {
            throw new ApplicationException(ee.ToString());
        }
        finally
        {

        }

    }

    private ImageCodecInfo GetEncoder(ImageFormat format)
    {
        var codecs = ImageCodecInfo.GetImageDecoders();
        return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}