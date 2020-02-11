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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Cemovientes.Negocio;
using Presidencia.Almacen_Resguardos.Negocio;
using Presidencia.Control_Patrimonial_Reporte_Detalles_Bien.Negocio;

public partial class paginas_Compras_Frm_Rpt_Pat_Detalle_Bien : System.Web.UI.Page
{

    #region Variables


    Cls_Alm_Com_Resguardos_Negocio Consulta_Resguardos_Negocio = new Cls_Alm_Com_Resguardos_Negocio();

    #endregion

    #region  Page Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo Inicial de la página.
    ///PARAMETROS:  
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 06/Mayo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e) {
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
    }

#endregion

    #region Metodos

    #region Generar Reporte Bienes Muebles

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_DataSet_Resguardos_Bienes()
    ///DESCRIPCIÓN: Llena el dataSet "Data_Set_Resguardos_Bienes" con las personas a las que se les asigno el
    ///bien mueble y sus detalles, para que con estos datos se genere el reporte.
    ///PARAMETROS:  
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 17/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_DataSet_Resguardos_Bienes(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Id, String Tipo)
    {
        Consulta_Resguardos_Negocio = new Cls_Alm_Com_Resguardos_Negocio();
        DataSet Data_Set_Resguardos_Bienes;
        Bien_Id.P_Producto_Almacen = false;
        Consulta_Resguardos_Negocio.P_Operacion = Bien_Id.P_Operacion; // Se le asigna la variable que indica si es resguardo o recibo
        Data_Set_Resguardos_Bienes = Consulta_Resguardos_Negocio.Consulta_Resguardos_Bienes2(Bien_Id);
        
        Ds_Alm_Com_Resguardos_Bienes Ds_Consulta_Resguardos_Bienes = new Ds_Alm_Com_Resguardos_Bienes();
        if (Tipo.Equals("Pdf")) {
            Generar_Reporte_Bienes_Muebles(Data_Set_Resguardos_Bienes, Ds_Consulta_Resguardos_Bienes, "Rpt_Pat_Bien_Mueble_Detalles.rpt");
        } else {
            Generar_Reporte_Bienes_Muebles_Excel(Data_Set_Resguardos_Bienes, Ds_Consulta_Resguardos_Bienes, "Rpt_Pat_Bien_Mueble_Detalles.rpt");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Bienes_Muebles
    ///DESCRIPCIÓN: caraga el data set fisoco con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 15/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte_Bienes_Muebles(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte)
    {

        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        DataRow Renglon;
        Renglon = Data_Set_Consulta_DB.Tables[0].Rows[0];
        String Cantidad = Data_Set_Consulta_DB.Tables[0].Rows[0]["CANTIDAD"].ToString();
        String Costo = Data_Set_Consulta_DB.Tables[0].Rows[0]["COSTO_UNITARIO"].ToString();
        Double Resultado = (Convert.ToDouble(Cantidad)) * (Convert.ToDouble(Costo));
        Ds_Reporte.Tables[1].ImportRow(Renglon);
        Ds_Reporte.Tables[1].Rows[0].SetField("COSTO_TOTAL", Resultado);

        for (int Cont_Elementos = 0; Cont_Elementos < Data_Set_Consulta_DB.Tables[0].Rows.Count; Cont_Elementos++)
        {
            Renglon = Data_Set_Consulta_DB.Tables[0].Rows[Cont_Elementos];
            Ds_Reporte.Tables[0].ImportRow(Renglon);
            String Nombre_E = Data_Set_Consulta_DB.Tables[0].Rows[Cont_Elementos]["NOMBRE_E"].ToString();
            String Apellido_Paterno_E = Data_Set_Consulta_DB.Tables[0].Rows[Cont_Elementos]["APELLIDO_PATERNO_E"].ToString();
            String Apellido_Materno_E = Data_Set_Consulta_DB.Tables[0].Rows[Cont_Elementos]["APELLIDO_MATERNO_E"].ToString();
            String RFC_E = Data_Set_Consulta_DB.Tables[0].Rows[Cont_Elementos]["RFC_E"].ToString();
            String Resguardante = Nombre_E + " " + Apellido_Paterno_E + " " + Apellido_Materno_E + " " + "(" + RFC_E + ")";
            Ds_Reporte.Tables[0].Rows[Cont_Elementos].SetField("RESGUARDANTES", Resguardante);
        }
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Resguardos Bienes.pdf");
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        Reporte.Export(Export_Options);
        String Ruta = "../../Reporte/Resguardos Bienes.pdf";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Bienes_Muebles_Excel
    ///DESCRIPCIÓN: caraga el data set fisoco con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 15/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte_Bienes_Muebles_Excel(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte)
    {

        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        DataRow Renglon;
        Renglon = Data_Set_Consulta_DB.Tables[0].Rows[0];
        String Cantidad = Data_Set_Consulta_DB.Tables[0].Rows[0]["CANTIDAD"].ToString();
        String Costo = Data_Set_Consulta_DB.Tables[0].Rows[0]["COSTO_UNITARIO"].ToString();
        Double Resultado = (Convert.ToDouble(Cantidad)) * (Convert.ToDouble(Costo));
        Ds_Reporte.Tables[1].ImportRow(Renglon);
        Ds_Reporte.Tables[1].Rows[0].SetField("COSTO_TOTAL", Resultado);

        for (int Cont_Elementos = 0; Cont_Elementos < Data_Set_Consulta_DB.Tables[0].Rows.Count; Cont_Elementos++)
        {
            Renglon = Data_Set_Consulta_DB.Tables[0].Rows[Cont_Elementos];
            Ds_Reporte.Tables[0].ImportRow(Renglon);
            String Nombre_E = Data_Set_Consulta_DB.Tables[0].Rows[Cont_Elementos]["NOMBRE_E"].ToString();
            String Apellido_Paterno_E = Data_Set_Consulta_DB.Tables[0].Rows[Cont_Elementos]["APELLIDO_PATERNO_E"].ToString();
            String Apellido_Materno_E = Data_Set_Consulta_DB.Tables[0].Rows[Cont_Elementos]["APELLIDO_MATERNO_E"].ToString();
            String RFC_E = Data_Set_Consulta_DB.Tables[0].Rows[Cont_Elementos]["RFC_E"].ToString();
            String Resguardante = Nombre_E + " " + Apellido_Paterno_E + " " + Apellido_Materno_E + " " + "(" + RFC_E + ")";
            Ds_Reporte.Tables[0].Rows[Cont_Elementos].SetField("RESGUARDANTES", Resguardante);
        }
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Resguardos Bienes.xls");
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.Excel;
        Reporte.Export(Export_Options);
        String Ruta = "../../Reporte/Resguardos Bienes.xls";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }

    #endregion

    #region Generar Reporte Vehiculo

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_DataSet_Resguardos_Vehiculos
    ///DESCRIPCIÓN: Llena el dataSet "Data_Set_Resguardos_Vehiculos" con las personas a las que se les asigno el
    ///vehiculo, sus detalles generales y especificos, para que con estos datos se genere el reporte.
    ///PARAMETROS:  
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 23/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_DataSet_Resguardos_Vehiculos(Cls_Ope_Pat_Com_Vehiculos_Negocio Id_Vehiculo, String Tipo)
    {
        try
        {
            Cls_Alm_Com_Resguardos_Negocio Consulta_Resguardos_Vehiculos = new Cls_Alm_Com_Resguardos_Negocio();
            DataSet Data_Set_Resguardos_Vehiculos, Data_Set_Vehiculos_Asegurados;
            Id_Vehiculo.P_Producto_Almacen = false;
            Data_Set_Resguardos_Vehiculos = Consulta_Resguardos_Vehiculos.Consulta_Resguardos_Vehiculos(Id_Vehiculo);
            Data_Set_Vehiculos_Asegurados = Consulta_Resguardos_Vehiculos.Consulta_Vehiculos_Asegurados(Id_Vehiculo);
            Ds_Alm_Com_Resguardos_Vehiculos Ds_Consulta_Resguardos_Vehiculos = new Ds_Alm_Com_Resguardos_Vehiculos();
            if (Tipo.Equals("Pdf")) {
                Generar_Reporte_Vehiculo(Data_Set_Vehiculos_Asegurados, Data_Set_Resguardos_Vehiculos, Ds_Consulta_Resguardos_Vehiculos, "Rpt_Pat_Vehiculo_Detalles.rpt");
            } else {
                Generar_Reporte_Vehiculo_Excel(Data_Set_Vehiculos_Asegurados, Data_Set_Resguardos_Vehiculos, Ds_Consulta_Resguardos_Vehiculos, "Rpt_Pat_Vehiculo_Detalles.rpt");
            }
            
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Vehiculo
    ///DESCRIPCIÓN: caraga el data set fisico con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 15/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte_Vehiculo(DataSet Data_Set_Consulta_Vehiculos_A, DataSet Data_Set_Consulta_Resguardos_V, DataSet Ds_Reporte, string Nombre_Reporte)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);

        Reporte.Load(File_Path);

        if (Data_Set_Consulta_Resguardos_V.Tables[0].Rows.Count > 0)
        {
            DataRow Renglon;
            String Cantidad = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[0]["CANTIDAD"].ToString();
            String Costo = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[0]["COSTO_UNITARIO"].ToString();
            Double Resultado = (Convert.ToDouble(Cantidad)) * (Convert.ToDouble(Costo));

            String Total = "" + Resultado;
            Renglon = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[0];
            Ds_Reporte.Tables[1].ImportRow(Renglon);
            Ds_Reporte.Tables[1].Rows[0].SetField("COSTO_TOTAL", Total);

            for (int Cont_Elementos = 0; Cont_Elementos < Data_Set_Consulta_Resguardos_V.Tables[0].Rows.Count; Cont_Elementos++)
            {
                Renglon = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]; //Instanciar renglon e importarlo
                Ds_Reporte.Tables[0].ImportRow(Renglon);

                String Nombre_E = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]["NOMBRE_E"].ToString();
                String Apellido_Paterno_E = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]["APELLIDO_PATERNO_E"].ToString();
                String Apellido_Materno_E = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]["APELLIDO_MATERNO_E"].ToString();
                String RFC_E = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]["RFC_E"].ToString();
                String Resguardante = Nombre_E + " " + Apellido_Paterno_E + " " + Apellido_Materno_E + " " + "(" + RFC_E + ")";
                Ds_Reporte.Tables[0].Rows[Cont_Elementos].SetField("RESGUARDANTES", Resguardante);
            }

            if (Data_Set_Consulta_Vehiculos_A.Tables[0].Rows.Count > 0)
            {
                String Nombre_Aeguradora = Data_Set_Consulta_Vehiculos_A.Tables[0].Rows[0]["NOMBRE_ASEGURADORA"].ToString();
                String No_Poliza = Data_Set_Consulta_Vehiculos_A.Tables[0].Rows[0]["NO_POLIZA"].ToString();
                String Descripcion_Seguro = Data_Set_Consulta_Vehiculos_A.Tables[0].Rows[0]["DESCRIPCION_SEGURO"].ToString();
                String Cobertura = Data_Set_Consulta_Vehiculos_A.Tables[0].Rows[0]["COBERTURA"].ToString();
                Ds_Reporte.Tables[1].Rows[0].SetField("NOMBRE_ASEGURADORA", Nombre_Aeguradora);
                Ds_Reporte.Tables[1].Rows[0].SetField("NO_POLIZA", No_Poliza);
                Ds_Reporte.Tables[1].Rows[0].SetField("DESCRIPCION_SEGURO", Descripcion_Seguro);
                Ds_Reporte.Tables[1].Rows[0].SetField("COBERTURA", Cobertura);
            }
        }

        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Resguardos Vehiculos.pdf");
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        Reporte.Export(Export_Options);
        String Ruta = "../../Reporte/Resguardos Vehiculos.pdf";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Vehiculo
    ///DESCRIPCIÓN: caraga el data set fisico con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 15/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte_Vehiculo_Excel(DataSet Data_Set_Consulta_Vehiculos_A, DataSet Data_Set_Consulta_Resguardos_V, DataSet Ds_Reporte, string Nombre_Reporte)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);

        Reporte.Load(File_Path);

        if (Data_Set_Consulta_Resguardos_V.Tables[0].Rows.Count > 0)
        {
            DataRow Renglon;
            String Cantidad = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[0]["CANTIDAD"].ToString();
            String Costo = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[0]["COSTO_UNITARIO"].ToString();
            Double Resultado = (Convert.ToDouble(Cantidad)) * (Convert.ToDouble(Costo));

            String Total = "" + Resultado;
            Renglon = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[0];
            Ds_Reporte.Tables[1].ImportRow(Renglon);
            Ds_Reporte.Tables[1].Rows[0].SetField("COSTO_TOTAL", Total);

            for (int Cont_Elementos = 0; Cont_Elementos < Data_Set_Consulta_Resguardos_V.Tables[0].Rows.Count; Cont_Elementos++)
            {
                Renglon = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]; //Instanciar renglon e importarlo
                Ds_Reporte.Tables[0].ImportRow(Renglon);

                String Nombre_E = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]["NOMBRE_E"].ToString();
                String Apellido_Paterno_E = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]["APELLIDO_PATERNO_E"].ToString();
                String Apellido_Materno_E = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]["APELLIDO_MATERNO_E"].ToString();
                String RFC_E = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]["RFC_E"].ToString();
                String Resguardante = Nombre_E + " " + Apellido_Paterno_E + " " + Apellido_Materno_E + " " + "(" + RFC_E + ")";
                Ds_Reporte.Tables[0].Rows[Cont_Elementos].SetField("RESGUARDANTES", Resguardante);
            }

            if (Data_Set_Consulta_Vehiculos_A.Tables[0].Rows.Count > 0)
            {
                String Nombre_Aeguradora = Data_Set_Consulta_Vehiculos_A.Tables[0].Rows[0]["NOMBRE_ASEGURADORA"].ToString();
                String No_Poliza = Data_Set_Consulta_Vehiculos_A.Tables[0].Rows[0]["NO_POLIZA"].ToString();
                String Descripcion_Seguro = Data_Set_Consulta_Vehiculos_A.Tables[0].Rows[0]["DESCRIPCION_SEGURO"].ToString();
                String Cobertura = Data_Set_Consulta_Vehiculos_A.Tables[0].Rows[0]["COBERTURA"].ToString();
                Ds_Reporte.Tables[1].Rows[0].SetField("NOMBRE_ASEGURADORA", Nombre_Aeguradora);
                Ds_Reporte.Tables[1].Rows[0].SetField("NO_POLIZA", No_Poliza);
                Ds_Reporte.Tables[1].Rows[0].SetField("DESCRIPCION_SEGURO", Descripcion_Seguro);
                Ds_Reporte.Tables[1].Rows[0].SetField("COBERTURA", Cobertura);
            }
        }

        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Resguardos Vehiculos.xls");
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.Excel;
        Reporte.Export(Export_Options);
        String Ruta = "../../Reporte/Resguardos Vehiculos.xls";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }


    #endregion

    #region Generar Reporte Cemovientes

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_DataSet_Resguardos_Cemovientes
    ///DESCRIPCIÓN: Llena el dataSet "Data_Set_Resguardos_Vehiculos" con las personas a las que se les asigno el
    ///vehiculo, sus detalles generales y especificos, para que con estos datos se genere el reporte.
    ///PARAMETROS:  
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 23/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_DataSet_Resguardos_Cemovientes(Cls_Ope_Pat_Com_Cemovientes_Negocio Id_Cemoviente, String Tipo)
    {
        Cls_Alm_Com_Resguardos_Negocio Consulta_Resguardos_Cemovientes = new Cls_Alm_Com_Resguardos_Negocio();
        DataSet Ds_Consulta_Resguardos_Cemovientes = new DataSet();
        Id_Cemoviente.P_Producto_Almacen = false;
        Ds_Consulta_Resguardos_Cemovientes = Consulta_Resguardos_Cemovientes.Consulta_Resguardos_Cemovientes(Id_Cemoviente);
        Ds_Alm_Com_Resguardos_Cemovientes DS_Reporte_Resguardos_Cemovientes = new Ds_Alm_Com_Resguardos_Cemovientes();
        if (Tipo.Equals("Pdf")) {
            Generar_Reporte_Cemoviente(Ds_Consulta_Resguardos_Cemovientes, DS_Reporte_Resguardos_Cemovientes, "Rpt_Pat_Cemoviente_Detalles.rpt");
        } else {
            Generar_Reporte_Cemoviente_Excel(Ds_Consulta_Resguardos_Cemovientes, DS_Reporte_Resguardos_Cemovientes, "Rpt_Pat_Cemoviente_Detalles.rpt");
        }
        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Cemoviente
    ///DESCRIPCIÓN: Cargara el data Set fisico con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.-Ds_Consulta_Resguardos_Cemovientes.- Contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data Set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 15/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte_Cemoviente(DataSet Ds_Consulta_Resguardos_Cemovientes, DataSet Ds_Reporte, String Nombre_Reporte)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        DataRow Renglon;
        Renglon = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[0];

        String Cantidad = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[0]["CANTIDAD"].ToString();
        String Costo = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[0]["COSTO_ACTUAL"].ToString();
        Double Resultado = (Convert.ToDouble(Cantidad)) * (Convert.ToDouble(Costo));
        Ds_Reporte.Tables[0].ImportRow(Renglon);
        Ds_Reporte.Tables[0].Rows[0].SetField("COSTO_TOTAL", Resultado);

        for (int Cont_Elementos = 0; Cont_Elementos < Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows.Count; Cont_Elementos++)
        {
            Renglon = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[Cont_Elementos]; //Instanciar renglon e importarlo
            Ds_Reporte.Tables[1].ImportRow(Renglon);

            String Nombre_E = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[Cont_Elementos]["NOMBRE_E"].ToString();
            String Apellido_Paterno_E = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[Cont_Elementos]["APELLIDO_PATERNO_E"].ToString();
            String Apellido_Materno_E = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[Cont_Elementos]["APELLIDO_MATERNO_E"].ToString();
            String RFC_E = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[Cont_Elementos]["RFC_E"].ToString();
            String Resguardante = Nombre_E + " " + Apellido_Paterno_E + " " + Apellido_Materno_E + " " + "(" + RFC_E + ")";
            Ds_Reporte.Tables[1].Rows[Cont_Elementos].SetField("RESGUARDANTES", Resguardante);
        }

        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Resguardo Cemovientes.pdf");
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        Reporte.Export(Export_Options);
        String Ruta = "../../Reporte/Resguardo Cemovientes.pdf";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Cemoviente_Excel
    ///DESCRIPCIÓN: Cargara el data Set fisico con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.-Ds_Consulta_Resguardos_Cemovientes.- Contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data Set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 15/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte_Cemoviente_Excel(DataSet Ds_Consulta_Resguardos_Cemovientes, DataSet Ds_Reporte, String Nombre_Reporte)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        DataRow Renglon;
        Renglon = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[0];

        String Cantidad = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[0]["CANTIDAD"].ToString();
        String Costo = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[0]["COSTO_ACTUAL"].ToString();
        Double Resultado = (Convert.ToDouble(Cantidad)) * (Convert.ToDouble(Costo));
        Ds_Reporte.Tables[0].ImportRow(Renglon);
        Ds_Reporte.Tables[0].Rows[0].SetField("COSTO_TOTAL", Resultado);

        for (int Cont_Elementos = 0; Cont_Elementos < Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows.Count; Cont_Elementos++)
        {
            Renglon = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[Cont_Elementos]; //Instanciar renglon e importarlo
            Ds_Reporte.Tables[1].ImportRow(Renglon);

            String Nombre_E = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[Cont_Elementos]["NOMBRE_E"].ToString();
            String Apellido_Paterno_E = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[Cont_Elementos]["APELLIDO_PATERNO_E"].ToString();
            String Apellido_Materno_E = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[Cont_Elementos]["APELLIDO_MATERNO_E"].ToString();
            String RFC_E = Ds_Consulta_Resguardos_Cemovientes.Tables[0].Rows[Cont_Elementos]["RFC_E"].ToString();
            String Resguardante = Nombre_E + " " + Apellido_Paterno_E + " " + Apellido_Materno_E + " " + "(" + RFC_E + ")";
            Ds_Reporte.Tables[1].Rows[Cont_Elementos].SetField("RESGUARDANTES", Resguardante);
        }

        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Resguardos Bienes.xls");
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.Excel;
        Reporte.Export(Export_Options);
        String Ruta = "../../Reporte/Resguardo Cemovientes.pdf";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }

    #endregion

#endregion

#region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_PDF_Click
    ///DESCRIPCIÓN: Manda llamar los datos para cergarlos en el reporte dependiendo de
    ///             los filtros seleccionados.
    ///PARAMETROS:  
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Generar_Reporte_PDF_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Txt_No_Inventario.Text.Trim().Length > 0)
            {
                String No_Inventario = Txt_No_Inventario.Text.Trim();
                Cls_Rpt_Pat_Detalles_Bien_Mueble_Negocio Reporte = new Cls_Rpt_Pat_Detalles_Bien_Mueble_Negocio();
                Reporte.P_No_Inventario = No_Inventario;
                String Tipo = Reporte.Obtener_Tipo_Bien();
                if (Tipo != null)
                {
                    if (Tipo.Equals("BIEN_MUEBLE"))
                    {
                        Llenar_DataSet_Resguardos_Bienes(Reporte.P_Bien_Mueble_Detalles, "Pdf");
                    }
                    else if (Tipo.Equals("VEHICULO"))
                    {
                        Llenar_DataSet_Resguardos_Vehiculos(Reporte.P_Vehiculo_Detalles, "Pdf");
                    }
                    else if (Tipo.Equals("ANIMAL"))
                    {
                        Llenar_DataSet_Resguardos_Cemovientes(Reporte.P_Cemoviente_Detalles, "Pdf");
                    }
                    else
                    {
                        throw new Exception("Error: [El Bien con el No. de Inventario '" + No_Inventario + "' No fue encontrado en la Base de Datos]");
                    }
                }
                else
                {
                    throw new Exception("Error: [El Bien con el No. de Inventario '" + No_Inventario + "' No fue encontrado en la Base de Datos]");
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('Se debe introducir el No de Inventario del Bien');", true);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Error:";
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_Excel_Click
    ///DESCRIPCIÓN: Manda llamar los datos para cergarlos en el reporte dependiendo de
    ///             los filtros seleccionados.
    ///PARAMETROS:  
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO:  30/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Generar_Reporte_Excel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Txt_No_Inventario.Text.Trim().Length > 0)
            {
                String No_Inventario = Txt_No_Inventario.Text.Trim();
                Cls_Rpt_Pat_Detalles_Bien_Mueble_Negocio Reporte = new Cls_Rpt_Pat_Detalles_Bien_Mueble_Negocio();
                Reporte.P_No_Inventario = No_Inventario;
                String Tipo = Reporte.Obtener_Tipo_Bien();
                if (Tipo != null)
                {
                    if (Tipo.Equals("BIEN_MUEBLE"))
                    {
                        Llenar_DataSet_Resguardos_Bienes(Reporte.P_Bien_Mueble_Detalles, "Excel");
                    }
                    else if (Tipo.Equals("VEHICULO"))
                    {
                        Llenar_DataSet_Resguardos_Vehiculos(Reporte.P_Vehiculo_Detalles, "Excel");
                    }
                    else if (Tipo.Equals("ANIMAL"))
                    {
                        Llenar_DataSet_Resguardos_Cemovientes(Reporte.P_Cemoviente_Detalles, "Excel");
                    }
                    else
                    {
                        throw new Exception("Error: [El Bien con el No. de Inventario '" + No_Inventario + "' No fue encontrado en la Base de Datos]");
                    }
                }
                else
                {
                    throw new Exception("Error: [El Bien con el No. de Inventario '" + No_Inventario + "' No fue encontrado en la Base de Datos]");
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('Se debe introducir el No de Inventario del Bien');", true);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Error:";
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

#endregion

}
