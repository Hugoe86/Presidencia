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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Xml.Linq;
using Presidencia.Reportes_Contrarecibos.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Reportes;

public partial class paginas_Almacen_Frm_Ope_Alm_Reportes_Contrarecibos : System.Web.UI.Page
{

    #region Variables
    Cls_Ope_Com_Alm_Rpts_Contrarecibos_Negocio Consulta_Contrarecibos = new Cls_Ope_Com_Alm_Rpts_Contrarecibos_Negocio();
    #endregion

    #region Evento Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Llenar_Combos();
        }
    }
    #endregion


    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combos
    ///DESCRIPCIÓN:          Método utilizado para instanciar los métodos que llenan los combos
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           06/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combos()
    {
        Llenar_Combo_Contrarecibos();
        Llenar_Combo_Proveedores();
        Llenar_Combo_Usuarios();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Contrarecibos
    ///DESCRIPCIÓN:          Método utilizado para llenar el combo con los numeros de 
    ///                      contra recibos generados
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           06/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Contrarecibos()
    {
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Consulta_Contrarecibos.P_Nombre_Tabla = "CONTRARECIBOS";
            Dt_Consulta = Consulta_Contrarecibos.Consultar_Tablas();

            if (Dt_Consulta.Rows.Count > 0)
            {
                Cmb_ContraRecibos.DataSource = Dt_Consulta;
                Cmb_ContraRecibos.DataTextField = Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno;
                Cmb_ContraRecibos.DataValueField = Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno;
                Cmb_ContraRecibos.DataBind();
                Cmb_ContraRecibos.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
                Cmb_ContraRecibos.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Usuarios
    ///DESCRIPCIÓN:          Método utilizado para llenar el combo con los usuarios que 
    ///                      han generado contra recibos
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           06/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Usuarios()
    {
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Consulta_Contrarecibos.P_Nombre_Tabla = "USUARIOS";
            Dt_Consulta = Consulta_Contrarecibos.Consultar_Tablas();

            if (Dt_Consulta.Rows.Count > 0)
            {
                Cmb_Usuarios.DataSource = Dt_Consulta;
                Cmb_Usuarios.DataTextField = "EMPLEADO";
                Cmb_Usuarios.DataValueField = Cat_Empleados.Campo_Empleado_ID;
                Cmb_Usuarios.DataBind();
                Cmb_Usuarios.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
                Cmb_Usuarios.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Proveedores
    ///DESCRIPCIÓN:          Método utilizado para llenar el combo con los proveedores
    ///                      que se les ha generado el contra recibo
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           06/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Proveedores()
    {
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Consulta_Contrarecibos.P_Nombre_Tabla = "PROVEEDORES";
            Dt_Consulta = Consulta_Contrarecibos.Consultar_Tablas();

            if (Dt_Consulta.Rows.Count > 0)
            {
                Cmb_Proveedores.DataSource = Dt_Consulta;
                Cmb_Proveedores.DataTextField = "PROVEEDOR";
                Cmb_Proveedores.DataValueField = Cat_Com_Proveedores.Campo_Proveedor_ID;
                Cmb_Proveedores.DataBind();
                Cmb_Proveedores.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
                Cmb_Proveedores.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Opciones_Consulta
    ///DESCRIPCIÓN:          Método utilizado para validar que el usuario seleccione los
    ///                      combos, una vez que ha seleccionado los CheckBox
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           06/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public bool Validar_Opciones_Consulta()
    {
        Boolean Validacion = true;
        Lbl_Informacion.Text = "Es necesario.";
        String Mensaje_Error = "";

        if (Ckb_No_ContraRecibo.Checked)
        {
            if (Cmb_ContraRecibos.SelectedIndex == 0)
            {
                if (!Validacion)
                {
                    Mensaje_Error = Mensaje_Error + "<br>";
                }
                Mensaje_Error = Mensaje_Error + "+ Seleccionar una opción del combo No. Contra Recibo.";
                Validacion = false;
            }
        }

        if (Ckb_Proveedor.Checked)
        {
            if (Cmb_Proveedores.SelectedIndex == 0)
            {
                if (!Validacion)
                {
                    Mensaje_Error = Mensaje_Error + "<br>";
                }
                Mensaje_Error = Mensaje_Error + "+ Seleccionar una opción del combo Proveedor.";
                Validacion = false;
            }
        }

        if (Ckb_Usuario_Genero.Checked)
        {
            if (Cmb_Usuarios.SelectedIndex == 0)
            {
                if (!Validacion)
                {
                    Mensaje_Error = Mensaje_Error + "<br>";
                }
                Mensaje_Error = Mensaje_Error + "+ Seleccionar una opción del combo Usuario Generó.";
                Validacion = false;
            }
        }

        if (!Validacion)
        {
            Lbl_Informacion.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }

        return Validacion;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN:          Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///                      en la busqueda del Modalpopup
    ///PARAMETROS:   
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           06/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public bool Verificar_Fecha()
    {
        DateTime Date1 = new DateTime();  //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date2 = new DateTime();
        Boolean Fecha_Valida = true;

        try
        {
            if (Ckb_Fecha_Recepcion.Checked)
            {
                if ((Txt_Fecha_Inicial_B.Text.Length != 0))
                {
                    // Convertimos el Texto de los TextBox fecha a dateTime
                    Date1 = DateTime.Parse(Txt_Fecha_Inicial_B.Text);
                    Date2 = DateTime.Parse(Txt_Fecha_Final_B.Text);

                    //Validamos que las fechas sean iguales o la final sea mayor que la inicial, de lo contrario se manda un mensaje de error 
                    if ((Date1 < Date2) | (Date1 == Date2))
                    {
                        Fecha_Valida = true;
                    }
                    else
                    {
                        Lbl_Informacion.Text = " La fecha inicial no pude ser mayor que la fecha final <br />";
                        Fecha_Valida = false;
                    }
                }
            }
            return Fecha_Valida;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN:          Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:           1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                      en caso de que cumpla la condicion del if
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           06/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Formato_Fecha(String Fecha)
    {
        String Fecha_Valida = Fecha;
        //Se le aplica un split a la fecha 
        String[] aux = Fecha.Split('/');
        //Se modifica a mayusculas para que oracle acepte el formato. 
        switch (aux[1])
        {
            case "dic":
                aux[1] = "DEC";
                break;
        }
        //Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
        Fecha_Valida = aux[0] + "-" + aux[1] + "-" + aux[2];
        return Fecha_Valida;
    }




    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_DataSet_Reporte
    ///DESCRIPCIÓN:          Metodo utilizado para llenar el Dataset e instanciar el método Exportar_Reporte
    ///PARAMETROS:           1.- DataTable Dt_Consulta, Esta tabla contiene los datos de la 
    ///                          consulta que se realizó a la base de datos
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           06/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_DataSet_Reporte(DataTable Dt_Consulta, DataSet Ds_Contrarecibos,  String Formato )
    {
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        DataRow Renglon;
        String Usuario = Cls_Sessiones.Nombre_Empleado;

        int Cont_Elementos = 0;
        try
        {
            // Se llena la tabla Detalles del DataSet
            for (Cont_Elementos = 0; Cont_Elementos < Dt_Consulta.Rows.Count; Cont_Elementos++)
            {
                Renglon = Dt_Consulta.Rows[Cont_Elementos]; // Instanciar renglon e importarlo
                Ds_Contrarecibos.Tables[0].ImportRow(Renglon);
            }
            Ds_Contrarecibos.Tables[0].Rows[Cont_Elementos - 1].SetField("USUARIO_IMPRIMIO", Usuario);

            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Almacen/Rpt_Alm_Com_Rep_Contrarecibos.rpt";

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Rpt_Contrarecibos_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            // Se da el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar

            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Contrarecibos, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
            Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
            
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }





    /// *************************************************************************************
    /// NOMBRE:              Mostrar_Reporte
    /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
    /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
    ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
    /// USUARIO CREO:        Juan Alberto Hernández Negrete.
    /// FECHA CREO:          3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Salvador Hernández Ramírez
    /// FECHA MODIFICO:      17-Mayo-2011
    /// CAUSA MODIFICACIÓN:  Se asigno la opción para que en el mismo método se muestre el reporte en excel
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte_Generar, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
                "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }



    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Exportar_Reporte
    /////DESCRIPCIÓN:          Metodo utilizado para exportar el reporte y mostrarlo en formato PDF
    /////PARAMETROS:           1.- Ds_Reporte: Es el DataSet que se ha creado 
    /////                      2.- Nombre_Reporte: Es el nombre del reporte que se ha creado
    /////                      3.- Nombre_Archivo: Es el nombre del archivo en pdf o excel  
    /////CREO:                 Salvador Hernández Ramírez
    /////FECHA_CREO:           06/Mayo/2011 
    /////MODIFICO:
    /////FECHA_MODIFICO:
    /////CAUSA_MODIFICACIÓN:
    /////*******************************************************************************
    //private void Generar_Reporte(DataSet Ds_Reporte, String Nombre_Reporte_Crystal, String Nombre_Archivo_Generar)
    //{
    //    ReportDocument Reporte = new ReportDocument();

    //    String File_Path = Server.MapPath("..\\..\\App_Code/reportes/Almacen/" + Nombre_Reporte);
    //    Reporte.Load(File_Path);
    //    String Archivo_PDF = Nombre_Archivo;  // Es el nombre del archivo PDF  o Excel que se va a generar
    //    Reporte.SetDataSource(Ds_Reporte);

    //    ExportOptions Export_Options = new ExportOptions();
    //    DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
    //    Disk_File_Destination_Options.DiskFileName = Server.MapPath("~/Reporte/" + Archivo_PDF);
    //    Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
    //    Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
    //    Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;

    //    Reporte.Export(Export_Options);
    //    String Ruta = "../../Reporte/" + Archivo_PDF;
    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    //}


    #endregion


    #region Eventos

    protected void Ckb_Fecha_Recepcion_CheckedChanged(object sender, EventArgs e)
    {
        if (Ckb_Fecha_Recepcion.Checked)
        {
            Btn_Calendar_Fecha_Final.Enabled = true;
            Btn_Calendar_Fecha_Inicial.Enabled = true;
            Txt_Fecha_Inicial_B.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Final_B.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        }
        else
        {
            Btn_Calendar_Fecha_Final.Enabled = false;
            Btn_Calendar_Fecha_Inicial.Enabled = false;
            Txt_Fecha_Inicial_B.Text = "";
            Txt_Fecha_Final_B.Text = "";
        }
    }
    
    protected void Ckb_No_ContraRecibo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Ckb_No_ContraRecibo.Checked)
            {
                Cmb_ContraRecibos.Enabled = true;
            }
            else
                Cmb_ContraRecibos.Enabled = false;

            if (Cmb_ContraRecibos.Items.Count > 0)
                Cmb_ContraRecibos.SelectedIndex = 0;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    protected void Ckb_Proveedor_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Ckb_Proveedor.Checked)
            {
                Cmb_Proveedores.Enabled = true;
            }
            else
                Cmb_Proveedores.Enabled = false;

            if (Cmb_Proveedores.Items.Count > 0)
                Cmb_Proveedores.SelectedIndex = 0;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    protected void Ckb_Usuario_Genero_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Ckb_Usuario_Genero.Checked)
            {
                Cmb_Usuarios.Enabled = true;
            }
            else
                Cmb_Usuarios.Enabled = false;

            if (Cmb_Usuarios.Items.Count > 0)
                Cmb_Usuarios.SelectedIndex = 0;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    public void Realizar_Consulta(String Formato){

        DataTable Dt_Consulta = new DataTable();
        Boolean consulta = true;

        Ds_Alm_Com_Rep_Contrarecibos Ds_Contrarecibos= new Ds_Alm_Com_Rep_Contrarecibos();

        try
        {
            if (Validar_Opciones_Consulta())
            {
                if ((Ckb_No_ContraRecibo.Checked) && (Cmb_ContraRecibos.SelectedIndex != 0))
                    Consulta_Contrarecibos.P_No_ContraRecibo = Cmb_ContraRecibos.SelectedValue.Trim();

                else
                    Consulta_Contrarecibos.P_No_ContraRecibo = null;


                if ((Ckb_Proveedor.Checked) && (Cmb_Proveedores.SelectedIndex != 0))
                    Consulta_Contrarecibos.P_Proveedor_ID = Cmb_Proveedores.SelectedValue.Trim();
                else
                    Consulta_Contrarecibos.P_Proveedor_ID = null;


                if ((Ckb_Usuario_Genero.Checked) && (Cmb_Usuarios.SelectedIndex != 0))
                    Consulta_Contrarecibos.P_Usuario_Genero = Cmb_Usuarios.SelectedValue.Trim();
                else
                    Consulta_Contrarecibos.P_Usuario_Genero = null;


                if (Ckb_Fecha_Recepcion.Checked)
                {
                    if (!Verificar_Fecha())
                    {
                        Div_Contenedor_Msj_Error.Visible = true;
                        Consulta_Contrarecibos.P_Fecha_Inicial = null;
                        Consulta_Contrarecibos.P_Fecha_Final = null;
                        consulta = false;
                    }
                    else
                    {
                        Consulta_Contrarecibos.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicial_B.Text.Trim());
                        Consulta_Contrarecibos.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Final_B.Text.Trim());
                    }
                }

                if (consulta == true)
                {
                    // Se realiza la consulta
                    Dt_Consulta = Consulta_Contrarecibos.Consultar_Contra_Recibos();

                    if (Dt_Consulta.Rows.Count > 0)
                    {
                        Llenar_DataSet_Reporte(Dt_Consulta, Ds_Contrarecibos, Formato); // Se instancia el método que llena el DataSet
                        Div_Contenedor_Msj_Error.Visible = false;
                    }
                    else
                    {
                        Lbl_Informacion.Text = "No se encontraron contra recibos";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }

    }

    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        String Formato = "PDF";
        Realizar_Consulta(Formato);
    }

    protected void Btn_Imprimir_Excel_Click(object sender, ImageClickEventArgs e)
    {
        String Formato = "Excel";
        Realizar_Consulta(Formato);

    }

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }

    #endregion
  
}
