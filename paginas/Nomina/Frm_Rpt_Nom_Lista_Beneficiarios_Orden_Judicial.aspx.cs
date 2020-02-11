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
using Presidencia.Numalet;
using System.Globalization;
using Presidencia.Empleados.Negocios;
using Presidencia.Nomina_Reportes_Orden_Judicial.Negocio;

public partial class paginas_Nomina_Frm_Rpt_Nom_Lista_Beneficiarios_Orden_Judicial : System.Web.UI.Page
{
    #region (Load/Init)
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

        try
        {
            if (!IsPostBack)
            {
                Limpiar_Controles();//Limpia los controles de la forma
                Consultar_Tipos_Nominas();
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

    #region (Metodos)

    #region Metodos Generales
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 28/Marzo/2012
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

            if (Cmb_Tipo_Nomina.SelectedIndex > 0)
                Cmb_Tipo_Nomina.SelectedIndex = 0;

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
    /// NOMBRE DE LA FUNCION: Validar_Reporte
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el reporte
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 28/Marzo/2012
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

        if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE DE LISTA ORDEN JUDICIAL")
        {
            if (Cmb_Tipo_Nomina.SelectedIndex == 0)
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione el tipo de nomina.<br>";
                Datos_Validos = false;
            }
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
            Cargar_Combos(Cmb_Tipo_Nomina, Dt_Tipos_Nominas, Cat_Nom_Tipos_Nominas.Campo_Nomina,
            Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID, 0);//Carga el combo de tipos de nómina.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cosnultar los tipos de nomina que existen actualemte en sistema. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region Cargar Combos
    /// *************************************************************************************
    /// NOMBRE: Cargar_Combos
    /// 
    /// DESCRIPCIÓN: Carga cualquier ctlr DropDownList que se le pase como parámetro.
    ///              
    /// PARÁMETROS: Combo.- Ctlr que se va a cargar.
    ///             Dt_Datos.- Informacion que se cargara en el combo.
    ///             Text.- Texto que será la parte visible de la lista de tipos de nómina.
    ///             Value.- Valor que será el que almacenará el elemnto seleccionado.
    ///             Index.- Indice el cuál será el que se mostrara inicialmente. 
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 11:12 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    private void Cargar_Combos(DropDownList Combo, DataTable Dt_Datos, String Text, String Value, Int32 Index)
    {
        try
        {
            Combo.DataSource = Dt_Datos;
            Combo.DataTextField = Text;
            Combo.DataValueField = Value;
            Combo.DataBind();
            Combo.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Combo.SelectedIndex = Index;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar el Ctlr de Tipo DropDownList. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region Reporte
    
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Generar_Reporte_Orden_Judicial
    /// DESCRIPCION : Consulta las ordenes judiciales que tenga el empleado
    /// PARAMETROS  : String Formato.- Para saber que formato sera el archivo pdf, excel
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 27/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Generar_Reporte_Orden_Judicial(String Formato)
    {
        Ds_Rpt_Nom_Lista_Orden_Judicial Ds_Reporte = new Ds_Rpt_Nom_Lista_Orden_Judicial();
        Cls_Rpt_Nom_Orden_Judicial_Negocio Rs_Orden = new Cls_Rpt_Nom_Orden_Judicial_Negocio();
        DataTable Dt_Reporte = new DataTable();
        DataTable Dt_Reporte_Final = new DataTable();
        ReportDocument Reporte = new ReportDocument();
        DataRow Dt_Row;
        DataTable Dt_Elaboro = new DataTable();
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Reporte_Orden_Judicial_" + Session.SessionID; //Obtiene el nombre del archivo que sera asignado al documento

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            //  filtro por numero de empleado
            if (!String.IsNullOrEmpty(Txt_No_Empleado.Text))
                Rs_Orden.P_No_Empleado = Txt_No_Empleado.Text;

            //  filtro por tipo de nomina
            if (Cmb_Tipo_Nomina.SelectedIndex > 0)
                Rs_Orden.P_Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue;

            //  se realiza la consulta
            Dt_Reporte = Rs_Orden.Consultar_Orden_Judicial();
            Dt_Reporte_Final = Construir_Tabla_Orden_Judicial();//   se genera la tabla que contendra la informacion del reporte

            if (Dt_Reporte is DataTable)
            {
                if (Dt_Reporte.Rows.Count > 0)
                {
                    foreach (DataRow Dt_Row_Orden in Dt_Reporte.Rows)
                    {
                        if (Dt_Row_Orden is DataRow)
                        {
                            Dt_Row = Dt_Reporte_Final.NewRow();

                            //  Se agrega la informacion al datarow
                            if (!String.IsNullOrEmpty(Dt_Row_Orden[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString()))
                                Dt_Row["TIPO_NOMINA"] = Convert.ToDouble(Dt_Row_Orden[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString());

                            Dt_Row["NOMBRE_NOMINA"] = Dt_Row_Orden[Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString();
                            Dt_Row["CLAVE_DEPENENCIA"] = Dt_Row_Orden["Clave_Dependencia"].ToString();
                            Dt_Row["NOMBRE_DEPENENCIA"] = Dt_Row_Orden["Nombre_Dependencia"].ToString();
                            Dt_Row["CODIGO_PROGRAMATICO"] = Dt_Row_Orden[Cat_Empleados.Campo_SAP_Codigo_Programatico].ToString();
                            Dt_Row["RFC"] = Dt_Row_Orden[Cat_Empleados.Campo_RFC].ToString();
                            Dt_Row["CLAVE_EMPLEADO"] = Dt_Row_Orden[Cat_Empleados.Campo_No_Empleado].ToString();
                            Dt_Row["ELABORO"] = Cls_Sessiones.Nombre_Empleado; 
                            Dt_Row["NOMBRE_EMPLEADO"] = Dt_Row_Orden["Nombre_Empleado"].ToString();
                            Dt_Row["BENEFICIARIO"] = Dt_Row_Orden[Cat_Nom_Tab_Orden_Judicial.Campo_Beneficiario].ToString();
                            //  PARA LAS FECHAS
                            Dt_Row["FECHA_INICIO"] = Dt_Row_Orden[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString();
                            Dt_Row["FECHA_FIN"] = Dt_Row_Orden[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString();

                            //  para saber si contiene informacion el tipo de descuento
                            if (!String.IsNullOrEmpty(Dt_Row_Orden["Tipo_Descuento"].ToString()))
                            {
                                //  para saber si es porcentaje
                                if (Dt_Row_Orden["Tipo_Descuento"].ToString() == "PORCENTAJE")
                                {
                                    //  para saber si contiene algun tipo de sueldo
                                    if (!String.IsNullOrEmpty(Dt_Row_Orden["Tipo_Sueldo"].ToString()))
                                    {
                                        //  para saber si pertenece al sueldo bruto o neto
                                        if (Dt_Row_Orden["Tipo_Sueldo"].ToString() == "BRUTO")
                                        {
                                            if (!String.IsNullOrEmpty(Dt_Row_Orden["Descuento_Bruto"].ToString()))
                                            {
                                                //  se pasa la cantidad del descuento
                                                Dt_Row["IMPORTE"] = Dt_Row_Orden["Descuento_Bruto"].ToString();
                                            }
                                        }
                                        else if (Dt_Row_Orden["Tipo_Sueldo"].ToString() == "NETO")
                                        {
                                            if (!String.IsNullOrEmpty(Dt_Row_Orden["Descuento_Neto"].ToString()))
                                            {
                                                //  se pasa la cantidad del descuento
                                                Dt_Row["IMPORTE"] = Dt_Row_Orden["Descuento_Neto"].ToString();
                                            }
                                        }
                                    }
                                }
                                //  para saber si se maneja por cantidad
                                else if (Dt_Row_Orden["Tipo_Descuento"].ToString() == "CANTIDAD")
                                {
                                    //  para saber si contiene algun tipo de sueldo
                                    if (!String.IsNullOrEmpty(Dt_Row_Orden["Tipo_Sueldo"].ToString()))
                                    {
                                        //  para saber si pertenece al sueldo bruto o neto
                                        if (Dt_Row_Orden["Tipo_Sueldo"].ToString() == "BRUTO")
                                        {
                                            if (!String.IsNullOrEmpty(Dt_Row_Orden["Descuento_Cantidad_Bruto"].ToString()))
                                            {
                                                //  se pasa la cantidad del descuento
                                                Dt_Row["IMPORTE"] = Dt_Row_Orden["Descuento_Cantidad_Bruto"].ToString();
                                            }
                                        }
                                        else if (Dt_Row_Orden["Tipo_Sueldo"].ToString() == "NETO")
                                        {
                                            if (!String.IsNullOrEmpty(Dt_Row_Orden["Descuento_Cantidad_Neto"].ToString()))
                                            {
                                                //  se pasa la cantidad del descuento
                                                Dt_Row["IMPORTE"] = Dt_Row_Orden["Descuento_Cantidad_Neto"].ToString();
                                            }
                                        }
                                    }
                                }// fin del else if 
                            }// fin del if principal

                            //  se agrega la fila a la tabla  
                            Dt_Reporte_Final.Rows.Add(Dt_Row);
                        }
                    }
                }
            }

            //  se llena el dataset
            Ds_Reporte.Clear();
            Ds_Reporte.Tables.Clear();
            Ds_Reporte.Tables.Add(Dt_Reporte_Final.Copy());

            //  se carga el reporte
            Reporte.Load(Ruta_Archivo + "Cr_Rpt_Nom_Deducciones_Orden_Judicial.rpt");
            Reporte.SetDataSource(Ds_Reporte);

            //  se genera el tipo del archivo y se muestra el reporte
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
    /// NOMBRE DE LA FUNCION: Generar_Reporte
    /// DESCRIPCION : Consulta las ordenes judiciales que tenga el empleado
    /// PARAMETROS  : String Formato.- Para saber que formato sera el archivo pdf, excel
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 27/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Generar_Reporte(String Formato)
    {
        Ds_Rpt_Nom_Lista_Orden_Judicial Ds_Reporte = new Ds_Rpt_Nom_Lista_Orden_Judicial();
        Cls_Rpt_Nom_Orden_Judicial_Negocio Rs_Orden = new Cls_Rpt_Nom_Orden_Judicial_Negocio();
        DataTable Dt_Reporte = new DataTable();
        DataTable Dt_Reporte_Final = new DataTable();
        ReportDocument Reporte = new ReportDocument();
        DataRow Dt_Row;
        DataTable Dt_Elaboro = new DataTable();
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Reporte_Lista_Beneficiarios_Orden_Judicial_" + Session.SessionID; //Obtiene el nombre del archivo que sera asignado al documento
        
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            //  filtro por numero de empleado
            if (!String.IsNullOrEmpty(Txt_No_Empleado.Text))
                Rs_Orden.P_No_Empleado = Txt_No_Empleado.Text;

            //  filtro por tipo de nomina
            if (Cmb_Tipo_Nomina.SelectedIndex > 0)
                Rs_Orden.P_Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue;

            //  se realiza la consulta
            Dt_Reporte = Rs_Orden.Consultar_Orden_Judicial();
            Dt_Reporte_Final = Construir_Tabla_Orden();//   se genera la tabla que contendra la informacion del reporte

            if (Dt_Reporte is DataTable)
            {
                if (Dt_Reporte.Rows.Count > 0)
                {
                    foreach (DataRow Dt_Row_Orden in Dt_Reporte.Rows)
                    {
                        if (Dt_Row_Orden is DataRow)
                        {
                            Dt_Row = Dt_Reporte_Final.NewRow();

                            //  Se agrega la informacion al datarow
                            Dt_Row["FECHA_FINAL"] = Dt_Row_Orden[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString();
                            Dt_Row["TIPO_NOMINA"] = Dt_Row_Orden[Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString();
                            Dt_Row["BENEFICIARIO"] = Dt_Row_Orden[Cat_Nom_Tab_Orden_Judicial.Campo_Beneficiario].ToString();
                            Dt_Row["CATORCENA"] = Dt_Row_Orden[Cat_Nom_Nominas_Detalles.Campo_No_Nomina].ToString();

                            //  para saber si contiene informacion el tipo de descuento
                            if (!String.IsNullOrEmpty(Dt_Row_Orden["Tipo_Descuento"].ToString()))
                            {
                                //  para saber si es porcentaje
                                if (Dt_Row_Orden["Tipo_Descuento"].ToString() == "PORCENTAJE")
                                {
                                    //  para saber si contiene algun tipo de sueldo
                                    if (!String.IsNullOrEmpty(Dt_Row_Orden["Tipo_Sueldo"].ToString()))
                                    {
                                        //  para saber si pertenece al sueldo bruto o neto
                                        if (Dt_Row_Orden["Tipo_Sueldo"].ToString() == "BRUTO")
                                        {
                                            if (!String.IsNullOrEmpty(Dt_Row_Orden["Descuento_Bruto"].ToString()))
                                            {
                                                //  se pasa la cantidad del descuento
                                                Dt_Row["CANTIDAD_NUMERO"] = Dt_Row_Orden["Descuento_Bruto"].ToString();
                                            }
                                        }
                                        else if (Dt_Row_Orden["Tipo_Sueldo"].ToString() == "NETO")
                                        {
                                            if (!String.IsNullOrEmpty(Dt_Row_Orden["Descuento_Neto"].ToString()))
                                            {
                                                //  se pasa la cantidad del descuento
                                                Dt_Row["CANTIDAD_NUMERO"] = Dt_Row_Orden["Descuento_Neto"].ToString();
                                            }
                                        }
                                    }
                                }
                                //  para saber si se maneja por cantidad
                                else if (Dt_Row_Orden["Tipo_Descuento"].ToString() == "CANTIDAD")
                                {
                                    //  para saber si contiene algun tipo de sueldo
                                    if (!String.IsNullOrEmpty(Dt_Row_Orden["Tipo_Sueldo"].ToString()))
                                    {
                                        //  para saber si pertenece al sueldo bruto o neto
                                        if (Dt_Row_Orden["Tipo_Sueldo"].ToString() == "BRUTO")
                                        {
                                            if (!String.IsNullOrEmpty(Dt_Row_Orden["Descuento_Cantidad_Bruto"].ToString()))
                                            {
                                                //  se pasa la cantidad del descuento
                                                Dt_Row["CANTIDAD_NUMERO"] = Dt_Row_Orden["Descuento_Cantidad_Bruto"].ToString();
                                            }
                                        }
                                        else if (Dt_Row_Orden["Tipo_Sueldo"].ToString() == "NETO")
                                        {
                                            if (!String.IsNullOrEmpty(Dt_Row_Orden["Descuento_Cantidad_Neto"].ToString()))
                                            {
                                                //  se pasa la cantidad del descuento
                                                Dt_Row["CANTIDAD_NUMERO"] = Dt_Row_Orden["Descuento_Cantidad_Neto"].ToString();
                                            }
                                        }
                                    }
                                }// fin del else if 
                            }// fin del if principal

                            //  se agrega la fila a la tabla  
                            Dt_Reporte_Final.Rows.Add(Dt_Row);
                        }
                    }
                }
            }

            //  se llena el dataset
            Ds_Reporte.Clear();
            Ds_Reporte.Tables.Clear();
            Ds_Reporte.Tables.Add(Dt_Reporte_Final.Copy());

            //  se carga el reporte
            Reporte.Load(Ruta_Archivo + "Cr_Rpt_Nom_Lista_Beneficiarios_Orden_Judicial.rpt");
            Reporte.SetDataSource(Ds_Reporte);

            //  se genera el tipo del archivo y se muestra el reporte
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
    ///NOMBRE DE LA FUNCIÓN: Convertir_Cantidad_Letras
    ///DESCRIPCIÓN: Convertira el numero en su equivalente en letra (ejemplo; 1 ... uno)
    ///PARÁMETROS : Double Cantidad_Numero; es el numero que convertira en texto 
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 28-Marzo-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    protected String Convertir_Cantidad_Letras(Double Cantidad_Numero)
    {
        Numalet Obj_Numale = new Numalet();
        String Cantidad_Letra = String.Empty;

        try
        {
            Obj_Numale.MascaraSalidaDecimal = "00/100 M.N";
            Obj_Numale.SeparadorDecimalSalida = "pesos ";
            Obj_Numale.LetraCapital = true;
            //Obj_Numale.ConvertirDecimales = true;
            Obj_Numale.Decimales = 2;
            Obj_Numale.CultureInfo = new CultureInfo("es-MX");
            Obj_Numale.ApocoparUnoParteEntera = true;
            Cantidad_Letra = Obj_Numale.ToCustomCardinal(Cantidad_Numero).Trim().ToUpper();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al convertir la cantidad a letras. Error:[" + Ex.Message + "]");
        }
        return Cantidad_Letra;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Construir_Tabla_Orden
    ///DESCRIPCIÓN: Genera la tabla que se pasara al reporte
    ///PARÁMETROS : 
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 28-Marzo-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private DataTable Construir_Tabla_Orden()
    {
        DataTable Dt_Reporte = new DataTable();
        try
        {
            Dt_Reporte.Columns.Add("CANTIDAD_NUMERO", typeof(System.Double));
            Dt_Reporte.Columns.Add("CATORCENA", typeof(System.Double));
            Dt_Reporte.Columns.Add("FECHA_FINAL", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("BENEFICIARIO", typeof(System.String));
            Dt_Reporte.Columns.Add("TIPO_NOMINA", typeof(System.String));
            Dt_Reporte.TableName = "Dt_Reporte_Lista_Orden_Judicial";

            return Dt_Reporte;
        }
        catch (Exception ex)
        {
            throw new Exception("Abrir_Ventana " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Construir_Tabla_Orden_Judicial
    ///DESCRIPCIÓN: Genera la tabla que se pasara al reporte
    ///PARÁMETROS : 
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 28-Marzo-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private DataTable Construir_Tabla_Orden_Judicial()
    {
        DataTable Dt_Reporte = new DataTable();
        try
        {
            Dt_Reporte.Columns.Add("NOMBRE_NOMINA", typeof(System.String));
            Dt_Reporte.Columns.Add("CLAVE_DEPENENCIA", typeof(System.String));
            Dt_Reporte.Columns.Add("NOMBRE_DEPENENCIA", typeof(System.String));
            Dt_Reporte.Columns.Add("CODIGO_PROGRAMATICO", typeof(System.String));
            Dt_Reporte.Columns.Add("RFC", typeof(System.String));
            Dt_Reporte.Columns.Add("NOMBRE_EMPLEADO", typeof(System.String));
            Dt_Reporte.Columns.Add("BENEFICIARIO", typeof(System.String));
            Dt_Reporte.Columns.Add("CLAVE_EMPLEADO", typeof(System.String));
            Dt_Reporte.Columns.Add("ELABORO", typeof(System.String));

            Dt_Reporte.Columns.Add("IMPORTE", typeof(System.Double));
            Dt_Reporte.Columns.Add("TIPO_NOMINA", typeof(System.Double));

            Dt_Reporte.Columns.Add("FECHA_INICIO", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("FECHA_FIN", typeof(System.DateTime));

            Dt_Reporte.TableName = "Dt_Reporte_Orden_Judicial";

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
                if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE DE ORDEN JUDICIAL")
                {
                    Generar_Reporte_Orden_Judicial("PDF");
                }
                else if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE DE LISTA ORDEN JUDICIAL")
                {
                    Generar_Reporte("PDF");
                }
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
                if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE DE ORDEN JUDICIAL")
                {
                    Generar_Reporte_Orden_Judicial("EXCEL");
                }
                else if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE DE LISTA ORDEN JUDICIAL")
                {
                    Generar_Reporte("EXCEL");
                }
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
    #endregion

    #endregion



}
