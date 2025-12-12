
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace SG_Constancia_TSC
{
    public partial class MostrarPdf : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            if (!int.TryParse(id, out int solicitudId)) return;

            byte[] archivo = null;
            string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Archivoconstancia FROM Constancias WHERE SolicitudId = @SolicitudId", conn);
                cmd.Parameters.AddWithValue("@SolicitudId", solicitudId);
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                    archivo = (byte[])result;
            }

            if (archivo != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "inline;filename=Constancia.pdf");
                Response.BinaryWrite(archivo);
                Response.End();
            }
        }
    }
}
