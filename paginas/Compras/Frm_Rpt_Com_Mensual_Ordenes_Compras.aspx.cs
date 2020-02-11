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
using Presidencia.Constantes;
using Presidencia.Reporte_Mensual_Ordenes_Compra.Negocio;
using CarlosAg.ExcelXmlWriter;
using System.Text;

public partial class paginas_Compras_Frm_Rpt_Com_Mensual_Ordenes_Compras : System.Web.UI.Page
{
    #region PAGE LOAD

        ///*****************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
        ///DESCRIPCIÓN          : Ejecuta la operacion de click en el boton de salir
        ///PARAMETROS           : 
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 28/Diciembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*****************************************************************************************************************
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
                if (!IsPostBack)
                {
                    Reporte_Ordenes_Compra_Inicio();
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error en el inicio del reporte de ordenes de compras. Error [" + Ex.Message + "]");
            }
        }
    #endregion

    #region METODOS

        ///*****************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Reporte_Ordenes_Compra_Inicio
        ///DESCRIPCIÓN          : Metodo de inicio de la página
        ///PARAMETROS           : 
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 28/Diciembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*****************************************************************************************************************
        private void Reporte_Ordenes_Compra_Inicio()
        {
            try
            {
                Div_Contenedor_Msj_Error.Visible = false;
                Limpiar_Controles("Todo");
                Llenar_Combo_Estatus();
            }
            catch (Exception Ex)
            {
                throw new Exception("Error en el inicio del reporte de ordenes de compras. Error [" + Ex.Message + "]");
            }
        }

        ///*****************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Limpiar_Controles
        ///DESCRIPCIÓN          : Metodo para limpiar los controles del formulario
        ///PARAMETROS           1 Accion: para indicar que parte del codigo limpiara 
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 28/Diciembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*****************************************************************************************************************
        private void Limpiar_Controles(String Accion)
        {
            try
            {
                switch (Accion)
                {
                    case "Todo":
                        Txt_Fecha_Inicio.Text = "";
                        Txt_Fecha_Fin.Text = "";
                        Lbl_Ecabezado_Mensaje.Text = "";
                        Lbl_Mensaje_Error.Text = "";
                        break;
                    case "Error":
                        Lbl_Ecabezado_Mensaje.Text = "";
                        Lbl_Mensaje_Error.Text = "";
                        break;
                    case "Fecha":
                        Txt_Fecha_Inicio.Text = "";
                        Txt_Fecha_Fin.Text = "";
                        break;

                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al limpiar los controles Error [" + Ex.Message + "]");
            }
        }

        ///*****************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validar_Campos
        ///DESCRIPCIÓN          : Metodo para validar los campos del formulario
        ///PARAMETROS           : 
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 28/Diciembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*****************************************************************************************************************
        private Boolean Validar_Campos()
        {
            Boolean Datos_Validos = true;
            Limpiar_Controles("Error");
            Lbl_Ecabezado_Mensaje.Text = "Favor de:";
            try
            {
                if (String.IsNullOrEmpty(Txt_Fecha_Inicio.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Introducir una fecha de inicio <br />";
                    Datos_Validos = false;
                }
                else {
                    if (Txt_Fecha_Inicio.Text.Trim().Equals("__/___/____"))
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Introducir una fecha de inicio <br />";
                        Datos_Validos = false;
                    }
                }
               if (String.IsNullOrEmpty(Txt_Fecha_Fin.Text.Trim()))
               {
                   Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Introducir una fecha de termino <br />";
                   Datos_Validos = false;
               }
               else
               {
                   if (Txt_Fecha_Fin.Text.Trim().Equals("__/___/____"))
                   {
                       Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Introducir una fecha de termino <br />";
                       Datos_Validos = false;
                   }
               }
                return Datos_Validos;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al validar los campos del formulario Error [" + Ex.Message + "]");
            }
        }

        ///*****************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Estatus
        ///DESCRIPCIÓN          : Metodo para llenar el combo de los estatus de las ordenes de compra
        ///PARAMETROS           : 
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 28/Diciembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*****************************************************************************************************************
        private void Llenar_Combo_Estatus() 
        {
            Cls_Rpt_Com_Mensual_Orden_Compra_Negocio Rpt_Ordenes_Negocio = new Cls_Rpt_Com_Mensual_Orden_Compra_Negocio(); //Conexion con la capa de negocios
            DataTable Dt_Estatus = new DataTable(); //Para almacenar los datos de los estatus de las ordenes de compras
            
            try 
            {
                Dt_Estatus = Rpt_Ordenes_Negocio.Consultar_Estatus();

                Cmb_Estatus.Items.Clear();
                Cmb_Estatus.DataValueField = "ESTATUS";
                Cmb_Estatus.DataTextField = "ESTATUS";
                Cmb_Estatus.DataSource = Dt_Estatus;
                Cmb_Estatus.DataBind();
                Cmb_Estatus.Items.Insert(0, new ListItem("<- SELECCIONE ->", ""));
            }
            catch(Exception Ex)
            {
                throw new Exception("Error al llenar el combo de estatus: Error[" + Ex.Message + "]");
            }
        }

        ///*****************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Generar_Rpt_Mensual_Ordenes_Compras
        ///DESCRIPCIÓN          : Metodo generar el reporte mensual  de las ordenes de compras
        ///PARAMETROS           1 Dt_Ordenes: ordenes de compras que se pasaran al reporte 
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 29/Diciembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*****************************************************************************************************************
        public void Generar_Rpt_Mensual_Ordenes_Compras(DataTable Dt_Ordenes) 
        {
            String Ruta_Archivo = "Reporte_Mensual_Ordenes_Compra.xls";
            try
            {
                CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();
                Libro.Properties.Title = "Reporte Mensual de Ordenes de Compras";
                Libro.Properties.Created = DateTime.Now;
                Libro.Properties.Author = "SIAG";

                //Creamos una hoja que tendrá el libro.
                CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Reporte Mensual Ordenes Compras");
                //Agregamos un renglón a la hoja de excel.
                CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();
                //Creamos el estilo cabecera para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera = Libro.Styles.Add("HeaderStyle");
                //Creamos el estilo contenido para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido = Libro.Styles.Add("BodyStyle");
                //Creamos el estilo contenido para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cantidad = Libro.Styles.Add("BodyStyleCant");

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

                Estilo_Cantidad.Font.FontName = "Tahoma";
                Estilo_Cantidad.Font.Size = 9;
                Estilo_Cantidad.Font.Bold = true;
                Estilo_Cantidad.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                Estilo_Cantidad.Font.Color = "#000000";
                Estilo_Cantidad.Interior.Color = "White";
                Estilo_Cantidad.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Cantidad.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cantidad.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cantidad.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cantidad.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

                //Agregamos las columnas que tendrá la hoja de excel.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(70));//Folio.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(140));//Fecha Creo.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//Estatus.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(300));//Unidad Responsable.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(200));//Fuente Financiamiento.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(500));//Partida Especifica.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(500));//Concepto.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(200));//Cotizador.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(300));//Nombre Proveedor.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(140));//Fecha Contrarecibo.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(140));//Fecha Pago Contrarecibo
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//Total.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(200));//Observaciones.

                if(Dt_Ordenes is DataTable)
                {
                    if(Dt_Ordenes.Rows.Count > 0)
                    {
                        foreach(DataColumn Columna in Dt_Ordenes.Columns)
                        {
                            if(Columna is DataColumn)
                            {
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Columna.ColumnName, "HeaderStyle"));
                            }
                        }
                        foreach(DataRow Dr in Dt_Ordenes.Rows)
                        {
                            Renglon = Hoja.Table.Rows.Add();
                            foreach(DataColumn Columna in Dt_Ordenes.Columns)
                            {
                                if (Columna is DataColumn) 
                                {
                                    if (Columna.ColumnName.Equals("TOTAL"))
                                    {
                                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Dr[Columna.ColumnName].ToString(), DataType.String, "BodyStyleCant"));
                                    }
                                    else
                                    {
                                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Dr[Columna.ColumnName].ToString(), DataType.String, "BodyStyle"));
                                    }
                                }
                            }
                        }
                    }
                }

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + Ruta_Archivo);
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Libro.Save(Response.OutputStream);
                Response.End();

            }
            catch (Exception ex) 
            {
                throw new Exception("Error al generar el reporte en excel Error[" + ex.Message + "]");
            }
        }
    #endregion

    #region EVENTOS

        ///*****************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
        ///DESCRIPCIÓN          : Ejecuta la operacion de click en el boton de salir
        ///PARAMETROS           : 
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 28/Diciembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*****************************************************************************************************************
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip.Trim().Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
    }

        ///*****************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Generar_Click
        ///DESCRIPCIÓN          : Ejecuta la operacion de click en el boton de generar
        ///PARAMETROS           : 
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 28/Diciembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*****************************************************************************************************************
        protected void Btn_Generar_Click(object sender, EventArgs e)
        {
            Cls_Rpt_Com_Mensual_Orden_Compra_Negocio Rpt_Ordenes_Negocio = new Cls_Rpt_Com_Mensual_Orden_Compra_Negocio(); //Conexion con la capa de negocios
            DataTable Dt_Ordenes_Compras = new DataTable(); //Para almacenar los datos de las ordenes de compras
            Div_Contenedor_Msj_Error.Visible = false;
            Limpiar_Controles("Error");

            try 
            {
                if (Validar_Campos())
                {
                    Rpt_Ordenes_Negocio.P_Fecha_Inicio = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicio.Text.Trim()));
                    Rpt_Ordenes_Negocio.P_Fecha_Fin = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Fin.Text.Trim()));
                    if(Cmb_Estatus.SelectedIndex > 0){
                        Rpt_Ordenes_Negocio.P_Estatus = Cmb_Estatus.SelectedValue.Trim();
                    }
                    Dt_Ordenes_Compras = Rpt_Ordenes_Negocio.Consultar_Ordenes_Compra();

                    if (Dt_Ordenes_Compras != null)
                    {
                        if (Dt_Ordenes_Compras.Rows.Count > 0)
                        {

                            foreach (DataColumn Columna in Dt_Ordenes_Compras.Columns)
                            {
                                if (Columna.ColumnName.Equals("UNIDAD_RESPONSABLE"))
                                {
                                    Columna.ColumnName = "UNIDAD RESPONSABLE";
                                }
                                if (Columna.ColumnName.Equals("FUENTE_FINANCIAMIENTO"))
                                {
                                    Columna.ColumnName = "FUENTE FINANCIAMIENTO";
                                }
                                if (Columna.ColumnName.Equals("PARTIDA_ESPECIFICA"))
                                {
                                    Columna.ColumnName = "PARTIDA ESPECIFICA";
                                }
                                if (Columna.ColumnName.Equals("FECHA_CREO"))
                                {
                                    Columna.ColumnName = "FECHA CREO";
                                }
                                if (Columna.ColumnName.Equals("NOMBRE_PROVEEDOR"))
                                {
                                    Columna.ColumnName = "NOMBRE PROVEEDOR";
                                }
                            }

                            foreach (DataRow Dr in Dt_Ordenes_Compras.Rows)
                            {
                                Dr["TOTAL"] = String.Format("{0:c}", Dr["TOTALES"]);
                            }
                            Dt_Ordenes_Compras.Columns.Remove("TOTALES");
                            Generar_Rpt_Mensual_Ordenes_Compras(Dt_Ordenes_Compras);
                        }
                        else {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('No hay datos registrados con el rango de fechas y el estatus.');", true);
                        }
                    }
                    else {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('No hay datos registrados con el rango de fechas y el estatus.');", true);
                    }
                }
                else 
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error en el evento del boton de generar el reporte. Error[" + ex.Message + "]");
            }
        }
    #endregion
}