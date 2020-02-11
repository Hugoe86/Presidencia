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
using Presidencia.Empleados.Negocios;
using Presidencia.Cajas_Empleados.Negocios;

public partial class paginas_Predial_Frm_Cat_Pre_Cajas_Empleados : System.Web.UI.Page
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
            /// FECHA_CREO  : 10/Octubre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Inicializa_Controles()
            {
                try
                {
                    Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
                    Limpia_Controles();             //Limpia los controles de la pantalla para poder asignar los nuevos valores
                    Cargar_Combo_Modulos();         //Carga los modulos de cajas que se encuentran actualmente vigites
                    Consulta_Empleado();            //Consulta los datos generales del empleado que esta logeado en el sistema
                    Consulta_Caja_Empleado();       //Consulta el módulo y la caja del módulo que tiene asignado el empleado
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
            /// FECHA_CREO  : 02/Agosto/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpia_Controles()
            {
                try
                {
                    //Controles para la asistencia del personal
                    Txt_No_Empleado.Text = "";
                    Txt_Nombre_Empleado.Text = "";
                    Txt_Estatus_Empleado.Text = "";
                    Cmb_Modulos_Caja.SelectedIndex = -1;
                    Cmb_Cajas_Modulos.DataSource = new DataTable();
                    Cmb_Cajas_Modulos.DataBind();
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
                            Btn_Modificar.ToolTip = "Modificar";
                            Btn_Salir.ToolTip = "Salir";
                            Btn_Nuevo.Visible = true;
                            Btn_Modificar.Visible = true;
                            Btn_Nuevo.CausesValidation = false;
                            Btn_Modificar.CausesValidation = false;
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Configuracion_Acceso("Frm_Cat_Pre_Cajas_Empleados.aspx");
                            break;

                        case "Nuevo":
                            Habilitado = true;
                            Btn_Nuevo.ToolTip = "Dar de Alta";
                            Btn_Modificar.ToolTip = "Modificar";
                            Btn_Salir.ToolTip = "Cancelar";
                            Btn_Nuevo.Visible = true;
                            Btn_Modificar.Visible = false;
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
                            Btn_Nuevo.CausesValidation = true;
                            Btn_Modificar.CausesValidation = true;
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                            break;
                    }
                    Cmb_Cajas_Modulos.Enabled = Habilitado;
                    Cmb_Modulos_Caja.Enabled=Habilitado;
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
            /// DESCRIPCIÓN : Habilita las operaciones que podrá realizar el usuario en la página
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
            /// NOMBRE DE LA FUNCION: Validar_Datos_Caja_Empleado
            /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 10/Octubre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Validar_Datos_Caja_Empleado()
            {
                Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
                Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

                if (Cmb_Modulos_Caja.SelectedIndex<=0)
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El módulo de la caja. <br>";
                    Datos_Validos = false;
                }
                if (Cmb_Cajas_Modulos.SelectedIndex<=0)
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La caja que se le asignará al empleado <br>";
                    Datos_Validos = false;
                }
                return Datos_Validos;
            }
        #endregion
        #region (Consultas)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Cargar_Combo_Modulos
            /// DESCRIPCION : Consulta los Módulos de cajas que se encuentren vigentes para
            ///               poder realizar la apertura del turno
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 10-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Cargar_Combo_Modulos()
            {
                Cls_Cat_Pre_Cajas_Empleados_Negocios Rs_Consulta_Cat_Pre_Modulos = new Cls_Cat_Pre_Cajas_Empleados_Negocios(); //Variable de conexión con la capa de negocios.
                DataTable Dt_Modulos_Cajas = null; //Variable que almacena un listado de modulos registrados actualmente en el sistema.

                try
                {
                    Dt_Modulos_Cajas = Rs_Consulta_Cat_Pre_Modulos.Consulta_Modulos_Cajas();
                    Cmb_Modulos_Caja.DataSource = Dt_Modulos_Cajas;
                    Cmb_Modulos_Caja.DataTextField = "Modulo";
                    Cmb_Modulos_Caja.DataValueField = Cat_Pre_Modulos.Campo_Modulo_Id;
                    Cmb_Modulos_Caja.DataBind();

                    Cmb_Modulos_Caja.Items.Insert(0, new ListItem("<--  Seleccione  -->", ""));
                    Cmb_Modulos_Caja.SelectedIndex = -1;
                }
                catch (Exception Ex)
                {
                    throw new Exception("Cargar_Combo_Modulos. Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Cargar_Combo_Cajas_Modulo
            /// DESCRIPCION : Consulta las Cajas que tiene asignadas el módulo que fue asignado
            ///               o seleccionado por el empleado y que se encuentren Vigentes
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 10-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Cargar_Combo_Cajas_Modulo()
            {
                Cls_Cat_Pre_Cajas_Empleados_Negocios Rs_Consulta_Cat_Pre_Cajas = new Cls_Cat_Pre_Cajas_Empleados_Negocios(); //Variable de conexión con la capa de negocios.
                DataTable Dt_Cajas = null; //Variable que almacena un listado de modulos registrados actualmente en el sistema.

                try
                {
                    Rs_Consulta_Cat_Pre_Cajas.P_Modulo_ID = Cmb_Modulos_Caja.SelectedValue;
                    Dt_Cajas = Rs_Consulta_Cat_Pre_Cajas.Consulta_Cajas_Modulo();
                    Cmb_Cajas_Modulos.DataSource = Dt_Cajas;
                    Cmb_Cajas_Modulos.DataTextField = "Caja";
                    Cmb_Cajas_Modulos.DataValueField = Cat_Pre_Cajas.Campo_Caja_ID;
                    Cmb_Cajas_Modulos.DataBind();

                    Cmb_Cajas_Modulos.Items.Insert(0, new ListItem("<--  Seleccione  -->", ""));
                    Cmb_Cajas_Modulos.SelectedIndex = -1;
                }
                catch (Exception Ex)
                {
                    throw new Exception("Cargar_Combo_Cajas_Modulo. Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Caja_Empleado
            /// DESCRIPCION : Consulta la caja y el modulo en el cual esta asignado el empleado
            ///               que intenta abrir el turno
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 10-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Caja_Empleado()
            {
                Cls_Cat_Pre_Cajas_Empleados_Negocios Rs_Consulta_Cat_Pre_Cajas_Empleados = new Cls_Cat_Pre_Cajas_Empleados_Negocios(); //Variable de conexión hacia la capa de negocios
                DataTable Dt_Caja_Empleado = new DataTable(); //Obtiene los valores de la consulta
                try
                {
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Cmb_Modulos_Caja.SelectedIndex = -1;
                    Cmb_Cajas_Modulos.DataSource = null;
                    Cmb_Cajas_Modulos.DataBind();

                    Rs_Consulta_Cat_Pre_Cajas_Empleados.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                    Dt_Caja_Empleado = Rs_Consulta_Cat_Pre_Cajas_Empleados.Consulta_Caja_Empleado(); //Consulta la caja que tiene asignada el empleado

                    //Si el usuario ya esta asignado a una caja valida que el módulo a la cual pertenece la caja y la caja esten vigentes
                    //para poder asignarle la caja y el módulo en automático al empleado que intenta abrir el turno
                    if (Dt_Caja_Empleado.Rows.Count > 0)
                    {
                        foreach (DataRow Registro in Dt_Caja_Empleado.Rows)
                        {
                            //Valida que los estatus de la caja y módulo esten vigentes para podersela asignar al empleado de manera automática
                            if (Registro["Estatus_Modulo"].ToString() == "VIGENTE" && Registro["Estatus_Caja"].ToString() == "VIGENTE")
                            {
                                Cmb_Modulos_Caja.SelectedValue = Registro[Cat_Pre_Cajas.Campo_Modulo_Id].ToString();
                                Cargar_Combo_Cajas_Modulo(); //Consulta las cajas que tiene asignado el módulo al cual pertenece el empleado
                                Cmb_Cajas_Modulos.SelectedValue = Registro[Cat_Pre_Cajas.Campo_Caja_ID].ToString();
                                Btn_Nuevo.Visible = false;
                                Btn_Modificar.Visible = true;
                            }                            
                        }
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Consulta_Caja_Empleado. Error: [" + Ex.Message + "]");
                }
            }
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
                        Txt_Estatus_Empleado.Text = Registro[Cat_Empleados.Campo_Estatus].ToString();
                        Txt_No_Empleado.Text = Registro[Cat_Empleados.Campo_No_Empleado].ToString();
                        Txt_Nombre_Empleado.Text = Registro["Empleado"].ToString();
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Consulta_Empleado. Error: [" + Ex.Message + "]");
                }
            }
        #endregion
        #region (Métodos Operacion [Alta -  Modificacion])
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Alta_Caja_Empleado
            /// DESCRIPCION : Da de Alta la Caja que tendrá asignada el empleado
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 10-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Alta_Caja_Empleado()
            {
                Cls_Cat_Pre_Cajas_Empleados_Negocios Rs_Alta_Cat_Pre_Cajas_Empleados = new Cls_Cat_Pre_Cajas_Empleados_Negocios();  //Variable de conexión hacia la capa de Negocios

                try
                {
                    Rs_Alta_Cat_Pre_Cajas_Empleados.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                    Rs_Alta_Cat_Pre_Cajas_Empleados.P_Caja_ID = Cmb_Cajas_Modulos.SelectedValue;

                    Rs_Alta_Cat_Pre_Cajas_Empleados.Alta_Caja_Empleado(); //Asigna la caja al empleado
                    Habilitar_Controles("Inicial"); //Habilita los controles de la pantalla para la siguiente operación del sistema
                    Btn_Nuevo.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alta de Caja del Empleado", "alert('La Caja fue asignada al Empleado');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Alta_Caja_Empleado " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Modificar_Caja_Empleado
            /// DESCRIPCION : Reasigna la caja al empleado
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 10-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Modificar_Caja_Empleado()
            {
                Cls_Cat_Pre_Cajas_Empleados_Negocios Rs_Modificar_Cat_Pre_Cajas_Empleados = new Cls_Cat_Pre_Cajas_Empleados_Negocios(); //Variable de conexión hacia la capa de negocios

                try
                {
                    Rs_Modificar_Cat_Pre_Cajas_Empleados.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                    Rs_Modificar_Cat_Pre_Cajas_Empleados.P_Caja_ID = Cmb_Cajas_Modulos.SelectedValue;
                    Rs_Modificar_Cat_Pre_Cajas_Empleados.Modificar_Caja_Empleado(); //Realiza la reasignación de la caja al empleado

                    Habilitar_Controles("Inicial"); //Habilita los controles de la pantalla para la siguiente operación del sistema
                    Btn_Nuevo.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Asignación de Caja al Empleados", "alert('La reasignación de la caja fue exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Modificar_Caja_Empleado " + ex.Message.ToString(), ex);
                }
            }
        #endregion
    #endregion
    #region (Operacion [Alta - Modificar - Consultar])
        protected void Cmb_Modulos_Caja_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Cmb_Cajas_Modulos.DataSource = null;
                Cmb_Cajas_Modulos.DataBind();
                if (Cmb_Modulos_Caja.SelectedIndex > 0) Cargar_Combo_Cajas_Modulo(); //Consulta las cajas que estan asignadas al modulo seleccionado por el usuario y que se encuentren vigentes
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Ex.Message.ToString();
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
                    Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                }
                else
                {
                    //Valida si todos los campos requeridos estan llenos si es así da de alta los datos en la base de datos
                    if (Validar_Datos_Caja_Empleado())
                    {
                        Alta_Caja_Empleado(); //Asigna la caja al empleado
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
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Modificar.ToolTip.Trim().Equals("Modificar"))
            {
                Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos valores en la base de datos
                if (Validar_Datos_Caja_Empleado())
                {
                    Modificar_Caja_Empleado(); //Reasigna la caja seleccionada por el usuario al empleado
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
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
}
