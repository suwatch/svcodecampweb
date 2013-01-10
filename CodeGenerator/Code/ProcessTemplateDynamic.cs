using System;
using System.Collections.Generic;
using System.Reflection;

namespace ThreePLogicAccessCodeGen.Code
{
    public class ProcessTemplateDynamic
    {

       
        public void Template38(ICollection<string> newLines, string indentString, FullMeta fullMeta)
        {
            // IQueryable<AttendeesResult> results = GetBaseResultIQueryable(baseQuery);
            const string strTemplate =
                "IQueryable<{0}Result> results = GetBaseResultIQueryable(baseQuery);";
            newLines.Add(indentString + String.Format(strTemplate, fullMeta.TableName));
        }

        public void Template37(ICollection<string> newLines, string indentString, FullMeta fullMeta)
        {
            // public IQueryable<AttendeesResult> GetBaseResultIQueryable(IQueryable<Attendees> baseQuery)
            const string strTemplate =
                "public IQueryable<{0}Result> GetBaseResultIQueryable(IQueryable<{0}> baseQuery)";
            newLines.Add(indentString + String.Format(strTemplate, fullMeta.TableName));
        }

        public void Template36(ICollection<string> newLines, string indentString, FullMeta fullMeta)
        {
            // string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table {0} : Method {1}","ToDo:TableName" info.Name);
            const string strTemplate =
                "string errorMessage = String.Format(\"Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: {0} QueryColumnProblem: {{0}}\",info.Name);";
            newLines.Add(indentString + String.Format(strTemplate, fullMeta.TableName));
        }

        public void Template35(ICollection<string> newLines, string indentString, FullMeta fullMeta)
        {
            // foreach (var info in typeof (SessionsQuery).GetProperties())
            const string strTemplate = "foreach (var info in typeof ({0}Query).GetProperties())";
            newLines.Add(indentString + String.Format(strTemplate, fullMeta.TableName));
        }

        public void Template34(ICollection<string> newLines, string indentString, FullMeta fullMeta)
        {
            // public List<LoadResult> GetJustBaseTableColumns(LoadQuery query)
            const string strTemplate = "public List<{0}Result> GetJustBaseTableColumns({0}Query query)";
            newLines.Add(indentString + String.Format(strTemplate, fullMeta.TableName));
        }

        public void Template33(ICollection<string> newLines, string indentString, FullMeta fullMeta)
        {
            // var meta = new ThreePLogicDataContext();
            const string strTemplate = "var meta = new {0}DataContext();";
            newLines.Add(indentString + String.Format(strTemplate, fullMeta.DataContextPrefix));
        }

        public void Template32(ICollection<string> newLines, string indentString, FullMeta fullMeta)
        {
            const string strTemplate = "using {0};";
            newLines.Add(indentString + String.Format(strTemplate, fullMeta.entityName));
        }

        public void Template31(ICollection<string> newLines, string indentString, FullMeta fullMeta)
        {
            const string strTemplate = "namespace {0}";
            newLines.Add(indentString + String.Format(strTemplate, fullMeta.NameSpace));
        }

        public void Template30(ICollection<string> newLines, string indentString, FullMeta fullMeta)
        {
            //if (query.EffectiveDate != null) baseQuery = baseQuery.Where(a => a.EffectiveDate.CompareTo(query.EffectiveDate) == 0);
            //if (query.CompanyId != null) baseQuery = baseQuery.Where(a => a.CompanyId == query.CompanyId);
            // MUST make sure a.{0} which comes from link, {0} begins with capital letter. NOT query.{1} where {1} comes from table definition

            if (fullMeta.AttributeList.Count > 0)
            {
                foreach (AttributeInfo attr in fullMeta.AttributeList)
                {
                    if (!attr.ColumnName.Equals("id"))
                    {
                        if (attr.CsType.Equals("DateTime"))
                        {
                            //if (query.EffectiveDate != null) baseQuery = baseQuery.Where(a => a.EffectiveDate.CompareTo(query.EffectiveDate) == 0);
                            string showValue = string.Empty;
                            if (attr.AllowDBNull)
                            {
                                showValue = ".Value";
                            }

                            string appendLine =
                                String.Format(
                                    "if (query.{0} != null) baseQuery = baseQuery.Where(a => a.{1}{2}.CompareTo(query.{0}{2}) == 0);",
                                    attr.ColumnNameOriginal, UpperFirstLetter(attr.ColumnNameOriginal), showValue);
                            newLines.Add(indentString + appendLine);
                        }

                        else if (attr.CsType.ToLower().Equals("int") ||
                                 attr.CsType.ToLower().Equals("bool") ||
                                 attr.CsType.ToLower().Equals("bit") ||
                                 attr.CsType.ToLower().Equals("decimal") ||
                                 attr.CsType.ToLower().Equals("double") ||
                                 attr.CsType.ToLower().Equals("float"))
                        {
                            // if (query.CompanyId != null) baseQuery.Where(a => a.CompanyId == query.CompanyId);
                            string appendLine =
                                String.Format(
                                    "if (query.{0} != null) baseQuery = baseQuery.Where(a => a.{1} == query.{0});",
                                    attr.ColumnNameOriginal, UpperFirstLetter(attr.ColumnNameOriginal));
                            newLines.Add(indentString + appendLine);
                        }
                        else if (attr.CsType.ToLower().Equals("string"))
                        {
                            // Hmm, winged this one
                            string appendLine =
                                String.Format(
                                    "if (query.{0} != null) baseQuery = baseQuery.Where(a => a.{1}.ToLower().Equals(query.{0}.ToLower()));",
                                    attr.ColumnNameOriginal, UpperFirstLetter(attr.ColumnNameOriginal));
                            newLines.Add(indentString + appendLine);
                        }
                    }
                }
            }
        }


        public void Template29(ICollection<string> newLines, string indentString, FullMeta fullMeta)
        {
            if (fullMeta.AttributeList.Count > 0)
            {
                foreach (AttributeInfo attr in fullMeta.AttributeList)
                {
                    if (!attr.ColumnName.Equals("id"))
                    {
                        string appendLine1 = "[AutoGenColumn]";
                        newLines.Add(indentString + appendLine1);

                        //public DateTime? EffectiveDate { get; set; }
                        string appendLine2 = String.Format("public {0} {1} {{ get; set; }}",
                                                           attr.CsTypeIsNullable, attr.ColumnNameOriginal);
                        newLines.Add(indentString + appendLine2);
                    }
                }
            }
        }

        public void Template28(ICollection<string> newLines, string indentString, FullMeta fullMetal)
        {
            // private static IQueryable<CompanyAuthority> BaseQueryAutoGen(IQueryable<CompanyAuthority> baseQuery, CompanyAuthorityQuery query)
            const string strTemplate =
                "private static IQueryable<{0}> BaseQueryAutoGen(IQueryable<{0}> baseQuery, {0}Query query)";
            newLines.Add(indentString + String.Format(strTemplate, fullMetal.TableName));
        }


        public void Template27(ICollection<string> newLines, string indentString, FullMeta fullMetal)
        {
            // public partial class CompanyAuthorityQuery : QueryBase
            const string strTemplate = "public partial class {0}Query : QueryBase";
            newLines.Add(indentString + String.Format(strTemplate, fullMetal.TableName));
        }


        public void Template26(ICollection<string> newLines, string indentString, FullMeta fullMetal)
        {
            // return Get(new CompanyAuthorityQuery {IsMaterializeResult = true});
            const string strTemplate = "return Get(new {0}Query {{IsMaterializeResult = true}});";
            newLines.Add(indentString + String.Format(strTemplate, fullMetal.TableName));
        }

        public void Template25(ICollection<string> newLines, string indentString, FullMeta fullMetal)
        {
            // public List<CompanyAuthorityResult> GetAll()
            const string strTemplate = "public List<{0}Result> GetAll()";
            newLines.Add(indentString + String.Format(strTemplate, fullMetal.TableName));
        }

        public void Template24(ICollection<string> newLines, string indentString, FullMeta fullMetal)
        {
            // public partial class LoadManager
            const string strTemplate = "public partial class {0}Manager";
            newLines.Add(indentString + String.Format(strTemplate, fullMetal.TableName));
        }

        public void Template23(ICollection<string> newLines, string indentString, FullMeta fullMetal)
        {
            const string strTemplate = "List<{0}Result> resultList = GetFinalResults(results, query);";
            newLines.Add(indentString + String.Format(strTemplate, fullMetal.TableName));
        }


        public void Template22(ICollection<string> newLines, string indentString, FullMeta fullMetal)
        {
            //IQueryable<CompanyResult> results = (from mydata in baseQuery
            //                                     orderby company.Id
            //                                     select new CompanyResult
            //                                                {
            //                                                    Id = mydata.Id,
            //                                                    Name = mydata.Name,
            //                                                    CompanyURL = mydata.CompanyURL,
            //                                                    CompanyNotes = mydata.CompanyNotes,
            //                                                    ActiveFlag = mydata.ActiveFlag,
            //                                                    CompanyStatusId = mydata.CompanyStatusId,
            //                                                    ParentId = mydata.ParentId,
            //                                                    Createdate = myData.Createdate == null ? null :  (DateTime?) new DateTime(myData.Createdate.Value.Ticks,DateTimeKind.Utc),
            //                                                });
            string strTemplate =
                "IQueryable<{0}Result> results = (from myData in baseQuery orderby myData.Id select new {0}Result {{ Id= myData.Id,";
            newLines.Add(indentString + String.Format(strTemplate, fullMetal.TableName));
            var tempList = new List<string>();
            foreach (AttributeInfo attr in fullMetal.AttributeList)
            {
                if (!attr.ColumnName.Equals("id") && !attr.CsType.Equals("DateTime"))
                {
                    // Name = company.Name,
                    strTemplate = "{0} = myData.{0}";
                    tempList.Add(indentString + String.Format(strTemplate, UpperFirstLetter(attr.ColumnNameOriginal)));
                }
                else if (attr.CsType.Equals("DateTime"))
                {
                    // EffectiveDate = new DateTime(myData.EffectiveDate.Value.Ticks,DateTimeKind.Utc)
                    // Createdate = myData.Createdate == null ? null :  (DateTime?) new DateTime(myData.Createdate.Value.Ticks,DateTimeKind.Utc),
                    string showDotValue = string.Empty;
                    if (attr.AllowDBNull)
                    {
                        showDotValue = ".Value";
                    }

                    if (attr.AllowDBNull)
                    {
                        strTemplate =
                            "{0} = myData.{0} == null ? null :  (DateTime?) new DateTime(myData.{0}{1}.Ticks,DateTimeKind.Utc)";
                    }
                    else
                    {
                        strTemplate =
                            "{0} = new DateTime(myData.{0}{1}.Ticks,DateTimeKind.Utc)";
                    }
                    //  OLD: strTemplate = "{0} = new DateTime(myData.{0}{1}.Ticks,DateTimeKind.Utc)";
                    tempList.Add(indentString +
                                 String.Format(strTemplate, UpperFirstLetter(attr.ColumnNameOriginal), showDotValue));
                }
            }
            for (int i = 0; i < tempList.Count; i++)
            {
                string appendComma = string.Empty;
                if (i < tempList.Count - 1)
                {
                    appendComma = ",";
                }
                newLines.Add(indentString + tempList[i] + appendComma);
            }

            newLines.Add(indentString + "});");
        }


        public void Template21(ICollection<string> newLines, string indentString, FullMeta fullMetal)
        {
            // IQueryable<Company> baseQuery = from myData in meta.Company select myData;
            const string strTemplate = "IQueryable<{0}> baseQuery = from myData in meta.{0} select myData;";
            newLines.Add(indentString + String.Format(strTemplate, fullMetal.TableName));
        }

        public void Template20(ICollection<string> newLines, string indentString, FullMeta fullMetal)
        {
            const string strTemplate = "public List<{0}Result> Get({0}Query query)";
            newLines.Add(indentString + String.Format(strTemplate, fullMetal.TableName));
        }

        public void Template19(ICollection<string> newLines, string indentString, FullMeta fullMetal)
        {
            // public partial class LoadQuery
            const string strTemplate = "public partial class {0}Query";
            newLines.Add(indentString + String.Format(strTemplate, fullMetal.TableName));
        }

        public void Template18(ICollection<string> newLines, string indentString, FullMeta fullMetal)
        {
            // public partial class LoadResult
            const string strTemplate = "public partial class {0}Result";
            newLines.Add(indentString + String.Format(strTemplate, fullMetal.TableName));
        }

        public void Template17(ICollection<string> newLines, string indentString, FullMeta fullMetal)
        {
            // return (from r in meta.Load where r.Id == id select r).FirstOrDefault();
            const string strTemplate = "return (from r in meta.{0} where r.Id == id select r).FirstOrDefault();";
            newLines.Add(indentString + String.Format(strTemplate, fullMetal.TableName));
        }


        public void Template16(ICollection<string> newLines, string indentString, FullMeta fullMetal)
        {
            // protected override Load GetEntityById(ThreePLogicDataContext meta, int id)
            const string strTemplate = "protected override {0} GetEntityById({1}DataContext meta, int id)";
            newLines.Add(indentString + String.Format(strTemplate, fullMetal.TableName, fullMetal.DataContextPrefix));
        }


        public void Template15(ICollection<string> newLines, string indentString, FullMeta fullMetal)
        {
            // public partial class LoadManager : ManagerBase<LoadManager, LoadResult, Load, ThreePLogicDataContext>  (14)
            const string strTemplate =
                "public partial class {0}Manager : ManagerBase<{0}Manager, {0}Result, {0}, {1}DataContext>";
            newLines.Add(indentString + String.Format(strTemplate, fullMetal.TableName, fullMetal.DataContextPrefix));
        }

        public void Template14(ICollection<string> newLines, string indentString, FullMeta fullMetal)
        {
            // protected override void ApplyToDataModel(Load record, LoadResult result) (15)
            const string strTemplate = "protected override void ApplyToDataModel({0} record, {0}Result result)";
            newLines.Add(indentString + String.Format(strTemplate, fullMetal.TableName));
        }


        // 
        public void Template13(ICollection<string> newLines, string indentString, FullMeta fullMeta)
        {
            /*
            public int? LoadStatusId { get; set; }
            public int? ShipmentTypeId { get; set; }
            public int? TenderedByCompanyId { get; set; }
            public int? CreatedByCompanyId { get; set; }
            public string LoadReference { get; set; }
            public DateTime? DateCreated { get; set; }
            public double? Price { get; set; }
            public int? PlanId { get; set; }
            */

            if (fullMeta.AttributeList.Count > 0)
            {
                foreach (AttributeInfo attr in fullMeta.AttributeList)
                {
                    if (!attr.ColumnName.Equals("id"))
                    {
                        //public int? PlanId { get; set; }
                        string appendLine = String.Format("[DataMember] public {0} {1} {{ get; set; }}",
                                                          attr.AllowDBNull ? attr.CsTypeIsNullable : attr.CsType,
                                                          UpperFirstLetter(attr.ColumnNameOriginal));
                        newLines.Add(indentString + appendLine);
                    }
                }
            }
        }

        public void Template12(ICollection<string> newLines, string indentString, FullMeta fullMetal)
        {
            //public partial class LoadResult : ResultBase
            const string strTemplate = "public partial class {0}Result : ResultBase";
            newLines.Add(indentString + String.Format(strTemplate, fullMetal.TableName));
        }

        public void Template11(ICollection<string> newLines, string indentString, FullMeta fullMeta)
        {
            //// The columns below are all the columns from the table with the exception of the primary key (Id)
            //record.CreatedByCompanyId = result.CreatedByCompanyId ?? record.CreatedByCompanyId;
            //record.DateCreated = result.DateCreated ?? record.DateCreated;
            //record.LoadStatusId = result.LoadStatusId ?? record.LoadStatusId;
            //record.ShipmentTypeId = result.ShipmentTypeId ?? record.ShipmentTypeId;
            //record.TenderedByCompanyId = result.TenderedByCompanyId ?? record.TenderedByCompanyId;
            //record.LoadReference = result.LoadReference ?? record.LoadReference;
            //record.Price = result.Price ?? record.Price;
            //record.PlanId = result.PlanId ?? record.PlanId;

            if (fullMeta.AttributeList.Count > 0)
            {
                foreach (AttributeInfo attr in fullMeta.AttributeList)
                {
                    if (!attr.ColumnName.Equals("id"))
                    {
                        //string lineTemplate = "record.{0} = result.{0} ?? record.{0};";
                        ////if (attr.AllowDBNull && !attr.DataType.Equals("System.DateTime"))
                        //if (!attr.AllowDBNull) 
                        //{
                        //    lineTemplate = "record.{0} = result.{0};";
                        //}

                        const string lineTemplate = "record.{0} = result.{0};";

                        string appendLine = String.Format(lineTemplate,UpperFirstLetter(attr.ColumnNameOriginal));
                        newLines.Add(indentString + appendLine);
                    }
                }
            }
        }

        internal void DoIt(int iTemplateNumber, ICollection<string> newLines, string indentString, FullMeta fullMetal)
        {
            // Template11(newLines, indentString, fullMetal);
            string templateMethodName = String.Format("Template{0}", iTemplateNumber);
            var userParameters = new object[3];
            userParameters[0] = newLines;
            userParameters[1] = indentString;
            userParameters[2] = fullMetal;

            try
            {
                Type thisType = GetType();
                MethodInfo theMethod = thisType.GetMethod(templateMethodName);

                // that we have in this class that we don't want called.
                if (theMethod == null || (!CheckMethod(theMethod)))
                {
                    throw new ApplicationException(string.Format("[e] Command <{0}> not supported.", templateMethodName));
                }
                // Invoke the Method!
                theMethod.Invoke(this, userParameters);
            }
            catch (ArgumentNullException e)
            {
                // This exception is from the user entering in a null string on the command line
                throw new ApplicationException("[e] Please enter in a non-null string. (" + e.Message + ")");
            }
            catch (TargetParameterCountException e)
            {
                // This exception is thrown when the method is not passed the right number of parameters
                throw new ApplicationException(string.Format("[e] Command <{0}> requires parameters. ({1})",
                                                             templateMethodName, e.Message));
            }
            catch (Exception e)
            {
                // All other exceptions!
                throw new ApplicationException(string.Format("[e] General Exception:\n{0}", e));
            }
        }


        // This method makes sure that the method being called by the
        // user is Public, Not Static and Not Inherited. This is all done
        // by checking properties of the reflection method description
        // that is encapsulated in the MethodInfo object.
        private bool CheckMethod(MethodBase method)
        {
            // Make sure it's public
            if (!method.IsPublic)
            {
                return false;
            }

            // Make sure it's not static
            if (method.IsStatic)
            {
                return false;
            }

            // To check if the method is inherited, we get the Type of the
            // object that the method originates from, and check it against
            // the type for our own DynamicMethods object. If they don't match
            // then the method is from and inherited object.
            if (method.DeclaringType != GetType())
            {
                return false;
            }

            // If we get here then we have passed
            // the test.
            return true;
        }

        // Uppercase Just First Letter
        private static string UpperFirstLetter(string inString)
        {
            string retString = inString;
            if (!String.IsNullOrEmpty(inString))
            {
                if (inString.Length > 0)
                {
                    retString = inString.Substring(0, 1).ToUpper();
                }
                if (inString.Length > 1)
                {
                    retString += inString.Substring(1);
                }
            }
            return retString;
        }
    }
}