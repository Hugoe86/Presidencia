using System;
using System.Collections;
using System.Collections.Generic;
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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using Presidencia.Reportes;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Menus.Negocios;
using Presidencia.Operaciones_Apertura_Turnos.Negocio;
using Presidencia.Caja_Pagos.Negocio;

public partial class paginas_Predial_Frm_Ope_Caj_Cierre_Turno : System.Web.UI.Page
{
    #region Page Load
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
    #region Metodos
    #region Metodos Generales
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 21-Octubre-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial");   //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Limpia_Controles(true);           //Limpia los controles del forma
            Consulta_Cajero_General();        //Consulta el nombre del cajero principal  
            Consulta_Datos_Generales_Turno(); //Consulta todos los datos del último turno que fue abierto por el empleado
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : Incializa: Trae valor de verdadero si se va a inicilizar la forma
    ///                          con respecto a si es la primera vez que se realiza la
    ///                          consulta de los valores
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 22-Octubre-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles(Boolean Inicializa)
    {
        try
        {
            if (Inicializa == true)
            {
                //Datos generales del empleado y caja
                Txt_No_Empleado.Text = "";
                Txt_Nombre_Empleado.Text = "";
                Hdn_Caja_ID.Value = "";
                Txt_Caja_Empleado.Text = "";
                Txt_Modulo_Caja_Empleado.Text = "";
                Txt_Nombre_Recibe_Efectivo.Text = "";
                //Datos generales del turno
                Txt_No_Turno.Text = "";
                Txt_Estatus_Turno.Text = "";
                Txt_Fecha_Apertura_Turno.Text = "";
                Txt_Hora_Apertura_Turno.Text = "";
                Txt_Fecha_Aplicacion_Turno.Text = "";
                Txt_Fecha_Cierre_Turno.Text = "";
                Txt_Hora_Cierre_Turno.Text = "";
                Txt_Fondo_Inicial_Turno.Text = "";
                Txt_Recibo_Inicial_Turno.Text = "";
                Txt_Recibo_Final_Turno.Text = "";
                //Datos de total recaudado
                Hdn_Efectivo_Sistema.Value = "0.00";
                Hdn_No_Recolecciones.Value = "0";
                Txt_Total_Cortes_Parciales.Text = "0.00";
                Txt_Parcial_Conteo_Bancos.Text = "0";
                Txt_Parcial_Total_Bancos.Text = "0.00";
                Txt_Parcial_Conteo_Cheques.Text = "0";
                Txt_Parcial_Total_Cheques.Text = "0.00";
                Txt_Parcial_Conteo_Transferencias.Text = "0";
                Txt_Parcial_Total_Transferencias.Text = "0.00";
                Txt_Conteo_Bancos.Text = "0";
                Txt_Total_Bancos.Text = "0.00";
                Txt_Conteo_Cheques.Text = "0";
                Txt_Total_Cheques.Text = "0.00";
                Txt_Conteo_Transferencias.Text = "0";
                Txt_Total_Transferencia.Text = "0.00";
            }
            //Datos de total recaudado
            Hdn_Efectivo_Caja.Value = "0.00";
            Txt_Total_Efectivo.Text = "0.00";
            Txt_Sobrante_Faltante.Text = "0.00";
            //Controles Billetes
            Txt_Billetes_1000.Text = "";
            Txt_Billetes_500.Text = "";
            Txt_Billetes_100.Text = "";
            Txt_Billetes_200.Text = "";
            Txt_Billetes_50.Text = "";
            Txt_Billetes_20.Text = "";
            //Controles Monedas            
            Txt_Monedas_20.Text = "";
            Txt_Monedas_10.Text = "";
            Txt_Monedas_5.Text = "";
            Txt_Monedas_2.Text = "";
            Txt_Monedas_1.Text = "";
            Txt_Monedas_050.Text = "";
            Txt_Monedas_020.Text = "";
            Txt_Monedas_010.Text = "";
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
    ///                          si es una modificacion
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 22-Octubre-2011
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
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Imprimir.ToolTip = "Imprimir";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Imprimir.Visible = true;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Configuracion_Acceso("Frm_Ope_Caj_Cierre_Turno.aspx");
                    break;

                case "Modficar":
                    Habilitado = true;
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Modificar.Visible = true;
                    Btn_Imprimir.Visible = false;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    break;
            }
            //Habilitación de controles generale
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //Habilitacion de Controles Billetes
            Txt_Billetes_1000.Enabled = Habilitado;
            Txt_Billetes_500.Enabled = Habilitado;
            Txt_Billetes_100.Enabled = Habilitado;
            Txt_Billetes_200.Enabled = Habilitado;
            Txt_Billetes_50.Enabled = Habilitado;
            Txt_Billetes_20.Enabled = Habilitado;
            //Habilitación de Controles Monedas            
            Txt_Monedas_20.Enabled = Habilitado;
            Txt_Monedas_10.Enabled = Habilitado;
            Txt_Monedas_5.Enabled = Habilitado;
            Txt_Monedas_2.Enabled = Habilitado;
            Txt_Monedas_1.Enabled = Habilitado;
            Txt_Monedas_050.Enabled = Habilitado;
            Txt_Monedas_020.Enabled = Habilitado;
            Txt_Monedas_010.Enabled = Habilitado;
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
            Botones.Add(Btn_Modificar);

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
    #region (Operaciones)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Datos_Generales_Turno
    /// DESCRIPCION : Consulta los datos del último turno que fue abierto por el empleado
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 23-Octubre-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Datos_Generales_Turno()
    {
        Double Monto_Total = 0;                          //Indica la suma total de los registrado en caja
        Double Total_Efectivo_Sistema = 0;               //Indica el monto en efectivo menos el cambio
        DataTable Dt_Formas_Pago_Turno = new DataTable(); //Obtiene las formas de pago que tuvo el turno
        DataTable Dt_Datos_Turno = new DataTable();       //Obtiene los valores de la consulta
        Cls_Ope_Pre_Apertura_Turno_Negocio Rs_Consulta_Ope_Caj_Turnos = new Cls_Ope_Pre_Apertura_Turno_Negocio(); //Variable de conexion hacia la capa de negocio
        try
        {
            Rs_Consulta_Ope_Caj_Turnos.P_Estatus = "ABIERTO";
            Rs_Consulta_Ope_Caj_Turnos.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
            Dt_Datos_Turno = Rs_Consulta_Ope_Caj_Turnos.Consultar_Datos_Turno(); //Consulta los datos del último turno que fue abierto por el empleado
            if (Dt_Datos_Turno.Rows.Count <= 0)
            {
                Rs_Consulta_Ope_Caj_Turnos.P_Estatus = "";
                Dt_Datos_Turno = new DataTable();
                Dt_Datos_Turno = Rs_Consulta_Ope_Caj_Turnos.Consultar_Datos_Turno();
            }
            if (Dt_Datos_Turno.Rows.Count > 0)
            {
                Lbl_Sobrante_Faltante.Visible = false;
                Txt_Sobrante_Faltante.Visible = false;
                Hdn_Efectivo_Sistema.Value = "0.00";
                Hdn_Efectivo_Caja.Value = "0.00";
                Txt_Total_Bancos.Text = "0.00";
                Txt_Total_Cheques.Text = "0.00";
                Txt_Total_Transferencia.Text = "0.00";
                Txt_Total_Cortes_Parciales.Text = "0.00";
                Txt_Total_Efectivo.Text = "0.00";
                //Agrega los valores de la consulta en los campos correspondientes
                foreach (DataRow Registro in Dt_Datos_Turno.Rows)
                {
                    Txt_No_Empleado.Text = Cls_Sessiones.No_Empleado;
                    Txt_Nombre_Empleado.Text = Cls_Sessiones.Nombre_Empleado;

                    Hdn_Caja_ID.Value = Registro[Ope_Caj_Turnos.Campo_Caja_ID].ToString();
                    Txt_Caja_Empleado.Text = Registro["Caja"].ToString();

                    if (!String.IsNullOrEmpty(Registro["Modulo"].ToString()))
                        Txt_Modulo_Caja_Empleado.Text = Registro["Modulo"].ToString();

                    Txt_No_Turno.Text = Registro[Ope_Caj_Turnos.Campo_No_Turno].ToString();

                    Txt_Estatus_Turno.Text = Registro[Ope_Caj_Turnos.Campo_Estatus].ToString();

                    if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos.Campo_Aplicacion_Pago].ToString()))
                        Txt_Fecha_Aplicacion_Turno.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Ope_Caj_Turnos.Campo_Aplicacion_Pago].ToString()));

                    if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos.Campo_Fecha_Turno].ToString()))
                        Txt_Fecha_Apertura_Turno.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Ope_Caj_Turnos.Campo_Fecha_Turno].ToString()));

                    if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos.Campo_Fondo_Inicial].ToString()))
                        Txt_Fondo_Inicial_Turno.Text = String.Format("{0:###,##0.00}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Fondo_Inicial].ToString()));

                    if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos.Campo_Hora_Apertura].ToString()))
                        Txt_Hora_Apertura_Turno.Text = String.Format("{0:T}", Convert.ToDateTime(Registro[Ope_Caj_Turnos.Campo_Hora_Apertura].ToString()));

                    if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos.Campo_Recibo_Inicial].ToString()))
                        Txt_Recibo_Inicial_Turno.Text = Registro[Ope_Caj_Turnos.Campo_Recibo_Inicial].ToString();

                    Rs_Consulta_Ope_Caj_Turnos.P_No_Turno = Txt_No_Turno.Text.ToString();
                    //Consulta los montos y formas de pago registrado durante el turno
                    if (Txt_Estatus_Turno.Text == "ABIERTO")
                    {
                        Dt_Formas_Pago_Turno = Rs_Consulta_Ope_Caj_Turnos.Consultar_Formas_Pago_Turno(); //Consulta el monto total que fue pago en las diferentes formas de pago
                        foreach (DataRow Renglon in Dt_Formas_Pago_Turno.Rows)
                        {
                            if (Renglon[Ope_Caj_Pagos_Detalles.Campo_Forma_Pago].ToString() == "EFECTIVO") Total_Efectivo_Sistema = Total_Efectivo_Sistema + Convert.ToDouble(Renglon["Total_Pagado"].ToString());
                            if (Renglon[Ope_Caj_Pagos_Detalles.Campo_Forma_Pago].ToString() == "CAMBIO") Total_Efectivo_Sistema = Total_Efectivo_Sistema - Convert.ToInt64(Convert.ToDouble(Renglon["Total_Pagado"].ToString()));                                
                            if (Renglon[Ope_Caj_Pagos_Detalles.Campo_Forma_Pago].ToString() == "BANCO")
                            {
                                Txt_Conteo_Bancos.Text = String.Format("{0:##0}", Convert.ToDouble(Renglon["Conteo"].ToString()));
                                Txt_Total_Bancos.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Renglon["Total_Pagado"].ToString()));
                            }
                            if (Renglon[Ope_Caj_Pagos_Detalles.Campo_Forma_Pago].ToString() == "CHEQUE")
                            {
                                Txt_Conteo_Cheques.Text = String.Format("{0:##0}", Convert.ToDouble(Renglon["Conteo"].ToString()));
                                Txt_Total_Cheques.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Renglon["Total_Pagado"].ToString()));
                            }
                            if (Renglon[Ope_Caj_Pagos_Detalles.Campo_Forma_Pago].ToString() == "TRANSFERENCIA")
                            {
                                Txt_Conteo_Transferencias.Text = String.Format("{0:##0}", Convert.ToDouble(Renglon["Conteo"].ToString()));
                                Txt_Total_Transferencia.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Renglon["Total_Pagado"].ToString()));
                            }
                        }
                        Hdn_Efectivo_Sistema.Value = Total_Efectivo_Sistema.ToString();
                        Dt_Formas_Pago_Turno = new DataTable();
                        Dt_Formas_Pago_Turno = Rs_Consulta_Ope_Caj_Turnos.Consultar_Monto_Recolectado(); //Consulta_Datos_Generales_Turno le monto total recolectado durante el turno
                        if (Dt_Formas_Pago_Turno.Rows.Count > 0)
                        {
                            foreach (DataRow Recoleccion in Dt_Formas_Pago_Turno.Rows)
                            {
                                Hdn_No_Recolecciones.Value = Recoleccion["No_Recolecciones"].ToString();
                                Txt_Total_Cortes_Parciales.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Recoleccion["Total_Recoleccion"].ToString()));
                                Monto_Total = Convert.ToDouble(Hdn_Efectivo_Sistema.Value) - Convert.ToDouble(Recoleccion["Total_Recoleccion"].ToString());
                                Hdn_Efectivo_Sistema.Value = Monto_Total.ToString();

                                Txt_Parcial_Conteo_Bancos.Text = Recoleccion["Conteo_Tarjeta"].ToString();
                                Txt_Parcial_Total_Bancos.Text = Recoleccion["Total_Tarjeta"].ToString();
                                Txt_Parcial_Conteo_Cheques.Text = Recoleccion["Conteo_Cheques"].ToString();
                                Txt_Parcial_Total_Cheques.Text = Recoleccion["Total_Cheques"].ToString();
                                Txt_Parcial_Conteo_Transferencias.Text = Recoleccion["Conteo_Transferncias"].ToString();
                                Txt_Parcial_Total_Transferencias.Text = Recoleccion["Total_Transferencias"].ToString();
                            }
                        }
                        else
                        {
                            Txt_Parcial_Conteo_Bancos.Text = "0";
                            Txt_Parcial_Total_Bancos.Text = "0.00";
                            Txt_Parcial_Conteo_Cheques.Text = "0";
                            Txt_Parcial_Total_Cheques.Text = "0.00";
                            Txt_Parcial_Conteo_Transferencias.Text = "0";
                            Txt_Parcial_Total_Transferencias.Text = "0.00";
                        }
                        Txt_Conteo_Bancos.Text = String.Format("{0:##0}", Convert.ToInt32(Txt_Conteo_Bancos.Text) - Convert.ToInt32(Txt_Parcial_Conteo_Bancos.Text));
                        Txt_Total_Bancos.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Txt_Total_Bancos.Text) - Convert.ToDouble(Txt_Parcial_Total_Bancos.Text));
                        Txt_Conteo_Cheques.Text = String.Format("{0:##0}", Convert.ToInt32(Txt_Conteo_Cheques.Text) - Convert.ToInt32(Txt_Parcial_Conteo_Cheques.Text));
                        Txt_Total_Cheques.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Txt_Total_Cheques.Text) - Convert.ToDouble(Txt_Parcial_Total_Cheques.Text));
                        Txt_Conteo_Transferencias.Text = String.Format("{0:##0}", Convert.ToInt32(Txt_Conteo_Transferencias.Text) - Convert.ToInt32(Txt_Parcial_Conteo_Transferencias.Text));
                        Txt_Total_Transferencia.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Txt_Total_Transferencia.Text) - Convert.ToDouble(Txt_Parcial_Total_Transferencias.Text));

                        Txt_Parcial_Conteo_Bancos.Text = String.Format("{0:##0}", Convert.ToInt32(Txt_Parcial_Conteo_Bancos.Text));
                        Txt_Parcial_Total_Bancos.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Txt_Parcial_Total_Bancos.Text));
                        Txt_Parcial_Conteo_Cheques.Text = String.Format("{0:##0}", Convert.ToInt32(Txt_Parcial_Conteo_Cheques.Text));
                        Txt_Parcial_Total_Cheques.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Txt_Parcial_Total_Cheques.Text));
                        Txt_Parcial_Conteo_Transferencias.Text = String.Format("{0:##0}", Convert.ToInt32(Txt_Parcial_Conteo_Transferencias.Text));
                        Txt_Parcial_Total_Transferencias.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Txt_Parcial_Total_Transferencias.Text));

                        Rs_Consulta_Ope_Caj_Turnos.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                        Rs_Consulta_Ope_Caj_Turnos.P_No_Turno = Txt_No_Turno.Text;
                        Dt_Formas_Pago_Turno = Rs_Consulta_Ope_Caj_Turnos.Consulta_Datos_Turno_Empleado(); //Consulta el último recibo que fue generado por el empleado
                        if (Dt_Formas_Pago_Turno.Rows.Count > 0)
                        {
                            foreach (DataRow Recibo in Dt_Formas_Pago_Turno.Rows)
                            {
                                if (!String.IsNullOrEmpty(Recibo["ULTIMO_FOLIO_TURNO"].ToString())) Txt_Recibo_Final_Turno.Text = Recibo["ULTIMO_FOLIO_TURNO"].ToString();
                            }
                        }
                        Habilitar_Controles("Modficar");
                        Txt_Fecha_Cierre_Turno.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);
                    }
                    //Asigna a los controles el valor correspondientes que fue registrado en el cierre de turno como es los totales y denominaciones
                    else
                    {
                        Txt_Recibo_Final_Turno.Text = Registro[Ope_Caj_Turnos.Campo_Recibo_Final].ToString();
                        Txt_Fecha_Cierre_Turno.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Ope_Caj_Turnos.Campo_Fecha_Cierre].ToString()));
                        Txt_Hora_Cierre_Turno.Text = String.Format("{0:T}", Convert.ToDateTime(Registro[Ope_Caj_Turnos.Campo_Hora_Cierre].ToString()));
                        Txt_Nombre_Recibe_Efectivo.Text = Registro[Ope_Caj_Turnos.Campo_Nombre_Recibe].ToString();

                        Btn_Modificar.Visible = false;
                        Monto_Total = 0;
                        Monto_Total = Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Total_Efectivo_Caja].ToString());
                        Monto_Total += Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Total_Bancos].ToString());
                        Monto_Total += Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Total_Cheques].ToString());
                        Monto_Total += Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Total_Transferencias].ToString());
                        Monto_Total += Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Total_Recolectado].ToString());
                        Monto_Total += Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Total_Recolectado_Bancos].ToString());
                        Monto_Total += Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Total_Recolectado_Cheques].ToString());
                        Monto_Total += Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Total_Recolectado_Transferencias].ToString());

                        Hdn_Efectivo_Sistema.Value = String.Format("{0:#,##0.00}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Total_Efectivo_Sistema].ToString()));
                        Txt_Total_Efectivo.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Total_Efectivo_Caja].ToString()));
                        Txt_Conteo_Bancos.Text = String.Format("{0:#0}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Conteo_Bancos].ToString()));
                        Txt_Total_Bancos.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Total_Bancos].ToString()));
                        Txt_Conteo_Cheques.Text = String.Format("{0:#0}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Conteo_Cheques].ToString()));
                        Txt_Total_Cheques.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Total_Cheques].ToString()));
                        Txt_Conteo_Transferencias.Text = String.Format("{0:#0}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Conteo_Transferencias].ToString()));
                        Txt_Total_Transferencia.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Total_Transferencias].ToString()));
                        Txt_Total_Cortes_Parciales.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Total_Recolectado].ToString()));
                        Txt_Parcial_Conteo_Bancos.Text = String.Format("{0:#0}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Conteo_Recolectado_Bancos].ToString()));
                        Txt_Parcial_Total_Bancos.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Total_Recolectado_Bancos].ToString()));
                        Txt_Parcial_Conteo_Cheques.Text = String.Format("{0:#0}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Conteo_Recolectado_Cheques].ToString()));
                        Txt_Parcial_Total_Cheques.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Total_Recolectado_Cheques].ToString()));
                        Txt_Parcial_Conteo_Transferencias.Text = String.Format("{0:#0}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Conteo_Recolectado_Transferencias].ToString()));
                        Txt_Parcial_Total_Transferencias.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Total_Recolectado_Transferencias].ToString()));
                        Txt_Monto_Total.Text = String.Format("{0:#,##0.00}", Monto_Total);

                        //Agrega ya sea el sobrante o faltante registrado del turno
                        if (Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Sobrante].ToString()) > 0 || Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Faltante].ToString()) > 0)
                        {
                            Lbl_Sobrante_Faltante.Visible = true;
                            Txt_Sobrante_Faltante.Visible = true;
                            if (Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Sobrante].ToString()) > 0)
                            {
                                Lbl_Sobrante_Faltante.Text = "Sobrante";
                                Txt_Sobrante_Faltante.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Sobrante].ToString()));
                            }
                            else
                            {
                                Lbl_Sobrante_Faltante.Text = "Faltante";
                                Txt_Sobrante_Faltante.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Faltante].ToString()));
                            }
                        }
                        Dt_Formas_Pago_Turno = new DataTable();
                        Dt_Formas_Pago_Turno = Rs_Consulta_Ope_Caj_Turnos.Consulta_Detalles_Turno(); //Consulta el detalles de las denominaciones proporcinadas por el usuario
                        //Asigna a cada uno de los controles de las denominaciones los valores correspondientes
                        foreach (DataRow Denominaciones in Dt_Formas_Pago_Turno.Rows)
                        {
                            Txt_Billetes_1000.Text = Denominaciones[Ope_Caj_Turnos_Detalles.Campo_Billete_1000].ToString();
                            Txt_Billetes_500.Text = Denominaciones[Ope_Caj_Turnos_Detalles.Campo_Billete_500].ToString();
                            Txt_Billetes_200.Text = Denominaciones[Ope_Caj_Turnos_Detalles.Campo_Billete_200].ToString();
                            Txt_Billetes_100.Text = Denominaciones[Ope_Caj_Turnos_Detalles.Campo_Billete_100].ToString();
                            Txt_Billetes_50.Text = Denominaciones[Ope_Caj_Turnos_Detalles.Campo_Billete_50].ToString();
                            Txt_Billetes_20.Text = Denominaciones[Ope_Caj_Turnos_Detalles.Campo_Billete_20].ToString();

                            Txt_Monedas_20.Text = Denominaciones[Ope_Caj_Turnos_Detalles.Campo_Moneda_20].ToString();
                            Txt_Monedas_10.Text = Denominaciones[Ope_Caj_Turnos_Detalles.Campo_Moneda_10].ToString();
                            Txt_Monedas_5.Text = Denominaciones[Ope_Caj_Turnos_Detalles.Campo_Moneda_5].ToString();
                            Txt_Monedas_2.Text = Denominaciones[Ope_Caj_Turnos_Detalles.Campo_Moneda_2].ToString();
                            Txt_Monedas_1.Text = Denominaciones[Ope_Caj_Turnos_Detalles.Campo_Moneda_1].ToString();
                            Txt_Monedas_050.Text = Denominaciones[Ope_Caj_Turnos_Detalles.Campo_Moneda_050].ToString();
                            Txt_Monedas_020.Text = Denominaciones[Ope_Caj_Turnos_Detalles.Campo_Moneda_020].ToString();
                            Txt_Monedas_010.Text = Denominaciones[Ope_Caj_Turnos_Detalles.Campo_Moneda_010].ToString();
                        }
                    }
                }
            }
            else
            {
                Btn_Modificar.Visible = false;
                Btn_Imprimir.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Consulta_Datos_Generales_Turno. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cierre_Turno
    /// DESCRIPCION : Cierra el turno abierto por el empleado con todos los datos 
    ///               proporcionados y consultados
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 23-Octubre-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cierre_Turno()
    {
        Cls_Ope_Pre_Apertura_Turno_Negocio Rs_Modifica_Ope_Caj_Turnos = new Cls_Ope_Pre_Apertura_Turno_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Txt_Total_Efectivo.Text = Hdn_Efectivo_Caja.Value;
            if (String.IsNullOrEmpty(Txt_Billetes_1000.Text)) Txt_Billetes_1000.Text = "0";
            if (String.IsNullOrEmpty(Txt_Billetes_500.Text)) Txt_Billetes_500.Text = "0";
            if (String.IsNullOrEmpty(Txt_Billetes_200.Text)) Txt_Billetes_200.Text = "0";
            if (String.IsNullOrEmpty(Txt_Billetes_100.Text)) Txt_Billetes_100.Text = "0";
            if (String.IsNullOrEmpty(Txt_Billetes_50.Text)) Txt_Billetes_50.Text = "0";
            if (String.IsNullOrEmpty(Txt_Billetes_20.Text)) Txt_Billetes_20.Text = "0";

            if (String.IsNullOrEmpty(Txt_Monedas_20.Text)) Txt_Monedas_20.Text = "0";
            if (String.IsNullOrEmpty(Txt_Monedas_10.Text)) Txt_Monedas_10.Text = "0";
            if (String.IsNullOrEmpty(Txt_Monedas_5.Text)) Txt_Monedas_5.Text = "0";
            if (String.IsNullOrEmpty(Txt_Monedas_2.Text)) Txt_Monedas_2.Text = "0";
            if (String.IsNullOrEmpty(Txt_Monedas_1.Text)) Txt_Monedas_1.Text = "0";
            if (String.IsNullOrEmpty(Txt_Monedas_050.Text)) Txt_Monedas_050.Text = "0";
            if (String.IsNullOrEmpty(Txt_Monedas_020.Text)) Txt_Monedas_020.Text = "0";
            if (String.IsNullOrEmpty(Txt_Monedas_010.Text)) Txt_Monedas_010.Text = "0";

            Rs_Modifica_Ope_Caj_Turnos.P_No_Turno = Txt_No_Turno.Text;
            Rs_Modifica_Ope_Caj_Turnos.P_Caja_Id = Hdn_Caja_ID.Value;
            Rs_Modifica_Ope_Caj_Turnos.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
            Rs_Modifica_Ope_Caj_Turnos.P_Nombre_Empleado = Cls_Sessiones.Nombre_Empleado;
            Rs_Modifica_Ope_Caj_Turnos.P_Nombre_Recibe = Txt_Nombre_Recibe_Efectivo.Text.Trim();
            if (!String.IsNullOrEmpty(Txt_Recibo_Final_Turno.Text)) Rs_Modifica_Ope_Caj_Turnos.P_Recibo_Final = Txt_Recibo_Final_Turno.Text.ToString();
            Rs_Modifica_Ope_Caj_Turnos.P_Sobrante = 0;
            Rs_Modifica_Ope_Caj_Turnos.P_Faltante = 0;
            Rs_Modifica_Ope_Caj_Turnos.P_Total_Efectivo_Sistema = Convert.ToDecimal(Hdn_Efectivo_Sistema.Value);
            Rs_Modifica_Ope_Caj_Turnos.P_Total_Efectivo_Caja = Convert.ToDecimal(Convert.ToString(Txt_Total_Efectivo.Text.ToString().Replace(",", "")).Replace("$", ""));
            Rs_Modifica_Ope_Caj_Turnos.P_Conteo_Bancos = Convert.ToInt32(Txt_Conteo_Bancos.Text);
            Rs_Modifica_Ope_Caj_Turnos.P_Total_Bancos = Convert.ToDecimal(Txt_Total_Bancos.Text.ToString().Replace(",", ""));
            Rs_Modifica_Ope_Caj_Turnos.P_Conteo_Cheques = Convert.ToInt32(Txt_Conteo_Cheques.Text);
            Rs_Modifica_Ope_Caj_Turnos.P_Total_Cheques = Convert.ToDecimal(Txt_Total_Cheques.Text.ToString().Replace(",", ""));
            Rs_Modifica_Ope_Caj_Turnos.P_Conteo_Transferencias = Convert.ToInt32(Txt_Conteo_Transferencias.Text);
            Rs_Modifica_Ope_Caj_Turnos.P_Total_Trasnferencias = Convert.ToDecimal(Txt_Total_Transferencia.Text.ToString().Replace(",", ""));
            Rs_Modifica_Ope_Caj_Turnos.P_Total_Recolectado = Convert.ToDecimal(Txt_Total_Cortes_Parciales.Text.ToString().Replace(",", ""));
            Rs_Modifica_Ope_Caj_Turnos.P_Conteo_Recolectado_Bancos = Convert.ToInt32(Txt_Parcial_Conteo_Bancos.Text);
            Rs_Modifica_Ope_Caj_Turnos.P_Total_Recolectado_Bancos = Convert.ToDecimal(Txt_Parcial_Total_Bancos.Text.ToString().Replace(",", ""));
            Rs_Modifica_Ope_Caj_Turnos.P_Conteo_Recolectado_Cheques = Convert.ToInt32(Txt_Parcial_Conteo_Cheques.Text);
            Rs_Modifica_Ope_Caj_Turnos.P_Total_Recolectado_Cheques = Convert.ToDecimal(Txt_Parcial_Total_Cheques.Text.ToString().Replace(",", ""));
            Rs_Modifica_Ope_Caj_Turnos.P_Conteo_Recolectado_Transferencias = Convert.ToInt32(Txt_Parcial_Conteo_Transferencias.Text);
            Rs_Modifica_Ope_Caj_Turnos.P_Total_Recolectado_Trasnferencias = Convert.ToDecimal(Txt_Parcial_Total_Transferencias.Text.ToString().Replace(",", ""));

            if (Convert.ToDouble(Convert.ToString(Txt_Total_Efectivo.Text.ToString().Replace(",", "")).Replace("$", "")) != Convert.ToDouble(Hdn_Efectivo_Sistema.Value))
            {
                if (Convert.ToDouble(Convert.ToString(Txt_Total_Efectivo.Text.ToString().Replace(",", "")).Replace("$", "")) > Convert.ToDouble(Hdn_Efectivo_Sistema.Value))
                {
                    Rs_Modifica_Ope_Caj_Turnos.P_Sobrante = Convert.ToDecimal(Convert.ToString(Txt_Total_Efectivo.Text.ToString().Replace(",", "")).Replace("$", "")) - Convert.ToDecimal(Hdn_Efectivo_Sistema.Value);
                }
                else
                {
                    Rs_Modifica_Ope_Caj_Turnos.P_Faltante = Convert.ToDecimal(Hdn_Efectivo_Sistema.Value) - Convert.ToDecimal(Convert.ToString(Txt_Total_Efectivo.Text.ToString().Replace(",", "")).Replace("$", ""));
                }
            }
            //Registro de Denominaciones
            Rs_Modifica_Ope_Caj_Turnos.P_Biillete_1000 = Convert.ToInt32(Txt_Billetes_1000.Text);
            Rs_Modifica_Ope_Caj_Turnos.P_Biillete_500 = Convert.ToInt32(Txt_Billetes_500.Text);
            Rs_Modifica_Ope_Caj_Turnos.P_Biillete_200 = Convert.ToInt32(Txt_Billetes_200.Text);
            Rs_Modifica_Ope_Caj_Turnos.P_Biillete_100 = Convert.ToInt32(Txt_Billetes_100.Text);
            Rs_Modifica_Ope_Caj_Turnos.P_Biillete_50 = Convert.ToInt32(Txt_Billetes_50.Text);
            Rs_Modifica_Ope_Caj_Turnos.P_Biillete_20 = Convert.ToInt32(Txt_Billetes_20.Text);
            Rs_Modifica_Ope_Caj_Turnos.P_Moneda_20 = Convert.ToInt32(Txt_Monedas_20.Text);
            Rs_Modifica_Ope_Caj_Turnos.P_Moneda_10 = Convert.ToInt32(Txt_Monedas_10.Text);
            Rs_Modifica_Ope_Caj_Turnos.P_Moneda_5 = Convert.ToInt32(Txt_Monedas_5.Text);
            Rs_Modifica_Ope_Caj_Turnos.P_Moneda_2 = Convert.ToInt32(Txt_Monedas_2.Text);
            Rs_Modifica_Ope_Caj_Turnos.P_Moneda_1 = Convert.ToInt32(Txt_Monedas_1.Text);
            Rs_Modifica_Ope_Caj_Turnos.P_Moneda_050 = Convert.ToInt32(Txt_Monedas_050.Text);
            Rs_Modifica_Ope_Caj_Turnos.P_Moneda_020 = Convert.ToInt32(Txt_Monedas_020.Text);
            Rs_Modifica_Ope_Caj_Turnos.P_Moneda_010 = Convert.ToInt32(Txt_Monedas_010.Text);

            Rs_Modifica_Ope_Caj_Turnos.Modificar_Datos_Turno(); //Cierra el turno con todos sus valores
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Ciere de Turno", "alert('El Cierre de Turno fue Exitoso');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Cierre_Turno " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Datos_Impresion_Cierre_Turno
    /// DESCRIPCION : Realiza el DataSet que se necesita para poder pasar los datos
    ///               de la pantalla al reporte de Crystal
    /// PARAMETROS  : 
    /// CREO        : Yazmin Abigail Delgado Gómez
    /// FECHA_CREO  : 24-Octubre-2011
    /// MODIFICO          : Ismael Prieto Sánchez
    /// FECHA_MODIFICO    : 17/Noviembre/2011
    /// CAUSA_MODIFICACION: Agregar cortes parciales al cierre
    ///*******************************************************************************
    private void Datos_Impresion_Cierre_Turno()
    {
        Cls_Ope_Pre_Apertura_Turno_Negocio Rs_Consulta_Ope_Caj_Turnos = new Cls_Ope_Pre_Apertura_Turno_Negocio(); //Variable de conexion hacia la capa de negocio
        String Ruta_Archivo = @Server.MapPath("../Rpt/Cajas/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Cierre_Turno_" + Session.SessionID; //Obtiene el nombre del archivo que sera asignado al documento
        Ds_Ope_Caj_Turnos_Cierre Ds_Cierre_Turno = new Ds_Ope_Caj_Turnos_Cierre();
        DataTable Dt_Cierre_General = new DataTable(); //Variable a conter los valores a pasar al reporte
        DataTable Dt_Cierre_Detalles = new DataTable(); //Variable a conter los valores a pasar al reporte
        DataTable Dt_Cortes_Parciales = new DataTable(); //Variable a conter los valores a pasar al reporte
        DataTable Dt_Consulta; //Variable para consultar las recolecciones del turno

        try
        {
            //Se define la estructura general del DataSet a contener los valores generales del cierre de turno
            Dt_Cierre_General.Columns.Add("No_Empleado", typeof(System.String));
            Dt_Cierre_General.Columns.Add("No_Turno", typeof(System.String));
            Dt_Cierre_General.Columns.Add("Modulo", typeof(System.String));
            Dt_Cierre_General.Columns.Add("Caja", typeof(System.String));
            Dt_Cierre_General.Columns.Add("Nombre", typeof(System.String));
            Dt_Cierre_General.Columns.Add("Nombre_Recibe", typeof(System.String));
            Dt_Cierre_General.Columns.Add("Fecha_Apertura", typeof(System.DateTime));
            Dt_Cierre_General.Columns.Add("Hora_Apertura", typeof(System.DateTime));
            Dt_Cierre_General.Columns.Add("Fecha_Aplicacion", typeof(System.DateTime));
            Dt_Cierre_General.Columns.Add("Fondo_Inicial", typeof(System.Double));
            Dt_Cierre_General.Columns.Add("Recibo_Inicial", typeof(System.String));
            Dt_Cierre_General.Columns.Add("Recibo_Final", typeof(System.String));
            Dt_Cierre_General.Columns.Add("Estatus", typeof(System.String));
            Dt_Cierre_General.Columns.Add("Fecha_Cierre", typeof(System.DateTime));
            Dt_Cierre_General.Columns.Add("Hora_Cierre", typeof(System.DateTime));
            Dt_Cierre_General.Columns.Add("Total_Efectivo_Sistema", typeof(System.Double));
            Dt_Cierre_General.Columns.Add("Total_Efectivo_Caja", typeof(System.Double));
            Dt_Cierre_General.Columns.Add("Total_Cortes_Parciales", typeof(System.Double));
            Dt_Cierre_General.Columns.Add("Conteo_Tarjetas", typeof(System.Int32));
            Dt_Cierre_General.Columns.Add("Total_Bancos", typeof(System.Double));
            Dt_Cierre_General.Columns.Add("Conteo_Cheques", typeof(System.Int32));
            Dt_Cierre_General.Columns.Add("Total_Cheques", typeof(System.Double));
            Dt_Cierre_General.Columns.Add("Conteo_Transferencias", typeof(System.Int32));
            Dt_Cierre_General.Columns.Add("Total_Transferencias", typeof(System.Double));
            Dt_Cierre_General.Columns.Add("Monto_Total", typeof(System.Double));
            Dt_Cierre_General.Columns.Add("Sobrante", typeof(System.Double));
            Dt_Cierre_General.Columns.Add("Faltante", typeof(System.Double));
            Dt_Cierre_General.Columns.Add("No_Recolecciones", typeof(System.Int32));
            DataRow General; //Variable para asignación del valor al DataTable
            //Asigna los valores a pasar al DataTable
            General = Dt_Cierre_General.NewRow();
            General["No_Empleado"] = Txt_No_Empleado.Text;
            General["No_Turno"] = Txt_No_Turno.Text;
            General["Modulo"] = Txt_Modulo_Caja_Empleado.Text;
            General["Caja"] = Txt_Caja_Empleado.Text;
            General["Nombre"] = Txt_Nombre_Empleado.Text;
            General["Nombre_Recibe"] = Txt_Nombre_Recibe_Efectivo.Text.Trim();
            General["Fecha_Apertura"] = Convert.ToDateTime(Txt_Fecha_Apertura_Turno.Text);
            General["Hora_Apertura"] = Convert.ToDateTime(Txt_Hora_Apertura_Turno.Text);
            General["Fecha_Aplicacion"] = Convert.ToDateTime(Txt_Fecha_Aplicacion_Turno.Text);
            General["Fondo_Inicial"] = Convert.ToDouble(Txt_Fondo_Inicial_Turno.Text);
            General["Recibo_Inicial"] = Txt_Recibo_Inicial_Turno.Text;
            if (!String.IsNullOrEmpty(Txt_Recibo_Final_Turno.Text)) General["Recibo_Final"] = Txt_Recibo_Final_Turno.Text;
            General["Estatus"] = Txt_Estatus_Turno.Text;
            General["Fecha_Cierre"] = Convert.ToDateTime(Txt_Fecha_Cierre_Turno.Text);
            General["Hora_Cierre"] = Convert.ToDateTime(Txt_Hora_Cierre_Turno.Text);
            General["Total_Efectivo_Sistema"] = Convert.ToDouble(Hdn_Efectivo_Sistema.Value);
            General["Total_Efectivo_Caja"] = Convert.ToDouble(Txt_Total_Efectivo.Text.ToString().Replace(",", ""));
            General["Total_Cortes_Parciales"] = Convert.ToDouble(Txt_Total_Cortes_Parciales.Text.ToString().Replace(",", ""));
            General["Conteo_Tarjetas"] = Convert.ToInt32(Txt_Conteo_Bancos.Text.ToString().Replace(",", ""));
            General["Total_Bancos"] = Convert.ToDouble(Txt_Total_Bancos.Text.ToString().Replace(",", ""));
            General["Conteo_Cheques"] = Convert.ToInt32(Txt_Conteo_Cheques.Text.ToString().Replace(",", ""));
            General["Total_Cheques"] = Convert.ToDouble(Txt_Total_Cheques.Text.ToString().Replace(",", ""));
            General["Conteo_Transferencias"] = Convert.ToInt32(Txt_Conteo_Transferencias.Text.ToString().Replace(",", ""));
            General["Total_Transferencias"] = Convert.ToDouble(Txt_Total_Transferencia.Text.ToString().Replace(",", ""));
            General["Monto_Total"] = Convert.ToDouble(Txt_Monto_Total.Text.ToString().Replace(",", ""));
            General["Sobrante"] = 0;
            General["Faltante"] = 0;
            if (Lbl_Sobrante_Faltante.Text == "Sobrante")
            {
                General["Sobrante"] = Convert.ToDouble(Txt_Sobrante_Faltante.Text.ToString().Replace(",", ""));
            }
            else
            {
                General["Faltante"] = Convert.ToDouble(Txt_Sobrante_Faltante.Text.ToString().Replace(",", ""));
            }
            General["No_Recolecciones"] = Convert.ToInt32(Hdn_No_Recolecciones.Value);
            Dt_Cierre_General.Rows.Add(General);

            //Se define la estructura a generar de los cortes parciales que introdujo el usuario para el cierre de turno
            Dt_Cortes_Parciales.Columns.Add("No_Entrega", typeof(System.Int32));
            Dt_Cortes_Parciales.Columns.Add("Realizo", typeof(System.String));
            Dt_Cortes_Parciales.Columns.Add("Monto", typeof(System.Double));
            Dt_Cortes_Parciales.Columns.Add("Monto_Tarjeta", typeof(System.Double));
            Dt_Cortes_Parciales.Columns.Add("Monto_Cheques", typeof(System.Double));
            Dt_Cortes_Parciales.Columns.Add("Monto_Transferencias", typeof(System.Double));
            DataRow Cortes_Parciales; //Variable para asignación del valor al DataTable
            //Asigna los cortes parciales
            Rs_Consulta_Ope_Caj_Turnos.P_No_Turno = Txt_No_Turno.Text;
            Dt_Consulta = Rs_Consulta_Ope_Caj_Turnos.Consultar_Monto_Recolectado_Detalles();
            if (Dt_Consulta.Rows.Count > 0)
            {
                foreach (DataRow Registro in Dt_Consulta.Rows)
                {
                    Cortes_Parciales = Dt_Cortes_Parciales.NewRow();
                    Cortes_Parciales["No_Entrega"] = Convert.ToInt32(Registro["Num_Recoleccion"].ToString());
                    Cortes_Parciales["Realizo"] = Registro["Recibe_Efectivo"].ToString();
                    Cortes_Parciales["Monto"] = Convert.ToDouble(Registro["Monto_Recolectado"].ToString());
                    Cortes_Parciales["Monto_Tarjeta"] = Convert.ToDouble(Registro["Monto_Tarjeta"].ToString());
                    Cortes_Parciales["Monto_Cheques"] = Convert.ToDouble(Registro["Monto_Cheque"].ToString());
                    Cortes_Parciales["Monto_Transferencias"] = Convert.ToDouble(Registro["Monto_Transferencia"].ToString());
                    Dt_Cortes_Parciales.Rows.Add(Cortes_Parciales);
                }
            }
            else
            {
                Cortes_Parciales = Dt_Cortes_Parciales.NewRow();
                Cortes_Parciales["No_Entrega"] = 0;
                Cortes_Parciales["Realizo"] = "NO SE REALIZARON CORTES PARCIALES";
                Cortes_Parciales["Monto"] = 0;
                Cortes_Parciales["Monto_Tarjeta"] = 0;
                Cortes_Parciales["Monto_Cheques"] = 0;
                Cortes_Parciales["Monto_Transferencias"] = 0;
                Dt_Cortes_Parciales.Rows.Add(Cortes_Parciales);
            }

            //Se define la estructura a generar de las denominaciones que introdujo el usuario para el cierre de turno
            Dt_Cierre_Detalles.Columns.Add("Denominacion", typeof(System.String));
            Dt_Cierre_Detalles.Columns.Add("Cantidad", typeof(System.Int32));
            Dt_Cierre_Detalles.Columns.Add("Monto", typeof(System.Double));
            DataRow Denominaciones; //Variable para asignación del valor al DataTable
            //Asigna los valores a pasar al DataTable
            if (Convert.ToInt32(Txt_Billetes_1000.Text) > 0)
            {
                Denominaciones = Dt_Cierre_Detalles.NewRow();
                Denominaciones["Denominacion"] = "BILLETE DE $ 1,000.00";
                Denominaciones["Monto"] = 1000 * Convert.ToInt32(Txt_Billetes_1000.Text);
                Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Billetes_1000.Text);
                Dt_Cierre_Detalles.Rows.Add(Denominaciones);
            }
            if (Convert.ToInt32(Txt_Billetes_500.Text) > 0)
            {
                Denominaciones = Dt_Cierre_Detalles.NewRow();
                Denominaciones["Denominacion"] = "BILLETE DE $   500.00";
                Denominaciones["Monto"] = 500 * Convert.ToInt32(Txt_Billetes_500.Text);
                Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Billetes_500.Text);
                Dt_Cierre_Detalles.Rows.Add(Denominaciones);
            }
            if (Convert.ToInt32(Txt_Billetes_200.Text) > 0)
            {
                Denominaciones = Dt_Cierre_Detalles.NewRow();
                Denominaciones["Denominacion"] = "BILLETE DE $   200.00";
                Denominaciones["Monto"] = 200 * Convert.ToInt32(Txt_Billetes_200.Text);
                Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Billetes_200.Text);
                Dt_Cierre_Detalles.Rows.Add(Denominaciones);
            }
            if (Convert.ToInt32(Txt_Billetes_100.Text) > 0)
            {
                Denominaciones = Dt_Cierre_Detalles.NewRow();
                Denominaciones["Denominacion"] = "BILLETE DE $   100.00";
                Denominaciones["Monto"] = 100 * Convert.ToInt32(Txt_Billetes_100.Text);
                Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Billetes_100.Text);
                Dt_Cierre_Detalles.Rows.Add(Denominaciones);
            }
            if (Convert.ToInt32(Txt_Billetes_50.Text) > 0)
            {
                Denominaciones = Dt_Cierre_Detalles.NewRow();
                Denominaciones["Denominacion"] = "BILLETE DE $    50.00";
                Denominaciones["Monto"] = 50 * Convert.ToInt32(Txt_Billetes_50.Text);
                Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Billetes_50.Text);
                Dt_Cierre_Detalles.Rows.Add(Denominaciones);
            }
            if (Convert.ToInt32(Txt_Billetes_20.Text) > 0)
            {
                Denominaciones = Dt_Cierre_Detalles.NewRow();
                Denominaciones["Denominacion"] = "BILLETE DE $    20.00";
                Denominaciones["Monto"] = 20 * Convert.ToInt32(Txt_Billetes_20.Text);
                Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Billetes_20.Text);
                Dt_Cierre_Detalles.Rows.Add(Denominaciones);
            }
            if (Convert.ToInt32(Txt_Monedas_20.Text) > 0)
            {
                Denominaciones = Dt_Cierre_Detalles.NewRow();
                Denominaciones["Denominacion"] = "MONEDA DE $    20.00";
                Denominaciones["Monto"] = 20 * Convert.ToInt32(Txt_Monedas_20.Text);
                Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Monedas_20.Text);
                Dt_Cierre_Detalles.Rows.Add(Denominaciones);
            }
            if (Convert.ToInt32(Txt_Monedas_10.Text) > 0)
            {
                Denominaciones = Dt_Cierre_Detalles.NewRow();
                Denominaciones["Denominacion"] = "MONEDA DE $    10.00";
                Denominaciones["Monto"] = 10 * Convert.ToInt32(Txt_Monedas_10.Text);
                Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Monedas_10.Text);
                Dt_Cierre_Detalles.Rows.Add(Denominaciones);
            }
            if (Convert.ToInt32(Txt_Monedas_5.Text) > 0)
            {
                Denominaciones = Dt_Cierre_Detalles.NewRow();
                Denominaciones["Denominacion"] = "MONEDA DE $     5.00";
                Denominaciones["Monto"] = 5 * Convert.ToInt32(Txt_Monedas_5.Text);
                Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Monedas_5.Text);
                Dt_Cierre_Detalles.Rows.Add(Denominaciones);
            }
            if (Convert.ToInt32(Txt_Monedas_2.Text) > 0)
            {
                Denominaciones = Dt_Cierre_Detalles.NewRow();
                Denominaciones["Denominacion"] = "MONEDA DE $     2.00";
                Denominaciones["Monto"] = 2 * Convert.ToInt32(Txt_Monedas_2.Text);
                Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Monedas_2.Text);
                Dt_Cierre_Detalles.Rows.Add(Denominaciones);
            }
            if (Convert.ToInt32(Txt_Monedas_1.Text) > 0)
            {
                Denominaciones = Dt_Cierre_Detalles.NewRow();
                Denominaciones["Denominacion"] = "MONEDA DE $     1.00";
                Denominaciones["Monto"] = 1 * Convert.ToInt32(Txt_Monedas_1.Text);
                Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Monedas_1.Text);
                Dt_Cierre_Detalles.Rows.Add(Denominaciones);
            }
            if (Convert.ToInt32(Txt_Monedas_050.Text) > 0)
            {
                Denominaciones = Dt_Cierre_Detalles.NewRow();
                Denominaciones["Denominacion"] = "MONEDA DE $     0.50";
                Denominaciones["Monto"] = 0.50 * Convert.ToInt32(Txt_Monedas_050.Text);
                Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Monedas_050.Text);
                Dt_Cierre_Detalles.Rows.Add(Denominaciones);
            }
            if (Convert.ToInt32(Txt_Monedas_020.Text) > 0)
            {
                Denominaciones = Dt_Cierre_Detalles.NewRow();
                Denominaciones["Denominacion"] = "MONEDA DE $     0.20";
                Denominaciones["Monto"] = 0.20 * Convert.ToInt32(Txt_Monedas_020.Text);
                Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Monedas_020.Text);
                Dt_Cierre_Detalles.Rows.Add(Denominaciones);
            }
            if (Convert.ToInt32(Txt_Monedas_010.Text) > 0)
            {
                Denominaciones = Dt_Cierre_Detalles.NewRow();
                Denominaciones["Denominacion"] = "MONEDA DE $     0.10";
                Denominaciones["Monto"] = 0.10 * Convert.ToInt32(Txt_Monedas_010.Text);
                Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Monedas_010.Text);
                Dt_Cierre_Detalles.Rows.Add(Denominaciones);
            }
            Dt_Cierre_General.TableName = "Cierre_Turno";
            Dt_Cierre_Detalles.TableName = "Denominaciones_Turno";
            Dt_Cortes_Parciales.TableName = "Cortes_Parciales";

            Ds_Cierre_Turno.Clear();
            Ds_Cierre_Turno.Tables.Clear();

            Ds_Cierre_Turno.Tables.Add(Dt_Cierre_General.Copy());
            Ds_Cierre_Turno.Tables.Add(Dt_Cierre_Detalles.Copy());
            Ds_Cierre_Turno.Tables.Add(Dt_Cortes_Parciales.Copy());

            ReportDocument Reporte = new ReportDocument();
            Reporte.Load(Ruta_Archivo + "Rpt_Ope_Caj_Turnos_Cierre.rpt");
            Reporte.SetDataSource(Ds_Cierre_Turno);

            DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();

            Nombre_Archivo += ".pdf";
            Ruta_Archivo = @Server.MapPath("../../Reporte/");
            m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

            ExportOptions Opciones_Exportacion = new ExportOptions();
            Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
            Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
            Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Opciones_Exportacion);

            Abrir_Ventana(Nombre_Archivo);
        }
        catch (Exception ex)
        {
            throw new Exception("Datos_Impresion_Cierre_Turno " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Cajero_General
    /// DESCRIPCION : Consulta el nombre del cajero general
    /// PARAMETROS  : 
    /// CREO        : Ismael Prieto Sánchez
    /// FECHA_CREO  : 23/Abril/2012 2:45pm
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Cajero_General()
    {
        Cls_Ope_Caj_Pagos_Negocio Rs_Consulta_Cajero_General = new Cls_Ope_Caj_Pagos_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            Txt_Nombre_Recibe_Efectivo.Text = Rs_Consulta_Cajero_General.Consulta_Cajero_General();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Cajero_General " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Abrir_Ventana
    ///DESCRIPCIÓN: Abre en otra ventana el archivo pdf
    ///PARÁMETROS : Nombre_Archivo: Guarda el nombre del archivo que se desea abrir
    ///                             para mostrar los datos al usuario
    ///CREO       : Yazmin A Delgado Gómez
    ///FECHA_CREO : 12-Octubre-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private void Abrir_Ventana(String Nombre_Archivo)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
        try
        {
            Pagina = Pagina + Nombre_Archivo;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
            "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Abrir_Ventana " + ex.Message.ToString(), ex);
        }
    }
    #endregion
    #endregion
    #region Eventos
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                Habilitar_Controles("Modficar");//Habilita los controles para la siguiente operación del usuario
            }
            else
            {
                Cierre_Turno();                 //Cierra el turno con todos los datos consultados y proporcionados por el usuario
                Inicializa_Controles();         //Consulta todos los datos del turno
                Datos_Impresion_Cierre_Turno(); //Manda a pantalla el recibo de cierre de caja para que el empleado pueda Imprimir el mismo
            }
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
        if (!String.IsNullOrEmpty(Txt_No_Turno.Text)) Datos_Impresion_Cierre_Turno();
    }
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
                Limpia_Controles(false); //Limpia los controles correspondientes a los valores que introdujo el usuario
                Habilitar_Controles("Inicial");//Habilita los controles para la siguiente operación del usuario
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
}
