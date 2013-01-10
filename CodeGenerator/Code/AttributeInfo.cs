using System.Collections.Generic;
using System.Text;

namespace ThreePLogicAccessCodeGen.Code
{
    public class StringClass
    {
        public StringClass(string s)
        {
            StringValue = s;
        }

        public string StringValue { get; set; }
    }


    /// <summary>
    /// Summary description for AttributeInfo
    /// </summary>
    public class AttributeInfo
    {
        public bool AllowDBNull;
        public string ColumnNameOriginal;
        public int ColumnSize;
        public string csGetterName;
        public string csNullValue;
        public string CsType; // type from c#

        public string CsTypeIsNullable;
                      // include ? if required (like bool?, not string?) (does not apply if column is nullable)

        public string DatabaseDataType; // derived datatype for mysql
        private string dataType;
        public string DefaultSortColumn;
        public bool IsAutoIncrement;

        public bool isBinaryData;
                    // Will create a separate (not default) update and insert method for this binary field.  

        public bool isComputedColumn;
        // this means doesn't update the database but is a property and is in updater and inserter and getter

        public bool IsKey;
        public bool IsReadOnly;
        public bool IsSortable;
        public bool IsUnique;
        public int NumericPrecision;
        public int NumericScale;
        public List<StringClass> sqlServerDataTypeList;
        public string TypeOfDatabase;

        public AttributeInfo()
        {
            ColumnNameOriginal = "";
            ColumnSize = 0;
            NumericPrecision = 0;
            NumericScale = 0;
            IsUnique = false;
            IsReadOnly = false;
            AllowDBNull = true;
            IsAutoIncrement = false;
            DataType = "";
            IsKey = false;
            IsSortable = true;
            DefaultSortColumn = "";
            isBinaryData = false;
            isComputedColumn = false;

            sqlServerDataTypeList = new List<StringClass>();
            sqlServerDataTypeList.Add(new StringClass("System.Byte"));
            sqlServerDataTypeList.Add(new StringClass("System.Byte[]"));
            sqlServerDataTypeList.Add(new StringClass("System.Guid"));
            sqlServerDataTypeList.Add(new StringClass("System.Int32"));
            sqlServerDataTypeList.Add(new StringClass("System.Boolean"));
            sqlServerDataTypeList.Add(new StringClass("System.DateTime"));
            sqlServerDataTypeList.Add(new StringClass("System.Double"));
            sqlServerDataTypeList.Add(new StringClass("System.String"));
        }

        // defaults will not include.  This is because it breaks the visual model that goes to the gridview
        // (editing a picture doesn't make sense.  Maybe sometime later make fancy javascript thing to populate
        //  a column with a picture or something.  For now, will have to be handled out of visual view)


        //
        // Setters and Getters Below
        //


        public string DataType
        {
            get { return dataType; }
            set
            {
                dataType = value;
                // System.Data.SqlTypes.SqlBinary
                if (dataType.StartsWith("System.Byte[]") && dataType.Length == 13)
                {
                    //DatabaseDataType = "Image";
                    //CsType = "SqlBytes";
                    //csGetterName = "GetSqlBytes";
                    //csNullValue = "new SqlBytes()";
                    //isBinaryData = true;
                    //IsSortable = false;
                    //CsTypeIsNullable = CsType;
                    DatabaseDataType = "Image";
                    CsType = "System.Data.Linq.Binary";
                    csGetterName = "System.Data.Linq.Binary";
                    csNullValue = "new System.Data.Linq.Binary()";
                    isBinaryData = true;
                    IsSortable = false;
                    CsTypeIsNullable = CsType;

                    // System.Data.Linq.Binary
                }

                if (dataType.StartsWith("System.Byte") && dataType.Length == 11)
                {
                    DatabaseDataType = "Int";
                    CsType = "int";
                    csGetterName = "GetInt32";
                    csNullValue = "0";
                    isBinaryData = false;
                    IsSortable = true;
                    CsTypeIsNullable = CsType + "?";
                }

                if (dataType.StartsWith("System.Guid"))
                {
                    DatabaseDataType = "UniqueIdentifier";
                    CsType = "Guid";
                    csGetterName = "GetGuid";
                    IsSortable = false;
                    csNullValue = "Guid.NewGuid()";
                    CsTypeIsNullable = CsType + "?";
                }

                if (dataType.StartsWith("System.Int64") && dataType.Length == 12)
                {
                    DatabaseDataType = "Int";
                    CsType = "Int64";
                    csGetterName = "GetInt64";
                    csNullValue = "0";
                    CsTypeIsNullable = CsType + "?";
                }

                if (dataType.StartsWith("System.Int32") && dataType.Length == 12)
                {
                    DatabaseDataType = "Int";
                    CsType = "int";
                    csGetterName = "GetInt32";
                    csNullValue = "0";
                    CsTypeIsNullable = CsType + "?";
                }

                if (dataType.StartsWith("System.Int16") && dataType.Length == 12)
                {
                    DatabaseDataType = "Int";
                    CsType = "short";
                    csGetterName = "GetInt16";
                    csNullValue = "(short) 0";
                    CsTypeIsNullable = CsType + "?";
                }

                else if (dataType.StartsWith("System.Boolean"))
                {
                    DatabaseDataType = "Bit";
                    CsType = "bool";
                    csGetterName = "GetBoolean";
                    csNullValue = "false";
                    CsTypeIsNullable = CsType + "?";
                }
                else if (dataType.StartsWith("System.DateTime"))
                {
                    DatabaseDataType = "DateTime";
                    CsType = "DateTime";
                    csGetterName = "GetDateTime";
                    csNullValue = "DateTime.Now";
                    CsTypeIsNullable = CsType + "?";
                }
                else if (dataType.StartsWith("System.Double"))
                {
                    DatabaseDataType = "Float";
                    CsType = "Double";
                    csGetterName = "GetDouble";
                    csNullValue = "0.0";
                    CsTypeIsNullable = CsType + "?";
                }
                else if (dataType.StartsWith("System.Single"))
                {
                    DatabaseDataType = "Float";
                    CsType = "float";
                    csGetterName = "GetFloat";
                    csNullValue = "(float) 0.0";
                    CsTypeIsNullable = CsType + "?";
                }
                else if (dataType.StartsWith("System.Decimal"))
                {
                    DatabaseDataType = "Decimal";
                    CsType = "decimal";
                    csGetterName = "GetDecimal";
                    csNullValue = "(decimal) 0.0";
                    CsTypeIsNullable = CsType + "?";
                }
                else if (dataType.StartsWith("System.String"))
                {
                    // don't know about this,need to test...
                    if (ColumnSize > 255)
                    {
                        DatabaseDataType = "VarChar";
                    }
                    else
                    {
                        DatabaseDataType = "VarChar";
                    }
                    CsType = "string";
                    csGetterName = "GetString";
                    csNullValue = "\"\"";
                    CsTypeIsNullable = CsType;
                }
            }
        }

        public string ColumnName
        {
            get
            {
                // make column name lowercase for purposes of ODS and remove non digits and underscore
                var sb = new StringBuilder();
                foreach (char c in ColumnNameOriginal)
                {
                    if (char.IsLetterOrDigit(c) || c == '_')
                    {
                        sb.Append(c);
                    }
                }
                string retString = sb.ToString();

                return retString.ToLower();
            }
        }
    }
}