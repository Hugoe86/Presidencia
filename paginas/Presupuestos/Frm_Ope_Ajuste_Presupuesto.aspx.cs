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
using System.Data.OracleClient;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Movimiento_Presupuestal.Negocio;
using Presidencia.Catalogo_SAP_Fuente_Financiamiento.Negocio;
using Presidencia.Area_Funcional.Negocio;
using Presidencia.Sap_Partida_Generica.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Catalogo_Compras_Partidas.Negocio;
using Presidencia.SAP_Operacion_Departamento_Presupuesto.Negocio;
using Presidencia.Ajuste_Presupuesto.Negocio;
using Presidencia.Ajuste_Presupuesto.Datos;
using Presidencia.Catalogo_Compras_Presupuesto_Dependencias.Negocio;

public partial class paginas_Presupuestos_Frm_Ope_Ajuste_Presupuesto : System.Web.UI.Page
{
    #region(Variables Globales)
    int Global_Indice = 0;
    #endregion

    #region (Load)
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
                Session["Session_Ajsute_Presupuesto"] = null;
                
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
        #region(Metodos Generales)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Inicializa_Controles
            /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda
            ///               realizar diferentes operaciones
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramírez Aguilera
            /// FECHA_CREO  : 07/Noviembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Inicializa_Controles()
            {
                try
                {
                    Limpiar_Controles(); //Limpia los controles del forma
                    Hacer_Visible(false);//muestra el grid de movimiento solamente
                    Habilitar_Controles("Inicial");//Inicializa todos los controles
                    Cargar_Combo_Responsable();
                    Cargar_Combo_Financiamiento();
                    //Cargar_Combo_Area_Funcional();
                    Cargar_Combo_Programa();
                    Cargar_Combo_Partida();
                    Consulta_Movimientos_Realizados();
                   
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
            /// CREO        : Hugo Enrique Ramírez Aguilera
            /// FECHA_CREO  : 07/Noviembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpiar_Controles()
            {
                try
                {
                    //datos generales
                    
                    Cmb_Unidad_Responsable.SelectedIndex = 0;
                    Cmb_Fuente_Financiamiento.SelectedIndex = 0;
                    Cmb_Programa.SelectedIndex = 0;
                    //Cmb_Area_Funcional.SelectedIndex = 0;
                    Cmb_Partidas_Datos_Generales.SelectedIndex = 0;
                    Txt_Descripcion_Datos_General.Text = "";
                    //Txt_Codigo.Text = "";
                   

                    //movimiento
                    Txt_Partida_Movimiento.Text = "";
                    Txt_Disponible_Movimiento.Text = "";
                    Txt_Ampliar_Movimiento.Text = "";
                    Txt_Reducir_Movimiento.Text = "";
                    Txt_Incrementar_Movimiento.Text = "";

                    //partidas en movimiento(Sumas)
                    Txt_Suma_Ampliacion.Text = "0";
                    Txt_Suma_Reduccion.Text = "0";
                }
                catch (Exception ex)
                {
                    throw new Exception("Limpia_Controles " + ex.Message.ToString());
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Habilitar_Controles
            /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
            ///               para a siguiente operación
            /// PARAMETROS  : 1.- Operacion: Indica la operación que se desea realizar por parte del usuario
            ///               si es una alta, modificacion
            ///                           
            /// CREO        : Hugo Enrique Ramírez Aguilera
            /// FECHA_CREO  : 07/Noviembre/2011
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

                            Configuracion_Acceso("Frm_Ope_Ajuste_Presupuesto.aspx");
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
                    //para datos generales
                    Cmb_Unidad_Responsable.Enabled = Habilitado;
                    Cmb_Fuente_Financiamiento.Enabled = Habilitado;
                    Cmb_Programa.Enabled = Habilitado;
                    //Cmb_Area_Funcional.Enabled = Habilitado;
                    Cmb_Partidas_Datos_Generales.Enabled = Habilitado;
                    Txt_Descripcion_Datos_General.Enabled = Habilitado;
                    //para movimiento
                    Txt_Partida_Movimiento.Enabled = Habilitado;
                    Txt_Disponible_Movimiento.Enabled = Habilitado;
                    Txt_Ampliar_Movimiento.Enabled = Habilitado;
                    Txt_Reducir_Movimiento.Enabled = Habilitado;
                    Txt_Incrementar_Movimiento.Enabled = Habilitado;
                    //para partidas en movimietno(Suma)
                    Txt_Suma_Ampliacion.Enabled = Habilitado;
                    Txt_Suma_Reduccion.Enabled = Habilitado;
                    //mensajes de error
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;

                    switch (Operacion)
                    {
                        case "Nuevo":
                            //Cmb_Area_Funcional.Enabled = false;
                            Cmb_Programa.Enabled = false;
                            Cmb_Unidad_Responsable.Enabled = false;
                            Cmb_Partidas_Datos_Generales.Enabled = false;
                            Txt_Reducir_Movimiento.Enabled = false;
                            //Lbl_Codigo.Visible = false;
                            //Txt_Codigo.Visible = false;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Hacer_Visible
            /// DESCRIPCION : hace visibles a las cajas de texto y etiquetas 
            /// PARAMETROS  : 1.- Boolean Habilitado  Pasa el valor de verdadero o falso 
            /// CREO        : Hugo Enrique Ramírez Aguilera
            /// FECHA_CREO  : 07/Noviembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Hacer_Visible(Boolean Habilitado)
            {
                try
                {
                    //para la seccion del gird movimiento
                    Contenedor_Grid_Movimiento.Visible = !Habilitado;
                    Tab_Grid_Movimiento.Visible = !Habilitado;
                    Grid_Movimiento.Visible = !Habilitado;

                    //para la seccion datos generales
                    Contenedor_Datos_Generales.Visible = Habilitado;
                    Tab_Datos_Generales.Visible = Habilitado;
                    Lbl_Unidad_Responsable_Datos_General.Visible = Habilitado;
                    Cmb_Unidad_Responsable.Visible = Habilitado;
                    Lbl_Fuente_Financiamiento_Datos_General.Visible = Habilitado;
                    Cmb_Fuente_Financiamiento.Visible = Habilitado;
                    //Lbl_Area_Funcional.Visible = Habilitado;
                    //Cmb_Area_Funcional.Visible = Habilitado;
                    Lbl_Programa_Datos_General.Visible = Habilitado;
                    Cmb_Programa.Visible = Habilitado;
                    Lbl_Partida_Datos_General.Visible = Habilitado;
                    Cmb_Partidas_Datos_Generales.Visible = Habilitado;
                    Lbl_Estatus.Visible = Habilitado;
                    TXt_Estatus_Movimiento.Visible = Habilitado;
                    Lbl_Descripcion_Datos_General.Visible = Habilitado;
                    Txt_Descripcion_Datos_General.Visible = Habilitado;
                    //Lbl_Codigo.Visible = Habilitado;
                    //Txt_Codigo.Visible = Habilitado;

                    //para la seccion Movimiento
                    Contenedor_Movimiento.Visible = Habilitado;
                    Tab_Movimiento.Visible = Habilitado;
                    Lbl_Partida_Movimiento.Visible = Habilitado;
                    Txt_Partida_Movimiento.Visible = Habilitado;
                    Lbl_Disponible_Movimiento.Visible = Habilitado;
                    Txt_Disponible_Movimiento.Visible = Habilitado;
                    Lbl_Ampliar_Movimiento.Visible = Habilitado;
                    Txt_Ampliar_Movimiento.Visible = Habilitado;
                    Lbl_Reducir_Movimiento.Visible = Habilitado;
                    Txt_Reducir_Movimiento.Visible = Habilitado;
                    Lbl_Incrementar_Movimiento.Visible = Habilitado;
                    Txt_Incrementar_Movimiento.Visible = Habilitado;
                    Btn_Agregar.Visible = Habilitado;
                    Btn_Limpiar.Visible = Habilitado;

                    //para la seccion del grid partidas en movimiento
                    Tab_Partidas_Movimiento.Visible = Habilitado;
                    Grid_Partidas_Movimiento.Visible = Habilitado;
                    Lbl_Sumas.Visible = Habilitado;
                    Txt_Suma_Ampliacion.Visible = Habilitado;
                    Txt_Suma_Reduccion.Visible = Habilitado;
                }
                catch (Exception ex)
                {
                    throw new Exception("Hacer_Visible " + ex.Message.ToString());
                }
            }
        #endregion

        #region(Control Acceso Pagina)
            /// ******************************************************************************
            /// NOMBRE: Configuracion_Acceso
            /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
            /// PARÁMETROS  :
            /// USUARIO CREÓ: Hugo Enrique Ramírez Aguilera
            /// FECHA CREÓ  : 07/Noviembre/2011 
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
            /// NOMBRE DE LA FUNCION: Es_Numero
            /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
            /// PARÁMETROS  : Cadena.- El dato a evaluar si es numerico.
            /// CREO        : Hugo Enrique Ramírez Aguilera
            /// FECHA_CREO  : 07/Noviembre/2011 
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

        #region(Validacion)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Validar_Datos
            /// DESCRIPCION : Validar que se hallan proporcionado todos los datos
            /// CREO        : Hugo Enrique Ramírez Aguilera
            /// FECHA_CREO  : 07/Noviembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Validar_Datos()
            {
                String Espacios_Blanco;
                Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
                Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                int Suma_Ampliacion;
                int Suma_Reduccion;

                //para la seccion de datos generales
               /* if (Cmb_Unidad_Responsable.SelectedIndex <= 0)
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione alguna unidad Responsable en la sección de Datos Generales. <br>";
                    Datos_Validos = false;
                }
                if (Cmb_Fuente_Financiamiento.SelectedIndex <= 0)
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione alguna Fuente de Financiamiento en la sección de Datos Generales.<br>";
                    Datos_Validos = false;
                }
                if (Cmb_Programa.SelectedIndex <= 0)
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione algun Programa en la sección de Datos Generales.<br>";
                    Datos_Validos = false;
                }
                if (Cmb_Partidas_Datos_Generales.SelectedIndex <= 0)
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Favor de introducir la Partida en la sección de Datos Generales.<br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Descripcion_Datos_General.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Favor de introducir la Descripción en la sección de Datos Generales.<br>";
                    Datos_Validos = false;
                }
                */
                //para la seccion de partidas en movimiento
                Suma_Ampliacion = Convert.ToInt32(Txt_Suma_Ampliacion.Text.Trim());
                Suma_Reduccion = Convert.ToInt32(Txt_Suma_Reduccion.Text.Trim());
                if ((Suma_Ampliacion == 0) && (Suma_Reduccion == 0))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Favor de ingresar algun  movimiento.<br>";
                    Datos_Validos = false;
                }

                if (Suma_Reduccion != Suma_Ampliacion)
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Favor de Revisar la sección de Partidas en movimiento, ya que las sumas no son iguales.<br>";
                    Datos_Validos = false;
                }
                return Datos_Validos;
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Validar_Movimiento
            /// DESCRIPCION : Validar que se hallan proporcionado todos los datos
            /// CREO        : Hugo Enrique Ramírez Aguilera
            /// FECHA_CREO  : 07/Noviembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public Boolean Validar_Movimiento()
            {
                String Espacios_Blanco;
                Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
                Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

                if (string.IsNullOrEmpty(Txt_Partida_Movimiento.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Favor de seleccinar alguna Partida en la sección de Datos Generales.<br>";
                    Datos_Validos = false;
                }
                /*if (string.IsNullOrEmpty(Txt_Disponible_Movimiento.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Favor de seleccinar alguna Partida en la sección de Datos Generales.<br>";
                    Datos_Validos = false;
                }*/
                if ((string.IsNullOrEmpty(Txt_Ampliar_Movimiento.Text.Trim())) && (string.IsNullOrEmpty(Txt_Reducir_Movimiento.Text.Trim())) && (string.IsNullOrEmpty( Txt_Incrementar_Movimiento.Text.Trim())))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Favor de ingresar el monto que desea Ajustar en la sección de Movimiento.<br>";
                    Datos_Validos = false;
                }
                
                return Datos_Validos;

            }
        #endregion

        #region (Método Operacion)
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Sumar_PartidaS_Movimientos
            ///DESCRIPCIÓN: realizara la suma de las ampliaciones y reducciones
            ///             las cajas de Ampliar y reducir
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  07/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************  
            protected void Sumar_Partidas_Movimientos()
            {
                try
                {

                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
                }
            }
        #endregion

        #region (Metodos de Consulta)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Movimientos_Realizados
            /// DESCRIPCION : Consulta los movimientos que estan dadas de alta en la BD
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramírez Aguilera
            /// FECHA_CREO  : 08/Noviembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Movimientos_Realizados()
            {
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Rs_Consulta_Movimiento = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
                DataTable Dt_Movimiento; //Variable que obtendra los datos de la consulta 

                try
                {
                    Dt_Movimiento = Rs_Consulta_Movimiento.Consulta_Movimiento();
                    Session["Consulta_Movimiento"] = Dt_Movimiento;
                    Grid_Movimiento.DataSource = (DataTable)Session["Consulta_Movimiento"];
                    Grid_Movimiento.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta Movimientos Realizados" + ex.Message.ToString(), ex);
                }
            }
        #endregion
    #endregion

    #region(Eventos)
        #region(Botones)
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
            ///DESCRIPCIÓN: Habilita las cajas de texto necesarias para crear un Nuevo Movimiento
            ///             se convierte en dar alta cuando oprimimos Nuevo y dar alta  Crea un registro  
            ///                de un movimiento presupuestal en la base de datos
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  07/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
            {
                
                Cls_Ope_Psp_Ajuste_Presupuesto_Negocio Rs_Modificar_Presupuesto = new Cls_Ope_Psp_Ajuste_Presupuesto_Negocio();
                Cls_Cat_Com_Partidas_Negocio Rs_Partida = new Cls_Cat_Com_Partidas_Negocio();
                DataTable Dt_Temporal = new DataTable();
                String Ampliacion;
                String Reduccion;
                String Incremento;
                String Disponible;
                String Partida;
                String Fuente_Financiamiento_Id;
                String Programa_id;
                String Dependencia;
                int Contador_For;
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";
                    if (Btn_Nuevo.ToolTip == "Nuevo")
                    {
                        Hacer_Visible(true);//oculta el grid de movimietno y muestra todo lo demas
                        Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                        Dt_Temporal = (DataTable)Session["Session_Ajsute_Presupuesto"];
                        Grid_Partidas_Movimiento.DataSource = Dt_Temporal;
                        Grid_Partidas_Movimiento.DataBind();
                    }
                    else
                    {
                        
                        //se validaran los datos para saber si las sumas son iguales
                        if (Validar_Datos())
                        {
                            if (Grid_Partidas_Movimiento.Rows.Count > 0)
                            {

                                for (Contador_For = 0; Contador_For < Grid_Partidas_Movimiento.Rows.Count; Contador_For++)
                                {
                                    Rs_Modificar_Presupuesto.P_Monto_Ampliacion = "";
                                    Rs_Modificar_Presupuesto.P_Monto_Reduccion = "";
                                    Rs_Modificar_Presupuesto.P_Monto_Incremento = "";
                                    Grid_Partidas_Movimiento.Columns[6].Visible = true;
                                    Grid_Partidas_Movimiento.Columns[7].Visible = true;
                                    Grid_Partidas_Movimiento.Columns[8].Visible = true;
                                    GridViewRow selectedRow = Grid_Partidas_Movimiento.Rows[Grid_Partidas_Movimiento.SelectedIndex = Contador_For];
                                    Partida = HttpUtility.HtmlDecode(selectedRow.Cells[1].Text).ToString();
                                    Ampliacion = HttpUtility.HtmlDecode(selectedRow.Cells[2].Text).ToString();
                                    Reduccion = HttpUtility.HtmlDecode(selectedRow.Cells[3].Text).ToString();
                                    Incremento = HttpUtility.HtmlDecode(selectedRow.Cells[4].Text).ToString();
                                    Disponible = HttpUtility.HtmlDecode(selectedRow.Cells[5].Text).ToString();
                                    Fuente_Financiamiento_Id = HttpUtility.HtmlDecode(selectedRow.Cells[6].Text).ToString();
                                    Programa_id = HttpUtility.HtmlDecode(selectedRow.Cells[7].Text).ToString();
                                    Dependencia = HttpUtility.HtmlDecode(selectedRow.Cells[8].Text).ToString();
                                    //pasar los datos a la capa 
                                    //


                                    Rs_Modificar_Presupuesto.P_Partida_Id = Partida.Trim();
                                    Rs_Modificar_Presupuesto.P_Programa_Id = Programa_id.Trim();
                                    Rs_Modificar_Presupuesto.P_Fuente_Financiamiento_Id = Fuente_Financiamiento_Id.Trim();
                                    Rs_Modificar_Presupuesto.P_Unidad_Responsable_Id = Dependencia.Trim();
                                    Rs_Modificar_Presupuesto.P_Monto_Disponible = Disponible;
                                    Rs_Modificar_Presupuesto.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;

                                    Dt_Temporal = Rs_Modificar_Presupuesto.Consultar_Presupuesto();

                                    //para la partida
                                    Rs_Partida.P_Clave = Partida;
                                    Dt_Temporal = Rs_Partida.Consulta_Datos_Partidas();
                                    foreach (DataRow Registro in Dt_Temporal.Rows)
                                    {
                                        Partida = Registro[Cat_Com_Partidas.Campo_Partida_ID].ToString();
                                    }
                                    Rs_Modificar_Presupuesto.P_Partida_Id = Partida.Trim();
                                    if (Ampliacion != "0")
                                    {

                                        Rs_Modificar_Presupuesto.P_Monto_Ampliacion = Ampliacion;
                                    }
                                    else if (Reduccion != "0")
                                    {
                                        Rs_Modificar_Presupuesto.P_Monto_Reduccion = Reduccion;
                                    }
                                    else if (Incremento != "0")
                                    {
                                        Rs_Modificar_Presupuesto.P_Monto_Incremento = Incremento;
                                    }
                                    Rs_Modificar_Presupuesto.Modificar_Presupuesto();

                                    Grid_Partidas_Movimiento.Columns[6].Visible = false;
                                    Grid_Partidas_Movimiento.Columns[7].Visible = false;
                                    Grid_Partidas_Movimiento.Columns[8].Visible = false;

                                }
                            }
                            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Ajuste Presupuesto", "alert('El Alta del Ajuste de Presupuesto fue Exitosa');", true);
                            Session["Session_Ajsute_Presupuesto"] = null;
                            
                            Dt_Temporal = (DataTable)Session["Session_Ajsute_Presupuesto"];
                            Grid_Partidas_Movimiento.DataSource = Dt_Temporal;
                            Grid_Partidas_Movimiento.DataBind();
                        }
                            //de lo contrario no lo deja modificar los elementos
                        else
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                        }//else de validar_Datos
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
            ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
            ///DESCRIPCIÓN: Habilita las cajas de texto necesarias para poder Modificar la informacion,
            ///             se convierte en actualizar cuando oprimimos Modificar y se actualiza el registro 
            ///             en la base de datos
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  07/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
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
                        Hacer_Visible(true);
                    }
                    else
                    {
                        if (Validar_Datos())
                        {
                            //pasan los datos a la capa de negocio
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                        }//else de validar_Datos
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
            ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
            ///DESCRIPCIÓN: Elimina un registro de un Asunto de la base de datos
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  07/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
            {
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Movimiento_Presupuestal_Click
            ///DESCRIPCIÓN: Busca el movimiento y lo carga en el grid
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  07/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
            {
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";
                    if (!string.IsNullOrEmpty(Txt_Busqueda.Text.Trim()))
                    {
                        //se realiza la busqueda
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
            ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
            ///DESCRIPCIÓN: Cancela la operacion actual qye se este realizando
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  07/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
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
                        Inicializa_Controles(); //Habilita los controles para la siguiente operación del usuario en el catálogo
                        Session["Session_Ajsute_Presupuesto"] = null;
                        DataTable Dt_Temporal=new DataTable();
                        Dt_Temporal = (DataTable)Session["Session_Ajsute_Presupuesto"];
                        Grid_Partidas_Movimiento.DataSource = Dt_Temporal;
                        Grid_Partidas_Movimiento.DataBind();
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
            ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Click
            ///DESCRIPCIÓN: Agregara la informacion de la seccion de movimiento al grid
            ///             de partidas en movimiento.
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  08/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Btn_Agregar_Click(object sender, ImageClickEventArgs e)
            {
                DataTable Dt_Partidas_Movimiento = new DataTable();
                double Suma_Ampliacion;
                double Suma_Auxiliar;
                double Suma_Reduccion;
                double Monto_Disponible_Modificado;
                double Monto_Reduccion;
                double Monto_Ampliado;
                double Monto_Incremento;

                String Auxiliar;
                String Partida;
                String Ampliacion;
                String Reduccion;
                String Incremento;
                String Disponible;
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";


                    if (Validar_Movimiento())
                    {
                        //paso los valores a strings
                        Partida = Txt_Partida_Movimiento.Text;
                        Ampliacion = Txt_Ampliar_Movimiento.Text;
                        Reduccion = Txt_Reducir_Movimiento.Text;
                        Incremento = Txt_Incrementar_Movimiento.Text;
                        Disponible = Txt_Disponible_Movimiento.Text;

                        //sin no contiene nada se les agrega el cero para que se pueda visualisar en el grid
                        if (Ampliacion == "")
                        {
                            Ampliacion = "0";
                        }
                        if (Reduccion == "")
                        {
                            Reduccion = "0";
                        }
                        if (Incremento == "")
                        {
                            Incremento = "0";
                        }
                        //paso los valores al datarow
                        string[] Registro = { Partida, Ampliacion, Reduccion, Incremento, Disponible };

                        //para la suma de ampliacion
                        Auxiliar = Txt_Suma_Ampliacion.Text;
                        Suma_Ampliacion = Convert.ToDouble(Auxiliar);
                        Suma_Auxiliar = Convert.ToDouble(Ampliacion);
                        Suma_Ampliacion = Suma_Ampliacion + Suma_Auxiliar;
                        Txt_Suma_Ampliacion.Text = "" + Suma_Ampliacion;

                        //para la suma de reduccion
                        Auxiliar = Txt_Suma_Reduccion.Text;
                        Suma_Reduccion = Convert.ToDouble(Auxiliar);
                        Suma_Auxiliar = Convert.ToDouble(Reduccion);
                        Suma_Reduccion = Suma_Reduccion + Suma_Auxiliar;
                        Txt_Suma_Reduccion.Text = "" + Suma_Reduccion;

                        if (!string.IsNullOrEmpty(Txt_Ampliar_Movimiento.Text))
                        {
                            ////para realizar la operacion y poder aumentar el monto disponible
                            Monto_Disponible_Modificado=Convert.ToDouble(Disponible);
                            Monto_Ampliado = Convert.ToDouble(Ampliacion);
                            Monto_Disponible_Modificado = Monto_Disponible_Modificado + Monto_Ampliado;
                            Disponible =""+ Monto_Disponible_Modificado;
                        }

                        if (!string.IsNullOrEmpty(Txt_Reducir_Movimiento.Text))
                        {
                            //para realizar la operacion y poder restarle al monto disponible
                            Monto_Disponible_Modificado = Convert.ToDouble(Disponible);
                            Monto_Reduccion = Convert.ToDouble(Reduccion);
                            Monto_Disponible_Modificado = Monto_Disponible_Modificado - Monto_Reduccion;
                            Reduccion = "-" + Reduccion;
                            Disponible = "" + Monto_Disponible_Modificado;

                        }
                        if (!string.IsNullOrEmpty(Txt_Incrementar_Movimiento.Text))
                        {
                            ////para realizar la operacion y poder aumentar el monto disponible
                            Monto_Disponible_Modificado = Convert.ToDouble(Disponible);
                            Monto_Incremento = Convert.ToDouble(Incremento);
                            Monto_Disponible_Modificado = Monto_Disponible_Modificado + Monto_Incremento;
                            Disponible =""+ Monto_Disponible_Modificado;
                        }

                        DataTable Dt_Temporal = new DataTable();
                        DataRow Dt_row = null;

                        Grid_Partidas_Movimiento.Columns[6].Visible=true;
                        Grid_Partidas_Movimiento.Columns[7].Visible = true;
                        Grid_Partidas_Movimiento.Columns[8].Visible = true;

                        if (Session["Session_Ajsute_Presupuesto"] != null)
                        {
                            Dt_Temporal = (DataTable)Session["Session_Ajsute_Presupuesto"];
                        }

                        if (Dt_Temporal.Columns.Count == 0)
                        {
                            Dt_Temporal.Columns.Add("Partida");
                            Dt_Temporal.Columns.Add("Ampliacion");
                            Dt_Temporal.Columns.Add("Reduccion");
                            Dt_Temporal.Columns.Add("Incremento");
                            Dt_Temporal.Columns.Add("Disponible");
                            Dt_Temporal.Columns.Add("Fuente_Financiamiento");
                            Dt_Temporal.Columns.Add("Programa");
                            Dt_Temporal.Columns.Add("Unidad_Responsable");
                            

                            Dt_row = Dt_Temporal.NewRow();
                            Dt_row["Partida"] = Partida;
                            Dt_row["Ampliacion"] = Ampliacion;
                            Dt_row["Reduccion"] = Reduccion;
                            Dt_row["Incremento"] = Incremento;
                            Dt_row["Disponible"] = Disponible;
                            Dt_row["Fuente_Financiamiento"] = Cmb_Fuente_Financiamiento.SelectedValue;
                            Dt_row["Programa"] = Cmb_Programa.SelectedValue;
                            Dt_row["Unidad_Responsable"] = Cmb_Unidad_Responsable.SelectedValue;
                           
                            Dt_Temporal.Rows.Add(Dt_row);
                        }

                        else
                        {
                            Dt_row = Dt_Temporal.NewRow();
                            Dt_row["Partida"] = Partida;
                            Dt_row["Ampliacion"] = Ampliacion;
                            Dt_row["Reduccion"] = Reduccion;
                            Dt_row["Incremento"] = Incremento;
                            Dt_row["Disponible"] = Disponible;
                            Dt_row["Fuente_Financiamiento"] = Cmb_Fuente_Financiamiento.SelectedValue;
                            Dt_row["Programa"] = Cmb_Programa.SelectedValue;
                            Dt_row["Unidad_Responsable"] = Cmb_Unidad_Responsable.SelectedValue;
                            
                            Dt_Temporal.Rows.Add(Dt_row);
                        }
                        
                        Grid_Partidas_Movimiento.DataSource = Dt_Temporal;
                        Session["Session_Ajsute_Presupuesto"] = Dt_Temporal;
                        Grid_Partidas_Movimiento.DataBind();
                        Grid_Partidas_Movimiento.SelectedIndex = -1;
                        Grid_Partidas_Movimiento.Columns[1].Visible = false;
                        Grid_Partidas_Movimiento.Columns[7].Visible=false;
                        Grid_Partidas_Movimiento.Columns[8].Visible = false;
                        Grid_Partidas_Movimiento.Columns[9].Visible = false;
                                              
                        //limpiar la seccion de moviminento para la siguiente insercion de registro en 
                        //la seccion de movimiento
                        Txt_Partida_Movimiento.Text = "";
                        Txt_Disponible_Movimiento.Text = "";
                        Txt_Ampliar_Movimiento.Text = "";
                        Txt_Ampliar_Movimiento.Enabled = true;
                        Txt_Reducir_Movimiento.Text = "";
                        Txt_Reducir_Movimiento.Enabled = false;
                        Txt_Incrementar_Movimiento.Text = "";
                        Txt_Incrementar_Movimiento.Enabled = true;
                        Cmb_Unidad_Responsable.SelectedIndex = 0;
                        Cmb_Fuente_Financiamiento.SelectedIndex = 0;
                        Cmb_Programa.Enabled = false;
                        Cmb_Programa.SelectedIndex = 0;
                        Cmb_Unidad_Responsable.Enabled = false;
                        Cmb_Unidad_Responsable.SelectedIndex = 0;
                        Cmb_Partidas_Datos_Generales.Enabled = false;
                        Cmb_Partidas_Datos_Generales.SelectedIndex = 0;
                    }

                    //else de validar_Datos
                    else   
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }
                    System.Threading.Thread.Sleep(500);

                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Click
            ///DESCRIPCIÓN: Limpiara la informacion de las cajas de texto en la seccion de movimiento
            ///             de partidas en movimiento.
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  07/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
            {
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";
                    //para las cajas de texto
                    Txt_Ampliar_Movimiento.Text = "";
                    Txt_Ampliar_Movimiento.Enabled = true;
                    Txt_Reducir_Movimiento.Text = "";
                    //Txt_Reducir_Movimiento.Enabled = true;
                    Txt_Incrementar_Movimiento.Text = "";
                    Txt_Incrementar_Movimiento.Enabled = true;
                    if (!string.IsNullOrEmpty(Txt_Disponible_Movimiento.Text))
                    {
                        Txt_Reducir_Movimiento.Enabled = true;
                    }
                    else 
                    {
                        Txt_Reducir_Movimiento.Enabled = false;
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
            ///NOMBRE DE LA FUNCIÓN: Delete_Partida_Click
            ///DESCRIPCIÓN: elimina el registro del grid temporal
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  11/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Delete_Partida_Click(object sender, ImageClickEventArgs e)
            {
                int indice_Grid;
                String Ampliacion;
                String Reduccion;
                String Incremento;
                String Disponible;
                double Monto_Ampliacion;
                double Monto_Reduccion;
                double Suma_Ampliacion = 0;
                double Suma_Reduccion = 0;
                DataTable Dt_Temporal = new DataTable();
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    

                    //para sacar el indice del grid seleccionado y se le suma uno para igualar el dato correcto
                    indice_Grid = Grid_Partidas_Movimiento.SelectedIndex;
                    if (indice_Grid > -1)
                    {
                        Grid_Partidas_Movimiento.Columns[1].Visible = true;
                        Grid_Partidas_Movimiento.Columns[6].Visible = true;
                        Grid_Partidas_Movimiento.Columns[7].Visible = true;
                        Grid_Partidas_Movimiento.Columns[8].Visible = true;
                        Dt_Temporal = (DataTable)Session["Session_Ajsute_Presupuesto"];

                        //para eliminar el registro
                        Dt_Temporal.Rows.RemoveAt(indice_Grid);
                        //asigno un nombre a la tabla para poder extraer los elementos
                        Dt_Temporal.TableName = "Temporal";

                        if (Dt_Temporal.Rows.Count > 0)
                        {
                            foreach (DataRow Registro in Dt_Temporal.Rows)
                            {
                                Ampliacion = (Registro["Ampliacion"].ToString());
                                Reduccion = (Registro["Reduccion"].ToString());
                                Incremento = (Registro["Incremento"].ToString());
                                Disponible = (Registro["Disponible"].ToString());

                                if (Ampliacion != "0")
                                {
                                    //para la suma de ampliacion
                                    Monto_Ampliacion = Convert.ToDouble(Ampliacion);
                                    Suma_Ampliacion = Suma_Ampliacion + Monto_Ampliacion;
                                    Txt_Suma_Ampliacion.Text = "" + Suma_Ampliacion;
                                }
                                else if (Reduccion != "0")
                                {
                                    //para lasuma de reduccion
                                    Monto_Reduccion = Convert.ToDouble(Reduccion);
                                    Monto_Reduccion = Monto_Reduccion * -1;
                                    Suma_Reduccion = Suma_Reduccion + Monto_Reduccion;
                                    Txt_Suma_Reduccion.Text = "" + Suma_Reduccion;
                                }
                                else
                                {
                                    //para incremento

                                }
                            }   //fin del foreach
                        }
                        else
                        {
                            Txt_Suma_Ampliacion.Text = "0";
                            Txt_Suma_Reduccion.Text = "0";
                        }

                        //para cargar el grid
                        Session["Session_Ajsute_Presupuesto"] = Dt_Temporal;
                        Grid_Partidas_Movimiento.DataSource = Dt_Temporal;
                        Grid_Partidas_Movimiento.DataBind();
                        Grid_Partidas_Movimiento.SelectedIndex = 0;
                        Grid_Partidas_Movimiento.Columns[6].Visible = false;
                        Grid_Partidas_Movimiento.Columns[7].Visible = false;
                        Grid_Partidas_Movimiento.Columns[8].Visible = false;
                        Grid_Partidas_Movimiento.Columns[1].Visible = false;


                        Txt_Reducir_Movimiento.Enabled = false;
                        System.Threading.Thread.Sleep(500);
                    }
                    else 
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Seleccione un registro en la sección de Partidas En movimiento";
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

        #region(Combos)
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Responsable
            ///DESCRIPCIÓN: Cargara todos los responsables dentro del combo 
            ///             (Proviene del metodo Inicializa_Controles())
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  07/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cargar_Combo_Responsable()
            {
                Cls_Cat_Dependencias_Negocio Cls_Respondable = new Cls_Cat_Dependencias_Negocio();
                DataTable Dt_Responsable = new DataTable();
                try
                {
                    Cmb_Unidad_Responsable.Items.Clear();
                    Dt_Responsable = Cls_Respondable.Consulta_Dependencias();
                    Cmb_Unidad_Responsable.DataSource = Dt_Responsable;
                    Cmb_Unidad_Responsable.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
                    Cmb_Unidad_Responsable.DataTextField = Cat_Dependencias.Campo_Nombre;
                    Cmb_Unidad_Responsable.DataBind();
                    Cmb_Unidad_Responsable.Items.Insert(0, "< SELECCIONE RESPONSABLE >");
                    
                    Cmb_Unidad_Responsable.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Financiamiento
            ///DESCRIPCIÓN: Cargara todos las fuentes de financiamiento dentro del combo 
            ///             (Proviene del metodo Inicializa_Controles())
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  07/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cargar_Combo_Financiamiento()
            {
                Cls_Cat_SAP_Fuente_Financiamiento_Negocio Cls_Financiamiento = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio();
                DataTable Dt_Financinamiento = new DataTable();
                try
                {
                   
                    Cmb_Fuente_Financiamiento.Items.Clear();
                    Dt_Financinamiento = Cls_Financiamiento.Consulta_Datos_Fuente_Financiamiento();
                    Cmb_Fuente_Financiamiento.DataSource = Dt_Financinamiento;
                    Cmb_Fuente_Financiamiento.DataValueField = Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID;
                    Cmb_Fuente_Financiamiento.DataTextField = Cat_SAP_Fuente_Financiamiento.Campo_Descripcion;
                    Cmb_Fuente_Financiamiento.DataBind();
                    Cmb_Fuente_Financiamiento.Items.Insert(0, "< SELECCIONE FUENTE FINANCIAMIENTO >");
                    Cmb_Fuente_Financiamiento.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Programa
            ///DESCRIPCIÓN: Cargara todos los Programas dentro del combo 
            ///             (Proviene del metodo Inicializa_Controles())
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  07/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cargar_Combo_Programa()
            {
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Cls_Programa = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
                DataTable Dt_Programa = new DataTable();
                try
                {

                    Cmb_Programa.Items.Clear();
                    Dt_Programa = Cls_Programa.Consultar_Programa();
                    Cmb_Programa.DataSource = Dt_Programa;
                    Cmb_Programa.DataValueField = Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id;
                    Cmb_Programa.DataTextField = Cat_Sap_Proyectos_Programas.Campo_Nombre;
                    Cmb_Programa.DataBind();
                    Cmb_Programa.Items.Insert(0, "< SELECCIONE PROGRAMA >");
                    Cmb_Programa.SelectedIndex = 0;
                    
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Area_Funcional
            ///DESCRIPCIÓN: Cargara todos las areas funcionales dentro del combo 
            ///             (Proviene del metodo Inicializa_Controles())
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  07/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            /*protected void Cargar_Combo_Area_Funcional()
            {
                Cls_Ope_Pre_Movimiento_Presupuestal_Negocio  Rs_Area_Funcional = new Cls_Ope_Pre_Movimiento_Presupuestal_Negocio();
                DataTable Dt_Area = new DataTable();
                try
                {
                    Cmb_Area_Funcional.Items.Clear();
                    Dt_Area = Rs_Area_Funcional.Consultar_Area_Funciona();
                    Cmb_Area_Funcional.DataSource = Dt_Area;
                    Cmb_Area_Funcional.DataValueField = Cat_SAP_Area_Funcional.Campo_Clave;
                    Cmb_Area_Funcional.DataTextField = Cat_SAP_Area_Funcional.Campo_Descripcion;
                    Cmb_Area_Funcional.DataBind();
                    Cmb_Area_Funcional.Items.Insert(0, "< SELECIONEN AREA FUNCIONAL >");
                    Cmb_Area_Funcional.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }*/
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Partida
            ///DESCRIPCIÓN: Cargara todos las partidas dentro del combo 
            ///             (Proviene del metodo Inicializa_Controles())
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  07/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cargar_Combo_Partida()
            {
                Cls_Cat_Com_Partidas_Negocio Cls_Partida = new Cls_Cat_Com_Partidas_Negocio();
                DataTable Dt_Partida = new DataTable();
                try
                {
                    Cmb_Partidas_Datos_Generales.Items.Clear();
                    Dt_Partida = Cls_Partida.Consulta_Datos_Partidas();
                    Cmb_Partidas_Datos_Generales.DataSource = Dt_Partida;
                    Cmb_Partidas_Datos_Generales.DataValueField = Cat_Com_Partidas.Campo_Clave;
                    Cmb_Partidas_Datos_Generales.DataTextField = Cat_Com_Partidas.Campo_Nombre;
                    Cmb_Partidas_Datos_Generales.DataBind();
                    Cmb_Partidas_Datos_Generales.Items.Insert(0, "< SELECCIONE PARTIDA >");
                    Cmb_Partidas_Datos_Generales.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cmb_Fuente_Financiamiento_SelectedIndexChanged
            ///DESCRIPCIÓN: habilita el siguiente combo y pasa la informacion de la clave
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  08/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cmb_Fuente_Financiamiento_SelectedIndexChanged(object sender, EventArgs e)
            {
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    
                    if (Cmb_Fuente_Financiamiento.SelectedIndex == 0)
                    {
                        Cmb_Programa.Enabled = false;
                        Cmb_Unidad_Responsable.Enabled = false;
                        Cmb_Programa.Enabled = false;
                        Cmb_Partidas_Datos_Generales.Enabled = false;
                        Cmb_Programa.SelectedIndex = 0;
                    }
                    else 
                    {
                        Cmb_Programa.Enabled = true;
                    }
                    
                   

                    /* Txt_Codigo.Text = "";
                    Cmb_Area_Funcional.Enabled = true; Txt_Codigo.Text = "" + Cmb_Fuente_Financiamiento.SelectedValue;
                    
                    Auxiliar = Txt_Codigo.Text;
                    Txt_Codigo.Text = Auxiliar + "-" + Cmb_Area_Funcional.SelectedValue;
                    
                    Auxiliar = Txt_Codigo.Text;
                    Txt_Codigo.Text = Auxiliar + "-" + Cmb_Programa.SelectedValue;
                   
                    Auxiliar = Txt_Codigo.Text;
                    Txt_Codigo.Text = Auxiliar + "-" + Cmb_Unidad_Responsable.SelectedValue;
                   
                    Auxiliar = Txt_Codigo.Text; 
                    Txt_Codigo.Text = Auxiliar + "-" + Cmb_Partidas_Datos_Generales.SelectedValue;
                    */
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cmb_Programa_SelectedIndexChanged
            ///DESCRIPCIÓN: habilita el siguiente combo y pasa la informacion de la clave
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  08/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cmb_Programa_SelectedIndexChanged(object sender, EventArgs e)
            {
                try
                {
                    if (Cmb_Programa.SelectedIndex == 0)
                    {
                        Cmb_Unidad_Responsable.Enabled = false;
                        Cmb_Partidas_Datos_Generales.Enabled = false;
                        Cmb_Programa.Enabled = false;
                        Cmb_Unidad_Responsable.SelectedIndex = 0;
                    }
                    else
                    {
                        Cmb_Unidad_Responsable.Enabled = true;
                    }
                   /* Txt_Codigo.Text = "";
                    String Auxiliar;
                    Txt_Codigo.Text = "" + Cmb_Fuente_Financiamiento.SelectedValue;

                    Auxiliar = Txt_Codigo.Text;
                    Txt_Codigo.Text = Auxiliar + "-" + Cmb_Area_Funcional.SelectedValue;

                    Auxiliar = Txt_Codigo.Text;
                    Txt_Codigo.Text = Auxiliar + "-" + Cmb_Programa.SelectedValue;

                    Auxiliar = Txt_Codigo.Text;
                    Txt_Codigo.Text = Auxiliar + "-" + Cmb_Unidad_Responsable.SelectedValue;

                    Auxiliar = Txt_Codigo.Text;
                    Txt_Codigo.Text = Auxiliar + "-" + Cmb_Partidas_Datos_Generales.SelectedValue;
                    */
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cmb_Unidad_Responsable_SelectedIndexChanged
            ///DESCRIPCIÓN: habilita el siguiente combo y pasa la informacion de la clave
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  08/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cmb_Unidad_Responsable_SelectedIndexChanged(object sender, EventArgs e)
            {
                try
                {
                    if (Cmb_Unidad_Responsable.SelectedIndex == 0)
                    {
                        Cmb_Partidas_Datos_Generales.Enabled = false;
                        Cmb_Partidas_Datos_Generales.SelectedIndex = 0;
                    }
                    else
                    {
                        Cmb_Partidas_Datos_Generales.Enabled = true;
                    }
                    /*String Auxiliar;
                    Txt_Codigo.Text = "";

                    Txt_Codigo.Text = "" + Cmb_Fuente_Financiamiento.SelectedValue;

                    Auxiliar = Txt_Codigo.Text;
                    Txt_Codigo.Text = Auxiliar + "-" + Cmb_Area_Funcional.SelectedValue;

                    Auxiliar = Txt_Codigo.Text;
                    Txt_Codigo.Text = Auxiliar + "-" + Cmb_Programa.SelectedValue;

                    Auxiliar = Txt_Codigo.Text;
                    Txt_Codigo.Text = Auxiliar + "-" + Cmb_Unidad_Responsable.SelectedValue;

                    Auxiliar = Txt_Codigo.Text;
                    Txt_Codigo.Text = Auxiliar + "-" + Cmb_Partidas_Datos_Generales.SelectedValue;
                     */
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cmb_Partidas_Datos_Generales_SelectedIndexChanged
            ///DESCRIPCIÓN: carga el elemento seleccionado en la caja de texto partida
            ///             en la seccion de movimiento, busca el monto disponible y lo carga
            ///             en la caja de texto disponible en la seccion de movimiento 
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  07/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cmb_Partidas_Datos_Generales_SelectedIndexChanged(object sender, EventArgs e)
            {
                Cls_Cat_Com_Partidas_Negocio Rs_Partida = new Cls_Cat_Com_Partidas_Negocio();
                Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Presupuesto = new Cls_Ope_SAP_Dep_Presupuesto_Negocio();
                Cls_Ope_Psp_Ajuste_Presupuesto_Negocio Rs_Ajuste = new Cls_Ope_Psp_Ajuste_Presupuesto_Negocio();

                DataTable Dt_Partida = new DataTable();
                DataTable Dt_Presupuesto = new DataTable();
                String Partida_Id;
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";
                    //Txt_Codigo.Text = "";
                    String Importe_Disponible;

                    if (Cmb_Partidas_Datos_Generales.SelectedIndex == 0)
                    {
                        Txt_Partida_Movimiento.Text = "";
                    }
                    else
                    {
                        //Cat_Com_Dep_Presupuesto para presupuesto
                        Txt_Partida_Movimiento.Text = Cmb_Partidas_Datos_Generales.SelectedValue;
                        Rs_Partida.P_Clave = Txt_Partida_Movimiento.Text;
                        Dt_Partida = Rs_Partida.Consulta_Datos_Partidas();

                        //para buscar el partida_id
                        foreach (DataRow Registro in Dt_Partida.Rows)
                        {
                            Partida_Id = Registro[Cat_Com_Partidas.Campo_Partida_ID].ToString();

                            Rs_Presupuesto.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue;
                            Rs_Presupuesto.P_Fuente_Financiamiento_ID = Cmb_Fuente_Financiamiento.SelectedValue;
                            Rs_Presupuesto.P_Programa_ID = Cmb_Programa.SelectedValue;
                            Rs_Presupuesto.P_Partida_ID = Partida_Id;
                            Dt_Presupuesto = Rs_Presupuesto.Consulta_Datos_Presupuestos();


                            Rs_Ajuste.P_Fuente_Financiamiento_Id = Cmb_Fuente_Financiamiento.SelectedValue;
                            Rs_Ajuste.P_Partida_Id = Partida_Id;
                            Rs_Ajuste.P_Programa_Id = Cmb_Programa.SelectedValue;
                            Rs_Ajuste.P_Unidad_Responsable_Id = Cmb_Unidad_Responsable.SelectedValue;
                        }

                        Dt_Presupuesto = Rs_Ajuste.Consultar_Presupuesto();


                        foreach (DataRow Registro in Dt_Presupuesto.Rows)
                        {
                            Importe_Disponible = Registro[Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible].ToString();
                            Txt_Disponible_Movimiento.Text = Importe_Disponible;
                            
                            
                        }
                        if (Dt_Presupuesto.Rows.Count == 0)
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "No se encuentra registro en la base de datos";
                            Cmb_Programa.Enabled = false;
                            Cmb_Programa.SelectedIndex = 0;
                            Cmb_Unidad_Responsable.Enabled = false;
                            Cmb_Unidad_Responsable.SelectedIndex = 0;
                            Cmb_Partidas_Datos_Generales.Enabled = false;
                            Cmb_Partidas_Datos_Generales.SelectedIndex = 0;
                            Cmb_Fuente_Financiamiento.SelectedIndex = 0;
                            Txt_Partida_Movimiento.Text = "";
                            Txt_Disponible_Movimiento.Text = "";
                            Txt_Reducir_Movimiento.Enabled = false;
                        }
                        else
                        {
                            Txt_Reducir_Movimiento.Enabled = true;
                        }
                        /*Txt_Codigo.Text = "" + Cmb_Fuente_Financiamiento.SelectedValue;
                        Auxiliar = Txt_Codigo.Text;
                        Txt_Codigo.Text = Auxiliar + "-" + Cmb_Area_Funcional.SelectedValue;
                        Auxiliar = Txt_Codigo.Text;
                        Txt_Codigo.Text = Auxiliar + "-" + Cmb_Programa.SelectedValue;
                        Auxiliar = Txt_Codigo.Text;
                        Txt_Codigo.Text = Auxiliar + "-" + Cmb_Unidad_Responsable.SelectedValue;
                        Auxiliar = Txt_Codigo.Text;
                        Txt_Codigo.Text = Auxiliar + "-" + Cmb_Partidas_Datos_Generales.SelectedValue;

                        Lbl_Codigo.Visible = true;
                        Txt_Codigo.Visible = true;*/
                        //aqui va la busqueda del monto


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

        #region(TextBox)
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Txt_Ampliar_Movimiento_TextChanged
            ///DESCRIPCIÓN: al introducir algun valor en la caja de texto se desabilitaran
            ///             las cajas de reduccir e incrementar
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  07/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************           
            protected void Txt_Ampliar_Movimiento_TextChanged(object sender, EventArgs e)
            {
                try
                {
                    Txt_Reducir_Movimiento.Enabled = false;
                    Txt_Incrementar_Movimiento.Enabled = false;
                    Txt_Ampliar_Movimiento.Enabled = false;
                    Btn_Agregar.Enabled = true;
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Txt_Reducir_Movimiento_TextChanged
            ///DESCRIPCIÓN: al introducir algun valor en la caja de texto se desabilitaran
            ///             las cajas de Ampliar e Incrementar
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  07/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************           
            protected void Txt_Reducir_Movimiento_TextChanged(object sender, EventArgs e)
            {
                try
                {
                    double Monto_Disponible;
                    double Monto_Reducido;
                    String Disponible;
                    String Reducir;

                    //comvertir los en numero
                    Disponible = Txt_Disponible_Movimiento.Text;
                    Reducir = Txt_Reducir_Movimiento.Text;
                    Monto_Disponible = Convert.ToDouble(Disponible);
                    Monto_Reducido = Convert.ToDouble(Reducir);

                    //por si se exede de lo que dispone
                    if ( Monto_Reducido > Monto_Disponible )
                    {
                        Txt_Reducir_Movimiento.Text = "";
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "No puede execerse del monto al que dispone";
                    }
                    else
                    { 
                        
                        Txt_Ampliar_Movimiento.Enabled = false;
                        Txt_Incrementar_Movimiento.Enabled = false;
                        Txt_Reducir_Movimiento.Enabled = false;
                        Btn_Agregar.Enabled = true;
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
            ///NOMBRE DE LA FUNCIÓN: Txt_Incrementar_Movimiento_TextChanged
            ///DESCRIPCIÓN: al introducir algun valor en la caja de texto se desabilitaran
            ///             las cajas de Ampliar y reducir
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  07/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************           
            protected void Txt_Incrementar_Movimiento_TextChanged(object sender, EventArgs e)
            {
                try
                {
                    if (Txt_Incrementar_Movimiento.Text == "")
                    {
                        Txt_Ampliar_Movimiento.Enabled = true;
                        Txt_Reducir_Movimiento.Enabled = true;
                        Btn_Agregar.Enabled = false;
                    }
                    else
                    {
                        Txt_Ampliar_Movimiento.Enabled = false;
                        Txt_Reducir_Movimiento.Enabled = false;
                        Txt_Incrementar_Movimiento.Enabled = false;
                        Btn_Agregar.Enabled = true;
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


    #region(Grid)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Movimiento_SelectedIndexChanged
        /// DESCRIPCION : 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 08/Noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Movimiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Rs_Consulta_Movimiento = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
            String Numero_Solicitud;
            String Auxiliar_Datos;
            
            DataTable Dt_Consulta = new DataTable();
            try
            {
                GridViewRow selectedRow = Grid_Movimiento.Rows[Grid_Movimiento.SelectedIndex];
                
                Numero_Solicitud = HttpUtility.HtmlDecode(selectedRow.Cells[1].Text).ToString();
                Rs_Consulta_Movimiento.P_No_Solicitud = Numero_Solicitud;
                Dt_Consulta = Rs_Consulta_Movimiento.Consulta_Movimiento();

                
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        /// ********************************************************************************
        /// NOMBRE: Grid_Movimiento_Sorting
        /// 
        /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
        /// 
        /// CREÓ:        Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:  09/Noviembre/2011
        /// MODIFICÓ:
        /// FECHA MODIFICÓ:
        /// CAUSA MODIFICACIÓN:
        /// **********************************************************************************
        protected void Grid_Movimiento_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                Consulta_Movimientos_Realizados();
                DataTable Dt_Movimiento = (Grid_Movimiento.DataSource as DataTable);

                if (Dt_Movimiento != null)
                {
                    DataView Dv_Movimiento = new DataView(Dt_Movimiento);
                    String Orden = ViewState["SortDirection"].ToString();

                    if (Orden.Equals("ASC"))
                    {
                        Dv_Movimiento.Sort = e.SortExpression + " " + "DESC";
                        ViewState["SortDirection"] = "DESC";
                    }
                    else
                    {
                        Dv_Movimiento.Sort = e.SortExpression + " " + "ASC";
                        ViewState["SortDirection"] = "ASC";
                    }

                    Grid_Movimiento.DataSource = Dv_Movimiento;
                    Grid_Movimiento.DataBind();
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
        /// NOMBRE DE LA FUNCION: Grid_Movimiento_SelectedIndexChanged
        /// DESCRIPCION : Elimina del grid el registro
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 09/Noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Partidas_Movimiento_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
               
                GridViewRow selectedRow = Grid_Partidas_Movimiento.Rows[Grid_Partidas_Movimiento.SelectedIndex];
                Global_Indice = Grid_Partidas_Movimiento.SelectedIndex;
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
