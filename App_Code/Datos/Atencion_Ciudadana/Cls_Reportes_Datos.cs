using System;
using System.Data;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Reportes_Atencion_Ciudadana.Negocios;

namespace Presidencia.Reportes_Atencion_Ciudadana.Datos
{

    public class Cls_Reportes_Atencion_Ciudadana_Datos
    {
        #region Metodos
        public Cls_Reportes_Atencion_Ciudadana_Datos()
        {
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Reporte_Detallado_Peticiones
        ///DESCRIPCIÓN: Realiza una consulta a la base de datos para buscar informacion 
        ///de una peticion y traer sus detalles por medio de Complemento_Sentencia_Sql de busqueda definidos
        ///PARAMETROS: 1.-Reporte, objeto de la calse de negocio que contiene los datos para realizar la consulta
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 6/Septiembre/2010 
        ///MODIFICO: Roberto González Oseguera
        ///FECHA_MODIFICO: 11-jun-2012
        ///CAUSA_MODIFICACIÓN: Se agregaron filtros opcionalies a la consulta por programa_id y asunto_id
        ///*******************************************************************************
        public static DataSet Consulta_Reporte_Detallado_Peticiones(Cls_Rpt_Ate_Reportes_Negocio Reporte)
        {
            String Mi_SQL = "SELECT PETICION." + Ope_Ate_Peticiones.Campo_Folio
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Estatus 
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Tipo_Solucion
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Fecha_Peticion
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Fecha_Solucion_Probable
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Apellido_Paterno + " || ' ' || "
                + "PETICION." + Ope_Ate_Peticiones.Campo_Apellido_Materno + " || ' ' || "
                + "PETICION." + Ope_Ate_Peticiones.Campo_Nombre_Solicitante + " AS  NOMBRE_SOLICITANTE" // subconsulta
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Origen_De_Registro + " AS MODULO_ORIGEN"
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Descripcion_Peticion
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Telefono
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Sexo
                + ", PETICION." + Ope_Ate_Peticiones.Campo_No_Peticion
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Anio_Peticion
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Programa_ID
                + ", ASUNTO." + Cat_Ate_Asuntos.Campo_Nombre + " AS ASUNTO"
                + ", COLONIA." + Cat_Ate_Colonias.Campo_Nombre + " AS COLONIA"
                + ", (SELECT " + Cat_Pre_Calles.Campo_Nombre + " FROM "
                + Cat_Pre_Calles.Tabla_Cat_Pre_Calles+ " WHERE "
                + Cat_Pre_Calles.Campo_Calle_ID + " = PETICION." + Ope_Ate_Peticiones.Campo_Calle_ID + ") CALLE"
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Descripcion_Solucion + " RESPUESTA"
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Numero_Exterior
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Numero_Interior
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Referencia
                + ", NVL(DEPENDENCIA." + Cat_Dependencias.Campo_Nombre + ",'SIN ASIGNAR') AS DEPENDENCIA"
                + " FROM "
                + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " PETICION LEFT JOIN "
                + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIA ON PETICION."
                + Ope_Ate_Peticiones.Campo_Colonia_ID + " = COLONIA." + Cat_Ate_Colonias.Campo_Colonia_ID
                + " LEFT JOIN " + Cat_Ate_Asuntos.Tabla_Cat_Ate_Asuntos + " ASUNTO "
                + "ON PETICION." + Ope_Ate_Peticiones.Campo_Asunto_ID + " = ASUNTO." + Cat_Ate_Asuntos.Campo_AsuntoID
                + " LEFT JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIA "
                + "ON ASUNTO." + Cat_Ate_Asuntos.Campo_DependenciaID + " = DEPENDENCIA." + Cat_Dependencias.Campo_Dependencia_ID
                + " WHERE ";

            // agregar filtros
            if (!string.IsNullOrEmpty(Reporte.P_Dependencia))
            {
                Mi_SQL += "PETICION." + Ope_Ate_Peticiones.Campo_Dependencia_ID + " = '" +
                    Reporte.P_Dependencia + "' AND ";
            }
            if (!string.IsNullOrEmpty(Reporte.P_Fecha_Inicio))
            {
                Mi_SQL += "PETICION." + Ope_Ate_Peticiones.Campo_Fecha_Peticion + " >= TO_DATE('"
                    + Reporte.P_Fecha_Inicio + " 00:00:00','dd/mm/yyyy hh24:mi:ss') AND ";
            }
            if (!string.IsNullOrEmpty(Reporte.P_Fecha_Fin))
            {
                Mi_SQL += "PETICION." + Ope_Ate_Peticiones.Campo_Fecha_Peticion + " <= TO_DATE('"
                    + Reporte.P_Fecha_Fin + " 23:59:59','dd/mm/yyyy hh24:mi:ss') AND ";
            }
            if (!string.IsNullOrEmpty(Reporte.P_Colonia))
            {
                Mi_SQL += "COLONIA." + Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Reporte.P_Colonia + "' AND ";
            }
            if (!string.IsNullOrEmpty(Reporte.P_Programa_ID))
            {
                Mi_SQL += "PETICION." + Ope_Ate_Peticiones.Campo_Programa_ID + " = '" + Reporte.P_Programa_ID + "' AND ";
            }
            if (!string.IsNullOrEmpty(Reporte.P_Asunto_ID))
            {
                Mi_SQL += "PETICION." + Ope_Ate_Peticiones.Campo_Asunto_ID + " = '" + Reporte.P_Asunto_ID + "' AND ";
            }
            if (!string.IsNullOrEmpty(Reporte.P_Tipo_Solucion))
            {
                Mi_SQL += "PETICION." + Ope_Ate_Peticiones.Campo_Tipo_Solucion + " = '" + Reporte.P_Tipo_Solucion + "' AND ";
            }
            if (!string.IsNullOrEmpty(Reporte.P_Estatus))
            {
                Mi_SQL += "PETICION." + Ope_Ate_Peticiones.Campo_Estatus + "= '" + Reporte.P_Estatus + "' AND ";
            }
            if (!string.IsNullOrEmpty(Reporte.P_Genera_Noticia))
            {
                Mi_SQL += "PETICION." + Ope_Ate_Peticiones.Campo_Genera_Noticia + " = '" + Reporte.P_Genera_Noticia + "' AND ";
            }
            if (!string.IsNullOrEmpty(Reporte.P_Folio_Vencido))
            {
                Mi_SQL += "PETICION." + Ope_Ate_Peticiones.Campo_Fecha_Solucion_Probable + " < SYSDATE AND "
                    + "PETICION." + Ope_Ate_Peticiones.Campo_Estatus + " != 'TERMINADA'";
            }
            if (!string.IsNullOrEmpty(Reporte.P_Sexo))
            {
                Mi_SQL += "PETICION." + Ope_Ate_Peticiones.Campo_Sexo + " = '" + Reporte.P_Sexo + "'";
            }
            if (!string.IsNullOrEmpty(Reporte.P_Filtros_Dinamicos))
            {
                Mi_SQL += Reporte.P_Filtros_Dinamicos;
            }

            // quitar AND o WHERE del final de la consulta
            if (Mi_SQL.EndsWith(" AND "))
            {
                Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
            }
            else if (Mi_SQL.EndsWith(" WHERE "))
            {
                Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
            }

            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Estadistica_Peticiones
        ///DESCRIPCIÓN: Forma y ejecuta una consulta de peticiones y regresa el dataset obtenido
        ///         con filtros opcionales por rango de fecha, 
        ///PARAMETROS:  
        ///         1. Filtros: instancia de la clase de negocio con los filtros para la búsqueda
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 22-jun-2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataSet Consulta_Estadistica_Peticiones(Cls_Rpt_Ate_Reportes_Negocio Filtros)
        {
            String Mi_SQL = "SELECT " + Ope_Ate_Peticiones.Campo_No_Peticion
                + "," + Ope_Ate_Peticiones.Campo_Anio_Peticion
                + "," + Ope_Ate_Peticiones.Campo_Programa_ID
                + ",CASE WHEN " + Ope_Ate_Peticiones.Campo_Tipo_Solucion + "='POSITIVA' THEN 'POSITIVAS' WHEN "
                + Ope_Ate_Peticiones.Campo_Tipo_Solucion + "='NEGATIVA' THEN 'NEGATIVAS'"
                + " WHEN " + Ope_Ate_Peticiones.Campo_Estatus + " IN ('GENERADA','EN PROCESO') THEN 'EN PROCESO' END AS ESTATUS"
                + ",NVL((SELECT " + Cat_Dependencias.Campo_Nombre + " FROM "
                + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE "
                + Cat_Dependencias.Campo_Dependencia_ID + "="
                + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." + Ope_Ate_Peticiones.Campo_Dependencia_ID + "), 'SIN ASIGNAR') DEPENDENCIA"
                + ",NVL((SELECT " + Cat_Ate_Asuntos.Campo_Nombre + " FROM "
                + Cat_Ate_Asuntos.Tabla_Cat_Ate_Asuntos + " WHERE "
                + Cat_Ate_Asuntos.Campo_AsuntoID + "="
                + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." + Ope_Ate_Peticiones.Campo_Asunto_ID + "), 'SIN ASIGNAR') ASUNTO"
                + ",(SELECT " + Cat_Ate_Programas.Campo_Nombre + " FROM "
                + Cat_Ate_Programas.Tabla_Cat_Ate_Programas + " WHERE "
                + Cat_Ate_Programas.Campo_Programa_ID + "="
                + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." + Ope_Ate_Peticiones.Campo_Programa_ID + ") ORIGEN";
            // si se especifican campos dinámicos
            if (!string.IsNullOrEmpty(Filtros.P_Campos_Dinamicos))
            {
                Mi_SQL += Filtros.P_Campos_Dinamicos;
            }
            Mi_SQL += " FROM "
                + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones
                + " WHERE ";

            // agregar filtros
            if (!string.IsNullOrEmpty(Filtros.P_Dependencia))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Dependencia_ID + " = '" +
                    Filtros.P_Dependencia + "' AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Fecha_Inicio))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Fecha_Peticion + " >= TO_DATE('"
                    + Filtros.P_Fecha_Inicio + " 00:00:00','dd/mm/yyyy hh24:mi:ss') AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Fecha_Fin))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Fecha_Peticion + " <= TO_DATE('"
                    + Filtros.P_Fecha_Fin + " 23:59:59','dd/mm/yyyy hh24:mi:ss') AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Colonia))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Colonia_ID + " = '" + Filtros.P_Colonia + "' AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Programa_ID))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Programa_ID + " = '" + Filtros.P_Programa_ID + "' AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Asunto_ID))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Asunto_ID + " = '" + Filtros.P_Asunto_ID + "' AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Tipo_Solucion))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Tipo_Solucion + " = '" + Filtros.P_Tipo_Solucion + "' AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Estatus))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Estatus + "= '" + Filtros.P_Estatus + "' AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Genera_Noticia))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Genera_Noticia + " = '" + Filtros.P_Genera_Noticia + "' AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Folio_Vencido))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Fecha_Solucion_Probable + " < SYSDATE AND "
                    + Ope_Ate_Peticiones.Campo_Estatus + " != 'TERMINADA'";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Sexo))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Sexo + " = '" + Filtros.P_Sexo + "'";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Filtros_Dinamicos))
            {
                Mi_SQL += Filtros.P_Filtros_Dinamicos;
            }

            // quitar AND o WHERE del final de la consulta
            if (Mi_SQL.EndsWith(" AND "))
            {
                Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
            }
            else if (Mi_SQL.EndsWith(" WHERE "))
            {
                Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
            }

            if (!string.IsNullOrEmpty(Filtros.P_Ordenamiento_Dinamico))
            {
                Mi_SQL += " ORDER BY " + Filtros.P_Ordenamiento_Dinamico;
            }

            // ejecutar consulta y regresar el dataset obtenido
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Estadistica_Peticiones_Por_Usuario
        ///DESCRIPCIÓN: Forma y ejecuta una consulta de peticiones y regresa el dataset obtenido
        ///         con filtros opcionales por rango de fecha, 
        ///PARAMETROS:  
        ///         1. Filtros: instancia de la clase de negocio con los filtros para la búsqueda
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 22-jun-2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataSet Consulta_Estadistica_Peticiones_Por_Usuario(Cls_Rpt_Ate_Reportes_Negocio Filtros)
        {
            String Mi_SQL = "SELECT " + Ope_Ate_Peticiones.Campo_Usuario_Creo
                + ",(SELECT " + Cat_Ate_Programas.Campo_Nombre + " FROM "
                + Cat_Ate_Programas.Tabla_Cat_Ate_Programas + " WHERE "
                + Cat_Ate_Programas.Campo_Programa_ID + "="
                + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." + Ope_Ate_Peticiones.Campo_Programa_ID + ") ORIGEN";
            // si se especifican campos dinámicos
            if (!string.IsNullOrEmpty(Filtros.P_Campos_Dinamicos))
            {
                Mi_SQL += Filtros.P_Campos_Dinamicos;
            }
            Mi_SQL += " FROM "
                + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones
                + " WHERE ";

            // agregar filtros
            if (!string.IsNullOrEmpty(Filtros.P_Fecha_Inicio))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Fecha_Peticion + " >= TO_DATE('"
                    + Filtros.P_Fecha_Inicio + " 00:00:00','dd/mm/yyyy hh24:mi:ss') AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Fecha_Fin))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Fecha_Peticion + " <= TO_DATE('"
                    + Filtros.P_Fecha_Fin + " 23:59:59','dd/mm/yyyy hh24:mi:ss') AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Programa_ID))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Programa_ID + " = '" + Filtros.P_Programa_ID + "' AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Asunto_ID))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Asunto_ID + " = '" + Filtros.P_Asunto_ID + "' AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Tipo_Solucion))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Tipo_Solucion + " = '" + Filtros.P_Tipo_Solucion + "' AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Estatus))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Estatus + "= '" + Filtros.P_Estatus + "' AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Filtros_Dinamicos))
            {
                Mi_SQL += Filtros.P_Filtros_Dinamicos;
            }

            // quitar AND o WHERE del final de la consulta
            if (Mi_SQL.EndsWith(" AND "))
            {
                Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
            }
            else if (Mi_SQL.EndsWith(" WHERE "))
            {
                Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
            }

            if (!string.IsNullOrEmpty(Filtros.P_Ordenamiento_Dinamico))
            {
                Mi_SQL += " ORDER BY " + Filtros.P_Ordenamiento_Dinamico;
            }

            // ejecutar consulta y regresar el dataset obtenido
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Tiempo_Promedio_Respuesta_Peticion
        ///DESCRIPCIÓN: Forma y ejecuta una consulta de tiempo promedio de respuesta restando 
        ///         la fecha de la petición a la fecha de respuesta y regresa el dataset obtenido
        ///PARAMETROS:  
        ///         1. Filtros: instancia de la clase de negocio con los filtros para la búsqueda
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 23-jun-2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataSet Consulta_Tiempo_Promedio_Respuesta_Peticion(Cls_Rpt_Ate_Reportes_Negocio Filtros)
        {
            String Mi_SQL = "SELECT ROUND(AVG(CAST(" + Ope_Ate_Peticiones.Campo_Fecha_Solucion_Real
                + " AS DATE) - CAST(" + Ope_Ate_Peticiones.Campo_Fecha_Peticion + " AS DATE)),2) DIAS"
                + ",NVL((SELECT " + Cat_Dependencias.Campo_Nombre + " FROM "
                + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE "
                + Cat_Dependencias.Campo_Dependencia_ID + "="
                + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." + Ope_Ate_Peticiones.Campo_Dependencia_ID + "), 'SIN ASIGNAR') DEPENDENCIA"
                + " FROM "
                + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones
                + " WHERE " + Ope_Ate_Peticiones.Campo_Estatus + "= 'TERMINADA' AND ";

            // agregar filtros
            if (!string.IsNullOrEmpty(Filtros.P_Dependencia))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Dependencia_ID + " = '" +
                    Filtros.P_Dependencia + "' AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Fecha_Inicio))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Fecha_Peticion + " >= TO_DATE('"
                    + Filtros.P_Fecha_Inicio + " 00:00:00','dd/mm/yyyy hh24:mi:ss') AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Fecha_Fin))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Fecha_Peticion + " <= TO_DATE('"
                    + Filtros.P_Fecha_Fin + " 23:59:59','dd/mm/yyyy hh24:mi:ss') AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Colonia))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Colonia_ID + " = '" + Filtros.P_Colonia + "' AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Programa_ID))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Programa_ID + " = '" + Filtros.P_Programa_ID + "' AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Asunto_ID))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Asunto_ID + " = '" + Filtros.P_Asunto_ID + "' AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Tipo_Solucion))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Tipo_Solucion + " = '" + Filtros.P_Tipo_Solucion + "' AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Genera_Noticia))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Genera_Noticia + " = '" + Filtros.P_Genera_Noticia + "' AND ";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Sexo))
            {
                Mi_SQL += Ope_Ate_Peticiones.Campo_Sexo + " = '" + Filtros.P_Sexo + "'";
            }
            if (!string.IsNullOrEmpty(Filtros.P_Filtros_Dinamicos))
            {
                Mi_SQL += Filtros.P_Filtros_Dinamicos;
            }

            // quitar AND o WHERE del final de la consulta
            if (Mi_SQL.EndsWith(" AND "))
            {
                Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
            }
            else if (Mi_SQL.EndsWith(" WHERE "))
            {
                Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
            }

            // agrupar por dependencia_id
            Mi_SQL += " GROUP BY " + Ope_Ate_Peticiones.Campo_Dependencia_ID;

            // ejecutar consulta y regresar el dataset obtenido
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Reporte_Acumulado_Peticiones
        ///DESCRIPCIÓN: Realiza una consulta a la base de datos para buscar información 
        ///de una petición para contar acumulados por medio de Complemento_Sentencia_Sql de búsqueda definidos
        ///PARAMETROS: 1.-Reporte, objeto de la clase de negocio que contiene los datos para realizar la consulta
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 7/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataSet Consulta_Reporte_Acumulado_Peticiones(Cls_Rpt_Ate_Reportes_Negocio Reporte)
        {

            String Mi_SQL = "SELECT DISTINCT(SELECT COUNT(PETICION." + Ope_Ate_Peticiones.Campo_Peticion_ID + ") "
                + "FROM " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " PETICION " + Complemento_Sentencia_Sql(Reporte) + ") TOTAL_PETICIONES"
                + ",(SELECT COUNT(PETICION." + Ope_Ate_Peticiones.Campo_Peticion_ID
                + ") FROM " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " PETICION " + Complemento_Sentencia_Sql(Reporte)
                + "AND PETICION." + Ope_Ate_Peticiones.Campo_Tipo_Solucion + "= 'POSITIVA') SOLUCIONADAS_POSITIVAMENTE"
                + ",(SELECT COUNT(PETICION." + Ope_Ate_Peticiones.Campo_Peticion_ID
                + ") FROM " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " PETICION" + Complemento_Sentencia_Sql(Reporte)
                + "AND PETICION." + Ope_Ate_Peticiones.Campo_Tipo_Solucion + " = 'NEGATIVA') SOLUCIONADAS_NEGATIVAMENTE"
                + ",(SELECT COUNT(PETICION." + Ope_Ate_Peticiones.Campo_Peticion_ID
                + ") FROM " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " PETICION" + Complemento_Sentencia_Sql(Reporte)
                + "AND PETICION." + Ope_Ate_Peticiones.Campo_Estatus + " IN ('EN PROCESO','GENERADA')) SOLUCION_PENDIENTE"
                + ",(SELECT COUNT(PETICION." + Ope_Ate_Peticiones.Campo_Peticion_ID
                + ") FROM " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " PETICION" + Complemento_Sentencia_Sql(Reporte)
                + "AND PETICION." + Ope_Ate_Peticiones.Campo_Estatus + " IN ('GENERADA')) POR_ASIGNAR"
                + ",(SELECT COUNT(PETICION." + Ope_Ate_Peticiones.Campo_Peticion_ID
                + ") FROM " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " PETICION" + Complemento_Sentencia_Sql(Reporte)
                + "AND PETICION." + Ope_Ate_Peticiones.Campo_Estatus + " != 'TERMINADA'"
                + "AND PETICION." + Ope_Ate_Peticiones.Campo_Fecha_Solucion_Probable + " < SYSDATE) PETICIONES_VENCIDAS"
                + " FROM "
                + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " PETICION";

            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Complemento_Sentencia_Sql
        ///DESCRIPCIÓN: una cadena de sql dependiendo de los Complemento_Sentencia_Sql de búsqueda seleccionados
        ///PARAMETROS: 1.-Reporte, objeto de la clase de negocio que contiene los datos para realizar la consulta
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 7/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static String Complemento_Sentencia_Sql(Cls_Rpt_Ate_Reportes_Negocio Reporte)
        {
            String Mi_SQL = " LEFT JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias
                + " COLONIA ON PETICION." + Ope_Ate_Peticiones.Campo_Colonia_ID + " = COLONIA." + Cat_Ate_Colonias.Campo_Colonia_ID
                + " LEFT JOIN " + Cat_Ate_Asuntos.Tabla_Cat_Ate_Asuntos + " ASUNTO "
                + "ON PETICION." + Ope_Ate_Peticiones.Campo_Asunto_ID + " = ASUNTO." + Cat_Ate_Asuntos.Campo_AsuntoID
                + " LEFT JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIA "
                + "ON ASUNTO." + Cat_Ate_Asuntos.Campo_DependenciaID + " = DEPENDENCIA." + Cat_Dependencias.Campo_Dependencia_ID + " WHERE ";

            if (!string.IsNullOrEmpty(Reporte.P_Fecha_Inicio))
            {
                Mi_SQL += "PETICION." + Ope_Ate_Peticiones.Campo_Fecha_Peticion + " >= TO_DATE('"
                    + Reporte.P_Fecha_Inicio + " 00:00:00','dd/mm/yyyy hh24:mi:ss') AND ";
            }
            if (!string.IsNullOrEmpty(Reporte.P_Fecha_Fin))
            {
                Mi_SQL += "PETICION." + Ope_Ate_Peticiones.Campo_Fecha_Peticion + " <= TO_DATE('"
                    + Reporte.P_Fecha_Fin + " 23:59:59','dd/mm/yyyy hh24:mi:ss') AND ";
            }
            if (Reporte.P_Dependencia != null)
            {
                Mi_SQL += "DEPENDENCIA." + Cat_Dependencias.Campo_Dependencia_ID + " = '" + Reporte.P_Dependencia + "' AND ";
            }
            if (Reporte.P_Colonia != null)
            {
                Mi_SQL += "COLONIA." + Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Reporte.P_Colonia + "' AND ";
            }
            if (!string.IsNullOrEmpty(Reporte.P_Programa_ID))
            {
                Mi_SQL += "PETICION." + Ope_Ate_Peticiones.Campo_Programa_ID + " = '" + Reporte.P_Programa_ID + "' AND ";
            }
            if (!string.IsNullOrEmpty(Reporte.P_Asunto_ID))
            {
                Mi_SQL += "PETICION." + Ope_Ate_Peticiones.Campo_Asunto_ID + " = '" + Reporte.P_Asunto_ID + "' AND ";
            }
            if (!string.IsNullOrEmpty(Reporte.P_Sexo))
            {
                Mi_SQL += "PETICION." + Ope_Ate_Peticiones.Campo_Sexo + " = '" + Reporte.P_Sexo + "' AND ";
            }
            if (!string.IsNullOrEmpty(Reporte.P_Filtros_Dinamicos))
            {
                Mi_SQL += Reporte.P_Filtros_Dinamicos;
            }

            // quitar AND o WHERE del final de la consulta
            if (Mi_SQL.EndsWith(" AND "))
            {
                Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
            }
            else if (Mi_SQL.EndsWith(" WHERE "))
            {
                Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
            }

            return Mi_SQL;
        }
        
        #endregion
    }
}
