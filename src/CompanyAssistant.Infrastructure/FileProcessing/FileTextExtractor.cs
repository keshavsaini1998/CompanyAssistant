using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace CompanyAssistant.Infrastructure.FileProcessing
{
    public class FileTextExtractor
    {
        public static string Extract(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext == ".pdf")
            {
                using var pdf = PdfDocument.Open(file.OpenReadStream());
                return string.Join("\n", pdf.GetPages().Select(p => p.Text));
            }
            else if (ext == ".docx")
            {
                using var doc = WordprocessingDocument.Open(file.OpenReadStream(), false);
                return doc.MainDocumentPart!.Document.Body!.InnerText;
            }
            else throw new Exception("Unsupported file type");
        }
    }
}
