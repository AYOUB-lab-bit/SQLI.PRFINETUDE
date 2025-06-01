using ClosedXML.Excel;
using GestionVisaBlazorServer.Models;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace GestionVisaBlazorServer.Services
{
    public class ExportService
    {
        public async Task<byte[]> GenerateExcelCollaborateurs(List<Collaborateur> collaborateurs)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Collaborateurs");

            ws.Cell(1, 1).Value = "Nom";
            ws.Cell(1, 2).Value = "Prénom";
            ws.Cell(1, 3).Value = "Poste";
            ws.Cell(1, 4).Value = "Ancienneté (années)";
            ws.Cell(1, 5).Value = "Date validité passeport";
            ws.Cell(1, 6).Value = "Début Visa";
            ws.Cell(1, 7).Value = "Fin Visa";

            for (int i = 0; i < collaborateurs.Count; i++)
            {
                var c = collaborateurs[i];
                ws.Cell(i + 2, 1).Value = c.Nom;
                ws.Cell(i + 2, 2).Value = c.Prenom;
                ws.Cell(i + 2, 3).Value = c.Poste;
                ws.Cell(i + 2, 4).Value = c.AnciennetéProjet;
                ws.Cell(i + 2, 5).Value = c.DateValiditePasseport.ToShortDateString();
                ws.Cell(i + 2, 6).Value = c.DateDebutVisa?.ToShortDateString() ?? "—";
                ws.Cell(i + 2, 7).Value = c.DateFinVisa?.ToShortDateString() ?? "—";
            }

            ws.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            return ms.ToArray();
        }

        public async Task<byte[]> GeneratePdfCollaborateurs(List<Collaborateur> collaborateurs)
        {
            using var ms = new MemoryStream();
            using var writer = new PdfWriter(ms);
            using var pdf = new PdfDocument(writer);
            var doc = new Document(pdf);

            var bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var normal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            doc.Add(new Paragraph("Liste des Collaborateurs")
                .SetFont(bold)
                .SetFontSize(18)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(20));

            var table = new Table(7).UseAllAvailableWidth();
            string[] headers = { "Nom", "Prénom", "Poste", "Ancienneté", "Date validité passeport", "Début Visa", "Fin Visa" };
            foreach (var h in headers)
                table.AddHeaderCell(new Cell().Add(new Paragraph(h).SetFont(bold))
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(5));

            foreach (var c in collaborateurs)
            {
                table.AddCell(new Paragraph(c.Nom).SetFont(normal));
                table.AddCell(new Paragraph(c.Prenom).SetFont(normal));
                table.AddCell(new Paragraph(c.Poste ?? "—").SetFont(normal));
                table.AddCell(new Paragraph(c.AnciennetéProjet.ToString()).SetFont(normal).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(new Paragraph(c.DateValiditePasseport.ToShortDateString()).SetFont(normal).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(new Paragraph(c.DateDebutVisa?.ToShortDateString() ?? "—").SetFont(normal).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(new Paragraph(c.DateFinVisa?.ToShortDateString() ?? "—").SetFont(normal).SetTextAlignment(TextAlignment.CENTER));
            }

            doc.Add(table);
            doc.Close();

            return ms.ToArray();
        }
        // Même principe pour DemandesVisa et Deplacements, exemples :

        public async Task<byte[]> GenerateExcelDemandesVisa(List<DemandeVisa> demandes)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("DemandesVisa");

            worksheet.Cell(1, 1).Value = "Collaborateur";
            worksheet.Cell(1, 2).Value = "Statut";
            worksheet.Cell(1, 3).Value = "Date Demande";
            worksheet.Cell(1, 4).Value = "Durée Visa";
            worksheet.Cell(1, 5).Value = "Observations";

            int row = 2;
            foreach (var d in demandes)
            {
                worksheet.Cell(row, 1).Value = $"{d.Collaborateur?.Nom} {d.Collaborateur?.Prenom}";
                worksheet.Cell(row, 2).Value = d.Statut.ToString();
                worksheet.Cell(row, 3).Value = d.DateDemande.ToString("dd/MM/yyyy");
                worksheet.Cell(row, 4).Value = d.DureeVisa;
                worksheet.Cell(row, 5).Value = d.Observations ?? "";
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }



        public async Task<byte[]> GeneratePdfDemandesVisa(List<DemandeVisa> demandes)
        {
            using var ms = new MemoryStream();
            using var writer = new PdfWriter(ms);
            using var pdf = new PdfDocument(writer);
            var doc = new Document(pdf);

            var bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var normal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            doc.Add(new Paragraph("Liste des Demandes de VISA")
                .SetFont(bold)
                .SetFontSize(18)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(20));

            var table = new Table(5).UseAllAvailableWidth();
            string[] headers = { "Collaborateur", "Statut", "Date Demande", "Durée", "Observations" };
            foreach (var h in headers)
                table.AddHeaderCell(new Cell().Add(new Paragraph(h).SetFont(bold))
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(5));

            foreach (var d in demandes)
            {
                table.AddCell(new Paragraph(d.Collaborateur?.Nom + " " + d.Collaborateur?.Prenom ?? "—").SetFont(normal));
                table.AddCell(new Paragraph(d.Statut.ToString()).SetFont(normal));
                table.AddCell(new Paragraph(d.DateDemande.ToShortDateString()).SetFont(normal));
                table.AddCell(new Paragraph(d.DureeVisa?.ToString() ?? "—").SetFont(normal).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(new Paragraph(d.Observations ?? "—").SetFont(normal));
            }

            doc.Add(table);
            doc.Close();

            return ms.ToArray();
        }

        public async Task<byte[]> GenerateExcelDeplacements(List<Deplacement> deplacements)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Déplacements");

            ws.Cell(1, 1).Value = "Date début";
            ws.Cell(1, 2).Value = "Date fin";
            ws.Cell(1, 3).Value = "Équipe";
            ws.Cell(1, 4).Value = "Observations";
            ws.Cell(1, 5).Value = "Coût (€)";

            for (int i = 0; i < deplacements.Count; i++)
            {
                var d = deplacements[i];
                ws.Cell(i + 2, 1).Value = d.DateDebut.ToShortDateString();
                ws.Cell(i + 2, 2).Value = d.DateFin.ToShortDateString();
                ws.Cell(i + 2, 3).Value = d.Equipe.ToString();
                ws.Cell(i + 2, 4).Value = d.Observations ?? "—";
                ws.Cell(i + 2, 5).Value = d.CoutTotal;
                ws.Cell(i + 2, 5).Style.NumberFormat.Format = "#,##0.00 €";
            }

            ws.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            return ms.ToArray();
        }

        // Export PDF des déplacements
        public async Task<byte[]> GeneratePdfDeplacements(List<Deplacement> deplacements)
        {
            using var ms = new MemoryStream();
            using var writer = new PdfWriter(ms);
            using var pdf = new PdfDocument(writer);
            var doc = new Document(pdf);

            var bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var normal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            doc.Add(new Paragraph("Liste des Déplacements")
                .SetFont(bold)
                .SetFontSize(18)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(20));

            var table = new Table(5).UseAllAvailableWidth();

            string[] headers = { "Date début", "Date fin", "Équipe", "Observations", "Coût (€)" };
            foreach (var h in headers)
            {
                table.AddHeaderCell(new Cell()
                    .Add(new Paragraph(h).SetFont(bold))
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(5));
            }

            foreach (var d in deplacements)
            {
                table.AddCell(new Paragraph(d.DateDebut.ToShortDateString()).SetFont(normal));
                table.AddCell(new Paragraph(d.DateFin.ToShortDateString()).SetFont(normal));
                table.AddCell(new Paragraph(d.Equipe.ToString()).SetFont(normal));
                table.AddCell(new Paragraph(d.Observations ?? "—").SetFont(normal));
                table.AddCell(new Paragraph(d.CoutTotal.ToString("N2") + " €")
                    .SetFont(normal)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetPadding(5));
            }

            doc.Add(table);
            doc.Close();

            return ms.ToArray();
        }
    }
}
