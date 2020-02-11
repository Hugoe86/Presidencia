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
using Presidencia.Catalogo_Tramites.Negocio;

public partial class paginas_Tramites_Ventanas_Emergente_Frm_Busqueda_Avanzada_Cuenta_Contable : System.Web.UI.Page
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
            Session["BUSQUEDA_CUENTA"] = false;
        }
        Frm_Busqueda_Avanzada_Ciudadano.Page.Title = "Búsqueda Avanzada de Cuentas contables";
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
        Session["BUSQUEDA_CUENTA"] = false;
        Session.Remove("CUENTA_CLAVE");
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
        Txt_Filtro_Cuenta.Text = "";
        Txt_Filtro_Nombre.Text = "";
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
        Session["BUSQUEDA_CUENTA"] = true;
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }
    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN      : Grid_Cuentas_SelectedIndexChanged
    ///DESCRIPCIÓN         : Maneja el Evento de Cambio de Selección del Grid 
    ///PARÁMETROS:
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 30-may-2012 
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Cuentas_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["CUENTA_CONTABLE_ID"] = Grid_Cuentas.SelectedRow.Cells[1].Text.ToString();
            Session["CUENTA_CLAVE"] = Grid_Cuentas.SelectedRow.Cells[2].Text.ToString();
            Session["COSTO_CUENTA"] = Grid_Cuentas.SelectedRow.Cells[5].Text.ToString();

            Btn_Aceptar_Click(sender, null);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Btn_Buscar_Cuenta_Click
    /// DESCRIPCION             : llama al método que realiza la búsqueda y carga los resultados en el grid
    ///CREO                     : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO               : 30-may-2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Buscar_Cuenta_Click(object sender, EventArgs e)
    {
        Buscar_Cuenta_Contable();
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Buscar_Tramites
    /// DESCRIPCION             : buscara los tramites
    ///CREO                     : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO               : 30-may-2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Buscar_Cuenta_Contable()
    {
        Cls_Cat_Tramites_Negocio Clase_Consulta_Usuario = new Cls_Cat_Tramites_Negocio();
        DataTable Dt_Consulta = new DataTable();
        String Usuario_ID = "";
        int Valor = 0;

        //  filtro nombre
        if (Txt_Filtro_Nombre.Text != "")
            Clase_Consulta_Usuario.P_Nombre_Cuenta = Txt_Filtro_Nombre.Text;

        //  filtro cuenta
        if (Txt_Filtro_Cuenta.Text != "")
            Clase_Consulta_Usuario.P_Cuenta = Txt_Filtro_Cuenta.Text;


        //  se realiza la consulta
        Dt_Consulta = Clase_Consulta_Usuario.Consultar_Cuenta_Contable();

        if (Dt_Consulta != null)
        {
            if (Dt_Consulta is DataTable)
            {
                if (Dt_Consulta.Rows.Count > 0)
                {
                    Grid_Cuentas.Columns[1].Visible = true;
                    Grid_Cuentas.Columns[4].Visible = true;
                    Grid_Cuentas.DataSource = Dt_Consulta;
                    Grid_Cuentas.DataBind();
                    Grid_Cuentas.Columns[1].Visible = false;
                    Grid_Cuentas.Columns[4].Visible = false;
                }
                else
                {
                    Grid_Cuentas.Columns[1].Visible = true;
                    Grid_Cuentas.Columns[4].Visible = true;
                    Grid_Cuentas.DataSource = new DataTable();
                    Grid_Cuentas.DataBind();
                    Grid_Cuentas.Columns[1].Visible = false;
                    Grid_Cuentas.Columns[4].Visible = false;
                }
            }
        }

    }
}

