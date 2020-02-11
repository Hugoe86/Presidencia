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
using Presidencia.Sap_Partida_Generica.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Text;
using System.Collections.Generic;

public partial class paginas_Paginas_Generales_Frm_Cat_Sap_Partida_Generica : System.Web.UI.Page
{

    #region (Load/Init)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Validan que exista una session activa al ingresar a la página de antiguedad sindicatos.
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Configuracion_Inicial();
                ViewState["SortDirection"] = "ASC";
            }
            Lbl_Mensaje_Error.Text = "";
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    #endregion

    #region (Métodos)

    #region (Métodos Genericos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///DESCRIPCIÓN: Configuracion Inicial del Catalogo de Bancos
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 26/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Habilitar_Controles("Inicial");                 //Habilita la configuración inicial de los controles de  la página.
        Limpiar_Controles();                            //Limpia los controles de la página. 
        Consulta_Sap_Partidas_Genericas();              //Consulta las partidas genericas que se encuantran actualmente registradas en el sistema.
        Consultar_Sap_Capitulos();                      //Consulta los capitulos de SAP.
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Ctlr
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 26/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        Txt_Partida_Generica_ID.Text = "";
        Txt_Clave.Text = "";
        Txt_Comentarios.Text = "";
        Cmb_Estatus.SelectedIndex = -1;
        Cmb_Sap_Capitulo.SelectedIndex = -1;
        Cmb_Sap_Conceptos.SelectedIndex = -1;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 26/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado;

        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    //Campos de Busqueda
                    Txt_Busqueda_Sap_Partidas_Genericas.Enabled = true;
                    Btn_Busqueda_Sap_Partidas_Genericas.Enabled = true;
                    //Campo de Validacion
                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;

                    Configuracion_Acceso("Frm_Cat_Sap_Partida_Generica.aspx");
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
                    //Campos de Busqueda
                    Txt_Busqueda_Sap_Partidas_Genericas.Enabled = true;
                    Btn_Busqueda_Sap_Partidas_Genericas.Enabled = true;
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
                    //Campos de Busqueda
                    Txt_Busqueda_Sap_Partidas_Genericas.Enabled = true;
                    Btn_Busqueda_Sap_Partidas_Genericas.Enabled = true;
                    break;
            }

            Txt_Partida_Generica_ID.Enabled = false;
            Txt_Clave.Enabled = false;
            Txt_Comentarios.Enabled = Habilitado;
            Cmb_Estatus.Enabled = Habilitado;
            Cmb_Sap_Capitulo.Enabled = Habilitado;
            Cmb_Sap_Conceptos.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Metodos Validacion)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Bancos
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 26/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Sap_Partidas_Genericas()
    {
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (string.IsNullOrEmpty(Txt_Clave.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Clave es un dato requerido por el sistema. <br>";
            Datos_Validos = false;
        }

        if (Cmb_Estatus.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Estatus es un dato requerido por el sistema. <br>";
            Datos_Validos = false;
        }

        if (Cmb_Sap_Capitulo.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Capitulo es un dato requerido por el sistema. <br>";
            Datos_Validos = false;
        }

        if (Cmb_Sap_Conceptos.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Concepto es un dato requerido por el sistema. <br>";
            Datos_Validos = false;
        }
        return Datos_Validos;
    }
    #endregion

    #region (Métodos Operación)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Sap_Partida_Generica
    /// DESCRIPCION : Ejecuta el alta de un Sap_Partida_Generica.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 26/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Sap_Partida_Generica()
    {
        Cls_Cat_Sap_Partida_Generica_Negocio Alta_Sap_Partida_Generica = new Cls_Cat_Sap_Partida_Generica_Negocio();//Variable de conexion con la capa de negocios.
        try
        {
            Alta_Sap_Partida_Generica.P_Clave = Txt_Clave.Text.Trim();
            Alta_Sap_Partida_Generica.P_Descripcion = Txt_Comentarios.Text.Trim();
            Alta_Sap_Partida_Generica.P_Estatus = Cmb_Estatus.SelectedItem.Text.Trim();
            Alta_Sap_Partida_Generica.P_Concepto_ID = Cmb_Sap_Conceptos.SelectedValue.Trim();            
            Alta_Sap_Partida_Generica.P_Usuario_Creo = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);

            if (Alta_Sap_Partida_Generica.Alta_Partida_Generica())
            {
                Configuracion_Inicial();
                Limpiar_Controles();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al dar de Alta de la Partida Generica. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Sap_Partida_Generica
    /// DESCRIPCION : Ejecuta la Actualizacion los datos de la Sap_Partida_Generica.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 26/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Sap_Partida_Generica()
    {
        Cls_Cat_Sap_Partida_Generica_Negocio Modificar_Sap_Partida_Generica = new Cls_Cat_Sap_Partida_Generica_Negocio();//Variable de conexion con la capa de negocios.
        try
        {
            Modificar_Sap_Partida_Generica.P_Partida_Generica_ID = Txt_Partida_Generica_ID.Text.Trim();
            Modificar_Sap_Partida_Generica.P_Clave = Txt_Clave.Text.Trim();
            Modificar_Sap_Partida_Generica.P_Descripcion = Txt_Comentarios.Text.Trim();
            Modificar_Sap_Partida_Generica.P_Estatus = Cmb_Estatus.SelectedItem.Text.Trim();
            Modificar_Sap_Partida_Generica.P_Concepto_ID = Cmb_Sap_Conceptos.SelectedValue.Trim();
            Modificar_Sap_Partida_Generica.P_Usuario_Modifico = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);

            if (Modificar_Sap_Partida_Generica.Modificar_Partida_Generica())
            {
                Configuracion_Inicial();
                Limpiar_Controles();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al Actualizar la Sap_Partida_Generica. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Sap_Partida_Generica
    /// DESCRIPCION : Ejecuta la Baja de un Banco
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 26/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Sap_Partida_Generica()
    {
        Cls_Cat_Sap_Partida_Generica_Negocio Eliminar_Sap_Partida_Generica = new Cls_Cat_Sap_Partida_Generica_Negocio();//Variable de conexion con la capa de negocios.
        try
        {
            Eliminar_Sap_Partida_Generica.P_Partida_Generica_ID = Txt_Partida_Generica_ID.Text.Trim();

            if (Eliminar_Sap_Partida_Generica.Baja_Partida_Generica())
            {
                Configuracion_Inicial();
                Limpiar_Controles();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al dar de Baja Partida Generica. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Métodos Consulta)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Sap_Partidas_Genericas
    /// DESCRIPCION : Consulta las Sap_Partidas_Genericas registradas en el sistema.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 26/Febrero/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Sap_Partidas_Genericas()
    {
        Cls_Cat_Sap_Partida_Generica_Negocio Cls_Sap_Partidas_Genericas = new Cls_Cat_Sap_Partida_Generica_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Sap_Partidas_Genericas = null;//Variable que almacenara una lista de las Partidas Genericas registradas.

        try
        {
            if (!string.IsNullOrEmpty(Txt_Busqueda_Sap_Partidas_Genericas.Text))
            {
                Cls_Sap_Partidas_Genericas.P_Partida_Generica_ID = Txt_Busqueda_Sap_Partidas_Genericas.Text.Trim();
                Cls_Sap_Partidas_Genericas.P_Clave = Txt_Busqueda_Sap_Partidas_Genericas.Text.Trim();
            }

            Dt_Sap_Partidas_Genericas = Cls_Sap_Partidas_Genericas.Consultar_Partidas_Genericas();//Consulta las Partidas Genericas que se encuentran registrados en el sistema
            Cargar_Grid_Sap_Partidas_Genericas(Dt_Sap_Partidas_Genericas);//cargamos el grid  de antiguedad sindicatos
            //Validar que la busqueda halla encontrdo resultados.
            if (Grid_Sap_Partidas_Genericas.Rows.Count == 0 && !string.IsNullOrEmpty(Txt_Busqueda_Sap_Partidas_Genericas.Text))
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron registros de Antiguedad de sindicatos con los datos buscados";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las antiguedades de sindicatos registradas en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Sap_Capitulos
    /// DESCRIPCION : Consulta los capitulos registrados en el sistema.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 26/Febrero/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Sap_Capitulos()
    {
        Cls_Cat_Sap_Partida_Generica_Negocio Sap_Capitulos = new Cls_Cat_Sap_Partida_Generica_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Sap_Capitulos = null;//Lista de Capitulos registrados actualmente en el sistema.

        try
        {
            Dt_Sap_Capitulos = Sap_Capitulos.Consultar_Sap_Capitulos();
            Cmb_Sap_Capitulo.DataSource = Dt_Sap_Capitulos;
            Cmb_Sap_Capitulo.DataTextField = "DESCRIPCION";
            Cmb_Sap_Capitulo.DataValueField = "CAPITULO_ID";
            Cmb_Sap_Capitulo.DataBind();
            Cmb_Sap_Capitulo.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
            Cmb_Sap_Capitulo.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los el catálogo de capitulos de SAP. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Sap_Partidas_Genericas
    /// DESCRIPCION : Consulta los conceptos registrados en el sistema.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 26/Febrero/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Sap_Conceptos(String Capitulo_ID)
    {
        Cls_Cat_Sap_Partida_Generica_Negocio Sap_Capitulos_Conceptos = new Cls_Cat_Sap_Partida_Generica_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Sap_Capitulos_Conceptos = null;//Lista de Capitulos registrados actualmente en el sistema.

        try
        {
            Sap_Capitulos_Conceptos.P_Capitulo_ID = Capitulo_ID;
            Dt_Sap_Capitulos_Conceptos = Sap_Capitulos_Conceptos.Consultar_Conceptos_Pertencen_Capitulo();
            Cmb_Sap_Conceptos.DataSource = Dt_Sap_Capitulos_Conceptos;
            Cmb_Sap_Conceptos.DataTextField = "DESCRIPCION";
            Cmb_Sap_Conceptos.DataValueField = "CONCEPTO_ID";
            Cmb_Sap_Conceptos.DataBind();
            Cmb_Sap_Conceptos.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
            Cmb_Sap_Conceptos.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los el catálogo de capitulos de SAP. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Control Acceso Pagina)
    /// *****************************************************************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// 
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
    /// 
    /// PARÁMETROS: No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
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
            Botones.Add(Btn_Busqueda_Sap_Partidas_Genericas);

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

    #endregion

    #region (Grid)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Sap_Partidas_Genericas_PageIndexChanging
    ///DESCRIPCIÓN: Realiza el Cambio de la pagina de la tabla.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 26/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Sap_Partidas_Genericas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Sap_Partidas_Genericas.PageIndex = e.NewPageIndex;
            Consulta_Sap_Partidas_Genericas();
            ScriptManager.RegisterStartupScript(UPnl_Sap_Partidas_Genericas, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Sap_Partidas_Genericas();", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error cambiar de un de la tabla. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Sap_Partidas_Genericas_SelectedIndexChanged
    ///DESCRIPCIÓN: Realiza la seleccion de un elemento de la tabla
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 26/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Sap_Partidas_Genericas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Sap_Partida_Generica_Negocio Consulta_Sap_Partidas_Genericas = new Cls_Cat_Sap_Partida_Generica_Negocio();//Variable de conexion con l capa de negocios.
        DataTable Dt_Sap_Partidas_Genericas = null;//Variable que almacenara una lista de los bancos registrados en el sistema.
        String Partida_Generica_ID = "";//Variable que almacena el identificador único del banco.

        try
        {
            int Fila_Seleccionada = Grid_Sap_Partidas_Genericas.SelectedIndex;

            if (Fila_Seleccionada != -1)
            {
                GridViewRow selectedRow = Grid_Sap_Partidas_Genericas.Rows[Grid_Sap_Partidas_Genericas.SelectedIndex];
                Partida_Generica_ID = HttpUtility.HtmlDecode(selectedRow.Cells[2].Text).ToString().Trim();

                Consulta_Sap_Partidas_Genericas.P_Clave = Partida_Generica_ID;
                Dt_Sap_Partidas_Genericas = Consulta_Sap_Partidas_Genericas.Consultar_Partidas_Genericas();

                if (Dt_Sap_Partidas_Genericas is DataTable)
                {
                    if (Dt_Sap_Partidas_Genericas.Rows.Count > 0)
                    {
                        foreach (DataRow Partida_Generica in Dt_Sap_Partidas_Genericas.Rows)
                        {
                            if (!string.IsNullOrEmpty(Partida_Generica[Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID].ToString()))
                                Txt_Partida_Generica_ID.Text = Partida_Generica[Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID].ToString();

                            if (!string.IsNullOrEmpty(Partida_Generica[Cat_Sap_Partidas_Genericas.Campo_Clave].ToString()))
                                Txt_Clave.Text = Partida_Generica[Cat_Sap_Partidas_Genericas.Campo_Clave].ToString();

                            if (!string.IsNullOrEmpty(Partida_Generica[Cat_Sap_Partidas_Genericas.Campo_Descripcion].ToString()))
                                Txt_Comentarios.Text = Partida_Generica[Cat_Sap_Partidas_Genericas.Campo_Descripcion].ToString();

                            if (!string.IsNullOrEmpty(Partida_Generica[Cat_Sap_Partidas_Genericas.Campo_Estatus].ToString()))
                                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText(Partida_Generica[Cat_Sap_Partidas_Genericas.Campo_Estatus].ToString()));

                            if (!string.IsNullOrEmpty(Partida_Generica[Cat_Sap_Partidas_Genericas.Campo_Concepto_ID].ToString()))
                            {
                                
                                Consulta_Sap_Partidas_Genericas.P_Concepto_ID = Partida_Generica[Cat_Sap_Partidas_Genericas.Campo_Concepto_ID].ToString();
                                DataTable Dt_Sap_Capitulo = Consulta_Sap_Partidas_Genericas.Consultar_Capitulo_Concepto();

                                if (Dt_Sap_Capitulo is DataTable) {
                                    foreach (DataRow Sap_Capitulo in Dt_Sap_Capitulo.Rows) {
                                        if (Sap_Capitulo is DataRow)
                                        {
                                            if (!string.IsNullOrEmpty(Sap_Capitulo["CAPITULO_ID"].ToString()))
                                            {
                                                Cmb_Sap_Capitulo.SelectedIndex = Cmb_Sap_Capitulo.Items.IndexOf(Cmb_Sap_Capitulo.Items.FindByValue(Sap_Capitulo["CAPITULO_ID"].ToString()));

                                                Consultar_Sap_Conceptos(Cmb_Sap_Capitulo.SelectedValue.Trim());
                                                Cmb_Sap_Conceptos.SelectedIndex = Cmb_Sap_Conceptos.Items.IndexOf(Cmb_Sap_Conceptos.Items.FindByValue(Partida_Generica[Cat_Sap_Partidas_Genericas.Campo_Concepto_ID].ToString()));
                                            }
                                        }
                                    }
                                }   
                            }
                        }
                    }
                }
            }
            ScriptManager.RegisterStartupScript(UPnl_Sap_Partidas_Genericas, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Sap_Partidas_Genericas();", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error seleccionar un elemento de la tabla. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Grid_Sap_Partidas_Genericas
    /// DESCRIPCION : Carga las Partidas Genericas registrados en el sistema. 
    ///               registradas en el sistema.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 26/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Grid_Sap_Partidas_Genericas(DataTable Dt_Sap_Partidas_Genericas)
    {
        try
        {
            Grid_Sap_Partidas_Genericas.Columns[1].Visible = true;
            Grid_Sap_Partidas_Genericas.DataSource = Dt_Sap_Partidas_Genericas;
            Grid_Sap_Partidas_Genericas.DataBind();
            Grid_Sap_Partidas_Genericas.SelectedIndex = -1;
            Grid_Sap_Partidas_Genericas.Columns[1].Visible = false;
            Limpiar_Controles();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar las Partidas Genricas en la tabla que los listara. Error: [" + Ex.Message + "]");
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Sap_Partidas_Genericas_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 26/Febrero/2011
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Sap_Partidas_Genericas_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            Consulta_Sap_Partidas_Genericas();
            DataTable Dt_Sap_Partidas_Genericas = (Grid_Sap_Partidas_Genericas.DataSource as DataTable);

            if (Dt_Sap_Partidas_Genericas != null)
            {
                DataView Dv_Sap_Partidas_Genericas = new DataView(Dt_Sap_Partidas_Genericas);
                String Orden = ViewState["SortDirection"].ToString();

                if (Orden.Equals("ASC"))
                {
                    Dv_Sap_Partidas_Genericas.Sort = e.SortExpression + " " + "DESC";
                    ViewState["SortDirection"] = "DESC";
                }
                else
                {
                    Dv_Sap_Partidas_Genericas.Sort = e.SortExpression + " " + "ASC";
                    ViewState["SortDirection"] = "ASC";
                }

                Grid_Sap_Partidas_Genericas.DataSource = Dv_Sap_Partidas_Genericas;
                Grid_Sap_Partidas_Genericas.DataBind();
            }
            ScriptManager.RegisterStartupScript(UPnl_Sap_Partidas_Genericas, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Sap_Partidas_Genericas();", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al ejecutar el reorden de la columna ya sea en forma ASC o DESC. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Eventos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Alta Sap Partida Generica
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 26/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip.Equals("Nuevo"))
            {
                Habilitar_Controles("Nuevo");
                Limpiar_Controles();
            }
            else
            {
                if (Validar_Datos_Sap_Partidas_Genericas())
                {
                    Alta_Sap_Partida_Generica();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(UPnl_Sap_Partidas_Genericas, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Sap_Partidas_Genericas();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Modificar Sap Partida Generica
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 26/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Btn_Modificar.ToolTip.Equals("Modificar"))
            {
                if (Grid_Sap_Partidas_Genericas.SelectedIndex != -1 & !Txt_Partida_Generica_ID.Text.Equals(""))
                {
                    Habilitar_Controles("Modificar");
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
                if (Validar_Datos_Sap_Partidas_Genericas())
                {
                    Modificar_Sap_Partida_Generica();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(UPnl_Sap_Partidas_Genericas, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Sap_Partidas_Genericas();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Eliminar Sap Partida Generica
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 26/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Btn_Eliminar.ToolTip.Equals("Eliminar"))
            {
                if (Grid_Sap_Partidas_Genericas.SelectedIndex != -1 & !Txt_Partida_Generica_ID.Text.Equals(""))
                {
                    Eliminar_Sap_Partida_Generica();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea eliminar <br>";
                }
            }
            ScriptManager.RegisterStartupScript(UPnl_Sap_Partidas_Genericas, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Sap_Partidas_Genericas();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Salir de la Operacion Actual
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 26/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Inicio")
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Configuracion_Inicial();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
            ScriptManager.RegisterStartupScript(UPnl_Sap_Partidas_Genericas, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Sap_Partidas_Genericas();", true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Sap_Partidas_Genericas_Click
    ///DESCRIPCIÓN: Busqueda Partidas Genericas en el sistema.
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 26/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Sap_Partidas_Genericas_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Consulta_Sap_Partidas_Genericas();
            ScriptManager.RegisterStartupScript(UPnl_Sap_Partidas_Genericas, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Sap_Partidas_Genericas();", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error Ejecutar la Búsqueda de Partidas Genericas en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Sap_Capitulo_SelectedIndexChanged
    ///DESCRIPCIÓN: Carga los conceptos de acuerdo al capitulo seleccionado.
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 26/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Sap_Capitulo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Sap_Capitulo.SelectedIndex > 0)
            {
                Consultar_Sap_Conceptos(Cmb_Sap_Capitulo.SelectedValue.Trim());
            }
            else
            {
                Cmb_Sap_Conceptos.DataSource = new DataTable();
                Cmb_Sap_Conceptos.DataBind();
            }
            ScriptManager.RegisterStartupScript(UPnl_Sap_Partidas_Genericas, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Sap_Partidas_Genericas();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Sap_Conceptos_SelectedIndexChanged
    ///DESCRIPCIÓN: Habilita el campo para ingresar la clave de la partida generica. Y valida 
    ///             que la clave de la partida generica no sea menor o igual a la del concepto
    ///             al que pertence.
    ///             
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 26/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Sap_Conceptos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Sap_Partida_Generica_Negocio Sap_Partidas_Genericas = new Cls_Cat_Sap_Partida_Generica_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Sap_Partidas_Genricas = null;//Variable que lista  las partidas genericas.
        Int64 Clave_Concepto = 0;//Variable que almacena la clave del concepto.

        try
        {
            Sap_Partidas_Genericas.P_Concepto_ID = Cmb_Sap_Conceptos.SelectedValue.Trim();
            Dt_Sap_Partidas_Genricas = Sap_Partidas_Genericas.Consultar_Conceptos_Pertencen_Capitulo();

            if (Dt_Sap_Partidas_Genricas is DataTable) {
                foreach (DataRow Sap_Partida_Genrica in Dt_Sap_Partidas_Genricas.Rows)
                {
                    if (Sap_Partida_Genrica is DataRow)
                    {
                        if (!String.IsNullOrEmpty(Sap_Partida_Genrica["CLAVE"].ToString()))
                        {
                            Txt_Clave_Oculta.Value = Sap_Partida_Genrica["CLAVE"].ToString();
                            Txt_Clave.Enabled = true;
                        }
                    }
                }
            }

            ScriptManager.RegisterStartupScript(UPnl_Sap_Partidas_Genericas, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Sap_Partidas_Genericas();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Clave_TextChanged
    ///DESCRIPCIÓN: Valida que la clave de la partida generica no sea menor o igual a la del concepto
    ///             al que pertence.
    ///             
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 26/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Clave_TextChanged(object sender, EventArgs e)
    {
        Int64 Clave_Concepto = 0;
        Int64 Clave_Partida_Generica = 0;
        try
        {
            if (!string.IsNullOrEmpty(Txt_Clave_Oculta.Value.Trim()))
            {
                Clave_Concepto = Convert.ToInt64(Txt_Clave_Oculta.Value.Trim());
                Clave_Partida_Generica = Convert.ToInt64(Txt_Clave.Text.Trim());

                if (Clave_Partida_Generica <= Clave_Concepto) {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "+ La Clave de la partida generica debe ser mayor a la clave del concepto seleccionado. Clave Concepto: [" + Clave_Concepto + "]<br />";
                }
            }
        }
        catch (Exception Ex)
        {

            throw new Exception("Error al validar que la clave a ingresar sea mayor a la de su ascendencia. Error: [" + Ex.Message + "]");
        }
    }
    #endregion
}
