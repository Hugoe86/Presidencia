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
using Presidencia.Nomina_Operacion_Proveedores.Negocio;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Empleados.Negocios;
using Presidencia.Faltas_Empleado.Negocio;

public partial class paginas_Nomina_Frm_Ope_Nom_Ver_Registros_Proveedores : System.Web.UI.Page
{

    #region Page_Load
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Carga e inicializa los campos del formulario.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 22/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Lbl_Ecabezado_Mensaje.Text = "";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = false;
            if (!IsPostBack)
            {
                Consultar_Calendarios_Nomina();
                //Seleccionar_Nomina_Periodo();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Se ha producido un Error:";
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

    #region (Metodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Seleccionar_Nomina_Periodo
    ///DESCRIPCIÓN: Se carga la parte de nomina y periodo de acuerdo al actual.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 22/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Seleccionar_Nomina_Periodo()
    {
        Cls_Ope_Nom_Proveedores_Negocio Obj_Proveedores = new Cls_Ope_Nom_Proveedores_Negocio();
        DataTable Dt_Proveedores = null;

        try
        {
            Obj_Proveedores.P_Fecha_Busqueda = String.Format("{0:dd/MM/yyyy}", DateTime.Now);
            Dt_Proveedores = Obj_Proveedores.Identificar_Periodo_Nomina();

            if (Dt_Proveedores is DataTable)
            {
                if (Dt_Proveedores.Rows.Count > 0)
                {
                    foreach (DataRow NOMINA in Dt_Proveedores.Rows)
                    {
                        if (NOMINA is DataRow)
                        {
                            Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf(
                                Cmb_Calendario_Nomina.Items.FindByValue(NOMINA[Cat_Nom_Nominas_Detalles.Campo_Nomina_ID].ToString()));

                            Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());

                            Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Periodos_Catorcenales_Nomina.Items.IndexOf(
                                Cmb_Periodos_Catorcenales_Nomina.Items.FindByValue(NOMINA[Cat_Nom_Nominas_Detalles.Campo_No_Nomina].ToString()));
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Se ha producido un Error:";
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    #endregion

    #region Validaciones
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean IsNumeric(String Cadena)
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
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Carga_Datos
    ///DESCRIPCIÓN: Valida que los campos para cargar los datos.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 22/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Carga_Datos()
    {
        Lbl_Ecabezado_Mensaje.Text = "Para hacer la Busqueda es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Cmb_Seleccionar_Empleado.SelectedIndex == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Seleccionar el Empleado.";
            Validacion = false;
        }
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }
    #endregion

    #endregion

    #region Grids
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Listado_Detalles
    ///DESCRIPCIÓN: Se carga el grid de detalle dependiendo de los campos.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 22/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Listado_Detalles()
    {
        try
        {
            Grid_Detalles_Proveedores.Columns[0].Visible = true;
            Grid_Detalles_Proveedores.Columns[1].Visible = true;
            Cls_Ope_Nom_Proveedores_Negocio Proveedores_Negocio = new Cls_Ope_Nom_Proveedores_Negocio();
            Proveedores_Negocio.P_Empleado_ID = Cmb_Seleccionar_Empleado.SelectedItem.Value;
            Proveedores_Negocio.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedItem.Value;
            if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex > 0) Proveedores_Negocio.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());
            Grid_Detalles_Proveedores.DataSource = Proveedores_Negocio.Consultar_Detalles_Registro_Proveedores();
            Grid_Detalles_Proveedores.DataBind();
            Grid_Detalles_Proveedores.Columns[0].Visible = false;
            Grid_Detalles_Proveedores.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Se ha producido un Error:";
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Detalles_Proveedoresr_RowDataBound
    ///DESCRIPCIÓN: Evento RowDataBound del Grid de Detalle.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 22/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Detalles_Proveedoresr_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                Button Btn_Autorizar = null;
                Button Btn_Rechazar = null;
                if (e.Row.FindControl("Btn_Autorizar") != null)
                {
                    Btn_Autorizar = (Button)e.Row.FindControl("Btn_Autorizar");
                    Btn_Autorizar.CommandArgument = e.Row.Cells[0].Text.Trim();
                    Btn_Autorizar.ToolTip = "Autorizar";
                }
                if (e.Row.FindControl("Btn_Rechazar") != null)
                {
                    Btn_Rechazar = (Button)e.Row.FindControl("Btn_Rechazar");
                    Btn_Rechazar.CommandArgument = e.Row.Cells[0].Text.Trim();
                    Btn_Rechazar.ToolTip = "Rechazar";
                }
                if (e.Row.Cells[1].Text.Contains("RECHAZADO"))
                {
                    e.Row.Style.Add("background", "#F78181 url(../imagenes/paginas/titleBackground.png) repeat-x top");
                }
                else
                {
                    e.Row.Style.Add("background", "#CEF6CE url(../imagenes/paginas/titleBackground.png) repeat-x top");
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Se ha producido un Error:";
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    #endregion

    #region Eventos

    #region (Botones)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cargar_Datos_Click
    ///DESCRIPCIÓN: Evento Click del Btn_Cargar. Manda llenar el Grid de Detalle.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 22/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Cargar_Datos_Click(object sender, EventArgs e)
    {
        if (Validar_Carga_Datos())
        {
            Llenar_Grid_Listado_Detalles();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Autorizar_Click
    ///DESCRIPCIÓN: Evento Click del Btn_Autorizar. Autoriza el detalle de pago del
    ///             proveedor.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 22/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Autorizar_Click(object sender, EventArgs e)
    {
        try
        {
            Cls_Ope_Nom_Proveedores_Negocio Proveedor = new Cls_Ope_Nom_Proveedores_Negocio();
            Proveedor.P_No_Movimiento_Detalle = Convert.ToInt32(((Button)sender).CommandArgument);
            Proveedor.P_Estatus = "ACEPTADO";
            Proveedor.Modificar_Estatus_Detalle_Proveedores();

            Llenar_Grid_Listado_Detalles();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Se ha producido un Error:";
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Rechazar_Click
    ///DESCRIPCIÓN: Evento Click del Btn_Rechazar. Rechaza el detalle de pago del
    ///             proveedor.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 22/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Rechazar_Click(object sender, EventArgs e)
    {
        try
        {
            Cls_Ope_Nom_Proveedores_Negocio Proveedor = new Cls_Ope_Nom_Proveedores_Negocio();
            Proveedor.P_No_Movimiento_Detalle = Convert.ToInt32(((Button)sender).CommandArgument);
            Proveedor.P_Estatus = "RECHAZADO";
            Proveedor.Modificar_Estatus_Detalle_Proveedores();
            Llenar_Grid_Listado_Detalles();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Se ha producido un Error:";
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Empleado_Click
    ///DESCRIPCIÓN: Evento Click del Btn_Buscar_Empleado. Llena el Combo con las coin-
    ///             cidencias seleccionadas.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 22/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Empleado_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Txt_Buscar_Empleado_No_Empleado.Text.Trim().Length > 0 || Txt_Buscar_Empleado_Nombre.Text.Trim().Length > 0)
            {
                Cls_Cat_Empleados_Negocios Empleados_Negocio = new Cls_Cat_Empleados_Negocios();
                Empleados_Negocio.P_No_Empleado = (Txt_Buscar_Empleado_No_Empleado.Text.Trim().Length > 0) ? Txt_Buscar_Empleado_No_Empleado.Text.Trim() : "";
                Empleados_Negocio.P_Nombre = (Txt_Buscar_Empleado_Nombre.Text.Trim().Length > 0) ? Txt_Buscar_Empleado_Nombre.Text.Trim() : "";
                DataTable Dt_Empleados = Empleados_Negocio.Consulta_Empleados_General();
                Cmb_Seleccionar_Empleado.DataSource = Dt_Empleados;
                Cmb_Seleccionar_Empleado.DataTextField = "EMPLEADO";
                Cmb_Seleccionar_Empleado.DataValueField = Cat_Empleados.Campo_Empleado_ID;
                Cmb_Seleccionar_Empleado.DataBind();
                Cmb_Seleccionar_Empleado.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
                Txt_Buscar_Empleado_No_Empleado.Text = "";
                Txt_Buscar_Empleado_Nombre.Text = "";

                if (Dt_Empleados.Rows.Count > 0)
                {
                    Cmb_Seleccionar_Empleado.SelectedIndex = 1;
                    Cmb_Seleccionar_Empleado.Focus();
                }

            }
            else
            {
                Cmb_Seleccionar_Empleado.DataSource = new DataTable();
                Cmb_Seleccionar_Empleado.DataBind();
                Cmb_Seleccionar_Empleado.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
                Lbl_Ecabezado_Mensaje.Text = "No hay Datos en los Filtros de Busqueda.";
                Lbl_Mensaje_Error.Text = "[Debe haber minimo en uno]";
                Div_Contenedor_Msj_Error.Visible = true;
            }
            Grid_Detalles_Proveedores.DataSource = new DataTable();
            Grid_Detalles_Proveedores.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Se ha producido un Error:";
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    #endregion

    #region (Eventos Combos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Calendario_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 index = Cmb_Calendario_Nomina.SelectedIndex;
        if (index > 0)
        {
            Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
        }
        else
        {
            Cmb_Periodos_Catorcenales_Nomina.DataSource = new DataTable();
            Cmb_Periodos_Catorcenales_Nomina.DataBind();
        }
        Cmb_Calendario_Nomina.Focus();
    }
    #endregion

    #region (TextBox)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Txt_Buscar_Empleado_No_Empleado_TextChanged
    /// DESCRIPCION : Busca al empleado por Numero de Control
    /// CREO        : Armando Zavala Moreno
    /// FECHA_CREO  : 30/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Buscar_Empleado_No_Empleado_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Txt_Buscar_Empleado_No_Empleado.Text = String.Format("{0:000000}", Convert.ToInt64(Txt_Buscar_Empleado_No_Empleado.Text.Trim()));
            Btn_Buscar_Empleado_Click(Btn_Buscar_Empleado, null);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al buscar el Empleado. Error: [" + Ex.Message + "]");
        }
    }
        ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Txt_Buscar_Empleado_Nombre
    /// DESCRIPCION : Busca al empleado por Nombre
    /// CREO        : Armando Zavala Moreno
    /// FECHA_CREO  : 30/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Buscar_Empleado_Nombre_TextChanged(object sender, EventArgs e)
    {
        try
        {            
            Btn_Buscar_Empleado_Click(Btn_Buscar_Empleado, null);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al buscar el Empleado. Error: [" + Ex.Message + "]");
        }
    }    
    #endregion

    #endregion

    #region (Calendario Nomina)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Calendarios_Nomina
    /// DESCRIPCION : 
    /// 
    /// PARAMETROS:
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Calendarios_Nomina()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nominales = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Calendarios_Nominales = null;//Variable que almacena los calendarios nominales que existén actualmente en el sistema.
        try
        {
            Dt_Calendarios_Nominales = Obj_Calendario_Nominales.Consultar_Calendario_Nominas();
            Dt_Calendarios_Nominales = Formato_Fecha_Calendario_Nomina(Dt_Calendarios_Nominales);

            if (Dt_Calendarios_Nominales is DataTable)
            {
                Cmb_Calendario_Nomina.DataSource = Dt_Calendarios_Nominales;
                Cmb_Calendario_Nomina.DataTextField = "Nomina";
                Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                Cmb_Calendario_Nomina.DataBind();
                Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
                Cmb_Calendario_Nomina.SelectedIndex = -1;
                Cmb_Calendario_Nomina.DataSource = Dt_Calendarios_Nominales;
                Cmb_Calendario_Nomina.DataTextField = "Nomina";
                Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                Cmb_Calendario_Nomina.DataBind();
                Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));

                Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf
                    (Cmb_Calendario_Nomina.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Anyo));

                if (Cmb_Calendario_Nomina.SelectedIndex > 0)
                {
                    Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
                }
                Cmb_Calendario_Nomina.Focus();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los calendarios de nómina que existen actualmente registrados en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales para el 
    ///calendario de nomina seleccionado.
    ///PARAMETROS: Nomina_ID.- Indica el calendario de nomina del cuál se desea consultar
    ///                        los periodos catorcenales.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Periodos_Catorcenales_Nomina(String Nomina_ID)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nomina_Periodos = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Clase de conexion con la capa de negocios
        DataTable Dt_Periodos_Catorcenales = null;//Variable que almacenra unaa lista de los periodos catorcenales que le correspónden a la nomina seleccionada.

        try
        {
            Consulta_Calendario_Nomina_Periodos.P_Nomina_ID = Nomina_ID;
            Dt_Periodos_Catorcenales = Consulta_Calendario_Nomina_Periodos.Consulta_Detalles_Nomina();
            if (Dt_Periodos_Catorcenales != null)
            {
                if (Dt_Periodos_Catorcenales.Rows.Count > 0)
                {
                    Cmb_Periodos_Catorcenales_Nomina.DataSource = Dt_Periodos_Catorcenales;
                    Cmb_Periodos_Catorcenales_Nomina.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataValueField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataBind();
                    Cmb_Periodos_Catorcenales_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;

                    Validar_Periodos_Pago(Cmb_Periodos_Catorcenales_Nomina);

                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Periodos_Catorcenales_Nomina.Items.IndexOf(Cmb_Periodos_Catorcenales_Nomina.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Periodo));
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    IBtn_Imagen_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron periodos catorcenales para la nomina seleccionada.";
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los periodos catorcenales del  calendario de nomina seleccionado. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la deduccion 
    ///a partir del periodo actual.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Validar_Periodos_Pago(DropDownList Combo)
    {
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Actual = DateTime.Now;
        DateTime Fecha_Inicio = new DateTime();
        DateTime Fecha_Fin = new DateTime();

        Prestamos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

        foreach (ListItem Elemento in Combo.Items)
        {
            if (IsNumeric(Elemento.Text.Trim()))
            {
                Prestamos.P_No_Nomina = Convert.ToInt32(Elemento.Text.Trim());
                Dt_Detalles_Nomina = Prestamos.Consultar_Fechas_Periodo();

                if (Dt_Detalles_Nomina != null)
                {
                    if (Dt_Detalles_Nomina.Rows.Count > 0)
                    {
                        Fecha_Inicio = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString());
                        Fecha_Fin = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString());

                        //if (Fecha_Fin >= Fecha_Actual)
                        //{
                        //    Elemento.Enabled = true;
                        //}
                        //else
                        //{
                        //    Elemento.Enabled = false;
                        //}
                    }
                }
            }
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
    /// DESCRIPCION : Crea el DataTable con la consulta de las nomina vigentes en el 
    /// sistema.
    /// PARAMETROS: Dt_Calendario_Nominas.- Lista de las nominas vigentes actualmente 
    ///             en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Formato_Fecha_Calendario_Nomina(DataTable Dt_Calendario_Nominas)
    {
        DataTable Dt_Nominas = new DataTable();
        DataRow Renglon_Dt_Clon = null;
        Dt_Nominas.Columns.Add("Nomina", typeof(System.String));
        Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

        if (Dt_Calendario_Nominas is DataTable)
        {
            foreach (DataRow Renglon in Dt_Calendario_Nominas.Rows)
            {
                if (Renglon is DataRow)
                {
                    Renglon_Dt_Clon = Dt_Nominas.NewRow();
                    Renglon_Dt_Clon["Nomina"] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString().Split(new char[] { ' ' })[0].Split(new char[] { '/' })[2];
                    Renglon_Dt_Clon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID];
                    Dt_Nominas.Rows.Add(Renglon_Dt_Clon);
                }
            }
        }
        return Dt_Nominas;
    }
    #endregion
}
