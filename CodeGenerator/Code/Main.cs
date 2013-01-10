using System;
using Plossum.CommandLine;

namespace ThreePLogicAccessCodeGen.Code
{
    /// <summary>
    /// Generate Data Access Code for All Tables Designated
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// If you leave out any command line parameters, you will get the old way of executing with build in parameters.
        /// If you put in paramters, you will get new way.  To see parameters, enter -Help as command line parameter.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static int Main(string[] args)
        {
            var options = new Options();
            var parser = new CommandLineParser(options);
            parser.Parse();

            //Console.WriteLine("Args Length: " + args.Length);

            // If there are any args, use the Plossum Library (see ThirdParty directory) for parsing them

            Console.WriteLine(parser.UsageInfo.GetHeaderAsString(78));
            if (Options.Help)
            {
                Console.WriteLine(parser.UsageInfo.GetOptionsAsString(78));
                return 0;
            }
            if (parser.HasErrors)
            {
                Console.WriteLine(parser.UsageInfo.GetErrorsAsString(78));
                ShowVerboseMessages(options, String.Empty);
                return -1;
            }

            string newConnectionString = string.Empty;
            bool errorFound = false;


            const string connectionStringTrustedTemplate =
                "Data Source={0};Initial Catalog={1};Integrated Security=True";
            const string connectionStringWithUserTemplate =
                "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}";

            if (String.IsNullOrEmpty(options.Server))
            {
                options.Server = ".";
            }

            // connectionString always takes precedence.
            if (!String.IsNullOrEmpty(options.ConnectionString))
            {
                newConnectionString = options.ConnectionString;
            }
            else if (String.IsNullOrEmpty(options.ConnectionString) &&
                     !String.IsNullOrEmpty(options.SqlServerCatalogName)
                     && String.IsNullOrEmpty(options.User) && String.IsNullOrEmpty(options.Password))
            {
                newConnectionString = String.Format(connectionStringTrustedTemplate,
                                                    options.Server,
                                                    options.SqlServerCatalogName);
            }
            else if (String.IsNullOrEmpty(options.ConnectionString) &&
                     !String.IsNullOrEmpty(options.SqlServerCatalogName)
                     && !String.IsNullOrEmpty(options.User) && !String.IsNullOrEmpty(options.Password))
            {
                newConnectionString = String.Format(connectionStringWithUserTemplate,
                                                    options.Server,
                                                    options.SqlServerCatalogName,
                                                    options.User,
                                                    options.Password);
            }
            else
            {
                Console.WriteLine("problem with connection string not complete");
                errorFound = true;
            }

            if (options.Verbose)
            {
                ShowVerboseMessages(options, newConnectionString);
            }


            if (!errorFound)
            {
                if (!options.Practice)
                {
                    var bigProcessing = new BigProcessing();
                    bigProcessing.DoAllTheWork(options.TableToProcess, options.FileExclusionList,
                                               options.OverWriteFilesInAutoGen, options.OverWriteFilesInBaseDirectory,
                                               options.BaseDirectory, newConnectionString, options.AccessNameSpace,
                                               options.EntityNameSpace, options.DataContextPrefix,
                                               options.EnumAssemblyName);
                }
                else
                {
                    Console.WriteLine("No Execution.  Practice Run.  No code built.");
                }
            }
            else
            {
                Console.WriteLine("No Execution.  Error found");
                ShowVerboseMessages(options, newConnectionString);
            }

            return 0;
        }

        private static void ShowVerboseMessages(Options options, string newConnectionString)
        {
            Console.WriteLine("BaseDirectory:                 " + options.BaseDirectory);
            Console.WriteLine("TableToProcess:                " + options.TableToProcess);
            Console.WriteLine("OverWriteFilesInAutoGen:       " + options.OverWriteFilesInAutoGen);
            Console.WriteLine("OverWriteFilesInBaseDirectory: " + options.OverWriteFilesInBaseDirectory);
            Console.WriteLine("FileExclusionList:             " + options.FileExclusionList);
            Console.WriteLine("EntityNameSpace:               " + options.EntityNameSpace);
            Console.WriteLine("AccessNameSpace:               " + options.AccessNameSpace);
            Console.WriteLine("DataContextPrefix:             " + options.DataContextPrefix);
            Console.WriteLine("ConnectionString:              " + options.ConnectionString);
            Console.WriteLine("SqlServerCatalogName:          " + options.SqlServerCatalogName);
            Console.WriteLine("User:                          " + options.User);
            Console.WriteLine("Password:                      " + options.Password);
            Console.WriteLine("Server:                        " + options.Server);
            Console.WriteLine("Practice:                      " + options.Practice);
            Console.WriteLine("EnumAssemblyName:              " + options.EnumAssemblyName);
            Console.WriteLine("NewConnectionString:           " + newConnectionString);
        }

        #region Nested type: Options

        [CommandLineManager(ApplicationName = "ThreePLogicAccessCodeGen", Copyright = "Copyright (c) 3PLogic, Inc.")]
        private class Options
        {
            [CommandLineOption(Description = "Displays this help text")]
            public static bool Help { get; set; }


            [CommandLineOption(Name = "p", Aliases = "practice",
                Description = "Practice Run, Do Not execute actual code builder")]
            public bool Practice { get; set; }

            [CommandLineOption(
                Description = "Produce verbose output")]
            public bool Verbose { get; set; }

            [CommandLineOption(
                Description = "If set, files will be overwritten in Base directory (whether there or not)",
                MinOccurs = 0)]
            public bool OverWriteFilesInBaseDirectory { get; set; }

            [CommandLineOption(
                Description =
                    "If set, files will be overwritten in AutoGen directory off of BaseDirectory always (whether there or not)"
                , MinOccurs = 0)]
            public bool OverWriteFilesInAutoGen { get; set; }


            [CommandLineOption(Description = "Table To Process.  If not specified, all tables are processed",
                MinOccurs = 0, RequireExplicitAssignment = false)]
            public string TableToProcess { get; set; }

            [CommandLineOption(
                Description =
                    "Base Directory to Process Files To.  Normally this would be something like ../../../ThreePLogicAccess/"
                , MinOccurs = 1, RequireExplicitAssignment = true)]
            public string BaseDirectory { get; set; }

            [CommandLineOption(
                Description =
                    "Filename (with relative directory, typically ../../FileExclusionList.text) of comma separated list of tables not to generate files for"
                , MinOccurs = 0)]
            public string FileExclusionList { get; set; }

            [CommandLineOption(Description = "", MinOccurs = 1)]
            public string EntityNameSpace { get; set; }

            [CommandLineOption(Description = "", MinOccurs = 1)]
            public string AccessNameSpace { get; set; }

            [CommandLineOption(
                Description = "This is the prefix of the DataConext generated by LINQ.  Typically ThreePLogic",
                MinOccurs = 1)]
            public string DataContextPrefix { get; set; }

            [CommandLineOption(
                Description =
                    "This is the full connection string that connects to the database.  If not specified, then we will try to connect with local SqlServer Connection using just the Database Name"
                , MinOccurs = 0, Aliases = "Conn")]
            public string ConnectionString { get; set; }

            [CommandLineOption(
                Description =
                    "Username For SqlServer Connection"
                , MinOccurs = 0, Aliases = "u")]
            public string User { get; set; }

            [CommandLineOption(
                Description =
                    "Password For SqlServer Connection"
                , MinOccurs = 0)]
            public string Password { get; set; }

            [CommandLineOption(
                Description =
                    "Server Name.  if not specified, local is used as dot (.)"
                , MinOccurs = 0, Aliases = "s")]
            public string Server { get; set; }

            [CommandLineOption(
                Description =
                    "If this is specified, then should not specify connection string.  This will force usage of trusted SqlServer connection string."
                , MinOccurs = 0, Aliases = "Database")]
            public string SqlServerCatalogName { get; set; }

            [CommandLineOption(Description = "ENUM Assembly Name.  If specified, then these classes are skipped",
                MinOccurs = 0)]
            public string EnumAssemblyName { get; set; }
        }

        #endregion
    }
}