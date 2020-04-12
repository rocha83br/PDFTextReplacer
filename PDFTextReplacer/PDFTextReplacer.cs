using System;
using System.Text;
using System.Collections.Generic;
using iText.Kernel.Pdf;

namespace Rochas.PDFText
{
    public static class PDFTextReplacer
    {
        public static bool ReplaceText(string sourceFile, string destinationFile, Dictionary<string, string> replacements, int pageNumber = 1)
        {
            var result = false;

            CheckArguments(sourceFile, destinationFile, replacements);

            var pdfReader = new PdfReader(sourceFile);
            var pdfWriter = new PdfWriter(destinationFile);
            if ((pdfReader != null) && (pdfWriter != null))
            {
                var pdfDoc = new PdfDocument(pdfReader, pdfWriter);
                if (pdfDoc != null)
                {
                    var pdfPage = pdfDoc.GetPage(pageNumber);
                    var pdfDic = pdfPage.GetPdfObject();
                    var pdfObject = pdfDic.Get(PdfName.Contents);
                    var pdfContent = (PdfStream)pdfObject;
                    var pdfData = pdfContent.GetBytes();
                    var pdfStrData = Encoding.GetEncoding(1252).GetString(pdfData);

                    foreach (var replace in replacements)
                        pdfStrData = pdfStrData.Replace(replace.Key, replace.Value);

                    pdfContent.SetData(Encoding.GetEncoding(1252).GetBytes(pdfStrData));                    
                }

                pdfWriter.Flush();

                pdfDoc.Close();
                pdfReader.Close();
                pdfWriter.Close();

                result = true;
            }

            return result;
        }

        private static void CheckArguments(string sourceFile, string destinationFile, Dictionary<string, string> replacements)
        {
            if (string.IsNullOrWhiteSpace(sourceFile))
                throw new ArgumentNullException("Source file name argument is required.");

            if (string.IsNullOrWhiteSpace(sourceFile))
                throw new ArgumentNullException("Destination file name argument is required.");

            if (replacements == null)
                throw new ArgumentNullException("String replacements is required.");
        }
    }
}
