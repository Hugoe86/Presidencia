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
using Presidencia.Horarios_Empleados.Negocio;
using Presidencia.Empleados.Negocios;

public partial class paginas_Nomina_Frm_Ope_Nom_Horarios_Empleados : System.Web.UI.Page
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
                    Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    Limpia_Controles_Datos_Generales(); //Limpia los controles de los datos generales del empleado
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
            /// FECHA_CREO  : 06/Agosto/2011
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
            /// FECHA_CREO  : 06/Agosto/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpia_Controles()
            {
                try
                {
                    //Controles para la asistencia del personal
                    Txt_No_Horario_Empleado.Text = "";
                    Txt_Fecha_Entrada_Horario.Text = "";
                    Txt_Fecha_Termino_Horario.Text = "";
                    Txt_Hora_Entrada.Text = "";
                    Txt_Hora_Salida.Text = "";

                    Grid_Busqueda_Empleados.DataSource = new DataTable();
                    Grid_Busqueda_Empleados.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Limpia_Controles " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Limpia_Controles_Datos_Generales
            /// DESCRIPCION : Limpia los controles que se encuentran en la forma
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 06/Agosto/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpia_Controles_Datos_Generales()
            {
                try
                {
                    //Controles para la busqueda
                    Txt_Busqueda_No_Empleado.Text = "";
                    Txt_Busqueda_Nombre_Empleado.Text = "";
                    //Controles para los datos generales
                    Txt_Empleado_ID.Text = "";
                    Txt_No_Empleado_Horario.Text = "";
                    Txt_Nombre_Empleado_Horario.Text = "";
                    Txt_Estatus_Empleado_Horario.Text = "";
                    Txt_Unidad_Responsable_Empleado_Horario.Text = "";
                    Grid_Horarios_Empleado.DataSource = new DataTable();
                    Grid_Horarios_Empleado.DataBind();
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
            /// FECHA_CREO  : 06/Agosto/2011
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
                            Btn_Eliminar.Visible = true;
                            Btn_Nuevo.CausesValidation = false;
                            Btn_Modificar.CausesValidation = false;
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Configuracion_Acceso("Frm_Ope_Nom_Horarios_Empleados.aspx");
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
                            break;
                    }
                    Txt_Fecha_Entrada_Horario.Enabled = Habilitado;
                    Txt_Fecha_Termino_Horario.Enabled = Habilitado;
                    Txt_Hora_Entrada.Enabled = Habilitado;
                    Txt_Hora_Salida.Enabled = Habilitado;
                    Grid_Horarios_Empleado.Enabled = !Habilitado;
                    Btn_Busqueda_Empleados.Enabled = !Habilitado;
                }
                catch (Exception ex)
                {
                    throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
                }
            }
        #endregion
        #region (Control Acceso Pagina)
            ///*************************************************************************************
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
            ///*************************************************************************************
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
            /// NOMBRE DE LA FUNCION: Validar_Datos_Horario_Empleado
            /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 06-Agosto-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Validar_Datos_Horario_Empleado()
            {
                Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
                Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

                if (string.IsNullOrEmpty(Txt_Fecha_Entrada_Horario.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Fecha de Inicio es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }                
                if (string.IsNullOrEmpty(Txt_Fecha_Termino_Horario.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Fecha de Termino es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Hora_Entrada.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Hora de Entrada es un dato requerido por el sistema. La clave deberá de ser de 5 carácteres alfanumericos. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Hora_Salida.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Hora de Salida es un dato requerido por el sistema. La clave deberá de ser de 5 carácteres alfanumericos. <br>";
                    Datos_Validos = false;
                }
                return Datos_Validos;
            }
        #endregion
        #region (Consulta)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Empleados
            /// DESCRIPCION : Consulta a los empleados que coincidan con los datos proporcionados
            ///               por el usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 06-Agosto-2011
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

                    if (!String.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text))
                    {
                        Rs_Consulta_Cat_Empleados.P_No_Empleado = Txt_Busqueda_No_Empleado.Text.ToString();
                    }
                    if (!String.IsNullOrEmpty(Txt_Busqueda_Nombre_Empleado.Text))
                    {
                        Rs_Consulta_Cat_Empleados.P_Nombre = Txt_Busqueda_Nombre_Empleado.Text.ToString();
                    }
                    Dt_Empleados = Rs_Consulta_Cat_Empleados.Consulta_Empleados(); //Consulta a los empleados que coincidan con los datos proporcionados por el usuario
                    Session["Consulta_Empleados"] = Dt_Empleados;
                    Llena_Grid_Busqueda_Empleado(); //Agrega empleados obtenidas de la consulta anterior
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Empleados " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Empleado
            /// DESCRIPCION : Consulta los datos generales del empleado que fue seleccionado
            /// PARAMETROS  : Empleado_ID: ID del empleado a consultar sus datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 06-Agosto-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Datos_Empleado(String Empleado_ID)
            {
                Cls_Ope_Nom_Horarios_Empleados_Negocio Rs_Ope_Nom_Horarios_Empleados = new Cls_Ope_Nom_Horarios_Empleados_Negocio(); //Vaiable de conexión hacia la capa de Negocios
                DataTable Dt_Empleado; //Variable a contener los datos de la consulta

                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Rs_Ope_Nom_Horarios_Empleados.P_Empleado_ID = Empleado_ID.ToString();
                    Dt_Empleado = Rs_Ope_Nom_Horarios_Empleados.Consulta_Datos_Empleado();

                    if (Dt_Empleado.Rows.Count > 0)
                    {
                        //Agrega los valores de los campos a los controles correspondientes de la forma
                        foreach (DataRow Registro in Dt_Empleado.Rows)
                        {
                            Txt_Empleado_ID.Visible = true;
                            Txt_Empleado_ID.Text = Registro[Cat_Empleados.Campo_Empleado_ID].ToString();
                            Txt_No_Empleado_Horario.Text = Registro[Cat_Empleados.Campo_No_Empleado].ToString();
                            Txt_Nombre_Empleado_Horario.Text = Registro["Empleado"].ToString();
                            Txt_Estatus_Empleado_Horario.Text = Registro[Cat_Empleados.Campo_Estatus].ToString();
                            Txt_Unidad_Responsable_Empleado_Horario.Text = Registro["Unidad_Responsable"].ToString();
                            Txt_Empleado_ID.Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Datos_Empleado " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Horarios_Empleado
            /// DESCRIPCION : Consulta los horarios que estan dadas de alta en la BD y que
            ///               pertenencen al empleado
            /// PARAMETROS  : Empleado_ID: ID del empleado a consultar sus datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 06-Agosto-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Horarios_Empleado(String Empleado_ID)
            {
                Cls_Ope_Nom_Horarios_Empleados_Negocio Rs_Consulta_Ope_Nom_Horarios_Empleados = new Cls_Ope_Nom_Horarios_Empleados_Negocio(); //Variable de conexión hacia la capa de Negocios
                DataTable Dt_Horarios_Empleado; //Variable que obtendra los datos de la consulta 

                try
                {
                    Rs_Consulta_Ope_Nom_Horarios_Empleados.P_Empleado_ID = Empleado_ID.ToString();
                    Dt_Horarios_Empleado = Rs_Consulta_Ope_Nom_Horarios_Empleados.Consulta_Horarios_Empleados(); //Consulta los datos generales de los horarios del empleado dadas de alta en la BD
                    Session["Consulta_Horarios_Empleado"] = Dt_Horarios_Empleado;
                    Llena_Grid_Horarios_Empleado(); //Agrega las horarios obtenidas de la consulta anterior
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Horarios_Empleado " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Llena_Grid_Horarios_Empleado
            /// DESCRIPCION : Llena el grid con los horarios que pertenecen al empleado que
            ///               fue seleccionado por empleado
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 06-Agosto-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Llena_Grid_Horarios_Empleado()
            {
                DataTable Dt_Horarios_Empleado; //Variable que obtendra los datos de la consulta 
                try
                {
                    Grid_Horarios_Empleado.DataBind();
                    Dt_Horarios_Empleado = (DataTable)Session["Consulta_Horarios_Empleado"];
                    Grid_Horarios_Empleado.DataSource = Dt_Horarios_Empleado;
                    Grid_Horarios_Empleado.DataBind();
                    Grid_Horarios_Empleado.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    throw new Exception("Llena_Grid_Horarios_Empleado " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Llena_Grid_Busqueda_Empleado
            /// DESCRIPCION : Llena el grid con los empleados
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 08-Agosto-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Llena_Grid_Busqueda_Empleado()
            {
                DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta 
                try
                {
                    Grid_Busqueda_Empleados.DataBind();
                    Dt_Empleados = (DataTable)Session["Consulta_Empleados"];
                    Grid_Busqueda_Empleados.Columns[1].Visible = true;
                    Grid_Busqueda_Empleados.DataSource = Dt_Empleados;
                    Grid_Busqueda_Empleados.DataBind();
                    Grid_Busqueda_Empleados.Columns[1].Visible = false;
                    Grid_Busqueda_Empleados.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    throw new Exception("Llena_Grid_Busqueda_Empleado " + ex.Message.ToString(), ex);
                }
            }
        #endregion
        #region (Métodos Operacion)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Alta_Horario_Empleado
            /// DESCRIPCION : Da de Alta el Horario del Empleado con los datos proporcionados
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 06-Agosto-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Alta_Horario_Empleado()
            {
                Cls_Ope_Nom_Horarios_Empleados_Negocio Rs_Alta_Ope_Nom_Horarios_Empleados = new Cls_Ope_Nom_Horarios_Empleados_Negocio();  //Variable de conexión hacia la capa de Negocios
                String Fecha_Hora; //Obtiene la fecha y la hora inicio y termino del horario del empleado

                try
                {
                    Rs_Alta_Ope_Nom_Horarios_Empleados.P_Empleado_ID = Txt_Empleado_ID.Text.ToString();
                    Fecha_Hora = String.Format("{0:dd/MMM/yyyy}", Txt_Fecha_Entrada_Horario.Text.ToString() + " 00:00:00");
                    Rs_Alta_Ope_Nom_Horarios_Empleados.P_Fecha_Inicio = Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy HH:mm:ss}", Fecha_Hora));

                    Fecha_Hora = String.Format("{0:dd/MMM/yyyy}", Txt_Fecha_Termino_Horario.Text.ToString()+ " 00:00:00");
                    Rs_Alta_Ope_Nom_Horarios_Empleados.P_Fecha_Termino = Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy HH:mm:ss}", Fecha_Hora));
                    Rs_Alta_Ope_Nom_Horarios_Empleados.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

                    Rs_Alta_Ope_Nom_Horarios_Empleados.P_Horario_Entrada = Convert.ToDateTime(String.Format("{0:T}", Txt_Hora_Entrada.Text));
                    Rs_Alta_Ope_Nom_Horarios_Empleados.P_Horario_Salida = Convert.ToDateTime(String.Format("{0:T}", Txt_Hora_Salida.Text));

                    Rs_Alta_Ope_Nom_Horarios_Empleados.Alta_Horario_Empleado(); //Da de alto los datos del horario en la BD

                    Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    Habilitar_Controles("Inicial");
                    Consulta_Horarios_Empleado(Txt_Empleado_ID.Text.ToString());

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Horarios de Empleados", "alert('El Alta del Horario del Empleado fue Exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Alta_Horario_Empleado " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Modificar_Horario_Empleado
            /// DESCRIPCION : Modifica los datos del Horario del Empleado con los proporcionados 
            ///               por el usuario en la BD
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 06-Agosto-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Modificar_Horario_Empleado()
            {
                Cls_Ope_Nom_Horarios_Empleados_Negocio Rs_Modificar_Ope_Nom_Horarios_Empleados = new Cls_Ope_Nom_Horarios_Empleados_Negocio(); //Variable de conexión hacia la capa de negocios
                String Fecha_Hora; //Obtiene la fecha y la hora de inicio y termino del horario

                try
                {
                    Rs_Modificar_Ope_Nom_Horarios_Empleados.P_Empleado_ID = Txt_Empleado_ID.Text.ToString();
                    Rs_Modificar_Ope_Nom_Horarios_Empleados.P_No_Horario_Empleado = Txt_No_Horario_Empleado.Text.ToString();
                    
                    Fecha_Hora = String.Format("{0:dd/MMM/yyyy}", Txt_Fecha_Entrada_Horario.Text.ToString() + " 00:00:00");
                    Rs_Modificar_Ope_Nom_Horarios_Empleados.P_Fecha_Inicio = Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy HH:mm:ss}", Fecha_Hora));
                    
                    Fecha_Hora = String.Format("{0:dd/MMM/yyyy}", Txt_Fecha_Termino_Horario.Text.ToString() + " 00:00:00");
                    Rs_Modificar_Ope_Nom_Horarios_Empleados.P_Fecha_Termino = Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy HH:mm:ss}", Fecha_Hora));

                    Rs_Modificar_Ope_Nom_Horarios_Empleados.P_Horario_Entrada = Convert.ToDateTime(String.Format("{0:T}", Txt_Hora_Entrada.Text));
                    Rs_Modificar_Ope_Nom_Horarios_Empleados.P_Horario_Salida = Convert.ToDateTime(String.Format("{0:T}", Txt_Hora_Salida.Text));
                    Rs_Modificar_Ope_Nom_Horarios_Empleados.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

                    Rs_Modificar_Ope_Nom_Horarios_Empleados.Modificar_Horario_Empleado(); //Da de alta los datos del Horario en la BD

                    Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    Habilitar_Controles("Inicial");
                    Consulta_Horarios_Empleado(Txt_Empleado_ID.Text.ToString());

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Horarios de Empleados", "alert('La Modificación del Horario del Empleado fue Exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Modificar_Horario_Empleado " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Eliminar_Horario_Empleado
            /// DESCRIPCION : Elimina el Registro del horario que fue seleccionada por el Usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 06-Agosto-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Eliminar_Horario_Empleado()
            {
                Cls_Ope_Nom_Horarios_Empleados_Negocio Rs_Eliminar_Ope_Nom_Horarios_Empleados = new Cls_Ope_Nom_Horarios_Empleados_Negocio(); //Variable para la conexión de la capa de Negocios
                try
                {
                    Rs_Eliminar_Ope_Nom_Horarios_Empleados.P_Empleado_ID = Txt_Empleado_ID.Text.ToString();
                    Rs_Eliminar_Ope_Nom_Horarios_Empleados.P_No_Horario_Empleado = Txt_No_Horario_Empleado.Text.ToString();
                    Rs_Eliminar_Ope_Nom_Horarios_Empleados.Eliminar_Horario_Empleado(); //Elimina el horario del empleado que selecciono el usuario de la BD
                    Inicializa_Controles();                                             //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    Consulta_Horarios_Empleado(Txt_Empleado_ID.Text.ToString());        //Consulta los horarios del empleado
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Horarios Empleados", "alert('La Eliminación del Horarios del Empleado fue Exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Eliminar_Horario_Empleado " + ex.Message.ToString(), ex);
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
                Consulta_Datos_Empleado(Grid_Busqueda_Empleados.SelectedRow.Cells[1].Text);    //Consulta los datos generales del empleado que fue seleccionado
                Consulta_Horarios_Empleado(Grid_Busqueda_Empleados.SelectedRow.Cells[1].Text); //Consulta los horarios del empleado seleccionado
                Grid_Busqueda_Empleados.Columns[1].Visible = false;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        protected void Grid_Busqueda_Empleados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Limpia_Controles();                                 //Limpia todos los controles de la forma
                Grid_Busqueda_Empleados.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Llena_Grid_Busqueda_Empleado();                     //Carga los horarios que estan asignados a la página seleccionadas
                Grid_Busqueda_Empleados.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        protected void Grid_Horarios_Empleado_Sorting(object sender, GridViewSortEventArgs e)
        {
            Txt_Empleado_ID.Visible = true;
            Consulta_Horarios_Empleado(Txt_Empleado_ID.Text.ToString());//Consulta las asistencias del empleado
            Txt_Empleado_ID.Visible = false;
            DataTable Dt_Horarios = (Grid_Horarios_Empleado.DataSource as DataTable);

            if (Dt_Horarios.Rows.Count > 0)
            {
                DataView Dv_Horarios = new DataView(Dt_Horarios);
                String Orden = ViewState["SortDirection"].ToString();

                if (Orden.Equals("ASC"))
                {
                    Dv_Horarios.Sort = e.SortExpression + " " + "DESC";
                    ViewState["SortDirection"] = "DESC";
                }
                else
                {
                    Dv_Horarios.Sort = e.SortExpression + " " + "ASC";
                    ViewState["SortDirection"] = "ASC";
                }

                Grid_Horarios_Empleado.DataSource = Dv_Horarios;
                Grid_Horarios_Empleado.DataBind();
            }
        }
        protected void Grid_Horarios_Empleado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Limpia_Controles();                                //Limpia todos los controles de la forma
                Grid_Horarios_Empleado.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Llena_Grid_Horarios_Empleado();                    //Carga los horarios que estan asignados a la página seleccionada
                Grid_Horarios_Empleado.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        protected void Grid_Horarios_Empleado_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime Fecha_Hora; //Guarda la fecha con la hora que se tiene en la base de datos
            String Hora;         //Obtiene la hora de entrada y salida del empleado
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Txt_No_Horario_Empleado.Text = Grid_Horarios_Empleado.SelectedRow.Cells[1].Text;
                Txt_Fecha_Entrada_Horario.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(HttpUtility.HtmlDecode(Grid_Horarios_Empleado.SelectedRow.Cells[2].Text)));
                Txt_Fecha_Termino_Horario.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(HttpUtility.HtmlDecode(Grid_Horarios_Empleado.SelectedRow.Cells[3].Text)));

                Fecha_Hora = Convert.ToDateTime(HttpUtility.HtmlDecode(Grid_Horarios_Empleado.SelectedRow.Cells[4].Text));
                Hora = Fecha_Hora.ToString("T", DateTimeFormatInfo.InvariantInfo);
                Txt_Hora_Entrada.Text = Hora;

                Fecha_Hora = Convert.ToDateTime(HttpUtility.HtmlDecode(Grid_Horarios_Empleado.SelectedRow.Cells[5].Text));
                Hora = Fecha_Hora.ToString("T", DateTimeFormatInfo.InvariantInfo);
                Txt_Hora_Salida.Text = Hora;
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
        /// FECHA_CREO  : 06-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Btn_Cerrar_Ventana_Click(object sender, ImageClickEventArgs e)
        {
            Mpe_Busqueda_Empleados.Hide();
        }
        protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                if (!String.IsNullOrEmpty(Txt_No_Empleado_Horario.Text))
                {
                    if (Btn_Nuevo.ToolTip == "Nuevo")
                    {
                        Limpia_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                        Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                    }
                    else
                    {
                        //Valida si todos los campos requeridos estan llenos si es así da de alta los datos en la base de datos
                        if (Validar_Datos_Horario_Empleado())
                        {
                            Alta_Horario_Empleado(); //Da de alta el horario con los datos que proporciono el usuario
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
            if (!String.IsNullOrEmpty(Txt_No_Empleado_Horario.Text))
            {
                if (Btn_Modificar.ToolTip.Trim().Equals("Modificar"))
                {
                    if (!String.IsNullOrEmpty(Txt_No_Horario_Empleado.Text))
                    {
                        Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Seleccione el Horario del Empleado que desea modificar sus datos <br>";
                    }
                }
                else
                {
                    //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos valores en la base de datos
                    if (Validar_Datos_Horario_Empleado())
                    {
                        Modificar_Horario_Empleado(); //Modifica los datos del horario seleccionado con los datos proporcionados por el usuario
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
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
        protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                if (!String.IsNullOrEmpty(Txt_No_Empleado_Horario.Text))
                {
                    //Si el usuario selecciono un horario entonces la elimina de la base de datos
                    if (!String.IsNullOrEmpty(Txt_No_Horario_Empleado.Text))
                    {
                        Eliminar_Horario_Empleado(); //Elimina el Horario del Empleado que fue seleccionada por el usuario
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Seleccione el horario que desea eliminar <br>";
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Realizar la búsqueda del empleado antes de realizar la operacion <br>";
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
                    Session.Remove("Consulta_Horarios_Empleado");
                    Session.Remove("Consulta_Empleados");
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
                else
                {
                    Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario en la operación
                    Txt_Empleado_ID.Visible = true;
                    Consulta_Horarios_Empleado(Txt_Empleado_ID.Text.ToString());
                    Txt_Empleado_ID.Visible = false;
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
