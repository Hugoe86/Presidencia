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
using Presidencia.Ajuste_ISR.Negocio;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Proveedores.Negocios;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.Cat_Parametros_Nomina.Negocio;
using System.Collections.Generic;
using Presidencia.Ayudante_Informacion;

public partial class paginas_Nomina_Frm_Ope_Nom_Ajuste_ISR : System.Web.UI.Page
{
    #region (Page_Load)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Carga la configuracion inicial de la pagina.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Diciembre/2010
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
                Configuracion_Inicial();
            }
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Ajuste_ISR_OnSelectedIndexChanged
    ///DESCRIPCIÓN: Carga los datos con la informacion del Ajuste de ISR seleccionado..
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Ajuste_ISR_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Nom_Ajuste_ISR_Negocio Consulta_Ajuste_ISR = new Cls_Ope_Nom_Ajuste_ISR_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Empleados_Negocios Consulta_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        DataTable Dt_Ajuste_ISR = null;//Variable que almacenara una lista de Ajustes de ISR
        DataTable Dt_Empleados = null;//Variable que almacenara una lista de empleados.
        String No_Empleado = "";//Variable que alamcenara el numero del empleado que realiza la solicitud del prestamo.
        
        try
        {
            if (Grid_Ajuste_ISR.SelectedIndex != -1)
            {
                //Filtros de Busqueda
                Consulta_Ajuste_ISR.P_No_Ajuste_ISR = Grid_Ajuste_ISR.SelectedRow.Cells[1].Text;
                Dt_Ajuste_ISR = Consulta_Ajuste_ISR.Consulta_Ajuste_ISR().P_Dt_Ajustes_ISR;

                if (Dt_Ajuste_ISR != null)
                {
                    if (Dt_Ajuste_ISR.Rows.Count > 0)
                    {
                        //Consultamos el Numero de Empleado Solicitante
                        Consulta_Empleados.P_Empleado_ID = Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Empleado_ID].ToString();
                        Dt_Empleados = Consulta_Empleados.Consulta_Datos_Empleado();

                        if (Dt_Empleados != null)
                            if (Dt_Empleados.Rows.Count > 0)
                                No_Empleado = Dt_Empleados.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString();

                        //Se realiza la carga de los controles con la informacion del prestamo seleccionado.
                        Consultar_Datos_Empleado(No_Empleado);
                        Txt_No_Empleado.Text = No_Empleado;
                        Txt_Ajuste_ISR .Text = Grid_Ajuste_ISR.SelectedRow.Cells[1].Text;

                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR].ToString().Trim())) Txt_Fecha_Alta_Ajuste_ISR_CalendarExtender.SelectedDate = Convert.ToDateTime(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Fecha_Creo].ToString().Trim());
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Estatus_Ajuste_ISR].ToString().Trim())) Cmb_Estatus_Ajuste_ISR.SelectedIndex = Cmb_Estatus_Ajuste_ISR.Items.IndexOf(Cmb_Estatus_Ajuste_ISR.Items.FindByText(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Estatus_Ajuste_ISR].ToString().Trim()));
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Nomina_ID].ToString().Trim())) Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf(Cmb_Calendario_Nomina.Items.FindByValue(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Nomina_ID].ToString().Trim()));
                        Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());//Consultamos los periodos catorcenales de la nomina seleccionada.
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_No_Nomina].ToString().Trim())) Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Periodos_Catorcenales_Nomina.Items.IndexOf(Cmb_Periodos_Catorcenales_Nomina.Items.FindByText(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_No_Nomina].ToString().Trim()));
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Fecha_Inicio_Pago].ToString().Trim())) Txt_Fecha_Inicio_Ajuste_ISR.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Fecha_Inicio_Pago].ToString().Trim()));
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Fecha_Termino_Pago].ToString().Trim())) Txt_Fecha_Termino_Ajuste_ISR.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Fecha_Termino_Pago].ToString().Trim()));
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Comentarios_Ajuste].ToString().Trim())) Txt_Comentarios_Ajuste_ISR.Text = Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Comentarios_Ajuste].ToString();
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustar].ToString().Trim())) Txt_Total_ISR_Ajustar.Text = string.Format("{0:#,###,##0.00}", Convert.ToDouble(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustar].ToString().Trim()));
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_No_Catorcenas].ToString().Trim())) Txt_No_Catorcenas.Text = Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_No_Catorcenas].ToString().Trim();
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_No_Pago].ToString().Trim())) Txt_No_Pago.Text = Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_No_Pago].ToString().Trim();
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Pago_Catorcenal_ISR].ToString().Trim())) Txt_Pago_Catorcenal_ISR.Text = string.Format("{0:#,###,##0.00}", Convert.ToDouble(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Pago_Catorcenal_ISR].ToString().Trim()));
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado].ToString().Trim())) Txt_Total_ISR_Ajustado.Text = string.Format("{0:#,###,##0.00}", Convert.ToDouble((Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado].ToString().Trim())));
                        if (!string.IsNullOrEmpty(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Percepcion_Deduccion_ID].ToString().Trim())) Cmb_Percepcion.SelectedIndex = Cmb_Percepcion.Items.IndexOf(Cmb_Percepcion.Items.FindByValue(Dt_Ajuste_ISR.Rows[0][Ope_Nom_Ajuste_ISR.Campo_Percepcion_Deduccion_ID].ToString()));
                    }
                }
            }
                Mpe_Busqueda_Ajuste_ISR.Hide();
                Limpiar_Controles_Busqueda_Ajuste_ISR();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al seleccionar un prestamo del Grid de Ajuste ISR. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Ajuste_ISR_RowDataBound
    ///DESCRIPCIÓN: Es el evento previo antes cargar el grid con informacion de 
    ///los empleados
    ///PARAMETROS:  
    ///CREO: Juan Alberto Hernandez Negrete
    ///FECHA_CREO: 23/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Ajuste_ISR_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                if (e.Row.Cells[3].Text.Contains("Pendiente"))
                {
                    e.Row.Style.Add("background", "#F5F6CE url(../imagenes/paginas/titleBackground.png) repeat-x top");
                }
                else if (e.Row.Cells[3].Text.Contains("Proceso"))
                {
                    e.Row.Style.Add("background", "#CEF6CE url(../imagenes/paginas/titleBackground.png) repeat-x top");
                }
                else if (e.Row.Cells[3].Text.Contains("Pagado"))
                {
                    e.Row.Style.Add("background", "#F78181 url(../imagenes/paginas/titleBackground.png) repeat-x top");
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Ajuste_ISR_PageIndexChanging
    /// DESCRIPCION : Cambiar de pagina ala tabla de Ajustes de ISR
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Ajuste_ISR_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Consulta_Ajustes_ISR(e.NewPageIndex);//Consultamos los Ajustes de ISR
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Error al cambiar la pagina del grid de Ajustes de ISR. Error: [" + Ex.Message.ToString() + "]";
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: LLenar_Grid_Ajuste_ISR
    /// DESCRIPCION : Carga el grid con la lista de Ajustes de ISR. y cambia a la pagina indicada.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void LLenar_Grid_Ajuste_ISR(DataTable Dt_Ajustes_ISR, Int32 Pagina)
    {
        Grid_Ajuste_ISR.PageIndex = Pagina;
        Grid_Ajuste_ISR.DataSource = Dt_Ajustes_ISR;
        Grid_Ajuste_ISR.DataBind();
        Grid_Ajuste_ISR.SelectedIndex = -1;
    }
    #endregion

    #region (Metodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///DESCRIPCIÓN: Configuracion Inicial de los controles del Formulario.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Limpiar_Controles();//Limpia los controles de la pagina.
        Consultar_Calendario_Nominas();
        Consulta_Percepciones();
        Habilitar_Controles("Inicial");//Habilita la configuracion inicial de los controles
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            //Controles Datos Solicitud Prestamo
            Txt_Ajuste_ISR.Text = "";
            Txt_Fecha_Alta_Ajuste_ISR.Text = "";
            //Cmb_Estatus_Solicitud_Prestamo.SelectedIndex = -1;
            Cmb_Calendario_Nomina.SelectedIndex = -1;
            Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;
            Txt_Fecha_Inicio_Ajuste_ISR.Text = "";
            Txt_Fecha_Termino_Ajuste_ISR.Text = "";
            Txt_Comentarios_Ajuste_ISR.Text = "";
            Txt_Total_ISR_Ajustar.Text = "";
            Txt_No_Catorcenas.Text = "";
            Txt_Pago_Catorcenal_ISR.Text = "";
            Txt_No_Pago.Text = "";
            Txt_Total_ISR_Ajustado.Text = "";
            //Cmb_Percepcion.SelectedIndex = -1;
            //Controles Datos Empleado Solicita Credito
            Limpiar_Controles_Empleado();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al limpiar los controles de la pagina. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles_Empleado
    /// DESCRIPCION : Limpia los Controles del Empleado.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles_Empleado()
    {
        try
        {
            //Controles Datos Empleado Solicita Credito
            Txt_No_Empleado.Text = "";
            Txt_Nombre_Empleado.Text = "";
            Img_Foto_Empleado_Solicitante.ImageUrl = "~/paginas/imagenes/paginas/Sias_No_Disponible.JPG";
            Txt_RFC_Empleado.Text = "";
            Txt_Fecha_Ingreso_Empleado.Text = "";
            Txt_Sindicato_Empleado.Text = "";
            Txt_Dependencia_Empelado.Text = "";
            Txt_Direccion_Empleado.Text = "";
            Txt_Cuenta_Bancaria_Empleado.Text = "";
            Txt_Sueldo_Mensual_Empleado.Text = "";
            Txt_Clase_Nomina_Empleado.Text = String.Empty;
            Txt_Fecha_Alta_Ajuste_ISR.Text = String.Empty;
            Cmb_Estatus_Ajuste_ISR.SelectedIndex = 1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al limpiar los controles de la pagina. Error: [" + Ex + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles_Busqueda_Ajuste_ISR
    /// DESCRIPCION : Limpia los Controles de la busqueda de asjustes ISR.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles_Busqueda_Ajuste_ISR()
    {
        try
        {
            //Controles Datos la busqueda.
            Txt_Busqueda_No_Ajuste_ISR.Text = "";
            Txt_Busqueda_Empleado.Text = "";
            //Limpiar Grid_Ajustes_ISR
            Grid_Ajuste_ISR.DataSource = new DataTable();
            Grid_Ajuste_ISR.DataBind();
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
    /// FECHA_CREO  : 23/Diciembre/2010
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
                    //Habilitar el Boton de Busqueda de Ajustes_ISR
                    Btn_Busqueda_Avanzada.Enabled = !Habilitado;

                    Configuracion_Acceso("Frm_Ope_Nom_Ajuste_ISR.aspx");
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
                    Btn_Busqueda_Avanzada.Enabled = !Habilitado;
                    //Obtenemos la fecha del dia de hoy.
                    Txt_Fecha_Alta_Ajuste_ISR.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                    Cmb_Estatus_Ajuste_ISR.SelectedIndex = 1;
                    Txt_No_Empleado.Focus();
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
                    Btn_Busqueda_Avanzada.Enabled = !Habilitado;
                    break;
            }
            //Controles Datos Ajuste ISR
            Txt_Ajuste_ISR.Enabled = false;
            Txt_Fecha_Alta_Ajuste_ISR.Enabled = Habilitado;
            Cmb_Estatus_Ajuste_ISR.Enabled = false;
            Cmb_Calendario_Nomina.Enabled = Habilitado;
            Cmb_Periodos_Catorcenales_Nomina.Enabled = Habilitado;
            Txt_Fecha_Inicio_Ajuste_ISR.Enabled = false;
            Txt_Fecha_Termino_Ajuste_ISR.Enabled = false;
            Txt_Comentarios_Ajuste_ISR.Enabled = Habilitado;
            Txt_Total_ISR_Ajustar.Enabled = Habilitado;
            Txt_No_Catorcenas.Enabled = Habilitado;
            Txt_Pago_Catorcenal_ISR.Enabled = false;
            Txt_No_Pago.Enabled = false;
            Txt_Total_ISR_Ajustado.Enabled = false;
            Cmb_Percepcion.Enabled = false;

            //Controles Datos Empleado Solicita Credito
            Txt_No_Empleado.Enabled = Habilitado;
            Btn_Buscar_Empleado.Enabled = Habilitado;
            Txt_Nombre_Empleado.Enabled = false;
            Img_Foto_Empleado_Solicitante.Enabled = false;
            Txt_RFC_Empleado.Enabled = false;
            Txt_Fecha_Ingreso_Empleado.Enabled = false;
            Txt_Sindicato_Empleado.Enabled = false;
            Txt_Dependencia_Empelado.Enabled = false;
            Txt_Direccion_Empleado.Enabled = false;
            Txt_Cuenta_Bancaria_Empleado.Enabled = false;
            Txt_Sueldo_Mensual_Empleado.Enabled = false;

            Cmb_Estatus_Ajuste_ISR.Enabled = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Ajuste_ISR
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Dicicembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Ajuste_ISR()
    {
        Boolean Datos_Validos = true;//Variable que alamacenara el resultado de la validacion de los datos ingresados por el usuario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
        String No_Empleado = String.Empty;
        Cls_Ope_Nom_Ajuste_ISR_Negocio Obj_Ajuste_ISR = new Cls_Ope_Nom_Ajuste_ISR_Negocio();
        DataTable DT_Ajustes_ISR = null;

        if (string.IsNullOrEmpty(Txt_No_Empleado.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + No Empleado <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Fecha_Alta_Ajuste_ISR.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha de Ajuste ISR <br>";
            Datos_Validos = false;
        }
        else if (!Validar_Formato_Fecha(Txt_Fecha_Alta_Ajuste_ISR.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Formato de la Fecha de Ajuste Invalido  <br>";
            Datos_Validos = false;
        }

        if (Cmb_Estatus_Ajuste_ISR.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione el Estatus <br>";
            Datos_Validos = false;
        }

        if (Cmb_Calendario_Nomina.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione a que nomina  <br>";
            Datos_Validos = false;
        }

        if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione el Periodo catorcenal a partir del cual se comenzara el ajuste del ISR. <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Fecha_Inicio_Ajuste_ISR.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha de Inicio del ajuste de ISR <br>";
            Datos_Validos = false;
        }
        else if (!Validar_Formato_Fecha(Txt_Fecha_Inicio_Ajuste_ISR.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Formato de la Fecha de Inicio de ajuste de ISR Invalido  <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Fecha_Termino_Ajuste_ISR.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha de ajuste de ISR <br>";
            Datos_Validos = false;
        }
        else if (!Validar_Formato_Fecha(Txt_Fecha_Termino_Ajuste_ISR.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Formato de la Fecha de Ajuste de ISR Invalido  <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Comentarios_Ajuste_ISR.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Ingrese los comenatrios del Ajuste de ISR. <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Total_ISR_Ajustar.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Ingrese el Total de ISR ajustar al empleado <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_No_Catorcenas.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Falta Numero de catorcena en la que se cubrira el ajuste de ISR. <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Pago_Catorcenal_ISR.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Falta Cantidad catorcenal que el empleado recibira como percepcion por ajuste de ISR. <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Total_ISR_Ajustado.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Indica el total de ISR que se le a pagado al empleado como parte del ajuste de ISR. <br>";
            Datos_Validos = false;
        }


        if (Cmb_Percepcion.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione la percepcion que el empleado ajustar ISR comenzara a recibir. <br>";
            Datos_Validos = false;
        }

        Obj_Ajuste_ISR.P_No_Empleado = Txt_No_Empleado.Text.Trim();
        DT_Ajustes_ISR = Obj_Ajuste_ISR.Consulta_Ajuste_ISR().P_Dt_Ajustes_ISR;

        if (DT_Ajustes_ISR is DataTable) {
            if (DT_Ajustes_ISR.Rows.Count > 0) {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Empleado ya con cuenta con un registro de Ajuste de ISR actualmente. <br>";
                Datos_Validos = false;
            }
        }

        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Formato_Fecha
    /// DESCRIPCION : Valida el formato de las fechas.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Diciembre/2010
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
    /// FECHA_CREO  : 23/Diciembre/2010
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
    /// FECHA_CREO  : 23/Diciembre/2010
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

        foreach (DataRow Renglon in Dt_Calendario_Nominas.Rows)
        {
            Renglon_Dt_Clon = Dt_Nominas.NewRow();
            Renglon_Dt_Clon["Nomina"] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString().Split(new char[] { ' ' })[0].Split(new char[] { '/' })[2];
            Renglon_Dt_Clon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID];
            Dt_Nominas.Rows.Add(Renglon_Dt_Clon);
        }
        return Dt_Nominas;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Calculo_Pago_Catorcenal
    /// DESCRIPCION : Calcula el monto de cuanto se le estara pagando al empleado 
    ///               catorcenalmente.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Calculo_Pago_Catorcenal()
    {
        Double Pago_Catorcenal = 0.0;//Variable que almacenara la cantidad que le descontara al empleado catorcenalmente.
        Int32 No_Pagos = 0;//Variable que almacena el numero de catorcenas en las que se cubrira el pago total del prestamo.
        Double Total_ISR_Ajustar = 0.0;//variable que almacena el total de los importes, tanto de importe del prestamo como el de interes.
        try
        {
            if (!string.IsNullOrEmpty(Txt_Total_ISR_Ajustar.Text.Trim())) Total_ISR_Ajustar = Convert.ToDouble(Txt_Total_ISR_Ajustar.Text.Trim());
            if (!string.IsNullOrEmpty(Txt_No_Catorcenas.Text.Trim())) No_Pagos = Convert.ToInt32(Txt_No_Catorcenas.Text.Trim());

            if (Total_ISR_Ajustar > 0 && No_Pagos > 0)
            {
                Pago_Catorcenal = (Total_ISR_Ajustar / No_Pagos);
                Txt_Pago_Catorcenal_ISR.Text = string.Format("{0:#,###,##0.00}", Pago_Catorcenal);
                Txt_No_Pago.Text = "0";
                Txt_Total_ISR_Ajustar.Text = string.Format("{0:#,###,##0.00}", Total_ISR_Ajustar);
                Txt_Total_ISR_Ajustado.Text = "0";
            }            
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al realizar el calculo del Pago Catorcenal. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Calcular_Fecha_Termino_Ajuste_ISR
    /// DESCRIPCION : Calcula la fecha en la que se terminara de hacer la percepcion 
    /// al empelado.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Calcular_Fecha_Termino_Ajuste_ISR()
    {
        DateTime Fecha_Inicia = new DateTime();//Variable que almacenara la fecha de inicio de los pagos.
        DateTime Fecha_Final = new DateTime();//Variable que almacenara la fecha de fin de los pagos.
        Int32 No_Catorcenas = 0;//Variable que almacena el numero de pagos que el empleado debera efectuar para liquidar el prestamo.

        try
        {
            No_Catorcenas = Convert.ToInt32((Txt_No_Catorcenas.Text.Trim().Equals("")) ? "0" : Txt_No_Catorcenas.Text.Trim());
            Fecha_Inicia = Convert.ToDateTime(Txt_Fecha_Inicio_Ajuste_ISR.Text.Trim());
            Fecha_Final = Fecha_Inicia;

            for (int index = 1; index <= No_Catorcenas; index++)
            {
                Fecha_Final = Fecha_Final.AddDays(14);
            }

            //Fecha de Fin de pago del prestamo.
            Txt_Fecha_Termino_Ajuste_ISR.Text = string.Format("{0:dd/MMM/yyyy}", Fecha_Final);

        }
        catch (Exception Ex)
        {
            throw new Exception("Error al calcular la fecha de termino del ajuste de ISR. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la percepcion 
    ///a partir del periodo actual.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Validar_Periodos_Pago(DropDownList Combo)
    {
        Cls_Ope_Nom_Ajuste_ISR_Negocio Ajuste_ISR = new Cls_Ope_Nom_Ajuste_ISR_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Actual = DateTime.Now;
        DateTime Fecha_Inicio = new DateTime();
        DateTime Fecha_Fin = new DateTime();

        Ajuste_ISR.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

        foreach (ListItem Elemento in Combo.Items)
        {
            if (IsNumeric(Elemento.Text.Trim()))
            {
                Ajuste_ISR.P_No_Nomina = Convert.ToInt32(Elemento.Text.Trim());
                Dt_Detalles_Nomina = Ajuste_ISR.Consultar_Fechas_Periodo();

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
    #endregion

    #region (Metodos Operacion)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Ajuste_ISR
    /// DESCRIPCION : Ejecuta la Peticion a la clase de negocio para que ejecute la alta
    /// del ajuste de ISR.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Ajuste_ISR()
    {
        Cls_Ope_Nom_Ajuste_ISR_Negocio Alta_Ajuste_ISR = new Cls_Ope_Nom_Ajuste_ISR_Negocio();//Variable de conexion con la capa de negocio.
        Cls_Cat_Empleados_Negocios Consulta_Empleado = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocio.
        DataTable Dt_Empleados = null;//Variable que almacenara una lista de empleados.

        try
        {
            //Consultamos el Empleado_ID del Empleado solicitante por medio de su no empleado.
            Consulta_Empleado.P_No_Empleado = Txt_No_Empleado.Text.Trim();
            Dt_Empleados = Consulta_Empleado.Consulta_Datos_Empleado();

            if (Dt_Empleados != null)
            {
                if (Dt_Empleados.Rows.Count > 0)
                {
                    Alta_Ajuste_ISR.P_Empleado_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString().Trim();
                    Alta_Ajuste_ISR.P_Dependencia_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString().Trim();
                }
            }

            Alta_Ajuste_ISR.P_Percepcion_Deduccion_ID = Cmb_Percepcion.SelectedValue.Trim();
            Alta_Ajuste_ISR.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Alta_Ajuste_ISR.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());
            Alta_Ajuste_ISR.P_Estatus_Ajuste_ISR = Cmb_Estatus_Ajuste_ISR.SelectedItem.Text.Trim();
            Alta_Ajuste_ISR.P_Estatus_Ajuste_ISR = "Proceso";
            Alta_Ajuste_ISR.P_Fecha_Inicio_Pago = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicio_Ajuste_ISR.Text.Trim()));
            Alta_Ajuste_ISR.P_Fecha_Termino_Pago = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Termino_Ajuste_ISR.Text.Trim()));
            Alta_Ajuste_ISR.P_Comentarios_Ajuste = Txt_Comentarios_Ajuste_ISR.Text.Trim();
            Alta_Ajuste_ISR.P_No_Catorcenas = Convert.ToInt32(Txt_No_Catorcenas.Text.Trim());
            Alta_Ajuste_ISR.P_Total_ISR_Ajustar = Convert.ToDouble(Txt_Total_ISR_Ajustar.Text.Trim());            
            Alta_Ajuste_ISR.P_Total_ISR_Ajustado = Convert.ToDouble(Txt_Total_ISR_Ajustado.Text.Trim());
            Alta_Ajuste_ISR.P_Pago_Catorcenal_ISR = Convert.ToDouble(Txt_Pago_Catorcenal_ISR.Text.Trim());
            Alta_Ajuste_ISR.P_No_Pago = Convert.ToInt32(string.IsNullOrEmpty(Txt_No_Pago.Text.Trim()) ? "0" : Txt_No_Pago.Text.Trim());
            Alta_Ajuste_ISR.P_Usuario_Creo = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);

            if (Alta_Ajuste_ISR.Alta_Ajuste_ISR())
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Ventana Estatus de la Operacion", "alert('Operación Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al ejecutar el alta de Ajuste de ISR. Error:[" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Ajuste_ISR
    /// DESCRIPCION : Ejecuta la Peticion a la clase de negocio para que ejecute la Actualizacion
    /// de los datos del ajuste de ISR seleccionado.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Ajuste_ISR()
    {
        Cls_Ope_Nom_Ajuste_ISR_Negocio Modificar_Ajuste_ISR = new Cls_Ope_Nom_Ajuste_ISR_Negocio();//Variable de conexion con la capa de negocio.
        Cls_Cat_Empleados_Negocios Consulta_Empleado = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocio.
        DataTable Dt_Empleados = null;//Variable que almacenara una lista de empleados.

        try
        {
            //Consultamos el Empleado_ID del Empleado solicitante por medio de su no empleado.
            Consulta_Empleado.P_No_Empleado = Txt_No_Empleado.Text.Trim();
            Dt_Empleados = Consulta_Empleado.Consulta_Datos_Empleado();

            if (Dt_Empleados != null)
            {
                if (Dt_Empleados.Rows.Count > 0)
                {
                    Modificar_Ajuste_ISR.P_Empleado_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString().Trim();
                    Modificar_Ajuste_ISR.P_Dependencia_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString().Trim();
                }
            }

            Modificar_Ajuste_ISR.P_No_Ajuste_ISR = Txt_Ajuste_ISR.Text.Trim();
            Modificar_Ajuste_ISR.P_Percepcion_Deduccion_ID = Cmb_Percepcion.SelectedValue.Trim();
            Modificar_Ajuste_ISR.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Modificar_Ajuste_ISR.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());
            Modificar_Ajuste_ISR.P_Estatus_Ajuste_ISR = Cmb_Estatus_Ajuste_ISR.SelectedItem.Text.Trim();
            Modificar_Ajuste_ISR.P_Estatus_Ajuste_ISR = "Proceso";
            Modificar_Ajuste_ISR.P_Fecha_Inicio_Pago = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicio_Ajuste_ISR.Text.Trim()));
            Modificar_Ajuste_ISR.P_Fecha_Termino_Pago = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Termino_Ajuste_ISR.Text.Trim()));
            Modificar_Ajuste_ISR.P_Comentarios_Ajuste = Txt_Comentarios_Ajuste_ISR.Text.Trim();
            Modificar_Ajuste_ISR.P_No_Catorcenas = Convert.ToInt32(Txt_No_Catorcenas.Text.Trim());
            Modificar_Ajuste_ISR.P_Total_ISR_Ajustar = Convert.ToDouble(Txt_Total_ISR_Ajustar.Text.Trim());
            Modificar_Ajuste_ISR.P_Total_ISR_Ajustado = Convert.ToDouble(Txt_Total_ISR_Ajustado.Text.Trim());
            Modificar_Ajuste_ISR.P_Pago_Catorcenal_ISR = Convert.ToDouble(Txt_Pago_Catorcenal_ISR.Text.Trim());
            Modificar_Ajuste_ISR.P_No_Pago = Convert.ToInt32(string.IsNullOrEmpty(Txt_No_Pago.Text.Trim()) ? "0" : Txt_No_Pago.Text.Trim());
            Modificar_Ajuste_ISR.P_Usuario_Modifico = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);

            if (Modificar_Ajuste_ISR.Modificar_Ajuste_ISR())
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Ventana Estatus de la Operacion", "alert('Operación Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al ejecutar una modificacion a los datos del ajuste de ISR. Error:[" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Ajuste_ISR
    /// DESCRIPCION : Ejecuta la Peticion a la clase de negocio para que ejecute la Baja
    /// del ajuste de ISR seleccinado.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Ajuste_ISR()
    {
        Cls_Ope_Nom_Ajuste_ISR_Negocio Eliminar_Solicitud_Prestamo = new Cls_Ope_Nom_Ajuste_ISR_Negocio();//Variable de conexion con la capa de negocio.

        try
        {
            Eliminar_Solicitud_Prestamo.P_No_Ajuste_ISR = Txt_Ajuste_ISR.Text.Trim();

            if (Eliminar_Solicitud_Prestamo.Baja_Ajuste_ISR())
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Ventana Estatus de la Operacion", "alert('Operación Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al ejecutar una Baja de un Ajuste de ISR. Error:[" + Ex.Message + "]");
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
    private void Consultar_Calendario_Nominas()
    {        
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nominales = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Calendarios_Nominales = null;//Variable que almacena los calendarios nominales que existén actualmente en el sistema.
        try
        {
            Dt_Calendarios_Nominales = Obj_Calendario_Nominales.Consultar_Calendario_Nominas();
            Dt_Calendarios_Nominales = Formato_Fecha_Calendario_Nomina(Dt_Calendarios_Nominales);

            if (Dt_Calendarios_Nominales is DataTable)
            {
                Cmb_Calendario_Nomina.DataSource = null;
                Cmb_Calendario_Nomina.DataBind();
                Cmb_Calendario_Nomina.DataSource = Dt_Calendarios_Nominales;
                Cmb_Calendario_Nomina.DataTextField = "Nomina";
                Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                Cmb_Calendario_Nomina.DataBind();
                Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));

                Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf
                    (Cmb_Calendario_Nomina.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Anyo));

                if (Cmb_Calendario_Nomina.SelectedIndex > 0)
                {
                    Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
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
    private void Consultar_Periodos_Catorcenales_Nomina(String Nomina_ID)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nomina_Periodos = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Clase de conexion con la capa de negocios
        DataTable Dt_Periodos_Catorcenales = null;//Variable que almacenra unaa lista de los periodos catorcenales que le correspónden a la nomina seleccionada.

        try
        {
            Consulta_Calendario_Nomina_Periodos.P_Nomina_ID = Nomina_ID;
            Dt_Periodos_Catorcenales = Consulta_Calendario_Nomina_Periodos.Consulta_Detalles_Nomina();
            if (Dt_Periodos_Catorcenales != null)
            {
                if (Dt_Periodos_Catorcenales.Rows.Count > 0)
                {
                    Cmb_Periodos_Catorcenales_Nomina.DataSource = Dt_Periodos_Catorcenales;
                    Cmb_Periodos_Catorcenales_Nomina.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataValueField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataBind();
                    Cmb_Periodos_Catorcenales_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;

                    Validar_Periodos_Pago(Cmb_Periodos_Catorcenales_Nomina);

                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Periodos_Catorcenales_Nomina.Items.IndexOf(Cmb_Periodos_Catorcenales_Nomina.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Periodo));
                    Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged(Cmb_Periodos_Catorcenales_Nomina, null);
                }
                else
                {
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
    ///NOMBRE DE LA FUNCIÓN: Consulta_Percepciones
    ///DESCRIPCIÓN: Consulta las Percepciones vigentes en el sistema.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consulta_Percepciones()
    {        
        Cls_Ope_Nom_Ajuste_ISR_Negocio Percepcion_Ajuste_ISR =new Cls_Ope_Nom_Ajuste_ISR_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Percepcion_Ajuste_ISR = null;//Variable que almacenará el registro del parámetro de la nómina.
        Cls_Cat_Nom_Parametros_Negocio INF_PARAMETROS = null;//Variable que almacenara los parámetros de la nómina.

        try
        {
            INF_PARAMETROS = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();

            Dt_Percepcion_Ajuste_ISR = Percepcion_Ajuste_ISR.Consultar_Percepcion_Ajuste_ISR();

            if (Dt_Percepcion_Ajuste_ISR != null)
            {
                if (Dt_Percepcion_Ajuste_ISR.Rows.Count > 0)
                {
                    Cmb_Percepcion.DataSource = Dt_Percepcion_Ajuste_ISR;
                    Cmb_Percepcion.DataTextField = Cat_Nom_Percepcion_Deduccion.Campo_Nombre;
                    Cmb_Percepcion.DataValueField = Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;
                    Cmb_Percepcion.DataBind();
                    Cmb_Percepcion.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Percepcion.SelectedIndex = -1;

                    if (!String.IsNullOrEmpty(INF_PARAMETROS.P_Percepcion_Ajuste_ISR))
                        Cmb_Percepcion.SelectedIndex = Cmb_Percepcion.Items.IndexOf(
                            Cmb_Percepcion.Items.FindByValue(INF_PARAMETROS.P_Percepcion_Ajuste_ISR));
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron Deducciones vigentes en el sistema.";
                }
            }

        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las percepciones en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Mostrar_Informacion_Empleado
    ///DESCRIPCIÓN: Consulta los datos del Empleado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Mostrar_Informacion_Empleado()
    {
        Cls_Cat_Empleados_Negocios Consulta_Empelado = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        Cls_Cat_Nom_Sindicatos_Negocio Consulta_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Dependencias_Negocio Consulta_Dependencias = new Cls_Cat_Dependencias_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Tipos_Nominas_Negocio Consulta_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexion con la clase de negocios.
        Cls_Ope_Nom_Ajuste_ISR_Negocio Consulta_Ajuste_ISR = new Cls_Ope_Nom_Ajuste_ISR_Negocio();//Variable de conexion con la capa de negocios.
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
            No_Empleado = Txt_No_Empleado.Text.Trim();
            //Limpiamos los controles del empleado solicitante del prestamo.
            Limpiar_Controles_Empleado();
            Txt_No_Empleado.Text = No_Empleado;

            if (!string.IsNullOrEmpty(Txt_No_Empleado.Text.Trim()))
            {
                Txt_No_Empleado.Text = String.Format("{0:000000}", Convert.ToInt64(Txt_No_Empleado.Text.Trim()));
                Consulta_Empelado.P_No_Empleado = Txt_No_Empleado.Text.Trim();
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

                        Sueldo_Mensual = string.Format("{0:#,###,###.00}", (Convert.ToDouble((Dt_Empleados.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString().Trim().Equals("")) ? "0" : Dt_Empleados.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString().Trim()) * 30.4));

                        if (!string.IsNullOrEmpty(Nombre))
                        {
                            Txt_Nombre_Empleado.Text = Nombre;
                            Txt_Nombre_Empleado.ToolTip = Nombre;
                        }
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_RFC].ToString())) Txt_RFC_Empleado.Text = Dt_Empleados.Rows[0][Cat_Empleados.Campo_RFC].ToString();
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Fecha_Inicio].ToString())) Txt_Fecha_Ingreso_Empleado.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Fecha_Inicio].ToString()));
                        if (!string.IsNullOrEmpty(Sindicato)) Txt_Sindicato_Empleado.Text = Sindicato;
                        if (!string.IsNullOrEmpty(Dependencia)) Txt_Dependencia_Empelado.Text = Dependencia;
                        if (!string.IsNullOrEmpty(Direccion)) Txt_Direccion_Empleado.Text = Direccion;
                        if (!string.IsNullOrEmpty(Nomina)) Txt_Clase_Nomina_Empleado.Text = Nomina;
                        if (!string.IsNullOrEmpty(Sueldo_Mensual)) Txt_Sueldo_Mensual_Empleado.Text = Sueldo_Mensual;
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString())) Txt_Cuenta_Bancaria_Empleado.Text = Dt_Empleados.Rows[0][Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString();

                        Img_Foto_Empleado_Solicitante.ImageUrl = (string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Ruta_Foto].ToString())) ? "~/paginas/imagenes/paginas/Sias_No_Disponible.JPG" : @Dt_Empleados.Rows[0][Cat_Empleados.Campo_Ruta_Foto].ToString();
                        Img_Foto_Empleado_Solicitante.DataBind();
                        Txt_Fecha_Alta_Ajuste_ISR.Focus();
                        Consultar_Calendario_Nominas();
                        Txt_Fecha_Alta_Ajuste_ISR.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                    }
                    else {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "El empleado no existe.";
                        Txt_No_Empleado.Focus();
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
    ///NOMBRE DE LA FUNCIÓN: Consulta_Ajustes_ISR
    ///DESCRIPCIÓN: Consulta los Ajustes de ISR que se encuantran vigentes en el sistema.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consulta_Ajustes_ISR(Int32 Pagina)
    {
        Cls_Ope_Nom_Ajuste_ISR_Negocio Consulta_Ajuste_ISR = new Cls_Ope_Nom_Ajuste_ISR_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Ajustes_ISR = null;//Variable que almacenara una lista de ajustes nde ISR
        try
        {
            if (!string.IsNullOrEmpty(Txt_Busqueda_No_Ajuste_ISR.Text.Trim()))
            {
                Txt_Busqueda_No_Ajuste_ISR.Text = string.Format("{0:0000000000}", Convert.ToInt32(Txt_Busqueda_No_Ajuste_ISR.Text.Trim().Equals("") ? "0" : Txt_Busqueda_No_Ajuste_ISR.Text.Trim()));
            }
            //Filtros de Busqueda
            Consulta_Ajuste_ISR.P_No_Ajuste_ISR= Txt_Busqueda_No_Ajuste_ISR.Text.Trim();
            Consulta_Ajuste_ISR.P_No_Empleado = Txt_Busqueda_Empleado.Text.Trim();
            Consulta_Ajuste_ISR.P_RFC_Empleado = Txt_Busqueda_RFC.Text.Trim();

            if (Cmb_Busqueda_Estatus.SelectedIndex > 0) Consulta_Ajuste_ISR.P_Estatus_Ajuste_ISR = Cmb_Busqueda_Estatus.SelectedItem.Text.Trim();

            if (Validar_Formato_Fecha(Txt_Busqueda_Fecha_Inicio.Text.Trim()))
            {
                Consulta_Ajuste_ISR.P_Fecha_Inicio_Pago = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Busqueda_Fecha_Inicio.Text.Trim()));
            }
            else { Txt_Busqueda_Fecha_Inicio.Text = ""; }
            if (Validar_Formato_Fecha(Txt_Busqueda_Fecha_Fin.Text.Trim()))
            {
                Consulta_Ajuste_ISR.P_Fecha_Termino_Pago = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Busqueda_Fecha_Fin.Text.Trim()));
            }
            else { Txt_Busqueda_Fecha_Fin.Text = ""; }

            Dt_Ajustes_ISR = Consulta_Ajuste_ISR.Consulta_Ajuste_ISR().P_Dt_Ajustes_ISR;
            if (Dt_Ajustes_ISR != null)
            {
                if (Dt_Ajustes_ISR.Rows.Count > 0)
                {
                    LLenar_Grid_Ajuste_ISR(Dt_Ajustes_ISR, Pagina);
                }
                else
                {
                    LLenar_Grid_Ajuste_ISR(new DataTable(), Pagina);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los Ajustes_ISR. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Empleado
    ///DESCRIPCIÓN: Consulta los datos del Empleado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Datos_Empleado(String Empleado_ID)
    {
        Cls_Cat_Empleados_Negocios Consulta_Empelado = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        Cls_Cat_Nom_Sindicatos_Negocio Consulta_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Dependencias_Negocio Consulta_Dependencias = new Cls_Cat_Dependencias_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Tipos_Nominas_Negocio Consulta_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexion con la clase de negocios.
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
            Limpiar_Controles_Empleado();

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

                        Sueldo_Mensual = string.Format("{0:#,###,###.00}", (Convert.ToDouble((Dt_Empleados.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString().Trim().Equals("")) ? "0" : Dt_Empleados.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString().Trim()) * 30.4));

                        if (!string.IsNullOrEmpty(Nombre))
                        {
                            Txt_Nombre_Empleado.Text = Nombre;
                            Txt_Nombre_Empleado.ToolTip = Nombre;
                        }
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_RFC].ToString())) Txt_RFC_Empleado.Text = Dt_Empleados.Rows[0][Cat_Empleados.Campo_RFC].ToString();
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Fecha_Inicio].ToString())) Txt_Fecha_Ingreso_Empleado.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Fecha_Inicio].ToString()));
                        if (!string.IsNullOrEmpty(Sindicato)) Txt_Sindicato_Empleado.Text = Sindicato;
                        if (!string.IsNullOrEmpty(Dependencia)) Txt_Dependencia_Empelado.Text = Dependencia;
                        if (!string.IsNullOrEmpty(Direccion)) Txt_Direccion_Empleado.Text = Direccion;
                        if (!string.IsNullOrEmpty(Nomina)) Txt_Clase_Nomina_Empleado.Text = Nomina;
                        if (!string.IsNullOrEmpty(Sueldo_Mensual)) Txt_Sueldo_Mensual_Empleado.Text = Sueldo_Mensual;
                        if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString())) Txt_Cuenta_Bancaria_Empleado.Text = Dt_Empleados.Rows[0][Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString();

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
            Botones.Add(Btn_Busqueda_Avanzada);

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
    ///DESCRIPCIÓN: Alta de un Ajuste de ISR
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Diciembre/2010 
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
                Consultar_Calendario_Nominas();
                Habilitar_Controles("Nuevo");
            }
            else
            {
                if (Validar_Datos_Ajuste_ISR())
                {
                    Alta_Ajuste_ISR();
                    Configuracion_Inicial();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
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
    ///DESCRIPCIÓN: Modificar el Ajuste de ISR
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Diciembre/2010
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
                if (!Txt_Ajuste_ISR.Text.Equals(""))
                {
                    if (!Cmb_Estatus_Ajuste_ISR.SelectedItem.Text.Trim().ToUpper().Equals("PENDIENTE"))
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "El registro ya fea aceptado, ya no es posible realizar ninguna modificacion <br>";
                    }
                    else
                    {
                        Habilitar_Controles("Modificar");//Habilita la configuracion de los controles para ejecutar la operacion de modificar.
                    }
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
                if (Validar_Datos_Ajuste_ISR())
                {
                    Modificar_Ajuste_ISR();
                    Configuracion_Inicial();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
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
    ///DESCRIPCIÓN: Eliminar el Ajuste de ISR seleccionado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Diciembre/2010 
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
                if (!Txt_Ajuste_ISR.Text.Equals(""))
                {
                    Eliminar_Ajuste_ISR();
                    Limpiar_Controles();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea eliminar <br>";
                }
            }
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
    ///FECHA_CREO: 23/Diciembre/2010 
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
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Empleado
    ///DESCRIPCIÓN: Consulta al Empleado en el sistema por su numero de empleado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Empleado_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Consultar_Mostrar_Informacion_Empleado();
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Ajustes_ISR_Click
    ///DESCRIPCIÓN: Consulta los Ajustes de ISR vigentes actualmente en el 
    ///sistema.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Ajustes_ISR_Click(object sender, EventArgs e)
    {
        try
        {
            Consulta_Ajustes_ISR(0);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN: Abre el Modal_Popup para realizar la busqueda.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Avanzada_Click(object sender, EventArgs e)
    {
        Mpe_Busqueda_Ajuste_ISR.Show();
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cerrar_Ventana_Busqueda_Click
    ///DESCRIPCIÓN: Cierra el Modal_Popup de la busqueda.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Cerrar_Ventana_Busqueda_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Busqueda_Ajuste_ISR.Hide();
    }
    #endregion

    #region (Eventos Combos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Diciembre/2010
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
        else
        {
            Cmb_Periodos_Catorcenales_Nomina.DataSource = new DataTable();
            Cmb_Periodos_Catorcenales_Nomina.DataBind();
        }
        Cmb_Calendario_Nomina.Focus();
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona la fecha de Inicio del periodo que se le comenzara a 
    ///descontar al empleado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Nom_Ajuste_ISR_Negocio Consulta_Ajustes_ISR = new Cls_Ope_Nom_Ajuste_ISR_Negocio();//Variable de Conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenara los detalles del periodo seleccionado.
        try
        {
            Consulta_Ajustes_ISR.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());
            Consulta_Ajustes_ISR.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Dt_Detalles_Nomina = Consulta_Ajustes_ISR.Consultar_Fecha_Inicio_Periodo_Pago();

            if (Dt_Detalles_Nomina != null)
            {
                if (Dt_Detalles_Nomina.Rows.Count > 0)
                {
                    Txt_Fecha_Inicio_Ajuste_ISR.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString()));

                    if (!string.IsNullOrEmpty(Txt_No_Catorcenas.Text.Trim()))
                    {
                        Calcular_Fecha_Termino_Ajuste_ISR();
                    }
                }
            }
            Cmb_Periodos_Catorcenales_Nomina.Focus();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar la Fecha de Inicio en la que al empelado se le comenzara a descontar. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Eventos TextBox)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Total_ISR_Ajustar_TextChanged
    ///DESCRIPCIÓN: Calcula el monto del pago catorcenal que estara recibiendo el 
    ///             empelado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Total_ISR_Ajustar_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Calculo_Pago_Catorcenal();
            Txt_No_Catorcenas.Focus();
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_No_Catorcenas_TextChanged
    ///DESCRIPCIÓN: Ejecuta el calculo para definir la fecha de termino de pago del 
    ///             ajuste de ISR en base a la fecha de inicio y el numero de catorcenas
    ///             definidas para cubrir el pago.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_No_Catorcenas_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex > 0)
            {
                Calcular_Fecha_Termino_Ajuste_ISR();
            }
            Calculo_Pago_Catorcenal();            
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_No_Empleado_TextChanged
    ///DESCRIPCIÓN: Consulta al Empleado en el sistema por su numero de empleado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 02/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_No_Empleado_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Consultar_Mostrar_Informacion_Empleado();//Consulta al empleado por su numero de empleado.            
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

}
