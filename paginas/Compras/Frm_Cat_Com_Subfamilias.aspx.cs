using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Catalogo_Compras_Familias.Negocio;
using Presidencia.Catalogo_Compras_Subfamilias.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data;


public partial class paginas_Compras_Frm_Cat_Com_Subfamilias : System.Web.UI.Page
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
        //Instancia el objeto de negocio de familias y consulta la lista de estas
        Cls_Cat_Com_Familias_Negocio Familias_Negocio = new Cls_Cat_Com_Familias_Negocio();
        Llenar_Combo_ID(Cmb_Familias, Familias_Negocio.Consulta_Familias(), Cat_Com_Familias.Campo_Nombre, Cat_Com_Familias.Campo_Familia_ID, "0");
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
            Cls_Cat_Com_Subfamilias_Negocio Subfamilia_Negocio = new Cls_Cat_Com_Subfamilias_Negocio();
            Subfamilia_Negocio.P_Nombre = M_Busqueda;
            Dt_Sub_Familias = Subfamilia_Negocio.Consulta_Subfamilias();
            Grid_Sub_Familias.PageIndex = Page_Index;
            Grid_Sub_Familias.DataSource = Dt_Sub_Familias;
            Grid_Sub_Familias.DataBind();
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
    private void Cargar_Datos(DataRow Dr_Sub_Familias)
    {
        try
        {
            Txt_Comentarios.Text = Dr_Sub_Familias[Cat_Com_Subfamilias.Campo_Descripcion].ToString();
            Txt_Abreviatura.Text = Dr_Sub_Familias[Cat_Com_Subfamilias.Campo_Abreviatura].ToString();
            Txt_ID.Text = Dr_Sub_Familias[Cat_Com_Subfamilias.Campo_Subfamilia_ID].ToString();
            Txt_Nombre.Text = Dr_Sub_Familias[Cat_Com_Subfamilias.Campo_Nombre].ToString();
            Cmb_Familias.SelectedValue = Dr_Sub_Familias[Cat_Com_Subfamilias.Campo_Familia_ID].ToString();
            Cmb_Estatus.SelectedValue = Dr_Sub_Familias[Cat_Com_Subfamilias.Campo_Estatus].ToString();
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
                Txt_Abreviatura.Text = String.Empty;
                Txt_ID.Text = String.Empty;
                Txt_Nombre.Text = String.Empty;

                Txt_Comentarios.Enabled = false;
                Txt_Abreviatura.Enabled = false;
                Txt_ID.Enabled = false;
                Txt_Nombre.Enabled = false;

                Cmb_Estatus.SelectedIndex = 0;
                Cmb_Familias.SelectedIndex = 0;

                Cmb_Estatus.Enabled = false;
                Cmb_Familias.Enabled = false;

                Grid_Sub_Familias.Enabled = true;
                Grid_Sub_Familias.SelectedIndex = (-1);

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

                Btn_Busqueda.ToolTip = "Buscar";
                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Salir";

                Btn_Busqueda.ImageUrl = "~/paginas/imagenes/paginas/busqueda.png";
                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                Btn_Eliminar.Visible = true;
                Btn_Salir.Visible = true;
                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = true;

                break;

            case 1: //Nuevo
                Mensaje_Error();

                Txt_Comentarios.Text = String.Empty;
                Txt_Abreviatura.Text = String.Empty;
                Txt_Nombre.Text = String.Empty;
                Txt_ID.Text = String.Empty;

                Cmb_Estatus.SelectedIndex = 0;
                Cmb_Familias.SelectedIndex = 0;

                Txt_Comentarios.Enabled = true;
                Txt_Abreviatura.Enabled = true;
                Txt_Nombre.Enabled = true;

                Cmb_Estatus.Enabled = true;
                Cmb_Familias.Enabled = true;

                Btn_Eliminar.Enabled = false;
                Btn_Modificar.Enabled = false;
                Btn_Nuevo.Enabled = true;
                Btn_Salir.Enabled = true;

                Grid_Sub_Familias.SelectedIndex = (-1);
                Grid_Sub_Familias.Enabled = false;

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

                Btn_Eliminar.Visible = false;
                Btn_Salir.Visible = true;
                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = false;

                break;

            case 2: //Modificar
                Mensaje_Error();

                Txt_Comentarios.Enabled = true;
                Txt_Abreviatura.Enabled = true;
                Txt_Nombre.Enabled = true;

                Cmb_Estatus.Enabled = true;
                Cmb_Familias.Enabled = true;

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

                Grid_Sub_Familias.Enabled = false;

                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar_deshabilitado.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo_deshabilitado.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Eliminar.Visible = false;
                Btn_Salir.Visible = true;
                Btn_Nuevo.Visible = false;
                Btn_Modificar.Visible = true;

                break;

            case 3: //Buscar
                Mensaje_Error();

                Txt_Comentarios.Text = String.Empty;
                Txt_Abreviatura.Text = String.Empty;
                Txt_ID.Text = String.Empty;
                Txt_Nombre.Text = String.Empty;

                Cmb_Estatus.SelectedIndex = 0;
                Cmb_Familias.SelectedIndex = 0;

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
                Mensaje_Error("Favor de ingresar el nombre de la Subfamilia");
            }
            if (Txt_Abreviatura.Text.Trim() == "")
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar la Abreviatura de la Subfamilia");
            }
            if (Cmb_Familias.SelectedIndex <= 0)
            {
                Resultado = false;
                Mensaje_Error("Favor de especificar la Familia relacionada con la nueva Subfamilia");
            }
            if (Cmb_Estatus.SelectedIndex <= 0)
            {
                Resultado = false;
                Mensaje_Error("Favor de especificar el Estatus de la Subfamilia");
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
    ///NOMBRE DE LA FUNCIÓN: Alta_Sub_Familia
    ///DESCRIPCIÓN: Realiza el alta de un nuevo registro de una subfamilia
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/02/2011 19:36:45
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Alta_Sub_Familia()
    {
        try
        {
            if (Validar_Datos())
            {
                Cls_Cat_Com_Subfamilias_Negocio Subfamilias_Negocio = new Cls_Cat_Com_Subfamilias_Negocio();
                if (!Txt_Comentarios.Text.Equals(""))
                {
                    Subfamilias_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();
                }
                Subfamilias_Negocio.P_Abreviatura = Txt_Abreviatura.Text.Trim();
                Subfamilias_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
                Subfamilias_Negocio.P_Familia_ID = Cmb_Familias.SelectedValue;
                Subfamilias_Negocio.P_Nombre = Txt_Nombre.Text.Trim();
                Subfamilias_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;                
                Subfamilias_Negocio.Alta_Subfamilias();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Subfamilias", "alert('El Alta de la Subfamilia fue Exitosa');", true);
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
            Cls_Cat_Com_Subfamilias_Negocio Subfamilias_Negocio = new Cls_Cat_Com_Subfamilias_Negocio();
            Subfamilias_Negocio.P_Subfamilia_ID = HttpUtility.HtmlDecode(Grid_Sub_Familias.SelectedRow.Cells[1].Text.Trim());
            Subfamilias_Negocio.Baja_Subfamilias();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Subfamilias", "alert('La Baja de la Subfamilia fue Exitosa');", true);
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
                Cls_Cat_Com_Subfamilias_Negocio Subfamilias_Negocio = new Cls_Cat_Com_Subfamilias_Negocio();
                if (!Txt_Comentarios.Text.Equals(""))
                {
                    Subfamilias_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();
                }
                Subfamilias_Negocio.P_Subfamilia_ID = HttpUtility.HtmlDecode(Grid_Sub_Familias.SelectedRow.Cells[1].Text.Trim());
                Subfamilias_Negocio.P_Abreviatura = Txt_Abreviatura.Text.Trim();
                Subfamilias_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
                Subfamilias_Negocio.P_Familia_ID = Cmb_Familias.SelectedValue;
                Subfamilias_Negocio.P_Nombre = Txt_Nombre.Text.Trim();
                Subfamilias_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;                
                Subfamilias_Negocio.Cambio_Subfamilias();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Subfamilias", "alert('La modificación de la Subfamilia fue Exitosa');", true);
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
                Alta_Sub_Familia();
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
            if (Grid_Sub_Familias.SelectedIndex > (-1))
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
                Mensaje_Error("Favor de seleccionar la Subfamilia a modificar");
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
            if (Grid_Sub_Familias.SelectedIndex > (-1))
            {
                Baja();
            }
            else
            {
                Mensaje_Error("Favor de seleccionar la Subfamilia a eliminar");
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
            Grid_Sub_Familias.SelectedIndex = (-1);
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
    
    protected void Grid_Sub_Familias_SelectedIndexChanged(object sender, EventArgs e)
    {        
        try
        {            
            if (Grid_Sub_Familias.SelectedIndex > (-1))
            {
                Cargar_Datos(Dt_Sub_Familias.Rows[Grid_Sub_Familias.SelectedIndex + (Grid_Sub_Familias.PageIndex * 5)]);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Grid_Sub_Familias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Sub_Familias.SelectedIndex = (-1);
        Cargar_Grid(e.NewPageIndex);
    }
    #endregion
}