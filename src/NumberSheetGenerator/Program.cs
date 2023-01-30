using System;
using System.Diagnostics;
using System.IO;

namespace NumberSheetGenerator
{
    public static class Program
    {
        private const string _pdfReaderApplicationPath = @"c:\Program Files (x86)\Adobe\Acrobat Reader DC\Reader\AcroRd32.exe";

        public static void Main()
        {
            var generator = new PdfGenerator();
            var pdfBytes = generator.GenerateNumberSheet(750);

            var fileName = "numbers.pdf";
            File.WriteAllBytes(fileName, pdfBytes);

            var filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            
            Process p = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = _pdfReaderApplicationPath,
                    Arguments = filePath
                }
            };
            p.Start();
        }
    }
}
