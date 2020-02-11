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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Tipo_Solicitud_Pagos.Negocios;
using Presidencia.Solicitud_Pagos.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Catalogo_Compras_Proveedores.Negocio;

public partial class paginas_Contabilidad_Frm_Ope_Con_Solicitud_Pagos : System.Web.UI.Page
{
    #region (Page Load)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Refresca la sesion del usuario logeado en el sistema
                Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
                //Valida que existe un usuario logueado en el sistema
                if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

                if (!IsPostBack)
                {
                    Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    ViewState["SortDirection"] = "ASC";

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
            /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda
            ///               realizar diferentes operaciones
            /// PARAMETROS  : 
            /// CREO        : Yazmin Abigail Delgado Gómez
            /// FECHA_CREO  : 18-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Inicializa_Controles()
            {
                try
                {
                    Consultar_Tipos_Solicitud();    //Consulta todos los tipos de solicitud que esten dados de alta y que se encuentren activos
                    Consulta_Reservas();            //Consulta las reservas
                    Consulta_Dependencia();         //Consulta las dependencias que fueron dadas de alta con anterioridad
                    Limpia_Controles();             //Limpia los controles del forma
                    Habilitar_Controles("Inicial"); //Inicializa todos los controles
                }
                catch (Exception ex)
                {
                    throw new Exception("Inicializa_Controles " + ex.Message.ToString());
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Limpiar_Controles
            /// DESCRIPCION : Limpia los controles que se encuentran en la forma
            /// PARAMETROS  : 
            /// CREO        : Yazmin Abigail Delgado Gómez
            /// FECHA_CREO  : 18-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpia_Controles()
            {
                try
                {
                    Cmb_Tipo_Solicitud_Pago.SelectedIndex = -1;
                    Cmb_Reserva_Pago.SelectedIndex = -1;
                    Cmb_Busqueda_Dependencia.SelectedIndex = -1;
                    Cmb_Busqueda_Estatus_Solicitud_Pago.SelectedIndex = -1;
                    Cmb_Busqueda_Tipo_Solicitud.SelectedIndex = -1;
                    Cmb_Proveedor_Solicitud_Pago.DataSource = new DataTable();
                    Cmb_Proveedor_Solicitud_Pago.DataBind();
                    Txt_Busqueda_Fecha_Fin.Text = "";
                    Txt_Busqueda_Fecha_Inicio.Text = "";
                    Txt_Busqueda_No_Reserva.Text = "";
                    Txt_Busqueda_No_Solicitud_Pago.Text = "";
                    Txt_Area_Funcional_Reserva.Text = "";
                    Txt_Codigo_Programatico_Reserva.Text = "";
                    Txt_Concepto_Reserva.Text = "";
                    Txt_Concepto_Solicitud_Pago.Text = "";
                    Txt_Dependencia_Reserva.Text = "";
                    Txt_Estatus_solicitud_Pago.Text = "";
                    Txt_Fecha_Factura_Solicitud_Pago.Text = "";
                    Txt_Fecha_Solicitud_Pago.Text = "";
                    Txt_Fuente_Financiamiento_Reserva.Text = "";
                    Txt_Monto_Solicitud_Pago.Text = "";
                    Txt_No_Factura_Solicitud_Pago.Text = "";
                    Txt_No_Solicitud_Pago.Text = "";
                    Txt_Nombre_Proveedor_Solicitud_Pago.Text = "";
                    Txt_Partida_Reserva.Text = "";
                    Txt_Proyecto_Programa_Reserva.Text = "";
                    Txt_Saldo_Reserva.Text = "";
                    Txt_No_Reserva_Anterior.Value = "";
                    Txt_Monto_Solicitud_Anterior.Value = "";
                    Txt_Cuenta_Contable_ID_Proveedor.Value = "";
                    Txt_Cuenta_Contable_ID_Proveedor_Anterior.Value = "";
                    Txt_Cuenta_Contable_ID.Value = "";
                    Txt_Cuenta_Contable_ID_Anterior.Value = "";
                }
                catch (Exception ex)
                {
                    throw new Exception("Limpia_Controles " + ex.Message.ToString());
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Limpia_Datos_Reserva
            /// DESCRIPCION : Limpia los controles que pertenecen a la reserva
            /// PARAMETROS  : 
            /// CREO        : Yazmin Abigail Delgado Gómez
            /// FECHA_CREO  : 18-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpia_Datos_Reserva()
            {
                Txt_Saldo_Reserva.Text = "";
                Txt_Concepto_Reserva.Text = "";
                Txt_Fuente_Financiamiento_Reserva.Text = "";
                Txt_Area_Funcional_Reserva.Text = "";
                Txt_Proyecto_Programa_Reserva.Text = "";
                Txt_Dependencia_Reserva.Text = "";
                Txt_Partida_Reserva.Text = "";
                Txt_Codigo_Programatico_Reserva.Text = "";
                Txt_Cuenta_Contable_ID.Value = "";
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Habilitar_Controles
            /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
            ///                para a siguiente operación
            /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
            ///                           si es una alta, modificacion
            /// CREO        : Yazmin Abigail Delgado Gómez
            /// FECHA_CREO  : 18-Noviembre-2011
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
                            Btn_Nuevo.Visible = true;
                            Btn_Modificar.Visible = true;
                            //Btn_Eliminar.Visible = true;
                            Btn_Nuevo.CausesValidation = false;
                            Btn_Modificar.CausesValidation = false;
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                            Configuracion_Acceso("Frm_Ope_Con_Solicitud_Pagos.aspx");
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
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
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
                            break;
                    }
                    Cmb_Tipo_Solicitud_Pago.Enabled = Habilitado;
                    Cmb_Reserva_Pago.Enabled = Habilitado;
                    Cmb_Proveedor_Solicitud_Pago.Enabled = Habilitado;
                    Txt_No_Factura_Solicitud_Pago.Enabled = Habilitado;
                    Txt_Fecha_Factura_Solicitud_Pago.Enabled = Habilitado;
                    Txt_Monto_Solicitud_Pago.Enabled = Habilitado;
                    Txt_Nombre_Proveedor_Solicitud_Pago.Enabled = Habilitado;
                    Txt_Concepto_Solicitud_Pago.Enabled = Habilitado;
                    Btn_Fecha_Factura_Solicitud_Pago.Enabled = Habilitado;
                    Btn_Buscar_Proveedor_Solicitud_Pagos.Enabled = Habilitado;
                    Btn_Mostrar_Popup_Busqueda.Enabled = !Habilitado;
                    Grid_Solicitud_Pagos.Enabled = !Habilitado;
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
            ///******************************************************************************
            /// NOMBRE: Configuracion_Acceso
            /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
            /// PARÁMETROS  :
            /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
            /// FECHA CREÓ  : 23/Mayo/2011 10:43 a.m.
            /// USUARIO MODIFICO  :
            /// FECHA MODIFICO    :
            /// CAUSA MODIFICACIÓN:
            ///******************************************************************************
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
        #region (Método Consulta)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consultar_Tipos_Solicitud
            /// DESCRIPCION : Llena el Cmb_Tipo_Solicitud_Pago con los tipos de solicitud de pago.
            /// PARAMETROS  : 
            /// CREO        : Yazmin Abigail Delgado Gómez
            /// FECHA_CREO  : 18-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consultar_Tipos_Solicitud()
            {
                Cls_Cat_Con_Tipo_Solicitud_Pagos_Negocio Rs_Cat_Con_Tipo_Solicitud_Pagos = new Cls_Cat_Con_Tipo_Solicitud_Pagos_Negocio(); //Variable de conexión hacia la capa de negocios
                DataTable Dt_Tipo_Solicitud; //Variable a obtener los datos de la consulta
                try
                {   

                    Dt_Tipo_Solicitud = Rs_Cat_Con_Tipo_Solicitud_Pagos.Consulta_Tipo_Solicitud_Pagos_Combo(); //Consulta los tipos de solicitud que fueron dados de alta en la base de datos

                    Cmb_Tipo_Solicitud_Pago.DataSource = Dt_Tipo_Solicitud;
                    Cmb_Tipo_Solicitud_Pago.DataTextField = Cat_Con_Tipo_Solicitud_Pagos.Campo_Descripcion;
                    Cmb_Tipo_Solicitud_Pago.DataValueField = Cat_Con_Tipo_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID;
                    Cmb_Tipo_Solicitud_Pago.DataBind();
                    Cmb_Tipo_Solicitud_Pago.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
                    Cmb_Tipo_Solicitud_Pago.SelectedIndex = -1;

                    Cmb_Busqueda_Tipo_Solicitud.DataSource = Dt_Tipo_Solicitud;
                    Cmb_Busqueda_Tipo_Solicitud.DataTextField = Cat_Con_Tipo_Solicitud_Pagos.Campo_Descripcion;
                    Cmb_Busqueda_Tipo_Solicitud.DataValueField = Cat_Con_Tipo_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID;
                    Cmb_Busqueda_Tipo_Solicitud.DataBind();
                    Cmb_Busqueda_Tipo_Solicitud.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
                    Cmb_Busqueda_Tipo_Solicitud.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    throw new Exception("Consultar_Tipos_Solicitud " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Dependencia
            /// DESCRIPCION : Consulta las dependecias que fueron dados de alta con anterioridad
            /// PARAMETROS  : 
            /// CREO        : Yazmin Abigail Delgado Gómez
            /// FECHA_CREO  : 19-Noviembre-2011s
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Dependencia()
            {
                Cls_Cat_Dependencias_Negocio Rs_Cat_Dependencias = new Cls_Cat_Dependencias_Negocio(); //Variable de conexión hacia la capa de negocios
                DataTable Dt_Dependecias; //Variable a obtener los datos de la consulta
                try
                {
                    Dt_Dependecias = Rs_Cat_Dependencias.Consulta_Dependencias(); //Consulta las dependencias que fueron dadas de alta con anterioridad

                    Cmb_Busqueda_Dependencia.DataSource = Dt_Dependecias;
                    Cmb_Busqueda_Dependencia.DataTextField = Cat_Dependencias.Campo_Nombre;
                    Cmb_Busqueda_Dependencia.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
                    Cmb_Busqueda_Dependencia.DataBind();
                    Cmb_Busqueda_Dependencia.Items.Insert(0, new ListItem("<- Seleccione ->",""));
                    Cmb_Busqueda_Dependencia.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Dependencia " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Reservas
            /// DESCRIPCION : Llena el Cmb_Reserva_Pago con todas las reservas que tiene todavia
            ///               saldo.
            /// PARAMETROS  : 
            /// CREO        : Yazmisn Abigail Delgado Gómez
            /// FECHA_CREO  : 18-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Reservas()
            {
                Cls_Ope_Con_Solicitud_Pagos_Negocio Rs_Consulta_Ope_PSP_Reservas = new Cls_Ope_Con_Solicitud_Pagos_Negocio(); //Variable de conexi{on hacia la capa de negocios
                DataTable Dt_Reservas; //Variable a obtener los datos de la consulta
                try
                {
                    Session.Remove("Consulta_Reservas");
                    Dt_Reservas = Rs_Consulta_Ope_PSP_Reservas.Consulta_Reservas(); //Consulta todas las reservas que aun tenga saldo
                    Cmb_Reserva_Pago.DataSource = new DataTable();
                    Cmb_Reserva_Pago.DataBind();
                    Cmb_Reserva_Pago.DataSource = Dt_Reservas;
                    Session["Consulta_Reservas"] = Dt_Reservas;
                    Cmb_Reserva_Pago.DataTextField = "Reserva";
                    Cmb_Reserva_Pago.DataValueField = Ope_Psp_Reservas.Campo_No_Reserva;
                    Cmb_Reserva_Pago.DataBind();
                    Cmb_Reserva_Pago.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
                    Cmb_Reserva_Pago.SelectedIndex = -1;

                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Reservas " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Reserva
            /// DESCRIPCION : Consulta todos los datos que corresponden al número de reserva
            ///               que fue seleccionada por el usuario
            /// PARAMETROS  : No_Reserva : Indica el No de Reserva a obtener los datos de la
            ///                             base de datos
            /// CREO        : Yazmisn Abigail Delgado Gómez
            /// FECHA_CREO  : 18-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Datos_Reserva(Double No_Reserva)
            {
                Cls_Ope_Con_Solicitud_Pagos_Negocio Rs_Consulta_Ope_PSP_Reservas = new Cls_Ope_Con_Solicitud_Pagos_Negocio(); //Variable de conexión hacia la capa de negocios
                DataTable Dt_Datos_Reserva; //Variable a obtener los datos de la consulta
                try
                {
                    Rs_Consulta_Ope_PSP_Reservas.P_No_Reserva = No_Reserva;
                    Dt_Datos_Reserva = Rs_Consulta_Ope_PSP_Reservas.Consulta_Datos_Reserva(); //Consulta todos los datos correspondientes a la reserva que el usuario selecciono

                    foreach (DataRow Registro in Dt_Datos_Reserva.Rows)
                    {
                        Txt_Concepto_Reserva.Text = Registro["Reservado"].ToString();
                        Txt_Nombre_Proveedor_Solicitud_Pago.Text = Registro[Ope_Psp_Reservas.Campo_Beneficiario].ToString();
                        Txt_Saldo_Reserva.Text = Registro[Ope_Psp_Reservas.Campo_Saldo].ToString();
                        Txt_Codigo_Programatico_Reserva.Text = Registro["Codigo_Programatico"].ToString();
                        Txt_Fuente_Financiamiento_Reserva.Text = Registro["Fuente_Financiamiento"].ToString();
                        Txt_Area_Funcional_Reserva.Text = Registro["Area_Funcional"].ToString();
                        Txt_Proyecto_Programa_Reserva.Text = Registro["Proyectos_Programas"].ToString();
                        Txt_Dependencia_Reserva.Text = Registro["Dependencia"].ToString();
                        Txt_Partida_Reserva.Text = Registro["Partida"].ToString();
                        Txt_Cuenta_Contable_ID.Value = Registro[Cat_Sap_Partidas_Especificas.Campo_Cuenta_Contable_ID].ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Datos_Reserva " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Solicitudes_Pagos
            /// DESCRIPCION : Consulta las Solicitudfes de pago que coincidan con los parametros
            ///               proporcionados por el empleado
            /// PARAMETROS  :
            /// CREO        : Yazmisn Abigail Delgado Gómez
            /// FECHA_CREO  : 18-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Solicitudes_Pagos()
            {
                Cls_Ope_Con_Solicitud_Pagos_Negocio Rs_Consulta_Ope_Con_Solicitud_Pagos = new Cls_Ope_Con_Solicitud_Pagos_Negocio(); //Variable de conexión hacia la capa de negocios
                DataTable Dt_Solicitudes_Pagos; //Variable que va a contener los datos de la consulta realizada
                try
                {
                    Session.Remove("Consulta_Solicitud_Pagos");
                    if(!String.IsNullOrEmpty(Txt_Busqueda_No_Reserva.Text)) Rs_Consulta_Ope_Con_Solicitud_Pagos.P_No_Reserva = Convert.ToDouble(Txt_Busqueda_No_Reserva.Text);
                    if (!String.IsNullOrEmpty(Txt_Busqueda_No_Solicitud_Pago.Text)) Rs_Consulta_Ope_Con_Solicitud_Pagos.P_No_Solicitud_Pago = String.Format("{0:0000000000}", Convert.ToDouble(Txt_Busqueda_No_Solicitud_Pago.Text));
                    if(Cmb_Busqueda_Estatus_Solicitud_Pago.SelectedIndex > 0) Rs_Consulta_Ope_Con_Solicitud_Pagos.P_Estatus = Cmb_Busqueda_Estatus_Solicitud_Pago.SelectedValue;
                    if(Cmb_Busqueda_Tipo_Solicitud.SelectedIndex > 0) Rs_Consulta_Ope_Con_Solicitud_Pagos.P_Tipo_Solicitud_Pago_ID = Cmb_Busqueda_Tipo_Solicitud.SelectedValue;
                    if(Cmb_Busqueda_Dependencia.SelectedIndex > 0) Rs_Consulta_Ope_Con_Solicitud_Pagos.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedValue;
                    if (!String.IsNullOrEmpty(Txt_Busqueda_Fecha_Inicio.Text) && !String.IsNullOrEmpty(Txt_Busqueda_Fecha_Fin.Text))
                    {
                        Rs_Consulta_Ope_Con_Solicitud_Pagos.P_Fecha_Inicial = String.Format("{0:dd/MM/yyyy}", Txt_Busqueda_Fecha_Inicio.Text);
                        Rs_Consulta_Ope_Con_Solicitud_Pagos.P_Fecha_Final = String.Format("{0:dd/MM/yyyy}", Txt_Busqueda_Fecha_Fin.Text);
                    }                    
                    Dt_Solicitudes_Pagos = Rs_Consulta_Ope_Con_Solicitud_Pagos.Consultar_Solicitud_Pago(); //Consulta las solicitudes de pagos que coinciden con los parámetros seleccionados por el usuario
                    Limpia_Controles();
                    Session["Consulta_Solicitud_Pagos"] = Dt_Solicitudes_Pagos;
                    Llena_Grid_Solicitudes_Pagos(); //Agrega las Solicitudes de Pagos obtenidas de la consulta anterior
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Solicitudes_Pagos " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Llena_Grid_Solicitudes_Pagos
            /// DESCRIPCION : Llena el grid con las Solicitudes de pago que fueron consultadas
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 19-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Llena_Grid_Solicitudes_Pagos()
            {
                DataTable Dt_Solicitudes_Pagos; //Variable que obtendra los datos de la consulta 
                try
                {
                    Grid_Solicitud_Pagos.DataBind();
                    Dt_Solicitudes_Pagos = (DataTable)Session["Consulta_Solicitud_Pagos"];
                    Grid_Solicitud_Pagos.DataSource = Dt_Solicitudes_Pagos;
                    Grid_Solicitud_Pagos.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Llena_Grid_Solicitudes_Pagos " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Solicitud_Pago
            /// DESCRIPCION : Consulta los datos de la solicitud de pago que fue seleccionada
            ///               por el usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 19-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Datos_Solicitud_Pago(String No_Solicitud_Pago)
            {
                Cls_Ope_Con_Solicitud_Pagos_Negocio Rs_Consulta_Ope_Con_Solicitud_Pagos = new Cls_Ope_Con_Solicitud_Pagos_Negocio(); //Variable de conexión hacia la capa de negocios
                DataTable Dt_Solicitud_Pago; //Variable a contener los datos de la consulta
                DataTable Dt_Reservaciones = new DataTable(); //Obtiene las reservaciones que se tiene consultadas 
                try
                {   
                    Rs_Consulta_Ope_Con_Solicitud_Pagos.P_No_Solicitud_Pago = No_Solicitud_Pago;
                    Dt_Solicitud_Pago = Rs_Consulta_Ope_Con_Solicitud_Pagos.Consulta_Datos_Solicitud_Pago(); //Consulta todos los datos de la solicitud que fue seleccionado por el usuario
                    Limpia_Controles();
                    if (Dt_Solicitud_Pago.Rows.Count > 0)
                    {
                        Cmb_Proveedor_Solicitud_Pago.DataSource = new DataTable();
                        Cmb_Proveedor_Solicitud_Pago.DataBind();
                        Cmb_Proveedor_Solicitud_Pago.DataSource = Dt_Solicitud_Pago;
                        Cmb_Proveedor_Solicitud_Pago.DataTextField = Cat_Com_Proveedores.Campo_Nombre;
                        Cmb_Proveedor_Solicitud_Pago.DataValueField = Cat_Com_Proveedores.Campo_Proveedor_ID;
                        Cmb_Proveedor_Solicitud_Pago.DataBind();
                        Cmb_Proveedor_Solicitud_Pago.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
                        Cmb_Proveedor_Solicitud_Pago.SelectedIndex = 1;
                        //Agrega los datos obtenidos de la consulta en los controles correspondientes para poder mostrar estos al usuario
                        foreach (DataRow Registro in Dt_Solicitud_Pago.Rows)
                        {
                            //Datos generales de la solicitud
                            Txt_No_Solicitud_Pago.Text = Registro[Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago].ToString();
                            Txt_Estatus_solicitud_Pago.Text = Registro[Ope_Con_Solicitud_Pagos.Campo_Estatus].ToString();
                            Txt_Fecha_Solicitud_Pago.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Ope_Con_Solicitud_Pagos.Campo_Fecha_Solicitud].ToString()));
                            Cmb_Tipo_Solicitud_Pago.SelectedValue = Registro[Ope_Con_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID].ToString();
                            //Datos de la solicitud
                            Txt_No_Factura_Solicitud_Pago.Text = Registro[Ope_Con_Solicitud_Pagos.Campo_No_Factura].ToString();
                            Txt_Fecha_Factura_Solicitud_Pago.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Ope_Con_Solicitud_Pagos.Campo_Fecha_Factura].ToString()));
                            Txt_Monto_Solicitud_Pago.Text = Registro[Ope_Con_Solicitud_Pagos.Campo_Monto].ToString();
                            Txt_Monto_Solicitud_Anterior.Value = Registro[Ope_Con_Solicitud_Pagos.Campo_Monto].ToString();
                            Txt_Concepto_Solicitud_Pago.Text = Registro[Ope_Con_Solicitud_Pagos.Campo_Concepto].ToString();
                            //Datos de la reservación
                            Txt_No_Reserva_Anterior.Value = Registro[Ope_Con_Solicitud_Pagos.Campo_No_Reserva].ToString();
                            Dt_Reservaciones = (DataTable)Session["Consulta_Reservas"];
                            Cmb_Reserva_Pago.SelectedIndex = -1;
                            if (Dt_Reservaciones.Rows.Count > 0)
                            {
                                foreach (DataRow Reserva in Dt_Reservaciones.Rows)
                                {
                                    if (Registro[Ope_Con_Solicitud_Pagos.Campo_No_Reserva].ToString() == Reserva[Ope_Con_Solicitud_Pagos.Campo_No_Reserva].ToString()) Cmb_Reserva_Pago.SelectedValue = Registro[Ope_Con_Solicitud_Pagos.Campo_No_Reserva].ToString();
                                }
                                if (Cmb_Reserva_Pago.SelectedIndex <= 0)
                                {
                                    Cmb_Reserva_Pago.Items.Insert(Dt_Reservaciones.Rows.Count + 1, new ListItem(Registro["Reserva"].ToString(), Registro[Ope_Con_Solicitud_Pagos.Campo_No_Reserva].ToString()));
                                    Cmb_Reserva_Pago.SelectedIndex = Dt_Reservaciones.Rows.Count;
                                }
                            }
                            Txt_Cuenta_Contable_ID_Proveedor.Value = Registro[Cat_Com_Proveedores.Campo_Cuenta_Contable_ID].ToString();
                            Txt_Cuenta_Contable_ID_Proveedor_Anterior.Value = Registro[Cat_Com_Proveedores.Campo_Cuenta_Contable_ID].ToString();

                            Consulta_Datos_Reserva(Convert.ToDouble(Registro[Ope_Con_Solicitud_Pagos.Campo_No_Reserva].ToString())); //Consulta los datos de la reserva que fue asignada a la solicitud
                            Txt_Cuenta_Contable_ID_Anterior.Value = Txt_Cuenta_Contable_ID.Value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Datos_Solicitud_Pago " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Proveedores
            /// DESCRIPCION : Consulta a todos los proveedores que coincidan con el nombre, rfc
            ///               o compañia de acuerdo a lo proporcionado por el usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 23-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Proveedores()
            {
                Cls_Cat_Com_Proveedores_Negocio Rs_Consulta_Cat_Com_Proveedores = new Cls_Cat_Com_Proveedores_Negocio(); //Variable de conexión hacia la capa de negocios
                DataTable Dt_Proveedores; //Obtiene la lista de proveedores qe coincidan con el RFC o Nombre que proporciono el usuario
                try
                {
                    Rs_Consulta_Cat_Com_Proveedores.P_Busqueda = Txt_Nombre_Proveedor_Solicitud_Pago.Text;
                    Dt_Proveedores = Rs_Consulta_Cat_Com_Proveedores.Consulta_Proveedores(); //Consulta los proveedores que coincidan con el nombre, compañia, rfc
                    Cmb_Proveedor_Solicitud_Pago.DataSource = new DataTable();
                    Cmb_Proveedor_Solicitud_Pago.DataBind();
                    Cmb_Proveedor_Solicitud_Pago.DataSource = Dt_Proveedores;
                    Cmb_Proveedor_Solicitud_Pago.DataTextField = Cat_Com_Proveedores.Campo_Nombre;
                    Cmb_Proveedor_Solicitud_Pago.DataValueField = Cat_Com_Proveedores.Campo_Proveedor_ID;
                    Cmb_Proveedor_Solicitud_Pago.DataBind();
                    Cmb_Proveedor_Solicitud_Pago.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
                    Cmb_Proveedor_Solicitud_Pago.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Proveedores " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Cuenta_Contable_Proveedor
            /// DESCRIPCION : Consulta la cuenta contable que tiene asignado el proveedor
            /// PARAMETROS  : Proveedor_ID: ID del proveedor del cual se pretende consultar
            ///               la cuenta contable
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 23-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Cuenta_Contable_Proveedor(String Proveedor_ID)
            {
                Cls_Ope_Con_Solicitud_Pagos_Negocio Rs_Consulta_Cat_Com_Proveedores = new Cls_Ope_Con_Solicitud_Pagos_Negocio(); //Variable de conexión hacia la capa de negocios
                DataTable Dt_Cuenta_Contable_Proveedor = new DataTable(); //Obtiene el ID de la cuenta contable que pertenece al proveedor
                try
                {
                    Txt_Cuenta_Contable_ID_Proveedor.Value = "";
                    Rs_Consulta_Cat_Com_Proveedores.P_Proveedor_ID = Proveedor_ID;
                    Dt_Cuenta_Contable_Proveedor = Rs_Consulta_Cat_Com_Proveedores.Consulta_Cuenta_Contable_Proveedor(); //Consulta el ID de la cuenta contable
                    //Agrega la cuenta contable ID que tiene asignado el proveedor en el control correspondiente
                    foreach (DataRow Registro in Dt_Cuenta_Contable_Proveedor.Rows)
                    {
                        Txt_Cuenta_Contable_ID_Proveedor.Value = Registro[Cat_Com_Proveedores.Campo_Cuenta_Contable_ID].ToString();
                    }
                    
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Cuenta_Contable_Proveedor " + ex.Message.ToString(), ex);
                }
            }
        #endregion
        #region (Metodos Validacion)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Validar_Datos_Solicitud_Pagos
            /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
            /// CREO        : Yazmin Abigail Delgado Gómez
            /// FECHA_CREO  : 18-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Validar_Datos_Solicitud_Pagos()
            {
                String Espacios_Blanco;
                Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
                try
                {
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

                    if (String.IsNullOrEmpty(Txt_No_Factura_Solicitud_Pago.Text.Trim()))
                    {
                        Lbl_Mensaje_Error.Text += Espacios_Blanco + " + El No de Documento es un dato requerido por el sistema. <br>";
                        Datos_Validos = false;
                    }
                    if (String.IsNullOrEmpty(Txt_Fecha_Factura_Solicitud_Pago.Text.Trim()))
                    {
                        Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Fecha del Documento es un dato requerido por el sistema. <br>";
                        Datos_Validos = false;
                    }
                    if (String.IsNullOrEmpty(Txt_Monto_Solicitud_Pago.Text.Trim()))
                    {
                        Lbl_Mensaje_Error.Text += Espacios_Blanco + " + El Monto a solicitar para el pago es un dato requerido por el sistema. <br>";
                        Datos_Validos = false;
                    }
                    else
                    {
                        if (Btn_Nuevo.ToolTip!="Nuevo")
                        {
                            if (Convert.ToDouble(Txt_Saldo_Reserva.Text) < Convert.ToDouble(Txt_Monto_Solicitud_Pago.Text.Replace(",", "").Replace("$", "")))
                            {
                                Lbl_Mensaje_Error.Text += Espacios_Blanco + " + El Monto a Solicitar no debe ser mayor al Saldo de la Reserva. <br>";
                                Datos_Validos = false;
                            }
                        }
                        else
                        {
                            if (Txt_No_Reserva_Anterior.Value != Cmb_Reserva_Pago.SelectedValue)
                            {
                                if (Convert.ToDouble(Txt_Saldo_Reserva.Text) < Convert.ToDouble(Txt_Monto_Solicitud_Pago.Text.Replace(",", "").Replace("$", "")))
                                {
                                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + El Monto a Solicitar no debe ser mayor al Saldo de la Reserva. <br>";
                                    Datos_Validos = false;
                                }
                            }
                            else
                            {
                                Double Saldo = 0; //Variable a Contener el saldo final de la reserva
                                Saldo+= Convert.ToDouble(Txt_Saldo_Reserva.Text) + Convert.ToDouble(Txt_Monto_Solicitud_Anterior.Value);
                                if (Saldo < Convert.ToDouble(Txt_Monto_Solicitud_Pago.Text.Replace(",", "").Replace("$", "")))
                                {
                                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + El Monto a Solicitar no debe ser mayor al Saldo de la Reserva. <br>";
                                    Datos_Validos = false;
                                }
                            }
                        }
                    }
                    if (Cmb_Proveedor_Solicitud_Pago.SelectedIndex <= 0)
                    {
                        Lbl_Mensaje_Error.Text += Espacios_Blanco + " + El Proveedor es un dato requerido por el sistema. <br>";
                        Datos_Validos = false;
                    }
                    if (String.IsNullOrEmpty(Txt_Concepto_Solicitud_Pago.Text.Trim()))
                    {
                        Lbl_Mensaje_Error.Text += Espacios_Blanco + " + El Concepto de la Solicitud es un dato requerido por el sistema. <br>";
                        Datos_Validos = false;
                    }
                    if (Cmb_Tipo_Solicitud_Pago.SelectedIndex == 0)
                    {
                        Lbl_Mensaje_Error.Text += Espacios_Blanco + " + El Tipo de Solicitud es un dato requerido por el sistema. <br>";
                        Datos_Validos = false;
                    }
                    if (Cmb_Reserva_Pago.SelectedIndex == 0)
                    {
                        Lbl_Mensaje_Error.Text += Espacios_Blanco + " + El No. de Reserva es un dato requerido por el sistema. <br>";
                        Datos_Validos = false;
                    }
                    return Datos_Validos;
                }
                catch (Exception ex)
                {
                    throw new Exception("Validar_Datos_Solicitud_Pagos " + ex.Message.ToString(), ex);
                }
            }
        #endregion
        #region (Métodos Operación)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Alta_Solicitud_Pagos
            /// DESCRIPCION : Da de Alta la Solicitud del Pago con los datos proporcionados por 
            ///               el usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 18-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************      
            private void Alta_Solicitud_Pagos()
            {
                Cls_Ope_Con_Solicitud_Pagos_Negocio Rs_Alta_Ope_Con_Solicitud_Pagos = new Cls_Ope_Con_Solicitud_Pagos_Negocio(); //Variable de conexión hacia la capa de negocio
                DataTable Dt_Partidas_Polizas = new DataTable(); //Obtiene los detalles de la póliza que se debera generar para el movimiento
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
                    row[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Txt_Cuenta_Contable_ID.Value;
                    row[Ope_Con_Polizas_Detalles.Campo_Concepto] = Txt_Concepto_Solicitud_Pago.Text.ToString();
                    row[Ope_Con_Polizas_Detalles.Campo_Debe] = Convert.ToDouble(Txt_Monto_Solicitud_Pago.Text.ToString());
                    row[Ope_Con_Polizas_Detalles.Campo_Haber] = 0;

                    Dt_Partidas_Polizas.Rows.Add(row); //Agrega el registro creado con todos sus valores a la tabla
                    Dt_Partidas_Polizas.AcceptChanges();

                    row = Dt_Partidas_Polizas.NewRow();
                    //Agrega el abono del registro de la póliza
                    row[Ope_Con_Polizas_Detalles.Campo_Partida] = 2;
                    row[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Txt_Cuenta_Contable_ID_Proveedor.Value;
                    row[Ope_Con_Polizas_Detalles.Campo_Concepto] = Txt_Concepto_Solicitud_Pago.Text.ToString();
                    row[Ope_Con_Polizas_Detalles.Campo_Debe] = 0;
                    row[Ope_Con_Polizas_Detalles.Campo_Haber] = Convert.ToDouble(Txt_Monto_Solicitud_Pago.Text.ToString());

                    Dt_Partidas_Polizas.Rows.Add(row); //Agrega el registro creado con todos sus valores a la tabla
                    Dt_Partidas_Polizas.AcceptChanges();
                    //Agrega los valores a pasar a la capa de negocios para ser dados de alta
                    Rs_Alta_Ope_Con_Solicitud_Pagos.P_Dt_Detalles_Poliza = Dt_Partidas_Polizas;
                    Rs_Alta_Ope_Con_Solicitud_Pagos.P_Tipo_Solicitud_Pago_ID = Cmb_Tipo_Solicitud_Pago.SelectedValue;
                    Rs_Alta_Ope_Con_Solicitud_Pagos.P_No_Reserva = Convert.ToDouble(Cmb_Reserva_Pago.SelectedValue);
                    Rs_Alta_Ope_Con_Solicitud_Pagos.P_Proveedor_ID = Cmb_Proveedor_Solicitud_Pago.SelectedValue;
                    Rs_Alta_Ope_Con_Solicitud_Pagos.P_Beneficiario = Cmb_Proveedor_Solicitud_Pago.SelectedItem.Text;
                    Rs_Alta_Ope_Con_Solicitud_Pagos.P_Concepto = Txt_Concepto_Solicitud_Pago.Text;
                    Rs_Alta_Ope_Con_Solicitud_Pagos.P_Monto = Convert.ToDouble(Txt_Monto_Solicitud_Pago.Text.Replace(",", "").Replace("$", ""));
                    Rs_Alta_Ope_Con_Solicitud_Pagos.P_No_Factura = Txt_No_Factura_Solicitud_Pago.Text;
                    Rs_Alta_Ope_Con_Solicitud_Pagos.P_Fecha_Factura = String.Format("{0:dd/MM/yy}", Convert.ToDateTime(Txt_Fecha_Factura_Solicitud_Pago.Text));
                    Rs_Alta_Ope_Con_Solicitud_Pagos.P_Estatus = "PENDIENTE";
                    Rs_Alta_Ope_Con_Solicitud_Pagos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Rs_Alta_Ope_Con_Solicitud_Pagos.Alta_Solicitud_Pago(); //Da de alto los datos de la Solicitud de Pago en la BD                    
                    
                    Inicializa_Controles();                                //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Solicitud de Pago", "alert('El Alta de la Solicitud de Pago fue Exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Alta_Solicitud_Pagos " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Modificar_Solicitud_Pago
            /// DESCRIPCION : Modifica los datos de la Solicitud del Pago con los datos 
            ///               proporcionados por el usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 20-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Modificar_Solicitud_Pago()
            {
                DataTable Dt_Partidas_Polizas = new DataTable(); //Obtiene los detalles de la póliza que se debera generar para el movimiento
                Cls_Ope_Con_Solicitud_Pagos_Negocio Rs_Modificar_Ope_Con_Solicitud_Pagos = new Cls_Ope_Con_Solicitud_Pagos_Negocio(); //Variable de conexión hacia la capa de Negocios
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
                    row[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Txt_Cuenta_Contable_ID.Value;
                    row[Ope_Con_Polizas_Detalles.Campo_Concepto] = Txt_Concepto_Solicitud_Pago.Text.ToString();
                    row[Ope_Con_Polizas_Detalles.Campo_Debe] = Convert.ToDouble(Txt_Monto_Solicitud_Pago.Text.ToString());
                    row[Ope_Con_Polizas_Detalles.Campo_Haber] = 0;
                    Dt_Partidas_Polizas.Rows.Add(row); //Agrega el registro creado con todos sus valores a la tabla
                    Dt_Partidas_Polizas.AcceptChanges();

                    row = Dt_Partidas_Polizas.NewRow();
                    //Agrega el abono del registro de la póliza
                    row[Ope_Con_Polizas_Detalles.Campo_Partida] = 2;
                    row[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Txt_Cuenta_Contable_ID_Proveedor.Value;
                    row[Ope_Con_Polizas_Detalles.Campo_Concepto] = Txt_Concepto_Solicitud_Pago.Text.ToString();
                    row[Ope_Con_Polizas_Detalles.Campo_Debe] = 0;
                    row[Ope_Con_Polizas_Detalles.Campo_Haber] = Convert.ToDouble(Txt_Monto_Solicitud_Pago.Text.ToString());
                    Dt_Partidas_Polizas.Rows.Add(row); //Agrega el registro creado con todos sus valores a la tabla
                    Dt_Partidas_Polizas.AcceptChanges();

                    row = Dt_Partidas_Polizas.NewRow();
                    //Agrega el abono del registro de la póliza
                    row[Ope_Con_Polizas_Detalles.Campo_Partida] = 3;
                    row[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Txt_Cuenta_Contable_ID_Anterior.Value;
                    row[Ope_Con_Polizas_Detalles.Campo_Concepto] = Txt_Concepto_Solicitud_Pago.Text.ToString();
                    row[Ope_Con_Polizas_Detalles.Campo_Debe] = 0;
                    row[Ope_Con_Polizas_Detalles.Campo_Haber] = Convert.ToDouble(Txt_Monto_Solicitud_Anterior.Value.ToString());
                    Dt_Partidas_Polizas.Rows.Add(row); //Agrega el registro creado con todos sus valores a la tabla
                    Dt_Partidas_Polizas.AcceptChanges();

                    row = Dt_Partidas_Polizas.NewRow();
                    //Agrega el abono del registro de la póliza
                    row[Ope_Con_Polizas_Detalles.Campo_Partida] = 4;
                    row[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Txt_Cuenta_Contable_ID_Proveedor_Anterior.Value;
                    row[Ope_Con_Polizas_Detalles.Campo_Concepto] = Txt_Concepto_Solicitud_Pago.Text.ToString();
                    row[Ope_Con_Polizas_Detalles.Campo_Debe] = Convert.ToDouble(Txt_Monto_Solicitud_Anterior.Value.ToString());
                    row[Ope_Con_Polizas_Detalles.Campo_Haber] = 0;
                    Dt_Partidas_Polizas.Rows.Add(row); //Agrega el registro creado con todos sus valores a la tabla
                    Dt_Partidas_Polizas.AcceptChanges();

                    //Agrega los valores a pasar a la capa de negocios para ser dados de alta
                    Rs_Modificar_Ope_Con_Solicitud_Pagos.P_Dt_Detalles_Poliza = Dt_Partidas_Polizas;
                    Rs_Modificar_Ope_Con_Solicitud_Pagos.P_No_Solicitud_Pago = Txt_No_Solicitud_Pago.Text;
                    Rs_Modificar_Ope_Con_Solicitud_Pagos.P_Tipo_Solicitud_Pago_ID = Cmb_Tipo_Solicitud_Pago.SelectedValue;
                    Rs_Modificar_Ope_Con_Solicitud_Pagos.P_No_Reserva = Convert.ToDouble(Cmb_Reserva_Pago.SelectedValue);
                    Rs_Modificar_Ope_Con_Solicitud_Pagos.P_No_Reserva_Anterior = Convert.ToDouble(Txt_No_Reserva_Anterior.Value);
                    Rs_Modificar_Ope_Con_Solicitud_Pagos.P_Proveedor_ID = Cmb_Proveedor_Solicitud_Pago.SelectedValue;
                    Rs_Modificar_Ope_Con_Solicitud_Pagos.P_Beneficiario = Cmb_Proveedor_Solicitud_Pago.SelectedItem.Text;
                    Rs_Modificar_Ope_Con_Solicitud_Pagos.P_Concepto = Txt_Concepto_Solicitud_Pago.Text;
                    Rs_Modificar_Ope_Con_Solicitud_Pagos.P_Monto = Convert.ToDouble(Txt_Monto_Solicitud_Pago.Text.Replace(",", "").Replace("$", ""));
                    Rs_Modificar_Ope_Con_Solicitud_Pagos.P_Monto_Anterior = Convert.ToDouble(Txt_Monto_Solicitud_Anterior.Value.Replace(",", "").Replace("$", ""));
                    Rs_Modificar_Ope_Con_Solicitud_Pagos.P_No_Factura = Txt_No_Factura_Solicitud_Pago.Text;
                    Rs_Modificar_Ope_Con_Solicitud_Pagos.P_Fecha_Factura = String.Format("{0:dd/MM/yy}", Convert.ToDateTime(Txt_Fecha_Factura_Solicitud_Pago.Text));
                    Rs_Modificar_Ope_Con_Solicitud_Pagos.P_Estatus = "PENDIENTE";
                    Rs_Modificar_Ope_Con_Solicitud_Pagos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

                    Rs_Modificar_Ope_Con_Solicitud_Pagos.Modificar_Solicitud_Pago(); //Modifica el registro que fue seleccionado por el usuario con los nuevos datos proporcionados
                    Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Solicitud de Pagos", "alert('La Modificación de la Solicitud de Pago fue Exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Modificar_Solicitud_Pago " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Eliminar_Solicitud_Pago
            /// DESCRIPCION : Elimina los datos de la Solicitud del Pago que fue 
            ///               seleccionada por el Usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 20-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Eliminar_Solicitud_Pago()
            {
                Cls_Ope_Con_Solicitud_Pagos_Negocio Rs_Consulta_Ope_Con_Solicitud_Pagos = new Cls_Ope_Con_Solicitud_Pagos_Negocio(); //Variable de conexión hacia la capa de negocios
                try
                {
                    Rs_Consulta_Ope_Con_Solicitud_Pagos.P_No_Reserva =Convert.ToDouble(Cmb_Reserva_Pago.SelectedValue);
                    Rs_Consulta_Ope_Con_Solicitud_Pagos.P_Monto = Convert.ToDouble(Txt_Monto_Solicitud_Pago.Text.Replace(",","").Replace("$",""));
                    Rs_Consulta_Ope_Con_Solicitud_Pagos.P_No_Solicitud_Pago = Txt_No_Solicitud_Pago.Text.Trim();
                    Rs_Consulta_Ope_Con_Solicitud_Pagos.Eliminar_Solicitud_Pago();//Elimina la solicitud seleccionada por el usuario de la BD

                    Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Solicitud de Pagos", "alert('La Eliminación de la Solicitud de Pago fue Exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Eliminar_Solicitud_Pago" + ex.Message.ToString(), ex);
                }
            }
        #endregion
    #endregion
    #region(Grid)
        protected void Grid_Solicitud_Pagos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;                
                Consulta_Datos_Solicitud_Pago(Grid_Solicitud_Pagos.SelectedRow.Cells[1].Text); //Consulta los datos de la solicitud que fue seleccionada por el usuario
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        protected void Grid_Solicitud_Pagos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Limpia_Controles();                              //Limpia los controles de la forma
                Grid_Solicitud_Pagos.PageIndex = e.NewPageIndex; //Asigna la nueva página que selecciono el usuario
                Llena_Grid_Solicitudes_Pagos();                  //Muestra la solicitudes que estan asignadas en la página seleccionada por el usuario
                Grid_Solicitud_Pagos.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        protected void Grid_Solicitud_Pagos_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                //Se consultan los Tipos de solicitudes que actualmente se encuentran registradas en el sistema.
                Consulta_Solicitudes_Pagos();

                DataTable Dt_Solicitud = (Grid_Solicitud_Pagos.DataSource as DataTable);

                if (Dt_Solicitud != null)
                {
                    DataView Dv_Solicitud = new DataView(Dt_Solicitud);
                    String Orden = ViewState["SortDirection"].ToString();

                    if (Orden.Equals("ASC"))
                    {
                        Dv_Solicitud.Sort = e.SortExpression + " " + "DESC";
                        ViewState["SortDirection"] = "DESC";
                    }
                    else
                    {
                        Dv_Solicitud.Sort = e.SortExpression + " " + "ASC";
                        ViewState["SortDirection"] = "ASC";
                    }
                    Grid_Solicitud_Pagos.DataSource = Dv_Solicitud;
                    Grid_Solicitud_Pagos.DataBind();
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
    #region (Eventos)
        protected void Cmb_Reserva_Pago_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                Limpia_Datos_Reserva(); //Limpia los controles que pertenecen a la reserva
                if (Cmb_Reserva_Pago.SelectedIndex > 0) Consulta_Datos_Reserva(Convert.ToDouble(Cmb_Reserva_Pago.SelectedValue)); //Consulta los datos de la reserva que fue seleccionada por el usuario
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }

        }
        protected void Cmb_Proveedor_Solicitud_Pago_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Txt_Cuenta_Contable_ID_Proveedor.Value = "";
                if (Cmb_Proveedor_Solicitud_Pago.SelectedIndex > 0)
                {
                    Consulta_Cuenta_Contable_Proveedor(Cmb_Proveedor_Solicitud_Pago.SelectedValue); //Consulta la cuenta contable ID del proveedor que fue seleccionada por el usuario
                    if (String.IsNullOrEmpty(Txt_Cuenta_Contable_ID_Proveedor.Value))
                    {
                        Lbl_Mensaje_Error.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; El Proveedor " + Cmb_Proveedor_Solicitud_Pago.SelectedItem.Text + "no tiene una cuenta contable asignada por lo que no se puede asiganar la solicitud de pago";
                        Cmb_Proveedor_Solicitud_Pago.SelectedIndex = 0;
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
        protected void Btn_Buscar_Proveedor_Solicitud_Pagos_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                if (!String.IsNullOrEmpty(Txt_Nombre_Proveedor_Solicitud_Pago.Text))
                {
                    Consulta_Proveedores(); //Consulta los proveedores que coincidan con el rFC o el nombre proporcionado por el usuario
                }
                else
                {
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
        /// NOMBRE DE LA FUNCION: Btn_Cerrar_Ventana_Click
        /// DESCRIPCION : Cierra la ventana de busqueda de empleados.
        /// CREO        : Yazmin Delgado Gómez
        /// FECHA_CREO  : 19-Noviembre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Btn_Cerrar_Ventana_Click(object sender, ImageClickEventArgs e)
        {
            Mpe_Busqueda_Solicitud_Pago.Hide();
        }
        protected void Btn_Busqueda_Solicitud_Pago_Click(object sender, EventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Consulta_Solicitudes_Pagos(); //Consulta a todos las solicitudes de pagos con los datos proporcionados por el usuario
                Mpe_Busqueda_Solicitud_Pago.Hide();
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
                    Consulta_Reservas();          //Consulta las reservas que tienen un saldo pendiente
                    Limpia_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                    Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                }
                else
                {
                    //Valida si todos los campos requeridos estan llenos si es así da de alta los datos en la base de datos
                    if (Validar_Datos_Solicitud_Pagos())
                    {   
                        Alta_Solicitud_Pagos(); //Da de alta la Solicitud del Pago con los datos que proporciono el usuario
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
        protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                if (Btn_Modificar.ToolTip == "Modificar")
                {
                    //Si el usuario selecciono un Tipo de Solicitud entonces habilita los controles para que pueda modificar la información
                    if (!string.IsNullOrEmpty(Txt_No_Solicitud_Pago.Text.Trim()))
                    {
                        if (Txt_Estatus_solicitud_Pago.Text == "PENDIENTE")
                        {
                            Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Solo se puede mdificar Solicitudes pendientes de autorización <br>";
                        }
                    }
                    //Si el usuario no selecciono una Solicitud le indica al usuario que la seleccione para poder modificar
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Seleccione la de Solicitud de Pago que desea modificar sus datos <br>";
                    }
                }
                else
                {
                    //Si el usuario proporciono todos los datos requeridos entonces modificar los datos de la Solicitud en la BD
                    if (Validar_Datos_Solicitud_Pagos())
                    {
                        Modificar_Solicitud_Pago(); //Modifica los datos de la Solicitud con los datos proporcionados por el usuario
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
        //protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        Lbl_Mensaje_Error.Visible = false;
        //        Img_Error.Visible = false;
        //        //Si el usuario selecciono una Solicitud entonces la elimina de la base de datos
        //        if (!string.IsNullOrEmpty(Txt_No_Solicitud_Pago.Text.Trim()))
        //        {
        //            if (Txt_Estatus_solicitud_Pago.Text == "PENDIENTE")
        //            {
        //                Eliminar_Solicitud_Pago(); //Elimina la Solicitud que fue seleccionada por el usuario
        //            }
        //            else
        //            {
        //                Lbl_Mensaje_Error.Visible = true;
        //                Img_Error.Visible = true;
        //                Lbl_Mensaje_Error.Text = "Solo se puede eliminar solicitudes pendientes de autorización <br>";
        //            }
        //        }
        //        //Si el usuario no selecciono alguna Solicitud manda un mensaje indicando que es necesario que 
        //        //seleccione alguna para poder eliminar
        //        else
        //        {
        //            Lbl_Mensaje_Error.Visible = true;
        //            Img_Error.Visible = true;
        //            Lbl_Mensaje_Error.Text = "Seleccione la Solicitud de Pago que desea eliminar <br>";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Lbl_Mensaje_Error.Visible = true;
        //        Img_Error.Visible = true;
        //        Lbl_Mensaje_Error.Text = ex.Message.ToString();
        //    }
        //}
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                if (Btn_Salir.ToolTip == "Salir")
                {
                    Session.Remove("Consulta_Solicitud_Pagos");
                    Session.Remove("Consulta_Reservas");
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
    #endregion
}
