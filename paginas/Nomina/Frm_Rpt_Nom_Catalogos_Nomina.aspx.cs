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
using Presidencia.Rpt_Cat_Nomina.Negocio;
using System.Text;
using Presidencia.Constantes;
using Presidencia.Ayudante_Excel;
using CarlosAg.ExcelXmlWriter;
using Presidencia.Ayudante_CarlosAG;

public partial class paginas_Nomina_Frm_Rpt_Nom_Catalogos_Nomina : System.Web.UI.Page
{
    #region (Load/Init)
    /// *************************************************************************************************************************
    /// Nombre Método: Page_Load
    /// 
    /// Descripción: Inicializa a un estado inicial los controles de la página.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack) {
                Configuracion_Inicial();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
        }
    }
    #endregion

    #region (Metodos)

    #region (Generales)
    /// *************************************************************************************************************************
    /// Nombre Método: Configuracion_Inicial
    /// 
    /// Descripción: Método que carga la configuración inical de la página.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private void Configuracion_Inicial()
    {
        try
        {
            Consultar_Tablas_Nomina();//Consultamos las tablas de nomina.

            if (Session["Dt_Campos"] != null)
                Session.Remove("Dt_Campos");
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar la configuración inicial de la página. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************************************************
    /// Nombre Método: Evitar_Campo_Duplicados
    /// 
    /// Descripción: Método que valida que no exista campos duplicados en el reporte a generar.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private Boolean Evitar_Campo_Duplicados(DataTable Dt_Datos, String Campo_Agregar)
    {
        Boolean Estatus = true;//Variable que guarda el esttus para evitar que los campos se dupliquen.

        try
        {
            if (Dt_Datos is DataTable)
            {
                if (Dt_Datos.Rows.Count > 0)
                {
                    foreach (DataRow CAMPO in Dt_Datos.Rows)
                    {
                        if (CAMPO is DataRow)
                        {
                            if (!String.IsNullOrEmpty(CAMPO["NOMBRE_CAMPO"].ToString())) {
                                if (CAMPO["NOMBRE_CAMPO"].ToString().Trim().Equals(Campo_Agregar))
                                {
                                    Estatus = false;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al realizar la validacion de campos duplicados en el grid. Error: [" + Ex.Message + "]");
        }
        return Estatus;
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
    #endregion

    #region (Consulta)
    /// *************************************************************************************************************************
    /// Nombre Método: Consultar_Tablas_Nomina
    /// 
    /// Descripción: Método que consulta todas las tablas de nomina de tipo catalogo.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private void Consultar_Tablas_Nomina()
    {
        Cls_Rpt_Nom_Catalogos_Nomina_Negocio Obj_Catalogos_Nomina = new Cls_Rpt_Nom_Catalogos_Nomina_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Tablas_Catalogos_Nomina = null;//Variable que listara las tablas de tipo catalogo de nomina.

        try
        {
            Dt_Tablas_Catalogos_Nomina = Obj_Catalogos_Nomina.Consultar_Tablas_Nomina();
            Cmb_Tablas_Nomina.DataSource = Dt_Tablas_Catalogos_Nomina;
            Cmb_Tablas_Nomina.DataTextField = "NOMBRE_CATALOGO";
            Cmb_Tablas_Nomina.DataValueField = "NOMBRE_CATALOGO";
            Cmb_Tablas_Nomina.DataBind();

            Cmb_Tablas_Nomina.Items.Insert(0, new ListItem("<-- Seleccione -->", String.Empty));
            Cmb_Tablas_Nomina.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las tablas de nómina. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************************************************
    /// Nombre Método: Consultar_Campos_Por_Tabla
    /// 
    /// Descripción: Método que consulta todos los campos de la tabla seleccionada.
    /// 
    /// Parámetros: Tabla.- Tabla de la cuál se consultaran los campos.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private void Consultar_Campos_Por_Tabla(String Tabla)
    {
        Cls_Rpt_Nom_Catalogos_Nomina_Negocio Obj_Catalogos_Nomina = new Cls_Rpt_Nom_Catalogos_Nomina_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Campos_Por_Tabla = null;//Variable que lista los campos de la tabla seleccionada.

        try
        {
            Obj_Catalogos_Nomina.P_Tabla = Cmb_Tablas_Nomina.SelectedValue.Trim();
            Dt_Campos_Por_Tabla = Obj_Catalogos_Nomina.Consultar_Campos_Por_Tabla();

            Cmb_Campos_Por_Tabla.DataSource = Dt_Campos_Por_Tabla;
            Cmb_Campos_Por_Tabla.DataTextField = "NOMBRE_CAMPO";
            Cmb_Campos_Por_Tabla.DataValueField = "NOMBRE_CAMPO";
            Cmb_Campos_Por_Tabla.DataBind();

            Cmb_Campos_Por_Tabla.Items.Insert(0, new ListItem("<-- Seleccione -->", String.Empty));
            Cmb_Campos_Por_Tabla.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los campos de la tabla seleccionada. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (+/- Campos)
    /// *************************************************************************************************************************
    /// Nombre Método: Agregar_Campo
    /// 
    /// Descripción: Agrega un campo a la consulta del catalogo.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private void Agregar_Campo()
    {
        DataTable Dt_Campos = new DataTable();

        try
        {            
            if (Session["Dt_Campos"] != null)
            {
                Dt_Campos = (DataTable)Session["Dt_Campos"];

                if (Evitar_Campo_Duplicados(Dt_Campos, Cmb_Campos_Por_Tabla.SelectedValue.Trim()))
                {
                    DataRow Dr = Dt_Campos.NewRow();
                    Dr["NOMBRE_CAMPO"] = Cmb_Campos_Por_Tabla.SelectedValue.Trim();
                    Dt_Campos.Rows.Add(Dr);

                    Session["Dt_Campos"] = Dt_Campos;
                    Llenar_Grid((DataTable)Session["Dt_Campos"]);
                }
                else {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Informacion", "alert('No es posible duplicar campos en el reporte.');", true);
                }
            }
            else {
                if (Evitar_Campo_Duplicados(Dt_Campos, Cmb_Campos_Por_Tabla.SelectedValue.Trim()))
                {
                    Dt_Campos.Columns.Add("NOMBRE_CAMPO", typeof(String));

                    DataRow Dr = Dt_Campos.NewRow();
                    Dr["NOMBRE_CAMPO"] = Cmb_Campos_Por_Tabla.SelectedValue.Trim();
                    Dt_Campos.Rows.Add(Dr);

                    Session["Dt_Campos"] = Dt_Campos;
                    Llenar_Grid((DataTable)Session["Dt_Campos"]);
                }
                else {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Informacion", "alert('No es posible duplicar campos en el reporte.');", true);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al agregar un campo a la tabla. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************************************************
    /// Nombre Método: Eliminar_Campo
    /// 
    /// Descripción: Elimina un campo a la consulta del catalogo.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private void Eliminar_Campo(String Dato_Eliminar)
    {
        try
        {
            if (Session["Dt_Campos"] != null)
            {
                DataTable Dt_Datos = (DataTable)Session["Dt_Campos"];
                DataRow[] Fila_Buscada = Dt_Datos.Select("NOMBRE_CAMPO='" + Dato_Eliminar + "'");

                if (Fila_Buscada.Length > 0) {
                    Dt_Datos.Rows.Remove(Fila_Buscada[0]);

                    Session["Dt_Campos"] = Dt_Datos;
                    Llenar_Grid((DataTable)Session["Dt_Campos"]);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al eliminar un campo a la tabla. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Generar_Query)
    /// *************************************************************************************************************************
    /// Nombre Método: Generar_Consulta
    /// 
    /// Descripción: Genera la consulta que se mandara al reporte del catálogo.
    /// 
    /// Parámetros: Tabla.- Nombre de la tabla de la cual se se genera el reporte.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private void Generar_Consulta(String Tabla)
    {
        CarlosAg.ExcelXmlWriter.Workbook Libro = null;//Creamos la variable que almacenara el libro de excel.
        Cls_Rpt_Nom_Catalogos_Nomina_Negocio Obj_Catalogos_Nomina = new Cls_Rpt_Nom_Catalogos_Nomina_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Rs_Consulta_Datos = null;//Variable que listara la información de la consulta realizada.
        DataTable Dt_Datos = null;//Variable que almacenara los campos que se consultaran.
        String Campos_Mostrar = String.Empty;//Variable que almacena los campos que se mostraran en la consulta.
        String Parte_Consulta = String.Empty;//Variable que almacena una parte de la consulta. 
        String Cierre_Consulta = String.Empty;//Variable que almacena el cierre de la consulta cuando un campo esta relacionado.
        String Mi_SQL = String.Empty;//Variable que almacenara la consulta que se manadara a la clase de datos.

        try
        {
            if (Session["Dt_Campos"] != null)
            {
                Dt_Datos = (DataTable)Session["Dt_Campos"];

                if (Dt_Datos is DataTable)
                {
                    if (Dt_Datos.Rows.Count > 0)
                    {
                        foreach (DataRow CAMPO in Dt_Datos.Rows)
                        {
                            if (!String.IsNullOrEmpty(CAMPO["NOMBRE_CAMPO"].ToString()))
                            {
                                if (CAMPO["NOMBRE_CAMPO"].ToString().Trim().Contains("_ID"))
                                {
                                    switch (Tabla)
                                    {
                                        case "CAT_NOM_CALENDARIO_RELOJ":
                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Calendario_Reloj.Campo_Nomina_ID))
                                                Cierre_Consulta += Cat_Nom_Calendario_Reloj.Tabla_Cat_Nom_Calendario_Reloj + "." + Cat_Nom_Calendario_Reloj.Campo_Nomina_ID;
                                            break;
                                        case "CAT_NOM_TAB_ORDEN_JUDICIAL":
                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_Empleado_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                                            break;
                                        case "CAT_NOM_TERCEROS":
                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Terceros.Campo_Percepcion_Deduccion_ID))
                                                Cierre_Consulta += Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros + "." + Cat_Nom_Terceros.Campo_Percepcion_Deduccion_ID;
                                            break;
                                        case "CAT_NOM_PERCEPCION_DEDUCCION":
                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Percepcion_Deduccion.Campo_Cuenta_Contable_ID))
                                                Cierre_Consulta += Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Cuenta_Contable_ID;
                                            break;
                                        case "CAT_NOM_NOMINAS_DETALLES":
                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Nominas_Detalles.Campo_Nomina_ID))
                                                Cierre_Consulta += Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID;
                                            break;
                                        case "CAT_NOM_PROVEEDORES":
                                            Cierre_Consulta += Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores + "." + Cat_Nom_Proveedores.Campo_Cuenta_Contable_ID;
                                            break;
                                        case "CAT_NOM_BANCOS":
                                            Cierre_Consulta += Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Cuenta_Contable_ID;
                                            break;
                                        case "CAT_EMPLEADOS":

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_Zona_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Zona_ID;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_Turno_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Turno_ID;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_Tipo_Trabajador_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Trabajador_ID;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_Tipo_Nomina_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_Tipo_Contrato_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Contrato_ID;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_Terceros_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Terceros_ID;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_Sindicato_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Sindicato_ID;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_Rol_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Rol_ID;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_Reloj_Checador_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Reloj_Checador_ID;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_Puesto_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Puesto_ID;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_SAP_Programa_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Programa_ID;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_SAP_Partida_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Partida_ID;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_Indemnizacion_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Indemnizacion_ID;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_Indemnizacion_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Indemnizacion_ID;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_SAP_Fuente_Financiamiento_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Fuente_Financiamiento_ID;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_Escolaridad_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Escolaridad_ID;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_Dependencia_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_Banco_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Banco_ID;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_Area_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Area_ID;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Empleados.Campo_SAP_Area_Responsable_ID))
                                                Cierre_Consulta += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Area_Responsable_ID;

                                            break;
                                        case "CAT_DEPENDENCIAS":
                                            Cierre_Consulta += Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias + "." + Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID;
                                            break;
                                        case "CAT_PUESTOS":
                                            Cierre_Consulta += Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Plaza_ID;
                                            break;
                                        case "CAT_NOM_PARAMETROS":
                                            Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Zona_ID;
                                            break;

                                        default:
                                            break;
                                    }

                                    Parte_Consulta = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion.Cambiar_Foreign_Key_Por_Nombre(CAMPO["NOMBRE_CAMPO"].ToString().Trim(), Cierre_Consulta).ToString();
                                    Campos_Mostrar += Parte_Consulta + ", ";

                                    Parte_Consulta = String.Empty;
                                    Cierre_Consulta = String.Empty;
                                }
                                else
                                {
                                    if (Tabla.Trim().ToUpper().Equals("CAT_NOM_PARAMETROS"))
                                    {
                                        if (CAMPO["NOMBRE_CAMPO"].ToString().Trim().Contains("DEDUCCION_") || CAMPO["NOMBRE_CAMPO"].ToString().Trim().Contains("PERCEPCION_"))
                                        {
                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Deduccion_ISSEG))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Deduccion_ISSEG;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Deduccion_Vacaciones_Tomadas_Mas))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Deduccion_Vacaciones_Tomadas_Mas;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Deduccion_Sueldo_Pagado_Mas))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Deduccion_Sueldo_Pagado_Mas;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Deduccion_Retardos))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Deduccion_Retardos;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Deduccion_Prima_Vacacional_Pagada_Mas))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Deduccion_Prima_Vacacional_Pagada_Mas;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Deduccion_Tipo_Desc_Orden_Judicial))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Deduccion_Tipo_Desc_Orden_Judicial;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Prima_Vacacional))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Prima_Vacacional;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Indemnizacion))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Indemnizacion;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Aguinaldo))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Aguinaldo;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Deduccion_ISR))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Deduccion_ISR;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Deduccion_IMSS))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Deduccion_IMSS;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Deduccion_Fondo_Retiro))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Deduccion_Fondo_Retiro;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Deduccion_Faltas))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Deduccion_Faltas;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Deduccion_Aguinaldo_Pagado_Mas))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Deduccion_Aguinaldo_Pagado_Mas;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Percepcion_Aguinaldo))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Percepcion_Aguinaldo;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Percepcion_Ajuste_ISR))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Percepcion_Ajuste_ISR;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Percepcion_Despensa))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Percepcion_Despensa;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Percepcion_Dia_Doble))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Percepcion_Dia_Doble;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Percepcion_Dia_Domingo))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Percepcion_Dia_Domingo;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Percepcion_Dias_Festivos))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Percepcion_Dias_Festivos;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Percepcion_Fondo_Retiro))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Percepcion_Fondo_Retiro;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Percepcion_Horas_Extra))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Percepcion_Horas_Extra;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Percepcion_Incapacidades))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Percepcion_Incapacidades;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Percepcion_Indemnizacion))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Percepcion_Indemnizacion;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Percepcion_Prima_Antiguedad))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Percepcion_Prima_Antiguedad;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Percepcion_Prima_Dominical))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Percepcion_Prima_Dominical;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Percepcion_Prima_Vacacional))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Percepcion_Prima_Vacacional;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Percepcion_Prevision_Social_Multiple))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Percepcion_Prevision_Social_Multiple;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Percepcion_Quinquenio))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Percepcion_Quinquenio;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Percepcion_Subsidio))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Percepcion_Subsidio;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Percepcion_Sueldo_Normal))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Percepcion_Sueldo_Normal;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Percepcion_Vacaciones))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Percepcion_Vacaciones;

                                            if (CAMPO["NOMBRE_CAMPO"].ToString().Equals(Cat_Nom_Parametros.Campo_Percepcion_Vacaciones_Pendientes_Pagar))
                                                Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Percepcion_Vacaciones_Pendientes_Pagar;

                                            Parte_Consulta = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion.Cambiar_Foreign_Key_Por_Nombre("PERCEPCION_DEDUCCION_ID", Cierre_Consulta).ToString();
                                            Campos_Mostrar += Parte_Consulta + ", ";

                                            Parte_Consulta = String.Empty;
                                            Cierre_Consulta = String.Empty;

                                        }
                                        else
                                        {
                                            Cierre_Consulta += Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + "." + Cat_Nom_Parametros.Campo_Zona_ID;
                                            Parte_Consulta = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion.Cambiar_Foreign_Key_Por_Nombre("PERCEPCION_DEDUCCION_ID", Cierre_Consulta).ToString();
                                            Campos_Mostrar += Parte_Consulta + ", ";
                                        }
                                    }
                                    else
                                    {
                                        Campos_Mostrar += CAMPO["NOMBRE_CAMPO"].ToString() + ", ";
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Campos_Mostrar = Campos_Mostrar.Trim();
            Campos_Mostrar = Campos_Mostrar.TrimEnd(new Char[] { ',' });

            Mi_SQL = "SELECT " + Campos_Mostrar + " FROM " + Cmb_Tablas_Nomina.SelectedValue.Trim();

            Obj_Catalogos_Nomina.P_Mi_SQL = Mi_SQL;
            Dt_Rs_Consulta_Datos = Obj_Catalogos_Nomina.Ejecutar_Consulta();

            //Obtenemos el libro.
            Libro = Cls_Ayudante_Crear_Excel.Generar_Excel(Dt_Rs_Consulta_Datos);
            //Mandamos a imprimir el reporte en excel.
            Mostrar_Excel(Libro, "Catalogo.xls");
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar la consulta del reporte. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

    #region (GridView)
    /// *************************************************************************************************************************
    /// Nombre Método: Llenar_Grid
    /// 
    /// Descripción: Carga el grid con los datos quen son pasados como parametros.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private void Llenar_Grid(DataTable Dt_Datos)
    {
        try
        {
            Grid_Campos_Mostrar_Reporte.DataSource = Dt_Datos;
            Grid_Campos_Mostrar_Reporte.DataBind();
            Grid_Campos_Mostrar_Reporte.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar el grid de campos. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Eventos)

    #region (Botones)
    /// *************************************************************************************************************************
    /// Nombre Método: Btn_Agregar_Campo_Click
    /// 
    /// Descripción: Evento que agrega un campo a la consulta del catalogo.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    protected void Btn_Agregar_Campo_Click(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Campos_Por_Tabla.SelectedIndex > 0)
            {
                Agregar_Campo();
            }
            else {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Informacion", "alert('No se ha seleccionado ningun campo ha agregar al reporte.');", true);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
        }
    }
    /// *************************************************************************************************************************
    /// Nombre Método: Btn_Eliminar_Campo_Click
    /// 
    /// Descripción: Evento que elimina un campo a la consulta del catalogo.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    protected void Btn_Eliminar_Campo_Click(object sender, EventArgs e)
    {
        try
        {
            Eliminar_Campo(((ImageButton)sender).CommandArgument);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
        }
    }

    protected void Button1_Click(object sender, EventArgs e) {
        Generar_Consulta(Cmb_Tablas_Nomina.SelectedValue.Trim());
    }
    #endregion

    #region (Combos)
    /// *************************************************************************************************************************
    /// Nombre Método: Cmb_Tablas_Nomina_SelectedIndexChanged
    /// 
    /// Descripción: evento que carga los campos de la tabla seleccionada.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    protected void Cmb_Tablas_Nomina_SelectedIndexChanged(Object sender, EventArgs e) {

        try
        {
            if (Cmb_Tablas_Nomina.SelectedIndex > 0) {
                Consultar_Campos_Por_Tabla(Cmb_Tablas_Nomina.SelectedValue.Trim());

                Llenar_Grid(new DataTable());

                if (Session["Dt_Campos"] != null)
                {
                    Session.Remove("Dt_Campos");
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
        }
    }
    #endregion

    #endregion
}
