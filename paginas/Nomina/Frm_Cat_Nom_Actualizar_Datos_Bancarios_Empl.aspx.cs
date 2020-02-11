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
using Presidencia.Empleados.Negocios;
using Presidencia.Dependencias.Negocios;
using Presidencia.Constantes;
using Presidencia.Bancos_Nomina.Negocio;
using Presidencia.Ayudante_Informacion;

public partial class paginas_Nomina_Frm_Cat_Nom_Actualizar_Datos_Bancarios_Empl : System.Web.UI.Page
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
            Consultar_Bancos();
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

            Txt_Cuenta_Bancaria.Enabled = Habilitado;
            Txt_No_Tarjeta.Enabled = Habilitado;
            Cmb_Bancos.Enabled = Habilitado;
            Cmb_Forma_Pago.Enabled = Habilitado;
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
        Txt_Cuenta_Bancaria.Text = String.Empty;
        Txt_No_Empleado.Text = String.Empty;
        Txt_No_Tarjeta.Text = String.Empty;
        Txt_Nombre_Empleado.Text = String.Empty;
        Cmb_Bancos.SelectedIndex = -1;
        Cmb_Forma_Pago.SelectedIndex = -1;

        Grid_Busqueda_Empleados.SelectedIndex = -1;
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
    /// NOMBRE DE LA FUNCION: Consultar_Bancos
    /// 
    /// DESCRIPCION : Consulta los bancos que existen actualmente registrados en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Bancos()
    {
        Cls_Cat_Nom_Bancos_Negocio Obj_Bancos = new Cls_Cat_Nom_Bancos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Bancos = null;//Variable que almacena la lista de bancos registrados actualmente en el sistema.

        try
        {
            Dt_Bancos = Obj_Bancos.Consulta_Bancos();
            Cmb_Bancos.DataSource = Dt_Bancos;
            Cmb_Bancos.DataTextField = Cat_Nom_Bancos.Campo_Nombre;
            Cmb_Bancos.DataValueField = Cat_Nom_Bancos.Campo_Banco_ID;
            Cmb_Bancos.DataBind();
            Cmb_Bancos.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
            Cmb_Bancos.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los bancos que existen actualmente en sistema. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Metodos Validacion)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Bancos
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (string.IsNullOrEmpty(Txt_Cuenta_Bancaria.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El número de cuanta bancaria es un dato requerido por el sistema. <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_No_Tarjeta.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Número de tarjeta es un dato requerido por el sistema. <br>";
            Datos_Validos = false;
        }

        if (Cmb_Bancos.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Banco es un dato requerido por el sistema. <br>";
            Datos_Validos = false;
        }

        if (Cmb_Forma_Pago.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La forma de pago es un dato requerido por el sistema. <br>";
            Datos_Validos = false;
        }
        return Datos_Validos;
    }
    #endregion

    #region (Operación)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Actualizar_Datos_Bancarios_Empleado
    /// 
    /// DESCRIPCION : Ejecuta la actualziación de los datos bancarios del empleado.
    ///               
    /// PARAMETROS  : No Aplica.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Actualizar_Datos_Bancarios_Empleado()
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.

        try
        {
            Obj_Empleados.P_No_Empleado = Txt_No_Empleado.Text.Trim();
            Obj_Empleados.P_No_Tarjeta = Txt_No_Tarjeta.Text.Trim();
            Obj_Empleados.P_No_Cuenta_Bancaria = Txt_Cuenta_Bancaria.Text.Trim();
            Obj_Empleados.P_Banco_ID = Cmb_Bancos.SelectedValue.Trim();
            Obj_Empleados.P_Forma_Pago = Cmb_Forma_Pago.SelectedItem.Text.Trim();

            Obj_Empleados.Actualizar_Datos_Bancarios_Empleado();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al actualizar los datos bancarios. Error: [" + Ex.Message + "]");
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
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llenar_Grid_Busqueda_Empleados()
    {
        Cls_Cat_Empleados_Negocios Negocio = new Cls_Cat_Empleados_Negocios();

        Grid_Busqueda_Empleados.SelectedIndex = (-1);
        Grid_Busqueda_Empleados.Columns[1].Visible = true;
       
        if (Txt_Busqueda_No_Empleado.Text.Trim().Length > 0) { Negocio.P_No_Empleado = Txt_Busqueda_No_Empleado.Text.Trim(); }
        if (Txt_Busqueda_RFC.Text.Trim().Length > 0) { Negocio.P_RFC = Txt_Busqueda_RFC.Text.Trim(); }
        if (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length > 0) { Negocio.P_Nombre = Txt_Busqueda_Nombre_Empleado.Text.Trim(); }
        if (Cmb_Busqueda_Dependencia.SelectedIndex > 0) { Negocio.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedItem.Value; }


        Grid_Busqueda_Empleados.DataSource = Negocio.Consultar_Empleados_Resguardos();
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
        Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;

        try
        {
            if (Grid_Busqueda_Empleados.SelectedIndex > (-1))
            {
                INF_EMPLEADO = Cls_Ayudante_Nom_Informacion._Informacion_Empleado(HttpUtility.HtmlDecode(Grid_Busqueda_Empleados.Rows[Grid_Busqueda_Empleados.SelectedIndex].Cells[2].Text));

                Txt_No_Empleado.Text = HttpUtility.HtmlDecode(Grid_Busqueda_Empleados.Rows[Grid_Busqueda_Empleados.SelectedIndex].Cells[2].Text);
                Txt_Nombre_Empleado.Text = HttpUtility.HtmlDecode(Grid_Busqueda_Empleados.Rows[Grid_Busqueda_Empleados.SelectedIndex].Cells[3].Text);

                Txt_Cuenta_Bancaria.Text = INF_EMPLEADO.P_No_Cuenta_Bancaria;
                Txt_No_Tarjeta.Text = INF_EMPLEADO.P_No_Tarjeta;
                Cmb_Bancos.SelectedIndex = Cmb_Bancos.Items.IndexOf(Cmb_Bancos.Items.FindByValue(INF_EMPLEADO.P_Banco_ID));
                Cmb_Forma_Pago.SelectedIndex = Cmb_Forma_Pago.Items.IndexOf(Cmb_Forma_Pago.Items.FindByText(INF_EMPLEADO.P_Forma_Pago));

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
                    Actualizar_Datos_Bancarios_Empleado();
                    Habilitar_Controles("Inicial");
                    Limpiar_Controles();
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
