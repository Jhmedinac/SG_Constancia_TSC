using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using SG_Constancia_TSC.App_Start;
using DevExpress.XtraPrinting.BarCode;

namespace SG_Constancia_TSC.Reportes
{
    public partial class Comprobante : DevExpress.XtraReports.UI.XtraReport
    {
        public Comprobante()
        {
            InitializeComponent();
            //Util.Encripta(this.codigo_ticket.ToString());
            this.xrBarCode1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
             new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "'https://presdjl.tsc.gob.hn/Doc.aspx?id=' + [Id]")

        });
            this.xrBarCode1.ShowText = false;


        }
        //private void InitializeComponent()
        //{
        //    // Agrega un parámetro para el ID encriptado si aún no existe
        //    DevExpress.XtraReports.Parameters.Parameter encryptedIdParam = new DevExpress.XtraReports.Parameters.Parameter
        //    {
        //        Name = "EncryptedId",
        //        Type = typeof(string),
        //        Visible = true // Cambia a false si no quieres que aparezca en la interfaz de usuario del reporte
        //    };
        //    this.Parameters.Add(encryptedIdParam);

        //    XRBarCode xrBarCode1 = new XRBarCode();
        //    this.Detail.Controls.Add(xrBarCode1);
        //    xrBarCode1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
        //    new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "'http://localhost:63129/Doc.aspx?id=' + ?EncryptedId")


        //    });


        //    this.xrBarCode1.ShowText = false;
        //}
         
    }
}
