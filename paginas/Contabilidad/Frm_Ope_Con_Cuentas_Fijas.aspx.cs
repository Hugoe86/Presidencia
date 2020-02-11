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
using Presidencia.Cuentas_Contables_Fijas.Negocio;
using Presidencia.Constantes;


public partial class paginas_Contabilidad_Frm_Ope_Con_Cuentas_Fijas : System.Web.UI.Page
{
    private const String MODO_INICIAL = "INICIAL";
    private const String MODO_MODIFICAR = "MODIFICAR";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Cls_Sessiones.Mostrar_Menu = true;
            Llenar_Combos();
            Cargar_Datos();
            Habilitar_Controles(MODO_INICIAL);
        }
    }
    #region EVENTOS
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == "Inicio")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else 
        {
            Habilitar_Controles(MODO_INICIAL);
        }
    }
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Nuevo.ToolTip == "Modificar")
        {
            Habilitar_Controles(MODO_MODIFICAR);
           // Btn_Nuevo.ToolTip = "Guardar";
        }
        else 
        {
            Cls_Ope_Con_Cuentas_Fijas_Negocio Negocio = new Cls_Ope_Con_Cuentas_Fijas_Negocio();
            Negocio.P_Cuenta_Almacen_General = Cmb_Cuentas_Compras_Almacen.SelectedValue.Trim();
            Negocio.P_Cuenta_Iva_Acreditable_Compras = Cmb_Cuentas_Iva_Acreditable_Compras.SelectedValue.Trim();
            int Registros_Actualizados = Negocio.Guardar_Cuentas_Fijas();
            if (Registros_Actualizados > 0)
            {
                ScriptManager.RegisterStartupScript(
                        this, this.GetType(), "Requisiciones", "alert('Los datos se actualizaron');", true);
            }
            else 
            {
                ScriptManager.RegisterStartupScript(
                       this, this.GetType(), "Requisiciones", "alert('No se pudieron actualizar los datos');", true);
            }
            Habilitar_Controles(MODO_INICIAL);
        }
    }

    #endregion

    #region MÉTODOS
    private void Cargar_Datos() 
    {
        Cls_Ope_Con_Cuentas_Fijas_Negocio Negocio = new Cls_Ope_Con_Cuentas_Fijas_Negocio();
        DataTable Dt_Cuentas_Fijas = Negocio.Consultar_Cuentas_Fijas();
        try
        {
            Cmb_Cuentas_Compras_Almacen.SelectedValue =
                Dt_Cuentas_Fijas.Rows[0][Cat_Con_Cuentas_Fijas.Campo_Almacen_General].ToString().Trim();
        }
        catch (Exception Ex) 
        {
            Ex.ToString();
            Cmb_Cuentas_Compras_Almacen.SelectedIndex = 0;
        }
        try
        {
            Cmb_Cuentas_Iva_Acreditable_Compras.SelectedValue =
                Dt_Cuentas_Fijas.Rows[0][Cat_Con_Cuentas_Fijas.Campo_Iva_Acreditable_Compras].ToString().Trim();
        }
        catch (Exception Ex)
        {
            Ex.ToString();
            Cmb_Cuentas_Iva_Acreditable_Compras.SelectedIndex = 0;
        }
    }
    private void Llenar_Combos()
    {
        Cls_Ope_Con_Cuentas_Fijas_Negocio Negocio = new Cls_Ope_Con_Cuentas_Fijas_Negocio();      
        DataTable Dt_Cuentas_Fijas = Negocio.Consultar_Cuentas_Contables();
        Cls_Util.Llenar_Combo_Con_DataTable_Generico
            (Cmb_Cuentas_Compras_Almacen, Dt_Cuentas_Fijas,Cat_Con_Cuentas_Contables.Campo_Cuenta, Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID);
        Cls_Util.Llenar_Combo_Con_DataTable_Generico
            (Cmb_Cuentas_Iva_Acreditable_Compras, Dt_Cuentas_Fijas,Cat_Con_Cuentas_Contables.Campo_Cuenta, Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID);
    }

    private void Habilitar_Controles(String Modo) 
    {
        switch (Modo)
        {
            case MODO_INICIAL:
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Modificar";
                Btn_Salir.ToolTip = "Inicio";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Cmb_Cuentas_Compras_Almacen.Enabled = false;
                Cmb_Cuentas_Iva_Acreditable_Compras.Enabled = false;
                break;
            case MODO_MODIFICAR:
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Guardar";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Cmb_Cuentas_Compras_Almacen.Enabled = true;
                Cmb_Cuentas_Iva_Acreditable_Compras.Enabled = true;
                break;
        }
    }

    #endregion 
}
