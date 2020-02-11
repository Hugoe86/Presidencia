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
using Presidencia.Empleados.Negocios;
using Presidencia.Indemnizacion.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Constantes;
using Presidencia.Bancos_Nomina.Negocio;
using Presidencia.Ayudante_Informacion;
using Presidencia.Nomina_Pago_Indemnizacion.Negocio;

public partial class paginas_Nomina_Frm_Cat_Nom_Pago_Indemnizacion : System.Web.UI.Page
{
    #region (Page Load)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Page_Load
    /// 
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    ///               
    /// PARAMETROS  : No Aplica.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Inicializa_Controles();
            }

            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Ecabezado_Mensaje.Text = String.Empty;
            Lbl_Ecabezado_Mensaje.Visible = false;
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    #endregion

    #region (Metodos)

    #region (Generales)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// 
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    ///               
    /// PARAMETROS  : No Aplica.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Limpiar_Controles();
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Consultar_SAP_Unidades_Responsables();
            Consultar_Indemnizaciones();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al inicializar los controles de la página. Error: [" + ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Diciembre/2011
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
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }
            Cmb_Tipo_Finiquito.Enabled = Habilitado;
            Txt_Fecha_Baja.Enabled = false;
            Txt_Nombre_Empleado.Enabled = false;
            Txt_No_Empleado.Enabled = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Ctlr
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        Txt_No_Empleado.Text = String.Empty;
        Txt_Nombre_Empleado.Text = String.Empty;
        Cmb_Tipo_Finiquito.SelectedIndex = -1;
        Txt_Fecha_Baja.Text = String.Empty;

        Grid_Busqueda_Empleados.DataSource = new DataTable();
        Grid_Busqueda_Empleados.DataBind();
    }
    #endregion

    #region (Consultas)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_SAP_Unidades_Responsables
    /// 
    /// DESCRIPCION : Consulta los unidades responsables que existen actualmente 
    ///               registrados en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_SAP_Unidades_Responsables()
    {
        Cls_Cat_Dependencias_Negocio Obj_Unidades_Responsables = new Cls_Cat_Dependencias_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Unidades_Responsables = null;//Variable que lista las unidades responsables registrdas en sistema.

        try
        {
            Dt_Unidades_Responsables = Obj_Unidades_Responsables.Consulta_Dependencias();
            Cmb_Busqueda_Dependencia.DataSource = Dt_Unidades_Responsables;
            Cmb_Busqueda_Dependencia.DataTextField = "CLAVE_NOMBRE";
            Cmb_Busqueda_Dependencia.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_Busqueda_Dependencia.DataBind();
            Cmb_Busqueda_Dependencia.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Dependencia.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las unidades responsables registradas en sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Indemnizaciones
    /// 
    /// DESCRIPCION : Consulta los registros de indemnización.
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 20/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Consultar_Indemnizaciones()
    {
        Cls_Cat_Nom_Indemnizacion_Negocio Obj_Indemniacion = new Cls_Cat_Nom_Indemnizacion_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Indemnizaciones = null;//Variable que listas las indemnizaciones registradas en sistema.

        try
        {
            Dt_Indemnizaciones = Obj_Indemniacion.Consultar_Indemnizaciones();
            Cmb_Tipo_Finiquito.DataSource = Dt_Indemnizaciones;

            if (Dt_Indemnizaciones is DataTable)
            {
                if (Dt_Indemnizaciones.Rows.Count > 0)
                {
                    foreach (DataRow INDEMNIZACION in Dt_Indemnizaciones.Rows)
                    {
                        if (INDEMNIZACION is DataRow)
                        {
                            INDEMNIZACION[Cat_Nom_Indemnizacion.Campo_Nombre] = "[" + INDEMNIZACION[Cat_Nom_Indemnizacion.Campo_Dias].ToString().Trim() + "] -- " +
                                INDEMNIZACION[Cat_Nom_Indemnizacion.Campo_Nombre].ToString().Trim();
                        }
                    }
                }
            }

            Cmb_Tipo_Finiquito.DataTextField = Cat_Nom_Indemnizacion.Campo_Nombre;
            Cmb_Tipo_Finiquito.DataValueField = Cat_Nom_Indemnizacion.Campo_Indemnizacion_ID;
            Cmb_Tipo_Finiquito.DataBind();

            Cmb_Tipo_Finiquito.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
            Cmb_Tipo_Finiquito.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las indemnizaciones en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    
    #endregion

    #region (Metodos Validacion)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Bancos
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 03/Abril/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (string.IsNullOrEmpty(Txt_No_Empleado.Text) || string.IsNullOrEmpty(Txt_Nombre_Empleado.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione algun registro. <br>";
            Datos_Validos = false;
        }

        if (Cmb_Tipo_Finiquito.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione el tipo de finiquito. <br>";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }
    #endregion

    #region (Operación)
     ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Empleado
    /// DESCRIPCION : Modifica los datos del Empleado con los proporcionados por el usuario en la BD
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :Juan Alberto Hernandez Negrete
    /// FECHA_MODIFICO    :3/Noviembre/2010
    /// CAUSA_MODIFICACION: Completar el Catalogo
    ///*******************************************************************************
    private void Modificar_Empleado()
    {
        Cls_Cat_Nom_Pago_Indemnizacion_Negocio Rs_Modificar = new Cls_Cat_Nom_Pago_Indemnizacion_Negocio();
        try
        {
            Rs_Modificar.P_Empleado_ID = Hdf_Empleado_ID.Value;
            Rs_Modificar.P_Tipo_Finiquito = Cmb_Tipo_Finiquito.SelectedValue;
            Rs_Modificar.Modificar_Empleado_Tipo_Finiquito();
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Empleado " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #endregion


    #region (GridView)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llenar_Grid_Busqueda_Empleados
    /// 
    /// DESCRIPCION : Consulta y carga el grid de los empleados.
    ///               
    /// PARAMETROS  : No Aplica.
    /// 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 03/Abril/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llenar_Grid_Busqueda_Empleados()
    {
        Cls_Cat_Empleados_Negocios Negocio = new Cls_Cat_Empleados_Negocios(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Consulta = new DataTable();

        Grid_Busqueda_Empleados.SelectedIndex = (-1);
        Grid_Busqueda_Empleados.Columns[1].Visible = true;

        if (Txt_Busqueda_No_Empleado.Text.Trim().Length > 0) { Negocio.P_No_Empleado = Txt_Busqueda_No_Empleado.Text.Trim(); }
        
        if (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length > 0) { Negocio.P_Nombre = Txt_Busqueda_Nombre_Empleado.Text.Trim(); }
        if (Cmb_Busqueda_Dependencia.SelectedIndex > 0) { Negocio.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedItem.Value; }
        Negocio.P_Estatus = "INACTIVO";
        Dt_Consulta = Negocio.Consulta_Empleados_General();

        Grid_Busqueda_Empleados.DataSource = Dt_Consulta;
        Grid_Busqueda_Empleados.DataBind();
        Grid_Busqueda_Empleados.Columns[1].Visible = false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Busqueda_Empleados_PageIndexChanging
    /// 
    /// DESCRIPCION : Cambia la pagina del grid del empleados.
    ///               
    /// PARAMETROS  : No Aplica.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Busqueda_Empleados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Busqueda_Empleados.PageIndex = e.NewPageIndex;
            Llenar_Grid_Busqueda_Empleados();
            MPE_Empleados.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Busqueda_Empleados_SelectedIndexChanged
    /// 
    /// DESCRIPCION : Carga la información del registro seleccionado.
    ///               
    /// PARAMETROS  : No Aplica.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Busqueda_Empleados_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Busqueda_Empleados.SelectedIndex > (-1))
            {
                Grid_Busqueda_Empleados.Columns[1].Visible = true;
                Hdf_Empleado_ID.Value = HttpUtility.HtmlDecode(Grid_Busqueda_Empleados.Rows[Grid_Busqueda_Empleados.SelectedIndex].Cells[1].Text);
                Txt_No_Empleado.Text = HttpUtility.HtmlDecode(Grid_Busqueda_Empleados.Rows[Grid_Busqueda_Empleados.SelectedIndex].Cells[2].Text);
                Txt_Nombre_Empleado.Text = HttpUtility.HtmlDecode(Grid_Busqueda_Empleados.Rows[Grid_Busqueda_Empleados.SelectedIndex].Cells[3].Text);
                Txt_Fecha_Baja.Text = String.Format("{0:dd/MMM/yyyy}",Convert.ToDateTime(HttpUtility.HtmlDecode(Grid_Busqueda_Empleados.Rows[Grid_Busqueda_Empleados.SelectedIndex].Cells[4].Text)));
                Cmb_Tipo_Finiquito.SelectedIndex = Cmb_Tipo_Finiquito.Items.IndexOf(Cmb_Tipo_Finiquito.Items.FindByValue(HttpUtility.HtmlDecode(Grid_Busqueda_Empleados.Rows[Grid_Busqueda_Empleados.SelectedIndex].Cells[5].Text)));
                
                Grid_Busqueda_Empleados.Columns[1].Visible = false;

                MPE_Empleados.Hide();
                UpPnl_aux_Busqueda.Update();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    #endregion

     #region (Eventos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Actualizar ,los datos bancarios del empleado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 08/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Modificar.ToolTip.Equals("Modificar"))
            {
                if (!Txt_No_Empleado.Text.Equals(""))
                {
                    Habilitar_Controles("Modificar");
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea modificar sus datos <br>";
                }
            }
            else
            {
                if (Validar_Datos())
                {
                    Modificar_Empleado();
                    Habilitar_Controles("Inicial");
                    Limpiar_Controles();
                    Pnl_Busqueda_Contenedor.Style.Add("display", "none");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Operación Completa');", true);
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
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
                Habilitar_Controles("Inicial");//Habilita los controles para la siguiente operación del usuario en el catálogo
                Limpiar_Controles();
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Empleados_Click
    ///DESCRIPCIÓN: Ejecuta la busqueda de empleados.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 08/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e)
    {
        try
        {
            Grid_Busqueda_Empleados.PageIndex = 0;
            Llenar_Grid_Busqueda_Empleados();
            MPE_Empleados.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    #endregion

}

