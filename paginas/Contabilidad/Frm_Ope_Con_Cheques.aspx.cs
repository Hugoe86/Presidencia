using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Cuentas_Contables.Negocio;
using AjaxControlToolkit;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using Presidencia.Parametros_Contabilidad.Negocio;
using Presidencia.SAP_Partidas_Especificas.Negocio;
using Presidencia.Catalogo_SAP_Fuente_Financiamiento.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Area_Funcional.Negocio;
//using Presidencia.Catalogo_Compras_Proyectos_Programas.Negocio;
//using Presidencia.SAP_Operacion_Departamento_Presupuesto.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Compromisos_Contabilidad.Negocios;
using Presidencia.Generar_Reservas.Negocio;
using Presidencia.Manejo_Presupuesto.Datos;
using Presidencia.Cheque.Negocio;
using Presidencia.Bancos_Nomina.Negocio;
using Presidencia.Autoriza_Solicitud_Pago_Contabilidad.Negocio;
using Presidencia.Solicitud_Pagos.Negocio;

public partial class paginas_Contabilidad_Frm_Ope_Con_Cheques : System.Web.UI.Page
{
    private static String P_Dt_Solicitud = "P_Dt_Solicitud";
    //private static String P_Dt_Programas = "P_Dt_Programas";
    //private static String P_Dt_Partidas = "P_Dt_Partidas";
    //private static String Importe = "Importe";

    #region (Page Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

        try
        {
            if (!IsPostBack)
            {
                Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                Limpia_Controles();     //Limpia los controles del forma
                ViewState["SortDirection"] = "ASC";
                //Llenar_Cmb_Unidad_Responsable_Busqueda();
                DateTime _DateTime = DateTime.Now;
                int dias = _DateTime.Day;
                dias = dias * -1;
                dias++;
                _DateTime = _DateTime.AddDays(dias);
                Txt_Fecha.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
                Txt_Fecha_Inicio.Text = _DateTime.ToString("dd/MMM/yyyy").ToUpper();
                Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
                Txt_Fecha_No_Pago.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
                Cls_Ope_Con_Cheques_Negocio Tipos_Negocio = new Cls_Ope_Con_Cheques_Negocio();
                DataTable Dt_Tipos = Tipos_Negocio.Consulta_Tipos_Solicitudes();
                //Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Unidad_Responsable, Dt_Dependencias, "CLAVE_NOMBRE", "DEPENDENCIA_ID");
                Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Tipo_Solicitud, Dt_Tipos, 1, 0);
                Llenar_Grid_Solicitudes_Pendientes();
                Cls_Cat_Nom_Bancos_Negocio Bancos_Negocio = new Cls_Cat_Nom_Bancos_Negocio();
                DataTable Dt_Bancos = Bancos_Negocio.Consulta_Bancos();
                Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Banco, Dt_Bancos, "NOMBRE", "BANCO_ID");
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
            //Botones.Add(Btn_Eliminar);
            //Botones.Add(Btn_Mostrar_Popup_Busqueda);

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

    //#region (Metodos)
    //#region(Metodos Generales)
    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Llenar_Grid_Solicitudes_Pendientes
    // DESCRIPCIÓN: Llena el grid principal de las solicitudes de pago que estan autorizadas
    // RETORNA: 
    // CREO: Sergio Manuel Gallardo Andrade
    // FECHA_CREO: 18/noviembre/2011 
    // MODIFICO:
    // FECHA_MODIFICO:
    // CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    public void Llenar_Grid_Solicitudes_Pendientes()
    {
        Cls_Ope_Con_Cheques_Negocio Solicitudes = new Cls_Ope_Con_Cheques_Negocio();     
        //Requisicion_Negocio.P_Fecha_Inicial = Txt_Fecha_Inicial.Text;
        Solicitudes.P_Fecha_Inicio = String.Format("{0:dd-MMM-yyyy}", Txt_Fecha_Inicio.Text);
        Solicitudes.P_Fecha_Final = Txt_Fecha_Final.Text;
        Solicitudes.P_Fecha_Final = String.Format("{0:dd-MMM-yyyy}", Txt_Fecha_Final.Text);
        if (Cmb_Tipo_Solicitud.SelectedIndex != 0)
        {
            Solicitudes.P_Tipo_Solicitud_Pago_ID = Cmb_Tipo_Solicitud.SelectedValue;
        }
        if (Txt_No_Solicitud_Pago.Text.Trim().Length > 0)
        {
            String No_Solicitud = Txt_No_Solicitud_Pago.Text;
            No_Solicitud = No_Solicitud.ToUpper();
            int Int_No_Solicitud = 0;
            try
            {
                Int_No_Solicitud = int.Parse(No_Solicitud);
            }
            catch (Exception Ex)
            {
                String Str = Ex.ToString();
                No_Solicitud = "0";
            }
            Solicitudes.P_No_Solicitud_Pago = No_Solicitud;
        }
        Session[P_Dt_Solicitud] = Solicitudes.Consulta_Solicitudes_Autorizadas();
        if (Session[P_Dt_Solicitud] != null && ((DataTable)Session[P_Dt_Solicitud]).Rows.Count > 0)
        {
            Grid_Solicitudes.Columns[2].Visible = true;
            Grid_Solicitudes.DataSource = Session[P_Dt_Solicitud] as DataTable;
            Grid_Solicitudes.DataBind();
            Grid_Solicitudes.Columns[2].Visible = false;
        }
        else
        {
            Session[P_Dt_Solicitud] = null;
            Grid_Solicitudes.DataSource = null;
            Grid_Solicitudes.DataBind();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Sergio Manuel Gallardo Andrade
    /// FECHA_CREO  : 18/Noviembre/2011
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACION: 
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Limpia_Controles();
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Llenar_Grid_Solicitudes_Pendientes();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Sergio Manuel Gallardo Andrade
    /// FECHA_CREO  : 18/Noviembre/2011
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACION: 
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Txt_No_Solicitud_Pago.Text ="";
            // text de solicitud de pago
            Txt_No_Solicitud.Text = "";
            Txt_Fecha.Text = "";
            Txt_Motivo_Cancelacion.Text = "";
            Txt_Tipo_Solicitud_Pago.Text = "";
            Txt_MesAnio.Text = "";
            Txt_No_Reserva_Solicitud.Text = "";
            Txt_Estatus_Solicitud.Text = "";
            Txt_Concepto.Text = "";
            Txt_Monto.Text = "";
            Txt_No_Poliza.Text = "";
            Txt_No_Factura.Text = "";
            Txt_Fecha_Factura.Text = "";
            // Text de Datos de pago
            Txt_No_Pago.Text = "";
            Txt_Fecha_No_Pago.Text = "";
            Cmb_Tipo_Pago.SelectedIndex = 0;
            Cmb_Estatus.SelectedIndex = 0;
            Cmb_Banco.SelectedIndex = -1;
            Txt_No_Cheque.Text = "";
            Txt_Referencia_Pago.Text = "";
            Txt_Comentario_Pago.Text = "";
            Txt_Beneficiario_Pago.Text = "";

        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Sergio Manuel Gallardo Andrade
    /// FECHA_CREO  : 18/Noviembre/2011
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
                    Habilitado = false;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    //Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Div_Solicitudes_Pendientes.Style.Add("display", "block");
                    Div_Datos_Solicitud.Style.Add("display", "none");
                    Configuracion_Acceso("Frm_Ope_Con_Cheques.aspx");
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = false;
                    Cmb_Estatus.Enabled = false;
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    //Btn_Eliminar.Visible = false;                    
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Div_Solicitudes_Pendientes.Style.Add("display", "none");
                    Div_Datos_Solicitud.Style.Add("display", "block");
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Cmb_Estatus.Enabled = false;
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    //Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Cmb_Estatus.Enabled= true;
                    //Habilitado = true;
                    //Btn_Nuevo.ToolTip = "Nuevo";
                    //Btn_Modificar.ToolTip = "Actualizar";
                    //Btn_Salir.ToolTip = "Cancelar";
                    //Btn_Nuevo.Visible = false;
                    //Btn_Modificar.Visible = true;
                    ////Btn_Eliminar.Visible = false;
                    //Btn_Nuevo.CausesValidation = true;
                    //Btn_Modificar.CausesValidation = true;
                    //Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    //Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    //Cmb_Estatus.Enabled = true;
                    break;
            }


            // text de solicitud de pago
            Txt_No_Solicitud.Enabled = false;
            Txt_Fecha.Enabled = false;
            Txt_Tipo_Solicitud_Pago.Enabled = false;
            Txt_MesAnio.Enabled = false;
            Txt_No_Reserva_Solicitud.Enabled = false;
            Txt_Estatus_Solicitud.Enabled = false;
            Txt_Concepto.Enabled = false;
            Txt_Monto.Enabled = false;
            Txt_No_Poliza.Enabled = false;
            Txt_No_Factura.Enabled = false;
            Txt_Fecha_Factura.Enabled = false;
            // Text de Datos de pago
            Txt_No_Pago.Enabled = Habilitado;
            Txt_Fecha_No_Pago.Enabled = Habilitado;
            Cmb_Tipo_Pago.Enabled = Habilitado;
            Cmb_Banco.Enabled = Habilitado;
            Txt_No_Cheque.Enabled = Habilitado;
            Txt_Referencia_Pago.Enabled = Habilitado;
            Txt_Comentario_Pago.Enabled = Habilitado;
            Txt_Beneficiario_Pago.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Seleccionar_Requisicion_Click
    ///DESCRIPCIÓN: Metodo para consultar la reserva
    ///CREO: Sergio Manuel Gallardo Andrade
    ///FECHA_CREO: 17/noviembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Seleccionar_Solicitud_Click(object sender, ImageClickEventArgs e)
    {
        String No_Solicitud = ((ImageButton)sender).CommandArgument;
        Evento_Grid_Solicitudes_Seleccionar(No_Solicitud);
    }
      ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Btn_Buscar_Solicitud_Click
    ///DESCRIPCIÓN: Metodo para consultar la reserva
    ///CREO: Sergio Manuel Gallardo Andrade
    ///FECHA_CREO: 17/noviembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Solicitud_Click(object sender, ImageClickEventArgs e)
    {
     Cls_Ope_Con_Cheques_Negocio consulta_Negocio = new Cls_Ope_Con_Cheques_Negocio();
     consulta_Negocio.P_Fecha_Inicio = String.Format("{0:dd-MMM-yyyy}", Txt_Fecha_Inicio.Text);
     consulta_Negocio.P_Fecha_Final = String.Format("{0:dd-MMM-yyyy}", Txt_Fecha_Final.Text);
     if (Txt_No_Solicitud_Pago.Text != "")
     {
         consulta_Negocio.P_No_Solicitud_Pago =Txt_No_Solicitud_Pago.Text;
     }
     if (Cmb_Tipo_Solicitud.SelectedIndex > 0)
     {
         consulta_Negocio.P_Tipo_Solicitud_Pago_ID = Cmb_Tipo_Solicitud.SelectedValue;
     }
     consulta_Negocio.P_Estatus = "Busqueda";
     Session[P_Dt_Solicitud] = consulta_Negocio.Consulta_Solicitudes_Autorizadas();
     if (Session[P_Dt_Solicitud] != null && ((DataTable)Session[P_Dt_Solicitud]).Rows.Count > 0)
     {
         Grid_Solicitudes.Columns[2].Visible = true;
         Grid_Solicitudes.DataSource = Session[P_Dt_Solicitud] as DataTable;
         Grid_Solicitudes.DataBind();
         Grid_Solicitudes.Columns[2].Visible = false;
     }
     else
     {
         Session[P_Dt_Solicitud] = null;
         Grid_Solicitudes.DataSource = null;
         Grid_Solicitudes.DataBind();
     }
    }
   
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Evento_Grid_Solicitudes_Seleccionar
    ///DESCRIPCIÓN:
    ///CREO: Sergio Manuel Gallardo Andrade
    ///FECHA_CREO: 17/Noviembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Evento_Grid_Solicitudes_Seleccionar(String Dato)
    {
        Cls_Ope_Con_Solicitud_Pagos_Negocio Rs_Consulta_Ope_Con_Solicitud_Pagos = new Cls_Ope_Con_Solicitud_Pagos_Negocio(); //Variable de coneción hacia la capa de datos
        DataTable Dt_Solicitud_Pagos = new DataTable(); //Variable a contener los datos de la consulta de la solicitud de pago
        Habilitar_Controles("Inicial");
        Btn_Modificar.ToolTip = "Cancelar Pago";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
        Btn_Modificar.Visible = false;
        Div_Datos_Solicitud.Style.Add("display", "block");
        Div_Solicitudes_Pendientes.Style.Add("display", "none");
        Txt_Fecha_No_Pago.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
        Cls_Ope_Con_Cheques_Negocio Cheques = new Cls_Ope_Con_Cheques_Negocio();
        Cls_Cat_Dependencias_Negocio Dependencia_Negocio = new Cls_Cat_Dependencias_Negocio();
        Cls_Ope_Con_Reservas_Negocio Reserva_Negocio = new Cls_Ope_Con_Reservas_Negocio();
        String No_Solicitud = Dato;//Grid_Requisiciones.SelectedDataKey["No_Requisicion"].ToString();
        DataRow[] Solicitud = ((DataTable)Session[P_Dt_Solicitud]).Select("NO_SOLICITUD_PAGO = '" + No_Solicitud + "'");
        Txt_No_Solicitud.Text = Solicitud[0]["NO_SOLICITUD_PAGO"].ToString();
        String Fecha = Solicitud[0]["FECHA_SOLICITUD"].ToString();
        Fecha = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Fecha));
        Txt_Fecha.Text = Fecha.ToUpper();
        //Seleccionar combo dependencia
        Txt_Tipo_Solicitud_Pago.Text =
            Solicitud[0]["TIPO_PAGO"].ToString().Trim();
        Txt_MesAnio.Text =
            Solicitud[0]["MES_ANO"].ToString();
        //Total de la requisición
        Txt_No_Reserva_Solicitud.Text =
            Solicitud[0]["NO_RESERVA"].ToString();
        //Se llena el combo partidas
        Txt_Estatus_Solicitud.Text=
            Solicitud[0]["ESTATUS"].ToString();
        Txt_Concepto.Text =
            Solicitud[0]["CONCEPTO"].ToString();
        String Monto = Solicitud[0]["MONTO"].ToString();
        Monto = String.Format("{0:c}", Monto);
         Txt_Monto.Text = Monto;
        Txt_No_Poliza.Text =
             Solicitud[0]["NO_POLIZA"].ToString();
        Txt_No_Factura.Text =
             Solicitud[0]["NO_FACTURA"].ToString();
        Txt_Fecha_Factura.Text =
             Solicitud[0]["FECHA_FACTURA"].ToString();
        Btn_Nuevo.Visible = true;
        Rs_Consulta_Ope_Con_Solicitud_Pagos.P_No_Solicitud_Pago = Solicitud[0]["NO_SOLICITUD_PAGO"].ToString();
        Dt_Solicitud_Pagos = Rs_Consulta_Ope_Con_Solicitud_Pagos.Consulta_Datos_Solicitud_Pago();
        foreach (DataRow Registro in Dt_Solicitud_Pagos.Rows)
        {
            Txt_Cuenta_Contable_Proveedor.Value = Registro[Cat_Com_Proveedores.Campo_Cuenta_Contable_ID].ToString();
        }
        if (Txt_Estatus_Solicitud.Text == "PAGADO")         
        { 
           Cheques.P_No_Solicitud_Pago= No_Solicitud;
           Cheques.P_Estatus = "PAGADO";
           DataTable Pago = Cheques.Consulta_Datos_Pago();
            if (Pago.Rows.Count > 0)
            {
                Txt_No_Pago.Text=
                Pago.Rows[0]["NO_PAGO"].ToString().Trim();
                Txt_Fecha.Text=
                 string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Pago.Rows[0]["FECHA_PAGO"].ToString()));
                Cmb_Tipo_Pago.SelectedValue= Pago.Rows[0]["FORMA_PAGO"].ToString().Trim();
                Cmb_Estatus.SelectedValue = Pago.Rows[0]["ESTATUS"].ToString().Trim();
                if (Pago.Rows[0]["ESTATUS"].ToString().Trim() == "CANCELADO")
                {
                  Txt_Motivo_Cancelacion.Text=
                 Pago.Rows[0]["MOTIVO_CANCELACION"].ToString();
                }
                Txt_No_Cheque.Text=
                    Pago.Rows[0]["NO_CHEQUE"].ToString(); 
                Txt_Referencia_Pago.Text =
                    Pago.Rows[0]["REFERENCIA_TRANSFERENCIA_BANCA"].ToString();
                Txt_Comentario_Pago.Text =
                    Pago.Rows[0]["COMENTARIOS"].ToString();
                Txt_Beneficiario_Pago.Text=
                    Pago.Rows[0]["BENEFICIARIO_PAGO"].ToString();
                Cmb_Banco.SelectedValue = Pago.Rows[0]["BANCO_ID"].ToString();
                Txt_Cuenta_Contable_ID_Banco.Value = Pago.Rows[0]["CUENTA_CONTABLE_ID"].ToString();
            }
            Btn_Modificar.Visible = true;
            Cmb_Estatus.Enabled = true;
        }
    }
    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Llenar_Combos_Generales()
    // DESCRIPCIÓN: Llena los combos principales de la interfaz de usuario
    // RETORNA: 
    // CREO: Sergio Manuel Gallardo Andrade
    // FECHA_CREO: 17/Noviembre/2011 
    // MODIFICO:
    // FECHA_MODIFICO:
    // CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    public void Llenar_Combos_Generales()
    {
        Cls_Cat_Nom_Bancos_Negocio Bancos_Negocio = new Cls_Cat_Nom_Bancos_Negocio();
        DataTable Dt_Bancos = Bancos_Negocio.Consulta_Bancos();
        Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Banco,Dt_Bancos,"NOMBRE","BANCO_ID");
    }
    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Validaciones
    // DESCRIPCIÓN: Genera el String con la informacion que falta y ejecuta la 
    // operacion solicitada si las validaciones son positivas
    // RETORNA: 
    // CREO: Gustavo Angeles Cruz
    // FECHA_CREO: 24/Agosto/2010 
    // MODIFICO:
    // FECHA_MODIFICO:
    // CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    private Boolean Validaciones(bool Validar_Completo)
    {
        Boolean Bln_Bandera;
        Bln_Bandera = true;
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
        //Verifica que campos esten seleccionados o tengan valor valor
        if (Cmb_Banco.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += " Seleccionar un Banco. <br>";
            Bln_Bandera = false;
        }
        if (Cmb_Tipo_Pago.SelectedValue  == "CHEQUE")
        {
            if (Txt_No_Cheque.Text == "")
            {
                Lbl_Mensaje_Error.Text += " Introduccir el número de cheque <br>";
                Bln_Bandera = false;
            }
        }
        else {
            if (Txt_Referencia_Pago.Text == "")
            {
                Lbl_Mensaje_Error.Text += " Introduccir la Referencia de la transferencia<br>";
                Bln_Bandera = false;
            }
        }
        if (Txt_Comentario_Pago.Text == "")
        {
            Lbl_Mensaje_Error.Text += " Introduccir un comentario <br>";
            Bln_Bandera = false;
        }
        if (Txt_Beneficiario_Pago.Text == "")
        {
            Lbl_Mensaje_Error.Text += " Introduccir un beneficiario <br>";
            Bln_Bandera = false;
        }
        if (Cmb_Estatus.SelectedValue == "CANCELADO") {
            if (Txt_Motivo_Cancelacion.Text == "")
            {
                Lbl_Mensaje_Error.Text += " Introduccir el motivo de la cancelación  <br>";
                Bln_Bandera = false;
            } 
        }
        if (!Bln_Bandera)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
        return Bln_Bandera;
    }
    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Validaciones
    // DESCRIPCIÓN: Genera el String con la informacion que falta y ejecuta la 
    // operacion solicitada si las validaciones son positivas
    // RETORNA: 
    // CREO: Gustavo Angeles Cruz
    // FECHA_CREO: 24/Agosto/2010 
    // MODIFICO:
    // FECHA_MODIFICO:
    // CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    private Boolean Validar_Modificacion(bool Validar_Completo)
    {
        Boolean Bln_Bandera;
        Bln_Bandera = true;
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
        //Verifica que campos esten seleccionados o tengan valor valor
        if (Txt_Motivo_Cancelacion.Text == "" || Txt_Motivo_Cancelacion.Text == null)
        {
            Lbl_Mensaje_Error.Text += " El Motivo de la cancelacion del Pago <br>";
            Bln_Bandera = false;
        }
        if (!Bln_Bandera)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
        return Bln_Bandera;
    }
    //#endregion

    //#region (Metodos de Operacion [Alta - Modificar - Eliminar])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Cheque
    /// DESCRIPCION : Modifica los datos del pago  con los datos 
    ///               proporcionados por el usuario
    /// PARAMETROS  : 
    /// CREO        : Sergio Manuel Gallardo Andrade
    /// FECHA_CREO  : 25/Noviembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Cheque()
    {
        DataTable Dt_Partidas_Polizas = new DataTable(); //Obtiene los detalles de la póliza que se debera generar para el movimiento
        //DataTable Dt_Datos_Polizas = new DataTable(); //Obtiene los detalles de la póliza que se debera generar para el movimiento
        Cls_Ope_Con_Cheques_Negocio Rs_Modificar_Ope_Con_Pagos = new Cls_Ope_Con_Cheques_Negocio(); //Variable de conexión hacia la capa de Negocios
        //Rs_Modificar_Ope_Con_Solicitud_Pagos.P_No_Solicitud_Pago = No_Solicitud_Pago;
        //Dt_Datos_Polizas = Rs_Modificar_Ope_Con_Solicitud_Pagos.Consulta_Datos_Solicitud_Pago();
        //foreach (DataRow Registro in Dt_Datos_Polizas.Rows)
        //{
        //    Txt_Monto_Solicitud.Value = Registro[Ope_Con_Solicitud_Pagos.Campo_Monto].ToString();
        //    Txt_Cuenta_Contable_ID_Proveedor.Value = Registro[Cat_Com_Proveedores.Campo_Cuenta_Contable_ID].ToString();
        //    Txt_Cuenta_Contable_reserva.Value = Registro["Cuenta_Contable_Reserva"].ToString();
        //    Txt_No_Reserva.Value = Registro["NO_RESERVA"].ToString();
        //}
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
            row[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Txt_Cuenta_Contable_Proveedor.Value;
            row[Ope_Con_Polizas_Detalles.Campo_Concepto] = "CANCELACION-" + Txt_No_Pago.Text.ToString();
            row[Ope_Con_Polizas_Detalles.Campo_Debe] = 0;
            row[Ope_Con_Polizas_Detalles.Campo_Haber] = Convert.ToDouble(Txt_Monto.Text.ToString());
            Dt_Partidas_Polizas.Rows.Add(row); //Agrega el registro creado con todos sus valores a la tabla
            Dt_Partidas_Polizas.AcceptChanges();

            row = Dt_Partidas_Polizas.NewRow();
            //Agrega el abono del registro de la póliza
            row[Ope_Con_Polizas_Detalles.Campo_Partida] = 2;
            row[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Txt_Cuenta_Contable_ID_Banco.Value;
            row[Ope_Con_Polizas_Detalles.Campo_Concepto] = "CANCELACION-" + Txt_No_Pago.Text.ToString();
            row[Ope_Con_Polizas_Detalles.Campo_Debe] = Convert.ToDouble(Txt_Monto.Text.ToString());
            row[Ope_Con_Polizas_Detalles.Campo_Haber] = 0;
            Dt_Partidas_Polizas.Rows.Add(row); //Agrega el registro creado con todos sus valores a la tabla
            Dt_Partidas_Polizas.AcceptChanges();

            //Agrega los valores a pasar a la capa de negocios para ser dados de alta
            Rs_Modificar_Ope_Con_Pagos.P_Dt_Detalles_Poliza = Dt_Partidas_Polizas;
            Rs_Modificar_Ope_Con_Pagos.P_No_Solicitud_Pago = Txt_No_Solicitud.Text;
            Rs_Modificar_Ope_Con_Pagos.P_Estatus = "CANCELADO";
            Rs_Modificar_Ope_Con_Pagos.P_Monto = Txt_Monto.Text;
            Rs_Modificar_Ope_Con_Pagos.P_Comentario  = "CANCELACION-" + Txt_No_Pago.Text;
            Rs_Modificar_Ope_Con_Pagos.P_Motivo_Cancelacion = Txt_Motivo_Cancelacion.Text;
            Rs_Modificar_Ope_Con_Pagos.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;
            Rs_Modificar_Ope_Con_Pagos.P_No_Reserva =Convert.ToDouble(Txt_No_Reserva_Solicitud.Text);
            Rs_Modificar_Ope_Con_Pagos.Modificar_Pago(); //Modifica el registro que fue seleccionado por el usuario con los nuevos datos proporcionados
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Solicitud de Pagos", "alert('La Modificación de la Solicitud de Pago fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Solicitud_Pago " + ex.Message.ToString(), ex);
        }
    }
    /////*******************************************************************************
    ///// NOMBRE DE LA FUNCION: Modificar_Cheque
    ///// DESCRIPCION : Modifica el cheque con los datos proporcionados por el usuario
    ///// PARAMETROS  : 
    ///// CREO        : Sergio Manuel Gallardo Andrade
    ///// FECHA_CREO  : 22/oviembre/2011
    ///// MODIFICO          :
    ///// FECHA_MODIFICO    :
    ///// CAUSA_MODIFICACION:
    /////*******************************************************************************
    //private void Modificar_Cheque()
    //{
    //    //String Reserva;
    //    //String No_Poliza;
    //    //String Tipo_Poliza;
    //    //String Mes_Anio;
    //    //String Monto;
    //    //int Actualiza_Presupuesto;
    //    //int Registra_Movimiento;
    //    //DataTable Dt_Solicitud_pago;
    //    //Cls_Ope_Con_Autoriza_Solicitud_Pago_Contabilidad_Negocio Rs_Solicitud = new Cls_Ope_Con_Autoriza_Solicitud_Pago_Contabilidad_Negocio();
    //    //Cls_Ope_Con_Cheques_Negocio Rs_Cheque= new Cls_Ope_Con_Cheques_Negocio(); //Variable de conexion hacia la capa de negocios
    //    //try
    //    //{
    //    //    Rs_Cheque.P_No_Solicitud_Pago = Txt_No_Solicitud.Text.Trim();
    //    //    Rs_Cheque.P_No_Pago = Txt_No_Pago.Text.Trim();
    //    //    Rs_Cheque.P_Estatus = Cmb_Estatus.SelectedValue.ToString();
    //    //    Rs_Cheque.P_Comentario = Txt_Comentario_Pago.Text;
    //    //    Rs_Cheque.P_Beneficiario_Pago  = Txt_Beneficiario_Pago.Text;
    //    //    Rs_Cheque.P_Banco_ID = Cmb_Banco.SelectedValue;
    //    //    Rs_Cheque.P_Tipo_Pago = Cmb_Tipo_Pago.SelectedValue.ToString();
    //    //    if (Cmb_Tipo_Pago.SelectedValue.ToString() == "CHEQUE")
    //    //    {
    //    //     Rs_Cheque.P_No_Cheque=Txt_No_Cheque.Text.Trim();
    //    //    }
    //    //    else {
    //    //        Rs_Cheque.P_Referencia = Txt_Referencia_Pago.Text.Trim();
    //    //    }
    //    //    if (Cmb_Estatus.SelectedValue.ToString() == "CANCELADO")
    //    //    {
    //    //        Rs_Cheque.P_Motivo_Cancelacion = Txt_Motivo_Cancelacion.Text.Trim();
    //    //    }
    //    //    Rs_Cheque.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;
    //    //    Rs_Cheque.Modificar_Cheque(); //Modifica el registro en base a los datos proporcionados
    //    //    if (Cmb_Estatus.SelectedValue.ToString() == "CANCELADO")
    //    //    {
    //    //        Rs_Solicitud.P_No_Solicitud_Pago = Txt_No_Solicitud.Text.Trim();
    //    //        Dt_Solicitud_pago = Rs_Solicitud.Consultar_Solicitud_Pago();
    //    //        if (Dt_Solicitud_pago.Rows.Count > 0)
    //    //        {
    //    //            Reserva = Dt_Solicitud_pago.Rows[0]["NO_RESERVA"].ToString().Trim();
    //    //            Monto = Dt_Solicitud_pago.Rows[0]["MONTO"].ToString().Trim();
    //    //            No_Poliza = Dt_Solicitud_pago.Rows[0]["NO_POLIZA"].ToString().Trim();
    //    //            Tipo_Poliza = Dt_Solicitud_pago.Rows[0]["TIPO_POLIZA_ID"].ToString().Trim();
    //    //            Mes_Anio = Dt_Solicitud_pago.Rows[0]["MES_ANO"].ToString().Trim();
    //    //            Actualiza_Presupuesto = Cls_Ope_Psp_Manejo_Presupuesto.Actualizar_Momentos_Presupuestales(Reserva, "EJERCIDO", "PAGADO", Convert.ToDouble(Monto));
    //    //            Registra_Movimiento = Cls_Ope_Psp_Manejo_Presupuesto.Registro_Movimiento_Presupuestal(Reserva, "EJERCIDO", "PAGADO", Convert.ToDouble(Monto), No_Poliza, Tipo_Poliza, Mes_Anio, "");
    //    //        }
    //    //        Reserva = "";
    //    //        Monto = "";
    //    //        No_Poliza = "";
    //    //        Tipo_Poliza = "";
    //    //        Mes_Anio = "";
    //    //        Dt_Solicitud_pago = null;
    //    //    }            
    //    //    Limpia_Controles(); //Limpia los controles del modulo
    //    //    Inicializa_Controles();
    //    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pagos", "alert('La modificación del Pago fue Exitosa');", true);
    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //    throw new Exception("Modificar_Cheque " + ex.Message.ToString(), ex);
    //    //}
    //}
    //#endregion
    //#endregion


    //#region (Eventos)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Cheque
    /// DESCRIPCION : Da de Alta el Cheque con los datos proporcionados
    /// PARAMETROS  : 
    /// CREO        : Sergio Manuel Gallardo Andrade
    /// FECHA_CREO  : 21/Noviembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Cheque()
    {
        try
        {
            DataTable Dt_Banco_Cuenta_Contable = new DataTable();
            DataTable Dt_Partidas_Polizas = new DataTable(); //Obtiene los detalles de la póliza que se debera generar para el movimiento
            Cls_Ope_Con_Autoriza_Solicitud_Pago_Contabilidad_Negocio Rs_Solicitud = new Cls_Ope_Con_Autoriza_Solicitud_Pago_Contabilidad_Negocio();
            Cls_Ope_Con_Cheques_Negocio Rs_Ope_Con_Cheques = new Cls_Ope_Con_Cheques_Negocio(); //Variable de conexion con la capa de datos
           // Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Presupuesto = new Cls_Ope_SAP_Dep_Presupuesto_Negocio(); //Variable de conexion con la capa de Negocios.
            Rs_Ope_Con_Cheques.P_Banco_ID = Cmb_Banco.SelectedValue;
           Dt_Banco_Cuenta_Contable= Rs_Ope_Con_Cheques.Consulta_Cuenta_Contable_Banco();
           foreach (DataRow Renglon in Dt_Banco_Cuenta_Contable.Rows )
           {
               Txt_Cuenta_Contable_ID_Banco.Value = Renglon[Cat_Nom_Bancos.Campo_Cuenta_Contable_ID].ToString();
           }
            //Agrega los campos que va a contener el DataTable de los detalles de la póliza
            Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Partida, typeof(System.Int32));
            Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID, typeof(System.String));
            Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Debe, typeof(System.Double));
            Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Haber, typeof(System.Double));

            DataRow row = Dt_Partidas_Polizas.NewRow(); //Crea un nuevo registro a la tabla

           
            //Agrega el abono del registro de la póliza
            row[Ope_Con_Polizas_Detalles.Campo_Partida] = 1;
            row[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Txt_Cuenta_Contable_ID_Banco.Value;
            row[Ope_Con_Polizas_Detalles.Campo_Debe] = 0;
            row[Ope_Con_Polizas_Detalles.Campo_Haber] = Convert.ToDouble(Txt_Monto.Text.ToString());

            Dt_Partidas_Polizas.Rows.Add(row); //Agrega el registro creado con todos sus valores a la tabla
            Dt_Partidas_Polizas.AcceptChanges();
            row = Dt_Partidas_Polizas.NewRow();
            //Agrega el cargo del registro de la póliza
            row[Ope_Con_Polizas_Detalles.Campo_Partida] = 2;
            row[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Txt_Cuenta_Contable_Proveedor.Value;
            row[Ope_Con_Polizas_Detalles.Campo_Debe] = Convert.ToDouble(Txt_Monto.Text.ToString());
            row[Ope_Con_Polizas_Detalles.Campo_Haber] = 0;

            Dt_Partidas_Polizas.Rows.Add(row); //Agrega el registro creado con todos sus valores a la tabla
            Dt_Partidas_Polizas.AcceptChanges();
            Rs_Ope_Con_Cheques.P_No_Reserva = Convert.ToDouble(Txt_No_Reserva_Solicitud.Text.Trim());
            Rs_Ope_Con_Cheques.P_Dt_Detalles_Poliza = Dt_Partidas_Polizas;
            Rs_Ope_Con_Cheques.P_No_Solicitud_Pago= Txt_No_Solicitud.Text.Trim();
            Rs_Ope_Con_Cheques.P_Comentario = Txt_Comentario_Pago.Text.Trim();
            Rs_Ope_Con_Cheques.P_Monto = Txt_Monto.Text.Trim();
            Rs_Ope_Con_Cheques.P_Estatus= Cmb_Estatus.SelectedItem.ToString();
            Rs_Ope_Con_Cheques.P_Fecha_Pago= Txt_Fecha.Text;
            Rs_Ope_Con_Cheques.P_Mes_Ano = String.Format("{0:MMyy}", DateTime.Now).ToString();
            Rs_Ope_Con_Cheques.P_Tipo_Pago = Cmb_Tipo_Pago.SelectedItem.ToString();
            Rs_Ope_Con_Cheques.P_Banco_ID = Cmb_Banco.SelectedValue;
            Rs_Ope_Con_Cheques.P_No_Poliza = "0000000038";
            Rs_Ope_Con_Cheques.P_Beneficiario_Pago = Txt_Beneficiario_Pago.Text.Trim();
            Rs_Ope_Con_Cheques.P_Tipo_Poliza_ID = "00001";
            if (Cmb_Tipo_Pago.SelectedValue == "CHEQUE")
            {
                Rs_Ope_Con_Cheques.P_No_Cheque = Txt_No_Cheque.Text.Trim();
            }
            else {
                Rs_Ope_Con_Cheques.P_Referencia = Txt_Referencia_Pago.Text.Trim();
            }
            Rs_Ope_Con_Cheques.P_Fecha_Creo = String.Format("{0:dd/MMM/yy}", DateTime.Now).ToString();
            Rs_Ope_Con_Cheques.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
            Rs_Ope_Con_Cheques.Alta_Cheque();
            Limpia_Controles();
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Compromiso " + ex.Message.ToString(), ex);
        }
    }
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                if (Txt_No_Pago.Text == "")
                {
                    Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                    Llenar_Combos_Generales();
                }
                else {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pagos", "alert('EL Cheque solo se puede modificar');", true);
                }
               
            }
            else
            {
                //Valida los datos ingresados por el usuario.
                if (Validaciones(true))
                {
                    Alta_Cheque();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Cheques", "alert('El alta del Pago fue exitoso');", true);
                    Inicializa_Controles();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Metodo que permite modificar la reserva
    ///CREO: Sergio Manuel Gallardo Andrade
    ///FECHA_CREO: 17/Noviembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
         try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                if (Btn_Modificar.ToolTip == "Modificar")
                {
                        Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                    if (Validar_Modificacion(true))
                    {
                        Modificar_Cheque(); //Modifica los datos de la Cuenta Contable con los datos proporcionados por el usuario
                    }                      
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
    }
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
                Lbl_Mensaje_Error.Text ="";
                Img_Error.Visible = false;


            if (Btn_Salir.ToolTip == "Salir")
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario en el catálogo
                Limpia_Controles();//Limpia los controles de la forma
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
}