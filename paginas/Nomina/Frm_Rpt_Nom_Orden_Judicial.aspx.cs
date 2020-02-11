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
using Presidencia.Numalet;
using System.Globalization;
using Presidencia.Empleados.Negocios;
using Presidencia.Nomina_Reportes_Orden_Judicial.Negocio;

public partial class paginas_Nomina_Frm_Rpt_Nom_Orden_Judicial : System.Web.UI.Page
{
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
        try
        {
            if (!IsPostBack)
            {
                Inicializacion();

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
    /// NOMBRE DE LA FUNCION: Inicializacion
    /// DESCRIPCION :
    /// PARAMETROS  :
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 27/MARZO/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializacion()
    {
        try
        {
            Limpiar_Controles();//Limpia los controles de la forma
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 27/Marzo/2012
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

    #region Validaciones
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Reporte
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el reporte
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  :27/Marzo/2012
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

        //  validacion para cuando se selecciona algun numero de nomina o periodo
        //if (Txt_Nombre_Empleado.Text == "")
        //{
        //    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el numero del empleado.<br>";
        //    Datos_Validos = false;
        //}
        //else
        //{
        //    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el nombre del empleado.<br>";
        //    Datos_Validos = false;
        //}
           



        return Datos_Validos;
    }    
    #endregion

    #region Consultas
    #endregion

    #region Operaciones
    //Generar_Reporte

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
        Ds_Rpt_Nom_Orden_Judicial Ds_Reporte = new Ds_Rpt_Nom_Orden_Judicial();
        Cls_Rpt_Nom_Orden_Judicial_Negocio Rs_Orden = new Cls_Rpt_Nom_Orden_Judicial_Negocio();
        DataTable Dt_Reporte = new DataTable();
        DataTable Dt_Reporte_Final = new DataTable();
        ReportDocument Reporte = new ReportDocument();
        DataRow Dt_Row;
        DataTable Dt_Elaboro = new DataTable();
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Reporte_Orden_Judicial_" + Session.SessionID; //Obtiene el nombre del archivo que sera asignado al documento
        //String Cantidad_Letra = String.Empty;
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (!String.IsNullOrEmpty(Txt_No_Empleado.Text))
                Rs_Orden.P_No_Empleado = Txt_No_Empleado.Text;



            Dt_Reporte = Rs_Orden.Consultar_Orden_Judicial();
            Dt_Reporte_Final = Construir_Tabla_Orden();

            if (Dt_Reporte is DataTable)
            {
                if (Dt_Reporte.Rows.Count > 0)
                {
                    foreach (DataRow Dt_Row_Orden in Dt_Reporte.Rows)
                    {
                        if (Dt_Row_Orden is DataRow)
                        {
                            Dt_Row = Dt_Reporte_Final.NewRow();

                            Dt_Row["FECHA_FINAL"] = Dt_Row_Orden[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString();
                            Dt_Row["NOMBRE_EMPLEADO"] = Dt_Row_Orden["NOMBRE_EMPLEADO"].ToString();
                            Dt_Row["NOMBRE_DEPENDENCIA"] = Dt_Row_Orden["NOMBRE_DEPENDENCIA"].ToString();
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
                                                Dt_Row["CANTIDAD_LETRA"] = Convertir_Cantidad_Letras(Convert.ToDouble(Dt_Row_Orden["Descuento_Bruto"].ToString()));
                                            }
                                        }
                                        else if (Dt_Row_Orden["Tipo_Sueldo"].ToString() == "NETO")
                                        {
                                            if (!String.IsNullOrEmpty(Dt_Row_Orden["Descuento_Neto"].ToString()))
                                            {
                                                //  se pasa la cantidad del descuento
                                                Dt_Row["CANTIDAD_NUMERO"] = Dt_Row_Orden["Descuento_Neto"].ToString();
                                                Dt_Row["CANTIDAD_LETRA"] = Convertir_Cantidad_Letras(Convert.ToDouble(Dt_Row_Orden["Descuento_Neto"].ToString()));
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
                                                Dt_Row["CANTIDAD_LETRA"] = Convertir_Cantidad_Letras(Convert.ToDouble(Dt_Row_Orden["Descuento_Cantidad_Bruto"].ToString()));
                                            }
                                        }
                                        else if (Dt_Row_Orden["Tipo_Sueldo"].ToString() == "NETO")
                                        {
                                            if (!String.IsNullOrEmpty(Dt_Row_Orden["Descuento_Cantidad_Neto"].ToString()))
                                            {
                                                //  se pasa la cantidad del descuento
                                                Dt_Row["CANTIDAD_NUMERO"] = Dt_Row_Orden["Descuento_Cantidad_Neto"].ToString();
                                                Dt_Row["CANTIDAD_LETRA"] = Convertir_Cantidad_Letras(Convert.ToDouble(Dt_Row_Orden["Descuento_Cantidad_Neto"].ToString()));
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
            Reporte.Load(Ruta_Archivo + "Cr_Rpt_Nom_Orden_judicial.rpt");
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
    ///NOMBRE DE LA FUNCIÓN: Construir_Tabla_Orden
    ///DESCRIPCIÓN: Genera la tabla con las columnas que se necesitan para el reporte
    ///PARÁMETROS : 
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 26-Marzo-2012
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
            Dt_Reporte.Columns.Add("CANTIDAD_LETRA", typeof(System.String));
            Dt_Reporte.Columns.Add("CATORCENA", typeof(System.Double));
            Dt_Reporte.Columns.Add("FECHA_FINAL", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("NOMBRE_DEPENDENCIA", typeof(System.String));
            Dt_Reporte.Columns.Add("NOMBRE_EMPLEADO", typeof(System.String));
            Dt_Reporte.Columns.Add("BENEFICIARIO", typeof(System.String));
            Dt_Reporte.TableName = "Dt_Rpt_Orden_Judicial";
            return Dt_Reporte;
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

    #endregion

    #endregion


    #region Eventos

    #region Botones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Reporte_Pdf_Click
    ///DESCRIPCIÓN: Realizara los metodos requeridos para el reporte
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  27/Marzo/2012
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
                Generar_Reporte("PDF");
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
    ///FECHA_CREO:  27/Marzo/2012
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
                Generar_Reporte("EXCEL");
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
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 27/Marzo/2012 
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
    ///FECHA_CREO:  27/Marzo/2012
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
    ///DESCRIPCIÓN: carga la caja de texto de empleado con el numero de empleado
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO : 24/Marzo/2012
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
