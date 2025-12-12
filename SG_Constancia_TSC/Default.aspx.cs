using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SG_Constancia_TSC
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (cmbTipo.Value == null)
                    cmbTipo.Value = 0; // Natural por defecto
                ActualizarTitulo();
            }



        }

        protected void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // El SqlDataSource toma el valor del combo automáticamente
            ChartEstado.DataBind();
            ActualizarTitulo();
        }

        private void ActualizarTitulo()
        {
            var tipo = (cmbTipo.Value != null && cmbTipo.Value.ToString() == "1") ? "Jurídicas" : "Naturales";
            string desde = cmbFecInicio.Value == null ? "" : Convert.ToDateTime(cmbFecInicio.Value).ToString("dd-MM-yyyy");
            string hasta = cmbFecFin.Value == null ? "" : Convert.ToDateTime(cmbFecFin.Value).ToString("dd-MM-yyyy");

            string rango = (desde == "" && hasta == "") ? "" :
                           $" — {(desde == "" ? "" : "Desde " + desde)}{(desde != "" && hasta != "" ? " - " : "")}{(hasta == "" ? "" : "Hasta " + hasta)}";

            litTitulo.Text = $"Estado de las Solicitudes  {tipo}{rango}";
        }

        protected void Fechas_Changed(object sender, EventArgs e)
        {
            ChartEstado.DataBind();
            ActualizarTitulo();
        }

    }
}