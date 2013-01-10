using System.Collections.Generic;

namespace ThreePLogicAccessCodeGen.Code
{
    public class FullMeta
    {
        public FullMeta(string databaseName, string tableName, List<AttributeInfo> attributeList)
        {
            DatabaseName = databaseName;
            TableName = tableName;
            AttributeList = attributeList;
        }

        public string DatabaseName { get; set; }
        public string TableName { get; set; }
        public List<AttributeInfo> AttributeList { get; set; }
        public string NameSpace { get; set; }
        public string entityName { get; set; }
        public string DataContextPrefix { get; set; }
    }
}