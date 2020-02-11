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
//using Presidencia.Reporte_Mensual_Ordenes_Compra.Negocio;
using CarlosAg.ExcelXmlWriter;
using System.Text;
using Presidencia.Generar_Requisicion.Negocio;


public partial class paginas_Compras_Frm_Rpt_Com_Mensual_Requisiciones : System.Web.UI.Page
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
                    Cmb_Tipo_Reporte.Items.Clear();
                    Cmb_Tipo_Reporte.Items.Add("SEGUIMIENTO");
                    Cmb_Tipo_Reporte.Items.Add("TRANSITORIA");
                    Cmb_Tipo_Reporte.Items.Add("STOCK");
                    Cmb_Tipo_Reporte.Items[0].Selected = true;
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
                //Llenar_Combo_Estatus();
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
            //Cls_Rpt_Com_Mensual_Orden_Compra_Negocio Rpt_Ordenes_Negocio = new Cls_Rpt_Com_Mensual_Orden_Compra_Negocio(); //Conexion con la capa de negocios
            //DataTable Dt_Estatus = new DataTable(); //Para almacenar los datos de los estatus de las ordenes de compras
            
            //try 
            //{
            //    Dt_Estatus = Rpt_Ordenes_Negocio.Consultar_Estatus();

            //    Cmb_Estatus.Items.Clear();
            //    Cmb_Estatus.DataValueField = "ESTATUS";
            //    Cmb_Estatus.DataTextField = "ESTATUS";
            //    Cmb_Estatus.DataSource = Dt_Estatus;
            //    Cmb_Estatus.DataBind();
            //    Cmb_Estatus.Items.Insert(0, new ListItem("<- SELECCIONE ->", ""));
            //}
            //catch(Exception Ex)
            //{
            //    throw new Exception("Error al llenar el combo de estatus: Error[" + Ex.Message + "]");
            //}
        }

        ///*****************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Generar_Rpt_Seguimiento_Requisiciones
        ///DESCRIPCIÓN          : Metodo generar el reporte mensual  de las ordenes de compras
        ///PARAMETROS           1 Dt_Ordenes: ordenes de compras que se pasaran al reporte 
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 29/Diciembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*****************************************************************************************************************
        public void Generar_Rpt_Seguimiento_Requisiciones(DataTable Dt_Ordenes) 
        {
            String Ruta_Archivo = "Reporte_Requisiciones.xls";
            try
            {
                CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();
                Libro.Properties.Title = "Reporte Requisiciones";
                Libro.Properties.Created = DateTime.Now;
                Libro.Properties.Author = "SIAG";

                //Creamos una hoja que tendrá el libro.
                CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Reporte Requisiciones");
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

                if (Cmb_Tipo_Reporte.SelectedValue == "SEGUIMIENTO")
                {
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//Folio.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//Fecha Creo.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(230));//Fecha Creo.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(230));//Estatus.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(230));//Unidad Responsable.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(230));//Fuente Financiamiento.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(230));//Partida Especifica.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(140));//Concepto.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(140));//Cotizador.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(140));//Nombre Proveedor.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(140));//Total.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(230));//Observaciones.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(230));//Observaciones.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(230));//Observaciones.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(230));//Observaciones.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(140));//Observaciones.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(230));//Observaciones.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(230));//Observaciones.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(230));//Observaciones.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(230));//Observaciones.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(230));//Observaciones.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(350));//Observaciones.

                }
                else if (Cmb_Tipo_Reporte.SelectedValue == "TRANSITORIA")
                {
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//NO_REQ
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//NO_ORDEN_COMPRA
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//TOTALES
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(300));//TOTAL
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(300));//UR
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//Partida Especifica.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//Concepto.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//estatus
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//fecha_creo
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//fecha_Autorizacio
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//Fecha_Entrega_MAteria
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//Fecha_Contrarecibo
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//FEcha_PAgo
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//Fecha_Surtida
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//Salida
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//Fecha_Salida
                }
                else if (Cmb_Tipo_Reporte.SelectedValue == "STOCK")
                {
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//Folio.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//Fecha Creo.
                    //Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//Estatus.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(300));//Unidad Responsable.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(300));//Fuente Financiamiento.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//Partida Especifica.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//Concepto.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//Cotizador.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//Nombre Proveedor.
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(130));//Nombre Proveedor.
                }
                //Agregamos las columnas que tendrá la hoja de excel.
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
            //Cls_Rpt_Com_Mensual_Orden_Compra_Negocio Rpt_Ordenes_Negocio = new Cls_Rpt_Com_Mensual_Orden_Compra_Negocio(); //Conexion con la capa de negocios
            Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio = new Cls_Ope_Com_Requisiciones_Negocio();          
            DataTable Dt_Ordenes_Compras = new DataTable(); //Para almacenar los datos de las ordenes de compras
            Div_Contenedor_Msj_Error.Visible = false;
            Limpiar_Controles("Error");

            try 
            {
                if (Validar_Campos())
                {
                    Requisicion_Negocio.P_Fecha_Inicial = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicio.Text.Trim()));
                    Requisicion_Negocio.P_Fecha_Final = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Fin.Text.Trim()));
                    if(Cmb_Estatus.SelectedIndex > 0){
                        Requisicion_Negocio.P_Estatus = Cmb_Estatus.SelectedValue.Trim();
                    }

                    if (Cmb_Tipo_Reporte.SelectedValue == "SEGUIMIENTO")
                    {
                        Dt_Ordenes_Compras = Requisicion_Negocio.Consultar_Requisiciones_Reporte_Gerencial();
                    }
                    else if (Cmb_Tipo_Reporte.SelectedValue == "TRANSITORIA")
                    {
                        Dt_Ordenes_Compras = Requisicion_Negocio.Consultar_Requisiciones_Entrega_Bienes_Transitorios();
                    }
                    else if (Cmb_Tipo_Reporte.SelectedValue == "STOCK")
                    {
                        Dt_Ordenes_Compras = Requisicion_Negocio.Consultar_Requisiciones_Entrega_Stock();
                    }
                    //Dt_Ordenes_Compras = Requisicion_Negocio.Consultar_Requisiciones_Reporte_Gerencial();
                    //Dt_Ordenes_Compras = Requisicion_Negocio.Consultar_Requisiciones_Entrega_Stock();

                    if (Dt_Ordenes_Compras != null)
                    {
                        if (Dt_Ordenes_Compras.Rows.Count > 0)
                        {
                            foreach (DataRow Dr in Dt_Ordenes_Compras.Rows)
                            {
                                Dr["TOTAL"] = String.Format("{0:c}", Dr["TOTALES"]);
                            }
                            Dt_Ordenes_Compras.Columns.Remove("TOTALES");
                            Generar_Rpt_Seguimiento_Requisiciones(Dt_Ordenes_Compras);
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
        protected void Cmb_Tipo_Reporte_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
}