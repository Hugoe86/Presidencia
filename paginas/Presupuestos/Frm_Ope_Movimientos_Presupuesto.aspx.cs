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
using Presidencia.Autorizar_Traspaso_Presupuestal.Negocio;

public partial class paginas_Presupuestos_Frm_Ope_Movimientos_Presupuesto : System.Web.UI.Page
{
  
    #region(Load)
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
                Session["Session_Movimientos_Presupuesto"] = null;

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
                    
                    Habilitar_Controles("Inicial");//Inicializa todos los controles
                    Cargar_Combo_Responsable();
                    Consulta_Movimiento();
                    Cargar_Combo_Financiamiento(1);
                    Cargar_Combo_Financiamiento(2);
                    Habilitar_Visible(false);

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
                    //para busqueta
                    Txt_Busqueda.Text = "";

                    //para datos generales
                    Cmb_Operacion.SelectedIndex = 0;
                    Txt_Importe.Text = "";
                    Txt_No_Solicitud.Text = "";
                    Cmb_Estatus.SelectedIndex = 0;

                    //partida Origen
                    Txt_Codigo1.Text = "";
                    Cmb_Fuente_Financiamiento_Origen.Items.Clear();
                    Cmb_Programa_Origen.Items.Clear();
                    Cmb_Partida_Origen.Items.Clear();
                    Cmb_Capitulo_Origen.Items.Clear();



                    //partida Destino
                    Txt_Codigo2.Text = "";
                    Cmb_Fuente_Financiamiento_Destino.Items.Clear();
                    Cmb_Programa_Destino.Items.Clear();
                    Cmb_Partida_Destino.Items.Clear();
                    Cmb_Capitulo_Destino.Items.Clear();
                    Txt_Justificacion.Text = "";
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

                            Configuracion_Acceso("Frm_Ope_Movimientos_Presupuesto.aspx");
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
                    Cmb_Operacion.Enabled = Habilitado;
                    Txt_Importe.Enabled = Habilitado;
                    Cmb_Estatus.Enabled = Habilitado;

                    //partida Origen
                    Txt_Codigo1.Enabled = Habilitado;
                    Cmb_Fuente_Financiamiento_Origen.Enabled = Habilitado;
                    Cmb_Programa_Origen.Enabled = Habilitado;
                    Cmb_Partida_Origen.Enabled = Habilitado;
                    Cmb_Capitulo_Origen.Enabled = Habilitado;


                    //partida Destino
                    Txt_Codigo2.Enabled = Habilitado;
                    Cmb_Fuente_Financiamiento_Destino.Enabled = Habilitado;
                    Cmb_Programa_Destino.Enabled = Habilitado;
                    Cmb_Partida_Destino.Enabled = Habilitado;
                    Cmb_Capitulo_Destino.Enabled = Habilitado;
                    Txt_Justificacion.Enabled = Habilitado;
                    //mensajes de error
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                }

                catch (Exception ex)
                {
                    throw new Exception("Limpia_Controles " + ex.Message.ToString());
                }
        }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Habilitar_Visible
            /// DESCRIPCION : hace visibles a las cajas de texto y etiquetas
            /// PARAMETROS  : 1.- Boolean Habilitado  Pasa el valor de ture o false 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 24-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Habilitar_Visible(Boolean Habilitado)
            {
                try
                {
                    //para inicial
                    Div_Grid_Movimientos.Visible = !Habilitado;
                    Lbl_Unidad_Responsable.Visible = !Habilitado;
                    Cmb_Unidad_Responsable.Visible = !Habilitado;
                    Grid_Movimiento.Visible = !Habilitado;
                    //Lbl_Buscar_Estatus.Visible = !Habilitado;
                    //Cmb_Buscar_Estatus.Visible = !Habilitado;
                    Lbl_Fecha.Visible = !Habilitado;
                    Txt_Fecha_Inicial.Visible = !Habilitado;
                    Txt_Fecha_Final.Visible = !Habilitado;


                    //para datos generales
                    Div_Datos_Generales.Visible = Habilitado;
                    Lbl_Datos_Generales.Visible = Habilitado;
                    Lbl_Operacion.Visible = Habilitado;
                    Cmb_Operacion.Visible = Habilitado;
                    Lbl_Estatus.Visible = Habilitado;
                    Cmb_Estatus.Visible = Habilitado;
                    Lbl_Importe.Visible = Habilitado;
                    Txt_Importe.Visible = Habilitado;
                    Lbl_Numero_solicitud.Visible = Habilitado;
                    Txt_No_Solicitud.Visible = Habilitado;

                    //para partida origen
                    Div_Partida_Origen.Visible = Habilitado;
                    Lbl_Partida_Origen_Encabezado.Visible = Habilitado;
                    Lbl_Codigo_Origen.Visible = Habilitado;
                    Txt_Codigo1.Visible = Habilitado;
                    Lbl_Capitulo_Origen.Visible = Habilitado;
                    Cmb_Capitulo_Origen.Visible = Habilitado;
                    lbl_Unidad_Responsable_Origen.Visible = Habilitado;
                    Cmb_Unidad_Responsable_Origen.Visible = Habilitado;
                    Lbl_Partida_Origen.Visible = Habilitado;
                    Cmb_Partida_Origen.Visible = Habilitado;
                    Lbl_Fuente_Financiamiento_Origen.Visible = Habilitado;
                    Cmb_Fuente_Financiamiento_Origen.Visible = Habilitado;
                    Lbl_Programa_Origen.Visible = Habilitado;
                    Cmb_Programa_Origen.Visible = Habilitado;
                    Lbl_Partida_Origen.Visible = Habilitado;
                    Cmb_Partida_Origen.Visible = Habilitado;

                    //para partida destino
                    Div_Partida_Destino.Visible = Habilitado;
                    Lbl_Partida_Destino_Encabezado.Visible = Habilitado;
                    Lbl_Codigo_Pragramatico_Destino.Visible = Habilitado;
                    Txt_Codigo2.Visible = Habilitado;
                    Lbl_Capitulo_Destino.Visible = Habilitado;
                    Cmb_Capitulo_Destino.Visible = Habilitado;
                    Lbl_Unidad_Responsable_Destino.Visible = Habilitado;
                    Cmb_Unidad_Responsable_Destino.Visible = Habilitado;
                    Lbl_Partida_Destino.Visible = Habilitado;
                    Cmb_Partida_Destino.Visible = Habilitado;
                    Lbl_Fuente_Financiamiento_Destino.Visible = Habilitado;
                    Cmb_Fuente_Financiamiento_Destino.Visible = Habilitado;
                    Lbl_Programa_Destino.Visible = Habilitado;
                    Cmb_Programa_Destino.Visible = Habilitado;
                    Lbl_Partida_Destino.Visible = Habilitado;
                    Cmb_Partida_Destino.Visible = Habilitado;
                    Lbl_Justificacion.Visible = Habilitado;
                    Txt_Justificacion.Visible = Habilitado;

                    //para el grid de comentarios
                    Div_Grid_Comentarios.Visible = Habilitado;
                    Grid_Comentarios.Visible = Habilitado;
                    
                }
                catch (Exception ex)
                {
                    throw new Exception("Limpia_Controles " + ex.Message.ToString());
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
                 if (Cmb_Operacion.SelectedIndex <= 0)
                 {
                     Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione la operacion deseada. <br>";
                     Datos_Validos = false;
                 }
                 if (Txt_Importe.Text=="")
                 {
                     Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el monto deseado.<br>";
                     Datos_Validos = false;
                 }
                 if (Cmb_Fuente_Financiamiento_Origen.SelectedIndex <= 0)
                 {
                     Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione algun Fuente de financiamiento de Origen.<br>";
                     Datos_Validos = false;
                 }
                 if (Cmb_Fuente_Financiamiento_Destino.SelectedIndex <= 0)
                 {
                     Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione algun Fuente de financiamiento de Destino.<br>";
                     Datos_Validos = false;
                 }
                 if (Cmb_Programa_Origen.SelectedIndex <= 0)
                 {
                     Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione algun Programa de Origen.<br>";
                     Datos_Validos = false;
                 }
                 if (Cmb_Programa_Destino.SelectedIndex <= 0)
                 {
                     Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione algun Programa de Destino.<br>";
                     Datos_Validos = false;
                 }
                 if (Cmb_Capitulo_Origen.SelectedIndex <= 0)
                 {
                     Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione algun Capitulo de Origen.<br>";
                     Datos_Validos = false;
                 }
                 if (Cmb_Capitulo_Destino.SelectedIndex <= 0)
                 {
                     Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione algun Capitulo de Destino.<br>";
                     Datos_Validos = false;
                 }
                 if (Cmb_Partida_Origen.SelectedIndex <= 0)
                 {
                     Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione alguna Partida de Origen.<br>";
                     Datos_Validos = false;
                 }
                 if (Cmb_Partida_Destino.SelectedIndex <= 0)
                 {
                     Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione alguna Partida de Destino.<br>";
                     Datos_Validos = false;
                 }
                 if (Txt_Justificacion.Text == "")
                 {
                     Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la justificación.<br>";
                     Datos_Validos = false;
                 }
                
                return Datos_Validos;
            }
            #endregion

            #region(metodos De consulta)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Buscar_Clave_Individual
            /// DESCRIPCION : separa la clave de codigo programatico en las claves que la forman
            /// PARAMETROS  : 1.codigo: Es el codigo programatico que se desgloza en ses id
            ///               2.Posicion. a que codigo pertenese 1 a origen, 2 a destino  
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 15-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Buscar_Clave_Individual(String Codigo, int Posicion)
            {
                Cls_Cat_Com_Partidas_Negocio Capitulos = new Cls_Cat_Com_Partidas_Negocio(); //Variable de conexión hacia la capa de Negocios
                string Fuente_Financiamiento = "";//se usaran las variables string para contener las claves 
                string Area_Funcional;
                string Programa;
                string Responsable;
                string Partida;
                string Partida_Capitulo;
                string Clave = ""; //se usara para almacenenar las claves individuales que sean obtenidas en el ciclo for 
                int Cont_For;//contador para el ciclo for
                int Cont_Posicion = 1;//contador para la posicion del guion en la cadena de texto
                int Operacion;
                try
                {
                    switch (Posicion)
                    {
                        case 1:
                            Operacion = 1;
                            //Buscara de uno en uno hasta encontrar un guion en la cadena
                            //posteriormente pasara la informacion obtenida en los caracteres anteriores a clave
                            //luego se compara la posicion del guia para saber que clave es
                            //se pase la informacion para realizar la consulta
                            for (Cont_For = 0; Cont_For < Codigo.Length; Cont_For++)
                            {
                                if (Codigo.Substring(Cont_For, 1) == "-")//sirve para saber la posicion 
                                {
                                    if (Cont_Posicion == 1)//para la consulta de Fuente de Financiamiento 
                                    {
                                        Fuente_Financiamiento = Clave;
                                        if (Fuente_Financiamiento.Length < 5)
                                        {
                                            Fuente_Financiamiento += " ";
                                        }
                                        Cmb_Fuente_Financiamiento_Origen.SelectedIndex = Cmb_Fuente_Financiamiento_Origen.Items.IndexOf(Cmb_Fuente_Financiamiento_Origen.Items.FindByValue(Fuente_Financiamiento));
                                        Clave = "";
                                    }
                                    if (Cont_Posicion == 2)//para la consulta de Area Funcional
                                    {
                                        Area_Funcional = Clave;
                                        Txt_Area_Origen.Text = Area_Funcional;
                                        Clave = "";
                                    }
                                    if (Cont_Posicion == 3)//para la consulta de programa
                                    {
                                        Programa = Clave;
                                        Cmb_Programa_Origen.SelectedIndex = Cmb_Programa_Origen.Items.IndexOf(Cmb_Programa_Origen.Items.FindByValue(Programa));
                                        Clave = "";
                                    }
                                    if (Cont_Posicion == 4)//para la consulta de Responsable
                                    {
                                        Responsable = Clave;
                                        Cargar_Combo_Consulta_Responsable(Responsable, Operacion);
                                        Cmb_Unidad_Responsable_Origen.SelectedIndex = Cmb_Unidad_Responsable_Origen.Items.IndexOf(Cmb_Unidad_Responsable_Origen.Items.FindByValue(Responsable));
                                        Clave = "";
                                    }

                                    Cont_Posicion++;//incrementa el valor, para asi de esta manera pasar a la siguiente clave 
                                }
                                else
                                {
                                    Clave += Codigo.Substring(Cont_For, 1);
                                }
                            }
                            //para la ultima clave que es partida
                            Partida = Clave;
                            if (Partida.Length < 5)
                            {
                                Partida += " ";
                            }
                            Partida_Capitulo = Partida.Substring(0, 1);
                            Partida_Capitulo += "000 ";
                            Cmb_Capitulo_Origen.SelectedIndex = Cmb_Capitulo_Origen.Items.IndexOf(Cmb_Capitulo_Origen.Items.FindByValue(Partida_Capitulo));
                            
                            Cargar_Combo_Partida(Operacion);

                            Cmb_Partida_Origen.SelectedIndex = Cmb_Partida_Origen.Items.IndexOf(Cmb_Partida_Origen.Items.FindByValue(Partida));
                            Clave = "";
                            break;

                        case 2:
                            Operacion = 2;
                            for (Cont_For = 0; Cont_For < Codigo.Length; Cont_For++)
                            {
                                if (Codigo.Substring(Cont_For, 1) == "-")//sirve para saber la posicion 
                                {
                                    if (Cont_Posicion == 1)//para la consulta de Fuente de Financiamiento 
                                    {
                                        Fuente_Financiamiento = Clave;
                                        if (Fuente_Financiamiento.Length < 5)
                                        {
                                            Fuente_Financiamiento += " ";
                                        }
                                        Cmb_Fuente_Financiamiento_Destino.SelectedIndex = Cmb_Fuente_Financiamiento_Destino.Items.IndexOf(Cmb_Fuente_Financiamiento_Destino.Items.FindByValue(Fuente_Financiamiento));
                                        Clave = "";
                                    }
                                    if (Cont_Posicion == 2)//para la consulta de Area Funcional
                                    {
                                        Area_Funcional = Clave;
                                       Txt_Area_Destino.Text = Area_Funcional;
                                        Clave = "";
                                    }
                                    if (Cont_Posicion == 3)//para la consulta de programa
                                    {
                                        Programa = Clave;
                                        Cmb_Programa_Destino.SelectedIndex = Cmb_Programa_Destino.Items.IndexOf(Cmb_Programa_Destino.Items.FindByValue(Programa));
                                        Clave = "";
                                    }
                                    if (Cont_Posicion == 4)//para la consulta de Responsable
                                    {
                                        Responsable = Clave;
                                        Cargar_Combo_Consulta_Responsable(Responsable, Operacion);
                                        Cmb_Unidad_Responsable_Destino.SelectedIndex = Cmb_Unidad_Responsable_Destino.Items.IndexOf(Cmb_Unidad_Responsable_Destino.Items.FindByValue(Responsable));
                                        Clave = "";
                                    }

                                    Cont_Posicion++;//incrementa el valor, para asi de esta manera pasar a la siguiente clave 
                                }
                                else
                                {
                                    Clave += Codigo.Substring(Cont_For, 1);
                                }
                            }
                            //para la ultima clave que es partida
                            Partida = Clave;
                            if (Partida.Length < 5)
                            {
                                Partida += " ";
                            }
                            Partida_Capitulo = Partida.Substring(0, 1);
                            Partida_Capitulo += "000 ";
                            Cmb_Capitulo_Destino.SelectedIndex = Cmb_Capitulo_Destino.Items.IndexOf(Cmb_Capitulo_Destino.Items.FindByValue(Partida_Capitulo));
                            
                            Cargar_Combo_Partida(Operacion);

                            Cmb_Partida_Destino.SelectedIndex = Cmb_Partida_Destino.Items.IndexOf(Cmb_Partida_Destino.Items.FindByValue(Partida));
                            Clave = "";
                            break;
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
            /// NOMBRE DE LA FUNCION: Formato_Importe
            /// DESCRIPCION : da formato al importe con los seperadores de miles
            ///               que pertenescan al estatus de generada
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 15-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Formato_Importe()
            {
                String Formato = "";
                int Contador_For;
                String Importe;
                String Miles = "";
                int indice;
                int Multiplo;
                int Residuo;
                try
                {
                    Quita_Formato_Importe();
                    Importe = Txt_Importe.Text;
                    indice = Txt_Importe.Text.Length;
                    Multiplo = indice / 3;
                    Residuo = indice % 3;

                    if (indice <= 3)
                    {
                    }
                    else
                    {
                        for (int Contador_Coma = 0; Contador_Coma <= Multiplo; Contador_Coma++)
                        {
                            if (Contador_Coma == 0)
                            {
                                if (Residuo == 0)
                                {
                                }
                                else
                                {
                                    Formato = Importe.Substring(0, Residuo);
                                    Formato = Formato + ",";
                                }
                            }
                            else
                            {

                                if (Contador_Coma == Multiplo)
                                {
                                    Miles = Importe.Substring(Residuo, 3);
                                    Formato = "" + Formato + Miles;
                                    Txt_Importe.Text = Formato;

                                }
                                else
                                {

                                    Miles = Importe.Substring(Residuo, 3);
                                    Formato = "" + Formato + Miles + ",";
                                    Residuo = Residuo + 3;
                                }
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    throw new Exception("Consultar Movimientos " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Quita_Formato_Importe
            /// DESCRIPCION : quita el formato al importe con los seperadores de miles
            ///               que pertenescan al estatus de generada
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 15-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Quita_Formato_Importe()
            {
                int Indice;
                int Contador_For;
                String Letra = "";
                String Formato = "";
                try
                {

                    Letra = Txt_Importe.Text;
                    Indice = Txt_Importe.Text.Length;
                    for (Contador_For = 0; Contador_For < Indice; Contador_For++)
                    {
                        if (Txt_Importe.Text.Substring(Contador_For, 1) != ",")
                        {
                            Formato = "" + Formato + Letra.Substring(Contador_For, 1);
                        }

                    }
                    Txt_Importe.Text = Formato;

                }
                catch (Exception ex)
                {
                    throw new Exception("Consultar Movimientos " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Movimiento
            /// DESCRIPCION : Consulta los movimientos que estan dadas de alta en la BD
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 17-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Movimiento()
            {
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Consulta = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
                DataTable Dt_Movimiento; //Variable que obtendra los datos de la consulta 

                try
                {
                    Consulta.P_Responsable = Cmb_Unidad_Responsable.SelectedValue;
                    Dt_Movimiento = Consulta.Consultar_Like_Movimiento();
                    Session["Consulta_Movimiento_Presupuestal"] = Dt_Movimiento;
                    Grid_Movimiento.DataSource = (DataTable)Session["Consulta_Movimiento_Presupuestal"];
                    Grid_Movimiento.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Movimiento Presupuestal" + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Movimiento
            /// DESCRIPCION : Consulta los movimientos que estan dadas de alta en la BD
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 17-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Cargar_Grid_Comentario(String Indice)
            {
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Consulta = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
                DataTable Dt_Consulta; //Variable que obtendra los datos de la consulta 
                
                try
                {
                    Consulta.P_No_Solicitud = Indice;
                    Dt_Consulta = Consulta.Consulta_Datos_Comentarios();
                    Session["Consulta_Comentarios"] = Dt_Consulta;
                    Grid_Comentarios.DataSource = (DataTable)Session["Consulta_Comentarios"];
                    Grid_Comentarios.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Movimiento Presupuestal" + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Id_Fuente_Financiamiento
            /// DESCRIPCION : Consulta el id de fuente de financiamiento
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 17-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private String Consulta_Id_Fuente_Financiamiento(String Clave_Fuente_Financiamiento)
            {
                Cls_Cat_SAP_Fuente_Financiamiento_Negocio Rs_Consulta = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio();
                DataTable Dt_Consulta = new DataTable();
                String Calve_Id="";
                try 
                {
                    Rs_Consulta.P_Clave = Clave_Fuente_Financiamiento;
                    Dt_Consulta = Rs_Consulta.Consulta_Fuente_Financiamiento();//se llena el datatable con la consulta   
                    foreach (DataRow Registro in Dt_Consulta.Rows)//se llena el registro para pasar el valor a la caja de texto
                    {
                        Calve_Id = Registro[Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID].ToString();
                        
                    }
 
                }
                catch (Exception ex)
                {
                    throw new Exception("Movimiento Presupuestal" + ex.Message.ToString(), ex);
                }
                return Calve_Id;
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Id_Dependencia
            /// DESCRIPCION : Consulta el id de fuente de financiamiento
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 17-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private String Consulta_Id_Dependencia(String Clave_Dependencia)
            {
                Cls_Cat_Dependencias_Negocio Rs_Consulta = new Cls_Cat_Dependencias_Negocio();
                DataTable Dt_Consulta = new DataTable();
                String Calve_Id = "";
                try
                {
                    Rs_Consulta.P_Clave = Clave_Dependencia;
                    Dt_Consulta = Rs_Consulta.Consulta_Dependencias();//se llena el datatable con la consulta   
                    foreach (DataRow Registro in Dt_Consulta.Rows)//se llena el registro para pasar el valor a la caja de texto
                    {
                        Calve_Id = Registro[Cat_Dependencias.Campo_Dependencia_ID].ToString();
                        
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("Movimiento Presupuestal" + ex.Message.ToString(), ex);
                }
                return Calve_Id;
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Id_Programa
            /// DESCRIPCION : Consulta el id de fuente de financiamiento
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 17-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private String Consulta_Id_Programa(String Clave_Programa)
            {
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Rs_Consulta = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
                DataTable Dt_Consulta = new DataTable();
                String Calve_Id = "";
                try
                {
                    Rs_Consulta.P_Programa = Clave_Programa;
                    Dt_Consulta = Rs_Consulta.Consultar_Programa();//se llena el datatable con la consulta   
                    foreach (DataRow Registro in Dt_Consulta.Rows)//se llena el registro para pasar el valor a la caja de texto
                    {
                        Calve_Id = Registro[Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id].ToString();
                        
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("Movimiento Presupuestal" + ex.Message.ToString(), ex);
                }
                return Calve_Id;
            }

            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Id_Area_Funcional
            /// DESCRIPCION : Consulta el id de fuente de financiamiento
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 17-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private String Consulta_Id_Area_Funcional (String Clave_Area)
            {
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Rs_Consulta = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
                DataTable Dt_Consulta = new DataTable();
                String Calve_Id = "";
                try
                {
                    if (Clave_Area == "")
                    { }
                    else
                    { 
                        Rs_Consulta.P_Area_Funcional = Clave_Area;
                        Dt_Consulta = Rs_Consulta.Consultar_Area_Funciona();//se llena el datatable con la consulta   
                        foreach (DataRow Registro in Dt_Consulta.Rows)//se llena el registro para pasar el valor a la caja de texto
                        {
                            Calve_Id = Registro[Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID].ToString();
                           
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("Movimiento Presupuestal" + ex.Message.ToString(), ex);
                }
                return Calve_Id;
            }

            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Id_Partida
            /// DESCRIPCION : Consulta el id de fuente de financiamiento
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 17-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private String Consulta_Id_Partida(String Clave_Partida)
            {
                Cls_Cat_Com_Partidas_Negocio Rs_Consulta = new Cls_Cat_Com_Partidas_Negocio();
                DataTable Dt_Consulta = new DataTable();
                String Calve_Id = "";
                try
                {
                    Rs_Consulta.P_Clave = Clave_Partida;
                    Dt_Consulta = Rs_Consulta.Consulta_Datos_Partidas();//se llena el datatable con la consulta   
                    foreach (DataRow Registro in Dt_Consulta.Rows)//se llena el registro para pasar el valor a la caja de texto
                    {
                        Calve_Id = Registro[Cat_Com_Partidas.Campo_Partida_ID].ToString();
                        
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("Movimiento Presupuestal" + ex.Message.ToString(), ex);
                }
            return Calve_Id;
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
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Rs_Alta_Movimiento = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";
                    if (Btn_Nuevo.ToolTip == "Nuevo")
                    {
                        Limpiar_Controles();
                        Cargar_Combo_Responsable();
                        Cargar_Combo_Financiamiento(1);
                        Cargar_Combo_Financiamiento(2);
                        Habilitar_Visible(true);
                        Grid_Comentarios.DataSource = null;
                        Grid_Comentarios.DataBind();
                        Cmb_Estatus.SelectedIndex = 1;
                        

                        Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                        Cmb_Estatus.Enabled = false;
                    }
                    else
                    {

                        //se validaran los datos para saber si las sumas son iguales
                        if (Validar_Datos())
                        {
                            Rs_Alta_Movimiento.P_Codigo_Programatico_De = Txt_Codigo1.Text.ToUpper();//informacion del origen
                            Rs_Alta_Movimiento.P_Codigo_Programatico_Al = Txt_Codigo2.Text.ToUpper();//informacion del destino
                            Quita_Formato_Importe();
                            Rs_Alta_Movimiento.P_Monto = Txt_Importe.Text;
                            Rs_Alta_Movimiento.P_Justificacion = Txt_Justificacion.Text;
                            Rs_Alta_Movimiento.P_Estatus = Cmb_Estatus.SelectedValue;
                            Rs_Alta_Movimiento.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                            Rs_Alta_Movimiento.P_Tipo_Operacion = Cmb_Operacion.SelectedValue;
                            Rs_Alta_Movimiento.P_Origen_Fuente_Financiamiento_Id = Consulta_Id_Fuente_Financiamiento(Cmb_Fuente_Financiamiento_Origen.SelectedValue);
                            Rs_Alta_Movimiento.P_Origen_Area_Funcional_Id = Consulta_Id_Area_Funcional(Txt_Area_Origen.Text);
                            Rs_Alta_Movimiento.P_Origen_Programa_Id = Consulta_Id_Programa(Cmb_Programa_Origen.SelectedValue);
                            Rs_Alta_Movimiento.P_Origen_Dependencia_Id = Consulta_Id_Dependencia(Cmb_Unidad_Responsable_Origen.SelectedValue);
                            Rs_Alta_Movimiento.P_Origen_Partida_Id = Consulta_Id_Partida(Cmb_Partida_Origen.SelectedValue);
                            Rs_Alta_Movimiento.P_Destino_Fuente_Financiamiento_Id = Consulta_Id_Fuente_Financiamiento(Cmb_Fuente_Financiamiento_Destino.SelectedValue);
                            Rs_Alta_Movimiento.P_Destino_Area_Funcional_Id = Consulta_Id_Area_Funcional(Txt_Area_Destino.Text);
                            Rs_Alta_Movimiento.P_Destino_Programa_Id = Consulta_Id_Programa(Cmb_Programa_Destino.SelectedValue);
                            Rs_Alta_Movimiento.P_Destino_Dependencia_Id = Consulta_Id_Dependencia(Cmb_Unidad_Responsable_Destino.SelectedValue);
                            Rs_Alta_Movimiento.P_Destino_Partida_Id = Consulta_Id_Partida(Cmb_Partida_Destino.SelectedValue);
                          
                            Rs_Alta_Movimiento.Alta_Movimiento();
                            //Rs_Alta_Movimiento.Alta_Comentario();
                            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Movimiento Presupuestal", "alert('El Alta del Movimiento Presupuestal fue Exitosa');", true);
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
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Rs_Modificar = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";
                    if (Btn_Modificar.ToolTip == "Modificar")
                    {
                        if (Txt_No_Solicitud.Text == "")
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "No se encuentra ningun registro seleccionado.<br> *Pruebe seleccionando algun Movimiento de la tabla.";
                        }
                        else
                        {
                            Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                            if (Cmb_Estatus.SelectedValue == "AUTORIZADA")
                            {
                                Cmb_Estatus.Enabled = false;
                            }
                            else 
                            {
                                if (Cmb_Estatus.Items.Count == 4)
                                {
                                    Cmb_Estatus.Items.RemoveAt(3);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Validar_Datos())
                        {
                            if (Cmb_Estatus.SelectedValue == "AUTORIZADA")
                            {
                                Lbl_Mensaje_Error.Text = "No se puede modificar este registro porque contiene Estatus de AUTORIZADA";
                                Lbl_Mensaje_Error.Visible = true;
                                Img_Error.Visible = true;
                            }
                            else
                            {
                                //pasan los datos a la capa de negocio
                                Rs_Modificar.P_No_Solicitud = Txt_No_Solicitud.Text;
                                Rs_Modificar.P_Codigo_Programatico_Al = Txt_Codigo1.Text;
                                Rs_Modificar.P_Codigo_Programatico_De = Txt_Codigo2.Text;
                                Quita_Formato_Importe();
                                Rs_Modificar.P_Monto = Txt_Importe.Text;
                                Rs_Modificar.P_Tipo_Operacion = Cmb_Operacion.SelectedValue;
                                Rs_Modificar.P_Justificacion = Txt_Justificacion.Text;
                                Rs_Modificar.P_Estatus = Cmb_Estatus.SelectedValue;
                                Rs_Modificar.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                                Rs_Modificar.P_Origen_Fuente_Financiamiento_Id = Consulta_Id_Fuente_Financiamiento(Cmb_Fuente_Financiamiento_Origen.SelectedValue);
                                Rs_Modificar.P_Origen_Area_Funcional_Id = Consulta_Id_Area_Funcional(Txt_Area_Origen.Text);
                                Rs_Modificar.P_Origen_Programa_Id = Consulta_Id_Programa(Cmb_Programa_Origen.SelectedValue);
                                Rs_Modificar.P_Origen_Dependencia_Id = Consulta_Id_Dependencia(Cmb_Unidad_Responsable_Origen.SelectedValue);
                                Rs_Modificar.P_Origen_Partida_Id = Consulta_Id_Partida(Cmb_Partida_Origen.SelectedValue);
                                Rs_Modificar.P_Destino_Fuente_Financiamiento_Id = Consulta_Id_Fuente_Financiamiento(Cmb_Fuente_Financiamiento_Destino.SelectedValue);
                                Rs_Modificar.P_Destino_Area_Funcional_Id = Consulta_Id_Area_Funcional(Txt_Area_Destino.Text);
                                Rs_Modificar.P_Destino_Programa_Id = Consulta_Id_Programa(Cmb_Programa_Destino.SelectedValue);
                                Rs_Modificar.P_Destino_Dependencia_Id = Consulta_Id_Dependencia(Cmb_Unidad_Responsable_Destino.SelectedValue);
                                Rs_Modificar.P_Destino_Partida_Id = Consulta_Id_Partida(Cmb_Partida_Destino.SelectedValue);
                                Rs_Modificar.Modificar_Movimiento();
                                Cmb_Estatus.Items.RemoveAt(2);
                                Cmb_Estatus.Items.Insert(2, "RECHAZADA");
                                Inicializa_Controles();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Movimiento Presupuestal", "alert('La Modificacion del Movimiento Presupuestal fue Exitosa');", true);
                            }
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
            ///DESCRIPCIÓN: cambiara el estatus a cancelada
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  07/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
            {
                Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio Rs_Modificar_Comentario = new Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio();
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Rs_Eliminar = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
                DataTable Dt_Estatus = new DataTable();
                String Estatus;
                try
                {
                   
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";
                    
                    if (!string.IsNullOrEmpty(Txt_No_Solicitud.Text.Trim()))
                    {
                            if (Cmb_Estatus.SelectedValue == "AUTORIZADA")
                            {
                                Lbl_Mensaje_Error.Text = "No se puede modificar este registro porque contiene Estatus de AUTORIZADA";
                                Lbl_Mensaje_Error.Visible = true;
                                Img_Error.Visible = true;
                            }
                            else
                            {
                                //pasan los datos a la capa de negocio
                                Rs_Eliminar.P_No_Solicitud = Txt_No_Solicitud.Text;
                                Rs_Eliminar.P_Codigo_Programatico_Al = Txt_Codigo1.Text;
                                Rs_Eliminar.P_Codigo_Programatico_De = Txt_Codigo2.Text;
                                Quita_Formato_Importe();
                                Rs_Eliminar.P_Monto = Txt_Importe.Text;
                                Rs_Eliminar.P_Tipo_Operacion = Cmb_Operacion.SelectedValue;
                                Rs_Eliminar.P_Justificacion = Txt_Justificacion.Text;
                                Rs_Eliminar.P_Estatus = "CANCELADA";
                                Rs_Eliminar.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                                Rs_Eliminar.P_Origen_Fuente_Financiamiento_Id = Consulta_Id_Fuente_Financiamiento(Cmb_Fuente_Financiamiento_Origen.SelectedValue);
                                Rs_Eliminar.P_Origen_Area_Funcional_Id = Consulta_Id_Area_Funcional(Txt_Area_Origen.Text);
                                Rs_Eliminar.P_Origen_Programa_Id = Consulta_Id_Programa(Cmb_Programa_Origen.SelectedValue);
                                Rs_Eliminar.P_Origen_Dependencia_Id = Consulta_Id_Dependencia(Cmb_Unidad_Responsable_Origen.SelectedValue);
                                Rs_Eliminar.P_Origen_Partida_Id = Consulta_Id_Partida(Cmb_Partida_Origen.SelectedValue);
                                Rs_Eliminar.P_Destino_Fuente_Financiamiento_Id = Consulta_Id_Fuente_Financiamiento(Cmb_Fuente_Financiamiento_Destino.SelectedValue);
                                Rs_Eliminar.P_Destino_Area_Funcional_Id = Consulta_Id_Area_Funcional(Txt_Area_Destino.Text);
                                Rs_Eliminar.P_Destino_Programa_Id = Consulta_Id_Programa(Cmb_Programa_Destino.SelectedValue);
                                Rs_Eliminar.P_Destino_Dependencia_Id = Consulta_Id_Dependencia(Cmb_Unidad_Responsable_Destino.SelectedValue);
                                Rs_Eliminar.P_Destino_Partida_Id = Consulta_Id_Partida(Cmb_Partida_Destino.SelectedValue);
                                Rs_Eliminar.Modificar_Movimiento();

                               
                                Inicializa_Controles();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Movimiento Presupuestal", "alert('La cancelación fue Exitosa');", true);
                            }
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Seleccione algun movimiento de la tabla";
                        }//else de validar_Datos
                    /*if (!string.IsNullOrEmpty(Txt_No_Solicitud.Text.Trim()))
                    {
                        Eliminar.P_No_Solicitud = Txt_No_Solicitud.Text;
                        Dt_Estatus = Eliminar.Consulta_Movimiento();
                        foreach (DataRow Registro in Dt_Estatus.Rows)
                        {
                            Estatus = (Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Estatus].ToString());
                            if (Estatus == "AUTORIZADA")
                            {
                                Lbl_Mensaje_Error.Text = "No se puede Eliminar este registro porque contiene Estatus de AUTORIZADO";
                                Lbl_Mensaje_Error.Visible = true;
                                Img_Error.Visible = true;
                            }
                            else
                            {
                                Eliminar.P_No_Solicitud = Txt_No_Solicitud.Text;
                                Eliminar.Eliminar_Movimiento(); //Elimina el movimiento presupuestal que fue seleccionada por el usuario
                                Inicializa_Controles();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Movimientos Presupuestal", "alert('Se Elimino correctamente la información');", true);
                                
                            }
                        }*/
                   
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
            ///FECHA_CREO:  15/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
            {
                Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio Rs_Buscar_Traspaso = new Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio();
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Rs_Buscar_Fecha = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
                DataTable Dt_Busqueda = new DataTable();
                Boolean Resultado_Numerico = false;//guardara si el resultado es numero o no
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";
                    Habilitar_Controles(""); 
                    Cargar_Combo_Responsable();

                    if (!string.IsNullOrEmpty(Txt_Busqueda.Text.Trim()))
                    {
                        Resultado_Numerico=Es_Numero(Txt_Busqueda.Text.Trim());
                        if (Resultado_Numerico == true)
                        {
                            Rs_Buscar_Fecha.P_No_Solicitud = Txt_Busqueda.Text.ToUpper().Trim();
                            Rs_Buscar_Fecha.P_Responsable = Cmb_Unidad_Responsable.SelectedValue;
                            Dt_Busqueda = Rs_Buscar_Fecha.Consulta_Movimiento_Btn_Busqueda();
                            Grid_Movimiento.DataSource = Dt_Busqueda;
                            Grid_Movimiento.DataBind();
                            Grid_Movimiento.SelectedIndex = -1;
                        }
                        //Consulta_Movimiento_Fecha
                        
                    }
                    else
                    {
                        Rs_Buscar_Fecha.P_Responsable = Cmb_Unidad_Responsable.SelectedValue;

                        if (Txt_Fecha_Inicial.Text == Txt_Fecha_Final.Text)
                        {
                            Rs_Buscar_Fecha.P_Fecha_Inicio = String.Format("{0:dd-MMM-yyyy}", Txt_Fecha_Inicial.Text);
                        }
                        else
                        {
                            Rs_Buscar_Fecha.P_Fecha_Inicio = String.Format("{0:dd-MMM-yyyy}", Txt_Fecha_Inicial.Text);
                            Rs_Buscar_Fecha.P_Fecha_Final = String.Format("{0:dd-MMM-yyyy}", Txt_Fecha_Final.Text);
                        }
                        Dt_Busqueda = Rs_Buscar_Fecha.Consulta_Movimiento_Fecha();
                        Grid_Movimiento.DataSource = Dt_Busqueda;
                        Grid_Movimiento.DataBind();
                        Grid_Movimiento.SelectedIndex = -1;
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
                    if ((Btn_Salir.ToolTip == "Salir") && (Div_Grid_Movimientos.Visible == true))
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                    else if (Btn_Salir.ToolTip == "Cancelar")
                    {
                        Inicializa_Controles(); //Habilita los controles para la siguiente operación del usuario en el catálogo
                    }
                    else
                    {
                        Inicializa_Controles(); //Habilita los controles para la siguiente operación del usuario en el catálogo
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
            ///FECHA_CREO:  14/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cargar_Combo_Responsable()
            {
                
                Cls_Cat_Dependencias_Negocio Cls_Respondable = new Cls_Cat_Dependencias_Negocio();
                DataTable Dt_Responsable = new DataTable();
                DataTable Dt_Indice = new DataTable();
                String Clave;
                try
                {
                    Cmb_Unidad_Responsable_Origen.Items.Clear();
                    Cmb_Unidad_Responsable_Destino.Items.Clear();
                    Cmb_Unidad_Responsable.Items.Clear();
                    
                    Dt_Responsable = Cls_Respondable.Consulta_Dependencias();
                    Cmb_Unidad_Responsable_Origen.Items.Clear();
                    Cmb_Unidad_Responsable_Destino.Items.Clear();

                    Cmb_Unidad_Responsable_Origen.Items.Insert(0, "< SELECCIONE RESPONSABLE >");
                    Cmb_Unidad_Responsable_Origen.Enabled = false;

                    Cmb_Unidad_Responsable_Destino.Items.Insert(0, "< SELECCIONE RESPONSABLE >");
                    Cmb_Unidad_Responsable_Destino.Enabled = false;

                    Cmb_Unidad_Responsable.Items.Insert(0, "< SELECCIONE RESPONSABLE >");
                    Cmb_Unidad_Responsable.Enabled = false;

                    //foreach (DataRow Registro in Dt_Responsable.Rows)
                    //{
                    //para origen
                    Cmb_Unidad_Responsable_Origen.DataSource = Dt_Responsable;
                    Cmb_Unidad_Responsable_Origen.DataValueField = Cat_Dependencias.Campo_Clave;
                    Cmb_Unidad_Responsable_Origen.DataTextField = Cat_Dependencias.Campo_Nombre;
                    Cmb_Unidad_Responsable_Origen.DataBind();
                         
                    //para destino
                    Cmb_Unidad_Responsable_Destino.DataSource = Dt_Responsable;
                    Cmb_Unidad_Responsable_Destino.DataValueField = Cat_Dependencias.Campo_Clave;
                    Cmb_Unidad_Responsable_Destino.DataTextField = Cat_Dependencias.Campo_Nombre;
                    Cmb_Unidad_Responsable_Destino.DataBind();

                    //para la inicial
                    Cmb_Unidad_Responsable.DataSource = Dt_Responsable;
                    Cmb_Unidad_Responsable.DataValueField = Cat_Dependencias.Campo_Clave;
                    Cmb_Unidad_Responsable.DataTextField = Cat_Dependencias.Campo_Nombre;
                    Cmb_Unidad_Responsable.DataBind(); 
                   // }
                    Cls_Respondable.P_Dependencia_ID=  Cls_Sessiones.Dependencia_ID_Empleado;
                    Dt_Indice = Cls_Respondable.Consulta_Dependencias();

                    foreach (DataRow Registro1 in Dt_Indice.Rows)
                    {
                        //para el area funcional obtenida de la unidad responsable(Dependencia)
                        Txt_Area_Origen.Text= (Registro1[Cat_Dependencias.Campo_Area_Funcional_ID].ToString());
                        Txt_Area_Destino.Text = (Registro1[Cat_Dependencias.Campo_Area_Funcional_ID].ToString());
                        Clave = (Registro1[Cat_Dependencias.Campo_Clave].ToString());
                        Cmb_Unidad_Responsable_Origen.SelectedIndex = Cmb_Unidad_Responsable_Origen.Items.IndexOf(Cmb_Unidad_Responsable_Origen.Items.FindByValue(Clave));
                        Cmb_Unidad_Responsable_Destino.SelectedIndex = Cmb_Unidad_Responsable_Destino.Items.IndexOf(Cmb_Unidad_Responsable_Destino.Items.FindByValue(Clave));
                        Cmb_Unidad_Responsable.SelectedIndex = Cmb_Unidad_Responsable.Items.IndexOf(Cmb_Unidad_Responsable.Items.FindByValue(Clave));
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
            ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Responsable
            ///DESCRIPCIÓN: Cargara todos los responsables dentro del combo 
            ///             (Proviene del metodo Inicializa_Controles())
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  14/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cargar_Combo_Consulta_Responsable(String Clave, int Operacion)
            {

                Cls_Cat_Dependencias_Negocio Cls_Respondable = new Cls_Cat_Dependencias_Negocio();
                DataTable Dt_Responsable = new DataTable();
                DataTable Dt_Area = new DataTable();
                try
                {
                    Cls_Respondable.P_Clave = Clave;
                    Dt_Responsable = Cls_Respondable.Consulta_Dependencias();

                    foreach (DataRow Registro in Dt_Responsable.Rows)
                    {
                        switch (Operacion)
                        {
                            case 1:

                                
                                Cmb_Unidad_Responsable_Origen.SelectedIndex = Cmb_Unidad_Responsable_Origen.Items.IndexOf(Cmb_Unidad_Responsable_Origen.Items.FindByValue(Clave));
                                break;

                            case 2:
                                //para destino
                                
                                Cmb_Unidad_Responsable_Destino.SelectedIndex = Cmb_Unidad_Responsable_Destino.Items.IndexOf(Cmb_Unidad_Responsable_Destino.Items.FindByValue(Clave));
                                break;
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
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Financiamiento
            ///DESCRIPCIÓN: Cargara todos las fuentes de financiamiento dentro del combo 
            ///PARAMETROS: int Parametros.- Sirve para saber cual es el combo que va a cargar
            ///             si el de origen o destino
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  07/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cargar_Combo_Financiamiento(int Operacion)
            {
                Cls_Cat_SAP_Fuente_Financiamiento_Negocio Cls_Financiamiento = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio();
                DataTable Dt_Financinamiento = new DataTable();
                try
                {
                    switch (Operacion)
                    {
                        case 1:

                            Cmb_Fuente_Financiamiento_Origen.Items.Clear();
                            Dt_Financinamiento = Cls_Financiamiento.Consulta_Datos_Fuente_Financiamiento();
                            Cmb_Fuente_Financiamiento_Origen.DataSource = Dt_Financinamiento;
                            Cmb_Fuente_Financiamiento_Origen.DataValueField = Cat_SAP_Fuente_Financiamiento.Campo_Clave;
                            Cmb_Fuente_Financiamiento_Origen.DataTextField = Cat_SAP_Fuente_Financiamiento.Campo_Descripcion;
                            Cmb_Fuente_Financiamiento_Origen.DataBind();
                            Cmb_Fuente_Financiamiento_Origen.Items.Insert(0, "< SELECCIONE FUENTE FINANCIAMIENTO >");
                            Cmb_Fuente_Financiamiento_Origen.SelectedIndex = 0;
                            break;

                        case 2:
                            Cmb_Fuente_Financiamiento_Destino.Items.Clear();
                            Cls_Financiamiento.P_Dependencia_ID = Cmb_Unidad_Responsable_Origen.SelectedValue;
                            Dt_Financinamiento = Cls_Financiamiento.Consulta_Datos_Fuente_Financiamiento();
                            Cmb_Fuente_Financiamiento_Destino.DataSource = Dt_Financinamiento;
                            Cmb_Fuente_Financiamiento_Destino.DataValueField = Cat_SAP_Fuente_Financiamiento.Campo_Clave;
                            Cmb_Fuente_Financiamiento_Destino.DataTextField = Cat_SAP_Fuente_Financiamiento.Campo_Descripcion;
                            Cmb_Fuente_Financiamiento_Destino.DataBind();
                            Cmb_Fuente_Financiamiento_Destino.Items.Insert(0, "< SELECCIONE FUENTE FINANCIAMIENTO >");
                            Cmb_Fuente_Financiamiento_Destino.SelectedIndex = 0;
                            break;
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
            ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Programa
            ///DESCRIPCIÓN: Cargara todos los Programas dentro del combo 
            ///             (Proviene del metodo Inicializa_Controles())
            ///PARAMETROS: int Parametros.- Sirve para saber cual es el combo que va a cargar
            ///             si el de origen o destino
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  07/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cargar_Combo_Programa(int Operacion)
            {
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Cls_Programa = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
                DataTable Dt_Programa = new DataTable();
                try
                {
                    switch (Operacion)
                    {
                        case 1:

                            Cmb_Programa_Origen.Items.Clear();
                            Cls_Programa.P_Fuente_Financiera = Cmb_Fuente_Financiamiento_Origen.SelectedValue;
                            Dt_Programa = Cls_Programa.Consultar_Programa();
                            Cmb_Programa_Origen.DataSource = Dt_Programa;
                            Cmb_Programa_Origen.DataValueField = Cat_Sap_Proyectos_Programas.Campo_Clave;
                            Cmb_Programa_Origen.DataTextField = Cat_Sap_Proyectos_Programas.Campo_Nombre;
                            Cmb_Programa_Origen.DataBind();
                            Cmb_Programa_Origen.Items.Insert(0, "< SELECCIONE PROGRAMA >");
                            Cmb_Programa_Origen.SelectedIndex = 0;
                            break;

                        case 2:
                            Cmb_Programa_Destino.Items.Clear();
                            Cls_Programa.P_Fuente_Financiera = Cmb_Programa_Destino.SelectedValue;
                            Dt_Programa = Cls_Programa.Consultar_Programa();
                            Cmb_Programa_Destino.DataSource = Dt_Programa;
                            Cmb_Programa_Destino.DataValueField = Cat_Sap_Proyectos_Programas.Campo_Clave;
                            Cmb_Programa_Destino.DataTextField = Cat_Sap_Proyectos_Programas.Campo_Nombre;
                            Cmb_Programa_Destino.DataBind();
                            Cmb_Programa_Destino.Items.Insert(0, "< SELECCIONE PROGRAMA >");
                            Cmb_Programa_Destino.SelectedIndex = 0;
                            break;
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
            /// NOMBRE DE LA FUNCION: Cargar_Combo_Capitulos
            /// DESCRIPCION: Consulta los Capítulos dados de alta en la base de datos
            ///PARAMETROS: int Parametros.- Sirve para saber cual es el combo que va a cargar
            ///             si el de origen o destino
            /// CREO: Hugo Enrique Ramírez Aguilera
            /// FECHA_CREO: 28-Feb-2011
            /// MODIFICO:
            /// FECHA_MODIFICO:
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Cargar_Combo_Capitulos(int Operacion)
            {
                DataTable Dt_Capitulos; //Variable que obtendrá los datos de la consulta        
                Cls_Cat_Com_Partidas_Negocio Rs_Consulta_Cat_SAP_Capitulos = new Cls_Cat_Com_Partidas_Negocio(); //Variable de conexión hacia la capa de Negocios

                try
                {
                    switch (Operacion)
                    {
                        case 1:
                            Cmb_Capitulo_Origen.Items.Clear();
                            Dt_Capitulos = Rs_Consulta_Cat_SAP_Capitulos.Consulta_Capitulos(); //Consulta todos los capítulos que estan dadas de alta en la BD
                            Cmb_Capitulo_Origen.DataSource = Dt_Capitulos;
                            Cmb_Capitulo_Origen.DataValueField = Cat_SAP_Capitulos.Campo_Clave;
                            Cmb_Capitulo_Origen.DataTextField = "CLAVE_DESCRIPCION";
                            Cmb_Capitulo_Origen.DataBind();
                            Cmb_Capitulo_Origen.Items.Insert(0, "----- < SELECCIONE > -----");
                            Cmb_Capitulo_Origen.SelectedIndex = 0;
                            break;

                        case 2:
                            Cmb_Capitulo_Destino.Items.Clear();
                            Dt_Capitulos = Rs_Consulta_Cat_SAP_Capitulos.Consulta_Capitulos(); //Consulta todos los capítulos que estan dadas de alta en la BD
                            Cmb_Capitulo_Destino.DataSource = Dt_Capitulos;
                            Cmb_Capitulo_Destino.DataValueField = Cat_SAP_Capitulos.Campo_Clave;
                            Cmb_Capitulo_Destino.DataTextField = "CLAVE_DESCRIPCION";
                            Cmb_Capitulo_Destino.DataBind();
                            Cmb_Capitulo_Destino.Items.Insert(0, "----- < SELECCIONE > -----");
                            Cmb_Capitulo_Destino.SelectedIndex = 0;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Cargar_Combo_Capitulos " + ex.Message.ToString(), ex);
                }
            }
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
            protected void Cargar_Combo_Partida(int Operacion)
            {
                Cls_Cat_Com_Partidas_Negocio Cls_Partida = new Cls_Cat_Com_Partidas_Negocio();
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Rs_Partida_Movimiento = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
                DataTable Dt_Partida = new DataTable();
                String Letra="";
                try
                {
                    
                    switch (Operacion)
                    {
                        case 1:
                            Cmb_Partida_Origen.Items.Clear();
                            
                            
                            
                            Letra= Cmb_Capitulo_Origen.SelectedValue;
                            Letra = Letra.Substring(0, 1);
                            Cls_Partida.P_Capitulo_ID = Letra;
                            Rs_Partida_Movimiento.P_Partida = Letra;
                            Dt_Partida = Rs_Partida_Movimiento.Consulta_Datos_Partidas();
                            Cmb_Partida_Origen.DataSource = Dt_Partida;
                            Cmb_Partida_Origen.DataValueField = Cat_Com_Partidas.Campo_Clave;
                            Cmb_Partida_Origen.DataTextField = Cat_Com_Partidas.Campo_Nombre;
                            Cmb_Partida_Origen.DataBind();
                            Cmb_Partida_Origen.Items.Insert(0, "< SELECCIONE PARTIDA >");
                            Cmb_Partida_Origen.SelectedIndex = 0;
                            break;

                        case 2:
                            Letra = Cmb_Capitulo_Destino.SelectedValue;
                            Letra = Letra.Substring(0, 1);
                            Cls_Partida.P_Capitulo_ID = Letra;
                            Rs_Partida_Movimiento.P_Partida = Letra;
                            Dt_Partida = Rs_Partida_Movimiento.Consulta_Datos_Partidas();
                            Cmb_Partida_Destino.Items.Clear();
                            Cmb_Partida_Destino.DataSource = Dt_Partida;
                            Cmb_Partida_Destino.DataValueField = Cat_Com_Partidas.Campo_Clave;
                            Cmb_Partida_Destino.DataTextField = Cat_Com_Partidas.Campo_Nombre;
                            Cmb_Partida_Destino.DataBind();
                            Cmb_Partida_Destino.Items.Insert(0, "< SELECCIONE PARTIDA >");
                            Cmb_Partida_Destino.SelectedIndex = 0;
                            break;
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
            ///NOMBRE DE LA FUNCIÓN: Cmb_Fuente_Financiamiento_Origen_SelectedIndexChanged
            ///DESCRIPCIÓN: habilita el siguiente combo y pasa la informacion de la clave
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  08/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cmb_Fuente_Financiamiento_Origen_SelectedIndexChanged(object sender, EventArgs e)
            {
                try
                {
                    String Codigo_Presupuesto = "";
                    if ((Cmb_Unidad_Responsable_Origen.SelectedIndex == 0) && (Cmb_Fuente_Financiamiento_Origen.SelectedIndex == 0)
                        && (Cmb_Programa_Origen.SelectedIndex == 0) && (Cmb_Capitulo_Origen.SelectedIndex == 0) && (Cmb_Partida_Origen.SelectedIndex == 0))
                    {
                    }
                    else
                    {
                        //cargar el codigo programatico
                        Codigo_Presupuesto = Cmb_Fuente_Financiamiento_Origen.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Txt_Area_Origen.Text;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Programa_Origen.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Unidad_Responsable_Origen.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Partida_Origen.SelectedValue;
                        Txt_Codigo1.Text = Codigo_Presupuesto;
                    }

                    if (Cmb_Programa_Origen.SelectedIndex < 0)
                    {
                        int Operacion = 1;//sirve para indicar que pertenese el llenado a origen

                        Cargar_Combo_Programa(Operacion);
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
            ///NOMBRE DE LA FUNCIÓN: Cmb_Programa_Origen_SelectedIndexChanged
            ///DESCRIPCIÓN: habilita el siguiente combo y pasa la informacion de la clave
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  08/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cmb_Programa_Origen_SelectedIndexChanged(object sender, EventArgs e)
            {
                try
                {
                    String Codigo_Presupuesto = "";
                    if ((Cmb_Unidad_Responsable_Origen.SelectedIndex == 0) && (Cmb_Fuente_Financiamiento_Origen.SelectedIndex == 0)
                        && (Cmb_Programa_Origen.SelectedIndex == 0) && (Cmb_Capitulo_Origen.SelectedIndex == 0) && (Cmb_Partida_Origen.SelectedIndex == 0))
                    {
                    }
                    else
                    {
                        //cargar el codigo programatico
                        Codigo_Presupuesto = Cmb_Fuente_Financiamiento_Origen.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Txt_Area_Origen.Text;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Programa_Origen.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Unidad_Responsable_Origen.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Partida_Origen.SelectedValue;
                        Txt_Codigo1.Text = Codigo_Presupuesto;
                    }

                    if (Cmb_Capitulo_Origen.SelectedIndex < 0)
                    {
                        int Operacion = 1;//sirve para indicar que pertenese el llenado a origen

                        Cargar_Combo_Capitulos(Operacion);
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
            ///NOMBRE DE LA FUNCIÓN: Cmb_Capitulo_Origen_SelectedIndexChanged
            ///DESCRIPCIÓN: habilita el siguiente combo y pasa la informacion de la clave
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  08/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cmb_Capitulo_Origen_SelectedIndexChanged(object sender, EventArgs e)
            {
                try
                {
                    String Codigo_Presupuesto = "";
                    if ((Cmb_Unidad_Responsable_Origen.SelectedIndex == 0) && (Cmb_Fuente_Financiamiento_Origen.SelectedIndex == 0)
                        && (Cmb_Programa_Origen.SelectedIndex == 0) && (Cmb_Capitulo_Origen.SelectedIndex == 0) && (Cmb_Partida_Origen.SelectedIndex == 0))
                    {
                    }
                    else
                    {
                        //cargar el codigo programatico
                        Codigo_Presupuesto = Cmb_Fuente_Financiamiento_Origen.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Txt_Area_Origen.Text;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Programa_Origen.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Unidad_Responsable_Origen.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Partida_Origen.SelectedValue;
                        Txt_Codigo1.Text = Codigo_Presupuesto;
                    }

                    
                    int Operacion = 1;//sirve para indicarle que pertenese el llenado a origen

                    Cargar_Combo_Partida(Operacion);
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cmb_Partida_Origen_SelectedIndexChanged
            ///DESCRIPCIÓN: genera el codigo programatico
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  08/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cmb_Partida_Origen_SelectedIndexChanged(object sender, EventArgs e)
            {
                try
                {

                    String Codigo_Presupuesto="";
                    if ((Cmb_Unidad_Responsable_Origen.SelectedIndex == 0) && (Cmb_Fuente_Financiamiento_Origen.SelectedIndex == 0)
                        && (Cmb_Programa_Origen.SelectedIndex == 0) && (Cmb_Capitulo_Origen.SelectedIndex == 0) && (Cmb_Partida_Origen.SelectedIndex == 0))
                    { 
                    }
                    else 
                    {
                        //cargar el codigo programatico
                        Codigo_Presupuesto = Cmb_Fuente_Financiamiento_Origen.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Txt_Area_Origen.Text;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Programa_Origen.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Unidad_Responsable_Origen.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Partida_Origen.SelectedValue;
                        Txt_Codigo1.Text = Codigo_Presupuesto;
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
            ///NOMBRE DE LA FUNCIÓN: Cmb_Fuente_Financiamiento_Destino_SelectedIndexChanged
            ///DESCRIPCIÓN: habilita el siguiente combo y pasa la informacion de la clave
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  08/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cmb_Fuente_Financiamiento_Destino_SelectedIndexChanged(object sender, EventArgs e)
            {
                try
                {
                    String Codigo_Presupuesto = "";
                    if ((Cmb_Unidad_Responsable_Destino.SelectedIndex == 0) && (Cmb_Fuente_Financiamiento_Destino.SelectedIndex == 0)
                        && (Cmb_Programa_Destino.SelectedIndex == 0) && (Cmb_Capitulo_Destino.SelectedIndex == 0) && (Cmb_Partida_Destino.SelectedIndex == 0))
                    {
                    }
                    else
                    {
                        //cargar el codigo programatico
                        Codigo_Presupuesto = Cmb_Fuente_Financiamiento_Destino.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Txt_Area_Destino.Text;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Programa_Destino.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Unidad_Responsable_Destino.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Partida_Destino.SelectedValue;
                        Txt_Codigo2.Text = Codigo_Presupuesto;
                    }

                    if (Cmb_Programa_Destino.SelectedIndex < 0)
                    {
                        int Operacion = 2;//sirve para indicar que pertenese el llenado a origen

                        Cargar_Combo_Programa(Operacion);
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
            ///NOMBRE DE LA FUNCIÓN: Cmb_Programa_Destino_SelectedIndexChanged
            ///DESCRIPCIÓN: habilita el siguiente combo y pasa la informacion de la clave
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  08/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cmb_Programa_Destino_SelectedIndexChanged(object sender, EventArgs e)
            {
                try
                {
                    String Codigo_Presupuesto = "";
                    if ((Cmb_Unidad_Responsable_Destino.SelectedIndex == 0) && (Cmb_Fuente_Financiamiento_Destino.SelectedIndex == 0)
                        && (Cmb_Programa_Destino.SelectedIndex == 0) && (Cmb_Capitulo_Destino.SelectedIndex == 0) && (Cmb_Partida_Destino.SelectedIndex == 0))
                    {
                    }
                    else
                    {
                        //cargar el codigo programatico
                        Codigo_Presupuesto = Cmb_Fuente_Financiamiento_Destino.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Txt_Area_Destino.Text;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Programa_Destino.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Unidad_Responsable_Destino.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Partida_Destino.SelectedValue;
                        Txt_Codigo2.Text = Codigo_Presupuesto;
                    }
                    if (Cmb_Capitulo_Destino.SelectedIndex < 0)
                    {
                        int Operacion = 2;//sirve para indicar que pertenese el llenado a origen

                        Cargar_Combo_Capitulos(Operacion);
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
            ///NOMBRE DE LA FUNCIÓN: Cmb_Capitulo_destino_SelectedIndexChanged
            ///DESCRIPCIÓN: habilita el siguiente combo y pasa la informacion de la clave
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  08/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cmb_Capitulo_Destino_SelectedIndexChanged(object sender, EventArgs e)
            {
                try
                {
                    String Codigo_Presupuesto = "";
                    if ((Cmb_Unidad_Responsable_Destino.SelectedIndex == 0) && (Cmb_Fuente_Financiamiento_Destino.SelectedIndex == 0)
                        && (Cmb_Programa_Destino.SelectedIndex == 0) && (Cmb_Capitulo_Destino.SelectedIndex == 0) && (Cmb_Partida_Destino.SelectedIndex == 0))
                    {
                    }
                    else
                    {
                        //cargar el codigo programatico
                        Codigo_Presupuesto = Cmb_Fuente_Financiamiento_Destino.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Txt_Area_Destino.Text;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Programa_Destino.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Unidad_Responsable_Destino.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Partida_Destino.SelectedValue;
                        Txt_Codigo2.Text = Codigo_Presupuesto;
                    }
                    int Operacion = 2;//sirve para indicar que pertenese el llenado a origen

                    Cargar_Combo_Partida(Operacion);
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cmb_Partida_Destino_SelectedIndexChanged
            ///DESCRIPCIÓN: genera el codigo programatico destino
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  08/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cmb_Partida_Destino_SelectedIndexChanged(object sender, EventArgs e)
            {
                try
                {
                    String Codigo_Presupuesto = "";
                    if ((Cmb_Unidad_Responsable_Destino.SelectedIndex == 0) && (Cmb_Fuente_Financiamiento_Destino.SelectedIndex == 0)
                        && (Cmb_Programa_Destino.SelectedIndex == 0) && (Cmb_Capitulo_Destino.SelectedIndex == 0) && (Cmb_Partida_Destino.SelectedIndex == 0))
                    {
                    }
                    else
                    {
                        //cargar el codigo programatico
                        Codigo_Presupuesto = Cmb_Fuente_Financiamiento_Destino.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Txt_Area_Destino.Text;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Programa_Destino.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Unidad_Responsable_Destino.SelectedValue;
                        Codigo_Presupuesto = Codigo_Presupuesto + "-" + Cmb_Partida_Destino.SelectedValue;
                        Txt_Codigo2.Text = Codigo_Presupuesto;
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
            ///NOMBRE DE LA FUNCIÓN: Cmb_Buscar_Estatus_SelectedIndexChanged
            ///DESCRIPCIÓN: genera el codigo programatico destino
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  08/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            /*protected void Cmb_Buscar_Estatus_SelectedIndexChanged(object sender, EventArgs e)
            {
                Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio Rs_Buscar_Traspaso = new Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio();
                DataTable Dt_Busqueda = new DataTable();
                try
                {
                    Rs_Buscar_Traspaso.P_Estatus = Cmb_Buscar_Estatus.SelectedValue;
                    Dt_Busqueda = Rs_Buscar_Traspaso.Consulta_Autorizacion_Traspaso();
                    Grid_Movimiento.DataSource = Dt_Busqueda;
                    Grid_Movimiento.DataBind();
                    Grid_Movimiento.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
    */

        #endregion

        #region(TextBox)
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Txt_Importe_OnTextChanged
            ///DESCRIPCIÓN: al introducir el importe se le da formato
            ///PARAMETROS: 
            ///CREO: Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO: 15/Noviembre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************   
            protected void Txt_Importe_OnTextChanged(object sender, EventArgs e)
            {
                try
                {
                    Formato_Importe();
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
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Grid_Movimiento_SelectedIndexChanged
            /// DESCRIPCION : Consulta los datos de los movimientos seleccionada por el usuario
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 21-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            protected void Grid_Movimiento_SelectedIndexChanged(object sender, EventArgs e)
            {
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Rs_Consulta = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();//Variable de conexión hacia la capa de Negocios
                DataTable Dt_Movimiento;//Variable que obtendra los datos de la consulta 
                int Operacion;
                int Indice = 0;
                String Tipo_Operacion;
                String Estatus = "";
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Limpiar_Controles(); //Limpia los controles del la forma para poder agregar los valores del registro seleccionado

                    Rs_Consulta.P_No_Solicitud = Grid_Movimiento.SelectedRow.Cells[1].Text;
                    Dt_Movimiento = Rs_Consulta.Consulta_Movimiento();//Consulta todos los datos de los movimientos que fue seleccionada por el usuario

                    Habilitar_Visible(true);

                    if (Dt_Movimiento.Rows.Count > 0)
                    {
                        foreach (DataRow Registro in Dt_Movimiento.Rows)
                        {
                            if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud].ToString()))
                            {
                                Txt_No_Solicitud.Text = (Registro[Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud].ToString());

                                Cargar_Grid_Comentario(Txt_No_Solicitud.Text);
                            }
                            if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Codigo1].ToString()))
                            {
                                Txt_Codigo1.Text = (Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Codigo1].ToString());
                                Operacion = 1;
                                Cargar_Combo_Programa(Operacion);
                                Cargar_Combo_Financiamiento(Operacion);
                                Cargar_Combo_Capitulos(Operacion);
                                Buscar_Clave_Individual(Txt_Codigo1.Text, Operacion);
                            }
                            if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Codigo2].ToString()))
                            {
                                Txt_Codigo2.Text = (Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Codigo2].ToString());
                                Operacion = 2;
                                Cargar_Combo_Programa(Operacion);
                                Cargar_Combo_Financiamiento(Operacion);
                                Cargar_Combo_Capitulos(Operacion);
                                Buscar_Clave_Individual(Txt_Codigo2.Text, Operacion);
                            }
                            if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Importe].ToString()))
                            {
                                Txt_Importe.Text = (Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Importe].ToString());
                                Formato_Importe();
                            }

                            if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Estatus].ToString()))
                            {
                                Estatus=(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Estatus].ToString());
                                
                                if (Estatus == "AUTORIZADA")
                                {
                                    Cmb_Estatus.Items.Insert(3, "AUTORIZADA");
                                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Estatus));
                                    Cmb_Estatus.Enabled = false;
                                }
                                else if (Estatus == "CANCELADA")
                                {
                                    Cmb_Estatus.Items.RemoveAt(2);
                                    Cmb_Estatus.Items.Insert(2, "CANCELADA");
                                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Estatus));
                                    Cmb_Estatus.Enabled = false;
                                }
                                else 
                                {
                                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Estatus));
                                }
                            }
                            if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Justificacion].ToString()))
                            {
                                Txt_Justificacion.Text = (Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Justificacion].ToString());
                            }
                            if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Tipo_Operacion].ToString()))
                            {
                                Tipo_Operacion = (Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Tipo_Operacion].ToString());
                                Cmb_Operacion.SelectedIndex = Cmb_Operacion.Items.IndexOf(Cmb_Operacion.Items.FindByValue(Tipo_Operacion));
                            }
                            
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
            /// ********************************************************************************
            /// NOMBRE: Grid_Movimiento_Sorting
            /// 
            /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
            /// 
            /// CREÓ:   Hugo Enrique Ramirez Aguilera
            /// FECHA CREÓ: 21-Octubre-2011
            /// MODIFICÓ:
            /// FECHA MODIFICÓ:
            /// CAUSA MODIFICACIÓN:
            /// **********************************************************************************
            protected void Grid_Movimiento_Sorting(object sender, GridViewSortEventArgs e)
            {
                /*//Se consultan los movimientos que actualmente se encuentran registradas en el sistema.
                Consultar_Grid_Movimientos();
                DataTable Dt_Movimiento_Presupuestal = (Grid_Movimiento_Presupuestal.DataSource as DataTable);

                if (Dt_Movimiento_Presupuestal != null)
                {
                    DataView Dv_Movimiento_Presupuestal = new DataView(Dt_Movimiento_Presupuestal);
                    String Orden = ViewState["SortDirection"].ToString();

                    if (Orden.Equals("ASC"))
                    {
                        Dv_Movimiento_Presupuestal.Sort = e.SortExpression + " " + "DESC";
                        ViewState["SortDirection"] = "DESC";
                    }
                    else
                    {
                        Dv_Movimiento_Presupuestal.Sort = e.SortExpression + " " + "ASC";
                        ViewState["SortDirection"] = "ASC";
                    }

                    Grid_Movimiento_Presupuestal.DataSource = Dv_Movimiento_Presupuestal;
                    Grid_Movimiento_Presupuestal.DataBind();
                }*/
            }
    #endregion

#endregion
}
