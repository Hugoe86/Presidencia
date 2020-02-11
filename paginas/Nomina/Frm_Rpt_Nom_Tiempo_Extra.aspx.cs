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
using CrystalDecisions.Web;
using CrystalDecisions.ReportSource;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Reflection;
using Presidencia.Nomina_Reporte_Retardos_Faltas.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Tipos_Nominas.Negocios;
using CarlosAg.ExcelXmlWriter;
using Presidencia.Ayudante_CarlosAG;
using System.Text;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Ayudante_Informacion;
using Presidencia.Prestamos.Negocio;
using Presidencia.Calendario_Reloj_Checador.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Nomina_Reporte_Tiempo_Extra.Negocio;

public partial class paginas_Nomina_Frm_Rpt_Nom_Tiempo_Extra : System.Web.UI.Page
{
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Consultar_Tipos_Nominas();
                Consultar_Calendarios_Nomina();
                Cmb_Calendario_Nomina.Focus();
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region Metodos
    #region Metodos Generales
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 22/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Txt_No_Empleado.Text = "";
            Txt_Nombre_Empleado.Text = "";

            if (Cmb_Nombre_Empleado.SelectedIndex > 0)
                Cmb_Nombre_Empleado.SelectedIndex = 0;

            if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex > 0)
                Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = 0;

            if (Cmb_Calendario_Nomina.SelectedIndex > 0)
                Cmb_Calendario_Nomina.SelectedIndex = 0;

        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
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

    #endregion

    #region Validacion
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la deduccion 
    ///             a partir del periodo actual.
    ///CREO       : Juan alberto Hernández Negrete
    ///FECHA_CREO : 06/Abril/2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Validar_Periodos_Pago(DropDownList Combo)
    {
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Actual = DateTime.Now;
        DateTime Fecha_Inicio = new DateTime();
        DateTime Fecha_Fin = new DateTime();

        Prestamos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

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
    /// NOMBRE DE LA FUNCION: Validar_Reporte
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el reporte
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 21/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Reporte()
    {
        String Espacios_Blanco;
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario: <br>";
        Lbl_Mensaje_Error.Visible = true;
        Img_Error.Visible = true;

        if (Cmb_Calendario_Nomina.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Selecciona la nomina.<br>";
            Datos_Validos = false;
        }
        if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Selecciona el periodo de la nomina.<br>";
            Datos_Validos = false;
        }
        return Datos_Validos;
    }

    #endregion

    #region Consultas
    /// *************************************************************************************
    /// NOMBRE: Consultar_Tipos_Nominas
    /// 
    /// DESCRIPCIÓN: Consulta los tipos de nómina que se encuantran dadas de alta 
    ///              actualmente en sistema.
    ///              
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 10:52 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Consultar_Tipos_Nominas()
    {
        Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Tipos_Nominas = null;//Variable que almacena la lista de tipos de nominas. 
        try
        {
            Dt_Tipos_Nominas = Obj_Tipos_Nominas.Consulta_Tipos_Nominas();//Consulta los tipos de nominas.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cosnultar los tipos de nomina que existen actualemte en sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Calendarios_Nomina
    /// DESCRIPCION :
    /// PARAMETROS  :
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Calendarios_Nomina()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nominales = new Cls_Cat_Nom_Calendario_Nominas_Negocio(); //Variable de conexión con la capa de negocios.
        DataTable Dt_Calendarios_Nominales = null; //Variable que almacena los calendarios nominales que existén actualmente en el sistema.
        try
        {
            Dt_Calendarios_Nominales = Obj_Calendario_Nominales.Consultar_Calendario_Nominas();
            Dt_Calendarios_Nominales = Formato_Fecha_Calendario_Nomina(Dt_Calendarios_Nominales);

            if (Dt_Calendarios_Nominales is DataTable)
            {
                Cmb_Calendario_Nomina.DataSource = Dt_Calendarios_Nominales;
                Cmb_Calendario_Nomina.DataTextField = "Nomina";
                Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                Cmb_Calendario_Nomina.DataBind();
                Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));

                Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf
                    (Cmb_Calendario_Nomina.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Anyo));

                if (Cmb_Calendario_Nomina.SelectedIndex > 0)
                {
                    Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
                }
            }            
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los calendarios de nómina que existen actualmente registrados en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales para el 
    ///calendario de nomina seleccionado.
    ///PARAMETROS: Nomina_ID.- Indica el calendario de nomina del cuál se desea consultar
    ///                        los periodos catorcenales.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Periodos_Catorcenales_Nomina(String Nomina_ID)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
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
                    Cmb_Periodos_Catorcenales_Nomina.DataSource = Dt_Periodos_Catorcenales;
                    Cmb_Periodos_Catorcenales_Nomina.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataValueField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataBind();
                    Cmb_Periodos_Catorcenales_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;

                    Validar_Periodos_Pago(Cmb_Periodos_Catorcenales_Nomina);

                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Periodos_Catorcenales_Nomina.Items.IndexOf(Cmb_Periodos_Catorcenales_Nomina.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Periodo));

                    Int32 index = Cmb_Periodos_Catorcenales_Nomina.SelectedIndex;
                    Hdn_Fecha_Inicial.Value = "";
                    Hdn_Fecha_Final.Value = "";
                    if (index > 0)
                    {
                        Consulta_Fechas_Periodo_Nominal(); //Consulta la fecha de inicio y termino para la generación de asistencias del empleado
                    }                    
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
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
    ///NOMBRE DE LA FUNCIÓN: Consulta_Fechas_Periodo_Nominal
    ///DESCRIPCIÓN: Consulta las fecchas del periodo nominal que fue seleccionado por
    ///             el usuario para poder realizar las asistencias de los empleados
    ///CREO       : Yazmin A Delgado Gómez
    ///FECHA_CREO : 05-Octubre-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Consulta_Fechas_Periodo_Nominal()
    {
        Cls_Cat_Nom_Calendario_Reloj_Checador_Negocio Rs_Consulta_Cat_Nom_Calendario_Reloj_Checador = new Cls_Cat_Nom_Calendario_Reloj_Checador_Negocio(); //Variable de conexión hacia la capa de negocios
        DataTable Dt_Periodo_Nominal; //Obtiene los valores de la consulta y servira para poder asignar estos a los controles correspondientes
        try
        {
            Rs_Consulta_Cat_Nom_Calendario_Reloj_Checador.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.ToString();
            Rs_Consulta_Cat_Nom_Calendario_Reloj_Checador.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.ToString());
            Dt_Periodo_Nominal = Rs_Consulta_Cat_Nom_Calendario_Reloj_Checador.Consulta_Fechas_Calendario_Reloj_Checador(); //Consulta las fechas de inicio y fin del periodo nominal seleccionado por el usuario

            //Asigna los valores obtenidos de la consulta a los controles correspondientes
            foreach (DataRow Registro in Dt_Periodo_Nominal.Rows)
            {
                if (!String.IsNullOrEmpty(Registro[Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio].ToString()))
                {
                    Hdn_Fecha_Inicial.Value = Registro[Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio].ToString();
                    Hdn_Fecha_Final.Value = Registro[Cat_Nom_Calendario_Reloj.Campo_Fecha_Fin].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Fechas_Periodo_Nominal " + ex.Message.ToString());
        }
    }
    #endregion

    #region Reporte
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Generar_Reporte_Tiempo_Extra
    /// DESCRIPCION : Consulta el personal que trabajo tiempo extra
    /// PARAMETROS  : String Formato.- Para saber que formato sera el archivo pdf, excel
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 21/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Generar_Reporte_Tiempo_Extra(String Formato)
    {
        Cls_Rpt_Nom_Retardos_Faltas_Negocio Rs_Faltas = new Cls_Rpt_Nom_Retardos_Faltas_Negocio();
        Cls_Rpt_Nom_Tiempo_Extra_Negocio Rs_Tiempo_Extra = new Cls_Rpt_Nom_Tiempo_Extra_Negocio();
        DataTable Dt_Reporte = new DataTable();
        DataTable Dt_Consulta = new DataTable();
        DataTable Dt_Periodo = new DataTable();
        DataTable Dt_Auxiliar = new DataTable();
        String Empleado_ID = "";
        DataRow Dt_Row;
        Double Tipo_Nomina = 0.0;
        Ds_Rpt_Nom_Tiempo_Extra Ds_Reporte = new Ds_Rpt_Nom_Tiempo_Extra();
        ReportDocument Reporte = new ReportDocument();
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Reporte_Horas_Extras_" + Session.SessionID; //Obtiene el nombre del archivo que sera asignado al documento
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            
            //  se busca el empleado id a partir del numero de empleado
            if (Txt_No_Empleado.Text != "")
            {
                Rs_Faltas.P_No_Empleado = Txt_No_Empleado.Text;
                Dt_Auxiliar = Rs_Faltas.Consultar_Informacion_Empleado();
                Rs_Faltas.P_No_Empleado = null;

                if (Dt_Auxiliar is DataTable)
                {
                    if (Dt_Auxiliar.Rows.Count > 0)
                    {
                        foreach (DataRow Dt_Row_Empleado in Dt_Auxiliar.Rows)
                        {
                            if (Dt_Row_Empleado is DataRow)
                            {
                                Empleado_ID = Dt_Row_Empleado[Cat_Empleados.Campo_Empleado_ID].ToString();
                            }
                        }
                    }
                }
            }

            //  se realiza la consulta
            Rs_Tiempo_Extra.P_Nomina_id = Cmb_Calendario_Nomina.SelectedValue;
            Rs_Tiempo_Extra.P_No_Nomina = Cmb_Periodos_Catorcenales_Nomina.SelectedValue;
            
            if (!String.IsNullOrEmpty(Empleado_ID))
                Rs_Tiempo_Extra.P_Empleado_ID = Empleado_ID;

            Dt_Consulta = Rs_Tiempo_Extra.Consultar_Tiempo_Extra();

            Dt_Reporte = Construir_Tabla();
            Dt_Periodo =Construir_Tabla_Periodo();

            Dt_Row = Dt_Periodo.NewRow();
            Dt_Row["FECHA_INICIAL"] = Hdn_Fecha_Inicial.Value;
            Dt_Row["FECHA_FINAL"] = Hdn_Fecha_Final.Value;
            Dt_Row["NOMINA_ID"] = Cmb_Calendario_Nomina.SelectedItem.Text;
            Dt_Row["NO_NOMINA"] = Cmb_Periodos_Catorcenales_Nomina.SelectedItem.Value;
            Dt_Row["TIPO_REPORTE"] = "TIEMPO EXTRA";
            Dt_Row["TIPO_COLUMNA"] = "HORAS";
            //  para ingresar la informacion al DataTable
            Dt_Periodo.Rows.Add(Dt_Row);


            if (Dt_Consulta is DataTable)
            {
                if (Dt_Consulta.Rows.Count > 0)
                {
                    foreach (DataRow Dt_Row_Tiempo_Extra in Dt_Consulta.Rows)
                    {
                        if (Dt_Row_Tiempo_Extra is DataRow)
                        {
                            Dt_Row = Dt_Reporte.NewRow();
                            
                            Dt_Row["NO_EMPLEADO"] = Dt_Row_Tiempo_Extra[Cat_Empleados.Campo_No_Empleado].ToString();
                            Dt_Row["NOMBRE_EMPLEADO"] = Dt_Row_Tiempo_Extra["Nombre_Empleado"].ToString();
                            
                            if(!String.IsNullOrEmpty(Dt_Row_Tiempo_Extra[Cat_Empleados.Campo_Salario_Diario].ToString()))
                                Dt_Row["SALARIO_DIARIO"] = Dt_Row_Tiempo_Extra[Cat_Empleados.Campo_Salario_Diario].ToString();

                            if (!String.IsNullOrEmpty(Dt_Row_Tiempo_Extra[Ope_Nom_Tiempo_Extra.Campo_Fecha].ToString()))
                                Dt_Row["FECHA"] = Dt_Row_Tiempo_Extra[Ope_Nom_Tiempo_Extra.Campo_Fecha].ToString();

                            if (!String.IsNullOrEmpty(Dt_Row_Tiempo_Extra[Ope_Nom_Tiempo_Extra.Campo_Horas].ToString()))
                                Dt_Row["HORAS"] = Dt_Row_Tiempo_Extra[Ope_Nom_Tiempo_Extra.Campo_Horas].ToString();

                            Dt_Row["CLAVE_DEPENDENCIA"] = Dt_Row_Tiempo_Extra["Clave_Dependencia"].ToString();
                            Dt_Row["NOMBRE_DEPENDENCIA"] = Dt_Row_Tiempo_Extra["Nombre_Dependencia"].ToString();
                            Dt_Row["ELABORO"] = Cls_Sessiones.Nombre_Empleado;
                            Dt_Row["APLICA_ISSEG"] = Dt_Row_Tiempo_Extra[Cat_Empleados.Campo_Aplica_ISSEG].ToString();
                            
                            if (!String.IsNullOrEmpty(Dt_Row_Tiempo_Extra[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString()))
                            {
                                Tipo_Nomina = Convert.ToDouble(Dt_Row_Tiempo_Extra[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString());
                                Dt_Row["TIPO_NOMINA"] = Tipo_Nomina;
                            }

                            Dt_Row["NOMBRE_NOMINA"] = Dt_Row_Tiempo_Extra[Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString();


                            if (Dt_Row_Tiempo_Extra[Cat_Empleados.Campo_Aplica_ISSEG].ToString() == "SI")
                            {
                                //  para sacar el saldo del tiempo extra con el psm
                                if (!String.IsNullOrEmpty(Dt_Row_Tiempo_Extra["Psm_Extra"].ToString()))
                                    Dt_Row["TOTAL"] = Dt_Row_Tiempo_Extra["Psm_Extra"].ToString();

                                if (!String.IsNullOrEmpty(Dt_Row_Tiempo_Extra["Psm_diario"].ToString()))
                                    Dt_Row["PSM"] = Dt_Row_Tiempo_Extra["Psm_diario"].ToString();
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(Dt_Row_Tiempo_Extra["Salario_Normal_Extra"].ToString()))
                                    Dt_Row["TOTAL"] = Dt_Row_Tiempo_Extra["Salario_Normal_Extra"].ToString();
                            }

                            //  para ingresar la informacion al DataTable
                            Dt_Reporte.Rows.Add(Dt_Row);
                        }
                    }
                }
            }

            //  se llena el dataset
            Ds_Reporte.Clear();
            Ds_Reporte.Tables.Clear();
            Ds_Reporte.Tables.Add(Dt_Reporte.Copy());
            Ds_Reporte.Tables.Add(Dt_Periodo.Copy());

            //  se carga el reporte
            Reporte.Load(Ruta_Archivo + "Cr_Rpt_Nom_Tiempo_Extra.rpt");
            Reporte.SetDataSource(Ds_Reporte);

            DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();

            if (Formato == "PDF")
            {
                Nombre_Archivo += ".pdf";
                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Opciones_Exportacion);
                Abrir_Ventana(Nombre_Archivo);
            }
            else if (Formato == "EXCEL")
            {
                Nombre_Archivo += ".xls";
                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;
                Reporte.Export(Opciones_Exportacion);

                String Ruta = "../../Reporte/" + Nombre_Archivo;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Diario_General " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Generar_Reporte_Prima_Dominical
    /// DESCRIPCION : Consulta el personal que laboro en domingo
    /// PARAMETROS  : String Formato.- Para saber que formato sera el archivo pdf, excel
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 23/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Generar_Reporte_Prima_Dominical(String Formato)
    {
        Cls_Rpt_Nom_Retardos_Faltas_Negocio Rs_Faltas = new Cls_Rpt_Nom_Retardos_Faltas_Negocio();
        Cls_Rpt_Nom_Tiempo_Extra_Negocio Rs_Tiempo_Extra = new Cls_Rpt_Nom_Tiempo_Extra_Negocio();
        DataTable Dt_Reporte = new DataTable();
        DataTable Dt_Consulta = new DataTable();
        DataTable Dt_Periodo = new DataTable();
        DataTable Dt_Auxiliar = new DataTable();
        String Empleado_ID = "";
        DataRow Dt_Row;
        Double Tipo_Nomina = 0.0;
        Ds_Rpt_Nom_Tiempo_Extra Ds_Reporte = new Ds_Rpt_Nom_Tiempo_Extra();
        ReportDocument Reporte = new ReportDocument();
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Reporte_Prima_Dominical_" + Session.SessionID; //Obtiene el nombre del archivo que sera asignado al documento
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            //  se busca el empleado id a partir del numero de empleado
            if (Txt_No_Empleado.Text != "")
            {
                Rs_Faltas.P_No_Empleado = Txt_No_Empleado.Text;
                Dt_Auxiliar = Rs_Faltas.Consultar_Informacion_Empleado();
                Rs_Faltas.P_No_Empleado = null;

                if (Dt_Auxiliar is DataTable)
                {
                    if (Dt_Auxiliar.Rows.Count > 0)
                    {
                        foreach (DataRow Dt_Row_Empleado in Dt_Auxiliar.Rows)
                        {
                            if (Dt_Row_Empleado is DataRow)
                            {
                                Empleado_ID = Dt_Row_Empleado[Cat_Empleados.Campo_Empleado_ID].ToString();
                            }
                        }
                    }
                }
            }

            //  se realiza la consulta
            Rs_Tiempo_Extra.P_Nomina_id = Cmb_Calendario_Nomina.SelectedValue;
            Rs_Tiempo_Extra.P_No_Nomina = Cmb_Periodos_Catorcenales_Nomina.SelectedValue;

            if (!String.IsNullOrEmpty(Empleado_ID))
                Rs_Tiempo_Extra.P_Empleado_ID = Empleado_ID;

            Dt_Consulta = Rs_Tiempo_Extra.Consultar_Prima_Dominical();

            Dt_Reporte = Construir_Tabla();
            Dt_Periodo = Construir_Tabla_Periodo();

            Dt_Row = Dt_Periodo.NewRow();
            Dt_Row["FECHA_INICIAL"] = Hdn_Fecha_Inicial.Value;
            Dt_Row["FECHA_FINAL"] = Hdn_Fecha_Final.Value;
            Dt_Row["NOMINA_ID"] = Cmb_Calendario_Nomina.SelectedItem.Text;
            Dt_Row["NO_NOMINA"] = Cmb_Periodos_Catorcenales_Nomina.SelectedItem.Value;
            Dt_Row["TIPO_REPORTE"] = "PRIMA DOMINICAL";
            Dt_Row["TIPO_COLUMNA"] = "DIAS";
            //  para ingresar la informacion al DataTable
            Dt_Periodo.Rows.Add(Dt_Row);


            if (Dt_Consulta is DataTable)
            {
                if (Dt_Consulta.Rows.Count > 0)
                {
                    foreach (DataRow Dt_Row_Tiempo_Extra in Dt_Consulta.Rows)
                    {
                        if (Dt_Row_Tiempo_Extra is DataRow)
                        {
                            Dt_Row = Dt_Reporte.NewRow();

                            Dt_Row["NO_EMPLEADO"] = Dt_Row_Tiempo_Extra[Cat_Empleados.Campo_No_Empleado].ToString();
                            Dt_Row["NOMBRE_EMPLEADO"] = Dt_Row_Tiempo_Extra["Nombre_Empleado"].ToString();

                            if (!String.IsNullOrEmpty(Dt_Row_Tiempo_Extra[Cat_Empleados.Campo_Salario_Diario].ToString()))
                                Dt_Row["SALARIO_DIARIO"] = Dt_Row_Tiempo_Extra[Cat_Empleados.Campo_Salario_Diario].ToString();

                            if (!String.IsNullOrEmpty(Dt_Row_Tiempo_Extra[Ope_Nom_Domingos_Empleado.Campo_Fecha].ToString()))
                                Dt_Row["FECHA"] = Dt_Row_Tiempo_Extra[Ope_Nom_Domingos_Empleado.Campo_Fecha].ToString();

                            //  se tomara para los dias laborados
                            Dt_Row["HORAS"] = 1;

                            Dt_Row["CLAVE_DEPENDENCIA"] = Dt_Row_Tiempo_Extra["Clave_Dependencia"].ToString();
                            Dt_Row["NOMBRE_DEPENDENCIA"] = Dt_Row_Tiempo_Extra["Nombre_Dependencia"].ToString();
                            Dt_Row["ELABORO"] = Cls_Sessiones.Nombre_Empleado;
                            Dt_Row["APLICA_ISSEG"] = Dt_Row_Tiempo_Extra[Cat_Empleados.Campo_Aplica_ISSEG].ToString();

                            if (!String.IsNullOrEmpty(Dt_Row_Tiempo_Extra[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString()))
                            {
                                Tipo_Nomina = Convert.ToDouble(Dt_Row_Tiempo_Extra[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString());
                                Dt_Row["TIPO_NOMINA"] = Tipo_Nomina;
                            }

                            Dt_Row["NOMBRE_NOMINA"] = Dt_Row_Tiempo_Extra[Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString();


                            if (Dt_Row_Tiempo_Extra[Cat_Empleados.Campo_Aplica_ISSEG].ToString() == "SI")
                            {
                                //  para sacar el saldo del tiempo extra con el psm
                                if (!String.IsNullOrEmpty(Dt_Row_Tiempo_Extra["Psm_Extra"].ToString()))
                                    Dt_Row["TOTAL"] = Dt_Row_Tiempo_Extra["Psm_Extra"].ToString();

                                if (!String.IsNullOrEmpty(Dt_Row_Tiempo_Extra["Psm_diario"].ToString()))
                                    Dt_Row["PSM"] = Dt_Row_Tiempo_Extra["Psm_diario"].ToString();
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(Dt_Row_Tiempo_Extra["Salario_Dia_Extra"].ToString()))
                                    Dt_Row["TOTAL"] = Dt_Row_Tiempo_Extra["Salario_Dia_Extra"].ToString();
                            }

                            //  para ingresar la informacion al DataTable
                            Dt_Reporte.Rows.Add(Dt_Row);
                        }
                    }
                }
            }

            //  se llena el dataset
            Ds_Reporte.Clear();
            Ds_Reporte.Tables.Clear();
            Ds_Reporte.Tables.Add(Dt_Reporte.Copy());
            Ds_Reporte.Tables.Add(Dt_Periodo.Copy());

            //  se carga el reporte
            Reporte.Load(Ruta_Archivo + "Cr_Rpt_Nom_Tiempo_Extra.rpt");
            Reporte.SetDataSource(Ds_Reporte);

            DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();

            if (Formato == "PDF")
            {
                Nombre_Archivo += ".pdf";
                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Opciones_Exportacion);
                Abrir_Ventana(Nombre_Archivo);
            }
            else if (Formato == "EXCEL")
            {
                Nombre_Archivo += ".xls";
                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;
                Reporte.Export(Opciones_Exportacion);

                String Ruta = "../../Reporte/" + Nombre_Archivo;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Diario_General " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Generar_Reporte_Dia_Festivo
    /// DESCRIPCION : Consulta el personal que trabajo en algun dia festivo
    /// PARAMETROS  : String Formato.- Para saber que formato sera el archivo pdf, excel
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 23/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Generar_Reporte_Dia_Festivo(String Formato)
    {
        Cls_Rpt_Nom_Retardos_Faltas_Negocio Rs_Faltas = new Cls_Rpt_Nom_Retardos_Faltas_Negocio();
        Cls_Rpt_Nom_Tiempo_Extra_Negocio Rs_Tiempo_Extra = new Cls_Rpt_Nom_Tiempo_Extra_Negocio();
        DataTable Dt_Reporte = new DataTable();
        DataTable Dt_Consulta = new DataTable();
        DataTable Dt_Periodo = new DataTable();
        DataTable Dt_Auxiliar = new DataTable();
        String Empleado_ID = "";
        DataRow Dt_Row;
        Double Tipo_Nomina = 0.0;
        Ds_Rpt_Nom_Tiempo_Extra Ds_Reporte = new Ds_Rpt_Nom_Tiempo_Extra();
        ReportDocument Reporte = new ReportDocument();
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Reporte_Dia_Festivo_" + Session.SessionID; //Obtiene el nombre del archivo que sera asignado al documento
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            //  se busca el empleado id a partir del numero de empleado
            if (Txt_No_Empleado.Text != "")
            {
                Rs_Faltas.P_No_Empleado = Txt_No_Empleado.Text;
                Dt_Auxiliar = Rs_Faltas.Consultar_Informacion_Empleado();
                Rs_Faltas.P_No_Empleado = null;

                if (Dt_Auxiliar is DataTable)
                {
                    if (Dt_Auxiliar.Rows.Count > 0)
                    {
                        foreach (DataRow Dt_Row_Empleado in Dt_Auxiliar.Rows)
                        {
                            if (Dt_Row_Empleado is DataRow)
                            {
                                Empleado_ID = Dt_Row_Empleado[Cat_Empleados.Campo_Empleado_ID].ToString();
                            }
                        }
                    }
                }
            }

            //  se realiza la consulta
            Rs_Tiempo_Extra.P_Nomina_id = Cmb_Calendario_Nomina.SelectedValue;
            Rs_Tiempo_Extra.P_No_Nomina = Cmb_Periodos_Catorcenales_Nomina.SelectedValue;

            if (!String.IsNullOrEmpty(Empleado_ID))
                Rs_Tiempo_Extra.P_Empleado_ID = Empleado_ID;

            Dt_Consulta = Rs_Tiempo_Extra.Consultar_Dia_Festivo();

            Dt_Reporte = Construir_Tabla();
            Dt_Periodo = Construir_Tabla_Periodo();

            Dt_Row = Dt_Periodo.NewRow();
            Dt_Row["FECHA_INICIAL"] = Hdn_Fecha_Inicial.Value;
            Dt_Row["FECHA_FINAL"] = Hdn_Fecha_Final.Value;
            Dt_Row["NOMINA_ID"] = Cmb_Calendario_Nomina.SelectedItem.Text;
            Dt_Row["NO_NOMINA"] = Cmb_Periodos_Catorcenales_Nomina.SelectedItem.Value;
            Dt_Row["TIPO_REPORTE"] = "DIA FESTIVO";
            Dt_Row["TIPO_COLUMNA"] = "DIAS";
            //  para ingresar la informacion al DataTable
            Dt_Periodo.Rows.Add(Dt_Row);


            if (Dt_Consulta is DataTable)
            {
                if (Dt_Consulta.Rows.Count > 0)
                {
                    foreach (DataRow Dt_Row_Tiempo_Extra in Dt_Consulta.Rows)
                    {
                        if (Dt_Row_Tiempo_Extra is DataRow)
                        {
                            Dt_Row = Dt_Reporte.NewRow();

                            Dt_Row["NO_EMPLEADO"] = Dt_Row_Tiempo_Extra[Cat_Empleados.Campo_No_Empleado].ToString();
                            Dt_Row["NOMBRE_EMPLEADO"] = Dt_Row_Tiempo_Extra["Nombre_Empleado"].ToString();

                            if (!String.IsNullOrEmpty(Dt_Row_Tiempo_Extra[Cat_Empleados.Campo_Salario_Diario].ToString()))
                                Dt_Row["SALARIO_DIARIO"] = Dt_Row_Tiempo_Extra[Cat_Empleados.Campo_Salario_Diario].ToString();

                            if (!String.IsNullOrEmpty(Dt_Row_Tiempo_Extra[Tab_Nom_Dias_Festivos.Campo_Fecha].ToString()))
                                Dt_Row["FECHA"] = Dt_Row_Tiempo_Extra[Tab_Nom_Dias_Festivos.Campo_Fecha].ToString();

                            //  se tomara para los dias laborados
                            Dt_Row["HORAS"] = 1;

                            Dt_Row["CLAVE_DEPENDENCIA"] = Dt_Row_Tiempo_Extra["Clave_Dependencia"].ToString();
                            Dt_Row["NOMBRE_DEPENDENCIA"] = Dt_Row_Tiempo_Extra["Nombre_Dependencia"].ToString();
                            Dt_Row["ELABORO"] = Cls_Sessiones.Nombre_Empleado;
                            Dt_Row["APLICA_ISSEG"] = Dt_Row_Tiempo_Extra[Cat_Empleados.Campo_Aplica_ISSEG].ToString();

                            if (!String.IsNullOrEmpty(Dt_Row_Tiempo_Extra[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString()))
                            {
                                Tipo_Nomina = Convert.ToDouble(Dt_Row_Tiempo_Extra[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString());
                                Dt_Row["TIPO_NOMINA"] = Tipo_Nomina;
                            }

                            Dt_Row["NOMBRE_NOMINA"] = Dt_Row_Tiempo_Extra[Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString();


                            if (Dt_Row_Tiempo_Extra[Cat_Empleados.Campo_Aplica_ISSEG].ToString() == "SI")
                            {
                                //  para sacar el saldo del tiempo extra con el psm
                                if (!String.IsNullOrEmpty(Dt_Row_Tiempo_Extra["Psm_Extra"].ToString()))
                                    Dt_Row["TOTAL"] = Dt_Row_Tiempo_Extra["Psm_Extra"].ToString();

                                if (!String.IsNullOrEmpty(Dt_Row_Tiempo_Extra["Psm_diario"].ToString()))
                                    Dt_Row["PSM"] = Dt_Row_Tiempo_Extra["Psm_diario"].ToString();
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(Dt_Row_Tiempo_Extra["Salario_Dia_Extra"].ToString()))
                                    Dt_Row["TOTAL"] = Dt_Row_Tiempo_Extra["Salario_Dia_Extra"].ToString();
                            }

                            //  para ingresar la informacion al DataTable
                            Dt_Reporte.Rows.Add(Dt_Row);
                        }
                    }
                }
            }

            //  se llena el dataset
            Ds_Reporte.Clear();
            Ds_Reporte.Tables.Clear();
            Ds_Reporte.Tables.Add(Dt_Reporte.Copy());
            Ds_Reporte.Tables.Add(Dt_Periodo.Copy());

            //  se carga el reporte
            Reporte.Load(Ruta_Archivo + "Cr_Rpt_Nom_Tiempo_Extra.rpt");
            Reporte.SetDataSource(Ds_Reporte);

            DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();

            if (Formato == "PDF")
            {
                Nombre_Archivo += ".pdf";
                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Opciones_Exportacion);
                Abrir_Ventana(Nombre_Archivo);
            }
            else if (Formato == "EXCEL")
            {
                Nombre_Archivo += ".xls";
                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;
                Reporte.Export(Opciones_Exportacion);

                String Ruta = "../../Reporte/" + Nombre_Archivo;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Diario_General " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Abrir_Ventana
    ///DESCRIPCIÓN: Abre en otra ventana el archivo pdf
    ///PARÁMETROS : Nombre_Archivo: Guarda el nombre del archivo que se desea abrir
    ///                             para mostrar los datos al usuario
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 21-Febrero-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private void Abrir_Ventana(String Nombre_Archivo)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
        try
        {
            Pagina = Pagina + Nombre_Archivo;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
            "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Abrir_Ventana " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Construir_Tabla
    ///DESCRIPCIÓN: Genera la tabla con las columnas que se necesitan para el reporte
    ///PARÁMETROS : 
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 21-Marzo-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private DataTable Construir_Tabla()
    {
        DataTable Dt_Reporte = new DataTable();
        try
        {
            Dt_Reporte.Columns.Add("NO_EMPLEADO", typeof(System.String));
            Dt_Reporte.Columns.Add("NOMBRE_EMPLEADO", typeof(System.String));
            Dt_Reporte.Columns.Add("SALARIO_DIARIO", typeof(System.Double));
            Dt_Reporte.Columns.Add("PSM", typeof(System.Double));
            Dt_Reporte.Columns.Add("APLICA_ISSEG", typeof(System.String));
            Dt_Reporte.Columns.Add("HORAS", typeof(System.Double));
            Dt_Reporte.Columns.Add("FECHA", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("CLAVE_DEPENDENCIA", typeof(System.String));
            Dt_Reporte.Columns.Add("NOMBRE_DEPENDENCIA", typeof(System.String));
            Dt_Reporte.Columns.Add("TIPO_NOMINA", typeof(System.String));
            Dt_Reporte.Columns.Add("NOMBRE_NOMINA", typeof(System.String));
            Dt_Reporte.Columns.Add("ELABORO", typeof(System.String));
            Dt_Reporte.Columns.Add("TOTAL", typeof(System.Double));
            Dt_Reporte.TableName = "Dt_Reporte_Horas_Extras";

            return Dt_Reporte;
        }
        catch (Exception ex)
        {
            throw new Exception("Abrir_Ventana " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Construir_Tabla_Periodo
    ///DESCRIPCIÓN: Genera la tabla con las columnas que se necesitan para el reporte
    ///PARÁMETROS : 
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 23-Marzo-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private DataTable Construir_Tabla_Periodo()
    {
        DataTable Dt_Reporte = new DataTable();
        try
        {
            Dt_Reporte.Columns.Add("FECHA_INICIAL", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("FECHA_FINAL", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("NOMINA_ID", typeof(System.String));
            Dt_Reporte.Columns.Add("NO_NOMINA", typeof(System.String));
            Dt_Reporte.Columns.Add("TIPO_REPORTE", typeof(System.String));
            Dt_Reporte.Columns.Add("TIPO_COLUMNA", typeof(System.String));
            Dt_Reporte.TableName = "Dt_Periodo";

            return Dt_Reporte;
        }
        catch (Exception ex)
        {
            throw new Exception("Abrir_Ventana " + ex.Message.ToString(), ex);
        }
    }
    #endregion
    #endregion

    #region Eventos

    #region Botones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Reporte_Pdf_Click
    ///DESCRIPCIÓN: Realizara los metodos requeridos para el reporte
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  13/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Reporte_Pdf_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            if (Validar_Reporte())
            {
                if (Cmb_Tipo_Reporte.SelectedValue == "TIEMPO EXTRA")
                    Generar_Reporte_Tiempo_Extra("PDF");

                else if (Cmb_Tipo_Reporte.SelectedValue == "DIA FESTIVO")
                    Generar_Reporte_Dia_Festivo("PDF");

                else if (Cmb_Tipo_Reporte.SelectedValue == "PRIMA DOMINICAL")
                    Generar_Reporte_Prima_Dominical("PDF");
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }

        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception(Ex.Message, Ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Reporte_Excel_Click
    ///DESCRIPCIÓN: Realizara los metodos requeridos para el reporte
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  13/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Reporte_Excel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            if (Validar_Reporte())
            {
                if (Cmb_Tipo_Reporte.SelectedValue == "TIEMPO EXTRA")
                    Generar_Reporte_Tiempo_Extra("EXCEL");

                else if (Cmb_Tipo_Reporte.SelectedValue == "DIA FESTIVO")
                    Generar_Reporte_Dia_Festivo("EXCEL");

                else if (Cmb_Tipo_Reporte.SelectedValue == "PRIMA DOMINICAL")
                    Generar_Reporte_Prima_Dominical("EXCEL");
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception(Ex.Message, Ex);
        }
    }
    ///*********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Empleado_Click
    ///DESCRIPCIÓN          : Evento del boton de busqueda de empleados
    ///PROPIEDADES          :
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 21/Diciembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    protected void Btn_Buscar_Empleado_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Empleados_Negocios Rs_Consulta_Ca_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta 
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (!String.IsNullOrEmpty(Txt_Nombre_Empleado.Text))
            {
                if (!string.IsNullOrEmpty(Txt_Nombre_Empleado.Text.Trim()))
                {
                    Rs_Consulta_Ca_Empleados.P_Nombre = Txt_Nombre_Empleado.Text.Trim();
                }
                Rs_Consulta_Ca_Empleados.P_Estatus = "ACTIVO";
                Dt_Empleados = Rs_Consulta_Ca_Empleados.Consulta_Empleados_General();
                Cmb_Nombre_Empleado.DataSource = new DataTable();
                Cmb_Nombre_Empleado.DataBind();
                Cmb_Nombre_Empleado.DataSource = Dt_Empleados;
                Cmb_Nombre_Empleado.DataTextField = "Empleado";
                Cmb_Nombre_Empleado.DataValueField = Cat_Empleados.Campo_No_Empleado;
                Cmb_Nombre_Empleado.DataBind();
                Cmb_Nombre_Empleado.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
                Cmb_Nombre_Empleado.SelectedIndex = -1;
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Realizara los metodos requeridos para el reporte
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  13/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception(Ex.Message, Ex);
        }
    }
    #endregion

    #region Combos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Nombre_Empleado_OnSelectedIndexChanged
    ///DESCRIPCIÓN: Habilitara las cajas de texto correspondientes al reporte
    ///CREO       : Juan Alberto Hernández Negrete
    ///FECHA_CREO : 06/Abril/2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Nombre_Empleado_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Nombre_Empleado.SelectedIndex > 0)
            {
                Txt_No_Empleado.Text = Cmb_Nombre_Empleado.SelectedValue;
            }
            else
            {
                Txt_No_Empleado.Text = "";
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception(Ex.Message, Ex);
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO       : Juan Alberto Hernández Negrete
    ///FECHA_CREO : 06/Abril/2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Calendario_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 index = Cmb_Calendario_Nomina.SelectedIndex;
        if (index > 0)
        {
            Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim()); //Consulta los periodos nominales validos
        }
        else
        {
            Cmb_Periodos_Catorcenales_Nomina.DataSource = new DataTable();
            Cmb_Periodos_Catorcenales_Nomina.DataBind();
        }
        Cmb_Calendario_Nomina.Focus();
    }
    
    protected void Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Int32 index = Cmb_Periodos_Catorcenales_Nomina.SelectedIndex;
            Hdn_Fecha_Inicial.Value = "";
            Hdn_Fecha_Final.Value = "";
            if (index > 0)
            {
                Consulta_Fechas_Periodo_Nominal(); //Consulta la fecha de inicio y termino para la generación de asistencias del empleado
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
    /// DESCRIPCION: Crea el DataTable con la consulta de las nomina vigentes en el 
    ///              sistema.
    /// PARAMETROS : Dt_Calendario_Nominas.- Lista de las nominas vigentes actualmente 
    ///              en el sistema.
    /// CREO       : Juan Alberto Hernandez Negrete
    /// FECHA_CREO : 06/Abril/2011
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
    #endregion
    #endregion
}
