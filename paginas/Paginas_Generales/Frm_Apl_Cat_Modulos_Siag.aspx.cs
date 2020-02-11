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
using Presidencia.Modulos_SIAG.Negocio;

public partial class paginas_Paginas_Generales_Frm_Apl_Cat_Modulos_Siag : System.Web.UI.Page
{
    #region load
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
                Session["Session_Modulos_SIAG"] = null;

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

    #region Metodos
    #region (Generales)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda
    ///               realizar diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 10/Enero/2012
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
            Cargar_Modulos_Siag();
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
    /// FECHA_CREO  : 10/Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Txt_Modulo_ID.Text = "";
            Txt_Nombre.Text = "";
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
    /// FECHA_CREO  : 10/Enero/2012
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

                    //Configuracion_Acceso("Frm_Apl_Cat_Modulos_Siag.aspx");
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
            Txt_Modulo_ID.Enabled = Habilitado;
            Txt_Nombre.Enabled = Habilitado;

            //mensajes de error
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }

        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }
    #endregion

    #region(Control Acceso Pagina)
    /// ******************************************************************************
    /// NOMBRE:         Configuracion_Acceso
    /// DESCRIPCIÓN:    Habilita las operaciones que podrá realizar el usuario en la página.
    /// PARÁMETROS  :
    /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
    /// FECHA CREÓ  :   10/Enero/2012
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
    /// DESCRIPCION :   Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS  :   Cadena.- El dato a evaluar si es numerico.
    /// CREO        :   Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  :   10/Enero/2012
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
    /// FECHA_CREO  : 10/Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        String Espacios_Blanco;
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: <br>";

       
        if (Txt_Nombre.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Es necesario proporcionar un nombre.<br>";
            Datos_Validos = false;
        }
        return Datos_Validos;
    }
    #endregion

    #region(metodos De consulta)
    ///*******************************************************************************
    /// NOMBRE:         Cargar_Modulos_Siag
    /// DESCRIPCION :   Consulta los tipos de pago que estan dadas de alta en la BD
    /// PARAMETROS  : 
    /// CREO        :   Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO  :   11/Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Modulos_Siag()
    {
        Cls_Apl_Cat_Modulos_Siag_Negocio Consulta = new Cls_Apl_Cat_Modulos_Siag_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Grid_Modulos_Siag.Columns[1].Visible = true;
            Dt_Consulta = Consulta.Consultar_Modulo_Siag();
            Session["Session_Modulos_SIAG"] = Dt_Consulta;
            Grid_Modulos_Siag.DataSource = (DataTable)Session["Session_Modulos_SIAG"];
            Grid_Modulos_Siag.DataBind();
            Grid_Modulos_Siag.Columns[1].Visible = false;
            Grid_Modulos_Siag.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid_Tipo_Pago" + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Operaciones)
    ///*******************************************************************************
    ///NOMBRE:      Alta_Modulo_Siag
    ///DESCRIPCIÓN: toma la informacion para poder realizar la alta
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  11/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Alta_Modulo_Siag()
    {
        Cls_Apl_Cat_Modulos_Siag_Negocio Alta = new Cls_Apl_Cat_Modulos_Siag_Negocio();
        try
        {
            Alta.P_Nombre = Txt_Nombre.Text.ToUpper();
            Alta.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
            Alta.Alta_Modulo_Siag();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alta_Modulo_Siag", "alert('Alta Exitosa');", true);
            Inicializa_Controles();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE:      Modificar_Modulo_Siag
    ///DESCRIPCIÓN: toma la informacion para poder realizar la modificacion
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  11/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Modificar_Modulo_Siag()
    {
        Cls_Apl_Cat_Modulos_Siag_Negocio Modificar = new Cls_Apl_Cat_Modulos_Siag_Negocio();
        try
        {
            Modificar.P_Modulo_ID = Txt_Modulo_ID.Text;
            Modificar.P_Nombre = Txt_Nombre.Text.ToUpper();
            Modificar.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;
            Modificar.Modificar_Modulo_Siag();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Mosdificar_Modulo_Siag", "alert('Modificación Exitosa');", true);
            Inicializa_Controles();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE:      Eliminar_Modulo_Siag
    ///DESCRIPCIÓN: toma la informacion para poder realizar la baja
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  11/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Eliminar_Modulo_Siag()
    {
        Cls_Apl_Cat_Modulos_Siag_Negocio Eliminar = new Cls_Apl_Cat_Modulos_Siag_Negocio();
        try
        {
            Eliminar.P_Modulo_ID = Txt_Modulo_ID.Text;
            Eliminar.Eliminar_Modulo_Siag();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Eliminar_Modulo_Siag", "alert('Baja Exitosa');", true);
            Inicializa_Controles();
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

    #region Eventos
    #region Botones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN:  da de alta un registro en la tabla
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  11/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Limpiar_Controles();
                Grid_Modulos_Siag.SelectedIndex = -1;
                Habilitar_Controles("Nuevo");
            }
            else
            {
                if (Validar_Datos())
                {
                    Alta_Modulo_Siag();
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Modificara el registro en la base de datos
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  11/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                if (Txt_Modulo_ID.Text == "")
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encuentra ningun registro seleccionado.<br> *Pruebe seleccionando algun registro de la tabla.";
                }
                else
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
            }
            else
            {
                if (Validar_Datos())
                {
                    Modificar_Modulo_Siag();
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Eliminara el registro de la base de datos
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  11/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(Txt_Modulo_ID.Text))
            {
                Eliminar_Modulo_Siag();
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encuentra ningun registro seleccionado.<br> *Pruebe seleccionando algun registro de la tabla.";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///***********************************************************************************
    ///NOMBRE:      Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operacion actual que se este realizando
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  10/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///***********************************************************************************
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
    #endregion

    #region Grid
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Modulos_Siag_SelectedIndexChanged
    /// DESCRIPCION : Consulta los datos de los movimientos seleccionada por el usuario
    /// CREO        : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO  : 10/Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Modulos_Siag_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 Fila_Seleccionada = Grid_Modulos_Siag.SelectedIndex;//Obtenemos la fila seleccionada de la tabla de roles.
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Limpiar_Controles(); //Limpia los controles del la forma para poder agregar los valores del registro seleccionado

            Txt_Modulo_ID.Text = HttpUtility.HtmlDecode(Grid_Modulos_Siag.Rows[Fila_Seleccionada].Cells[1].Text);
            Txt_Nombre.Text = HttpUtility.HtmlDecode(Grid_Modulos_Siag.Rows[Fila_Seleccionada].Cells[2].Text);

        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    /// ********************************************************************************
    /// NOMBRE:      Grid_Modulos_Siag_Sorting
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// CREÓ:        Hugo Enrique Ramirez Aguilera
    /// FECHA CREÓ:  10/Enero/2012
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **********************************************************************************
    protected void Grid_Modulos_Siag_Sorting(object sender, GridViewSortEventArgs e)
    {
        //Se consultan los modulos que actualmente se encuentran registradas en el sistema.
        Cargar_Modulos_Siag();
        Grid_Modulos_Siag.Columns[1].Visible = true;
        DataTable Dt_Consulta = (Grid_Modulos_Siag.DataSource as DataTable);

        if (Dt_Consulta != null)
        {
            DataView Dv_Consulta = new DataView(Dt_Consulta);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Consulta.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Consulta.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Modulos_Siag.DataSource = Dv_Consulta;
            Grid_Modulos_Siag.DataBind();
            Grid_Modulos_Siag.Columns[1].Visible = false;
        }
    }
    #endregion
}
