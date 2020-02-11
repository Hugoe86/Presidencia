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
using Presidencia.Incapacidades.Negocio;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;
using Presidencia.Constantes;
using Presidencia.Tipos_Nominas.Negocios;
using CarlosAg.ExcelXmlWriter;
using Presidencia.Ayudante_CarlosAG;
using System.Text;
using Presidencia.Rpt_Retencion_IMSS.Negocio;
using Presidencia.Utilidades_Nomina;

public partial class paginas_Nomina_Frm_Rpt_Nom_Retencion_IMSS : System.Web.UI.Page
{
    #region (Load/Init)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Configuracion_Inicial();
            }

            Lbl_Mensaje_Error.Text = "";
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    #endregion

    #region (Métodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///
    ///DESCRIPCIÓN: Habilita la configuración inicial de los controles
    ///             página de generación de la nómina.
    ///             
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Cargar_Combo_Tipos_Nomina();//Consultan los tipos de nomina que estan registrados en el sistema.
        Consultar_Calendario_Nominas();//Se consultan los calendarios de nómina que existen actualmente en el sistema.
        Limpiar_Controles();
        Consultar_UR();
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : limpia los constroles de la página para realizar la siguiente operación.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Cmb_Calendario_Nomina.SelectedIndex = -1;
            Cmb_Periodo.SelectedIndex = -1;
            Txt_No_Empleado.Text = "";
            Txt_Nombre_Empleado.Text = "";
            Cmb_Tipo_Nominas.SelectedIndex = -1;
            Cmb_UR.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al limpiar los controles de la pagina de generacion de la nómina. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la deduccion 
    ///             a partir del periodo actual.
    ///             
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Validar_Periodos_Pago(DropDownList Combo)
    {
        Cls_Ope_Nom_Pestamos_Negocio Prestamos_Negocio = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Actual = DateTime.Now;//Variable que almacenará la fecha actual.
        DateTime Fecha_Inicio = new DateTime();//Variable que almacenará la fecha de inicio de la catorcena.
        DateTime Fecha_Fin = new DateTime();//Variable que almacenará la fecha final de la catorcena.
        Fecha_Actual = Fecha_Actual.AddDays(-14);//Obtenemos la fecha inicial dela catorcena anterior.

        Prestamos_Negocio.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();//Obtenemos la nomina seleccionada.

        foreach (ListItem Elemento in Combo.Items)
        {
            if (Es_Numerico(Elemento.Text.Trim()))
            {
                Prestamos_Negocio.P_No_Nomina = Convert.ToInt32(Elemento.Text.Trim());
                Dt_Detalles_Nomina = Prestamos_Negocio.Consultar_Fechas_Periodo();

                if (Dt_Detalles_Nomina != null)
                {
                    if (Dt_Detalles_Nomina.Rows.Count > 0)
                    {
                    }
                }
            }
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
    /// FECHA_CREO  : 06/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Formato_Fecha_Calendario_Nomina(DataTable Dt_Calendario_Nominas)
    {
        DataTable Dt_Nominas = new DataTable();//Variable que almacenara los calendarios de nóminas.
        DataRow Renglon_Dt_Clon = null;//Variable que almacenará un renglón del calendario de la nómina.

        //Creamos las columnas.
        Dt_Nominas.Columns.Add("Nomina", typeof(System.String));
        Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

        foreach (DataRow Renglon in Dt_Calendario_Nominas.Rows)
        {
            Renglon_Dt_Clon = Dt_Nominas.NewRow();
            Renglon_Dt_Clon["Nomina"] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString().Split(new char[] { ' ' })[0].Split(new char[] { '/' })[2];
            Renglon_Dt_Clon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID];
            Dt_Nominas.Rows.Add(Renglon_Dt_Clon);
        }
        return Dt_Nominas;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Es_Numerico
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Es_Numerico(String Cadena)
    {
        Boolean Resultado = true;//Almacena el resultado true si es numerica o false si no lo es.
        Char[] Array = Cadena.ToCharArray();//Obtenemos un arreglo de carácteres a partir de la cadena que se recibio como parámetro el método.

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
    /// NOMBRE DE LA FUNCION: Validar_Datos
    /// DESCRIPCION : Valida los datos que son requeridos para la generación del reporte
    ///               de incapacidades.
    ///               
    /// PARÁMETROS: No Aplica. 
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        Boolean Datos_Validos = true;//Variable que alamacenara el resultado de la validacion de los datos ingresados por el usuario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (Cmb_Calendario_Nomina.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Año de la Nómina es un dato requerido <br>";
            Datos_Validos = false;
        }

        if (Cmb_Periodo.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Periodo de la Nómina es un dato requerido. <br>";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }
    /// *************************************************************************************
    /// NOMBRE: Mostrar_Excel
    /// 
    /// DESCRIPCIÓN: Muestra el reporte en excel.
    ///              
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 10/Diciembre/2011.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    private void Mostrar_Excel(CarlosAg.ExcelXmlWriter.Workbook Libro, String Nombre_Archivo)
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Nombre_Archivo);
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Libro.Save(Response.OutputStream);
            Response.End();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte en excel. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Acomodar_Dt_Retencion
    ///DESCRIPCIÓN: Acomodar el datatable para el reporte de excel
    ///CREO: Leslie Gonzalez Vazquez
    ///FECHA_CREO: 06/Abril/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private DataTable Acomodar_Dt_Retencion(DataTable Dt_Retencion_Imss)
    {
        Double Salario_diario = 0.00;
        Double Salario_Mensual = 0.00;
        try
        {
            if (Dt_Retencion_Imss != null)
            {
                if (Dt_Retencion_Imss.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt_Retencion_Imss.Rows)
                    {
                        Salario_diario = Convert.ToDouble(String.IsNullOrEmpty(Dr["SALARIO_DIARIO"].ToString().Trim()) ? "0" : Dr["SALARIO_DIARIO"].ToString().Trim());
                        Salario_Mensual = Salario_diario * Cls_Utlidades_Nomina.Dias_Mes_Fijo;

                        Dr["SALARIO_MENSUAL"] = String.Format("{0:n}", Salario_Mensual);
                        Dr["RETENCION_IMSS"] = String.Format("{0:n}", Dr["RETENCION"]);
                    }
                    Dt_Retencion_Imss.Columns.Remove("SALARIO_DIARIO");
                    Dt_Retencion_Imss.Columns.Remove("RETENCION");
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al acomodar la tabla de retención de imss. Error[" + ex.Message + "]");
        }
        return Dt_Retencion_Imss;
    }

    #endregion

    #region (Metodos Consulta)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Tipos_Nomina
    ///DESCRIPCIÓN: Consulta y carga el ctrl que almacena los tipos de nómina que se 
    ///             encuentran dados de alta actualmente en el sistema.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Combo_Tipos_Nomina()
    {
        Cls_Cat_Tipos_Nominas_Negocio Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Tipos_Nominas = null;//Variable que almacenara una lista de las nominas que existe actualmente en el sistema.

        try
        {
            Dt_Tipos_Nominas = Tipos_Nominas.Consulta_Datos_Tipo_Nomina();
            Cmb_Tipo_Nominas.DataSource = Dt_Tipos_Nominas;
            Cmb_Tipo_Nominas.DataTextField = Cat_Nom_Tipos_Nominas.Campo_Nomina;
            Cmb_Tipo_Nominas.DataValueField = Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID;
            Cmb_Tipo_Nominas.DataBind();
            Cmb_Tipo_Nominas.Items.Insert(0, new ListItem("< Seleccione >", ""));
            Cmb_Tipo_Nominas.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los tipos de nómina que existén actualmente en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    /// ***********************************************************************************************
    /// Nombre: Consultar_Incapacidades
    /// 
    /// Descripción: Consulta las incapacidades de los empleados.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 05/Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***********************************************************************************************
    protected DataTable Consultar_Retencion_Excel()
    {
        Cls_Rpt_Nom_Retencion_IMSS_Negocio Obj_Retencion = new Cls_Rpt_Nom_Retencion_IMSS_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Retencion = null;//Variable que almacena los registros de incapacidades.
        DataSet Ds_Datos = null;//Variable que almacenara la tabla con las nóminas en negativo.

        try
        {
            Obj_Retencion.P_Tipo_Nomina_ID = Cmb_Tipo_Nominas.SelectedValue.Trim();
            Obj_Retencion.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Obj_Retencion.P_No_Nomina = Convert.ToInt32(Cmb_Periodo.SelectedItem.Text.Trim());
            Obj_Retencion.P_No_Empleado = Txt_No_Empleado.Text.Trim();
            Obj_Retencion.P_Nombre_Empleado = Txt_Nombre_Empleado.Text.Trim();
            Obj_Retencion.P_Dependencia_ID = Cmb_UR.SelectedValue.Trim();

            Dt_Retencion = Obj_Retencion.Consultar_Rpt_Retencion_Imss();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las Retenciones de IMSS de los empleados. Error: [" + Ex.Message + "]");
        }
        return Dt_Retencion;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales para el 
    ///calendario de nomina seleccionado.
    ///PARAMETROS: Nomina_ID.- Indica el calendario de nomina del cuál se desea consultar
    ///                        los periodos catorcenales.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 05/Diciembre/2011
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
                    Cmb_Periodo.DataValueField = Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID;
                    Cmb_Periodo.DataBind();
                    Cmb_Periodo.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Periodo.SelectedIndex = -1;

                    Validar_Periodos_Pago(Cmb_Periodo);
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
    ///NOMBRE DE LA FUNCIÓN: Consultar_Calendario_Nominas
    ///DESCRIPCIÓN: Consulta los calendarios de nomina vigentes actualmente.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 05/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Calendario_Nominas()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nominas = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Calendario_Nominas = null;//Variable que almacenara una lista de los calendarios de nomina vigentes.
        try
        {
            Dt_Calendario_Nominas = Consulta_Calendario_Nominas.Consultar_Calendario_Nominas();
            if (Dt_Calendario_Nominas != null)
            {
                if (Dt_Calendario_Nominas.Rows.Count > 0)
                {
                    Dt_Calendario_Nominas = Formato_Fecha_Calendario_Nomina(Dt_Calendario_Nominas);
                    Cmb_Calendario_Nomina.DataSource = Dt_Calendario_Nominas;
                    Cmb_Calendario_Nomina.DataTextField = "Nomina";
                    Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                    Cmb_Calendario_Nomina.DataBind();
                    Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Calendario_Nomina.SelectedIndex = -1;
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron nominas vigentes";
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las nominas. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_UR
    ///DESCRIPCIÓN: Consulta las unidades responsables.
    ///CREO: Leslie Gonzalez Vazquez
    ///FECHA_CREO: 05/Abril/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_UR()
    {
        Cls_Ope_Nom_Incapacidades_Negocio Incapacidades_Negocio = new Cls_Ope_Nom_Incapacidades_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_UR = null;//Variable que almacenara una lista de los calendarios de nomina vigentes.
        try
        {
            Dt_UR = Incapacidades_Negocio.Identificar_UR();
            if (Dt_UR != null)
            {
                if (Dt_UR.Rows.Count > 0)
                {
                    Cmb_UR.DataSource = Dt_UR;
                    Cmb_UR.DataTextField = "Clave_Nombre";
                    Cmb_UR.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
                    Cmb_UR.DataBind();
                    Cmb_UR.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_UR.SelectedIndex = -1;
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron lesUnidades Responsab";
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las Unidades responsables. Error: [" + Ex.Message + "]");
        }
    }

    /// ***********************************************************************************************
    /// Nombre: Consultar_Incapacidades
    /// 
    /// Descripción: Consulta las incapacidades de los empleados.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 05/Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***********************************************************************************************
    protected void Consultar_Retencion()
    {
        Cls_Rpt_Nom_Retencion_IMSS_Negocio Obj_Retencion= new Cls_Rpt_Nom_Retencion_IMSS_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Retencion = null;//Variable que almacena los registros de incapacidades.
        DataSet Ds_Datos = null;//Variable que almacenara la tabla con las nóminas en negativo.

        try
        {
            Obj_Retencion.P_Tipo_Nomina_ID = Cmb_Tipo_Nominas.SelectedValue.Trim();
            Obj_Retencion.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Obj_Retencion.P_No_Nomina = Convert.ToInt32(Cmb_Periodo.SelectedItem.Text.Trim());
            Obj_Retencion.P_No_Empleado = Txt_No_Empleado.Text.Trim();
            Obj_Retencion.P_Nombre_Empleado = Txt_Nombre_Empleado.Text.Trim();
            Obj_Retencion.P_Dependencia_ID = Cmb_UR.SelectedValue.Trim();

            Dt_Retencion = Obj_Retencion.Consultar_Rpt_Retencion_Imss();

            if (Dt_Retencion.Rows.Count > 0)
            {
                if (Dt_Retencion is DataTable)
                {
                    if (Dt_Retencion.Rows.Count > 0)
                    {
                        Ds_Datos = new DataSet();
                        Dt_Retencion.TableName = "Retencion_Imss";
                        Ds_Datos.Tables.Add(Dt_Retencion.Copy());

                        Generar_Reporte(ref Ds_Datos, "CR_Rpt_Nom_Retencion_IMSS.rpt",
                            "Rpt_Nom_Retencion_IMSS_" + Session.SessionID + ".pdf");
                    }
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron registros de Retencion.";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las retenciones de imss de los empleados. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Botones)
    /// *************************************************************************************
    /// NOMBRE: Btn_Generar_Reporte_Click
    /// 
    /// DESCRIPCIÓN: Consulta los Empleados de acuerdo a los filtros establecidos para
    ///              ejecutar la búsqueda, Genera y muestra el reporte. 
    ///              
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 10/Diciembre/2011.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Btn_Generar_Reporte_Click(object sender, EventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;
        try
        {
            if (Validar_Datos())
            {
                Consultar_Retencion();
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
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Btn_Generar_Reporte_Minimo_Click
    /// 
    /// DESCRIPCIÓN: Consulta los Empleados de acuerdo a los filtros establecidos para
    ///              ejecutar la búsqueda, Genera y muestra el reporte. 
    ///              
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 10/Diciembre/2011.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Btn_Generar_Reporte_Minimo_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Retencion = null;//Variable que almacena un listado de empleados.
        CarlosAg.ExcelXmlWriter.Workbook Libro = null;//Creamos la variable que almacenara el libro de excel.
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        try
        {
            if (Validar_Datos())
            {
                //Consultamos la información del empleado.
                Dt_Retencion = Consultar_Retencion_Excel();
                Dt_Retencion = Acomodar_Dt_Retencion(Dt_Retencion);
                //Obtenemos el libro.
                Libro = Cls_Ayudante_Crear_Excel.Generar_Excel(Dt_Retencion);
                //Mandamos a imprimir el reporte en excel.
                Mostrar_Excel(Libro, "");
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte retencion imss. Error: [" + Ex.Message + "]");
        }
    }

    #endregion

    #region (Combos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Calendario_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 Nomina_Seleccionada = 0;//Variable que almacena la nómina seleccionada del combo.
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;
        try
        {
            //Obtenemos elemento seleccionado del combo.
            Nomina_Seleccionada = Cmb_Calendario_Nomina.SelectedIndex;

            if (Nomina_Seleccionada > 0)
            {
                Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
            }
            else
            {
                Cmb_Periodo.DataSource = new DataTable();
                Cmb_Periodo.DataBind();
                Configuracion_Inicial();
                Limpiar_Controles();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    #endregion

    #endregion

    #region (Reportes)
    /// *************************************************************************************
    /// NOMBRE: Generar_Reporte
    /// 
    /// DESCRIPCIÓN: Método que invoca la generación del reporte.
    ///              
    /// PARÁMETROS: Nombre_Plantilla_Reporte.- Nombre del archivo del Crystal Report.
    ///             Nombre_Reporte_Generar.- Nombre que tendrá el reporte generado.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:15 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Generar_Reporte(ref DataSet Ds_Datos, String Nombre_Plantilla_Reporte, String Nombre_Reporte_Generar)
    {
        ReportDocument Reporte = new ReportDocument();//Variable de tipo reporte.
        String Ruta = String.Empty;//Variable que almacenara la ruta del archivo del crystal report. 

        try
        {
            Ruta = @Server.MapPath("../Rpt/Nomina/" + Nombre_Plantilla_Reporte);
            Reporte.Load(Ruta);

            if (Ds_Datos is DataSet)
            {
                if (Ds_Datos.Tables.Count > 0)
                {
                    Reporte.SetDataSource(Ds_Datos);
                    Exportar_Reporte_PDF(Reporte, Nombre_Reporte_Generar);
                    Mostrar_Reporte(Nombre_Reporte_Generar);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Exportar_Reporte_PDF
    /// 
    /// DESCRIPCIÓN: Método que guarda el reporte generado en formato PDF en la ruta
    ///              especificada.
    ///              
    /// PARÁMETROS: Reporte.- Objeto de tipo documento que contiene el reporte a guardar.
    ///             Nombre_Reporte.- Nombre que se le dará al reporte.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:19 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Exportar_Reporte_PDF(ReportDocument Reporte, String Nombre_Reporte)
    {
        ExportOptions Opciones_Exportacion = new ExportOptions();
        DiskFileDestinationOptions Direccion_Guardar_Disco = new DiskFileDestinationOptions();
        PdfRtfWordFormatOptions Opciones_Formato_PDF = new PdfRtfWordFormatOptions();

        try
        {
            if (Reporte is ReportDocument)
            {
                Direccion_Guardar_Disco.DiskFileName = @Server.MapPath("../../Reporte/" + Nombre_Reporte);
                Opciones_Exportacion.ExportDestinationOptions = Direccion_Guardar_Disco;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Opciones_Exportacion);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al exportar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Mostrar_Reporte
    /// 
    /// DESCRIPCIÓN: Muestra el reporte en pantalla.
    ///              
    /// PARÁMETROS: Nombre_Reporte.- Nombre que tiene el reporte que se mostrara en pantalla.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt_Nominas_Negativas",
                "window.open('" + Pagina + "', 'Busqueda_Empleados','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    #endregion
}
