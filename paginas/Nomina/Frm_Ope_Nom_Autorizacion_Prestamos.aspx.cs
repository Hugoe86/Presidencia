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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Text.RegularExpressions;
using Presidencia.Empleados.Negocios;
using Presidencia.Dependencias.Negocios;
using Presidencia.Prestamos.Negocio;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Proveedores.Negocios;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.DateDiff;
using Presidencia.Cat_Parametros_Nomina.Negocio;
using Presidencia.Utilidades_Nomina;
using Presidencia.Bancos_Nomina.Negocio;
using System.Collections.Generic;
using Presidencia.Ayudante_Calendario_Nomina;

public partial class paginas_Nomina_Frm_Ope_Nom_Autorizacion_Prestamos : System.Web.UI.Page
{

    #region (Page_Load)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Carga la configuracion inicial de la pagina.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Inicial();
            }
            Btn_Autorizacion_Prestamos.Visible = false;
            Btn_Cancelacion_Prestamo.Visible = false;
            Lbl_Observaciones_Autorizacion.Text = "";
            Lbl_Observaciones_Cancelacion.Text = "";  
            
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
    }
    #endregion

    #region (Grid)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Prestamos_OnSelectedIndexChanged
    ///DESCRIPCIÓN: Carga los datos del prestamo seleccionado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 02/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Prestamos_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Nom_Pestamos_Negocio Consulta_Solicitudes_Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Empleados_Negocios Consulta_Empleados =new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        DataTable Dt_Solicitudes_Prestamos = null;//Variable que almacenara una lista de solicitudes de prestamos.
        DataTable Dt_Empleados = null;//Variable que almacenara una lista de empleados.
        String No_Empleado_Solicitante = "";//Variable que alamcenara el numero del empleado que realiza la solicitud del prestamo.
        String No_Empleado_Aval ="";//Variable que almacena el no de empleado que sera el aval.

        try
        {
            if (Grid_Prestamos.SelectedIndex != -1)
            {
                //Filtros de Busqueda
                Consulta_Solicitudes_Prestamos.P_No_Solicitud = Grid_Prestamos.SelectedRow.Cells[1].Text;
                Dt_Solicitudes_Prestamos = Consulta_Solicitudes_Prestamos.Consulta_Solicitudes_Prestamos().P_Dt_Solicitudes_Prestamos;

                if (Dt_Solicitudes_Prestamos != null)
                {
                    if (Dt_Solicitudes_Prestamos.Rows.Count > 0)
                    {
                        //Consultamos el Numero de Empleado Solicitante
                        Consulta_Empleados.P_Empleado_ID = Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID].ToString();
                        Dt_Empleados = Consulta_Empleados.Consulta_Datos_Empleado();

                        if (Dt_Empleados != null)
                        {
                            if (Dt_Empleados.Rows.Count > 0)
                            {
                                No_Empleado_Solicitante = Dt_Empleados.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString();
                            }
                        }

                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID].ToString()))
                        {
                            //Consultamos el Numero de Empleado Aval
                            Consulta_Empleados.P_Empleado_ID = Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Aval_Empleado_ID].ToString();
                            Dt_Empleados = Consulta_Empleados.Consulta_Datos_Empleado();

                            if (Dt_Empleados != null)
                            {
                                if (Dt_Empleados.Rows.Count > 0)
                                {
                                    No_Empleado_Aval = Dt_Empleados.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString();
                                }
                            }
                            Consulta_Datos_Empleado_Aval(No_Empleado_Aval);
                            Txt_No_Empleado_Aval.Text = No_Empleado_Aval;
                        }

                        //Se realiza la carga de los controles con la informacion del prestamo seleccionado.
                        Consultar_Datos_Empleado_Con_Solicitud_Prestamo(No_Empleado_Solicitante);                        
                        Txt_No_Empleado_Solicitante_Prestamo.Text = No_Empleado_Solicitante;                        
                        Txt_No_Solocitud.Text = Grid_Prestamos.SelectedRow.Cells[1].Text;

                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Solicitud].ToString().Trim())) Txt_Fecha_Solicitud_Prestamo_CalendarExtender.SelectedDate = Convert.ToDateTime(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Solicitud].ToString().Trim());
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Solicitud].ToString().Trim())) Cmb_Estatus_Solicitud_Prestamo.SelectedIndex = Cmb_Estatus_Solicitud_Prestamo.Items.IndexOf(Cmb_Estatus_Solicitud_Prestamo.Items.FindByText(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Solicitud].ToString().Trim()));
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Nomina_ID].ToString().Trim()))
                        {
                            Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf(Cmb_Calendario_Nomina.Items.FindByValue(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Nomina_ID].ToString().Trim()));
                            Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());//Consultamos los periodos catorcenales de la nomina seleccionada.
                            if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Nomina].ToString().Trim())) Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Periodos_Catorcenales_Nomina.Items.IndexOf(Cmb_Periodos_Catorcenales_Nomina.Items.FindByText(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Nomina].ToString().Trim()));
                        }

                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Inicio_Pago].ToString().Trim())) Txt_Fecha_Inicio_Pago_Prestamo.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Inicio_Pago].ToString().Trim()));
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Termino_Pago].ToString().Trim())) Txt_Fecha_Termino_Pago_Prestamo.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Termino_Pago].ToString().Trim()));
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Motivo_Prestamo].ToString().Trim())) Txt_Finalidad_Prestamo.Text = Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Motivo_Prestamo].ToString();
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Importe_Prestamo].ToString().Trim())) Txt_Importe_Prestamo.Text = string.Format("{0:#,###,##0.00}", Convert.ToDouble(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Importe_Prestamo].ToString().Trim()));
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Importe_Interes].ToString().Trim())) Txt_Importe_Interes.Text = string.Format("{0:#,###,##0.00}", Convert.ToDouble(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Importe_Interes].ToString().Trim()));
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Total_Prestamo].ToString().Trim())) Txt_Total_Prestamo.Text = string.Format("{0:#,###,##0.00}", Convert.ToDouble(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Total_Prestamo].ToString().Trim()));
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Pagos].ToString().Trim())) Txt_No_Pagos.Text = Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Pagos].ToString().Trim();
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Abono].ToString().Trim())) Txt_Abono.Text = string.Format("{0:#,###,##0.00}", Convert.ToDouble(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Abono].ToString().Trim()));
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Abono].ToString().Trim())) Txt_No_Abonos.Text = Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_No_Abono].ToString().Trim();
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual].ToString().Trim())) Txt_Saldo_Actual.Text = string.Format("{0:#,###,##0.00}", Convert.ToDouble(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual].ToString().Trim()));
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Proveedor_ID].ToString().Trim())) Cmb_Proveedor.SelectedIndex = Cmb_Proveedor.Items.IndexOf(Cmb_Proveedor.Items.FindByValue(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Proveedor_ID].ToString()));
                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Proveedor_ID].ToString().Trim()))
                        {
                            Cmb_Proveedor.SelectedIndex = Cmb_Proveedor.Items.IndexOf(Cmb_Proveedor.Items.FindByValue(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Proveedor_ID].ToString()));
                            Consulta_Deducciones(Cmb_Proveedor.SelectedValue.Trim());
                            if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Percepcion_Deduccion_ID].ToString().Trim())) Cmb_Deduccion.SelectedIndex = Cmb_Deduccion.Items.IndexOf(Cmb_Deduccion.Items.FindByValue(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Percepcion_Deduccion_ID].ToString()));
                        }

                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Aplica_Validaciones].ToString().Trim()))
                            Chk_Omitir_Validaciones.Checked = (Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Aplica_Validaciones].ToString().Trim().Equals("SI")) ? true : false;

                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Comentarios_Cancelacion].ToString().Trim()))
                        {
                            Txt_Motivo_Cancelacion_Rechazo.Text = Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Comentarios_Cancelacion].ToString().Trim();
                            Div_Comentarios.Visible = true;
                            Lbl_Motivo_Cancelacion_Rechazo_Prestamo.Text = "Motivo Cancelación";
                        }

                        if (!string.IsNullOrEmpty(Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Comentarios_Rechazo].ToString().Trim()))
                        {
                            Txt_Motivo_Cancelacion_Rechazo.Text = Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Comentarios_Rechazo].ToString().Trim();
                            Div_Comentarios.Visible = true;
                            Lbl_Motivo_Cancelacion_Rechazo_Prestamo.Text = "Motivo Rechazo";
                        }

                        if (Cmb_Estatus_Solicitud_Prestamo.SelectedItem.Text.Trim().ToUpper().Equals("PENDIENTE"))
                        {
                            Btn_Autorizacion_Prestamos.Visible = true;
                            Configuracion_Acceso_Autorizar_Cancelar("Frm_Ope_Nom_Autorizacion_Prestamos.aspx");
                            Btn_Cancelacion_Prestamo.Visible = false;
                        }
                        else if (Cmb_Estatus_Solicitud_Prestamo.SelectedItem.Text.Trim().ToUpper().Equals("AUTORIZADO") &&
                            (Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo].ToString().Trim().ToUpper().Equals("PROCESO") ||
                            Dt_Solicitudes_Prestamos.Rows[0][Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo].ToString().Trim().ToUpper().Equals("PENDIENTE")))
                        {
                            Btn_Cancelacion_Prestamo.Visible = true;
                            Configuracion_Acceso_Autorizar_Cancelar("Frm_Ope_Nom_Autorizacion_Prestamos.aspx");
                            Btn_Autorizacion_Prestamos.Visible = false;
                        }
                    }
                }
                Mpe_Busqueda_Prestamos.Hide();
                Limpiar_Controles_Busqueda_Solicitudes_Prestamos();                
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al seleccionar un prestamo del Grid de prestamos. Error: [" + Ex.Message + "]");
        }        
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Prestamos_RowDataBound
    ///DESCRIPCIÓN: Es el evento previo antes cargar el grid con informacion de 
    ///los empleados
    ///PARAMETROS:  
    ///CREO: Juan Alberto Hernandez Negrete
    ///FECHA_CREO: 03/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Prestamos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                if (e.Row.Cells[3].Text.Contains("Pendiente"))
                {
                    e.Row.Style.Add("background", "#F5F6CE url(../imagenes/paginas/titleBackground.png) repeat-x top");
                }
                else if (e.Row.Cells[3].Text.Contains("Autorizado"))
                {
                    e.Row.Style.Add("background", "#CEF6CE url(../imagenes/paginas/titleBackground.png) repeat-x top");
                }
                else if (e.Row.Cells[3].Text.Contains("Rechazado"))
                {
                    e.Row.Style.Add("background", "#F78181 url(../imagenes/paginas/titleBackground.png) repeat-x top");
                }
                else if (e.Row.Cells[3].Text.Contains("Cancelado"))
                {
                    e.Row.Style.Add("background", "#81BEF7 url(../imagenes/paginas/titleBackground.png) repeat-x top");
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Prestamos_PageIndexChanging
    /// DESCRIPCION : Cambiar de pagina ala tabla de Prestamos
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 03/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Prestamos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Consulta_Solicitudes_Prestamos(e.NewPageIndex);//Consultamos las solicitudes de prestamos.
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Error al cambiar la pagina del grid de solicitudes de prestamos. Error: [" + Ex.Message.ToString() + "]";
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: LLenar_Grid_Prestamos
    /// DESCRIPCION : Carga el grid con la lista de Prestamos. y cambia a la pagina indicada.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 03/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void LLenar_Grid_Prestamos(DataTable Dt_Prestamos, Int32 Pagina)
    {
        Grid_Prestamos.PageIndex = Pagina;
        Grid_Prestamos.DataSource = Dt_Prestamos;
        Grid_Prestamos.DataBind();
        Grid_Prestamos.SelectedIndex = -1;
    }
    #endregion

    #region (Metodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///DESCRIPCIÓN: Configuracion Inicial de los controles del Formulario.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Limpiar_Controles();//Limpia los controles de la pagina.
        Consultar_Calendario_Nominas();
        Consultar_Proveedores();
        Habilitar_Controles("Inicial");//Habilita la configuracion inicial de los controles
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 01/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            //Controles Datos Solicitud Prestamo
            Txt_No_Solocitud.Text = "";
            Txt_Fecha_Solicitud_Prestamo.Text = "";
            //Cmb_Estatus_Solicitud_Prestamo.SelectedIndex = -1;
            Cmb_Calendario_Nomina.SelectedIndex = -1;
            Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;
            Txt_Fecha_Inicio_Pago_Prestamo.Text = "";
            Txt_Fecha_Termino_Pago_Prestamo.Text = "";
            Txt_Finalidad_Prestamo.Text = "";
            Txt_Importe_Prestamo.Text = "";
            Txt_Importe_Interes.Text = "";
            Txt_Total_Prestamo.Text = "";
            Txt_No_Pagos.Text = "";
            Txt_Abono.Text = "";
            Txt_No_Abonos.Text = "";
            Txt_Saldo_Actual.Text = "";
            Cmb_Proveedor.SelectedIndex = -1;
            Cmb_Deduccion.SelectedIndex = -1;
            Chk_Aplica_Porcentaje.Checked = false;
            Lbl_Importe_Interes.Text = "Importe Interes";
            Chk_Omitir_Validaciones.Checked = false;
            //Controles Datos Empleado Solicita Credito
            Limpiar_Controles_Empleado_Solicitante();
            //Controles Datos Empleado Aval
            Limpiar_Controles_Empleado_Aval();

            Lbl_Motivo_Cancelacion_Rechazo_Prestamo.Text = "";
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al limpiar los controles de la pagina. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles_Empleado_Solicitante
    /// DESCRIPCION : Limpia los Controles del Empleado Solicitante.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 02/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles_Empleado_Solicitante()
    {
        try
        {
            //Controles Datos Empleado Solicita Credito
            Txt_No_Empleado_Solicitante_Prestamo.Text = "";
            Txt_Nombre_Empleado_Solicitante.Text = "";
            Img_Foto_Empleado_Solicitante.ImageUrl = "~/paginas/imagenes/paginas/Sias_No_Disponible.JPG";
            Txt_RFC_Empleado_Solicitante.Text = "";
            Txt_Fecha_Ingreso_Empleado_Solicitante.Text = "";
            Txt_Sindicato_Empleado_Solicitante.Text = "";
            Txt_Dependencia_Empelado_Solicitante.Text = "";
            Txt_Direccion_Empleado_Solicitante.Text = "";
            Txt_Cuenta_Bancaria_Empleado_Solicitante.Text = "";
            Txt_Sueldo_Mensual_Empleado_Solicitante.Text = "";
            Txt_Banco_Empleado_Solicitante.Text = "";
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al limpiar los controles de la pagina. Error: [" + Ex + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles_Empleado_Aval
    /// DESCRIPCION : Limpia los Controles del Empleado Aval.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 02/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles_Empleado_Aval()
    {
        try
        {
            //Controles Datos Empleado Aval
            Txt_No_Empleado_Aval.Text = "";
            Img_Empleado_Aval.ImageUrl = "~/paginas/imagenes/paginas/Sias_No_Disponible.JPG";
            Img_Empleado_Aval.DataBind();
            Txt_Nombre_Empleado_Aval.Text = "";
            Txt_RFC_Aval.Text = "";
            Txt_Sindicato_Aval.Text = "";
            Txt_Dependencia_Aval.Text = "";
            Txt_Direccion_Aval.Text = "";
            Txt_Fecha_Ingreso_Aval.Text = "";
            Txt_Sueldo_Mensual_Aval.Text = "";

            Txt_Clase_Nomina_Empleado.Text = String.Empty;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al limpiar los controles de la pagina. Error: [" + Ex + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles_Busqueda_Solicitudes_Prestamos
    /// DESCRIPCION : Limpia los Controles de la busqueda de prestamos.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 02/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles_Busqueda_Solicitudes_Prestamos()
    {
        try
        {
            //Controles Datos la busqueda.
            Txt_Busqueda_No_Solicitud.Text = "";
            Txt_Busqueda_Empleado_Solicitante.Text = "";
            Txt_Busqueda_Empleado_Aval.Text = "";
            //Limpiar Grid_Prestamos
            Grid_Prestamos.DataSource = new DataTable();
            Grid_Prestamos.DataBind();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al limpiar los controles de la pagina. Error: [" + Ex + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 01/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado;//Variable que sirve para almacenar el estatus de los controles habilitado o deshabilitado.

        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    //Mensajes de Error.
                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    //Habilitar el Boton de Busqueda de Prestamos
                    Btn_Busqueda_Prestamos.Enabled = !Habilitado;

                    Configuracion_Acceso("Frm_Ope_Nom_Autorizacion_Prestamos.aspx");
                    break;
                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    //Deshabilitar el Boton de Busqueda al realizar la operacion de nuevo
                    Btn_Busqueda_Prestamos.Enabled = !Habilitado;
                    //Obtenemos la fecha del dia de hoy.
                    Txt_Fecha_Solicitud_Prestamo_CalendarExtender.SelectedDate = DateTime.Now;
                    Cmb_Proveedor.SelectedIndex = 22;
                    Cmb_Proveedor_SelectedIndexChanged(Cmb_Proveedor, null);
                    Cmb_Deduccion.SelectedIndex = 1;
                    Txt_Finalidad_Prestamo.Text = "Gastos Personales";

                    Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf(
                        Cmb_Calendario_Nomina.Items.FindByText(new Cls_Ayudante_Calendario_Nomina().P_Anyo));

                    if (Cmb_Calendario_Nomina.SelectedIndex > 0)
                    {
                        Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
                        Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Periodos_Catorcenales_Nomina.Items.IndexOf(
                            Cmb_Periodos_Catorcenales_Nomina.Items.FindByText(new Cls_Ayudante_Calendario_Nomina().P_Periodo));

                        Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged(Cmb_Periodos_Catorcenales_Nomina, new EventArgs());
                    }
                    break;
                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    //Deshabilitar el Boton de Busqueda al realizar la operacion de modificar
                    Btn_Busqueda_Prestamos.Enabled = !Habilitado;
                    break;
            }
            //Controles Datos Solicitud Prestamo
            Txt_No_Solocitud.Enabled = false;
            Txt_Fecha_Solicitud_Prestamo.Enabled = false;
            Btn_Fecha_Solicitud_Prestamo.Enabled = false;
            Cmb_Estatus_Solicitud_Prestamo.Enabled = false;
            Cmb_Calendario_Nomina.Enabled = Habilitado;
            Cmb_Periodos_Catorcenales_Nomina.Enabled = Habilitado;
            Txt_Fecha_Inicio_Pago_Prestamo.Enabled = false;
            Txt_Fecha_Termino_Pago_Prestamo.Enabled = false;
            Txt_Finalidad_Prestamo.Enabled = Habilitado;
            Txt_Importe_Prestamo.Enabled = Habilitado;
            Txt_Importe_Interes.Enabled = Habilitado;
            Txt_Total_Prestamo.Enabled = false;
            Txt_No_Pagos.Enabled = Habilitado;
            Txt_Abono.Enabled = false;
            Txt_No_Abonos.Enabled = false;
            Txt_Saldo_Actual.Enabled = false;
            Cmb_Proveedor.Enabled = Habilitado;
            Cmb_Deduccion.Enabled = false;
            Chk_Aplica_Porcentaje.Enabled = Habilitado;
            Chk_Omitir_Validaciones.Enabled = Habilitado;
            //Controles Datos Empleado Solicita Credito
            Txt_No_Empleado_Solicitante_Prestamo.Enabled = Habilitado;
            Btn_Buscar_Empleado_Solicitante.Enabled = Habilitado;
            Txt_Nombre_Empleado_Solicitante.Enabled = false;
            Img_Foto_Empleado_Solicitante.Enabled = false;
            Txt_RFC_Empleado_Solicitante.Enabled = false;
            Txt_Fecha_Ingreso_Empleado_Solicitante.Enabled = false;
            Txt_Sindicato_Empleado_Solicitante.Enabled = false;
            Txt_Dependencia_Empelado_Solicitante.Enabled = false;
            Txt_Direccion_Empleado_Solicitante.Enabled = false;
            Txt_Cuenta_Bancaria_Empleado_Solicitante.Enabled = false;
            Txt_Sueldo_Mensual_Empleado_Solicitante.Enabled = false;
            //Controles Datos Empleado Aval
            Txt_No_Empleado_Aval.Enabled = Habilitado;
            Btn_Buscar_Empleado_Aval.Enabled = Habilitado;
            Img_Empleado_Aval.Enabled = false;
            Txt_Nombre_Empleado_Aval.Enabled = false;
            Txt_RFC_Aval.Enabled = false;
            Txt_Sindicato_Aval.Enabled = false;
            Txt_Dependencia_Aval.Enabled = false;
            Txt_Direccion_Aval.Enabled = false;
            Txt_Fecha_Ingreso_Aval.Enabled = false;
            Txt_Sueldo_Mensual_Aval.Enabled = false;
            //Opciones de Autorizacion de Prestamos
            Btn_Autorizacion_Prestamos.Visible = false;
            Chk_Autorizar.Checked = true;
            Div_Comentarios.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Vacaciones_Empleado
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Vacaciones_Empleado()
    {
        Boolean Datos_Validos = true;//Variable que alamacenara el resultado de la validacion de los datos ingresados por el usuario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (string.IsNullOrEmpty(Txt_No_Empleado_Solicitante_Prestamo.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + No Empleado <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Fecha_Solicitud_Prestamo.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha de Solicitud Prestamo <br>";
            Datos_Validos = false;
        }
        else if (!Validar_Formato_Fecha(Txt_Fecha_Solicitud_Prestamo.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Formato de la Fecha de  Solicitud de Prestamo Invalido  <br>";
            Datos_Validos = false;
        }

        if (Cmb_Estatus_Solicitud_Prestamo.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione el Estatus de la Solicitud del Prestamo <br>";
            Datos_Validos = false;
        }

        if (Cmb_Calendario_Nomina.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione aque nomina correspondera el prestamo  <br>";
            Datos_Validos = false;
        }

        if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione el Periodo catorcenal a partir del cual se comenzara a descontar. <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Fecha_Inicio_Pago_Prestamo.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha de Inicio de Pago del Prestamo <br>";
            Datos_Validos = false;
        }
        else if (!Validar_Formato_Fecha(Txt_Fecha_Inicio_Pago_Prestamo.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Formato de la Fecha de Inicio de Pago del Prestamo Invalido  <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Fecha_Termino_Pago_Prestamo.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha de Termino de Pago del Prestamo <br>";
            Datos_Validos = false;
        }
        else if (!Validar_Formato_Fecha(Txt_Fecha_Termino_Pago_Prestamo.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Formato de la Fecha de Termino de Pago del Prestamo Invalido  <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Finalidad_Prestamo.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Ingrese cual sera la finalidad o uso del prestamo. <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Importe_Prestamo.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Ingrese el Importe del Prestamo <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Total_Prestamo.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Total. <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_No_Pagos.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Falta Numero de pagos que debera realizar el empleado para liquidar el prestamo. <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Abono.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Falta Cantidad catorcenal a descontar al empleado. <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Saldo_Actual.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Indica el Saldo Actual del prestamo. <br>";
            Datos_Validos = false;
        }

        if (Cmb_Proveedor.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione el proveedor. <br>";
            Datos_Validos = false;
        }

        if (Cmb_Deduccion.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione la Deduccion. <br>";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Formato_Fecha
    /// DESCRIPCION : Valida el formato de las fechas.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 01/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Formato_Fecha(String Fecha)
    {
        String Cadena_Fecha = @"^(([0-9])|([0-2][0-9])|([3][0-1]))\/(ene|feb|mar|abr|may|jun|jul|ago|sep|oct|nov|dic)\/\d{4}$";
        if (Fecha != null) return Regex.IsMatch(Fecha, Cadena_Fecha);
        else return false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 01/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean IsNumeric(String Cadena)
    {
        Boolean Resultado = true;
        Char[] Array = Cadena.ToCharArray();
        try
        {
            for (int index = 0; index < Array.Length; index++)
            {
                if (!Char.IsDigit(Array[index])) return false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
    /// DESCRIPCION : Crea el DataTable con la consulta de las nomina vigentes en el 
    /// sistema.
    /// PARAMETROS: Dt_Calendario_Nominas.- Lista de las nominas vigentes actualmente 
    ///             en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 01/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Formato_Fecha_Calendario_Nomina(DataTable Dt_Calendario_Nominas)
    {
        DataTable Dt_Nominas = new DataTable();
        DataRow Renglon_Dt_Clon = null;
        Dt_Nominas.Columns.Add("Nomina", typeof(System.String));
        Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

        foreach (DataRow Renglon in Dt_Calendario_Nominas.Rows) {
            Renglon_Dt_Clon = Dt_Nominas.NewRow();
            Renglon_Dt_Clon["Nomina"] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString().Split(new char[] { ' ' })[0].Split(new char[]{'/'})[2];
            Renglon_Dt_Clon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID];
            Dt_Nominas.Rows.Add(Renglon_Dt_Clon);
        }
        return Dt_Nominas;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
    /// DESCRIPCION : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 01/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Sumar_Importes_Prestamo_Interes() {
        Double Importe_Prestamo = 0.0;//Variable que alamacena la catidad solicitada por el empleado.
        Double Importe_Interes = 0.0;//Variable que almacena la cantidad de interes que se le cobrara al empelado por el prestamo, solo si este aplica.
        Double Total = 0.0;//Variable que almacenara el resultado de la sumatoria de importes.
        Double Interes = 0;

        try
        {
            if (!string.IsNullOrEmpty(Txt_Importe_Prestamo.Text.Trim()))
            {
                Importe_Prestamo = Convert.ToDouble(Convert.ToString(Txt_Importe_Prestamo.Text.Trim()).Replace("$", ""));
            }

            if (!string.IsNullOrEmpty(Txt_Importe_Interes.Text.Trim()))
                Importe_Interes = Convert.ToDouble(Convert.ToString(Txt_Importe_Interes.Text.Trim()).Replace("$", ""));

            if (Lbl_Importe_Interes.Text.Trim().Contains("%"))
            {
                if (Importe_Interes >= 0 && Importe_Interes <= 100)
                {
                    Interes = (Importe_Prestamo * Importe_Interes) / 100;
                    Importe_Interes = Interes;

                    Total = (Importe_Prestamo + Importe_Interes);
                    Txt_Total_Prestamo.Text = string.Format("{0:#,###,##0.00}", Total);
                    Txt_Importe_Prestamo.Text = string.Format("{0:#,###,##0.00}", Importe_Prestamo);
                    Txt_Importe_Interes.Text = string.Format("{0:#,###,###.00}", Txt_Importe_Interes.Text.Trim());
                }
                else
                {
                    Txt_Importe_Interes.Text = "";
                    Txt_Total_Prestamo.Text = Txt_Importe_Prestamo.Text.Trim();

                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "El porcentaje no puede ser mayor a 100 o menor a 0.";
                }
            }
            else
            {
                Total = (Importe_Prestamo + Importe_Interes);
                Txt_Total_Prestamo.Text = string.Format("{0:#,###,##0.00}", Total);
                Txt_Importe_Prestamo.Text = string.Format("{0:#,###,##0.00}", Importe_Prestamo);
                Txt_Importe_Interes.Text = string.Format("{0:#,###,###.00}", Txt_Importe_Interes.Text.Trim());
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al sumar los importes. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
    /// DESCRIPCION : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 01/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Calculo_Abono() {
        Double Abono = 0.0;//Variable que almacenara la cantidad que le descontara al empleado catorcenalmente.
        Int32 No_Abonos = 0;//Variable que almacena el numero de catorcenas en las que se cubrira el pago total del prestamo.
        Double Total_Importes = 0.0;//variable que almacena el total de los importes, tanto de importe del prestamo como el de interes.
        try
        {
            if (!string.IsNullOrEmpty(Txt_Total_Prestamo.Text.Trim())) Total_Importes = Convert.ToDouble(Txt_Total_Prestamo.Text.Trim());
            if (!string.IsNullOrEmpty(Txt_No_Pagos.Text.Trim())) No_Abonos = Convert.ToInt32(Txt_No_Pagos.Text.Trim());

            if (Total_Importes > 0 && No_Abonos > 0)
            {
                Abono = (Total_Importes / No_Abonos);
                Txt_Abono.Text = string.Format("{0:#,###,##0.00}", Abono);
                Txt_No_Abonos.Text = "0";
                Txt_Saldo_Actual.Text = string.Format("{0:#,###,##0.00}", Total_Importes);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al realizar el calculo del Abono. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Calcular_Fecha_Termino_Prestamo
    /// DESCRIPCION : Calcula la fecha en la que se terminara de hacer la deduccion 
    /// al empelado.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 01/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Calcular_Fecha_Termino_Prestamo() {
        DateTime Fecha_Inicia = new DateTime();//Variable que almacenara la fecha de inicio de los pagos.
        DateTime Fecha_Final = new DateTime();//Variable que almacenara la fecha de fin de los pagos.
        Int32 No_Pagos = 0;//Variable que almacena el numero de pagos que el empleado debera efectuar para liquidar el prestamo.

        try
        {
            No_Pagos = Convert.ToInt32((Txt_No_Pagos.Text.Trim().Equals("")) ? "0" : Txt_No_Pagos.Text.Trim());
            Fecha_Inicia = Convert.ToDateTime(Txt_Fecha_Inicio_Pago_Prestamo.Text.Trim());
            Fecha_Final = Fecha_Inicia;

            for (int index = 1; index <= No_Pagos; index++)
            {
                Fecha_Final = Fecha_Final.AddDays(14);
            }

            //Fecha de Fin de pago del prestamo.
            Txt_Fecha_Termino_Pago_Prestamo.Text = string.Format("{0:dd/MMM/yyyy}", Fecha_Final);

        }
        catch (Exception Ex)
        {
            throw new Exception("Error al calcular la fecha de liquidacion del prestamo. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la deduccion 
    ///a partir del periodo actual.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Validar_Periodos_Pago(DropDownList Combo)
    {
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Actual = DateTime.Now;
        DateTime Fecha_Inicio = new DateTime();
        DateTime Fecha_Fin = new DateTime();

        Prestamos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

        foreach (ListItem Elemento in Combo.Items)
        {
            if (IsNumeric(Elemento.Text.Trim()))
            {
                Prestamos.P_No_Nomina = Convert.ToInt32(Elemento.Text.Trim());
                Dt_Detalles_Nomina = Prestamos.Consultar_Fechas_Periodo();

                if (Dt_Detalles_Nomina != null)
                {
                    if (Dt_Detalles_Nomina.Rows.Count > 0)
                    {
                        Fecha_Inicio = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString());
                        Fecha_Fin = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString());

                        //if (Fecha_Fin >= Fecha_Actual)
                        //{
                        //    Elemento.Enabled = true;
                        //}
                        //else
                        //{
                        //    Elemento.Enabled = false;
                        //}
                    }
                }
            }
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Juntar_Clave_Nombre
    /// DESCRIPCION : Junta el nombre del concepto con la clave.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected DataTable Juntar_Clave_Nombre(DataTable Dt_Conceptos)
    {
        try
        {
            if (Dt_Conceptos is DataTable)
            {
                if (Dt_Conceptos.Rows.Count > 0)
                {
                    foreach (DataRow CONCEPTO in Dt_Conceptos.Rows)
                    {
                        if (CONCEPTO is DataRow)
                        {
                            CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] =
                                "[" + CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString().Trim() + "] -- " +
                                    CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString().Trim();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al unir el nombre con la clave del concepto. Error: [" + Ex.Message + "]");
        }
        return Dt_Conceptos;
    }
    #endregion

    #region (Metodos Operacion)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Solicitud_Prestamo
    /// DESCRIPCION : Ejecuta la Peticion a la clase de negocio para que ejecute la alta
    /// de la solicitud del prestamo.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 01/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Solicitud_Prestamo() {
        Cls_Ope_Nom_Pestamos_Negocio Alta_Solicitud_Prestamo = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocio.
        Cls_Cat_Empleados_Negocios Consulta_Empleado = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocio.
        DataTable Dt_Empleados = null;//Variable que almacenara una lista de empleados.
        
        try
        {
            //Consultamos el Empleado_ID del Empleado solicitante por medio de su no empleado.
            Consulta_Empleado.P_No_Empleado = Txt_No_Empleado_Solicitante_Prestamo.Text.Trim();
            Dt_Empleados = Consulta_Empleado.Consulta_Datos_Empleado();

            if(Dt_Empleados != null){
                if (Dt_Empleados.Rows.Count > 0) {
                    Alta_Solicitud_Prestamo.P_Solicita_Empleado_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString().Trim();                    
                }
            }

            Dt_Empleados = null;

            //Consultamos el Empleado_ID del Empleado que Sera el Aval
            Consulta_Empleado.P_No_Empleado = Txt_No_Empleado_Aval.Text.Trim();
            Dt_Empleados = Consulta_Empleado.Consulta_Datos_Empleado();

            if (Dt_Empleados != null)
            {
                if (Dt_Empleados.Rows.Count > 0)
                {
                    Alta_Solicitud_Prestamo.P_Aval_Empleado_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString().Trim();
                }
            }

            Alta_Solicitud_Prestamo.P_Proveedor_ID = Cmb_Proveedor.SelectedValue.Trim();
            Alta_Solicitud_Prestamo.P_Percepcion_Deduccion_ID = Cmb_Deduccion.SelectedValue.Trim();
            Alta_Solicitud_Prestamo.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Alta_Solicitud_Prestamo.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());
            Alta_Solicitud_Prestamo.P_Estatus_Solicitud = Cmb_Estatus_Solicitud_Prestamo.SelectedItem.Text.Trim();
            Alta_Solicitud_Prestamo.P_Estado_Prestamo = "PENDIENTE";
            Alta_Solicitud_Prestamo.P_Estatus_Pago = "Pendiente";
            Alta_Solicitud_Prestamo.P_Fecha_Solicitud = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Solicitud_Prestamo.Text.Trim()));
            Alta_Solicitud_Prestamo.P_Fecha_Inicio_Pago = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicio_Pago_Prestamo.Text.Trim()));
            Alta_Solicitud_Prestamo.P_Fecha_Termino_Pago = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Termino_Pago_Prestamo.Text.Trim()));
            Alta_Solicitud_Prestamo.P_Motivo_Prestamo = Txt_Finalidad_Prestamo.Text.Trim();
            Alta_Solicitud_Prestamo.P_No_Pagos = Convert.ToInt32(Txt_No_Pagos.Text.Trim());
            Alta_Solicitud_Prestamo.P_Importe_Prestamo = Convert.ToDouble(Txt_Importe_Prestamo.Text.Trim());
            Alta_Solicitud_Prestamo.P_Importe_Interes = Convert.ToDouble((string.IsNullOrEmpty(Txt_Importe_Interes.Text.Trim()) || Txt_Importe_Interes.Text.Trim().Equals("$  _,___,___.__")) ? "0" : Txt_Importe_Interes.Text.Trim());
            Alta_Solicitud_Prestamo.P_Total_Prestamo = Convert.ToDouble(Txt_Total_Prestamo.Text.Trim());
            Alta_Solicitud_Prestamo.P_Saldo_Actual = Convert.ToDouble(Txt_Total_Prestamo.Text.Trim());
            Alta_Solicitud_Prestamo.P_Abono = Convert.ToDouble(Txt_Abono.Text.Trim());
            Alta_Solicitud_Prestamo.P_No_Abono = Convert.ToInt32(string.IsNullOrEmpty(Txt_No_Abonos.Text.Trim()) ? "0" : Txt_No_Abonos.Text.Trim());
            Alta_Solicitud_Prestamo.P_Usuario_Creo = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);
            Alta_Solicitud_Prestamo.P_Aplica_Validaciones = (Chk_Omitir_Validaciones.Checked) ? "SI" : "NO";

            if (Alta_Solicitud_Prestamo.Alta_Solicitud_Prestamo())
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Ventana Estatus de la Operacion", "alert('Operación Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al ejecutar el alta de una solicitud de prestamo. Error:[" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Solicitud_Prestamo
    /// DESCRIPCION : Ejecuta la Peticion a la clase de negocio para que ejecute la Actualizacion
    /// de los datos de la solicitud del prestamo.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 01/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Solicitud_Prestamo()
    {
        Cls_Ope_Nom_Pestamos_Negocio Modificar_Solicitud_Prestamo = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocio.
        Cls_Cat_Empleados_Negocios Consulta_Empleado = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocio.
        DataTable Dt_Empleados = null;//Variable que almacenara una lista de empleados.

        try
        {
            //Consultamos el Empleado_ID del Empleado solicitante por medio de su no empleado.
            Consulta_Empleado.P_No_Empleado = Txt_No_Empleado_Solicitante_Prestamo.Text.Trim();
            Dt_Empleados = Consulta_Empleado.Consulta_Datos_Empleado();

            if (Dt_Empleados != null)
            {
                if (Dt_Empleados.Rows.Count > 0)
                {
                    Modificar_Solicitud_Prestamo.P_Solicita_Empleado_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString().Trim();
                }
            }

            Dt_Empleados = null;

            //Consultamos el Empleado_ID del Empleado que Sera el Aval
            Consulta_Empleado.P_No_Empleado = Txt_No_Empleado_Aval.Text.Trim();
            Dt_Empleados = Consulta_Empleado.Consulta_Datos_Empleado();

            if (Dt_Empleados != null)
            {
                if (Dt_Empleados.Rows.Count > 0)
                {
                    Modificar_Solicitud_Prestamo.P_Aval_Empleado_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString().Trim();
                }
            }

            Modificar_Solicitud_Prestamo.P_No_Solicitud = Txt_No_Solocitud.Text.Trim();
            Modificar_Solicitud_Prestamo.P_Proveedor_ID = Cmb_Proveedor.SelectedValue.Trim();
            Modificar_Solicitud_Prestamo.P_Percepcion_Deduccion_ID = Cmb_Deduccion.SelectedValue.Trim();
            Modificar_Solicitud_Prestamo.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Modificar_Solicitud_Prestamo.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());
            Modificar_Solicitud_Prestamo.P_Estatus_Solicitud = Cmb_Estatus_Solicitud_Prestamo.SelectedItem.Text.Trim();
            Modificar_Solicitud_Prestamo.P_Estatus_Pago = "Pendiente";
            Modificar_Solicitud_Prestamo.P_Fecha_Solicitud = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Solicitud_Prestamo.Text.Trim()));
            Modificar_Solicitud_Prestamo.P_Fecha_Inicio_Pago = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicio_Pago_Prestamo.Text.Trim()));
            Modificar_Solicitud_Prestamo.P_Fecha_Termino_Pago = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Termino_Pago_Prestamo.Text.Trim()));
            Modificar_Solicitud_Prestamo.P_Motivo_Prestamo = Txt_Finalidad_Prestamo.Text.Trim();
            Modificar_Solicitud_Prestamo.P_No_Pagos = Convert.ToInt32(Txt_No_Pagos.Text.Trim());
            Modificar_Solicitud_Prestamo.P_Importe_Prestamo = Convert.ToDouble(Txt_Importe_Prestamo.Text.Trim());
            Modificar_Solicitud_Prestamo.P_Importe_Interes = Convert.ToDouble((string.IsNullOrEmpty(Txt_Importe_Interes.Text.Trim()) || Txt_Importe_Interes.Text.Trim().Equals("$  _,___,___.__")) ? "0" : Txt_Importe_Interes.Text.Trim());
            Modificar_Solicitud_Prestamo.P_Total_Prestamo = Convert.ToDouble(Txt_Total_Prestamo.Text.Trim());
            Modificar_Solicitud_Prestamo.P_Saldo_Actual = Convert.ToDouble(Txt_Total_Prestamo.Text.Trim());
            Modificar_Solicitud_Prestamo.P_Abono = Convert.ToDouble(Txt_Abono.Text.Trim());
            Modificar_Solicitud_Prestamo.P_No_Abono = Convert.ToInt32(string.IsNullOrEmpty(Txt_No_Abonos.Text.Trim()) ? "0" : Txt_No_Abonos.Text.Trim());
            Modificar_Solicitud_Prestamo.P_Usuario_Modifico = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);
            Modificar_Solicitud_Prestamo.P_Aplica_Validaciones = (Chk_Omitir_Validaciones.Checked) ? "SI" : "NO";

            if (Modificar_Solicitud_Prestamo.Modificar_Solicitud_Prestamo())
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Ventana Estatus de la Operacion", "alert('Operación Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al ejecutar una modificacion a los datos de una solicitud de prestamo. Error:[" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Solicitud_Prestamo
    /// DESCRIPCION : Ejecuta la Peticion a la clase de negocio para que ejecute la Baja
    /// de la solicitud del prestamo.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 01/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Solicitud_Prestamo()
    {
        Cls_Ope_Nom_Pestamos_Negocio Eliminar_Solicitud_Prestamo = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocio.

        try
        {            
            Eliminar_Solicitud_Prestamo.P_No_Solicitud = Txt_No_Solocitud.Text.Trim();

            if (Eliminar_Solicitud_Prestamo.Eliminar_Solicitud_Prestamo())
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Ventana Estatus de la Operacion", "alert('Operación Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al ejecutar una Baja de una solicitud de prestamo. Error:[" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Metodos Consulta)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Calendario_Nominas
    ///DESCRIPCIÓN: Consulta los calendarios de nomina vigentes actualmente.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Calendario_Nominas() {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nominas = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Calendario_Nominas = null;//Variable que almacenara una lista de los calendarios de nomina vigentes.
        try
        {
            Dt_Calendario_Nominas = Consulta_Calendario_Nominas.Consultar_Calendario_Nominas();
            if (Dt_Calendario_Nominas != null) {
                if (Dt_Calendario_Nominas.Rows.Count > 0)
                {
                    Dt_Calendario_Nominas = Formato_Fecha_Calendario_Nomina(Dt_Calendario_Nominas);
                    Cmb_Calendario_Nomina.DataSource = Dt_Calendario_Nominas;
                    Cmb_Calendario_Nomina.DataTextField = "Nomina";
                    Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                    Cmb_Calendario_Nomina.DataBind();
                    Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Calendario_Nomina.SelectedIndex = -1;

                    //::::::::::::::::::::::::::::::::::::::
                    Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf(
                        Cmb_Calendario_Nomina.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Anyo));

                    if (Cmb_Calendario_Nomina.SelectedIndex > 0) {
                        Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
                        Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Periodos_Catorcenales_Nomina.Items.IndexOf(
                            Cmb_Periodos_Catorcenales_Nomina.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Periodo));

                        Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged(Cmb_Periodos_Catorcenales_Nomina, new EventArgs());
                    }
                    //::::::::::::::::::::::::::::::::::::::
                }
                else {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron nominas vigentes";
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las nominas. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales para el 
    ///calendario de nomina seleccionado.
    ///PARAMETROS: Nomina_ID.- Indica el calendario de nomina del cuál se desea consultar
    ///                        los periodos catorcenales.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Periodos_Catorcenales_Nomina(String Nomina_ID) {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nomina_Periodos = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Clase de conexion con la capa de negocios
        DataTable Dt_Periodos_Catorcenales = null;//Variable que almacenra unaa lista de los periodos catorcenales que le correspónden a la nomina seleccionada.

        try
        {
            Consulta_Calendario_Nomina_Periodos.P_Nomina_ID = Nomina_ID;
            Dt_Periodos_Catorcenales = Consulta_Calendario_Nomina_Periodos.Consulta_Detalles_Nomina();
            if (Dt_Periodos_Catorcenales != null) {
                if (Dt_Periodos_Catorcenales.Rows.Count > 0)
                {
                    Cmb_Periodos_Catorcenales_Nomina.DataSource = Dt_Periodos_Catorcenales;
                    Cmb_Periodos_Catorcenales_Nomina.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataValueField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataBind();
                    Cmb_Periodos_Catorcenales_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;

                    Validar_Periodos_Pago(Cmb_Periodos_Catorcenales_Nomina);
                }
                else {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron periodos catorcenales para la nomina seleccionada.";
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los periodos catorcenales del  calendario de nomina seleccionado. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Proveedores
    ///DESCRIPCIÓN: Consulta los Proveedores vigentes en el sistema.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Proveedores()
    {
        Cls_Cat_Nom_Proveedores_Negocio Consulta_Proveedores = new Cls_Cat_Nom_Proveedores_Negocio();//Variable de conexion con la clase de negocio.
        DataTable Dt_proveedores = null;//Variable que almacenara una lista de proveedores vigentes en el sistema.
        try
        {
            Dt_proveedores = Consulta_Proveedores.Consultar_Proveedores();
            if (Dt_proveedores != null)
            {
                if (Dt_proveedores.Rows.Count > 0)
                {
                    Cmb_Proveedor.DataSource = Dt_proveedores;
                    Cmb_Proveedor.DataTextField = Cat_Nom_Proveedores.Campo_Nombre;
                    Cmb_Proveedor.DataValueField = Cat_Nom_Proveedores.Campo_Proveedor_ID;
                    Cmb_Proveedor.DataBind();
                    Cmb_Proveedor.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Proveedor.SelectedIndex = -1;
                }
                else {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron proveedores vigentes en el sistema.";
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar a los proveedores. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Deducciones
    ///DESCRIPCIÓN: Consulta las Deducciones vigentes en el sistema.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consulta_Deducciones(String Proveedor_ID)
    {
        Cls_Cat_Nom_Proveedores_Negocio Consulta_Deducciones = new Cls_Cat_Nom_Proveedores_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Deducciones = null;//Variable que almacenara una lista de deducciones.
        try
        {
            Consulta_Deducciones.P_Proveedor_ID = Proveedor_ID;
            Dt_Deducciones = Consulta_Deducciones.Consultar_Deducciones_Proveedor();
            Dt_Deducciones = Juntar_Clave_Nombre(Dt_Deducciones);

            if (Dt_Deducciones != null)
            {
                if (Dt_Deducciones.Rows.Count > 0)
                {
                    Cmb_Deduccion.DataSource = Dt_Deducciones;
                    Cmb_Deduccion.DataTextField = Cat_Nom_Percepcion_Deduccion.Campo_Nombre;
                    Cmb_Deduccion.DataValueField = Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;
                    Cmb_Deduccion.DataBind();
                    Cmb_Deduccion.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Deduccion.SelectedIndex = -1;
                }
                else
                {
                    Cmb_Deduccion.DataSource = new DataTable();
                    Cmb_Deduccion.DataBind();
                    Cmb_Deduccion.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Deduccion.SelectedIndex = -1;

                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron Deducciones vigentes en el sistema.";
                }
            }

        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las deducciones en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Mostrar_Informacion_Empleado_Solicitante
    ///DESCRIPCIÓN: Consulta los datos del Empleado solicitante.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Mostrar_Informacion_Empleado_Solicitante()
    {
        Cls_Cat_Empleados_Negocios Consulta_Empelado = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        Cls_Cat_Nom_Sindicatos_Negocio Consulta_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Dependencias_Negocio Consulta_Dependencias = new Cls_Cat_Dependencias_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Tipos_Nominas_Negocio Consulta_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexion con la clase de negocios.
        Cls_Ope_Nom_Pestamos_Negocio Consulta_Empleados_Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Nom_Bancos_Negocio Rs_Consulta_Cat_Nom_Bancos = new Cls_Cat_Nom_Bancos_Negocio();//Variable de conexión con la clase de negocios.
        DataTable Dt_Bancos = null; //Variable que almacen a el banco en el cual se le deposita el dinero al empleado
        DataTable Dt_Empleados = null;//Variable que almacenara la informacion del empelado.
        DataTable Dt_Sindicatos = null;//Variable que almacenara los datos del sindicato seleccioado.
        DataTable Dt_Dependencias = null;//Variable que almacenara los datos de la dependencia seleccionada.
        DataTable Dt_Tipos_Nomina = null;//Variable que almacenara los datos de la nomina seleccionada.
        String Sindicato = "";//Variable que almacenara el Sindicato a la que pertence el empleado solicitante.
        String Dependencia = "";//Variable que almacenara la Dependencia a la que pertence el empleado solicitante.
        String Direccion = "";//Variable que almacenara la direccion del Empleado solicitante.
        String Nomina = "";//Variable que almacenara la nomina a la pertenece el empleado solicitante.
        String Nombre = "";//Variable que almacenara el nombre del empleado completo.
        String No_Empleado = "";//Variable que almacenara en nu del empleado.
        String Sueldo_Mensual = "";//Variable que almecenara el sueldo mesual del empleado solicitante.

        try
        {
            No_Empleado = Txt_No_Empleado_Solicitante_Prestamo.Text.Trim();
            //Limpiamos los controles del empleado solicitante del prestamo.
            Limpiar_Controles_Empleado_Solicitante();
            Txt_No_Empleado_Solicitante_Prestamo.Text = No_Empleado;

            if (!string.IsNullOrEmpty(Txt_No_Empleado_Solicitante_Prestamo.Text.Trim()))
            {
                Consulta_Empelado.P_No_Empleado = Txt_No_Empleado_Solicitante_Prestamo.Text.Trim();
                Dt_Empleados = Consulta_Empelado.Consulta_Empleados_General();

                if (Dt_Empleados != null)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        Consulta_Sindicatos.P_Sindicato_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Sindicato_ID].ToString().Trim();
                        Dt_Sindicatos = Consulta_Sindicatos.Consulta_Sindicato();
                        if (Dt_Sindicatos != null)
                        {
                            if (Dt_Sindicatos.Rows.Count > 0)
                            {
                                Sindicato = Dt_Sindicatos.Rows[0][Cat_Nom_Sindicatos.Campo_Nombre].ToString().Trim();
                            }
                        }

                        Consulta_Dependencias.P_Dependencia_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString().Trim();
                        Dt_Dependencias = Consulta_Dependencias.Consulta_Dependencias();
                        if (Dt_Dependencias != null)
                        {
                            if (Dt_Dependencias.Rows.Count > 0)
                            {
                                Dependencia = Dt_Dependencias.Rows[0][Cat_Dependencias.Campo_Nombre].ToString().Trim();
                            }
                        }

                        Consulta_Tipos_Nominas.P_Tipo_Nomina_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Tipo_Nomina_ID].ToString().Trim();
                        Dt_Tipos_Nomina = Consulta_Tipos_Nominas.Consulta_Datos_Tipo_Nomina();
                        if (Dt_Tipos_Nomina != null)
                        {
                            if (Dt_Tipos_Nomina.Rows.Count > 0)
                            {
                                Nomina = Dt_Tipos_Nomina.Rows[0][Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString().Trim();
                            }
                        }

                        Direccion = "Calle:" + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Calle].ToString() +
                                    "  Colonia:" + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Colonia].ToString() +
                                    "  CP:" + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Codigo_Postal].ToString();

                        Nombre = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Nombre].ToString() + " " + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString() +
                               " " + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString();

                        Sueldo_Mensual = string.Format("{0:#,###,##0.00}", (Convert.ToDouble((Dt_Empleados.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString().Trim().Equals("")) ? "0" : Dt_Empleados.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString().Trim()) * (new Cls_Utlidades_Nomina(DateTime.Now).P_Dias_Promedio_Mes_Anio)));

                        if (!string.IsNullOrEmpty(Nombre))
                        {
                            Txt_Nombre_Empleado_Solicitante.Text = Nombre;
                            Txt_Nombre_Empleado_Solicitante.ToolTip = Nombre;
                        }
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_RFC].ToString())) Txt_RFC_Empleado_Solicitante.Text = Dt_Empleados.Rows[0][Cat_Empleados.Campo_RFC].ToString();
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Fecha_Inicio].ToString())) Txt_Fecha_Ingreso_Empleado_Solicitante.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Fecha_Inicio].ToString()));
                        if (!string.IsNullOrEmpty(Sindicato)) Txt_Sindicato_Empleado_Solicitante.Text = Sindicato;
                        if (!string.IsNullOrEmpty(Dependencia)) Txt_Dependencia_Empelado_Solicitante.Text = Dependencia;
                        if (!string.IsNullOrEmpty(Direccion)) Txt_Direccion_Empleado_Solicitante.Text = Direccion;
                        if (!string.IsNullOrEmpty(Nomina)) Txt_Clase_Nomina_Empleado.Text = Nomina;
                        if (!string.IsNullOrEmpty(Sueldo_Mensual)) Txt_Sueldo_Mensual_Empleado_Solicitante.Text = Sueldo_Mensual;
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString())) Txt_Cuenta_Bancaria_Empleado_Solicitante.Text = Dt_Empleados.Rows[0][Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString();
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Banco_ID].ToString()))
                        {
                            Rs_Consulta_Cat_Nom_Bancos.P_Banco_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Banco_ID].ToString();
                            Dt_Bancos = Rs_Consulta_Cat_Nom_Bancos.Consulta_Bancos();
                            if (Dt_Bancos != null)
                            {
                                Txt_Banco_Empleado_Solicitante.Text = Dt_Bancos.Rows[0][Cat_Nom_Bancos.Campo_Nombre].ToString();
                            }
                        }

                        Img_Foto_Empleado_Solicitante.ImageUrl = (string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Ruta_Foto].ToString())) ? "~/paginas/imagenes/paginas/Sias_No_Disponible.JPG" : @Dt_Empleados.Rows[0][Cat_Empleados.Campo_Ruta_Foto].ToString();
                        Img_Foto_Empleado_Solicitante.DataBind();
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "El empleado no existe.";
                    }
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Ingrese el numero de empleado que desea buscar.";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Mostrar_Informacion_Empleado_Aval
    ///DESCRIPCIÓN: Consulta los datos del Empleado Aval.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Mostrar_Informacion_Empleado_Aval()
    {
        Cls_Cat_Empleados_Negocios Consulta_Empelado = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        Cls_Cat_Nom_Sindicatos_Negocio Consulta_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Dependencias_Negocio Consulta_Dependencias = new Cls_Cat_Dependencias_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Ope_Nom_Pestamos_Negocio Consulta_Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Ope_Nom_Pestamos_Negocio Prestamos_Consultados = null;//Variable de conexion con la capa de negocios.
        DataTable Dt_Empleados = null;//Variable que almacenara la informacion del empelado.
        DataTable Dt_Sindicatos = null;//Variable que almacenara los datos del sindicato seleccioado.
        DataTable Dt_Dependencias = null;//Variable que almacenara los datos de la dependencia seleccionada.
        String Sindicato = "";//Variable que almacenara el Sindicato a la que pertence el empleado solicitante.
        String Dependencia = "";//Variable que almacenara la Dependencia a la que pertence el empleado solicitante.
        String Direccion = "";//Variable que almacenara la direccion del Empleado solicitante.
        String Nombre = "";//Variable que almacenara el nombre del empleado completo.
        String No_Empleado = "";//Variable que almacenara en nu del empleado.
        String Sueldo_Mensual = "";//Variable que almecenara el sueldo mesual del empleado solicitante.

        try
        {
            No_Empleado = Txt_No_Empleado_Aval.Text.Trim();
            //Limpiamos los controles de empleado aval del prestamo.
            Limpiar_Controles_Empleado_Aval();
            Txt_No_Empleado_Aval.Text = No_Empleado;

            if (!string.IsNullOrEmpty(Txt_No_Empleado_Aval.Text.Trim()))
            {
                //Consultamos para validar y verificar que el empleado candidato a ser aval no lo sea actualmente.
                Consulta_Prestamos.P_Aval_No_Empleado = Txt_No_Empleado_Aval.Text.Trim(); ;
                Prestamos_Consultados = Consulta_Prestamos.Consulta_Prestamos();
                //Verificamos y Validamos.
                if (Prestamos_Consultados.P_Dt_Prestamos != null)
                {
                    if (Prestamos_Consultados.P_Dt_Prestamos.Rows.Count > 0)
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "El empleado ya ha sido aval. Un empleado no puede ser aval en mas de una ocacion.";
                        return;
                    }
                }

                Consulta_Empelado.P_No_Empleado = Txt_No_Empleado_Aval.Text.Trim();
                Dt_Empleados = Consulta_Empelado.Consulta_Empleados_General();

                if (Dt_Empleados != null)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {

                        Consulta_Sindicatos.P_Sindicato_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Sindicato_ID].ToString().Trim();
                        Dt_Sindicatos = Consulta_Sindicatos.Consulta_Sindicato();
                        if (Dt_Sindicatos != null)
                        {
                            if (Dt_Sindicatos.Rows.Count > 0)
                            {
                                Sindicato = Dt_Sindicatos.Rows[0][Cat_Nom_Sindicatos.Campo_Nombre].ToString().Trim();
                            }
                        }

                        Consulta_Dependencias.P_Dependencia_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString().Trim();
                        Dt_Dependencias = Consulta_Dependencias.Consulta_Dependencias();
                        if (Dt_Dependencias != null)
                        {
                            if (Dt_Dependencias.Rows.Count > 0)
                            {
                                Dependencia = Dt_Dependencias.Rows[0][Cat_Dependencias.Campo_Nombre].ToString().Trim();
                            }
                        }

                        Direccion = "Calle:" + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Calle].ToString() +
                                    "  Colonia:" + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Colonia].ToString() +
                                    "  CP:" + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Codigo_Postal].ToString();

                        Nombre = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Nombre].ToString() + " " + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString() +
                               " " + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString();

                        Sueldo_Mensual = string.Format("{0:#,###,##0.00}", (Convert.ToDouble((Dt_Empleados.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString().Trim().Equals("")) ? "0" : Dt_Empleados.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString().Trim()) * (new Cls_Utlidades_Nomina(DateTime.Now).P_Dias_Promedio_Mes_Anio)));

                        if (!string.IsNullOrEmpty(Nombre))
                        {
                            Txt_Nombre_Empleado_Aval.Text = Nombre;
                            Txt_Nombre_Empleado_Aval.ToolTip = Nombre;
                        }
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_RFC].ToString())) Txt_RFC_Aval.Text = Dt_Empleados.Rows[0][Cat_Empleados.Campo_RFC].ToString();
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Fecha_Inicio].ToString())) Txt_Fecha_Ingreso_Aval.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Fecha_Inicio].ToString()));
                        if (!string.IsNullOrEmpty(Sindicato)) Txt_Sindicato_Aval.Text = Sindicato;
                        if (!string.IsNullOrEmpty(Dependencia)) Txt_Dependencia_Aval.Text = Dependencia;
                        if (!string.IsNullOrEmpty(Direccion)) Txt_Direccion_Aval.Text = Direccion;
                        if (!string.IsNullOrEmpty(Sueldo_Mensual)) Txt_Sueldo_Mensual_Aval.Text = Sueldo_Mensual;
                        Img_Empleado_Aval.ImageUrl = (string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Ruta_Foto].ToString())) ? "~/paginas/imagenes/paginas/Sias_No_Disponible.JPG" : @Dt_Empleados.Rows[0][Cat_Empleados.Campo_Ruta_Foto].ToString();
                        Img_Empleado_Aval.DataBind();
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "El empleado no existe.";
                    }
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Ingrese el numero de empleado que desea buscar.";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consulta_Solicitudes_Prestamos
    ///DESCRIPCIÓN: Consulta las solicitudes de prestamos.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 02/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consulta_Solicitudes_Prestamos(Int32 Pagina)
    {
        Cls_Ope_Nom_Pestamos_Negocio Consulta_Solicitudes_Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Solicitudes_Prestamos = null;//Variable que almacenara una lista de solicitudes de prestamos.
        try
        {
            if (!string.IsNullOrEmpty(Txt_Busqueda_No_Solicitud.Text.Trim()))
            {
                Txt_Busqueda_No_Solicitud.Text = string.Format("{0:0000000000}", Convert.ToInt32(Txt_Busqueda_No_Solicitud.Text.Trim().Equals("") ? "0" : Txt_Busqueda_No_Solicitud.Text.Trim()));
            }
            //Filtros de Busqueda
            Consulta_Solicitudes_Prestamos.P_No_Solicitud = Txt_Busqueda_No_Solicitud.Text.Trim();
            if (Cmb_Busqueda_Estado_Prestamo.SelectedIndex > 0) Consulta_Solicitudes_Prestamos.P_Estado_Prestamo = Cmb_Busqueda_Estado_Prestamo.SelectedItem.Text.Trim().ToUpper();
            Consulta_Solicitudes_Prestamos.P_Solicita_No_Empleado= Txt_Busqueda_Empleado_Solicitante.Text.Trim();
            Consulta_Solicitudes_Prestamos.P_Aval_No_Empleado = Txt_Busqueda_Empleado_Aval.Text.Trim();
            Consulta_Solicitudes_Prestamos.P_RFC_Empleado_Solicita_Prestamo = Txt_Busqueda_RFC_Solicita.Text.Trim();
            Consulta_Solicitudes_Prestamos.P_RFC_Empleado_Aval_Prestamo = Txt_Busqueda_RFC_Aval.Text.Trim();

            if (Cmb_Busqueda_Estatus_Solicitud.SelectedIndex > 0) Consulta_Solicitudes_Prestamos.P_Estatus_Solicitud = Cmb_Busqueda_Estatus_Solicitud.SelectedItem.Text.Trim();

            if (Validar_Formato_Fecha(Txt_Busqueda_Fecha_Inicio.Text.Trim()))
            {
                Consulta_Solicitudes_Prestamos.P_Fecha_Inicio_Pago = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Busqueda_Fecha_Inicio.Text.Trim()));
            }
            else { Txt_Busqueda_Fecha_Inicio.Text = ""; }
            if (Validar_Formato_Fecha(Txt_Busqueda_Fecha_Fin.Text.Trim()))
            {
                Consulta_Solicitudes_Prestamos.P_Fecha_Termino_Pago = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Busqueda_Fecha_Fin.Text.Trim()));
            }
            else { Txt_Busqueda_Fecha_Fin.Text = ""; }

            Dt_Solicitudes_Prestamos = Consulta_Solicitudes_Prestamos.Consulta_Solicitudes_Prestamos().P_Dt_Solicitudes_Prestamos;
            if (Dt_Solicitudes_Prestamos != null) {
                if (Dt_Solicitudes_Prestamos.Rows.Count > 0)
                {
                    LLenar_Grid_Prestamos(Dt_Solicitudes_Prestamos, Pagina);
                }
                else {
                    LLenar_Grid_Prestamos(new DataTable(), Pagina);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las solicitudes de prestamos. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Empleado_Con_Solicitud_Prestamo
    ///DESCRIPCIÓN: Consulta los datos del Empleado solicitante.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Datos_Empleado_Con_Solicitud_Prestamo(String Empleado_ID)
    {
        Cls_Cat_Empleados_Negocios Consulta_Empelado = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        Cls_Cat_Nom_Sindicatos_Negocio Consulta_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Dependencias_Negocio Consulta_Dependencias = new Cls_Cat_Dependencias_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Tipos_Nominas_Negocio Consulta_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexion con la clase de negocios.
        Cls_Cat_Nom_Bancos_Negocio Rs_Consulta_Cat_Nom_Bancos = new Cls_Cat_Nom_Bancos_Negocio();//Variable de conexión con la clase de negocios.
        DataTable Dt_Bancos = null; //Variable que almacen a el banco en el cual se le deposita el dinero al empleado
        DataTable Dt_Empleados = null;//Variable que almacenara la informacion del empelado.
        DataTable Dt_Sindicatos = null;//Variable que almacenara los datos del sindicato seleccioado.
        DataTable Dt_Dependencias = null;//Variable que almacenara los datos de la dependencia seleccionada.
        DataTable Dt_Tipos_Nomina = null;//Variable que almacenara los datos de la nomina seleccionada.
        String Sindicato = "";//Variable que almacenara el Sindicato a la que pertence el empleado solicitante.
        String Dependencia = "";//Variable que almacenara la Dependencia a la que pertence el empleado solicitante.
        String Direccion = "";//Variable que almacenara la direccion del Empleado solicitante.
        String Nomina = "";//Variable que almacenara la nomina a la pertenece el empleado solicitante.
        String Nombre = "";//Variable que almacenara el nombre del empleado completo.
        String Sueldo_Mensual = "";//Variable que almecenara el sueldo mesual del empleado solicitante.

        try
        {
            Limpiar_Controles_Empleado_Solicitante();

            if (!string.IsNullOrEmpty(Empleado_ID))
            {
                Consulta_Empelado.P_No_Empleado = Empleado_ID;
                Dt_Empleados = Consulta_Empelado.Consulta_Empleados_General();

                if (Dt_Empleados != null)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        Consulta_Sindicatos.P_Sindicato_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Sindicato_ID].ToString().Trim();
                        Dt_Sindicatos = Consulta_Sindicatos.Consulta_Sindicato();
                        if (Dt_Sindicatos != null)
                        {
                            if (Dt_Sindicatos.Rows.Count > 0)
                            {
                                Sindicato = Dt_Sindicatos.Rows[0][Cat_Nom_Sindicatos.Campo_Nombre].ToString().Trim();
                            }
                        }

                        Consulta_Dependencias.P_Dependencia_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString().Trim();
                        Dt_Dependencias = Consulta_Dependencias.Consulta_Dependencias();
                        if (Dt_Dependencias != null)
                        {
                            if (Dt_Dependencias.Rows.Count > 0)
                            {
                                Dependencia = Dt_Dependencias.Rows[0][Cat_Dependencias.Campo_Nombre].ToString().Trim();
                            }
                        }

                        Consulta_Tipos_Nominas.P_Tipo_Nomina_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Tipo_Nomina_ID].ToString().Trim();
                        Dt_Tipos_Nomina = Consulta_Tipos_Nominas.Consulta_Datos_Tipo_Nomina();
                        if (Dt_Tipos_Nomina != null)
                        {
                            if (Dt_Tipos_Nomina.Rows.Count > 0)
                            {
                                Nomina = Dt_Tipos_Nomina.Rows[0][Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString().Trim();
                            }
                        }

                        Direccion = "Calle:" + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Calle].ToString() +
                                    "  Colonia:" + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Colonia].ToString() +
                                    "  CP:" + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Codigo_Postal].ToString();

                        Nombre = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Nombre].ToString() + " " + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString() +
                               " " + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString();

                        Sueldo_Mensual = string.Format("{0:#,###,##0.00}", (Convert.ToDouble((Dt_Empleados.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString().Trim().Equals("")) ? "0" : Dt_Empleados.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString().Trim()) * (new Cls_Utlidades_Nomina(DateTime.Now).P_Dias_Promedio_Mes_Anio)));

                        if (!string.IsNullOrEmpty(Nombre))
                        {
                            Txt_Nombre_Empleado_Solicitante.Text = Nombre;
                            Txt_Nombre_Empleado_Solicitante.ToolTip = Nombre;
                        }
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_RFC].ToString())) Txt_RFC_Empleado_Solicitante.Text = Dt_Empleados.Rows[0][Cat_Empleados.Campo_RFC].ToString();
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Fecha_Inicio].ToString())) Txt_Fecha_Ingreso_Empleado_Solicitante.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Fecha_Inicio].ToString()));
                        if (!string.IsNullOrEmpty(Sindicato)) Txt_Sindicato_Empleado_Solicitante.Text = Sindicato;
                        if (!string.IsNullOrEmpty(Dependencia)) Txt_Dependencia_Empelado_Solicitante.Text = Dependencia;
                        if (!string.IsNullOrEmpty(Direccion)) Txt_Direccion_Empleado_Solicitante.Text = Direccion;
                        if (!string.IsNullOrEmpty(Nomina)) Txt_Clase_Nomina_Empleado.Text = Nomina;
                        if (!string.IsNullOrEmpty(Sueldo_Mensual)) Txt_Sueldo_Mensual_Empleado_Solicitante.Text = Sueldo_Mensual;
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString())) Txt_Cuenta_Bancaria_Empleado_Solicitante.Text = Dt_Empleados.Rows[0][Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString();
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Banco_ID].ToString()))
                        {
                            Rs_Consulta_Cat_Nom_Bancos.P_Banco_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Banco_ID].ToString();
                            Dt_Bancos = Rs_Consulta_Cat_Nom_Bancos.Consulta_Bancos();
                            if (Dt_Bancos != null)
                            {
                                Txt_Banco_Empleado_Solicitante.Text = Dt_Bancos.Rows[0][Cat_Nom_Bancos.Campo_Nombre].ToString();
                            }
                        }
                        Img_Foto_Empleado_Solicitante.ImageUrl = (string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Ruta_Foto].ToString())) ? "~/paginas/imagenes/paginas/Sias_No_Disponible.JPG" : @Dt_Empleados.Rows[0][Cat_Empleados.Campo_Ruta_Foto].ToString();
                        Img_Foto_Empleado_Solicitante.DataBind();
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "El empleado no existe.";
                    }
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Ingrese el numero de empleado que desea buscar.";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Empleado_Aval
    ///DESCRIPCIÓN: Consulta los datos del Empleado Aval.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consulta_Datos_Empleado_Aval(String Empleado_ID)
    {
        Cls_Cat_Empleados_Negocios Consulta_Empelado = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        Cls_Cat_Nom_Sindicatos_Negocio Consulta_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Dependencias_Negocio Consulta_Dependencias = new Cls_Cat_Dependencias_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Ope_Nom_Pestamos_Negocio Consulta_Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Empleados = null;//Variable que almacenara la informacion del empelado.
        DataTable Dt_Sindicatos = null;//Variable que almacenara los datos del sindicato seleccioado.
        DataTable Dt_Dependencias = null;//Variable que almacenara los datos de la dependencia seleccionada.
        String Sindicato = "";//Variable que almacenara el Sindicato a la que pertence el empleado solicitante.
        String Dependencia = "";//Variable que almacenara la Dependencia a la que pertence el empleado solicitante.
        String Direccion = "";//Variable que almacenara la direccion del Empleado solicitante.
        String Nombre = "";//Variable que almacenara el nombre del empleado completo.
        String Sueldo_Mensual = "";//Variable que almecenara el sueldo mesual del empleado solicitante.

        try
        {
            //Limpiamos los controles de empleado aval del prestamo.
            Limpiar_Controles_Empleado_Aval();

            if (!string.IsNullOrEmpty(Empleado_ID))
            {
                Consulta_Empelado.P_No_Empleado = Empleado_ID;
                Dt_Empleados = Consulta_Empelado.Consulta_Empleados_General();

                if (Dt_Empleados != null)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        Consulta_Sindicatos.P_Sindicato_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Sindicato_ID].ToString().Trim();
                        Dt_Sindicatos = Consulta_Sindicatos.Consulta_Sindicato();
                        if (Dt_Sindicatos != null)
                        {
                            if (Dt_Sindicatos.Rows.Count > 0)
                            {
                                Sindicato = Dt_Sindicatos.Rows[0][Cat_Nom_Sindicatos.Campo_Nombre].ToString().Trim();
                            }
                        }

                        Consulta_Dependencias.P_Dependencia_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString().Trim();
                        Dt_Dependencias = Consulta_Dependencias.Consulta_Dependencias();
                        if (Dt_Dependencias != null)
                        {
                            if (Dt_Dependencias.Rows.Count > 0)
                            {
                                Dependencia = Dt_Dependencias.Rows[0][Cat_Dependencias.Campo_Nombre].ToString().Trim();
                            }
                        }

                        Direccion = "Calle:" + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Calle].ToString() +
                                    "  Colonia:" + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Colonia].ToString() +
                                    "  CP:" + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Codigo_Postal].ToString();

                        Nombre = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Nombre].ToString() + " " + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString() +
                               " " + Dt_Empleados.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString();

                        Sueldo_Mensual = string.Format("{0:#,###,##0.00}", (Convert.ToDouble((Dt_Empleados.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString().Trim().Equals("")) ? "0" : Dt_Empleados.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString().Trim()) * (new Cls_Utlidades_Nomina(DateTime.Now).P_Dias_Promedio_Mes_Anio)));

                        if (!string.IsNullOrEmpty(Nombre))
                        {
                            Txt_Nombre_Empleado_Aval.Text = Nombre;
                            Txt_Nombre_Empleado_Aval.ToolTip = Nombre;
                        }
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_RFC].ToString())) Txt_RFC_Aval.Text = Dt_Empleados.Rows[0][Cat_Empleados.Campo_RFC].ToString();
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Fecha_Inicio].ToString())) Txt_Fecha_Ingreso_Aval.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Fecha_Inicio].ToString()));
                        if (!string.IsNullOrEmpty(Sindicato)) Txt_Sindicato_Aval.Text = Sindicato;
                        if (!string.IsNullOrEmpty(Dependencia)) Txt_Dependencia_Aval.Text = Dependencia;
                        if (!string.IsNullOrEmpty(Direccion)) Txt_Direccion_Aval.Text = Direccion;
                        if (!string.IsNullOrEmpty(Sueldo_Mensual)) Txt_Sueldo_Mensual_Aval.Text = Sueldo_Mensual;
                        Img_Empleado_Aval.ImageUrl = (string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Ruta_Foto].ToString())) ? "~/paginas/imagenes/paginas/Sias_No_Disponible.JPG" : @Dt_Empleados.Rows[0][Cat_Empleados.Campo_Ruta_Foto].ToString();
                        Img_Empleado_Aval.DataBind();
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "El empleado no existe.";
                    }
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Ingrese el numero de empleado que desea buscar.";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion

    #region (Control Acceso Pagina)
    /// *****************************************************************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// 
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
    /// 
    /// PARÁMETROS: No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    protected void Configuracion_Acceso(String URL_Pagina)
    {
        List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Nuevo);
            Botones.Add(Btn_Modificar);
            Botones.Add(Btn_Eliminar);
            Botones.Add(Btn_Busqueda_Prestamos);

            if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
            {
                if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                {
                    //Consultamos el menu de la página.
                    Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                    if (Dr_Menus.Length > 0)
                    {
                        //Validamos que el menu consultado corresponda a la página a validar.
                        if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                        {
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al habilitar la configuración de accesos a la página. Error: [" + Ex.Message + "]");
        }
    }
    /// *****************************************************************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// 
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
    /// 
    /// PARÁMETROS: No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    protected void Configuracion_Acceso_Autorizar_Cancelar(String URL_Pagina)
    {
        List<Button> Botones = new List<Button>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Autorizacion_Prestamos);
            Botones.Add(Btn_Cancelacion_Prestamo);

            if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
            {
                if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                {
                    //Consultamos el menu de la página.
                    Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                    if (Dr_Menus.Length > 0)
                    {
                        //Validamos que el menu consultado corresponda a la página a validar.
                        if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                        {
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al habilitar la configuración de accesos a la página. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Es_Numero(String Cadena)
    {
        Boolean Resultado = true;
        Char[] Array = Cadena.ToCharArray();
        try
        {
            for (int index = 0; index < Array.Length; index++)
            {
                if (!Char.IsDigit(Array[index])) return false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Eventos Operacion Alta - Modificar - Eliminar - Consultar)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Alta de una Solicitud Prestamo
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 02/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip.Equals("Nuevo"))
            {
                Limpiar_Controles();
                Habilitar_Controles("Nuevo");
            }
            else
            {
                if (Validar_Datos_Vacaciones_Empleado())
                {
                    if (!Txt_No_Empleado_Aval.Text.Trim().Equals(Txt_No_Empleado_Solicitante_Prestamo.Text.Trim()))
                    {
                        if (Validaciones_Recursos_Humanos())
                        {
                            Alta_Solicitud_Prestamo();
                            Configuracion_Inicial();
                        }
                    }
                    else {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "El Empleado solicitante no puede ser su propio aval.";
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Modificar el prestamo
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 02/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Btn_Modificar.ToolTip.Equals("Modificar"))
            {
                if (!Txt_No_Solocitud.Text.Equals(""))
                {
                    //if (!Cmb_Estatus_Solicitud_Prestamo.SelectedItem.Text.Trim().ToUpper().Equals("PENDIENTE"))
                    //{
                    //    Lbl_Mensaje_Error.Visible = true;
                    //    Img_Error.Visible = true;
                    //    Lbl_Mensaje_Error.Text = "El registro ya fea aceptado, ya no es posible realizar ninguna modificacion <br>";
                    //}
                    //else
                    //{
                        Habilitar_Controles("Modificar");//Habilita la configuracion de los controles para ejecutar la operacion de modificar.
                    //}
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea modificar sus datos <br>";
                }
            }
            else
            {
                if (Validar_Datos_Vacaciones_Empleado())
                {
                    if (!Txt_No_Empleado_Aval.Text.Trim().Equals(Txt_No_Empleado_Solicitante_Prestamo.Text.Trim()))
                    {
                        if (Validaciones_Recursos_Humanos())
                        {
                            Modificar_Solicitud_Prestamo();
                            Configuracion_Inicial();
                        }
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "El Empleado solicitante no puede ser su propio aval.";
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Eliminar un la solicitud de prestamo seleccionada.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 02/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Btn_Eliminar.ToolTip.Equals("Eliminar"))
            {
                if (!Txt_No_Solocitud.Text.Equals(""))
                {
                    Eliminar_Solicitud_Prestamo();
                    Limpiar_Controles();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea eliminar <br>";
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Salir de la Operacion Actual
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Inicio")
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Configuracion_Inicial();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Empleado_Solicitante_Click
    ///DESCRIPCIÓN: Consulta al Empleado en el sistema por su numero de empleado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Empleado_Solicitante_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Consultar_Mostrar_Informacion_Empleado_Solicitante();
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Empleado_Aval_Click
    ///DESCRIPCIÓN: Consulta al Empleado en el sistema por su numero de empleado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Empleado_Aval_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Consultar_Mostrar_Informacion_Empleado_Aval();
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Empleado_Aval_Click
    ///DESCRIPCIÓN: Consulta las solicitudes de prestamos vigentes actualmente en el 
    ///sistema.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Solicitudes_Prestamos_Click(object sender, EventArgs e)
    {
        try
        {
            Consulta_Solicitudes_Prestamos(0);
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Prestamos_Click
    ///DESCRIPCIÓN: Abre el Modal_Popup para realizar la busqueda.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 02/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Prestamos_Click(object sender, EventArgs e) {
        Mpe_Busqueda_Prestamos.Show();
        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Prestamos_Click
    ///DESCRIPCIÓN: Cierra el Modal_Popup de la busqueda.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 02/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Cerrar_Ventana_Autorizacion_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Busqueda_Prestamos.Hide();
        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Guardar_Autorizacion_Prestamos_Click
    /// DESCRIPCION : Ejecuta la autorizacion de los Prestamos
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************  
    protected void Btn_Guardar_Autorizacion_Prestamos_Click(object sender, EventArgs e)
    {
        Cls_Ope_Nom_Pestamos_Negocio Consulta_Solicitudes_Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocio.
        try
        {
            Consulta_Solicitudes_Prestamos.P_No_Solicitud = Txt_No_Solocitud.Text.Trim();
            Consulta_Solicitudes_Prestamos.P_Estatus_Solicitud = (Chk_Autorizar.Checked) ? "Autorizado" : "Rechazado";
            Consulta_Solicitudes_Prestamos.P_Comentarios_Rechazo = Txt_Autorizacion_Comentarios.Text;

            //Si se autoriza el prestamo los comentarios no son obligatorios.
            if (Chk_Autorizar.Checked)
            {
                if (Consulta_Solicitudes_Prestamos.Autorizacion_Solicitudes_Prestamos())
                {
                    Configuracion_Inicial();
                    Lbl_Observaciones_Autorizacion.Text = "";
                    Chk_Autorizar.Checked = true;
                }
            }
            else if (!string.IsNullOrEmpty(Consulta_Solicitudes_Prestamos.P_Comentarios_Rechazo.Trim()))
            {
                //Si se ingresaron los comentarios se cambia el estatus a rechazado.
                if (Consulta_Solicitudes_Prestamos.Autorizacion_Solicitudes_Prestamos())
                {
                    Configuracion_Inicial();
                    Lbl_Observaciones_Autorizacion.Text = "";
                    Chk_Autorizar.Checked = true;
                }
            }
            else {
                //Si el estatus es rechazado y no se ingreso el motivo, no sera posible dar cambiar el estatus.
                Lbl_Observaciones_Autorizacion.Text = "<font style='color:red;font-size:11px;'>Las observaciones sobre el motivo de rechazo del prestamo son obligatorias.</font>";
                MPE_Autorizacion_Prestamos.Show();
                Chk_Autorizar.Checked = true;
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Error al autorizar la Prestamo del empleado. Error: [" + Ex.Message.ToString() + "]";
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Cancelar_Prestamo_Click
    /// DESCRIPCION : Ejecuta la cancelacion del Prestamos
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 03/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************  
    protected void Btn_Cancelar_Prestamo_Click(object sender, EventArgs e) {
        Cls_Ope_Nom_Pestamos_Negocio Consulta_Solicitudes_Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocio.
        try
        {
            Consulta_Solicitudes_Prestamos.P_No_Solicitud = Txt_No_Solocitud.Text.Trim();
            Consulta_Solicitudes_Prestamos.P_Estatus_Solicitud = (Chk_Cancelar_Prestamo.Checked) ? "Cancelado" : "Autorizado";
            Consulta_Solicitudes_Prestamos.P_Referencia_Recibo_Pago = Txt_Cancelacion_Referencia_Pago.Text.Trim();
            Consulta_Solicitudes_Prestamos.P_Comentarios_Cancelacion = Txt_Cancelacion_Comentarios.Text.Trim();

            //Si se autoriza el prestamo los comentarios no son obligatorios.
            if (Chk_Cancelar_Prestamo.Checked)
            {
                //Se validan que se hallan ingresado el motivo de la cancelacion del prestamo.
                if (!string.IsNullOrEmpty(Txt_Cancelacion_Comentarios.Text.Trim()) && !string.IsNullOrEmpty(Txt_Cancelacion_Referencia_Pago.Text.Trim()))
                {
                    //Si los comentarios existen se guarda el nuevo estatus como cancelado.
                    if (Consulta_Solicitudes_Prestamos.Cancelacion_Por_Liquidacion_Prestamos())
                    {
                        Configuracion_Inicial();//Cambio de estatus.
                        Lbl_Observaciones_Cancelacion.Text = "";
                        Chk_Cancelar_Prestamo.Checked = false;
                    }
                }
                else {
                    if (string.IsNullOrEmpty(Txt_Cancelacion_Comentarios.Text.Trim())) Lbl_Observaciones_Cancelacion.Text = "<font style='color:red;font-size:10px;'> +Las observaciones acerca de la cancelacion del prestamo son obligatorias.</font><br />";
                    if (string.IsNullOrEmpty(Txt_Cancelacion_Referencia_Pago.Text.Trim())) Lbl_Observaciones_Cancelacion.Text = "<font style='color:red;font-size:10px;'> +La Referencia del pago para la cancelacion del prestamo es obligatorio.</font>";
                    Mpe_Cancelar_Prestamo.Show();
                    Chk_Cancelar_Prestamo.Checked = false;
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cancelar el prestamo. Error: [" + Ex.Message + "]");
        }
    }
    
    #endregion

    #region (Eventos Combos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Calendario_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 index = Cmb_Calendario_Nomina.SelectedIndex;
        if (index > 0)
        {
            Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
        }
        else {
            Cmb_Periodos_Catorcenales_Nomina.DataSource = new DataTable();
            Cmb_Periodos_Catorcenales_Nomina.DataBind();
        }

        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona la fecha de Inicio del periodo que se le comenzara a 
    ///descontar al empleado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Nom_Pestamos_Negocio Consulta_Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de Conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenara los detalles del periodo seleccionado.
        try
        {
            Consulta_Prestamos.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());
            Consulta_Prestamos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Dt_Detalles_Nomina = Consulta_Prestamos.Consultar_Fecha_Inicio_Periodo_Pago();

            if (Dt_Detalles_Nomina != null) {
                if (Dt_Detalles_Nomina.Rows.Count > 0) {
                    Txt_Fecha_Inicio_Pago_Prestamo.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString()));

                    if (!string.IsNullOrEmpty(Txt_No_Pagos.Text.Trim()))
                    {
                        Calcular_Fecha_Termino_Prestamo();
                    }
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar la Fecha de Inicio en la que al empelado se le comenzara a descontar. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Proveedor_SelectedIndexChanged
    ///DESCRIPCIÓN: Carga las deducciones del proveedor seleccionado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 10/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Proveedor_SelectedIndexChanged(object sender, EventArgs e)
    {
        String Proveedor_ID = "";
        try
        {
            Proveedor_ID = Cmb_Proveedor.SelectedValue.Trim();
            Consulta_Deducciones(Proveedor_ID);
            Cmb_Deduccion.Enabled = true;

            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    #endregion

    #region (Eventos TextBox)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Importe_Prestamo_TextChanged
    ///DESCRIPCIÓN: Realiza la suma del Importe del Prestamo mas el Importe del 
    ///Interes.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Importe_Prestamo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Sumar_Importes_Prestamo_Interes();//Aqui se realiza la sumatoria de los importes.
            Calculo_Abono();
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Importe_Interes_TextChanged
    ///DESCRIPCIÓN: Realiza la suma del Importe del Prestamo mas el Importe del 
    ///Interes.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Importe_Interes_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(Txt_Importe_Prestamo.Text.Trim()))
            {
                Sumar_Importes_Prestamo_Interes();//Aqui se realiza la sumatoria de los importes.
                Calculo_Abono();
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_No_Pagos_TextChanged
    ///DESCRIPCIÓN: Realiza el calculo del abono.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_No_Pagos_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex > 0)
            {
                Calcular_Fecha_Termino_Prestamo();
            }
            Calculo_Abono();
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_No_Empleado_Solicitante_Prestamo_TextChanged
    ///DESCRIPCIÓN: Consulta al Empleado en el sistema por su numero de empleado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 02/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_No_Empleado_Solicitante_Prestamo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Consultar_Mostrar_Informacion_Empleado_Solicitante();
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_No_Empleado_Aval_TextChanged
    ///DESCRIPCIÓN: Consulta al Empleado en el sistema por su numero de empleado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 02/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_No_Empleado_Aval_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Consultar_Mostrar_Informacion_Empleado_Aval();
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Prestamos_Internos_Presidencia();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    #endregion

    #endregion


    /// *********************************************************************************************************************************************
    /// NOMBRE: [Consultar_Tipo_Nomina_Empleado_Solicitante]
    /// 
    /// DESCRIPCIÓN: Consulta si el empleado de acuerdo al tipo de nómina al que pertence aplica para un prestamo. Donde si el empleado es Base, 
    ///              Subsemún o Subrogados deberá tener una antigüedad de 6 meses. Si es Eventual la antigüedad deberá ser de 1 año.
    /// 
    /// PARÁMETROS: Sin parámetros.
    /// 
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete. 
    /// FECHA CREÓ: 23/Marzo/2011 11:20 am
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// *********************************************************************************************************************************************
    private Boolean Consultar_Tipo_Nomina_Empleado_Solicitante()
    {
        Cls_Ope_Nom_Pestamos_Negocio Obj_Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexión con la capa de negocios.
        String Tipo_Nomina_ID = null;//Variable que almacenara la informacion del tipo de nomina consultada.
        Boolean Aplica_Prestamo = false;

        try
        {
            Obj_Prestamos.P_No_Empleado = Txt_No_Empleado_Solicitante_Prestamo.Text.Trim();
            Tipo_Nomina_ID = Obj_Prestamos.Consultar_Tipo_Nomina_Empleado_Solicitante();

            if (!String.IsNullOrEmpty(Tipo_Nomina_ID)) {
                switch (Tipo_Nomina_ID)
                {
                    case "00001":
                        Aplica_Prestamo = Aplica_Prestamo_Empleado_Solicitante(Txt_No_Empleado_Solicitante_Prestamo.Text.Trim(), 6);
                        break;
                    case "00002":
                        Aplica_Prestamo = Aplica_Prestamo_Empleado_Solicitante(Txt_No_Empleado_Solicitante_Prestamo.Text.Trim(), 12);
                        break;
                    case "00004":
                        Aplica_Prestamo = Aplica_Prestamo_Empleado_Solicitante(Txt_No_Empleado_Solicitante_Prestamo.Text.Trim(), 6);
                        break;
                    case "00009":
                        Aplica_Prestamo = Aplica_Prestamo_Empleado_Solicitante(Txt_No_Empleado_Solicitante_Prestamo.Text.Trim(), 6);
                        break;                        
                    default:
                        break;
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar el tipo de nomina a la que pertence el empleado. Error: [" + Ex.Message + "]");
        }
        return Aplica_Prestamo;
    }
    /// *********************************************************************************************************************************************
    /// NOMBRE: [Aplica_Prestamo_Empleado_Solicitante]
    /// 
    /// DESCRIPCIÓN: Consulta si el empleado de acuerdo al tipo de nómina al que pertence aplica para un prestamo. Donde si el empleado es Base, 
    ///              Subsemún o Subrogados deberá tener una antigüedad de 6 meses. Si es Eventual la antigüedad deberá ser de 1 año.
    /// 
    /// PARÁMETROS: No_Empleado.- Identifcador con el que recursos humanos realizara las operacoiones sobre el empleado.
    /// 
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete. 
    /// FECHA CREÓ: 23/Marzo/2011 11:20 am
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// *********************************************************************************************************************************************
    private Boolean Aplica_Prestamo_Empleado_Solicitante(String No_Empleado, Int32 No_Meses)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexion a la capa de negocios.
        DataTable Dt_Informacion_Empleado = null;//Variable que almacena la informacion del empleado.
        DateTime Fecha_Ingreso = new DateTime();
        long Meses_Antiguedad = 0;
        Boolean Aplica_Prestamo = false;

        try
        {
            Obj_Empleados.P_No_Empleado = No_Empleado;
            Dt_Informacion_Empleado = Obj_Empleados.Consulta_Empleados_General();

            if (Dt_Informacion_Empleado is DataTable) {
                if (Dt_Informacion_Empleado.Rows.Count > 0) {
                    foreach (DataRow Empleado in Dt_Informacion_Empleado.Rows) {
                        if (Empleado is DataRow) {
                            if (!String.IsNullOrEmpty(Empleado[Cat_Empleados.Campo_Fecha_Inicio].ToString())) {
                                Fecha_Ingreso = Convert.ToDateTime(Empleado[Cat_Empleados.Campo_Fecha_Inicio].ToString());

                                Meses_Antiguedad = Cls_DateAndTime.DateDiff(DateInterval.Month, Fecha_Ingreso, DateTime.Now);

                                if (Meses_Antiguedad > No_Meses)
                                {
                                    Aplica_Prestamo = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener la antiguedad del empleado en presidencia. Error: [" + Ex.Message + "]");
        }
        return Aplica_Prestamo;
    }
    /// *********************************************************************************************************************************************
    /// NOMBRE: [Aplica_Sueldo_Prestamo]
    /// 
    /// DESCRIPCIÓN: Valida si el sueldo mensual del empleado no el mayor al parametro establecido como tope salarial 
    ///              para otorgar prestamos internos a los empleados.
    /// 
    /// PARÁMETROS: Sin parametros.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete. 
    /// FECHA CREÓ: 23/Marzo/2011 11:20 am
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// *********************************************************************************************************************************************
    private Boolean Aplica_Sueldo_Prestamo()
    {
        Cls_Cat_Nom_Parametros_Negocio Obj_Parametros = new Cls_Cat_Nom_Parametros_Negocio();//Variable de conexión con la capa de negocios.
        Cls_Cat_Empleados_Negocios INF_EMPLEADO = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        DataTable Dt_Inf_Parametro = null;//Variable que almacena la información del parámetro consultado.
        Double Sueldo_Limite_Prestamo = 0.0;//Variable que almacena el sueldo limite que debe tener un empleado para poder solicitar un prestamo.
        Boolean Aplica_Prestamo = false;//Variable que almacena si el empleado aplica para un prestamo.
        Double Importe_Total_Prestamo = 0.0;//Variable que almacena el monto total del prestamo.

        try
        {
            Dt_Inf_Parametro = Obj_Parametros.Consulta_Parametros();//Consultamos la tabla de parámetros de la nómina.

            if (Dt_Inf_Parametro is DataTable) {
                if (Dt_Inf_Parametro.Rows.Count > 0) {
                    foreach (DataRow Parametro in Dt_Inf_Parametro.Rows) {
                        if (Parametro is DataRow) {
                            if (!String.IsNullOrEmpty(Parametro[Cat_Nom_Parametros.Campo_Salario_Limite_Prestamo].ToString())) {
                                //Obtenemos el salario limite para poder otorgar un prestamo a los empleados.
                                Sueldo_Limite_Prestamo = Convert.ToDouble(Parametro[Cat_Nom_Parametros.Campo_Salario_Limite_Prestamo].ToString());

                                INF_EMPLEADO = Consultar_Informacion_Empleado(String.Empty, Txt_No_Empleado_Solicitante_Prestamo.Text.Trim(), String.Empty);

                                //Validamos que el sueldo mensual no exeda el salario el tope establecido para la autorizacion de 
                                //prestamos.
                                if ((INF_EMPLEADO.P_Salario_Diario * Cls_Utlidades_Nomina.Dias_Mes_Fijo) <= Sueldo_Limite_Prestamo)
                                {
                                    if (!String.IsNullOrEmpty(Txt_Importe_Prestamo.Text.Trim()))
                                    {
                                        Importe_Total_Prestamo = Convert.ToDouble(Txt_Importe_Prestamo.Text.Trim());
                                        //Validamos que el importe total del prestamo no sea mayor al salario mensual del empleado.
                                        if (Importe_Total_Prestamo <= (INF_EMPLEADO.P_Salario_Diario * Cls_Utlidades_Nomina.Dias_Mes_Fijo))
                                        {
                                            Aplica_Prestamo = true;
                                        }
                                        else
                                        {
                                            Lbl_Mensaje_Error.Text = "Importe_Invalido";
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
            throw new Exception("Error al validar que el empleado que solicita el prestamo no tiene un sueldo que rebase el parámetro establecido como tope salarial para" +
                                "otorgar un prestamo a los empleados solicitantes. Error: [" + Ex.Message + "]");
        }
        return Aplica_Prestamo;
    }
    ///*****************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Obtener_Salario_Diario_Empleado
    /// DESCRIPCION : Obtenemos el salario diario del empleado segun su puesto actual.
    /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
    ///                           su salario diario.
    ///              Calculo: 
    ///                      Salario_Diario = (Sueldo_Mensual_Puesto/(365/12))
    ///                      
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 16/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///***************************************************************************************************************************************************** 
    public Double Obtener_Salario_Diario_Empleado(String Empleado_ID)
    {
        Cls_Cat_Empleados_Negocios Consulta_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        DataTable Dt_Empleados = null;//Variable que almacenara los datos del empleado consultado.
        DataTable Dt_Datos_Puesto = null;//Variable que almacenara la informacion del puesto del e            
        String Salario_Mensual_Puesto = "";//Variable que almacenara el salario del puesto del empleado.
        String Puesto_Empleado = "";//Variable que almacenara el Puesto_ID
        Double Salario_Diario = 0.0;//Variable que almacenara el salario diario del empleado que le corresponde segun el puesto.
        Double DIAS_MES = (new Cls_Utlidades_Nomina(DateTime.Now).P_Dias_Promedio_Mes_Anio);//Variable que almacenara los dias del mes que se tomaran para obtener el salario diario mensual.

        try
        {
            //Consultamos el puesto del empleado que tiene actualmente.
            Consulta_Empleados.P_Empleado_ID = Empleado_ID;
            Dt_Empleados = Consulta_Empleados.Consulta_Empleados_General();
            //Validamos que exista algun registro que corresponda con el id del empleado buscado.
            if (Dt_Empleados != null)
            {
                if (Dt_Empleados.Rows.Count > 0)
                {
                    //Obtenemos el puesto del empleado, que nos servira para obtener el salario mensual que le corresponde al puesto.
                    if (!String.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Puesto_ID].ToString())) Puesto_Empleado = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Puesto_ID].ToString();
                }
            }

            //Consultar el salario mensual del puesto, para obtener el salario diario del empleado.
            Consulta_Empleados.P_Puesto_ID = Puesto_Empleado;
            Dt_Datos_Puesto = Consulta_Empleados.Consulta_Puestos_Empleados();//Consultamos la informacion del puesto
            //Validamos que exista algun resultado que corresponda con el id del puesto buscado.
            if (Dt_Datos_Puesto != null)
            {
                if (Dt_Datos_Puesto.Rows.Count > 0)
                {
                    //Obtenemos el salario diario del empleado, esto de acuerdo al salario mensual que le corresponde al puesto.
                    Salario_Mensual_Puesto = HttpUtility.HtmlDecode(Dt_Datos_Puesto.Rows[0][Cat_Puestos.Campo_Salario_Mensual].ToString());
                    if (!string.IsNullOrEmpty(Salario_Mensual_Puesto))
                    {
                        Salario_Diario = (Convert.ToDouble(Salario_Mensual_Puesto) / DIAS_MES);
                    }
                }
                else
                {
                    Salario_Diario = 0;
                }
            }
            else
            {
                Salario_Diario = 0;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener el salario diario del empleado. Error: [" + Ex.Message + "]");
        }
        return Salario_Diario;
    }
    ///*****************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: [Validar_Salario_Empleado_Aval_Contra_Salario_Empleado_Solicitante]
    /// 
    /// DESCRIPCION : Validamos el salario del empleado que funcionara como aval del prestamo fuera mayor o igual que el del empleado 
    ///               que hace la solicitud del prestamo.
    /// 
    /// PARAMETROS:  sin parámetros.
    ///                      
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///***************************************************************************************************************************************************** 
    private Boolean Validar_Salario_Empleado_Aval_Contra_Salario_Empleado_Solicitante()
    {
        Double Salario_Mensual_Empleado_Solicitante = 0;//Variable que almacena el salario mensual del empleado que realiza la solicitud del prestamo.
        Double Salario_Munsual_Empleado_Aval = 0;//Variable que firmara como aval del prestamo.
        Boolean Aplica_Prestamo = false;//Variable que almacenara si el empleado aval tiene un ingreso mayor o igual al empleado solicitante.
        Cls_Cat_Empleados_Negocios INF_EMPLEADO_SOLICITANTE = null;
        Cls_Cat_Empleados_Negocios INF_EMPLEADO_AVAL = null;

        try
        {
            INF_EMPLEADO_SOLICITANTE = Consultar_Informacion_Empleado(String.Empty, Txt_No_Empleado_Solicitante_Prestamo.Text.Trim(), String.Empty);
            INF_EMPLEADO_AVAL = Consultar_Informacion_Empleado(String.Empty, Txt_No_Empleado_Aval.Text.Trim(), String.Empty);

            Salario_Mensual_Empleado_Solicitante = INF_EMPLEADO_SOLICITANTE.P_Salario_Diario * Cls_Utlidades_Nomina.Dias_Mes_Fijo;//Consultamos el salario mensual del empleado solicitante.
            Salario_Munsual_Empleado_Aval = INF_EMPLEADO_AVAL.P_Salario_Diario * Cls_Utlidades_Nomina.Dias_Mes_Fijo;//Consultamos el salario mensual del empleado aval.

            if (Salario_Munsual_Empleado_Aval >= Salario_Mensual_Empleado_Solicitante) {
                Aplica_Prestamo = true;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar que el salario del empleado aval se igual o mayor que el del empleado solicitante. Error: [" + Ex.Message + "]");
        }
        return Aplica_Prestamo;
    }
    ///*****************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Validaciones_Recursos_Humanos
    /// DESCRIPCION : Obtenemos el salario diario del empleado segun su puesto actual.
    /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
    ///                           su salario diario.
    ///              Calculo: 
    ///                      Salario_Diario = (Sueldo_Mensual_Puesto/(365/12))
    ///                      
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 16/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///***************************************************************************************************************************************************** 
    private Boolean Validaciones_Recursos_Humanos()
    {
        Boolean Estatus = true;//variable que almacena si al empleado se le permitira realizar la solicitud del prestamo.

        if (Chk_Omitir_Validaciones.Checked)
        {
            if (!Aplica_Sueldo_Prestamo())
            {
                Estatus = false;
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                if (Lbl_Mensaje_Error.Text.Trim().Equals("Importe_Invalido"))
                {
                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Text = "El Importe Total del Prestamo no puede ser mayor al sueldo mensual del empleado.";
                }
                else
                {
                    Lbl_Mensaje_Error.Text = "El empleado tiene un salario mensual mayor que el establecido como tope salarial para otorgar prestamos.";
                }
            }


            if (!Consultar_Tipo_Nomina_Empleado_Solicitante())
            {
                Estatus = false;
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "El Empleado no tiene la antiguedad requerida para poder otorgarle un prestamo de acuerdo a su tipo de nomina.";
            }

            if (!Validar_Salario_Empleado_Aval_Contra_Salario_Empleado_Solicitante())
            {
                Estatus = false;
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "El Empleado que servira de aval debe tener un salario mensual igual o mayor que el empleado que realiza la solicitud.";
            }

            if (!Validar_Monto_Porcentaje_Ingresado())
            {
                Estatus = false;
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Si la cantidad el importe de interes es una porcentaje no debe ser meno a cero o mayor a 100.";
            }
        }

        return Estatus;
    }
    ///*****************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Chk_Aplica_Porcentaje_CheckedChanged
    /// DESCRIPCION : Obtenemos el salario diario del empleado segun su puesto actual.
    ///                      
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 16/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///***************************************************************************************************************************************************** 
    protected void Chk_Aplica_Porcentaje_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Lbl_Importe_Interes.Text.Trim().Contains("%"))
            {
                Lbl_Importe_Interes.Text = "Importe Interes";
            }
            else
            {
                Lbl_Importe_Interes.Text = "Importe Interes [%]";
            }

            Sumar_Importes_Prestamo_Interes();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al seleccionar si el importe de interes aplica como una cantidad fija o como un porcentaje. Error[" + Ex.Message + "]");
        }
    }
    ///*****************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Chk_Aplica_Porcentaje_CheckedChanged
    /// DESCRIPCION : Obtenemos el salario diario del empleado segun su puesto actual.
    ///                      
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 16/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///***************************************************************************************************************************************************** 
    private Boolean Validar_Monto_Porcentaje_Ingresado()
    {
        Double Cantidad = 0;            //Variable que almacena la cantidad ingresada como importe de interes. 
        Boolean Cantidad_Valida = false;
        try
        {
            if (!String.IsNullOrEmpty(Txt_Importe_Interes.Text.Trim()))
            {
                Cantidad = Convert.ToDouble(Txt_Importe_Interes.Text.Trim());

                if (Lbl_Importe_Interes.Text.Trim().Contains("%"))
                {
                    if ((Cantidad >= 0) && (Cantidad <= 100))
                    {
                        Cantidad_Valida = true;
                    }
                }
                else
                {
                    Cantidad_Valida = true;
                }
            }
            else { Cantidad_Valida = true; }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar el monto o la cantidad ingresada en el importe de interes del prestamo. Error: [" + Ex.Message + "]");
        }
        return Cantidad_Valida;
    }
    /// ***********************************************************************************
    /// Nombre: Consultar_Informacion_Empleado
    /// 
    /// Descripción: Consulta la información general del empleado.
    /// 
    /// Parámetros: No_Empleado.- Identificador interno del sistema para las operaciones que
    ///                           se realizan sobre los empelados.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 16/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***********************************************************************************
    protected Cls_Cat_Empleados_Negocios Consultar_Informacion_Empleado(String Empleado_ID, String No_Empleado, String Nombre)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
        Cls_Cat_Empleados_Negocios INF_EMPLEADO = new Cls_Cat_Empleados_Negocios();//Variable que almacenara la información del empleado.
        DataTable Dt_Empleado = null;//Variable que almacena el registro búscado del empleado.

        try
        {
            Obj_Empleados.P_Empleado_ID = Empleado_ID;
            Obj_Empleados.P_No_Empleado = No_Empleado;
            Obj_Empleados.P_Nombre = Nombre;
            Dt_Empleado = Obj_Empleados.Consulta_Empleados_General();//Consultamos la información del empleado.

            if (Dt_Empleado is DataTable)
            {
                if (Dt_Empleado.Rows.Count > 0)
                {
                    foreach (DataRow EMPLEADO in Dt_Empleado.Rows)
                    {
                        if (EMPLEADO is DataRow)
                        {
                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString().Trim()))
                                INF_EMPLEADO.P_Tipo_Nomina_ID = EMPLEADO[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString().Trim();

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString().Trim()))
                                INF_EMPLEADO.P_No_Empleado = EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString().Trim();

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Zona_ID].ToString().Trim()))
                                INF_EMPLEADO.P_Zona_ID = EMPLEADO[Cat_Empleados.Campo_Zona_ID].ToString().Trim();

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim()))
                                INF_EMPLEADO.P_Empleado_ID = EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim();

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString().Trim()))
                                INF_EMPLEADO.P_Salario_Diario = Convert.ToDouble(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString().Trim());

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Terceros_ID].ToString().Trim()))
                                INF_EMPLEADO.P_Terceros_ID = EMPLEADO[Cat_Empleados.Campo_Terceros_ID].ToString().Trim();

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString().Trim()))
                                INF_EMPLEADO.P_Salario_Diario = Convert.ToDouble(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString().Trim());

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Dependencia_ID].ToString().Trim()))
                                INF_EMPLEADO.P_Dependencia_ID = EMPLEADO[Cat_Empleados.Campo_Dependencia_ID].ToString().Trim();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las información del empleado. Error: [" + Ex.Message + "]");
        }
        return INF_EMPLEADO;
    }
}