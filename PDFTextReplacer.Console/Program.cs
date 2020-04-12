using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rochas.PDFText;
using System.IO;

namespace Rochas.PDFText.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var sourceFile = string.Empty;
            var destinationFile = string.Empty;
            var replacements = new Dictionary<string, string>();
            var pageNumber = 1;

            System.Console.Clear();
            System.Console.WriteLine("Rochas PDF Text Replacer");
            System.Console.WriteLine("------------------------");

            GetSourceFile(ref sourceFile);
            GetDestinationFile(ref destinationFile);
            GetReplacements(replacements);
            GetPageNumber(out pageNumber);

            try
            {
                System.Console.WriteLine();
                PDFTextReplacer.ReplaceText(sourceFile, destinationFile, replacements, pageNumber);
                System.Console.WriteLine("Done!");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error while managing pdf document:{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
            finally
            {
                System.Console.ReadLine();
            }
        }

        static void GetSourceFile(ref string sourceFile)
        {
            while (!File.Exists(sourceFile))
            {
                System.Console.Write("Enter the source file path : ");
                sourceFile = System.Console.ReadLine();

                if (!File.Exists(sourceFile))
                {
                    System.Console.WriteLine("FILE NOT FOUND.");
                    System.Console.WriteLine();
                }
                else
                {
                    System.Console.WriteLine("OK.");
                    System.Console.WriteLine();
                }
            }
        }

        static void GetDestinationFile(ref string destinationFile)
        {
            var destinationPathExists = false;
            while (!destinationPathExists)
            {
                System.Console.Write("Enter the destination file path : ");
                destinationFile = System.Console.ReadLine();

                destinationPathExists = !string.IsNullOrWhiteSpace(destinationFile)
                                      && Directory.Exists(Path.GetDirectoryName(destinationFile));

                if (!destinationPathExists)
                {
                    System.Console.WriteLine("PATH NOT FOUND.");
                    System.Console.WriteLine();
                }
                else
                {
                    System.Console.WriteLine("OK.");
                    System.Console.WriteLine();
                }
            }
        }

        static void GetReplacements(IDictionary<string, string> replacements)
        {
            var keyValue = string.Empty;
            while (string.IsNullOrWhiteSpace(keyValue))
            {
                if (!replacements.Any())
                    System.Console.WriteLine("Enter the replacement items, after type END to continue : ");
                else
                    System.Console.WriteLine("Enter the next replacement item :");

                keyValue = System.Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(keyValue))
                {
                    if (keyValue.Contains("|"))
                    {
                        var keyValueArr = keyValue.Split('|');
                        replacements.Add(keyValueArr[0], keyValueArr[1]);
                    }
                    else if (keyValue.ToUpper().Equals("END"))
                    {
                        if (replacements.Any())
                        {
                            System.Console.WriteLine("OK.");
                            System.Console.WriteLine();
                            return;
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("INVALID REPLACEMENT FORMAT, USE: Source|Replacement FORMAT.");
                        System.Console.WriteLine();
                    }
                }

                keyValue = string.Empty;
            }
        }

        static void GetPageNumber(out int pageNumber)
        {
            System.Console.Write("Enter the page number to act : ");
            var page = System.Console.ReadLine();

            if (!int.TryParse(page, out pageNumber))
            {
                if (!string.IsNullOrWhiteSpace(page))
                {
                    System.Console.WriteLine("INVALID PAGE NUMBER.");
                    System.Console.WriteLine();
                }
                else
                    pageNumber = 1;
            }
            else
            {
                System.Console.WriteLine("OK.");
                System.Console.WriteLine();
            }
        }
    }
}
