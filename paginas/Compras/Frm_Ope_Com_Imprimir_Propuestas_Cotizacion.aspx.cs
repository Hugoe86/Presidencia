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
using Presidencia.Imprimir_Propuestas.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

public partial class paginas_Compras_Frm_Ope_Com_Imprimir_Propuestas_Cotizacion : System.Web.UI.Page
{
    

        ///*******************************************************************************
        ///PAGE_LOAD
        ///*******************************************************************************
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Clase_Negocio = new Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio();

                ViewState["SortDirection"] = "ASC";
                Configurar_Formulario("Inicio");
                Llenar_Grid_Requisiciones(Clase_Negocio);
                Llenar_Combo_Proveedores(Cmb_Proveedor);
            }
        }

        #endregion

        ///*******************************************************************************
        ///METODOS
        ///*******************************************************************************
        #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configurar_Formulario
    ///DESCRIPCIÓN: Metodo que configura el formulario con respecto al estado de habilitado o visible
    ///´de los componentes de la pagina
    ///PARAMETROS: 1.- String Estatus: Estatus que puede tomar el formulario con respecto a sus componentes, ya sea "Inicio" o "Nuevo"
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    
    public void Configurar_Formulario(String Estatus)
    {

        switch (Estatus)
        {
            case "Inicio":

                Div_Detalle_Requisicion.Visible = false;
                Div_Grid_Requisiciones.Visible = true;
                Div_Busqueda.Visible = true;
                //Boton Modificar
                Btn_Nuevo.Visible = false;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Nuevo.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                //
                Grid_Requisiciones.Visible = true;
                Grid_Requisiciones.Enabled = true;
                Div_Detalle_Requisicion.Visible = false;
                Grid_Productos.Enabled = false;
                //Cargamos las fechas al dia de hoy
                Txt_Busqueda_Fecha_Entrega_Ini.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Busqueda_Fecha_Entrega_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Busqueda_Fecha_Elaboracion_Ini.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Busqueda_Fecha_Elaboracion_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Busqueda_Vigencia_Propuesta_Ini.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Busqueda_Vigencia_Propuesta_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");

                Txt_Busqueda_Fecha_Entrega_Ini.Enabled = false;
                Txt_Busqueda_Fecha_Entrega_Fin.Enabled = false;
                Txt_Busqueda_Fecha_Elaboracion_Ini.Enabled = false;
                Txt_Busqueda_Fecha_Elaboracion_Fin.Enabled = false;
                Txt_Busqueda_Vigencia_Propuesta_Ini.Enabled = false;
                Txt_Busqueda_Vigencia_Propuesta_Fin.Enabled = false;

                //Deseleccionamos los check box
                Chk_Fecha_Elaboracion.Checked = false;
                Chk_Fecha_Entrega.Checked = false;
                Chk_Vigencia_Propuesta.Checked = false;
                //DEshabilitamos botones
                Btn_Busqueda_Fecha_Elaboracion_Ini.Enabled = false;
                Btn_Busqueda_Fecha_Elaboracion_Fin.Enabled = false;
                Btn_Busqueda_Fecha_Entrega_Fin.Enabled = false;
                Btn_Busqueda_Fecha_Entrega_Ini.Enabled = false;
                Btn_Busqueda_Vigencia_Propuesta_Ini.Enabled = false;
                Btn_Busqueda_Vigencia_Propuesta_Fin.Enabled = false;


                break;
            case "General":
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Cotizar";
                Div_Busqueda.Visible = false;
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Nuevo.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Listado";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                //
                Div_Grid_Requisiciones.Visible = false;
                Div_Detalle_Requisicion.Visible = true;
                Grid_Productos.Enabled = false;
                //Deshabilitar controles 
                Txt_Reg_Padron_Prov.Enabled = false;
                Txt_Garantia.Enabled = false;
                Txt_Tiempo_Entrega.Enabled = false;
                Txt_Fecha_Elaboracio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Estatus_Propuesta.Enabled = false;
                
                

                break;
            case "Nuevo":

                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Guardar";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Nuevo.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                //
                Div_Grid_Requisiciones.Visible = false;
                Div_Detalle_Requisicion.Visible = true;
                Grid_Productos.Enabled = true;
                //habilitar controles 
                Txt_Reg_Padron_Prov.Enabled = true;
                Txt_Garantia.Enabled = true;
                Txt_Tiempo_Entrega.Enabled = true;
                Txt_Fecha_Elaboracio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Estatus_Propuesta.Enabled = true;

                break;
        }//fin del switch

    }



    public void Limpiar_Componentes()
    {
        Session["Concepto_ID"] = null;
        Session["Dt_Requisiciones"] = null;
        Session["No_Requisicion"] = null;
        Session["TIPO_ARTICULO"] = null;
        Session["Concepto_ID"] = null;
        Session["Proveedor_ID"] = null;
        Session["No_Requisicion"] = null;
        Session["Dt_Detalle_Requisicion"] = null;
        Session["Dt_Propuesta"] = null;
        Session["Dt_Productos"] = null;
             
        Txt_Dependencia.Text = "";
        Txt_Concepto.Text = "";
        Txt_Folio.Text = "";
        Txt_Concepto.Text = "";
        Txt_Fecha_Generacion.Text = "";
        Txt_Tipo.Text = "";
        Txt_Tipo_Articulo.Text = "";
        Chk_Verificacion.Checked = false;
        Txt_Justificacion.Text = "";
        Txt_Especificacion.Text = "";
        Grid_Productos.DataSource = new DataTable();
        Grid_Productos.DataBind();
        Txt_SubTotal_Cotizado_Requisicion.Text = "";
        Txt_Estatus_Propuesta.Text = "";
        Txt_Estatus.Text ="";
        Txt_Total_Cotizado_Requisicion.Text = "";
        Txt_IEPS_Cotizado.Text = "";
        Txt_IVA_Cotizado.Text = "";
        Txt_Reg_Padron_Prov.Text = "";
        Txt_Vigencia.Text = "";
        Txt_Fecha_Elaboracio.Text = "";
        Txt_Garantia.Text = "";
        Txt_Tiempo_Entrega.Text = "";

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///PARAMETROS: 1.-TextBox Fecha_Inicial 
    ///            2.-TextBox Fecha_Final
    ///            3.-Label Mensaje_Error
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Verificar_Fecha(TextBox Fecha_Inicial, TextBox Fecha_Final,Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Clase_Negocio)
    {

        //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date1 = new DateTime();
        DateTime Date2 = new DateTime();
        bool Fecha_Valida = true;

        if ((Fecha_Inicial.Text.Length == 11) && (Fecha_Final.Text.Length == 11))
        {
            //Convertimos el Texto de los TextBox fecha a dateTime
            Date1 = DateTime.Parse(Fecha_Inicial.Text);
            Date2 = DateTime.Parse(Fecha_Final.Text);
            //Validamos que las fechas sean iguales o la final sea mayor que la inicias, si no se manda un mensaje de error 
            if ((Date1 < Date2) | (Date1 == Date2))
            {
                //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                if (Chk_Fecha_Elaboracion.Checked == true)
                {
                    Clase_Negocio.P_Busqueda_Fecha_Generacion_Ini = Formato_Fecha(Fecha_Inicial.Text);
                    Clase_Negocio.P_Busqueda_Fecha_Generacion_Fin = Formato_Fecha(Fecha_Final.Text);
                }

                if (Chk_Fecha_Entrega.Checked == true)
                {
                    Clase_Negocio.P_Busqueda_Fecha_Entrega_Ini = Formato_Fecha(Fecha_Inicial.Text);
                    Clase_Negocio.P_Busqueda_Fecha_Entrega_Fin = Formato_Fecha(Fecha_Final.Text);
                }
                if (Chk_Vigencia_Propuesta.Checked == true)
                {
                    Clase_Negocio.P_Busqueda_Vigencia_Propuesta_Ini = Formato_Fecha(Fecha_Inicial.Text);
                    Clase_Negocio.P_Busqueda_Vigencia_Propuesta_Fin = Formato_Fecha(Fecha_Final.Text);
                }

            }
            else
            {
                Lbl_Mensaje_Error.Text += "+ Fecha no valida <br />";
            }
        }
        else
        {
            Lbl_Mensaje_Error.Text += "+ Fecha no valida <br />";
        }

        return Clase_Negocio;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN: Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:  1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                     en caso de que cumpla la condicion del if
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 2/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Formato_Fecha(String Fecha)
    {

        String Fecha_Valida = Fecha;
        //Se le aplica un split a la fecha 
        String[] aux = Fecha.Split('/');
        //Se modifica el es a solo mayusculas para que oracle acepte el formato. 
        switch (aux[1])
        {
            case "dic":
                aux[1] = "DEC";
                break;
        }
        //Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
        Fecha_Valida = aux[0] + "-" + aux[1] + "-" + aux[2];
        return Fecha_Valida;
    }// fin de Formato_Fecha

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
        String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Proveedores
    ///DESCRIPCIÓN: Metodo que Consulta los proveedores dados de alta en la tabla CAT_COM_PROVEEDORES
    ///PARAMETROS: 1.- DropDownList Cmb_Combo: combo dentro de la pagina a llenar 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Proveedores(DropDownList Cmb_Combo)
    {
        Cmb_Combo.Items.Clear();
        Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Negocios = new Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio();
        DataTable Data_Table = Negocios.Consultar_Proveedores();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Combo, Data_Table);
        Cmb_Combo.SelectedIndex = 0;
    }
        #endregion

        ///*******************************************************************************
        ///GRID
        ///*******************************************************************************
        #region Grid

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:Llenar_Grid_Requisiciones
        ///DESCRIPCIÓN: Metodo que permite llenar el Grid_Requisiciones
        ///PARAMETROS:
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 01/JULIO/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Llenar_Grid_Requisiciones(Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Clase_Negocio)
        {
            DataTable Dt_Requisiciones = Clase_Negocio.Consultar_Requisiciones();
            if (Dt_Requisiciones.Rows.Count != 0)
            {
                Session["Dt_Requisiciones"] = Dt_Requisiciones;
                Grid_Requisiciones.DataSource = Dt_Requisiciones;
                Grid_Requisiciones.DataBind();

            }
            else
            {
                Grid_Requisiciones.EmptyDataText = "No se han encontrado registros.";
                Grid_Requisiciones.DataSource = new DataTable();
                Grid_Requisiciones.DataBind();

            }


        }

        protected void Grid_Requisiciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Clase_Negocio = new Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio();
            GridViewRow selectedRow = Grid_Requisiciones.Rows[Grid_Requisiciones.SelectedIndex];
            int num_fila = Grid_Requisiciones.SelectedIndex;
            DataTable Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones"];

            Clase_Negocio.P_Proveedor_ID = Dt_Requisiciones.Rows[num_fila]["Proveedor_ID"].ToString().Trim();
            Session["Proveedor_ID"] = Clase_Negocio.P_Proveedor_ID.Trim();

            Clase_Negocio.P_No_Requisicion = Grid_Requisiciones.SelectedDataKey["No_Requisicion"].ToString();
            Session["No_Requisicion"] = Clase_Negocio.P_No_Requisicion;
            //Consultamos los detalles del producto seleccionado
            DataTable Dt_Detalle_Requisicion = Clase_Negocio.Consultar_Detalle_Requisicion();
            Session["Dt_Detalle_Requisicion"] = Dt_Detalle_Requisicion;

            //Mostramos el div de detalle y el grid de Requisiciones
            Div_Grid_Requisiciones.Visible = false;
            Div_Detalle_Requisicion.Visible = true;
            Btn_Salir.ToolTip = "Listado";
            //llenamos la informacion del detalle de la requisicion seleccionada
            Txt_Proveedor.Text = Dt_Requisiciones.Rows[num_fila]["Nombre_Proveedor"].ToString().Trim();
            Txt_Dependencia.Text = Dt_Detalle_Requisicion.Rows[0]["DEPENDENCIA"].ToString().Trim();
            Txt_Folio.Text = Dt_Detalle_Requisicion.Rows[0]["FOLIO"].ToString().Trim();
            Txt_Concepto.Text = Dt_Detalle_Requisicion.Rows[0]["CONCEPTO"].ToString().Trim();
            Txt_Fecha_Generacion.Text = Dt_Detalle_Requisicion.Rows[0]["FECHA_GENERACION"].ToString().Trim();
            Txt_Tipo.Text = Dt_Detalle_Requisicion.Rows[0]["TIPO"].ToString().Trim();
            Txt_Tipo_Articulo.Text = Dt_Detalle_Requisicion.Rows[0]["TIPO_ARTICULO"].ToString().Trim();
            Txt_Estatus.Text = Dt_Detalle_Requisicion.Rows[0]["ESTATUS"].ToString().Trim();
            Txt_Justificacion.Text = Dt_Detalle_Requisicion.Rows[0]["JUSTIFICACION_COMPRA"].ToString().Trim();
            Txt_Especificacion.Text = Dt_Detalle_Requisicion.Rows[0]["ESPECIFICACION_PROD_SERV"].ToString().Trim();
            Session["TIPO_ARTICULO"] = Txt_Tipo_Articulo.Text.Trim();
            Session["Concepto_ID"] = Dt_Detalle_Requisicion.Rows[0]["CONCEPTO_ID"].ToString().Trim();
            //LLenamos los text de la propuesta de Cotizacion
            DataTable Dt_Propuesta = Clase_Negocio.Consultar_Propuesta_Cotizacion();
            Session["Dt_Propuesta"] = Dt_Propuesta;
            if (Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Registro_Padron_Prov].ToString().Trim() != String.Empty)
                Txt_Reg_Padron_Prov.Text = int.Parse(Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Registro_Padron_Prov].ToString().Trim()).ToString();
            Txt_Garantia.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Garantia].ToString().Trim();
            Txt_Fecha_Elaboracio.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Fecha_Elaboracion].ToString().Trim();
            Txt_Tiempo_Entrega.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Tiempo_Entrega].ToString().Trim();
            Txt_Vigencia.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Vigencia_Propuesta].ToString().Trim();
            Txt_Estatus_Propuesta.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Estatus].ToString().Trim();
            //Asignamos los text
            if(Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Total_Cotizado_Requisicion].ToString().Trim() != String.Empty)
                Txt_Total_Cotizado_Requisicion.Text = String.Format("{0:C}",double.Parse(Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Total_Cotizado_Requisicion].ToString().Trim()));
            if (Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Subtotal_Cotizado_Requisicion].ToString().Trim() != String.Empty)
                Txt_SubTotal_Cotizado_Requisicion.Text = String.Format("{0:C}",double.Parse(Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Subtotal_Cotizado_Requisicion].ToString().Trim()));
            if(Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_IEPS_Cotizado_Req].ToString().Trim() != String.Empty)
                Txt_IEPS_Cotizado.Text = String.Format("{0:C}",double.Parse( Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_IEPS_Cotizado_Req].ToString().Trim()));
            if(Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_IVA_Cotizado_Req].ToString().Trim() != String.Empty)
                Txt_IVA_Cotizado.Text = String.Format("{0:C}",double.Parse(Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_IVA_Cotizado_Req].ToString().Trim()));

            Div_Grid_Requisiciones.Visible = false;
            Div_Detalle_Requisicion.Visible = true;
            //VALIDAMOS EL CAMPO DE VERIFICAR CARACTERISTICAS, GARANTIA Y POLIZAS
            if (Dt_Detalle_Requisicion.Rows[0]["VERIFICACION_ENTREGA"].ToString().Trim() == "NO" || Dt_Detalle_Requisicion.Rows[0]["VERIFICACION_ENTREGA"].ToString().Trim() == String.Empty)
            {
                Chk_Verificacion.Checked = false;
            }
            if (Dt_Detalle_Requisicion.Rows[0]["VERIFICACION_ENTREGA"].ToString().Trim() == "SI")
            {
                Chk_Verificacion.Checked = true;
            }
            //Consultamos los productos de esta requisicion
            Clase_Negocio.P_Tipo_Articulo = Session["TIPO_ARTICULO"].ToString().Trim();
            DataTable Dt_Productos = Clase_Negocio.Consultar_Productos_Servicios();
            //llenamos el grid de productos
            if (Dt_Productos.Rows.Count != 0)
            {
                Session["Dt_Productos"] = Dt_Productos;
                Grid_Productos.DataSource = Dt_Productos;
                Grid_Productos.DataBind();
                Grid_Productos.Visible = true;
                Grid_Productos.Enabled = false;
            }
            else
            {
                Grid_Productos.EmptyDataText = "No se encontraron Productos";
                Grid_Productos.DataSource = new DataTable();
                Grid_Productos.DataBind();
            }
            Configurar_Formulario("General");
            Btn_Salir.ToolTip = "Listado";

        }

        #endregion

        ///*******************************************************************************
        ///EVENTOS
        ///*******************************************************************************
        #region Eventos

        protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            if (Txt_Reg_Padron_Prov.Text.Trim() != String.Empty)
            {
                //Si se selecciono un proveedor se imprime la propuesta
                //reALIZAMOS LA CONSULTA PARA TRAERNOS LOS DATOS QUE SE GUARDARON
                Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Clase_Negocio = new Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio();
                Clase_Negocio.P_Proveedor_ID = Session["Proveedor_ID"].ToString().Trim();
                DataTable Datos_Proveedor = Clase_Negocio.Consultar_Datos_Proveedor();
                Ds_Ope_Com_Imprimir_Propuestas_Cotizacion Obj_Imp_Prop = new Ds_Ope_Com_Imprimir_Propuestas_Cotizacion();
                DataSet Ds_Imprimir_Propuesta = new DataSet();
                //Traemos los datatable que ocuparemos 
                DataTable Dt_Detalle_Requisicion = (DataTable)Session["Dt_Detalle_Requisicion"];
                DataTable Dt_Propuesta = (DataTable)Session["Dt_Propuesta"];
                DataTable Dt_Productos = (DataTable)Session["Dt_Productos"];
                Ds_Imprimir_Propuesta.Tables.Add(Dt_Detalle_Requisicion.Copy());
                Ds_Imprimir_Propuesta.Tables[0].TableName = "Dt_Detalle_Requisicion";
                Ds_Imprimir_Propuesta.AcceptChanges();
                Ds_Imprimir_Propuesta.Tables.Add(Dt_Propuesta.Copy());
                Ds_Imprimir_Propuesta.Tables[1].TableName = "Dt_Propuesta";
                Ds_Imprimir_Propuesta.AcceptChanges();
                Ds_Imprimir_Propuesta.Tables.Add(Dt_Productos.Copy());
                Ds_Imprimir_Propuesta.Tables[2].TableName = "Dt_Productos";
                Ds_Imprimir_Propuesta.AcceptChanges();
                Ds_Imprimir_Propuesta.Tables.Add(Datos_Proveedor.Copy());
                Ds_Imprimir_Propuesta.Tables[3].TableName = "Datos_Proveedor";
                Ds_Imprimir_Propuesta.AcceptChanges();

                Generar_Reporte(Ds_Imprimir_Propuesta, Obj_Imp_Prop, "Rpt_Ope_Com_Imprimir_Propuestas_Cotizacion.rpt", "Rpt_Ope_Com_Imprimir_Propuestas_Cotizacion.pdf");

            }
            else
            {
                Lbl_Mensaje_Error.Text = "Es necesario seleccionar una propuesta de cotizacion";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        

        #endregion

        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Clase_Negocios = new Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio();
            if (Txt_Requisicion_Busqueda.Text.Trim() != String.Empty)
            {
                Clase_Negocios.P_Requisicion_Busqueda = Txt_Requisicion_Busqueda.Text.Trim();
            }

            if (Cmb_Proveedor.SelectedIndex != 0)
            {
                Clase_Negocios.P_Busqueda_Proveedor = Cmb_Proveedor.SelectedValue.Trim();
            }

            if (Chk_Fecha_Elaboracion.Checked == true)
            {
                Clase_Negocios = Verificar_Fecha(Txt_Busqueda_Fecha_Elaboracion_Ini, Txt_Busqueda_Fecha_Elaboracion_Fin, Clase_Negocios);
            }

            if (Chk_Vigencia_Propuesta.Checked == true)
            {
                Clase_Negocios = Verificar_Fecha(Txt_Busqueda_Vigencia_Propuesta_Ini, Txt_Busqueda_Vigencia_Propuesta_Fin, Clase_Negocios);
            }

            if (Chk_Fecha_Entrega.Checked == true)
            {
                Clase_Negocios = Verificar_Fecha(Txt_Busqueda_Fecha_Entrega_Ini, Txt_Busqueda_Fecha_Entrega_Fin, Clase_Negocios);
            }

            if (Div_Contenedor_Msj_Error.Visible == false)
            {
                DataTable Dt_Table = Clase_Negocios.Consultar_Requisiciones();
                Llenar_Grid_Requisiciones(Clase_Negocios);


            }
        }
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
        {
            switch (Btn_Salir.ToolTip)
            {
                case "Inicio":
                    Limpiar_Componentes();
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    break;
                case "Listado":

                    Limpiar_Componentes();
                    Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Clase_Negocio = new Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio();
                    Llenar_Grid_Requisiciones(Clase_Negocio);
                    Configurar_Formulario("Inicio");
                break;
            }

        }
}

