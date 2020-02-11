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
using Presidencia.Sessiones;
using Presidencia.Solicitud_Tramites.Negocios;
using Presidencia.Dependencias.Negocios;
using Presidencia.Listado_Tramites.Negocio;
public partial class paginas_Tramites_Frm_Ope_Tra_Tramite_Listado_Tramites : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Cls_Sessiones.Mostrar_Menu = true;
            Llenar_Combo_Dependencias(null);
            Llenar_Combo_Tramites(null);
        }
    }
    private void Llenar_Combo_Tramites(String Dependencia_ID) 
    {
        //llenar combo tramites
        Cls_Ope_Tra_Listado_Tramites_Negocio Negocio = new Cls_Ope_Tra_Listado_Tramites_Negocio();
        Negocio.P_Dependencia_ID = Dependencia_ID;
        DataTable Dt_Tramites = Negocio.Consultar_Tramites();
        Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Tramites,Dt_Tramites,"NOMBRE","TRAMITE_ID");
    }

    private void Llenar_Combo_Dependencias(String Dependencia_ID)
    {
        //llenar combo dependencias
        Cls_Ope_Tra_Listado_Tramites_Negocio Negocio = new Cls_Ope_Tra_Listado_Tramites_Negocio();
        Negocio.P_Dependencia_ID = Dependencia_ID;
        DataTable Dt_Dependencias = Negocio.Consultar_Unidades_Responsables();
        Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Dependencias, Dt_Dependencias, "CLAVE_NOMBRE", "DEPENDENCIA_ID");
    }
    
    protected void Cmb_Dependencias_SelectedIndexChanged(object sender, EventArgs e)
    {
        //llenar combo tramites
        Llenar_Combo_Tramites(Cmb_Dependencias.SelectedValue);
    }
    protected void Cmb_Tramites_SelectedIndexChanged(object sender, EventArgs e)
    {
        //llenar combo dependencias
        //Llenar_Combo_Dependencias(Cmb_Dependencias);
        Cls_Ope_Tra_Listado_Tramites_Negocio Negocio = new Cls_Ope_Tra_Listado_Tramites_Negocio();
        Negocio.P_Tramite_ID = Cmb_Tramites.SelectedValue;
        DataTable Dt_Dependencias = Negocio.Consultar_Unidades_Responsables();
        if (Dt_Dependencias != null && Dt_Dependencias.Rows.Count > 0) 
        {
            Cmb_Dependencias.SelectedValue = Dt_Dependencias.Rows[0]["DEPENDENCIA_ID"].ToString().Trim();        
        }
    }
    protected void Txt_Busqueda_TextChanged(object sender, EventArgs e)
    {

    }
    protected void Btn_Busqueda_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void Btn_Refrescar_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Combo_Tramites(null);
        Cmb_Dependencias.SelectedIndex = 0;
    }
    protected void Btn_Solicitar_Tramite_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Tramites/Frm_Ope_Tra_Solicitud_Ciudadano.aspx");
    }
}
