﻿using System.IO;
using System.Web;

namespace WebAPI.Code
{
    //http://forums.asp.net/t/1579755.aspx/1
    public class GuidTracker : IHttpHandler
    {
        // http://stackoverflow.com/questions/2570633/smallest-filesize-for-transparent-single-pixel-image
        //static readonly byte[] TrackingGif = 
        //{ 0x47, 0x49, 0x46, 0x38, 0x39, 0x61, 0x1, 0x0, 0x1, 0x0, 0x80, 0x0, 0x0, 0xff, 0xff, 0xff, 0x0, 0x0, 0x0, 0x2c, 0x0, 0x0, 0x0, 0x0, 0x1, 0x0, 0x1, 0x0, 0x0, 0x2, 0x2, 0x44, 0x1, 0x0, 0x3b };


        private static readonly byte[] TrackingGif =
            {
                0x47, 0x49, 0x46, 0x38, 0x39, 0x61, 0x01, 0x00, 0x01, 0x00, 0x81, 0x00, 0x00, 0xff, 0xff, 0xff, 0x00,
                0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x21, 0xff, 0x0b, 0x4e, 0x45, 0x54, 0x53, 0x43, 0x41, 0x50, 0x45, 0x32,
                0x2e, 0x30, 0x03,
                0x01, 0x01, 0x00, 0x00, 0x21, 0xf9, 0x04, 0x01, 0x00, 0x00, 0x00, 0x00, 0x2c, 0x00, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x01,
                0x00, 0x00, 0x08, 0x04, 0x00, 0x01, 0x04, 0x04, 0x00, 0x3b
            };

        public void ProcessRequest(HttpContext ctx)
        {
            ctx.Response.StatusCode = 200;
            ctx.Response.ContentType = "image/gif";
            ctx.Response.BinaryWrite(TrackingGif);
        }

        public
            bool IsReusable 
            {
                get
                {
                    return true;
                }
            }
        }
    }