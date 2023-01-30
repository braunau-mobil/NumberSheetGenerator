using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.IO;

namespace NumberSheetGenerator
{
    internal class PdfGenerator
    {
        //  @todo config
        private const int _regularFontSize = 20;

        private static byte[] CreatePdf(Action<PdfDocument, Document> decorate)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                using (var pdfWriter = new PdfWriter(memoryStream))
                using (var pdfDoc = new PdfDocument(pdfWriter))
                using (var doc = new Document(pdfDoc))
                {
                    pdfDoc.SetDefaultPageSize(pageSize: PageSize.A4);
                    doc.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
                    //  @todo Margins!
                    //doc.SetMargins(settings.PageMargins.Top, settings.PageMargins.Right, settings.PageMargins.Bottom, settings.PageMargins.Left);

                    decorate(pdfDoc, doc);
                }

                bytes = memoryStream.GetBuffer();
            }
            return bytes;
        }

        public PdfGenerator()
        {
        }

        public byte[] GenerateNumberSheet(int maxNumber)
        {
            return CreatePdf((pdfDoc, doc) =>
            {
                var numbersTable = new Table(5)
                    .UseAllAvailableWidth()
                    .SetKeepTogether(true)
                    .
                    //.SetExtendBottomRowOnSplit(true)
                    .SetFontSize(_regularFontSize);

                for (var number = 0; number < maxNumber; number++)
                {
                    var p = new Paragraph($"{number}")
                        .SetBold()
                        .SetKeepTogether(true);

                    var cell = new Cell()
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE)
                        .SetHeight(60)
                        .Add(p);

                    numbersTable.AddCell(cell);
                }

                doc.Add(numbersTable);
            });
        }
    }
}
