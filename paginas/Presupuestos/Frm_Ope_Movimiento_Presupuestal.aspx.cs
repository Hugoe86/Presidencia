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




public partial class paginas_presupuestos_Frm_Ope_Movimiento_Presupuestal : System.Web.UI.Page
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
            /// FECHA_CREO  : 12-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Inicializa_Controles()
            {
                try
                {
                    Limpia_Controles();             //Limpia los controles del forma
                    Habilitar_Controles("Inicial"); //Inicializa todos los controles
                    Consulta_Movimiento_Presupuestal();//busca toda la informacion de las operaciones en la basae de datos
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
            /// FECHA_CREO  : 12-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpia_Controles()
            {
                try
                {
                    Txt_Codigo_Programatico_De.Text = "";
                    Txt_Codigo_Programatico_Al.Text = "";
                    Txt_Fuente_Financiamiento_De.Text = "";
                    Txt_Fuente_Financiamiento_Al.Text = "";
                    Txt_Area_Funcional_De.Text = "";
                    Txt_Area_Funcional_Al.Text = "";
                    Txt_Programa_De.Text = "";
                    Txt_Programa_Al.Text = "";
                    Txt_Unidad_Responsable_De.Text = "";
                    Txt_Unidad_Responsable_Al.Text = "";
                    Txt_Partida_De.Text = "";
                    Txt_Partida_Al.Text = "";
                    Txt_Monto_Traspaso.Text = "";
                    Txt_Justificacion.Text = "";
                    Cmb_Tipo_Estatus.SelectedIndex = 0;
                    Txt_Numero_Solicitud.Text = "";
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
            /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
            ///               si es una alta, modificacion
            ///                           
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 12-Octubre-2011
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

                            Configuracion_Acceso("Frm_Ope_Movimiento_Presupuestal.aspx");
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
                    Txt_Codigo_Programatico_De.Enabled = Habilitado;
                    Txt_Codigo_Programatico_Al.Enabled = Habilitado;
                    Txt_Fuente_Financiamiento_De.Enabled = Habilitado;
                    Txt_Fuente_Financiamiento_Al.Enabled = Habilitado;
                    Txt_Area_Funcional_De.Enabled = Habilitado;
                    Txt_Area_Funcional_Al.Enabled = Habilitado;
                    Txt_Programa_De.Enabled = Habilitado;
                    Txt_Programa_Al.Enabled = Habilitado;
                    Txt_Unidad_Responsable_De.Enabled = Habilitado;
                    Txt_Unidad_Responsable_Al.Enabled = Habilitado;
                    Txt_Partida_De.Enabled = Habilitado;
                    Txt_Partida_Al.Enabled = Habilitado;
                    Txt_Monto_Traspaso.Enabled = Habilitado;
                    Txt_Justificacion.Enabled = Habilitado;
                    Txt_Numero_Solicitud.Enabled = Habilitado;
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                }
                catch (Exception ex)
                {
                    throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
                }
            }

            private void Buscar_Clave_Individual()
            {
                Cls_Ope_Pre_Movimiento_Presupuestal_Negocio Buscar_Clave = new Cls_Ope_Pre_Movimiento_Presupuestal_Negocio();//Variable de conexion con la capa de negocios.
                DataTable Dt_Descripcion_Claves = new DataTable();//tomara la descripcion de la clave que se esta buscando
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

                    Clave = Txt_Codigo_Programatico_De.Text.ToUpper(); //convierto la cade en mayusculas para poder buscar en las tablas
                    Txt_Codigo_Programatico_De.Text = "" + Clave;
                    Clave = "";
                    //Buscara de uno en uno hasta encontrar un guion en la cadena
                    //posteriormente pasara la informacion obtenida en los caracteres anteriores a clave
                    //luego se compara la posicion del guia para saber que clave es
                    //se pase la informacion para realizar la consulta
                    for (Cont_For = 0; Cont_For < Txt_Codigo_Programatico_De.Text.Length; Cont_For++)
                    {
                        if (Txt_Codigo_Programatico_De.Text.Substring(Cont_For, 1) == "")
                        {
                            break;//si no hay texto termina el ciclo
                        }
                        if (Txt_Codigo_Programatico_De.Text.Substring(Cont_For, 1) == "-")//sirve para saber la posicion 
                        {
                            if (Cont_Posicion == 1)//para la consulta de Fuente de Financiamiento 
                            {
                                Fuente_Financiamiento = Clave;
                                Dt_Descripcion_Claves = Consultar_Fuente_Financiamiento(Fuente_Financiamiento);//se llena el datatable con la consulta   


                                foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                                {
                                    Txt_Fuente_Financiamiento_De.Text = Registro[Cat_SAP_Fuente_Financiamiento.Campo_Descripcion].ToString();
                                }
                                Clave = "";
                                Dt_Descripcion_Claves = null;
                            }
                            if (Cont_Posicion == 2)//para la consulta de Area Funcional
                            {
                                Area_Funcional = Clave;
                                Dt_Descripcion_Claves = Consultar_Area_Funcional(Area_Funcional);

                                foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                                {
                                    Txt_Area_Funcional_De.Text = Registro[Cat_SAP_Area_Funcional.Campo_Descripcion].ToString();
                                }
                                Dt_Descripcion_Claves = null;
                                Clave = "";
                            }
                            if (Cont_Posicion == 3)//para la consulta de programa
                            {
                                Programa = Clave;

                                Dt_Descripcion_Claves = Consultar_Programa(Programa);
                                foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                                {
                                    Txt_Programa_De.Text = Registro[Cat_Sap_Proyectos_Programas.Campo_Nombre].ToString();
                                }
                                Dt_Descripcion_Claves = null;
                                Clave = "";
                            }
                            if (Cont_Posicion == 4)//para la consulta de Responsable
                            {
                                Responsable = Clave;
                                Dt_Descripcion_Claves = Consultar_Responsable(Responsable);
                                foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                                {
                                    Txt_Unidad_Responsable_De.Text = Registro[Cat_Dependencias.Campo_Nombre].ToString();
                                }
                                Dt_Descripcion_Claves = null;
                                Clave = "";
                            }

                            Cont_Posicion++;//incrementa el valor, para asi de esta manera pasar a la siguiente clave 
                        }
                        else
                        {
                            Clave += Txt_Codigo_Programatico_De.Text.Substring(Cont_For, 1);
                        }
                    }
                    //para la ultima clave que es partida
                    Partida = Clave;
                    Dt_Descripcion_Claves = Consultar_Partida(Partida);
                    foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                    {
                        Txt_Partida_De.Text = Registro[Cat_SAP_Partida_Generica.Campo_Descripcion].ToString();
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

            private void Buscar_Clave_Individual_Destino()
            {
                Cls_Ope_Pre_Movimiento_Presupuestal_Negocio Buscar_Clave = new Cls_Ope_Pre_Movimiento_Presupuestal_Negocio();//Variable de conexion con la capa de negocios.
                DataTable Dt_Descripcion_Claves = new DataTable();//tomara la descripcion de la clave que se esta buscando
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

                    Clave = Txt_Codigo_Programatico_Al.Text.ToUpper(); //convierto la cade en mayusculas para poder buscar en las tablas
                    Txt_Codigo_Programatico_Al.Text = "" + Clave;
                    Clave = "";
                    //Buscara de uno en uno hasta encontrar un guion en la cadena
                    //posteriormente pasara la informacion obtenida en los caracteres anteriores a clave
                    //luego se compara la posicion del guia para saber que clave es
                    //se pase la informacion para realizar la consulta
                    for (Cont_For = 0; Cont_For < Txt_Codigo_Programatico_Al.Text.Length; Cont_For++)
                    {
                        if (Txt_Codigo_Programatico_Al.Text.Substring(Cont_For, 1) == "")
                        {
                            break;//si no hay texto termina el ciclo
                        }
                        if (Txt_Codigo_Programatico_Al.Text.Substring(Cont_For, 1) == "-")//sirve para saber la posicion 
                        {
                            if (Cont_Posicion == 1)//para la consulta de Fuente de Financiamiento 
                            {
                                Fuente_Financiamiento = Clave;
                                Dt_Descripcion_Claves = Consultar_Fuente_Financiamiento(Fuente_Financiamiento);//se llena el datatable con la consulta   


                                foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                                {
                                    Txt_Fuente_Financiamiento_Al.Text = Registro[Cat_SAP_Fuente_Financiamiento.Campo_Descripcion].ToString();
                                }
                                Clave = "";
                                Dt_Descripcion_Claves = null;
                            }
                            if (Cont_Posicion == 2)//para la consulta de Area Funcional
                            {
                                Area_Funcional = Clave;
                                Dt_Descripcion_Claves = Consultar_Area_Funcional(Area_Funcional);

                                foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                                {
                                    Txt_Area_Funcional_Al.Text = Registro[Cat_SAP_Area_Funcional.Campo_Descripcion].ToString();
                                }
                                Dt_Descripcion_Claves = null;
                                Clave = "";
                            }
                            if (Cont_Posicion == 3)//para la consulta de programa
                            {
                                Programa = Clave;

                                Dt_Descripcion_Claves = Consultar_Programa(Programa);
                                foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                                {
                                    Txt_Programa_Al.Text = Registro[Cat_Sap_Proyectos_Programas.Campo_Nombre].ToString();
                                }
                                Dt_Descripcion_Claves = null;
                                Clave = "";
                            }
                            if (Cont_Posicion == 4)//para la consulta de Responsable
                            {
                                Responsable = Clave;
                                Dt_Descripcion_Claves = Consultar_Responsable(Responsable);
                                foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                                {
                                    Txt_Unidad_Responsable_Al.Text = Registro[Cat_Dependencias.Campo_Nombre].ToString();
                                }
                                Dt_Descripcion_Claves = null;
                                Clave = "";
                            }

                            Cont_Posicion++;//incrementa el valor, para asi de esta manera pasar a la siguiente clave 
                        }
                        else
                        {
                            Clave += Txt_Codigo_Programatico_Al.Text.Substring(Cont_For, 1);
                        }
                    }
                    //para la ultima clave que es partida
                    Partida = Clave;
                    Dt_Descripcion_Claves = Consultar_Partida(Partida);
                    foreach (DataRow Registro in Dt_Descripcion_Claves.Rows)//se llena el registro para pasar el valor a la caja de texto
                    {
                        Txt_Partida_Al.Text = Registro[Cat_SAP_Partida_Generica.Campo_Descripcion].ToString();
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
                    Botones.Add(Btn_Nuevo);
                    Botones.Add(Btn_Modificar);
                    Botones.Add(Btn_Eliminar);
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
            /// FECHA_CREO  : 12/Octubre/2011
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
            /// NOMBRE DE LA FUNCION: Consultar_Fuente_Financiera
            /// DESCRIPCION : Consulta la clave de la tabla  para asi devolver de la 
            ///               DB su descripción
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 14-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private DataTable Consultar_Fuente_Financiamiento(String Clave_Financiamiento)
            {
                String My_SQL;//variable para almacenar la cadena de la consulta
                try 
                {
                    
                    My_SQL=" Select " +  Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + "  ";
                    My_SQL=My_SQL +" from " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento;
                    My_SQL=My_SQL +" where " + Cat_SAP_Fuente_Financiamiento.Campo_Clave +"='" +Clave_Financiamiento +"'";
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_SQL.ToString()).Tables[0];
                   
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error: " + Ex.Message);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consultar_Area_Funcional
            /// DESCRIPCION : Consulta la clave de la tabla  para asi devolver de la 
            ///               DB su descripción
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera 
            /// FECHA_CREO  : 14-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private DataTable Consultar_Area_Funcional(String Clave_Area_Funcional)
            {
                try 
                {
                    StringBuilder My_SQL;
                    My_SQL = new StringBuilder();
                    My_SQL.Append("Select " + Cat_SAP_Area_Funcional.Campo_Descripcion);
                    My_SQL.Append(" From " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional);
                    My_SQL.Append(" where " + Cat_SAP_Area_Funcional.Campo_Clave +"='" + Clave_Area_Funcional +"'");
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_SQL.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error: [" + Ex.Message + "]");
                }
            }
            
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consultar_Responsable
            /// DESCRIPCION : Consulta la clave de la tabla  para asi devolver de la 
            ///               DB su descripción
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera 
            /// FECHA_CREO  : 13-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private DataTable Consultar_Responsable(String Clave_Responsable)
            {
                try
                {
                    StringBuilder My_SQL;
                    My_SQL = new StringBuilder();
                    My_SQL.Append("Select " + Cat_Dependencias.Campo_Nombre);
                    My_SQL.Append(" from " + Cat_Dependencias.Tabla_Cat_Dependencias);
                    My_SQL.Append(" where " + Cat_Dependencias.Campo_Dependencia_ID + "='" + Clave_Responsable + "'");
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_SQL.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consultar_Partida
            /// DESCRIPCION : Consulta la clave de la tabla para asi devolver de la 
            ///               DB su descripción
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera 
            /// FECHA_CREO  : 13-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private DataTable Consultar_Partida(String Clave_Partida)
            {
                try
                {
                    StringBuilder My_SQL;
                    My_SQL = new StringBuilder();
                    My_SQL.Append("Select " +Cat_SAP_Partida_Generica.Campo_Descripcion);
                    My_SQL.Append (" from " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica);
                    My_SQL.Append(" where " + Cat_SAP_Partida_Generica.Campo_Clave  + "='" +Clave_Partida +"'");
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_SQL.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consultar_Programa
            /// DESCRIPCION : Consulta la clave de la tabla para asi devolver de la 
            ///               DB su descripción
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera 
            /// FECHA_CREO  : 14-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private DataTable Consultar_Programa(String Clave_Programa)
            {
                try
                {
                    StringBuilder My_SQL;
                    My_SQL = new StringBuilder();
                    My_SQL.Append("Select " + Cat_Sap_Proyectos_Programas.Campo_Nombre);
                    My_SQL.Append(" from " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas);
                    My_SQL.Append(" where " + Cat_Sap_Proyectos_Programas.Campo_Clave + "='" + Clave_Programa + "'");
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_SQL.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error: [" + Ex.Message + "]");
                }
            }

            
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Movimiento_Presupuestal
            /// DESCRIPCION : Consulta los movimientos que estan dadas de alta en la BD
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 19-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Movimiento_Presupuestal()
            {
                Cls_Ope_Pre_Movimiento_Presupuestal_Negocio Rs_Consulta_Movimiento_Presupuestal = new Cls_Ope_Pre_Movimiento_Presupuestal_Negocio();
                DataTable Dt_Movimiento_Presupuestal; //Variable que obtendra los datos de la consulta 

                try
                {
                    Dt_Movimiento_Presupuestal = Rs_Consulta_Movimiento_Presupuestal.Consulta_Movimiento() ;
                    Session[""] = Dt_Movimiento_Presupuestal;
                    Grid_Movimiento_Presupuestal.DataSource=(DataTable)Session[""];
                    Grid_Movimiento_Presupuestal.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Cuentas_Contables" + ex.Message.ToString(), ex);
                }
            }
        #endregion

        #region(Validacion)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Validar_Movimiento_Presupuestal
            /// DESCRIPCION : Validar que se hallan proporcionado todos los datos
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 12-Octubre-2011
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

                if (string.IsNullOrEmpty(Txt_Codigo_Programatico_De.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Cuenta Programatica es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Codigo_Programatico_Al.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Cuenta Programatica a donde sera enviada la información es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Fuente_Financiamiento_De.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Fuente de Financiamiento es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Fuente_Financiamiento_Al.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Fuente de financiamiento a donde sera enviada la información es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Area_Funcional_De.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Area Funcional es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Area_Funcional_Al.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Area Funcional a donde sera enviada la información es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Programa_De.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Persona que lo programa es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Programa_Al.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Persona que lo programa a donde sera enviada la información es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Unidad_Responsable_De.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La unidad Responsable es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Unidad_Responsable_Al.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La unidad Responsable donde se enviarea la informacion es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Partida_De.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La partida es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Partida_Al.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La partida a enviarse la información es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Monto_Traspaso.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + El monto es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                if (string.IsNullOrEmpty(Txt_Justificacion.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Justificación es un dato requerido por el sistema. <br>";
                    Datos_Validos = false;
                }
                return Datos_Validos;
            }
            
        #endregion

        #region(Operacion)
            
        #endregion

    #endregion

    #region (Eventos)
            protected void Txt_Codigo_Programatico_De_TextChanged(object sender, EventArgs e)
            {
                try
                {
                    String Clave;
                    Clave = Txt_Codigo_Programatico_De.Text.ToUpper();
                    Buscar_Clave_Individual();
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            protected void Txt_Codigo_Programatico_Al_TextChanged(object sender, EventArgs e)
            {
                try
                {
                    String Clave;
                    Clave =Txt_Codigo_Programatico_Al.Text.ToUpper();
                    Buscar_Clave_Individual_Destino();
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            protected void Cmb_Tipo_Estatus_Movimiento_SelectedIndexChanged(object sender, EventArgs e)
            {
                Cls_Ope_Pre_Movimiento_Presupuestal_Negocio Rs_Estatus = new Cls_Ope_Pre_Movimiento_Presupuestal_Negocio();
                DataTable Dt_Estatus = new DataTable();    
                try
                {
                    String Valor_Selecionado;
                    Valor_Selecionado = Cmb_Tipo_Estatus.SelectedValue;
                    if ((Valor_Selecionado == "GENERADA")  || (Valor_Selecionado == "AUTORIZADA"))
                    {
                        Limpia_Controles();
                        Rs_Estatus.P_Estatus = Cmb_Tipo_Estatus.SelectedValue;
                        Consultar_Grid_Movimientos_Estatus(Valor_Selecionado);//para cualquier estatus
                    }
                    if (Valor_Selecionado == "TODOS")
                    {
                        Inicializa_Controles();
                    }
                    //Dt_Estatus = Rs_Estatus.Consulta_Movimiento();
                    //SE LLENA EL GRID CON LA INFORMACION DEL DATATABLE DEL ESTATUS
                    

                   
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
                Cls_Ope_Pre_Movimiento_Presupuestal_Negocio Rs_Alta_Movimiento_Presupuestal = new Cls_Ope_Pre_Movimiento_Presupuestal_Negocio();
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
                        if (Validar_Movimiento_Presupuestal())
                        {
                            Rs_Alta_Movimiento_Presupuestal.P_Codigo_Programatico_De = Txt_Codigo_Programatico_De.Text.ToUpper();
                            Rs_Alta_Movimiento_Presupuestal.P_Codigo_Programatico_Al = Txt_Codigo_Programatico_Al.Text.ToUpper();
                            Rs_Alta_Movimiento_Presupuestal.P_Monto = Txt_Monto_Traspaso.Text;
                            Rs_Alta_Movimiento_Presupuestal.P_Justificacion = Txt_Justificacion.Text;
                            Rs_Alta_Movimiento_Presupuestal.P_Estatus = "GENERADA";
                            Rs_Alta_Movimiento_Presupuestal.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                            Rs_Alta_Movimiento_Presupuestal.Alta_Movimiento();
                            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Movimiento Presupuestal", "alert('El Alta del Movimiento Presupuestal fue Exitosa');", true);
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
                    Cls_Ope_Pre_Movimiento_Presupuestal_Negocio Rs_Modificar_Movimiento_Presupuestal = new Cls_Ope_Pre_Movimiento_Presupuestal_Negocio();
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";
                    if (Btn_Modificar.ToolTip == "Modificar")
                    {
                        Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                        
                    }
                   
                    else
                    {
                        
                        //Si el usuario proporciono todos los datos requeridos entonces modificar los datos de el movimiento presupuestal en la BD
                        if (Validar_Movimiento_Presupuestal())
                        {//comienza a guardar
                            Rs_Modificar_Movimiento_Presupuestal.P_No_Solicitud = Txt_Numero_Solicitud.Text;
                            Rs_Modificar_Movimiento_Presupuestal.P_Codigo_Programatico_De = Txt_Codigo_Programatico_De.Text.ToUpper();
                            Rs_Modificar_Movimiento_Presupuestal.P_Codigo_Programatico_Al = Txt_Codigo_Programatico_Al.Text.ToUpper();
                            Rs_Modificar_Movimiento_Presupuestal.P_Monto = Txt_Monto_Traspaso.Text;
                            Rs_Modificar_Movimiento_Presupuestal.P_Justificacion = Txt_Justificacion.Text;
                            Rs_Modificar_Movimiento_Presupuestal.P_Estatus = "GENERADA";
                            Rs_Modificar_Movimiento_Presupuestal.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                            Rs_Modificar_Movimiento_Presupuestal.Modificar_Movimiento(); //Modifica los datos de el movimiento presupuestal con los datos proporcionados por el usuario
                            Inicializa_Controles(); 
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Movimiento Presupuestal", "alert('La Modificacion del Movimiento Presupuestal fue Exitosa');", true);
                            
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
                Cls_Ope_Pre_Movimiento_Presupuestal_Negocio Rs_Eliminar_Movimiento_Presupuestal = new Cls_Ope_Pre_Movimiento_Presupuestal_Negocio();
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";
                    if (!string.IsNullOrEmpty(Txt_Codigo_Programatico_De.Text.Trim()))
                    {
                        Rs_Eliminar_Movimiento_Presupuestal.P_No_Solicitud = Txt_Numero_Solicitud.Text;
                        Rs_Eliminar_Movimiento_Presupuestal.Eliminar_Movimiento(); //Elimina el movimiento presupuestal que fue seleccionada por el usuario
                        Inicializa_Controles();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Movimiento Presupuestal", "alert('Se Elimino correctamente la información del Movimiento Presupuestal');", true);
                            
                    }
                    //Si el usuario no selecciono alguna movimiento presupuestal manda un mensaje indicando que es necesario que 
                    //seleccione alguna para poder eliminar
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Seleccione el movimiento presupuestal que desea eliminar <br>";
                    }
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            protected void Btn_Buscar_Movimiento_Presupuestal_Click(object sender, ImageClickEventArgs e)
            {
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";
                    //Consulta_Movimietno_Presupuestal(); //Consulta los Niveles de Poliza que coincidan con el nombre porporcionado por el usuario
                    //Si no se encontraron Movimientos Presupuestales con una descripción similar al proporcionado por el usuario entonces manda un mensaje al usuario
                    
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
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
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

    #region(Grid)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consultar_Grid_Movimientos_Estatus
            /// DESCRIPCION : Llena el grid con los movimientos presupuestales que se encuentran en la 
            ///               base de datos
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 19-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consultar_Grid_Movimientos_Estatus(String Estatus)
            {
                Cls_Ope_Pre_Movimiento_Presupuestal_Negocio Rs_Consulta_Estatus = new Cls_Ope_Pre_Movimiento_Presupuestal_Negocio();//Variable de conexión hacia la capa de Negocios
                DataTable Dt_Movimiento_Presupuestal = null; //Variable que obtendra los datos de la consulta 
                try
                {
                    Rs_Consulta_Estatus.P_Estatus = Estatus;
                    Dt_Movimiento_Presupuestal = Rs_Consulta_Estatus.Consulta_Movimiento();
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
            /// DESCRIPCION : Llena el grid con los movimientos presupuestales que se encuentran en la 
            ///               base de datos
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 19-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consultar_Grid_Movimientos()
            {
                Cls_Ope_Pre_Movimiento_Presupuestal_Negocio Rs_Consultar_Movimiento_Presupuestal = new Cls_Ope_Pre_Movimiento_Presupuestal_Negocio();//Variable de conexión hacia la capa de Negocios
                DataTable Dt_Movimiento_Presupuestal = null; //Variable que obtendra los datos de la consulta 
                try
                {
                    Dt_Movimiento_Presupuestal = Rs_Consultar_Movimiento_Presupuestal.Consulta_Movimiento();
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
            ///NOMBRE DE LA FUNCIÓN: Grid_Movimiento_Presupuestal_PageIndexChanging
            ///DESCRIPCIÓN:Cambiar de Pagina de la tabla de las operaciones de solicitud de transferencia
            ///CREO: Hugo Enrique Ramirez Aguilera
            ///FECHA_CREO: 19/Octubre/2011 
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
            /// FECHA_CREO  : 19-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            protected void Grid_Movimiento_Presupuestal_SelectedIndexChanged(object sender, EventArgs e)
            {
                Cls_Ope_Pre_Movimiento_Presupuestal_Negocio Rs_Consultar_Ope_Pre_Movimiento_Presupuestal = new Cls_Ope_Pre_Movimiento_Presupuestal_Negocio();//Variable de conexión hacia la capa de Negocios
                DataTable Dt_Movimiento_Presupuestal;//Variable que obtendra los datos de la consulta 

                try
                {
                    String Clave;
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Limpia_Controles(); //Limpia los controles del la forma para poder agregar los valores del registro seleccionado
                    
                    Rs_Consultar_Ope_Pre_Movimiento_Presupuestal.P_No_Solicitud = Grid_Movimiento_Presupuestal.SelectedRow.Cells[1].Text;
                    Dt_Movimiento_Presupuestal = Rs_Consultar_Ope_Pre_Movimiento_Presupuestal.Consulta_Movimiento();//Consulta todos los datos de los movimientos que fue seleccionada por el usuario

                    if (Dt_Movimiento_Presupuestal.Rows.Count > 0)
                    {
                        //Asigna los valores de los campos obtenidos de la consulta anterior a los controles de la forma
                        foreach (DataRow Registro in Dt_Movimiento_Presupuestal.Rows)
                        {
                            if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud].ToString()))
                            {
                                Txt_Numero_Solicitud.Text = (Registro[Cat_Ope_Com_Solicitud_Transf.Campo_No_Solicitud].ToString()); ;
                            }
                            if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Codigo1].ToString()))
                            { 
                                Txt_Codigo_Programatico_De.Text = Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Codigo1].ToString();
                                Clave = Txt_Codigo_Programatico_De.Text;
                                Buscar_Clave_Individual();//para desglozar las claves
                            }
                            if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Codigo2].ToString()))
                            {
                                Txt_Codigo_Programatico_Al.Text = Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Codigo2].ToString();
                                Clave = Txt_Codigo_Programatico_Al.Text;
                                Buscar_Clave_Individual_Destino();//para desglozar las claves
                            }
                            if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Importe].ToString()))
                            {
                                Txt_Monto_Traspaso.Text = Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Importe].ToString();
                            }
                            if (!String.IsNullOrEmpty(Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Justificacion].ToString()))
                            {
                                Txt_Justificacion.Text= Registro[Cat_Ope_Com_Solicitud_Transf.Campo_Justificacion].ToString();
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
    #endregion


           
}
