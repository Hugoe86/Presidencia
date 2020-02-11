using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Catalogo_SAP_Conceptos.Negocio;
using System.Data;

public partial class paginas_Paginas_Generales_Frm_Cat_SAP_Conceptos : System.Web.UI.Page
{

    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 1;
    private const int Const_Estado_Modificar = 2;
    private const int Const_Estado_Buscar = 3;
    private const int Const_Estado_Deshabilitado = 4;
    //private static DataTable Dt_SAP_Conceptos = new DataTable();
    private static String Dt_SAP_Conceptos = "Dt_SAP_Conceptos";
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
                ViewState["SortDirection"] = "DESC";
            }
            Mensaje_Error();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
            Estado_Botones(Const_Estado_Deshabilitado);

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
        try
        {
            Cls_Cat_SAP_Conceptos_Negocio SAP_Concepto = new Cls_Cat_SAP_Conceptos_Negocio();
            Llenar_Combo_ID(Cmb_Capitulo, SAP_Concepto.Consulta_Capitulos(), Cat_SAP_Capitulos.Campo_Clave, Cat_SAP_Capitulos.Campo_Capitulo_ID, "0");
            Cmb_Estatus.Items.Add(new ListItem("<SELECCIONE>", "0"));
            Cmb_Estatus.Items.Add(new ListItem("ACTIVO", "ACTIVO"));
            Cmb_Estatus.Items.Add(new ListItem("INACTIVO", "INACTIVO"));
        }
        catch (Exception Ex)
        {
            throw new Exception ("No se pudieron cargar los datos necesarios para dar de alta un Concepto SAP" + "</br>" + Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid
    ///DESCRIPCIÓN: Realizar la consulta y llenar el grid con estos datos
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
            Cls_Cat_SAP_Conceptos_Negocio SAP_Concepto = new Cls_Cat_SAP_Conceptos_Negocio();            
            SAP_Concepto.P_Clave = M_Busqueda;
            Session[Dt_SAP_Conceptos] = SAP_Concepto.Consulta_Concepto_SAP();
            Grid_SAP_Conceptos.PageIndex = Page_Index;
            Grid_SAP_Conceptos.DataSource = (DataTable)Session[Dt_SAP_Conceptos];
            Grid_SAP_Conceptos.DataBind();
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
    private void Cargar_Datos(DataRow Dr_Conceptos)
    {
        try
        {
            Txt_Comentarios.Text = Dr_Conceptos[Cat_Sap_Concepto.Campo_Descripcion].ToString();
            Txt_ID.Text = Dr_Conceptos[Cat_Sap_Concepto.Campo_Concepto_ID].ToString();
            Txt_Clave.Text = Dr_Conceptos[Cat_Sap_Concepto.Campo_Clave].ToString();
            Cmb_Capitulo.SelectedValue = Dr_Conceptos[Cat_Sap_Concepto.Campo_Capitulo_ID].ToString();
            Cmb_Estatus.SelectedValue = Dr_Conceptos[Cat_Sap_Concepto.Campo_Estatus].ToString();
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
                Txt_Comentarios.Text = String.Empty;
                Txt_ID.Text = String.Empty;
                Txt_Clave.Text = String.Empty;

                Txt_Comentarios.Enabled = false;
                Txt_ID.Enabled = false;
                Txt_Clave.Enabled = false;

                Cmb_Estatus.SelectedIndex = 0;
                Cmb_Capitulo.SelectedIndex = 0;

                Cmb_Estatus.Enabled = false;
                Cmb_Capitulo.Enabled = false;

                Grid_SAP_Conceptos.Enabled = true;
                Grid_SAP_Conceptos.SelectedIndex = (-1);

                Btn_Busqueda.Visible = true;
                Btn_Eliminar.Visible = true;
                Btn_Modificar.Visible = true;
                Btn_Nuevo.Visible = true;
                Btn_Salir.Visible = true;

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

                Configuracion_Acceso("Frm_Cat_SAP_Conceptos.aspx");
                break;

            case 1: //Nuevo
                Mensaje_Error();

                Txt_Comentarios.Text = String.Empty;
                Txt_Clave.Text = String.Empty;
                Txt_ID.Text = String.Empty;

                Cmb_Estatus.SelectedIndex = 0;
                Cmb_Capitulo.SelectedIndex = 0;

                Txt_Comentarios.Enabled = true;
                Txt_Clave.Enabled = true;

                Cmb_Estatus.Enabled = true;
                Cmb_Capitulo.Enabled = true;

                Btn_Eliminar.Visible = false;
                Btn_Modificar.Visible = false;
                Btn_Nuevo.Visible = true;
                Btn_Salir.Visible = true;

                Grid_SAP_Conceptos.SelectedIndex = (-1);
                Grid_SAP_Conceptos.Enabled = false;

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
                Txt_Clave.Enabled = true;

                Cmb_Estatus.Enabled = true;
                Cmb_Capitulo.Enabled = true;

                Btn_Eliminar.Visible = false;
                Btn_Modificar.Visible = true;
                Btn_Nuevo.Visible = false;
                Btn_Salir.Visible = true;

                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Guardar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Cancelar";

                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Guardar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Cancelar";

                Grid_SAP_Conceptos.Enabled = false;

                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar_deshabilitado.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo_deshabilitado.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";

                break;

            case 3: //Buscar
                Mensaje_Error();

                Txt_Comentarios.Text = String.Empty;
                Txt_ID.Text = String.Empty;
                Txt_Clave.Text = String.Empty;

                Cmb_Estatus.SelectedIndex = 0;
                Cmb_Capitulo.SelectedIndex = 0;

                break;

            case 4: //Desabilitar

                Txt_Busqueda.Text = String.Empty;
                Txt_Comentarios.Text = String.Empty;
                Txt_ID.Text = String.Empty;
                Txt_Clave.Text = String.Empty;

                Txt_Comentarios.Enabled = false;
                Txt_ID.Enabled = false;
                Txt_Clave.Enabled = false;               

                Cmb_Estatus.Enabled = false;
                Cmb_Capitulo.Enabled = false;

                Grid_SAP_Conceptos.Enabled = true;
                Grid_SAP_Conceptos.SelectedIndex = (-1);

                Btn_Busqueda.Visible = false;
                Btn_Eliminar.Visible = false;
                Btn_Modificar.Visible = false;
                Btn_Nuevo.Visible = false;
                Btn_Salir.Visible = false;

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
            if (Txt_Clave.Text.Trim() == "")
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar la Clave del Concepto");
            }
            if (Cmb_Capitulo.SelectedIndex <= 0)
            {
                Resultado = false;
                Mensaje_Error("Favor de especificar el Capitulo relacionado con el Concepto");
            }
            if (Cmb_Estatus.SelectedIndex <= 0)
            {
                Resultado = false;
                Mensaje_Error("Favor de especificar el Estatus del Concepto");
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
    ///NOMBRE DE LA FUNCIÓN: Alta_Concepto
    ///DESCRIPCIÓN: Realiza el alta de un nuevo registro de un concepto
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 26/02/2011 11:09:45
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Alta_Concepto()
    {
        try
        {
            if (Validar_Datos())
            {
                Cls_Cat_SAP_Conceptos_Negocio SAP_Concepto = new Cls_Cat_SAP_Conceptos_Negocio();
                if (!Txt_Comentarios.Text.Equals(""))
                {
                    SAP_Concepto.P_Descripcion = Txt_Comentarios.Text.Trim();
                }
                SAP_Concepto.P_Estatus = Cmb_Estatus.SelectedValue;
                SAP_Concepto.P_Capitulo_ID = Cmb_Capitulo.SelectedValue;
                SAP_Concepto.P_Clave = Txt_Clave.Text.Trim();
                SAP_Concepto.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                SAP_Concepto.Alta_Concepto_SAP();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Conceptos", "alert('El Alta del Concepto SAP fue Exitosa');", true);
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
    ///NOMBRE DE LA FUNCIÓN: Baja_Concepto
    ///DESCRIPCIÓN: dar de baja un registro de la base de datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/04/2011 11:25:30 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Baja_Concepto()
    {
        try
        {
            Cls_Cat_SAP_Conceptos_Negocio SAP_Concepto = new Cls_Cat_SAP_Conceptos_Negocio();
            SAP_Concepto.P_Concepto_ID = HttpUtility.HtmlDecode(Grid_SAP_Conceptos.SelectedRow.Cells[1].Text.Trim());
            SAP_Concepto.Baja_Concepto_SAP();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Conceptos", "alert('La Baja del Concepto SAP fue Exitosa');", true);
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
    ///NOMBRE DE LA FUNCIÓN: Modificar_Concepto
    ///DESCRIPCIÓN: Modifica un registro y lo guarda en la base de datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/04/2011 11:58:30 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Modificar_Concepto()
    {
        try
        {
            if (Validar_Datos())
            {
                Cls_Cat_SAP_Conceptos_Negocio SAP_Concepto = new Cls_Cat_SAP_Conceptos_Negocio();
                if (!Txt_Comentarios.Text.Equals(""))
                {
                    SAP_Concepto.P_Descripcion = Txt_Comentarios.Text.Trim();
                }
                SAP_Concepto.P_Concepto_ID = HttpUtility.HtmlDecode(Grid_SAP_Conceptos.SelectedRow.Cells[1].Text.Trim());
                SAP_Concepto.P_Estatus = Cmb_Estatus.SelectedValue;
                SAP_Concepto.P_Capitulo_ID = Cmb_Capitulo.SelectedValue;
                SAP_Concepto.P_Clave = Txt_Clave.Text.Trim();
                SAP_Concepto.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                SAP_Concepto.Cambio_Concepto_SAP();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Conceptos", "alert('La modificación del Concepto SAP fue Exitosa');", true);
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
                Alta_Concepto();
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
            if (Grid_SAP_Conceptos.SelectedIndex > (-1))
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    Estado_Botones(Const_Estado_Modificar);
                }
                else
                {
                    Modificar_Concepto();
                }
            }
            else
            {
                Mensaje_Error("Favor de seleccionar el Concepto a modificar");
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
            if (Grid_SAP_Conceptos.SelectedIndex > (-1))
            {
                Baja_Concepto();
            }
            else
            {
                Mensaje_Error("Favor de seleccionar el Concepto a eliminar");
            }
        }
        catch (Exception Ex)
        {
            String Msg = Ex.ToString();
            Mensaje_Error("Es posible que existan registros secuendarios del registro que desea eliminar");
            //Mensaje_Error(Ex.Message);
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
            Grid_SAP_Conceptos.SelectedIndex = (-1);
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
    
    protected void Grid_SAP_Conceptos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_SAP_Conceptos.SelectedIndex = (-1);
            Cargar_Grid(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Grid_SAP_Conceptos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_SAP_Conceptos_Negocio SAP_Concepto = new Cls_Cat_SAP_Conceptos_Negocio();
        DataTable Dt_Capitulos = new DataTable();
        
        try
        {
            if (Grid_SAP_Conceptos.SelectedIndex > (-1))
            {
                GridViewRow selectedRow = Grid_SAP_Conceptos.Rows[Grid_SAP_Conceptos.SelectedIndex];
                String Clave = HttpUtility.HtmlDecode(selectedRow.Cells[1].Text).ToString();
                String Estatus = HttpUtility.HtmlDecode(selectedRow.Cells[2].Text).ToString();
                String Capitulo = HttpUtility.HtmlDecode(selectedRow.Cells[3].Text).ToString();
                String Descripcion = HttpUtility.HtmlDecode(selectedRow.Cells[4].Text).ToString();

                Dt_Capitulos = SAP_Concepto.Consulta_Capitulos();
                foreach (DataRow Registro in Dt_Capitulos.Rows) 
                {
                    Cmb_Capitulo.Items.Add(Registro[Cat_SAP_Capitulos.Campo_Clave].ToString());
                }

                Txt_Clave.Text = Clave;
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Estatus));
                Cmb_Capitulo.SelectedIndex= Cmb_Capitulo.Items.IndexOf(Cmb_Capitulo.Items.FindByValue(Capitulo));
                Txt_Comentarios.Text = Descripcion;
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    //protected void custPager_PageChanged(object sender, CustomPageChangeArgs e)
    //{
    //    Grid_SAP_Conceptos.DataSource = Dt_SAP_Conceptos;
    //    Grid_SAP_Conceptos.PageSize = e.CurrentPageSize;
    //    Grid_SAP_Conceptos.PageIndex = e.CurrentPageNumber;
        
    //    //Consulta_Percepciones_Deducciones();
    //}
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

    #region ORDENAR GRIDS
    /// ******************************************************************************************
    /// NOMBRE: Grid_Requisiciones_Sorting
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// CREÓ: Gustavo Angeles Cruz
    /// FECHA CREÓ: 11/Junio/2011
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// ******************************************************************************************

    protected void Grid_SAP_Conceptos_Sorting(object sender, GridViewSortEventArgs e)
    {
        Grid_Sorting(Grid_SAP_Conceptos, ((DataTable)Session[Dt_SAP_Conceptos]), e);
        
    }
    /// *****************************************************************************************
    /// NOMBRE: Grid_Sorting
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// CREÓ: Gustavo Angeles Cruz
    /// FECHA CREÓ: 11/Junio/2011
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************
    private void Grid_Sorting(GridView Grid, DataTable Dt_Table, GridViewSortEventArgs e)
    {
        if (Dt_Table != null)
        {
            DataView Dv_Vista = new DataView(Dt_Table);
            String Orden = ViewState["SortDirection"].ToString();
            if (Orden.Equals("ASC"))
            {
                Dv_Vista.Sort = e.SortExpression + " DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Vista.Sort = e.SortExpression + " ASC";
                ViewState["SortDirection"] = "ASC";
            }
            Grid.DataSource = Dv_Vista;
            Grid.DataBind();
        }
    }
    #endregion

}

