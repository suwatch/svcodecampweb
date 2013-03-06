using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeCampSV;
using aspNetEmail;

namespace WebAPI.Api
{
    public class EmailSendDetail
    {
        public string PreviewEmailSend { get; set; }
        public string EmailUrl { get; set; }
        public string Subject { get; set; }
        public string SqlStatement { get; set; }
        public string EmailHtml { get; set; }
    }

    public class EmailController : ApiController
    {

        [HttpPost]
        [ActionName("EmailPreview")]
        public HttpResponseMessage PostEmailPreview(EmailSendDetail emailSendDetail)
        {
            string url = emailSendDetail.EmailUrl ?? "http://pkellner.site44.com/";
            string sendTo = emailSendDetail.PreviewEmailSend;
            HtmlUtility utility = new HtmlUtility();


            //convert linked css sheets to inline <style> content
            utility.CssOption = CssOption.EmbedLinkedSheets;


            utility.LoadUrl(url);

            //set the UrlContent base
            utility.SetUrlContentBase = true;

            //set the basetag in the html
            utility.SetHtmlBaseTag = true;

            //embed the images
            utility.EmbedImageOption = EmbedImageOption.ContentLocation;

            //render the Html so it is properly formatted for email
            utility.Render();

            //create an EmailMessage with appropriate text and html parts
            EmailMessage email = new EmailMessage(true, false);

            //generate the parts
            MimeBodyPart textPart = utility.ToPlainTextPart();
            MimeBodyPart htmlPart = utility.ToHtmlPart();

            email.Subject = "Html Utility Test";
            email.To = sendTo;

            //add the parts and the embedded images
            email.AddMimeBodyPart(textPart);
            email.AddMimeBodyPart(htmlPart);
            email.EmbedObject(utility.EmbeddedImages);



            HttpResponseMessage httpResponseMessage;
            if (email.Send())
            {
                httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                httpResponseMessage =
                    Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                email.LastException().Message);
                //if an error occurred, write it out to the screen
                //litMsg.Text = "<font color=#FF0033>The following error occurred while sending the email: " + msg.LastException().Message + "</font><br><br>";
                //litMsg.Text += Server.HtmlEncode( msg.GetLog() ).Replace("\r\n", "<br>");
            }

            return httpResponseMessage;



            //EmailMessage email = new EmailMessage(true, false);
            //email.To = emailSendDetail.PreviewEmailSend;

            //email.Subject = "Html Utility test";

            //HtmlUtility utility = new HtmlUtility();
            //string url = "http://www.google.com";
            //utility.LoadUrl(url);

            ////set the UrlContent base
            //utility.SetUrlContentBase = true;

            ////set the basetag in the html
            //utility.SetHtmlBaseTag = true;

            ////render the Html so it is properly formatted for email
            //utility.Render();

            ////populate the EmailMessage with RawHtmlContent loaded by LoadUrl()
            //email = utility.ToEmailMessage(utility.RawHtmlContent, email);

            //HttpResponseMessage httpResponseMessage;
            //if (email.Send())
            //{
            //    httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            //}
            //else
            //{
            //    httpResponseMessage =
            //        Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
            //                                    email.LastException().Message);
            //    //if an error occurred, write it out to the screen
            //    //litMsg.Text = "<font color=#FF0033>The following error occurred while sending the email: " + msg.LastException().Message + "</font><br><br>";
            //    //litMsg.Text += Server.HtmlEncode( msg.GetLog() ).Replace("\r\n", "<br>");
            //}

            //return httpResponseMessage;



           // var utility = new HtmlUtility();
            
           // string url = emailSendDetail.EmailUrl ?? "http://pkellner.site44.com/";

           // //convert linked css sheets to inline <style> content
           // utility.CssOption = CssOption.EmbedLinkedSheets;


           // utility.LoadUrl(url);

           // //set the UrlContent base
           // utility.SetUrlContentBase = true;

           // //set the basetag in the html
           // utility.SetHtmlBaseTag = true;

           // //embed the images
           // utility.EmbedImageOption = EmbedImageOption.ContentLocation;

           

           // //render the Html so it is properly formatted for email
           // utility.Render();

           //var xx =   utility.RawHtmlContent;
           // var xxx = utility.RenderedHtmlContent;

           // //create an EmailMessage with appropriate text and html parts
           // EmailMessage email = utility.ToEmailMessage(utility.red

           // //load values from the web.config
           // email.LoadFromConfig();

           // email.Subject = "Html Utility Test";

           // int portNumber = Convert.ToInt32(ConfigurationManager.AppSettings["EmailMessage.Port"].ToString());
           // email.Port = portNumber;

           // if (email.Server.Equals("smtp.gmail.com"))
           // {
           //     var ssl = new AdvancedIntellect.Ssl.SslSocket();
           //     email.LoadSslSocket(ssl);
           //     email.Port = 587;
           // }

           // email.FromAddress = "service2012@siliconvalley-codecamp.com";
           // email.To = emailSendDetail.PreviewEmailSend;
           // email.Subject = emailSendDetail.Subject;
            //msg.Body = emailSendDetail.EmailHtml;

           
            //var emailSendDetails = new List<EmailSendDetail>() {emailSendDetail};

            //DataTable dt = (DataTable)emailSendDetails;
           
            //var query = from data in emailSendDetails
            //            select new
            //                       {
            //                           Subject = data.Subject
            //                       };
            //DataTable table = DataTableExtensions.CopyToDataTable(query);


//            // Create a sequence. 
//            Item[] items = new Item[] 
//{ new Book{Id = 1, Price = 13.50, Genre = "Comedy", Author = "Gustavo Achong"}, 
//  new Book{Id = 2, Price = 8.50, Genre = "Drama", Author = "Jessie Zeng"},
//  new Movie{Id = 1, Price = 22.99, Genre = "Comedy", Director = "Marissa Barnes"},
//  new Movie{Id = 1, Price = 13.40, Genre = "Action", Director = "Emmanuel Fernandez"}};

//            // Query for items with price greater than 9.99.
//            var query = from i in items
//                        where i.Price > 9.99
//                        orderby i.Price
//                        select i;

//            // Load the query results into new DataTable.
//            DataTable table = query.CopyToDataTable();


           

          



        }


        ///// <summary>
        ///// http://stackoverflow.com/questions/564366/convert-generic-list-enumerable-to-datatable
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //public static DataTable ToDataTable<T>(IList<T> data)
        //{
        //    PropertyDescriptorCollection properties =
        //        TypeDescriptor.GetProperties(typeof(T));
        //    DataTable table = new DataTable();
        //    foreach (PropertyDescriptor prop in properties)
        //        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        //    foreach (T item in data)
        //    {
        //        DataRow row = table.NewRow();
        //        foreach (PropertyDescriptor prop in properties)
        //            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        //        table.Rows.Add(row);
        //    }
        //    return table;
        //}


    }




}
