﻿using System;
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
using Presidencia.Tipo_Polizas.Negocios;

public partial class paginas_Contabilidad_Frm_Cat_Con_Tipo_Polizas : System.Web.UI.Page
{
    #region (Page Load)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Page_Load
        /// DESCRIPCION : Carga la configuración inicial de los controles de la página.
        /// CREO        : Yazmin Abigail Delgado Gómez
        /// FECHA_CREO  : 09-Junio-2011
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
        #region (Métodos Generales)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Inicializa_Controles
            /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
            ///               diferentes operaciones
            /// PARAMETROS  : 
            /// CREO        : Yazmin Abigail Delgado Gómez
            /// FECHA_CREO  : 09-Junio-2011
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
                    Consulta_Tipo_Poliza();        //Consulta todas los Tipos de Polizas que fueron dadas de alta en la BD
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
            /// CREO        : Yazmin Abigail Delgado Gómez
            /// FECHA_CREO  : 09-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpia_Controles()
            {
                try
                {
                    Txt_Tipo_Poliza_ID.Text = "";
                    Txt_Abreviacion_Tipo_Poliza.Text = "";
                    Txt_Descripcion_Tipo_Poliza.Text = "";
                    Txt_Comentarios_Tipo_Poliza.Text = "";
                    Txt_Busqueda_Tipo_Poliza.Text = "";
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
            /// CREO        : Yazmin Abigail Delgado Gómez
            /// FECHA_CREO  : 09-Junio-2011
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

                        Configuracion_Acceso("Frm_Cat_Con_Tipo_Polizas.aspx");
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
                Txt_Abreviacion_Tipo_Poliza.Enabled = Habilitado;
                Txt_Descripcion_Tipo_Poliza.Enabled = Habilitado;
                Txt_Comentarios_Tipo_Poliza.Enabled = Habilitado;
                Txt_Busqueda_Tipo_Poliza.Enabled = !Habilitado;
                Btn_Buscar_Descripcion_Tipo_Poliza.Enabled = !Habilitado;
                Grid_Tipo_Poliza.Enabled = !Habilitado;
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
                    Botones.Add(Btn_Buscar_Descripcion_Tipo_Poliza);

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
            /// NOMBRE DE LA FUNCION: Consulta_Tipo_Poliza
                /// DESCRIPCION : Consulta los Tipos de Poliza que estan dadas de alta en la BD
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 09-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Tipo_Poliza()
            {
                Cls_Cat_Con_Tipo_Polizas_Negocio Rs_Consulta_Cat_Con_Tipo_Polizas = new Cls_Cat_Con_Tipo_Polizas_Negocio(); //Variable de conexión hacia la capa de Negocios
                DataTable Dt_Tipo_Poliza; //Variable que obtendra los datos de la consulta 

                try
                {
                    if (!string.IsNullOrEmpty(Txt_Busqueda_Tipo_Poliza.Text.Trim()))
                    {
                        Rs_Consulta_Cat_Con_Tipo_Polizas.P_Descripcion = Txt_Busqueda_Tipo_Poliza.Text;
                    }
                    Dt_Tipo_Poliza = Rs_Consulta_Cat_Con_Tipo_Polizas.Consulta_Datos_Tipo_Poliza(); //Consulta los datos generales de los Tipos de Poliza dados de alta en la BD
                    Session["Consulta_Tipo_Poliza"] = Dt_Tipo_Poliza;
                    Llena_Grid_Tipo_Poliza(); //Agrega los tipos de Poliza obtenidas de la consulta anterior
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Tipo_Poliza " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Llena_Grid_Tipo_Poliza
            /// DESCRIPCION : Llena el grid con los Tipos de Poliza que se encuentran en la 
            ///               base de datos
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 09-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Llena_Grid_Tipo_Poliza()
            {
                DataTable Dt_Tipo_Poliza; //Variable que obtendra los datos de la consulta 
                try
                {
                    Grid_Tipo_Poliza.DataBind();
                    Dt_Tipo_Poliza = (DataTable)Session["Consulta_Tipo_Poliza"];
                    Grid_Tipo_Poliza.DataSource = Dt_Tipo_Poliza;
                    Grid_Tipo_Poliza.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Llena_Grid_Tipo_Poliza " + ex.Message.ToString(), ex);
                }
            }
        #endregion

        #region (Metodos Validacion)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Validar_Datos_Tipo_Poliza
            /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
            /// CREO        : Yazmin Abigail Delgado Gómez
            /// FECHA_CREO  : 09/Junio/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Validar_Datos_Tipo_Poliza()
        {
            String Espacios_Blanco;
            Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
            Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

            if (string.IsNullOrEmpty(Txt_Abreviacion_Tipo_Poliza.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Abreviatura del Tipo de Poliza es un dato requerido por el sistema. <br>";
                Datos_Validos = false;
            }

            if (string.IsNullOrEmpty(Txt_Descripcion_Tipo_Poliza.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + " + La Descripción del Tipo de Poliza es un dato requerido por el sistema. <br>";
                Datos_Validos = false;
            }
            return Datos_Validos;
        }
        #endregion

        #region (Métodos Operación)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Alta_Tipo_Poliza
            /// DESCRIPCION : Da de Alta del Tipo de Poliza con los datos proporcionados por 
            ///               el usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 09-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Alta_Tipo_Poliza()
            {
                Cls_Cat_Con_Tipo_Polizas_Negocio Rs_Alta_Cat_Con_Tipo_Polizas = new Cls_Cat_Con_Tipo_Polizas_Negocio();  //Variable de conexión hacia la capa de Negocios

                try
                {
                    Rs_Alta_Cat_Con_Tipo_Polizas.P_Abreviacion = Txt_Abreviacion_Tipo_Poliza.Text.Trim();
                    Rs_Alta_Cat_Con_Tipo_Polizas.P_Descripcion = Txt_Descripcion_Tipo_Poliza.Text.Trim();
                    Rs_Alta_Cat_Con_Tipo_Polizas.P_Comentarios = Txt_Comentarios_Tipo_Poliza.Text.Trim();
                    Rs_Alta_Cat_Con_Tipo_Polizas.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

                    Rs_Alta_Cat_Con_Tipo_Polizas.Alta_Tipo_Poliza(); //Da de alto los datos del Tipo de Poliza en la BD
                    Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    Habilitar_Controles("Inicial");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipo de Poliza", "alert('El Alta del Tipo de Poliza fue Exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Alta_Tipo_Poliza " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Modificar_Tipo_Poliza
            /// DESCRIPCION : Modifica los datos del Tipo de Poliza por los datos proporcionados
            ///               por el usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 09-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************0
            private void Modificar_Tipo_Poliza()
            {
                Cls_Cat_Con_Tipo_Polizas_Negocio Rs_Modifica_Cat_Con_Tipo_Polizas = new Cls_Cat_Con_Tipo_Polizas_Negocio();
                try
                {
                    Rs_Modifica_Cat_Con_Tipo_Polizas.P_Tipo_Poliza_ID = Txt_Tipo_Poliza_ID.Text.Trim();
                    Rs_Modifica_Cat_Con_Tipo_Polizas.P_Abreviacion = Txt_Abreviacion_Tipo_Poliza.Text.Trim();
                    Rs_Modifica_Cat_Con_Tipo_Polizas.P_Descripcion = Txt_Descripcion_Tipo_Poliza.Text.Trim();
                    Rs_Modifica_Cat_Con_Tipo_Polizas.P_Comentarios = Txt_Comentarios_Tipo_Poliza.Text.Trim();
                    Rs_Modifica_Cat_Con_Tipo_Polizas.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

                    Rs_Modifica_Cat_Con_Tipo_Polizas.Modificar_Tipo_Poliza();
                    Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    Habilitar_Controles("Inicial");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipo de Poliza", "alert('La Modificación del Tipo de Poliza fue Exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Modificar_Tipo_Poliza " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Eliminar_Tipo_Poliza
            /// DESCRIPCION : Elimina los datos del Tipo de Poliza que fue seleccionada por el Usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 09-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Eliminar_Tipo_Poliza()
            {
                Cls_Cat_Con_Tipo_Polizas_Negocio Rs_Eliminar_Cat_Con_Tipo_Polizas = new Cls_Cat_Con_Tipo_Polizas_Negocio();

                try
                {
                    Rs_Eliminar_Cat_Con_Tipo_Polizas.P_Tipo_Poliza_ID = Txt_Tipo_Poliza_ID.Text.Trim();
                    Rs_Eliminar_Cat_Con_Tipo_Polizas.Eliminar_Tipo_Poliza();//Elimina el Tipo de Poliza seleccionada por el usuario de la BD

                    Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipo de Poliza", "alert('La Eliminación del Tipo de Poliza fue Exitosa');", true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Eliminar_Tipo_Poliza" + ex.Message.ToString(), ex);
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
                    if (Validar_Datos_Tipo_Poliza())
                    {
                        Alta_Tipo_Poliza(); //Da de alta el Tipo de Poliza con los datos que proporciono el usuario
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
                if (Btn_Modificar.ToolTip == "Modificar")
                {
                    //Si el usuario selecciono un Tipo de Poliza entonces habilita los controles para que pueda modificar la información
                    if (!string.IsNullOrEmpty(Txt_Tipo_Poliza_ID.Text.Trim()))
                    {
                        Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                    }
                    //Si el usuario no selecciono un Tipo de Poliza le indica al usuario que la seleccione para poder modificar
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Seleccione el Tipo de Poliza que desea modificar sus datos <br>";
                    }
                }
                else
                {
                    //Si el usuario proporciono todos los datos requeridos entonces modificar los datos del Tipo de Poliza en la BD
                    if (Validar_Datos_Tipo_Poliza())
                    {
                        Modificar_Tipo_Poliza(); //Modifica los datos del Tipo de Poliza con los datos proporcionados por el usuario
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
                //Si el usuario selecciono un Tipo de Poliza entonces la elimina de la base de datos
                if (!string.IsNullOrEmpty(Txt_Tipo_Poliza_ID.Text.Trim()))
                {
                    Eliminar_Tipo_Poliza(); //Elimina el Tipo de Poliza que fue seleccionada por el usuario
                }
                //Si el usuario no selecciono algún Tipo de Poliza manda un mensaje indicando que es necesario que 
                //seleccione alguna para poder eliminar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Tipo de Poliza que desea eliminar <br>";
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
                    Session.Remove("Consulta_Tipo_Polizas");
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
        protected void Btn_Buscar_Descripcion_Tipo_Poliza_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Consulta_Tipo_Poliza(); //Consulta los Tipos de Poliza que coincidan con el nombre porporcionado por el usuario
                //Si no se encontraron Tipos de Poliza con una descripción similar al proporcionado por el usuario entonces manda un mensaje al usuario
                if (Grid_Tipo_Poliza.Rows.Count <= 0)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron Tipos de Polizas con la descripción proporcionada <br>";
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
        protected void Grid_Tipo_Poliza_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Se consultan los Tipos de Poliza que actualmente se encuentran registradas en el sistema.
            Consulta_Tipo_Poliza();

            DataTable Dt_Tipo_Poliza = (Grid_Tipo_Poliza.DataSource as DataTable);

            if (Dt_Tipo_Poliza != null)
            {
                DataView Dv_Tipo_Poliza = new DataView(Dt_Tipo_Poliza);
                String Orden = ViewState["SortDirection"].ToString();

                if (Orden.Equals("ASC"))
                {
                    Dv_Tipo_Poliza.Sort = e.SortExpression + " " + "DESC";
                    ViewState["SortDirection"] = "DESC";
                }
                else
                {
                    Dv_Tipo_Poliza.Sort = e.SortExpression + " " + "ASC";
                    ViewState["SortDirection"] = "ASC";
                }

                Grid_Tipo_Poliza.DataSource = Dv_Tipo_Poliza;
                Grid_Tipo_Poliza.DataBind();
            }
        }
        protected void Grid_Tipo_Poliza_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Limpia_Controles();                          //Limpia los controles de la forma
                Grid_Tipo_Poliza.PageIndex = e.NewPageIndex; //Asigna la nueva página que selecciono el usuario
                Llena_Grid_Tipo_Poliza();                    //Muestra los Tipos de Polizas que estan asignadas en la página seleccionada por el usuario
                Grid_Tipo_Poliza.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Tipo_Poliza_SelectedIndexChanged
        /// DESCRIPCION : Consulta los datos del Tipo de Poliza seleccionado por el usuario
        /// CREO        : Yazmin Abigail Delgado Gómez
        /// FECHA_CREO  : 09-Junio-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Tipo_Poliza_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cls_Cat_Con_Tipo_Polizas_Negocio Rs_Consulta_Con_Tipo_Polizas = new Cls_Cat_Con_Tipo_Polizas_Negocio(); //Variable de conexión hacia la capa de Negocios
            DataTable Dt_Tipo_Poliza; //Variable que obtendra los datos de la consulta 

            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Limpia_Controles(); //Limpia los controles del la forma para poder agregar los valores del registro seleccionado

                Rs_Consulta_Con_Tipo_Polizas.P_Tipo_Poliza_ID = Grid_Tipo_Poliza.SelectedRow.Cells[1].Text;
                Dt_Tipo_Poliza = Rs_Consulta_Con_Tipo_Polizas.Consulta_Datos_Tipo_Poliza(); //Consulta todos los datos del Tipo de Polizas que fue seleccionada por el usuario
                if (Dt_Tipo_Poliza.Rows.Count > 0)
                {
                    //Asigna los valores de los campos obtenidos de la consulta anterior a los controles de la forma
                    foreach (DataRow Registro in Dt_Tipo_Poliza.Rows)
                    {
                        if (!String.IsNullOrEmpty(Registro[Cat_Con_Tipo_Polizas.Campo_Tipo_Poliza_ID].ToString()))
                            Txt_Tipo_Poliza_ID.Text = Registro[Cat_Con_Tipo_Polizas.Campo_Tipo_Poliza_ID].ToString();

                        if (!String.IsNullOrEmpty(Registro[Cat_Con_Tipo_Polizas.Campo_Abreviatura].ToString()))
                            Txt_Abreviacion_Tipo_Poliza.Text = Registro[Cat_Con_Tipo_Polizas.Campo_Abreviatura].ToString();

                        if (!String.IsNullOrEmpty(Registro[Cat_Con_Tipo_Polizas.Campo_Descripcion].ToString()))
                            Txt_Descripcion_Tipo_Poliza.Text = Registro[Cat_Con_Tipo_Polizas.Campo_Descripcion].ToString();

                        if (!String.IsNullOrEmpty(Registro[Cat_Con_Tipo_Polizas.Campo_Comentarios].ToString()))
                            Txt_Comentarios_Tipo_Poliza.Text = Registro[Cat_Con_Tipo_Polizas.Campo_Comentarios].ToString();
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
