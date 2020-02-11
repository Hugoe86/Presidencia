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

public partial class paginas_Nomina_Frm_Cat_Nom_Reporteador : System.Web.UI.Page
{
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
            //Consultamos los campos que serán mostrados en el reporte.
            Dt_Datos = Obtener_DataTable_Campos_Reporte();

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
                                        ////////////////////////////////////////// DEDUCCIONES /////////////////////////////////////////

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

                                        ////////////////////////////////////////// PERCEPCIONES /////////////////////////////////////////

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


            Campos_Mostrar = Campos_Mostrar.Trim();
            Campos_Mostrar = Campos_Mostrar.TrimEnd(new Char[] { ',' });

            Mi_SQL = "SELECT " + Campos_Mostrar + " FROM " + Tabla;

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

    #region (Generales)
    /// *************************************************************************************************************************
    /// Nombre Método: Obtener_DataTable_Campos_Reporte
    /// 
    /// Descripción: Método que crea la tabla que contendra los campos que serán mostrados en el reporte.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    public DataTable Obtener_DataTable_Campos_Reporte()
    {
        DataTable Dt_Campos = new DataTable();//Variable que almacenara el listado de los campos que seran mostrados en el reporte.

        try
        {
            //Obtenemos los campos que seran mostrados en el reporte.
            String[] array = Txt_Campos_Reporte.Value.Trim().Split(new Char[] { ',' });

            //Creamos la estructura de la tabla que almacenara los campos a mostrar en el reporte.
            Dt_Campos.Columns.Add("NOMBRE_CAMPO", typeof(String));

            if (array.Length > 0)
            {
                foreach (String CAMPO in array)
                {
                    if (CAMPO is String)
                    {
                        DataRow Dr = Dt_Campos.NewRow();
                        Dr["NOMBRE_CAMPO"] = CAMPO;
                        Dt_Campos.Rows.Add(Dr);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar la tabla con los campos que se mostraran en el reporte. Error: [" + Ex.Message + "]");
        }
        return Dt_Campos;
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

    #region (Botones)
    /// *************************************************************************************************************************
    /// Nombre Método: Btn_Generar_Reporte_Click
    /// 
    /// Descripción: Método que genera el reporte.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    protected void Btn_Generar_Reporte_Click(object sender, EventArgs e)
    {
        try
        {
            Generar_Consulta(Txt_Tabla.Value.Trim());
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    #endregion
}
