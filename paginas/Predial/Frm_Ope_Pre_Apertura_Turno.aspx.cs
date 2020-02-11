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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Cajas_Empleados.Negocios;
using Presidencia.Operacion_Fechas_Aplicacion.Negocio;
using Presidencia.Operaciones_Apertura_Turnos.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Menus.Negocios;
using Presidencia.Reportes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Apertura_Turno : System.Web.UI.Page
{
    #region (Load/Init)
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load.
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página.
        ///PROPIEDADES:     
        ///CREO       : Miguel Angel Bedolla Moreno.
        ///FECHA_CREO : 07/Julio/2011 
        ///MODIFICO          :
        ///FECHA_MODIFICO    :
        ///CAUSA_MODIFICACIÓN:
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
                    Inicializa_Controles("Inicial");//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
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
    #region (Metodos)
        #region (Métodos Generales)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Inicializa_Controles
            /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
            ///               diferentes operaciones
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 11/Octubre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Inicializa_Controles(String Operacion)
            {
                try
                {
                    Habilitar_Controles(Operacion);   //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
                    Limpia_Controles();               //Limpia los controles del forma
                    Consulta_Datos_Generales_Turno(); //Consulta los datos del turno que pueda tener abierto el empleado
                    if(String.IsNullOrEmpty(Txt_No_Empleado.Text)) Consulta_Caja_Empleado();         //Consulta que el empleado tenga asignada una caja
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Habilitar_Controles
            /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara
            ///               la página para a siguiente operación
            /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte
            ///                          del usuario si es una alta, modificacion
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 10/Octubre/2011
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
                            Btn_Modificar.Visible = true;
                            Btn_Imprimir.Visible = true;
                            Btn_Nuevo.CausesValidation = false;
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Txt_Recibo_Inicial_Turno.Enabled = Habilitado;
                            Txt_Fondo_Inicial_Turno.Enabled = Habilitado;
                            Configuracion_Acceso("Frm_Ope_Pre_Apertura_Turno.aspx");
                            break;

                        case "Inicial_Modifica":
                            Habilitado = false;
                            Btn_Modificar.ToolTip = "Modificar";
                            Btn_Salir.ToolTip = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Btn_Imprimir.Visible = true;
                            Txt_Recibo_Inicial_Turno.Enabled = Habilitado;
                            Txt_Fondo_Inicial_Turno.Enabled = Habilitado;
                            break;

                        case "Nuevo":
                            Habilitado = true;
                            Btn_Nuevo.ToolTip = "Dar de Alta";
                            Btn_Salir.ToolTip = "Cancelar";
                            Btn_Nuevo.Visible = true;
                            Btn_Modificar.Visible = false;
                            Btn_Imprimir.Visible = false;
                            Btn_Nuevo.CausesValidation = true;
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                            Txt_Recibo_Inicial_Turno.Enabled = Habilitado;
                            Txt_Fondo_Inicial_Turno.Enabled = Habilitado;
                            break;

                        case "Modificar":
                            Habilitado = true;
                            Btn_Nuevo.Visible = false;
                            Btn_Modificar.Visible = true;
                            Btn_Imprimir.Visible = false;
                            Btn_Modificar.CausesValidation = false;
                            Btn_Modificar.ToolTip = "Actualizar";
                            Btn_Salir.ToolTip = "Cancelar";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                            Txt_Recibo_Inicial_Turno.Enabled = Habilitado;
                            Txt_Fondo_Inicial_Turno.Enabled = Habilitado;
                            break;
                    }
                    Btn_Reapertura_Turno.Visible = false;
                    Btn_Buscar.Enabled = !Habilitado;
                    Btn_Fecha_Busqueda.Enabled = !Habilitado;
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
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 10/Octubre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpia_Controles()
            {
                try
                {
                    Hfd_No_Turno.Value = "";
                    Txt_Busqueda.Text = "";
                    Txt_Caja_ID.Text = "";
                    Txt_Caja_Empleado.Text = "";
                    Txt_Estatus_Turno.Text = "";
                    Txt_Fecha_Aplicacion_Turno.Text = "";
                    Txt_Fecha_Movimiento_Turno.Text = "";
                    Txt_Fondo_Inicial_Turno.Text = "";
                    Hfd_Fondo_Inicial.Value = "0";
                    Txt_Hora_Apertura_Turno.Text = "";
                    Txt_Hora_Cierre_Turno.Text = "";
                    Txt_Modulo_Caja_Empleado.Text = "";
                    Txt_No_Empleado.Text = "";
                    Txt_Nombre_Empleado.Text = "";
                    Txt_Recibo_Inicial_Turno.Text = "";
                }
                catch (Exception ex)
                {
                    throw new Exception("Limpia_Controles " + ex.Message.ToString(), ex);
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
                    Botones.Add(Btn_Buscar);

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
            /// DESCRIPCION: Evalua que la cadena pasada como parametro sea un Numerica.
            /// PARÁMETROS : Cadena.- El dato a evaluar si es numerico.
            /// CREO       : Juan Alberto Hernandez Negrete
            /// FECHA_CREO : 29/Noviembre/2010
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
        #region (Consultas)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Empleado
            /// DESCRIPCION : Consulta el No y Nombre del empleado que esta logeado en el sistema
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 10-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Empleado()
            {
                DataTable Dt_Datos_Empleados = new DataTable(); //Obtiene los datos generales de empleado que esta logeado en el sistema
                Cls_Cat_Empleados_Negocios Rs_Consulta_Cat_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión hacia la capa de negocios
                try
                {
                    Rs_Consulta_Cat_Empleados.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                    Dt_Datos_Empleados = Rs_Consulta_Cat_Empleados.Consulta_Empleados(); //Consulta los datos generales del empleado
                    //Asigna los valores de la consulta a los campos correspondientes
                    foreach (DataRow Registro in Dt_Datos_Empleados.Rows)
                    {
                        Txt_No_Empleado.Text = Registro[Cat_Empleados.Campo_No_Empleado].ToString();
                        Txt_Nombre_Empleado.Text = Registro["Empleado"].ToString();
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Consulta_Empleado. Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Generales_Turno
            /// DESCRIPCION : Consulta los datos del turno que esta abierto por el empleado
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 11-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Datos_Generales_Turno()
            {
                DataTable Dt_Datos_Turno = new DataTable(); //Obtiene los valores de la consulta
                String ReApertura_Autorizada = "NO";        //Obtiene si tiene autoizacion de raperturar el turno
                Cls_Ope_Pre_Apertura_Turno_Negocio Rs_Consulta_Ope_Caj_Turnos = new Cls_Ope_Pre_Apertura_Turno_Negocio(); //Variable de conexion hacia la capa de negocio
                try
                {
                    Limpia_Controles(); //Limpia los controles de forma
                    Rs_Consulta_Ope_Caj_Turnos.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                    Dt_Datos_Turno = Rs_Consulta_Ope_Caj_Turnos.Consulta_Datos_Generales_Turno(); //Consulta los datos del turno que tiene abierto el empleado
                    if (Dt_Datos_Turno.Rows.Count > 0)
                    {
                        //Agrega los valores de la consulta en los campos correspondientes
                        foreach (DataRow Registro in Dt_Datos_Turno.Rows)
                        {
                            Hfd_No_Turno.Value = Registro["No_Turno"].ToString();
                            Txt_Caja_ID.Text = "";
                            Txt_Hora_Cierre_Turno.Text = "";
                            Txt_Caja_Empleado.Text = Registro["Caja"].ToString();
                            Txt_Estatus_Turno.Text = Registro[Ope_Caj_Turnos.Campo_Estatus].ToString();


                            if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos.Campo_ReApertura_Autorizo].ToString()))
                                ReApertura_Autorizada = Registro[Ope_Caj_Turnos.Campo_ReApertura_Autorizo].ToString();

                            if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos.Campo_Aplicacion_Pago].ToString()))
                                Txt_Fecha_Aplicacion_Turno.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Ope_Caj_Turnos.Campo_Aplicacion_Pago].ToString()));

                            if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos.Campo_Fecha_Turno].ToString()))
                                Txt_Fecha_Movimiento_Turno.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Ope_Caj_Turnos.Campo_Fecha_Turno].ToString()));

                            if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos.Campo_Fondo_Inicial].ToString()))
                            {
                                Txt_Fondo_Inicial_Turno.Text = String.Format("{0:###,##0.00}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Fondo_Inicial].ToString()));
                                Hfd_Fondo_Inicial.Value = String.Format("{0:#0.00}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Fondo_Inicial].ToString()));
                            }

                            if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos.Campo_Hora_Apertura].ToString()))
                                Txt_Hora_Apertura_Turno.Text = String.Format("{0:T}", Convert.ToDateTime(Registro[Ope_Caj_Turnos.Campo_Hora_Apertura].ToString()));

                            if (!String.IsNullOrEmpty(Registro["Modulo"].ToString()))
                                Txt_Modulo_Caja_Empleado.Text = Registro["Modulo"].ToString();

                            if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos.Campo_Recibo_Inicial].ToString()))
                                Txt_Recibo_Inicial_Turno.Text = Registro[Ope_Caj_Turnos.Campo_Recibo_Inicial].ToString();
                        }
                        Consulta_Empleado(); //Consulta los datos generales del empleado
                        Btn_Nuevo.Visible = false; //No permite al empleado abrir un nuevo turno ya que se tiene uno abierto con anterioridad
                        Btn_Modificar.Visible = true;
                    }
                    else
                    {
                        Btn_Nuevo.Visible = true;
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
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Turno_Empleado
            /// DESCRIPCION : Consulta los datos del último turno abierto por el empleado
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 11-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Datos_Turno_Empleado()
            {
                String Ultimo_Folio = ""; //Obtiene los valores de la consulta
                Cls_Ope_Pre_Apertura_Turno_Negocio Rs_Consulta_Ope_Caj_Turnos = new Cls_Ope_Pre_Apertura_Turno_Negocio(); //Variable de conexion hacia la capa de negocio
                try
                {
                    Rs_Consulta_Ope_Caj_Turnos.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                    Rs_Consulta_Ope_Caj_Turnos.P_Caja_Id = Txt_Caja_ID.Text;
                    Ultimo_Folio = Rs_Consulta_Ope_Caj_Turnos.Consulta_Ultimo_Folio(); //Consulta los datos generales del último turno abierto por el empleado
                    if (!String.IsNullOrEmpty(Ultimo_Folio))
                    {
                        Txt_Recibo_Inicial_Turno.Text = Ultimo_Folio;
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Consulta_Datos_Turno_Empleado. Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Caja_Empleado
            /// DESCRIPCION : Consulta la caja y el modulo en el cual esta asignado el empleado
            ///               que intenta abrir el turno
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 11-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Caja_Empleado()
            {
                DataTable Dt_Menu = new DataTable(); //Obtiene el ID de menu de asignación de cajas
                Cls_Apl_Cat_Menus_Negocio Rs_Consulta_Apl_Cat_Menus = new Cls_Apl_Cat_Menus_Negocio(); //Variable de conexión hacia la capa de negocios
                Cls_Cat_Pre_Cajas_Empleados_Negocios Rs_Consulta_Cat_Pre_Cajas_Empleados = new Cls_Cat_Pre_Cajas_Empleados_Negocios(); //Variable de conexión hacia la capa de negocios
                DataTable Dt_Caja_Empleado = new DataTable(); //Obtiene los valores de la consulta
                String Pagina = ""; //Obtiene la dirección de la página
                try
                {
                    Rs_Consulta_Cat_Pre_Cajas_Empleados.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                    Dt_Caja_Empleado = Rs_Consulta_Cat_Pre_Cajas_Empleados.Consulta_Caja_Empleado(); //Consulta la caja que tiene asignada el empleado
                    Btn_Nuevo.Visible=false;
                    //Si el usuario ya esta asignado a una caja valida que el módulo a la cual pertenece la caja y la caja esten vigentes
                    //para poder asignarle la caja y el módulo en automático al empleado que intenta abrir el turno
                    if (Dt_Caja_Empleado.Rows.Count > 0)
                    {
                        foreach (DataRow Registro in Dt_Caja_Empleado.Rows)
                        {
                            //Valida que los estatus de la caja y módulo esten vigentes para podersela asignar al empleado de manera automática
                            if (Registro["Estatus_Modulo"].ToString() == "VIGENTE" && Registro["Estatus_Caja"].ToString() == "VIGENTE")
                            {
                                Txt_Caja_ID.Text = Registro[Cat_Pre_Cajas_Empleados.Campo_Caja_ID].ToString();
                                Txt_Caja_Empleado.Text = Registro["Caja"].ToString();
                                Txt_Modulo_Caja_Empleado.Text = Registro["Modulo"].ToString();
                                Btn_Nuevo.Visible = true;
                            }
                        }
                    }
                    //Si no tiene asignado una caja entonces redirecciona al empleado para que indique que caja va a tener asignada
                    if (Btn_Nuevo.Visible == false)
                    {
                        Rs_Consulta_Apl_Cat_Menus.P_Url_Link = "../Predial/Frm_Cat_Pre_Cajas_Empleados.aspx";
                        Dt_Menu = Rs_Consulta_Apl_Cat_Menus.Consulta_Menu_Parent_ID(); //Consulta el ID de menu
                        foreach (DataRow Menu in Dt_Menu.Rows)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Apertura de Turno", "alert('Favor de indicar que caja va a utilizar en el submenu Asignación de Cajas');", true);
                            Pagina = "../Predial/Frm_Cat_Pre_Cajas_Empleados.aspx?PAGINA=" + Menu[Apl_Cat_Menus.Campo_Menu_ID].ToString();
                            Response.Redirect(Pagina);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Consulta_Caja_Empleado. Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Turno_Abierto_Caja
            /// DESCRIPCION : Consulta si la caja que se pretende abrir ya tiene asignado
            ///               un turno abierto
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 11-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Turno_Abierto_Caja()
            {
                DataTable Dt_Datos_Turno = new DataTable(); //Obtiene los valores de la consulta
                Cls_Ope_Pre_Apertura_Turno_Negocio Rs_Consulta_Ope_Caj_Turnos = new Cls_Ope_Pre_Apertura_Turno_Negocio(); //Variable de conexion hacia la capa de negocio

                 try
                 {   
                     Rs_Consulta_Ope_Caj_Turnos.P_Caja_Id = Txt_Caja_ID.Text.ToString();
                     Rs_Consulta_Ope_Caj_Turnos.P_Fecha_Turno = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);
                     Dt_Datos_Turno= Rs_Consulta_Ope_Caj_Turnos.Consulta_Turno_Abierto_Caja(); //Consulta si ya se encuentra abierto un turno en la caja
                     if (Dt_Datos_Turno.Rows.Count > 0)
                     {
                         foreach (DataRow Registro in Dt_Datos_Turno.Rows)
                         {
                             Lbl_Mensaje_Error.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La caja ya se encuentra abierta por el empleado. " + Registro[Ope_Caj_Turnos.Campo_Usuario_Creo].ToString() + "<br>";
                             Lbl_Mensaje_Error.Visible = true;
                             Img_Error.Visible = true;
                             Btn_Nuevo.Visible = false;
                         }
                     }
                 }
                 catch (Exception Ex)
                 {
                     throw new Exception("Consulta_Caja_Empleado. Error: [" + Ex.Message + "]");
                 }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Turno_Abierto_Dia
            /// DESCRIPCION : Consulta si hay turno abierto del dia anterior
            /// PARAMETROS  : 
            /// CREO        : Ismael Prieto Sánchez
            /// FECHA_CREO  : 20-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Turno_Abierto_Dia()
            {
                DataTable Dt_Datos_Turno = new DataTable(); //Obtiene los valores de la consulta
                Cls_Ope_Pre_Apertura_Turno_Negocio Rs_Consulta_Ope_Caj_Turnos = new Cls_Ope_Pre_Apertura_Turno_Negocio(); //Variable de conexion hacia la capa de negocio

                try
                {
                    Rs_Consulta_Ope_Caj_Turnos.P_Fecha_Turno = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);
                    Dt_Datos_Turno = Rs_Consulta_Ope_Caj_Turnos.Consulta_Turno_Abierto_Dia(); //Consulta si esta abierto un turno anterior de dia
                    if (Dt_Datos_Turno.Rows.Count > 0)
                    {
                        foreach (DataRow Registro in Dt_Datos_Turno.Rows)
                        {
                            Lbl_Mensaje_Error.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Existe un turno de día anterior que no ha sido cerrado, favor de verificarlo.<br>";
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Habilitar_Controles("Inicial");
                            Btn_Nuevo.Visible = false;
                            Btn_Modificar.Visible = false;
                            Btn_Imprimir.Visible = false;
                        }
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Consulta_Turno_Abierto_Dia. Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Obtener_Fecha_Aplicacion
            /// DESCRIPCION : Consulta la fecha de aplicación que va a tener el alta del turno
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 11-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Obtener_Fecha_Aplicacion()
            {
                DataTable Dt_Fecha_Aplicacion = new DataTable(); //Obtiene la fecha de aplicacion
                Cls_Ope_Pre_Fechas_Aplicacion_Negocio Rs_Consulta_Cat_Pre_Fechas_Aplicacion = new Cls_Ope_Pre_Fechas_Aplicacion_Negocio(); //Variable de conexión hacia la capa de datos
                try
                {
                    Txt_Fecha_Aplicacion_Turno.Text = "";
                    Rs_Consulta_Cat_Pre_Fechas_Aplicacion.P_Fecha_Movimiento = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);
                    Txt_Fecha_Aplicacion_Turno.Text = Rs_Consulta_Cat_Pre_Fechas_Aplicacion.Obtener_Fecha_Aplicacion().P_Fecha_Aplicacion; //Obtiene la fecha de aplicación que tendra el turno
                    if (Consultar_Fecha_Foranea())
                    {
                        Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inabiles = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
                        Txt_Fecha_Aplicacion_Turno.Text = Dias_Inabiles.Calcular_Fecha(DateTime.Now.ToShortDateString(), "1").ToString("dd/MMM/yyyy");
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(Txt_Fecha_Aplicacion_Turno.Text.ToString()))
                        {
                            Txt_Fecha_Aplicacion_Turno.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Txt_Fecha_Aplicacion_Turno.Text.ToString()));
                        }
                        else
                        {
                            Txt_Fecha_Aplicacion_Turno.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Obtener_Fecha_Aplicacion. Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consultar_Fecha_Foranea
            /// DESCRIPCION : Consulta si la caja es foranea, y los turnos que ha tenido durante el ultimo turno abierto.
            /// PARAMETROS  : 
            /// CREO        : Miguel Angel Bedolla Moreno
            /// FECHA_CREO  : 23-Marzo-2012
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Consultar_Fecha_Foranea()
            {
                Boolean Bandera = false;
                try
                {
                    String Campo;
                    String Condiciones;
                    String Respuesta;
                    Campo = "(SELECT NVL(COUNT(TURNO." + Ope_Caj_Turnos.Campo_No_Turno + "),0) FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + " TURNO WHERE TURNO." + Ope_Caj_Turnos.Campo_Caja_Id + "=" + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_Id + " AND TO_DATE(TURNO." + Ope_Caj_Turnos.Campo_Fecha_Turno + ")=TO_DATE(" + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Fecha_Turno + ")) ||'/'||";
                    Campo += "(SELECT " + Cat_Pre_Cajas.Campo_Foranea + " FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " WHERE " + Cat_Pre_Cajas.Campo_Caja_Id + "=" + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_Id + ") AS FORANEA ";
                    Condiciones = Ope_Caj_Turnos.Campo_Empleado_ID + "='" + Cls_Sessiones.Empleado_ID + "' AND " + Ope_Caj_Turnos.Campo_Estatus + "='CERRADO' AND ";
                    Condiciones += Ope_Caj_Turnos.Campo_No_Turno + "=(SELECT MAX(" + Ope_Caj_Turnos.Campo_No_Turno + ") FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + " WHERE " + Ope_Caj_Turnos.Campo_Empleado_ID + "='" + Cls_Sessiones.Empleado_ID + "' AND " + Ope_Caj_Turnos.Campo_Estatus + "='CERRADO')";
                    Respuesta = Obtener_Dato_Consulta(Campo, Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos, Condiciones);
                    if (Respuesta.Trim() != "")
                    {
                        String[] Respuesta_ar = Respuesta.Split('/');
                        if (Respuesta_ar[1] == "SI")
                        {
                            if (Convert.ToInt32(Respuesta_ar[0]) >= 1)
                            {
                                Bandera = true;
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Obtener_Fecha_Aplicacion. Error: [" + Ex.Message + "]");
                }
                return Bandera;
            }
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
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Generales_Turno_Fecha
            /// DESCRIPCION : Consulta todos los datos de un turno abierto por el empleado de
            ///               acuerdo a la fecha proporcionada
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 27-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Datos_Generales_Turno_Fecha()
            {
                String Estatus_Cierre_Dia = "CERRADO";      //Obtiene el estatus del turno del día al cual pertenece el turno que se esta consultando
                String ReApertura_Autorizada = "NO";        //Obtiene si tiene autoizacion de raperturar el turno
                String Fecha_Busqueda;                      //Obtiene la fecha en que desea consultar el usuario
                String No_Turno_Dia = "";                   //Obtiene el No_Turno_Dia al cual pertenece el turno que el empleado esta consultando
                DataTable Dt_Datos_Turno = new DataTable(); //Obtiene los valores de la consulta
                Cls_Ope_Pre_Apertura_Turno_Negocio Rs_Consulta_Ope_Caj_Turnos = new Cls_Ope_Pre_Apertura_Turno_Negocio(); //Variable de conexion hacia la capa de negocio
                try
                {
                    Fecha_Busqueda = Txt_Busqueda.Text;
                    Limpia_Controles(); //Limpia los controles de forma
                    Btn_Reapertura_Turno.Visible = false;
                    Rs_Consulta_Ope_Caj_Turnos.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                    Txt_Busqueda.Text = Fecha_Busqueda;
                    Rs_Consulta_Ope_Caj_Turnos.P_Fecha_Turno = Txt_Busqueda.Text;
                    Dt_Datos_Turno = Rs_Consulta_Ope_Caj_Turnos.Consulta_Datos_Generales_Turno_Fecha(); //Consulta los datos del turno que tiene abierto el empleado
                    if (Dt_Datos_Turno.Rows.Count > 0)
                    {
                        //Agrega los valores de la consulta en los campos correspondientes
                        foreach (DataRow Registro in Dt_Datos_Turno.Rows)
                        {
                            Hfd_No_Turno.Value = Registro[Ope_Caj_Turnos.Campo_No_Turno].ToString();
                            if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos.Campo_No_Turno_Dia].ToString())) No_Turno_Dia = Registro[Ope_Caj_Turnos.Campo_No_Turno_Dia].ToString();
                            Txt_Caja_ID.Text = "";
                            Txt_Hora_Cierre_Turno.Text = "";
                            Txt_Caja_Empleado.Text = Registro["Caja"].ToString();
                            Txt_Estatus_Turno.Text = Registro[Ope_Caj_Turnos.Campo_Estatus].ToString();

                            if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos.Campo_ReApertura_Autorizo].ToString()))
                                ReApertura_Autorizada = Registro[Ope_Caj_Turnos.Campo_ReApertura_Autorizo].ToString();

                            if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos.Campo_Aplicacion_Pago].ToString()))
                                Txt_Fecha_Aplicacion_Turno.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Ope_Caj_Turnos.Campo_Aplicacion_Pago].ToString()));

                            if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos.Campo_Fecha_Turno].ToString()))
                                Txt_Fecha_Movimiento_Turno.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Ope_Caj_Turnos.Campo_Fecha_Turno].ToString()));

                            if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos.Campo_Fondo_Inicial].ToString()))
                            {
                                Txt_Fondo_Inicial_Turno.Text = String.Format("{0:###,##0.00}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Fondo_Inicial].ToString()));
                                Hfd_Fondo_Inicial.Value = String.Format("{0:#0.00}", Convert.ToDouble(Registro[Ope_Caj_Turnos.Campo_Fondo_Inicial].ToString()));
                            }
                            if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos.Campo_Hora_Apertura].ToString()))
                                Txt_Hora_Apertura_Turno.Text = String.Format("{0:T}", Convert.ToDateTime(Registro[Ope_Caj_Turnos.Campo_Hora_Apertura].ToString()));

                            if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos.Campo_Hora_Cierre].ToString()))
                                Txt_Hora_Cierre_Turno.Text = String.Format("{0:T}", Convert.ToDateTime(Registro[Ope_Caj_Turnos.Campo_Hora_Cierre].ToString()));
                            
                            if (!String.IsNullOrEmpty(Registro["Modulo"].ToString()))
                                Txt_Modulo_Caja_Empleado.Text = Registro["Modulo"].ToString();

                            if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos.Campo_Recibo_Inicial].ToString()))
                                Txt_Recibo_Inicial_Turno.Text = Registro[Ope_Caj_Turnos.Campo_Recibo_Inicial].ToString();
                        }
                        Consulta_Empleado();       //Consulta los datos generales del empleado
                        Btn_Nuevo.Visible = false; //No permite al empleado abrir un nuevo turno ya que se tiene uno abierto con anterioridad
                        Btn_Modificar.Visible = false;
                        if (Txt_Estatus_Turno.Text == "CERRADO") 
                        {
                            if (!String.IsNullOrEmpty(No_Turno_Dia))
                            {
                                Rs_Consulta_Ope_Caj_Turnos.P_No_Turno_Dia = No_Turno_Dia;
                                Estatus_Cierre_Dia = Rs_Consulta_Ope_Caj_Turnos.Consulta_Datos_Cierre_Dia(); //Consulta el estatus del turno del día al cual pertenece el turno que esta siendo consultado
                                if (Estatus_Cierre_Dia == "ABIERTO" && ReApertura_Autorizada == "SI")
                                {
                                    Btn_Reapertura_Turno.Visible = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        Btn_Nuevo.Visible = true;
                        Btn_Modificar.Visible = false;
                        Btn_Imprimir.Visible = false;
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Consulta_Datos_Generales_Turno_Fecha. Error: [" + Ex.Message + "]");
                }
            }
        #endregion
        #region (Metodos Validacion)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Validar_Datos_Turno
            /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 11-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Validar_Datos_Turno()
            {
                Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
                Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

                if (string.IsNullOrEmpty(Txt_Fondo_Inicial_Turno.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El fondo inicial del turno. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Recibo_Inicial_Turno.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Número de Recibo inicial. <br>";
                    Datos_Validos = false;
                }
                else
                {
                    Cls_Ope_Pre_Apertura_Turno_Negocio Rs_Alta_Ope_Caj_Turnos = new Cls_Ope_Pre_Apertura_Turno_Negocio(); //Variable de conexión hacia la capa de negocios
                    Rs_Alta_Ope_Caj_Turnos.P_Recibo_Inicial = Txt_Recibo_Inicial_Turno.Text;
                    Boolean Valida = Rs_Alta_Ope_Caj_Turnos.Valida_Recibo_Inicial();
                    if (Valida)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Número de Recibo inicial ya ha sido utilizado. <br>";
                        Datos_Validos = false;
                    }
                }                
                return Datos_Validos;
            }
        #endregion
        #region (Métodos Operacion)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Alta_Turno
            /// DESCRIPCION : Da de Alta el Turno con los datos proporcionados por el usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 11-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Alta_Turno()
            {
                Cls_Ope_Pre_Apertura_Turno_Negocio Rs_Alta_Ope_Caj_Turnos = new Cls_Ope_Pre_Apertura_Turno_Negocio(); //Variable de conexión hacia la capa de negocios
                try
                {
                    Rs_Alta_Ope_Caj_Turnos.P_Fecha_Turno=String.Format("{0:MM/dd/yyyy}", DateTime.Today);
                    Rs_Alta_Ope_Caj_Turnos.P_Caja_Id = Txt_Caja_ID.Text.ToString();
                    Rs_Alta_Ope_Caj_Turnos.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                    Rs_Alta_Ope_Caj_Turnos.P_Recibo_Inicial =  String.Format("{0:0000000000}", Convert.ToInt32(Txt_Recibo_Inicial_Turno.Text.ToString()));
                    Rs_Alta_Ope_Caj_Turnos.P_Contador_Recibo = String.Format("{0:0000000000}", Convert.ToInt32(Txt_Recibo_Inicial_Turno.Text.ToString()));
                    Rs_Alta_Ope_Caj_Turnos.P_Fondo_Inicial = Convert.ToDouble(Txt_Fondo_Inicial_Turno.Text.ToString().Replace("$",""));
                    Rs_Alta_Ope_Caj_Turnos.P_Aplicacion_Pago = Txt_Fecha_Aplicacion_Turno.Text.ToString();
                    Rs_Alta_Ope_Caj_Turnos.P_Fecha_Turno = Txt_Fecha_Movimiento_Turno.Text.ToString();
                    Rs_Alta_Ope_Caj_Turnos.P_Nombre_Empleado = Cls_Sessiones.Nombre_Empleado;
                    Rs_Alta_Ope_Caj_Turnos.Alta_Apertura_Turno(); //Da de alta el turno con los datos del empleado
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Apertura de Turno", "alert('El Alta del Turno del Empleado fue Exitosa');", true);
                    Habilitar_Controles("Inicial"); //Habilita los controles de la pantalla para la siguiente operación del sistema
                    Btn_Nuevo.Visible = false;
                }
                catch (Exception ex)
                {
                    throw new Exception("Alta_Turno " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Modifica_Turno
            /// DESCRIPCION : Modifica el Turno con los datos proporcionados por el usuario
            /// PARAMETROS  : 
            /// CREO        : Ismael Prieto Sánchez
            /// FECHA_CREO  : 19-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Modifica_Turno()
            {
                Cls_Ope_Pre_Apertura_Turno_Negocio Rs_Modifica_Ope_Caj_Turnos = new Cls_Ope_Pre_Apertura_Turno_Negocio(); //Variable de conexión hacia la capa de negocios
                try
                {
                    Rs_Modifica_Ope_Caj_Turnos.P_No_Turno = Hfd_No_Turno.Value;
                    Rs_Modifica_Ope_Caj_Turnos.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                    Rs_Modifica_Ope_Caj_Turnos.P_Fondo_Inicial = Convert.ToDouble(Txt_Fondo_Inicial_Turno.Text.ToString().Replace("$", ""));
                    Rs_Modifica_Ope_Caj_Turnos.P_Recibo_Inicial = String.Format("{0:0000000000}", Convert.ToInt32(Txt_Recibo_Inicial_Turno.Text.ToString()));
                    Rs_Modifica_Ope_Caj_Turnos.P_Contador_Recibo = String.Format("{0:0000000000}", Convert.ToInt32(Txt_Recibo_Inicial_Turno.Text.ToString()));
                    Rs_Modifica_Ope_Caj_Turnos.P_Nombre_Empleado = Cls_Sessiones.Nombre_Empleado;
                    Rs_Modifica_Ope_Caj_Turnos.Modifica_Apertura_Turno(); //Modifica el turno con los datos del empleado
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Apertura de Turno", "alert('La Actualización del Turno del Empleado fue Exitosa');", true);
                    Habilitar_Controles("Inicial_Modifica"); //Habilita los controles de la pantalla para la siguiente operación del sistema
                    Btn_Nuevo.Visible = false;
                    Btn_Imprimir.Visible = true;

                }
                catch (Exception ex)
                {
                    throw new Exception("Modifica_Turno " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Impresion_Apertura_Turno
            /// DESCRIPCION : Asigna los datos del turno proporcionados por el usuario para la
            ///               impresión del recibo
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 12-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Impresion_Apertura_Turno()
            {
                String Ruta_Archivo = @Server.MapPath("../Rpt/Cajas/");//Obtiene la ruta en la cual será guardada el archivo
                String Nombre_Archivo = "Apertura_Caja" + Session.SessionID + Convert.ToString(String.Format("{0:ddMMMyyyHHmmss}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
                DataRow Registro; //Obtiene los valores de la consulta realizada para la impresión del recibo
                Ds_Ope_Caj_Turnos_Apertura_Turno Ds_Apertura_Caja = new Ds_Ope_Caj_Turnos_Apertura_Turno();
                DataTable Dt_Apertura_Caja = new DataTable(); //Va a conter los valores a pasar al reporte

                try
                {
                    //Se realiza la estructura a contener los datos
                    Dt_Apertura_Caja.Columns.Add("Caja", typeof(System.String));
                    Dt_Apertura_Caja.Columns.Add("Cajero", typeof(System.String));
                    Dt_Apertura_Caja.Columns.Add("No_Recibo_Inicial", typeof(System.String));
                    Dt_Apertura_Caja.Columns.Add("Fondo_Inicial", typeof(System.Decimal));
                    Dt_Apertura_Caja.Columns.Add("Fecha", typeof(System.DateTime));
                    Dt_Apertura_Caja.Columns.Add("Hora", typeof(System.DateTime));
                    DataRow Renglon;

                    //Agrega los datos del turno que fue abierto
                    Renglon = Dt_Apertura_Caja.NewRow();
                    Renglon["Caja"] = Txt_Caja_Empleado.Text.ToString();
                    Renglon["Cajero"] = Txt_Nombre_Empleado.Text.ToString();
                    Renglon["No_Recibo_Inicial"] = Txt_Recibo_Inicial_Turno.Text.ToString();
                    Renglon["Fondo_Inicial"] = Convert.ToDecimal(Txt_Fondo_Inicial_Turno.Text);
                    Renglon["Fecha"] = Convert.ToDateTime(String.Format("{0:MMM/dd/yyyy}", Convert.ToDateTime(Txt_Fecha_Movimiento_Turno.Text.ToString())));
                    Renglon["Hora"] = Convert.ToDateTime(String.Format("{0:HH:mm:ss}", Convert.ToDateTime(Txt_Hora_Apertura_Turno.Text.ToString())));
                    Dt_Apertura_Caja.Rows.Add(Renglon);

                    // Se llena la cabecera del DataSet                
                    Registro = Dt_Apertura_Caja.Rows[0];

                    Ds_Apertura_Caja.Tables["Apertura_Turno"].ImportRow(Registro);

                    ReportDocument Reporte = new ReportDocument();
                    Reporte.Load(Ruta_Archivo + "Rpt_Ope_Caj_Turnos_Apertura_Turno.rpt");
                    Reporte.SetDataSource(Ds_Apertura_Caja);
                    DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();

                    Nombre_Archivo += ".pdf";
                    Ruta_Archivo = @Server.MapPath("../../Reporte/");
                    m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                    ExportOptions Opciones_Exportacion = new ExportOptions();
                    Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                    Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                    Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                    Reporte.Export(Opciones_Exportacion);

                    //Abrir_Ventana("../../Reporte/" + Nombre_Archivo);
                    Abrir_Ventana(Nombre_Archivo);
                }
                catch (Exception Ex)
                {
                    throw new Exception("Impresion_Apertura_Turno. Error: [" + Ex.Message + "]");
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
    #region (Operacion [Alta - Modificar - Eliminar - Consultar])
        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                if (!String.IsNullOrEmpty(Txt_Busqueda.Text))
                {
                    Consulta_Datos_Generales_Turno_Fecha(); //Consulta los datos del turno que fue abierto en la fecha proporcionada por el usuario
                    if (String.IsNullOrEmpty(Txt_Caja_Empleado.Text))
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + No fue abierto ningun turno en la fecha proporcionada, favor de verificar. <br>";
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Debe seleccionar la fecha en que fue abierto el turno para poder consultar. <br>";
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
                if (Btn_Nuevo.ToolTip == "Nuevo")
                {
                    Consulta_Turno_Abierto_Dia(); //Consulta el turno del dia anterior
                    Consulta_Turno_Abierto_Caja(); //Consulta si la caja tiene un turno abierto
                    if (Btn_Nuevo.Visible == true)
                    {
                        Habilitar_Controles("Nuevo");    //Habilita los controles para la introducción de datos por parte del usuario
                        Consulta_Caja_Empleado();        //Consulta la caja que tiene asignado el empleado que quiere abrir el turno
                        Consulta_Empleado();             //Consulta los datos generales del turno
                        Consulta_Datos_Turno_Empleado(); //Consulta el número de recibo inicial tentativo del turno
                        Obtener_Fecha_Aplicacion();      //Obtiene la fecha de aplicación para los pagos que tendrá el turno
                        Txt_Estatus_Turno.Text = "ABIERTO";
                        Txt_Fecha_Movimiento_Turno.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);
                        Txt_Hora_Apertura_Turno.Text = String.Format("{0:HH:mm}", DateTime.Today);
                    }
                }
                else
                {
                    //Valida si todos los campos requeridos estan llenos si es así da de alta los datos en la base de datos
                    if (Validar_Datos_Turno())
                    {
                        Alta_Turno(); //Asigna la caja al empleado
                        Consulta_Datos_Generales_Turno(); //Consulta los datos generales del turno que fue abierto por el usuario
                        Impresion_Apertura_Turno();
                    }
                    //Muestra los campos que son requeridos por el usuario y que no fueron proporcionados
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
        protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                if (Btn_Modificar.ToolTip == "Modificar")
                {
                    Habilitar_Controles("Modificar");    //Habilita los controles para la introducción de datos por parte del usuario
                }
                else
                {
                    //Valida si todos los campos requeridos estan llenos si es así da de alta los datos en la base de datos
                    if (Validar_Datos_Turno())
                    {
                        Modifica_Turno(); //Actualiza el turno
                        if (Convert.ToDouble(Txt_Fondo_Inicial_Turno.Text.ToString().Replace("$", "")) != Convert.ToDouble(Hfd_Fondo_Inicial.Value.ToString().Replace("$", "")))
                        {
                            Consulta_Datos_Generales_Turno(); //Consulta los datos generales del turno que fue abierto por el usuario
                            Impresion_Apertura_Turno();
                        }
                    }
                    //Muestra los campos que son requeridos por el usuario y que no fueron proporcionados
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
        protected void Btn_Reapertura_Turno_Click(object sender, ImageClickEventArgs e)
        {
            Cls_Ope_Pre_Apertura_Turno_Negocio Rs_Modificar_Ope_Caj_Turnos = new Cls_Ope_Pre_Apertura_Turno_Negocio(); //Variable de conexión hacia la capa de negocios
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                if (!String.IsNullOrEmpty(Txt_Caja_Empleado.Text))
                {
                    Rs_Modificar_Ope_Caj_Turnos.P_No_Turno = Hfd_No_Turno.Value;
                    Rs_Modificar_Ope_Caj_Turnos.Modificar_Turno_Reapertura(); //Realiza la reapertura del turno
                    Consulta_Datos_Generales_Turno();                         //Consulta los datos generales del turno que fue abierto
                    Btn_Reapertura_Turno.Visible = false;
                    Btn_Imprimir.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Apertura de Turno", "alert('El turno fue abierto con exito');", true);
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Debe seleccionar la fecha en que fue abierto el turno para poder consultar. <br>";
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
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Impresion_Apertura_Turno(); //Imprime los datos del turno que fueron asignados por el usuario
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
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                if (Btn_Salir.ToolTip == "Salir")
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
                else
                {
                    if (Btn_Nuevo.Visible == true)
                    {
                        Inicializa_Controles("Inicial");//Habilita los controles para la siguiente operación del usuario en la operación
                    }
                    else
                    {
                        Inicializa_Controles("Inicial_Modifica");//Habilita los controles para la siguiente operación del usuario en la operación
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
    #endregion
}