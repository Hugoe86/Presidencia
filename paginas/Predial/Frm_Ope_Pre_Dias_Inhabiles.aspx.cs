using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Globalization;
using Presidencia.DateDiff;
using Presidencia.Dias_Festivos.Negocios;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;

public partial class paginas_Predial_Ope_Pre_Dias_Inhabiles : System.Web.UI.Page
{
    #region PageLoad
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
                    Limpia_Controles(); //Limpia los controles de los datos generales del empleado
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
            /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
            ///               diferentes operaciones
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 08/Octubre/2011
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
                    Consulta_Dias_Festivos();       //Consulta los días festivos que se tienen dados de alta
                    Consulta_Dias_Inhabiles();      //Consulta todos los días inhabiles que se tienen registrados en la base de datos
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
            /// FECHA_CREO  : 08/Octubre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpia_Controles()
            {
                try
                {
                    //Controles para el día inhabil
                    Txt_Busqueda_Año.Text = "";
                    Txt_No_Dia_Inhabil.Text = "";
                    Txt_Descripcion_Dia_Inhabil.Text="";
                    Txt_Fecha_Dia_Inhabil.Text = "";
                    Txt_Motivo_Dia_Inhabil.Text = "";

                    Cmb_Dia_Festivo.SelectedIndex = -1;
                    Cmb_Estatus_Dia_Inhabil.SelectedIndex = -1;
                    Cmb_Tipo_Dia_Inhabil.SelectedIndex = -1;
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
            ///                          del usuario si es una alta, modificacion
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 08/Octubre/2011
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
                    Cmb_Estatus_Dia_Inhabil.Enabled = false;
                    switch (Operacion)
                    {
                        case "Inicial":
                            Habilitado = false;
                            Cmb_Estatus_Dia_Inhabil.SelectedIndex = -1;
                            Txt_Fecha_Dia_Inhabil.Enabled = Habilitado;
                            Btn_Fecha_Dia_Inhabil.Enabled = Habilitado;
                            Btn_Nuevo.ToolTip = "Nuevo";
                            Btn_Modificar.ToolTip = "Modificar";
                            Btn_Salir.ToolTip = "Salir";
                            Btn_Nuevo.Visible = true;
                            Btn_Modificar.Visible = true;
                            Btn_Eliminar.Visible = true;
                            Btn_Nuevo.CausesValidation = false;
                            Btn_Modificar.CausesValidation = false;
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Configuracion_Acceso("Frm_Ope_Pre_Dias_Inhabiles.aspx");
                            break;

                        case "Nuevo":
                            Habilitado = true;
                            Cmb_Estatus_Dia_Inhabil.SelectedIndex = 1;
                            Btn_Nuevo.ToolTip = "Dar de Alta";
                            Btn_Modificar.ToolTip = "Modificar";
                            Btn_Salir.ToolTip = "Cancelar";
                            Btn_Nuevo.Visible = true;
                            Btn_Modificar.Visible = false;
                            Btn_Eliminar.Visible = false;
                            Btn_Nuevo.CausesValidation = true;
                            Btn_Modificar.CausesValidation = true;
                            Txt_Fecha_Dia_Inhabil.Enabled = !Habilitado;
                            Btn_Fecha_Dia_Inhabil.Enabled = !Habilitado;
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
                            Btn_Eliminar.Visible = false;
                            Btn_Nuevo.CausesValidation = true;
                            Btn_Modificar.CausesValidation = true;
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                            Cmb_Estatus_Dia_Inhabil.Enabled = true;
                            break;
                    }
                    Cmb_Tipo_Dia_Inhabil.Enabled = Habilitado;
                    Cmb_Dia_Festivo.Enabled = Habilitado;

                    Txt_Descripcion_Dia_Inhabil.Enabled=Habilitado;
                    Txt_Motivo_Dia_Inhabil.Enabled = Habilitado;
                    Txt_Busqueda_Año.Enabled = !Habilitado;

                    Btn_Buscar_Año.Enabled = !Habilitado;

                    Grid_Dias_Inhabiles.Enabled = !Habilitado;
                }
                catch (Exception ex)
                {
                    throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Habilitar_Controles_Dia_Inhabil
            /// DESCRIPCION : Habilita y Deshabilita los controles de acuerdo al tipo de Día
            ///               inhabil que selecciono el usuario
            /// PARAMETROS  : Habilitar: Indica si será o no habilitado el control
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 08/Octubre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Habilitar_Controles_Dia_Inhabil(Boolean Habilitar)
            {
                try
                {
                    Txt_Fecha_Dia_Inhabil.Enabled = Habilitar;
                    Btn_Fecha_Dia_Inhabil.Enabled = Habilitar;
                }
                catch (Exception ex)
                {
                    throw new Exception("Habilitar_Controles_Dia_Inhabil " + ex.Message.ToString(), ex);
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
                    Botones.Add(Btn_Modificar);
                    Botones.Add(Btn_Eliminar);
                    Botones.Add(Btn_Buscar_Año);

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
        #region (Metodos Validacion)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Validar_Datos_Dia_Inhabil
            /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 08-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Validar_Datos_Dia_Inhabil()
            {
                Boolean Datos_Validos = true; //Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
                Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                if(Cmb_Estatus_Dia_Inhabil.SelectedIndex<=0)
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Estatus es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (String.IsNullOrEmpty(Txt_Descripcion_Dia_Inhabil.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Descripción es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (Cmb_Tipo_Dia_Inhabil.SelectedIndex <= 0)
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione el Tipo de Día Inhabil. <br>";
                    Datos_Validos = false;
                }
                if (String.IsNullOrEmpty(Txt_Fecha_Dia_Inhabil.Text.Trim()) && Txt_Fecha_Dia_Inhabil.Text.ToString() != "__/___/____" && Txt_Fecha_Dia_Inhabil.Text.ToString() != "__ / ___ / ____")
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Día Inhabil es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                //if (Cls_DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Txt_Fecha_Dia_Inhabil.Text.ToString()))), Convert.ToDateTime(String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Txt_Fecha_Aplicacion.Text.ToString())))) <= 0)
                //{
                //    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La fecha del Día Inhabil no puede ser mayor o igual a la fecha de aplicación. <br>";
                //    Datos_Validos = false;
                //}
                return Datos_Validos;
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Fechas_Repetidas
            /// DESCRIPCION : Verifica que la fecha del día inhabil no se encuentren ya 
            ///               registradas en el sistema
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 09-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Consulta_Fechas_Repetidas()
            {
                Boolean Fechas_Validas = true; //Variable que almacena el valor de true si las fechas no estan asignadas y false si ya estan asignadas a otro registro
                DataTable Dt_Dias_Inhabiles = new DataTable(); //Obtiene los registros de la consulta
                Cls_Ope_Pre_Dias_Inhabiles_Negocio Rs_Consulta_Ope_Pre_Dias_Inhabiles = new Cls_Ope_Pre_Dias_Inhabiles_Negocio(); //Variable de conexión hacia la capa de Negocios
                try
                {
                    Lbl_Mensaje_Error.Text = "Favor de cambiar <br>";
                    Rs_Consulta_Ope_Pre_Dias_Inhabiles.P_Fecha_Consulta = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Dia_Inhabil.Text.ToString()));
                    Dt_Dias_Inhabiles = Rs_Consulta_Ope_Pre_Dias_Inhabiles.Consulta_Dias_Inhabiles(); //Consulta si el día inhabil propocionado por el usuario ya fue registrado con anterioridad

                    if (Dt_Dias_Inhabiles.Rows.Count > 0)
                    {
                        foreach (DataRow Registro in Dt_Dias_Inhabiles.Rows)
                        {
                            if (Txt_No_Dia_Inhabil.Text.ToString() != Registro[Ope_Pre_Dias_Inhabiles.Campo_Dia_Inhabil_ID].ToString())
                            {
                                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Día Inhabil ya que se encuentra registrado con anterioridad </br>";
                                Fechas_Validas = false;
                            }
                        }
                    }
                    return Fechas_Validas;
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Fechas_Repetidas " + ex.Message.ToString(), ex);
                }
            }
        #endregion
        #region (Consulta)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Dias_Festivos
            /// DESCRIPCION : Consulta los días festivos que fueron dados de alta por el usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 08-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Dias_Festivos()
            {
                Cls_Tab_Nom_Dias_Festivos_Negocios Rs_Consulta_Tab_Nom_Dias_Festivos = new Cls_Tab_Nom_Dias_Festivos_Negocios(); //Variable de conexión con la capa de negocios.
                DataTable Dt_Dias_Festivos = null; //Variable que almacena un listado de los días festivos que fueron dados de alta

                try
                {
                    Dt_Dias_Festivos = Rs_Consulta_Tab_Nom_Dias_Festivos.Consulta_Datos_Dia_Festivo();
                    Cmb_Dia_Festivo.DataSource = Dt_Dias_Festivos;
                    Cmb_Dia_Festivo.DataTextField = Tab_Nom_Dias_Festivos.Campo_Descripcion;
                    Cmb_Dia_Festivo.DataValueField = Tab_Nom_Dias_Festivos.Campo_Dia_ID;
                    Cmb_Dia_Festivo.DataBind();

                    Cmb_Dia_Festivo.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
                    Cmb_Dia_Festivo.SelectedIndex = -1;
                }
                catch (Exception Ex)
                {
                    throw new Exception("Consulta_Dias_Festivos. Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Dias_Inhabiles
            /// DESCRIPCION : Consulta los días inhabiles que correspondel al año proporcionado
            ///               por el usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 08-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Dias_Inhabiles()
            {
                Cls_Ope_Pre_Dias_Inhabiles_Negocio Rs_Consulta_Ope_Pre_Dias_Inhabiles = new Cls_Ope_Pre_Dias_Inhabiles_Negocio(); //Variable de conexión hacia la capa de Negocios
                DataTable Dt_Dias_Inhabiles; //Variable que obtendra los datos de la consulta 

                try
                {
                    Session.Remove("Consulta_Dias_Inhabiles");
                    Grid_Dias_Inhabiles.DataSource = new DataTable();
                    Grid_Dias_Inhabiles.DataBind();

                    if (!String.IsNullOrEmpty(Txt_Busqueda_Año.Text)) Rs_Consulta_Ope_Pre_Dias_Inhabiles.P_Anio = Txt_Busqueda_Año.Text.ToString();
                    Dt_Dias_Inhabiles = Rs_Consulta_Ope_Pre_Dias_Inhabiles.Consulta_Dias_Inhabiles(); //Consulta los días inhabiles que corresponden al año introducido por el usuario
                    Session["Consulta_Dias_Inhabiles"] = Dt_Dias_Inhabiles;
                    Llena_Grid_Dias_Inhabiles(); //Muestra los días inhabiles obtenidos de la consulta anterior
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Dias_Inhabiles " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Llena_Grid_Dias_Inhabiles
            /// DESCRIPCION : Llena el grid con los Días Inhabiles que pertenecen al empleado 
            ///               que fue seleccionado por empleado
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 08-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Llena_Grid_Dias_Inhabiles()
            {
                DataTable Dt_Dias_Inhabiles; //Variable que obtendra los datos de la consulta 
                try
                {
                    Grid_Dias_Inhabiles.Columns[1].Visible = true;
                    Grid_Dias_Inhabiles.Columns[2].Visible = true;
                    Grid_Dias_Inhabiles.Columns[6].Visible = true;
                    Grid_Dias_Inhabiles.Columns[7].Visible = true;

                    Grid_Dias_Inhabiles.DataSource=null;
                    Grid_Dias_Inhabiles.DataBind();

                    Dt_Dias_Inhabiles = (DataTable)Session["Consulta_Dias_Inhabiles"];
                    Grid_Dias_Inhabiles.DataSource = Dt_Dias_Inhabiles;
                    Grid_Dias_Inhabiles.DataBind();

                    Grid_Dias_Inhabiles.Columns[1].Visible = false;
                    Grid_Dias_Inhabiles.Columns[2].Visible = false;
                    Grid_Dias_Inhabiles.Columns[6].Visible = false;
                    Grid_Dias_Inhabiles.Columns[7].Visible = false;

                    Grid_Dias_Inhabiles.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    throw new Exception("Llena_Grid_Asistencias " + ex.Message.ToString(), ex);
                }
            }
        #endregion
        #region (Métodos Operacion)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Alta_Dia_Inhabil
            /// DESCRIPCION : Da de Alta el Día Inhabil con los datos proporcionados por el usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 08-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Alta_Dia_Inhabil()
            {
                Cls_Ope_Pre_Dias_Inhabiles_Negocio Rs_Alta_Ope_Pre_Dias_Inhabiles = new Cls_Ope_Pre_Dias_Inhabiles_Negocio(); //Variable de conexión hacia la capa de Negocios

                try
                {
                    Rs_Alta_Ope_Pre_Dias_Inhabiles.P_Descripcion = Txt_Descripcion_Dia_Inhabil.Text.ToString().ToUpper();
                    Rs_Alta_Ope_Pre_Dias_Inhabiles.P_Estatus = Cmb_Estatus_Dia_Inhabil.SelectedValue.ToString();
                    if (Cmb_Dia_Festivo.SelectedIndex > 0) Rs_Alta_Ope_Pre_Dias_Inhabiles.P_Dia_ID = Cmb_Dia_Festivo.SelectedValue.ToString();
                    Rs_Alta_Ope_Pre_Dias_Inhabiles.P_Fecha=Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Dia_Inhabil.Text.ToString())));
                    if (!String.IsNullOrEmpty(Txt_Motivo_Dia_Inhabil.Text)) Rs_Alta_Ope_Pre_Dias_Inhabiles.P_Motivo = Txt_Motivo_Dia_Inhabil.Text.ToString();
                    Rs_Alta_Ope_Pre_Dias_Inhabiles.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Rs_Alta_Ope_Pre_Dias_Inhabiles.Alta_Dia_Inhabil(); //Da de alta los datos del Día Inhabil en la BD

                    Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    Habilitar_Controles("Inicial");
                    Consulta_Dias_Inhabiles();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Día Inhabil", "alert('El Alta del Día Inhabil fue Exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Alta_Dia_Inhabil " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Modificar_Dia_Inhabil
            /// DESCRIPCION : Modifica los datos del Día Inhabil con los proporcionados por el
            ///               usuario en la BD
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 08-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Modificar_Dia_Inhabil()
            {
                Cls_Ope_Pre_Dias_Inhabiles_Negocio Rs_Modificar_Ope_Pre_Dias_Inhabiles = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();

                try
                {
                    Rs_Modificar_Ope_Pre_Dias_Inhabiles.P_No_Dia_Inhabil = Txt_No_Dia_Inhabil.Text.ToString();
                    Rs_Modificar_Ope_Pre_Dias_Inhabiles.P_Descripcion = Txt_Descripcion_Dia_Inhabil.Text.ToString().ToUpper();
                    Rs_Modificar_Ope_Pre_Dias_Inhabiles.P_Estatus = Cmb_Estatus_Dia_Inhabil.SelectedValue.ToString();
                    if (Cmb_Dia_Festivo.SelectedIndex > 0) Rs_Modificar_Ope_Pre_Dias_Inhabiles.P_Dia_ID = Cmb_Dia_Festivo.SelectedValue.ToString();
                    Rs_Modificar_Ope_Pre_Dias_Inhabiles.P_Fecha = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Dia_Inhabil.Text.ToString())));
                    if (!String.IsNullOrEmpty(Txt_Motivo_Dia_Inhabil.Text)) Rs_Modificar_Ope_Pre_Dias_Inhabiles.P_Motivo = Txt_Motivo_Dia_Inhabil.Text.ToString();
                    Rs_Modificar_Ope_Pre_Dias_Inhabiles.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

                    Rs_Modificar_Ope_Pre_Dias_Inhabiles.Modiicar_Dia_Inhabil();//Modifica los datos del registro seleccionado
                    Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    Habilitar_Controles("Inicial");//Inicializa los controles de la forma para dejarlos lista para la siguiente operación del usuario
                    Consulta_Dias_Inhabiles(); //Consulta todos los días inhabiles que fueron dados de alta
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Día Inhabil", "alert('La Modificación del Día Inhabil fue Exitosa');", true);

                }
                catch (Exception ex)
                {
                    throw new Exception("Modificar_Dia_Inhabil " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Eliminar_Dia_Inhabil
            /// DESCRIPCION : Elimina el Registro del Día Inhabil que fue seleccionado por el Usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 08-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Eliminar_Dia_Inhabil()
            {
                Cls_Ope_Pre_Dias_Inhabiles_Negocio Rs_Eliminar_Ope_Pre_Dias_Inhabiles = new Cls_Ope_Pre_Dias_Inhabiles_Negocio(); //Variable para la conexión de la capa de Negocios
                try
                {
                    Rs_Eliminar_Ope_Pre_Dias_Inhabiles.P_No_Dia_Inhabil = Txt_No_Dia_Inhabil.Text.ToString();
                    Rs_Eliminar_Ope_Pre_Dias_Inhabiles.Eliminar_Dia_Inhabil(); //Elimina el día inhabil que selecciono el usuario de la BD
                    Inicializa_Controles();                                    //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    Consulta_Dias_Inhabiles(); //Consulta los días inhabiles
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Día Inhabil", "alert('La Eliminación del Día Inhabil fue Exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Eliminar_Dia_Inhabil " + ex.Message.ToString(), ex);
                }
            }
        #endregion
    #endregion
    #region(Grid)
        protected void Grid_Dias_Inhabiles_Sorting(object sender, GridViewSortEventArgs e)
        {
            Consulta_Dias_Inhabiles();//Consulta los días inhabiles
            DataTable Dt_Dias_Inhabiles = (Grid_Dias_Inhabiles.DataSource as DataTable);

            if (Dt_Dias_Inhabiles.Rows.Count > 0)
            {
                DataView Dv_Dias_Inhabiles = new DataView(Dt_Dias_Inhabiles);
                String Orden = ViewState["SortDirection"].ToString();

                if (Orden.Equals("ASC"))
                {
                    Dv_Dias_Inhabiles.Sort = e.SortExpression + " " + "DESC";
                    ViewState["SortDirection"] = "DESC";
                }
                else
                {
                    Dv_Dias_Inhabiles.Sort = e.SortExpression + " " + "ASC";
                    ViewState["SortDirection"] = "ASC";
                }

                Grid_Dias_Inhabiles.DataSource = Dv_Dias_Inhabiles;
                Grid_Dias_Inhabiles.DataBind();
            }
        }
        protected void Grid_Dias_Inhabiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {   
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Limpia_Controles();//Limpia los controles de la pantalla para poder agregar los valores del registro seleccionado por el usuario
                Grid_Dias_Inhabiles.Columns[1].Visible = true;
                Grid_Dias_Inhabiles.Columns[2].Visible = true;
                Grid_Dias_Inhabiles.Columns[6].Visible = true;
                Grid_Dias_Inhabiles.Columns[7].Visible = true;
                Txt_No_Dia_Inhabil.Text = Grid_Dias_Inhabiles.SelectedRow.Cells[1].Text;
                Txt_Fecha_Dia_Inhabil.Text = Grid_Dias_Inhabiles.SelectedRow.Cells[4].Text;
                Txt_Descripcion_Dia_Inhabil.Text = HttpUtility.HtmlDecode(Grid_Dias_Inhabiles.SelectedRow.Cells[5].Text);
                Txt_Motivo_Dia_Inhabil.Text = HttpUtility.HtmlDecode(Grid_Dias_Inhabiles.SelectedRow.Cells[7].Text);
                Cmb_Estatus_Dia_Inhabil.SelectedValue = Grid_Dias_Inhabiles.SelectedRow.Cells[6].Text;
                Cmb_Tipo_Dia_Inhabil.SelectedIndex = 1;
                if(Grid_Dias_Inhabiles.SelectedRow.Cells[2].Text!= "&nbsp;")
                {
                    Cmb_Tipo_Dia_Inhabil.SelectedIndex = 2;
                    Cmb_Dia_Festivo.SelectedValue = Grid_Dias_Inhabiles.SelectedRow.Cells[2].Text;
                }
                Grid_Dias_Inhabiles.Columns[1].Visible = false;
                Grid_Dias_Inhabiles.Columns[2].Visible = false;
                Grid_Dias_Inhabiles.Columns[6].Visible = false;
                Grid_Dias_Inhabiles.Columns[7].Visible = false;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        protected void Grid_Dias_Inhabiles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Limpia_Controles();                             //Limpia todos los controles de la forma
                Grid_Dias_Inhabiles.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Llena_Grid_Dias_Inhabiles();                    //Carga los Días Inhabiles que estan asignados a la página seleccionada
                Grid_Dias_Inhabiles.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
    #endregion
    #region (Operacion [Alta - Modificar - Eliminar - Consultar])
        protected void Cmb_Tipo_Dia_Inhabil_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cmb_Dia_Festivo.SelectedIndex=-1;
            Txt_Fecha_Dia_Inhabil.Text = "";
            switch (Cmb_Tipo_Dia_Inhabil.SelectedValue.ToString())
            {
                case "OTRO":
                    Cmb_Dia_Festivo.Enabled = false;
                    Habilitar_Controles_Dia_Inhabil(true);
                    break;
                case "DIA FESTIVO":
                    Cmb_Dia_Festivo.Enabled = true;
                    Habilitar_Controles_Dia_Inhabil(false);
                    break;
                default:
                    Cmb_Dia_Festivo.Enabled = false;
                    Habilitar_Controles_Dia_Inhabil(false);
                    break;
            };
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Cmb_Dia_Festivo_SelectedIndexChanged
        /// DESCRIPCION : Consulta la fecha del día festivo que fue seleccionado por el usuario
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 08-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Cmb_Dia_Festivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cls_Tab_Nom_Dias_Festivos_Negocios Rs_Consulta_Tab_Nom_Dias_Festivos = new Cls_Tab_Nom_Dias_Festivos_Negocios(); //Variable de conexión hacia la capa de negocios
            DataTable Dt_Fecha_Dia_Festivo; //Obtiene la fecha del días festivo que fue seleccionado por el usuario
            try
            {
                Txt_Fecha_Dia_Inhabil.Text = "";
                if (Cmb_Dia_Festivo.SelectedIndex > 0)
                {
                    Rs_Consulta_Tab_Nom_Dias_Festivos.P_Dia_ID = Cmb_Dia_Festivo.SelectedValue.ToString();
                    Dt_Fecha_Dia_Festivo = Rs_Consulta_Tab_Nom_Dias_Festivos.Consulta_Datos_Dia_Festivo(); //Consulta la fecha del día festivo seleccionado

                    //Agrega la fecha del día festivo al control correspondiente para ser mostrado al usuario
                    foreach (DataRow Registro in Dt_Fecha_Dia_Festivo.Rows)
                    {
                        Txt_Fecha_Dia_Inhabil.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Tab_Nom_Dias_Festivos.Campo_Fecha].ToString()));
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
        protected void Btn_Buscar_Año_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Limpia_Controles();//Limpia los controles de la forma
                if (!String.IsNullOrEmpty(Txt_Busqueda_Año.Text.ToString()))
                {
                    //Si el usuario solo propociono los últimos 2 digitos del año entonces concatena al año 20 para poder obtener el año correctamente
                    if (Convert.ToInt16(Txt_Busqueda_Año.Text.ToString().Length) == 2) Txt_Busqueda_Año.Text = "20" + Txt_Busqueda_Año.Text.ToString();
                    if (Convert.ToInt16(Txt_Busqueda_Año.Text.ToString().Length) == 4)
                    {
                        if (Convert.ToInt16(Txt_Busqueda_Año.Text.ToString()) <= Convert.ToInt16(DateTime.Today.AddYears(1).Year) && Convert.ToInt16(Txt_Busqueda_Año.Text.ToString()) >= 2011)
                        {
                            Consulta_Dias_Inhabiles(); //Consulta todos los días inhabiles del año proporcionado por el usuario
                            //Si no se encontraron días festivos con el año proporcionado por el usuario muestra un mensaje al usuario
                            if (Grid_Dias_Inhabiles.Rows.Count <= 0)
                            {
                                Lbl_Mensaje_Error.Visible = true;
                                Img_Error.Visible = true;
                                Lbl_Mensaje_Error.Text = "No se encontraron Días  con el Año proporcionado <br>";
                            }
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "El año proporcionado para la consulta no es valido, favor de verificar <br>";
                        }
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "No es correcto el formato del año proporcionado debe estar a 4 cifras <br>";
                    }
                }
                else
                {
                    Consulta_Dias_Inhabiles(); //Consulta todos los días inhabiles del año proporcionado por el usuario
                    //Si no se encontraron días festivos con el año proporcionado por el usuario muestra un mensaje al usuario
                    if (Grid_Dias_Inhabiles.Rows.Count <= 0)
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "No se encontraron Días  con el Año proporcionado <br>";
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
        protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                if (Btn_Nuevo.ToolTip == "Nuevo")
                {
                    Limpia_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                    Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                    Cmb_Dia_Festivo.Enabled = false;
                }
                else
                {
                    //Si todos los campos requeridos fueron proporcionados por el usuario entonces da de alta los mismo en la base de datos
                    if (Validar_Datos_Dia_Inhabil())
                    {
                        if (Consulta_Fechas_Repetidas())
                        {
                            Alta_Dia_Inhabil(); //Da de alta los datos proporcionados por el usuario
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                        }
                    }
                    //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
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
                    if (!String.IsNullOrEmpty(Txt_No_Dia_Inhabil.Text.ToString()))
                    {
                        Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                        if (Cmb_Dia_Festivo.SelectedIndex > 0)
                        {
                            Cmb_Dia_Festivo.Enabled = true;
                            Txt_Fecha_Dia_Inhabil.Enabled = false;
                            Btn_Fecha_Dia_Inhabil.Enabled = false;
                        }
                        else
                        {
                            Cmb_Dia_Festivo.Enabled = false;
                            Txt_Fecha_Dia_Inhabil.Enabled = true;
                            Btn_Fecha_Dia_Inhabil.Enabled = true;
                        }
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Seleccione el Día Inhabil que desea modificar sus datos <br>";
                    }
                }
                else
                {
                    //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos en la BD
                    if (Validar_Datos_Dia_Inhabil())
                    {
                        if(Consulta_Fechas_Repetidas())
                        {
                            Modificar_Dia_Inhabil(); //Modifica los datos del Día Inhabil con los datos proporcionados por el usuario
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                        }
                    }
                    //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
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
        protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                //Si el usuario selecciono una Escolaridad entonces la elimina de la base de datos
                if (!String.IsNullOrEmpty(Txt_No_Dia_Inhabil.Text.ToString()))
                {
                    Eliminar_Dia_Inhabil(); //Elimina el Día Inhabil que fue seleccionado por el usuario
                }
                //Si el usuario no selecciono algún registro muestra un mensaje al usuario
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Día Inhabil que desea eliminar <br>";
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
                    Session.Remove("Consulta_Dias_Inhabiles");
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
                else
                {
                    Inicializa_Controles(); //Habilita los controles para la siguiente operación del usuario en la operación
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