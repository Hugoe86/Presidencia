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
using System.Globalization;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using Presidencia.Recoleccion_Caja.Negocio;
using Presidencia.Caja_Pagos.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Operaciones_Apertura_Turnos.Negocio;

public partial class paginas_Predial_Frm_Ope_Caj_Recolecciones : System.Web.UI.Page
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
            /// FECHA_CREO  : 20-Agosto-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Inicializa_Controles()
            {
                try
                {
                    Habilitar_Controles("Inicial");    //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
                    Limpia_Controles();                //Limpia los controles del forma
                    Consulta_Cajero_General();          //Consulta el nombre del cajero general
                    Consulta_Caja_Empleado();          //Consulta los datos del empleado
                    Txt_Fecha_Recoleccion_Busqueda.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);
                    Consulta_Recolecciones_Empleado(); //Consulta todos las recolecciones que estan dados de alta en la base de datos
                    Consulta_Unidad_Responsable_Empleado(); //Cosulta la unidad responsable que tiene asignado el empleado
                    Txt_Fecha_Recoleccion.Text = "";
                    Consulta_Datos_Generales_Turno(); //Consulta que el turno del empleado se encuentre abierto para poder hacer la recolección en caja
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
            /// FECHA_CREO  : 20-Agosto-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpia_Controles()
            {
                try
                {
                    //Datos generales de la recolección
                    Txt_Caja_ID.Text = "";
                    Txt_Caja_Recoleccion.Text = "";
                    Txt_Fecha_Recoleccion.Text = "";
                    Txt_Fecha_Recoleccion_Busqueda.Text = "";
                    Txt_Modulo.Text = "";
                    Txt_Monto_Recolectado.Text = "";
                    Txt_Nombre_Cajero.Text = "";
                    Txt_Numero_Recolecciones.Text = "";
                    Txt_Unidad_Responsable.Text = "";
                    Hdn_Efectivo_Caja.Value = "";
                    //Controles Billetes
                    Txt_Billete_1000.Text = "";
                    Txt_Billete_500.Text = "";
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
                    Txt_Moneda_050.Text = "";
                    Txt_Moneda_020.Text = "";
                    Txt_Moneda_010.Text = "";
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
            /// FECHA_CREO  : 20-Agosto-2011
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
                            Btn_Imprimir.ToolTip = "Imprimir";
                            Btn_Salir.ToolTip = "Salir";
                            Btn_Nuevo.Visible = true;
                            Btn_Imprimir.Visible = true;
                            Btn_Nuevo.CausesValidation = false;
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Configuracion_Acceso("Frm_Ope_Caj_Recolecciones.aspx");
                            break;

                        case "Nuevo":
                            Habilitado = true;
                            Btn_Nuevo.ToolTip = "Dar de Alta";
                            Btn_Salir.ToolTip = "Cancelar";
                            Btn_Nuevo.Visible = true;
                            Btn_Imprimir.Visible = false;
                            Btn_Nuevo.CausesValidation = true;
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                            break;
                    }
                    //Habilitación de controles generale
                    Txt_Monto_Recolectado.Enabled = Habilitado;
                    Txt_Fecha_Recoleccion_Busqueda.Enabled = !Habilitado;
                    Btn_Fecha_Recoleccion_Busqueda.Enabled = !Habilitado;
                    Btn_Buscar_Recolecciones.Enabled = !Habilitado;
                    Grid_Recolecciones_Caja.Enabled = !Habilitado;
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Txt_Nombre_Recibe_Efectivo.Enabled = Habilitado;
                    //Habilitacion de Controles Billetes
                    Txt_Billete_1000.Enabled = Habilitado;
                    Txt_Billete_500.Enabled = Habilitado;
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
                    Txt_Moneda_050.Enabled = Habilitado;
                    Txt_Moneda_020.Enabled = Habilitado;
                    Txt_Moneda_010.Enabled = Habilitado;
                }
                catch (Exception ex)
                {
                    throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Llena_Grid_Recolecciones
            /// DESCRIPCION : Llena el grid con las recolecciones que se encuentran en la base de datos
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 20-Agosto-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Llena_Grid_Recolecciones()
        {
            DataTable Dt_Recolecciones; //Variable que obtendra los datos de la consulta 
            try
            {
                Grid_Recolecciones_Caja.Columns[6].Visible=true;
                Grid_Recolecciones_Caja.Columns[7].Visible = true;
                Grid_Recolecciones_Caja.Columns[8].Visible = true;
                Grid_Recolecciones_Caja.Columns[9].Visible = true;
                Grid_Recolecciones_Caja.Columns[10].Visible = true;
                Grid_Recolecciones_Caja.Columns[11].Visible = true;
                Grid_Recolecciones_Caja.Columns[12].Visible = true;
                Grid_Recolecciones_Caja.Columns[13].Visible = true;
                Dt_Recolecciones = (DataTable)Session["Consulta_Recolecciones"];
                Grid_Recolecciones_Caja.DataSource = Dt_Recolecciones;
                Grid_Recolecciones_Caja.DataBind();
                Grid_Recolecciones_Caja.SelectedIndex = -1;
                Grid_Recolecciones_Caja.Columns[6].Visible = false;
                Grid_Recolecciones_Caja.Columns[7].Visible = false;
                Grid_Recolecciones_Caja.Columns[8].Visible = false;
                Grid_Recolecciones_Caja.Columns[9].Visible = false;
                Grid_Recolecciones_Caja.Columns[10].Visible = false;
                Grid_Recolecciones_Caja.Columns[11].Visible = false;
                Grid_Recolecciones_Caja.Columns[12].Visible = false;
                Grid_Recolecciones_Caja.Columns[13].Visible = false;
            }
            catch (Exception ex)
            {
                throw new Exception("Llena_Grid_Recolecciones " + ex.Message.ToString(), ex);
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
                    Botones.Add(Btn_Buscar_Recolecciones);

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
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Cajero_General
            /// DESCRIPCION : Consulta el nombre del cajero general
            /// PARAMETROS  : 
            /// CREO        : Ismael Prieto Sánchez
            /// FECHA_CREO  : 5-Diciembre-2011
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
                            Txt_No_Turno.Text = Registro[Ope_Caj_Turnos.Campo_No_Turno].ToString();
                            Txt_Caja_Recoleccion.Text = Registro["Caja"].ToString();
                            Txt_Modulo.Text = Registro["Modulo"].ToString();
                            Txt_Nombre_Cajero.Text = Cls_Sessiones.Nombre_Empleado;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Caja_Empleado " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Recolecciones_Empleado
            /// DESCRIPCION : Consulta las Recolecciones que estan dados de alta en la BD y que
            ///               pertenecen al empleado que esta logeado
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 20-Agosto-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Recolecciones_Empleado()
            {
                Cls_Ope_Caj_Recolecciones_Negocio Rs_Consulta_Ope_Caj_Recolecciones = new Cls_Ope_Caj_Recolecciones_Negocio(); //Variable de conexión hacia la capa de Negocios
                DataTable Dt_Recolecciones; //Variable que obtendra los datos de la consulta 

                try
                {
                    Rs_Consulta_Ope_Caj_Recolecciones.P_No_Turno = Txt_No_Turno.Text;
                    Rs_Consulta_Ope_Caj_Recolecciones.P_Fecha_Busqueda = Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Txt_Fecha_Recoleccion_Busqueda.Text.ToString())));
                    Rs_Consulta_Ope_Caj_Recolecciones.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                    Dt_Recolecciones = Rs_Consulta_Ope_Caj_Recolecciones.Consulta_Recolecciones(); //Consulta todas las recolecciones del empleado con sus datos generales            
                    Session["Consulta_Recolecciones"] = Dt_Recolecciones;
                    Llena_Grid_Recolecciones();
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Recolecciones_Empleado " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Unidad_Responsable_Empleado
            /// DESCRIPCION : Consulta la Unidad Responsable que tiene asignada el empleado
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 14-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Unidad_Responsable_Empleado()
            {
                DataTable Dt_Unidad_Responsable = new DataTable(); //Obtiene los datos de la consulta
                Cls_Cat_Dependencias_Negocio Rs_Consulta_Cat_Dependencias = new Cls_Cat_Dependencias_Negocio(); //Variabla de conexión hacia la capa de negocios
                try
                {
                    Rs_Consulta_Cat_Dependencias.P_Dependencia_ID = Cls_Sessiones.Dependencia_ID_Empleado;
                    Dt_Unidad_Responsable = Rs_Consulta_Cat_Dependencias.Consulta_Dependencias(); //Consulta la Unidad Responsale que tiene asignada el empleado

                    foreach (DataRow Registro in Dt_Unidad_Responsable.Rows)
                    {
                        Txt_Unidad_Responsable.Text = Registro["CLAVE_NOMBRE"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Unidad_Responsable_Empleado " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Impresion_Recoleccion_Caja
            /// DESCRIPCION : Asigna los datos de la recoleccion proporcionados por el usuario 
            ///              para la impresión del recibo
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 13-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Impresion_Recoleccion_Caja()
            {
                String Ruta_Archivo = @Server.MapPath("../Rpt/Cajas/");//Obtiene la ruta en la cual será guardada el archivo
                String Nombre_Archivo = "Recoleccion_Caja_" + Session.SessionID; //Obtiene el nombre del archivo que sera asignado al documento
                Ds_Ope_Caj_Recolecciones Ds_Recolecciones = new Ds_Ope_Caj_Recolecciones();
                DataTable Dt_Recoleccion_Caja = new DataTable(); //Va a conter los valores a pasar al reporte
                DataTable Dt_Recoleccion_Detalles = new DataTable(); //Variable a conter los valores a pasar al reporte
                try
                {
                    //Se realiza la estructura a contener los datos
                    Dt_Recoleccion_Caja.Columns.Add("Caja", typeof(System.String));
                    Dt_Recoleccion_Caja.Columns.Add("Cajero", typeof(System.String));
                    Dt_Recoleccion_Caja.Columns.Add("Unidad_Responsable", typeof(System.String));
                    Dt_Recoleccion_Caja.Columns.Add("Modulo", typeof(System.String));
                    Dt_Recoleccion_Caja.Columns.Add("Recibio_Efectivo", typeof(System.String));
                    Dt_Recoleccion_Caja.Columns.Add("No_Recoleccion", typeof(System.Int32));
                    Dt_Recoleccion_Caja.Columns.Add("Fecha", typeof(System.DateTime));
                    Dt_Recoleccion_Caja.Columns.Add("Hora", typeof(System.DateTime));
                    Dt_Recoleccion_Caja.Columns.Add("Monto", typeof(System.Decimal));
                    Dt_Recoleccion_Caja.Columns.Add("Monto_Tarjeta", typeof(System.Decimal));
                    Dt_Recoleccion_Caja.Columns.Add("Monto_Cheques", typeof(System.Decimal));
                    Dt_Recoleccion_Caja.Columns.Add("Monto_Transferencias", typeof(System.Decimal));
                    Dt_Recoleccion_Caja.Columns.Add("Conteo_Tarjeta", typeof(System.Int32));
                    Dt_Recoleccion_Caja.Columns.Add("Conteo_Cheques", typeof(System.Int32));
                    Dt_Recoleccion_Caja.Columns.Add("Conteo_Transferencias", typeof(System.Int32));

                    DataRow Renglon;

                    //Agrega los datos del turno que fue abierto
                    Renglon = Dt_Recoleccion_Caja.NewRow();
                    Renglon["Caja"] = Txt_Caja_Recoleccion.Text.ToString();
                    Renglon["Cajero"] = Txt_Nombre_Cajero.Text.ToString();
                    Renglon["Unidad_Responsable"] = Txt_Unidad_Responsable.Text.ToString();
                    Renglon["Modulo"] = Txt_Modulo.Text.ToString();
                    Renglon["Recibio_Efectivo"] = Txt_Nombre_Recibe_Efectivo.Text.ToUpper();
                    Renglon["No_Recoleccion"] = Convert.ToInt32(Txt_Numero_Recolecciones.Text);
                    Renglon["Monto"] = Convert.ToDecimal(Txt_Monto_Recolectado.Text);
                    Renglon["Conteo_Tarjeta"] = Convert.ToDecimal(Txt_Conteo_Tarjeta.Text);
                    Renglon["Monto_Tarjeta"] = Convert.ToDecimal(Txt_Total_Tarjeta.Text);
                    Renglon["Conteo_Cheques"] = Convert.ToDecimal(Txt_Conteo_Cheques.Text);
                    Renglon["Monto_Cheques"] = Convert.ToDecimal(Txt_Total_Cheques.Text);
                    Renglon["Conteo_Transferencias"] = Convert.ToDecimal(Txt_Conteo_Transferencias.Text);
                    Renglon["Monto_Transferencias"] = Convert.ToDecimal(Txt_Total_Transferencias.Text);
                    Renglon["Fecha"] = Convert.ToDateTime(Txt_Fecha_Recoleccion.Text);
                    Renglon["Hora"] =  Convert.ToDateTime(Txt_Fecha_Recoleccion.Text);
                    Dt_Recoleccion_Caja.Rows.Add(Renglon);

                    //Se define la estructura a generar de las denominaciones que introdujo el usuario para el cierre de turno
                    Dt_Recoleccion_Detalles.Columns.Add("Denominacion", typeof(System.String));
                    Dt_Recoleccion_Detalles.Columns.Add("Cantidad", typeof(System.Int32));
                    Dt_Recoleccion_Detalles.Columns.Add("Monto", typeof(System.Double));
                    DataRow Denominaciones; //Variable para asignación del valor al DataTable

                    //Asigna los valores a pasar al DataTable
                    if (Convert.ToInt32(Txt_Billete_1000.Text) > 0)
                    {
                        Denominaciones = Dt_Recoleccion_Detalles.NewRow();
                        Denominaciones["Denominacion"] = "BILLETE DE $ 1,000.00";
                        Denominaciones["Monto"] = 1000 * Convert.ToInt32(Txt_Billete_1000.Text);
                        Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Billete_1000.Text);
                        Dt_Recoleccion_Detalles.Rows.Add(Denominaciones);
                    }
                    if (Convert.ToInt32(Txt_Billete_500.Text) > 0)
                    {
                        Denominaciones = Dt_Recoleccion_Detalles.NewRow();
                        Denominaciones["Denominacion"] = "BILLETE DE $   500.00";
                        Denominaciones["Monto"] = 500 * Convert.ToInt32(Txt_Billete_500.Text);
                        Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Billete_500.Text);
                        Dt_Recoleccion_Detalles.Rows.Add(Denominaciones);
                    }
                    if (Convert.ToInt32(Txt_Billetes_200.Text) > 0)
                    {
                        Denominaciones = Dt_Recoleccion_Detalles.NewRow();
                        Denominaciones["Denominacion"] = "BILLETE DE $   200.00";
                        Denominaciones["Monto"] = 200 * Convert.ToInt32(Txt_Billetes_200.Text);
                        Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Billetes_200.Text);
                        Dt_Recoleccion_Detalles.Rows.Add(Denominaciones);
                    }
                    if (Convert.ToInt32(Txt_Billetes_100.Text) > 0)
                    {
                        Denominaciones = Dt_Recoleccion_Detalles.NewRow();
                        Denominaciones["Denominacion"] = "BILLETE DE $   100.00";
                        Denominaciones["Monto"] = 100 * Convert.ToInt32(Txt_Billetes_100.Text);
                        Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Billetes_100.Text);
                        Dt_Recoleccion_Detalles.Rows.Add(Denominaciones);
                    }
                    if (Convert.ToInt32(Txt_Billetes_50.Text) > 0)
                    {
                        Denominaciones = Dt_Recoleccion_Detalles.NewRow();
                        Denominaciones["Denominacion"] = "BILLETE DE $    50.00";
                        Denominaciones["Monto"] = 50 * Convert.ToInt32(Txt_Billetes_50.Text);
                        Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Billetes_50.Text);
                        Dt_Recoleccion_Detalles.Rows.Add(Denominaciones);
                    }
                    if (Convert.ToInt32(Txt_Billetes_20.Text) > 0)
                    {
                        Denominaciones = Dt_Recoleccion_Detalles.NewRow();
                        Denominaciones["Denominacion"] = "BILLETE DE $    20.00";
                        Denominaciones["Monto"] = 20 * Convert.ToInt32(Txt_Billetes_20.Text);
                        Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Billetes_20.Text);
                        Dt_Recoleccion_Detalles.Rows.Add(Denominaciones);
                    }
                    if (Convert.ToInt32(Txt_Monedas_20.Text) > 0)
                    {
                        Denominaciones = Dt_Recoleccion_Detalles.NewRow();
                        Denominaciones["Denominacion"] = "MONEDA DE $    20.00";
                        Denominaciones["Monto"] = 20 * Convert.ToInt32(Txt_Monedas_20.Text);
                        Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Monedas_20.Text);
                        Dt_Recoleccion_Detalles.Rows.Add(Denominaciones);
                    }
                    if (Convert.ToInt32(Txt_Monedas_10.Text) > 0)
                    {
                        Denominaciones = Dt_Recoleccion_Detalles.NewRow();
                        Denominaciones["Denominacion"] = "MONEDA DE $    10.00";
                        Denominaciones["Monto"] = 10 * Convert.ToInt32(Txt_Monedas_10.Text);
                        Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Monedas_10.Text);
                        Dt_Recoleccion_Detalles.Rows.Add(Denominaciones);
                    }
                    if (Convert.ToInt32(Txt_Monedas_5.Text) > 0)
                    {
                        Denominaciones = Dt_Recoleccion_Detalles.NewRow();
                        Denominaciones["Denominacion"] = "MONEDA DE $     5.00";
                        Denominaciones["Monto"] = 5 * Convert.ToInt32(Txt_Monedas_5.Text);
                        Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Monedas_5.Text);
                        Dt_Recoleccion_Detalles.Rows.Add(Denominaciones);
                    }
                    if (Convert.ToInt32(Txt_Monedas_2.Text) > 0)
                    {
                        Denominaciones = Dt_Recoleccion_Detalles.NewRow();
                        Denominaciones["Denominacion"] = "MONEDA DE $     2.00";
                        Denominaciones["Monto"] = 2 * Convert.ToInt32(Txt_Monedas_2.Text);
                        Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Monedas_2.Text);
                        Dt_Recoleccion_Detalles.Rows.Add(Denominaciones);
                    }
                    if (Convert.ToInt32(Txt_Monedas_1.Text) > 0)
                    {
                        Denominaciones = Dt_Recoleccion_Detalles.NewRow();
                        Denominaciones["Denominacion"] = "MONEDA DE $     1.00";
                        Denominaciones["Monto"] = 1 * Convert.ToInt32(Txt_Monedas_1.Text);
                        Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Monedas_1.Text);
                        Dt_Recoleccion_Detalles.Rows.Add(Denominaciones);
                    }
                    if (Convert.ToInt32(Txt_Moneda_050.Text) > 0)
                    {
                        Denominaciones = Dt_Recoleccion_Detalles.NewRow();
                        Denominaciones["Denominacion"] = "MONEDA DE $     0.50";
                        Denominaciones["Monto"] = 0.50 * Convert.ToInt32(Txt_Moneda_050.Text);
                        Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Moneda_050.Text);
                        Dt_Recoleccion_Detalles.Rows.Add(Denominaciones);
                    }
                    if (Convert.ToInt32(Txt_Moneda_020.Text) > 0)
                    {
                        Denominaciones = Dt_Recoleccion_Detalles.NewRow();
                        Denominaciones["Denominacion"] = "MONEDA DE $     0.20";
                        Denominaciones["Monto"] = 0.20 * Convert.ToInt32(Txt_Moneda_020.Text);
                        Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Moneda_020.Text);
                        Dt_Recoleccion_Detalles.Rows.Add(Denominaciones);
                    }
                    if (Convert.ToInt32(Txt_Moneda_010.Text) > 0)
                    {
                        Denominaciones = Dt_Recoleccion_Detalles.NewRow();
                        Denominaciones["Denominacion"] = "MONEDA DE $     0.10";
                        Denominaciones["Monto"] = 0.10 * Convert.ToInt32(Txt_Moneda_010.Text);
                        Denominaciones["Cantidad"] = Convert.ToInt32(Txt_Moneda_010.Text);
                        Dt_Recoleccion_Detalles.Rows.Add(Denominaciones);
                    }
                    Dt_Recoleccion_Caja.TableName = "Recoleccion";
                    Dt_Recoleccion_Detalles.TableName = "Denominaciones_Recoleccion";

                    Ds_Recolecciones.Clear();
                    Ds_Recolecciones.Tables.Clear();

                    Ds_Recolecciones.Tables.Add(Dt_Recoleccion_Caja.Copy());
                    Ds_Recolecciones.Tables.Add(Dt_Recoleccion_Detalles.Copy());
                    
                    ReportDocument Reporte = new ReportDocument();
                    Reporte.Load(Ruta_Archivo + "Rpt_Ope_Caj_Recolecciones.rpt");
                    Reporte.SetDataSource(Ds_Recolecciones);

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
                catch (Exception Ex)
                {
                    throw new Exception("Impresion_Recoleccion_Caja. Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Alta_Recoleccion
            /// DESCRIPCION : Da de Alta de la Recolección con los datos proporcionados por el usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 20-Agosto-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Alta_Recoleccion()
            {
                Decimal Monto_Recolectado = 0; //Indica el monto que fue recolectado por el usuario
                Decimal Monto_Tarjeta = 0;
                Decimal Monto_Cheques = 0;
                Decimal Monto_Transferencias = 0;
                Cls_Ope_Caj_Recolecciones_Negocio Rs_Alta_Ope_Caj_Recolecciones = new Cls_Ope_Caj_Recolecciones_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
                try
                {
                    Txt_Monto_Recolectado.Text = Hdn_Efectivo_Caja.Value;
                    if (String.IsNullOrEmpty(Txt_Billete_1000.Text)) Txt_Billete_1000.Text = "0";
                    if (String.IsNullOrEmpty(Txt_Billete_500.Text)) Txt_Billete_500.Text = "0";
                    if (String.IsNullOrEmpty(Txt_Billetes_200.Text)) Txt_Billetes_200.Text = "0";
                    if (String.IsNullOrEmpty(Txt_Billetes_100.Text)) Txt_Billetes_100.Text = "0";
                    if (String.IsNullOrEmpty(Txt_Billetes_50.Text)) Txt_Billetes_50.Text = "0";
                    if (String.IsNullOrEmpty(Txt_Billetes_20.Text)) Txt_Billetes_20.Text = "0";

                    if (String.IsNullOrEmpty(Txt_Monedas_20.Text)) Txt_Monedas_20.Text = "0";
                    if (String.IsNullOrEmpty(Txt_Monedas_10.Text)) Txt_Monedas_10.Text = "0";
                    if (String.IsNullOrEmpty(Txt_Monedas_5.Text)) Txt_Monedas_5.Text = "0";
                    if (String.IsNullOrEmpty(Txt_Monedas_2.Text)) Txt_Monedas_2.Text = "0";
                    if (String.IsNullOrEmpty(Txt_Monedas_1.Text)) Txt_Monedas_1.Text = "0";
                    if (String.IsNullOrEmpty(Txt_Moneda_050.Text)) Txt_Moneda_050.Text = "0";
                    if (String.IsNullOrEmpty(Txt_Moneda_020.Text)) Txt_Moneda_020.Text = "0";
                    if (String.IsNullOrEmpty(Txt_Moneda_010.Text)) Txt_Moneda_010.Text = "0";

                    Monto_Recolectado = Convert.ToDecimal(Txt_Monto_Recolectado.Text.ToString().Replace("$","").Replace(",",""));
                    Monto_Tarjeta = Convert.ToDecimal(Txt_Total_Tarjeta.Text.ToString().Replace("$", "").Replace(",", ""));
                    Monto_Cheques = Convert.ToDecimal(Txt_Total_Cheques.Text.ToString().Replace("$", "").Replace(",", ""));
                    Monto_Transferencias = Convert.ToDecimal(Txt_Total_Transferencias.Text.ToString().Replace("$", "").Replace(",", ""));
                    Rs_Alta_Ope_Caj_Recolecciones.P_Caja_ID = Txt_Caja_ID.Text;
                    Rs_Alta_Ope_Caj_Recolecciones.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                    Rs_Alta_Ope_Caj_Recolecciones.P_Monto_Recolectado = Monto_Recolectado;
                    Rs_Alta_Ope_Caj_Recolecciones.P_Conteo_Tarjeta = Convert.ToInt32(Txt_Conteo_Tarjeta.Text);
                    Rs_Alta_Ope_Caj_Recolecciones.P_Monto_Tarjeta = Monto_Tarjeta;
                    Rs_Alta_Ope_Caj_Recolecciones.P_Conteo_Cheque = Convert.ToInt32(Txt_Conteo_Cheques.Text);
                    Rs_Alta_Ope_Caj_Recolecciones.P_Monto_Cheque = Monto_Cheques;
                    Rs_Alta_Ope_Caj_Recolecciones.P_Conteo_Transferencia = Convert.ToInt32(Txt_Conteo_Transferencias.Text);
                    Rs_Alta_Ope_Caj_Recolecciones.P_Monto_Transferencia = Monto_Transferencias;
                    Rs_Alta_Ope_Caj_Recolecciones.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Rs_Alta_Ope_Caj_Recolecciones.P_No_Turno = Txt_No_Turno.Text;
                    Rs_Alta_Ope_Caj_Recolecciones.P_Recibe_Efectivo = Txt_Nombre_Recibe_Efectivo.Text.Trim().ToUpper();
                    Rs_Alta_Ope_Caj_Recolecciones.P_Fecha_Busqueda = Convert.ToDateTime(Convert.ToDateTime(Txt_Fecha_Recoleccion.Text).ToString("dd/MMM/yyyy"));
                    Rs_Alta_Ope_Caj_Recolecciones.P_Biillete_1000 = Convert.ToInt32(Txt_Billete_1000.Text);
                    Rs_Alta_Ope_Caj_Recolecciones.P_Biillete_500 = Convert.ToInt32(Txt_Billete_500.Text);
                    Rs_Alta_Ope_Caj_Recolecciones.P_Biillete_200 = Convert.ToInt32(Txt_Billetes_200.Text);
                    Rs_Alta_Ope_Caj_Recolecciones.P_Biillete_100 = Convert.ToInt32(Txt_Billetes_100.Text);
                    Rs_Alta_Ope_Caj_Recolecciones.P_Biillete_50 = Convert.ToInt32(Txt_Billetes_50.Text);
                    Rs_Alta_Ope_Caj_Recolecciones.P_Biillete_20 = Convert.ToInt32(Txt_Billetes_20.Text);
                    Rs_Alta_Ope_Caj_Recolecciones.P_Moneda_20 = Convert.ToInt32(Txt_Monedas_20.Text);
                    Rs_Alta_Ope_Caj_Recolecciones.P_Moneda_10 = Convert.ToInt32(Txt_Monedas_10.Text);
                    Rs_Alta_Ope_Caj_Recolecciones.P_Moneda_5 = Convert.ToInt32(Txt_Monedas_5.Text);
                    Rs_Alta_Ope_Caj_Recolecciones.P_Moneda_2 = Convert.ToInt32(Txt_Monedas_2.Text);
                    Rs_Alta_Ope_Caj_Recolecciones.P_Moneda_1 = Convert.ToInt32(Txt_Monedas_1.Text);
                    Rs_Alta_Ope_Caj_Recolecciones.P_Moneda_050 = Convert.ToInt32(Txt_Moneda_050.Text);
                    Rs_Alta_Ope_Caj_Recolecciones.P_Moneda_020 = Convert.ToInt32(Txt_Moneda_020.Text);
                    Rs_Alta_Ope_Caj_Recolecciones.P_Moneda_010 = Convert.ToInt32(Txt_Moneda_010.Text);
                    Rs_Alta_Ope_Caj_Recolecciones.Alta_Recoleccion(); //Da de alta todos los datos en la base de datos
                    Txt_Numero_Recolecciones.Text = Rs_Alta_Ope_Caj_Recolecciones.P_Numero_Recoleccion.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Recolección de Caja", "alert('El Alta de la Recolección fue Exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Alta_Recoleccion " + ex.Message.ToString(), ex);
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
                Cls_Ope_Pre_Apertura_Turno_Negocio Rs_Consulta_Ope_Caj_Turnos = new Cls_Ope_Pre_Apertura_Turno_Negocio(); //Variable de conexion hacia la capa de negocio
                try
                {
                    Rs_Consulta_Ope_Caj_Turnos.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                    Dt_Datos_Turno = Rs_Consulta_Ope_Caj_Turnos.Consulta_Datos_Generales_Turno(); //Consulta los datos del turno que tiene abierto el empleado
                    if (Dt_Datos_Turno.Rows.Count <= 0)
                    {
                        Btn_Imprimir.Visible = false;
                        Btn_Nuevo.Visible = false; //No permite al empleado abrir un nuevo turno ya que se tiene uno abierto con anterioridad
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Consulta_Datos_Generales_Turno. Error: [" + Ex.Message + "]");
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consulta_Caja_Total_Cobrado
            ///DESCRIPCIÓN: Realiza la consulta de los montos con las formas de pago
            ///PROPIEDADES:     
            ///CREO: Ismael Prieto Sánchez
            ///FECHA_CREO: 8/Diciembre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Consulta_Caja_Total_Cobrado()
            {
                try
                {
                    Cls_Ope_Caj_Recolecciones_Negocio Pagos = new Cls_Ope_Caj_Recolecciones_Negocio();
                    Pagos.P_No_Turno = Txt_No_Turno.Text;
                    DataTable Dt_Total_Cobrado = Pagos.Consultar_Caja_Total_Cobrado();

                    Txt_Conteo_Tarjeta.Text = Dt_Total_Cobrado.Rows[0]["CONTEO_TARJETA"].ToString();
                    Txt_Total_Tarjeta.Text = String.Format("{0:#0.00}", Convert.ToDouble(Dt_Total_Cobrado.Rows[0]["TOTAL_TARJETA"].ToString()));
                    Txt_Conteo_Cheques.Text = Dt_Total_Cobrado.Rows[0]["CONTEO_CHEQUE"].ToString();
                    Txt_Total_Cheques.Text = String.Format("{0:#0.00}", Convert.ToDouble(Dt_Total_Cobrado.Rows[0]["TOTAL_CHEQUE"].ToString()));
                    Txt_Conteo_Transferencias.Text = Dt_Total_Cobrado.Rows[0]["CONTEO_TRANSFERENCIA"].ToString();
                    Txt_Total_Transferencias.Text = String.Format("{0:#0.00}", Convert.ToDouble(Dt_Total_Cobrado.Rows[0]["TOTAL_TRANSFERENCIA"].ToString()));

                    //Txt_Total_General.Text = String.Format("{0:#0.00}", Convert.ToDouble(Dt_Total_Cobrado.Rows[0]["TOTAL_TARJETA"].ToString()) + Convert.ToDouble(Dt_Total_Cobrado.Rows[0]["TOTAL_CHEQUE"].ToString()) + Convert.ToDouble(Dt_Total_Cobrado.Rows[0]["TOTAL_TRANSFERENCIA"].ToString()));
                }
                catch (Exception Ex)
                {
                    throw new Exception("Llenar_Caja_Total_Cobrado. Error: [" + Ex.Message + "]");
                }
            }
        #endregion
    #endregion
    #region Grid
        protected void Grid_Recolecciones_Caja_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable Dt_Detalles_Recoleccion = new DataTable(); //Obtiene los datos del detalle de la recoleccion
            Cls_Ope_Caj_Recolecciones_Negocio Rs_Consulta_Ope_Caj_Recolecciones = new Cls_Ope_Caj_Recolecciones_Negocio(); //Variable de conexión hacia la capa de negocios
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                int index = ((Grid_Recolecciones_Caja.PageIndex) * Grid_Recolecciones_Caja.PageSize) + (Grid_Recolecciones_Caja.SelectedIndex);
                if (index != -1)
                {
                    Txt_Numero_Recolecciones.Text = Grid_Recolecciones_Caja.Rows[index].Cells[1].Text;
                    Txt_Modulo.Text = Grid_Recolecciones_Caja.Rows[index].Cells[3].Text;
                    Txt_Caja_Recoleccion.Text = Grid_Recolecciones_Caja.Rows[index].Cells[2].Text;
                    Txt_Nombre_Cajero.Text = Cls_Sessiones.Nombre_Empleado;
                    Txt_Fecha_Recoleccion.Text = String.Format("{0:dd/MMM/yyyy HH:mm:ss}", Convert.ToDateTime(Grid_Recolecciones_Caja.Rows[index].Cells[4].Text.ToString()));
                    Txt_Monto_Recolectado.Text = Grid_Recolecciones_Caja.Rows[index].Cells[5].Text;
                    Grid_Recolecciones_Caja.Columns[6].Visible = true;
                    Rs_Consulta_Ope_Caj_Recolecciones.P_No_Recoleccion = Grid_Recolecciones_Caja.Rows[index].Cells[6].Text;
                    Grid_Recolecciones_Caja.Columns[6].Visible = false;
                    Rs_Consulta_Ope_Caj_Recolecciones.P_No_Turno = Txt_No_Turno.Text.ToString();
                    Rs_Consulta_Ope_Caj_Recolecciones.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                    Grid_Recolecciones_Caja.Columns[7].Visible = true;
                    Txt_Nombre_Recibe_Efectivo.Text = Grid_Recolecciones_Caja.Rows[index].Cells[7].Text;
                    Grid_Recolecciones_Caja.Columns[7].Visible = false;
                    Grid_Recolecciones_Caja.Columns[8].Visible = true;
                    Grid_Recolecciones_Caja.Columns[9].Visible = true;
                    Grid_Recolecciones_Caja.Columns[10].Visible = true;
                    Grid_Recolecciones_Caja.Columns[11].Visible = true;
                    Grid_Recolecciones_Caja.Columns[12].Visible = true;
                    Grid_Recolecciones_Caja.Columns[13].Visible = true;
                    Txt_Conteo_Tarjeta.Text = Grid_Recolecciones_Caja.Rows[index].Cells[8].Text;
                    Txt_Total_Tarjeta.Text = String.Format("{0:#0.00}", Grid_Recolecciones_Caja.Rows[index].Cells[9].Text);
                    Txt_Conteo_Cheques.Text = Grid_Recolecciones_Caja.Rows[index].Cells[10].Text;
                    Txt_Total_Cheques.Text = String.Format("{0:#0.00}", Grid_Recolecciones_Caja.Rows[index].Cells[11].Text);
                    Txt_Conteo_Transferencias.Text = Grid_Recolecciones_Caja.Rows[index].Cells[12].Text;
                    Txt_Total_Transferencias.Text = String.Format("{0:#0.00}", Grid_Recolecciones_Caja.Rows[index].Cells[13].Text);
                    Grid_Recolecciones_Caja.Columns[8].Visible = false;
                    Grid_Recolecciones_Caja.Columns[9].Visible = false;
                    Grid_Recolecciones_Caja.Columns[10].Visible = false;
                    Grid_Recolecciones_Caja.Columns[11].Visible = false;
                    Grid_Recolecciones_Caja.Columns[12].Visible = false;
                    Grid_Recolecciones_Caja.Columns[13].Visible = false;
                    Dt_Detalles_Recoleccion = Rs_Consulta_Ope_Caj_Recolecciones.Consulta_Detalles_Recolecciones();//Consulta los detalles de la recolección que fue seleccionada por el usuario
                    //Realiza el vaciado de los valores obtenidos de la consulta a los controles correspondientes
                    foreach (DataRow Registro in Dt_Detalles_Recoleccion.Rows)
                    {
                        Txt_Billete_1000.Text = Registro[Ope_Caj_Recolecciones_Detalles.Campo_Billete_1000].ToString();
                        Txt_Billete_500.Text = Registro[Ope_Caj_Recolecciones_Detalles.Campo_Billete_500].ToString();
                        Txt_Billetes_200.Text = Registro[Ope_Caj_Recolecciones_Detalles.Campo_Billete_200].ToString();
                        Txt_Billetes_100.Text = Registro[Ope_Caj_Recolecciones_Detalles.Campo_Billete_100].ToString();
                        Txt_Billetes_50.Text = Registro[Ope_Caj_Recolecciones_Detalles.Campo_Billete_50].ToString();
                        Txt_Billetes_20.Text = Registro[Ope_Caj_Recolecciones_Detalles.Campo_Billete_20].ToString();

                        Txt_Monedas_20.Text = Registro[Ope_Caj_Recolecciones_Detalles.Campo_Moneda_20].ToString();
                        Txt_Monedas_10.Text = Registro[Ope_Caj_Recolecciones_Detalles.Campo_Moneda_10].ToString();
                        Txt_Monedas_5.Text = Registro[Ope_Caj_Recolecciones_Detalles.Campo_Moneda_5].ToString();
                        Txt_Monedas_2.Text = Registro[Ope_Caj_Recolecciones_Detalles.Campo_Moneda_2].ToString();
                        Txt_Monedas_1.Text = Registro[Ope_Caj_Recolecciones_Detalles.Campo_Moneda_1].ToString();
                        Txt_Moneda_050.Text = Registro[Ope_Caj_Recolecciones_Detalles.Campo_Moneda_050].ToString();
                        Txt_Moneda_020.Text = Registro[Ope_Caj_Recolecciones_Detalles.Campo_Moneda_020].ToString();
                        Txt_Moneda_010.Text = Registro[Ope_Caj_Recolecciones_Detalles.Campo_Moneda_010].ToString();
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
        protected void Grid_Recolecciones_Caja_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Grid_Recolecciones_Caja.PageIndex = e.NewPageIndex;
                Consulta_Recolecciones_Empleado();
                Grid_Recolecciones_Caja.SelectedIndex = -1;
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            }
        }
    #endregion
    #region Eventos
        protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                if (Btn_Nuevo.ToolTip.Trim().Equals("Nuevo"))
                {
                    Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                    Limpia_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                    Consulta_Caja_Empleado();     //Consulta los datos generales de la caja en la cual esta logeado el empleado
                    Consulta_Unidad_Responsable_Empleado(); //Consulta la unidad responsable que tiene asignado el empleado
                    Consulta_Caja_Total_Cobrado(); //Consulta el total de las formas de pago que no son efectivo
                    Txt_Fecha_Recoleccion.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                }
                else
                {
                    //Si todos los campos requeridos fueron proporcionados por el usuario entonces da de alta los mismo en la base de datos
                    if (!String.IsNullOrEmpty(Txt_Monto_Recolectado.Text.ToString()) && !String.IsNullOrEmpty(Txt_Nombre_Recibe_Efectivo.Text.ToString()))
                    {
                        Txt_Fecha_Recoleccion.Text = String.Format("{0:dd/MMM/yyyy HH:mm:ss}", DateTime.Now);
                        Alta_Recoleccion(); //Da de alta los datos proporcionados por el usuario
                        Habilitar_Controles("Inicial"); //Habiluta y deshabilita los controles de la form para su próxima operación
                        Impresion_Recoleccion_Caja();//Imprime el recibo de recoleccion
                        Txt_Fecha_Recoleccion_Busqueda.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                        Consulta_Recolecciones_Empleado();
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Proporcione el monto o quien recibe efectivo de lo que se pretende recolectar en caja";
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
        protected void Btn_Buscar_Recolecciones_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                if (string.IsNullOrEmpty(Txt_Fecha_Recoleccion_Busqueda.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Fecha de Busqueda es un dato requerido por el sistema. <br>";
                }
                else
                {
                    Consulta_Recolecciones_Empleado(); //Consulta las recolecciones que coincidan con la descipción porporcionada por el usuario
                    //Si no se encontraron recolecciones con entonces manda un mensaje al usuario
                    if (Grid_Recolecciones_Caja.Rows.Count <= 0)
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "No se encontraron Recolecciones con la /// FECHA proporcionada <br>";
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
                if (Btn_Salir.ToolTip == "Salir")
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    Session.Remove("Consulta_Recolecciones");
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
        protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(Txt_Monto_Recolectado.Text)) Impresion_Recoleccion_Caja();//Imprime los datos del recibo
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