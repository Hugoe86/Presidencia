using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;

public partial class paginas_Predial_Frm_Ope_Pre_Imprimir_Numero_Nota : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                //Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                Llenar_Grid();
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    // llenar grid con columnas en blanco
    private void Llenar_Grid()
    {
        DataTable Tabla_Nueva = new DataTable();

        try
        {
            // ---------- Inicializar columnas

            Tabla_Nueva.Columns.Add("ORDEN", typeof(String));
            Tabla_Nueva.Columns.Add("ESTATUS", typeof(String));
            Tabla_Nueva.Columns.Add("REALIZO", typeof(String));
            Tabla_Nueva.Columns.Add("FECHA", typeof(String));
            Tabla_Nueva.Columns.Add("TIPO", typeof(String));
            Tabla_Nueva.Columns.Add("CUENTA", typeof(String));
            Tabla_Nueva.Columns.Add("c2", typeof(String));


            // generar filas en blanco
            for (int i = 0; i < 12; i++)
            {
                Tabla_Nueva.Rows.Add(
                    HttpUtility.HtmlDecode("&nbsp;"),
                    HttpUtility.HtmlDecode("&nbsp;"),
                    HttpUtility.HtmlDecode("&nbsp;"),
                    HttpUtility.HtmlDecode("&nbsp;"),
                    HttpUtility.HtmlDecode("&nbsp;"),
                    HttpUtility.HtmlDecode("&nbsp;"),
                    HttpUtility.HtmlDecode("&nbsp;")
                    );
            }

            Grid.DataSource = Tabla_Nueva;
            Grid.DataBind();
            GridView1.DataSource = Tabla_Nueva;
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Grid: " + ex.Message.ToString(), ex);
        }
    }
}
