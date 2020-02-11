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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Collections.Generic;
using Presidencia.Reloj_Checador.Negocios;

public partial class paginas_Nomina_Frm_Cat_Nom_Reloj_Checador : System.Web.UI.Page
{
    #region (Page Load)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
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
    #region (Control Acceso Pagina)
        ///*******************************************************************************
        ///NOMBRE: Configuracion_Acceso
        ///DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
        ///PARÁMETROS: No Áplica.
        ///USUARIO CREÓ: Juan Alberto Hernández Negrete.
        ///FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
        ///USUARIO MODIFICO:
        ///FECHA MODIFICO:
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
                Botones.Add(Btn_Buscar_Reloj_Checador);

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
    #region (Metodos Generales)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Inicializa_Controles
        /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
        ///               diferentes operaciones
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
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
                Consulta_Reloj_Checadores();    //Consulta todas los reloj checadores que fueron dadas de alta en la BD
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
        /// FECHA_CREO  : 17-Julio-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Limpia_Controles()
        {
            try
            {
                Txt_Reloj_Checador_ID.Text = "";
                Txt_Busqueda_Reloj_Checador.Text = "";
                Txt_Clave_Reloj_Checador.Text = "";
                Txt_Ubicacion_Reloj_Checador.Text = "";
                Txt_Comentarios_Reloj_Checador.Text = "";
            }
            catch (Exception ex)
            {
                throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Habilitar_Controles
        /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
        ///               para a siguiente operación
        /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
        ///                          si es una alta, modificacion
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 17-Julio-2010
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

                        Configuracion_Acceso("Frm_Cat_Nom_Reloj_Checador.aspx");
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
                Txt_Clave_Reloj_Checador.Enabled = Habilitado;
                Txt_Ubicacion_Reloj_Checador.Enabled = Habilitado;
                Txt_Comentarios_Reloj_Checador.Enabled = Habilitado;
                Txt_Busqueda_Reloj_Checador.Enabled = !Habilitado;
                Btn_Buscar_Reloj_Checador.Enabled = !Habilitado;
                Grid_Reloj_Checador.Enabled = !Habilitado;
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
            }
            catch (Exception ex)
            {
                throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Reloj_Checadores
        /// DESCRIPCION : Consulta los Reloj Chechadores que estan dadas de alta en la BD
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 17-Julio-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Consulta_Reloj_Checadores()
        {
            Cls_Cat_Nom_Reloj_Checador_Negocio Rs_Consulta_Cat_Nom_Reloj_Checador = new Cls_Cat_Nom_Reloj_Checador_Negocio(); //Variable de conexión hacia la capa de Negocios
            DataTable Dt_Reloj_Checadores; //Variable que obtendra los datos de la consulta 

            try
            {
                if (Txt_Busqueda_Reloj_Checador.Text != "")
                {
                    Rs_Consulta_Cat_Nom_Reloj_Checador.P_Clave = Txt_Busqueda_Reloj_Checador.Text;
                }
                Dt_Reloj_Checadores = Rs_Consulta_Cat_Nom_Reloj_Checador.Consulta_Datos_Reloj_Checador(); //Consulta todos los reloj checadores con sus datos generales
                Session["Consulta_Reloj_Checador"] = Dt_Reloj_Checadores;
                Llena_Grid_Reloj_Checador();
            }
            catch (Exception ex)
            {
                throw new Exception("Consulta_Reloj_Checadores " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Llena_Grid_Reloj_Checador
        /// DESCRIPCION : Llena el grid con los reloj Checadores que se encuentran en la BD
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 17-Julio-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Llena_Grid_Reloj_Checador()
        {
            DataTable Dt_Reloj_Checadores; //Variable que obtendra los datos de la consulta 
            try
            {
                Grid_Reloj_Checador.Columns[1].Visible = true;
                Grid_Reloj_Checador.Columns[4].Visible = true;
                Grid_Reloj_Checador.DataBind();
                Dt_Reloj_Checadores = (DataTable)Session["Consulta_Reloj_Checador"];
                Grid_Reloj_Checador.DataSource = Dt_Reloj_Checadores;
                Grid_Reloj_Checador.DataBind();
                Grid_Reloj_Checador.Columns[1].Visible = false;
                Grid_Reloj_Checador.Columns[4].Visible = false;
            }
            catch (Exception ex)
            {
                throw new Exception("Llena_Grid_Reloj_Checador " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Validar_Datos_Reloj_Checador
        /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 21/Julio/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private Boolean Validar_Datos_Reloj_Checador()
        {
            Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

            if (string.IsNullOrEmpty(Txt_Clave_Reloj_Checador.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Clave del Reloj Checador <br>";
                Datos_Validos = false;
            }

            if (string.IsNullOrEmpty(Txt_Ubicacion_Reloj_Checador.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Ubicación es un dato requerido por el sistema. <br>";
                Datos_Validos = false;
            }
            return Datos_Validos;
        }
    #endregion
    #region (Métodos Operación)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Reloj_Checador
        /// DESCRIPCION : Da de Alta el registro del Reloj Checador con los datos 
        /// proporcionados por el usuario
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 17-Julio-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Alta_Reloj_Checador()
        {
            Cls_Cat_Nom_Reloj_Checador_Negocio Rs_Alta_Cat_Nom_Reloj_Checador = new Cls_Cat_Nom_Reloj_Checador_Negocio(); //Variable para la conexión hacia la capa de negocios

            try
            {
                Rs_Alta_Cat_Nom_Reloj_Checador.P_Clave = Txt_Clave_Reloj_Checador.Text.ToString();
                Rs_Alta_Cat_Nom_Reloj_Checador.P_Ubicacion = Txt_Ubicacion_Reloj_Checador.Text.ToString();
                Rs_Alta_Cat_Nom_Reloj_Checador.P_Comentarios = Txt_Comentarios_Reloj_Checador.Text.ToString();
                Rs_Alta_Cat_Nom_Reloj_Checador.Alta_Reloj_Checador();

                Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                Habilitar_Controles("Inicial");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Reloj Checador", "alert('El Alta de Reloj Checador fue Exitosa');", true);
            }
            catch (Exception ex)
            {
                throw new Exception("Alta_Reloj_Checador " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Reloj_Checador
        /// DESCRIPCION : Modifica los datos del Reloj Checador con los proporcionados por
        ///               el usuario en la BD
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 21-Julio-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Modificar_Reloj_Checador()
        {
            Cls_Cat_Nom_Reloj_Checador_Negocio Rs_Modificar_Cat_Nom_Reloj_Checador = new Cls_Cat_Nom_Reloj_Checador_Negocio();  //Variable de conexión hacia la capa de Negocios
            try
            {
                Rs_Modificar_Cat_Nom_Reloj_Checador.P_Reloj_Checador_ID = Txt_Reloj_Checador_ID.Text;
                Rs_Modificar_Cat_Nom_Reloj_Checador.P_Clave = Txt_Clave_Reloj_Checador.Text;
                Rs_Modificar_Cat_Nom_Reloj_Checador.P_Ubicacion = Txt_Ubicacion_Reloj_Checador.Text;
                Rs_Modificar_Cat_Nom_Reloj_Checador.P_Comentarios = Txt_Comentarios_Reloj_Checador.Text;
                Rs_Modificar_Cat_Nom_Reloj_Checador.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

                Rs_Modificar_Cat_Nom_Reloj_Checador.Modificar_Reloj_Checador(); //Sustituye los datos del reloj que se encuentran en la BD por los que fueron proporcionados por el usuario
                Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones            
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Reloj Checador", "alert('La Modificación del Reloj Checador fue Exitosa');", true);
            }
            catch (Exception ex)
            {
                throw new Exception("Modificar_Reloj_Checador " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Eliminar_Reloj_Checador
        /// DESCRIPCION : Elimina los datos de la Reloj Checador que fue seleccionada por el Usuario
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 21-Julio-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Eliminar_Reloj_Checador()
        {
            Cls_Cat_Nom_Reloj_Checador_Negocio Rs_Eliminar_Cat_Nom_Reloj_Checador = new Cls_Cat_Nom_Reloj_Checador_Negocio();  //Variable de conexión hacia la capa de Negocios
            try
            {
                Rs_Eliminar_Cat_Nom_Reloj_Checador.P_Reloj_Checador_ID = Txt_Reloj_Checador_ID.Text;
                Rs_Eliminar_Cat_Nom_Reloj_Checador.Elimina_Reloj_Checador(); //Elimina el reloj checador seleccionado por el usuario de la BD

                Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Reloj Checador", "alert('La Eliminación del Reloj Checador fue Exitosa');", true);
            }
            catch (Exception ex)
            {
                throw new Exception("Eliminar_Reloj_Checador " + ex.Message.ToString(), ex);
            }
        }
    #endregion
    #region (Eventos Operación)
        protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
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
                if (Validar_Datos_Reloj_Checador())
                {
                    Alta_Reloj_Checador(); //Da de alta el registro del Reloj con los datos que proporciono el usuario
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
                //Si el usuario selecciono un registrado entonces habilita los controles para que pueda modificar la información
                if (Txt_Reloj_Checador_ID.Text != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                //Si el usuario no selecciono un reloj le indica al usuario que la seleccione para poder modificar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Reloj Checador que desea modificar sus datos <br>";
                }
            }
            else
            {
                //Si el usuario proporciono todos los datos requeridos entonces modificar los datos del reloj checador en la BD
                if (Validar_Datos_Reloj_Checador())
                {
                    Modificar_Reloj_Checador(); //Modifica los datos del registro del reloj checador con los datos proporcionados por el usuario
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
            //Si el usuario selecciono un reloj checador entonces la elimina de la base de datos
            if (Txt_Reloj_Checador_ID.Text != "")
            {
                Eliminar_Reloj_Checador(); //Elimina el Reloj Checador que fue seleccionada por el usuario
            }
            //Si el usuario no selecciono algún Reloj Checador manda un mensaje indicando que es necesario que seleccione alguna para
            //poder eliminar
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el Reloj Checador que desea eliminar <br>";
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
                    Session.Remove("Consulta_Reloj_Checador");
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
        protected void Btn_Buscar_Reloj_Checador_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Consulta_Reloj_Checadores(); //Consulta los reloj checadores que coincidan con el nombre porporcionado por el usuario
                Limpia_Controles();
                //Si no se encontraron reloj checador con un nombre similar al proporcionado por el usuario entonces manda un mensaje al usuario
                if (Grid_Reloj_Checador.Rows.Count <= 0)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron Reloj Checador con el nombre proporcionado <br>";
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
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Reloj_Checador_SelectedIndexChanged
        /// DESCRIPCION : Consulta los datos del reloj que selecciono el usuario
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 15-Julio-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Reloj_Checador_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Limpia_Controles();
                Grid_Reloj_Checador.Columns[1].Visible = true;
                Grid_Reloj_Checador.Columns[4].Visible = true;
                Txt_Reloj_Checador_ID.Text = Grid_Reloj_Checador.SelectedRow.Cells[1].Text;
                Txt_Clave_Reloj_Checador.Text = HttpUtility.HtmlDecode(Grid_Reloj_Checador.SelectedRow.Cells[2].Text);
                Txt_Ubicacion_Reloj_Checador.Text = HttpUtility.HtmlDecode(Grid_Reloj_Checador.SelectedRow.Cells[3].Text);
                Txt_Comentarios_Reloj_Checador.Text = HttpUtility.HtmlDecode(Grid_Reloj_Checador.SelectedRow.Cells[4].Text);
                Grid_Reloj_Checador.Columns[1].Visible = false;
                Grid_Reloj_Checador.Columns[4].Visible = false;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }

        }
        /// **************************************************************************************************************************************
        /// NOMBRE: Grid_Reloj_Checador_Sorting
        /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
        /// CREÓ:   Yazmin Delgado Gómex
        /// FECHA CREÓ: 21/Julio/2011
        /// MODIFICÓ:
        /// FECHA MODIFICÓ:
        /// CAUSA MODIFICACIÓN:
        /// **************************************************************************************************************************************
        protected void Grid_Reloj_Checador_Sorting(object sender, GridViewSortEventArgs e)
        {
            Consulta_Reloj_Checadores();
            DataTable Dt_Reloj = (Grid_Reloj_Checador.DataSource as DataTable);

            if (Dt_Reloj != null)
            {
                DataView Dv_Reloj = new DataView(Dt_Reloj);
                String Orden = ViewState["SortDirection"].ToString();

                if (Orden.Equals("ASC"))
                {
                    Dv_Reloj.Sort = e.SortExpression + " " + "DESC";
                    ViewState["SortDirection"] = "DESC";
                }
                else
                {
                    Dv_Reloj.Sort = e.SortExpression + " " + "ASC";
                    ViewState["SortDirection"] = "ASC";
                }

                Grid_Reloj_Checador.DataSource = Dv_Reloj;
                Grid_Reloj_Checador.DataBind();
            }
        }
        protected void Grid_Reloj_Checador_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Limpia_Controles();                             //Limpia todos los controles de la forma
                Grid_Reloj_Checador.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Llena_Grid_Reloj_Checador();                    //Carga las Escolaridades que estan asignadas a la página seleccionada
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
