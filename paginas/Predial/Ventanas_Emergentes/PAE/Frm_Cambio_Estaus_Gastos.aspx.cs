using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Constantes;
using Presidencia.Catalogo_Gastos_Ejecucion.Negocio;

public partial class paginas_Predial_Ventanas_Emergentes_PAE_Frm_Cambio_Estaus_Gastos : System.Web.UI.Page
{
    string No_Detalle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            No_Detalle = Request.QueryString["NO_DETALLE_ETAPA"].ToString();
            Session["AGREGA_GASTOS_PAE"] = false;
            Session["NO_DETALLE_ETAPA"] = No_Detalle;
            Cargar_Combo_Gastos_Ejecucion();
        }
        //Frm_Seguimiento_Pae_Gastos.Page.Title = "Gastos de Ejecución";
        Lbl_Title.Text = "Gastos de Ejecución";
        Mensaje_Error();
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Armando Zavala Moreno.
    ///FECHA_CREO  : 17-Abril-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        IBtn_Imagen_Error.Visible = true;
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Ecabezado_Mensaje.Text += P_Mensaje;// +"</br>";
        Div_Contenedor_Msj_Error.Visible = true;

    }
    private void Mensaje_Error()
    {
        IBtn_Imagen_Error.Visible = false;
        Lbl_Ecabezado_Mensaje.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Gastos_Ejecucion_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo para cargar el costo del gasto de ejecucion en un TextBox
    ///PARAMETROS: 
    ///CREO: Angel Antonio Escamilla Trejo 
    ///FECHA_CREO: 23/03/2012 04:53:12 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Gastos_Ejecucion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable Dt_Gastos = (DataTable)Session["Gastos"];
            String Valor = Cmb_Gastos_Ejecucion.SelectedItem.Text;
            for (int Cont_Gastos = 0; Cont_Gastos < Dt_Gastos.Rows.Count; Cont_Gastos++)
            {
                if (Valor == Dt_Gastos.Rows[Cont_Gastos]["NOMBRE"].ToString() && Dt_Gastos.Rows[Cont_Gastos]["COSTO"].ToString() != null)
                {
                    Txt_Costo.Text = Dt_Gastos.Rows[Cont_Gastos]["COSTO"].ToString();
                    Txt_Costo.ReadOnly = true;
                    break;
                }
                else
                {
                    Txt_Costo.ReadOnly = false;
                    Txt_Costo.Text = "";
                    break;
                }

            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Gastos_Ejecucion
    ///DESCRIPCIÓN: Metodo usado para cargar el listado de los gastos de ejecucion
    ///PARAMETROS: 
    ///CREO: Armando Zavala Moreno
    ///FECHA_CREO: 06/03/2012 12:22:12 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combo_Gastos_Ejecucion()
    {
        DataTable Dt_Gastos = new DataTable();
        try
        {
            Cls_Cat_Pre_Gastos_Ejecucion_Negocio Rs_Gastos = new Cls_Cat_Pre_Gastos_Ejecucion_Negocio();
            Rs_Gastos.P_Filtro = "";
            Cmb_Gastos_Ejecucion.DataTextField = Cat_Pre_Gastos_Ejecucion.Campo_Descripcion;
            Cmb_Gastos_Ejecucion.DataValueField = Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID;

            Dt_Gastos = Rs_Gastos.Consultar_Gastos_Ejecucion();
            Session["Gastos"] = Dt_Gastos;
            foreach (DataRow Dr_Fila in Dt_Gastos.Rows)
            {
                if (Dr_Fila[Cat_Pre_Gastos_Ejecucion.Campo_Estatus].ToString() != "VIGENTE")//Busca el estatus
                {
                    Dr_Fila.Delete();//Borra el registro                    
                    break;
                }
            }
            Cmb_Gastos_Ejecucion.DataSource = Dt_Gastos;
            Cmb_Gastos_Ejecucion.DataBind();
            Cmb_Gastos_Ejecucion.Items.Insert(0, new ListItem("<-- SELECCIONE -->", "0"));
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Honorarios
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para los Honorarios
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/03/2012 01:20:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Honorarios()
    {
        DataTable Dt_Honorarios = new DataTable();
        Dt_Honorarios.Columns.Add(new DataColumn("GASTO_EJECUCION_ID", typeof(String)));
        Dt_Honorarios.Columns.Add(new DataColumn("TIPO_DE_GASTO", typeof(String)));
        Dt_Honorarios.Columns.Add(new DataColumn("IMPORTE", typeof(Decimal)));
        return Dt_Honorarios;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_Honorarios
    ///DESCRIPCIÓN          : Agrega una nueva fila a la tabla de Honorarios
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/02/2012 01:49:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_DataRow_Honorarios(DataTable Dt_Honorarios)
    {
        DataRow Dr_Honorario;
        Dr_Honorario = Dt_Honorarios.NewRow();
        Dr_Honorario["GASTO_EJECUCION_ID"] = Cmb_Gastos_Ejecucion.SelectedValue;
        Dr_Honorario["TIPO_DE_GASTO"] = Cmb_Gastos_Ejecucion.SelectedItem.Text;
        Dr_Honorario["IMPORTE"] = Txt_Costo.Text;
        Dt_Honorarios.Rows.Add(Dr_Honorario);//Se asigna la nueva fila a la tabla
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Honorarios
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para los Honorarios
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/03/2012 01:20:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Cambio()
    {
        DataTable Dt_Honorarios = new DataTable();
        Dt_Honorarios.Columns.Add(new DataColumn("ESTATUS", typeof(String)));
        Dt_Honorarios.Columns.Add(new DataColumn("MOTIVO", typeof(String)));
        Dt_Honorarios.Columns.Add(new DataColumn("RESOLUCION", typeof(Decimal)));
        return Dt_Honorarios;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_Cambio
    ///DESCRIPCIÓN          : Agrega una nueva fila a la tabla de Honorarios
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/02/2012 01:49:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_DataRow_Cambio(DataTable Dt_Cambio)
    {
        DataRow Dr_Nueva_Fila;
        Dr_Nueva_Fila = Dt_Cambio.NewRow();
        Dr_Nueva_Fila["ESTATUS"] = Cmb_Gastos_Ejecucion.SelectedValue;
        Dr_Nueva_Fila["MOTIVO"] = Cmb_Gastos_Ejecucion.SelectedItem.Text;
        Dr_Nueva_Fila["RESOLUCION"] = Txt_Costo.Text;
        Dt_Cambio.Rows.Add(Dr_Nueva_Fila);//Se asigna la nueva fila a la tabla
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_Cambio
    ///DESCRIPCIÓN          : Agrega una nueva fila a la tabla de Honorarios
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/02/2012 01:49:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected Boolean Validar_Guardar_Cambio()
    { 
        Boolean Correcto=true;
        DataTable Dt_Cambio = Crear_Tabla_Cambio();
        if (Cmb_Estatus_Etapa.SelectedIndex > 0 || Txt_Motivo.Text.Length > 0 || Txt_Resolucion.Text.Length > 0)
        {
            if (Cmb_Estatus_Etapa.SelectedIndex < 1)
            {
                Lbl_Ecabezado_Mensaje.Text += "Selecciona un estatus";
                Correcto = false;
            }
            if (Txt_Motivo.Text.Length < 1)
            {
                Lbl_Ecabezado_Mensaje.Text += "Introduce un motivo de cambio de estatus";
                Correcto = false;
            }
            if (Correcto)
            {
                Llenar_DataRow_Cambio(Dt_Cambio);
                Session["CAMBIOS"] = Dt_Cambio;
            }
        }
        return Correcto;
    }
    #region Eventos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Agregar_Gasto_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 14/Marzo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Agregar_Gasto_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Honorarios;
        Boolean Validacion = true;
        String Mensaje = "";
        Mensaje_Error();
        if (Session["HONORARIOS"] != null)
        {
            Dt_Honorarios = (DataTable)Session["HONORARIOS"];
        }
        else
        {
            Dt_Honorarios = Crear_Tabla_Honorarios();
        }

        if (Cmb_Gastos_Ejecucion.SelectedIndex < 1)
        {
            Mensaje += "Selecciona un gasto de ejecucion <br>";
            Validacion = false;
        }
        if (Txt_Costo.Text.Length < 1)
        {
            Mensaje += "Introduce un costo de ejecucion";
            Validacion = false;
        }
        if (Validacion == false)
        {
            Mensaje_Error(Mensaje);
        }
        else
        {
            Llenar_DataRow_Honorarios(Dt_Honorarios);
            Grid_Gastos_Ejecucion.DataSource = Dt_Honorarios;
            Grid_Gastos_Ejecucion.DataBind();
            Cargar_Combo_Gastos_Ejecucion();
            Txt_Costo.Text = "";
            Session["HONORARIOS"] = Dt_Honorarios;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Aceptar_Click
    ///DESCRIPCIÓN          : Acepta el motivo de omision
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 13/Marzo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Honorarios = (DataTable)Session["HONORARIOS"];
        DataTable Dt_Cambio = (DataTable)Session["CAMBIOS"];
        Mensaje_Error();

        try
        {
            if (Validar_Guardar_Cambio())
            {
                //Si inserta y despues son borrados la tabla ya no es nula pero no tiene datos y marca error
                if (Dt_Honorarios != null && Dt_Honorarios.Rows.Count < 1) { Dt_Honorarios = null; }

                if (Dt_Honorarios != null || Dt_Cambio.Rows.Count > 0)
                {
                    Session["AGREGA_GASTOS"] = true;
                    //Cierra la ventana
                    string Pagina = "<script language='JavaScript'>";
                    Pagina += "window.close();";
                    Pagina += "</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
                }
                else
                {
                    IBtn_Imagen_Error.Visible = true;
                    Lbl_Ecabezado_Mensaje.Text = "No se introdujo ningun dato para guardar";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 14/Marzo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        Session["AGREGA_GASTOS_PAE"] = false;
        Session.Remove("HONORARIOS");
        Session.Remove("NO_DETALLE_ETAPA");
        Session.Remove("CAMBIOS");
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }
    #endregion
    #region Grids
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Gastos_Ejecucion_SelectedIndexChanged
    ///DESCRIPCIÓN          : Borra un Registro del Grid Gastos Ejecucion
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 08/03/2012 05:53:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Gastos_Ejecucion_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt_Honorarios = (DataTable)Session["HONORARIOS"];
        Dt_Honorarios.Rows.RemoveAt(Grid_Gastos_Ejecucion.SelectedIndex);
        Grid_Gastos_Ejecucion.DataSource = Dt_Honorarios;
        Grid_Gastos_Ejecucion.DataBind();
        Session["HONORARIOS"] = Dt_Honorarios;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Gastos_Ejecucion_PageIndexChanging
    ///DESCRIPCIÓN          : Cambia de pagina en el Grid Gastos de ejecucion
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 08/03/2012 05:53:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Gastos_Ejecucion_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Gastos_Ejecucion.PageIndex = e.NewPageIndex;
            Grid_Gastos_Ejecucion.DataSource = Session["HONORARIOS"];
            Grid_Gastos_Ejecucion.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion
}
