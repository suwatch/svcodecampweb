

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
//using Gurock.SmartInspect.LinqToSql;


namespace CodeCampSV
{
    public partial class CodeCampDataContext
    {
        private readonly bool _smartInspectEnabled = (ConfigurationManager.AppSettings["SmartInspectEnabled"] ?? "").Equals("true");
    

        /// <summary>
        /// We override the connection string in the constructer to make sure 
        /// we are always using the right one. 
        /// </summary>
        public CodeCampDataContext()
            : base(string.Empty)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CodeCampSV06"].ToString();
            Connection.ConnectionString = connectionString;

            //if (_smartInspectEnabled)
            //{
            //    Log = new SmartInspectLinqToSqlAdapter();
            //}
        }
    }
}

