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
using CarlosAg.ExcelXmlWriter;

namespace Presidencia.Ayudante_CarlosAG
{
    public class Cls_Ayudante_Crear_Excel
    {
        /// *************************************************************************************************************************
        /// Nombre: Generar_Excel
        /// 
        /// Descripción: Metodo que devuelve un libro de excel.
        /// 
        /// Parámetros: Dt_Reporte.- Información que se mostrara en el reporte.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 10/Diciembre/2011.
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// *************************************************************************************************************************
        public static CarlosAg.ExcelXmlWriter.Workbook Generar_Excel(DataTable Dt_Reporte)
        {
            //Creamos el libro de Excel.
            CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();

            try
            {
                Libro.Properties.Title = "Reporte de Empleados";
                Libro.Properties.Created = DateTime.Now;
                Libro.Properties.Author = "Presidencia_RH";

                //Creamos una hoja que tendrá el libro.
                CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Reporte Empleados");
                //Agregamos un renglón a la hoja de excel.
                CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();
                //Creamos el estilo cabecera para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera = Libro.Styles.Add("HeaderStyle");
                //Creamos el estilo contenido para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido = Libro.Styles.Add("BodyStyle");

                Estilo_Cabecera.Font.FontName = "Tahoma";
                Estilo_Cabecera.Font.Size = 10;
                Estilo_Cabecera.Font.Bold = true;
                Estilo_Cabecera.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_Cabecera.Font.Color = "#FFFFFF";
                Estilo_Cabecera.Interior.Color = "#193d61";
                Estilo_Cabecera.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Cabecera.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

                Estilo_Contenido.Font.FontName = "Tahoma";
                Estilo_Contenido.Font.Size = 9;
                Estilo_Contenido.Font.Bold = true;
                Estilo_Contenido.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                Estilo_Contenido.Font.Color = "#000000";
                Estilo_Contenido.Interior.Color = "White";
                Estilo_Contenido.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Contenido.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");


                if (Dt_Reporte is System.Data.DataTable)
                {
                    if (Dt_Reporte.Rows.Count > 0)
                    {
                        foreach (System.Data.DataColumn COLUMNA in Dt_Reporte.Columns)
                        {
                            if (COLUMNA is System.Data.DataColumn)
                            {
                                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(200));
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(COLUMNA.ColumnName, "HeaderStyle"));
                            }
                        }

                        foreach (System.Data.DataRow FILA in Dt_Reporte.Rows)
                        {
                            if (FILA is System.Data.DataRow)
                            {
                                Renglon = Hoja.Table.Rows.Add();

                                foreach (System.Data.DataColumn COLUMNA in Dt_Reporte.Columns)
                                {
                                    if (COLUMNA is System.Data.DataColumn)
                                    {
                                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(FILA[COLUMNA.ColumnName].ToString(), DataType.String, "BodyStyle"));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar el reporte de Empleados. Error: [" + Ex.Message + "]");
            }
            return Libro;
        }
        /// *************************************************************************************************************************
        /// Nombre: Generar_Excel
        /// 
        /// Descripción: Metodo que devuelve un libro de excel.
        /// 
        /// Parámetros: Dt_Reporte.- Información que se mostrara en el reporte.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 10/Diciembre/2011.
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// *************************************************************************************************************************
        public static CarlosAg.ExcelXmlWriter.Workbook Generar_Excel_Poliza(DataTable Dt_Reporte)
        {
            //Creamos el libro de Excel.
            CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();
            int Contador = 0;

            try
            {
                Libro.Properties.Title = "Reporte de Empleados";
                Libro.Properties.Created = DateTime.Now;
                Libro.Properties.Author = "Presidencia_RH";

                //Creamos una hoja que tendrá el libro.
                CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Reporte Empleados");
                //Agregamos un renglón a la hoja de excel.
                CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();
                //Creamos el estilo cabecera para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera = Libro.Styles.Add("HeaderStyle");
                //Creamos el estilo contenido para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido = Libro.Styles.Add("BodyStyle");

                Estilo_Cabecera.Font.FontName = "Tahoma";
                Estilo_Cabecera.Font.Size = 10;
                Estilo_Cabecera.Font.Bold = true;
                Estilo_Cabecera.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_Cabecera.Font.Color = "#FFFFFF";
                Estilo_Cabecera.Interior.Color = "#193d61";
                Estilo_Cabecera.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Cabecera.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

                Estilo_Contenido.Font.FontName = "Tahoma";
                Estilo_Contenido.Font.Size = 9;
                Estilo_Contenido.Font.Bold = true;
                Estilo_Contenido.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                Estilo_Contenido.Font.Color = "#000000";
                Estilo_Contenido.Interior.Color = "White";
                Estilo_Contenido.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Contenido.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");


                if (Dt_Reporte is System.Data.DataTable)
                {
                    if (Dt_Reporte.Rows.Count > 0)
                    {
                        foreach (System.Data.DataColumn COLUMNA in Dt_Reporte.Columns)
                        {
                            if (COLUMNA is System.Data.DataColumn)
                            {
                                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(200));
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(COLUMNA.Caption, "HeaderStyle"));
                            }
                        }

                        foreach (System.Data.DataRow FILA in Dt_Reporte.Rows)
                        {
                            if (FILA is System.Data.DataRow)
                            {
                                Renglon = Hoja.Table.Rows.Add();

                                foreach (System.Data.DataColumn COLUMNA in Dt_Reporte.Columns)
                                {
                                    if (COLUMNA is System.Data.DataColumn)
                                    {
                                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(FILA[COLUMNA.ColumnName].ToString(), DataType.String, "BodyStyle"));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar el reporte de Empleados. Error: [" + Ex.Message + "]");
            }
            return Libro;
        }
    }
}
