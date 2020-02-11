using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using Presidencia.Catalogo_Compras_Servicios.Negocio;
using Presidencia.Catalogo_Compras_Impuestos.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Compras_Partidas.Negocio;
using CarlosAg.ExcelXmlWriter;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Excel = Microsoft.Office.Interop.Excel;


public partial class paginas_Compras_Frm_Cat_Com_Servicios : System.Web.UI.Page
{
    #region Variables Globales
    
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 1;
    private const int Const_Estado_Modificar = 2;
    private const int Const_Estado_Buscar = 3;    
    private static DataTable Dt_servicios = new DataTable();
    private static string M_Busqueda = "";

    #endregion

    #region Page Load / Init
    protected void Page_Load(object sender, EventArgs e)
    {
        List<DropDownList> Lista_Combos = new List<DropDownList>();
        Lista_Combos.Add(Cmb_Capitulo);
        Lista_Combos.Add(Cmb_Concepto);
        Lista_Combos.Add(Cmb_Partida_Generica);
        Lista_Combos.Add(Cmb_Partida_Especifica);

        try
        {
            //Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty))
                Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Estado_Botones(Const_Estado_Inicial);
                Cargar_Combos();
            }
            else
            {
                foreach (DropDownList Cmb in Lista_Combos)
                {
                    foreach (ListItem LI in Cmb.Items)
                        LI.Attributes.Add("Title", LI.Text);
                }
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
    ///NOMBRE DE LA FUNCIÓN: Exportar_Excel_Registros_Grid
    ///DESCRIPCIÓN: Consulta los prdocutos y los envia a excel
    ///PARAMETROS: 
    ///CREO: Susana Trigueros
    ///FECHA_CREO: 2/FEB/13 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Exportar_Excel_Registros()
    {
        try
        {
            Cls_Cat_Com_Servicios_Negocio Negocio = new Cls_Cat_Com_Servicios_Negocio(); //Variable para la capa de negocios
            DataTable Dt_Servicios = new DataTable();
            //Tabla con los datos de las cuentas principales
            Dt_Servicios = Negocio.Consulta_Datos_A_Exportar_Excel();
            if (Dt_Servicios != null && Dt_Servicios.Rows.Count > 0)
            {
                string Ruta_Archivo;

                //Se determina el nombre del archivo
                string filename = "Rpt_Servicios_Registrados" + String.Format("{0:MM-dd-yyyy}", DateTime.Now) + ".xls";
                //Se establece la ruta donde se guardara el archivo
                Ruta_Archivo = Server.MapPath("~") + "\\" + "Archivos" + "\\" + filename;
                Workbook book = new Workbook();
                // Especificar qué hoja debe ser abierto y el tamaño de la ventana por defecto
                book.ExcelWorkbook.ActiveSheetIndex = 0;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;
                // Propiedades del documento
                book.Properties.Author = "CONTEL";
                book.Properties.Title = "PRODUCTOS_REGISTRADOS";
                book.Properties.Created = DateTime.Now;

                // Se agrega estilo al libro
                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Courier New";
                style.Font.Size = 9;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style.Font.Color = "White";
                //style.Interior.Color = "Black";
                style.Interior.Color = "Blue";
                style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");


                style.Interior.Pattern = StyleInteriorPattern.Solid;

                // Estilo
                WorksheetStyle style_Filas_Primarias = book.Styles.Add("FilaPrincipalStyle");
                style_Filas_Primarias.Font.FontName = "Courier New";
                style_Filas_Primarias.Font.Size = 8;
                style_Filas_Primarias.Font.Bold = false;
                style_Filas_Primarias.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style_Filas_Primarias.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                style_Filas_Primarias.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                style_Filas_Primarias.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                style_Filas_Primarias.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

                // Se Crea el estilo a usar
                style = book.Styles.Add("Default");
                style.Font.FontName = "Courier New";
                style.Font.Size = 8;
                // Se le da nombre a la hoja
                Worksheet sheet = book.Worksheets.Add("Hoja1");
                // Se ajustan las columnas
                sheet.Table.Columns.Add(new WorksheetColumn(40));//CLAVE
                sheet.Table.Columns.Add(new WorksheetColumn(120));//NOMBRE
                sheet.Table.Columns.Add(new WorksheetColumn(55));//COSTO
                sheet.Table.Columns.Add(new WorksheetColumn(74));//IMPUESTO
                sheet.Table.Columns.Add(new WorksheetColumn(150));//PARTIDA
                
                WorksheetRow row = sheet.Table.Rows.Add();
                row.Index = 0;//Para saltarse Filas

                row.Cells.Add(new WorksheetCell("CLAVE", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("NOMBRE", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("COSTO", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("IMPUESTO", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("PARTIDA", "HeaderStyle"));
                //                row.Cells.Add(new WorksheetCell("REF_JAPAMI", "HeaderStyle"));

                // Se recorre el grid 
                for (int Cont_Filas = 0; Cont_Filas < Dt_Servicios.Rows.Count; Cont_Filas++)
                {
                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell(Dt_Servicios.Rows[Cont_Filas]["CLAVE"].ToString().Trim(), "FilaPrincipalStyle"));
                    row.Cells.Add(new WorksheetCell(Dt_Servicios.Rows[Cont_Filas]["NOMBRE"].ToString(), "FilaPrincipalStyle"));
                    row.Cells.Add(new WorksheetCell(Dt_Servicios.Rows[Cont_Filas]["COSTO"].ToString(), "FilaPrincipalStyle"));                    
                    row.Cells.Add(new WorksheetCell(Dt_Servicios.Rows[Cont_Filas]["IMPUESTO"].ToString(), "FilaPrincipalStyle"));
                    row.Cells.Add(new WorksheetCell(Dt_Servicios.Rows[Cont_Filas]["PARTIDA"].ToString(), "FilaPrincipalStyle"));
                }
                ////GUARDAR Y MOSTRAR REPORTE 
                
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                Response.Charset = "UTF-8";
                Response.ContentEncoding = System.Text.Encoding.Default;
                book.Save(Response.OutputStream);
                Response.End();

       
            }
            else
            {
                Mensaje_Error("&nbsp; &nbsp; &nbsp; &nbsp; + No se encontraron registros");
            }

        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message, Ex);
        }

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos
    ///DESCRIPCIÓN: metodo que recibe el DataRow seleccionado de la grilla y carga los datos en los componetes de l formulario
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 02:07:54 p.m.
    ///MODIFICO:            Roberto González Oseguera
    ///FECHA_MODIFICO:      06-mar-2011
    ///CAUSA_MODIFICACIÓN:  Se agregó la consulta de IDs para a partir del ID de Partida 
    ///             específica, obtener los IDs de la partida genérica, concepto y capítulo 
    ///             al que pertenece y seleccionar los valores correspondientes en cada combo
    ///*******************************************************************************
    private void Cargar_Datos(DataRow Dr_Servicio)
    {
        Cls_Cat_Com_Partidas_Negocio Rs_Consulta_Cat_Com_Partidas = new Cls_Cat_Com_Partidas_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos
        DataTable Dt_IDs_Partida;    //Tabla para obtener los IDs de la Partida genérica, Concepto y Capítulo de una partida seleccionada

        try
        {
            Txt_Comentarios.Text = Dr_Servicio[Cat_Com_Servicios.Campo_Comentarios].ToString();
            Txt_Costo.Text = Dr_Servicio[Cat_Com_Servicios.Campo_Costo].ToString();
            Hdn_Servicio_ID.Value = Dr_Servicio[Cat_Com_Servicios.Campo_Servicio_ID].ToString();
            Txt_Clave.Text = Dr_Servicio[Cat_Com_Servicios.Campo_Clave].ToString();
            Txt_Nombre_Servicio.Text = Dr_Servicio[Cat_Com_Servicios.Campo_Nombre].ToString();
            Cmb_Impuestos.SelectedValue = Dr_Servicio[Cat_Com_Servicios.Campo_Impuesto_ID].ToString();
            
            Rs_Consulta_Cat_Com_Partidas.P_Partida_ID = Dr_Servicio[Cat_Com_Servicios.Campo_Partida_ID].ToString();
            Dt_IDs_Partida = Rs_Consulta_Cat_Com_Partidas.Consulta_IDs();        //Consulta los IDs

            if (Dt_IDs_Partida.Rows.Count > 0)      //Si se obtuvieron los IDs, seleccionar los valores correspondientes en cada combo
            {
                Cmb_Capitulo.SelectedIndex = Cmb_Capitulo.Items.IndexOf(Cmb_Capitulo.Items.FindByValue(Dt_IDs_Partida.Rows[0][Cat_Sap_Concepto.Campo_Capitulo_ID].ToString()));
                Cmb_Capitulo_SelectedIndexChanged(this, EventArgs.Empty);
                Cmb_Concepto.SelectedIndex = Cmb_Concepto.Items.IndexOf(Cmb_Concepto.Items.FindByValue(Dt_IDs_Partida.Rows[0][Cat_Sap_Concepto.Campo_Concepto_ID].ToString()));
                Cmb_Concepto_SelectedIndexChanged(this, EventArgs.Empty);
                Cmb_Partida_Generica.SelectedIndex = Cmb_Partida_Generica.Items.IndexOf(Cmb_Partida_Generica.Items.FindByValue(Dt_IDs_Partida.Rows[0][Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID].ToString()));
                Cmb_Partida_Generica_SelectedIndexChanged(this, EventArgs.Empty);
                Cmb_Partida_Especifica.SelectedIndex = Cmb_Partida_Especifica.Items.IndexOf(Cmb_Partida_Especifica.Items.FindByValue(Dr_Servicio[Cat_Com_Servicios.Campo_Partida_ID].ToString()));
                Cmb_Partida_Especifica.SelectedValue = Dr_Servicio[Cat_Com_Servicios.Campo_Partida_ID].ToString();
                //Deshabilitar combos activados por la llamada al evento SelectedIndexChanged
                Cmb_Concepto.Enabled = false;
                Cmb_Partida_Generica.Enabled = false;
                Cmb_Partida_Especifica.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
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
    private void Cargar_Grid(int Page_Index){
        try{
            Cls_Cat_Com_Servicios_Negocio Servicios_Negocio = new Cls_Cat_Com_Servicios_Negocio();
            Servicios_Negocio.P_Nombre = M_Busqueda;
            Servicios_Negocio.P_Clave = M_Busqueda;
            Dt_servicios = Servicios_Negocio.Consulta_Servicios();
            Grid_Servicios.PageIndex = Page_Index;
            Grid_Servicios.DataSource = Dt_servicios;
            Grid_Servicios.DataBind();
            
        }
        catch(Exception Ex){
            Mensaje_Error(Ex.Message);
        }
    }    

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
        //Instancia el objeto de negocio de impuestos y consulta la lista de impuestos
        Cls_Cat_Com_Impuestos_Negocio Impuestos_Negocio = new Cls_Cat_Com_Impuestos_Negocio();
        Llenar_Combo_ID(Cmb_Impuestos, Impuestos_Negocio.Consulta_Impuestos(), Cat_Com_Impuestos.Campo_Nombre, Cat_Com_Impuestos.Campo_Impuesto_ID, "0");
        Cargar_Combo_Capitulos();
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
                Txt_Costo.Text = String.Empty;
                Hdn_Servicio_ID.Value = String.Empty;
                Txt_Clave.Text = String.Empty;
                Txt_Nombre_Servicio.Text = String.Empty;

                Txt_Comentarios.Enabled = false;
                Txt_Costo.Enabled = false;
                Txt_Clave.Enabled = false;
                Txt_Nombre_Servicio.Enabled = false;

                Cmb_Impuestos.SelectedIndex = 0;
                Cmb_Impuestos.SelectedIndex = 0;
                Cmb_Capitulo.SelectedIndex = 0;
                Cmb_Concepto.SelectedIndex = -1;
                Cmb_Partida_Generica.SelectedIndex = -1;
                Cmb_Partida_Especifica.SelectedIndex = -1;

                Cmb_Impuestos.Enabled = false;
                Cmb_Capitulo.Enabled = false;
                Cmb_Concepto.Enabled = false;
                Cmb_Partida_Generica.Enabled = false;
                Cmb_Partida_Especifica.Enabled = false;

                Grid_Servicios.Enabled = true;
                Grid_Servicios.SelectedIndex = (-1);

                Btn_Buscar_Servicio.Enabled = true;
                Btn_Eliminar.Enabled = true;
                Btn_Eliminar.Visible = true;
                Btn_Modificar.Enabled = true;
                Btn_Modificar.Visible = true;
                Btn_Nuevo.Enabled = true;
                Btn_Nuevo.Visible = true;
                Btn_Salir.Enabled = true;
                Btn_Buscar_Servicio.AlternateText = "Buscar";
                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Salir";

                Btn_Buscar_Servicio.ToolTip = "Consultar";
                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Salir";

                Btn_Buscar_Servicio.ImageUrl = "~/paginas/imagenes/paginas/busqueda.png";
                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                Configuracion_Acceso("Frm_Cat_Com_Servicios.aspx");
                break;

            case 1: //Nuevo
                Mensaje_Error();
                
                Txt_Comentarios.Text = String.Empty;
                Txt_Costo.Text = String.Empty;
                Hdn_Servicio_ID.Value = String.Empty;
                Txt_Clave.Text = String.Empty;
                Txt_Nombre_Servicio.Text = String.Empty;

                Cmb_Impuestos.SelectedIndex = 0;
                Cmb_Impuestos.SelectedIndex = 0;
                Cmb_Capitulo.SelectedIndex = 0;
                Cmb_Concepto.SelectedIndex = -1;
                Cmb_Partida_Generica.SelectedIndex = -1;
                Cmb_Partida_Especifica.SelectedIndex = -1;

                Txt_Comentarios.Enabled = true;
                Txt_Costo.Enabled = true;                
                Txt_Nombre_Servicio.Enabled = true;

                Cmb_Impuestos.Enabled = true;
                Cmb_Capitulo.Enabled = true;
                
                Btn_Eliminar.Enabled = false;
                Btn_Eliminar.Visible = false;
                Btn_Modificar.Enabled = false;
                Btn_Modificar.Visible = false;
                Btn_Nuevo.Enabled = true;
                Btn_Salir.Enabled = true;

                Grid_Servicios.SelectedIndex = (-1);
                Grid_Servicios.Enabled = false;
                
                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Nuevo.AlternateText = "Guardar";
                Btn_Salir.AlternateText = "Cancelar";
                
                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Guardar";
                Btn_Salir.ToolTip = "Cancelar";

                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";

                break;

            case 2: //Modificar
                Mensaje_Error();
                
                Txt_Comentarios.Enabled = true;
                Txt_Costo.Enabled = true;                
                Txt_Nombre_Servicio.Enabled = true;

                Cmb_Impuestos.Enabled = true;
                Cmb_Capitulo.Enabled = true;
                Cmb_Concepto.Enabled = true;
                Cmb_Partida_Generica.Enabled = true;
                Cmb_Partida_Especifica.Enabled = true;
                
                Btn_Eliminar.Enabled = false;
                Btn_Eliminar.Visible = false;
                Btn_Modificar.Enabled = true;
                Btn_Nuevo.Enabled = false;
                Btn_Nuevo.Visible = false;
                Btn_Salir.Enabled = true;

                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Guardar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Cancelar";

                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Guardar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Cancelar";

                Grid_Servicios.Enabled = false;

                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar_deshabilitado.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo_deshabilitado.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";

                break;

            case 3: //Buscar
                Mensaje_Error();

                Txt_Comentarios.Text = String.Empty;
                Txt_Costo.Text = String.Empty;
                Hdn_Servicio_ID.Value = String.Empty;
                Txt_Clave.Text = String.Empty;
                Txt_Nombre_Servicio.Text = String.Empty;

                Cmb_Impuestos.SelectedIndex = 0;             

                break;
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

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Combo_Capitulos
    /// DESCRIPCION: Consulta los Capítulos dados de alta en la base de datos y los 
    ///             muestra en el combo capítulos utilizando la capa de negocio del 
    ///             Catálogo de Capítulos
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 05-mar-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Capitulos()
    {
        DataTable Dt_Capitulos; //Variable que obtendrá los datos de la consulta        
        Cls_Cat_Com_Partidas_Negocio Rs_Consulta_Cat_SAP_Capitulos = new Cls_Cat_Com_Partidas_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            Dt_Capitulos = Rs_Consulta_Cat_SAP_Capitulos.Consulta_Capitulos(); //Consulta todos los capítulos que estan dadas de alta en la BD
            Cmb_Capitulo.DataSource = Dt_Capitulos;
            Cmb_Capitulo.DataValueField = Cat_SAP_Capitulos.Campo_Capitulo_ID;
            Cmb_Capitulo.DataTextField = "CLAVE_DESCRIPCION";
            Cmb_Capitulo.ToolTip = "Capítulo o Unidad responsable";
            foreach (ListItem LI in Cmb_Capitulo.Items)
                LI.Attributes.Add("Title", LI.Text);
            Cmb_Capitulo.DataBind();
            Cmb_Capitulo.Items.Insert(0, "< SELECCIONE >");
            Cmb_Capitulo.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Capitulos " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Combo_Conceptos
    /// DESCRIPCION: Consulta los Conceptos dados de alta en la base de datos
    ///         (filtrados por capítulo)
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 05-mar-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Conceptos()
    {
        DataTable Dt_Conceptos; //Variable que obtendrá los datos de la consulta        
        Cls_Cat_Com_Partidas_Negocio Rs_Consulta_Cat_SAP_Conceptos = new Cls_Cat_Com_Partidas_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            if (Cmb_Capitulo.SelectedIndex > 0) //Almacenar el valor seleccionado en el combo Capítulo si es mayor a 0
            {
                Rs_Consulta_Cat_SAP_Conceptos.P_Capitulo_ID = Cmb_Capitulo.SelectedValue;
                Dt_Conceptos = Rs_Consulta_Cat_SAP_Conceptos.Consulta_Conceptos(); //Consulta todos los Conceptos que estan dados de alta en la BD
                Cmb_Concepto.DataSource = Dt_Conceptos;
                Cmb_Concepto.DataValueField = Cat_Sap_Concepto.Campo_Concepto_ID;
                Cmb_Concepto.DataTextField = "CLAVE_DESCRIPCION";
                Cmb_Concepto.DataBind();
                foreach (ListItem LI in Cmb_Concepto.Items)
                    LI.Attributes.Add("Title", LI.Text);
                Cmb_Concepto.Items.Insert(0, "< SELECCIONE >");
            }
            else
            {       // Si no hay un capítulo seleccionado, borrar los elementos del combo Conceptos
                if (Cmb_Concepto.SelectedIndex != -1)         //si hay un elemento seleccionado en el combo, seleccionar al elemento 0
                    Cmb_Concepto.SelectedIndex = 0;
                Cmb_Concepto.DataSource = null;
                Cmb_Concepto.DataBind();
                Cmb_Concepto.Enabled = false;
                Cargar_Combo_Partida_Generica();        // llamar el método para limpiar el combo
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Concepto " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Combo_Partida_Generica
    /// DESCRIPCION: Consulta las Partidas genéricas que están dadas de alta en la base de datos
    ///         (filtradas por Concepto)
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 05-mar-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Partida_Generica()
    {
        DataTable Dt_Partidas_Genericas; //Variable que obtendrá los datos de la consulta        
        Cls_Cat_Com_Partidas_Negocio Rs_Consulta_Cat_SAP_Partidas_Genericas = new Cls_Cat_Com_Partidas_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            if (Cmb_Concepto.SelectedIndex > 0) //Si el índice seleccionado en el combo Concepto es mayor a 0
            {
                Rs_Consulta_Cat_SAP_Partidas_Genericas.P_Concepto_ID = Cmb_Concepto.SelectedValue;
                Dt_Partidas_Genericas = Rs_Consulta_Cat_SAP_Partidas_Genericas.Consulta_Partidas_Genericas(); //Consulta todas las Partidas genéricas que estan dadas de alta en la BD
                Cmb_Partida_Generica.DataSource = Dt_Partidas_Genericas;
                Cmb_Partida_Generica.DataValueField = Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID;
                Cmb_Partida_Generica.DataTextField = "Clave_Descripcion";
                Cmb_Partida_Generica.DataBind();
                foreach (ListItem LI in Cmb_Partida_Generica.Items)
                    LI.Attributes.Add("Title", LI.Text);
                Cmb_Partida_Generica.Items.Insert(0, "< SELECCIONE >");
            }
            else
            {           //Si no hay un Concepto seleccionado, borrar los elementos del combo Cmb_Partida_Generica
                if (Cmb_Partida_Generica.SelectedIndex != -1)         //si hay un elemento seleccionado en el combo, seleccionar al elemento 0
                    Cmb_Partida_Generica.SelectedIndex = 0;
                Cmb_Partida_Generica.DataSource = null;
                Cmb_Partida_Generica.DataBind();
                Cmb_Partida_Generica.Enabled = false;
                Cargar_Combo_Partida_Especifica();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Partidas_Genericas " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Combo_Partida_Especifica
    /// DESCRIPCION: Consulta las Partidas específicas que están dadas de alta en la base de datos
    ///         (sólo las que pertenecen a la partida genérica seleccionada)
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 05-mar-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Partida_Especifica()
    {
        DataTable Dt_Partidas_Especificas; //Variable que obtendrá los datos de la consulta        
        Cls_Cat_Com_Partidas_Negocio Rs_Consulta_Cat_SAP_Partidas_Especificas = new Cls_Cat_Com_Partidas_Negocio(); //Variable de conexión hacia la capa de Negocios del catálogo Partidas

        try
        {
            if (Cmb_Partida_Generica.SelectedIndex > 0) // Si el combo Partida genérica es mayor a 0
            {
                Rs_Consulta_Cat_SAP_Partidas_Especificas.P_Partida_Generica_ID = Cmb_Partida_Generica.SelectedValue;
                Dt_Partidas_Especificas = Rs_Consulta_Cat_SAP_Partidas_Especificas.Consulta_Partidas(); //Consulta todas las Partidas genéricas que estan dadas de alta en la BD
                Cmb_Partida_Especifica.DataSource = Dt_Partidas_Especificas;
                Cmb_Partida_Especifica.DataValueField = Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
                Cmb_Partida_Especifica.DataTextField = "Clave_Descripcion";
                Cmb_Partida_Especifica.DataBind();
                foreach (ListItem LI in Cmb_Partida_Especifica.Items)
                    LI.Attributes.Add("Title", LI.Text);
                Cmb_Partida_Especifica.Items.Insert(0, "< SELECCIONE >");
            }
            else
            {           //Si no hay un Partida genérica seleccionada, borrar los elementos del combo Cmb_Partida_Especifica
                if (Cmb_Partida_Especifica.SelectedIndex != -1)         //si hay un elemento seleccionado en el combo, seleccionar al elemento 0
                    Cmb_Partida_Especifica.SelectedIndex = 0;
                Cmb_Partida_Especifica.DataSource = null;
                Cmb_Partida_Especifica.DataBind();
                Cmb_Partida_Especifica.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Partida_Especifica " + ex.Message.ToString(), ex);
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
            if (Txt_Nombre_Servicio.Text.Trim() == "")
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar el nombre del servicio");
            }

            //Verificar que la descripción contenga texto y sea menor de 100 caracteres
            if (!Txt_Comentarios.Text.Trim().Equals(""))
            {
                if (Txt_Comentarios.Text.Trim().Length >= 3600)
                {
                    Txt_Comentarios.Text = Txt_Comentarios.Text.Trim().Substring(0, 3599);
                }
            }
            else
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar una Descripción para el Servicio");
            }
            
            //Verificar que hay un valor seleccionado en los combos
            if (Cmb_Capitulo.SelectedIndex <= 0)
            {
                Resultado = false;
                Mensaje_Error("Favor de seleccionar un Capítulo");
            }
            if (Cmb_Concepto.SelectedIndex <= 0)
            {
                Resultado = false;
                Mensaje_Error("Favor de seleccionar un Concepto");
            }
            if (Cmb_Partida_Generica.SelectedIndex <= 0)
            {
                Resultado = false;
                Mensaje_Error("Favor de seleccionar una Partida genérica");
            }
            if (Cmb_Partida_Especifica.SelectedIndex <= 0)
            {
                Resultado = false;
                Mensaje_Error("Favor de seleccionar una Partida específica");
            }

            if (Txt_Costo.Text.Trim() == "")    //Si el campo costo esta vacio, mostrar mensaje
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar el costo del servicio");
            }
            else                //Si no esta vacio el campo costo, verificar el valor que contiene
            {
                if (Convert.ToDouble(Txt_Costo.Text) <= 0.0)    // Si el costo es menor o igual a 0, mostrar mensaje
                {
                    Resultado = false;
                    Mensaje_Error("Favor de ingresar un costo mayor a 0 para el servicio");
                }
            }

            //Verificar que hay un valor seleccionado en el combo Impuesto
            if (Cmb_Impuestos.SelectedIndex <= 0)
            {
                Resultado = false;
                Mensaje_Error("Favor de especificar el Impuesto del servicio");
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
        return Resultado;
    }    

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Alta_Servicio
    ///DESCRIPCIÓN: validar datos para los procedimientos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 10:43:22 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Alta_Servicio()
    {
        try{
        if (Validar_Datos())
        {
            Cls_Cat_Com_Servicios_Negocio Servicios_Negocio = new Cls_Cat_Com_Servicios_Negocio();
            Servicios_Negocio.P_Nombre = Txt_Nombre_Servicio.Text.Trim();
            Servicios_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();
            Servicios_Negocio.P_Costo = Convert.ToDouble( Txt_Costo.Text.Trim() );
            Servicios_Negocio.P_Impuesto_ID = Cmb_Impuestos.SelectedValue;
            Servicios_Negocio.P_Partida_ID = Cmb_Partida_Especifica.SelectedValue.ToString();
            Servicios_Negocio.P_Clave_Partida = Cmb_Partida_Especifica.SelectedItem.Text.Substring(0,4);
            Servicios_Negocio.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
            Servicios_Negocio.Alta_Servicio();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Servicios", "alert('El Alta del Servicio fue Exitosa');", true);
            Estado_Botones(Const_Estado_Inicial);
            Txt_Busqueda.Text = "";
            Cargar_Grid(0);        
        }
        }
        catch(Exception Ex){
            throw new Exception(Ex.Message, Ex);
        }

    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Baja_Servicio
    ///DESCRIPCIÓN: dar de baja un servicio de la base de datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/04/2011 11:25:30 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Baja_Servicio()
    {
        try
        {
            Cls_Cat_Com_Servicios_Negocio Servicios_Negocio = new Cls_Cat_Com_Servicios_Negocio();
            Servicios_Negocio.P_Servicio_ID = Hdn_Servicio_ID.Value;
            Servicios_Negocio.Baja_Servicio();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Servicios", "alert('La Baja del Servicio fue Exitosa');", true);
            Estado_Botones(Const_Estado_Inicial);
            Cargar_Grid(0);
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message, Ex);
        }

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Modificar_Servicio
    ///DESCRIPCIÓN: Modifica un registro de un Servicio y lo guarda en la base de datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/04/2011 11:58:30 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Modificar_Servicio()
    {
        Cls_Cat_Com_Servicios_Negocio Servicios_Negocio = new Cls_Cat_Com_Servicios_Negocio();

        try
        {
            if (Validar_Datos())
            {
                Servicios_Negocio.P_Clave = Txt_Clave.Text.Trim();
                // Validar cambio de partida especifica (comparar los primeros 4 caracteres de la partida seleccionada en el combo con los primeros 4 caracteres del campo de texto Clave)
                //if (Cmb_Partida_Especifica.SelectedItem.Text.Substring(0, 4) == Txt_Clave.Text.Substring(0, 4))
                //    Servicios_Negocio.P_Clave = Txt_Clave.Text;
                //else
                //{
                //    Servicios_Negocio.P_Clave = "";     // Sin valor para que la capa de datos asigne nueva Clave
                //    Servicios_Negocio.P_Clave_Partida = Cmb_Partida_Especifica.SelectedItem.Text.Substring(0, 4);
                //}
                Servicios_Negocio.P_Servicio_ID = Hdn_Servicio_ID.Value;
                Servicios_Negocio.P_Costo = Convert.ToDouble(Txt_Costo.Text.Trim());
                Servicios_Negocio.P_Impuesto_ID = Cmb_Impuestos.SelectedValue;
                Servicios_Negocio.P_Nombre = Txt_Nombre_Servicio.Text.Trim();
                Servicios_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();
                Servicios_Negocio.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                Servicios_Negocio.P_Partida_ID = Cmb_Partida_Especifica.SelectedValue;
                Servicios_Negocio.Modificar_Servicio();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Servicios", "alert('La modificación del Servicio fue Exitosa');", true);
                Estado_Botones(Const_Estado_Inicial);
                Cargar_Grid(0);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message, Ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Extraer_Numero
    /// 	DESCRIPCIÓN: Mediante una expresión regular encuentra números en el Texto recibido
    /// 	            hasta 12 enteros y hasta cuatro decimales
    /// 	PARÁMETROS:
    /// 		1. Texto: Texto en el que se va a buscar un número
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 05-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private String Extraer_Numero(String Texto)
    {
        Regex Rge_Decimal = new Regex(@"(?<entero>[0-9]{1,12})(?:\.[0-9]{0,4})?");
        Match Numero_Encontrado = Rge_Decimal.Match(Texto);

        return Numero_Encontrado.Value;
    }

    #endregion

    #region Eventos

    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Mensaje_Error();
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Estado_Botones(Const_Estado_Nuevo);
            }
            else
            {
                Alta_Servicio();                
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Mensaje_Error();
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
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Mensaje_Error();
        try
        {
            Cls_Cat_Com_Servicios_Negocio Clase_Negocio = new Cls_Cat_Com_Servicios_Negocio();
            Clase_Negocio.P_Servicio_ID = String.Format("{0:00000}" ,Convert.ToInt32( Txt_Clave.Text.Trim()));
            DataTable Dt_Servicio_en_Proceso  = Clase_Negocio.Consultar_Servicio_en_Proceso();
            if (Dt_Servicio_en_Proceso.Rows.Count ==0)
            {
                if (Hdn_Servicio_ID.Value != String.Empty)
                {

                    if (Btn_Modificar.AlternateText.Equals("Modificar"))
                    {
                        Estado_Botones(Const_Estado_Modificar);
                    }
                    else
                    {
                        Modificar_Servicio();
                    }
                }
                else
                {
                    Mensaje_Error("Favor de seleccionar el Servicio a modificar");
                }
            }
            else{
                Mensaje_Error("El servicio no se puede modificar ya que esta en proceso en una requisicion");
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);

        }
    }
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        Mensaje_Error();
        try
        {
            if (Hdn_Servicio_ID.Value != String.Empty)
            {
                Baja_Servicio();
            }
            else
            {
                Mensaje_Error("Favor de seleccionar el Servicio a eliminar");
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    protected void Btn_Buscar_Servicio_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Estado_Botones(Const_Estado_Buscar);
            Grid_Servicios.SelectedIndex = (-1);
            M_Busqueda = Txt_Busqueda.Text.Trim();
            Cargar_Grid(0);
            Txt_Busqueda.Focus();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Cmb_Capitulo_SelectedIndexChanged
    /// 	DESCRIPCIÓN: Evento Cambio de índice en combo Cmb_Capitulo, activar y cargar el Cmb_Concepto con 
    /// 	            los Conceptos del Capítulo seleccionado
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 05-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Capitulo_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Si se selecciona capítulo del combo, activar el combo concepto
        if (Cmb_Capitulo.SelectedIndex > 0)
        {
            // Activar el combo Concepto y volverlo a cargar
            Cmb_Concepto.Enabled = true;
            Cmb_Partida_Generica.Enabled = false;
            Cargar_Combo_Conceptos();
            Cargar_Combo_Partida_Generica();
            Cmb_Concepto.Focus();
        }
        else
        {
            // Desactivar y limpiar el combo Concepto
            Cmb_Concepto.Enabled = false;
            Cmb_Partida_Generica.Enabled = false;
            Cargar_Combo_Conceptos();
            Cargar_Combo_Partida_Generica();
            Cmb_Capitulo.Focus();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Cmb_Concepto_SelectedIndexChanged
    /// 	DESCRIPCIÓN: Evento Cambio de índice en combo Cmb_Concepto, activar y cargar el Cmb_Partida_Generica con 
    /// 	            las Partidas Genéricas del Concepto seleccionado
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 05-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Concepto_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Si se selecciona concepto del combo, activar el combo partidas genericas
        if (Cmb_Concepto.SelectedIndex > 0)
        {
            // Activar el combo partidas genericas y volverlo a cargar
            Cmb_Partida_Generica.Enabled = true;
            Cargar_Combo_Partida_Generica();
            Cmb_Partida_Generica.Focus();
        }
        else
        {
            // Desactivar y limpiar el combo partidas genericas
            Cargar_Combo_Partida_Generica();
            Cmb_Partida_Generica.Enabled = false;
            Cmb_Concepto.Focus();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Cmb_Partida_Generica_SelectedIndexChanged
    /// 	DESCRIPCIÓN: Evento Cambio de índice en combo Cmb_Concepto, activar y cargar el Cmb_Partida_Generica con 
    /// 	            las Partidas Genéricas del Concepto seleccionado
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 05-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Partida_Generica_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Si se selecciona una partida genérica del combo, activar el combo partidas específicas
        if (Cmb_Partida_Generica.SelectedIndex > 0)
        {
            // Activar el combo partidas especificas y volverlo a cargar
            Cmb_Partida_Especifica.Enabled = true;
            Cargar_Combo_Partida_Especifica();
            Cmb_Partida_Especifica.Focus();
        }
        else
        {
            // Desactivar y limpiar el combo partidas específicas
            Cargar_Combo_Partida_Especifica();
            Cmb_Partida_Especifica.Enabled = false;
            Cmb_Partida_Generica.Focus();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Txt_Costo_TextChanged
    /// 	DESCRIPCIÓN: Al cambiar el texto de Txt_Costo, cambiar a número de máximo 12 posiciones 
    /// 	            enteras y dos decimales
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 05-mar-2011 
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Txt_Costo_TextChanged(object sender, EventArgs e)
    {
        Txt_Costo.Text = String.Format("{0:0.00}", Convert.ToDouble(Extraer_Numero(Txt_Costo.Text)));
        Cmb_Impuestos.Focus();
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Exportar_Excel_Click
    /// 	DESCRIPCIÓN: Evento que genera el Excel
    /// 	PARÁMETROS:
    /// 	CREO: Susana Trigueros Armenta
    /// 	FECHA_CREO: 6-feb-13 
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Exportar_Excel_Click(object sender, ImageClickEventArgs e)
    {
        Exportar_Excel_Registros();
    }

    #endregion

    #region Eventos Grid
    protected void Grid_Servicios_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Servicios.SelectedIndex > (-1))
            {
                Cargar_Datos(Dt_servicios.Rows[Grid_Servicios.SelectedIndex + (Grid_Servicios.PageIndex*5)]);                
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    protected void Grid_Servicios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Servicios.SelectedIndex = (-1);
        Cargar_Grid(e.NewPageIndex);
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
            Botones.Add(Btn_Nuevo);
            Botones.Add(Btn_Modificar);
            Botones.Add(Btn_Eliminar);
            Botones.Add(Btn_Buscar_Servicio);

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
}