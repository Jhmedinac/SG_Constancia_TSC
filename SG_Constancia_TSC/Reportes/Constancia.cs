using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SG_Constancia_TSC.Reportes
{
    public partial class Constancia : DevExpress.XtraReports.UI.XtraReport
    {
        public Constancia(string dynamicText)
        {
            InitializeComponent();

            // Asignar el texto dinámico a la etiqueta
            /*rRichText3.AllowMarkupText = true;*/


            xrRichText3.Html = dynamicText;
            //xrRichText3.Html  = ConvertHtmlToRtf(dynamicText);
            //xrRichText3.Rtf = ConvertHtmlToRtf(dynamicText);

        }

        public string ConvertHtmlToRtf(string html)
        {
            RichTextBox rtb = new RichTextBox();
            rtb.Text = "";
            rtb.Rtf = "";
            rtb.Text = html;
            return rtb.Rtf;
        }

    }
}
