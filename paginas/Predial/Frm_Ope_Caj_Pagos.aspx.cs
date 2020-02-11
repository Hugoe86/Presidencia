using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Caja_Pagos.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using Presidencia.Reportes;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using System.Data.OracleClient;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;

public partial class paginas_Predial_Frm_Ope_Caj_Pagos : System.Web.UI.Page
{
    #region(Load/Init)
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
                //valida si viene de la ventana de resumen
                if (Request.QueryString["Referencia"] != null)
                {
                    if (!String.IsNullOrEmpty(this.Request.QueryString["Referencia"].ToString()))
                    {
                        Txt_Busqueda_Referencia.Text = this.Request.QueryString["Referencia"].ToString().Trim();
                        //Realiza la consulta del pasivo
                        Consulta_Datos_Pasivo("POR PAGAR", "");
                    }
                }
                
                Lbl_Mensaje_Error.Text = "";
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                //String Str_Fecha_Apertura_Turno = Obtener_Dato_Consulta(Ope_Caj_Turnos.Campo_Fecha_Turno, Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos, Ope_Caj_Turnos.Campo_Estatus + " = 'ABIERTO' AND " + Ope_Caj_Turnos.Campo_Empleado_ID + " = '" + Cls_Sessiones.Empleado_ID + "'");
                //if (Str_Fecha_Apertura_Turno.Trim() != "")
                //{
                //    if (Convert.ToDateTime(Str_Fecha_Apertura_Turno.Trim()).ToShortDateString() != DateTime.Now.ToShortDateString())
                //    {
                //        Lbl_Mensaje_Error.Text = "El turno actual está abierto desde el " + Convert.ToDateTime(Str_Fecha_Apertura_Turno.Trim()).ToString("dd/MMM/yyyy") + ". Favor de Rectificarlo.";
                //        Lbl_Mensaje_Error.Visible = true;
                //        Img_Error.Visible = true;
                //    }
                //}
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
    #region(Metodos)
    #region(Metodos Generales)

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Mostrar_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN          : Muestra los datos de la consulta
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Mostrar_Busqueda_Avanzada_Click(object sender, ImageClickEventArgs e)
    {
        Boolean Busqueda_Ubicaciones;
        String Cuenta_Predial_ID;
        String Cuenta_Predial;
        //Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        Busqueda_Ubicaciones = Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]);


        if (Busqueda_Ubicaciones)
        {
            if (Session["CUENTA_PREDIAL_ID"] != null)
            {
                Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Session["Cuenta_Predial_ID"] = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                //Hdf_Cuenta_Predial_ID.Value = Cuenta_Predial_ID;
                Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
                //Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = Cuenta_Predial;
                //Txt_Cuenta_Predial.Text = Cuenta_Predial;
                //Habilitar_Controles();
                Txt_Busqueda_Referencia.Text = Cuenta_Predial;
            }
        }
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");
        Session.Remove("CUENTA_PREDIAL");

        //Consultar_Datos_Cuenta_Constancia();
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 22-Agosto-2011
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
            Consulta_Caja_Empleado();       //Consulta la caja que tiene asignado el empleado
            Consulta_Bancos_Ingresos();     //Consulta los bancos que son unicamente para ingresos
            Consulta_Datos_Turno();
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
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 22-Agosto-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Session.Remove("CAJA_FORMAS_PAGO");
            Grid_Folios_Ingresos_Pasivos.DataSource = new DataTable();
            Grid_Folios_Ingresos_Pasivos.DataBind();
            Grid_Folios_Ingresos_Pasivos.DataBind();
            Cmb_Forma_Pago_Bancos.SelectedIndex = -1;
            Cmb_Plan_Pago_Bancario.SelectedIndex = -1;
            Hdn_Cuenta_Predial_ID.Value = "";
            Hdn_Cuenta_Predial.Value = "";
            Hdn_Propietario_ID.Value = "";
            Hdn_Tasa_ID.Value = "";
            Txt_Busqueda_Referencia.Text = "";
            Txt_Cambio_Ingresos.Text = "";
            //Txt_Descripcion_Ingresos.Text = "";
            Txt_Estatus_Ingresos.Text = "";
            //Txt_Fecha_Tramite_Ingresos.Text = "";
            //Txt_Fecha_Vencimiento.Text = "";
            Txt_Ajuste_Tarifario.Text = "";
            Txt_Importe_Ingresos.Text = "";
            //Txt_Ingresos.Text = "";
            Txt_Forma_Pago_Monto.Text = "";
            Txt_Forma_Pago_Banco_Referencia.Text = "";
            Txt_Forma_Pago_Monto.Text = "";
            //Txt_No_Transaccion_Bancaria.Text = "";
            Txt_Total_Pagar_Ingreso.Text = "";
            //Txt_No_Recibo_Pago.Text = "";
            Txt_Total_Pagado.Text = "";
            //Cmb_Meses_Bancos.DataSource = new DataSet();
            Cmb_Plan_Pago_Bancario.DataBind();
            Lbl_Forma_Pago.Text = "EFECTIVO";
            Lbl_Forma_Pago_Banco.Visible = false;
            Cmb_Forma_Pago_Bancos.Visible = false;
            Lbl_Forma_Pago_Referencia.Visible = false;
            Txt_Forma_Pago_Banco_Referencia.Visible = false;
            Txt_Forma_Pago_Banco_No_Tarjeta.Visible = false;
            Lbl_Forma_Pago_Plan_Pago.Visible = false;
            Cmb_Plan_Pago_Bancario.Visible = false;
            Txt_Forma_Pago_Monto.Focus();
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///                para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 22-Agosto-2011
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
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    //Configuracion_Acceso("Frm_Ope_Caj_Pagos.aspx");
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    break;
            }
            Txt_Busqueda_Referencia.Enabled = !Habilitado;
            Txt_Forma_Pago_Monto.Enabled = Habilitado;
            Txt_Forma_Pago_Banco_Referencia.Enabled = Habilitado;
            Txt_Forma_Pago_Banco_No_Tarjeta.Enabled = Habilitado;
            Cmb_Forma_Pago_Bancos.Enabled = Habilitado;
            Cmb_Plan_Pago_Bancario.Enabled = Habilitado;
            Btn_Buscar_Referencia.Enabled = !Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    #endregion
    #region (Control Acceso Pagina)
    ///*******************************************************************************
    /// NOMBRE      : Configuracion_Acceso
    /// DESCRIPCIÓN : Habilita las operaciones que podrá realizar el usuario en la página.
    /// PARÁMETROS  : No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ  : 23/Mayo/2011 10:43 a.m.
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
            Botones.Add(Btn_Nuevo);
            Botones.Add(Btn_Buscar_Referencia);

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
    /// PARÁMETROS  : Cadena.- El dato a evaluar si es numerico.
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
    #region (Operaciones)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Caja_Empleado
    /// DESCRIPCION : Consulta la caja que tiene abierto el empleado
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 22-Agosto-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Caja_Empleado()
    {
        DataTable Dt_Caja; //Variable que obtendra los datos de la consulta 
        Cls_Ope_Caj_Pagos_Negocio Rs_Consulta_Cat_Pre_Cajas = new Cls_Ope_Caj_Pagos_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            Rs_Consulta_Cat_Pre_Cajas.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
            Dt_Caja = Rs_Consulta_Cat_Pre_Cajas.Consulta_Caja_Empleado();
            if (Dt_Caja.Rows.Count > 0)
            {
                //Muestra todos los datos que tiene el folio que proporciono el usuario
                foreach (DataRow Registro in Dt_Caja.Rows)
                {
                    Txt_Caja_ID.Text = Registro[Ope_Caj_Turnos.Campo_Caja_Id].ToString();
                    Hdn_No_Caja.Value = Registro[Cat_Pre_Cajas.Campo_Numero_De_Caja].ToString();
                    Txt_No_Turno.Text = Registro[Ope_Caj_Turnos.Campo_No_Turno].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Caja_Empleado " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Bancos_Ingresos
    /// DESCRIPCION : Consulta los bancos que son validos para ingresos y los agrega
    ///               a los combos correspondientes los valores encontrados
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 23-Agosto-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Bancos_Ingresos()
    {
        DataTable Dt_Bancos; //Obtiene los valores obtenidos de la consulta
        Cls_Ope_Caj_Pagos_Negocio Rs_Consulta_Cat_Nom_Bancos = new Cls_Ope_Caj_Pagos_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            Dt_Bancos = Rs_Consulta_Cat_Nom_Bancos.Consulta_Bancos_Ingresos(); //Consulta los bancos que pertenecen a ingresos
            //----------------------------BANCOS----------------------------//
            Cmb_Forma_Pago_Bancos.DataSource = Dt_Bancos;
            Cmb_Forma_Pago_Bancos.DataTextField = "Nombre";
            Cmb_Forma_Pago_Bancos.DataValueField = "Banco_ID";
            Cmb_Forma_Pago_Bancos.DataBind();
            Cmb_Forma_Pago_Bancos.Items.Insert(0, new ListItem("<-SELECCIONE->", ""));
            Cmb_Forma_Pago_Bancos.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Bancos_Ingresos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Datos_Turno
    /// DESCRIPCION : Consulta los datos del turno
    /// PARAMETROS  : 
    /// CREO        : Ismael Prieto Sánchez
    /// FECHA_CREO  : 23-Octubre-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Datos_Turno()
    {
        DataTable Dt_Turno; //Variable que obtendra los datos de la consulta 
        Cls_Ope_Caj_Pagos_Negocio Rs_Consulta_Datos_Turno = new Cls_Ope_Caj_Pagos_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            Rs_Consulta_Datos_Turno.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
            Rs_Consulta_Datos_Turno.P_Caja_ID = Txt_Caja_ID.Text;
            Dt_Turno = Rs_Consulta_Datos_Turno.Consulta_Datos_Turno();
            if (Dt_Turno.Rows.Count > 0)
            {
                foreach (DataRow Registro in Dt_Turno.Rows)
                {
                    Txt_No_Recibo_Pago.Text = Registro[Ope_Caj_Turnos.Campo_Contador_Recibo].ToString();
                    //Consultar si la caja es foranea
                    Txt_Fecha_Aplicacion.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Ope_Caj_Turnos.Campo_Aplicacion_Pago].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Datos_Turno " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Turnos_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de un Turno Seleccionado para mostrarlos en el 
    ///             formulario.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 28/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Folios_Cuenta_Predial_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Folios_Cuenta_Predial.SelectedIndex > (-1))
        {
            Txt_Busqueda_Referencia.Text = Grid_Folios_Cuenta_Predial.SelectedRow.Cells[1].Text.Trim();
            Div_Datos_Cuenta_Predial_Pasivo.Visible = false;
        }
    }
    protected void Grid_Folios_Cuenta_Predial_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cls_Ope_Caj_Pagos_Negocio Rs_Consulta_Ope_Ing_Pasivo = new Cls_Ope_Caj_Pagos_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Pasivo;
        try
        {
            Rs_Consulta_Ope_Ing_Pasivo.P_Cuenta_Predial = Txt_Busqueda_Referencia.Text.Trim();
            Dt_Pasivo = Rs_Consulta_Ope_Ing_Pasivo.Consulta_Datos_Pasivo_Cuenta_Predial();
            Grid_Folios_Cuenta_Predial.PageIndex = e.NewPageIndex;
            Grid_Folios_Cuenta_Predial.DataSource = Dt_Pasivo;
            Grid_Folios_Cuenta_Predial.DataBind();
        }
        catch (Exception Ex)
        {
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Datos_Pasivo
    /// DESCRIPCION : Consulta los datos del folio que introdujo el empleado
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 22-Agosto-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Datos_Pasivo(string Estatus, string No_Pago)
    {
        DateTime Fecha_Pago = DateTime.Today;             //Obtiene la fecha en que fue pagado el ingreso
        String Estatus_No_Operacion = "";               //Obtiene el Estatus y No de Operacion que tiene el recibo de pago
        Double Total_Importe = 0;
        //Decimal Total_Recargos = 0;
        Double Total_Ajuste_Tarifario = 0;
        Double Total_Pagar = 0;
        Double Decimales = 0;
        String No_Folio;                                 //Obtiene el No de Folio que el empleado desea consultar sus datos
        DataTable Dt_Pasivo;                             //Variable que obtendra los datos de la consulta
        DataTable Dt_Formas_Pago_Pasivo = new DataTable(); //Obtiene las formas de pago que tuvo el ingreso            
        Cls_Ope_Caj_Pagos_Negocio Rs_Consulta_Ope_Ing_Pasivo = new Cls_Ope_Caj_Pagos_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            No_Folio = Txt_Busqueda_Referencia.Text;

            Rs_Consulta_Ope_Ing_Pasivo.P_Referencia = Txt_Busqueda_Referencia.Text.ToUpper().Trim();
            Rs_Consulta_Ope_Ing_Pasivo.P_No_Pago = No_Pago;
            Rs_Consulta_Ope_Ing_Pasivo.P_Estatus = Estatus;
            Dt_Pasivo = Rs_Consulta_Ope_Ing_Pasivo.Consulta_Datos_Pasivo(); //Consulta los datos de la referencia que introjo el empleado
            Limpia_Controles(); //Limpia los controles de la forma
            Txt_Busqueda_Referencia.Text = No_Folio;
            if (Dt_Pasivo.Rows.Count > 0)
            {
                Grid_Folios_Ingresos_Pasivos.Columns[1].Visible = true;
                Grid_Folios_Ingresos_Pasivos.Columns[2].Visible = true;
                Grid_Folios_Ingresos_Pasivos.Columns[3].Visible = true;
                Grid_Folios_Ingresos_Pasivos.Columns[4].Visible = true;
                Grid_Folios_Ingresos_Pasivos.Columns[5].Visible = true;
                Grid_Folios_Ingresos_Pasivos.DataSource = Dt_Pasivo;
                Grid_Folios_Ingresos_Pasivos.DataBind();
                Grid_Folios_Ingresos_Pasivos.Columns[1].Visible = false;
                Grid_Folios_Ingresos_Pasivos.Columns[2].Visible = false;
                Grid_Folios_Ingresos_Pasivos.Columns[3].Visible = false;
                Grid_Folios_Ingresos_Pasivos.Columns[4].Visible = false;
                Grid_Folios_Ingresos_Pasivos.Columns[5].Visible = false;
                //Muestra todos los datos que tiene el folio que proporciono el usuario
                foreach (DataRow Registro in Dt_Pasivo.Rows)
                {
                    Txt_Busqueda_Referencia.Text = Registro[Ope_Ing_Pasivo.Campo_Referencia].ToString();
                    //if (Registro[Ope_Ing_Pasivo.Campo_No_Recibo].ToString() != "")
                    //{
                    //    Txt_No_Recibo_Pago.Text = Registro[Ope_Ing_Pasivo.Campo_No_Recibo].ToString();
                    //}
                    Txt_Estatus_Ingresos.Text = Registro[Ope_Ing_Pasivo.Campo_Estatus].ToString();
                    if (Registro[Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID] != null)
                    {
                        Hdn_Cuenta_Predial_ID.Value = Registro[Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID].ToString();
                        if (Hdn_Cuenta_Predial_ID.Value != "")
                        {
                            Hdn_Cuenta_Predial.Value = Consulta_Cuenta_Predial();
                        }
                    }
                    //Txt_Descripcion_Ingresos.Text = Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString();
                    //Txt_Fecha_Tramite_Ingresos.Text = String.Format("{0:dd/MMMM/yyyy}", Convert.ToDateTime(Registro[Ope_Ing_Pasivo.Campo_Fecha_Ingreso].ToString()));
                    //Txt_Fecha_Vencimiento.Text = String.Format("{0:dd/MMMM/yyyy}", Convert.ToDateTime(Registro[Ope_Ing_Pasivo.Campo_Fecha_Vencimiento].ToString()));
                    //Txt_Ingresos.Text = Registro["Ingreso"].ToString();
                    Total_Importe += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto]);
                    //if (String.IsNullOrEmpty(Registro[Ope_Ing_Pasivo.Campo_Recargos].ToString()))
                    //{
                    //    Total_Recargos += Convert.ToDecimal(Registro[Ope_Ing_Pasivo.Campo_Recargos].ToString());
                    //}

                    //Rs_Consulta_Ope_Ing_Pasivo.P_No_Pasivo = Registro["NO_PAGO"].ToString();

                    //Dt_Formas_Pago_Pasivo = Rs_Consulta_Ope_Ing_Pasivo.Consulta_Detalles_Formas_Pago(); //Consulta las formas de pago que tuvo el ingreso
                    //if (Dt_Formas_Pago_Pasivo.Rows.Count > 0)
                    //{
                    //    foreach (DataRow Registro_Pagos in Dt_Formas_Pago_Pasivo.Rows)
                    //    {
                    //        Fecha_Pago = Convert.ToDateTime(Registro_Pagos[Ope_Caj_Pagos.Campo_Fecha_Creo].ToString());
                    //        Estatus_No_Operacion = Registro_Pagos[Ope_Caj_Pagos.Campo_Estatus].ToString() + "/" + Hdn_No_Caja.Value + "/" + Convert.ToString(Convert.ToInt16(Registro_Pagos[Ope_Caj_Pagos.Campo_No_Operacion].ToString())) + "/";
                    //        if (Registro_Pagos[Ope_Caj_Pagos_Detalles.Campo_Forma_Pago].ToString() == "EFECTIVO")
                    //        {
                    //            Txt_Monto_Efectivo.Text = String.Format("{0:#0.00}", Convert.ToDecimal(Registro_Pagos[Ope_Caj_Pagos_Detalles.Campo_Monto].ToString()));
                    //        }
                    //        if (Registro_Pagos[Ope_Caj_Pagos_Detalles.Campo_Forma_Pago].ToString() == "BANCO")
                    //        {
                    //            Cmb_Plan_Pago_Bancario.DataBind();
                    //            Cmb_Plan_Pago_Bancario.Items.Insert(0, new ListItem("<-SELECCIONE->", ""));
                    //            Cmb_Plan_Pago_Bancario.Items.Insert(1, new ListItem("0", ""));
                    //            Cmb_Forma_Pago_Bancos.SelectedValue = Registro_Pagos[Ope_Caj_Pagos_Detalles.Campo_Banco_ID].ToString();
                    //            Txt_Monto_Banco.Text = String.Format("{0:#0.00}", Convert.ToDecimal(Registro_Pagos[Ope_Caj_Pagos_Detalles.Campo_Monto].ToString()));
                    //            Txt_No_Autorizacion_Bancaria.Text = Registro_Pagos[Ope_Caj_Pagos_Detalles.Campo_No_Autorizacion].ToString();
                    //            //Txt_No_Transaccion_Bancaria.Text = Registro_Pagos[Ope_Caj_Pagos_Detalles.Campo_No_Transaccion].ToString();
                    //            if (Registro_Pagos[Ope_Caj_Pagos_Detalles.Campo_Plan_Pago].ToString() == "NORMAL")
                    //            {
                    //                Cmb_Plan_Pago_Bancario.SelectedIndex = 1;
                    //            }
                    //            else
                    //            {
                    //                Cmb_Plan_Pago_Bancario.Items.Insert(2, new ListItem(Registro_Pagos[Ope_Caj_Pagos_Detalles.Campo_Meses].ToString(), ""));
                    //                Cmb_Plan_Pago_Bancario.SelectedIndex = 2;
                    //            }
                    //        }
                    //        if (Registro_Pagos[Ope_Caj_Pagos_Detalles.Campo_Forma_Pago].ToString() == "CHEQUE")
                    //        {
                    //            Cmb_Bancos_Cheques.SelectedValue = Registro_Pagos[Ope_Caj_Pagos_Detalles.Campo_Banco_ID].ToString();
                    //            Txt_No_Cheque.Text = Registro_Pagos[Ope_Caj_Pagos_Detalles.Campo_No_Cheque].ToString();
                    //            Txt_Monto_Cheque.Text = String.Format("{0:#0.00}", Convert.ToDecimal(Registro_Pagos[Ope_Caj_Pagos_Detalles.Campo_Monto].ToString()));
                    //        }
                    //        if (Registro_Pagos[Ope_Caj_Pagos_Detalles.Campo_Forma_Pago].ToString() == "TRANSFERENCIA")
                    //        {
                    //            Cmb_Bancos_Transferencia.SelectedValue = Registro_Pagos[Ope_Caj_Pagos_Detalles.Campo_Banco_ID].ToString();
                    //            Txt_Transferencia_Bancaria.Text = Registro_Pagos[Ope_Caj_Pagos_Detalles.Campo_Referencia_Transferencia].ToString();
                    //            Txt_Monto_Transferencia_Bancaria.Text = String.Format("{0:#0.00}", Convert.ToDecimal(Registro_Pagos[Ope_Caj_Pagos_Detalles.Campo_Monto].ToString()));
                    //        }
                    //    }
                    //}
                    Txt_Importe_Ingresos.Text = String.Format("{0:#,##0.00}", Total_Importe);

                    Decimales = Convert.ToDouble(String.Format("{0:#0.00}", Total_Importe - Math.Truncate(Total_Importe)));
                    if (Decimales <= 0.5)
                    {
                        Total_Pagar = Math.Floor(Total_Importe);
                    }
                    else
                    {
                        Total_Pagar = Math.Ceiling(Total_Importe);
                    }
                    Total_Ajuste_Tarifario = Total_Pagar - Total_Importe;

                    Txt_Ajuste_Tarifario.Text = String.Format("{0:#,##0.00}", Convert.ToDecimal(Total_Ajuste_Tarifario));
                    Txt_Total_Pagar_Ingreso.Text = String.Format("{0:#,##0.00}", Convert.ToDecimal(Total_Pagar));
                    //Datos para la impresión de recibo
                    if (Dt_Formas_Pago_Pasivo.Rows.Count > 0)
                    {
                        DataTable Dt_Generales_Recibo = new DataTable(); //Obtiene los datos generales del recibo que fue consultado y que se encuentra ya pagado
                        DataTable Dt_Detalles_Recibo = new DataTable();  //Obtiene los detalles del recibo que fue consultando y que se encuentra ya pagado

                        //Agrega los campos a contener para los detalles del recibo
                        Dt_Detalles_Recibo.Columns.Add("Ingreso", typeof(System.String));
                        Dt_Detalles_Recibo.Columns.Add("Descripcion", typeof(System.String));
                        Dt_Detalles_Recibo.Columns.Add("Importe", typeof(System.Decimal));
                        //Detalles del recibo de pago
                        DataRow Renglon;
                        foreach (DataRow General in Dt_Pasivo.Rows)
                        {
                            Renglon = Dt_Detalles_Recibo.NewRow();
                            Renglon["Descripcion"] = General[Ope_Ing_Pasivo.Campo_Descripcion].ToString();
                            Renglon["Ingreso"] = General["Ingreso"].ToString();
                            Renglon["Importe"] = Convert.ToDecimal(General[Ope_Ing_Pasivo.Campo_Monto]);
                            Dt_Detalles_Recibo.Rows.Add(Renglon);
                        }

                        //Agrega los campos a contener para los datos generales del recibo
                        Dt_Generales_Recibo.Columns.Add("Recargos", typeof(System.Decimal));
                        Dt_Generales_Recibo.Columns.Add("Total_Pagar", typeof(System.Decimal));
                        Dt_Generales_Recibo.Columns.Add("Nota_Pie", typeof(System.String));
                        Dt_Generales_Recibo.Columns.Add("Fecha_Pago", typeof(System.DateTime));
                        //Datos generales del recibo de pago
                        DataRow Detalles;
                        Detalles = Dt_Generales_Recibo.NewRow();
                        Detalles["Recargos"] = 0;
                        Detalles["Total_Pagar"] = Convert.ToDouble(Txt_Total_Pagar_Ingreso.Text.Replace("$", ""));
                        Detalles["Fecha_Pago"] = Fecha_Pago;
                        Detalles["Nota_Pie"] = Estatus_No_Operacion + String.Format("{0:yyyy.MM.dd}", Fecha_Pago) + "/" + String.Format("{0:HH:mm:ss}", Fecha_Pago) + "/" + Txt_Total_Pagar_Ingreso.Text.ToString() + "/" + Txt_No_Recibo_Pago.Text.ToString();
                        if (Hdn_Cuenta_Predial.Value != "")
                        {
                            Detalles["Nota_Pie"] += "/" + Hdn_Cuenta_Predial.Value;
                        }
                        Dt_Generales_Recibo.Rows.Add(Detalles);

                        Session["Generales_Recibo"] = Dt_Generales_Recibo;
                        Session["Detalles_Recibo"] = Dt_Detalles_Recibo;
                    }
                }
                if (Convert.ToString(Txt_Estatus_Ingresos.Text.Trim()) == "POR PAGAR")
                {
                    Habilitar_Controles("Nuevo");
                }
                else
                {
                    Habilitar_Controles("Inicial");
                    Obtiene_Total_Pagado(); //Obtiene el total de los montos proporcionados
                    if (Convert.ToString(Txt_Estatus_Ingresos.Text.Trim()) == "PAGADO") ScriptManager.RegisterStartupScript(this, this.GetType(), "Pago en Caja", "alert('Pago realizado.');", true);
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron datos con el Folio proporcionado";
            }
            //}

        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Datos_Pasivo " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Bancos_Ingresos
    /// DESCRIPCION : Consulta los bancos que son validos para ingresos y los agrega
    ///               a los combos correspondientes los valores encontrados
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 23-Agosto-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private String Consulta_Cuenta_Predial()
    {
        Cls_Ope_Caj_Pagos_Negocio Rs_Consulta_Cuenta_Predial = new Cls_Ope_Caj_Pagos_Negocio(); //Variable de conexión hacia la capa de Negocios

        Rs_Consulta_Cuenta_Predial.P_Cuenta_Predial_ID = Hdn_Cuenta_Predial_ID.Value;
        return Rs_Consulta_Cuenta_Predial.Consulta_Cuenta_Predial();
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Obtiene_Total_Pagado
    /// DESCRIPCION : Suma los montos proporcionados por el usuario de las diferentes
    ///              formas de pago para obtener el faltante o cambio a entregar
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 23-Agosto-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Obtiene_Total_Pagado()
    {
        DataTable Dt_Formas_Pago = new DataTable(); //Obtiene las formas de pago con las cuales fue cubierto el importe del ingreso
        Double Total_Pagado = 0;
        Double Cambio = 0;
        String Suma = "0";

        try
        {
            Dt_Formas_Pago = (DataTable)Session["CAJA_FORMAS_PAGO"];
            if (Dt_Formas_Pago != null)
            {
                if (Dt_Formas_Pago.Rows.Count > 0)
                {
                    Suma = Dt_Formas_Pago.Compute("SUM(Monto)", "").ToString();
                }
                Total_Pagado = Convert.ToDouble(Suma);
                Cambio = Total_Pagado - Convert.ToDouble(Txt_Total_Pagar_Ingreso.Text.ToString().Replace(",", ""));
                Txt_Total_Pagado.Text = String.Format("{0:#,##0.00}", Total_Pagado);
                Txt_Cambio_Ingresos.Text = String.Format("{0:#,##0.00}", Cambio);
                if (Cambio < 0)
                {
                    Lbl_Cambio.Text = "Faltante";
                }
                else
                {
                    Lbl_Cambio.Text = "Cambio";
                }
                if (Dt_Formas_Pago.Rows.Count == 0)
                {
                    Lbl_Cambio.Text = "Cambio";
                    Txt_Cambio_Ingresos.Text = "0.00";
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Obtiene_Total_Pagado " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Pago_Caja
    /// DESCRIPCION : Da de Alta el Pago con los datos proporcionados por el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 22-Agosto-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Pago_Caja()
    {
        DataTable Dt_Formas_Pago = new DataTable(); //Obtiene las formas de pago con las cuales fue cubierto el importe del ingreso
        DataRow Renglon;
        Cls_Ope_Caj_Pagos_Negocio Rs_Alta_Ope_Caj_Pagos = new Cls_Ope_Caj_Pagos_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            //Asigna la session de las formas de pago
            Dt_Formas_Pago = (DataTable)Session["CAJA_FORMAS_PAGO"];
            //Si tiene ajuste tarifario
            if (Convert.ToDouble(Txt_Ajuste_Tarifario.Text) != 0) //Ajuste tarfario
            {
                //Agrega los datos del ajuste tarifario
                Renglon = Dt_Formas_Pago.NewRow();
                Renglon["Forma_Pago"] = "AJUSTE TARIFARIO";
                Renglon["Monto"] = Convert.ToDouble(Txt_Ajuste_Tarifario.Text);
                Dt_Formas_Pago.Rows.Add(Renglon);
            }
            //Si tiene cambio por efectivo
            if (Convert.ToDouble(Txt_Cambio_Ingresos.Text) > 0) //Cambio efectivo
            {
                //Agrega los datos del ajuste tarifario
                Renglon = Dt_Formas_Pago.NewRow();
                Renglon["Forma_Pago"] = "CAMBIO";
                Renglon["Monto"] = Convert.ToDouble(Txt_Cambio_Ingresos.Text);
                Dt_Formas_Pago.Rows.Add(Renglon);
            }

            Rs_Alta_Ope_Caj_Pagos.P_No_Recibo = string.Format("{0:0000000000}", Convert.ToInt32(Txt_No_Recibo_Pago.Text));
            Rs_Alta_Ope_Caj_Pagos.P_Cuenta_Predial_ID = Hdn_Cuenta_Predial_ID.Value;
            Rs_Alta_Ope_Caj_Pagos.P_Cuenta_Predial = Hdn_Cuenta_Predial.Value;
            Rs_Alta_Ope_Caj_Pagos.P_Referencia = Txt_Busqueda_Referencia.Text;
            Rs_Alta_Ope_Caj_Pagos.P_Caja_ID = Txt_Caja_ID.Text;
            Rs_Alta_Ope_Caj_Pagos.P_No_Caja = Hdn_No_Caja.Value;
            Rs_Alta_Ope_Caj_Pagos.P_No_Turno = Txt_No_Turno.Text;
            Rs_Alta_Ope_Caj_Pagos.P_Fecha_Pago = Convert.ToDateTime(Txt_Fecha_Aplicacion.Text + " " + DateTime.Now.ToString("HH:mm:ss"));
            Rs_Alta_Ope_Caj_Pagos.P_Ajuste_Tarifario = Convert.ToDouble(Txt_Ajuste_Tarifario.Text.Replace("$", ""));
            Rs_Alta_Ope_Caj_Pagos.P_Total_Pagar = Convert.ToDouble(Txt_Total_Pagar_Ingreso.Text.Replace("$", ""));
            Rs_Alta_Ope_Caj_Pagos.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
            Rs_Alta_Ope_Caj_Pagos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            //Rs_Alta_Ope_Caj_Pagos.P_Monto_Recargos=Convert.ToDouble(Txt_Importe_Recargos.Text);
            Rs_Alta_Ope_Caj_Pagos.P_Monto_Corriente = Convert.ToDouble(Txt_Importe_Ingresos.Text);
            Rs_Alta_Ope_Caj_Pagos.P_Dt_Formas_Pago = Dt_Formas_Pago;
            if (Session["ADEUDO_PREDIAL_CAJA"] != null)
            {
                Rs_Alta_Ope_Caj_Pagos.P_Dt_Adeudos_Predial_Cajas = (DataTable)Session["ADEUDO_PREDIAL_CAJA"];
            }
            if (Session["ADEUDO_PREDIAL_DETALLES"] != null)
            {
                Rs_Alta_Ope_Caj_Pagos.P_Dt_Adeudos_Predial_Cajas_Detalle = (DataTable)Session["ADEUDO_PREDIAL_DETALLES"];
            }
            Rs_Alta_Ope_Caj_Pagos.Alta_Pago_Caja(); //Da de alta el pago del ingreso
            Hdn_No_Pago.Value = Rs_Alta_Ope_Caj_Pagos.P_No_Pago;
            Habilitar_Controles("Inicial");         //Habilita los controles para la siguiente operacion
            Txt_Estatus_Ingresos.Text = "PAGADO";
            Session.Remove("ADEUDO_PREDIAL_CAJA");
            Session.Remove("ADEUDO_PREDIAL_DETALLES");
            Session.Remove("CAJA_FORMAS_PAGO");
            Session.Remove("Generales_Recibo");
            Session.Remove("Detalles_Recibo");

            //Abrir_Ventana(Nombre_Archivo);
            if (Char.IsLetter(Txt_Busqueda_Referencia.Text.Trim(), 1))
            {
                Response.Redirect("Frm_Ope_Listado_Adeudos_Predial.aspx?Referencia=" + Txt_Busqueda_Referencia.Text.Trim() + "&No_Pago=" + Convert.ToInt32(Hdn_No_Pago.Value), true);
            }
            else
            {
                Response.Redirect("Frm_Ope_Listado_Adeudos_Predial.aspx?Referencia=" + Convert.ToInt32(Hdn_Cuenta_Predial_ID.Value) + "&No_Pago=" + Convert.ToInt32(Hdn_No_Pago.Value), true);
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Pago_Caja " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Agregar_Forma_Pago
    ///DESCRIPCIÓN: Agrega las formas de pago segun el pago
    ///PARÁMETROS : 
    ///CREO       : Ismael Prieto Sánchez
    ///FECHA_CREO : 29/Noviembre/2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private void Agregar_Forma_Pago()
    {
        DataTable Dt_Formas_Pago = new DataTable(); //Obtiene las formas de pago con las cuales fue cubierto el importe del ingreso
        DataRow Renglon;
        Double Monto = 0;
        String Referencia = "";
        Boolean Valida_Error = false;
        Boolean Encontro = false;
        Int32 No_Renglon = -1;

        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;

        //Valida si la session de las formas de pago ya se creo
        if (Session["CAJA_FORMAS_PAGO"] == null)
        {
            //Se realiza la estructura a contener los datos
            Dt_Formas_Pago.Columns.Add("Banco_ID", typeof(System.String));
            Dt_Formas_Pago.Columns.Add("Banco", typeof(System.String));
            Dt_Formas_Pago.Columns.Add("Forma_Pago", typeof(System.String));
            Dt_Formas_Pago.Columns.Add("No_Transaccion", typeof(System.String));
            Dt_Formas_Pago.Columns.Add("No_Autorizacion", typeof(System.String));
            Dt_Formas_Pago.Columns.Add("Plan_Pago", typeof(System.String));
            Dt_Formas_Pago.Columns.Add("Meses", typeof(System.Int16));
            Dt_Formas_Pago.Columns.Add("Monto", typeof(System.Double));
            Dt_Formas_Pago.Columns.Add("No_Cheque", typeof(System.String));
            Dt_Formas_Pago.Columns.Add("Referencia_Transferencia", typeof(System.String));
            Dt_Formas_Pago.Columns.Add("Referencia", typeof(System.String));
            Dt_Formas_Pago.Columns.Add("No_Tarjeta_Bancaria", typeof(System.String));
        }
        else
        {
            Dt_Formas_Pago = (DataTable)Session["CAJA_FORMAS_PAGO"];
        }

        //Valida el monto que sea numerico
        if (String.IsNullOrEmpty(Txt_Forma_Pago_Monto.Text.ToString()))
        {
            Monto = 0;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Debe de proporcionar el monto para el pago.";
            return;
        }
        else
        {
            Monto = Convert.ToDouble(Txt_Forma_Pago_Monto.Text.Replace("$", ""));
        }


        //Valida el monto pagado
        if (String.IsNullOrEmpty(Txt_Total_Pagado.Text.ToString())) Txt_Total_Pagado.Text = "0.00";
        if (String.IsNullOrEmpty(Txt_Cambio_Ingresos.Text.ToString())) Txt_Cambio_Ingresos.Text = Txt_Total_Pagar_Ingreso.Text;
        if (Monto > Math.Abs(Convert.ToDouble(Txt_Cambio_Ingresos.Text.Replace("$", ""))) && Lbl_Forma_Pago.Text != "EFECTIVO")
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "El monto no puede ser mayor al total pagado.";
            return;
        }

        //Agrega las forma de pago
        if (Lbl_Forma_Pago.Text == "EFECTIVO")
        {
            //Busca si es la misma forma de pago
            foreach (DataRow Registro in Dt_Formas_Pago.Rows)
            {
                No_Renglon += 1;
                if (Registro["Forma_Pago"].ToString() == "EFECTIVO")
                {
                    Encontro = true;
                    break;
                }
            }
            //Agrega los datos de la forma de pago en efectivo
            if (!Encontro)
            {
                Renglon = Dt_Formas_Pago.NewRow();
                Renglon["Forma_Pago"] = "EFECTIVO";
                Renglon["Monto"] = Monto;
                Dt_Formas_Pago.Rows.Add(Renglon);
            }
            else
            {
                Renglon = Dt_Formas_Pago.Rows[No_Renglon];
                Renglon.BeginEdit();
                Renglon["Monto"] = Convert.ToDouble(Renglon["Monto"].ToString()) + Monto;
                Renglon.EndEdit();
                Dt_Formas_Pago.AcceptChanges();
            }
        }
        if (Lbl_Forma_Pago.Text == "TARJETA")
        {
            Lbl_Mensaje_Error.Text = "Debe de proporcionar:</br>";
            if (Cmb_Forma_Pago_Bancos.SelectedItem.Value == "")
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;+El banco con el que se paga.";
                Valida_Error = true;
            }
            if (String.IsNullOrEmpty(Txt_Forma_Pago_Banco_Referencia.Text.Trim().ToString()))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;+El número de autorización del banco.";
                Valida_Error = true;
            }
            if (String.IsNullOrEmpty(Txt_Forma_Pago_Banco_No_Tarjeta.Text.Trim().ToString()))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;+El número de tarjeta del banco (Ultimos 4 digitos).";
                Valida_Error = true;
            }
            if (Valida_Error)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                return;
            }
            else
            {
                //Agrega los datos de la forma de pago de Bancos
                Renglon = Dt_Formas_Pago.NewRow();
                Renglon["Banco_ID"] = Cmb_Forma_Pago_Bancos.SelectedValue;
                Renglon["Banco"] = Cmb_Forma_Pago_Bancos.SelectedItem.Text;
                Renglon["Forma_Pago"] = "BANCO";
                Renglon["No_Transaccion"] = "";
                Renglon["No_Autorizacion"] = Txt_Forma_Pago_Banco_Referencia.Text;
                Renglon["No_Tarjeta_Bancaria"] = Txt_Forma_Pago_Banco_No_Tarjeta.Text;
                if (Cmb_Plan_Pago_Bancario.SelectedValue.ToString() != "")
                {
                    if (Convert.ToInt16(Cmb_Plan_Pago_Bancario.SelectedValue.ToString()) <= 1)
                    {
                        Renglon["Plan_Pago"] = "NORMAL";
                    }
                    else
                    {
                        Renglon["Plan_Pago"] = "MESES SIN INTERESES";
                    }
                }
                else
                {
                    Renglon["Plan_Pago"] = "MESES SIN INTERESES";
                }
                Referencia += Renglon["Plan_Pago"];
                if (Cmb_Plan_Pago_Bancario.SelectedValue.ToString() != "")
                {
                    Renglon["Meses"] = Convert.ToInt16(Cmb_Plan_Pago_Bancario.SelectedValue.ToString());
                    Referencia += "/" + Renglon["Meses"];
                }
                else
                {
                    Renglon["Meses"] = 0;
                }
                Referencia += "/" + Renglon["No_Autorizacion"];
                Renglon["Referencia"] = Referencia;
                Renglon["Monto"] = Monto;
                Dt_Formas_Pago.Rows.Add(Renglon);
            }
        }
        if (Lbl_Forma_Pago.Text == "CHEQUE")
        {
            Lbl_Mensaje_Error.Text = "Debe de proporcionar:</br>";
            if (Cmb_Forma_Pago_Bancos.SelectedItem.Value == "")
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;+El banco con el que se paga.";
                Valida_Error = true;
            }
            if (String.IsNullOrEmpty(Txt_Forma_Pago_Banco_Referencia.Text.Trim().ToString()))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;+El número de cheque del banco.";
                Valida_Error = true;
            }
            if (Valida_Error)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                return;
            }
            else
            {
                //Agrega los datos de la forma de pago de Cheque
                Renglon = Dt_Formas_Pago.NewRow();
                Renglon["Banco_ID"] = Cmb_Forma_Pago_Bancos.SelectedValue;
                Renglon["Banco"] = Cmb_Forma_Pago_Bancos.SelectedItem.Text;
                Renglon["Forma_Pago"] = "CHEQUE";
                Renglon["Referencia"] = "CHQ-" + Txt_Forma_Pago_Banco_Referencia.Text;
                Renglon["No_Cheque"] = Txt_Forma_Pago_Banco_Referencia.Text;
                Renglon["Monto"] = Monto;
                Dt_Formas_Pago.Rows.Add(Renglon);
            }
        }
        if (Lbl_Forma_Pago.Text == "TRANSFERENCIA")
        {
            Lbl_Mensaje_Error.Text = "Debe de proporcionar:</br>";
            if (Cmb_Forma_Pago_Bancos.SelectedItem.Value == "")
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;+El banco con el que se paga.";
                Valida_Error = true;
            }
            if (String.IsNullOrEmpty(Txt_Forma_Pago_Banco_Referencia.Text.Trim().ToString()))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;+El número de cheque del banco.";
                Valida_Error = true;
            }
            if (Valida_Error)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                return;
            }
            else
            {
                //Agrega los datos de la forma de pago de Transferencia Bancaria
                Renglon = Dt_Formas_Pago.NewRow();
                Renglon["Forma_Pago"] = "TRANSFERENCIA";
                Renglon["Banco"] = Cmb_Forma_Pago_Bancos.SelectedItem.Text;
                Renglon["Banco_ID"] = Cmb_Forma_Pago_Bancos.SelectedValue;
                Renglon["Referencia_Transferencia"] = Txt_Forma_Pago_Banco_Referencia.Text;
                Renglon["Referencia"] = "TRA-" + Txt_Forma_Pago_Banco_Referencia.Text;
                Renglon["Monto"] = Monto;
                Dt_Formas_Pago.Rows.Add(Renglon);
            }
        }
        //Asigna la session
        Session["CAJA_FORMAS_PAGO"] = Dt_Formas_Pago;
        //Calcula el total
        Obtiene_Total_Pagado();
        //Asigna al grid
        Grid_Formas_Pago.DataSource = Dt_Formas_Pago;
        Grid_Formas_Pago.DataBind();
        //Limpia los montos
        Lbl_Forma_Pago.Text = "EFECTIVO";
        Lbl_Forma_Pago_Banco.Visible = false;
        Cmb_Forma_Pago_Bancos.Visible = false;
        Lbl_Forma_Pago_Referencia.Visible = false;
        Txt_Forma_Pago_Banco_Referencia.Visible = false;
        Txt_Forma_Pago_Banco_No_Tarjeta.Visible = false;
        Lbl_Forma_Pago_Plan_Pago.Visible = false;
        Cmb_Plan_Pago_Bancario.Visible = false;
        Cmb_Forma_Pago_Bancos.SelectedIndex = 0;
        Txt_Forma_Pago_Banco_Referencia.Text = "";
        Txt_Forma_Pago_Monto.Text = "";
        Txt_Forma_Pago_Monto.Focus();
    }
    #endregion
    #region (Eventos)
    protected void Txt_Busqueda_Referencia_TextChanged(object sender, EventArgs e)
    {
        String Folio; //Contiene el folio que se pretende consultar
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Folio = Txt_Busqueda_Referencia.Text;
            Limpia_Controles(); //Limpia los controles de la forma
            Habilitar_Controles("Inicial"); //Habilita los controles para las operaciones a realizar
            Txt_Busqueda_Referencia.Text = Folio;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        String Ruta_Archivo = @Server.MapPath("../Rpt/Predial/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Pago_Recibo" + Session.SessionID + Convert.ToString(String.Format("{0:ddMMMyyyHHmmss}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
        DataRow Registro; //Obtiene los valores de la consulta realizada para la impresión del recibo
        Ds_Rpt_Caj_Recibo_Cobro Ds_Recibo_Cobro = new Ds_Rpt_Caj_Recibo_Cobro();

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            DataTable Dt_Generales_Recibo = (DataTable)Session["Generales_Recibo"];
            DataTable Dt_Detalles_Recibo = (DataTable)Session["Detalles_Recibo"];

            // Se llena la cabecera del DataSet                
            Registro = Dt_Generales_Recibo.Rows[0];
            Ds_Recibo_Cobro.Tables["Recibo_Cobro"].ImportRow(Registro);

            if (Dt_Detalles_Recibo.Rows.Count > 0)
            {
                // Llenar los documentos del DataSet
                for (int Elementos = 0; Elementos < Dt_Detalles_Recibo.Rows.Count; Elementos++)
                {
                    Registro = Dt_Detalles_Recibo.Rows[Elementos]; // Instanciar renglon e importarlo
                    Ds_Recibo_Cobro.Tables["Detalles_Recibo"].ImportRow(Registro);
                }
            }

            ReportDocument Reporte = new ReportDocument();
            Reporte.Load(Ruta_Archivo + "Rpt_Ope_Caj_Pagos.rpt");
            Reporte.SetDataSource(Ds_Recibo_Cobro);
            DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();

            Nombre_Archivo += ".pdf";
            Ruta_Archivo = @Server.MapPath("../../Reporte/");
            m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

            ExportOptions Opciones_Exportacion = new ExportOptions();
            Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
            Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
            Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Opciones_Exportacion);
            Session.Remove("Generales_Recibo");
            Session.Remove("Detalles_Recibo");
            //Abrir_Ventana("../../Reporte/" + Nombre_Archivo);
            //Abrir_Ventana(Nombre_Archivo);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Cmb_Bancos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Caj_Pagos_Negocio Rs_Consulta_Cat_Nom_Bancos = new Cls_Ope_Caj_Pagos_Negocio(); //Veriable para la capa de negocios
        DataTable Dt_Bancos; //Variable a contener los datos de la consulta realizada
        try
        {
            Cmb_Plan_Pago_Bancario.DataSource = new DataTable();
            Cmb_Plan_Pago_Bancario.DataBind();
            Cmb_Plan_Pago_Bancario.Items.Insert(0, new ListItem("NORMAL", "0"));
            if (Cmb_Forma_Pago_Bancos.SelectedIndex > 0)
            {
                Rs_Consulta_Cat_Nom_Bancos.P_Banco_ID = Cmb_Forma_Pago_Bancos.SelectedValue;
                Dt_Bancos = Rs_Consulta_Cat_Nom_Bancos.Consulta_Plan_Pago_Banco();//Consulta los datos generales del banco que fue seleccionado por el usuario
                if (Dt_Bancos.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Dt_Bancos.Rows)
                    {
                        if (!String.IsNullOrEmpty(Registro[Cat_Nom_Bancos.Campo_Plan_Pago].ToString()))
                        {
                            if (Registro[Cat_Nom_Bancos.Campo_Plan_Pago].ToString() == "MESES SIN INTERESES") Cmb_Plan_Pago_Bancario.Items.Insert(1, new ListItem(Registro[Cat_Nom_Bancos.Campo_No_Meses].ToString() + " MESES S/INTERESES", Registro[Cat_Nom_Bancos.Campo_No_Meses].ToString()));
                        }
                    }
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
    protected void Btn_Buscar_Referencia_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (!String.IsNullOrEmpty(Txt_Busqueda_Referencia.Text))
            {
                Consulta_Datos_Pasivo("POR PAGAR", ""); //Consulta los datos de la referencia que proporciono el empleado
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Debe proporcionar el Folio del Ingreso";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (!String.IsNullOrEmpty(Txt_Busqueda_Referencia.Text))
            {
                if (Txt_Estatus_Ingresos.Text.Trim() == "POR PAGAR")
                {
                    if (String.IsNullOrEmpty(Txt_Forma_Pago_Monto.Text)) Txt_Forma_Pago_Monto.Text = "0.00";
                    if (String.IsNullOrEmpty(Txt_Total_Pagado.Text)) Txt_Total_Pagado.Text = "0.00";
                    Obtiene_Total_Pagado(); //Obtiene el total de los montos proporcionados
                    //Si ya se cubrio la cantidad total a pagar entonces da de alta el ingreso del cobro
                    if (Convert.ToDecimal(Txt_Total_Pagar_Ingreso.Text.ToString().Replace(",", "")) <= Convert.ToDecimal(Txt_Total_Pagado.Text.ToString().Replace(",", "")))
                    {
                        Alta_Pago_Caja(); //Da de alta el pago del ingreso
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Debe de proporcionar la forma y monto de pago correcto.";
                    }
                }
                else
                {
                    if (Txt_Estatus_Ingresos.Text.Trim() == "PAGADO")
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "El Folio ya se encuentra pagado";
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "El Folio se encuentra cancelado";
                    }
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Debe proporcionar el Folio del Ingreso";
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
            //if (Btn_Salir.ToolTip == "Salir")
            //{
            //    Session.Remove("Generales_Recibo");
            //    Session.Remove("Detalles_Recibo");
            //    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            //}
            //else
            //{
            //    Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario en el catálogo
            //}
            if (char.IsLetter(Txt_Busqueda_Referencia.Text, 1))
            {
                Response.Redirect("../Predial/Frm_Ope_Listado_Adeudos_Predial.aspx");
            }
            else
            {
                Session["CUENTA_ADEUDO_PREDIAL"] = Txt_Busqueda_Referencia.Text;
                Response.Redirect("../Predial/Frm_Ope_Pre_Recepcion_Pagos_Predial.aspx");
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Img_Btn_Agregar_Forma_Pago_Click(object sender, ImageClickEventArgs e)
    {
        Agregar_Forma_Pago();
    }
    protected void Btn_Pago_Efectivo_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Forma_Pago.Text = "EFECTIVO";
        Lbl_Forma_Pago_Banco.Visible = false;
        Cmb_Forma_Pago_Bancos.Visible = false;
        Lbl_Forma_Pago_Referencia.Visible = false;
        Txt_Forma_Pago_Banco_Referencia.Visible = false;
        Txt_Forma_Pago_Banco_No_Tarjeta.Visible = false;
        Lbl_Forma_Pago_Plan_Pago.Visible = false;
        Cmb_Plan_Pago_Bancario.Visible = false;
    }
    protected void Btn_Pago_Banco_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Forma_Pago.Text = "TARJETA";
        Lbl_Forma_Pago_Referencia.Text = "No. Aut./No. Tarjeta";
        Lbl_Forma_Pago_Banco.Visible = true;
        Cmb_Forma_Pago_Bancos.Visible = true;
        Lbl_Forma_Pago_Referencia.Visible = true;
        Txt_Forma_Pago_Banco_Referencia.Visible = true;
        Txt_Forma_Pago_Banco_No_Tarjeta.Visible = true;
        Txt_Forma_Pago_Banco_Referencia.Style.Add("width", "60%");
        Txt_Forma_Pago_Banco_No_Tarjeta.Style.Add("width", "30%");
        Lbl_Forma_Pago_Plan_Pago.Visible = true;
        Cmb_Plan_Pago_Bancario.Visible = true;
    }
    protected void Btn_Pago_Cheque_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Forma_Pago.Text = "CHEQUE";
        Lbl_Forma_Pago_Referencia.Text = "No. Cheque";
        Lbl_Forma_Pago_Banco.Visible = true;
        Cmb_Forma_Pago_Bancos.Visible = true;
        Lbl_Forma_Pago_Referencia.Visible = true;
        Txt_Forma_Pago_Banco_Referencia.Visible = true;
        Txt_Forma_Pago_Banco_No_Tarjeta.Visible = false;
        Txt_Forma_Pago_Banco_Referencia.Style.Add("width", "97%");
        Txt_Forma_Pago_Banco_No_Tarjeta.Style.Add("width", "0%");
        Lbl_Forma_Pago_Plan_Pago.Visible = false;
        Cmb_Plan_Pago_Bancario.Visible = false;
    }
    protected void Btn_Pago_Transferencia_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Forma_Pago.Text = "TRANSFERENCIA";
        Lbl_Forma_Pago_Referencia.Text = "Referencia";
        Lbl_Forma_Pago_Banco.Visible = true;
        Cmb_Forma_Pago_Bancos.Visible = true;
        Lbl_Forma_Pago_Referencia.Visible = true;
        Txt_Forma_Pago_Banco_Referencia.Visible = true;
        Txt_Forma_Pago_Banco_No_Tarjeta.Visible = false;
        Txt_Forma_Pago_Banco_Referencia.Style.Add("width", "97%");
        Txt_Forma_Pago_Banco_No_Tarjeta.Style.Add("width", "0%");
        Lbl_Forma_Pago_Plan_Pago.Visible = false;
        Cmb_Plan_Pago_Bancario.Visible = false;
    }
    protected void Grid_Formas_Pago_RowDataCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Eliminar":
                DataTable Dt_Formas_Pago = new DataTable();
                int index = Convert.ToInt32(e.CommandArgument);
                Dt_Formas_Pago = (DataTable)Session["CAJA_FORMAS_PAGO"];
                Dt_Formas_Pago.Rows.RemoveAt(index);
                Session["CAJA_FORMAS_PAGO"] = Dt_Formas_Pago;
                //Asigna al grid
                Grid_Formas_Pago.DataSource = Dt_Formas_Pago;
                Grid_Formas_Pago.DataBind();
                //Calcula el total
                Obtiene_Total_Pagado();
                Txt_Forma_Pago_Monto.Focus();
                break;
        }
    }
    #endregion

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
    ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private String Obtener_Dato_Consulta(String Campo, String Tabla, String Condiciones)
    {
        String Mi_SQL;
        String Dato_Consulta = "";

        try
        {
            Mi_SQL = "SELECT " + Campo;
            if (Tabla != "")
            {
                Mi_SQL += " FROM " + Tabla;
            }
            if (Condiciones != "")
            {
                Mi_SQL += " WHERE " + Condiciones;
            }

            OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Dr_Dato.Read())
            {
                if (Dr_Dato[0] != null)
                {
                    Dato_Consulta = Dr_Dato[0].ToString();
                }
                else
                {
                    Dato_Consulta = "";
                }
                Dr_Dato.Close();
            }
            else
            {
                Dato_Consulta = "";
            }
            if (Dr_Dato != null)
            {
                Dr_Dato.Close();
            }
            Dr_Dato = null;
        }
        catch
        {
        }
        finally
        {
        }

        return Dato_Consulta;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Txt_Forma_Pago_Monto_TextChanged
    /// DESCRIPCION             : Evento TextChange de TextBox que ejecuta evento Click de Botón.
    /// PARAMETROS: 
    /// CREO                    : Antonio Salvador Benavides Guardado
    /// FECHA_CREO              : 03/Febrero/2012
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Forma_Pago_Monto_TextChanged(object sender, EventArgs e)
    {
        Img_Btn_Agregar_Forma_Pago_Click(sender, null);
    }
}
