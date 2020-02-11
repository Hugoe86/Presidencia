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
using Presidencia.Operacion_Predial_Recepcion_Documentos.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;

public partial class paginas_Predial_Ventanas_Emergentes_Recepcion_Documentos_Frm_Pre_Busqueda_Notarios : System.Web.UI.Page
{
    
    #region Load/Init
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Btn_Aceptar.Visible = false;   
            }
            Mensaje_Error();

            
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);            
        }
    }
    #endregion



    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Busqueda_Notarios_Click
    /// DESCRIPCION : Ejecuta la búsqueda de notarios mediante el modal popup 
    ///             Mpe_Busqueda_Notarios y llena el grid Notarios con los resultados
    /// CREO        : Roberto González Oseguera
    /// FECHA_CREO  : 01-abr-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Busqueda_Notarios_Click(object sender, EventArgs e)
    {
        try
        {   
            {
                Cargar_Grid(0);                
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error();   
        }
    }

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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Aceptar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Aceptar
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        
        try
        {
            
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
        

    }
    #region Metodos/Grids
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
        Cls_Ope_Pre_Recepcion_Documentos_Negocio Notarios_Negocio = new Cls_Ope_Pre_Recepcion_Documentos_Negocio();
        DataTable Dt_Notarios;
        try
        {
                Notarios_Negocio.P_Estatus_Notario = "VIGENTE";
            if (Txt_Numero_Notaria.Text.Trim() != "")
                Notarios_Negocio.P_Numero_Notaria = Txt_Numero_Notaria.Text.Trim();
            if (Txt_No_Notario.Text.Trim() != "")
                Notarios_Negocio.P_Notario_ID = Txt_No_Notario.Text.Trim();
            if (Txt_Nombre_Notario.Text != "")
                Notarios_Negocio.P_Nombre_Notario = Txt_Nombre_Notario.Text;
            if (Txt_RFC.Text != "")
                Notarios_Negocio.P_RFC_Notario = Txt_RFC.Text.Trim();

            Dt_Notarios = Notarios_Negocio.Consulta_Notarios();
            Session["Dt_Busqueda_Notarios"] = Dt_Notarios;
            if (Dt_Notarios.Rows.Count > 0)
            {
                Grid_Notarios.PageIndex = Page_Index;
                Grid_Notarios.DataSource = (DataTable)Session["Dt_Busqueda_Notarios"];
                Grid_Notarios.DataBind();
            }
            else
            {
                Grid_Notarios.DataSource = null;
                Grid_Notarios.DataBind();
                Mensaje_Error("No se encontraron Registros");
            }


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


    #region Metodos ABC
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos
    ///DESCRIPCIÓN: metodo que recibe el DataRow seleccionado de la grilla y carga los datos en los componetes de l formulario
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 02:07:54 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos(DataRow Dr_Notarios)
    {
        try
        {
            Txt_No_Notario.Text = Dr_Notarios[Cat_Pre_Notarios.Campo_Notario_ID].ToString();
            Txt_Numero_Notaria.Text = Dr_Notarios[Cat_Pre_Notarios.Campo_Numero_Notaria].ToString();
            Txt_RFC.Text = Dr_Notarios[Cat_Pre_Notarios.Campo_RFC].ToString();
            Txt_Nombre_Notario.Text = Dr_Notarios["NOMBRE_COMPLETO"].ToString();

            
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    } 
    #endregion


    protected void Grid_Notarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Notarios.SelectedIndex = (-1);
            Cargar_Grid(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }


    protected void Grid_Notarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataRow Notario;
        try
        {
            if (Grid_Notarios.SelectedIndex > (-1))
            {
                //Cargar_Datos(((DataTable)Session["Dt_Busqueda_Notarios"]).Rows[Grid_Notarios.SelectedIndex +
                    //(Grid_Notarios.PageIndex * Grid_Notarios.PageSize)]);
                Notario = ((DataTable)Session["Dt_Busqueda_Notarios"]).Rows[Grid_Notarios.SelectedIndex +
                    (Grid_Notarios.PageIndex * Grid_Notarios.PageSize)];

                Session["Notario_Datos"] = Notario; // Notario[Cat_Pre_Notarios.Campo_Notario_ID];
                Response.Write("<script>var $w = window.self; $w.opener = window.self; $w.close();</script>");
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }    
}