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
using Presidencia.Sessiones;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Text;
using Presidencia.Reportes_Nomina.Prestamos.Negocio;
using Presidencia.Ayudante_Calendario_Nomina;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Constantes;
using Presidencia.Prestamos.Negocio;
using System.Reflection;
using CarlosAg.ExcelXmlWriter;

public partial class paginas_Nomina_Frm_Rpt_Nom_Prestamos : System.Web.UI.Page{

    #region "Page_Load"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Evento se carga al iniciar la pagina
        ///PARAMETROS:  
        ///CREO: Susana Trigueros Armenta.
        ///FECHA_CREO: 04/Abril/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e) {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            Div_Contenedor_Msj_Error.Visible = false;
            if (!IsPostBack) {
                Llenar_Combo_Unidades_Responsables();
                Llenar_Combo_Tipos_Nomina();
                Consultar_Calendario_Nominas();
            }
        }

    #endregion

    #region "Metodos"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Unidades_Responsables
        ///DESCRIPCIÓN: Se llena el Combo de las Dependencias
        ///PARAMETROS:  1.- Tipo.- Tipo de Reporte.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 05/Abril/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Llenar_Combo_Unidades_Responsables() {
            Cmb_Unidad_Responsable.DataSource = Cls_Rpt_Nom_Prestamos_Negocio.Consultar_Dependencias_Activas();
            Cmb_Unidad_Responsable.DataTextField = "NOMBRE";
            Cmb_Unidad_Responsable.DataValueField = "DEPENDENCIA_ID";
            Cmb_Unidad_Responsable.DataBind();
            Cmb_Unidad_Responsable.Items.Insert(0, new ListItem("< - TODAS - >", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Unidades_Responsables
        ///DESCRIPCIÓN: Se llena el Combo de las Dependencias
        ///PARAMETROS:  1.- Tipo.- Tipo de Reporte.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 05/Abril/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Llenar_Combo_Tipos_Nomina() {
            Cmb_Tipos_Nomina.DataSource = Cls_Rpt_Nom_Prestamos_Negocio.Consultar_Tipos_Nomina();
            Cmb_Tipos_Nomina.DataTextField = "NOMBRE";
            Cmb_Tipos_Nomina.DataValueField = "TIPO_NOMINA_ID";
            Cmb_Tipos_Nomina.DataBind();
            Cmb_Tipos_Nomina.Items.Insert(0, new ListItem("< - TODOS - >", ""));
        }

       ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Calendario_Nominas
        ///DESCRIPCIÓN: Consulta los calendarios de nomina vigentes actualmente.
        ///CREO: Juan alberto Hernández Negrete
        ///FECHA_CREO: 19/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Consultar_Calendario_Nominas() {
            Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nominas = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
            DataTable Dt_Calendario_Nominas = null;//Variable que almacenara una lista de los calendarios de nomina vigentes.
            try  {
                Dt_Calendario_Nominas = Consulta_Calendario_Nominas.Consultar_Calendario_Nominas();
                if (Dt_Calendario_Nominas != null) {
                    if (Dt_Calendario_Nominas.Rows.Count > 0) {
                        Dt_Calendario_Nominas = Formato_Fecha_Calendario_Nomina(Dt_Calendario_Nominas);
                        Cmb_Anio.DataSource = Dt_Calendario_Nominas;
                        Cmb_Anio.DataTextField = "Nomina";
                        Cmb_Anio.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                        Cmb_Anio.DataBind();
                        Cmb_Anio.Items.Insert(0, new ListItem("< - TODOS - >", ""));
                        Cmb_Anio.SelectedIndex = -1;


                        Cmb_Anio.SelectedIndex = Cmb_Anio.Items.IndexOf(Cmb_Anio.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Anyo));

                        Consultar_Periodos_Catorcenales_Nomina(Cmb_Anio.SelectedValue.Trim());

                        Cmb_Periodo.SelectedIndex = Cmb_Periodo.Items.IndexOf(Cmb_Periodo.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Periodo));

                    } else {
                        Lbl_Ecabezado_Mensaje.Text = "No se encontraron nominas vigentes";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
            } catch (Exception Ex) {
                throw new Exception("Error al consultar las nominas. Error: [" + Ex.Message + "]");
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
        /// FECHA_CREO  : 19/Enero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private DataTable Formato_Fecha_Calendario_Nomina(DataTable Dt_Calendario_Nominas) {
            DataTable Dt_Nominas = new DataTable();//Variable que almacenara los calendarios de nóminas.
            DataRow Renglon_Dt_Clon = null;//Variable que almacenará un renglón del calendario de la nómina.

            //Creamos las columnas.
            Dt_Nominas.Columns.Add("Nomina", typeof(System.String));
            Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

            foreach (DataRow Renglon in Dt_Calendario_Nominas.Rows) {
                Renglon_Dt_Clon = Dt_Nominas.NewRow();
                Renglon_Dt_Clon["Nomina"] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString().Split(new char[] { ' ' })[0].Split(new char[] { '/' })[2];
                Renglon_Dt_Clon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID];
                Dt_Nominas.Rows.Add(Renglon_Dt_Clon);
            }
            return Dt_Nominas;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
        ///DESCRIPCIÓN: Consulta los periodos catorcenales para el 
        ///calendario de nomina seleccionado.
        ///PARAMETROS: Nomina_ID.- Indica el calendario de nomina del cuál se desea consultar
        ///                        los periodos catorcenales.
        ///CREO: Juan alberto Hernández Negrete
        ///FECHA_CREO: 19/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Consultar_Periodos_Catorcenales_Nomina(String Nomina_ID) {
            Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nomina_Periodos = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Clase de conexion con la capa de negocios
            DataTable Dt_Periodos_Catorcenales = null;//Variable que almacenra unaa lista de los periodos catorcenales que le correspónden a la nomina seleccionada.

            try {
                Consulta_Calendario_Nomina_Periodos.P_Nomina_ID = Nomina_ID;
                Dt_Periodos_Catorcenales = Consulta_Calendario_Nomina_Periodos.Consulta_Detalles_Nomina();

                if (Dt_Periodos_Catorcenales != null) {
                    if (Dt_Periodos_Catorcenales.Rows.Count > 0) {
                        Cmb_Periodo.DataSource = Dt_Periodos_Catorcenales;
                        Cmb_Periodo.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                        Cmb_Periodo.DataValueField = Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID;
                        Cmb_Periodo.DataBind();
                        Cmb_Periodo.Items.Insert(0, new ListItem("< - TODOS - >", ""));
                        Cmb_Periodo.SelectedIndex = -1;
                    } else {
                        Lbl_Ecabezado_Mensaje.Text = "No se encontraron periodos catorcenales para la nomina seleccionada.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
            } catch (Exception Ex) {
                throw new Exception("Error al consultar los periodos catorcenales del  calendario de nomina seleccionado. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Crear_Tablas_Reporte
        ///DESCRIPCIÓN: Crea las Tablas con las que se generara el Reporte
        ///PARAMETROS:  1.- Tipo.- Tipo de Reporte.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Abril/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Crear_Tablas_Reporte(String Tipo) {
            Ds_Rpt_Nom_Prestamos Ds_Reporte = new Ds_Rpt_Nom_Prestamos();
            //Se hace la consulta del reporte.
            Cls_Rpt_Nom_Prestamos_Negocio Negocio = new Cls_Rpt_Nom_Prestamos_Negocio();
            Negocio.P_Tipo_Nomina_ID = Cmb_Tipos_Nomina.SelectedItem.Value;
            Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedItem.Value;
            Negocio.P_No_Empleado = (Txt_No_Empleado.Text.Trim().Length > 0) ? String.Format("{0:000000}", Convert.ToInt32(Txt_No_Empleado.Text)) : "";
            Negocio.P_RFC_Empleado = Txt_RFC_Empleado.Text.Trim();
            Negocio.P_Nombre_Empleado = Txt_Nombre_Empleado.Text.Trim();
            Negocio.P_Tipo_Reporte = RBL_Listado_Reportes_Disponibles.SelectedItem.Value;
            if (Cmb_Anio.SelectedIndex > 0) { Negocio.P_Nomina_ID = Cmb_Anio.SelectedItem.Value; }
            if (Cmb_Periodo.SelectedIndex > 0) { Negocio.P_No_Nomina = Cmb_Periodo.SelectedItem.Text.Trim(); }
            DataTable Dt_Consulta = Negocio.Consultar_Datos_Prestamos();
            Dt_Consulta.TableName = "DT_GENERALES_EMPLEADO";
            if (Tipo.Trim().Equals("PDF")) { 
                DataSet Ds_Consulta = new DataSet();
                Ds_Consulta.Tables.Add(Dt_Consulta.Copy());
                Ds_Consulta.Tables.Add(Obtener_Datos_Generales_Reporte());
                Generar_Reporte(Ds_Consulta, Ds_Reporte, "Cr_Rpt_Nom_Prestamos.rpt", "Cr_Rpt_Nom_Prestamos.pdf");            
            } else if (Tipo.Trim().Equals("EXCEL")) {
                Cambiar_Nombre_Cabeceras(ref Dt_Consulta);
                Pasar_DataTable_A_Excel(Dt_Consulta);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Datos_Generales_Reporte
        ///DESCRIPCIÓN: Carga los Datos Generales del Reporte
        ///PARAMETROS:  
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Abril/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private DataTable Obtener_Datos_Generales_Reporte() {
            DataTable Dt_Datos = new DataTable("DT_GENERALES_REPORTE");
            Dt_Datos.Columns.Add("ELABORO", Type.GetType("System.String"));
            Dt_Datos.Columns.Add("NOMBRE_REPORTE", Type.GetType("System.String"));

            DataRow Fila = Dt_Datos.NewRow();
            Fila["ELABORO"] = Cls_Sessiones.Nombre_Empleado;
            Fila["NOMBRE_REPORTE"] = "Reporte de " + RBL_Listado_Reportes_Disponibles.SelectedItem.Text;
            Dt_Datos.Rows.Add(Fila);

            return Dt_Datos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
        ///DESCRIPCIÓN: caraga el data set fisico con el cual se genera el Reporte especificado
        ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
        ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
        ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
        ///CREO: Susana Trigueros Armenta.
        ///FECHA_CREO: 01/Mayo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, String Nombre_Reporte, String Nombre_Archivo) {
            String Ruta = "../../Reporte/ " + Nombre_Archivo;
            ReportDocument Reporte = new ReportDocument();
            String File_Path = Server.MapPath("../Rpt/Nomina/" + Nombre_Reporte);
            Reporte.Load(File_Path);
            Ds_Reporte = Data_Set_Consulta_DB;
            Reporte.SetDataSource(Ds_Reporte);
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath(Ruta);
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            Habilitar_Subreportes(ref Reporte);
            Reporte.SetParameterValue("Numero_Registros", Data_Set_Consulta_DB.Tables[0].Rows.Count);
            Reporte.Export(Export_Options);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600');", true);
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Habilitar_Subreportes
        ///DESCRIPCIÓN: Muestra u oculta los Subreportes.
        ///PARAMETROS:
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Abril/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Habilitar_Subreportes(ref ReportDocument Reporte) {
            foreach (ListItem Item in RBL_Listado_Reportes_Disponibles.Items)
            {
                try
                {
                    if (Reporte.ReportDefinition.ReportObjects[Item.Value] != null)
                    {
                        if (Item.Selected)
                        {
                            Reporte.ReportDefinition.ReportObjects[Item.Value].ObjectFormat.EnableSuppress = false;
                        }
                    }
                }
                catch (Exception Ex)
                {
                    continue;
                }
            }
            for (Int32 Cnt_Secciones = 0; Cnt_Secciones < Reporte.ReportDefinition.Sections.Count; Cnt_Secciones++)
            {
                for (Int32 Cnt_Objetos = 0; Cnt_Objetos < Reporte.ReportDefinition.Sections[Cnt_Secciones].ReportObjects.Count; Cnt_Objetos++)
                {
                    Boolean Salir = false;
                    String Nombre_Objeto = Reporte.ReportDefinition.Sections[Cnt_Secciones].ReportObjects[Cnt_Objetos].Name.Trim();
                    foreach (ListItem Item in RBL_Listado_Reportes_Disponibles.Items)
                    {
                        if (Item.Value.Trim().Equals(Nombre_Objeto))
                        {
                            Reporte.ReportDefinition.Sections[Cnt_Secciones].SectionFormat.EnableSuppress = !Item.Selected;
                            Salir = true;
                            break;
                        }
                    }
                    if (Salir) { break; }
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cambiar_Nombre_Cabeceras
        ///DESCRIPCIÓN: Cambia el Nombre de las Cabeceras
        ///PARAMETROS:
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 09/Abril/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Cambiar_Nombre_Cabeceras(ref DataTable Dt_Datos_Empleado) {
            Dt_Datos_Empleado.Columns["NOMBRE_EMPLEADO"].ColumnName = "Nombre";
            Dt_Datos_Empleado.Columns["NO_EMPLEADO"].ColumnName = "No. Empleado";
            Dt_Datos_Empleado.Columns["NOMBRE_DEPENDENCIA"].ColumnName = "Unidad Responsable";
            Dt_Datos_Empleado.Columns["NO_SOLICITUD"].ColumnName = "No. Solicitud";
            Dt_Datos_Empleado.Columns["IMPORTE_PRESTAMO"].ColumnName = "Importe Prestamo";
            Dt_Datos_Empleado.Columns["CATORCENAS_DESCONTAR"].ColumnName = "Catorcenas a Descontar";
            Dt_Datos_Empleado.Columns["CANTIDAD_DESCONTAR"].ColumnName = "Cantidad a Descontar";
            Dt_Datos_Empleado.Columns["ESTATUS_SOLICITUD"].ColumnName = "Estatus de Solicitud";
            Dt_Datos_Empleado.Columns["CANTIDAD_PAGADA"].ColumnName = "Cantidad Pagada";
            Dt_Datos_Empleado.Columns["SALDO_PRESTAMO"].ColumnName = "Saldo del Prestamo";
            Dt_Datos_Empleado.Columns["NO_CUENTA"].ColumnName = "Motivo Baja";
            Dt_Datos_Empleado.Columns["BANCO"].ColumnName = "Banco";
            Dt_Datos_Empleado.Columns["CATORCENAS_PAGAR"].ColumnName = "Catorcenas a Pagar";
            Dt_Datos_Empleado.Columns["CLAVE_DEDUCCION"].ColumnName = "Clave Deducción";
            Dt_Datos_Empleado.Columns["INICIO_PAGO"].ColumnName = "Inicio Pago";
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Reporte_Detalle
        ///DESCRIPCIÓN: Se define el orden y las columnas a Mostrar dependiendo del reporte.
        ///PARAMETROS:
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 09/Abril/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private Int32[] Obtener_Reporte_Detalle() {
            Int32[] Columnas_Mostrar = new Int32[]{};
            switch (RBL_Listado_Reportes_Disponibles.SelectedItem.Value) {
                case "PRESTAMOS_CAPTURADOS":    Columnas_Mostrar = new Int32[] { 4, 3, 1, 2, 5, 6, 7, 8 };
                                                break;
                case "SALDO_PRESTAMOS":         Columnas_Mostrar = new Int32[] { 4, 3, 1, 2, 5, 9, 10 };
                                                break;
                case "DEPOSITO_PRESTAMOS":      Columnas_Mostrar = new Int32[] { 4, 3, 1, 2, 5, 12, 11, 14 };
                                                break;
                case "PRESTAMOS_AUTORIZADOS":   Columnas_Mostrar = new Int32[] { 4, 3, 1, 2, 5, 7, 10, 15, 13 };
                                                break;
            }
            return Columnas_Mostrar;
        }

        /// *************************************************************************************************************************
        /// Nombre: Pasar_DataTable_A_Excel
        /// 
        /// Descripción: Pasa DataTable a Excel.
        /// 
        /// Parámetros: Dt_Reporte.- DataTable que se pasara a excel.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 18/Octubre/2011.
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// *************************************************************************************************************************
        public void Pasar_DataTable_A_Excel(System.Data.DataTable Dt_Reporte) {
            String Ruta = RBL_Listado_Reportes_Disponibles.SelectedItem.Text + ".xls";//Variable que almacenara el nombre del archivo. 

            try {

                Int32[] Columas_Mostrar = Obtener_Reporte_Detalle();
                //Creamos el libro de Excel.
                CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();

                Libro.Properties.Title = "RH";
                Libro.Properties.Created = DateTime.Now;
                Libro.Properties.Author = "RH";

                //Creamos una hoja que tendrá el libro.
                CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Registros");
                //Agregamos un renglón a la hoja de excel.
                CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();
                //Creamos el estilo cabecera para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera = Libro.Styles.Add("HeaderStyle");
                //Creamos el estilo contenido para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido = Libro.Styles.Add("BodyStyle");
                //Creamos el estilo titulo para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Titulo = Libro.Styles.Add("TitleStyle");

                Estilo_Titulo.Font.FontName = "Tahoma";
                Estilo_Titulo.Font.Size = 9;
                Estilo_Titulo.Font.Bold = true;
                Estilo_Titulo.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_Titulo.Alignment.Vertical = StyleVerticalAlignment.Center;
                Estilo_Titulo.Font.Color = "#808080";
                Estilo_Titulo.Interior.Color = "#FFFFFF";
                Estilo_Titulo.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Titulo.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 0, "Black");
                Estilo_Titulo.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 0, "Black");
                Estilo_Titulo.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 0, "Black");
                Estilo_Titulo.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 0, "Black");
                Estilo_Titulo.Alignment.WrapText = true;

                Estilo_Cabecera.Font.FontName = "Tahoma";
                Estilo_Cabecera.Font.Size = 10;
                Estilo_Cabecera.Font.Bold = true;
                Estilo_Cabecera.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_Cabecera.Alignment.Vertical = StyleVerticalAlignment.Center;
                Estilo_Cabecera.Font.Color = "#FFFFFF";
                Estilo_Cabecera.Interior.Color = "#193d61";
                Estilo_Cabecera.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Cabecera.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Alignment.WrapText = true;

                Estilo_Contenido.Font.FontName = "Tahoma";
                Estilo_Contenido.Font.Size = 8;
                Estilo_Contenido.Font.Bold = true;
                Estilo_Contenido.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_Contenido.Font.Color = "#000000";
                Estilo_Contenido.Interior.Color = "White";
                Estilo_Contenido.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Contenido.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Alignment.WrapText = true;

                //SE CARGA LA CABECERA PRINCIPAL DEL ARCHIVO
                WorksheetCell cell = Renglon.Cells.Add("MUNICIPIO DE IRAPUATO, GUANAJUATO");
                cell.MergeAcross = Columas_Mostrar.Length - 1;
                cell.StyleID = "TitleStyle";
                Renglon.Height = 20;
                Renglon = Hoja.Table.Rows.Add();

                cell = Renglon.Cells.Add("COORDINACION GENERAL DE ADMINISTRACION - DIRECCION DE RECURSOS HUMANOS");
                cell.MergeAcross = Columas_Mostrar.Length - 1;
                cell.StyleID = "TitleStyle";
                Renglon.Height = 20;
                Renglon = Hoja.Table.Rows.Add();

                cell = Renglon.Cells.Add(Obtener_Datos_Generales_Reporte().Rows[0]["NOMBRE_REPORTE"].ToString());
                cell.MergeAcross = Columas_Mostrar.Length - 1;
                cell.StyleID = "TitleStyle";
                Renglon.Height = 20;
                Renglon = Hoja.Table.Rows.Add();

                cell = Renglon.Cells.Add("");
                cell.MergeAcross = Columas_Mostrar.Length - 1;
                cell.StyleID = "TitleStyle";
                Renglon.Height = 20;
                Renglon = Hoja.Table.Rows.Add();

                //Agregamos las columnas que tendrá la hoja de excel.
                foreach (Int32 Columna in Columas_Mostrar) {
                    if (Columna <= Dt_Reporte.Columns.Count) {
                        Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(180));
                    }
                }
                if (Dt_Reporte is System.Data.DataTable) {
                    if (Dt_Reporte.Rows.Count > 0) {
                        foreach (Int32 Columna in Columas_Mostrar) {
                            if (Columna <= Dt_Reporte.Columns.Count) {
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Dt_Reporte.Columns[Columna].ColumnName, "HeaderStyle"));
                                Renglon.Height = 20;
                            }
                        }
                        foreach (System.Data.DataRow FILA in Dt_Reporte.Rows) {
                            if (FILA is System.Data.DataRow) {
                                Renglon = Hoja.Table.Rows.Add();
                                foreach (Int32 Columna in Columas_Mostrar) {
                                    if (Columna <= Dt_Reporte.Columns.Count) {
                                        if (!String.IsNullOrEmpty(FILA[Columna].ToString())) { 
                                            if (Columna == 5 || Columna == 7 || Columna == 9 || Columna == 10) {
                                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(String.Format("{0:c}", Convert.ToDouble(FILA[Columna])), DataType.String, "BodyStyle"));
                                            }
                                            else if (Columna == 15) {
                                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(FILA[Columna])), DataType.String, "BodyStyle"));
                                            } else {
                                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(FILA[Columna].ToString(), DataType.String, "BodyStyle"));
                                            }
                                        } else {
                                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(FILA[Columna].ToString(), DataType.String, "BodyStyle"));
                                        }
                                    }
                                }
                                Renglon.Height = 35;
                                Renglon.AutoFitHeight = true;
                            }
                        }
                    }
                }

                //Abre el archivo de excel
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + Ruta);
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Libro.Save(Response.OutputStream);
                Response.End();
            } catch (Exception Ex) {
                throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
            }
        }

    #endregion

    #region "Eventos"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_PDF_Click
        ///DESCRIPCIÓN: Ejecuta la generacion del REporte en PDF
        ///PARAMETROS:  
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Abril/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        protected void Btn_Generar_Reporte_PDF_Click(object sender, ImageClickEventArgs e) {
            try {
                if (RBL_Listado_Reportes_Disponibles.SelectedIndex > (-1)) {
                    Crear_Tablas_Reporte("PDF");
                } else {
                    Lbl_Ecabezado_Mensaje.Text = "Es necesario Seleccionar un Reporte.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_PDF_Click
        ///DESCRIPCIÓN: Ejecuta la generacion del REporte en PDF
        ///PARAMETROS:  
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 09/Abril/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        protected void Btn_Generar_Reporte_Excel_Click(object sender, ImageClickEventArgs e) {
            try {
                if (RBL_Listado_Reportes_Disponibles.SelectedIndex > (-1)) {
                    Crear_Tablas_Reporte("EXCEL");
                } else {
                    Lbl_Ecabezado_Mensaje.Text = "Es necesario Seleccionar un Reporte.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
        ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
        ///CREO: Juan alberto Hernández Negrete
        ///FECHA_CREO: 19/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Cmb_Anio_SelectedIndexChanged(object sender, EventArgs e) {
            Int32 Nomina_Seleccionada = 0;//Variable que almacena la nómina seleccionada del combo.

            try {
                ////Obtenemos elemento seleccionado del combo.
                if (Cmb_Anio.SelectedIndex > 0) {
                    Nomina_Seleccionada = Convert.ToInt32(Cmb_Anio.SelectedItem.Text.Trim());
                    Consultar_Periodos_Catorcenales_Nomina(Cmb_Anio.SelectedValue.Trim());
                } else {
                    Cmb_Periodo.DataSource = new DataTable();
                    Cmb_Periodo.DataBind();  
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message.ToString();
                Lbl_Mensaje_Error.Text = ""; 
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    #endregion

}