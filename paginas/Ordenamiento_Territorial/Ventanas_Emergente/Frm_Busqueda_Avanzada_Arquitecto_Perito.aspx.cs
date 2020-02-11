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
using Presidencia.Acciones_AC.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Catalogo_Tramites.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Orden_Territorial_Bitacora_Documentos.Negocio;
using Presidencia.Ordenamiento_Territorial_Inspectores.Negocio;

public partial class paginas_Ordenamiento_Territorial_Ventanas_Emergente_Frm_Busqueda_Avanzada_Arquitecto_Perito : System.Web.UI.Page
{
    String Boton_Busqueda_Pulsado = "";
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS           : sender y e
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 30-mayo-2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        if (!IsPostBack)
        {
            Session["BUSQUEDA_ARQUITECTO"] = false;
        }
        Frm_Busqueda_Avanzada_Tramites.Page.Title = "Búsqueda Avanzada de Arquitecto";
        if (Session["Boton_Busqueda_Pulsado"] != null)
        {
            Boton_Busqueda_Pulsado = Session["Boton_Busqueda_Pulsado"].ToString();
        }


    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : NO APLICA
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 30-may-2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        Session["BUSQUEDA_ARQUITECTO"] = false;
        Session.Remove("INSPECTOR_ID");
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Limpiar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Limpiar
    ///PARAMETROS           : sender y e
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 30-may-2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Txt_Nombre.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Aceptar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Aceptar
    ///PARAMETROS           : sender y e
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 30-may-2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Session["BUSQUEDA_ARQUITECTO"] = true;
            //Cierra la ventana
            string Pagina = "<script language='JavaScript'>";
            Pagina += "window.close();";
            Pagina += "</script>";
            ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN      : Grid_Personal_SelectedIndexChanged
    ///DESCRIPCIÓN         : Maneja el Evento de Cambio de Selección del Grid 
    ///PARÁMETROS:
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 30-may-2012 
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Personal_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["INSPECTOR_ID"] = Grid_Personal.SelectedRow.Cells[1].Text;
            Btn_Aceptar_Click(sender, null);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Btn_Buscar_Click
    /// DESCRIPCION             : llama al método que realiza la búsqueda y carga los resultados en el grid
    ///CREO                     : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO               : 30-may-2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, EventArgs e)
    {
        try
        {
            Buscar_Personal(0);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Buscar_Personal
    /// DESCRIPCION             : buscara los formatos
    ///CREO                     : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO               : 19-Junio-2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Buscar_Personal(int Indice_Pagina)
    {
        Boolean Estado = false;
        Cls_Cat_Ort_Inspectores_Negocio Negocio_Arquitecto = new Cls_Cat_Ort_Inspectores_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            if (Txt_Nombre.Text != "")
            {
                Negocio_Arquitecto.P_Nombre= Txt_Nombre.Text.Trim();
                Estado = true;
            }
           

            if (Estado == true)
            {
                Dt_Consulta = Negocio_Arquitecto.Consultar_Inspectores();
                if (Dt_Consulta is DataTable)
                {
                    if (Dt_Consulta.Rows.Count > 0)
                    {
                        Grid_Personal.Columns[1].Visible = true;
                        Grid_Personal.DataSource = Dt_Consulta;
                        Grid_Personal.DataBind();
                        Grid_Personal.Columns[1].Visible = false;
                    }
                }
            }
            else
            {
                Dt_Consulta = Negocio_Arquitecto.Consultar_Inspectores();
                if (Dt_Consulta is DataTable)
                {
                    if (Dt_Consulta.Rows.Count > 0)
                    {
                        Grid_Personal.Columns[1].Visible = true;
                        Grid_Personal.DataSource = Dt_Consulta;
                        Grid_Personal.DataBind();
                        Grid_Personal.Columns[1].Visible = false;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }


}
