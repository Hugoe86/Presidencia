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

public partial class paginas_Tramites_Ventanas_Emergente_Frm_Busqueda_Avanzada_Tramites : System.Web.UI.Page
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
            Session["BUSQUEDA_TRAMITES"] = false;
            Llenar_Combo_Unidad_Responsable();
        }
        Frm_Busqueda_Avanzada_Tramites.Page.Title = "Búsqueda Avanzada de Tramites";
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
        Session["BUSQUEDA_TRAMITES"] = false;
        Session.Remove("TRAMITE_ID");
        Session.Remove("NOMBRE_TRAMITE");
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
        Txt_Nombre_Tramite_Filtro.Text = "";
        Txt_Clave_Tramite_Filtro.Text = "";

        if (Cmb_Unidad_Responsable_Filtro.Items.Count > 0 && Cmb_Unidad_Responsable_Filtro.SelectedIndex > 0)
            Cmb_Unidad_Responsable_Filtro.SelectedIndex = 0;
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
            Session["BUSQUEDA_TRAMITES"] = true;
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
            Session["TRAMITE_ID"] = Grid_Tramites_Generales.SelectedRow.Cells[1].Text;
            //Session["NOMBRE_TRAMITE"] = Grid_Tramites_Generales.SelectedRow.Cells[3].Text;

            Btn_Aceptar_Click(sender, null);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Btn_Buscar_Tramite_Filtro_Click
    /// DESCRIPCION             : llama al método que realiza la búsqueda y carga los resultados en el grid
    ///CREO                     : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO               : 30-may-2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Buscar_Tramite_Filtro_Click(object sender, EventArgs e)
    {
        try
        {
            Buscar_Tramites(0);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Llenar_Combo_Unidad_Responsable
    /// DESCRIPCION             : llama el combo de unidad responsable
    ///CREO                     : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO               : 30-may-2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llenar_Combo_Unidad_Responsable()
    {
        Cls_Cat_Dependencias_Negocio Rs_Responsable = new Cls_Cat_Dependencias_Negocio();
        DataTable Dt_Unidad_Responsable = new DataTable();
        try
        {
            //  1 para la unidad resposable
            Dt_Unidad_Responsable = Rs_Responsable.Consulta_Dependencias();
            //   2 SE ORDENA LA TABLA POR 
            DataView Dv_Ordenar = new DataView(Dt_Unidad_Responsable);
            Dv_Ordenar.Sort = Cat_Dependencias.Campo_Nombre;
            Dt_Unidad_Responsable = Dv_Ordenar.ToTable();
            Cmb_Unidad_Responsable_Filtro.DataSource = Dt_Unidad_Responsable;
            Cmb_Unidad_Responsable_Filtro.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_Unidad_Responsable_Filtro.DataTextField = Cat_Dependencias.Campo_Nombre;
            Cmb_Unidad_Responsable_Filtro.DataBind();
            Cmb_Unidad_Responsable_Filtro.Items.Insert(0, "< SELECCIONE >");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
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
    private void Buscar_Tramites(int Indice_Pagina)
    {
        Boolean Estado = false;
        Cls_Ope_Ven_Lista_Tramites_Negocio Rs_Consulta_Tramites = new Cls_Ope_Ven_Lista_Tramites_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            if (Txt_Nombre_Tramite_Filtro.Text != "")
            {
                Rs_Consulta_Tramites.P_Nombre_Tramite = Txt_Nombre_Tramite_Filtro.Text;
            }
            if (Txt_Clave_Tramite_Filtro.Text != "")
            {
                Rs_Consulta_Tramites.P_Clave_Tramite = Txt_Clave_Tramite_Filtro.Text;
            }

            if (Cmb_Unidad_Responsable_Filtro.SelectedIndex > 0)
            {
                Rs_Consulta_Tramites.P_Dependencia_Tramite = Cmb_Unidad_Responsable_Filtro.SelectedValue;
            }
            Rs_Consulta_Tramites.P_Estatus = Cmb_Estatus_Tramite.SelectedValue;


            Dt_Consulta = Rs_Consulta_Tramites.Consultar_Tramites();
            if (Dt_Consulta is DataTable)
            {
                if (Dt_Consulta.Rows.Count > 0)
                {
                    Grid_Tramites_Generales.Columns[1].Visible = true;
                    Grid_Tramites_Generales.DataSource = Dt_Consulta;
                    Grid_Tramites_Generales.DataBind();
                    Grid_Tramites_Generales.Columns[1].Visible = false;
                }
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }


}
