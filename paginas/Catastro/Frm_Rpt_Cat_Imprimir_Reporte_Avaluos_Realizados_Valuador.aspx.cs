using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Data;
using Presidencia.Catalogo_Cat_Peritos_Internos.Negocio;


public partial class paginas_Catastro_Frm_Imprimir_Reporte_Avaluos_Realizados_Valuador : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                
                Session["Activa"] = true;//Variable para mantener la session activa.
                
                
            }

        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
        //Div_Contenedor_Msj_Error.Visible = false;
    
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Imprimir_Cuentas_Click
    ///DESCRIPCIÓN          : Impresion del Reporte
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Junio/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_Cuentas_Click(object sender, ImageClickEventArgs e)
    {
        Imprimir_Reporte(Crear_Ds_Avaluos_Realizados_Valuador(), "Rpt_Cat_Numero_Entregas_Personales.rpt", "Cuotas_Asigdas", "Window_Frm", "Cuotas_Asignadas");
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
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Avaluos_Realizados_Valuador
    ///DESCRIPCIÓN          : Crea un DataSet para el reporte del avalúo urbano
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Junio/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Avaluos_Realizados_Valuador()
    {
        Ds_Rpt_Cat_Numero_Entregas Ds_Cuentas_Asignadas = new Ds_Rpt_Cat_Numero_Entregas();
        Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Negocio Reporte = new Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Negocio();

        DataTable Dt_Nombres;
        Dt_Nombres = Ds_Cuentas_Asignadas.Tables["DT_NUM_ENTREGAS_PERS_NOMBRE"];
        DataTable Dt_Prim_Entrega;
        Dt_Prim_Entrega = Ds_Cuentas_Asignadas.Tables["DT_NUM_ENTREGAS_PERS_PRIM"];
        DataTable Dt_Seg_Entrega;
        Dt_Prim_Entrega = Ds_Cuentas_Asignadas.Tables["DT_NUM_ENTREGAS_PERS_SEG"];
        DataTable Dt_Ter_Entrega;
        Dt_Prim_Entrega = Ds_Cuentas_Asignadas.Tables["DT_NUM_ENTREGAS_PERS_TER"];
        DataTable Dt_Cua_Entrega;
        Dt_Prim_Entrega = Ds_Cuentas_Asignadas.Tables["DT_NUM_ENTREGAS_PERS_CUA"];
        DataTable Dt_Quin_Entrega;
        Dt_Prim_Entrega = Ds_Cuentas_Asignadas.Tables["DT_NUM_ENTREGAS_PERS_QUIN"];
        DataTable Dt_Sext_Entrega;
        Dt_Prim_Entrega = Ds_Cuentas_Asignadas.Tables["DT_NUM_ENTREGAS_PERS_SEXT"];
        DataTable Dt_Sept_Entrega;
        Dt_Prim_Entrega = Ds_Cuentas_Asignadas.Tables["DT_NUM_ENTREGAS_PERS_SEPT"];
        Cls_Cat_Cat_Peritos_Internos_Negocio Perito = new Cls_Cat_Cat_Peritos_Internos_Negocio();
        DataTable Dt_Consullta_Peritos = Perito.Consultar_Peritos_Internos();
      
        Dt_Prim_Entrega = Reporte.Consultar_Avaluos_Asignados_Primera_Entrega();
        Dt_Seg_Entrega = Reporte.Consultar_Avaluos_Asignados_Segunda_Entrega();
        Dt_Ter_Entrega = Reporte.Consultar_Avaluos_Asignados_Tercera_Entrega();
        Dt_Cua_Entrega = Reporte.Consultar_Avaluos_Asignados_Cuarta_Entrega();
        Dt_Quin_Entrega = Reporte.Consultar_Avaluos_Asignados_Quinta_Entrega();
        Dt_Sext_Entrega = Reporte.Consultar_Avaluos_Asignados_Sexta_Entrega();
        Dt_Sept_Entrega = Reporte.Consultar_Avaluos_Asignados_Septima_Entrega();
        DataRow Dr_Renglon_Nuevo;
        DataColumn Dc_Columna_Nueva_1A= Dt_Prim_Entrega.Columns.Add("DIFERENCIA", typeof (Int32));
        DataColumn Dc_Columna_Nueva_2A = Dt_Seg_Entrega.Columns.Add("DIFERENCIA", typeof(Int32));
        DataColumn Dc_Columna_Nueva_3A = Dt_Ter_Entrega.Columns.Add("DIFERENCIA", typeof(Int32));
        DataColumn Dc_Columna_Nueva_4A = Dt_Cua_Entrega.Columns.Add("DIFERENCIA", typeof(Int32));
        DataColumn Dc_Columna_Nueva_5A = Dt_Quin_Entrega.Columns.Add("DIFERENCIA", typeof(Int32));
        DataColumn Dc_Columna_Nueva_6A = Dt_Sext_Entrega.Columns.Add("DIFERENCIA", typeof(Int32));
        DataColumn Dc_Columna_Nueva_7A = Dt_Sept_Entrega.Columns.Add("DIFERENCIA", typeof(Int32));


        foreach (DataRow Dr_Renglon_Actual in Dt_Prim_Entrega.Rows)
        {
            if (Dr_Renglon_Actual["CUOTA"].ToString() != "" && Dr_Renglon_Actual["TOTAL_ENTREGADOS"].ToString() != "" && Dr_Renglon_Actual["CUOTA"].ToString() != null && Dr_Renglon_Actual["TOTAL_ENTREGADOS"].ToString() != null)
            {
                Dr_Renglon_Actual["DIFERENCIA"] = Convert.ToInt32(Dr_Renglon_Actual["TOTAL_ENTREGADOS"]) - Convert.ToInt32(Dr_Renglon_Actual["CUOTA"]);

            }
        
        }
        foreach (DataRow Dr_Renglon_Actual in Dt_Seg_Entrega.Rows)
        {
            if (Dr_Renglon_Actual["CUOTA"].ToString() != "" && Dr_Renglon_Actual["TOTAL_ENTREGADOS"].ToString() != "" && Dr_Renglon_Actual["CUOTA"].ToString() != null && Dr_Renglon_Actual["TOTAL_ENTREGADOS"].ToString() != null)
            {
                Dr_Renglon_Actual["DIFERENCIA"] = Convert.ToInt32(Dr_Renglon_Actual["TOTAL_ENTREGADOS"]) - Convert.ToInt32(Dr_Renglon_Actual["CUOTA"]);

            }

        }
        foreach (DataRow Dr_Renglon_Actual in Dt_Ter_Entrega.Rows)
        {
            if (Dr_Renglon_Actual["CUOTA"].ToString() != "" && Dr_Renglon_Actual["TOTAL_ENTREGADOS"].ToString() != "" && Dr_Renglon_Actual["CUOTA"].ToString() != null && Dr_Renglon_Actual["TOTAL_ENTREGADOS"].ToString() != null)
            {
                Dr_Renglon_Actual["DIFERENCIA"] = Convert.ToInt32(Dr_Renglon_Actual["TOTAL_ENTREGADOS"]) - Convert.ToInt32(Dr_Renglon_Actual["CUOTA"]);

            }
        }
        foreach (DataRow Dr_Renglon_Actual in Dt_Cua_Entrega.Rows)
        {
            if (Dr_Renglon_Actual["CUOTA"].ToString() != "" && Dr_Renglon_Actual["TOTAL_ENTREGADOS"].ToString() != "" && Dr_Renglon_Actual["CUOTA"].ToString() != null && Dr_Renglon_Actual["TOTAL_ENTREGADOS"].ToString() != null)
            {
                Dr_Renglon_Actual["DIFERENCIA"] = Convert.ToInt32(Dr_Renglon_Actual["TOTAL_ENTREGADOS"]) - Convert.ToInt32(Dr_Renglon_Actual["CUOTA"]);

            }
        }
        foreach (DataRow Dr_Renglon_Actual in Dt_Quin_Entrega.Rows)
        {
            if (Dr_Renglon_Actual["CUOTA"].ToString() != "" && Dr_Renglon_Actual["TOTAL_ENTREGADOS"].ToString() != "" && Dr_Renglon_Actual["CUOTA"].ToString() != null && Dr_Renglon_Actual["TOTAL_ENTREGADOS"].ToString() != null)
            {
                Dr_Renglon_Actual["DIFERENCIA"] = Convert.ToInt32(Dr_Renglon_Actual["TOTAL_ENTREGADOS"]) - Convert.ToInt32(Dr_Renglon_Actual["CUOTA"]);

            }
        }
        foreach (DataRow Dr_Renglon_Actual in Dt_Sext_Entrega.Rows)
        {
            if (Dr_Renglon_Actual["CUOTA"].ToString() != "" && Dr_Renglon_Actual["TOTAL_ENTREGADOS"].ToString() != "" && Dr_Renglon_Actual["CUOTA"].ToString() != null && Dr_Renglon_Actual["TOTAL_ENTREGADOS"].ToString() != null)
            {
                Dr_Renglon_Actual["DIFERENCIA"] = Convert.ToInt32(Dr_Renglon_Actual["TOTAL_ENTREGADOS"]) - Convert.ToInt32(Dr_Renglon_Actual["CUOTA"]);

            }
        }
        foreach (DataRow Dr_Renglon_Actual in Dt_Sept_Entrega.Rows)
        {
            if (Dr_Renglon_Actual["CUOTA"].ToString() != "" && Dr_Renglon_Actual["TOTAL_ENTREGADOS"].ToString() != "" && Dr_Renglon_Actual["CUOTA"].ToString() != null && Dr_Renglon_Actual["TOTAL_ENTREGADOS"].ToString() != null)
            {
                Dr_Renglon_Actual["DIFERENCIA"] = Convert.ToInt32(Dr_Renglon_Actual["TOTAL_ENTREGADOS"]) - Convert.ToInt32(Dr_Renglon_Actual["CUOTA"]);

            }
        }
            foreach (DataRow Dr_Renglon_Actual in Dt_Consullta_Peritos.Rows)
            {
                if( Dr_Renglon_Actual["ESTATUS"].ToString()=="VIGENTE")
                {
                Dr_Renglon_Nuevo = Dt_Nombres.NewRow();
                Dr_Renglon_Nuevo["NOMBRE"] = Dr_Renglon_Actual["NOMBRE"];
                Dr_Renglon_Nuevo["PERITO_INTERNO_ID"] = Dr_Renglon_Actual["PERITO_INTERNO_ID"];
                
                Dt_Nombres.Rows.Add(Dr_Renglon_Nuevo);
                }
            }

            Dt_Prim_Entrega.TableName = "DT_NUM_ENTREGAS_PERS_PRIM";
            Dt_Seg_Entrega.TableName = "DT_NUM_ENTREGAS_PERS_SEG";
            Dt_Ter_Entrega.TableName = "DT_NUM_ENTREGAS_PERS_TER";
            Dt_Cua_Entrega.TableName = "DT_NUM_ENTREGAS_PERS_CUA";
            Dt_Quin_Entrega.TableName = "DT_NUM_ENTREGAS_PERS_QUIN";
            Dt_Sext_Entrega.TableName = "DT_NUM_ENTREGAS_PERS_SEXT";
            Dt_Sept_Entrega.TableName = "DT_NUM_ENTREGAS_PERS_SEPT";





            Ds_Cuentas_Asignadas.Tables.Remove("DT_NUM_ENTREGAS_PERS_NOMBRE");
            Ds_Cuentas_Asignadas.Tables.Remove("DT_NUM_ENTREGAS_PERS_PRIM");
            Ds_Cuentas_Asignadas.Tables.Remove("DT_NUM_ENTREGAS_PERS_SEG");
            Ds_Cuentas_Asignadas.Tables.Remove("DT_NUM_ENTREGAS_PERS_TER");
            Ds_Cuentas_Asignadas.Tables.Remove("DT_NUM_ENTREGAS_PERS_CUA");
            Ds_Cuentas_Asignadas.Tables.Remove("DT_NUM_ENTREGAS_PERS_QUIN");
            Ds_Cuentas_Asignadas.Tables.Remove("DT_NUM_ENTREGAS_PERS_SEXT");
            Ds_Cuentas_Asignadas.Tables.Remove("DT_NUM_ENTREGAS_PERS_SEPT");

            Ds_Cuentas_Asignadas.Tables.Add(Dt_Nombres.Copy());
            Ds_Cuentas_Asignadas.Tables.Add(Dt_Prim_Entrega.Copy());
            Ds_Cuentas_Asignadas.Tables.Add(Dt_Seg_Entrega.Copy());
            Ds_Cuentas_Asignadas.Tables.Add(Dt_Ter_Entrega.Copy());
            Ds_Cuentas_Asignadas.Tables.Add(Dt_Cua_Entrega.Copy());
            Ds_Cuentas_Asignadas.Tables.Add(Dt_Quin_Entrega.Copy());
            Ds_Cuentas_Asignadas.Tables.Add(Dt_Sext_Entrega.Copy());
            Ds_Cuentas_Asignadas.Tables.Add(Dt_Sept_Entrega.Copy());
            
            
        return Ds_Cuentas_Asignadas;
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Imprimir_Cuentas_Click
    ///DESCRIPCIÓN          : Impresion del Reporte
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Junio/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_Avaluos_Fiscales_Click(object sender, ImageClickEventArgs e)
    {
        Imprimir_Reporte(Crear_Ds_Avaluos_Fiscales(), "Rpt_Cat_Cat_Relacion_Avaluos_Fiscales.rpt", "Cuotas_Asigdas", "Window_Frm", "Cuotas_Asignadas");
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
    private DataSet Crear_Ds_Avaluos_Fiscales()
    {

        Ds_Rpt_Cat_Relacion_Aaluos_Fiscales Ds_Avaluos_Fiscales = new Ds_Rpt_Cat_Relacion_Aaluos_Fiscales();
       
        Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Negocio Reporte = new Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Negocio();

        DataTable Dt_Avaluos_Fiscales;
        Dt_Avaluos_Fiscales = Ds_Avaluos_Fiscales.Tables["DT_RELACION_AVALUOS_FISCALES"];
        Dt_Avaluos_Fiscales = Reporte.Consultar_Avaluos_Fiscales();
        DataColumn Dc_No_Avaluo = Dt_Avaluos_Fiscales.Columns.Add("NO_AVALUO", typeof(Int32));
       int i=1;
        foreach (DataRow Dr_Renglon_Actual in Dt_Avaluos_Fiscales.Rows)
        {
            
                Dr_Renglon_Actual["NO_AVALUO"] = i;
            i++   ;
          
        }

        Dt_Avaluos_Fiscales.TableName = "DT_RELACION_AVALUOS_FISCALES";





        Ds_Avaluos_Fiscales.Tables.Remove("DT_RELACION_AVALUOS_FISCALES");


        Ds_Avaluos_Fiscales.Tables.Add(Dt_Avaluos_Fiscales.Copy());



        return Ds_Avaluos_Fiscales;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Imprimir_Cuentas_Click
    ///DESCRIPCIÓN          : Impresion del Reporte
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Junio/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_Metas_Autorizacion_Click(object sender, ImageClickEventArgs e)
    {
        Imprimir_Reporte(Crear_Ds_Avaluos_Metas_Autorizacion(), "Rpt_Cat_Metas_Autoraciones.rpt", "Cuotas_Asigdas", "Window_Frm", "Cuotas_Asignadas");
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
    private DataSet Crear_Ds_Avaluos_Metas_Autorizacion()
    {

        Ds_Rpt_Metas_Autorizaciones Ds_Metas_Autorizacion = new Ds_Rpt_Metas_Autorizaciones();

        Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Negocio Reporte = new Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Negocio();

        DataTable Dt_Metas_Autorizacion;
        Dt_Metas_Autorizacion = Ds_Metas_Autorizacion.Tables["DT_METAS_AUTORIZACIONES"];
        Dt_Metas_Autorizacion = Reporte.Consultar_Metas_Autorizacion();
        DataColumn Dc_No_Avaluo = Dt_Metas_Autorizacion.Columns.Add("TOTAL", typeof(Int32));
        int i = 1;
        foreach (DataRow Dr_Renglon_Actual in Dt_Metas_Autorizacion.Rows)
        {

            Dr_Renglon_Actual["TOTAL"] = Dr_Renglon_Actual["AUTORIZADOS"].ToString() + Dr_Renglon_Actual["CORRECCIONES"].ToString();
            i++;

        }

        Dt_Metas_Autorizacion.TableName = "DT_METAS_AUTORIZACIONES";





        Ds_Metas_Autorizacion.Tables.Remove("DT_METAS_AUTORIZACIONES");


        Ds_Metas_Autorizacion.Tables.Add(Dt_Metas_Autorizacion.Copy());



        return Ds_Metas_Autorizacion;
    }
}
