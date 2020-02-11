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
using Presidencia.Sessiones;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Empleados.Negocios;
using Presidencia.Empleados_Folios.Negocio;

public partial class paginas_Predial_Frm_Ope_Caj_Empleados_Folios : System.Web.UI.Page
{
    #region (Page Load)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
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
    #region(Metodos)
        #region (Metodos Generales)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Inicializa_Controles
            /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
            ///               diferentes operaciones
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16-Octubre-2010
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
            /// FECHA_CREO  : 16-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpia_Controles()
            {
                try
                {
                    //Datos del registro del Empleado
                    Txt_Busqueda_Empleado.Text = "";
                    Txt_Empleado_ID.Text = "";
                    Txt_No_Empleado.Text = "";
                    Txt_Nombre_Empleado.Text = "";
                    //Datos del registro del Folio
                    Txt_No_Folio.Text = "";
                    Txt_Folio_Final.Text = "";
                    Txt_Folio_Inicial.Text = "";
                    Txt_Ultimo_Folio_Utilizado.Text = "";
                    Grid_Empleados_Folios.DataSource = new DataTable();
                    Grid_Empleados_Folios.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Limpia_Controles_Folio
            /// DESCRIPCION : Limpia los controles que pertencen solamente al registro del folio
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpia_Controles_Folio()
            {
                try
                {
                    Txt_Busqueda_Empleado.Text = "";
                    Txt_Folio_Final.Text = "";
                    Txt_Folio_Inicial.Text = "";
                    Txt_No_Folio.Text = "";
                    Txt_Ultimo_Folio_Utilizado.Text = "";
                }
                catch (Exception ex)
                {
                    throw new Exception("Limpia_Controles_Folio " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Habilitar_Controles
            /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
            ///               para a siguiente operación
            /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
            ///                          si es una alta, modificacion
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16-Octubre-2010
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

                            Configuracion_Acceso("Frm_Ope_Caj_Empleados_Folios.aspx");
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
                    Txt_Folio_Inicial.Enabled = Habilitado;
                    Txt_Folio_Final.Enabled = Habilitado;
                    Txt_Busqueda_Empleado.Enabled = !Habilitado;
                    Btn_Buscar_Empleado.Enabled = !Habilitado;
                    Grid_Empleados_Folios.Enabled = !Habilitado;
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
            ///NOMBRE DE LA FUNCION: Configuracion_Acceso
            ///DESCRIPCIÓN : Habilita las operaciones que podrá realizar el usuario en la página.
            ///PARÁMETROS  : No Áplica.
            ///USUARIO CREÓ: Juan Alberto Hernández Negrete.
            ///FECHA CREÓ  : 23/Mayo/2011 10:43 a.m.
            ///USUARIO MODIFICO  :
            ///FECHA MODIFICO    :
            ///CAUSA MODIFICACIÓN:
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
                    Botones.Add(Btn_Buscar_Empleado);

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
            ///NOMBRE DE LA FUNCION: IsNumeric
            ///DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
            ///PARÁMETROS  : Cadena.- El dato a evaluar si es numerico.
            ///USUARIO CREÓ: Juan Alberto Hernandez Negrete
            ///FECHA_CREO  : 29/Noviembre/2010
            ///USUARIO MODIFICO  :
            ///FECHA_MODIFICO    :
            ///CAUSA_MODIFICACION:
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
        #region (Métodos Consulta Datos)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Empleado
            /// DESCRIPCION : Consulta el ID, No y Nombre del empleado que esta logeado en el sistema
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
                    Rs_Consulta_Cat_Empleados.P_Nombre =String.Format("{0:000000}", Convert.ToInt32(Txt_Busqueda_Empleado.Text.ToString()));
                    Dt_Datos_Empleados = Rs_Consulta_Cat_Empleados.Consulta_Empleados(); //Consulta los datos generales del empleado
                    //Asigna los valores de la consulta a los campos correspondientes
                    foreach (DataRow Registro in Dt_Datos_Empleados.Rows)
                    {
                        Txt_Empleado_ID.Text = Registro[Cat_Empleados.Campo_Empleado_ID].ToString();
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
            /// NOMBRE DE LA FUNCION: Consulta_Folios_Empleados
            /// DESCRIPCION : Llena el grid con los Folios que pertenece al empleado y que 
            ///               fueron registtados con anterioridad
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Folios_Empleados()
            {
                Cls_Ope_Caj_Empleados_Folio_Negocios Rs_Consulta_Ope_Caj_Empleados_Folio = new Cls_Ope_Caj_Empleados_Folio_Negocios(); //Variable de conexión hacia la capa de Negocios
                DataTable Dt_Folios_Empleados; //Variable que obtendra los datos de la consulta 

                try
                {
                    Session.Remove("Consulta_Folios");
                    Rs_Consulta_Ope_Caj_Empleados_Folio.P_Empleado_ID = Txt_Empleado_ID.Text;
                    Dt_Folios_Empleados = Rs_Consulta_Ope_Caj_Empleados_Folio.Consulta_Datos_Folios_Empleados(); //Consulta todos los folios que fueron asignado al empleado con sus datos generales
                    Session["Consulta_Folios"] = Dt_Folios_Empleados;
                    Llena_Grid_Folios_Empleados();
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Folios_Empleados " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Llena_Grid_Folios_Empleados
            /// DESCRIPCION : Llena el grid con los Folios del Empleado que se encuentran en la BD
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Llena_Grid_Folios_Empleados()
            {
                DataTable Dt_Folios_Empleados; //Variable que obtendra los datos de la consulta 
                try
                {
                    Grid_Empleados_Folios.DataSource = new DataTable();
                    Grid_Empleados_Folios.DataBind();
                    Dt_Folios_Empleados = (DataTable)Session["Consulta_Folios"];
                    Grid_Empleados_Folios.DataSource = Dt_Folios_Empleados;
                    Grid_Empleados_Folios.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Llena_Grid_Folios_Empleados " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Folio_Repetido
            /// DESCRIPCION : 1.Consulta si el folio inicial y final no estan ya dados de alta
            ///               2. Si estan dados de alta los dos o alguno de ellos enviar un valor
            ///                  de falso para indicar que estos ya fueron asignados, si no
            ///                  fueron asignados regresa un valor de verdadero para indicar
            ///                  que estos folios pueden utilizarse
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Consulta_Folio_Repetido()
            {
                Boolean Datos_Validos = true;//Variable que almacena el valor de true si no fue encontrado ni el folio inicial ni final en la base de datos
                DataTable Dt_Folios_Empleados; //Variable que obtendra los datos de la consulta 
                Cls_Ope_Caj_Empleados_Folio_Negocios Rs_Consulta_Ope_Caj_Empleados_Folio = new Cls_Ope_Caj_Empleados_Folio_Negocios(); //Variable para la conexión hacia la base de datos
                try
                {
                    Txt_Folio_Inicial.Text = String.Format("{0:0000000000}", Convert.ToInt32(Txt_Folio_Inicial.Text));
                    Txt_Folio_Final.Text = String.Format("{0:0000000000}", Convert.ToInt32(Txt_Folio_Final.Text));
                    if (!String.IsNullOrEmpty(Txt_No_Folio.Text)) Rs_Consulta_Ope_Caj_Empleados_Folio.P_No_Folio = Txt_No_Folio.Text.ToString();
                    Rs_Consulta_Ope_Caj_Empleados_Folio.P_Folio_Inicial=Txt_Folio_Inicial.Text.ToString();
                    Dt_Folios_Empleados = Rs_Consulta_Ope_Caj_Empleados_Folio.Consulta_Rango_Folio_Empleado(); //Consulta si el folio inicial ya fue dado de alta con anterioridad
                    //Si se encontro el folio dado de alta entonces regresa un valor de falso
                    if (Dt_Folios_Empleados.Rows.Count > 0)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + el folio inicial ya fue asignado a otro empleado <br>";
                        Datos_Validos = false;
                    }
                    Dt_Folios_Empleados = new DataTable();
                    Rs_Consulta_Ope_Caj_Empleados_Folio.P_Folio_Inicial = Txt_Folio_Final.Text.ToString();
                    Dt_Folios_Empleados = Rs_Consulta_Ope_Caj_Empleados_Folio.Consulta_Rango_Folio_Empleado(); //Consulta si el folio final ya fue dado de alta con anterioridad
                    //Si se encontro el folio dado de alta entonces regresa un valor de falso
                    if (Dt_Folios_Empleados.Rows.Count > 0)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + el folio final ya fue asignado a otro empleado <br>";
                        Datos_Validos = false;
                    }
                    return Datos_Validos;
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Folio_Repetido " + ex.Message.ToString(), ex);
                }
            }
        #endregion
        #region (Métodos Operacion)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Validar_Datos_Folio
            /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16/Octubre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Validar_Datos_Folio()
            {
                Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
                Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

                if (string.IsNullOrEmpty(Txt_Folio_Inicial.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Folio Inicial es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }

                if (string.IsNullOrEmpty(Txt_Folio_Final.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Folio Final es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                return Datos_Validos;
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCION: Alta_Folio_Empleado
            ///DESCRIPCION : Asigna al empleado los folios que proporciona el usuario
            ///PARAMETROS  : 
            ///CREO        : Yazmin A Delgado Gómez
            ///FECHA_CREO  : 16-Octubre-2011
            ///MODIFICO          :
            ///FECHA_MODIFICO    :
            ///CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Alta_Folio_Empleado()
            {
                Cls_Ope_Caj_Empleados_Folio_Negocios Rs_Alta_Ope_Caj_Empleados_Folios = new Cls_Ope_Caj_Empleados_Folio_Negocios(); //Variable para la conexión hacia la capa de negocios

                try
                {
                    Txt_Folio_Inicial.Text = String.Format("{0:0000000000}", Convert.ToInt32(Txt_Folio_Inicial.Text));
                    Txt_Folio_Final.Text = String.Format("{0:0000000000}", Convert.ToInt32(Txt_Folio_Final.Text));
                    Rs_Alta_Ope_Caj_Empleados_Folios.P_Empleado_ID = Txt_Empleado_ID.Text.ToString();
                    Rs_Alta_Ope_Caj_Empleados_Folios.P_Folio_Inicial = Txt_Folio_Inicial.Text.ToString();
                    Rs_Alta_Ope_Caj_Empleados_Folios.P_Folio_Final = Txt_Folio_Final.Text.ToString();
                    Rs_Alta_Ope_Caj_Empleados_Folios.P_Nombre_Empleado = Cls_Sessiones.Nombre_Empleado;
                    Rs_Alta_Ope_Caj_Empleados_Folios.Alta_Folio_Empleado(); //Da de alta el registro del folio con el empleado en la base de datos

                    Limpia_Controles_Folio();//Limpia los controles de la forma
                    Consulta_Folios_Empleados(); //Consulta todos los folios del empleadp
                    Habilitar_Controles("Inicial");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Asignación de Folios", "alert('La asignación de Folio al Empleado fue Exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Alta_Folio_Empleado " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Modificar_Folio_Empleado
            /// DESCRIPCION : Modifica el registro del folio que selecciono el usuario modificando
            ///               sus valores en la base de datos
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Modificar_Folio_Empleado()
            {
                Cls_Ope_Caj_Empleados_Folio_Negocios Rs_Modificar_Ope_Caj_Empleados_Folios = new Cls_Ope_Caj_Empleados_Folio_Negocios();  //Variable de conexión hacia la capa de Negocios
                try
                {
                    Txt_Folio_Inicial.Text = String.Format("{0:0000000000}", Convert.ToInt32(Txt_Folio_Inicial.Text));
                    Txt_Folio_Final.Text = String.Format("{0:0000000000}", Convert.ToInt32(Txt_Folio_Final.Text));
                    Rs_Modificar_Ope_Caj_Empleados_Folios.P_No_Folio = Txt_No_Folio.Text.ToString();
                    Rs_Modificar_Ope_Caj_Empleados_Folios.P_Folio_Inicial = Txt_Folio_Inicial.Text.ToString();
                    Rs_Modificar_Ope_Caj_Empleados_Folios.P_Folio_Final = Txt_Folio_Final.Text.ToString();
                    Rs_Modificar_Ope_Caj_Empleados_Folios.P_Nombre_Empleado = Cls_Sessiones.Nombre_Empleado;

                    Rs_Modificar_Ope_Caj_Empleados_Folios.Modificar_Folio_Empleado(); //Sustituye los datos del registro por los proporcionados por el usuario
                    Limpia_Controles_Folio();//Limpia los controles de la forma
                    Consulta_Folios_Empleados(); //Consulta todos los folios del empleado
                    Habilitar_Controles("Inicial");
                    Txt_Ultimo_Folio_Utilizado.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Asignación de Folios", "alert('La Modificación del registro del Folio fue Exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Modificar_Folio_Empleado " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Eliminar_Folio_Empleado
            /// DESCRIPCION : Elimina el registro del folio seleccionado por el usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Eliminar_Folio_Empleado()
            {
                Cls_Ope_Caj_Empleados_Folio_Negocios Rs_Eliminar_Caj_Empleados_Folios = new Cls_Ope_Caj_Empleados_Folio_Negocios(); //Variable de conexión hacia la capa de Negocios
                try
                {
                    Rs_Eliminar_Caj_Empleados_Folios.P_No_Folio = Txt_No_Folio.Text;
                    Rs_Eliminar_Caj_Empleados_Folios.Eliminar_Folio_Empleado(); //Elimina el reloj checador seleccionado por el usuario de la BD

                    Limpia_Controles_Folio();//Limpia los controles de la forma
                    Consulta_Folios_Empleados(); //Consulta todos los folios del empleado
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Asignación de Folios", "alert('La Eliminación del Folio fue Exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Eliminar_Folio_Empleado " + ex.Message.ToString(), ex);
                }
            }
        #endregion
    #endregion
    #region(Grid)
        protected void Grid_Empleados_Folios_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Limpia_Controles_Folio();//Limpia los controles del folio
                Txt_No_Folio.Text = Grid_Empleados_Folios.SelectedRow.Cells[1].Text;
                Txt_Folio_Inicial.Text = Grid_Empleados_Folios.SelectedRow.Cells[2].Text;
                Txt_Folio_Final.Text = Grid_Empleados_Folios.SelectedRow.Cells[3].Text;
                if (Grid_Empleados_Folios.SelectedRow.Cells[4].Text != "&nbsp;") Txt_Ultimo_Folio_Utilizado.Text = HttpUtility.HtmlDecode(Grid_Empleados_Folios.SelectedRow.Cells[4].Text);
                if (!String.IsNullOrEmpty(Txt_Ultimo_Folio_Utilizado.Text)) Txt_Ultimo_Folio_Utilizado.Enabled = false;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        protected void Grid_Empleados_Folios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Limpia_Controles_Folio();                         //Limpia todos los controles de la forma
                Grid_Empleados_Folios.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Llena_Grid_Folios_Empleados();                    //Carga los folios que estan asignadas a la página seleccionada
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        protected void Grid_Empleados_Folios_Sorting(object sender, GridViewSortEventArgs e)
        {
            Consulta_Folios_Empleados();
            DataTable Dt_Folios_Empleados = (Grid_Empleados_Folios.DataSource as DataTable);

            if (Dt_Folios_Empleados != null)
            {
                DataView Dv_Folios_Empleados = new DataView(Dt_Folios_Empleados);
                String Orden = ViewState["SortDirection"].ToString();

                if (Orden.Equals("ASC"))
                {
                    Dv_Folios_Empleados.Sort = e.SortExpression + " " + "DESC";
                    ViewState["SortDirection"] = "DESC";
                }
                else
                {
                    Dv_Folios_Empleados.Sort = e.SortExpression + " " + "ASC";
                    ViewState["SortDirection"] = "ASC";
                }

                Grid_Empleados_Folios.DataSource = Dv_Folios_Empleados;
                Grid_Empleados_Folios.DataBind();
            }
        }
    #endregion
    #region(Eventos)
        protected void Btn_Buscar_Empleado_Click(object sender, ImageClickEventArgs e)
    {
        String No_Empleado; //Guarda el No_Empleado a Seleccionar
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            No_Empleado = String.Format("{0:000000}", Txt_Busqueda_Empleado.Text.ToString());
            Limpia_Controles();   //Limpia los controles de la forma
            Txt_Busqueda_Empleado.Text = No_Empleado;
            Consulta_Empleado();  //Consulta el nombre del empleado del cual se desea saber su información
            if (String.IsNullOrEmpty(Txt_No_Empleado.Text))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El No. Empleado no fue encontrado en el sistema, favor de verificar. <br>";
            }
            else
            {
                Txt_Busqueda_Empleado.Text = "";
                Consulta_Folios_Empleados();//Consulta todos los folios que tiene seleccionado el empleado
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
                if (!String.IsNullOrEmpty(Txt_Empleado_ID.Text))
                {
                    if (Btn_Nuevo.ToolTip == "Nuevo")
                    {
                        Limpia_Controles_Folio();     //Limpia los controles de la forma para poder introducir nuevos datos
                        Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                        Txt_Folio_Inicial.Enabled = true;
                    }
                    else
                    {
                        //Valida si todos los campos requeridos estan llenos si es así da de alta los datos en la base de datos
                        if (Validar_Datos_Folio())
                        {
                            if (Convert.ToDouble(Txt_Folio_Inicial.Text) <= Convert.ToDouble(Txt_Folio_Final.Text))
                            {
                                if (Consulta_Folio_Repetido())
                                {
                                    Alta_Folio_Empleado(); //Da de alta el registro del folio con los datos que proporciono el usuario
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
                                Lbl_Mensaje_Error.Text = "El folio inicial no puede ser mayor al folio final, favor de verificar <br>";
                            }
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
                    Lbl_Mensaje_Error.Text = "Debe consultar primero al Empleado para poder asignar un folio <br>";
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
                if (!String.IsNullOrEmpty(Txt_Empleado_ID.Text))
                {
                    if (Btn_Modificar.ToolTip == "Modificar")
                    {
                        if (!String.IsNullOrEmpty(Txt_No_Folio.Text))
                        {
                            Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Seleccione el registro que desea modificar sus datos <br>";
                        }
                    }
                    else
                    {

                        //Valida si todos los campos requeridos estan llenos si es así da de alta los datos en la base de datos
                        if (Validar_Datos_Folio())
                        {
                            if (Convert.ToDouble(Txt_Folio_Inicial.Text) <= Convert.ToDouble(Txt_Folio_Final.Text))
                            {
                                if (!String.IsNullOrEmpty(Txt_Ultimo_Folio_Utilizado.Text))
                                {
                                    if (Convert.ToDouble(Txt_Ultimo_Folio_Utilizado.Text) >= Convert.ToDouble(Txt_Folio_Final.Text))
                                    {
                                        Lbl_Mensaje_Error.Visible = true;
                                        Img_Error.Visible = true;
                                        Lbl_Mensaje_Error.Text = "El folio utilizado no puede ser mayor al folio final, favor de verificar <br>";
                                    }
                                    else
                                    {
                                        if (Consulta_Folio_Repetido())
                                        {
                                            Modificar_Folio_Empleado(); //Modifica el registro del folio con los datos proporcionados por el usuario
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
                                    if (Consulta_Folio_Repetido())
                                    {
                                        Modificar_Folio_Empleado(); //Modifica el registro del folio con los datos proporcionados por el usuario
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
                                Lbl_Mensaje_Error.Text = "El folio inicial no puede ser mayor al folio final, favor de verificar <br>";
                            }
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
                    Lbl_Mensaje_Error.Text = "Debe consultar primero al Empleado para poder indicar que registro desea modificar <br>";
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
                if (!String.IsNullOrEmpty(Txt_Empleado_ID.Text))
                {
                    //Si el usuario selecciono una Escolaridad entonces la elimina de la base de datos
                    if (!String.IsNullOrEmpty(Txt_No_Folio.Text.ToString()))
                    {
                        if (String.IsNullOrEmpty(Txt_Ultimo_Folio_Utilizado.Text.ToString()))
                        {
                            Eliminar_Folio_Empleado(); //Elimina el folio que fue seleccionado por el usuario
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "No se puede borrar el registro porque ya fueron realizados pagos con estos <br>";
                        }
                    }
                    //Si el usuario no selecciono algun folio manda un mensaje indicando que es necesario que seleccione alguna para
                    //poder eliminar
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Seleccione el Folio que desea eliminar <br>";
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Debe consultar primero al Empleado para poder indicar que registro desea eliminar <br>";
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
                    Session.Remove("Consulta_Escolaridad");
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
                else
                {
                    Limpia_Controles_Folio();//Limpia los controles de la forma
                    Consulta_Folios_Empleados(); //Consulta todos los folios del empleadp
                    Habilitar_Controles("Inicial");
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
