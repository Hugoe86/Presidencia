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
using Presidencia.Reloj_Checador.Negocios;
using Presidencia.Empleados.Negocios;
using Presidencia.Reloj_Checador.Negocios;

public partial class paginas_Nomina_Frm_Cat_Nom_Empleado_Reloj_Checador : System.Web.UI.Page
{
    #region (Load/Init)
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
                    Inicializa_Controles();          //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    Limpia_Controles();              //Limpia los controles de los datos generales del empleado
                    Cargar_Combo_Reloj_Checadores(); //Carga todos los reloj checadores que han sido dados de alta
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
            /// FECHA_CREO  : 03/Octubre/2011
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
                    Cargar_Combo_Reloj_Checadores();    //Consulta los reloj checadores que han sido dados de alta
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
            /// FECHA_CREO  : 03/Octubre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpia_Controles()
            {
                try
                {
                    //Datos para la consulta de los datos del empleado
                    Txt_Busqueda_No_Empleado.Text = "";
                    Txt_Busqueda_Nombre_Empleado.Text = "";
                    Txt_Empleado_ID.Text = "";
                    Grid_Busqueda_Empleados.DataSource = new DataTable();
                    Grid_Busqueda_Empleados.DataBind();
                    
                    //Datos generales del empleado
                    Txt_No_Empleado.Text = "";
                    Txt_Estatus_Empleado.Text = "";
                    Txt_Nombre_Empleado.Text = "";
                    Txt_Unidad_Responsable.Text = "";

                    //Datos generales del reloj checador
                    Cmb_Checa_Asistencia.SelectedIndex = -1;
                    Txt_Fecha_Inicio_Reloj_Checador.Text = "";
                    Txt_Clave_Reloj_Checador.Text = "";
                    Cmb_Reloj_Checador_Asistencia.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    throw new Exception("Limpia_Controles " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Habilitar_Controles
            /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara
            ///               la página para a siguiente operación
            /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte
            ///                          del usuario si es una modificacion
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 03/Octubre/2011
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
                            Cmb_Reloj_Checador_Asistencia.SelectedIndex = 0;
                            Btn_Modificar.ToolTip = "Modificar";
                            Btn_Salir.ToolTip = "Salir";
                            Btn_Modificar.Visible = true;
                            Btn_Modificar.CausesValidation = false;
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Configuracion_Acceso("Frm_Cat_Nom_Empleado_Reloj_Checador.aspx");
                            break;
                        
                        case "Modificar":
                            Habilitado = true;
                            Btn_Modificar.ToolTip = "Actualizar";
                            Btn_Salir.ToolTip = "Cancelar";
                            Btn_Modificar.Visible = true;
                            Btn_Modificar.CausesValidation = true;
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                            break;
                    }
                    Btn_Busqueda_Empleados.Enabled = !Habilitado;
                    Cmb_Checa_Asistencia.Enabled = Habilitado;
                    Habilitar_Controles_Reloj_Checador(Habilitado); //Habilita o Deshabilita los controles del reloj checador de acuerdo a la operación que desea realizar el usuario
                }
                catch (Exception ex)
                {
                    throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Habilitar_Controles_Reloj_Checador
            /// DESCRIPCION : Habilita y Deshabilita los controles de los datos del reloj checador
            /// PARAMETROS  : Habilitar: Indica si se va a habilitar el control o no
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 04/Octubre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Habilitar_Controles_Reloj_Checador(Boolean Habilitar)
            {
                Txt_Fecha_Inicio_Reloj_Checador.Enabled = Habilitar;
                Txt_Clave_Reloj_Checador.Enabled = Habilitar;
                Cmb_Reloj_Checador_Asistencia.Enabled = Habilitar;
                Btn_Fecha_Inicio_Reloj_Checador.Enabled = Habilitar;
                Btn_Buscar_Reloj_Checador.Enabled = Habilitar;
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
        #region (Metodos Validacion)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Validar_Datos_Empleado_Reloj_Checador
            /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 03-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Validar_Datos_Empleado_Reloj_Checador()
            {
                Boolean Datos_Validos = true;   //Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
                Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

                if (Cmb_Checa_Asistencia.SelectedValue == "SI")
                {
                    if (string.IsNullOrEmpty(Txt_Fecha_Inicio_Reloj_Checador.Text.Trim()))
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Fecha de Inicio es un dato requerido por el sistema. <br>";
                        Datos_Validos = false;
                    }
                    if (Cmb_Reloj_Checador_Asistencia.SelectedIndex <= 0)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Reloj Checador es un dato requerido por el sistema. <br>";
                        Datos_Validos = false;
                    }
                }
                return Datos_Validos;
            }
        #endregion
        #region (Consulta)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Cargar_Combo_Reloj_Checadores
            /// DESCRIPCION : Consulta los reloj Checadores que estan dadas de alta en la BD
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 04-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Cargar_Combo_Reloj_Checadores()
            {
                Cls_Cat_Nom_Reloj_Checador_Negocio Rs_Consulta_Cat_Nom_Reloj_Checador = new Cls_Cat_Nom_Reloj_Checador_Negocio(); //Variable de conexión con la capa de negocios.
                DataTable Dt_Reloj_Checador = null; //Variable que almacena un listado de areas funcionales registradas actualmente en el sistema.

                try
                {
                    Dt_Reloj_Checador = Rs_Consulta_Cat_Nom_Reloj_Checador.Consulta_Reloj_Checador();
                    Cmb_Reloj_Checador_Asistencia.DataSource = Dt_Reloj_Checador;
                    Cmb_Reloj_Checador_Asistencia.DataTextField = "RELOJ";
                    Cmb_Reloj_Checador_Asistencia.DataValueField = Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID;
                    Cmb_Reloj_Checador_Asistencia.DataBind();

                    Cmb_Reloj_Checador_Asistencia.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
                    Cmb_Reloj_Checador_Asistencia.SelectedIndex = -1;
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al consultar los Reloj Checadores registrados actualmente en el sistema. Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Reloj_Checador
            /// DESCRIPCION : Consulta de acuerdo a la clave que proporciono el usuario el
            ///               reloj checador al cual pertenece dicha clave
            /// CREO        : Yazmin Delgado Gómez
            /// FECHA_CREO  : 04-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Reloj_Checador(String Tipo_Consulta)
            {
                Cls_Cat_Nom_Reloj_Checador_Negocio Rs_Consulta_Cat_Nom_Reloj_Checador = new Cls_Cat_Nom_Reloj_Checador_Negocio(); //Variable de conexión hacia la capa de negocios
                DataTable Dt_Reloj_Checador; //Variable a contener el reloj checador que se esta buscando

                try
                {
                    if(Tipo_Consulta.ToString() == "Clave") Rs_Consulta_Cat_Nom_Reloj_Checador.P_Clave = Txt_Clave_Reloj_Checador.Text.ToString();
                    if (Tipo_Consulta.ToString() == "Ubicacion") Rs_Consulta_Cat_Nom_Reloj_Checador.P_Reloj_Checador_ID = Cmb_Reloj_Checador_Asistencia.SelectedValue; 
                    Dt_Reloj_Checador = Rs_Consulta_Cat_Nom_Reloj_Checador.Consulta_Reloj_Checador();

                    //Selecciona de acuerdo a la clave del reloj checador la ubicación que tiene el reloj checador
                    foreach (DataRow Registro in Dt_Reloj_Checador.Rows)
                    {
                        if (Tipo_Consulta.ToString() == "Clave") Cmb_Reloj_Checador_Asistencia.SelectedValue = Registro[Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID].ToString();
                        if (Tipo_Consulta.ToString() == "Ubicacion") Txt_Clave_Reloj_Checador.Text = Registro[Cat_Nom_Reloj_Checador.Campo_Clave].ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Reloj_Checador " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Empleados
            /// DESCRIPCION : Consulta a los empleados que coincidan con los datos proporcionados
            ///               por el usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 04-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************        
            private void Consulta_Empleados()
            {
                Cls_Cat_Empleados_Negocios Rs_Consulta_Cat_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión hacia la capa de Negocios
                DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta 

                try
                {
                    //Limpia los datos de consultan anteriores
                    Grid_Busqueda_Empleados.DataSource = new DataTable();
                    Grid_Busqueda_Empleados.DataBind();

                    if (!String.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text)) Rs_Consulta_Cat_Empleados.P_No_Empleado = Txt_Busqueda_No_Empleado.Text.ToString();
                    if (!String.IsNullOrEmpty(Txt_Busqueda_Nombre_Empleado.Text)) Rs_Consulta_Cat_Empleados.P_Nombre = Txt_Busqueda_Nombre_Empleado.Text.ToString();
                    Dt_Empleados = Rs_Consulta_Cat_Empleados.Consulta_Empleados(); //Consulta a los empleados que coincidan con los datos proporcionados por el usuario

                    Grid_Busqueda_Empleados.Columns[1].Visible = true;
                    Grid_Busqueda_Empleados.DataSource = Dt_Empleados;
                    Grid_Busqueda_Empleados.DataBind();
                    Grid_Busqueda_Empleados.Columns[1].Visible = false;
                    Grid_Busqueda_Empleados.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Empleados " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Empleado_Reloj_Checador
            /// DESCRIPCION : Consulta todos los datos del empleado con respecto al reloj checador
            /// PARAMETROS  : Empleado_ID: ID del empleado a consultar sus datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 04-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Datos_Empleado_Reloj_Checador(String Empleado_ID)
            {
                Cls_Cat_Empleados_Negocios Rs_Consulta_Cat_Empleados = new Cls_Cat_Empleados_Negocios(); //Vaiable de conexión hacia la capa de Negocios
                DataTable Dt_Empleado; //Variable a contener los datos de la consulta

                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Limpia_Controles(); //Limpia los controles de la forma para poder asignar los datos generales del empleado
                    Rs_Consulta_Cat_Empleados.P_Empleado_ID = Empleado_ID.ToString();
                    Dt_Empleado = Rs_Consulta_Cat_Empleados.Consulta_Datos_Reloj_Checador_Empleado();//Consulta todos los datos generales del empleado que fue seleccionado por el usuario

                    if (Dt_Empleado.Rows.Count > 0)
                    {
                        //Agrega los valores de los campos a los controles correspondientes de la forma
                        foreach (DataRow Registro in Dt_Empleado.Rows)
                        {
                            Txt_Empleado_ID.Visible = true;
                            Txt_Empleado_ID.Text = Registro[Cat_Empleados.Campo_Empleado_ID].ToString();
                            Txt_No_Empleado.Text = Registro[Cat_Empleados.Campo_No_Empleado].ToString();
                            Txt_Nombre_Empleado.Text = Registro["Empleado"].ToString();
                            Txt_Estatus_Empleado.Text = Registro[Cat_Empleados.Campo_Estatus].ToString();
                            Txt_Unidad_Responsable.Text = Registro["Unidad_Responsable"].ToString();
                            //Si el empleado ya tiene asignado si checa o no asistencia entonces agrega los valores obtenidos de la consulta en los
                            //controles correspondientes
                            if (!String.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Fecha_Inicio_Reloj_Checador].ToString()))
                            {
                                Cmb_Checa_Asistencia.SelectedValue = Registro[Cat_Empleados.Campo_Reloj_Checador].ToString();
                                //Si el empleado checa asistencia entonces agrega los valores de la consulta en los controles correspondientes
                                if (Registro[Cat_Empleados.Campo_Reloj_Checador].ToString() == "SI")
                                {   
                                    Txt_Fecha_Inicio_Reloj_Checador.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Cat_Empleados.Campo_Fecha_Inicio_Reloj_Checador]));
                                    Cmb_Reloj_Checador_Asistencia.SelectedValue = Registro[Cat_Empleados.Campo_Reloj_Checador_ID].ToString();
                                    Txt_Clave_Reloj_Checador.Text = Registro[Cat_Nom_Reloj_Checador.Campo_Clave].ToString();
                                }
                            }
                            //Si el empleado aun no se le ha asignado el que cheque dentro de presidencia entonces indica que no checa
                            else
                            {
                                Cmb_Checa_Asistencia.SelectedIndex = 2; //Indica que el empleado no esta habilitado para checar asistencia
                            }
                            Txt_Empleado_ID.Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Datos_Empleado_Reloj_Checador " + ex.Message.ToString(), ex);
                }
            }
        #endregion
        #region (Métodos Operacion)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Modificar_Datos_Empleado_Reloj_Checador
            /// DESCRIPCION : Modifica el registro del empleado actualizando los valores de
            ///               si checa o no asistencia, así como la fecha de inicio de checada
            ///               y en que reloj lo hace
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 04-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Modificar_Datos_Empleado_Reloj_Checador()
            {
                Cls_Cat_Empleados_Negocios Rs_Modificar_Cat_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión hacía la capa de Negocios
                try
                {
                    Rs_Modificar_Cat_Empleados.P_Empleado_ID = Txt_Empleado_ID.Text.ToString();
                    Rs_Modificar_Cat_Empleados.P_Reloj_Checador = Cmb_Checa_Asistencia.SelectedValue.ToString();
                    Rs_Modificar_Cat_Empleados.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
                    if (Cmb_Checa_Asistencia.SelectedValue.ToString() == "SI")
                    {
                        Rs_Modificar_Cat_Empleados.P_Reloj_Checator_ID = Cmb_Reloj_Checador_Asistencia.SelectedValue.ToString();
                        Rs_Modificar_Cat_Empleados.P_Fecha_Inicio_Reloj_Checador =Convert.ToDateTime(String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicio_Reloj_Checador.Text.ToString())));
                    }
                    Rs_Modificar_Cat_Empleados.Alta_Modificacion_Reloj_Checador(); //Asigna los valores proporcionados por el usuario al empleado
                    Habilitar_Controles("Inicial");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Reloj Checador", "alert('La Modificación del Empleado fue Exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Modificar_Datos_Empleado_Reloj_Checador" + ex.Message.ToString(), ex);
                }
            }
        #endregion
    #endregion
    #region (Grid)
            protected void Grid_Busqueda_Empleados_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Grid_Busqueda_Empleados.Columns[1].Visible = true;
                Consulta_Datos_Empleado_Reloj_Checador(Grid_Busqueda_Empleados.SelectedRow.Cells[1].Text); //Consulta los datos generales del empleado que fue seleccionado
                Grid_Busqueda_Empleados.Columns[1].Visible = false;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
    #endregion
    #region (Operacion)
        #region (Busqueda Empleados - Cerrar Venta)
            protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e)
            {
                try
                {
                    Consulta_Empleados(); //Consulta a todos los empleados con los datos proporcionados por el usuario
                    Mpe_Busqueda_Empleados.Show();
                }
                catch (Exception Ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = Ex.Message.ToString();
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Btn_Cerrar_Ventana_Click
            /// DESCRIPCION : Cierra la ventana de busqueda de empleados.
            /// CREO        : Yazmin Delgado Gómez
            /// FECHA_CREO  : 03-Agosto-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            protected void Btn_Cerrar_Ventana_Click(object sender, ImageClickEventArgs e)
            {
                Mpe_Busqueda_Empleados.Hide();
            }            
        #endregion
        #region (Operacion [Busqueda - Modificar - Salir])            
            protected void Btn_Buscar_Reloj_Checador_Click(object sender, EventArgs e)
            {
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    if (!String.IsNullOrEmpty(Txt_Clave_Reloj_Checador.Text))
                    {
                        Consulta_Reloj_Checador("Clave"); //Consulta a que reloj checador pertenece la clave que proporciono el usuario
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Proporcione la clave del reloj que desea consultar. <br>";
                    }
                }
                catch (Exception Ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = Ex.Message.ToString();
                }
            }
            protected void Cmb_Reloj_Checador_Asistencia_SelectedIndexChanged(object sender, EventArgs e)
            {
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    if (Cmb_Reloj_Checador_Asistencia.SelectedIndex > 0)
                    {
                        Consulta_Reloj_Checador("Ubicacion"); //Consulta la clave del reloj checador que fue seleccionado por el usuario
                    }
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            protected void Cmb_Checa_Asistencia_SelectedIndexChanged(object sender, EventArgs e)
            {
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    //Habilita los controles para que el usuario pueda modificar los datos del empleado si este checa entrada y salida
                    if (Cmb_Checa_Asistencia.SelectedValue.ToString() == "SI")
                    {
                        Habilitar_Controles_Reloj_Checador(true); //Habilita los controles del reloj checador para que el usuario proporcione los datos correspondientes
                    }
                    //Limpia y Deshabilita los controles para que el usuario no pueda modificar los datos del empleado si este no checa entrada y salida
                    else
                    {
                        Txt_Fecha_Inicio_Reloj_Checador.Text = "";
                        Txt_Clave_Reloj_Checador.Text = "";
                        Cmb_Reloj_Checador_Asistencia.SelectedIndex = -1;
                        Habilitar_Controles_Reloj_Checador(false); //Deshabilita los controles del reloj checador para que el usuario no pueda introducir datos
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
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                //Verifica que el usuario haya consultado y seleccionado a un empleado previamente para poder empezar a realizar las operaciones
                //dentro de la página
                if (!String.IsNullOrEmpty(Txt_No_Empleado.Text))
                {
                    if (Btn_Modificar.ToolTip.Trim().Equals("Modificar"))
                    {   
                        Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                        //Si el empleado no tiene asignado la checada o bien no checa entonces deshabilita los controles del reloj checador
                        if (Cmb_Checa_Asistencia.SelectedIndex == 0 || Cmb_Checa_Asistencia.SelectedIndex==2)
                        {
                            Habilitar_Controles_Reloj_Checador(false);
                        }
                    }
                    //Actualiza los valores del empleado en la base de datos
                    else
                    {
                        if (Cmb_Checa_Asistencia.SelectedIndex > 0)
                        {
                            //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos valores en la base de datos
                            if (Validar_Datos_Empleado_Reloj_Checador())
                            {
                                Modificar_Datos_Empleado_Reloj_Checador(); //Modifica los datos del empleado con los proporcionados por el usuario
                            }
                            else
                            {
                                Lbl_Mensaje_Error.Visible = true;
                                Img_Error.Visible = true;
                            }
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Especificar si el empleado chequera asistencia <br>";
                        }
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Realizar la búsqueda del empleado antes de realizar la operacion <br>";
                }
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
                        Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario en la operación
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
    #endregion
}
