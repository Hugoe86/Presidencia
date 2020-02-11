using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Compras_Marcas.Negocio;

public partial class paginas_Compras_Frm_Cat_Com_Marcas : System.Web.UI.Page
{
    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 1;
    private const int Const_Estado_Modificar = 2;
    private const int Const_Estado_Buscar = 3;
    private static DataTable Dt_Unidades = new DataTable();
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
    ///DESCRIPCIÓN: metodo usado para caragar la informacion de todos los combos del formulario con la respectiva consulta
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 08:46:12 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combos()
    {
        Cmb_Tipos.Items.Add(new ListItem("<SELECCIONE>", "0"));
        Cmb_Tipos.Items.Add(new ListItem("ACTIVO", "ACTIVO"));
        Cmb_Tipos.Items.Add(new ListItem("INACTIVO", "INACTIVO"));

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
            Cls_Cat_Com_Marcas_Negocio Marcas_Negocio = new Cls_Cat_Com_Marcas_Negocio();
            Marcas_Negocio.P_Nombre = M_Busqueda;
            Dt_Unidades = Marcas_Negocio.Consulta_Marcas();
            Grid_Marcas.PageIndex = Page_Index;
            Grid_Marcas.DataSource = Dt_Unidades;
            Grid_Marcas.DataBind();
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
    private void Cargar_Datos(DataRow Dr_Tipos)
    {
        try
        {
            Txt_Comentarios.Text = Dr_Tipos[Cat_Com_Marcas.Campo_Comentarios].ToString();
            Txt_ID.Text = Dr_Tipos[Cat_Com_Marcas.Campo_Marca_ID].ToString();
            Txt_Nombre.Text = Dr_Tipos[Cat_Com_Marcas.Campo_Nombre].ToString();
            Cmb_Tipos.SelectedValue = Dr_Tipos[Cat_Com_Marcas.Campo_Estatus].ToString();

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
                Cmb_Tipos.Enabled = false;

                Cmb_Tipos.SelectedIndex = 0;

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

                Grid_Marcas.Enabled = true;
                Grid_Marcas.SelectedIndex = (-1);

                Btn_Busqueda.ImageUrl = "~/paginas/imagenes/paginas/busqueda.png";
                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                Configuracion_Acceso("Frm_Cat_Com_Marcas.aspx");
                break;

            case 1: //Nuevo
                Mensaje_Error();

                Txt_Comentarios.Text = String.Empty;                
                Txt_Nombre.Text = String.Empty;
                Txt_ID.Text = String.Empty;

                Txt_Comentarios.Enabled = true;                
                Txt_Nombre.Enabled = true;

                Btn_Eliminar.Enabled = false;
                Btn_Modificar.Enabled = false;
                Btn_Nuevo.Enabled = true;
                Btn_Salir.Enabled = true;

                Cmb_Tipos.SelectedIndex = 0;
                Cmb_Tipos.Enabled = true;

                Grid_Marcas.SelectedIndex = (-1);
                Grid_Marcas.Enabled = false;

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

                Btn_Eliminar.Enabled = false;
                Btn_Modificar.Enabled = true;
                Btn_Nuevo.Enabled = false;
                Btn_Salir.Enabled = true;

                Cmb_Tipos.Enabled = true;

                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Guardar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Cancelar";

                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Guardar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Cancelar";

                Grid_Marcas.Enabled = false;

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
                Mensaje_Error("Favor de ingresar el nombre de la Marca");
            }            
            if (Cmb_Tipos.SelectedIndex <= 0)
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar el estatus de la Marca");
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
    ///NOMBRE DE LA FUNCIÓN: Alta_Unidad
    ///DESCRIPCIÓN: Realiza el alta de un nuevo registro de una unidad
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/02/2011 19:36:45
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Alta()
    {
        try
        {
            if (Validar_Datos())
            {
                Cls_Cat_Com_Marcas_Negocio Marcas_Negocio = new Cls_Cat_Com_Marcas_Negocio();
                if (!Txt_Comentarios.Text.Equals(""))
                {
                    Marcas_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();
                }                
                Marcas_Negocio.P_Estatus = Cmb_Tipos.SelectedValue;
                Marcas_Negocio.P_Nombre = Txt_Nombre.Text.Trim();
                Marcas_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Marcas_Negocio.Alta_Marcas();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Marcas", "alert('El Alta de la Marca fue Exitosa');", true);
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
            Cls_Cat_Com_Marcas_Negocio Marca_Negocio = new Cls_Cat_Com_Marcas_Negocio();
            Marca_Negocio.P_Marca_ID = HttpUtility.HtmlDecode(Grid_Marcas.SelectedRow.Cells[1].Text.Trim());
            Marca_Negocio.Baja_Marcas();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipos Marcas", "alert('La Baja de la Marca fue Exitosa');", true);
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
                Cls_Cat_Com_Marcas_Negocio Tipo_Negocio = new Cls_Cat_Com_Marcas_Negocio();
                if (!Txt_Comentarios.Text.Equals(""))
                {
                    Tipo_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();
                }
                Tipo_Negocio.P_Marca_ID = HttpUtility.HtmlDecode(Grid_Marcas.SelectedRow.Cells[1].Text.Trim());                
                Tipo_Negocio.P_Nombre = Txt_Nombre.Text.Trim();
                Tipo_Negocio.P_Estatus = Cmb_Tipos.SelectedValue;
                Tipo_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Tipo_Negocio.Cambio_Marcas();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Marcas", "alert('La modificación de la Marca fue Exitosa');", true);
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
                Alta();
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
            if (Grid_Marcas.SelectedIndex > (-1))
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
                Mensaje_Error("Favor de seleccionar la Marca a modificar");
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
            if (Grid_Marcas.SelectedIndex > (-1))
            {
                Baja();
            }
            else
            {
                Mensaje_Error("Favor de seleccionar la Marca a eliminar");
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
            Grid_Marcas.SelectedIndex = (-1);
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
    protected void Grid_Marcas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Marcas.SelectedIndex = (-1);
            Cargar_Grid(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Grid_Marcas_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Marcas.SelectedIndex > (-1))
            {
                Cargar_Datos(Dt_Unidades.Rows[Grid_Marcas.SelectedIndex + (Grid_Marcas.PageIndex * 5)]);
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
