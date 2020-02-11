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
using System.Text;
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
using Presidencia.Autorizar_Traspaso_Presupuestal.Negocio;
using Presidencia.Catalogo_Compras_Presupuesto_Dependencias.Negocio;
using Presidencia.SAP_Operacion_Departamento_Presupuesto.Negocio;
using Presidencia.Catalogo_Compras_Partidas.Negocio;
using Presidencia.Manejo_Presupuesto.Datos;


public partial class paginas_presupuestos_Frm_Ope_Autorizar_Traspaso_Presupuestal : System.Web.UI.Page
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

    #region(Metodos)

        #region (Metodos Generales)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Inicializa_Controles
            /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda
            ///               realizar diferentes operaciones
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 21-Octubre-2011
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
                    Consulta_Movimiento_Presupuestal();//busca toda la informacion de las operaciones en la basae de datos
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
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 21-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpiar_Controles()
            {
                try
                {
                    Txt_Numero_Solicitud.Text = "";
                    Txt_Codigo_Programatico_Origen.Text = "";
                    Txt_Codigo_Programatico_Destino.Text = "";
                    Txt_Fuente_Financiamiento_Origen.Text = "";
                    Txt_Fuente_Financiamiento_Destino.Text = "";
                    Txt_Area_Funcional_Origen.Text = "";
                    Txt_Area_Funcional_Destino.Text = "";
                    Txt_Programa_Origen.Text = "";
                    Txt_Programa_Destino.Text = "";
                    Txt_Unidad_Responsable_Origen.Text = "";
                    Txt_Unidad_Responsable_Destino.Text = "";
                    Txt_Partida_Origen.Text = "";
                    Txt_Partida_Destino.Text = "";
                    Txt_Monto_Traspaso.Text = "";
                    Txt_Justidicacion_Actual.Text = "";
                    Txt_Justificacion_Solicitud.Text = "";
                    Txt_Estatus_Actual.Text = "";
                    Cmb_Tipo_Estatus.SelectedIndex = 0;
                    Txt_Tipo_Operacion.Text = "";
                    Txt_Tipo_Operacion.Text = "";
                    Txt_Busqueda_Movimiento_Presupuestal.Text = "";
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
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 21-Octubre-2011
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
                            Btn_Modificar.ToolTip = "Modificar";
                            
                            Btn_Salir.ToolTip = "Salir";


                            Btn_Modificar.CausesValidation = false;
                           
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                           

                            Configuracion_Acceso("Frm_Ope_Autorizar_Traspaso_Presupuestal.aspx");
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
                    Txt_Numero_Solicitud.Enabled = Habilitado;
                    Txt_Codigo_Programatico_Origen.Enabled = Habilitado;
                    Txt_Codigo_Programatico_Destino.Enabled = Habilitado;
                    Txt_Fuente_Financiamiento_Origen.Enabled = Habilitado;
                    Txt_Fuente_Financiamiento_Destino.Enabled = Habilitado;
                    Txt_Area_Funcional_Origen.Enabled = Habilitado;
                    Txt_Area_Funcional_Destino.Enabled = Habilitado;
                    Txt_Programa_Origen.Enabled = Habilitado;
                    Txt_Programa_Destino.Enabled = Habilitado;
                    Txt_Unidad_Responsable_Origen.Enabled = Habilitado;
                    Txt_Unidad_Responsable_Destino.Enabled = Habilitado;
                    Txt_Partida_Origen.Enabled = Habilitado;
                    Txt_Partida_Destino.Enabled = Habilitado;
                    Txt_Monto_Traspaso.Enabled = Habilitado;
                    Txt_Justidicacion_Actual.Enabled = Habilitado;
                    Cmb_Tipo_Estatus.Enabled = Habilitado;
                    Txt_Justificacion_Solicitud.Enabled = Habilitado;
                    Txt_Estatus_Actual.Enabled = Habilitado;
                    Txt_Justificacion_Solicitud.Enabled = Habilitado;
                    Txt_Tipo_Operacion.Enabled = Habilitado;
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                }
                catch (Exception ex)
                {
                    throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
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
                    Grid_Movimiento_Presupuestal.Visible = !Habilitado;
                    Div_Grid.Visible = !Habilitado;

                    Btn_Modificar.Visible = Habilitado;
                    lbl_Numero_Solicitud.Visible = Habilitado;
                    lbl_Codigo_Programatico_Origen.Visible = Habilitado;
                    Lbl_Codigo_Programatico_Destino.Visible = Habilitado;
                    Lbl_Fuente_Financiamento_Origen.Visible = Habilitado;
                    Lbl_Fuente_Financiamiento_Destino.Visible = Habilitado;
                    Lbl_Area_Funcional_Origen.Visible = Habilitado;
                    Lbl_Area_Funcional_Destino.Visible = Habilitado;
                    Lbl_Programa_Origen.Visible = Habilitado;
                    Lbl_Programa_Destino.Visible = Habilitado;
                    Lbl_Unidad_Responsable_Origen.Visible = Habilitado;
                    Lbl_Unidad_Responsable_Destino.Visible = Habilitado;
                    Lbl_Partida_Origen.Visible = Habilitado;
                    Lbl_Partida_Destino.Visible = Habilitado;
                    Lbl_Monto_Traspasar.Visible = Habilitado;
                    Lbl_Justificacion_Actual.Visible = Habilitado;
                    Lbl_Estatus_Actual.Visible = Habilitado;
                    Lbl_Estatus.Visible = Habilitado;
                    Lbl_Justificacion_Solicitud.Visible = Habilitado;
                    Lbl_Actualizar_Estatus.Visible = Habilitado;
                    Lbl_Tipo_Operacion.Visible = Habilitado;

                    
                    //cajas de texto
                    Txt_Numero_Solicitud.Visible = Habilitado;
                    Txt_Codigo_Programatico_Origen.Visible = Habilitado;
                    Txt_Codigo_Programatico_Destino.Visible = Habilitado;
                    Txt_Fuente_Financiamiento_Origen.Visible = Habilitado;
                    Txt_Fuente_Financiamiento_Destino.Visible = Habilitado;
                    Txt_Area_Funcional_Origen.Visible = Habilitado;
                    Txt_Area_Funcional_Destino.Visible = Habilitado;
                    Txt_Programa_Origen.Visible = Habilitado;
                    Txt_Programa_Destino.Visible = Habilitado;
                    Txt_Unidad_Responsable_Origen.Visible = Habilitado;
                    Txt_Unidad_Responsable_Destino.Visible = Habilitado;
                    Txt_Partida_Origen.Visible = Habilitado;
                    Txt_Partida_Destino.Visible = Habilitado;
                    Txt_Monto_Traspaso.Visible = Habilitado;
                    Txt_Estatus_Actual.Visible = Habilitado;
                    Txt_Justidicacion_Actual.Visible = Habilitado;
                    Cmb_Tipo_Estatus.Visible = Habilitado;
                    Txt_Justificacion_Solicitud.Visible = Habilitado;
                    Txt_Tipo_Operacion.Visible = Habilitado;

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
            /// USUARIO CREÓ: Hugo Enrique Ramirez Aguilera
            /// FECHA CREÓ  : 12/Octubre/2011 
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
                    Botones.Add(Btn_Modificar);
                   
                    Botones.Add(Btn_Buscar_Movimiento_Presupuestal);

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
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 24/Octubre/2011
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
            /// NOMBRE DE LA FUNCION: Buscar_Clave_Individual
            /// DESCRIPCION : separa la clave de codigo programatico en las claves que la forman
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 21-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Buscar_Clave_Individual()
            {
                Cls_Cat_SAP_Fuente_Financiamiento_Negocio Cls_Financiamiento = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio();
                Cls_Cat_SAP_Area_Funcional_Negocio Cls_Buscar_Clave_Funcional = new Cls_Cat_SAP_Area_Funcional_Negocio();
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Cls_Area_Funcional = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Cls_Programa = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
                Cls_Cat_Dependencias_Negocio Cls_Respondable = new Cls_Cat_Dependencias_Negocio();
                Cls_Cat_Sap_Partida_Generica_Negocio Cls_Partida = new Cls_Cat_Sap_Partida_Generica_Negocio();
                Cls_Cat_Com_Partidas_Negocio Partida_Especifica = new Cls_Cat_Com_Partidas_Negocio();
                Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio Rs_Partida_Especifica = new Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio();
                DataTable Dt_Descripcion_Claves = new DataTable();//tomara la descripcion de la clave que se esta buscando
                DataSet Dts_Descripcion = new DataSet();
                Dt_Descripcion_Claves = null;

                try
                {
                    string Fuente_Financiamiento = "";//se usaran las variables string para contener las claves 
                    string Area_Funcional;
                    string Programa;
                    string Responsable;
                    string Partida;
                    string Clave = ""; //se usara para almacenenar las claves individuales que sean obtenidas en el ciclo for 
                    int Cont_For;//contador para el ciclo for
                    int Cont_Posicion = 1;//contador para la posicion del guion en la cadena de texto

                    Clave = Txt_Codigo_Programatico_Origen.Text.ToUpper(); //convierto la cade en mayusculas para poder buscar en las tablas
                    Txt_Codigo_Programatico_Origen.Text = "" + Clave;
                    Clave = "";
                    //Buscara de uno en uno hasta encontrar un guion en la cadena
                    //posteriormente pasara la informacion obtenida en los caracteres anteriores a clave
                    //luego se compara la posicion del guia para saber que clave es
                    //se pase la informacion para realizar la consulta
                    for (Cont_For = 0; Cont_For < Txt_Codigo_Programatico_Origen.Text.Length; Cont_For++)
                    {
                        if (Txt_Codigo_Programatico_Origen.Text.Substring(Cont_For, 1) == "")
                        {
                            break;//si no hay texto termina el ciclo
                        }
                        if (Txt_Codigo_Programatico_Origen.Text.Substring(Cont_For, 1) == "-")//sirve para saber la posicion 
                        {
                            if (Cont_Posicion == 1)//para la consulta de Fuente de Financiamiento 
                            {
                                Fuente_Financiamiento = Clave;
                                Cls_Financiamiento.P_Clave = Fuente_Financiamiento;
                                Dt_Descripcion_Claves = Cls_Financiamiento.Consulta_Fuente_Financiamiento();//se llena el datatable con la consulta   
                                foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                                {
                                    Txt_Fuente_Financiamiento_Origen.Text = Registro[Cat_SAP_Fuente_Financiamiento.Campo_Descripcion].ToString();
                                }
                                Clave = "";
                                Dt_Descripcion_Claves = null;
                            }
                            if (Cont_Posicion == 2)//para la consulta de Area Funcional
                            {
                                Area_Funcional = Clave;
                                Cls_Area_Funcional.P_Area_Funcional = Area_Funcional;
                                Dt_Descripcion_Claves = Cls_Area_Funcional.Consultar_Area_Funciona();
                                foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                                {
                                    Txt_Area_Funcional_Origen.Text = Registro[Cat_SAP_Area_Funcional.Campo_Descripcion].ToString();
                                }

                                Dt_Descripcion_Claves = null;
                                Clave = "";
                            }
                            if (Cont_Posicion == 3)//para la consulta de programa
                            {
                                Programa = Clave;
                                Cls_Programa.P_Programa = Programa;
                                Dt_Descripcion_Claves = Cls_Programa.Consultar_Programa();
                                foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                                {
                                    Txt_Programa_Origen.Text = Registro[Cat_Sap_Proyectos_Programas.Campo_Nombre].ToString();
                                }
                                Dt_Descripcion_Claves = null;
                                Clave = "";
                            }
                            if (Cont_Posicion == 4)//para la consulta de Responsable
                            {
                                Responsable = Clave;
                                Cls_Respondable.P_Clave = Responsable;
                                Dt_Descripcion_Claves = Cls_Respondable.Consulta_Dependencias();
                                foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                                {
                                    Txt_Unidad_Responsable_Origen.Text = Registro[Cat_Dependencias.Campo_Nombre].ToString();
                                }
                                Dt_Descripcion_Claves = null;
                                Clave = "";
                            }

                            Cont_Posicion++;//incrementa el valor, para asi de esta manera pasar a la siguiente clave 
                        }
                        else
                        {
                            Clave += Txt_Codigo_Programatico_Origen.Text.Substring(Cont_For, 1);
                        }
                    }
                    //para la ultima clave que es partida
                    Partida = Clave;
                    Partida_Especifica.P_Clave = Partida;
                    Rs_Partida_Especifica.P_Partida = Partida;
                    Dt_Descripcion_Claves = Rs_Partida_Especifica.Consulta_Datos_Partidas();
                    foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                    {
                        Txt_Partida_Origen.Text = Registro[Cat_Com_Partidas.Campo_Nombre].ToString();
                    }
                    Dt_Descripcion_Claves = null;
                    Clave = "";
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Buscar_Clave_Individual_Destino
            /// DESCRIPCION : separa la clave de codigo programatico en las claves que la forman
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 21-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Buscar_Clave_Individual_Destino()
            {
                Cls_Cat_SAP_Fuente_Financiamiento_Negocio Cls_Financiamiento = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio();
                Cls_Cat_SAP_Area_Funcional_Negocio Cls_Buscar_Clave_Funcional = new Cls_Cat_SAP_Area_Funcional_Negocio();
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Cls_Area_Funcional = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Cls_Programa = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
                Cls_Cat_Dependencias_Negocio Cls_Respondable = new Cls_Cat_Dependencias_Negocio();
                Cls_Cat_Sap_Partida_Generica_Negocio Cls_Partida = new Cls_Cat_Sap_Partida_Generica_Negocio();
                Cls_Cat_Com_Partidas_Negocio Partida_Especifica = new Cls_Cat_Com_Partidas_Negocio();
                Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio Rs_Partida_Especifica = new Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio();

                DataTable Dt_Descripcion_Claves = new DataTable();//tomara la descripcion de la clave que se esta buscando
                DataSet Dts_Descripcion = new DataSet();
                Dt_Descripcion_Claves = null;

                try
                {
                    string Fuente_Financiamiento = "";//se usaran las variables string para contener las claves 
                    string Area_Funcional;
                    string Programa;
                    string Responsable;
                    string Partida;
                    string Clave = ""; //se usara para almacenenar las claves individuales que sean obtenidas en el ciclo for 
                    int Cont_For;//contador para el ciclo for
                    int Cont_Posicion = 1;//contador para la posicion del guion en la cadena de texto

                    Clave = Txt_Codigo_Programatico_Destino.Text.ToUpper(); //convierto la cade en mayusculas para poder buscar en las tablas
                    Txt_Codigo_Programatico_Destino.Text = "" + Clave;
                    Clave = "";
                    //Buscara de uno en uno hasta encontrar un guion en la cadena
                    //posteriormente pasara la informacion obtenida en los caracteres anteriores a clave
                    //luego se compara la posicion del guia para saber que clave es
                    //se pase la informacion para realizar la consulta
                    for (Cont_For = 0; Cont_For < Txt_Codigo_Programatico_Destino.Text.Length; Cont_For++)
                    {
                        if (Txt_Codigo_Programatico_Destino.Text.Substring(Cont_For, 1) == "")
                        {
                            break;//si no hay texto termina el ciclo
                        }
                        if (Txt_Codigo_Programatico_Destino.Text.Substring(Cont_For, 1) == "-")//sirve para saber la posicion 
                        {
                            if (Cont_Posicion == 1)//para la consulta de Fuente de Financiamiento 
                            {
                                Fuente_Financiamiento = Clave;
                                Cls_Financiamiento.P_Clave = Fuente_Financiamiento;
                                Dt_Descripcion_Claves = Cls_Financiamiento.Consulta_Fuente_Financiamiento();//se llena el datatable con la consulta   
                                foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                                {
                                    Txt_Fuente_Financiamiento_Destino.Text = Registro[Cat_SAP_Fuente_Financiamiento.Campo_Descripcion].ToString();
                                }
                                Clave = "";
                                Dt_Descripcion_Claves = null;
                            }
                            if (Cont_Posicion == 2)//para la consulta de Area Funcional
                            {
                                Area_Funcional = Clave;
                                Cls_Area_Funcional.P_Area_Funcional = Area_Funcional;
                                Dt_Descripcion_Claves = Cls_Area_Funcional.Consultar_Area_Funciona();
                                foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                                {
                                    Txt_Area_Funcional_Destino.Text = Registro[Cat_SAP_Area_Funcional.Campo_Descripcion].ToString();
                                }
                                Dt_Descripcion_Claves = null;
                                Clave = "";
                            }
                            if (Cont_Posicion == 3)//para la consulta de programa
                            {
                                Programa = Clave;
                                Cls_Programa.P_Programa = Programa;
                                Dt_Descripcion_Claves = Cls_Programa.Consultar_Programa();
                                foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                                {
                                    Txt_Programa_Destino.Text = Registro[Cat_Sap_Proyectos_Programas.Campo_Nombre].ToString();
                                }
                                Dt_Descripcion_Claves = null;
                                Clave = "";
                            }
                            if (Cont_Posicion == 4)//para la consulta de Responsable
                            {
                                Responsable = Clave;
                                Cls_Respondable.P_Clave = Responsable;
                                Dt_Descripcion_Claves = Cls_Respondable.Consulta_Dependencias();
                                foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                                {
                                    Txt_Unidad_Responsable_Destino.Text = Registro[Cat_Dependencias.Campo_Nombre].ToString();
                                }
                                Dt_Descripcion_Claves = null;
                                Clave = "";
                            }

                            Cont_Posicion++;//incrementa el valor, para asi de esta manera pasar a la siguiente clave 
                        }
                        else
                        {
                            Clave += Txt_Codigo_Programatico_Destino.Text.Substring(Cont_For, 1);
                        }
                    }
                    //para la ultima clave que es partida
                    Partida = Clave;
                    Partida_Especifica.P_Clave = Partida;
                    Rs_Partida_Especifica.P_Partida = Partida;
                    Dt_Descripcion_Claves = Rs_Partida_Especifica.Consulta_Datos_Partidas();
                    foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                    {
                        Txt_Partida_Destino.Text = Registro[Cat_Com_Partidas.Campo_Nombre].ToString();
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
            /// NOMBRE DE LA FUNCION: Consulta_Movimiento_Presupuestal
            /// DESCRIPCION : Consulta los movimientos que estan dadas de alta en la BD
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 21-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Movimiento_Presupuestal()
            {
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Rs_Consulta_Movimiento_Presupuestal = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
                DataTable Dt_Movimiento_Presupuestal; //Variable que obtendra los datos de la consulta 

                try
                {
                    Dt_Movimiento_Presupuestal = Rs_Consulta_Movimiento_Presupuestal.Consulta_Movimiento();
                    Session["Consulta_Movimiento_Presupuestal"] = Dt_Movimiento_Presupuestal;
                    Grid_Movimiento_Presupuestal.DataSource = (DataTable)Session["Consulta_Movimiento_Presupuestal"];
                    Grid_Movimiento_Presupuestal.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Movimiento Presupuestal" + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consultar_Grid_Movimientos_Estatus
            /// DESCRIPCION : realiza una consulta para obtener los estatus a los que pertenecen
            /// PARAMETROS  : 1.- String Estatus:  Muestra el estatus al que pertenece(Generada o autorizada)
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 21-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consultar_Grid_Movimientos_Estatus(String Estatus)
            {
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Cls_Consulta_Estatus = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();//Variable de conexión hacia la capa de Negocios
                DataTable Dt_Movimiento_Presupuestal = null; //Variable que obtendra los datos de la consulta 
                try
                {
                    Cls_Consulta_Estatus.P_Estatus = Estatus;
                    Dt_Movimiento_Presupuestal = Cls_Consulta_Estatus.Consulta_Movimiento();
                    Session["Consulta_Movimiento_Presupuestal"] = Dt_Movimiento_Presupuestal;
                    Grid_Movimiento_Presupuestal.DataSource = (DataTable)Session["Consulta_Movimiento_Presupuestal"];
                    Grid_Movimiento_Presupuestal.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Consultar Movimientos " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consultar_Grid_Movimientos
            /// DESCRIPCION : Realiza una consulta para obtener todos las registros de la tabla
            ///               que pertenescan al estatus de generada
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 21-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consultar_Grid_Movimientos()
            {
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Cls_Consultar_Traspazo_Presupuestal = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();//Variable de conexión hacia la capa de Negocios
                DataTable Dt_Movimiento_Presupuestal = null; //Variable que obtendra los datos de la consulta 
                try
                {
                    
                    Dt_Movimiento_Presupuestal = Cls_Consultar_Traspazo_Presupuestal.Consulta_Movimiento();
                    Session["Consulta_Movimiento_Presupuestal"] = Dt_Movimiento_Presupuestal;
                    Grid_Movimiento_Presupuestal.DataSource = (DataTable)Session["Consulta_Movimiento_Presupuestal"];
                    Grid_Movimiento_Presupuestal.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Consultar Movimientos " + ex.Message.ToString(), ex);
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
                String Formato="";
                int Contador_For;
                String Importe;
                String Miles="";
                int indice;
                int Multiplo;
                int Residuo;
                try 
                {
                    Importe = Txt_Monto_Traspaso.Text;
                    indice=Txt_Monto_Traspaso.Text.Length;
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
                                    Txt_Monto_Traspaso.Text = Formato;

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

                    Letra = Txt_Monto_Traspaso.Text;
                    Indice = Txt_Monto_Traspaso.Text.Length;
                    for (Contador_For = 0; Contador_For < Indice; Contador_For++)
                    {
                        if (Txt_Monto_Traspaso.Text.Substring(Contador_For, 1)!=",")
                        {
                            Formato ="" +Formato +Letra.Substring(Contador_For, 1);
                        }
                       
                    }
                    Txt_Monto_Traspaso.Text = Formato;

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
           
        #endregion

        #region(Validacion)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Validar_Movimiento_Presupuestal
            /// DESCRIPCION : Validar que se hallan proporcionado todos los datos
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 21-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Validar_Movimiento_Presupuestal()
            {
                String Espacios_Blanco;
                Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
                Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

                if (string.IsNullOrEmpty(Txt_Codigo_Programatico_Origen.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Cuenta Programatica Origen es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Codigo_Programatico_Destino.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Cuenta Programatica Destino es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Fuente_Financiamiento_Origen.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Fuente de Financiamiento Origen es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Fuente_Financiamiento_Destino.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Fuente de financiamiento Destino es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Area_Funcional_Origen.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Area Funcional Origen es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Area_Funcional_Destino.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Area Funcional Destino es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Programa_Origen.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + El programa Origen es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Programa_Destino.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + El programa Destino es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Unidad_Responsable_Origen.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La unidad Responsable Origen es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Unidad_Responsable_Destino.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La unidad Responsable Destino es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Partida_Origen.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La partida Oregen es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Partida_Destino.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La partida Destino es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Monto_Traspaso.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + El monto es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Justidicacion_Actual.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Justificación actual es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (Cmb_Tipo_Estatus.SelectedValue == "< SELECCIONE ESTATUS >")
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " +El estatus es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
               
                if ((Cmb_Tipo_Estatus.SelectedValue=="RECHAZADA") && (Txt_Justificacion_Solicitud.Text==""))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " +El comentario correspondiente. <br>";
                    Datos_Validos = false;
                }
                return Datos_Validos;
            }
            
        #endregion

    #endregion

    #region (Eventos)
        ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cmb_Tipo_Estatus_Movimiento_SelectedIndexChanged
            ///DESCRIPCIÓN: el escoger algun estatus este realizara una busqueda y lo cargara en 
            ///             el datagrid
            ///PARAMETROS: 
            ///CREO: Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO: 24/Octubre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************        
        protected void Cmb_Tipo_Estatus_Movimiento_SelectedIndexChanged(object sender, EventArgs e)
            {
                Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio Cls_Estatus = new Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio();
                DataTable Dt_Estatus = new DataTable();
                try
                {
                    String Valor_Selecionado;
                    Valor_Selecionado = Cmb_Tipo_Estatus.SelectedValue;
                    if ((Valor_Selecionado == "AUTORIZADA") || (Valor_Selecionado == "< SELECCIONE ESTATUS >"))
                    {
                        Txt_Justificacion_Solicitud.ReadOnly = true;   
                    }
                    if (Valor_Selecionado == "RECHAZADA")
                    {
                        Txt_Justificacion_Solicitud.Enabled = true;
                        Txt_Justificacion_Solicitud.Text = "";
                        Txt_Justificacion_Solicitud.ReadOnly = false;
                        this.SetFocus(Txt_Justificacion_Solicitud);
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
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Habilita las cajas de texto necesarias para crear un Nuevo Movimiento
        ///             se convierte en dar alta cuando oprimimos Nuevo y dar alta  Crea un registro  
        ///                de un movimiento presupuestal en la base de datos
        ///PARAMETROS: 
        ///CREO: Hugo Enrique Ramirez Aguilera
        ///FECHA_CREO: 25/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
        {
           
            try
            {
                
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
            ///FECHA_CREO:  17/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
            {
                Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio Rs_Alta_Traspaso = new Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio();
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Rs_Alta_Comentario = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
                Cls_Ope_Psp_Manejo_Presupuesto Rs_Traspaso_Presupuestal = new Cls_Ope_Psp_Manejo_Presupuesto();
                DataTable Dt_Consultar = new DataTable();
                String Origen_Dependencia = "";
                String Destino_Dependencia = "";
                String Origen_Fte_Financiamiento = "";
                String Destino_Fte_Financiamiento = "";
                String Origen_Programa= "";
                String Destino_Programa = "";
                String Origen_Partida = "";
                String Destino_Partida = "";
                String Año;
                int Origen_Anio;
                int Destino_Anio;
                double Importe;
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";
                    if (Btn_Modificar.ToolTip == "Modificar")
                    {
                        Habilitar_Controles("Modificar"); //Habilita los controles para la introducción de datos por parte del usuario

                    }
                    else
                    {
                        //Valida si todos los campos requeridos estan llenos si es así da de alta los datos en la base de datos
                        if (Validar_Movimiento_Presupuestal())
                        {
                            if (Txt_Estatus_Actual.Text == "GENERADA")
                            {
                                Rs_Alta_Traspaso.P_Numero_Solicitud = Txt_Numero_Solicitud.Text.Trim().ToUpper();
                                Rs_Alta_Traspaso.P_Codigo_Programatico_Origen = Txt_Codigo_Programatico_Origen.Text.ToUpper().Trim();
                                Rs_Alta_Traspaso.P_Codigo_Programatico_Destino = Txt_Codigo_Programatico_Destino.Text.ToUpper().Trim();
                                Quita_Formato_Importe();
                                Rs_Alta_Traspaso.P_Importe = Txt_Monto_Traspaso.Text;
                                Rs_Alta_Traspaso.P_Justificacion = Txt_Justidicacion_Actual.Text;
                                Rs_Alta_Traspaso.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                                Rs_Alta_Traspaso.P_Tipo_Operacion = Txt_Tipo_Operacion.Text; 
                                Rs_Alta_Traspaso.P_Estatus = Cmb_Tipo_Estatus.SelectedValue; 
                                

                                //para el estatus de autorizado
                                if (Cmb_Tipo_Estatus.SelectedValue == "AUTORIZADA")
                                {
                                   
                                    Rs_Alta_Traspaso.Modificar_Autorizacion_Traspaso(); 
                                    Rs_Alta_Traspaso.P_Estatus = Cmb_Tipo_Estatus.SelectedValue; 
                                    
                                    //para consultar los id del movimiento con numero de solicitud
                                    Rs_Alta_Traspaso.P_Numero_Solicitud = Txt_Numero_Solicitud.Text.Trim().ToUpper();
                                    Dt_Consultar = Rs_Alta_Traspaso.Consulta_Autorizacion_Traspaso();

                                    //para el traspaso de presupuesto
                                    Año = "" + System.DateTime.Today;
                                    Año = String.Format("{0:dd-MMM-yyyy}", Año);
                                    Año = Año.Substring(6, 4);
                                    Origen_Anio = Convert.ToInt32(Año);
                                    Destino_Anio = Convert.ToInt32(Año);
                                    Importe = Convert.ToDouble(Txt_Monto_Traspaso.Text);

                                    //para realzar el traspaso presupuestal
                                    foreach (DataRow Registro in Dt_Consultar.Rows)
                                    {
                                        //Para dependencia
                                        if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Origen_Dependencia_Id].ToString()))
                                        {
                                            Origen_Dependencia = Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Origen_Dependencia_Id].ToString();
                                        }
                                        if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Destino_Dependencia_Id].ToString()))
                                        {
                                           Destino_Dependencia = Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Destino_Dependencia_Id].ToString();
                                        }
                                        //para fuente de financiamiento
                                        if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Origen_Fuente_Financiamiento_Id].ToString()))
                                        {
                                            Origen_Fte_Financiamiento=Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Origen_Fuente_Financiamiento_Id].ToString();
                                        }
                                        if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Destino_Fuente_Financiamiento_Id].ToString()))
                                        {
                                            Destino_Fte_Financiamiento = Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Destino_Fuente_Financiamiento_Id].ToString();
                                        }
                                        //Para programa
                                        if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Origen_Programa_Id].ToString()))
                                        {
                                            Origen_Programa = Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Origen_Programa_Id].ToString();
                                        }
                                        if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Destino_Programa_Id].ToString()))
                                        {
                                            Destino_Programa = Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Destino_Programa_Id].ToString();
                                        }
                                        //Para partida
                                        if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Origen_Partida_Id].ToString()))
                                        {
                                            Origen_Partida = Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Origen_Partida_Id].ToString();
                                        }
                                        if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Destino_Partida_Id].ToString()))
                                        {
                                            Destino_Partida = Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Destino_Partida_Id].ToString();
                                        }
                                        Cls_Ope_Psp_Manejo_Presupuesto.Traspaso_Presupuestal(Origen_Dependencia, Origen_Fte_Financiamiento, 
                                                                     Origen_Programa,Origen_Partida,Origen_Anio,
                                                                     Destino_Dependencia,Destino_Fte_Financiamiento,
                                                                     Destino_Programa, Destino_Partida, Destino_Anio, Importe);
                                    }
                                }
                                
                                //PARA RECHAZADA MODIFICA EL COMENTARIO
                                else if (Cmb_Tipo_Estatus.SelectedValue == "RECHAZADA")
                                {
                                    Rs_Alta_Traspaso.P_Estatus = Cmb_Tipo_Estatus.SelectedValue;
                                    Rs_Alta_Traspaso.Modificar_Autorizacion_Traspaso();

                                    Rs_Alta_Comentario.P_No_Solicitud = Txt_Numero_Solicitud.Text.Trim().ToUpper();
                                    Rs_Alta_Comentario.P_Comentario = Txt_Justificacion_Solicitud.Text;
                                    Rs_Alta_Comentario.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                                    Rs_Alta_Comentario.Alta_Comentario();
                                }

                              


                               
                                Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "AUTORIZAR TRASPASO", "alert('La modificacion fue exitosa');", true);
                                Habilitar_Visible(false);
                                Grid_Movimiento_Presupuestal.Visible = true;
                                Div_Grid.Visible = true;
                            }
                            else
                            {
                                Lbl_Mensaje_Error.Text = "Verifique que el estatus actual no sea:<br>AUTORIZADA <br>RECHAZADA <br>CANCELADA";

                                Lbl_Mensaje_Error.Visible = true;
                                Img_Error.Visible = true;
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
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Movimiento_Presupuestal_Click
        ///DESCRIPCIÓN: busca el movimiento y lo carga en el grid
        ///PARAMETROS: 
        ///CREO: Hugo Enrique Ramirez Aguilera
        ///FECHA_CREO: 21/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Buscar_Movimiento_Presupuestal_Click(object sender, ImageClickEventArgs e)
        {
            Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio Cls_Buscar_Traspaso = new Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio();
            DataTable Dt_Busqueda = new DataTable();
            Boolean Resultado_Numerico = false;//guardara si el resultado es numero o no
            
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                Resultado_Numerico= Es_Numero(Txt_Busqueda_Movimiento_Presupuestal.Text.Trim());//para saber si es un numero
                if (!string.IsNullOrEmpty(Txt_Busqueda_Movimiento_Presupuestal.Text.Trim()))
                {
                    if (Resultado_Numerico == true)
                    {
                        Cls_Buscar_Traspaso.P_Numero_Solicitud = Txt_Busqueda_Movimiento_Presupuestal.Text.ToUpper().Trim();
                    }
                    else
                    {
                        Cls_Buscar_Traspaso.P_Estatus = Txt_Busqueda_Movimiento_Presupuestal.Text.ToUpper().Trim();

                    }
                    Dt_Busqueda = Cls_Buscar_Traspaso.Consulta_Autorizacion_Traspaso();
                    Cargar_Grid_Movimiento(Dt_Busqueda);
                    Grid_Movimiento_Presupuestal.Visible = true;
                    Div_Grid.Visible = true;
                    Habilitar_Visible(false);
                    if (Grid_Movimiento_Presupuestal.Rows.Count == 0 && !string.IsNullOrEmpty(Txt_Busqueda_Movimiento_Presupuestal.Text))
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "No se encontraron registros sobre el movimiento realizado pruebe otra vez";
                    }
                    Limpiar_Controles();

                }
                else 
                {
                    Inicializa_Controles();
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
        ///DESCRIPCIÓN:  Cancela la operacion actual qye se este realizando
        ///PARAMETROS: 
        ///CREO: Hugo Enrique Ramirez Aguilera
        ///FECHA_CREO: 21/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {/*
                if ((Btn_Salir.ToolTip == "Salir") && (Div_Grid_Movimientos.Visible == true))
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                    else if (Div_Grid_Movimientos.Visible == false)
                    {
                        Habilitar_Visible(false);
                    }
                    else
                    {
                        Inicializa_Controles(); //Habilita los controles para la siguiente operación del usuario en el catálogo
                    }*/
                if ((Btn_Salir.ToolTip == "Salir") && (Grid_Movimiento_Presupuestal.Visible==true))
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
                else if (Grid_Movimiento_Presupuestal.Visible == false)
                {
                    Habilitar_Visible(false);
                    Inicializa_Controles();
                    
                }
                else
                {   //Habilita los controles para la siguiente operación del usuario en el catálogo
                    Inicializa_Controles();
                    
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

    #region(Grid)
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Movimiento_Presupuestal_PageIndexChanging
        ///DESCRIPCIÓN:Cambiar de Pagina de la tabla de las operaciones de solicitud de transferencia
        ///CREO: Hugo Enrique Ramirez Aguilera
        ///FECHA_CREO: 21/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Movimiento_Presupuestal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Grid_Movimiento_Presupuestal.PageIndex = e.NewPageIndex; //Asigna la nueva página que selecciono el usuario
                Consultar_Grid_Movimientos();
                Grid_Movimiento_Presupuestal.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Movimiento_Presupuestal_SelectedIndexChanged
        /// DESCRIPCION : Consulta los datos de los movimientos seleccionada por el usuario
        /// CREO        : Hugo Enrique Ramirez Aguilera
        /// FECHA_CREO  : 21-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Movimiento_Presupuestal_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Cls_Consultar_Ope_Pre_Movimiento_Presupuestal = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();//Variable de conexión hacia la capa de Negocios
            DataTable Dt_Movimiento_Presupuestal;//Variable que obtendra los datos de la consulta 

            try
            {
                String Clave;
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Limpiar_Controles(); //Limpia los controles del la forma para poder agregar los valores del registro seleccionado

                Cls_Consultar_Ope_Pre_Movimiento_Presupuestal.P_No_Solicitud = Grid_Movimiento_Presupuestal.SelectedRow.Cells[1].Text;
                Dt_Movimiento_Presupuestal = Cls_Consultar_Ope_Pre_Movimiento_Presupuestal.Consulta_Movimiento();//Consulta todos los datos de los movimientos que fue seleccionada por el usuario

                Grid_Movimiento_Presupuestal.Visible = false;
                Habilitar_Visible(true);
                Div_Grid.Visible = false;

                String Año;
                

                if (Dt_Movimiento_Presupuestal.Rows.Count > 0)
                {
                    //Asigna los valores de los campos obtenidos de la consulta anterior a los controles de la forma
                    foreach (DataRow Registro in Dt_Movimiento_Presupuestal.Rows)
                    {
                        if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud].ToString()))
                        {
                            Txt_Numero_Solicitud.Text = (Registro[Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud].ToString()) ;
                            Cargar_Grid_Comentario(Txt_Numero_Solicitud.Text);
                        }
                        if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Codigo1].ToString()))
                        {
                            Txt_Codigo_Programatico_Origen.Text = Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Codigo1].ToString();
                            Clave = Txt_Codigo_Programatico_Origen.Text;
                            Buscar_Clave_Individual();//para desglozar las claves
                        }
                        if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Codigo2].ToString()))
                        {
                            Txt_Codigo_Programatico_Destino.Text = Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Codigo2].ToString();
                            Clave = Txt_Codigo_Programatico_Destino.Text;
                            Buscar_Clave_Individual_Destino();//para desglozar las claves
                        }
                        if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Importe].ToString()))
                        {
                            Txt_Monto_Traspaso.Text = Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Importe].ToString();
                            Formato_Importe();
                        }
                        if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Justificacion].ToString()))
                        {
                           Txt_Justidicacion_Actual.Text = Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Justificacion].ToString();
                        }
                        if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Estatus].ToString()))
                        {
                            Txt_Estatus_Actual.Text= Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Estatus].ToString();
                        }
                        if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Tipo_Operacion].ToString()))
                        {
                            Txt_Tipo_Operacion.Text = Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Tipo_Operacion].ToString();
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
        /// NOMBRE: Grid_Movimiento_Presupuestal_Sorting
        /// 
        /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
        /// 
        /// CREÓ:   Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ: 21-Octubre-2011
        /// MODIFICÓ:
        /// FECHA MODIFICÓ:
        /// CAUSA MODIFICACIÓN:
        /// **********************************************************************************
        protected void Grid_Movimiento_Presupuestal_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Se consultan los movimientos que actualmente se encuentran registradas en el sistema.
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
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Cargar_Grid_Bancos
        /// DESCRIPCION : Carga los Movimientos registrados en el sistema. 
        ///               registradas en el sistema.
        /// CREO        : Hugo Enrique Ramirez Aguilera
        /// FECHA_CREO  : 21-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Cargar_Grid_Movimiento(DataTable Dt_Busqueda_Movimiento)
        {
            try
            {
                Grid_Movimiento_Presupuestal.DataSource = Dt_Busqueda_Movimiento;
                Grid_Movimiento_Presupuestal.DataBind();
                Grid_Movimiento_Presupuestal.SelectedIndex = -1;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al cargar los movimientos en la tabla que los listara. Error: [" + Ex.Message + "]");
            }
        }

    #endregion
}
