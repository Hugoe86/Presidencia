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
using Presidencia.Bancos_Nomina.Negocio;
using System.Collections.Generic;

public partial class paginas_Nomina_Frm_Cat_Nom_Bancos : System.Web.UI.Page
{
    #region(Init/Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
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

    #region(Métodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///DESCRIPCIÓN: Configuracion Inicial del Catalogo de Bancos
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Habilitar_Controles("Inicial");
        Limpiar_Controles();
        Consulta_Bancos();
        LLenar_Combo_Meses();
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Ctlr
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 17/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        Txt_Banco_ID.Text = "";
        Txt_Nombre_Banco.Text = "";
        Txt_No_Cuenta.Text = "";
        Txt_Sucursal.Text = "";
        Txt_Referencia.Text = "";
        Txt_Comentarios.Text = "";
        //Cmb_Tipo.SelectedIndex = -1;
        Cmb_Plan_Pagos.SelectedIndex = -1;
        Cmb_No_Meses.SelectedIndex = -1;

        Grid_Bancos.SelectedIndex = -1;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 17/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado;
        Boolean Tipo_Usuario;

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
                    Txt_Busqueda_Bancos.Enabled = true;
                    Btn_Busqueda_Bancos.Enabled = true;
                    //Campo de Validacion
                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;

                    Configuracion_Acceso("Frm_Cat_Nom_Bancos.aspx");
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
                    Txt_Busqueda_Bancos.Enabled = true;
                    Btn_Busqueda_Bancos.Enabled = true;
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
                    Txt_Busqueda_Bancos.Enabled = true;
                    Btn_Busqueda_Bancos.Enabled = true;
                    break;
            }
            Txt_Banco_ID.Enabled = false;
            Txt_Nombre_Banco.Enabled = Habilitado;
            Txt_No_Cuenta.Enabled = Habilitado;
            Txt_Sucursal.Enabled = Habilitado;
            Txt_Referencia.Enabled = Habilitado;
            Txt_Comentarios.Enabled = Habilitado;
            Cmb_Tipo.Enabled = Habilitado;
            Cmb_Plan_Pagos.Enabled = Habilitado;
            Cmb_No_Meses.Enabled = Habilitado;

            Grid_Bancos.Enabled = !Habilitado;

            //  para saber si el tipo de usuario es de recursos humano
            Tipo_Usuario = (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? true : false;

            // si es recursos humanos
            if (Tipo_Usuario)
            {
                Tbl_Plan_Pagos.Style.Add("display", "none");
                //  se deshabilita el combo y se cara en nomina
                Cmb_Tipo.Enabled = false;
                Cmb_Tipo.SelectedIndex = 1;
            }
            //  otro usuario
            else
            {
                //  se habilita el combo y se permite al usuario seleccionar el tipo
                Tbl_Plan_Pagos.Style.Add("display", "block");
                Cmb_Tipo.Enabled = Habilitado;
                Cmb_Tipo.SelectedIndex = -1;
            }
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
    /// FECHA_CREO  : 17/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Bancos()
    {
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (string.IsNullOrEmpty(Txt_Nombre_Banco.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Nombre es un dato requerido por el sistema. <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_No_Cuenta.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Número de Cuenta es un dato requerido por el sistema. <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Sucursal.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La sucursal es un dato requerido por el sistema. <br>";
            Datos_Validos = false;
        }

        //if (string.IsNullOrEmpty(Txt_Referencia.Text))
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La referencia es un dato requerido por el sistema. <br>";
        //    Datos_Validos = false;
        //}

        if (Cmb_Tipo.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione si el banco a dar de alta sera para nómina o ingresos. <br>";
            Datos_Validos = false;
        }
        return Datos_Validos;
    }
    #endregion

    #region (Metodos Consulta)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Antiguedad_Sindicatos
    /// DESCRIPCION : Consulta las antiguedades de sindicato registradas en el sistema.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Bancos()
    {
        Cls_Cat_Nom_Bancos_Negocio Cls_Bancos = new Cls_Cat_Nom_Bancos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Bancos = null;//Variable que almacenara una lista de las antiguedades para los sindicatos registradas.

        try
        {
            if (!string.IsNullOrEmpty(Txt_Busqueda_Bancos.Text))
            {
                Cls_Bancos.P_Banco_ID= Txt_Busqueda_Bancos.Text.Trim();
                Cls_Bancos.P_Nombre = Txt_Busqueda_Bancos.Text.Trim();
            }

            Dt_Bancos = Cls_Bancos.Consulta_Bancos();//Consulta los Bancos que se encuentran registrados en el sistema
            Cargar_Grid_Bancos(Dt_Bancos);//cargamos el grid  de antiguedad sindicatos
            //Validar que la busqueda halla encontrdo resultados.
            if (Grid_Bancos.Rows.Count == 0 && !string.IsNullOrEmpty(Txt_Busqueda_Bancos.Text))
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
    #endregion

    #region (Metodos Operacion)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Banco
    /// DESCRIPCION : Ejecuta el alta de un Banco.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 17/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Banco()
    {
        Cls_Cat_Nom_Bancos_Negocio Alta_Banco = new Cls_Cat_Nom_Bancos_Negocio();//Variable de conexion con la capa de negocios.
        try
        {
            Alta_Banco.P_Nombre = Txt_Nombre_Banco.Text.Trim();
            Alta_Banco.P_No_Cuenta = Txt_No_Cuenta.Text.Trim();
            Alta_Banco.P_Sucursal = Txt_Sucursal.Text.Trim();
            Alta_Banco.P_Referencia = Txt_Referencia.Text.Trim();
            Alta_Banco.P_Comentarios = Txt_Comentarios.Text.Trim();
            Alta_Banco.P_Tipo = Cmb_Tipo.SelectedValue.Trim();
            Alta_Banco.P_Plan_Pago = Cmb_Plan_Pagos.SelectedValue.Trim();
            Alta_Banco.P_No_Meses = Cmb_No_Meses.SelectedValue.Trim();

            Alta_Banco.P_Usuario_Creo = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);

            if (Alta_Banco.Alta_Banco())
            {
                Configuracion_Inicial();
                Limpiar_Controles();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al dar de Alta Antiguedad Sindicato. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Banco
    /// DESCRIPCION : Ejecuta la Actualizacion los datos del Banco.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 17/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Banco()
    {
        Cls_Cat_Nom_Bancos_Negocio Modificar_Banco = new Cls_Cat_Nom_Bancos_Negocio();//Variable de conexion con la capa de negocios.
        try
        {
            Modificar_Banco.P_Banco_ID = Txt_Banco_ID.Text.Trim();
            Modificar_Banco.P_Nombre = Txt_Nombre_Banco.Text.Trim();
            Modificar_Banco.P_No_Cuenta = Txt_No_Cuenta.Text.Trim();
            Modificar_Banco.P_Sucursal = Txt_Sucursal.Text.Trim();
            Modificar_Banco.P_Referencia = Txt_Referencia.Text.Trim();
            Modificar_Banco.P_Comentarios = Txt_Comentarios.Text.Trim();
            Modificar_Banco.P_Tipo = Cmb_Tipo.SelectedValue.Trim();
            Modificar_Banco.P_Plan_Pago = Cmb_Plan_Pagos.SelectedValue.Trim();
            Modificar_Banco.P_No_Meses = Cmb_No_Meses.SelectedValue.Trim();

            Modificar_Banco.P_Usuario_Creo = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);

            if (Modificar_Banco.Modificar_Banco())
            {
                Configuracion_Inicial();
                Limpiar_Controles();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al Actualizar Antiguedad Sindicato. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Banco
    /// DESCRIPCION : Ejecuta la Baja de un Banco
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 17/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Banco()
    {
        Cls_Cat_Nom_Bancos_Negocio Eliminar_Banco = new Cls_Cat_Nom_Bancos_Negocio();//Variable de conexion con la capa de negocios.
        try
        {
            Eliminar_Banco.P_Banco_ID = Txt_Banco_ID.Text.Trim();

            if (Eliminar_Banco.Eliminar_Banco())
            {
                Configuracion_Inicial();
                Limpiar_Controles();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al dar de Baja a un Banco. Error: [" + Ex.Message + "]");
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
            Botones.Add(Btn_Busqueda_Bancos);

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

    #region(Grid)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Bancos_PageIndexChanging
    ///DESCRIPCIÓN: Realiza el Cambio de la pagina de la tabla.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Bancos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Bancos.PageIndex = e.NewPageIndex;
            Consulta_Bancos();
            ScriptManager.RegisterStartupScript(UPnl_Bancos, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Bancos();", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error cambiar de un de la tabla. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Bancos_SelectedIndexChanged
    ///DESCRIPCIÓN: Realiza la seleccion de un elemento de la tabla
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Bancos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Nom_Bancos_Negocio Consulta_Bancos = new Cls_Cat_Nom_Bancos_Negocio();//Variable de conexion con l capa de negocios.
        DataTable Dt_Bancos = null;//Variable que almacenara una lista de los bancos registrados en el sistema.
        String Banco_ID = "";//Variable que almacena el identificador único del banco.

        try
        {
            int Fila_Seleccionada = Grid_Bancos.SelectedIndex;

            if (Fila_Seleccionada != -1)
            {
                Banco_ID = HttpUtility.HtmlDecode(Grid_Bancos.Rows[Fila_Seleccionada].Cells[1].Text);

                Consulta_Bancos.P_Banco_ID = Banco_ID;
                Dt_Bancos = Consulta_Bancos.Consulta_Bancos();

                if (Dt_Bancos is DataTable) {
                    if (Dt_Bancos.Rows.Count > 0) {
                        foreach (DataRow Renglon in Dt_Bancos.Rows) {
                            if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Bancos.Campo_Banco_ID].ToString()))
                                Txt_Banco_ID.Text = Renglon[Cat_Nom_Bancos.Campo_Banco_ID].ToString();

                            if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Bancos.Campo_Nombre].ToString()))
                                Txt_Nombre_Banco.Text = Renglon[Cat_Nom_Bancos.Campo_Nombre].ToString();

                            if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Bancos.Campo_No_Cuenta].ToString()))
                                Txt_No_Cuenta.Text = Renglon[Cat_Nom_Bancos.Campo_No_Cuenta].ToString();

                            if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Bancos.Campo_Sucursal].ToString()))
                                Txt_Sucursal.Text = Renglon[Cat_Nom_Bancos.Campo_Sucursal].ToString();

                            if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Bancos.Campo_Referencia].ToString()))
                                Txt_Referencia.Text = Renglon[Cat_Nom_Bancos.Campo_Referencia].ToString();

                            if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Bancos.Campo_Comentarios].ToString()))
                                Txt_Comentarios.Text = Renglon[Cat_Nom_Bancos.Campo_Comentarios].ToString();

                            if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Bancos.Campo_Tipo].ToString()))
                                Cmb_Tipo.SelectedIndex = Cmb_Tipo.Items.IndexOf(Cmb_Tipo.Items.FindByValue(Renglon[Cat_Nom_Bancos.Campo_Tipo].ToString()));

                            if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Bancos.Campo_Plan_Pago].ToString()))
                                Cmb_Plan_Pagos.SelectedIndex = Cmb_Plan_Pagos.Items.IndexOf(Cmb_Plan_Pagos.Items.FindByValue(Renglon[Cat_Nom_Bancos.Campo_Plan_Pago].ToString()));

                            if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Bancos.Campo_No_Meses].ToString()))
                                Cmb_No_Meses.SelectedIndex = Cmb_No_Meses.Items.IndexOf(Cmb_No_Meses.Items.FindByValue(Renglon[Cat_Nom_Bancos.Campo_No_Meses].ToString()));
                        }
                    }
                }
            }
            ScriptManager.RegisterStartupScript(UPnl_Bancos, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Bancos();", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error seleccionar un elemento de la tabla. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Grid_Bancos
    /// DESCRIPCION : Carga los Bancos registrados en el sistema. 
    ///               registradas en el sistema.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 17/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Grid_Bancos(DataTable Dt_Bancos)
    {
        try
        {
            Grid_Bancos.DataSource = Dt_Bancos;
            Grid_Bancos.DataBind();
            Grid_Bancos.SelectedIndex = -1;
            Limpiar_Controles();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar los bancos en la tabla que los listara. Error: [" + Ex.Message + "]");
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Bancos_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Bancos_Sorting(object sender, GridViewSortEventArgs e)
    {
        Consulta_Bancos();
        DataTable Dt_Bancos = (Grid_Bancos.DataSource as DataTable);

        if (Dt_Bancos != null)
        {
            DataView Dv_Bancos = new DataView(Dt_Bancos);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Bancos.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Bancos.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Bancos.DataSource = Dv_Bancos;
            Grid_Bancos.DataBind();
        }
    }
    #endregion

    #region (Eventos)

    #region (Operacion Alta - Modificar - Eliminar - Consultar)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Alta Banco
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Febrero/2011 
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
                if (Validar_Datos_Bancos())
                {
                    Alta_Banco();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(UPnl_Bancos, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Bancos();", true);
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
    ///DESCRIPCIÓN: Modificar Banco
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Febrero/2011
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
                if (Grid_Bancos.SelectedIndex != -1 & !Txt_Banco_ID.Text.Equals(""))
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
                if (Validar_Datos_Bancos())
                {
                    Modificar_Banco();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(UPnl_Bancos, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Bancos();", true);
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
    ///DESCRIPCIÓN: Eliminar Banco
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Febrero/2011 
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
                if (Grid_Bancos.SelectedIndex != -1 & !Txt_Banco_ID.Text.Equals(""))
                {
                    Eliminar_Banco();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea eliminar <br>";
                }
            }
            ScriptManager.RegisterStartupScript(UPnl_Bancos, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Bancos();", true);
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
    ///FECHA_CREO: 17/Febrero/2011 
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
            ScriptManager.RegisterStartupScript(UPnl_Bancos, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Bancos();", true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Bancos_Click
    ///DESCRIPCIÓN: Busqueda Bancos en el sistema.
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 17/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Bancos_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Consulta_Bancos();
            ScriptManager.RegisterStartupScript(UPnl_Bancos, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Bancos();", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error Ejecutar la Búsqueda bancos en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

    private void LLenar_Combo_Meses()
    {
        try
        {
            for (Int32 Meses = 1; Meses <= 100; Meses++) {
                if (Meses == 1)
                    Cmb_No_Meses.Items.Insert((Meses - 1), new ListItem("[" + Meses.ToString() + "] MES", Meses.ToString()));
                else
                    Cmb_No_Meses.Items.Insert((Meses - 1), new ListItem("[" + Meses.ToString() + "] MESES", Meses.ToString()));
            }

            Cmb_No_Meses.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
            Cmb_No_Meses.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al llenar el combo de meses. Error: [" + Ex.Message + "]");
        }
    }
}
