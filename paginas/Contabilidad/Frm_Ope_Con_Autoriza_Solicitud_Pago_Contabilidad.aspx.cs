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
using System.Collections.Generic;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Autoriza_Solicitud_Pago_Contabilidad.Negocio;
using Presidencia.Manejo_Presupuesto.Datos;
using Presidencia.Solicitud_Pagos.Negocio;

public partial class paginas_Contabilidad_Frm_Ope_Con_Autoriza_Solicitud_Pago_Contabilidad : System.Web.UI.Page
{
#region "Page_Load"
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Page_Load
    /// DESCRIPCION : Carga la configuración inicial de los controles de la página.
    /// CREO        : Sergio Manuel Gallardo Andrade
    /// FECHA_CREO  : 15/Noviembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Refresca la session del usuario lagueado al sistema.
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            //Valida que exista algun usuario logueado al sistema.
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                ViewState["SortDirection"] = "ASC";
                //Cmb_Anio_Contable.SelectedValue = nuevo;
                //Llenar_Grid_Cierres_generales(nuevo);
                Acciones();
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
#endregion
    #region "Metodos"
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Sergio Manuel Gallardo Andrade 
    /// FECHA_CREO  : 15/Noviembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Limpia_Controles();             //Limpia los controles del forma
            Llenar_Grid_Solicitudes_Pago();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///                para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                           si es una alta, modificacion
    ///                           
    /// CREO        : Sergio Manuel Gallardo Andrade
    /// FECHA_CREO  : 15/Noviembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado; ///Indica si el control de la forma va hacer habilitado para utilización del usuario
        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = true;
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Configuracion_Acceso("Frm_Ope_Con_Autoriza_Solicitud_Pago_Contabilidad.aspx");
                    break;
            }
            //Cmb_Anio_Contable.Enabled = Habilitado;
            //Cmb_Mes_Contable.Enabled = Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Sergio Manuel Gallardo Andrade 
    /// FECHA_CREO  : 15/Noviembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {

            //Cmb_Mes_Contable.SelectedIndex = -1;
            //Cmb_Anio_Contable.SelectedIndex = -1;

        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llenar_Grid_Cierres_generales
    /// DESCRIPCION : Llena el grid Solicitudes de pago
    /// PARAMETROS  : 
    /// CREO        : Sergio Manuel Gallardo Andrade
    /// FECHA_CREO  : 15/noviembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llenar_Grid_Solicitudes_Pago()
    {
        try
        {
            Cls_Ope_Con_Autoriza_Solicitud_Pago_Contabilidad_Negocio Rs_Autoriza_Solicitud = new Cls_Ope_Con_Autoriza_Solicitud_Pago_Contabilidad_Negocio();
            DataTable Dt_Resultado = new DataTable();
            Dt_Resultado = Rs_Autoriza_Solicitud.Consulta_Solicitudes_SinAutorizar();
            if (Dt_Resultado.Rows.Count > 0)
            {
                Grid_Solicitud_Pagos.Columns[2].Visible = true;
                Grid_Solicitud_Pagos.DataSource = Dt_Resultado;   // Se iguala el DataTable con el Grid
                Grid_Solicitud_Pagos.DataBind();    // Se ligan los datos.;
                Grid_Solicitud_Pagos.Columns[2].Visible = false;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Solicitudes de Pago", "alert('En este momento no se tienen pagos pendientes por autorizar');", true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Meses estatus " + ex.Message.ToString(), ex);
        }
    }
    // ****************************************************************************************
    //'NOMBRE DE LA FUNCION:Accion
    //'DESCRIPCION : realiza la modificacion del estatus  
    //'PARAMETROS  : 
    //'CREO        : Sergio Manuel Gallardo
    //'FECHA_CREO  : 07/Noviembre/2011 12:12 pm
    //'MODIFICO          :
    //'FECHA_MODIFICO    :
    //'CAUSA_MODIFICACION:
    //'****************************************************************************************
    protected void Acciones()
    {
        int Actualiza_Presupuesto;
        int Registra_Movimiento;
        String Reserva;
        String Monto;
        DataTable Dt_solicitud;
        String Accion = String.Empty;
        String No_Solicitud = String.Empty;
         String Comentario = String.Empty;
         Cls_Ope_Con_Autoriza_Solicitud_Pago_Contabilidad_Negocio Rs_Solicitud_Pago = new Cls_Ope_Con_Autoriza_Solicitud_Pago_Contabilidad_Negocio();    //Objeto de acceso a los metodos.
        if (Request.QueryString["Accion"] != null)
        {
            Accion = HttpUtility.UrlDecode(Request.QueryString["Accion"].ToString());
            if (Request.QueryString["id"] != null)
            {
                No_Solicitud = HttpUtility.UrlDecode(Request.QueryString["id"].ToString());
            }
             if (Request.QueryString["x"] != null)
            {
                Comentario = HttpUtility.UrlDecode(Request.QueryString["x"].ToString());
            }
            //Response.Clear()
            switch (Accion)
            {
                case "Autorizar_Solicitud":
                    Rs_Solicitud_Pago.P_No_Solicitud_Pago  = No_Solicitud;
                    Rs_Solicitud_Pago.P_Estatus  = "PORPAGAR";
                    Rs_Solicitud_Pago.P_Comentario = Comentario;
                    Rs_Solicitud_Pago.P_Empleado_ID_Contabilidad = Cls_Sessiones.No_Empleado.ToString();
                    Rs_Solicitud_Pago.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado.ToString();
                    Rs_Solicitud_Pago.Cambiar_Estatus_Solicitud_Pago();
                    Rs_Solicitud_Pago.P_No_Solicitud_Pago = No_Solicitud;
                    Dt_solicitud=Rs_Solicitud_Pago.Consultar_Solicitud_Pago();
                    if (Dt_solicitud.Rows.Count > 0)
                    {
                        Reserva = Dt_solicitud.Rows[0]["NO_RESERVA"].ToString().Trim();
                        Monto = Dt_solicitud.Rows[0]["MONTO"].ToString().Trim();
                        Actualiza_Presupuesto = Cls_Ope_Psp_Manejo_Presupuesto.Actualizar_Momentos_Presupuestales(Reserva, "EJERCIDO", "DEVENGADO", Convert.ToDouble(Monto));
                        Registra_Movimiento = Cls_Ope_Psp_Manejo_Presupuesto.Registro_Movimiento_Presupuestal(Reserva, "EJERCIDO", "DEVENGADO", Convert.ToDouble(Monto), "", "", "", "");
                    }                    
                    break;
                case "Rechazar_Solicitud":
                    Cancela_Solicitud_Pago(No_Solicitud, Comentario, Cls_Sessiones.Empleado_ID.ToString(), Cls_Sessiones.Nombre_Empleado.ToString());
                    break;

            }
        }
    }
    private void Cancela_Solicitud_Pago(String No_Solicitud_Pago, String Comentario, String Empleado_ID, String Nombre_Empleado)
    {
        DataTable Dt_Partidas_Polizas = new DataTable(); //Obtiene los detalles de la póliza que se debera generar para el movimiento
        DataTable Dt_Datos_Polizas = new DataTable(); //Obtiene los detalles de la póliza que se debera generar para el movimiento
        Cls_Ope_Con_Solicitud_Pagos_Negocio Rs_Modificar_Ope_Con_Solicitud_Pagos = new Cls_Ope_Con_Solicitud_Pagos_Negocio(); //Variable de conexión hacia la capa de Negocios
        Rs_Modificar_Ope_Con_Solicitud_Pagos.P_No_Solicitud_Pago = No_Solicitud_Pago;
        Dt_Datos_Polizas = Rs_Modificar_Ope_Con_Solicitud_Pagos.Consulta_Datos_Solicitud_Pago();
        foreach (DataRow Registro in Dt_Datos_Polizas.Rows)
        {
            Txt_Monto_Solicitud.Value = Registro[Ope_Con_Solicitud_Pagos.Campo_Monto].ToString();
            Txt_Cuenta_Contable_ID_Proveedor.Value = Registro[Cat_Com_Proveedores.Campo_Cuenta_Contable_ID].ToString();
            Txt_Cuenta_Contable_reserva.Value = Registro["Cuenta_Contable_Reserva"].ToString();
            Txt_No_Reserva.Value = Registro["NO_RESERVA"].ToString();
        }
        try
        {
            //Agrega los campos que va a contener el DataTable de los detalles de la póliza
            Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Partida, typeof(System.Int32));
            Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID, typeof(System.String));
            Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Concepto, typeof(System.String));
            Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Debe, typeof(System.Double));
            Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Haber, typeof(System.Double));

            DataRow row = Dt_Partidas_Polizas.NewRow(); //Crea un nuevo registro a la tabla

            //Agrega el cargo del registro de la póliza
            row[Ope_Con_Polizas_Detalles.Campo_Partida] = 1;
            row[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Txt_Cuenta_Contable_reserva.Value;
            row[Ope_Con_Polizas_Detalles.Campo_Concepto] = "CANCELACION-" + No_Solicitud_Pago;
            row[Ope_Con_Polizas_Detalles.Campo_Debe] = 0;
            row[Ope_Con_Polizas_Detalles.Campo_Haber] = Convert.ToDouble(Txt_Monto_Solicitud.Value.ToString());
            Dt_Partidas_Polizas.Rows.Add(row); //Agrega el registro creado con todos sus valores a la tabla
            Dt_Partidas_Polizas.AcceptChanges();

            row = Dt_Partidas_Polizas.NewRow();
            //Agrega el abono del registro de la póliza
            row[Ope_Con_Polizas_Detalles.Campo_Partida] = 2;
            row[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Txt_Cuenta_Contable_ID_Proveedor.Value;
            row[Ope_Con_Polizas_Detalles.Campo_Concepto] = "CANCELACION-" + No_Solicitud_Pago;
            row[Ope_Con_Polizas_Detalles.Campo_Debe] = Convert.ToDouble(Txt_Monto_Solicitud.Value.ToString());
            row[Ope_Con_Polizas_Detalles.Campo_Haber] = 0;
            Dt_Partidas_Polizas.Rows.Add(row); //Agrega el registro creado con todos sus valores a la tabla
            Dt_Partidas_Polizas.AcceptChanges();

            //Agrega los valores a pasar a la capa de negocios para ser dados de alta
            Rs_Modificar_Ope_Con_Solicitud_Pagos.P_Dt_Detalles_Poliza = Dt_Partidas_Polizas;
            Rs_Modificar_Ope_Con_Solicitud_Pagos.P_No_Solicitud_Pago = No_Solicitud_Pago;
            Rs_Modificar_Ope_Con_Solicitud_Pagos.P_Monto_Anterior  = Convert.ToDouble(Txt_Monto_Solicitud.Value);
            Rs_Modificar_Ope_Con_Solicitud_Pagos.P_Estatus = "CANCELADO";
            Rs_Modificar_Ope_Con_Solicitud_Pagos.P_Concepto = "CANCELACION-" + No_Solicitud_Pago;
            Rs_Modificar_Ope_Con_Solicitud_Pagos.P_Fecha_Autorizo_Rechazo_Jefe = String.Format("{0:dd/MM/yyyy}", DateTime.Today);
            Rs_Modificar_Ope_Con_Solicitud_Pagos.P_Empleado_ID_Contabilidad = Empleado_ID;
            Rs_Modificar_Ope_Con_Solicitud_Pagos.P_Comentarios_Contabilidad = Comentario;
            Rs_Modificar_Ope_Con_Solicitud_Pagos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Modificar_Ope_Con_Solicitud_Pagos.P_No_Reserva_Anterior = Convert.ToDouble(Txt_No_Reserva.Value);
            Rs_Modificar_Ope_Con_Solicitud_Pagos.Modificar_Solicitud_Pago(); //Modifica el registro que fue seleccionado por el usuario con los nuevos datos proporcionados
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Solicitud de Pagos", "alert('La Modificación de la Solicitud de Pago fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Solicitud_Pago " + ex.Message.ToString(), ex);
        }
    }
    #endregion
    #region (Control Acceso Pagina)
    ///*******************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
    /// PARÁMETROS  :
    /// USUARIO CREÓ: Salvador L. Rea Ayala
    /// FECHA CREÓ  : 30/Septiembre/2011
    /// USUARIO MODIFICO  :
    /// FECHA MODIFICO    :
    /// CAUSA MODIFICACIÓN:
    ///*******************************************************************************
    protected void Configuracion_Acceso(String URL_Pagina)
    {
        List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            //Botones.Add(Btn_Nuevo);

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
                if (!String.IsNullOrEmpty(Request.QueryString["Accion"]))
                {
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
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
    /// USUARIO CREÓ: Salvador L. Rea Ayala
    /// FECHA CREÓ  : 30/Septiembre/2011
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
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Salir")
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #region"Grid"
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Solicitud_Pagos_PageIndexChanging
    /// DESCRIPCION : Cambia la pagina de la tabla de solicitudes
    ///               
    /// PARAMETROS  : 
    /// CREO        : Sergio Manuel Gallardo Andrade
    /// FECHA_CREO  : 15-Noviembre-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Solicitud_Pagos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Solicitud_Pagos.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llenar_Grid_Solicitudes_Pago();                    //Carga los Polizas que estan asignados a la página seleccionada
            Grid_Solicitud_Pagos.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

}
