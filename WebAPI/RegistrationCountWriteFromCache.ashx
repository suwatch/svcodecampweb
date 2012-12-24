<%@ WebHandler Language="C#" Class="RegistrationCountWriteFromCache" %>

using System;
using System.Web;

public class RegistrationCountWriteFromCache : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");

        var chartKey = context.Request["key"];
        if (chartKey != null)
        {
            //System.Web.UI.DataVisualization.Charting.Chart = new System.Web.UI.DataVisualization.Charting.Chart();
            
            
            
            //var cachedChart = System.Web.UI.DataVisualization.Charting.Chart.GetFromCache(key: chartKey);
            //if (cachedChart == null)
            //{
            //    cachedChart = new System.Web.UI.DataVisualization.Charting.Chart(600, 400);
            //    cachedChart.AddTitle("Cached Chart -- Cached at " + DateTime.Now);
            //    cachedChart.AddSeries(
            //       name: "Employee",
            //       axisLabel: "Name",
            //       xValue: new[] { "Peter", "Andrew", "Julie", "Mary", "Dave" },
            //       yValues: new[] { "2", "6", "4", "5", "3" });
            //    cachedChart.SaveToCache(key: chartKey,
            //       minutesToCache: 2,
            //       slidingExpiration: false);
            //}
            //System.Web.UI.DataVisualization.Charting.Chart.WriteFromCache(chartKey);
        }
        
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}