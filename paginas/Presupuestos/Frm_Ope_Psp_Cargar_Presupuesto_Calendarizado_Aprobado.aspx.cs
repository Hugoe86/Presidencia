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
using Presidencia.Cargar_Presupuesto_Calendarizado.Negocio;
using Presidencia.Cargar_Presupuesto_Calendarizado.Datos;
using Presidencia.Manejo_Presupuesto.Datos;

public partial class paginas_Presupuestos_Frm_Ope_Psp_Cargar_Presupuesto_Calendarizado_Aprobado : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Cls_Sessiones.Mostrar_Menu = true;
            Cargar_Anios_Presupuesto_Aprobado();
        }
    }

    #region MÉTODOS 

    public void Cargar_Anios_Presupuesto_Aprobado()
    {
        Cls_Ope_Psp_Cargar_Presup_Calendarizado_Negocio Negocio = new Cls_Ope_Psp_Cargar_Presup_Calendarizado_Negocio();
        DataTable Dt_Anios = Negocio.Consultar_Anios_Presupuestados();
        Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Anio, Dt_Anios, "ANIO", "ANIO");
    }
    #endregion

    protected void Cmb_Anio_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Psp_Cargar_Presup_Calendarizado_Negocio Negocio = new Cls_Ope_Psp_Cargar_Presup_Calendarizado_Negocio();
        Negocio.P_Anio = Cmb_Anio.SelectedValue;
        double Importe = 0;
        
        Importe = Negocio.Consultar_Importe_Presupuesto_Aprobado();
        Lbl_Aprobado.Text = " " + String.Format("{0:C}",Importe);

    }
    protected void Btn_Cargar_Presupuesto_Calendarizad_Click(object sender, EventArgs e)
    {
        //Consultar presupuesto calendarizado
        DataTable Dt_Presupuesto_Calendarizado = Cls_Ope_Psp_Manejo_Presupuesto.Consultar_Presupuesto_Calendarizado(int.Parse(Cmb_Anio.SelectedValue.Trim()), "AUTORIZADO");
        int Registros_Guardados = Cls_Ope_Psp_Manejo_Presupuesto.Guardar_Presupuesto_Aprobado(Dt_Presupuesto_Calendarizado);
        ScriptManager.RegisterStartupScript(
            this, this.GetType(), "Presupuesto", "alert('Se guardaron " + Registros_Guardados + "  partidas presupuestales');", true);
    }
}
