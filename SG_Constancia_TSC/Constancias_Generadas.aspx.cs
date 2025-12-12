using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using DevExpress.Web;
using SG_Constancia_TSC.App_Start;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;

using System.Text;

using SG_Constancia_TSC.Utili;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;

using DevExpress.Office.Internal;


using DevExpress.Web.Internal.XmlProcessor;
using DevExpress.CodeParser;
using System.Web.Services;
using DevExpress.Pdf.Native;
using System.Drawing.Imaging;
using System.Drawing;
using ZXing.QrCode;
using ZXing;
using DevExpress.XtraPrinting.BarCode;
using SG_Constancia_TSC.Reportes;
using DevExpress.CodeParser.VB;
using static DevExpress.DataProcessing.InMemoryDataProcessor.AddSurrogateOperationAlgorithm;
using System.Diagnostics;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using DevExpress.XtraReports.Web;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;


namespace SG_Constancia_TSC
{
    public partial class Constancias_Generadas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Form.Attributes.Add("autocomplete", "off");

            if (!IsPostBack)
            {   
                LoadStatuses();
                GV_PreUsuarios.FocusedRowIndex = -1; // Evita que se enfoque una fila al cargar la página

                cmbTipoFiltro.Value = "0";

                // 2) Ajusta el parámetro del SqlDataSource
                SqlDataUsers.SelectParameters["TipoSolicitud"].DefaultValue = "false";

                // 3) Rellena el grid
                GV_PreUsuarios.DataBind();

                // 4) Ajusta visibilidad de columnas
                GV_PreUsuarios.Columns["Identidad"].Visible = true;
                GV_PreUsuarios.Columns["FirstName"].Visible = true;
                GV_PreUsuarios.Columns["LastName"].Visible = true;
                GV_PreUsuarios.Columns["NumRtn"].Visible = false;
                GV_PreUsuarios.Columns["NomInstitucion"].Visible = false;

            }
        }

        private void LoadStatuses()
        {
            string connectionString = (ConfigurationManager.ConnectionStrings["connString"].ConnectionString);
            string query = "SELECT Id_Estado, Descripcion_Estado FROM Estados WHERE Id_Estado IN (6)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                cmbStatus.DataSource = dataTable;
                cmbStatus.DataBind();
            }
        }

        protected void ASPxCallbackPanel_Report_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            int solicitudId;
            if (int.TryParse(e.Parameter, out solicitudId))
            {
                iframePDF.Attributes["src"] = "MostrarPdf_1.aspx?id=" + solicitudId;
                iframePDF.Visible = true;

            }

        }

        protected void GV_PreUsuarios_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void GV_PreUsuarios_CustomCallback1(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void GV_PreUsuarios_BeforeExport(object sender, ASPxGridBeforeExportEventArgs e)
        {

        }

        protected void ASPxCallback_PopupUpdate_Callback(object source, CallbackEventArgs e)
        {

        }

        private DataTable GetFilteredDataFromGridView()
        {
            // Crear y definir el DataTable
            DataTable dt = new DataTable();
            dt.Columns.Add("FirstName", typeof(string));
            dt.Columns.Add("LastName", typeof(string));
            dt.Columns.Add("Identidad", typeof(string));
            dt.Columns.Add("nombre", typeof(string)); // Columna para el nombre completo
            dt.Columns.Add("Id", typeof(string)); // Columna para el nombre completo

            // Obtener el índice de la fila enfocada (seleccionada)
            int selectedRowIndex = GV_PreUsuarios.FocusedRowIndex;
            if (selectedRowIndex >= 0)
            {
                // Obtener los valores de la fila seleccionada
                object[] values = GV_PreUsuarios.GetRowValues(
                    selectedRowIndex,
                    new string[] { "FirstName", "LastName", "Identidad", "Id" }
                ) as object[];

                if (values != null)
                {
                    DataRow dr = dt.NewRow();
                    dr["FirstName"] = values[0];
                    dr["LastName"] = values[1];
                    dr["Identidad"] = values[2];
                    // Combina FirstName y LastName, agregando un espacio entre ellos
                    dr["nombre"] = values[0].ToString() + " " + values[1].ToString();
                    dr["Id"] = values[3];
                    dt.Rows.Add(dr);
                }
            }
            // Si no hay fila seleccionada, el DataTable quedará vacío
            return dt;
        }

       
        protected void cmbTipoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {

            bool isNatural = (cmbTipoFiltro.Value as string) == "0";

            // 1) Ajusto el parámetro del SqlDataSource
            SqlDataUsers.SelectParameters["TipoSolicitud"].DefaultValue = isNatural
                ? "false"
                : "true";

            // 2) Re-bindeo el grid
            GV_PreUsuarios.DataBind();

            // 3) Muestro/oculto columnas
            GV_PreUsuarios.Columns["Identidad"].Visible = isNatural;
            GV_PreUsuarios.Columns["FirstName"].Visible = isNatural;
            GV_PreUsuarios.Columns["LastName"].Visible = isNatural;
            GV_PreUsuarios.Columns["NumRtn"].Visible = !isNatural;
            GV_PreUsuarios.Columns["NomInstitucion"].Visible = !isNatural;


        }
    }
}