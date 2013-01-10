using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ThreePLogicAccessCodeGen.Code
{
    public class BigProcessing
    {
        public void DoAllTheWork(
            string tableToProcess,
            string exclusionFileList,
            bool overWriteFilesAutoGen,
            bool overWriteFilesBase,
            string baseDir,
            string connectionString,
            string fullNameSpace,
            string usingEntityName,
            string dataContextPrefix,
            string enumAssemblyName)
        {
            List<string> TablesToExclude = ReadExclusionFileList(exclusionFileList);
            string baseDirAutoGen = baseDir + "AutoGen/";

            Console.WriteLine("AutoGen Directory: " + baseDirAutoGen + " Overwrite files: " + overWriteFilesAutoGen);
            Console.WriteLine("Base Directory: " + baseDir + " Overwrite Files: " + overWriteFilesBase);

            var enumClasses = new List<string>();
            if (!String.IsNullOrEmpty(enumAssemblyName))
            {
                enumClasses = GetListOfEnumClasses(enumAssemblyName);
            }

            if (String.IsNullOrEmpty(connectionString))
            {
                throw new ApplicationException("Connection String in App.Config not set");
            }

            List<FullMeta> fullMetaList = SMOUtils.TableAttributesList(tableToProcess, connectionString);
            foreach (FullMeta fullM in fullMetaList)
            {
                fullM.DataContextPrefix = dataContextPrefix;
                fullM.NameSpace = fullNameSpace;
                fullM.entityName = usingEntityName;
            }

            var enumsSkipped = new List<string>();
            var excludedSkipped = new List<string>();

            int cntProcessed = 0;
            Console.WriteLine("------Tables with Code Generated----------");
            foreach (FullMeta fullMetal in fullMetaList)
            {
                if (enumClasses.Contains(fullMetal.TableName))
                {
                    enumsSkipped.Add(fullMetal.TableName);
                }
                else if (TablesToExclude.Contains(fullMetal.TableName))
                {
                    excludedSkipped.Add(fullMetal.TableName);
                }
                else
                {
                    // AutoGen Directory
                    string dataManagerStringAutoGen = ProcessTemplate.GenerateNewClass("DataManagerAutoGen", fullMetal,
                                                                                       true);
                    string dataResultStringAutoGen = ProcessTemplate.GenerateNewClass("DataResultAutoGen", fullMetal,
                                                                                      true);
                    string dataQueryStringAutoGen = ProcessTemplate.GenerateNewClass("DataQueryAutoGen", fullMetal, true);

                    // Files that are meant to be used as starting points, not regenerated once created the first time
                    string dataResultString = ProcessTemplate.GenerateNewClass("DataResult", fullMetal, true);
                    string dataQueryString = ProcessTemplate.GenerateNewClass("DataQuery", fullMetal, true);
                    string dataManagerString = ProcessTemplate.GenerateNewClass("DataManager", fullMetal, true);

                    bool wroteFileAutoGen1 = WriteFile(baseDirAutoGen, "{0}Manager.cs", fullMetal.TableName,
                                                       dataManagerStringAutoGen,
                                                       overWriteFilesAutoGen);
                    bool wroteFileAutoGen2 = WriteFile(baseDirAutoGen, "{0}Result.cs", fullMetal.TableName,
                                                       dataResultStringAutoGen,
                                                       overWriteFilesAutoGen);
                    bool wroteFileAutoGen3 = WriteFile(baseDirAutoGen, "{0}Query.cs", fullMetal.TableName,
                                                       dataQueryStringAutoGen,
                                                       overWriteFilesAutoGen);


                    bool wroteFile1 = WriteFile(baseDir, "{0}Result.cs", fullMetal.TableName, dataResultString,
                                                overWriteFilesBase);
                    bool wroteFile2 = WriteFile(baseDir, "{0}Query.cs", fullMetal.TableName, dataQueryString,
                                                overWriteFilesBase);
                    bool wroteFile3 = WriteFile(baseDir, "{0}Manager.cs", fullMetal.TableName, dataManagerString,
                                                overWriteFilesBase);

                    bool fileWritten = wroteFile1 || wroteFile2 || wroteFile3 ? true : false;
                    bool autoGenFileWritten = wroteFileAutoGen1 || wroteFileAutoGen2 || wroteFileAutoGen3 ? true : false;

                    if (autoGenFileWritten || fileWritten)
                    {
                        cntProcessed++;
                        Console.WriteLine("{0} overwrite AutoGen {1} overwrite Base {2}", fullMetal.TableName,
                                          autoGenFileWritten, fileWritten);
                    }
                }
            }

            Console.WriteLine("------Enums Skipped----------");
            foreach (string str in enumsSkipped)
            {
                Console.WriteLine("Skipping Enum Table {0}", str);
            }
            Console.WriteLine("------Exclusions Skipped----------");
            foreach (string str in excludedSkipped)
            {
                Console.WriteLine("Skipping Enum Table {0}", str);
            }

            Console.WriteLine("------DONE----------");
            Console.WriteLine("Generated Tables {0}, Enums {1}, Excluded {2}", cntProcessed, enumsSkipped.Count,
                              excludedSkipped.Count);
        }

        /// <summary>
        /// Read list of files to exclude and spit out list of filenames.  Parse comas, spaces, etc.
        /// </summary>
        /// <param name="exclusionFileList"></param>
        /// <returns></returns>
        private static List<string> ReadExclusionFileList(string exclusionFileList)
        {
            var returnNames = new List<string>();

            try
            {
                if (File.Exists(exclusionFileList))
                {
                    using (TextReader textReader = new StreamReader(exclusionFileList))
                    {
                        string input;
                        while ((input = textReader.ReadLine()) != null)
                        {
                            var charList = new[] {' ', ',', ';'};
                            string[] names = input.Split(charList);
                            foreach (string name in names)
                            {
                                if (!returnNames.Contains(name) && !String.IsNullOrEmpty(name))
                                {
                                    returnNames.Add(name);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Problem Reading Exclusion File List: " + exception);
            }
            return returnNames;
        }

        private static List<string> GetListOfEnumClasses(string enumAssembly)
        {
            var enumNameList = new List<string>();
            Assembly assembly = Assembly.LoadFrom(enumAssembly); // "ThreePLogic.Common.Enums.dll");
            Type[] typesList = assembly.GetTypes();
            foreach (Type myType in typesList)
            {
                string enumName = myType.FullName.Substring(myType.FullName.LastIndexOf(".") + 1);
                enumNameList.Add(enumName);
            }
            return enumNameList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseDir"></param>
        /// <param name="fileNameTemplate"></param>
        /// <param name="tableName"></param>
        /// <param name="fileData"></param>
        /// <param name="overWrite"></param>
        /// <returns>if true, it actually overwrite the file</returns>
        private static bool WriteFile(string baseDir, string fileNameTemplate, string tableName,
                                      string fileData, bool overWrite)
        {
            bool wroteFile = false;
            try
            {
                string path = baseDir + String.Format(fileNameTemplate, tableName);
                if (overWrite || !File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.Write(fileData);
                        wroteFile = true;
                    }
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
            return wroteFile;
        }
    }
}