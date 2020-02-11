using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Operacion_Cat_Asignacion_Cuentas.Negocio;
using System.Data;
using Presidencia.Sessiones;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Operacion.Predial_Tasas_Anuales.Negocio;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using Presidencia.Constantes;

public partial class paginas_Catastro_Frm_Rpt_Cat_Reportes_Pre_Estrategicos : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN:
    ///PROPIEDADES:     
    ///            
    ///CREO: David Herrera Rincon
    ///FECHA_CREO: 22/Octubre/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Formulario(true);                
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Establece la configuración del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: David Herrera Rincon
    ///FECHA_CREO: 22/Octubre/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Enabled)
    {
        Txt_Fecha_Inicio.Enabled = false;
        Txt_Fecha_Fin.Enabled = false;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Predios_Estrategicos
    ///DESCRIPCIÓN          : Crea un DataSet para el reporte de predios estrategiscos
    ///PARAMETROS:          : DataTable que trae registros 
    ///CREO                 : David Herrera rincon
    ///FECHA_CREO           : 24/Octubre/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Predios_Estrategicos(DataTable Dt_Temp)
    {
        Cls_Ope_Pre_Tasas_Anuales_Negocio Tasas = new Cls_Ope_Pre_Tasas_Anuales_Negocio();
        Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuotas = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
        Ds_Ope_Cat_Predios_Estrategicos Ds_Pre_Estrategicos = new Ds_Ope_Cat_Predios_Estrategicos();
        DataTable Dt_Baja = Ds_Pre_Estrategicos.Tables["Dt_Predios_Estrategicos"].Copy();
        Tasas.P_Anio = DateTime.Now.Year.ToString();
        DataTable Dt_Cuotas = Tasas.Consultar_Tasas_Anuales();
        Decimal Cuota_Minima = Cuotas.Consultar_Cuota_Minima_Anio(DateTime.Now.Year.ToString());
        Double Rustico = 0;
        Double Urbano_Edificado = 0;
        Double Urbano_Sin_Edificar = 0;
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
        foreach (DataRow Dr_Renglon in Dt_Temp.Rows)
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
        foreach (DataRow Dr_Temp in Dt_Temp.Rows)
        {
            DataRow Dr_Fila = Ds_Pre_Estrategicos.Tables["Dt_Predios_Estrategicos"].NewRow();
            Dr_Fila["No"] = Dr_Temp["FOLIO_PREDIAL"].ToString();
            Dr_Fila["Cuenta"] = Dr_Temp["CUENTA_PREDIAL"].ToString();
            Dr_Fila["Tipo"] = Dr_Temp["FOLIO_CATASTRO_TIPO"].ToString();
            Dr_Fila["Terreno"] = Dr_Temp["SUPERFICIE_TOTAL"].ToString();
            Dr_Fila["Const"] = Dr_Temp["SUPERFICIE_CONSTRUIDA"].ToString();
            Dr_Fila["Valor_Fiscal"] = Dr_Temp["VALOR_FISCAL"].ToString();
            Dr_Fila["Cuota_Anterior"] = Dr_Temp["CUOTA_ANUAL"].ToString();
            if (Convert.ToDouble(Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString()) == 100)
            {
                Dr_Fila["Cuota_Nueva"] =0;
                Dr_Fila["Diferencia"] = Dr_Temp["DIFERENCIA"].ToString();
            }
            else
            {
                if (Dr_Temp["FOLIO_CATASTRO_TIPO"].ToString() == "AR")
                {
                    if ((Convert.ToDecimal(Dr_Temp["VALOR_PREDIO"].ToString()) * (Convert.ToDecimal(Rustico) / 1000)) < Cuota_Minima)
                    {
                        Dr_Fila["Cuota_Nueva"] = Cuota_Minima;
                        Dr_Fila["Diferencia"] = Convert.ToDouble(Dr_Temp["CUOTA_ANUAL"].ToString()) - Convert.ToDouble(Cuota_Minima);
                    }
                    else
                    {
                        Dr_Fila["Cuota_Nueva"] = (Convert.ToDecimal(Dr_Temp["VALOR_PREDIO"].ToString()) * (Convert.ToDecimal(Rustico) / 1000));
                        Dr_Fila["Diferencia"] = Convert.ToDouble(Dr_Temp["CUOTA_ANUAL"].ToString()) - (Convert.ToDouble(Dr_Temp["VALOR_PREDIO"].ToString()) * (Convert.ToDouble(Rustico) / 1000));
                    }
                }
                else if (Dr_Temp["FOLIO_CATASTRO_TIPO"].ToString() == "AU")
                {
                    if (Convert.ToDouble(Dr_Temp["CALC_CONSTRUCCION"].ToString()) > 0)
                    {
                        if ((Convert.ToDecimal(Dr_Temp["VALOR_PREDIO"].ToString()) * (Convert.ToDecimal(Urbano_Edificado) / 1000)) < Cuota_Minima)
                        {
                            Dr_Fila["Cuota_Nueva"] = Cuota_Minima;
                            Dr_Fila["Diferencia"] = Convert.ToDouble(Dr_Temp["CUOTA_ANUAL"].ToString()) - Convert.ToDouble(Cuota_Minima);
                        }
                        else
                        {
                            Dr_Fila["Cuota_Nueva"] = (Convert.ToDecimal(Dr_Temp["VALOR_PREDIO"].ToString()) * (Convert.ToDecimal(Urbano_Edificado) / 1000));
                            Dr_Fila["Diferencia"] = Convert.ToDouble(Dr_Temp["CUOTA_ANUAL"].ToString()) - (Convert.ToDouble(Dr_Temp["VALOR_PREDIO"].ToString()) * (Convert.ToDouble(Urbano_Edificado) / 1000));
                        }
                    }
                    else
                    {
                        if ((Convert.ToDecimal(Dr_Temp["VALOR_PREDIO"].ToString()) * (Convert.ToDecimal(Urbano_Sin_Edificar) / 1000)) < Cuota_Minima)
                        {
                            Dr_Fila["Cuota_Nueva"] = Cuota_Minima;
                            Dr_Fila["Diferencia"] = Convert.ToDouble(Dr_Temp["CUOTA_ANUAL"].ToString()) - Convert.ToDouble(Cuota_Minima);
                        }
                        else
                        {
                            Dr_Fila["Cuota_Nueva"] = (Convert.ToDecimal(Dr_Temp["VALOR_PREDIO"].ToString()) * (Convert.ToDecimal(Urbano_Sin_Edificar) / 1000));
                            Dr_Fila["Diferencia"] = Convert.ToDouble(Dr_Temp["CUOTA_ANUAL"].ToString()) - (Convert.ToDouble(Dr_Temp["VALOR_PREDIO"].ToString()) * (Convert.ToDouble(Urbano_Sin_Edificar) / 1000));
                        }
                    }
                }
            }
            Dr_Fila["Propietario"] = Dr_Temp["NOMBRE_PROPIETARIO"].ToString();
            Dr_Fila["Calle"] = Dr_Temp["NOMBRE_CALLE"].ToString();
            Dr_Fila["Colonia"] = Dr_Temp["NOMBRE_COLONIA"].ToString();
            Ds_Pre_Estrategicos.Tables["Dt_Predios_Estrategicos"].Rows.Add(Dr_Fila);
            Ds_Pre_Estrategicos.Tables["Dt_Predios_Estrategicos"].AcceptChanges();
        }
        if (Cmb_Tipo_Reporte.SelectedValue == "BAJA")
        {
            DataTable Dt_Predios = Ds_Pre_Estrategicos.Tables["Dt_Predios_Estrategicos"];
            DataRow Dr_Renglon_Nuevo;
            foreach (DataRow Dr_Renglon in Dt_Predios.Rows)
            {
                if (Convert.ToDouble(Dr_Renglon["Diferencia"].ToString()) > 0)
                {
                    Dr_Renglon_Nuevo = Dt_Baja.NewRow();
                    Dr_Renglon_Nuevo["No"] = Dr_Renglon["No"];
                    Dr_Renglon_Nuevo["Cuenta"] = Dr_Renglon["Cuenta"];
                    Dr_Renglon_Nuevo["Tipo"] = Dr_Renglon["Tipo"];
                    Dr_Renglon_Nuevo["Terreno"] = Dr_Renglon["Terreno"];
                    Dr_Renglon_Nuevo["Const"] = Dr_Renglon["Const"];
                    Dr_Renglon_Nuevo["Valor_Fiscal"] = Dr_Renglon["Valor_Fiscal"];
                    Dr_Renglon_Nuevo["Cuota_Anterior"] = Dr_Renglon["Cuota_Anterior"];
                    Dr_Renglon_Nuevo["Cuota_Nueva"] = Dr_Renglon["Cuota_Nueva"];
                    Dr_Renglon_Nuevo["Diferencia"] = Dr_Renglon["Diferencia"];
                    Dr_Renglon_Nuevo["Propietario"] = Dr_Renglon["Propietario"];
                    Dr_Renglon_Nuevo["Calle"] = Dr_Renglon["Calle"];
                    Dr_Renglon_Nuevo["Colonia"] = Dr_Renglon["Colonia"];
                    Dt_Baja.Rows.Add(Dr_Renglon_Nuevo);
                }
            }
            Dt_Baja.TableName = "Dt_Predios_Estrategicos";
            Ds_Pre_Estrategicos.Tables.Remove("Dt_Predios_Estrategicos");
            Ds_Pre_Estrategicos.Tables.Add(Dt_Baja);

        }
        return Ds_Pre_Estrategicos;
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
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), Frm_Formato, "window.open('" + Pagina + "', '" + Formato + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del botón salir
    ///PROPIEDADES:     
    ///            
    ///CREO: David Herrera Rincon
    ///FECHA_CREO: 22/Octubre/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Configuracion_Formulario(true);
            Btn_Exportar_Predios.Visible = true;
            Btn_Exportar_Predios.AlternateText = "Reporte Predios Estrategicos";
            Btn_Exportar_Predios.ImageUrl = "~/paginas/imagenes/gridview/pdf.png";
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";            
        }
    }    

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Exportar_Solicitudes_Click
    ///DESCRIPCIÓN: Evento del botón Exportar a pdf
    ///PROPIEDADES:     
    ///            
    ///CREO: David Herrera Rincon
    ///FECHA_CREO: 23/Oct/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Exportar_Predios_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuenta = new Cls_Ope_Cat_Asignacion_Cuentas_Negocio();//Declaracion de la clase de negocios
        Cuenta.P_Tipo_Reporte = Cmb_Tipo_Reporte.SelectedValue;
        if (Cmb_Tipo_Reporte.SelectedValue == "TCAA")
        {
            if (Txt_Anio.Text != "")
            {
                Imprimir_Reporte(Crear_Ds_Cuentas_Asiganadas_Atendidas(), "Rpt_Cat_Cuentas_Asignadas_Atendidas.rpt", "Reporte_Cuentas_Asignadas_Atendidas", "Window_Frm", "Avaluo");
                Txt_Anio.Text = "";
            }

            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes Cuentas Asignadas-Atendidas", "alert('Introduzca el año a imprimir.');", true);
            }
        }
        else if (Cmb_Tipo_Reporte.SelectedValue == "VR")
        {
            Imprimir_Reporte(Crear_Ds_Ope_Cat_Correcciones(), "Rpt_Ope_Cat_Correcciones.rpt", "Reporte_Correcciones", "Window_Form", "Avaluo_Urbano");

        }
        else if (Cmb_Tipo_Reporte.SelectedValue == "AEF")
        {
            if (Txt_Anio.Text != "")
            {

                Imprimir_Reporte(Crear_Ds_Ejercicio_Fiscal(), "Rpt_Cat_Ejercicio_Fiscal.rpt", "Reporte_Predios_Estrategicos", "Window_Frm", "Avaluo_Urbano");
                Txt_Anio.Text = "";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes Ejercicio Fiscal", "alert('Introduzca el año a imprimir.');", true);
            }
        }
        else if (Cmb_Movimiento.SelectedValue == "ACV")
        {
            Cuenta.P_Con_Movimiento = true;
        }
        else
        {
            DataTable Dt_Temp = Cuenta.Consultar_Predios_Estrategicos();//Realizamos la consulta            
            //Si tiene datos, mostramos el reporte
            if (Dt_Temp.Rows.Count > 0)
                Imprimir_Reporte(Crear_Ds_Predios_Estrategicos(Dt_Temp), "Rpt_Ope_Cat_Predios_Estrategicos.rpt", "Reporte_Predios_Estrategicos", "Window_Frm", "Avaluo_Urbano");
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes Predios Estrategicos", "alert('No se encontraron registros.');", true);
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Crear_Ds_Ope_Cat_Correcciones
    ///DESCRIPCIÓN: Se crea el dataset con las consultas de cada metodo generando automaticamente
    ///PROPIEDADES:     
    ///            
    ///CREO: David Herrera Rincon
    ///FECHA_CREO: 23/Oct/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private DataSet Crear_Ds_Ope_Cat_Correcciones()
    {
        Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas = new Cls_Ope_Cat_Asignacion_Cuentas_Negocio();
        Ds_Ope_Cat_Correcciones Ds_Correcciones = new Ds_Ope_Cat_Correcciones();

        DataTable Dt_1_Entrega = Cuentas.Consultar_Veces_Rechazo_1();// Ds_Correcciones.Tables["DT_NOMBRE"].Copy();
        DataTable Dt_2_Entrega = Cuentas.Consultar_Veces_Rechazo_2();// Ds_Correcciones.Tables["DT_NOMBRE"].Copy();
        DataTable Dt_3_Entrega = Cuentas.Consultar_Veces_Rechazo_3();
        DataTable Dt_4_Entrega = Cuentas.Consultar_Veces_Rechazo_4();
        DataTable Dt_5_Entrega = Cuentas.Consultar_Veces_Rechazo_5();
        DataTable Dt_6_Entrega = Cuentas.Consultar_Veces_Rechazo_6();
        DataTable Dt_7_Entrega = Cuentas.Consultar_Veces_Rechazo_7();

        Dt_1_Entrega.TableName = "DT_1_ENTREGA";
        Dt_2_Entrega.TableName = "DT_2_ENTREGA";
        Dt_3_Entrega.TableName = "DT_3_ENTREGA";
        Dt_4_Entrega.TableName = "DT_4_ENTREGA";
        Dt_5_Entrega.TableName = "DT_5_ENTREGA";
        Dt_6_Entrega.TableName = "DT_6_ENTREGA";
        Dt_7_Entrega.TableName = "DT_7_ENTREGA";

        Ds_Correcciones.Tables.Remove("DT_1_ENTREGA");
        Ds_Correcciones.Tables.Remove("DT_2_ENTREGA");
        Ds_Correcciones.Tables.Remove("DT_3_ENTREGA");
        Ds_Correcciones.Tables.Remove("DT_4_ENTREGA");
        Ds_Correcciones.Tables.Remove("DT_5_ENTREGA");
        Ds_Correcciones.Tables.Remove("DT_6_ENTREGA");
        Ds_Correcciones.Tables.Remove("DT_7_ENTREGA");

        Ds_Correcciones.Tables.Add(Dt_1_Entrega.Copy());
        Ds_Correcciones.Tables.Add(Dt_2_Entrega.Copy());
        Ds_Correcciones.Tables.Add(Dt_3_Entrega.Copy());
        Ds_Correcciones.Tables.Add(Dt_4_Entrega.Copy());
        Ds_Correcciones.Tables.Add(Dt_5_Entrega.Copy());
        Ds_Correcciones.Tables.Add(Dt_6_Entrega.Copy());
        Ds_Correcciones.Tables.Add(Dt_7_Entrega.Copy());

        return Ds_Correcciones;
    }
	

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Avaluos_Realizados_Valuador
    ///DESCRIPCIÓN          : Crea un DataSet para el reporte del avalúo urbano
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Junio/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Ejercicio_Fiscal()
    {

        Ds_Rpt_Cat_Ejercicio_Fiscal Ds_Ejercicio_Fiscal = new Ds_Rpt_Cat_Ejercicio_Fiscal();

        Cls_Ope_Cat_Asignacion_Cuentas_Negocio Reporte = new Cls_Ope_Cat_Asignacion_Cuentas_Negocio();

        DataTable Dt_Ejercicio_Fiscal;
        DataTable Dt_Ejercicio_Fiscal_Temp;
        DataTable Dt_Ejercicio_Fiscal_Totales;
        Dt_Ejercicio_Fiscal = Ds_Ejercicio_Fiscal.Tables["DT_EJERCICIO_FISCAL_AV"];
        Reporte.P_Anio_Ejercicio_Fiscal = Txt_Anio.Text;
        Dt_Ejercicio_Fiscal = Reporte.Consultas_Ejercicio_Fiscal();
        Dt_Ejercicio_Fiscal_Totales = Ds_Ejercicio_Fiscal.Tables["DT_TOTALES_EJERCICIO_FISCAL"];
        DataColumn Dc_No_Entrega = Dt_Ejercicio_Fiscal.Columns.Add("NO_ENTREGA", typeof(Int16));
        DataColumn Dc_Porcentaje_Entregados = Dt_Ejercicio_Fiscal.Columns.Add("PORCENTAJE_ENTREGADOS", typeof(Double));
        DataColumn Dc_Diferencia = Dt_Ejercicio_Fiscal.Columns.Add("DIFERENCIA_AVALUOS", typeof(Int32));
        DataRow Dr_Renglon_Nuevo;
        Dt_Ejercicio_Fiscal_Temp = Reporte.Consultas_Ejercicio_Fiscal_Segunda_Entrega();

        foreach (DataRow Dr_Renglon_Actual in Dt_Ejercicio_Fiscal_Temp.Rows)
        {
            Dr_Renglon_Nuevo = Dt_Ejercicio_Fiscal.NewRow();
            Dr_Renglon_Nuevo["FECHA_ENTREGA"] = Dr_Renglon_Actual["FECHA_ENTREGA"];
            Dr_Renglon_Nuevo["FECHA_ENTREGA_REAL"] = Dr_Renglon_Actual["FECHA_ENTREGA_REAL"];
            Dr_Renglon_Nuevo["AVALUOS_ENTREGAR"] = Dr_Renglon_Actual["AVALUOS_ENTREGAR"];
            Dr_Renglon_Nuevo["AVALUOS_ENTREGADOS"] = Dr_Renglon_Actual["AVALUOS_ENTREGADOS"];
            Dt_Ejercicio_Fiscal.Rows.Add(Dr_Renglon_Nuevo);
        }
        Dt_Ejercicio_Fiscal_Temp = Reporte.Consultas_Ejercicio_Fiscal_Tercera_Entrega();

        foreach (DataRow Dr_Renglon_Actual in Dt_Ejercicio_Fiscal_Temp.Rows)
        {
            Dr_Renglon_Nuevo = Dt_Ejercicio_Fiscal.NewRow();
            Dr_Renglon_Nuevo["FECHA_ENTREGA"] = Dr_Renglon_Actual["FECHA_ENTREGA"];
            Dr_Renglon_Nuevo["FECHA_ENTREGA_REAL"] = Dr_Renglon_Actual["FECHA_ENTREGA_REAL"];
            Dr_Renglon_Nuevo["AVALUOS_ENTREGAR"] = Dr_Renglon_Actual["AVALUOS_ENTREGAR"];
            Dr_Renglon_Nuevo["AVALUOS_ENTREGADOS"] = Dr_Renglon_Actual["AVALUOS_ENTREGADOS"];
            Dt_Ejercicio_Fiscal.Rows.Add(Dr_Renglon_Nuevo);
        }
        Dt_Ejercicio_Fiscal_Temp = Reporte.Consultas_Ejercicio_Fiscal_Cuarta_Entrega();

        foreach (DataRow Dr_Renglon_Actual in Dt_Ejercicio_Fiscal_Temp.Rows)
        {

            Dr_Renglon_Nuevo = Dt_Ejercicio_Fiscal.NewRow();
            Dr_Renglon_Nuevo["FECHA_ENTREGA"] = Dr_Renglon_Actual["FECHA_ENTREGA"];
            Dr_Renglon_Nuevo["FECHA_ENTREGA_REAL"] = Dr_Renglon_Actual["FECHA_ENTREGA_REAL"];
            Dr_Renglon_Nuevo["AVALUOS_ENTREGAR"] = Dr_Renglon_Actual["AVALUOS_ENTREGAR"];
            Dr_Renglon_Nuevo["AVALUOS_ENTREGADOS"] = Dr_Renglon_Actual["AVALUOS_ENTREGADOS"];
            Dt_Ejercicio_Fiscal.Rows.Add(Dr_Renglon_Nuevo);
        }

        Dt_Ejercicio_Fiscal_Temp = Reporte.Consultas_Ejercicio_Fiscal_Quinta_Entrega();

        foreach (DataRow Dr_Renglon_Actual in Dt_Ejercicio_Fiscal_Temp.Rows)
        {

            Dr_Renglon_Nuevo = Dt_Ejercicio_Fiscal.NewRow();
            Dr_Renglon_Nuevo["FECHA_ENTREGA"] = Dr_Renglon_Actual["FECHA_ENTREGA"];
            Dr_Renglon_Nuevo["FECHA_ENTREGA_REAL"] = Dr_Renglon_Actual["FECHA_ENTREGA_REAL"];
            Dr_Renglon_Nuevo["AVALUOS_ENTREGAR"] = Dr_Renglon_Actual["AVALUOS_ENTREGAR"];
            Dr_Renglon_Nuevo["AVALUOS_ENTREGADOS"] = Dr_Renglon_Actual["AVALUOS_ENTREGADOS"];
            Dt_Ejercicio_Fiscal.Rows.Add(Dr_Renglon_Nuevo);
        }

        Dt_Ejercicio_Fiscal_Temp = Reporte.Consultas_Ejercicio_Fiscal_Sexta_Entrega();

        foreach (DataRow Dr_Renglon_Actual in Dt_Ejercicio_Fiscal_Temp.Rows)
        {
            Dr_Renglon_Nuevo = Dt_Ejercicio_Fiscal.NewRow();
            Dr_Renglon_Nuevo["FECHA_ENTREGA"] = Dr_Renglon_Actual["FECHA_ENTREGA"];
            Dr_Renglon_Nuevo["FECHA_ENTREGA_REAL"] = Dr_Renglon_Actual["FECHA_ENTREGA_REAL"];
            Dr_Renglon_Nuevo["AVALUOS_ENTREGAR"] = Dr_Renglon_Actual["AVALUOS_ENTREGAR"];
            Dr_Renglon_Nuevo["AVALUOS_ENTREGADOS"] = Dr_Renglon_Actual["AVALUOS_ENTREGADOS"];
            Dt_Ejercicio_Fiscal.Rows.Add(Dr_Renglon_Nuevo);
        }

        Dt_Ejercicio_Fiscal_Temp = Reporte.Consultas_Ejercicio_Fiscal_Septima_Entrega();

        foreach (DataRow Dr_Renglon_Actual in Dt_Ejercicio_Fiscal_Temp.Rows)
        {

            Dr_Renglon_Nuevo = Dt_Ejercicio_Fiscal.NewRow();
            Dr_Renglon_Nuevo["FECHA_ENTREGA"] = Dr_Renglon_Actual["FECHA_ENTREGA"];
            Dr_Renglon_Nuevo["FECHA_ENTREGA_REAL"] = Dr_Renglon_Actual["FECHA_ENTREGA_REAL"];
            Dr_Renglon_Nuevo["AVALUOS_ENTREGAR"] = Dr_Renglon_Actual["AVALUOS_ENTREGAR"];
            Dr_Renglon_Nuevo["AVALUOS_ENTREGADOS"] = Dr_Renglon_Actual["AVALUOS_ENTREGADOS"];
            Dt_Ejercicio_Fiscal.Rows.Add(Dr_Renglon_Nuevo);
        }

        int i = 1;
        foreach (DataRow Dr_Renglon_Actual in Dt_Ejercicio_Fiscal.Rows)
        {

            Dr_Renglon_Actual["NO_ENTREGA"] = i;

            i++;
            if (Dt_Ejercicio_Fiscal_Totales.Rows.Count == 0)
            {
                Dr_Renglon_Nuevo = Dt_Ejercicio_Fiscal_Totales.NewRow();
                Dt_Ejercicio_Fiscal_Totales.Rows.Add(Dr_Renglon_Nuevo);
            }
        }

        int Total_Entregar = 0;
        int Total_Entregados = 0;

        //Llenar total de avaluos a entregar y entregados ------------
        foreach (DataRow Dr_Renglon_Actual_Total in Dt_Ejercicio_Fiscal_Totales.Rows)
        {
            foreach (DataRow Dr_Renglon_Actual in Dt_Ejercicio_Fiscal.Rows)
            {
                // Diferencia_Totales = Diferencia_Totales + Convert.ToInt32(Dr_Renglon_Actual["DIFERENCIA_AVALUOS"].ToString());
                if (Dr_Renglon_Actual["AVALUOS_ENTREGAR"].ToString() != "" && Dr_Renglon_Actual["AVALUOS_ENTREGADOS"].ToString() != "")
                {
                    Total_Entregar = Total_Entregar + Convert.ToInt32(Dr_Renglon_Actual["AVALUOS_ENTREGAR"].ToString());
                    Total_Entregados = Total_Entregados + Convert.ToInt32(Dr_Renglon_Actual["AVALUOS_ENTREGADOS"].ToString());
                }
            }
            Dr_Renglon_Actual_Total["TOTAL_ENTREGAR"] = Total_Entregar;
            Dr_Renglon_Actual_Total["TOTAL_ENTREGADOS"] = Total_Entregados;
            Dr_Renglon_Actual_Total["TOTAL_PORCENTAJE_ENTREGADOS"] = 100.00;
        }
        //-------------------------------------
        //Sacar porcentajes-------------------------------

        foreach (DataRow Dr_Renglon_Actual_Total in Dt_Ejercicio_Fiscal_Totales.Rows)
        {
            int Porcentaje_Entregados = Convert.ToInt32(Dr_Renglon_Actual_Total["TOTAL_ENTREGADOS"].ToString());
            foreach (DataRow Dr_Renglon_Actual in Dt_Ejercicio_Fiscal.Rows)
            {
                if (Dr_Renglon_Actual["AVALUOS_ENTREGADOS"].ToString() != "0" && Porcentaje_Entregados != 0)
                {
                    Dr_Renglon_Actual["PORCENTAJE_ENTREGADOS"] = ((Convert.ToDouble(Dr_Renglon_Actual["AVALUOS_ENTREGADOS"].ToString()) * 100) / Porcentaje_Entregados);
                }
                else
                {
                    Dr_Renglon_Actual["PORCENTAJE_ENTREGADOS"] = 0.00;
                }
            }
        }
        //---------------------------
        foreach (DataRow Dr_Renglon_Actual in Dt_Ejercicio_Fiscal.Rows)
        {
            if (Dr_Renglon_Actual["AVALUOS_ENTREGADOS"].ToString() != "" && Dr_Renglon_Actual["AVALUOS_ENTREGADOS"].ToString() != "0")
            {
                Dr_Renglon_Actual["DIFERENCIA_AVALUOS"] = (Convert.ToInt32(Dr_Renglon_Actual["AVALUOS_ENTREGADOS"]) - Convert.ToInt32(Dr_Renglon_Actual["AVALUOS_ENTREGAR"]));
            }
            else
            {
                Dr_Renglon_Actual["DIFERENCIA_AVALUOS"] = 0;
            }
        }
        //Total de diferencias----------------------------
        int Diferencia_Totales = 0;
        foreach (DataRow Dr_Renglon_Actual_Total in Dt_Ejercicio_Fiscal_Totales.Rows)
        {
            foreach (DataRow Dr_Renglon_Actual in Dt_Ejercicio_Fiscal.Rows)
            {
                Diferencia_Totales = Diferencia_Totales + Convert.ToInt32(Dr_Renglon_Actual["DIFERENCIA_AVALUOS"].ToString());
            }
            Dr_Renglon_Actual_Total["TOTAL_DIFERENCIA_AVALUOS"] = Diferencia_Totales;
        }
        //----------------------
        Dt_Ejercicio_Fiscal.TableName = "DT_EJERCICIO_FISCAL_AV";
        Dt_Ejercicio_Fiscal_Totales.TableName = "DT_TOTALES_EJERCICIO_FISCAL";
        Ds_Ejercicio_Fiscal.Tables.Remove("DT_EJERCICIO_FISCAL_AV");
        Ds_Ejercicio_Fiscal.Tables.Remove("DT_TOTALES_EJERCICIO_FISCAL");
        Ds_Ejercicio_Fiscal.Tables.Add(Dt_Ejercicio_Fiscal.Copy());
        Ds_Ejercicio_Fiscal.Tables.Add(Dt_Ejercicio_Fiscal_Totales.Copy());
        return Ds_Ejercicio_Fiscal;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Avaluos_Realizados_Valuador
    ///DESCRIPCIÓN          : Crea un DataSet para el reporte del avalúo urbano
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Junio/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Cuentas_Asiganadas_Atendidas()
    {
        Ds_Rpt_Cuentas_Asiganadas_Atendidas Ds_Cuentas_Asiganadas_Atendidas = new Ds_Rpt_Cuentas_Asiganadas_Atendidas();
        Cls_Ope_Cat_Asignacion_Cuentas_Negocio Reporte = new Cls_Ope_Cat_Asignacion_Cuentas_Negocio();
        DataTable Dt_Total_Cuentas_Asig_Adq;
        Dt_Total_Cuentas_Asig_Adq = Ds_Cuentas_Asiganadas_Atendidas.Tables["DT_CUENTAS_ASIGNADAS"];
        Reporte.P_Anio = Txt_Anio.Text;
        Dt_Total_Cuentas_Asig_Adq = Reporte.Consultas_Avaluos_Asisgnados_Atendidos();
        Dt_Total_Cuentas_Asig_Adq.TableName = "DT_CUENTAS_ASIGNADAS";
        Ds_Cuentas_Asiganadas_Atendidas.Tables.Remove("DT_CUENTAS_ASIGNADAS");
        Ds_Cuentas_Asiganadas_Atendidas.Tables.Add(Dt_Total_Cuentas_Asig_Adq.Copy());
        return Ds_Cuentas_Asiganadas_Atendidas;
    }
}