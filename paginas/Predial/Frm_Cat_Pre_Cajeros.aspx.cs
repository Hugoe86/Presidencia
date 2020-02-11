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
using Presidencia.Catalogo_Cajeros.Negocio;

public partial class paginas_Predial_Frm_Cat_Pre_Cajeros : System.Web.UI.Page
{

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 01/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Configuracion_Formulario(true);
            Llenar_Tabla_Cajeros(0);
            Llenar_Combo_Modulo();
            Llenar_Combo_Turno();
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario de Cajeros
    ///             y la Asignacion de Cajas
    ///PROPIEDADES:     
    ///             1. Estatus.    Estatus en el que se cargara la configuración de los controles.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 04/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        //Btn_Salir.AlternateText = "Salir";
        //Btn_Guardar.AlternateText = "Guardar";
        Btn_Busqueda.AlternateText = "Buscar";
        Btn_Guardar.Visible = !Estatus;
        Tbp_Asignacion.Enabled = !Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Encontrar_Cajero
    ///DESCRIPCIÓN: Permite verificar que una Asignacion de Cajas no sea repetida.
    ///PROPIEDADES:   
    ///             1. Empleado.    Numero de Empleado que se verificara.
    ///             2. Modulo.      Nombre del Modulo que se verificara.
    ///             3. Caja.        Numero de la Caja que se verificara.
    ///             4. Turno.       Nombre del turno que se verificara.
    ///             5. Tabla.       Tabla a la que se hara la verificación.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 07/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Encontrar_Cajero(String Empleado, String Modulo, String Caja, String Turno, DataTable Tabla)
    {
        Boolean Encontrada = false;
        if (Tabla != null && Tabla.Rows.Count > 0)
        {
            for (int cnt = 0; cnt < Tabla.Rows.Count; cnt++)
            {
                if (Tabla.Rows[cnt][1].ToString().Trim().Equals(Empleado) &&
                    Tabla.Rows[cnt][2].ToString().Trim().Equals(Modulo) &&
                    Tabla.Rows[cnt][4].ToString().Trim().Equals(Caja) &&
                    Tabla.Rows[cnt][6].ToString().Trim().Equals(Turno))
                {
                    Encontrada = true;
                    break;
                }
            }
        }
        return Encontrada;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Modulo
    ///DESCRIPCIÓN: Metodo que llena el Combo de Modulos con los modulos existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 04/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Modulo()
    {
        try
        {
            Cls_Cat_Pre_Cajeros_Negocio Cajero = new Cls_Cat_Pre_Cajeros_Negocio();
            DataTable Cajeros = Cajero.Consultar_Modulos();
            DataRow fila = Cajeros.NewRow();
            fila[Cat_Pre_Modulos.Campo_Ubicacion] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            fila[Cat_Pre_Modulos.Campo_Modulo_Id] = "SELECCIONE";
            Cajeros.Rows.InsertAt(fila, 0);
            Cmb_Modulo.DataTextField = Cat_Pre_Modulos.Campo_Ubicacion;
            Cmb_Modulo.DataValueField = Cat_Pre_Modulos.Campo_Modulo_Id;
            Cmb_Modulo.DataSource = Cajeros;
            Cmb_Modulo.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Truno
    ///DESCRIPCIÓN: Metodo que llena el Combo de Turnos con los turnos existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 04/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Turno()
    {
        try
        {
            Cls_Cat_Pre_Cajeros_Negocio Cajero = new Cls_Cat_Pre_Cajeros_Negocio();
            DataTable Cajeros = Cajero.Consultar_Turnos();
            DataRow fila = Cajeros.NewRow();
            fila[Cat_Pre_Turnos.Campo_Descripcion] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            fila[Cat_Pre_Turnos.Campo_Turno_ID] = "SELECCIONE";
            Cajeros.Rows.InsertAt(fila, 0);
            Cmb_Turno.DataTextField = Cat_Pre_Turnos.Campo_Descripcion;
            Cmb_Turno.DataValueField = Cat_Pre_Turnos.Campo_Turno_ID;
            Cmb_Turno.DataSource = Cajeros;
            Cmb_Turno.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Caja
    ///DESCRIPCIÓN: Metodo que llena el Combo de Cajas con las cajas existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 04/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Caja()
    {
        try
        {
            Cls_Cat_Pre_Cajeros_Negocio Cajero = new Cls_Cat_Pre_Cajeros_Negocio();
            Cajero.P_Modulo_ID = HttpUtility.HtmlDecode(Cmb_Modulo.SelectedItem.Value.Trim());
            DataTable Cajeros = Cajero.Consultar_Cajas();
            DataRow fila = Cajeros.NewRow();
            fila[Cat_Pre_Cajas.Campo_Numero_De_Caja] = "0";
            fila[Cat_Pre_Cajas.Campo_Caja_Id] = "00000";
            Cajeros.Rows.InsertAt(fila, 0);
            Cmb_Caja.DataTextField = Cat_Pre_Cajas.Campo_Numero_De_Caja;
            Cmb_Caja.DataValueField = Cat_Pre_Cajas.Campo_Caja_Id;
            Cmb_Caja.DataSource = Cajeros;
            Cmb_Caja.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 04/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Cmb_Turno.SelectedIndex = 0;
        Cmb_Caja.SelectedIndex = 0;
    }
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Cajeros
    ///DESCRIPCIÓN: Permite validar que los campos de Asignacion de Cajas no esten vacios.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 07/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes_Cajeros()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Cmb_Modulo.SelectedIndex == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Modulo.";
            Validacion = false;
        }
        if (Cmb_Caja.SelectedIndex == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Caja.";
            Validacion = false;
        }
        if (Cmb_Turno.SelectedIndex == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Turno.";
            Validacion = false;
        }
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }
    
    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Cajeros
    ///DESCRIPCIÓN: Llena la tabla de Cajeros
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 05/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Cajeros(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Cajeros_Negocio Cajeros = new Cls_Cat_Pre_Cajeros_Negocio();
            DataTable Grid = new DataTable();
            Grid = Cajeros.Consultar_Cajeros();
            if (Grid.Rows.Count > 0)
            {
                Grid_Cajeros.DataSource = Grid;
                Session["Grid"] = Grid;
                Grid_Cajeros.PageIndex = Pagina;
                Grid_Cajeros.DataBind();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Cajeros
    ///DESCRIPCIÓN: Llena la tabla de Cajeros
    ///PROPIEDADES:     
    ///             1. Cajero.  Clase de negocios para buscar lo del campo de busqueda.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 07/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Cajeros(Cls_Cat_Pre_Cajeros_Negocio Cajero)
    {
        try
        {
            DataTable Grid = new DataTable();
            Grid = Cajero.Buscar_Cajero();
            if (Grid.Rows.Count > 0)
            {
                Grid_Cajeros.DataSource = Grid;
                Session["Grid"] = Grid;
                Grid_Cajeros.PageIndex = 0;
                Grid_Cajeros.DataBind();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Cajas_Detalles
    ///DESCRIPCIÓN: Llena la tabla de cajas detalles.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 06/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Cajas_Detalles() 
    {
        Cls_Cat_Pre_Cajeros_Negocio Cajero = new Cls_Cat_Pre_Cajeros_Negocio();
        Cajero.P_Cajas = (DataTable)Session["Asignacion_Turnos"];
        Cajero.Llenar_Tabla_Cajas_Detalles();
    }

    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Cajeros_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView General de los Cajeros
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 04/Julio/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Cajeros_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Tabla_Cajeros(e.NewPageIndex);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Cajeros_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos del Cajero seleccionado para mostrarlos en los
    ///             campos del formulario.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 04/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Cajeros_SelectedIndexChanged(object sender, EventArgs e)
    {
        String ID_Seleccionado = Grid_Cajeros.SelectedRow.Cells[1].Text;
        Cls_Cat_Pre_Cajeros_Negocio Cajero = new Cls_Cat_Pre_Cajeros_Negocio();
        Cajero.P_No_Empleado = ID_Seleccionado;
        Cajero = Cajero.Consultar_Datos_Cajero();
        Txt_Empleado_ID.Text = Cajero.P_Empleado_ID;
        Txt_Empleado_ID1.Text = Cajero.P_No_Empleado;
        Txt_Cajero.Text = Cajero.P_Nombre;
        Txt_Estatus.Text = Cajero.P_Estatus;
        Txt_Tipo.Text = Cajero.P_Tipo;
        Txt_Empleado_ID2.Text = Cajero.P_No_Empleado;
        System.Threading.Thread.Sleep(1000);
        Tbp_Asignacion.Enabled = true;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Asignacion_Turnos_SelectedIndexChanged
    ///DESCRIPCIÓN: Permite borrar un registro del grid antes de que sea insertado
    ///             a la base datos.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 06/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Asignacion_Turnos_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Asignacion_Turnos.Rows.Count > 0)
        {
            Int32 Registro = ((Grid_Asignacion_Turnos.PageIndex) *
            Grid_Asignacion_Turnos.PageSize) + (Grid_Asignacion_Turnos.SelectedIndex);
            if (Session["Asignacion_Turnos"] != null) 
            {
                DataTable Tabla = (DataTable)Session["Asignacion_Turnos"];
                Grid_Asignacion_Turnos.Columns[0].Visible = true;
                Grid_Asignacion_Turnos.Columns[3].Visible = true;
                Grid_Asignacion_Turnos.Columns[5].Visible = true;
                Tabla.Rows.RemoveAt(Registro);
                Session["Asignacion_Turnos"] = Tabla;
                Grid_Asignacion_Turnos.PageIndex = 0;
                Grid_Asignacion_Turnos.DataSource = Tabla;
                Grid_Asignacion_Turnos.DataBind();
                Grid_Asignacion_Turnos.Columns[0].Visible = false;
                Grid_Asignacion_Turnos.Columns[3].Visible = false;
                Grid_Asignacion_Turnos.Columns[5].Visible = false;
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Asignacion_Turnos_PageIndexChanging
    ///DESCRIPCIÓN: Permite manejar la paginacion del grid Asignacion de Cajas.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 06/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Asignacion_Turnos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable Tabla;
        Tabla = (DataTable)Session["Asignacion_Turnos"];
        Grid_Asignacion_Turnos.PageIndex = e.NewPageIndex;
        Grid_Asignacion_Turnos.DataSource = Tabla;
        Grid_Asignacion_Turnos.DataBind();
    }
   
    #endregion

    #region Combos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Modulo_SelectedIndexChanged
    ///DESCRIPCIÓN: Permite llenar automaticamente el combo de Cajas una vez que se haya
    ///             seleccionado un Modulo.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 05/Julio/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Modulo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Llenar_Combo_Caja();
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 07/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Pre_Cajeros_Negocio Cajero = new Cls_Cat_Pre_Cajeros_Negocio();
        Cajero.P_Busqueda = HttpUtility.HtmlDecode(Txt_Busqueda.Text.Trim().ToUpper());
        Llenar_Tabla_Cajeros(Cajero);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Click
    ///DESCRIPCIÓN: Permite ir agregando al grid de Asignacion de Cajas los detalles para
    ///             realizar posteriormente la inserccion de los Detalles de Caja.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 06/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Click(object sender, ImageClickEventArgs e)
    {
        if (Validar_Componentes_Cajeros())
        {
            Tbp_Cajeros.Enabled = false;
            Btn_Salir.AlternateText = "Cancelar";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
            Btn_Guardar.Visible = true;
            DataTable tabla;
            if (Session["Asignacion_Turnos"] == null)
            {
                tabla = (DataTable)Grid_Asignacion_Turnos.DataSource;
                Session["Asignacion_Turnos"] = tabla;
                tabla = new DataTable("Asignacion_Turnos");
                tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                tabla.Columns.Add("NO_EMPLEADO", Type.GetType("System.String"));
                tabla.Columns.Add("MODULO", Type.GetType("System.String"));
                tabla.Columns.Add("CAJA_ID", Type.GetType("System.String"));
                tabla.Columns.Add("CAJA", Type.GetType("System.String"));
                tabla.Columns.Add("TURNO_ID", Type.GetType("System.String"));
                tabla.Columns.Add("TURNO", Type.GetType("System.String"));
            }
            else
            {
                tabla = (DataTable)Session["Asignacion_Turnos"];
            }
            if (!Encontrar_Cajero(Txt_Empleado_ID2.Text.Trim(), Cmb_Modulo.SelectedItem.Text.ToString().Trim(),
                Cmb_Caja.SelectedItem.Text.ToString().Trim(), Cmb_Turno.SelectedItem.Text.ToString().Trim(), tabla))
            {
                DataRow fila = tabla.NewRow();
                fila["EMPLEADO_ID"] = HttpUtility.HtmlDecode(Txt_Empleado_ID.Text.Trim());
                fila["NO_EMPLEADO"] = HttpUtility.HtmlDecode(Txt_Empleado_ID2.Text.Trim());
                fila["MODULO"] = HttpUtility.HtmlDecode(Cmb_Modulo.SelectedItem.Text.Trim());
                fila["CAJA_ID"] = HttpUtility.HtmlDecode(Cmb_Caja.SelectedItem.Value);
                fila["CAJA"] = HttpUtility.HtmlDecode(Cmb_Caja.SelectedItem.Text.Trim());
                fila["TURNO_ID"] = HttpUtility.HtmlDecode(Cmb_Turno.SelectedItem.Value);
                fila["TURNO"] = HttpUtility.HtmlDecode(Cmb_Turno.SelectedItem.Text.Trim());
                Grid_Asignacion_Turnos.Columns[0].Visible = true;
                Grid_Asignacion_Turnos.Columns[3].Visible = true;
                Grid_Asignacion_Turnos.Columns[5].Visible = true;
                tabla.Rows.Add(fila);
                Session["Asignacion_Turnos"] = tabla;
                Grid_Asignacion_Turnos.DataSource = tabla;
                Grid_Asignacion_Turnos.DataBind();
                Grid_Asignacion_Turnos.Columns[0].Visible = false;
                Grid_Asignacion_Turnos.Columns[3].Visible = false;
                Grid_Asignacion_Turnos.Columns[5].Visible = false;
                Limpiar_Catalogo();
            }
            else 
            {
                Lbl_Ecabezado_Mensaje.Text = "Ese cajero con esa descripcion ya esta agregada en la tabla.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Guardar_Click
    ///DESCRIPCIÓN: Permite guardar los registros añadidos al grid de Asignacion de Cajas en la base 
    ///             de datos
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 06/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Guardar_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Tabla_Cajas_Detalles();
        //Session["Asignacion_Turnos"] = null;
        //Grid_Asignacion_Turnos.PageIndex = 0;
        //Grid_Asignacion_Turnos.DataSource = null;
        //Grid_Asignacion_Turnos.DataBind();
        Llenar_Combo_Modulo();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 11/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Session["Asignacion_Turnos"] = null;
            Grid_Asignacion_Turnos.PageIndex = 0;
            Grid_Asignacion_Turnos.DataSource = null;
            Grid_Asignacion_Turnos.DataBind();
            Configuracion_Formulario(true);
            Tbp_Asignacion.Enabled = false;
            Tab_Contenedor_Parametros.ActiveTabIndex = 0;
            Llenar_Tabla_Cajeros(0);
            Llenar_Combo_Modulo();
            Llenar_Combo_Turno();
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
        }
    }

    #endregion

}
