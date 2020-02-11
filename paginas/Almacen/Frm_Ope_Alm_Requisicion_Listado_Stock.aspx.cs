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
using Presidencia.Generar_Req_Listado.Negocio;
using Presidencia.Constantes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Seguimiento_Listado.Negocios;


public partial class paginas_Almacen_Frm_Ope_Alm_Requisicion_Listado_Stock : System.Web.UI.Page
{
    ///*******************************************************************************
    ///PAGE_LOAD
    ///*******************************************************************************
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["SortDirection"] = "ASC";
            Configurar_Formulario("Inicial");
            Llenar_Grid_Listados();

        }
    }
    #endregion

    ///*******************************************************************************
    ///METODOS
    ///******************************************************************************

    #region Metodos

    public void Configurar_Formulario(String Estatus)
    {
        switch (Estatus)
        {
            case "Inicial":
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                Div_Datos_Generales.Visible = false;
                Div_Grid_Listado.Visible = true;
                Div_Grid_Requisiciones_Listados.Visible = false;
                //Boton Modificar
                Btn_Modificar.Visible = false;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                //limpiamos los grid
                Grid_Productos.DataSource = new DataTable();
                Grid_Productos.DataBind();
                Btn_Realizar_Operacion.Enabled = false;
                Cmb_Operacion_Realizar.Enabled = false;
                Grid_Productos.Enabled = false;
                Cmb_Operacion_Realizar.SelectedIndex = 0;
                Txt_Motivo_Borrado.Text = "";
                Txt_Motivo_Borrado.Enabled = false;
                Cmb_Estatus.Enabled =false;
                break;
            case "General":
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                Div_Datos_Generales.Visible = true;
                Div_Grid_Listado.Visible = false;
                Div_Grid_Requisiciones_Listados.Visible = false;
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Listado";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Realizar_Operacion.Enabled = false;
                Cmb_Operacion_Realizar.Enabled = false;
                Grid_Productos.Enabled = false;
                Cmb_Operacion_Realizar.SelectedIndex = 0;
                Txt_Motivo_Borrado.Text = "";
                Txt_Motivo_Borrado.Enabled = false;
                Cmb_Estatus.Enabled = false;

                break;
            case "Modificar":
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                Div_Datos_Generales.Visible = true;
                Div_Grid_Listado.Visible = false;
               
                Btn_Modificar.ToolTip = "Guardar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Modificar.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Realizar_Operacion.Enabled = true;
                Cmb_Operacion_Realizar.Enabled = true;
                Grid_Productos.Enabled = true;
                Cmb_Operacion_Realizar.SelectedIndex = 0;
                Txt_Motivo_Borrado.Text = "";
                Txt_Motivo_Borrado.Enabled = true;
                Cmb_Estatus.Enabled = true;


                break;
        }
    }

    public void Limpiar_Componentes()
    {
        //Limpiamos los grid
        Grid_Requisiciones_Listados.DataSource = new DataTable();
        Grid_Requisiciones_Listados.DataBind();
        Grid_Productos.DataSource = new DataTable();
        Grid_Productos.DataBind();
        //CAJAS DE TEXTO
        Txt_Folio.Text = "";
        Txt_Fecha.Text = "";
        Txt_Tipo.Text = "";
        Txt_Total.Text = "";
        Cmb_Estatus.SelectedIndex = 1;
        Txt_Comentario.Text = "";

        Session["P_Dt_Productos"] = null;
        Session["Listado_ID"] = null;

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Checks_Seleccionados
    ///DESCRIPCIÓN:Metodo que refresa un bool indicndo si se selecciono el check dentro del Grid
    ///PARAMETROS:1.- GridView MyGrid: Grid que contiene el Check
    ///           2.-  String Nombre_Check: Nombre del Check que esta dentro del Grid
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public bool Checks_Seleccionados(GridView MyGrid, String Nombre_Check)
    {

        bool isChecked = false;
        //Obtenemos el numero de Checkbox seleccionados
        for (int i = 0; i < MyGrid.Rows.Count; i++)
        {
            GridViewRow row = MyGrid.Rows[i];
            isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl(Nombre_Check)).Checked;

            if (isChecked)
            {
                //si se encuentra alguno seleccionado se rompe 
                break;
            }
        }//fin del for i

        return isChecked;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Seleccionar_Cheks
    ///DESCRIPCIÓN: Metodo que selecciona los checkbox dentro de un Grid view de acuerdo al parametro estado
    ///PARAMETROS:  1.- GridView MyGrid grid que se va a recorrer
    ///             2.- String Nombre_check nombre del checkbox dentro del grid
    ///             3.- bool Estado estado al que se desea cambiar los check box del grid
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 22/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Seleccionar_Cheks(GridView MyGrid, String Nombre_Check, bool Estado)
    {

        //Seleccionamos todos los checks
        for (int i = 0; i < MyGrid.Rows.Count; i++)
        {
            GridViewRow row = MyGrid.Rows[i];
            ((System.Web.UI.WebControls.CheckBox)row.FindControl(Nombre_Check)).Checked = Estado;
        }//fin del for i


    }//fin de Seleccionar Cheks

    public DataTable Generar_DataTable_Productos()
    {
        DataTable Dt_Productos_Seleccionados = new DataTable();
        //Creamos la instruccion  para copiar la estructura de la variable de session de los productos de listado detalle
        DataTable Dt_Aux =(DataTable)Session["P_Dt_Productos"];
        Dt_Productos_Seleccionados = Dt_Aux.Clone();
        //Variable que ayuda a ver si esta seleccionado el check
        bool isChecked = false;
        //Recorremos el grid para traer los datos seleccionados 
       
        //Obtenemos el numero de Checkbox seleccionados
        for (int i = 0; i < Grid_Productos.Rows.Count; i++)
        {
            GridViewRow row = Grid_Productos.Rows[i];
            isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("Chk_Producto")).Checked;

            if (isChecked)
            {
                //si se encuentra alguno seleccionado se agrega al data table
                DataRow Fila_Nueva = Dt_Productos_Seleccionados.NewRow();
                //Asignamos los valores a la fila
                Fila_Nueva["Producto_ID"] = Dt_Aux.Rows[i]["Producto_ID"].ToString();
                Fila_Nueva["Partida_ID"] = Dt_Aux.Rows[i]["Partida_ID"].ToString();
                Fila_Nueva["Clave"] = Dt_Aux.Rows[i]["Clave"].ToString();
                Fila_Nueva["Producto_Nombre"] = Dt_Aux.Rows[i]["Producto_Nombre"].ToString();
                Fila_Nueva["Descripcion"] = Dt_Aux.Rows[i]["Descripcion"].ToString();
                Fila_Nueva["Cantidad"] = Dt_Aux.Rows[i]["Cantidad"].ToString();
                Fila_Nueva["Precio_Unitario"] = Dt_Aux.Rows[i]["Precio_Unitario"].ToString();
                Fila_Nueva["Costo_Compra"] = Dt_Aux.Rows[i]["Costo_Compra"].ToString();
                Fila_Nueva["Concepto_ID"] = Dt_Aux.Rows[i]["Concepto_ID"].ToString();
                Fila_Nueva["Concepto"] = Dt_Aux.Rows[i]["Concepto"].ToString();
                Fila_Nueva["Concepto_ID"] = Dt_Aux.Rows[i]["Concepto_ID"].ToString();
                Fila_Nueva["Importe"] = Dt_Aux.Rows[i]["Importe"].ToString();
                Fila_Nueva["Monto_IVA"] = Dt_Aux.Rows[i]["Monto_IVA"].ToString();
                Fila_Nueva["Monto_IEPS"] = Dt_Aux.Rows[i]["Monto_IEPS"].ToString();
                Fila_Nueva["Porcentaje_IVA"] = Dt_Aux.Rows[i]["Porcentaje_IVA"].ToString();
                Fila_Nueva["Porcentaje_IEPS"] = Dt_Aux.Rows[i]["Porcentaje_IEPS"].ToString();
                Dt_Productos_Seleccionados.Rows.Add(Fila_Nueva);
                Dt_Productos_Seleccionados.AcceptChanges();
            }
        }//fin del for i




        return Dt_Productos_Seleccionados;
    }

    public bool Verificar_Conceptos()
    {
        String Concepto_ID = "";
        DataTable Dt_Aux =(DataTable)Session["P_Dt_Productos"];
        bool Conceptos_Iguales = true;
        bool isChecked = false;

        //For que sirve para obtener el concepto a comparar
        for (int i = 0; i < Grid_Productos.Rows.Count; i++)
        {
            GridViewRow row = Grid_Productos.Rows[i];
            isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("Chk_Producto")).Checked;
            
            
            //Verificamos el concepto seleccionado 
            if (isChecked)
            {
            //Obtenemos el primer concepto seleccionado para compararlo con los demas
                Concepto_ID = Dt_Aux.Rows[i]["Concepto_ID"].ToString().Trim();
                break;
            }
        }
        //Ahora recorremos el for para comparar los conceptos ya que todos deben ser los mismos para generar la requisicion
        isChecked = false;
        for (int i = 0; i < Grid_Productos.Rows.Count; i++)
        {
            GridViewRow row = Grid_Productos.Rows[i];
            isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("Chk_Producto")).Checked;
            //Verificamos el concepto seleccionado 
            if (isChecked)
            {

                if (Dt_Aux.Rows[i]["Concepto_ID"].ToString().Trim() != Concepto_ID)
                {
                    Conceptos_Iguales = false;
                    break;
                }

            }
        }

        return Conceptos_Iguales;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: caraga el data set fisoco con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/Mayo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte, string Nombre_PDF)
    {

        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Almacen/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Ds_Reporte = Data_Set_Consulta_DB;
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Nombre_PDF);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        Reporte.Export(Export_Options);
        String Ruta = "../../Reporte/" + Nombre_PDF;
        Mostrar_Reporte(Nombre_PDF, "PDF");
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }

    /// *************************************************************************************
    /// NOMBRE:              Mostrar_Reporte
    /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
    /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
    ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
    /// USUARIO CREO:        Juan Alberto Hernández Negrete.
    /// FECHA CREO:          3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Salvador Hernández Ramírez
    /// FECHA MODIFICO:      16-Mayo-2011
    /// CAUSA MODIFICACIÓN:  Se asigno la opción para que en el mismo método se muestre el reporte en excel
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte_Generar, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
                "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }


    #endregion

    ///*******************************************************************************
    ///GRID
    ///*******************************************************************************
    #region Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo que carga los datos del listado seleccionado 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 28/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Listado_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Alm_Requisicion_Listado_Stock_Negocio Listado_Negocio = new Cls_Ope_Alm_Requisicion_Listado_Stock_Negocio();
        //Cargamos los combos 
        Configurar_Formulario("General");
        Listado_Negocio.P_Listado_ID = Grid_Listado.SelectedDataKey["Listado_ID"].ToString();
        //GridViewRow representa una fila individual de un control gridview
        GridViewRow selectedRow = Grid_Listado.Rows[Grid_Listado.SelectedIndex];
        Session["Listado_ID"] = Listado_Negocio.P_Listado_ID;
        DataTable Dt_Lista = Listado_Negocio.Consulta_Listado_Almacen();
        //CARGAMOS LOS DATOS EN LA PAGINA DE LISTADO DE ALMACEN 
        Txt_Folio.Text = Dt_Lista.Rows[0]["Folio"].ToString();
        Txt_Fecha.Text = Convert.ToString(selectedRow.Cells[3].Text); ;
        //Seleccion de Estatus
        Cmb_Estatus.SelectedValue = Dt_Lista.Rows[0][Ope_Com_Listado.Campo_Estatus].ToString().Trim();
        Txt_Tipo.Text = Dt_Lista.Rows[0][Ope_Com_Listado.Campo_Tipo].ToString();
        Txt_Total.Text = Dt_Lista.Rows[0][Ope_Com_Listado.Campo_Total].ToString().Trim();
        
        //Cargamos el grid de productos seleccionados
        DataTable Aux_Table = Listado_Negocio.Consulta_Listado_Detalle();
        Session["P_Dt_Productos"] = Aux_Table;
        //llenas el grid de Productos
        Grid_Productos.Visible = true;
        Grid_Productos.DataSource = (DataTable)Session["P_Dt_Productos"];
        Grid_Productos.DataBind();
        
        Listado_Negocio.P_Listado_ID = Session["Listado_ID"].ToString();
        //llenamos el grid de resquisiciones
        Llenar_Grid_Requisiciones();
    }

    protected void Grid_Listado_Sorting(object sender, GridViewSortEventArgs e)
    {
        
    }

    public void Llenar_Grid_Listados()
    {
        Cls_Ope_Alm_Requisicion_Listado_Stock_Negocio Clase_Negocio = new Cls_Ope_Alm_Requisicion_Listado_Stock_Negocio();
        DataTable Dt_Listados = Clase_Negocio.Consulta_Listado_Almacen();
        if (Dt_Listados.Rows.Count != 0)
        {
            Grid_Listado.DataSource = Dt_Listados;
            Grid_Listado.DataBind();
        }
        else
        {
            Grid_Listado.DataSource = new DataTable();
            Grid_Listado.DataBind();
        }

    }

    protected void Grid_Productos_Sorting(object sender, GridViewSortEventArgs e)
    {

    }

    public void Llenar_Grid_Requisiciones()
    {
        Cls_Ope_Alm_Requisicion_Listado_Stock_Negocio Clase_Negocio = new Cls_Ope_Alm_Requisicion_Listado_Stock_Negocio();
        Clase_Negocio.P_Listado_ID = Session["Listado_ID"].ToString().Trim();
        //Consultamos las requisiciones
        
        DataTable Dt_Requisiciones = Clase_Negocio.Consultar_Requisiciones_Listado();
        if (Dt_Requisiciones.Rows.Count != 0)
        {
            //llenamos el grid
            Grid_Requisiciones_Listados.DataSource = Dt_Requisiciones;
            Grid_Requisiciones_Listados.DataBind();
            Div_Grid_Requisiciones_Listados.Visible = true;
        }
        else
        {
            Grid_Requisiciones_Listados.DataSource = new DataTable();
            Grid_Requisiciones_Listados.DataBind();
            Div_Grid_Requisiciones_Listados.Visible = false;
        }


    }

    #endregion

    ///*******************************************************************************
    ///EVENTOS
    ///*******************************************************************************
    #region

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Evento del Boton Modificar
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        switch (Btn_Modificar.ToolTip)
        {
            case "Modificar":
                Configurar_Formulario("Modificar");
                
                break;
            case "Guardar":
                
                if ((Cmb_Estatus.SelectedItem.Text == "FILTRADA") && (Grid_Productos.Rows.Count != 0))
                {

                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No puede cambiar a estatus filtrada, hasta que asigne todos los productos a una requisicion";
                }

                if (Div_Contenedor_Msj_Error.Visible == false)
                {
                    Cls_Ope_Alm_Requisicion_Listado_Stock_Negocio Clase_Negocio = new Cls_Ope_Alm_Requisicion_Listado_Stock_Negocio();
                    Clase_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
                    Clase_Negocio.P_Listado_ID = Session["Listado_ID"].ToString().Trim();
                    bool Operacion_Realizada = Clase_Negocio.Modificar_Listado();
                    Limpiar_Componentes();
                    if (Operacion_Realizada == true)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Requisicion Listado Almacen", "alert('Se modifico el Listado Almacen ')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Requisicion Listado Almacen", "alert('No se pudo modificar el Listado Almacen ')", true);
                    }
                    Configurar_Formulario("Inicial");
                    Llenar_Grid_Listados();
                }
                
                break;
        }
    }

     ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del Boton Salir
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        switch (Btn_Salir.ToolTip)
        {
            case "Listado":
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = false;
                //Limpiamos el objeto de clase de negocios
                Configurar_Formulario("Inicial");
                Llenar_Grid_Listados();
                break;
            case "Inicio":
                //Limpiamos el objeto de clase de negocios
               
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                break;
            case "Cancelar":
                //Limpiamos el objeto de clase de negocios
                Configurar_Formulario("Inicial");
                Llenar_Grid_Listados();
                break;
        }//fin del switch
    }


    #endregion 

    protected void Btn_Realizar_Operacion_Click(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        bool Seleccion_Check = Checks_Seleccionados(Grid_Productos, "Chk_Producto");
        if (Seleccion_Check == false)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text += "Es necesario seleccionar algun producto </br>";
        }

        if(Div_Contenedor_Msj_Error.Visible == false)
        {
            Cls_Ope_Alm_Requisicion_Listado_Stock_Negocio Clase_Negocio = new Cls_Ope_Alm_Requisicion_Listado_Stock_Negocio();
            //Obtenemos el datatable de los productos seleccionados que se daran de alta en la requisicion nueva
            Clase_Negocio.P_Listado_ID = Session["Listado_ID"].ToString().Trim();
            Clase_Negocio.P_Dt_Productos = Generar_DataTable_Productos();
            try
            {
                String No_Requisicion = Clase_Negocio.Convertir_Requisicion_Transitoria();
                //Cargamos de nuevo el listado de productos
                DataTable Dt_Productos = Clase_Negocio.Consulta_Listado_Detalle();
                Session["P_Dt_Productos"] = null;
                Grid_Productos.DataSource = Dt_Productos;
                Grid_Productos.DataBind();
                Session["P_Dt_Productos"] = Dt_Productos;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Requisicion Listado Almacen", "alert('Se creo la requisicion Transitoria de Listado Almacen "+No_Requisicion+"')", true);
                //llenamos el grid con las requisiciones k se han creado 
                Llenar_Grid_Requisiciones();                
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Requisicion Listado Almacen", "alert('No se pudo generar la requisicion');", true);
            }
        }
    }

    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Txt_Folio.Text.Trim() == String.Empty)
        {
            Lbl_Mensaje_Error.Text = "Es necesario seleccionar un Listado Almacen ";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        if (Div_Contenedor_Msj_Error.Visible == false)
        {
            //Realizamos la Consulta del Listado para realizar el reporte. 
            Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio Clase_Negocio = new Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio();
            Clase_Negocio.P_Listado_ID = Session["Listado_ID"].ToString().Trim();
            DataTable DT_LISTADO = Clase_Negocio.Consulta_Listado_Reporte();
            DataTable DT_DETALLE_LISTADO = Clase_Negocio.Consulta_Listado_Detalles_Reporte();
            DataSet Ds_Imprimir_Listado = new DataSet();
            Ds_Imprimir_Listado.Tables.Add(DT_LISTADO.Copy());
            Ds_Imprimir_Listado.Tables[0].TableName = "DT_LISTADO";
            Ds_Imprimir_Listado.AcceptChanges();
            Ds_Imprimir_Listado.Tables.Add(DT_DETALLE_LISTADO.Copy());
            Ds_Imprimir_Listado.Tables[1].TableName = "DT_DETALLE_LISTADO";
            Ds_Imprimir_Listado.AcceptChanges();
            Ds_Alm_Listado_Almacen Obj_Imprimir_Listado = new Ds_Alm_Listado_Almacen();
            Generar_Reporte(Ds_Imprimir_Listado, Obj_Imprimir_Listado, "Rpt_Alm_Listado_Almacen.rpt", "Rpt_Alm_Listado_Almacen.pdf");
        }//Fin del if
    }
}
