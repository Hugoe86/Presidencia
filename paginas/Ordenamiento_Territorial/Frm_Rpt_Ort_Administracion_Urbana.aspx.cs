using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Orden_Territorial_Administracion_Urbana.Negocio;
using Presidencia.Catalogo_Tramites.Negocio;
using Presidencia.Solicitud_Tramites.Negocios;

public partial class paginas_Ordenamiento_Territorial_Frm_Rpt_Ort_Administracion_Urbana : System.Web.UI.Page
{
    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN         : Metodo que se carga cada que ocurre un PostBack de la Página
        ///PARÁMETROS          :
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                //Configuracion_Formulario(false);
                Llenar_Grid_Listado(0);
            }
            Div_Contenedor_Msj_Error.Visible = false;
        }

    #endregion

    #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Listado
        ///DESCRIPCIÓN         : Llena el Listado con una consulta que puede o no
        ///                      tener Filtros.
        ///PARÁMETROS          : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        private void Llenar_Grid_Listado(int Pagina)
        {
            try
            {
                Grid_Listado.SelectedIndex = (-1);
                Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
                Grid_Listado.Columns[1].Visible = true;
                Grid_Listado.Columns[2].Visible = true;
                Grid_Listado.Columns[3].Visible = true;
                Grid_Listado.Columns[4].Visible = true;
                Grid_Listado.DataSource = Negocio.Consultar_Tabla_Administracion_Urbana(Negocio);
                Grid_Listado.PageIndex = Pagina;
                Grid_Listado.DataBind();
                Grid_Listado.Columns[1].Visible = false;
                Grid_Listado.Columns[2].Visible = false;
                Grid_Listado.Columns[3].Visible = false;
                Grid_Listado.Columns[4].Visible = false;
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
        ///DESCRIPCIÓN         : Limpia los controles del Formulario
        ///PARÁMETROS          :
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        private void Limpiar_Catalogo()
        {
            Hdf_Administracion_Urbana_ID.Value = "";
            Hdf_Tramite_ID.Value = "";
            Hdf_Solicitud_ID.Value = "";
            Hdf_Subproceso_ID.Value = "";
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
        ///DESCRIPCIÓN         : genera el reporte de pdf
        ///PARÁMETROS          : 1. Ds_Reporte: Dataset con datos a imprimir
        ///                      2. Nombre_Reporte: Nombre del archivo de reporte .rpt
        ///                      3. Nombre_Archivo: Nombre del archivo a generar
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///******************************************************************************
        public void Generar_Reporte(DataSet Ds_Reporte, String Extension_Archivo, String Tipo, string Ruta_Archivo_Rpt)
        {
            //Obtiene el nombre del archivo que sera asignado al documento
            String Nombre_Archivo = "Reporte_Administracion_Urbana" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today));
            String Ruta_Archivo = @Server.MapPath(Ruta_Archivo_Rpt);//Obtiene la ruta en la cual será guardada el archivo
            ReportDocument Reporte = new ReportDocument();

            try
            {
                Reporte.Load(Ruta_Archivo);
                Reporte.SetDataSource(Ds_Reporte);

                DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();
                //  para el tipo de archivo
                if (Extension_Archivo == "PDF")
                    Nombre_Archivo += ".pdf";
                else if (Extension_Archivo == "EXCEL")
                    Nombre_Archivo += ".xls";

                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;

                //  para el tipo de archivo
                if (Extension_Archivo == "PDF")
                    Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                else if (Extension_Archivo == "EXCEL")
                    Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;

                Reporte.Export(Opciones_Exportacion);

                if (Extension_Archivo == "PDF")
                    Abrir_Ventana(Nombre_Archivo, Tipo);

                else if (Extension_Archivo == "EXCEL")
                {
                    String Ruta_Destino = "../../Reporte/" + Nombre_Archivo;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta_Destino + "', '" + Tipo + "','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Abrir_Ventana
        ///DESCRIPCIÓN         : Abre en otra ventana el archivo pdf
        ///PARÁMETROS          : Nombre_Archivo: Guarda el nombre del archivo que se desea abrir
        ///                      para mostrar los datos al usuario
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///******************************************************************************
        private void Abrir_Ventana(String Nombre_Archivo, String Tipo)
        {
            String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
            try
            {
                Pagina = Pagina + Nombre_Archivo;
                ScriptManager.RegisterStartupScript(this, this.GetType(), Tipo,
                "window.open('" + Pagina + "', '" + Tipo + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            catch (Exception ex)
            {
                throw new Exception("Abrir_Ventana " + ex.Message.ToString(), ex);
            }
        }

    #endregion

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Reporte_Click
        ///DESCRIPCIÓN         : Consulta los datos del elemento seleccionado para generar
        ///                      el reporte.
        ///PARÁMETROS          :     
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        protected void Btn_Reporte_Click(object sender, ImageClickEventArgs e)
        {
            if (Grid_Listado.SelectedIndex > (-1) && Hdf_Administracion_Urbana_ID.Value.ToString().Length != 0)
            {
                Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
                DataSet Ds_Reporte = new DataSet();
                DataTable Dt_Admon_Urbana = new DataTable();
                DataTable Dt_Tramite = new DataTable();
                DataTable Dt_Solicitud = new DataTable();
                DataTable Dt_Subproceso = new DataTable();
                DataTable Dt_Supervision = new DataTable();
                DataTable Dt_Condicion_Inmueble = new DataTable();
                DataTable Dt_Avance_Obra = new DataTable();
                DataTable Dt_Area = new DataTable();
                DataTable Dt_Uso_Actual = new DataTable();
                try
                {
                    Negocio.P_Administracion_Urbana_ID = Hdf_Administracion_Urbana_ID.Value;
                    Negocio.P_Tramite_ID = Hdf_Tramite_ID.Value;
                    Negocio.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                    Negocio.P_Subproceso_ID = Hdf_Subproceso_ID.Value;
                    Dt_Admon_Urbana = Negocio.Consultar_Administracion_Urbana(Negocio);
                    Dt_Admon_Urbana.TableName = "Dt_Admon_Urbana";

                    if (Dt_Admon_Urbana.Rows.Count >= 0)
                    {
                        Cls_Cat_Tramites_Negocio Tramites = new Cls_Cat_Tramites_Negocio();
                        Negocio.P_Tramite_ID = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Tramite_ID].ToString();
                        Dt_Tramite = Tramites.Consultar_Tabla_Tramite();
                        Dt_Tramite.TableName = "Dt_Tramite";

                        Cls_Ope_Solicitud_Tramites_Negocio Solicitud = new Cls_Ope_Solicitud_Tramites_Negocio();
                        Solicitud.P_Solicitud_ID = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Solicitud_ID].ToString();
                        Dt_Solicitud = Solicitud.Consultar_Solicitud().Tables[0];
                        Dt_Solicitud.TableName = "Dt_Solicitud";

                        Solicitud.P_Subproceso_ID = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Subproceso_ID].ToString();
                        Dt_Subproceso = Solicitud.Consultar_Tabla_Subproceso();
                        Dt_Subproceso.TableName = "Dt_Subproceso";

                        Dt_Supervision = Negocio.Consultar_Tipo_Supervision();
                        Dt_Supervision.TableName = "Dt_Supervision";

                        Dt_Condicion_Inmueble = Negocio.Consultar_Condiciones_Inmueble();
                        Dt_Condicion_Inmueble.TableName = "Dt_Condicion_Inmueble";

                        Dt_Avance_Obra = Negocio.Consultar_Avance_Obra();
                        Dt_Avance_Obra.TableName = "Dt_Avance_Obra";

                        Dt_Area = Negocio.Consultar_Condiciones_Via_Publica_Donacion();
                        Dt_Area.TableName = "Dt_Area";

                        Dt_Uso_Actual = Negocio.Consultar_Uso_Actual_Terreno();
                        Dt_Uso_Actual.TableName = "Dt_Uso_Actual";

                        Ds_Reporte.Tables.Add(Dt_Admon_Urbana.Copy());
                        Ds_Reporte.Tables.Add(Dt_Tramite.Copy());
                        Ds_Reporte.Tables.Add(Dt_Solicitud.Copy());
                        Ds_Reporte.Tables.Add(Dt_Subproceso.Copy());
                        Ds_Reporte.Tables.Add(Dt_Supervision.Copy());
                        Ds_Reporte.Tables.Add(Dt_Condicion_Inmueble.Copy());
                        Ds_Reporte.Tables.Add(Dt_Avance_Obra.Copy());
                        Ds_Reporte.Tables.Add(Dt_Area.Copy());
                        Generar_Reporte(Ds_Reporte, "PDF", "Administracion_Urbana", "../Rpt/Ordenamiento_Territorial/Rpt_Ort_Ficha_Revision.rpt");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Generar_Reporte_Ficha_Revision: " + ex.Message.ToString(), ex);
                }

            }
        }

    #endregion

    #region Grids

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_SelectedIndexChanged
        ///DESCRIPCIÓN         : Obtiene el elemento seleccionado.
        ///PARÁMETROS          :     
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        protected void Grid_Listado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Listado.SelectedIndex > (-1))
                {
                    Limpiar_Catalogo();
                    Hdf_Administracion_Urbana_ID.Value = HttpUtility.HtmlDecode(Grid_Listado.SelectedRow.Cells[1].Text.Trim());
                    Hdf_Tramite_ID.Value = HttpUtility.HtmlDecode(Grid_Listado.SelectedRow.Cells[2].Text.Trim());
                    Hdf_Solicitud_ID.Value = HttpUtility.HtmlDecode(Grid_Listado.SelectedRow.Cells[3].Text.Trim());
                    Hdf_Subproceso_ID.Value = HttpUtility.HtmlDecode(Grid_Listado.SelectedRow.Cells[4].Text.Trim());
                    //Mostrar_Datos(ID);
                    System.Threading.Thread.Sleep(500);
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
}