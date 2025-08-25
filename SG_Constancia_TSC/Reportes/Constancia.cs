using DevExpress.CodeParser;
using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraPrinting.Drawing;
using System.IO;

namespace SG_Constancia_TSC.Reportes
{
    public partial class Constancia : DevExpress.XtraReports.UI.XtraReport
    {
        public Constancia(string dynamicText, string solicitudId, byte[] firmaBytes)
        {
            {
                InitializeComponent();

                // Crear y configurar el parámetro
                var paramSolicitudId = new DevExpress.XtraReports.Parameters.Parameter
                {
                    Name = "SolicitudId",
                    Type = typeof(string),
                    Value = solicitudId,
                    Visible = false
                };

                this.Parameters.Add(paramSolicitudId);

                // Asignar imagen de la firma si existe
                if (firmaBytes != null && firmaBytes.Length > 0)
                {
                    using (var ms = new MemoryStream(firmaBytes))
                    {
                        xrPictureBoxFirma.ImageSource = new ImageSource(System.Drawing.Image.FromStream(ms));
                    }
                }

                // Configurar el código QR usando el parámetro
                this.xrBarCode1.ExpressionBindings.Add(new DevExpress.XtraReports.UI.ExpressionBinding(
                    "BeforePrint", "Text", "'https://constancia.tsc.gob.hn/Doc.aspx?id=' + ?SolicitudId"
                ));

                this.xrBarCode1.ShowText = false;

                // Asignar el contenido dinámico
                xrRichText3.Html = dynamicText;

            }
        }
            

        public string ConvertHtmlToRtf(string html)
        {
            RichTextBox rtb = new RichTextBox();
            rtb.Text = "";
            rtb.Rtf = "";
            rtb.Text = html;
            return rtb.Rtf;
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}
