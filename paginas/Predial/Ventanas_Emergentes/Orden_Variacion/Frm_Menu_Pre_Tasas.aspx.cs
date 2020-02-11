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
using Presidencia.Operacion.Predial_Tasas_Anuales.Negocio;

public partial class paginas_Predial_Ventanas_Emergentes_Orden_Variacion_Frm_Menu_Pre_Tasas : System.Web.UI.Page
{

    #region Variables
        
    #endregion
    #region Load/Init
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string Validacion = HttpUtility.HtmlDecode(Request.QueryString["Fecha"]).Trim();
                if (Validacion == "True")
                {
                    Txt_Ano_Tasa.Enabled = false;
                    Txt_Ano_Tasa.Text = Convert.ToString(DateTime.Now.Year);                   
                }
                else
                {
                    Txt_Ano_Tasa.Enabled = true;
                }
                if (Session["Dt_Tasas"] == null)
                Cargar_Grid(0);
            }
            Mensaje_Error();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion

    #region Metodos Grid
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid
    ///DESCRIPCIÓN: Realizar la consulta y llenar el grid con estos datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 21/Julio/2011 12:14:35 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Grid(int Page_Index)
    {
        Cls_Ope_Pre_Tasas_Anuales_Negocio Tasas_Negocio = new Cls_Ope_Pre_Tasas_Anuales_Negocio();
        DataTable Dt_Tasas;
        try
        {
            
            Tasas_Negocio.P_Tasa_Predial_ID = Txt_Tasa_ID.Text.Trim().ToUpper();
            Tasas_Negocio.P_Descripcion = Txt_Descripcion.Text.Trim().ToUpper();
            Tasas_Negocio.P_Anio = Txt_Ano_Tasa.Text.Trim().ToUpper();
            Tasas_Negocio.P_Identificador = Txt_Identificador.Text.Trim().ToUpper();
            Dt_Tasas = Tasas_Negocio.Consultar_Tasas_Anuales();
            Session["Dt_Tasas"] = Dt_Tasas;
            
            //Boolean Variable = Fecha;
            if (Dt_Tasas.Rows.Count > 0)
            {
                Grid_Tasas.PageIndex = Page_Index;
                Grid_Tasas.DataSource = Dt_Tasas;
                Grid_Tasas.DataBind();

            }
            else
            {
                Grid_Tasas.DataSource = null;
                Grid_Tasas.DataBind();
                Mensaje_Error("No se encontraron Registros");
            }
          
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    #endregion

    #region Eventos/Botones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        Session.Remove("Notario_ID");
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        //Page.RegisterStartupScript("Cerrar_Script", Pagina);
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }

    protected void Btn_Busqueda_Tasas_Click(object sender, EventArgs e)
    {
        try
        {
            Cargar_Grid(0);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    #endregion

    #region Metodos Generales
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        IBtn_Imagen_Error.Visible = true;
        Lbl_Mensaje_Error.Text += P_Mensaje + "</br>";
    }
    private void Mensaje_Error()
    {
        IBtn_Imagen_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
    }
    #endregion



    protected void Grid_Tasas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Tasas.SelectedIndex = (-1);
            Cargar_Grid(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Grid_Tasas_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt_Tasas;
        if (Session["Dt_Tasas"] != null)
        {
            Dt_Tasas = (DataTable)Session["Dt_Tasas"];
            //Dt_Row = Dt_Tasas.Select();
            Session["Dr_Tasa_Seleccionada"] = (Dt_Tasas.Rows[Grid_Tasas.SelectedIndex +
                        (Grid_Tasas.PageIndex * Grid_Tasas.PageSize)]);
            Session["Dt_Tasas"] = null;
            //Cierra la ventana
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", "window.close();", true);
        //ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }
    protected void Txt_Descripcion_TextChanged(object sender, EventArgs e)
    {
        Cargar_Grid(0);
    }
    protected void Txt_Identificador_TextChanged(object sender, EventArgs e)
    {
        Cargar_Grid(0);
    }
    protected void Txt_Ano_Tasa_TextChanged(object sender, EventArgs e)
    {
        Cargar_Grid(0);
    }
    protected void Txt_Tasa_ID_TextChanged(object sender, EventArgs e)
    {
        Cargar_Grid(0);
    }

}
