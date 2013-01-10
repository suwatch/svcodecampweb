using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ThreePLogicAccessCodeGen.Code
{
    public static class ProcessTemplate
    {
        public static string GenerateNewClass(string templateName, FullMeta fullMeta, bool stripComments)
        {
            List<string> newRawLines = ReadAllLinesOfTemplate(templateName);
            return ProcessFullTemplate(newRawLines, fullMeta, stripComments);
        }

        /// <summary>
        /// Make sure all templates are set to embedded so they can be read from assembly at runtime
        /// </summary>
        /// <param name="templateFileName"></param>
        /// <returns></returns>
        private static List<string> ReadAllLinesOfTemplate(string templateFileName)
        {
            var lines = new List<string>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            if (assembly != null)
            {
                using (var streamReader =
                    new StreamReader(
                        assembly.GetManifestResourceStream(String.Format("CodeGenerator.Templates.{0}.template",
                                                                         templateFileName))))
                {
                    string input;

                    while ((input = streamReader.ReadLine()) != null)
                    {
                        lines.Add(input);
                    }
                }
            }
            return lines;
        }

        private static string ProcessFullTemplate(IEnumerable<string> lines, FullMeta fullMeta, bool stripComments)
        {
            // generate the output to a generic List
            var newLines = new List<string>();
            foreach (string line in lines)
            {
                // Look for ### in template.  Then, based on what comes after
                // add in the appropriate stuff
                int indent = line.IndexOf("//###");
                if (indent >= 0)
                {
                    newLines.Add(line + " START");
                    string templateNumber = line.Trim().Substring(5);
                    AddTemplateCode(templateNumber, indent, newLines, fullMeta);
                    newLines.Add(line + " END");
                }
                else
                {
                    newLines.Add(line);
                }
            }

            var newLinesPostStrip = new List<string>();
            if (stripComments)
            {
                foreach (string line in newLines)
                {
                    if (!line.Trim().StartsWith("//"))
                    {
                        newLinesPostStrip.Add(line.Replace("//D", "// "));
                    }
                    else if (line.Trim().StartsWith("//D"))
                    {
                        // allow for special comment lines beginning with D for Directions
                        newLinesPostStrip.Add(line.Replace("//D", "// "));
                    }
                }
            }
            else
            {
                newLines.ForEach(newLinesPostStrip.Add);
            }


            var stringBuilder = new StringBuilder();
            newLinesPostStrip.ForEach(line => stringBuilder.AppendLine(line));
            return stringBuilder.ToString();
        }


        private static void AddTemplateCode(string templateNumber, int indent, ICollection<string> newLines,
                                            FullMeta fullMetal)
        {
            int iTemplateNumber = Convert.ToInt32(templateNumber);
            var sb = new StringBuilder(indent);
            sb.Append(' ', indent);
            string indentString = sb.ToString();

            var processTemplate = new ProcessTemplateDynamic();
            processTemplate.DoIt(iTemplateNumber, newLines, indentString, fullMetal);
        }
    }
}