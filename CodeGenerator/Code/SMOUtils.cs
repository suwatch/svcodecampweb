using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace ThreePLogicAccessCodeGen.Code
{
    public static class SMOUtils
    {
        //private const string Catalog = "3plogic";
        //private const string Password = "Zebra99";
        //private const string SystemName = "W500DEV";
        //private const string Username = "sa";

        /// <summary>
        /// gets attributes of all tables
        /// </summary>
        /// <param name="tableNameToFind">if empty, get all tables, otherwise, just tablename specified</param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static List<FullMeta> TableAttributesList(string tableNameToFind, string connectionString)
        {
            var results = new List<FullMeta>();

            // Pull out of connectionString the Catalog
            // Catalog=3plogic;
            const string catalogSearchString = "Catalog=";
            int pos1 = connectionString.IndexOf(catalogSearchString, StringComparison.Ordinal) + catalogSearchString.Length;
            int pos2 = connectionString.Substring(pos1).IndexOf(";", StringComparison.Ordinal) + pos1;
            string catalogName = connectionString.Substring(pos1, pos2 - pos1).ToLower();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var serverConnection =
                    new ServerConnection(sqlConnection);

                var svr = new Server(serverConnection);

                StringCollection sc = svr.GetDefaultInitFields(typeof (Table));
                svr.SetDefaultInitFields(typeof (View), sc);

                DatabaseCollection databaseCollection = svr.Databases;

                if (databaseCollection.Count > 0)
                {
                    foreach (Database database in databaseCollection)
                    {
                        // only process tables in our catalog
                        if (!database.Name.ToLower().Equals(catalogName))
                        {
                            continue;
                        }

                        TableCollection tableCollection = database.Tables;
                        foreach (Table table in tableCollection)
                        {
                            if (table.IsSchemaOwned && !table.IsSystemObject)
                            {
                                try
                                {
                                    if (String.IsNullOrEmpty(tableNameToFind) ||
                                        table.Name.ToLower().Equals(tableNameToFind.ToLower()))
                                    {
                                        string xml = CreateXMLFromTableSqlServer(table.Name, sqlConnection);
                                        List<AttributeInfo> listAttributeInfo = ParseXMLForAttributes(xml);
                                        results.Add(new FullMeta(database.Name, table.Name, listAttributeInfo));
                                    }
                                }
                                catch (Exception)
                                {
                                    // todo:  figure out how to skip tables that are not ours
                                    Debug.WriteLine("problem: " + table.Name);
                                }
                            }
                        }
                    }
                }
            }
            return results;
        }

        private static List<AttributeInfo> ParseXMLForAttributes(string xmlString)
        {
            XElement root;
            try
            {
                TextReader stringReader = new StringReader(xmlString);
                root = XElement.Load(stringReader);
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }

            var listAttributeInfo = new List<AttributeInfo>();

            if (root.HasElements)
            {
                foreach (XElement el in root.Elements())
                {
                    var attributeInfo = new AttributeInfo();
                    if (el.Name.ToString().Equals("SchemaTable"))
                    {
                        if (el.HasElements)
                        {
                            foreach (XElement el1 in el.Elements())
                            {
                                AddAttribute(attributeInfo, el1.Name, el1.Value);
                            }
                        }
                    }
                    listAttributeInfo.Add(attributeInfo);
                }
            }
            return listAttributeInfo;
        }

        private static void AddAttribute(AttributeInfo currentAttributeInfo, XName xNameValue, string valueIn)
        {
            string lastXMLTag = xNameValue.ToString();

            if (lastXMLTag.Equals("ColumnName"))
            {
                currentAttributeInfo.ColumnNameOriginal = valueIn;
            }
            else if (lastXMLTag.Equals("DataType"))
            {
                string dataType = valueIn;
                // get rid of anything after a comma
                currentAttributeInfo.DataType =
                    dataType.IndexOf(",", StringComparison.Ordinal) >= 0
                        ? dataType.Substring(0, dataType.IndexOf(",", StringComparison.Ordinal))
                        : dataType;

               


        }
            else if (lastXMLTag.Equals("ColumnSize"))
            {
                currentAttributeInfo.ColumnSize = Convert.ToInt32(valueIn);
            }
            else if (lastXMLTag.Equals("IsUnique"))
            {
                currentAttributeInfo.IsUnique = valueIn.ToUpper().Equals("TRUE") ? true : false;
            }
            else if (lastXMLTag.Equals("IsKey"))
            {
                currentAttributeInfo.IsKey = valueIn.ToUpper().Equals("TRUE") ? true : false;
            }
            else if (lastXMLTag.Equals("AllowDBNull"))
            {
                currentAttributeInfo.AllowDBNull = valueIn.ToUpper().Equals("TRUE") ? true : false;
            }
            else if (lastXMLTag.Equals("IsAutoIncrement"))
            {
                currentAttributeInfo.IsAutoIncrement = valueIn.ToUpper().Equals("TRUE") ? true : false;
            }
        }


        private static string CreateXMLFromTableSqlServer(string tableName, SqlConnection sqlConnection)
        {
            DataTable dt = null;
            string sqlString = string.Format("SELECT * FROM [dbo].[{0}]", tableName);
            using (var cmd = new SqlCommand(sqlString, sqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader != null) dt = reader.GetSchemaTable();
                }
            }


            var ba = new byte[1];
            if (dt != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var strmWrite = new StreamWriter(memoryStream))
                    {
                        dt.WriteXml(strmWrite);
                        ba = new byte[(int) memoryStream.Length];
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        memoryStream.Read(ba, 0, (int) memoryStream.Length);
                    }
                }
                // for debug only convert to string
            }
            return Encoding.ASCII.GetString(ba);
        }
    }
}

/*
<DocumentElement>
  <SchemaTable>
    <ColumnName>Id</ColumnName>
    <ColumnOrdinal>0</ColumnOrdinal>
    <ColumnSize>4</ColumnSize>
    <NumericPrecision>10</NumericPrecision>
    <NumericScale>255</NumericScale>
    <IsUnique>false</IsUnique>
    <BaseColumnName>Id</BaseColumnName>
    <DataType>System.Int32, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</DataType>
    <AllowDBNull>false</AllowDBNull>
    <ProviderType>8</ProviderType>
    <IsIdentity>true</IsIdentity>
    <IsAutoIncrement>true</IsAutoIncrement>
    <IsRowVersion>false</IsRowVersion>
    <IsLong>false</IsLong>
    <IsReadOnly>true</IsReadOnly>
    <ProviderSpecificDataType>System.Data.SqlTypes.SqlInt32, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ProviderSpecificDataType>
    <DataTypeName>int</DataTypeName>
    <NonVersionedProviderType>8</NonVersionedProviderType>
    <IsColumnSet>false</IsColumnSet>
  </SchemaTable>
  <SchemaTable>
    <ColumnName>Line1</ColumnName>
    <ColumnOrdinal>1</ColumnOrdinal>
    <ColumnSize>128</ColumnSize>
    <NumericPrecision>255</NumericPrecision>
    <NumericScale>255</NumericScale>
    <IsUnique>false</IsUnique>
    <BaseColumnName>Line1</BaseColumnName>
    <DataType>System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</DataType>
    <AllowDBNull>true</AllowDBNull>
    <ProviderType>12</ProviderType>
    <IsIdentity>false</IsIdentity>
    <IsAutoIncrement>false</IsAutoIncrement>
    <IsRowVersion>false</IsRowVersion>
    <IsLong>false</IsLong>
    <IsReadOnly>false</IsReadOnly>
    <ProviderSpecificDataType>System.Data.SqlTypes.SqlString, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ProviderSpecificDataType>
    <DataTypeName>nvarchar</DataTypeName>
    <NonVersionedProviderType>12</NonVersionedProviderType>
    <IsColumnSet>false</IsColumnSet>
  </SchemaTable>
  <SchemaTable>
    <ColumnName>Line2</ColumnName>
    <ColumnOrdinal>2</ColumnOrdinal>
    <ColumnSize>128</ColumnSize>
    <NumericPrecision>255</NumericPrecision>
    <NumericScale>255</NumericScale>
    <IsUnique>false</IsUnique>
    <BaseColumnName>Line2</BaseColumnName>
    <DataType>System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</DataType>
    <AllowDBNull>true</AllowDBNull>
    <ProviderType>12</ProviderType>
    <IsIdentity>false</IsIdentity>
    <IsAutoIncrement>false</IsAutoIncrement>
    <IsRowVersion>false</IsRowVersion>
    <IsLong>false</IsLong>
    <IsReadOnly>false</IsReadOnly>
    <ProviderSpecificDataType>System.Data.SqlTypes.SqlString, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ProviderSpecificDataType>
    <DataTypeName>nvarchar</DataTypeName>
    <NonVersionedProviderType>12</NonVersionedProviderType>
    <IsColumnSet>false</IsColumnSet>
  </SchemaTable>
  <SchemaTable>
    <ColumnName>City</ColumnName>
    <ColumnOrdinal>3</ColumnOrdinal>
    <ColumnSize>128</ColumnSize>
    <NumericPrecision>255</NumericPrecision>
    <NumericScale>255</NumericScale>
    <IsUnique>false</IsUnique>
    <BaseColumnName>City</BaseColumnName>
    <DataType>System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</DataType>
    <AllowDBNull>true</AllowDBNull>
    <ProviderType>12</ProviderType>
    <IsIdentity>false</IsIdentity>
    <IsAutoIncrement>false</IsAutoIncrement>
    <IsRowVersion>false</IsRowVersion>
    <IsLong>false</IsLong>
    <IsReadOnly>false</IsReadOnly>
    <ProviderSpecificDataType>System.Data.SqlTypes.SqlString, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ProviderSpecificDataType>
    <DataTypeName>nvarchar</DataTypeName>
    <NonVersionedProviderType>12</NonVersionedProviderType>
    <IsColumnSet>false</IsColumnSet>
  </SchemaTable>
  <SchemaTable>
    <ColumnName>State</ColumnName>
    <ColumnOrdinal>4</ColumnOrdinal>
    <ColumnSize>2</ColumnSize>
    <NumericPrecision>255</NumericPrecision>
    <NumericScale>255</NumericScale>
    <IsUnique>false</IsUnique>
    <BaseColumnName>State</BaseColumnName>
    <DataType>System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</DataType>
    <AllowDBNull>true</AllowDBNull>
    <ProviderType>12</ProviderType>
    <IsIdentity>false</IsIdentity>
    <IsAutoIncrement>false</IsAutoIncrement>
    <IsRowVersion>false</IsRowVersion>
    <IsLong>false</IsLong>
    <IsReadOnly>false</IsReadOnly>
    <ProviderSpecificDataType>System.Data.SqlTypes.SqlString, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ProviderSpecificDataType>
    <DataTypeName>nvarchar</DataTypeName>
    <NonVersionedProviderType>12</NonVersionedProviderType>
    <IsColumnSet>false</IsColumnSet>
  </SchemaTable>
  <SchemaTable>
    <ColumnName>Zipcode</ColumnName>
    <ColumnOrdinal>5</ColumnOrdinal>
    <ColumnSize>20</ColumnSize>
    <NumericPrecision>255</NumericPrecision>
    <NumericScale>255</NumericScale>
    <IsUnique>false</IsUnique>
    <BaseColumnName>Zipcode</BaseColumnName>
    <DataType>System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</DataType>
    <AllowDBNull>true</AllowDBNull>
    <ProviderType>12</ProviderType>
    <IsIdentity>false</IsIdentity>
    <IsAutoIncrement>false</IsAutoIncrement>
    <IsRowVersion>false</IsRowVersion>
    <IsLong>false</IsLong>
    <IsReadOnly>false</IsReadOnly>
    <ProviderSpecificDataType>System.Data.SqlTypes.SqlString, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ProviderSpecificDataType>
    <DataTypeName>nvarchar</DataTypeName>
    <NonVersionedProviderType>12</NonVersionedProviderType>
    <IsColumnSet>false</IsColumnSet>
  </SchemaTable>
  <SchemaTable>
    <ColumnName>Country</ColumnName>
    <ColumnOrdinal>6</ColumnOrdinal>
    <ColumnSize>128</ColumnSize>
    <NumericPrecision>255</NumericPrecision>
    <NumericScale>255</NumericScale>
    <IsUnique>false</IsUnique>
    <BaseColumnName>Country</BaseColumnName>
    <DataType>System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</DataType>
    <AllowDBNull>true</AllowDBNull>
    <ProviderType>12</ProviderType>
    <IsIdentity>false</IsIdentity>
    <IsAutoIncrement>false</IsAutoIncrement>
    <IsRowVersion>false</IsRowVersion>
    <IsLong>false</IsLong>
    <IsReadOnly>false</IsReadOnly>
    <ProviderSpecificDataType>System.Data.SqlTypes.SqlString, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ProviderSpecificDataType>
    <DataTypeName>nvarchar</DataTypeName>
    <NonVersionedProviderType>12</NonVersionedProviderType>
    <IsColumnSet>false</IsColumnSet>
  </SchemaTable>
  <SchemaTable>
    <ColumnName>Province</ColumnName>
    <ColumnOrdinal>7</ColumnOrdinal>
    <ColumnSize>128</ColumnSize>
    <NumericPrecision>255</NumericPrecision>
    <NumericScale>255</NumericScale>
    <IsUnique>false</IsUnique>
    <BaseColumnName>Province</BaseColumnName>
    <DataType>System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</DataType>
    <AllowDBNull>true</AllowDBNull>
    <ProviderType>12</ProviderType>
    <IsIdentity>false</IsIdentity>
    <IsAutoIncrement>false</IsAutoIncrement>
    <IsRowVersion>false</IsRowVersion>
    <IsLong>false</IsLong>
    <IsReadOnly>false</IsReadOnly>
    <ProviderSpecificDataType>System.Data.SqlTypes.SqlString, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ProviderSpecificDataType>
    <DataTypeName>nvarchar</DataTypeName>
    <NonVersionedProviderType>12</NonVersionedProviderType>
    <IsColumnSet>false</IsColumnSet>
  </SchemaTable>
  <SchemaTable>
    <ColumnName>Note</ColumnName>
    <ColumnOrdinal>8</ColumnOrdinal>
    <ColumnSize>2147483647</ColumnSize>
    <NumericPrecision>255</NumericPrecision>
    <NumericScale>255</NumericScale>
    <IsUnique>false</IsUnique>
    <BaseColumnName>Note</BaseColumnName>
    <DataType>System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</DataType>
    <AllowDBNull>true</AllowDBNull>
    <ProviderType>12</ProviderType>
    <IsIdentity>false</IsIdentity>
    <IsAutoIncrement>false</IsAutoIncrement>
    <IsRowVersion>false</IsRowVersion>
    <IsLong>true</IsLong>
    <IsReadOnly>false</IsReadOnly>
    <ProviderSpecificDataType>System.Data.SqlTypes.SqlString, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ProviderSpecificDataType>
    <DataTypeName>nvarchar</DataTypeName>
    <NonVersionedProviderType>12</NonVersionedProviderType>
    <IsColumnSet>false</IsColumnSet>
  </SchemaTable>
</DocumentElement>
*/