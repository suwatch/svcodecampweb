/*
Copyright © 2006, Peter Kellner
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PeterKellner.Utils
{
    /// <summary>
    /// Utility class used for passing parameters between HttpHandler and Custom
    /// server control
    /// </summary>
    public class CaptchaParams
    {
        public CaptchaParams()
        {
        }

        public CaptchaParams(
            string plainValue,
            string encryptedValue,
            int heightCaptchaPixels,
            int widthCaptchaPixels,
            int captchaType,
            string fontFamilyString)
        {
            PlainValue = plainValue;
            EncryptedValue = encryptedValue;
            HeightCaptchaPixels = heightCaptchaPixels;
            WidthCaptchaPixels = widthCaptchaPixels;
            CaptchaType = captchaType;
            FontFamilyString = fontFamilyString;
        }

        public string PlainValue { get; set; }

        public string EncryptedValue { get; set; }

        public int HeightCaptchaPixels { get; set; }

        public int WidthCaptchaPixels { get; set; }

        public int CaptchaType { get; set; }

        public string FontFamilyString { get; set; }
    }

    /// <summary>
    /// Utility Functions for Captcha Custom Control
    /// </summary>
    public class CaptchaImageUtils
    {
        public static readonly string SymetricKey = "233PGK";

        //Depends on how random you need.  Session.GetHashCode() changes on every postback - so a differnt class is created each time - as you would expect in a stateless system
        //SessionId changes each postback. My solution is fine, just add something like the following.  The overhead is minimal - it occurs just once at the start of the users session.
        //Random r = new Random();
        //Session.Add("MyRnd", r.Next);

        // http://www.csharper.net/blog/library_encrypt_and_decrypt_methods_using_tripledes_and_md5.aspx
        public static string Encrypt(string toEncrypt, string key, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            if (useHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
            else
                keyArray = Encoding.UTF8.GetBytes(key);

            var tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        // http://www.csharper.net/blog/library_encrypt_and_decrypt_methods_using_tripledes_and_md5.aspx
        public static string Decrypt(string toDecrypt, string key, bool useHashing)
        {
            var resultArray = new byte[0];
            try
            {
                byte[] keyArray;

                toDecrypt = toDecrypt.Replace(" ", "+");// http://forums.asp.net/t/1222829.aspx

                byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

                if (useHashing)
                {
                    var hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                    hashmd5.Clear();
                }
                else
                {
                    keyArray = Encoding.UTF8.GetBytes(key);
                }
                var tdes = new TripleDESCryptoServiceProvider
                               {
                                   Key = keyArray,
                                   Mode = CipherMode.ECB,
                                   Padding = PaddingMode.PKCS7
                               };

                ICryptoTransform cTransform = tdes.CreateDecryptor();
           
                resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return Encoding.UTF8.GetString(resultArray);
        }


        /// <summary>
        /// Get an array representing the Captcha as a bitmap
        /// Currently only supports two types of Captcha
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        public static byte[] GetImageCaptcha(CaptchaParams cp)
        {
            byte[] bitMapArray;
            if (cp.CaptchaType == 1)
            {
                bitMapArray = GenerateImageType1(cp);
            }
            else if (cp.CaptchaType == 2)
            {
                bitMapArray = GenerateImageType2(cp);
            }
            else
            {
                throw new ApplicationException("CaptchaType Must Be 1 or 2");
            }
            return bitMapArray;
        }

        /// <summary>
        /// Inspired and copied from an article on CodeProject at
        /// http://www.codeproject.com/aspnet/CaptchaImage.asp
        /// Written by BrainJar. (Mike Hall I think)
        /// Later, this code was modified by wumpus1 (Jeff Atwood)
        /// and can be found at this location:
        /// http://www.codeproject.com/aspnet/CaptchaControl.asp
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        private static byte[] GenerateImageType1(CaptchaParams cp)
        {
            const float _noise = 30F;
            const float _skewing = 4.5F;
            var random = new Random();

            // Create a new 32-bit bitmap image.
            var bitmap = new Bitmap(cp.WidthCaptchaPixels, cp.HeightCaptchaPixels, PixelFormat.Format32bppArgb);
            // Create a graphics object for drawing.
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new Rectangle(0, 0, cp.WidthCaptchaPixels, cp.HeightCaptchaPixels);
            // Fill in the background.
            var hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
            g.FillRectangle(hatchBrush, rect);
            // Set up the text font.
            SizeF size;
            float fontSize = rect.Height - 4;
            Font font = null;
            // Adjust the font size until the text fits within the image.
            do
            {
                fontSize--;
                font = new Font(cp.FontFamilyString, fontSize, FontStyle.Bold);
                size = g.MeasureString(cp.PlainValue, font);
            } while (size.Width > rect.Width);
            // Set up the text format.
            var format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            // Create a path using the text and warp it randomly.
            var path = new GraphicsPath();
            path.AddString(cp.PlainValue, font.FontFamily, (int) font.Style, font.Size, rect, format);
            float v = _skewing;
            PointF[] points = {
                                  new PointF(random.Next(rect.Width)/v, random.Next(rect.Height)/v),
                                  new PointF(rect.Width - random.Next(rect.Width)/v, random.Next(rect.Height)/v),
                                  new PointF(random.Next(rect.Width)/v, rect.Height - random.Next(rect.Height)/v),
                                  new PointF(rect.Width - random.Next(rect.Width)/v,
                                             rect.Height - random.Next(rect.Height)/v)
                              };
            var matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);
            // Draw the text.
            hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray);
            g.FillPath(hatchBrush, path);
            // Add some random noise.
            int m = Math.Max(rect.Width, rect.Height);
            for (int i = 0; i < (int) (rect.Width*rect.Height/_noise); i++)
            {
                int x = random.Next(rect.Width);
                int y = random.Next(rect.Height);
                int w = random.Next(m/50);
                int h = random.Next(m/50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }
            // Clean up.
            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();
            // Set the image.

            var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Gif);
            bitmap.Dispose();
            return stream.GetBuffer();
        }

        /// <summary>
        /// This image type was borrowed from the Blog Starter Kit
        /// Provided by Shanku Niyogi, Product Unit Manger of the UI
        /// Framework at Microsoft
        /// at this URL:  http://www.shankun.com/Post.aspx?postID=13
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        private static byte[] GenerateImageType2(CaptchaParams cp)
        {
            var rng = new RNGCryptoServiceProvider();
            var rand = new Byte[200];
            rng.GetBytes(rand);
            int i = 0;

            var bmp = new Bitmap(cp.WidthCaptchaPixels, cp.HeightCaptchaPixels, PixelFormat.Format24bppRgb);
            Bitmap cloneBmp = null;
            Graphics g = null;
            LinearGradientBrush backgroundBrush = null;
            LinearGradientBrush textBrush = null;
            var circleBrush = new SolidBrush[3];
            Font font = null;
            GraphicsPath path = null;

            try
            {
                g = Graphics.FromImage(bmp);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var r = new Rectangle(0, 0, cp.WidthCaptchaPixels, cp.HeightCaptchaPixels);
                backgroundBrush = new LinearGradientBrush(
                    new RectangleF(0, 0, cp.WidthCaptchaPixels, cp.HeightCaptchaPixels),
                    Color.FromArgb(rand[i++] / 2 + 128, rand[i++] / 2 + 128, 255),
                    Color.FromArgb(255, rand[i++] / 2 + 128, rand[i++] / 2 + 128),
                    rand[i++] * 360 / 256);
                g.FillRectangle(backgroundBrush, r);

                for (int br = 0; br < circleBrush.Length; br++)
                {
                    circleBrush[br] = new SolidBrush(Color.FromArgb(128, rand[i++], rand[i++], rand[i++]));
                }

                for (int circle = 0; circle < 30; circle++)
                {
                    int radius = rand[i++] % 10;
                    g.FillEllipse(circleBrush[circle % 2],
                                  rand[i++] * cp.WidthCaptchaPixels / 256,
                                  rand[i++] * cp.HeightCaptchaPixels / 256,
                                  radius, radius);
                }

                font = new Font(cp.FontFamilyString, cp.HeightCaptchaPixels / 2, FontStyle.Bold);
                var format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                path = new GraphicsPath();
                path.AddString(cp.PlainValue, font.FontFamily,
                               (int)font.Style, font.Size, r, format);

                textBrush = new LinearGradientBrush(
                    new RectangleF(0, 0, cp.WidthCaptchaPixels, cp.HeightCaptchaPixels),
                    Color.FromArgb(rand[i] % 128, rand[i] % 128, rand[i++] % 128),
                    Color.FromArgb(rand[i] % 128, rand[i] % 128, rand[i++] % 128),
                    rand[i++] * 360 / 256);
                g.FillPath(textBrush, path);


                cloneBmp = (Bitmap)bmp.Clone();

                int distortionSeed = rand[i++];
                double distortion = distortionSeed > 128 ? 5 + (distortionSeed - 128) % 5 : -5 - distortionSeed % 5;
                for (int y = 0; y < cp.HeightCaptchaPixels; y++)
                {
                    for (int x = 0; x < cp.WidthCaptchaPixels; x++)
                    {
                        // Adds a simple wave
                        var newX = (int)(x + (distortion * Math.Sin(Math.PI * y / 96.0)));
                        var newY = (int)(y + (distortion * Math.Cos(Math.PI * x / 64.0)));
                        if (newX < 0 || newX >= cp.WidthCaptchaPixels)
                        {
                            newX = 0;
                        }
                        if (newY < 0 || newY >= cp.HeightCaptchaPixels)
                        {
                            newY = 0;
                        }
                        bmp.SetPixel(x, y, cloneBmp.GetPixel(newX, newY));
                    }
                }

                var stream = new MemoryStream();
                bmp.Save(stream, ImageFormat.Gif);
                return stream.GetBuffer();
            }
            finally
            {
                if (backgroundBrush != null)
                {
                    backgroundBrush.Dispose();
                }
                if (textBrush != null)
                {
                    textBrush.Dispose();
                }
                for (int br = 0; br < circleBrush.Length; br++)
                {
                    if (circleBrush[br] != null)
                    {
                        circleBrush[br].Dispose();
                    }
                }
                if (font != null)
                {
                    font.Dispose();
                }
                if (path != null)
                {
                    path.Dispose();
                }
                if (g != null)
                {
                    g.Dispose();
                }

                bmp.Dispose();

                if (cloneBmp != null)
                {
                    cloneBmp.Dispose();
                }
            }
        }
    }

    /// <summary>
    /// referenced from web.config for displaying an image
    /// </summary>
    public class CaptchaTypeHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString["encryptedvalue"] == null)
            {
                // likely, this is in design mode and we want to just show the
                // default image for type 2.
                //Assembly assembly = Assembly.GetExecutingAssembly();
                //Stream imgStream =
                //    assembly.GetManifestResourceStream("CaptchaUltimateCustomControl.Images.CaptchaType2.gif");

                Stream imgStream = null;
                try
                {
                    string fileLocation = context.Server.MapPath("~/Images/ignore.bmp");
                    imgStream = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }


                if (imgStream != null)
                {
                    var ba = new byte[imgStream.Length];
                    imgStream.Read(ba, 0, Convert.ToInt32(imgStream.Length));
                    context.Response.ContentType = "image/bmp";
                    context.Response.BinaryWrite(ba);
                }
            }
            else
            {
                // this is live
                string encryptedValue = ConvertQueryString(context, "encryptedvalue", string.Empty);
                CaptchaImageUtils.Decrypt(encryptedValue, CaptchaImageUtils.SymetricKey, true);
                var cp = new CaptchaParams(
                    CaptchaImageUtils.Decrypt(encryptedValue, CaptchaImageUtils.SymetricKey, true),
                    ConvertQueryString(context, "encryptedvalue", string.Empty),
                    ConvertQueryStringToInt(context, "heightcaptchapixels", 50),
                    ConvertQueryStringToInt(context, "widthcaptchapixels", 140),
                    ConvertQueryStringToInt(context, "captchatype", 2),
                    ConvertQueryString(context, "fontfamilystring", "Courier New")
                    );

                byte[] ba = CaptchaImageUtils.GetImageCaptcha(cp);
                context.Response.BinaryWrite(ba);
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }

        #endregion

        private int ConvertQueryStringToInt(HttpContext context, string queryString, int defaultValue)
        {
            int result = defaultValue;
            if (context.Request.QueryString[queryString] != null)
            {
                string retString = context.Request.QueryString[queryString];
                Int32.TryParse(retString, out result);
            }
            return result;
            ;
        }

        private string ConvertQueryString(HttpContext context, string queryString, string defaultValue)
        {
            string retString = defaultValue;
            if (context.Request.QueryString[queryString] != null)
            {
                retString = context.Request.QueryString[queryString];
            }
            return retString;
        }
    }
}