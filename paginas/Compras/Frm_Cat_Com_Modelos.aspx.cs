using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Catalogo_Compras_Subfamilias.Negocio;
using Presidencia.Catalogo_Compras_Modelos.Negocio;

public partial class paginas_Compras_Frm_Cat_Com_Modelos : System.Web.UI.Page
{
    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 1;
    private const int Const_Estado_Modificar = 2;
    private const int Const_Estado_Buscar = 3;
    private static DataTable Dt_Sub_Familias = new DataTable();
    private static string M_Busqueda = "";
    #endregion

    #region Page Load / Init
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!Page.IsPostBack)
            {
                Estado_Botones(Const_Estado_Inicial);
                Cargar_Combos();
                Cargar_Grid(0);
            }
            Mensaje_Error();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion

    #region Metodos
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combos
    ///DESCRIPCIÓN: metodo usado para cargar la informacion de todos los combos del formulario con la respectiva consulta
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 08:46:12 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combos()
    {
        //Instancia el objeto de negocio de familias y consulta la lista de estas
        Cls_Cat_Com_Subfamilias_Negocio Sub_Familias_Negocio = new Cls_Cat_Com_Subfamilias_Negocio();
        Llenar_Combo_ID(Cmb_Sub_Familias, Sub_Familias_Negocio.Consulta_Subfamilias(), Cat_Com_Subfamilias.Campo_Nombre, Cat_Com_Subfamilias.Campo_Subfamilia_ID, "0");
        Cmb_Estatus.Items.Add(new ListItem("<SELECCIONE>", "0"));
        Cmb_Estatus.Items.Add(new ListItem("ACTIVO", "ACTIVO"));
        Cmb_Estatus.Items.Add(new ListItem("INACTIVO", "INACTIVO"));

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid
    ///DESCRIPCIÓN: Realizar la consulta y llenar el grido con estos datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 12:14:35 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Grid(int Page_Index)
    {
        try
        {
            Cls_Cat_Com_Modelos_Negocio Modelos_Negocio = new Cls_Cat_Com_Modelos_Negocio();
            Modelos_Negocio.P_Nombre = M_Busqueda;
            Dt_Sub_Familias = Modelos_Negocio.Consulta_Modelos();
            Grid_Modelos.PageIndex = Page_Index;
            Grid_Modelos.DataSource = Dt_Sub_Familias;
            Grid_Modelos.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA METODO: LLenar_Combo_Id
    ///        DESCRIPCIÓN: llena todos los combos
    ///         PARAMETROS: 1.- Obj_DropDownList: Combo a llenar
    ///                     2.- Dt_Temporal: DataTable genarada por una consulta a la base de datos
    ///                     3.- Texto: nombre de la columna del dataTable que mostrara el texto en el combo
    ///                     3.- Valor: nombre de la columna del dataTable que mostrara el valor en el combo
    ///                     3.- Seleccion: Id del combo el cual aparecera como seleccionado por default
    ///               CREO: Jesus S. Toledo Rdz.
    ///         FECHA_CREO: 06/9/2010
    ///           MODIFICO:
    ///     FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList, DataTable Dt_Temporal, String Texto, String Valor, String Seleccion)
    {
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("<SELECCIONE>", "0"));
            foreach (DataRow row in Dt_Temporal.Rows)
            {
                Obj_DropDownList.Items.Add(new ListItem(row[Texto].ToString(), row[Valor].ToString()));
            }
            Obj_DropDownList.SelectedValue = Seleccion;
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList)
    {
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("<SELECCIONE>", "0"));
            Obj_DropDownList.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos
    ///DESCRIPCIÓN: metodo que recibe el DataRow seleccionado de la grilla y carga los datos en los componetes de l formulario
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 02:07:54 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos(DataRow Dr_Modelos)
    {
        try
        {
            Txt_Comentarios.Text = Dr_Modelos[Cat_Com_Modelos.Campo_Comentarios].ToString();            
            Txt_ID.Text = Dr_Modelos[Cat_Com_Modelos.Campo_Modelo_ID].ToString();
            Txt_Nombre.Text = Dr_Modelos[Cat_Com_Modelos.Campo_Nombre].ToString();
            Cmb_Sub_Familias.SelectedValue = Dr_Modelos[Cat_Com_Modelos.Campo_Subfamilia_ID].ToString();
            Cmb_Estatus.SelectedValue = Dr_Modelos[Cat_Com_Modelos.Campo_Estatus].ToString();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        Img_Error.Visible = true;
        Lbl_Error.Text += P_Mensaje + "</br>";
    }
    private void Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Error.Text = "";
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Estado_Botones
    ///DESCRIPCIÓN: Metodo para establecer el estado de los botones y componentes del formulario
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/02/2011 05:49:53 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Estado_Botones(int P_Estado)
    {
        switch (P_Estado)
        {
            case 0: //Estado inicial

                Mensaje_Error();
                Txt_Busqueda.Text = String.Empty;
                //M_Busqueda = String.Empty;
                Txt_Comentarios.Text = String.Empty;                
                Txt_ID.Text = String.Empty;
                Txt_Nombre.Text = String.Empty;

                Txt_Comentarios.Enabled = false;                
                Txt_ID.Enabled = false;
                Txt_Nombre.Enabled = false;

                Cmb_Estatus.SelectedIndex = 0;
                Cmb_Sub_Familias.SelectedIndex = 0;

                Cmb_Estatus.Enabled = false;
                Cmb_Sub_Familias.Enabled = false;

                Grid_Modelos.Enabled = true;
                Grid_Modelos.SelectedIndex = (-1);

                Btn_Busqueda.Enabled = true;
                Btn_Eliminar.Enabled = true;
                Btn_Modificar.Enabled = true;
                Btn_Nuevo.Enabled = true;
                Btn_Salir.Enabled = true;
                Btn_Busqueda.AlternateText = "Buscar";
                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Salir";

                Btn_Busqueda.ToolTip = "Consultar";
                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Salir";

                Btn_Busqueda.ImageUrl = "~/paginas/imagenes/paginas/busqueda.png";
                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                Configuracion_Acceso("Frm_Cat_Com_Modelos.aspx");
                break;

            case 1: //Nuevo
                Mensaje_Error();

                Txt_Comentarios.Text = String.Empty;                
                Txt_Nombre.Text = String.Empty;
                Txt_ID.Text = String.Empty;

                Cmb_Estatus.SelectedIndex = 0;
                Cmb_Sub_Familias.SelectedIndex = 0;

                Txt_Comentarios.Enabled = true;                
                Txt_Nombre.Enabled = true;

                Cmb_Estatus.Enabled = true;
                Cmb_Sub_Familias.Enabled = true;

                Btn_Eliminar.Enabled = false;
                Btn_Modificar.Enabled = false;
                Btn_Nuevo.Enabled = true;
                Btn_Salir.Enabled = true;

                Grid_Modelos.SelectedIndex = (-1);
                Grid_Modelos.Enabled = false;

                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Nuevo.AlternateText = "Guardar";
                Btn_Salir.AlternateText = "Cancelar";

                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Guardar";
                Btn_Salir.ToolTip = "Cancelar";

                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar_deshabilitado.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar_deshabilitado.png";

                break;

            case 2: //Modificar
                Mensaje_Error();

                Txt_Comentarios.Enabled = true;                
                Txt_Nombre.Enabled = true;

                Cmb_Estatus.Enabled = true;
                Cmb_Sub_Familias.Enabled = true;

                Btn_Eliminar.Enabled = false;
                Btn_Modificar.Enabled = true;
                Btn_Nuevo.Enabled = false;
                Btn_Salir.Enabled = true;

                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Guardar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Cancelar";

                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Guardar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Cancelar";

                Grid_Modelos.Enabled = false;

                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar_deshabilitado.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo_deshabilitado.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";

                break;

            case 3: //Buscar
                Mensaje_Error();

                Txt_Comentarios.Text = String.Empty;                
                Txt_ID.Text = String.Empty;
                Txt_Nombre.Text = String.Empty;

                Cmb_Estatus.SelectedIndex = 0;
                Cmb_Sub_Familias.SelectedIndex = 0;

                break;
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Datos
    ///DESCRIPCIÓN: Guardar datos para dar de alta un nuevo registro de un servicio
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 10:45:17 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        Boolean Resultado = true;
        try
        {
            if (Txt_Nombre.Text.Trim() == "")
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar el nombre del Modelo");
            }            
            //if (Cmb_Sub_Familias.SelectedIndex <= 0)
            //{
            //    Resultado = false;
            //    Mensaje_Error("Favor de especificar la SubFamilia relacionada con el Modelo");
            //}
            if (Cmb_Estatus.SelectedIndex <= 0)
            {
                Resultado = false;
                Mensaje_Error("Favor de especificar el Estatus del Modelo");
            }
            if (!Txt_Comentarios.Text.Trim().Equals(""))
            {
                if (Txt_Comentarios.Text.Trim().Length >= 250)
                {
                    Txt_Comentarios.Text = Txt_Comentarios.Text.Trim().Substring(0, 250);
                }
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
        return Resultado;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Alta_Modelo
    ///DESCRIPCIÓN: Realiza el alta de un nuevo registro de un modelo
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/02/2011 19:36:45
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Alta_Modelo()
    {
        try
        {
            if (Validar_Datos())
            {
                Cls_Cat_Com_Modelos_Negocio Modelos_Negocio = new Cls_Cat_Com_Modelos_Negocio();
                if (!Txt_Comentarios.Text.Equals(""))
                {
                    Modelos_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();
                }                
                Modelos_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
                Modelos_Negocio.P_Subfamilia_ID = Cmb_Sub_Familias.SelectedValue;
                Modelos_Negocio.P_Nombre = Txt_Nombre.Text.Trim();
                Modelos_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Modelos_Negocio.Alta_Modelos();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Modelos", "alert('El Alta del Modelo fue Exitosa');", true);
                Estado_Botones(Const_Estado_Inicial);
                Txt_Busqueda.Text = "";
                Cargar_Grid(0);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message, Ex);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Baja
    ///DESCRIPCIÓN: dar de baja un registro de la base de datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/04/2011 11:25:30 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Baja()
    {
        try
        {
            Cls_Cat_Com_Modelos_Negocio Modelos_Negocio = new Cls_Cat_Com_Modelos_Negocio();
            Modelos_Negocio.P_Modelo_ID = HttpUtility.HtmlDecode(Grid_Modelos.SelectedRow.Cells[1].Text.Trim());
            Modelos_Negocio.Baja_Modelos();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Modelos", "alert('La Baja del Modelo fue Exitosa');", true);
            Estado_Botones(Const_Estado_Inicial);
            Txt_Busqueda.Text = "";
            Cargar_Grid(0);
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message, Ex);
        }

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Modificar
    ///DESCRIPCIÓN: Modifica un registro y lo guarda en la base de datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/04/2011 11:58:30 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Modificar()
    {
        try
        {
            if (Validar_Datos())
            {
                Cls_Cat_Com_Modelos_Negocio Modelos_Negocio = new Cls_Cat_Com_Modelos_Negocio();
                if (!Txt_Comentarios.Text.Equals(""))
                {
                    Modelos_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();
                }
                Modelos_Negocio.P_Modelo_ID = HttpUtility.HtmlDecode(Grid_Modelos.SelectedRow.Cells[1].Text.Trim());                
                Modelos_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
                Modelos_Negocio.P_Subfamilia_ID = Cmb_Sub_Familias.SelectedValue;
                Modelos_Negocio.P_Nombre = Txt_Nombre.Text.Trim();
                Modelos_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Modelos_Negocio.Cambio_Modelos();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Modelos", "alert('La modificación del Modelo fue Exitosa');", true);
                Estado_Botones(Const_Estado_Inicial);
                Txt_Busqueda.Text = "";
                Cargar_Grid(0);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message, Ex);
        }
    }
    #endregion

    #region Eventos
    
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Estado_Botones(Const_Estado_Nuevo);
            }
            else
            {
                Alta_Modelo();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Modelos.SelectedIndex > (-1))
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    Estado_Botones(Const_Estado_Modificar);
                }
                else
                {
                    Modificar();
                }
            }
            else
            {
                Mensaje_Error("Favor de seleccionar el Modelo a modificar");
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);

        }
    }
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Modelos.SelectedIndex > (-1))
            {
                Baja();
            }
            else
            {
                Mensaje_Error("Favor de seleccionar el Modelo a eliminar");
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.AlternateText.Equals("Salir"))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Estado_Botones(Const_Estado_Inicial);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Btn_Busqueda_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Estado_Botones(Const_Estado_Buscar);
            Grid_Modelos.SelectedIndex = (-1);
            M_Busqueda = Txt_Busqueda.Text.Trim();
            Cargar_Grid(0);

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion

    #region Eventos Grid
    protected void Grid_Modelos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Modelos.SelectedIndex = (-1);
            Cargar_Grid(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Grid_Modelos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Modelos.SelectedIndex > (-1))
            {
                Cargar_Datos(Dt_Sub_Familias.Rows[Grid_Modelos.SelectedIndex + (Grid_Modelos.PageIndex * 5)]);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
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
            Botones.Add(Btn_Busqueda);

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
}
