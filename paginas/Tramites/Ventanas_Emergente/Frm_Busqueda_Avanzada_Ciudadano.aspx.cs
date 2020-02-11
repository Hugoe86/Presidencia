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
using Presidencia.Ventanilla_Lista_Tramites.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Cls_Cat_Ven_Registro_Usuarios.Negocio;

public partial class paginas_Tramites_Ventanas_Emergente_Frm_Busqueda_Avanzada_Ciudadano : System.Web.UI.Page
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
            Session["BUSQUEDA_CIUDADANO"] = false;
        }
        Frm_Busqueda_Avanzada_Ciudadano.Page.Title = "Búsqueda Avanzada de Ciudadano";
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
        Session["BUSQUEDA_CIUDADANO"] = false;
        Session.Remove("CIUDADANO_ID");
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
        Txt_Filtro_Apellido_Materno.Text = "";
        Txt_Filtro_Apellido_Paterno.Text = "";
        Txt_Filtro_Curp.Text = "";
        Txt_Filtro_Email.Text = "";
        Txt_Filtro_Nombre.Text = "";
        Txt_Filtro_RFC.Text = "";
        Txt_Filtro_Telefono.Text = "";
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
        Session["BUSQUEDA_CIUDADANO"] = true;
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN      : Grid_Tramites_Generales_SelectedIndexChanged
    ///DESCRIPCIÓN         : Maneja el Evento de Cambio de Selección del Grid 
    ///PARÁMETROS:
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 30-may-2012 
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Tramites_Generales_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["CIUDADANO_ID"] = Grid_Ciudadano.SelectedRow.Cells[1].Text.ToString();

            Btn_Aceptar_Click(sender, null);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Btn_Buscar_Solicitante_Click
    /// DESCRIPCION             : llama al método que realiza la búsqueda y carga los resultados en el grid
    ///CREO                     : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO               : 30-may-2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Buscar_Solicitante_Click(object sender, EventArgs e)
    {
        Buscar_Ciudadano();
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
    private void Buscar_Ciudadano()
    {
        Cls_Cat_Ven_Registro_Usuarios_Negocio Clase_Consulta_Usuario = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
        DataTable Dt_Consulta = new DataTable();
        String Usuario_ID = "";
        int Valor = 0;

        //  filtro nombre
        if (Txt_Filtro_Nombre.Text != "")
            Clase_Consulta_Usuario.P_Nombre = Txt_Filtro_Nombre.Text;

        //  filtro apellido paterno
        if (Txt_Filtro_Apellido_Paterno.Text != "")
            Clase_Consulta_Usuario.P_Apellido_Paterno = Txt_Filtro_Apellido_Paterno.Text;

        //  filtro apellido materno
        if (Txt_Filtro_Apellido_Materno.Text != "")
            Clase_Consulta_Usuario.P_Apellido_Materno = Txt_Filtro_Apellido_Materno.Text;

        //  filtro rfc
        if (Txt_Filtro_RFC.Text != "")
            Clase_Consulta_Usuario.P_Rfc = Txt_Filtro_RFC.Text;

        //  filtro curp
        if (Txt_Filtro_Curp.Text != "")
            Clase_Consulta_Usuario.P_Curp = Txt_Filtro_Curp.Text;

        //  filtro email
        if (Txt_Filtro_Email.Text != "")
            Clase_Consulta_Usuario.P_Email = Txt_Filtro_Email.Text;

        //  filtro telefono o celular
        if (Txt_Filtro_Telefono.Text != "")
            Clase_Consulta_Usuario.P_Telefono_Casa = Txt_Filtro_Telefono.Text;

        //  se realiza la consulta
        Dt_Consulta = Clase_Consulta_Usuario.Consultar_Usuario_Soliucitante();

        if (Dt_Consulta != null)
        {
            if (Dt_Consulta is DataTable)
            {
                if (Dt_Consulta.Rows.Count > 0)
                {
                    Grid_Ciudadano.Columns[1].Visible = true;
                    Grid_Ciudadano.DataSource = Dt_Consulta;
                    Grid_Ciudadano.DataBind();
                    Grid_Ciudadano.Columns[1].Visible = false;
                }
            }
        }
    }
}
