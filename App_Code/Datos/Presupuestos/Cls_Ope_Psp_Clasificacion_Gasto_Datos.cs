using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using System.Text;
using Presidencia.Clasificacion_Gasto.Negocio;

namespace Presidencia.Clasificacion_Gasto.Datos
{
    public class Cls_Ope_Psp_Clasificacion_Gasto_Datos
    {
        #region METODOS
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Fuente_Financiamiento
            ///DESCRIPCIÓN          : consulta para obtener los datos de las fuentes de financiamiento
            ///PARAMETROS           : 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 30/Noviembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Fuente_Financiamiento(Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    Mi_Sql.Append("SELECT " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " ||' '|| ");
                    Mi_Sql.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " AS nombre, ");
                    Mi_Sql.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + " AS id, ");
                    Mi_Sql.Append("SUM("+Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Devengado + ") AS DEVENGADO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Ejercido + ") AS EJERCIDO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Pagado + ") AS PAGADO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Comprometido + ") AS COMPROMETIDO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Disponible + ") AS DISPONIBLE");
                    Mi_Sql.Append(" FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento);
                    Mi_Sql.Append(" INNER JOIN " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                    Mi_Sql.Append(" ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID);
                    Mi_Sql.Append(" = " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
                    Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Anio);
                    Mi_Sql.Append(" = '" + Negocio.P_Anio + "'");
                    Mi_Sql.Append(" GROUP BY " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + ", ");
                    Mi_Sql.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + ", ");
                    Mi_Sql.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
                    Mi_Sql.Append(" ORDER BY " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + " ASC");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros de las fuentes de financiamiento. Error: [" + Ex.Message + "]");
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Area_Funcional
            ///DESCRIPCIÓN          : consulta para obtener los datos de las areas funcionales
            ///PARAMETROS           : 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 30/Noviembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Area_Funcional(Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    Mi_Sql.Append("SELECT " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Clave + " ||' '|| ");
                    Mi_Sql.Append(Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Descripcion + " AS nombre, ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + " || '_' || ");
                    Mi_Sql.Append(Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID + " AS id, ");
                    Mi_Sql.Append(Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID + " AS Area_id, ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + " AS Fte_id, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Devengado + ") AS DEVENGADO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Ejercido + ") AS EJERCIDO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Pagado + ") AS PAGADO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Comprometido + ") AS COMPROMETIDO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Disponible + ") AS DISPONIBLE");
                    Mi_Sql.Append(" FROM " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional);
                    Mi_Sql.Append(" INNER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias);
                    Mi_Sql.Append(" ON " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Area_Funcional_ID);
                    Mi_Sql.Append(" = " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID);
                    Mi_Sql.Append(" INNER JOIN " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                    Mi_Sql.Append(" ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID);
                    Mi_Sql.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                    Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Anio);
                    Mi_Sql.Append(" = '" + Negocio.P_Anio + "'");
                    Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID);
                    Mi_Sql.Append(" = '" + Negocio.P_Fuente_Financiamiento_ID + "'");
                    Mi_Sql.Append(" GROUP BY " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Clave + ", ");
                    Mi_Sql.Append(Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Descripcion + ", ");
                    Mi_Sql.Append(Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID + ", ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID);
                    Mi_Sql.Append(" ORDER BY " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID + " ASC");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros de las areas funcionales. Error: [" + Ex.Message + "]");
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Programas
            ///DESCRIPCIÓN          : consulta para obtener los datos de los programas
            ///PARAMETROS           : 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 30/Noviembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Programas(Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    Mi_Sql.Append("SELECT " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Clave + " ||' '|| ");
                    Mi_Sql.Append(Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Nombre + " AS nombre, ");
                    Mi_Sql.Append(Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id + " AS Programa_id, ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + " AS Fte_id, ");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Area_Funcional_ID + " AS Area_id, ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + " || '_' || ");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Area_Funcional_ID + " || '_' || ");
                    Mi_Sql.Append(Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id + " AS id, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Devengado + ") AS DEVENGADO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Ejercido + ") AS EJERCIDO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Pagado + ") AS PAGADO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Comprometido + ") AS COMPROMETIDO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Disponible + ") AS DISPONIBLE");
                    Mi_Sql.Append(" FROM " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas);
                    Mi_Sql.Append(" INNER JOIN " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                    Mi_Sql.Append(" ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID);
                    Mi_Sql.Append(" = " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id);
                    Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Anio);
                    Mi_Sql.Append(" = '" + Negocio.P_Anio + "'");
                    Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID);
                    Mi_Sql.Append(" = '" + Negocio.P_Fuente_Financiamiento_ID + "'");
                    Mi_Sql.Append(" INNER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias);
                    Mi_Sql.Append(" ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID);
                    Mi_Sql.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                    Mi_Sql.Append(" WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Area_Funcional_ID);
                    Mi_Sql.Append(" = '" + Negocio.P_Area_Funcional_ID + "'");
                    Mi_Sql.Append(" GROUP BY " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Clave + ", ");
                    Mi_Sql.Append(Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Nombre + ", ");
                    Mi_Sql.Append(Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id + ", ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID  + ", ");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Area_Funcional_ID);
                    Mi_Sql.Append(" ORDER BY " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id + " ASC");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros de los programas. Error: [" + Ex.Message + "]");
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Dependencias
            ///DESCRIPCIÓN          : consulta para obtener los datos de las dependencias
            ///PARAMETROS           : 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 30/Noviembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Dependencias(Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    Mi_Sql.Append("SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " ||' '|| ");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS nombre, ");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " AS Dependencia_id, ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + " AS Fte_id, ");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Area_Funcional_ID + " AS Area_id, ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + " AS Programa_id, ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + " || '_' || ");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Area_Funcional_ID + " || '_' || ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + " || '_' || ");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " AS id, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Devengado + ") AS DEVENGADO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Ejercido + ") AS EJERCIDO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Pagado + ") AS PAGADO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Comprometido + ") AS COMPROMETIDO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Disponible + ") AS DISPONIBLE");
                    Mi_Sql.Append(" FROM " + Cat_Dependencias.Tabla_Cat_Dependencias);
                    Mi_Sql.Append(" INNER JOIN " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                    Mi_Sql.Append(" ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID);
                    Mi_Sql.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                    Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Anio);
                    Mi_Sql.Append(" = '" + Negocio.P_Anio + "'");
                    Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID);
                    Mi_Sql.Append(" = '" + Negocio.P_Fuente_Financiamiento_ID + "'");
                    Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID);
                    Mi_Sql.Append(" = '" + Negocio.P_Programa_ID + "'");
                    Mi_Sql.Append(" WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Area_Funcional_ID + " = '" + Negocio.P_Area_Funcional_ID + "'");
                    Mi_Sql.Append(" GROUP BY " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + ", ");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + ", ");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ", ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + ", ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + ", ");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Area_Funcional_ID);
                    Mi_Sql.Append(" ORDER BY " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " ASC");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros de las depencencias. Error: [" + Ex.Message + "]");
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Partida_Especifica
            ///DESCRIPCIÓN          : consulta para obtener los datos de las partidas especificas
            ///PARAMETROS           : 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 30/Noviembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Partida_Especifica(Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    Mi_Sql.Append("SELECT " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave + " ||' '|| ");
                    Mi_Sql.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Descripcion + " AS nombre, ");
                    Mi_Sql.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " Partida_id, ");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " AS Dependencia_id, ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + " AS Fte_id, ");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Area_Funcional_ID + " AS Area_id, ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + " AS Programa_id, ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + " || '_' || ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + " || '_' || ");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " || '_' || ");
                    Mi_Sql.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " as id, ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Devengado + ", ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Ejercido + ", ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Pagado + ", ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Comprometido + ", ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Disponible);
                    Mi_Sql.Append(" FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
                    Mi_Sql.Append(" INNER JOIN " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                    Mi_Sql.Append(" ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID);
                    Mi_Sql.Append(" = " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
                    Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Anio);
                    Mi_Sql.Append(" = '" + Negocio.P_Anio + "'");
                    Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID);
                    Mi_Sql.Append(" = '" + Negocio.P_Dependencia_ID + "'");
                    Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID);
                    Mi_Sql.Append(" = '" + Negocio.P_Fuente_Financiamiento_ID + "'");
                    Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID);
                    Mi_Sql.Append(" = '" + Negocio.P_Programa_ID + "'");
                    Mi_Sql.Append(" INNER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias);
                    Mi_Sql.Append(" ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID);
                    Mi_Sql.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                    Mi_Sql.Append(" WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Area_Funcional_ID);
                    Mi_Sql.Append(" = '" + Negocio.P_Area_Funcional_ID + "'");
                    Mi_Sql.Append(" ORDER BY " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " ASC");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros de las partidas especificas. Error: [" + Ex.Message + "]");
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Anios
            ///DESCRIPCIÓN          : consulta para obtener los años presupuestados aprobados
            ///PARAMETROS           : 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 30/Noviembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Anios()
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    Mi_Sql.Append("SELECT DISTINCT " + Ope_Psp_Presupuesto_Aprobado.Campo_Anio);
                    Mi_Sql.Append(" FROM " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                    Mi_Sql.Append(" ORDER BY " + Ope_Psp_Presupuesto_Aprobado.Campo_Anio + " DESC");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros de los años. Error: [" + Ex.Message + "]");
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles
            ///DESCRIPCIÓN          : consulta para obtener los detalles de la clasificacion
            ///PARAMETROS           : 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 02/Diciembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Detalles(Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                DataTable Dt_Datos = new DataTable();
                DataTable Dt_Temp = new DataTable(); 
                try
                {
                    Mi_Sql.Append("SELECT DISTINCT " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_No_Reserva + ", ");
                    Mi_Sql.Append(Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Estatus + ", ");
                    Mi_Sql.Append(Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Saldo + ", ");
                    Mi_Sql.Append(Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Fecha + ", ");
                    Mi_Sql.Append("'' AS CARGO, '' AS ABONO, '' AS IMPORTE");
                    Mi_Sql.Append(" FROM " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + ", " + Cat_Dependencias.Tabla_Cat_Dependencias);
                    Mi_Sql.Append(", " + Ope_Psp_Registro_Movimientos.Tabla_Ope_Psp_Registro_Movimientos);
                    Mi_Sql.Append(" WHERE " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Dependencia_ID);
                    Mi_Sql.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                    Mi_Sql.Append(" AND " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_No_Reserva);
                    Mi_Sql.Append(" = " + Ope_Psp_Registro_Movimientos.Tabla_Ope_Psp_Registro_Movimientos + "." + Ope_Psp_Registro_Movimientos.Campo_No_Reserva);

                    //FILTRO POR AÑO
                    if (!String.IsNullOrEmpty(Negocio.P_Anio))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Anio);
                            Mi_Sql.Append(" = '" + Negocio.P_Anio + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Anio);
                            Mi_Sql.Append(" = '" + Negocio.P_Anio + "'");
                        }
                    }

                    //FILTRO POR FUENTE DE FINANCIAMIENTO
                    if(!String.IsNullOrEmpty(Negocio.P_Fuente_Financiamiento_ID))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Fte_Financimiento_ID);
                            Mi_Sql.Append(" = '" + Negocio.P_Fuente_Financiamiento_ID + "'");
                        }
                        else 
                        {
                            Mi_Sql.Append(" WHERE " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Fte_Financimiento_ID);
                            Mi_Sql.Append(" = '" + Negocio.P_Fuente_Financiamiento_ID + "'");
                        }
                    }

                    //FILTRO POR AREA FUNCIONAL
                    if (!String.IsNullOrEmpty(Negocio.P_Area_Funcional_ID))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Area_Funcional_ID);
                            Mi_Sql.Append(" = '" + Negocio.P_Area_Funcional_ID + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Area_Funcional_ID);
                            Mi_Sql.Append(" = '" + Negocio.P_Area_Funcional_ID + "'");
                        }
                    }

                    //FILTRO POR PROGRAMA 
                    if (!String.IsNullOrEmpty(Negocio.P_Programa_ID))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Proyecto_Programa_ID);
                            Mi_Sql.Append(" = '" + Negocio.P_Programa_ID + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Proyecto_Programa_ID);
                            Mi_Sql.Append(" = '" + Negocio.P_Programa_ID + "'");
                        }
                    }

                    //FILTRO POR DEPENDENCIA
                    if (!String.IsNullOrEmpty(Negocio.P_Dependencia_ID))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Dependencia_ID);
                            Mi_Sql.Append(" = '" + Negocio.P_Dependencia_ID + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Dependencia_ID);
                            Mi_Sql.Append(" = '" + Negocio.P_Dependencia_ID + "'");
                        }
                    }

                    //FILTRO POR PARTIDA ESPECIFICA
                    if (!String.IsNullOrEmpty(Negocio.P_Partida_Especifica_ID))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Partida_ID);
                            Mi_Sql.Append(" = '" + Negocio.P_Partida_Especifica_ID + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Partida_ID);
                            Mi_Sql.Append(" = '" + Negocio.P_Partida_Especifica_ID + "'");
                        }
                    }

                    //FILTRO POR TIPO DE DESCRIPCION
                    if (!String.IsNullOrEmpty(Negocio.P_Tipo_Descripcion))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Ope_Psp_Registro_Movimientos.Tabla_Ope_Psp_Registro_Movimientos + "." + Ope_Psp_Registro_Movimientos.Campo_Cargo);
                            Mi_Sql.Append(" = '" + Negocio.P_Tipo_Descripcion + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Ope_Psp_Registro_Movimientos.Tabla_Ope_Psp_Registro_Movimientos + "." + Ope_Psp_Registro_Movimientos.Campo_Cargo);
                            Mi_Sql.Append(" = '" + Negocio.P_Tipo_Descripcion + "'");
                        }
                    }

                    Mi_Sql.Append(" ORDER BY " + Ope_Psp_Reservas.Campo_No_Reserva+ " ASC");

                    Dt_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                    //RECORREMOS EL DATATABLE PARA OBTENER EL ULTIMO REGISTRO DE MOVIMIENTO Y LO AGREGAMOS AL DATATABLE
                    if (Dt_Datos != null)
                    {
                        if (Dt_Datos.Rows.Count > 0)
                        {
                            foreach (DataRow Dr in Dt_Datos.Rows)
                            {
                                Mi_Sql = new StringBuilder();
                                Mi_Sql.Append("SELECT " + Ope_Psp_Registro_Movimientos.Campo_Cargo + ", ");
                                Mi_Sql.Append(Ope_Psp_Registro_Movimientos.Campo_Abono + ", ");
                                Mi_Sql.Append(Ope_Psp_Registro_Movimientos.Campo_Importe);
                                Mi_Sql.Append(" FROM " + Ope_Psp_Registro_Movimientos.Tabla_Ope_Psp_Registro_Movimientos);
                                Mi_Sql.Append(" WHERE " + Ope_Psp_Registro_Movimientos.Campo_No_Reserva + " = '" + Dr["NO_RESERVA"].ToString().Trim() + "'");
                                
                                //FILTRO POR TIPO DE DESCRIPCION (DEVENGADO, EJERCIDO, PAGADO, COMPROMETIDO, DISPONIBLE)
                                if (!String.IsNullOrEmpty(Negocio.P_Tipo_Descripcion))
                                {
                                   Mi_Sql.Append(" AND " + Ope_Psp_Registro_Movimientos.Campo_Cargo + " = '" + Negocio.P_Tipo_Descripcion + "'");
                                }

                                Mi_Sql.Append(" ORDER BY " + Ope_Psp_Registro_Movimientos.Campo_Fecha_Creo + " DESC");

                                Dt_Temp = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                                if (Dt_Temp != null)
                                {
                                    if (Dt_Temp.Rows.Count > 0)
                                    {
                                        Dr["CARGO"] = Dt_Temp.Rows[0]["CARGO"].ToString().Trim();
                                        Dr["ABONO"] = Dt_Temp.Rows[0]["ABONO"].ToString().Trim();
                                        Dr["IMPORTE"] = Dt_Temp.Rows[0]["IMPORTE"].ToString().Trim();
                                    }
                                }
                            }
                        }
                    }
                    

                    return Dt_Datos;
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los detalles de la clasificacion. Error: [" + Ex.Message + "]");
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Capitulos
            ///DESCRIPCIÓN          : consulta para obtener los datos de los capitulos
            ///PARAMETROS           : 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 05/Diciembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Capitulos(Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    Mi_Sql.Append("SELECT " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Clave + " ||' '|| ");
                    Mi_Sql.Append(Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Descripcion + " AS nombre, ");
                    Mi_Sql.Append(Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Capitulo_ID + " AS id, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Devengado + ") AS DEVENGADO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Ejercido + ") AS EJERCIDO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Pagado + ") AS PAGADO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Comprometido + ") AS COMPROMETIDO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Disponible + ") AS DISPONIBLE");
                    Mi_Sql.Append(" FROM " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos);
                    Mi_Sql.Append(" INNER JOIN " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                    Mi_Sql.Append(" ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Capitulo_ID);
                    Mi_Sql.Append(" = " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Capitulo_ID);
                    Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Anio);
                    Mi_Sql.Append(" = '" + Negocio.P_Anio + "'");
                    Mi_Sql.Append(" GROUP BY " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Clave + ", ");
                    Mi_Sql.Append(Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Descripcion + ", ");
                    Mi_Sql.Append(Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Capitulo_ID);
                    Mi_Sql.Append(" ORDER BY " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Capitulo_ID + " ASC");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros de los capitulos. Error: [" + Ex.Message + "]");
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Conceptos
            ///DESCRIPCIÓN          : consulta para obtener los datos de los conceptos
            ///PARAMETROS           : 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 05/Diciembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Conceptos(Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    Mi_Sql.Append("SELECT " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Clave + " ||' '|| ");
                    Mi_Sql.Append(Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Descripcion + " AS nombre, ");
                    Mi_Sql.Append(Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID + " AS Concepto_id, ");
                    Mi_Sql.Append(Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Capitulo_ID + " || '_' || ");
                    Mi_Sql.Append(Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID + " AS id, ");
                    Mi_Sql.Append(Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Capitulo_ID + " Capitulo_id, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Devengado + ") AS DEVENGADO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Ejercido + ") AS EJERCIDO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Pagado + ") AS PAGADO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Comprometido + ") AS COMPROMETIDO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Disponible + ") AS DISPONIBLE");
                    Mi_Sql.Append(" FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto);
                    Mi_Sql.Append(" INNER JOIN " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas);
                    Mi_Sql.Append(" ON " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID);
                    Mi_Sql.Append(" = " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID);
                    Mi_Sql.Append(" INNER JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
                    Mi_Sql.Append(" ON " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID);
                    Mi_Sql.Append(" = " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID);
                    Mi_Sql.Append(" INNER JOIN " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                    Mi_Sql.Append(" ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID);
                    Mi_Sql.Append(" = " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
                    Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Anio);
                    Mi_Sql.Append(" = '" + Negocio.P_Anio + "'");
                    Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Capitulo_ID);
                    Mi_Sql.Append(" = '" + Negocio.P_Capitulo_ID + "'");
                    Mi_Sql.Append(" GROUP BY " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Clave + ", ");
                    Mi_Sql.Append(Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Descripcion + ", ");
                    Mi_Sql.Append(Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Capitulo_ID + ", ");
                    Mi_Sql.Append(Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID);
                    Mi_Sql.Append(" ORDER BY " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID + " ASC");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros de los conceptos. Error: [" + Ex.Message + "]");
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Partida_Generica
            ///DESCRIPCIÓN          : consulta para obtener los datos de las partidas genericas
            ///PARAMETROS           : 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 05/Diciembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Partida_Generica(Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    Mi_Sql.Append("SELECT " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Clave + " ||' '|| ");
                    Mi_Sql.Append(Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Descripcion + " AS nombre, ");
                    Mi_Sql.Append(Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID + " AS Partida_Generica_id, ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Capitulo_ID + " || '_' || ");
                    Mi_Sql.Append(Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID + " || '_' || ");
                    Mi_Sql.Append(Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID + " AS id, ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Capitulo_ID + " AS Capitulo_id, ");
                    Mi_Sql.Append(Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID + " AS Concepto_id, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Devengado + ") AS DEVENGADO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Ejercido + ") AS EJERCIDO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Pagado + ") AS PAGADO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Comprometido + ") AS COMPROMETIDO, ");
                    Mi_Sql.Append("SUM(" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Disponible + ") AS DISPONIBLE");
                    Mi_Sql.Append(" FROM " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas);
                    Mi_Sql.Append(" INNER JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
                    Mi_Sql.Append(" ON " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID);
                    Mi_Sql.Append(" = " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID);
                    Mi_Sql.Append(" INNER JOIN " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                    Mi_Sql.Append(" ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID);
                    Mi_Sql.Append(" = " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
                    Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Anio);
                    Mi_Sql.Append(" = '" + Negocio.P_Anio + "'");
                    Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Capitulo_ID);
                    Mi_Sql.Append(" = '" + Negocio.P_Capitulo_ID + "'");
                    Mi_Sql.Append(" WHERE " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID);
                    Mi_Sql.Append(" = '" + Negocio.P_Concepto_ID + "'");
                    Mi_Sql.Append("GROUP BY " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Clave + ", ");
                    Mi_Sql.Append(Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Descripcion + ", ");
                    Mi_Sql.Append(Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID + ", ");
                    Mi_Sql.Append(Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID + ", ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Capitulo_ID);
                    Mi_Sql.Append(" ORDER BY " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID + " ASC");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros de las partidas genericas. Error: [" + Ex.Message + "]");
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Partida
            ///DESCRIPCIÓN          : consulta para obtener los datos de las partidas especificas
            ///PARAMETROS           : 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 05/Diciembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Partida(Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    Mi_Sql.Append("SELECT " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave + " ||' '|| ");
                    Mi_Sql.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Descripcion + " AS nombre, ");
                    Mi_Sql.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " Partida_id, ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Capitulo_ID + " AS Capitulo_id, ");
                    Mi_Sql.Append(Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID + " AS Concepto_id, ");
                    Mi_Sql.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID + " AS Partida_Generica_id, ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Capitulo_ID + " || '_' || ");
                    Mi_Sql.Append(Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID + " || '_' || ");
                    Mi_Sql.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID + " || '_' || ");
                    Mi_Sql.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " as id, ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Devengado + ", ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Ejercido + ", ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Pagado + ", ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Comprometido + ", ");
                    Mi_Sql.Append(Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Disponible);
                    Mi_Sql.Append(" FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
                    Mi_Sql.Append(" INNER JOIN " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                    Mi_Sql.Append(" ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID);
                    Mi_Sql.Append(" = " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
                    Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Anio);
                    Mi_Sql.Append(" = '" + Negocio.P_Anio + "'");
                    Mi_Sql.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Capitulo_ID);
                    Mi_Sql.Append(" = '" + Negocio.P_Capitulo_ID + "'");
                    Mi_Sql.Append(" INNER JOIN " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto);
                    Mi_Sql.Append(" ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Capitulo_ID);
                    Mi_Sql.Append(" = " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Capitulo_ID);
                    Mi_Sql.Append(" AND " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID);
                    Mi_Sql.Append(" = '" + Negocio.P_Concepto_ID + "'");
                    Mi_Sql.Append(" WHERE " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID);
                    Mi_Sql.Append(" = '" + Negocio.P_Partida__Generica_ID + "'");
                    Mi_Sql.Append(" ORDER BY " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " ASC");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros de las partidas especificas. Error: [" + Ex.Message + "]");
                }
            }
        #endregion
    }
}
