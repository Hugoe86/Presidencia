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
using Presidencia.Cuentas_Contables.Negocio;
using Presidencia.Tipo_Balance.Negocios;
using Presidencia.Tipo_Resultado.Negocios;
using Presidencia.Parametros_Contabilidad.Negocio;
using Presidencia.Catalogo_Compras_Partidas.Negocio;
using Presidencia.Niveles.Negocio;

public partial class paginas_Contabilidad_Frm_Cat_Con_Cuentas_Contables : System.Web.UI.Page
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
            /// FECHA_CREO  : 20-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Inicializa_Controles()
            { 
                try
                {
                    Consultar_Niveles();            //Consulta todos los niveles estan dados de alta
                    Limpia_Controles();             //Limpia los controles del forma
                    Habilitar_Controles("Inicial"); //Inicializa todos los controles
                    Consulta_Cuentas_Contables();   //Consulta las cuentas contables que estan dadas de alta
                    Llenar_Nombre_Presupuestos();   //Llena el Cmb con los nombres de los presupuestos.
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
            /// FECHA_CREO  : 20-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpia_Controles()
            {
                try
                {
                    Txt_Busqueda_Cuenta_Contable.Text = "";
                    Txt_Cuenta_Contable_ID.Text = "";
                    Txt_Descripcion_Cuenta_Contable.Text = "";
                    Txt_Cuenta_Contable.Text = "";
                    Txt_Comentarios_Cuenta_Contable.Text = "";
                    Txt_Cuenta_Presupuestal.Text = "";
                    Cmb_Cuenta_Presupuestal.SelectedIndex = 0;
                    Cmb_Nivel_Cuenta_Contable.SelectedIndex = 0;
                    Cmb_Cuenta_Detalle.SelectedIndex = 0;
                    Cmb_Tipo_Cuenta_Contable.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Limpia_Controles " + ex.Message.ToString());
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Habilitar_Controles
            /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
            ///                para a siguiente operación
            /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
            ///                           si es una alta, modificacion
            ///                           
            /// CREO        : Yazmin Abigail Delgado Gómez
            /// FECHA_CREO  : 22-Junio-2011
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

                            Configuracion_Acceso("Frm_Cat_Con_Cuentas_Contables.aspx");
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
                    Txt_Cuenta_Contable.Enabled = Habilitado;
                    Txt_Descripcion_Cuenta_Contable.Enabled = Habilitado;
                    Txt_Comentarios_Cuenta_Contable.Enabled = Habilitado;
                    Txt_Busqueda_Cuenta_Contable.Enabled = !Habilitado;
                    Cmb_Cuenta_Detalle.Enabled = Habilitado;
                    Cmb_Nivel_Cuenta_Contable.Enabled = Habilitado;
                    Cmb_Tipo_Cuenta_Contable.Enabled=Habilitado;
                    Btn_Buscar_Descripcion_Cuenta_Contable.Enabled = !Habilitado;
                    Grid_Cuenta_Contable.Enabled = !Habilitado;
                    Txt_Cuenta_Presupuestal.Enabled = Habilitado;
                    Cmb_Cuenta_Presupuestal.Enabled = Habilitado;
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                }
                catch (Exception ex)
                {
                    throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Llenar_Nombre_Presupuestos
            /// DESCRIPCION : Llena el Cmb_Cuenta_Presupuestal con los nombres.
            /// PARAMETROS  : 
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 26/Septiembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Llenar_Nombre_Presupuestos()
            {                
                try
                {
                    Cls_Cat_Com_Partidas_Negocio Rs_Cat_Com_Partidas_Negocio = new Cls_Cat_Com_Partidas_Negocio();
                    DataTable Dt_Partidas;
                    Dt_Partidas = Rs_Cat_Com_Partidas_Negocio.Consulta_Nombre_Partidas();

                    Cmb_Cuenta_Presupuestal.DataSource = Dt_Partidas;
                    Cmb_Cuenta_Presupuestal.DataTextField = Cat_Com_Partidas.Campo_Nombre;
                    Cmb_Cuenta_Presupuestal.DataValueField = Cat_Com_Partidas.Campo_Partida_ID;
                    Cmb_Cuenta_Presupuestal.DataBind();
                    Cmb_Cuenta_Presupuestal.Items.Insert(0, new ListItem("<- Seleccione ->",""));
                    Cmb_Cuenta_Presupuestal.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    throw new Exception("Llenar_Nombre_Presupuestos " + ex.Message.ToString(), ex);
                }
            }
        #endregion

        #region (Control Acceso Pagina)
            /// ******************************************************************************
            /// NOMBRE: Configuracion_Acceso
            /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
            /// PARÁMETROS  :
            /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
            /// FECHA CREÓ  : 23/Mayo/2011 10:43 a.m.
            /// USUARIO MODIFICO  :
            /// FECHA MODIFICO    :
            /// CAUSA MODIFICACIÓN:
            /// ******************************************************************************
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
                    Botones.Add(Btn_Buscar_Descripcion_Cuenta_Contable);

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
            /// NOMBRE DE LA FUNCION: Consultar_Niveles
            /// DESCRIPCION : Consulta los Niveles de las P{olizas que estan dadas de alta 
            /// PARAMETROS  :
            /// CREO        : Yazmin Abigail Delgado Gómez
            /// FECHA_CREO  : 23-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consultar_Niveles() 
            {
                DataTable Dt_Niveles; //Variable que obtendra los datos de la consulta 
                Cls_Cat_Con_Niveles_Negocio Rs_Consulta_Cat_Con_Niveles = new Cls_Cat_Con_Niveles_Negocio(); //Variable de conexión hacia la capa de Negocios

                try
                {
                    Dt_Niveles=Rs_Consulta_Cat_Con_Niveles.Consulta_Niveles();
                    Cmb_Nivel_Cuenta_Contable.DataSource=Dt_Niveles;
                    Cmb_Nivel_Cuenta_Contable.DataValueField=Cat_Con_Niveles.Campo_Nivel_ID;
                    Cmb_Nivel_Cuenta_Contable.DataTextField=Cat_Con_Niveles.Campo_Descripcion;
                    Cmb_Nivel_Cuenta_Contable.DataBind();
                    Cmb_Nivel_Cuenta_Contable.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
                    Cmb_Nivel_Cuenta_Contable.SelectedIndex=0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Consultar_Niveles" + ex.Message.ToString(), ex);
                }               
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Cuentas_Contables
            /// DESCRIPCION : Consulta las Cuentas Contables que estan dadas de alta en la BD
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 22-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Cuentas_Contables()
            { 
                Cls_Cat_Con_Cuentas_Contables_Negocio Rs_Consulta_Cat_Con_Cuentas_Contables = new Cls_Cat_Con_Cuentas_Contables_Negocio(); //Variable de conexión hacia la capa de Negocios
                DataTable Dt_Cuentas_Contables; //Variable que obtendra los datos de la consulta 

                try
                {
                    Session.Remove("Consulta_Cuentas_Contables");
                    if (!string.IsNullOrEmpty(Txt_Busqueda_Cuenta_Contable.Text.Trim()))
                    {
                        Rs_Consulta_Cat_Con_Cuentas_Contables.P_Descripcion = Convert.ToString(Txt_Busqueda_Cuenta_Contable.Text);
                    }
                    Dt_Cuentas_Contables = Rs_Consulta_Cat_Con_Cuentas_Contables.Consulta_Datos_Cuentas_Contables();//Consulta los datos generales de las Cuentas Contables dados de alta en la BD
                    Session["Consulta_Cuentas_Contables"] = Dt_Cuentas_Contables;
                    Llena_Grid_Cuentas_Contables(); //Agrega las Cuentas Contables obtenidas de la consulta anterior
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Cuentas_Contables" + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Llena_Grid_Cuentas_Contables
            /// DESCRIPCION : Llena el grid con las Cuentas Contables que se encuentran en la 
            ///               base de datos
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 22-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Llena_Grid_Cuentas_Contables()
            {
                DataTable Dt_Cuentas_Contables; //Variable que obtendra los datos de la consulta 
                try
                {
                    Grid_Cuenta_Contable.DataBind();
                    Dt_Cuentas_Contables = (DataTable)Session["Consulta_Cuentas_Contables"];
                    Grid_Cuenta_Contable.DataSource = Dt_Cuentas_Contables;
                    Grid_Cuenta_Contable.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Llena_Grid_Cuentas_Contables " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Parametros
            /// DESCRIPCION : Consulta la mascara para la cuenta contable actual.
            /// PARAMETROS  : 
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 19/Septiembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private string Consulta_Parametros()
            {
                Cls_Cat_Con_Parametros_Negocio Rs_Consulta_Cat_Con_Parametros_Negocio = new Cls_Cat_Con_Parametros_Negocio(); //Variable de conexión hacia la capa de Negocios
                DataTable Dt_Parametros; //Variable que obtendra los datos de la consulta 
                string Mascara_Cuenta_Contable; //Recibe la mascara contable actual.
                try
                {
                    Session.Remove("Consulta_Parametros");
                    Dt_Parametros = Rs_Consulta_Cat_Con_Parametros_Negocio.Consulta_Parametros();//Consulta los datos generales de las Cuentas Contables dados de alta en la BD
                    Session["Consulta_Parametros"] = Dt_Parametros;
                    Mascara_Cuenta_Contable = Dt_Parametros.Rows[0][0].ToString();
                    return Mascara_Cuenta_Contable;
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Parametros" + ex.Message.ToString(), ex);
                }
            }
        #endregion

        #region (Metodos Validacion)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Validar_Datos_Nivel
            /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
            /// CREO        : Yazmin Abigail Delgado Gómez
            /// FECHA_CREO  : 22-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Validar_Datos_Cuentas_Contables()
            {
                String Espacios_Blanco;
                Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
                Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

                if (string.IsNullOrEmpty(Txt_Descripcion_Cuenta_Contable.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Descripción de la Cuenta Contable es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Cuenta_Contable.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + El No. de Cuenta Contable es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (Cmb_Nivel_Cuenta_Contable.SelectedIndex==0)
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + El Nivel de la Cuenta Contable es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                return Datos_Validos;
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Validar_Mascara_Cuenta_Contable
            /// DESCRIPCION : Validar que el formato de la mascara sea el correcto.
            /// PARAMETROS  : TEXTO: Recibe el valor contenido en la propiedad Text de la Txt_Cuenta_Contable
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 19/Septiembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Validar_Mascara_Cuenta_Contable(string Texto)
            {
                try
                {
                    string Mascara_Cuenta_Contable = Consulta_Parametros();
                    if (Texto.Length == Mascara_Cuenta_Contable.Length)
                    {
                        for (int Cont_Caracteres = 0; Cont_Caracteres < Mascara_Cuenta_Contable.Length; Cont_Caracteres++)
                        {
                            if (Texto.Substring(Cont_Caracteres, 1) == "-")
                            {
                                if (Mascara_Cuenta_Contable.Substring(Cont_Caracteres, 1) != Texto.Substring(Cont_Caracteres, 1))
                                    throw new Exception("El formato de entrada de la cuenta contable no coincide con lo establecido en la mascara.");
                            }
                            else
                            {
                                if (Mascara_Cuenta_Contable.Substring(Cont_Caracteres, 1) != "#")
                                    throw new Exception("El formato de entrada de la cuenta contable no coincide con lo establecido en la mascara.");
                            }
                        }
                        return (Boolean)true;
                    }
                    else
                        throw new Exception("Validar_Mascara_Cuenta_Contable: La Cuenta Contable tiene mas caracteres de los requeridos.");

                }
                catch (Exception ex)
                {
                    throw new Exception("Validar_Mascara_Cuenta_Contable " + ex.Message.ToString(), ex);
                }
            }
        #endregion

        #region (Métodos Operación)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Alta_Cuenta_Contable
            /// DESCRIPCION : Da de Alta la Cuenta Contable con los datos proporcionados por 
            ///               el usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 23-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Alta_Cuenta_Contable()
            {
                DataTable Dt_Cuenta_Contable; //Obtiene los datos de la consulta
                Cls_Cat_Con_Cuentas_Contables_Negocio Rs_Alta_Cat_Con_Cuentas_Contables = new Cls_Cat_Con_Cuentas_Contables_Negocio();  //Variable de conexión hacia la capa de Negocios
                try
                {
                    Txt_Cuenta_Contable.Text = Txt_Cuenta_Contable.Text.Replace("-", ""); 
                    Rs_Alta_Cat_Con_Cuentas_Contables.P_Cuenta = Convert.ToString(Txt_Cuenta_Contable.Text);
                    Dt_Cuenta_Contable = Rs_Alta_Cat_Con_Cuentas_Contables.Consulta_Existencia_Cuenta_Contable(); //Verifica si la cuanta que se pretende dar de alta no exista

                    if (Dt_Cuenta_Contable.Rows.Count== 0)
                    {
                        Rs_Alta_Cat_Con_Cuentas_Contables.P_Descripcion = Convert.ToString(Txt_Descripcion_Cuenta_Contable.Text.Trim());
                        Rs_Alta_Cat_Con_Cuentas_Contables.P_Nivel_ID = Cmb_Nivel_Cuenta_Contable.SelectedValue;
                        Rs_Alta_Cat_Con_Cuentas_Contables.P_Afectable = Cmb_Cuenta_Detalle.SelectedValue;

                        //****************************
                        //CODIGO
                        switch (Cmb_Tipo_Cuenta_Contable.SelectedIndex)
                        {
                            case 0:
                                Rs_Alta_Cat_Con_Cuentas_Contables.P_Tipo_Cuenta = null;
                                break;
                            case 1:
                                Rs_Alta_Cat_Con_Cuentas_Contables.P_Tipo_Cuenta = Cmb_Tipo_Cuenta_Contable.SelectedItem.ToString();
                                break;
                            case 2:
                                Rs_Alta_Cat_Con_Cuentas_Contables.P_Tipo_Cuenta = Cmb_Tipo_Cuenta_Contable.SelectedItem.ToString();
                                break;
                        }


                        //****************************
                        //Rs_Alta_Cat_Con_Cuentas_Contables.P_PArtida_ID = 
                        Rs_Alta_Cat_Con_Cuentas_Contables.P_Comentarios = Convert.ToString(Txt_Comentarios_Cuenta_Contable.Text.Trim());
                        Rs_Alta_Cat_Con_Cuentas_Contables.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

                        Rs_Alta_Cat_Con_Cuentas_Contables.Alta_Cuenta_Contable(); //Da de alto los datos de la Cuenta Contable en la BD
                        Inicializa_Controles();                                   //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                        Habilitar_Controles("Inicial");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Cuentas Contables", "alert('El Alta de la Cuenta Contable fue Exitosa');", true);
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "La cuenta contable ya esta asignada, favor de verificar";
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Alta_Cuenta_Contable " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Modificar_Cuenta_Contable
            /// DESCRIPCION : Modifica los datos de la Cuenta Contable por los datos proporcionados
            ///               por el usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 23-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Modificar_Cuenta_Contable()
            {
                DataTable Dt_Cuenta_Contable; //Obtiene los datos de la consulta
                Cls_Cat_Con_Cuentas_Contables_Negocio Rs_Modificar_Cat_Con_Cuentas_Contables = new Cls_Cat_Con_Cuentas_Contables_Negocio();

                try
                {
                    Rs_Modificar_Cat_Con_Cuentas_Contables.P_Cuenta_Contable_ID = Convert.ToString(Txt_Cuenta_Contable_ID.Text.Trim());
                    Rs_Modificar_Cat_Con_Cuentas_Contables.P_Cuenta = Convert.ToString(Txt_Cuenta_Contable.Text);

                    Rs_Modificar_Cat_Con_Cuentas_Contables.P_Cuenta = Convert.ToString(Txt_Cuenta_Contable.Text);
                    Dt_Cuenta_Contable = Rs_Modificar_Cat_Con_Cuentas_Contables.Consulta_Existencia_Cuenta_Contable(); //Verifica si la cuanta que se pretende dar de alta no exista

                    if (Dt_Cuenta_Contable.Rows.Count == 0)
                    {
                        Rs_Modificar_Cat_Con_Cuentas_Contables.P_Descripcion = Convert.ToString(Txt_Descripcion_Cuenta_Contable.Text.Trim());
                        Rs_Modificar_Cat_Con_Cuentas_Contables.P_Nivel_ID = Cmb_Nivel_Cuenta_Contable.SelectedValue;
                        Rs_Modificar_Cat_Con_Cuentas_Contables.P_Afectable = Cmb_Cuenta_Detalle.SelectedValue;
                        
                        //*************************
                        //CODIGO
                        switch (Cmb_Tipo_Cuenta_Contable.SelectedIndex)
                        {
                            case 0:
                                Rs_Modificar_Cat_Con_Cuentas_Contables.P_Tipo_Cuenta = null;
                                break;
                            case 1:
                                Rs_Modificar_Cat_Con_Cuentas_Contables.P_Tipo_Cuenta = Cmb_Tipo_Cuenta_Contable.SelectedItem.ToString();
                                break;
                            case 2:
                                Rs_Modificar_Cat_Con_Cuentas_Contables.P_Tipo_Cuenta = Cmb_Tipo_Cuenta_Contable.SelectedItem.ToString();
                                break;
                        }

                        //*************************

                        Rs_Modificar_Cat_Con_Cuentas_Contables.P_Comentarios = Convert.ToString(Txt_Comentarios_Cuenta_Contable.Text.Trim());
                        Rs_Modificar_Cat_Con_Cuentas_Contables.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

                        Rs_Modificar_Cat_Con_Cuentas_Contables.Modificar_Cuenta_Contable();//Modifica el registro de la cuenta contable con los datos proporcionados por el usuario
                        Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                        Habilitar_Controles("Inicial");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Cuentas Contables", "alert('La Modificación de la Cuenta Contable fue Exitosa');", true);
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "La cuenta contable ya esta asignada, favor de verificar";
                    }
                }
                catch (Exception ex)
                { 
                    throw new Exception("Modificar_Cuenta_Contable " + ex.Message.ToString(),ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Elimina_Cuenta_Contable
            /// DESCRIPCION : Elimina los datos de la Cuenta Contable que fue seleccionada 
            ///               por el Usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 23-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Elimina_Cuenta_Contable()
            {
                Cls_Cat_Con_Cuentas_Contables_Negocio Rs_Eliminar_Cat_Con_Cuentas_Contables = new Cls_Cat_Con_Cuentas_Contables_Negocio(); //Variable de conexión hacia la capa de Negocios
                try
                {
                    Rs_Eliminar_Cat_Con_Cuentas_Contables.P_Cuenta_Contable_ID = Convert.ToString(Txt_Cuenta_Contable_ID.Text.Trim());
                    Rs_Eliminar_Cat_Con_Cuentas_Contables.Eliminar_Cuenta_Contable(); //Elimina la Cuenta Contable que fue seleccionada por el usuario

                    Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Cuentas Contables", "alert('La Eliminación de la Cuenta Contable fue Exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Elimina_Cuenta_Contable" + ex.Message.ToString(), ex);
                }
            }
        #endregion
    #endregion

    #region (Eventos)
        protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                if (Btn_Nuevo.ToolTip == "Nuevo")
                {
                    Limpia_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                    Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                }
                else
                {
                    //Valida si todos los campos requeridos estan llenos si es así da de alta los datos en la base de datos
                    if (Validar_Datos_Cuentas_Contables())
                    {
                        if (Validar_Mascara_Cuenta_Contable(Txt_Cuenta_Contable.Text))
                        {
                            Alta_Cuenta_Contable(); //Da de alta la Cuenta Contable con los datos que proporciono el usuario
                        }
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
                    if (Validar_Datos_Cuentas_Contables())
                    {
                        Modificar_Cuenta_Contable(); //Modifica los datos de la Cuenta Contable con los datos proporcionados por el usuario
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
        protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                if (!string.IsNullOrEmpty(Txt_Cuenta_Contable_ID.Text.Trim()))
                {
                    Elimina_Cuenta_Contable(); //Elimina la Cuenta Contable que fue seleccionada por el usuario
                }
                //Si el usuario no selecciono alguna Cuenta Contable manda un mensaje indicando que es necesario que 
                //seleccione alguna para poder eliminar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione la Cuenta Contable que desea eliminar <br>";
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        protected void Btn_Buscar_Descripcion_Cuenta_Contable_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                Consulta_Cuentas_Contables(); //Consulta los Niveles de Poliza que coincidan con el nombre porporcionado por el usuario
                //Si no se encontraron Cuentas Contables con una descripción similar al proporcionado por el usuario entonces manda un mensaje al usuario
                if (Grid_Cuenta_Contable.Rows.Count <= 0)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron Cuentas Contables con la descripción proporcionada <br>";
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Salir")
            {
                Session.Remove("Consulta_Cuentas_Contables");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Inicializa_Controles(); //Habilita los controles para la siguiente operación del usuario en el catálogo
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible=true;
            Img_Error.Visible=true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Txt_Cuenta_Contable_TextChanged
        /// DESCRIPCION : Consulta la Descripción más cercana de la cuenta que esta 
        ///               proporcionando el usuario
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 11/Julio/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Txt_Cuenta_Contable_TextChanged(object sender, EventArgs e)
        {
            Cls_Cat_Con_Cuentas_Contables_Negocio Rs_Consulta_Cat_Con_Cuentas_Contables = new Cls_Cat_Con_Cuentas_Contables_Negocio();
            bool Existe_Guion = false;

            try
            {
                for (int Cont_Caracteres = 0; Cont_Caracteres < Txt_Cuenta_Contable.Text.Length; Cont_Caracteres++)
                {
                    if (Txt_Cuenta_Contable.Text.Substring(Cont_Caracteres, 1) == "-")
                    {
                        Validar_Mascara_Cuenta_Contable(Txt_Cuenta_Contable.Text);
                        Txt_Cuenta_Contable.Text = Aplicar_Mascara_Cuenta_Contable(Txt_Cuenta_Contable.Text);
                        Existe_Guion = true;
                    }
                }
                if(Existe_Guion == false)
                    Txt_Cuenta_Contable.Text = Aplicar_Mascara_Cuenta_Contable(Txt_Cuenta_Contable.Text);
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Aplicar_Mascara_Cuenta_Contable
        /// DESCRIPCION : Aplica la Mascara a la Cuenta Contable
        /// PARAMETROS  : Cuenta_Contable: Recibe el numero de cuenta contable
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 20/Septiembre/2011
        /// MODIFICO          : sergio manuel gallardo andrade
        /// FECHA_MODIFICO    :3-noviembre-2011
        /// CAUSA_MODIFICACION:no funciona correctamente al aplicarle la mascara 
        ///*******************************************************************************
        private string Aplicar_Mascara_Cuenta_Contable(string Cuenta_Contable)
        {
            try
            {
                string Mascara_Cuenta_Contable = Consulta_Parametros(); //Consulta y almacena la mascara contable actual.
                string Cuenta_Contable_Con_Formato = "";    //Almacenara la cuenta contable ya estandarizada de acuerdo al formato.
                Boolean Primer_Numero = true;   //Detecta si el primer caracter es un numero.
                int Caracteres_Extraidos_Cuenta_Contable = 0;  //Variable que almacena la cantidad de caracteres extraidos de la cuenta.
                int contador_nuevo = 0;
                int Inicio_Extraccion = 0; //Variable que almacena el inicio de la cadena a extraer.
                int Fin_Extraccion = 0;    //Variable que almacena el fin de la cadena a extraer.
                for (int Cont_Desplazamiento = 0; Cont_Desplazamiento < Mascara_Cuenta_Contable.Length; Cont_Desplazamiento++)  //Ciclo de desplazamiento
                {
                    if (Primer_Numero == true && Mascara_Cuenta_Contable.Substring(Cont_Desplazamiento, 1) == "#")  //Detecta el primer numero dentro de la mascara contable
                    {
                        if (Cont_Desplazamiento == 0)
                            Inicio_Extraccion = Cont_Desplazamiento;
                        else
                            Inicio_Extraccion = contador_nuevo;
                        Primer_Numero = false;
                    }
                    if (Mascara_Cuenta_Contable.Substring(Cont_Desplazamiento, 1) != "#") //Detecta si el caracter es diferente de un numero en la mascara contable.
                    {
                        Fin_Extraccion = Cont_Desplazamiento;
                        if (Inicio_Extraccion == 0)
                        {
                            Cuenta_Contable_Con_Formato += Cuenta_Contable.Substring(Inicio_Extraccion, Fin_Extraccion - Inicio_Extraccion);
                            Caracteres_Extraidos_Cuenta_Contable = Fin_Extraccion - Inicio_Extraccion;
                        }
                        else
                        {
                            Cuenta_Contable_Con_Formato += Cuenta_Contable.Substring(Inicio_Extraccion, Fin_Extraccion - Inicio_Extraccion - contador_nuevo);
                            Caracteres_Extraidos_Cuenta_Contable += Fin_Extraccion - Inicio_Extraccion - contador_nuevo;
                        }
                        if (contador_nuevo < Cuenta_Contable.Length)
                        {
                            contador_nuevo = contador_nuevo + 1;
                        }
                        Primer_Numero = true;
                        Cuenta_Contable_Con_Formato += "-";

                    }
                }
                if (Caracteres_Extraidos_Cuenta_Contable != Cuenta_Contable.Length) //Concatena los caracteres sobrantes en la cuenta contable.
                {
                    Cuenta_Contable_Con_Formato += Cuenta_Contable.Substring(Caracteres_Extraidos_Cuenta_Contable, Cuenta_Contable.Length - Caracteres_Extraidos_Cuenta_Contable);
                }
                return Cuenta_Contable_Con_Formato;
            }
            catch (Exception ex)
            {
                throw new Exception("Aplicar_Mascara_Cuenta_Contable " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Txt_Cuenta_Presupuestal_TextChanged
        /// DESCRIPCION : Consulta el nombre de la Cuenta que fue ingresada
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 26/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Txt_Cuenta_Presupuestal_TextChanged(object sender, EventArgs e)
        {
            Cls_Cat_Com_Partidas_Negocio Rs_Cat_Com_Partidas = new Cls_Cat_Com_Partidas_Negocio();
            DataTable Dt_Cat_Com_Partidas;

            try
            {
                Rs_Cat_Com_Partidas.P_Cuenta_SAP = Txt_Cuenta_Presupuestal.Text;
                Dt_Cat_Com_Partidas = Rs_Cat_Com_Partidas.Consulta_Nombre_Cuenta_Partidas();
                Cmb_Cuenta_Presupuestal.SelectedValue = Dt_Cat_Com_Partidas.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Cmb_Cuenta_Presupuestal_SelectedIndexChanged
        /// DESCRIPCION : Consulta la Cuenta que fue ingresada de acuerdo al nombre
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 27/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Cmb_Cuenta_Presupuestal_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cls_Cat_Com_Partidas_Negocio Rs_Cat_Com_Partidas = new Cls_Cat_Com_Partidas_Negocio();
            DataTable Dt_Cat_Com_Partidas;

            try
            {
                Rs_Cat_Com_Partidas.P_Nombre_Partida = Cmb_Cuenta_Presupuestal.SelectedItem.ToString();
                Dt_Cat_Com_Partidas = Rs_Cat_Com_Partidas.Consulta_Nombre_Cuenta_Partidas();
                if (!string.IsNullOrEmpty(Dt_Cat_Com_Partidas.Rows[0][0].ToString()))
                    Txt_Cuenta_Presupuestal.Text = Dt_Cat_Com_Partidas.Rows[0][0].ToString();
                else
                {
                    throw new Exception("La Partida no tiene cuenta asignada", new Exception());
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
        protected void Grid_Cuenta_Contable_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Se consultan las Cuentas Contables que actualmente se encuentran registradas en el sistema.
            Consulta_Cuentas_Contables();

            DataTable Dt_Cuenta_Contable = (Grid_Cuenta_Contable.DataSource as DataTable);

            if (Dt_Cuenta_Contable != null)
            {
                DataView Dv_Cuenta_Contable = new DataView(Dt_Cuenta_Contable);
                String Orden = ViewState["SortDirection"].ToString();

                if (Orden.Equals("ASC"))
                {
                    Dv_Cuenta_Contable.Sort = e.SortExpression + " " + "DESC";
                    ViewState["SortDirection"] = "DESC";
                }
                else
                {
                    Dv_Cuenta_Contable.Sort = e.SortExpression + " " + "ASC";
                    ViewState["SortDirection"] = "ASC";
                }

                Grid_Cuenta_Contable.DataSource = Dv_Cuenta_Contable;
                Grid_Cuenta_Contable.DataBind();
            }
        }
        protected void Grid_Cuenta_Contable_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Limpia_Controles();                              //Limpia los controles de la forma
                Grid_Cuenta_Contable.PageIndex = e.NewPageIndex; //Asigna la nueva página que selecciono el usuario
                Llena_Grid_Cuentas_Contables();                  //Muestra las Cuentas Contables que estan asignadas en la página seleccionada por el usuario
                Grid_Cuenta_Contable.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Cuenta_Contable_SelectedIndexChanged
        /// DESCRIPCION : Consulta los datos de la Cuenta Contable seleccionada por el usuario
        /// CREO        : Yazmin Abigail Delgado Gómez
        /// FECHA_CREO  : 23-Junio-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Cuenta_Contable_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cls_Cat_Con_Cuentas_Contables_Negocio Rs_Consulta_Cat_Con_Cuentas_Contables = new Cls_Cat_Con_Cuentas_Contables_Negocio(); //Variable de conexión hacia la capa de Negocios
            DataTable Dt_Cuentas_Contables; //Variable que obtendra los datos de la consulta 

            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Limpia_Controles(); //Limpia los controles del la forma para poder agregar los valores del registro seleccionado

                Rs_Consulta_Cat_Con_Cuentas_Contables.P_Cuenta_Contable_ID = Grid_Cuenta_Contable.SelectedRow.Cells[1].Text;
                Dt_Cuentas_Contables = Rs_Consulta_Cat_Con_Cuentas_Contables.Consulta_Datos_Cuentas_Contables(); //Consulta todos los datos de la Cuenta Contable que fue seleccionada por el usuario
                if (Dt_Cuentas_Contables.Rows.Count > 0)
                {
                    //Asigna los valores de los campos obtenidos de la consulta anterior a los controles de la forma
                    foreach (DataRow Registro in Dt_Cuentas_Contables.Rows)
                    {
                        if (!String.IsNullOrEmpty(Registro[Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID].ToString()))
                            Txt_Cuenta_Contable_ID.Text = Registro[Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID].ToString();

                        if (!String.IsNullOrEmpty(Registro[Cat_Con_Cuentas_Contables.Campo_Descripcion].ToString()))
                            Txt_Descripcion_Cuenta_Contable.Text = Registro[Cat_Con_Cuentas_Contables.Campo_Descripcion].ToString();

                        if (!String.IsNullOrEmpty(Registro[Cat_Con_Cuentas_Contables.Campo_Cuenta].ToString()))
                            Txt_Cuenta_Contable.Text = Registro[Cat_Con_Cuentas_Contables.Campo_Cuenta].ToString();

                        if (!String.IsNullOrEmpty(Registro[Cat_Con_Cuentas_Contables.Campo_Nivel_ID].ToString()))
                            Cmb_Nivel_Cuenta_Contable.SelectedValue = Registro[Cat_Con_Cuentas_Contables.Campo_Nivel_ID].ToString();
                        Cmb_Tipo_Cuenta_Contable.SelectedIndex = 0;
                        if (!String.IsNullOrEmpty(Registro[Cat_Con_Cuentas_Contables.Campo_Tipo_Balance_ID].ToString()))
                        {
                            Cmb_Tipo_Cuenta_Contable.SelectedIndex = 1;
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(Registro[Cat_Con_Cuentas_Contables.Campo_Tipo_Resultado_ID].ToString()))
                            {
                                Cmb_Tipo_Cuenta_Contable.SelectedIndex = 2;
                            }
                        }
                        if (!String.IsNullOrEmpty(Registro[Cat_Con_Cuentas_Contables.Campo_Afectable].ToString()))
                            Cmb_Cuenta_Detalle.SelectedValue = Registro[Cat_Con_Cuentas_Contables.Campo_Afectable].ToString();

                        if (!String.IsNullOrEmpty(Registro[Cat_Con_Cuentas_Contables.Campo_Comentarios].ToString()))
                            Txt_Comentarios_Cuenta_Contable.Text = Registro[Cat_Con_Cuentas_Contables.Campo_Comentarios].ToString();
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
