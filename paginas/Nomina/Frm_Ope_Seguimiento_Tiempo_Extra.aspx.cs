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
using Presidencia.Tiempo_Extra.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Empleados.Negocios;
using Presidencia.Dependencias.Negocios;
using Presidencia.Faltas_Empleado.Negocio;
using System.Globalization;
using System.Text;
using Presidencia.Ayudante_Informacion;

public partial class paginas_Nomina_Frm_Ope_Seguimiento_Tiempo_Extra : System.Web.UI.Page
{

    #region (Load)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Carga inicial de la pagina
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            String onmouseoverStyle = "this.style.backgroundColor='#DFE8F6';this.style.cursor='hand';this.style.color='DarkBlue';" +
                "this.style.borderStyle='none';this.style.borderColor='Silver';";

            if (!IsPostBack)
            {
                Hf_No_Hora_Extra.Value = (String)Request.QueryString["No_Hora_Extra"];
                Configuracion_Inicial();
            }

            Btn_Autorizar.Attributes.Add("onmouseover", onmouseoverStyle);
            Btn_Autorizar.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF';this.style.color='Black';this.style.borderStyle='none';");
            Btn_Cancelar.Attributes.Add("onmouseover", onmouseoverStyle);
            Btn_Cancelar.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF';this.style.color='Black';this.style.borderStyle='none';");

            Lbl_Mensaje_Error.Text = "";
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception Ex) {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    #endregion

    #region(Grid)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Empleados_Autorizar
    ///DESCRIPCIÓN: Consulta y busca a los empleados que podran aplicar para el Tiempo Extra
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Empleados_Autorizar() {
        Cls_Ope_Nom_Tiempo_Extra_Negocio Rs_Consulta_Ope_Horas_Extra = new Cls_Ope_Nom_Tiempo_Extra_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos del puesto
        Cls_Ope_Nom_Tiempo_Extra_Negocio Horas_Extra = null;
        try
        {
            Pnl_Gral_Autirizar_Hrs_Extra.Visible = true;
            Rs_Consulta_Ope_Horas_Extra.P_No_Tiempo_Extra = Hf_No_Hora_Extra.Value;
            Horas_Extra = Rs_Consulta_Ope_Horas_Extra.Consultar_Tiempo_Extra("", "");
            LLenar_Grid_Emplados_Autorizar(Horas_Extra.P_Dt_Empleados, 0);

            if (Grid_Emplados_Autorizar.Rows.Count == 0)
                Lbl_Autorizar.Text = HttpUtility.HtmlDecode("<b style='color:red;'>No se Encontraron Empleados</b>");
            else
                Lbl_Autorizar.Text = "";

        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Error producido al realizar la Busqueda. Error: [" + Ex.Message + "]";
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: LLenar_Grid_Emplados_Autorizar
    ///DESCRIPCIÓN: Carga el Grid con los empleados encontrados
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void LLenar_Grid_Emplados_Autorizar(DataTable Dt_Empleados_Autorizar, Int32 No_Pagina)
    {
        try
        {
            if (Dt_Empleados_Autorizar != null)
            {
                Grid_Emplados_Autorizar.Columns[1].Visible = true;
                Grid_Emplados_Autorizar.Columns[2].Visible = true;
                Grid_Emplados_Autorizar.Columns[2].ItemStyle.Width = new Unit(30, UnitType.Percentage);
                Grid_Emplados_Autorizar.Columns[4].Visible = true;
                Grid_Emplados_Autorizar.Columns[6].Visible = true;
                Grid_Emplados_Autorizar.PageIndex = No_Pagina;
                Grid_Emplados_Autorizar.DataSource = Dt_Empleados_Autorizar;
                Grid_Emplados_Autorizar.DataBind();
                Grid_Emplados_Autorizar.Columns[1].Visible = false;
                Grid_Emplados_Autorizar.Columns[4].Visible = false;
                Grid_Emplados_Autorizar.Columns[6].Visible = false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Consultar los Empleados que pertenecen a la hora extra seleccionada. Errro: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Emplados_Autorizar_OnSelectedIndexChanged
    ///DESCRIPCIÓN: Seleccionar un elemento del grid y cargar los controles.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Emplados_Autorizar_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Empleados_Negocios Cat_Empleados_Consulta = new Cls_Cat_Empleados_Negocios(); //Variable de conexión a la capa de Negocios para la consulta de los datos del puesto
        DataTable Dt_Datos_Empleado = null;//Variable que almacenara iuna lista de empleados
        try
        {
            Habilitar_Controles(true);
            Txt_Autorizacion_Empleado_ID.Text = Grid_Emplados_Autorizar.SelectedRow.Cells[1].Text;
            if (!string.IsNullOrEmpty(Grid_Emplados_Autorizar.SelectedRow.Cells[4].Text)) Txt_Autorizacion_Comentarios_Estatus.Text = HttpUtility.HtmlDecode(Grid_Emplados_Autorizar.SelectedRow.Cells[4].Text);

            //Consultamos datos del empleado.
            Cat_Empleados_Consulta.P_Empleado_ID = Grid_Emplados_Autorizar.SelectedRow.Cells[1].Text;
            Dt_Datos_Empleado = Cat_Empleados_Consulta.Consulta_Datos_Empleado();

            if (Dt_Datos_Empleado != null)
            {
                if (Dt_Datos_Empleado.Rows.Count > 0)
                {
                    Txt_autorizacion_No_Empleado.Text = Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString();
                    Txt_Autorizacion_Nombre_Empleado.Text = Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString() + " " + Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString() +
                       " " + Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString();
                }
            }

            if (Grid_Emplados_Autorizar.Columns[6].Visible)
            {
                //Grid_Emplados_Autorizar.Columns[6].Visible = false;
                //Grid_Emplados_Autorizar.Columns[2].Visible = true;
                //Grid_Emplados_Autorizar.Columns[2].ItemStyle.Width = new Unit(30, UnitType.Percentage);

                foreach (GridViewRow Fila in Grid_Emplados_Autorizar.Rows)
                {
                    Fila.Cells[6].Visible = false;
                    if (Fila.RowIndex == Grid_Emplados_Autorizar.SelectedIndex)
                    {
                        // Grid_Emplados_Autorizar.Rows[Grid_Emplados_Autorizar.SelectedIndex].Cells[6].Visible = false;
                        Fila.Cells[6].RowSpan = 3;
                        Grid_Emplados_Autorizar.Rows[Grid_Emplados_Autorizar.SelectedIndex].Cells[6].Visible = true;
                        Grid_Emplados_Autorizar.Rows[Grid_Emplados_Autorizar.SelectedIndex].Style.Add("background", "Silver url(../imagenes/paginas/titleBackground.png) repeat-x top");
                    }
                    else
                    {
                        if (Fila.Cells[3].Text.Trim().Equals("Rechazado"))
                        {
                            Fila.Style.Add("background", "#F78181 url(../imagenes/paginas/titleBackground.png) repeat-x top");
                        }
                        else
                        {
                            Fila.Style.Add("background", "#CEF6CE url(../imagenes/paginas/titleBackground.png) repeat-x top");
                        }
                    }
                }
            }
            else
            {
                Grid_Emplados_Autorizar.Columns[6].Visible = true;
                Grid_Emplados_Autorizar.Columns[2].Visible = false;
                Grid_Emplados_Autorizar.Columns[2].ItemStyle.Width = new Unit(0, UnitType.Percentage);
                foreach (GridViewRow Fila in Grid_Emplados_Autorizar.Rows)
                {
                    Fila.Cells[6].Visible = false;
                    if (Fila.RowIndex == Grid_Emplados_Autorizar.SelectedIndex)
                    {
                        Fila.Cells[6].RowSpan = 3;
                        Grid_Emplados_Autorizar.Rows[Grid_Emplados_Autorizar.SelectedIndex].Cells[6].Visible = true;
                        Grid_Emplados_Autorizar.Rows[Grid_Emplados_Autorizar.SelectedIndex].Style.Add("background", "Silver url(../imagenes/paginas/titleBackground.png) repeat-x top");
                    }
                    else
                    {
                        if (Fila.Cells[3].Text.Trim().Equals("Rechazado"))
                        {
                            Fila.Style.Add("background", "#F78181 url(../imagenes/paginas/titleBackground.png) repeat-x top");
                        }
                        else
                        {
                            Fila.Style.Add("background", "#CEF6CE url(../imagenes/paginas/titleBackground.png) repeat-x top");
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Error al seleccionar un elemento de la tabla de empleados. Errro: [" + Ex.Message + "]";
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Empleados_RowDataBound
    ///DESCRIPCIÓN: Es el evento previo antes cargar el grid con informacion de 
    ///los empleados
    ///PARAMETROS:  
    ///CREO: Juan Alberto Hernandez Negrete
    ///FECHA_CREO: 22/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Emplados_Autorizar_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                //Autorizar el dia festivo
                ((Button)e.Row.Cells[5].FindControl("Btn_Autorizar_Hora_Extra")).CommandArgument = e.Row.Cells[1].Text.Trim();
                ((Button)e.Row.Cells[5].FindControl("Btn_Autorizar_Hora_Extra")).ToolTip = "Autorizar Empleado " + HttpUtility.HtmlDecode(e.Row.Cells[2].Text);
                //Rechazar el dia festivo
                ((Button)e.Row.Cells[5].FindControl("Btn_Rechazar_Hora_Extra")).CommandArgument = e.Row.Cells[1].Text.Trim();
                ((Button)e.Row.Cells[5].FindControl("Btn_Rechazar_Hora_Extra")).ToolTip = "Rechazar Empleado " + HttpUtility.HtmlDecode(e.Row.Cells[2].Text);

                if (e.Row.Cells[3].Text.Contains("Rechazado"))
                {
                    e.Row.Style.Add("background", "#F78181 url(../imagenes/paginas/titleBackground.png) repeat-x top");
                }
                else
                {
                    e.Row.Style.Add("background", "#CEF6CE url(../imagenes/paginas/titleBackground.png) repeat-x top");
                }

                if (e.Row.Cells[4].Text.Length > 20)
                {
                    e.Row.Cells[4].ToolTip = e.Row.Cells[4].Text;
                    e.Row.Cells[4].Text = e.Row.Cells[4].Text.Substring(0, 19) + "...";
                }
                else
                {
                    e.Row.Cells[4].Text = e.Row.Cells[4].Text;
                }

                Agregar_Informacion_Etiqueta(((Label)e.Row.Cells[6].FindControl("Lbl_Informacion")), e.Row.Cells[1].Text.Trim());
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion

    #region (Metodos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///DESCRIPCIÓN: Carga inicial de la pagina
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial(){
        Consultar_Empleados_Autorizar();
        Habilitar_Controles(false);
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Habilitar_Controles
    ///DESCRIPCIÓN: Configuracion inicial de los controles de la pagina.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Habilitar_Controles(Boolean Habilitar) {
        Txt_Autorizacion_Empleado_ID.Enabled = false;
        Txt_Autorizacion_Comentarios_Estatus.Enabled = false;
        Btn_Autorizar.Enabled = false;
        Txt_Autorizacion_Nombre_Empleado.Enabled = false;
        Txt_autorizacion_No_Empleado.Enabled = false;

        Hf_Estatus.Value = "";
        Txt_Autorizacion_Empleado_ID.Text = "";
        Txt_Autorizacion_Comentarios_Estatus.Text = "";
        Txt_Autorizacion_Nombre_Empleado.Text = "";
        Txt_autorizacion_No_Empleado.Text = "";
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cambio_Estatus_Tiempo_Extra_Empelado
    ///DESCRIPCIÓN: Ejecuta la Autorizacion de las Horas del Empleado Seleccionado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cambio_Estatus_Tiempo_Extra_Empelado() {
        Cls_Ope_Nom_Tiempo_Extra_Negocio Rs_Consulta_Ope_Horas_Extra = new Cls_Ope_Nom_Tiempo_Extra_Negocio(); 
        try
        {
            Rs_Consulta_Ope_Horas_Extra.P_Empleado_ID = Txt_Autorizacion_Empleado_ID.Text;
            Rs_Consulta_Ope_Horas_Extra.P_No_Tiempo_Extra = Hf_No_Hora_Extra.Value.Trim();
            Rs_Consulta_Ope_Horas_Extra.P_Estatus = Hf_Estatus.Value;
            Rs_Consulta_Ope_Horas_Extra.P_Comentarios_Estatus = Txt_Autorizacion_Comentarios_Estatus.Text;

            if (Rs_Consulta_Ope_Horas_Extra.Cambiar_Estatus_Hora_Extra_Empleados()) {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operacion Exitosa');", true);
                Configuracion_Inicial();         
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cambiar el estatus de las Horas Extra para los empleados. Errro: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Eventos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Clear_Ctlr_Click
    ///DESCRIPCIÓN: Limpiar los controles del formulario
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Clear_Ctlr_Click(object sender, EventArgs e) {
        Txt_Autorizacion_Empleado_ID.Text = "";
        Txt_Autorizacion_Comentarios_Estatus.Text = "";
        Grid_Emplados_Autorizar.SelectedIndex = -1;
        Habilitar_Controles(false);
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Autorizar_Click
    ///DESCRIPCIÓN: Autorizar las Horas Extra para el empleado selccionado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Autorizar_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(Txt_Autorizacion_Comentarios_Estatus.Text))
            {
                Cambio_Estatus_Tiempo_Extra_Empelado();
                Consultar_Empleados_Autorizar();
                Habilitar_Controles(true);
                Grid_Emplados_Autorizar.Enabled = true;
            }
            else {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                if (Txt_Autorizacion_Comentarios_Estatus.Text == "")
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Comentarios del Estatus <br>";
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Error al Autorizar el Tiempo extra para el empleado seleccionado. Errro: [" + Ex.Message + "]";
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cancelar_Click
    ///DESCRIPCIÓN: Ejecuta la cancelacion de la operacion actual.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Cancelar_Click(object sender, EventArgs e) {        
        Response.Redirect("Frm_Ope_Tiempo_Extra_Empleados.aspx?PAGINA=" + Request.QueryString["PAGINA"]);  
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Rechazar_Hora_Extra
    ///DESCRIPCIÓN: Rechaza la autorizacion de las Horas Extra
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 22/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Rechazar_Hora_Extra_Click(object sender, EventArgs e)
    {
        Button Btn_Rechazar_Hora_Extra = (Button)sender;
        Cls_Cat_Empleados_Negocios Cat_Empleados_Consulta = new Cls_Cat_Empleados_Negocios(); //Variable de conexión a la capa de Negocios para la consulta de los datos del puesto
        DataTable Dt_Datos_Empleado = null;//Variable que almacenara iuna lista de empleados
        try
        {
            Txt_Autorizacion_Comentarios_Estatus.Enabled = true;
            Btn_Autorizar.Enabled = true;
            Txt_Autorizacion_Comentarios_Estatus.Text = "";
            Txt_Autorizacion_Empleado_ID.Text = Btn_Rechazar_Hora_Extra.CommandArgument.Trim();
            Hf_Estatus.Value = "Rechazado";
            Grid_Emplados_Autorizar.Enabled = false;

            //Consultamos datos del empleado.
            Cat_Empleados_Consulta.P_Empleado_ID = Btn_Rechazar_Hora_Extra.CommandArgument.Trim();
            Dt_Datos_Empleado = Cat_Empleados_Consulta.Consulta_Datos_Empleado();

            if (Dt_Datos_Empleado != null)
            {
                if (Dt_Datos_Empleado.Rows.Count > 0)
                {
                    Txt_autorizacion_No_Empleado.Text = Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString();
                    Txt_Autorizacion_Nombre_Empleado.Text = Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString() + " " + Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString() +
                       " " + Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString();
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Error al Autorizar las Horas Extras para el empleado seleccionado. Errro: [" + Ex.Message + "]";
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Autorizar_Hora_Extra_Click
    ///DESCRIPCIÓN: DESCRIPCIÓN: Acepta la autorizacion del tiempo extra
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 22/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Autorizar_Hora_Extra_Click(object sender, EventArgs e)
    {
        Button Btn_Autorizar_Hora_Extra = (Button)sender;

        try
        {
            Txt_Autorizacion_Empleado_ID.Text = Btn_Autorizar_Hora_Extra.CommandArgument.Trim();
            Hf_Estatus.Value = "Autorizado";
            Txt_Autorizacion_Comentarios_Estatus.Text = "";
            Cambio_Estatus_Tiempo_Extra_Empelado();
            Consultar_Empleados_Autorizar();
            Habilitar_Controles(false);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Error al Autorizar las Horas Extras para el empleado seleccionado. Errro: [" + Ex.Message + "]";
        }
    }
    #endregion

    ///*********************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Informacion_Empleado
    ///
    ///DESCRIPCIÓN: Obtiene la informacion del empleado y del tipo de incidencia que se
    ///             captura actualmente al empleado.
    ///
    ///PARAMETROS:  Empleado_ID.- Identificador único del empleado. 
    ///             No_Tiempo_Extra.- Hora Extra que el empleado laborara en presidencia.
    ///
    ///CREO: Juan Alberto Hernandez Negrete
    ///FECHA_CREO: 17/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*********************************************************************************************
    private DataTable Obtener_Informacion_Empleado(String Empleado_ID, String No_Tiempo_Extra)
    {
        Cls_Ope_Nom_Tiempo_Extra_Negocio Obj_Tiempo_Extra = new Cls_Ope_Nom_Tiempo_Extra_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        DataTable Dt_Empleados = null;                                              //Variable que almacenara la información del empleado consultada. 
        DataTable Dt_Informacion_Incidencias = new DataTable();                     //Variable que almacenara la informacion del emnpleado y de la incidencia que se captura.
        DataTable Dt_Tiempo_Extra = null;                                           //Variable que almacenara la informacion del dia festivo consultado.    
        DataRow Fila = null;                                                        //Variable que almacenara el registro que se insertara en la estructura que almacena la informacion de la incidencia del empleado.
        Double Horas_Extra= 0;                                                       //Variable que almacena el identificador del empleado.                                              

        try
        {
            //Creamos la estructura que tendra la tabla que almacenara la informacion de la incidencia del empleado.
            Dt_Informacion_Incidencias.Columns.Add("NO_EMPLEADO", typeof(String));
            Dt_Informacion_Incidencias.Columns.Add("NOMBRE_EMPLEADO", typeof(String));
            Dt_Informacion_Incidencias.Columns.Add("PAGO_POR_HORA", typeof(String));
            Dt_Informacion_Incidencias.Columns.Add("PAGO_TOTAL_HORAS", typeof(String));
            Dt_Informacion_Incidencias.Columns.Add("PUESTO", typeof(String));
            Dt_Informacion_Incidencias.Columns.Add("DEPENDENCIA", typeof(String));
            //Se realiza la consulta de los datos de la incidencia del empleado.
            Obj_Empleados.P_Empleado_ID = Empleado_ID;
            Dt_Empleados = Obj_Empleados.Consultar_Informacion_Empleado_Incidencias();

            if (Dt_Empleados is DataTable)
            {
                if (Dt_Empleados.Rows.Count > 0)
                {
                    foreach (DataRow Empleado in Dt_Empleados.Rows)
                    {
                        if (Empleado is DataRow)
                        {

                            Fila = Dt_Informacion_Incidencias.NewRow();

                            if (!String.IsNullOrEmpty(Empleado["NO_EMPLEADO"].ToString()))
                            {
                                Fila["NO_EMPLEADO"] = Empleado["NO_EMPLEADO"].ToString();
                                Fila["DEPENDENCIA"] = Obtener_Nombre_Unidad_Responsable(Empleado["NO_EMPLEADO"].ToString());
                            }

                            if (!String.IsNullOrEmpty(Empleado["NOMBRE_EMPLEADO"].ToString()))
                                Fila["NOMBRE_EMPLEADO"] = Empleado["NOMBRE_EMPLEADO"].ToString();

                            if (!String.IsNullOrEmpty(Empleado["PAGO_DIA_NORMAL"].ToString()))
                                Fila["PAGO_POR_HORA"] = String.Format("{0:c}", (Cls_Ayudante_Nom_Informacion.Obtener_Cantidad_Diaria(Empleado_ID) / 8));

                            Obj_Tiempo_Extra.P_No_Tiempo_Extra = No_Tiempo_Extra;
                            Dt_Tiempo_Extra = Obj_Tiempo_Extra.Consultar_Tiempo_Extra("", "").P_Dt_Horas_Extra;

                            if (Dt_Tiempo_Extra is DataTable)
                            {
                                if (Dt_Tiempo_Extra.Rows.Count > 0)
                                {
                                    foreach (DataRow Tiempo_Extra in Dt_Tiempo_Extra.Rows)
                                    {
                                        if (Tiempo_Extra is DataRow)
                                        {
                                            if (!String.IsNullOrEmpty(Tiempo_Extra[Ope_Nom_Tiempo_Extra.Campo_Horas].ToString()))
                                            {
                                                Horas_Extra = Convert.ToDouble(Tiempo_Extra[Ope_Nom_Tiempo_Extra.Campo_Horas].ToString());

                                                if (!String.IsNullOrEmpty(Empleado["PAGO_DIA_NORMAL"].ToString()))
                                                    Fila["PAGO_TOTAL_HORAS"] = String.Format("{0:c}", ((Cls_Ayudante_Nom_Informacion.Obtener_Cantidad_Diaria(Empleado_ID) / 8) * Horas_Extra));

                                                if (!String.IsNullOrEmpty(Empleado["PUESTO"].ToString()))
                                                    Fila["PUESTO"] = Empleado["PUESTO"].ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            Dt_Informacion_Incidencias.Rows.Add(Fila);//Se agrega el registro con la información de la incidencia del empleado.
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener la información que se desplegara del empleado. Error: [" + Ex.Message + "]");
        }
        return Dt_Informacion_Incidencias;
    }
    ///*********************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Agregar_Informacion_Etiqueta
    ///
    ///DESCRIPCIÓN: Agrega la informacion del empleado del tipo de incidencia que se
    ///             captura actualmente al empleado.
    ///
    ///PARAMETROS:  Empleado_ID.- Identificador único del empleado. 
    ///             Etiqueta.- Control que contiene toda la informacion que se desplegara en el 
    ///                        la tabla de empleados.
    ///
    ///CREO: Juan Alberto Hernandez Negrete
    ///FECHA_CREO: 17/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*********************************************************************************************
    private void Agregar_Informacion_Etiqueta(Label Etiqueta, String Empleado_ID)
    {
        DataTable Dt_Informacion_Incidencias = null;//Variable que almacena la informacion de la incidencia del empleado.
        StringBuilder Tooltip = new StringBuilder();//Variable que almacena el mensaje que contendra toda la informacion que se mostrara
        //de la incidencia del empleado.

        try
        {
            //Se consulta la informacion del empleado que se mostrara al seleccionar al empleado en la lista de los empleados
            //posibles a autorizar el dia festivo como un dia laboral.
            Dt_Informacion_Incidencias = Obtener_Informacion_Empleado(Empleado_ID, Hf_No_Hora_Extra.Value.Trim());

            if (Dt_Informacion_Incidencias is DataTable)
            {
                if (Dt_Informacion_Incidencias.Rows.Count > 0)
                {
                    foreach (DataRow Incidencia in Dt_Informacion_Incidencias.Rows)
                    {
                        if (Incidencia is DataRow)
                        {
                            Tooltip.Append("<b style='color:white;'>No Empleado:</b> ");
                            if (!String.IsNullOrEmpty(Incidencia["NO_EMPLEADO"].ToString()))
                                Tooltip.Append(Incidencia["NO_EMPLEADO"].ToString());
                            Tooltip.Append("<br />");

                            Tooltip.Append("<b style='color:white;'>Nombre:</b> ");
                            if (!String.IsNullOrEmpty(Incidencia["NOMBRE_EMPLEADO"].ToString()))
                                Tooltip.Append(Incidencia["NOMBRE_EMPLEADO"].ToString());
                            Tooltip.Append("<br />");

                            Tooltip.Append("<b style='color:white;'>Pago Por Hora:</b> ");
                            if (!String.IsNullOrEmpty(Incidencia["PAGO_POR_HORA"].ToString()))
                                Tooltip.Append("&nbsp;" + Incidencia["PAGO_POR_HORA"].ToString());
                            Tooltip.Append("<br />");

                            Tooltip.Append("<b style='color:white;'>Pago Total Horas:</b> ");
                            if (!String.IsNullOrEmpty(Incidencia["PAGO_TOTAL_HORAS"].ToString()))
                                Tooltip.Append("&nbsp;" + Incidencia["PAGO_TOTAL_HORAS"].ToString());
                            Tooltip.Append("<br />");

                            Tooltip.Append("<b style='color:white;'>Puesto:</b> ");
                            if (!String.IsNullOrEmpty(Incidencia["PUESTO"].ToString()))
                                Tooltip.Append(Incidencia["PUESTO"].ToString());
                            Tooltip.Append("<br />");

                            Tooltip.Append("<b style='color:white;'>Unidad Reponsable:</b> ");
                            if (!String.IsNullOrEmpty(Incidencia["DEPENDENCIA"].ToString()))
                                Tooltip.Append(Incidencia["DEPENDENCIA"].ToString());
                            Tooltip.Append("<br />");

                            Etiqueta.Text = HttpUtility.HtmlDecode(Tooltip.ToString());
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al agregar la informacion de la incidencia al empleado. Error: [" + Ex.Message + "]");
        }
    }

    private String Obtener_Nombre_Unidad_Responsable(String No_Empleado)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();
        Cls_Cat_Dependencias_Negocio Obj_Dependencias = new Cls_Cat_Dependencias_Negocio();
        DataTable Dt_Empleado = null;
        DataTable Dt_Dependencia = null;
        String Nombre_Dependencia = String.Empty;

        try
        {
            Obj_Empleados.P_No_Empleado = No_Empleado;
            Dt_Empleado = Obj_Empleados.Consulta_Empleados_General();

            if (Dt_Empleado is DataTable) {
                if (Dt_Empleado.Rows.Count > 0) {
                    foreach (DataRow EMPLEADO in Dt_Empleado.Rows) {
                        if (EMPLEADO is DataRow) {
                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Dependencia_ID].ToString().Trim()))
                            {
                                Obj_Dependencias.P_Dependencia_ID = EMPLEADO[Cat_Empleados.Campo_Dependencia_ID].ToString().Trim();
                                Dt_Dependencia = Obj_Dependencias.Consulta_Dependencias();

                                if (Dt_Dependencia is DataTable) {
                                    if (Dt_Dependencia.Rows.Count > 0) { 
                                        foreach(DataRow DEPENDENCIA in Dt_Dependencia.Rows){
                                            if (DEPENDENCIA is DataRow) {
                                                if (!String.IsNullOrEmpty(DEPENDENCIA[Cat_Dependencias.Campo_Nombre].ToString().Trim())) {
                                                    Nombre_Dependencia = DEPENDENCIA[Cat_Dependencias.Campo_Nombre].ToString().Trim();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener el nombre de la unidad responsable del empleado. Error: [" + Ex.Message + "]");
        }
        return Nombre_Dependencia;
    }
}
