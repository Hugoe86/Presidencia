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
using Presidencia.SCG_Presupuestos.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Sessiones;
using Presidencia.Constantes;
public partial class paginas_Paginas_Generales_Frm_Ope_Presupuesto_UR : System.Web.UI.Page
{
    private Cls_Cat_Dependencias_Negocio Dependencia_Negocio;
    protected void Page_Load(object sender, EventArgs e)
    {
        //Valores de primera vez
        if (!IsPostBack)
        {
            ViewState["SortDirection"] = "DESC";
            Dependencia_Negocio = new Cls_Cat_Dependencias_Negocio();
            DataTable Dt_Dependencias = Dependencia_Negocio.Consulta_Dependencias();
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Dependencia_Panel, Dt_Dependencias, 1, 0);
            Cmb_Dependencia_Panel.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado;
            //Llenar_Grid_Requisiciones();
            //Verificar si su rol es jefe de dependencia, admin de modulo o admin de sistema
            DataTable Dt_Grupo_Rol = Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Rol_ID.ToString());
            if (Dt_Grupo_Rol != null)
            {
                String Grupo_Rol = Dt_Grupo_Rol.Rows[0][Apl_Cat_Roles.Campo_Grupo_Roles_ID].ToString();
                if (Grupo_Rol == "00001" || Grupo_Rol == "00002")
                {
                    Cmb_Dependencia_Panel.Enabled = true;
                }
                else
                {
                    DataTable Dt_URs = Cls_Util.Consultar_URs_De_Empleado(Cls_Sessiones.Empleado_ID);
                    if (Dt_URs.Rows.Count > 1)
                    {
                        Cmb_Dependencia_Panel.Enabled = true;
                        Cls_Util.Llenar_Combo_Con_DataTable_Generico
                            (Cmb_Dependencia_Panel, Dt_URs, 1, 0);
                        Cmb_Dependencia_Panel.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado;
                    }
                }
            }

        }
        //Tooltips
        //Agregar_Tooltip_Combo(Cmb_Fte_Financiamiento);
        //Agregar_Tooltip_Combo(Cmb_Programa);
        //Agregar_Tooltip_Combo(Cmb_Partida);
        //Mostrar_Informacion("", false);
    }

    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Llenar_Grid_Requisiciones
    // DESCRIPCIÓN: Llena el grid principal de requisiciones
    // RETORNA: 
    // CREO: Gustavo Angeles Cruz
    // FECHA_CREO: Diciembre/2010 
    // MODIFICO:
    // FECHA_MODIFICO:
    // CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    public void Llenar_Grid_Requisiciones()
    {
        Div_Contenido.Visible = false;
        Div_Listado_Requisiciones.Visible = true;
        Cls_Ope_Presupuesto_Negocio Presupuesto_Negocio = new Cls_Ope_Presupuesto_Negocio();
        Presupuesto_Negocio.P_Dependencia_ID = Cmb_Dependencia_Panel.SelectedValue.Trim();
        Session["xx"] = Presupuesto_Negocio.Consultar_Presupuesto_UR();
        Grid_Requisiciones.DataSource = (DataTable)Session["xx"];
        Grid_Requisiciones.DataBind();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Grid_Requisiciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Requisiciones.DataSource = ((DataTable)Session["xx"]);
        Grid_Requisiciones.PageIndex = e.NewPageIndex;
        Grid_Requisiciones.DataBind();
    }

    /// ******************************************************************************************
    /// NOMBRE: Grid_Requisiciones_Sorting
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// CREÓ: Gustavo Angeles Cruz
    /// FECHA CREÓ: 11/Junio/2011
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// ******************************************************************************************
    protected void Grid_Requisiciones_Sorting(object sender, GridViewSortEventArgs e)
    {
        Grid_Sorting(Grid_Requisiciones, ((DataTable)Session["xx"]), e);
    }

    /// *****************************************************************************************
    /// NOMBRE: Grid_Sorting
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// CREÓ: Gustavo Angeles Cruz
    /// FECHA CREÓ: 11/Junio/2011
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************
    private void Grid_Sorting(GridView Grid, DataTable Dt_Table, GridViewSortEventArgs e)
    {
        if (Dt_Table != null)
        {
            DataView Dv_Vista = new DataView(Dt_Table);
            String Orden = ViewState["SortDirection"].ToString();
            if (Orden.Equals("ASC"))
            {
                Dv_Vista.Sort = e.SortExpression + " DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Vista.Sort = e.SortExpression + " ASC";
                ViewState["SortDirection"] = "ASC";
            }
            Grid.DataSource = Dv_Vista;
            Grid.DataBind();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //Response.Redirect("../Paginas_Generales/Pagina.aspx");
        Llenar_Grid_Requisiciones();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "W",
        "window.open('" + "../Paginas_Generales/Pagina.aspx" + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);

    }
}
