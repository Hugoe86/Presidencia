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
using Presidencia.Almacen_Impresion_Recibos.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Reportes;
using Presidencia.Almacen_Elaborar_Contrarecibo.Negocio;

public partial class paginas_Almacen_Frm_Ope_Alm_Imprimir_Contra_Recibo : System.Web.UI.Page
{

    #region Variables
    Cls_Ope_Com_Alm_Impresion_Recibos_Negocio Contra_Recibo = new Cls_Ope_Com_Alm_Impresion_Recibos_Negocio();
    #endregion

    # region  Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {            
            Chk_Fecha_B.Checked = true;
            DateTime _DateTime = DateTime.Now;
            int dias = _DateTime.Day;
            dias = dias * -1;
            dias++;
            _DateTime = _DateTime.AddDays(dias);
            Txt_Fecha_Inicio.Text = _DateTime.ToString("dd/MMM/yyyy").ToUpper();
            Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();

            Div_Contra_Recibos.Visible = true;
            Div_Detalles_CR.Visible = false;

            Consultar_Contra_Recibos();
            if (Cmb_Proveedores.Items.Count != 0)
                Cmb_Proveedores.SelectedIndex = 0;
            Chk_Proveedor.Checked = false;                       
            Img_Btn_Fecha_Inicio.Enabled = true;
            Img_Btn_Fecha_Fin.Enabled = true;
            Txt_Contra_Recibo.Text = "";
            Txt_Busqueda.Text = "";
            Txt_Req_Buscar.Text = "";
            Llenar_Combo_Proveedores();
        }
    }
    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Click
    ///DESCRIPCIÓN:          Evento utiliado para mostrar en pantalla el recibo transitorio
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           25/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        String Formato ="PDF";
        if (Session["No_Contra_Recibo"] != null)
        {
            String No_Contra_Recibo = Session["No_Contra_Recibo"].ToString().Trim();
            Consultar_Datos_Contra_Recibo(No_Contra_Recibo, Formato);
        }

    }


    protected void Btn_Imprimir_Excel_Click(object sender, ImageClickEventArgs e)
    {
        String Formato = "Excel";
        if (Session["No_Contra_Recibo"] != null)
        {
            String No_Contra_Recibo = Session["No_Contra_Recibo"].ToString().Trim();
            Consultar_Datos_Contra_Recibo(No_Contra_Recibo, Formato);
        }
    }

    // Método Utilizado consultar los datos del contra recibo
    private void Consultar_Datos_Contra_Recibo(String No_Contra_Recibo, String Formato)
    {
        DataTable Dt_Datos_Generales = new DataTable();
        DataTable Dt_Facturas_Proveedor = new DataTable();
        DataTable Dt_Documentos_Soporte = new DataTable();

        Contra_Recibo.P_No_Contra_Recibo = No_Contra_Recibo;
        Dt_Datos_Generales = Contra_Recibo.Consultar_Datos_Generales_ContraRecibo();

        if (Session["Dt_Facturas"] != null)
            Dt_Facturas_Proveedor = (DataTable)Session["Dt_Facturas"];

        if (Session["Dt_Documentos_Soporte"] != null)
            Dt_Documentos_Soporte = (DataTable)Session["Dt_Documentos_Soporte"];

        Ds_Alm_Com_Contrarecibo Ds_Contra_Recibo = new Ds_Alm_Com_Contrarecibo(); // Se crea el  DataSet
        Generar_Reporte(Dt_Datos_Generales, Dt_Facturas_Proveedor, Dt_Documentos_Soporte, Ds_Contra_Recibo, Formato);
    }
    
    public void Llenar_Tablas_Reporte(String Formato)
    {

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN:          Evento que configura algunos de los componentes a su estatus inicial
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           25/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText == "Salir")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            //Estatus_Busqueda_Abanzada();
            Mostrar_Busqueda(true);
            Div_Detalles_CR.Visible = false;
            Div_Contra_Recibos.Visible = true;
            Estatus_Inicial();
            Configuracion_Botones(true);
        }
    }

    # region Busqueda


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:    Btn_Buscar_Click
    ///DESCRIPCIÓN:             Evento utilizado para consultar los contra recibos en base
    ///                         Numero de Contra Recibo asignado por el usuario
    ///PROPIEDADES:     
    ///CREO:                    Salvador Hernández Ramírez
    ///FECHA_CREO:              04/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Consultar_Contra_Recibos();
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:    Chk_Fecha_B_CheckedChanged
    ///DESCRIPCIÓN:             Maneja el evento click del CheckList "Chk_Fecha_B"
    ///PROPIEDADES:     
    ///CREO:                    Salvador Hernández Ramírez
    ///FECHA_CREO:              04/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Chk_Fecha_B_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Fecha_B.Checked == true)
        {
            Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");

            Img_Btn_Fecha_Inicio.Enabled = true;
            Img_Btn_Fecha_Fin.Enabled = true;
        }
        else
        {
            Txt_Fecha_Inicio.Text = "";
            Txt_Fecha_Fin.Text = "";
            Img_Btn_Fecha_Inicio.Enabled = false;
            Img_Btn_Fecha_Fin.Enabled = false;
        }
    }
 
    # endregion


    #region Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Contra_Recibos_SelectedIndexChanged
    ///DESCRIPCIÓN:          Evento utilizado para cargar los datos del Grid en variables e instancia al método
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           01/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Contra_Recibos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try {

            GridViewRow SelectedRow = Grid_Contra_Recibos.Rows[Grid_Contra_Recibos.SelectedIndex];//GridViewRow representa una fila individual de un control GridView
            String No_Contra_Recibo = Convert.ToString(SelectedRow.Cells[1].Text.Trim()); // Se obtiene el numero de Contra Recibo
            Session["No_Contra_Recibo"] = No_Contra_Recibo;
            String Factura_Proveedor = HttpUtility.HtmlDecode(Convert.ToString(SelectedRow.Cells[2].Text.Trim())); // Se obtiene la factura del proveedor
            String Fecha_Factura = Convert.ToString(SelectedRow.Cells[3].Text.Trim()); // Se obtiene la fecha de la factura
            String Proveedor = HttpUtility.HtmlDecode(Convert.ToString(SelectedRow.Cells[5].Text.Trim())); // Se obtiene el nombre del proveedor
            String Fecha_Resepcion = Convert.ToString(SelectedRow.Cells[2].Text.Trim()); // Se obtiene la fecha de resepción
            String Requisicion = HttpUtility.HtmlDecode(Convert.ToString(SelectedRow.Cells[4].Text.Trim())); // Se obtiene la fecha de resepción
            String Tipo_Articulo = HttpUtility.HtmlDecode(Convert.ToString(SelectedRow.Cells[9].Text.Trim())); // Se obtiene la 
            String Listado_Almacen = HttpUtility.HtmlDecode(Convert.ToString(SelectedRow.Cells[10].Text.Trim())); // Se obtiene si el listado de almacén es SI o NO

            Div_Contra_Recibos.Visible = false;
            Div_Detalles_CR.Visible = true;

            Mostrar_Detalles_ContraRecibo(No_Contra_Recibo, Factura_Proveedor, Fecha_Factura, Proveedor, Fecha_Resepcion, Requisicion, Tipo_Articulo ,Listado_Almacen);
        }catch(Exception Ex){
            throw new Exception(Ex.Message, Ex);
        }
    }


      # endregion

    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Botones
    ///DESCRIPCIÓN:          Método utilizado para configuran los botones
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           04/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Configuracion_Botones(bool Estatus)
    {
        if (Estatus == false)
        {
            Btn_Salir.AlternateText = "Atras";
            Btn_Salir.ToolTip = "Atras";
            Mostrar_Busqueda(false);
            Btn_Imprimir.Visible = true;
            Btn_Imprimir_Excel.Visible = true;
        }
        else if (Estatus == true)
        {
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ToolTip = "Salir";
            Mostrar_Busqueda(true);
            Btn_Imprimir.Visible = false;
            Btn_Imprimir_Excel.Visible = false;
        }

        if (Btn_Imprimir.Visible)
        {
            Configuracion_Acceso("Frm_Ope_Alm_Imprimir_Contra_Recibo.aspx");
            Configurar_Boton_Imprimir();
        }

    }


    public void Configurar_Boton_Imprimir(){

        if (Btn_Imprimir.Visible)
            Btn_Imprimir_Excel.Visible = true;
        else
            Btn_Imprimir_Excel.Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Busqueda
    ///DESCRIPCIÓN:          Método utilizado para mostrar y ocultar los cintroles
    ///                      utilizados para realziar la búsqueda simple y abanzada
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           12/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Mostrar_Busqueda(Boolean Estatus)
    {
        Div_Busqueda_Av.Visible = Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estatus_Inicial
    ///DESCRIPCIÓN:          Método utilizado para configurar inicialmente algunos de los componentes de la página
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           24/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Estatus_Inicial()
    {
        Consultar_Contra_Recibos();
        Estatus_Busqueda_Abanzada();
        Llenar_Combo_Proveedores();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Contra_Recibos
    ///DESCRIPCIÓN:          Método utilizado para consultar los contra recibos y mostrarlos en el grid
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           01/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
     public void Consultar_Contra_Recibos()
     {
        DataTable Dt_ContraRecibos = new DataTable();  // Se crea la tabla        
        try
        {
            if (Txt_Busqueda.Text.Trim() != "")
                Contra_Recibo.P_No_Orden_Compra = Txt_Busqueda.Text.Trim();
            else
                Contra_Recibo.P_No_Orden_Compra = null;

            if (Txt_Req_Buscar.Text.Trim() != "")
                Contra_Recibo.P_No_Requisicion = Txt_Req_Buscar.Text.Trim();
            else
                Contra_Recibo.P_No_Requisicion = null;

            if (Txt_Contra_Recibo.Text.Trim() != "")
                Contra_Recibo.P_No_Recibo = Txt_Contra_Recibo.Text.Trim();
            else
                Contra_Recibo.P_No_Recibo = null;

            if (Chk_Proveedor.Checked == true)
            {
                if (Cmb_Proveedores.SelectedIndex != 0)
                    Contra_Recibo.P_Proveedor = Cmb_Proveedores.SelectedValue.Trim();
                else
                    Contra_Recibo.P_Proveedor = null;
            }

            if (Chk_Fecha_B.Checked) // Si esta activado el Check
            {
                DateTime Date1 = new DateTime();  //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
                DateTime Date2 = new DateTime();

                if ((Txt_Fecha_Inicio.Text.Length != 0))
                {
                    if ((Txt_Fecha_Inicio.Text.Length == 11) && (Txt_Fecha_Fin.Text.Length == 11))
                    {
                        //Convertimos el Texto de los TextBox fecha a dateTime
                        Date1 = DateTime.Parse(Txt_Fecha_Inicio.Text);
                        Date2 = DateTime.Parse(Txt_Fecha_Fin.Text);

                        //Validamos que las fechas sean iguales o la final sea mayor que la inicial, de lo contrario se manda un mensaje de error 
                        if ((Date1 < Date2) | (Date1 == Date2))
                        {
                            if (Txt_Fecha_Fin.Text.Length != 0)
                            {
                                //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                                Contra_Recibo.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim());
                                Contra_Recibo.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Fin.Text.Trim());
                                Div_Contenedor_Msj_Error.Visible = false;
                            }
                            else
                            {
                                String Fecha = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()); //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                                Contra_Recibo.P_Fecha_Inicial = Fecha;
                                Contra_Recibo.P_Fecha_Final = Fecha;
                                Div_Contenedor_Msj_Error.Visible = false;
                            }
                        }
                        else
                        {
                            Lbl_Informacion.Text = " Fecha no valida ";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    }
                    else
                    {
                        Lbl_Informacion.Text = " Fecha no valida ";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
            }

            Dt_ContraRecibos = Contra_Recibo.Consulta_Contra_Recibos();

            if (Dt_ContraRecibos.Rows.Count > 0)
            {
                Grid_Contra_Recibos.Columns[6].Visible = true;
                Grid_Contra_Recibos.Columns[7].Visible = true;
                Grid_Contra_Recibos.Columns[9].Visible = true; // Tipo Articulo
                Grid_Contra_Recibos.Columns[10].Visible = true; // Listado_Almacén
                Grid_Contra_Recibos.DataSource = Dt_ContraRecibos;
                Session["Dt_ContraRecibosRCR"] = Dt_ContraRecibos;
                Grid_Contra_Recibos.DataBind();
                Grid_Contra_Recibos.Columns[6].Visible = false;
                Grid_Contra_Recibos.Columns[7].Visible = false;
                Grid_Contra_Recibos.Columns[9].Visible = false; // Tipo Articulo
                Grid_Contra_Recibos.Columns[10].Visible = false; // Listado_Almacén

                Div_Contenedor_Msj_Error.Visible = false;
            }
            else
            {
                Lbl_Informacion.Text = "No se encontraron contra recibos";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
         catch (Exception ex)
         {
             throw new Exception(ex.Message, ex);
         }
     }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Detalles_ContraRecibo
    ///DESCRIPCIÓN:          En este método se consultan los productos que pertenecen a la orden de compra
    ///                      seleccionada por el usuario
    ///PROPIEDADES:          1.- No_Contra_Recibo: Es el numero de contra recibo. 
    ///                      2.- Factura_Proveedor: Numero de la factura del proveeedor.
    ///                      3.- Fecha_Factura: La fecha de la factura del proveedor.
    ///                      4.- Proveedor: Nombre del proveedor.
    ///                      5.- Fecha_Resepcion: Es la fecha de recepción de la factura.
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           23/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************                            
    public void Mostrar_Detalles_ContraRecibo(String No_Contra_Recibo, String Factura_Proveedor, String Fecha_Factura, String Proveedor, String Fecha_Resepcion, String Requisicion, String Tipo_Articulo, String Listado_Almacen)
    {
         DataTable Dt_Productos_ContraRecibo = new DataTable();
         DataTable Dt_Documentos_Soporte = new DataTable();
         DataTable Dt_Detalles_Contra_Recibo= new DataTable();
         DataTable Dt_Montos_OC = new DataTable();
         DataTable Dt_Facturas = new DataTable();
         DataTable Dt_Unidad_Responsable = new DataTable();

        try
        {
              Contra_Recibo.P_No_Contra_Recibo = No_Contra_Recibo.Trim(); // Se asigna el No. de Contra Recibo

             if (Listado_Almacen.Trim() =="SI")
                 Contra_Recibo.P_Tipo_Articulo = "LISTADO_ALMACEN"; // Con esto se indica que se van a consultar los productos de un listado de almacén
            else
                Contra_Recibo.P_Tipo_Articulo =Tipo_Articulo.Trim(); // Se asigna el No. de Contra Recibo
            
             Dt_Productos_ContraRecibo = Contra_Recibo.Consulta_Productos_Contra_Recibo();
             Dt_Documentos_Soporte = Contra_Recibo.Consulta_Documentos_Soporte();
             Dt_Detalles_Contra_Recibo = Contra_Recibo.Consulta_Detalles_Contra_Recibo();
             Dt_Montos_OC = Contra_Recibo.Consulta_Montos_Orden_Compra();
             Dt_Facturas = Contra_Recibo.Consultar_Facturas_ContraRecibo();
             Contra_Recibo.P_Tipo_Tabla = "UNIDAD_RESPONSABLE";
             Dt_Unidad_Responsable= Contra_Recibo.Consulta_Tablas();

            //if (Dt_Productos_ContraRecibo.Rows.Count > 0) // Si la consulta contiene productos
            // {
                 // Se les agrega información a los TexBoxt
                 Txt_No_Contra_Recibo.Text = No_Contra_Recibo.Trim();
                 Txt_Proveedor.Text = Proveedor.Trim();
                 Txt_Fecha_Resepcion.Text = Fecha_Resepcion.Trim();
                 Txt_No_Requisicion.Text = Requisicion.Trim();

                 if (Dt_Unidad_Responsable.Rows.Count > 0) // Si la consulta contiene productos
                     Txt_Unidad_Responsable.Text = HttpUtility.HtmlDecode(Dt_Unidad_Responsable.Rows[0]["UNIDAD_RESPONSABLE"].ToString().Trim());
                 else
                     Txt_Unidad_Responsable.Text = "";

                 if (Dt_Detalles_Contra_Recibo.Rows.Count > 0) // Si la consulta contiene productos
                 {
                     if (Dt_Detalles_Contra_Recibo.Rows[0]["USUARIO_CREO"].ToString() != null)
                     Txt_Usuario_Elaboro.Text = HttpUtility.HtmlDecode(Dt_Detalles_Contra_Recibo.Rows[0]["USUARIO_CREO"].ToString().Trim());

                     if (Dt_Detalles_Contra_Recibo.Rows[0]["COMENTARIOS"].ToString() != null)
                         Txt_Observaciones.Text = HttpUtility.HtmlDecode(Dt_Detalles_Contra_Recibo.Rows[0]["COMENTARIOS"].ToString().Trim());
                 }
                 else
                 {
                     Txt_Usuario_Elaboro.Text = "";
                     Txt_Observaciones.Text = "";
                 }

                 // Se llena el Grid para mostrar los productos de la orden de compra
                 Grid_Productos_Orden_Compra.DataSource = Dt_Productos_ContraRecibo;
                 Grid_Productos_Orden_Compra.DataBind();

                 if (Dt_Montos_OC.Rows.Count > 0) // Si la consulta contiene Montos
                 {
                     Lbl_SubTotal.Text = HttpUtility.HtmlDecode(Dt_Montos_OC.Rows[0]["SUBTOTAL"].ToString().Trim());
                     Lbl_IVA.Text = HttpUtility.HtmlDecode(Dt_Montos_OC.Rows[0]["TOTAL_IVA"].ToString().Trim());
                     Lbl_Total.Text = HttpUtility.HtmlDecode(Dt_Montos_OC.Rows[0]["TOTAL"].ToString().Trim());
                 }
                 else
                 {
                     Lbl_SubTotal.Text = "";
                     Lbl_IVA.Text = "";
                     Lbl_Total.Text = "";
                 }
   
                 // Se llena el Grid que mostrará los documentos soporte
                 if (Dt_Documentos_Soporte.Rows.Count > 0)
                 {
                     Session["Dt_Documentos_Soporte"] = Dt_Documentos_Soporte;
                     Grid_Doc_Soporte.DataSource = Dt_Documentos_Soporte;
                     Grid_Doc_Soporte.DataBind();
                     //Div_Documentos_Soporte.Visible = true;
                 }
                 else
                 {
                     //Div_Documentos_Soporte.Visible = false;
                     Session["Dt_Documentos_Soporte"] = null;
                 }

                   // Se llena el Grid que mostrará las facturas de los proveedores
                 if (Dt_Facturas.Rows.Count > 0)
                 {
                     Session["Dt_Facturas"] = Dt_Facturas;
                     Grid_Facturas.DataSource = Dt_Facturas;
                     Grid_Facturas.DataBind();
                    // Div_Facturas.Visible = true;
                 }
                 else
                 {
                     //Div_Facturas.Visible = false;
                     Session["Dt_Facturas"] = null;
                 }
                Configuracion_Botones(false);
                 Mostrar_Busqueda(false);
                 Div_Contenedor_Msj_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN:          Carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:           1.- Data_Table_Consulta.- Contiene la informacion de la consulta a la base de datos
    ///                      2.- Dt_Documentos_Soporte - Contiene los documentos  soporte
    ///                      2.- DataSet_Reporte, Objeto que contiene la instancia del Data set fisico del reporte a generar
    ///                      3.- Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///                      4.- Nombre_Archivo, contiene el nombre del documento que se va a generar
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           16/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Generar_Reporte(DataTable Dt_Datos_Generales, DataTable Dt_Facturas_Proveedor, DataTable Dt_Documentos_Soporte, DataSet DataSet_Reporte, String Formato)
    {
        DataRow Renglon;
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";

        // Llenar la tabla "Cabecera" del Dataset
        Renglon = Dt_Datos_Generales.Rows[0];
        DataSet_Reporte.Tables[0].ImportRow(Renglon);

        if (Dt_Facturas_Proveedor.Rows.Count > 0)
        {
            // Llenar los detalles del DataSet
            for (int Cont_Elementos = 0; Cont_Elementos < Dt_Facturas_Proveedor.Rows.Count; Cont_Elementos++)
            {
                // Instanciar renglon e importarlo
                Renglon = Dt_Facturas_Proveedor.Rows[Cont_Elementos];
                DataSet_Reporte.Tables[1].ImportRow(Renglon);
            }
        }

        if (Dt_Documentos_Soporte.Rows.Count > 0)
        {
            // Llenar los documentos del DataSet
            for (int Elementos = 0; Elementos < Dt_Documentos_Soporte.Rows.Count; Elementos++)
            {
                // Instanciar renglon e importarlo
                Renglon = Dt_Documentos_Soporte.Rows[Elementos];
                DataSet_Reporte.Tables[2].ImportRow(Renglon);
            }
        }

        // Ruta donde se encuentra el reporte Crystal
        Ruta_Reporte_Crystal = "../Rpt/Almacen/Rpt_Alm_Com_Contrarecibo.rpt";

        // Se crea el nombre del reporte
        String Nombre_Reporte = "Rpt_Contra_Recibo_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

        // Se da el nombre del reporte que se va generar
        if (Formato == "PDF")
            Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
        else if (Formato == "Excel")
            Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar

        Cls_Reportes Reportes = new Cls_Reportes();
        Reportes.Generar_Reporte(ref DataSet_Reporte, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
        Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
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


    #region Métodos Busqueda


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estatus_Busqueda_Abanzada
    ///DESCRIPCIÓN:          Método utilizado para configurar el estatus inicial de la busqueda abanzada
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           04/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Estatus_Busqueda_Abanzada()
    {
        
        if (Cmb_Proveedores.Items.Count != 0)
            Cmb_Proveedores.SelectedIndex = 0;

        Chk_Proveedor.Checked = false;
        Cmb_Proveedores.Enabled = false;

        Chk_Fecha_B.Checked = false;
        Img_Btn_Fecha_Inicio.Enabled = false;
        Img_Btn_Fecha_Fin.Enabled = false;
        Txt_Fecha_Fin.Text = "";
        Txt_Fecha_Inicio.Text = "";
        Txt_Contra_Recibo.Text = "";
        Txt_Busqueda.Text = "";
        Txt_Req_Buscar.Text = "";
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Proveedores
    ///DESCRIPCIÓN:          Método utilizado para llenar el combo con los proveedores 
    ///                      que se encuentren en la Base de Datos
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           04/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    public void Llenar_Combo_Proveedores(){

        try
        {
            Contra_Recibo.P_Tipo_Tabla = "PROVEEDORES_CONTRA_RECIBO";
            Cmb_Proveedores.DataSource =Contra_Recibo.Consulta_Tablas();
            Cmb_Proveedores.DataTextField = Cat_Com_Proveedores.Campo_Nombre;
            Cmb_Proveedores.DataValueField = Cat_Com_Proveedores.Campo_Proveedor_ID;
            Cmb_Proveedores.DataBind();
            Cmb_Proveedores.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
            Cmb_Proveedores.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:    Validar_Estatus_Busqueda
    ///DESCRIPCIÓN:             Metodo que valida que seleccione un estatus dentro del ModalPopup
    ///PARAMETROS:   
    ///CREO:                    Salvador Hernández Ramírez
    ///FECHA_CREO:              04/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Validar_Estatus_Busqueda()
    {
        if (Chk_Proveedor.Checked == true)
        {
            if (Cmb_Proveedores.SelectedIndex != 0)
            {
                Contra_Recibo.P_Proveedor = Cmb_Proveedores.SelectedValue.Trim();
            }
            else
            {
                
                Lbl_Informacion.Text += "+ Seleccionar el Proveedor <br />";
            }
        }
    }

   

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:    Formato_Fecha
    ///DESCRIPCIÓN:             Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:              1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                         en caso de que cumpla la condicion del if
    ///CREO:                    Salvador Hernández Ramírez
    ///FECHA_CREO:              04/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Formato_Fecha(String Fecha)
    {
        String Fecha_Valida = Fecha;
        String[] aux = Fecha.Split('/'); //Se le aplica un split a la fecha 
        switch (aux[1]) //Se modifica a mayusculas para que oracle acepte el formato.
        {
            case "dic":
                aux[1] = "DEC";
                break;
        }
        // Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
        Fecha_Valida = aux[0] + "-" + aux[1] + "-" + aux[2];
        return Fecha_Valida;
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
            Botones.Add(Btn_Imprimir);
            Botones.Add(Btn_Buscar);

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
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS_AlternateText(Botones, Dr_Menus[0]); // Habilitamos la configuracón de los botones.
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
    protected void Configuracion_Acceso_LinkButton(String URL_Pagina)
    {
        List<LinkButton> Botones = new List<LinkButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            ////Agregamos los botones a la lista de botones de la página.
            //Botones.Add(Btn_Busqueda_Avanzada);

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
            throw new Exception("Error al validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }
    #endregion

    #endregion

    protected void Chk_Proveedor_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Proveedor.Checked)
        {
            Cmb_Proveedores.Enabled = true;
            Cmb_Proveedores.SelectedIndex = 0;
        }
        else
        {
            Cmb_Proveedores.Enabled = false;
            Cmb_Proveedores.SelectedIndex = 0;
        }
    }
    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Estatus_Busqueda_Abanzada();
    }
}