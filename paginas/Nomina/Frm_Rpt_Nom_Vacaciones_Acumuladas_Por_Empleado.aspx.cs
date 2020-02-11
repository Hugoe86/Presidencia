using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Vacaciones_Acumuladas_Por_Empleado.Negocio;
using System.Data;
using Presidencia.Constantes;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;


public partial class paginas_Nomina_Frm_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado : System.Web.UI.Page
{
    ///*******************************************************************************
    ///PAGE_LOAD
    ///*******************************************************************************
    ///
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Configurar_Formulario("Inicio");

        }
    }
    #endregion

    ///*******************************************************************************
    ///METODOS
    ///*******************************************************************************
    ///
    #region Metodos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configurar_Formulario
    ///DESCRIPCIÓN: Metodo que configura el formulario con respecto al estado de habilitado o visible
    ///´de los componentes de la pagina
    ///PARAMETROS: 1.- String Estatus: Estatus que puede tomar el formulario con respecto a sus componentes, ya sea "Inicio" o "Nuevo"
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Abr/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Configurar_Formulario(String Estatus)
    {
        switch (Estatus)
        {
            case "Inicio":
                //Limpiamos los datos de la catorcena 
                Txt_Buscar_Empleado.Text = "";
                Cmb_Empleado.Items.Clear();
                //Consultamos todas las unidades Responsabless. 
                Cmb_Unidad_Responsable.Items.Clear();
                Cls_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado_Negocio Clase_Negocio = new Cls_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado_Negocio();
                DataTable Dt_Unidades_Responsables = Clase_Negocio.Consultar_Unidades_Responsables();
                Cls_Util.Llenar_Combo_Con_DataTable_Generico
               (Cmb_Unidad_Responsable, Dt_Unidades_Responsables, Cat_Dependencias.Campo_Nombre, Cat_Dependencias.Campo_Dependencia_ID);
                Cmb_Unidad_Responsable.SelectedIndex = 0;
                //llenamos el Combo de nomina
                Llenar_Combo_Nomina();
                
                Llenar_Combo_Tipo_Nomina();
                break;

        }
    }

    public void Llenar_Combo_Tipo_Nomina()
    {
        Cls_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado_Negocio Clase_Negocio = new Cls_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado_Negocio();
        DataTable Dt_Tipo_Nomina = Clase_Negocio.Consultar_Tipo_Nomina();
        Cls_Util.Llenar_Combo_Con_DataTable_Generico
               (Cmb_Tipo_Nomina, Dt_Tipo_Nomina, Cat_Nom_Tipos_Nominas.Campo_Nomina, Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);
        Cmb_Tipo_Nomina.SelectedIndex = 0;
    }


    public void Llenar_Combo_Nomina()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nominales = new Cls_Cat_Nom_Calendario_Nominas_Negocio();
        try
        {
            DataTable Dt_Nominas = Obj_Calendario_Nominales.Consultar_Calendario_Nominas();
            Dt_Nominas = Formato_Fecha_Calendario_Nomina(Dt_Nominas);

            if (Dt_Nominas is DataTable)
            {
                Cmb_Anio.Items.Clear();
                Cmb_Anio.DataSource = Dt_Nominas;
                Cmb_Anio.DataTextField = "Nomina";
                Cmb_Anio.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                Cmb_Anio.DataBind();
                Cmb_Anio.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
                Cmb_Anio.SelectedIndex = -1;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los calendarios de nómina que existen actualmente registrados en el sistema. Error: [" + Ex.Message + "]");
        }

    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
    /// DESCRIPCION : Crea el DataTable con la consulta de las nomina vigentes en el 
    /// sistema.
    /// PARAMETROS: Dt_Calendario_Nominas.- Lista de las nominas vigentes actualmente 
    ///             en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Formato_Fecha_Calendario_Nomina(DataTable Dt_Calendario_Nominas)
    {
        DataTable Dt_Nominas = new DataTable();
        DataRow Renglon_Dt_Clon = null;
        Dt_Nominas.Columns.Add("Nomina", typeof(System.String));
        Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

        if (Dt_Calendario_Nominas is DataTable)
        {
            foreach (DataRow Renglon in Dt_Calendario_Nominas.Rows)
            {
                if (Renglon is DataRow)
                {
                    Renglon_Dt_Clon = Dt_Nominas.NewRow();
                    Renglon_Dt_Clon["Nomina"] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString().Split(new char[] { ' ' })[0].Split(new char[] { '/' })[2];
                    Renglon_Dt_Clon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID];
                    Dt_Nominas.Rows.Add(Renglon_Dt_Clon);
                }
            }
        }
        return Dt_Nominas;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales para el 
    ///calendario de nomina seleccionado.
    ///PARAMETROS: P_Nomina_ID.- Indica el calendario de nomina del cuál se desea consultar
    ///                        los periodos catorcenales.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Periodos_Catorcenales_Nomina(String Nomina_ID)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nomina_Periodos = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Clase de conexion con la capa de negocios
        DataTable Dt_Periodos_Catorcenales = null;//Variable que almacenra unaa lista de los periodos catorcenales que le correspónden a la nomina seleccionada.

        try
        {
            Consulta_Calendario_Nomina_Periodos.P_Nomina_ID = Nomina_ID;
            Dt_Periodos_Catorcenales = Consulta_Calendario_Nomina_Periodos.Consulta_Detalles_Nomina();
            if (Dt_Periodos_Catorcenales != null)
            {
                if (Dt_Periodos_Catorcenales.Rows.Count > 0)
                {
                    Cmb_Periodo.DataSource = Dt_Periodos_Catorcenales;
                    Cmb_Periodo.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodo.DataValueField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodo.DataBind();
                    Cmb_Periodo.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Periodo.SelectedIndex = -1;

                    Validar_Periodos_Pago(Cmb_Periodo);
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron periodos catorcenales para la nomina seleccionada.";
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los periodos catorcenales del  calendario de nomina seleccionado. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la deduccion 
    ///a partir del periodo actual.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Validar_Periodos_Pago(DropDownList Combo)
    {
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Actual = DateTime.Now;
        DateTime Fecha_Inicio = new DateTime();
        DateTime Fecha_Fin = new DateTime();

        Prestamos.P_Nomina_ID = Cmb_Anio.SelectedValue.Trim();

        foreach (ListItem Elemento in Combo.Items)
        {
            if (IsNumeric(Elemento.Text.Trim()))
            {
                Prestamos.P_No_Nomina = Convert.ToInt32(Elemento.Text.Trim());
                Dt_Detalles_Nomina = Prestamos.Consultar_Fechas_Periodo();

                if (Dt_Detalles_Nomina != null)
                {
                    if (Dt_Detalles_Nomina.Rows.Count > 0)
                    {
                        Fecha_Inicio = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString());
                        Fecha_Fin = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString());
                    }
                }
            }
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Mayo/2011 12:20p.m.
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean IsNumeric(String Cadena)
    {
        Boolean Resultado = true;
        Char[] Array = Cadena.ToCharArray();
        try
        {
            for (int index = 0; index < Array.Length; index++)
            {
                if (!Char.IsDigit(Array[index])) return false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: caraga el data set fisoco con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/Mayo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte, string Nombre_PDF)
    {

        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Nomina/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Ds_Reporte = Data_Set_Consulta_DB;
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Nombre_PDF);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        Reporte.Export(Export_Options);
        String Ruta = "../../Reporte/" + Nombre_PDF;
        Mostrar_Reporte(Nombre_PDF, "PDF");
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }

    /// *************************************************************************************
    /// NOMBRE:              Mostrar_Reporte
    /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
    /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
    ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
    /// USUARIO CREO:        Juan Alberto Hernández Negrete.
    /// FECHA CREO:          3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Salvador Hernández Ramírez
    /// FECHA MODIFICO:      16-Mayo-2011
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

    #endregion

    ///*******************************************************************************
    ///GRID
    ///*******************************************************************************
    ///
    #region Grid

    #endregion

    ///*******************************************************************************
    ///EVENTOS
    ///*******************************************************************************
    #region Eventos
    protected void Btn_Buscar_Empleado_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Txt_Buscar_Empleado.Text.Trim() != String.Empty)
        {
            Cls_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado_Negocio Clase_Negocio = new Cls_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado_Negocio();
            Clase_Negocio.P_No_Empleado = Txt_Buscar_Empleado.Text.Trim();
            //Consultamos el Empleado
            DataTable Dt_Empleado = Clase_Negocio.Consultar_Empleado();
            if (Dt_Empleado.Rows.Count > 0)
            {
                Cls_Util.Llenar_Combo_Con_DataTable_Generico
                    (Cmb_Empleado, Dt_Empleado, Cat_Empleados.Campo_Nombre, Cat_Empleados.Campo_Empleado_ID);
                Cmb_Empleado.SelectedIndex = 1;
            }
            else
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontro el empleado con ese Num Empleado";
            }
        }
        else
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "";
        }
    }
    protected void Btn_PDF_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Cls_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado_Negocio Clase_Negocio = new Cls_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado_Negocio();

        if (Cmb_Empleado.SelectedIndex >0)
        {
            Clase_Negocio.P_Empleado_ID = Cmb_Empleado.SelectedValue;
        }

        if (Cmb_Tipo_Nomina.SelectedIndex > 0)
        {
            Clase_Negocio.P_Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue;

        }

        if (Cmb_Anio.SelectedIndex > 0)
        {
            Clase_Negocio.P_Anio = Cmb_Anio.SelectedValue;
        }

        if (Cmb_Periodo.SelectedIndex > 0)
        {
            Clase_Negocio.P_Periodo_Vacacional = Cmb_Periodo.SelectedValue;
        }

        DataTable Dt_Vacaciones_Empleado = Clase_Negocio.Consultar_Vacaciones();
        if (Dt_Vacaciones_Empleado.Rows.Count > 0)
        {
            //Creamos el data set 
            DataSet Ds_Imprimir_Reporte = new DataSet();
            Ds_Imprimir_Reporte.Tables.Add(Dt_Vacaciones_Empleado.Copy());
            Ds_Imprimir_Reporte.Tables[0].TableName = "DT_VACACIONES";
            Ds_Imprimir_Reporte.AcceptChanges();
            Ds_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado Obj_Rpt_Vacaciones = new Ds_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado();
            Generar_Reporte(Ds_Imprimir_Reporte, Obj_Rpt_Vacaciones, "Cr_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado.rpt", "Cr_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado.pdf");


        }
        else
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se encontraron registros";
        }
    }


   
    #endregion


   
}
