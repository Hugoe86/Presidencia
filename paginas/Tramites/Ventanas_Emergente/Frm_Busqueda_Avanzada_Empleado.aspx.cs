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
using Presidencia.Tramites_Perfiles_Empleados.Negocio;

public partial class paginas_Tramites_Ventanas_Emergente_Frm_Busqueda_Avanzada_Empleado : System.Web.UI.Page
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
            Session["BUSQUEDA_EMPLEADO"] = false;
            Llenar_Combo_Unidad_Responsable();
        }
        Frm_Busqueda_Avanzada_Ciudadano.Page.Title = "Búsqueda Avanzada de Empleado";
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
        Session["BUSQUEDA_EMPLEADO"] = false;
        Session.Remove("EMPLEADO_ID");
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
        Txt_Filtro_Nombre_Empleado.Text = "";
        Txt_Filtro_Numero_Empleado.Text = "";
        if (Cmb_Filtro_Unidad_Responsable.SelectedIndex > 0) 
            Cmb_Filtro_Unidad_Responsable.SelectedIndex = 0; 
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
            Cmb_Filtro_Unidad_Responsable.DataSource = Dt_Unidad_Responsable;
            Cmb_Filtro_Unidad_Responsable.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_Filtro_Unidad_Responsable.DataTextField = Cat_Dependencias.Campo_Nombre;
            Cmb_Filtro_Unidad_Responsable.DataBind();
            Cmb_Filtro_Unidad_Responsable.Items.Insert(0, "< SELECCIONE >");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
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
        Session["BUSQUEDA_EMPLEADO"] = true;
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }
    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN      : Grid_Empleado_SelectedIndexChanged
    ///DESCRIPCIÓN         : Maneja el Evento de Cambio de Selección del Grid 
    ///PARÁMETROS:
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 30-may-2012 
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Empleado_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["EMPLEADO_ID"] = Grid_Empleado.SelectedRow.Cells[1].Text.ToString();

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
        Buscar_Empleado();
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
    private void Buscar_Empleado()
    {
        DataTable Dt_Datos_Empleado = new DataTable();
        Cls_Ope_Tra_Perfiles_Empleado_Negocio Negocio_Filtro_Unidad_Responsable = new Cls_Ope_Tra_Perfiles_Empleado_Negocio();
        int Valor = 0;

        //  para el nombre
        if (!String.IsNullOrEmpty(Txt_Filtro_Nombre_Empleado.Text))
        {
            Negocio_Filtro_Unidad_Responsable.P_Nombre_Empleado = Txt_Filtro_Nombre_Empleado.Text.ToUpper().Trim();
        }
        //  para el numero de empleado
        if (!String.IsNullOrEmpty(Txt_Filtro_Numero_Empleado.Text))
        {
            //String.Format("{0:00000}", Convert.ToInt32(Reloj_Checador_ID) + 1);
            Negocio_Filtro_Unidad_Responsable.P_Numero_Empleado = String.Format("{0:000000}", Convert.ToInt32(Txt_Filtro_Numero_Empleado.Text.ToUpper().Trim()));
        }

        //  para la unidad responsable
        if (Cmb_Filtro_Unidad_Responsable.SelectedIndex > 0)
        {
            Negocio_Filtro_Unidad_Responsable.P_Unidad_Responsable_ID = Cmb_Filtro_Unidad_Responsable.SelectedValue;
        }
        //Consulta_Empleados_Dependencia
        Dt_Datos_Empleado = Negocio_Filtro_Unidad_Responsable.Consultar_Empleado();

        if (Dt_Datos_Empleado != null)
        {
            if (Dt_Datos_Empleado is DataTable)
            {
                if (Dt_Datos_Empleado.Rows.Count > 0)
                {
                    Grid_Empleado.Columns[1].Visible = true;
                    Grid_Empleado.DataSource = Dt_Datos_Empleado;
                    Grid_Empleado.DataBind();
                    Grid_Empleado.Columns[1].Visible = false;
                    Session["Dt_Empleados"] =  Dt_Datos_Empleado;
                }
            }
        }
    }


}
