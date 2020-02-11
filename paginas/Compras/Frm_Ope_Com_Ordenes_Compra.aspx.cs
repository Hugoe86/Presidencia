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
using Presidencia.Orden_Compra.Negocio;
using Presidencia.Consolidar_Requisicion.Negocio;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Compras.Impresion_Requisiciones.Negocio;

public partial class paginas_Compras_Frm_Ope_Com_Ordenes_Compra : System.Web.UI.Page
{
    #region VARIABLES

    private static String P_Dt_Requisiciones = "REQUISICIONES_COMPRA";
    private static String P_Dt_Detalles_Requisicion = "DETALLES_REQUISICION_COMPRA";
    private const String COM_DIRECTA = "DIRECTA";
    #endregion

    #region PAGE LOAD / INIT

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) 
        {
            ViewState["SortDirection"] = "DESC";
            Session[P_Dt_Detalles_Requisicion] = null;
            Session[P_Dt_Requisiciones] = null;

            Cmb_Dias_Adicionales.Items.Clear();
            Cmb_Dias_Adicionales.Items.Add("0");
            Cmb_Dias_Adicionales.Items.Add("1");
            Cmb_Dias_Adicionales.Items.Add("2");
            Cmb_Dias_Adicionales.Items.Add("3");
            Cmb_Dias_Adicionales.Items.Add("4");
            Cmb_Dias_Adicionales.Items.Add("5");
            Cmb_Dias_Adicionales.Items.Add("6");
            Cmb_Dias_Adicionales.Items.Add("7");
            Cmb_Dias_Adicionales.Items.Add("8");
            Cmb_Dias_Adicionales.Items.Add("9");
            Cmb_Dias_Adicionales.Items.Add("10");
            Llenar_Combo_Cotizadores();
            Llenar_Grid_Compra_Directa();
            //Verificar si su rol es jefe de dependencia, admin de modulo o admin de sistema
            DataTable Dt_Grupo_Rol = Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Rol_ID.ToString());
            if (Dt_Grupo_Rol != null)
            {
                String Grupo_Rol = Dt_Grupo_Rol.Rows[0][Apl_Cat_Roles.Campo_Grupo_Roles_ID].ToString();
                if (Grupo_Rol == "00001" || Grupo_Rol == "00002")
                {
                    Cmb_Cotizadores.Enabled = true;
                }
                else
                {
                    Cmb_Cotizadores.Enabled = false;                    
                }
            }
        }
        Mostrar_Informacion("",false);

    }

    #endregion

    #region METODOS
    ///*******************************************************************************
    //NOMBRE DE LA FUNCIÓN: Dia_Habil
    //DESCRIPCIÓN: 
    //CREO: Gustavo Angeles Cruz
    //FECHA_CREO: 23/Febrero/2011
    //MODIFICO:
    //FECHA_MODIFICO:
    //CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    private bool Dia_Habil(DateTime Fecha)
    {
        bool Respuesta = true;
        if (Fecha.DayOfWeek.ToString() == "Saturday" || Fecha.DayOfWeek.ToString() == "Sunday") 
        {
            Respuesta = false;
        }
        return Respuesta;
    }

    ///*******************************************************************************
    //NOMBRE DE LA FUNCIÓN: Sumar_Dias_Habiles
    //DESCRIPCIÓN: 
    //CREO: Gustavo Angeles Cruz
    //FECHA_CREO: 23/Febrero/2011
    //MODIFICO:
    //FECHA_MODIFICO:
    //CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    private DateTime Sumar_Dias_Habiles(String Str_Fecha, int Dias)
    {
        DateTime Fecha = Convert.ToDateTime(Str_Fecha);
        int Cuenta_Dias = 0;
        while(Cuenta_Dias < Dias)
        {
            Fecha = Fecha.AddDays(1.0);
            if ( Dia_Habil(Fecha) )
            {
                Cuenta_Dias++;
            }
        }
        return Fecha;
    }
    private DateTime Sumar_Dias_Habiles(DateTime Str_Fecha, int Dias)
    {
        DateTime Fecha = Str_Fecha;
        int Cuenta_Dias = 0;
        while (Cuenta_Dias < Dias)
        {
            Fecha = Fecha.AddDays(1.0);
            if (Dia_Habil(Fecha))
            {
                Cuenta_Dias++;
            }
        }
        return Fecha;
    }
    ///*******************************************************************************
    //NOMBRE DE LA FUNCIÓN: Consultar_Dias_Plazo
    //DESCRIPCIÓN: 
    //CREO: Gustavo Angeles Cruz
    //FECHA_CREO: 23/Febrero/2011
    //MODIFICO:
    //FECHA_MODIFICO:
    //CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    //private int Consultar_Dias_Plazo()
    //{
    //    Cls_Ope_Com_Orden_Compra_Negocio Negocio_Compra = new Cls_Ope_Com_Orden_Compra_Negocio();
    //    P_Dias_Plazo = Negocio_Compra.Consultar_Dias_Plazo();
    //    //Lbl_Mensaje_Fecha.Text = "Se agregarán " + P_Dias_Plazo + " días hábiles de plazo a la fecha que ingrese";
    //}

    ///*******************************************************************************
    //NOMBRE DE LA FUNCIÓN: Consultar_Dias_Entrega_Proveedor
    //DESCRIPCIÓN: 
    //CREO: Gustavo Angeles Cruz
    //FECHA_CREO: 12 Oct 2011
    //MODIFICO:
    //FECHA_MODIFICO:
    //CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    private int Consultar_Dias_Entrega_Proveedor()
    {
        Cls_Ope_Com_Orden_Compra_Negocio Negocio_Compra = new Cls_Ope_Com_Orden_Compra_Negocio();
        return Negocio_Compra.Consultar_Dias_Entrega_Proveedor();
    }
    ///*******************************************************************************
    //NOMBRE DE LA FUNCIÓN: Mostrar_Información
    //DESCRIPCIÓN: Llena las areas de texto con el registro seleccionado del grid
    //CREO: Gustavo Angeles Cruz
    //FECHA_CREO: 24/Agosto/2010 
    //MODIFICO:
    //FECHA_MODIFICO:
    //CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    private void Mostrar_Informacion(String txt, Boolean mostrar)
    {
        Div_Contenedor_Msj_Error.Visible = true;
        Lbl_Mensaje_Error.Visible = mostrar;
        IBtn_Imagen_Error.Visible = mostrar;
        Lbl_Mensaje_Error.Text = txt;
    }
    ///*******************************************************************************
    //NOMBRE DE LA FUNCIÓN: Llenar_Combo_Cotizadores
    //DESCRIPCIÓN:Llenar_Combo_Cotizadores
    //CREO: Gustavo Angeles Cruz
    //FECHA_CREO: 14/Oct/2011 
    //MODIFICO:
    //FECHA_MODIFICO:
    //CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    private void Llenar_Combo_Cotizadores() 
    {
        Cls_Ope_Com_Orden_Compra_Negocio Negocio_Compra = new Cls_Ope_Com_Orden_Compra_Negocio();
        DataTable Dt_Cotizadores = Negocio_Compra.Consultar_Cotizadores();
        if (Dt_Cotizadores != null && Dt_Cotizadores.Rows.Count > 0)
        {
            Cls_Util.Llenar_Combo_Con_DataTable_Generico
                (Cmb_Cotizadores, Dt_Cotizadores, Cat_Com_Cotizadores.Campo_Nombre_Completo, Cat_Com_Cotizadores.Campo_Empleado_ID);
            Cmb_Cotizadores.SelectedValue = Cls_Sessiones.Empleado_ID;
        }
        else
        {
            Cmb_Cotizadores.Items.Clear();
            Cmb_Cotizadores.Items.Add("COTIZADORES");
            Cmb_Cotizadores.SelectedIndex = 0;
        }
    }
    private void Llenar_Grid_Compra_Directa()
    {
        Cls_Ope_Com_Orden_Compra_Negocio Negocio_Compra = new Cls_Ope_Com_Orden_Compra_Negocio();
        Negocio_Compra.P_Cotizador_ID = Cmb_Cotizadores.SelectedValue.Trim();
        DataTable Dt_Requisas_Compra_Directa = Negocio_Compra.Consultar_Requisiciones_Directas();
        if (Dt_Requisas_Compra_Directa != null && Dt_Requisas_Compra_Directa.Rows.Count > 0)
        {
            Session[P_Dt_Requisiciones] = Dt_Requisas_Compra_Directa;
            Grid_Requisiciones_Cotizadas.DataSource = Dt_Requisas_Compra_Directa;
            Grid_Requisiciones_Cotizadas.DataBind();
        }
        else
        {
            Session[P_Dt_Requisiciones] = null;
            Grid_Requisiciones_Cotizadas.DataSource = null;
            Grid_Requisiciones_Cotizadas.DataBind();
        }
    }


    #endregion

    #region EVENTOS GRID

    //Compras Directas
    protected void Grid_Requisiciones_Cotizadas_SelectedIndexChanged(object sender, EventArgs e)
    {
        String No_Requisicion = Grid_Requisiciones_Cotizadas.SelectedDataKey["NO_REQUISICION"].ToString().Trim();

        Cls_Ope_Com_Orden_Compra_Negocio Negocio_Compra = new Cls_Ope_Com_Orden_Compra_Negocio();
        Negocio_Compra.P_Lista_Requisiciones = No_Requisicion;       
        Negocio_Compra.P_No_Requisicion = long.Parse(No_Requisicion);
        Negocio_Compra.P_No_Cotizacion = 0;
        Negocio_Compra.P_No_ComiteCompra = 0;
        Negocio_Compra.P_No_Licitacion = 0;
        Negocio_Compra.P_No_Factura_Interno = 0;
        Negocio_Compra.P_Estatus = "GENERADA";
        Negocio_Compra.P_Tipo_Compra = "COMPRA DIRECTA";
        Negocio_Compra.P_Tipo_Articulo = Grid_Requisiciones_Cotizadas.SelectedDataKey["TIPO_ARTICULO"].ToString().Trim();
        
        //Verifica si la requisición necesita dividirse
        int No_Proveedores_De_Requisicion = Negocio_Compra.Consultar_Numero_Proveedores_De_Requisicion();
        if (No_Proveedores_De_Requisicion == 1)
        {
            Div_Articulos.Visible = true;
            Div_Requisiciones_Para_Orden_Compra.Visible = false;
            Btn_Salir.ToolTip = "Regresar";
            //Mustra los detalles de la compra
            DataTable Dt_Detalle_de_Compra = Consolidar(Negocio_Compra.P_Lista_Requisiciones, Negocio_Compra.P_Tipo_Articulo);
            Grid_Detalles_Compra.DataSource = Dt_Detalle_de_Compra;
            Grid_Detalles_Compra.DataBind();
            Session[P_Dt_Detalles_Requisicion] = Dt_Detalle_de_Compra;
            //Datos generales
            Txt_Proceso_Compra.Text = "COMPRA DIRECTA";
            Txt_Identificador_Compra.Text = Grid_Requisiciones_Cotizadas.SelectedDataKey["FOLIO"].ToString().Trim();
            Txt_Total.Text = Grid_Requisiciones_Cotizadas.SelectedDataKey["TOTAL_COTIZADO"].ToString().Trim();
            Txt_Unidad_Responsable.Text = Grid_Requisiciones_Cotizadas.SelectedDataKey["UNIDAD_RESPONSABLE"].ToString().Trim();
            Txt_Proveedor.Text = Dt_Detalle_de_Compra.Rows[0]["NOMBRE_PROVEEDOR"].ToString();
            Btn_Guardar.Visible = true;

            int Dias_Entrega_Proveedor = Negocio_Compra.Consultar_Dias_Entrega_Proveedor();
            DateTime Fecha_Actual = Sumar_Dias_Habiles(DateTime.Now, Dias_Entrega_Proveedor);
            Txt_Fecha_Entrega.Text = Fecha_Actual.ToString("dd/MMM/yyyy").ToUpper();
            Txt_Fecha_Plazo.Text = Fecha_Actual.ToString("dd/MMM/yyyy").ToUpper();

            //Para pasar la clase de negocio a un método
            //Negocio_Tmp = Negocio_Compra;
            Div_Articulos.Visible = true;
            //Div_Ordenes_Generadas.Visible = false;
            Configuracion_Acceso("Frm_Ope_Com_Ordenes_Compra.aspx");
        }
        else
        {
            ScriptManager.RegisterStartupScript(
                   this, this.GetType(), "Requisiciones", "alert('" + "Existe mas de un proveedor, debe dividir la requisición antes de generar la orden de compra" + "');", true);
        }
    }

    private DataTable Consolidar(String Requisas_Seleccionadas, String Tipo_Articulo)
    {
        Cls_Ope_Com_Consolidar_Requisicion_Negocio Negocio_Consolidar = 
            new Cls_Ope_Com_Consolidar_Requisicion_Negocio();
        Negocio_Consolidar.P_Requisas_Seleccionadas = Requisas_Seleccionadas;
        Negocio_Consolidar.P_Estatus = "CONFIRMADA";
        DataTable Dt_Articulos = null;
        if (Tipo_Articulo == "PRODUCTO")
        {
            Dt_Articulos = Negocio_Consolidar.Consolidar_Requisiciones_Productos();
        }
        else if (Tipo_Articulo == "SERVICIO")
        {
            Dt_Articulos = Negocio_Consolidar.Consolidar_Requisiciones_Servicios();
        }
        return Dt_Articulos;
    }

    #endregion

    #region EVENTOS

    protected void Btn_Guardar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Txt_Fecha_Plazo.Text.Trim().Length > 0)
            {
                DateTime FechaActual = DateTime.Now;
                DateTime FechaEntrega = Convert.ToDateTime(Txt_Fecha_Plazo.Text.Trim());
                if (FechaEntrega >= FechaActual )
                {
                    if (Txt_Condicion1.Text.Length > 3600)
                    {
                        ScriptManager.RegisterStartupScript(
                           this, this.GetType(), "Orden de Compra", "alert('" + "Las condiciones exceden el número de caracteres permitidos" + "');", true);
                        return;
                    }
                    try
                    {
                        //Datos de las Órdenes de Compra Generadas
                        Cls_Ope_Com_Orden_Compra_Negocio Negocio_Compra = new Cls_Ope_Com_Orden_Compra_Negocio();
                        Negocio_Compra.P_Lista_Requisiciones =
                            Txt_Identificador_Compra.Text = Txt_Identificador_Compra.Text.Replace("RQ-", "");
                        Negocio_Compra.P_No_Requisicion = long.Parse(Negocio_Compra.P_Lista_Requisiciones);
                        Negocio_Compra.P_No_Cotizacion = 0;
                        Negocio_Compra.P_No_ComiteCompra = 0;
                        Negocio_Compra.P_No_Licitacion = 0;
                        Negocio_Compra.P_No_Factura_Interno = 0;
                        String Listado = "";
                        DataRow[] _DataRow = ((DataTable)Session[P_Dt_Requisiciones]).
                            Select("NO_REQUISICION ='" + Negocio_Compra.P_No_Requisicion + "'");
                        Listado = _DataRow[0][Ope_Com_Requisiciones.Campo_Listado_Almacen].ToString().Trim();
                        if (Listado == "SI")
                        {
                            Negocio_Compra.P_Estatus = "AUTORIZADA";
                            Negocio_Compra.P_No_Reserva = "115110001";
                        }
                        else
                        {
                            Negocio_Compra.P_Estatus = "GENERADA";                            
                        }
                        Negocio_Compra.P_Tipo_Compra = "COMPRA DIRECTA";
                        Negocio_Compra.P_Tipo_Articulo = _DataRow[0][Ope_Com_Requisiciones.Campo_Tipo_Articulo].ToString().Trim();
                        Negocio_Compra.P_Fecha_Entrega = Txt_Fecha_Plazo.Text.Trim();
                        Negocio_Compra.P_Condicion1 = Txt_Condicion1.Text;
                        Negocio_Compra.P_Condicion2 = Txt_Condicion2.Text;
                        Negocio_Compra.P_Condicion3 = Txt_Condicion3.Text;
                        Negocio_Compra.P_Condicion4 = Txt_Condicion4.Text;
                        Negocio_Compra.P_Condicion5 = Txt_Condicion5.Text;
                        Negocio_Compra.P_Condicion6 = Txt_Condicion6.Text;

                        DataTable Dt_Ordenes_Compra = Negocio_Compra.Guardar_Orden_Compra();
                        
                        if (Dt_Ordenes_Compra != null && Dt_Ordenes_Compra.Rows.Count > 0)
                        {
                            String Orden_Compra_Folio = Dt_Ordenes_Compra.Rows[0]["FOLIO"].ToString();
                            String Orden_Compra_Numero = Orden_Compra_Folio.Replace("OC-","");
                            Cls_Util.Registrar_Historial_Orden_Compra
                                (Orden_Compra_Numero, Negocio_Compra.P_Estatus, 
                                Cls_Sessiones.Nombre_Empleado, 
                                Dt_Ordenes_Compra.Rows[0]["NOMBRE_PROVEEDOR"].ToString());
                            ScriptManager.RegisterStartupScript(
                                this, this.GetType(), 
                                "Requisiciones", "alert('Se generó la Orden de Compra: " + 
                                Orden_Compra_Folio + "');", true);
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(Negocio_Compra.P_Mensaje))
                            {
                                if (Negocio_Compra.P_Mensaje == "2")
                                {
                                    ScriptManager.RegisterStartupScript(
                                       this, this.GetType(), "Requisiciones", "alert('" + "Existe mas de un proveedor, debe dividir la requisición antes de generar la orden de compra" + "');", true);
                                    Negocio_Compra.P_Mensaje = "";
                                }
                            }
                            else 
                            {
                                ScriptManager.RegisterStartupScript(
                                    this, this.GetType(), "Requisiciones", "alert('" + "No se generó la orden compra, verifique con su administrador del sistema" + "');", true);
                            }
                        }
                        Div_Articulos.Visible = false;
                        Div_Requisiciones_Para_Orden_Compra.Visible = true;
                        Btn_Salir.ToolTip = "Inicio";
                        Btn_Guardar.Visible = false;
                        Llenar_Grid_Compra_Directa();
                    }
                    catch(Exception Ex)
                    {
                        Mostrar_Informacion("No se generó la orden compra, verifique con su " + 
                            "administrador del sistema [" + Ex.ToString() + "]", true);
                    }
                }
                else
                {
                    Mostrar_Informacion("La fecha de entrega debe ser mayor al día de hoy", true);
                }
            }
            else
            {
                Mostrar_Informacion("Debe ingresar una fecha de entrega", true);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Formato de fecha incorrecto [" + Ex.ToString() + "]" , true);            
            Txt_Fecha_Plazo.Text = "";
        }
    }

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == "Regresar")
        {
            Llenar_Grid_Compra_Directa();
            Btn_Salir.ToolTip = "Inicio";
            Div_Articulos.Visible = false;
            Div_Requisiciones_Para_Orden_Compra.Visible = true;
            Btn_Guardar.Visible = false;
        }
        else 
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
    }

    #endregion

    protected void Txt_Fecha_Entrega_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
             Txt_Fecha_Plazo.Text =
                 String.Format("{0:dd/MMM/yyyy}", Sumar_Dias_Habiles(Txt_Fecha_Entrega.Text,  
                                        int.Parse(Cmb_Dias_Adicionales.SelectedValue.Trim()) ));
        }
        catch(Exception Ex)
        {
            Mostrar_Informacion("El formato de fecha es incorrecto", true);
            Ex.ToString();
            Txt_Fecha_Plazo.Text = "";
        }
    }

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
            Botones.Add(Btn_Guardar);

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
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS_AlternateText(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
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
    //protected void Grid_Requisiciones_Cotizadas_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    Grid_Sorting(Grid_Requisiciones_Cotizadas, P_Dt_Requisas_Compra_Directa, e);
    //}
    /// ******************************************************************************************
    /// NOMBRE: Grid_Requisiciones_Sorting
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// CREÓ: Gustavo Angeles Cruz
    /// FECHA CREÓ: 11/Junio/2011
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// ******************************************************************************************
    //protected void Grid_Cotizaciones_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    Grid_Sorting(Grid_Cotizaciones, P_Dt_Cotizacion, e);
    //}



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

    protected void Cmb_Dias_Adicionales_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Txt_Fecha_Plazo.Text =
                String.Format("{0:dd/MMM/yyyy}", Sumar_Dias_Habiles(Txt_Fecha_Entrega.Text,
                                       int.Parse(Cmb_Dias_Adicionales.SelectedValue.Trim())));
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Formato de fecha es incorrecto", true);
            Ex.ToString();
            Txt_Fecha_Plazo.Text = "";
        }
    }
    protected void Cmb_Cotizadores_SelectedIndexChanged(object sender, EventArgs e)
    {
        Llenar_Grid_Compra_Directa();
    }
    protected void Btn_Imprimir_Req_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_Identificador_Compra.Text.Trim().Length > 0)
        {
            DataSet Ds_Reporte = null;
            DataTable Dt_Requisicion = null;
            Cls_Ope_Com_Impresion_Requisiciones_Negocio Req_Negocio = new Cls_Ope_Com_Impresion_Requisiciones_Negocio();
            DataTable Dt_Cabecera = new DataTable();
            DataTable Dt_Detalles = new DataTable();
            try
            {
                String Requisicion_ID = Txt_Identificador_Compra.Text.Replace("RQ-", "");
                Req_Negocio.P_Requisicion_ID = Requisicion_ID.Trim();
                Dt_Cabecera = Req_Negocio.Consultar_Requisiciones();
                Dt_Detalles = Req_Negocio.Consultar_Requisiciones_Detalles();
                Ds_Reporte = new DataSet();
                Dt_Cabecera.TableName = "REQUISICION";
                Dt_Detalles.TableName = "DETALLES";
                Ds_Reporte.Tables.Add(Dt_Cabecera.Copy());
                Ds_Reporte.Tables.Add(Dt_Detalles.Copy());
                //Se llama al método que ejecuta la operación de generar el reporte.
                Generar_Reporte(ref Ds_Reporte, "Rpt_Ope_Com_Requisiciones.rpt", "Requisicion.pdf");
            }
            catch (Exception Ex)
            {
                Mostrar_Informacion(Ex.Message.ToString(), true);
            }
        }
        else
        {
            Mostrar_Informacion("Seleccione una Requisición", true);
        }
    }
    #region REPORTES

    protected void Generar_Reporte(ref DataSet Ds_Datos, String Nombre_Plantilla_Reporte, String Nombre_Reporte_Generar)
    {
        ReportDocument Reporte = new ReportDocument();//Variable de tipo reporte.
        String Ruta = String.Empty;//Variable que almacenara la ruta del archivo del crystal report. 

        try
        {
            Ruta = @Server.MapPath("../Rpt/Compras/" + Nombre_Plantilla_Reporte);
            Reporte.Load(Ruta);

            if (Ds_Datos is DataSet)
            {
                if (Ds_Datos.Tables.Count > 0)
                {
                    Reporte.SetDataSource(Ds_Datos);
                    Exportar_Reporte_PDF(Reporte, Nombre_Reporte_Generar);
                    Mostrar_Reporte(Nombre_Reporte_Generar);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    protected void Exportar_Reporte_PDF(ReportDocument Reporte, String Nombre_Reporte)
    {
        ExportOptions Opciones_Exportacion = new ExportOptions();
        DiskFileDestinationOptions Direccion_Guardar_Disco = new DiskFileDestinationOptions();
        PdfRtfWordFormatOptions Opciones_Formato_PDF = new PdfRtfWordFormatOptions();

        try
        {
            if (Reporte is ReportDocument)
            {
                Direccion_Guardar_Disco.DiskFileName = @Server.MapPath("../../Reporte/" + Nombre_Reporte);
                Opciones_Exportacion.ExportDestinationOptions = Direccion_Guardar_Disco;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Opciones_Exportacion);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al exportar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    protected void Mostrar_Reporte(String Nombre_Reporte)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window",
                "window.open('" + Pagina + "', 'Requisición','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    #endregion
}
