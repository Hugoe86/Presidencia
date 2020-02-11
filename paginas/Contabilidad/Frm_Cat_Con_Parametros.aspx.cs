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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Parametros_Contabilidad.Negocio;

public partial class paginas_Contabilidad_Frm_Cat_Con_Parametros : System.Web.UI.Page
{
    #region (Page_Load)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Page_Load
    /// DESCRIPCION : Carga la configuración inicial de los controles de la página.
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 15/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
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
        #region (Metodos Generales)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Inicializa_Controles
            /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
            ///               diferentes operaciones
            /// PARAMETROS  : 
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 15/Septiembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Inicializa_Controles()
            {
                Cls_Cat_Con_Parametros_Negocio Rs_Consulta_Cat_Con_Parametros = new Cls_Cat_Con_Parametros_Negocio(); //Variable de conexión hacia la capa de Negocios
                DataTable Dt_Parametros; //Variable que obtendra los datos de la consulta 
                try
                {
                    Dt_Parametros = Rs_Consulta_Cat_Con_Parametros.Consulta_Datos_Parametros();//Consulta los datos generales de las Cuentas Contables dados de alta en la BD
                    if (Dt_Parametros.Rows.Count == 0)
                        Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
                    else
                        Habilitar_Controles("Existe"); //Habilita solo los controles de Modificar debido a que ya existe un registro.
                    Limpia_Controles();             //Limpia los controles del forma
                    Consulta_Parametros();
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
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 15/Septiembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpia_Controles()
            {
                try
                {
                    Txt_Mascara_Cuenta_Contable_Parametros.Text = "";
                    Cmb_Mes_Contable.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    throw new Exception("Limpia_Controles " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Habilitar_Controles
            /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
            ///                para a siguiente operación
            /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
            ///                           si es una alta, modificacion
            ///                           
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 15/Septiembre/2011
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
                            Btn_Nuevo.CausesValidation = false;
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                            Configuracion_Acceso("Frm_Cat_Con_Parametros.aspx");
                            break;

                        case "Existe":
                            Habilitado = false;
                            Btn_Nuevo.ToolTip = "Nuevo";
                            Btn_Modificar.ToolTip = "Modificar";
                            Btn_Salir.ToolTip = "Salir";
                            Btn_Nuevo.Visible = false;
                            Btn_Modificar.Visible = true;
                            Btn_Nuevo.CausesValidation = true;
                            Btn_Modificar.CausesValidation = true;
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            break;

                        case "Nuevo":
                            Habilitado = true;
                            Btn_Nuevo.ToolTip = "Dar de Alta";
                            Btn_Modificar.ToolTip = "Modificar";
                            Btn_Salir.ToolTip = "Cancelar";
                            Btn_Nuevo.Visible = true;
                            Btn_Modificar.Visible = false;
                            Btn_Nuevo.CausesValidation = true;
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
                    Txt_Mascara_Cuenta_Contable_Parametros.Enabled = Habilitado;
                    Cmb_Mes_Contable.Enabled = Habilitado;
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
            /// NOMBRE: Configuracion_Acceso
            /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
            /// PARÁMETROS  :
            /// USUARIO CREÓ: Salvador L. Rea Ayala
            /// FECHA CREÓ  : 15/Septiembre/2011
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
            /// USUARIO CREÓ: Salvador L. Rea Ayala
            /// FECHA CREÓ  : 15/Septiembre/2011
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
            /// NOMBRE DE LA FUNCION: Validar_Datos_Parametros
            /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 15/Septiembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Validar_Datos_Parametros()
            {
                String Espacios_Blanco;
                Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
                Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

                if (string.IsNullOrEmpty(Txt_Mascara_Cuenta_Contable_Parametros.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La mascara para la cuenta contable es un campo requerido. <br>";
                    Datos_Validos = false;
                }
                if (Cmb_Mes_Contable.SelectedIndex == 0)
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + El tipo de mes contable es un campo requerido. <br>";
                    Datos_Validos = false;
                }
                return Datos_Validos;
            }
        #endregion

        #region (Metodos Operacion)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Alta_Parametros
            /// DESCRIPCION : Da de Alta el Parametro con los datos proporcionados por 
            ///               el usuario
            /// PARAMETROS  : 
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 15/Septiembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Alta_Parametros()
            {
                Cls_Cat_Con_Parametros_Negocio Rs_Alta_Cat_Con_Parametros = new Cls_Cat_Con_Parametros_Negocio();  //Variable de conexión hacia la capa de Negocios

                try
                {
                    Rs_Alta_Cat_Con_Parametros.P_Mascara_Cuenta_Contable = Txt_Mascara_Cuenta_Contable_Parametros.Text.Trim();
                    Rs_Alta_Cat_Con_Parametros.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                    Rs_Alta_Cat_Con_Parametros.P_Mes_Contable = Cmb_Mes_Contable.SelectedItem.Text.ToUpper();

                    Rs_Alta_Cat_Con_Parametros.Alta_Parametros(); //Da de alto los datos del Parametro en la BD
                    Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Parametros", "alert('El Alta de Parametros fue Exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Alta_Parametros " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Modificar_Parametros
            /// DESCRIPCION : Modifica los datos del Parametro
            /// PARAMETROS  : 
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 21/Septiembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Modificar_Parametros()
            {
                Cls_Cat_Con_Parametros_Negocio Rs_Modificar_Cat_Con_Parametros = new Cls_Cat_Con_Parametros_Negocio(); //Variable de conexion a la capa de negocios

                try
                {
                    Rs_Modificar_Cat_Con_Parametros.P_Mascara_Cuenta_Contable = Txt_Mascara_Cuenta_Contable_Parametros.Text.Trim();                
                    Rs_Modificar_Cat_Con_Parametros.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;
                    Rs_Modificar_Cat_Con_Parametros.P_Mes_Contable = Cmb_Mes_Contable.SelectedItem.Text;

                    Rs_Modificar_Cat_Con_Parametros.Modificar_Parametros();//Modifica el registro del parametro de la mascara de la cuenta contable con los datos proporcionados por el usuario
                    Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    Habilitar_Controles("Inicial");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Parametros", "alert('La Modificación de los Parametros fue Exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Modificar_Parametros " + ex.Message.ToString(), ex);
                }
            }
        #endregion

        #region (Metodos Consulta)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Parametros
            /// DESCRIPCION : Consulta el Parametro que estan dado de alta en la BD
            /// PARAMETROS  : 
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 21/Septiembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Parametros()
            {
                Cls_Cat_Con_Parametros_Negocio Rs_Consulta_Cat_Con_Parametros = new Cls_Cat_Con_Parametros_Negocio(); //Variable de conexión hacia la capa de Negocios
                DataTable Dt_Parametros; //Variable que obtendra los datos de la consulta 

                try
                {
                    Session.Remove("Consulta_Parametros");
                    Dt_Parametros = Rs_Consulta_Cat_Con_Parametros.Consulta_Datos_Parametros();//Consulta los datos generales de las Cuentas Contables dados de alta en la BD
                    Session["Consulta_Parametros"] = Dt_Parametros;
                    Llena_Grid_Parametros(); //Agrega las Cuentas Contables obtenidas de la consulta anterior
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Parametros" + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Llena_Grid_Parametros
            /// DESCRIPCION : Llena el grid con los Parametros encontrados en la BD
            /// PARAMETROS  : 
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 21/Septiembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Llena_Grid_Parametros()
            {
                DataTable Dt_Parametros; //Variable que obtendra los datos de la consulta 
                try
                {
                    Grid_Parametros.DataBind();
                    Dt_Parametros = (DataTable)Session["Consulta_Parametros"];
                    Grid_Parametros.DataSource = Dt_Parametros;
                    Grid_Parametros.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Llena_Grid_Parametros " + ex.Message.ToString(), ex);
                }
            }
        #endregion

    #endregion

    #region (Eventos)
        protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Btn_Nuevo.ToolTip == "Nuevo")
                {
                    Limpia_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                    Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    //Valida si todos los campos requeridos estan llenos si es así da de alta los datos en la base de datos
                    if (Validar_Datos_Parametros())
                    {
                        Alta_Parametros(); //Da de alta el Tipo de Poliza con los datos que proporciono el usuario
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
                Lbl_Mensaje_Error.Text = "";
                if (Btn_Modificar.ToolTip == "Modificar")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                    //Si el usuario proporciono todos los datos requeridos entonces modificar los datos de la cuenta contable en la BD
                    if (Validar_Datos_Parametros())
                    {
                        Modificar_Parametros(); //Modifica los datos de la Cuenta Contable con los datos proporcionados por el usuario
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

    #region (Grid)
        protected void Grid_Parametros_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Se consultan las Cuentas Contables que actualmente se encuentran registradas en el sistema.
            Consulta_Parametros();

            DataTable Dt_Parametros = (Grid_Parametros.DataSource as DataTable);

            if (Dt_Parametros != null)
            {
                DataView Dv_Parametros = new DataView(Dt_Parametros);
                String Orden = ViewState["SortDirection"].ToString();

                if (Orden.Equals("ASC"))
                {
                    Dv_Parametros.Sort = e.SortExpression + " " + "DESC";
                    ViewState["SortDirection"] = "DESC";
                }
                else
                {
                    Dv_Parametros.Sort = e.SortExpression + " " + "ASC";
                    ViewState["SortDirection"] = "ASC";
                }

                Grid_Parametros.DataSource = Dv_Parametros;
                Grid_Parametros.DataBind();
            }
        }
        protected void Grid_Parametros_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Limpia_Controles();                              //Limpia los controles de la forma
                Grid_Parametros.PageIndex = e.NewPageIndex; //Asigna la nueva página que selecciono el usuario
                Llena_Grid_Parametros();                  //Muestra las Cuentas Contables que estan asignadas en la página seleccionada por el usuario
                Grid_Parametros.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Parametros_SelectedIndexChanged
        /// DESCRIPCION : Consulta los datos del Parametro seleccionado por el usuario
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 21/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Parametros_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cls_Cat_Con_Parametros_Negocio Rs_Consulta_Cat_Con_Parametros = new Cls_Cat_Con_Parametros_Negocio(); //Variable de conexión hacia la capa de Negocios
            DataTable Dt_Parametros; //Variable que obtendra los datos de la consulta 

            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Limpia_Controles(); //Limpia los controles del la forma para poder agregar los valores del registro seleccionado

                Rs_Consulta_Cat_Con_Parametros.P_Parametro_Contabilidad_ID = Grid_Parametros.SelectedRow.Cells[1].Text;
                Dt_Parametros = Rs_Consulta_Cat_Con_Parametros.Consulta_Datos_Parametros(); //Consulta todos los datos de la Cuenta Contable que fue seleccionada por el usuario
                if (Dt_Parametros.Rows.Count > 0)
                {
                    //Asigna los valores de los campos obtenidos de la consulta anterior a los controles de la forma
                    foreach (DataRow Registro in Dt_Parametros.Rows)
                    {
                        if (!String.IsNullOrEmpty(Registro[Cat_Con_Parametros.Campo_Mascara_Cuenta_Contable].ToString()))
                            Txt_Mascara_Cuenta_Contable_Parametros.Text = Registro[Cat_Con_Parametros.Campo_Mascara_Cuenta_Contable].ToString();
                        if (!String.IsNullOrEmpty(Registro[Cat_Con_Parametros.Campo_Mes_Contable].ToString()))
                            Cmb_Mes_Contable.SelectedValue = Registro[Cat_Con_Parametros.Campo_Mes_Contable].ToString();
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
