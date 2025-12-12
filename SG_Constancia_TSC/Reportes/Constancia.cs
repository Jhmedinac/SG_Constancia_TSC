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
       //Tamaño firma
        const float FirmaWidth = 380f;      
        const float FirmaHeight = 100f;      
        const float FirmaX = 139.7919f;  
        const float FirmaY = 430f;      



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

                // --- Config base del PictureBox de firma ---
                xrPictureBoxFirma.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
                xrPictureBoxFirma.CanGrow = false;
                xrPictureBoxFirma.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleCenter;

                // Configurar el código QR usando el parámetro
                this.xrBarCode1.ExpressionBindings.Add(new DevExpress.XtraReports.UI.ExpressionBinding(
                    "BeforePrint", "Text", "'https://consta-sec-dev.tsc.gob.hn:8011/VerificarConstancia.aspx?id=' + ?SolicitudId"
                ));

                this.xrBarCode1.ShowText = false;

                // Asignar el contenido dinámico
                xrRichText3.Html = dynamicText;

                // --- Blindar tamaño/posición justo antes de imprimir ---
                this.BeforePrint += Constancia_BeforePrint;

            }
        }
        private void Constancia_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            // 1) Tamaño y posición finales de la firma
            xrPictureBoxFirma.SizeF = new SizeF(FirmaWidth, FirmaHeight);
            xrPictureBoxFirma.LocationFloat = new DevExpress.Utils.PointFloat(FirmaX, FirmaY);

            // 2) Reacomodar la línea y etiquetas debajo de la firma
            float yLine = FirmaY + FirmaHeight;
            float yLabel3 = yLine + 2f;      // nombre de quien firma
            float yLabel2 = yLabel3 + 23f;   // cargo/descr.

            xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(FirmaX, yLine);
            xrLine1.SizeF = new SizeF(FirmaWidth, 2f);

            xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(FirmaX, yLabel3);
            xrLabel3.SizeF = new SizeF(FirmaWidth, xrLabel3.SizeF.Height);
            xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;

            xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(FirmaX + 20f, yLabel2);
            xrLabel2.SizeF = new SizeF(FirmaWidth - 40f, xrLabel2.SizeF.Height);
            xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;

            // 3) Evitar recortes: asegurar altura del Detail
            float contenidoInferior = yLabel2 + xrLabel2.SizeF.Height + 10f; // margen extra
            if (Detail.HeightF < contenidoInferior)
                Detail.HeightF = contenidoInferior;
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
