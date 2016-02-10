using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PdfSharp;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System.Diagnostics;
using PdfSharp.Pdf;
using System.Windows.Forms;

namespace EoF_Configuration_Manager.Helpers.Reports
{
    public static class PDFIndividualReport
    {

        public static void GenerateReport<T>(T o, Dictionary<String,String> props, String title)
        {
            var doc = CreateDocument();
            Section main = doc.AddSection();

            AddTitle(title, main);

            // get a table
            var table = main.AddTable();

            var propColumn = table.AddColumn();
            var valColumn = table.AddColumn();

            propColumn.Width = 150;
            valColumn.Width = 300;

            foreach (var property in props)
            {
                var row = table.AddRow();
                if (o.GetType().GetProperty(property.Key) != null)
                {
                    row.Cells[0].AddParagraph(property.Value);
                    row.Cells[1].AddParagraph(o.GetType().GetProperty(property.Key).GetValue(o, null).ToString());
                }
                else
                {
                    row.Cells[0].AddParagraph(property.Key);
                    row.Cells[1].AddParagraph(property.Value);
                }
            }

            var saveFD = new SaveFileDialog();
            saveFD.Filter = "Portable Document Format | *.pdf";

            var result = saveFD.ShowDialog();

            if (result == DialogResult.OK)
            {
                CreateFile(saveFD.FileName, doc);
            }

        }

        /// <summary>
        /// Creates the document for us. Any Document properties should be changed in here
        /// </summary>
        /// <returns>The document with all the properties we are looking for</returns>
        private static Document CreateDocument()
        {
            // Create a document with two sections, one header and one for the grid
            Document doc = new Document();

            doc.DefaultPageSetup.PageFormat = PageFormat.Letter;
            doc.DefaultPageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Portrait;
            return doc;
        }

        /// <summary>
        /// Creates the file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="doc"></param>
        private static void CreateFile(String filePath, Document doc)
        {

            const bool unicode = false;
            const PdfFontEmbedding embedding = PdfFontEmbedding.Always;

            // Create a renderer for the MigraDoc document
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = doc;

            // Layout and render the docuemnt to pdf
            pdfRenderer.RenderDocument();

            //Save the document
            pdfRenderer.PdfDocument.Save(filePath);
            Process.Start(filePath);
        }


        /// <summary>
        /// Adds the title
        /// </summary>
        /// <param name="HeaderText"></param>
        /// <param name="DataTableSection"></param>
        private static void AddTitle(String HeaderText, Section DataTableSection)
        {
            var Title = DataTableSection.AddParagraph(HeaderText);
            Title.Format.Font.Size = 30;
            Title.Format.Borders.Visible = true;
            Title.Format.Alignment = ParagraphAlignment.Center;
        }
    }
}
