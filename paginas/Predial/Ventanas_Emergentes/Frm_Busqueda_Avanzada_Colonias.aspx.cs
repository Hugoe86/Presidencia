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
using Presidencia.Catalogo_Colonias.Negocio;
using Presidencia.Constantes;

public partial class paginas_Predial_Ventanas_Emergentes_Frm_Busqueda_Avanzada_Colonias : System.Web.UI.Page
{

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Page_Load
    /// DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    /// PARÁMETROS: sender y e
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 18-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///******************************************************************************************************* 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["BUSQUEDA_COLONIAS"] = false;
        }

    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Regresar_Click
    /// DESCRIPCIÓN: Evento Click del control Button Regresar
    /// PARÁMETROS: sender y e
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 18-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        Session["BUSQUEDA_COLONIAS"] = false;
        Session.Remove("COLONIA_ID");
        Session.Remove("NOMBRE_COLONIA");
        //Cierra la ventana
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar_Busqueda", "window.close();", true);
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Limpiar_Click
    /// DESCRIPCIÓN: Evento Click del control Button Limpiar
    /// PARÁMETROS: sender y e
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 18-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///******************************************************************************************************* 
    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Hdn_Colonia_ID.Value = "";
        Txt_Busqueda_Colonia.Text = "";
        Txt_Busqueda_Clave_Colonia.Text = "";
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Buscar_Colonias_Click
    /// DESCRIPCIÓN: Carga en el grid la búsqueda con las Colonias
    /// PARÁMETROS: sender y e
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 18-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Buscar_Colonias_Click(object sender, EventArgs e)
    {
        Buscar_Colonias(0);
    }

    private void Buscar_Colonias(int Indice_Pagina)
    {
        String Mi_SQL = "";
        Cls_Ate_Colonias_Negocio Colonias = new Cls_Ate_Colonias_Negocio();
        DataTable Dt_Colonias;

        Mi_SQL = Cat_Ate_Colonias.Campo_Colonia_ID
                    + ", " + Cat_Ate_Colonias.Campo_Tipo_Colonia_ID
                    + ", " + Cat_Ate_Colonias.Campo_Nombre
                    + ", TO_NUMBER(" + Cat_Ate_Colonias.Campo_Colonia_ID + ") CLAVE_COLONIA"
                    + ", " + Cat_Ate_Colonias.Campo_Descripcion;
        Colonias.P_Campos_Dinamicos = Mi_SQL;
        Colonias.P_Filtros_Dinamicos = "";
        if (Txt_Busqueda_Clave_Colonia.Text.Trim() != "")
        {
            Colonias.P_Filtros_Dinamicos += Cat_Ate_Colonias.Campo_Colonia_ID + " LIKE '%" + Txt_Busqueda_Clave_Colonia.Text.Trim() + "%' AND ";
        }
        if (Txt_Busqueda_Colonia.Text.ToUpper().Trim() != "")
        {
            Colonias.P_Filtros_Dinamicos += Cat_Ate_Colonias.Campo_Nombre + " LIKE '%" + Txt_Busqueda_Colonia.Text.ToUpper().Trim() + "%' ";
        }
        // eliminar AND del final del filtro
        if (Colonias.P_Filtros_Dinamicos.EndsWith(" AND"))
        {
            Colonias.P_Filtros_Dinamicos = Colonias.P_Filtros_Dinamicos.Substring(0, Colonias.P_Filtros_Dinamicos.Length - 4);
        }
        Dt_Colonias = Colonias.Consultar_Colonias();
        Grid_Colonias.Columns[1].Visible = true;
        Grid_Colonias.DataSource = Dt_Colonias;
        Grid_Colonias.PageIndex = Indice_Pagina;
        Grid_Colonias.DataBind();
        Grid_Colonias.Columns[1].Visible = false;
    }

    protected void Grid_Colonias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Buscar_Colonias(e.NewPageIndex);
    }

    protected void Grid_Colonias_SelectedIndexChanged(object sender, EventArgs e)
    {
        Hdn_Colonia_ID.Value = Grid_Colonias.SelectedRow.Cells[1].Text;
        Txt_Busqueda_Colonia.Text = Grid_Colonias.SelectedRow.Cells[2].Text;
        Session["BUSQUEDA_COLONIAS"] = true;
        Session["COLONIA_ID"] = Hdn_Colonia_ID.Value;
        Session["NOMBRE_COLONIA"] = Txt_Busqueda_Colonia.Text;
        //Cierra la ventana
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", "window.close();", true);
    }
    
}
