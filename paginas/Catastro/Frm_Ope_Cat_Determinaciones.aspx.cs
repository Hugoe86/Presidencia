using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Operacion_Cat_Asignacion_Cuentas.Negocio;
using System.Data;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Tipos_Predio.Negocio;
using Presidencia.Constantes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Operacion.Predial_Tasas_Anuales.Negocio;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;

public partial class paginas_Catastro_Frm_Ope_Cat_Determinaciones : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Formulario(false);
                Llenar_Entregas(0);
            }
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Configuracion_Formulario(true);
            Llenar_Entregas(0);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Entregas.DataSource = null;
            Grid_Entregas.DataBind();
            Limpiar_Formulario();
        }
    }
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Entregas(0);
    }
    protected void Grid_Entregas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Entregas(e.NewPageIndex);
    }
    protected void Grid_Entregas_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Formulario
    ///DESCRIPCIÓN: Limpia todos los campos del formulario
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Limpiar_Formulario()
    {
        Txt_Entregas_Estrategicas.Text = "0";
        Txt_Entregas_Municipio.Text = "0";
        Txt_Entregas_Rusticas.Text = "0";
        Txt_Entregas_Urbanas.Text = "0";
        Txt_Anio.Text = DateTime.Now.Year.ToString();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Formulario
    ///DESCRIPCIÓN: Limpia todos los campos del formulario
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Configuracion_Formulario(Boolean Enabled)
    {
        Cmb_Entrega.Enabled = !Enabled;
        Txt_Anio.Enabled = !Enabled;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Establece la configuración del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Entregas(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas = new Cls_Ope_Cat_Asignacion_Cuentas_Negocio();
            DataTable Dt_Cuentas_Asignadas;

            //if (Cmb_Busqueda.SelectedValue == "COLONIA")
            //{
            //    Cuentas.P_Colonia = Txt_Busqueda.Text.ToUpper();
            //}
            //else if (Cmb_Busqueda.SelectedValue == "CALLE")
            //{
            //    Cuentas.P_Calle = Txt_Busqueda.Text.ToUpper();
            //}
            //else if (Cmb_Busqueda.SelectedValue == "PERITO")
            //{
            //    Cuentas.P_Perito = Txt_Busqueda.Text.ToUpper();
            //}
            //else if (Cmb_Busqueda.SelectedValue == "CUENTA_PREDIAL")
            //{
            //    Cuentas.P_Cuenta_Predial = Txt_Busqueda.Text.ToUpper();
            //}
            if (Txt_Anio.Text.Trim() != "")
            {
                Cuentas.P_Anio = Txt_Anio.Text.Trim();
            }
            else
            {
                Txt_Anio.Text = DateTime.Now.Year.ToString();
                Cuentas.P_Anio = DateTime.Now.Year.ToString();
            }
            Cuentas.P_No_Entrega = Cmb_Entrega.SelectedValue;
            Dt_Cuentas_Asignadas = Cuentas.Consultar_Cuentas_Entregar();
            Grid_Entregas.Columns[1].Visible = true;
            Grid_Entregas.Columns[3].Visible = true;
            Grid_Entregas.DataSource = Dt_Cuentas_Asignadas;
            Grid_Entregas.PageIndex = Pagina;
            Grid_Entregas.DataBind();
            Grid_Entregas.Columns[1].Visible = false;
            Grid_Entregas.Columns[3].Visible = false;
            Calcular_Totales_Entrega(Dt_Cuentas_Asignadas);
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    protected void Cmb_Entrega_SelectedIndexChanged(object sender, EventArgs e)
    {
        Llenar_Entregas(0);
    }
    protected void Txt_Anio_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Anio.Text.Trim() == "")
            {
                Txt_Anio.Text = DateTime.Now.Year.ToString();
            }
            else
            {
                Txt_Anio.Text = Txt_Anio.Text.Trim();
            }
            Llenar_Entregas(0);
        }
        catch
        {

        }
    }

    private void Calcular_Totales_Entrega(DataTable Dt_Entrega)
    {
        Double Total_Urbano = 0;
        Double Total_Rustico = 0;
        Double Total_Municipio = 0;
        Double Total_Estrategico = 0;
        foreach (DataRow Dr_Renglon in Dt_Entrega.Rows)
        {
            if (Convert.ToDouble(Dr_Renglon["PORCENTAJE_EXENCION"].ToString()) == 100)
            {
                Total_Municipio++;
            }
            else if (Convert.ToDouble(Dr_Renglon["SUPERFICIE_TOTAL"].ToString()) >= 2000)
            {
                Total_Estrategico++;
            }
            else if (Dr_Renglon["FOLIO_CATASTRO_TIPO"].ToString() == "AU")
            {
                Total_Urbano++;
            }
            else if (Dr_Renglon["FOLIO_CATASTRO_TIPO"].ToString() == "AR")
            {
                Total_Rustico++;
            }

        }
        Txt_Entregas_Estrategicas.Text = Total_Estrategico.ToString("###,###,###,###,###,##0");
        Txt_Entregas_Municipio.Text = Total_Municipio.ToString("###,###,###,###,###,##0");
        Txt_Entregas_Urbanas.Text = Total_Urbano.ToString("###,###,###,###,###,##0");
        Txt_Entregas_Rusticas.Text = Total_Rustico.ToString("###,###,###,###,###,##0");

    }

    protected void Btn_Imprime_Reporte_Click(object sender, ImageClickEventArgs e)
    {
        Imprimir_Reporte(Crear_Ds_Entregas(), "Rpt_Ope_Cat_Determinaciones.rpt", "Determinaciones", "Window_Frm", "Determinaciones");
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Avaluo_Urbano
    ///DESCRIPCIÓN          : Crea un DataSet para el reporte del avalúo urbano
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Junio/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Entregas()
    {
        Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas = new Cls_Ope_Cat_Asignacion_Cuentas_Negocio();
        Cls_Ope_Pre_Tasas_Anuales_Negocio Tasas = new Cls_Ope_Pre_Tasas_Anuales_Negocio();
        Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuotas = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
        Double Rustico = 0;
        Double Urbano_Edificado = 0;
        Double Urbano_Sin_Edificar = 0;
        Double Tasa = 0;
        Double Bimestre = 0;
        Double Anual = 0;
        if (Txt_Anio.Text.Trim() != "")
        {
            Cuentas.P_Anio = Txt_Anio.Text.Trim();
            Tasas.P_Anio = Txt_Anio.Text.Trim();
        }
        else
        {
            Txt_Anio.Text = DateTime.Now.Year.ToString();
            Cuentas.P_Anio = DateTime.Now.Year.ToString();
            Tasas.P_Anio = DateTime.Now.Year.ToString();
        }
        Cuentas.P_No_Entrega = Cmb_Entrega.SelectedValue;
        //
        DataTable Dt_Determinacion = Cuentas.Consultar_Cuentas_Determinaciones();
        Dt_Determinacion.TableName = "DT_DETALLES_CUENTA";
        DataTable Dt_Terreno = Cuentas.P_Dt_Terreno;
        Dt_Terreno.TableName = "DT_CALCULO_TERRENO";
        DataTable Dt_Construccion = Cuentas.P_Dt_Construccion;
        Dt_Construccion.TableName = "DT_CALCULO_CONSTRUCCION";
        DataTable Dt_Total = Cuentas.P_Dt_Totales;
        Dt_Total.TableName = "DT_TOTALES";
        Dt_Total.Columns.Add("TASA", typeof(Double));
        Dt_Total.Columns.Add("BIMESTRE", typeof(Double));
        Dt_Total.Columns.Add("ANUAL", typeof(Double));
        Ds_Ope_Cat_Determinaciones Ds_Det = new Ds_Ope_Cat_Determinaciones();
        DataTable Dt_Cuotas = Tasas.Consultar_Tasas_Anuales();
        Decimal Cuota_Minima = Cuotas.Consultar_Cuota_Minima_Anio(DateTime.Now.Year.ToString());

        foreach (DataRow Dr_Renglon in Dt_Cuotas.Rows)
        {
            if (Dr_Renglon[Cat_Pre_Tasas_Predial.Campo_Identificador].ToString() == "UR")
            {
                Urbano_Edificado = Convert.ToDouble(Dr_Renglon[Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual].ToString());
            }
            else if (Dr_Renglon[Cat_Pre_Tasas_Predial.Campo_Identificador].ToString() == "USE")
            {
                Urbano_Sin_Edificar = Convert.ToDouble(Dr_Renglon[Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual].ToString());
            }
            else if (Dr_Renglon[Cat_Pre_Tasas_Predial.Campo_Identificador].ToString().Trim() == "R")
            {
                Rustico = Convert.ToDouble(Dr_Renglon[Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual].ToString());
            }
        }


        foreach (DataRow Dr_Renglon in Dt_Determinacion.Rows)
        {
            if (Dr_Renglon[Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString().Trim() == "SI")
            {
                if (Dr_Renglon["NOMBRE_CALLE_NOTIFICACION"].ToString().Trim() == "")
                {
                    Dr_Renglon["NOMBRE_CALLE_NOTIFICACION"] = Dr_Renglon[Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion].ToString();
                }
                if (Dr_Renglon["NOMBRE_COLONIA_NOTIFICACION"].ToString().Trim() == "")
                {
                    Dr_Renglon["NOMBRE_COLONIA_NOTIFICACION"] = Dr_Renglon[Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion].ToString();
                }
            }
        }
        foreach (DataRow Dr_Renglon in Dt_Total.Rows)
        {
            if (Dr_Renglon["FOLIO_CATASTRO_TIPO"].ToString() == "RUSTICO")
            {
                Dr_Renglon["TASA"] = Rustico;
                if (Convert.ToDouble(Dr_Renglon[Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString()) == 100)
                {
                    Dr_Renglon["BIMESTRE"] = 0;
                    Dr_Renglon["ANUAL"] = 0;
                }
                else
                {
                    if ((Convert.ToDecimal(Dr_Renglon["VALOR_PREDIO"].ToString()) * (Convert.ToDecimal(Rustico) / 1000)) < Cuota_Minima)
                    {
                        Dr_Renglon["BIMESTRE"] = (Convert.ToDouble(Cuota_Minima)) / 6;
                        Dr_Renglon["ANUAL"] = Convert.ToDouble(Cuota_Minima);
                    }
                    else
                    {
                        Dr_Renglon["BIMESTRE"] = (Convert.ToDouble(Dr_Renglon["VALOR_PREDIO"].ToString()) * (Rustico / 1000)) / 6;
                        Dr_Renglon["ANUAL"] = Convert.ToDouble(Dr_Renglon["VALOR_PREDIO"].ToString()) * (Rustico / 1000);
                    }
                }
            }
            else
            {
                if (Convert.ToDouble(Dr_Renglon["CALC_CONSTRUCCION"].ToString()) > 0)
                {
                    Dr_Renglon["TASA"] = Urbano_Edificado;
                    if (Convert.ToDouble(Dr_Renglon[Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString()) == 100)
                    {
                        Dr_Renglon["BIMESTRE"] = 0;
                        Dr_Renglon["ANUAL"] = 0;
                    }
                    else
                    {
                        if (((Convert.ToDecimal(Dr_Renglon["VALOR_PREDIO"].ToString()) * (Convert.ToDecimal(Urbano_Edificado) / 1000)) / 6) < Cuota_Minima)
                        {
                            Dr_Renglon["BIMESTRE"] = (Convert.ToDouble(Cuota_Minima)) / 6;
                            Dr_Renglon["ANUAL"] = Convert.ToDouble(Cuota_Minima);
                        }
                        else
                        {
                            Dr_Renglon["BIMESTRE"] = (Convert.ToDouble(Dr_Renglon["VALOR_PREDIO"].ToString()) * (Urbano_Edificado / 1000)) / 6;
                            Dr_Renglon["ANUAL"] = Convert.ToDouble(Dr_Renglon["VALOR_PREDIO"].ToString()) * (Urbano_Edificado / 1000);
                        }
                    }
                }
                else
                {
                    Dr_Renglon["TASA"] = Urbano_Sin_Edificar;
                    if (Convert.ToDouble(Dr_Renglon[Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString()) == 100)
                    {
                        Dr_Renglon["BIMESTRE"] = 0;
                        Dr_Renglon["ANUAL"] = 0;
                    }
                    else
                    {
                        if (((Convert.ToDecimal(Dr_Renglon["VALOR_PREDIO"].ToString()) * (Convert.ToDecimal(Urbano_Sin_Edificar) / 1000)) / 6) < Cuota_Minima)
                        {
                            Dr_Renglon["BIMESTRE"] = (Convert.ToDouble(Cuota_Minima)) / 6;
                            Dr_Renglon["ANUAL"] = Convert.ToDouble(Cuota_Minima);
                        }
                        else
                        {
                            Dr_Renglon["BIMESTRE"] = (Convert.ToDouble(Dr_Renglon["VALOR_PREDIO"].ToString()) * (Urbano_Sin_Edificar / 1000)) / 6;
                            Dr_Renglon["ANUAL"] = Convert.ToDouble(Dr_Renglon["VALOR_PREDIO"].ToString()) * (Urbano_Sin_Edificar / 1000);
                        }
                    }
                }
            }
        }






        Ds_Det.Tables.Remove("DT_DETALLES_CUENTA");
        Ds_Det.Tables.Remove("DT_CALCULO_TERRENO");
        Ds_Det.Tables.Remove("DT_CALCULO_CONSTRUCCION");
        Ds_Det.Tables.Remove("DT_TOTALES");
        Dt_Determinacion.DefaultView.Sort = "FOLIO_CATASTRO_TIPO ASC, FOLIO_CATASTRO ASC";
        Ds_Det.Tables.Add(Dt_Determinacion.Copy());
        Dt_Construccion.DefaultView.Sort = "FOLIO_CATASTRO_TIPO ASC, FOLIO_CATASTRO ASC";
        Ds_Det.Tables.Add(Dt_Construccion.Copy());
        Dt_Terreno.DefaultView.Sort = "FOLIO_CATASTRO_TIPO ASC, FOLIO_CATASTRO ASC";
        Ds_Det.Tables.Add(Dt_Terreno.Copy());
        Dt_Total.DefaultView.Sort = "FOLIO_CATASTRO_TIPO ASC, FOLIO_CATASTRO ASC";
        Ds_Det.Tables.Add(Dt_Total.Copy());
        //Imprimir_Reporte(Ds_Det, "Rpt_Ope_Cat_Determinaciones.rpt", "Determinaciones", "Window_Frm", "Entrega");
        return Ds_Det;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Convenios, String Nombre_Reporte, String Nombre_Archivo, String Formato, String Tipo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Catastro/" + Nombre_Reporte);
        try
        {
            Reporte.Load(File_Path);
            Reporte.SetDataSource(Ds_Convenios);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }

        String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar    
        try
        {
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options);
        }
        catch (Exception Ex)
        {
            //Lbl_Mensaje_Error.Visible = true;
            //Lbl_Mensaje_Error.Text = "No se pudo exportar el reporte";
        }

        try
        {
            Mostrar_Reporte(Archivo_PDF, Tipo, Formato);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Reporte
    ///DESCRIPCIÓN          : Visualiza en pantalla el reporte indicado
    ///PARAMETROS           : Nombre_Reporte: cadena con el nombre del archivo.
    ///                     : Formato: Exensión del archivo a visualizar.
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Mostrar_Reporte(String Nombre_Reporte, String Formato, String Frm_Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            //if (Formato == "PDF")
            //{
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), Frm_Formato, "window.open('" + Pagina + "', '" + Formato + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            //}
            //else if (Formato == "Excel")
            //{
            //    String Ruta = "../../Reporte/" + Nombre_Reporte;
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            //}
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }
}
