using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Constantes;
using Presidencia.Sessiones;

namespace Presidencia.Reportes
{
    public class Cls_Reportes
    {

        /// *************************************************************************************
        /// NOMBRE:             Generar_Reporte
        /// DESCRIPCIÓN:        Método que invoca la generación del reporte.
        ///              
        /// PARÁMETROS:         Ds_Reporte_Crystal.- Es el DataSet con el que se muestra el reporte en cristal 
        ///                     Ruta_Reporte_Crystal.-  Ruta y Nombre del archivo del Crystal Report.
        ///                     Nombre_Reporte_Generar.- Nombre que tendrá el reporte generado.
        ///                     Formato.- Es el tipo de reporte "PDF", "Excel"
        /// USUARIO CREO:       Juan Alberto Hernández Negrete.
        /// FECHA CREO:         3/Mayo/2011 18:15 p.m.
        /// USUARIO MODIFICO:   Salvador Henrnandez Ramirez
        /// FECHA MODIFICO:     16/Mayo/2011
        /// CAUSA MODIFICACIÓN: Se cambio Nombre_Plantilla_Reporte por Ruta_Reporte_Crystal, ya que este contendrá tambien la ruta
        ///                     y se asigno la opción para que se tenga acceso al método que muestra el reporte en Excel.
        /// *************************************************************************************
        public void Generar_Reporte(ref DataSet Ds_Reporte_Crystal, String Ruta_Reporte_Crystal, String Nombre_Reporte_Generar, String Formato)
        {
            ReportDocument Reporte = new ReportDocument(); // Variable de tipo reporte.
            String Ruta = String.Empty;  // Variable que almacenará la ruta del archivo del crystal report. 

            try
            {
                Ruta = HttpContext.Current.Server.MapPath(Ruta_Reporte_Crystal);
                Reporte.Load(Ruta);

                if (Ds_Reporte_Crystal is DataSet)
                {
                    if (Ds_Reporte_Crystal.Tables.Count > 0)
                    {
                        Reporte.SetDataSource(Ds_Reporte_Crystal);

                        if (Formato == "PDF")
                        {
                            Exportar_Reporte_PDF(Reporte, Nombre_Reporte_Generar);
                        }
                        else if (Formato == "Excel")
                            Exportar_Reporte_Excel(Reporte, Nombre_Reporte_Generar);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
            }
        }

        /// *************************************************************************************
        /// NOMBRE:             Generar_Reporte
        /// DESCRIPCIÓN:        Sobrecarga de Método que invoca la generación del reporte.
        ///              
        /// PARÁMETROS:         Ds_Reporte_Crystal.- Es el DataSet con el que se muestra el reporte en cristal 
        ///                     Ruta_Reporte_Crystal.-  Ruta y Nombre del archivo del Crystal Report.
        ///                     Nombre_Reporte_Generar.- Nombre que tendrá el reporte generado.
        ///                     Formato.- Es el tipo de reporte "PDF", "Excel"
        /// USUARIO CREO:       Antonio Salvador Benavides Guardado
        /// FECHA CREO:         21/Noviembre/2011
        /// USUARIO MODIFICO:   
        /// FECHA MODIFICO:     
        /// CAUSA MODIFICACIÓN: 
        /// *************************************************************************************
        public void Generar_Reporte(ref DataSet Ds_Reporte_Crystal, String Ruta_Reporte_Crystal, String Ruta_Reporte_Generar, String Nombre_Reporte_Generar, String Formato)
        {
            ReportDocument Reporte = new ReportDocument(); // Variable de tipo reporte.
            String Ruta = String.Empty;  // Variable que almacenará la ruta del archivo del crystal report. 

            try
            {
                Ruta = HttpContext.Current.Server.MapPath(Ruta_Reporte_Crystal);
                Reporte.Load(Ruta);

                if (Ds_Reporte_Crystal is DataSet)
                {
                    if (Ds_Reporte_Crystal.Tables.Count > 0)
                    {
                        Reporte.SetDataSource(Ds_Reporte_Crystal);

                        if (Formato == "PDF")
                        {
                            Exportar_Reporte_PDF(Reporte, Ruta_Reporte_Generar, Nombre_Reporte_Generar);
                        }
                        else if (Formato == "Excel")
                            Exportar_Reporte_Excel(Reporte, Ruta_Reporte_Generar, Nombre_Reporte_Generar);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
            }
        }


        /// *************************************************************************************
        /// NOMBRE:             Exportar_Reporte_PDF
        /// DESCRIPCIÓN:        Método que guarda el reporte generado en formato PDF en la ruta
        ///                     especificada.
        /// PARÁMETROS:         Reporte.- Objeto de tipo documento que contiene el reporte a guardar.
        ///                     Nombre_Reporte.- Nombre que se le dio al reporte.
        /// USUARIO CREO:       Juan Alberto Hernández Negrete.
        /// FECHA CREO:         3/Mayo/2011 18:19 p.m.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// *************************************************************************************
        public void Exportar_Reporte_PDF(ReportDocument Reporte, String Nombre_Reporte_Generar)
        {
            ExportOptions Opciones_Exportacion = new ExportOptions();
            DiskFileDestinationOptions Direccion_Guardar_Disco = new DiskFileDestinationOptions();
            PdfRtfWordFormatOptions Opciones_Formato_PDF = new PdfRtfWordFormatOptions();

            try
            {
                if (Reporte is ReportDocument)
                {
                    Direccion_Guardar_Disco.DiskFileName = HttpContext.Current.Server.MapPath("../../Reporte/" + Nombre_Reporte_Generar);
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

        /// *************************************************************************************
        /// NOMBRE:             Exportar_Reporte_PDF
        /// DESCRIPCIÓN:        Sobrecarga del Método que guarda el reporte generado en formato PDF en la ruta
        ///                     especificada.
        /// PARÁMETROS:         Reporte.- Objeto de tipo documento que contiene el reporte a guardar.
        ///                     Nombre_Reporte.- Nombre que se le dio al reporte.
        /// USUARIO CREO:       Antonio Salvador Benavides Guardado
        /// FECHA CREO:         21/Noviembre/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// *************************************************************************************
        public void Exportar_Reporte_PDF(ReportDocument Reporte, String Ruta_Reporte_Generar, String Nombre_Reporte_Generar)
        {
            ExportOptions Opciones_Exportacion = new ExportOptions();
            DiskFileDestinationOptions Direccion_Guardar_Disco = new DiskFileDestinationOptions();
            PdfRtfWordFormatOptions Opciones_Formato_PDF = new PdfRtfWordFormatOptions();

            try
            {
                if (Reporte is ReportDocument)
                {
                    Direccion_Guardar_Disco.DiskFileName = HttpContext.Current.Server.MapPath(Ruta_Reporte_Generar + Nombre_Reporte_Generar);
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


        /// *************************************************************************************
        /// NOMBRE:             Exportar_Reporte_PDF
        /// DESCRIPCIÓN:        Método que guarda el reporte generado en formato PDF en la ruta
        ///                     especificada.
        /// PARÁMETROS:         Reporte.- Objeto de tipo documento que contiene el reporte a guardar.
        ///                     Nombre_Reporte.- Nombre que se le dio al reporte.
        /// USUARIO CREO:       Salvador Hernandez Ramírez.
        /// FECHA CREO:         16/Mayo/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// *************************************************************************************
        public void Exportar_Reporte_Excel(ReportDocument Reporte, String Nombre_Reporte_Generar)
        {
            if (Reporte is ReportDocument)
            {
                ExportOptions CrExportOptions = new ExportOptions();

                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = HttpContext.Current.Server.MapPath("../../Reporte/" + Nombre_Reporte_Generar);
                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;

                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                Reporte.Export(CrExportOptions);
            }
        }

        /// *************************************************************************************
        /// NOMBRE:             Exportar_Reporte_PDF
        /// DESCRIPCIÓN:        Sobrecarga de Método que guarda el reporte generado en archivo XLS en la ruta
        ///                     especificada.
        /// PARÁMETROS:         Reporte.- Objeto de tipo documento que contiene el reporte a guardar.
        ///                     Nombre_Reporte.- Nombre que se le dio al reporte.
        /// USUARIO CREO:       Antonio Salvador Benavides Guardado
        /// FECHA CREO:         21/Noviembre/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// *************************************************************************************
        public void Exportar_Reporte_Excel(ReportDocument Reporte, String Ruta_Reporte_Generar, String Nombre_Reporte_Generar)
        {
            if (Reporte is ReportDocument)
            {
                ExportOptions CrExportOptions = new ExportOptions();

                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = HttpContext.Current.Server.MapPath(Ruta_Reporte_Generar + Nombre_Reporte_Generar);
                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;

                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                Reporte.Export(CrExportOptions);
            }
        }
    }
}