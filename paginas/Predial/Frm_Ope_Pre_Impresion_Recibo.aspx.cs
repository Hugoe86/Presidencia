using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Text;

public partial class paginas_Predial_Frm_Ope_Pre_Impresion_Recibo : System.Web.UI.Page
{
    #region PAGELOAD
        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Page_Load
        //DESCRIPCIÓN          : Inicio de la pagina
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 28/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    StringBuilder Javascript = new StringBuilder();
        //    String Referencia = String.Empty; //para obtener la refeferencia del pago
        //    String Tipo_Recibo = String.Empty; //para guardar el tipo de recibo que se imprimira

        //    try
        //    {
        //        Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
        //        //if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../../Paginas_Generales/Frm_Apl_Login.aspx");
        //        if (!IsPostBack)
        //        {
        //            //if (Request.QueryString["Referencia"] != null)
        //            //{
        //            //    Referencia = Request.QueryString["Referencia"].ToString().Trim();
        //            //    Tipo_Recibo = "Traslacion_Dominio";
        //            //}

        //            Referencia = "TD642011";
        //            Tipo_Recibo = "Traslacion_Dominio";
        //            Javascript.Append("$(document).ready(function(){Generar_Recibo_" + Tipo_Recibo + "('" + Referencia + "');});");
        //            ClientScript.RegisterClientScriptBlock(this.GetType(), "", Javascript.ToString(), true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error al generar el recibo oficial Error[" + ex.Message + "]");
        //    }
        //}
    #endregion
}
