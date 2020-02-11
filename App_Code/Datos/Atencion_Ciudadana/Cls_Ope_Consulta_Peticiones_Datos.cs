using System;
using System.Data;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Consulta_Peticiones.Negocios;

namespace Presidencia.Consulta_Peticiones.Datos
{
    public class Cls_Ope_Consulta_Peticiones_Datos
    {
        #region Métodos
        public Cls_Ope_Consulta_Peticiones_Datos()
        {

        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Peticiones_General
        ///DESCRIPCIÓN: Realiza una consulta a la base de datos para buscar informacion 
        ///de una peticion por medio del folio o de la dependencia
        ///PARAMETROS: 1.-Negocio, objeto de la calse de Negocio que contiene los datos para realizar la consulta
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 27/Agosto/2010 
        ///MODIFICO: Roberto González Oseguera
        ///FECHA_MODIFICO: 26-may-2012
        ///CAUSA_MODIFICACIÓN: Se agregan filtros por acción y estatus y campos a la consulta
        ///*******************************************************************************
        public static DataSet Consulta_Peticiones_General(Cls_Ope_Consulta_Peticiones_Negocio Negocio)
        {
            String Mi_SQL = "SELECT PETICIONES." + Ope_Ate_Peticiones.Campo_Folio 
                + ", ASUNTOS." + Cat_Ate_Asuntos.Campo_Nombre + " AS ASUNTO"
                + ", RPAD(SUBSTR(PETICIONES." + Ope_Ate_Peticiones.Campo_Descripcion_Peticion + ",1,37),40,'.') AS PETICION"
                + ", PETICIONES." + Ope_Ate_Peticiones.Campo_Fecha_Peticion
                + ", PETICIONES." + Ope_Ate_Peticiones.Campo_Estatus
                + ", (SELECT " + Cat_Dependencias.Campo_Nombre + " FROM "
                + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE "
                + Cat_Dependencias.Campo_Dependencia_ID + "= PETICIONES." + Ope_Ate_Peticiones.Campo_Dependencia_ID + ") DEPENDENCIA"
                + ", PETICIONES." + Ope_Ate_Peticiones.Campo_Apellido_Paterno
                + "|| ' ' || PETICIONES." + Ope_Ate_Peticiones.Campo_Apellido_Materno + " || ' ' || PETICIONES."
                + Ope_Ate_Peticiones.Campo_Nombre_Solicitante + " AS NOMBRE_SOLICITANTE"
                + " FROM " 
                + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " PETICIONES LEFT JOIN "
                + Cat_Ate_Asuntos.Tabla_Cat_Ate_Asuntos + " ASUNTOS ON PETICIONES."
                + Ope_Ate_Peticiones.Campo_Asunto_ID + " = ASUNTOS." + Cat_Ate_Asuntos.Campo_AsuntoID + " WHERE ";

            // agregar filtros
            if (Negocio.P_Folio != null)
            {
                Mi_SQL += " UPPER(PETICIONES." + Ope_Ate_Peticiones.Campo_Folio +
                    ") like UPPER('%" + Negocio.P_Folio + "%') AND ";
            }
            if (!string.IsNullOrEmpty(Negocio.P_Area))
            {
                Mi_SQL += "PETICIONES." + Ope_Ate_Peticiones.Campo_Area_ID + " = '" +
                    Negocio.P_Area + "' AND ";
            }
            if (!string.IsNullOrEmpty(Negocio.P_Fecha_Inicio))
            {
                Mi_SQL += "PETICIONES." + Ope_Ate_Peticiones.Campo_Fecha_Peticion + " >= TO_DATE('" 
                    + Negocio.P_Fecha_Inicio + " 00:00:00','dd/mm/yyyy hh24:mi:ss') AND ";
            }
            if (!string.IsNullOrEmpty(Negocio.P_Fecha_Fin))
            {
                Mi_SQL += "PETICIONES." + Ope_Ate_Peticiones.Campo_Fecha_Peticion + " <= TO_DATE('"
                    + Negocio.P_Fecha_Fin + " 23:59:59','dd/mm/yyyy hh24:mi:ss') AND ";
            }
            if (!string.IsNullOrEmpty(Negocio.P_Dependencia))
            {
                Mi_SQL += "PETICIONES." + Ope_Ate_Peticiones.Campo_Dependencia_ID + " = '" +
                    Negocio.P_Dependencia + "' AND ";
            }
            if (!string.IsNullOrEmpty(Negocio.P_Asunto_ID))
            {
                Mi_SQL += "PETICIONES." + Ope_Ate_Peticiones.Campo_Asunto_ID + " = '" +
                    Negocio.P_Asunto_ID + "' AND ";
            }
            if (!string.IsNullOrEmpty(Negocio.P_Colonia_ID))
            {
                Mi_SQL += "PETICIONES." + Ope_Ate_Peticiones.Campo_Colonia_ID + " = '" +
                    Negocio.P_Colonia_ID + "' AND ";
            }
            if (!string.IsNullOrEmpty(Negocio.P_Estatus))
            {
                Mi_SQL += "PETICIONES." + Ope_Ate_Peticiones.Campo_Estatus + " = '" +
                    Negocio.P_Estatus + "' AND ";
            }
            if (Negocio.P_Programa_ID > 0)
            {
                Mi_SQL += "PETICIONES." + Ope_Ate_Peticiones.Campo_Programa_ID + " = '" +
                    Negocio.P_Programa_ID + "' AND ";
            }

            if (Negocio.P_Nombre_Solicitante != null)
            {
                Mi_SQL += "(UPPER(PETICIONES." + Ope_Ate_Peticiones.Campo_Nombre_Solicitante 
                    + " || ' ' || PETICIONES." + Ope_Ate_Peticiones.Campo_Apellido_Paterno
                    + " || ' ' || PETICIONES." + Ope_Ate_Peticiones.Campo_Apellido_Materno
                    + ") LIKE UPPER('%" + Negocio.P_Nombre_Solicitante + "%') OR "
                    + "UPPER(PETICIONES." + Ope_Ate_Peticiones.Campo_Apellido_Paterno
                    + " || ' ' || PETICIONES." + Ope_Ate_Peticiones.Campo_Apellido_Materno
                    + " || ' ' || PETICIONES." + Ope_Ate_Peticiones.Campo_Nombre_Solicitante
                    + ") LIKE UPPER('%" + Negocio.P_Nombre_Solicitante + "%')) AND ";
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

            DataSet data_set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return data_set;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Peticion_Detallada
        ///DESCRIPCIÓN: Realiza una consulta a la base de datos la cual regresa 
        ///información detallada de la petición que se requiere
        ///PARAMETROS: 1.-Negocio, objeto de la clase de Negocio que contiene los datos para realizar la consulta
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 30/Agosto/2010 
        ///MODIFICO: Roberto González Oseguera
        ///FECHA_MODIFICO: 26-may-2012
        ///CAUSA_MODIFICACIÓN: Se actualizan los campos (se modificó la estructura de la base de datos)
        ///*******************************************************************************
        public static DataSet Consulta_Peticion_Detallada(Cls_Ope_Consulta_Peticiones_Negocio Negocio)
        {
            string Mi_SQL = "SELECT PETICION." + Ope_Ate_Peticiones.Campo_Folio 
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Estatus + " AS ESTATUS"
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Fecha_Peticion + " AS FECHA_PETICION"
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Apellido_Paterno + " || ' ' || PETICION." 
                + Ope_Ate_Peticiones.Campo_Apellido_Materno + " || ' ' || PETICION."
                + Ope_Ate_Peticiones.Campo_Nombre_Solicitante + " AS NOMBRE_SOLICITANTE"
                + ", (SELECT " + Cat_Dependencias.Campo_Nombre + " FROM "
                + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE "
                + Cat_Dependencias.Campo_Dependencia_ID + "= PETICION." + Ope_Ate_Peticiones.Campo_Dependencia_ID + ") DEPENDENCIA"
                + ", (SELECT " + Cat_Pre_Calles.Campo_Nombre + " FROM " // subconsulta para obtener el nombre de la calle
                + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " WHERE "
                + Cat_Pre_Calles.Campo_Calle_ID + " = PETICION."+ Ope_Ate_Peticiones.Campo_Calle_ID + ") || ' ' || PETICION."
                + Ope_Ate_Peticiones.Campo_Numero_Exterior + " || ' ' || PETICION."
                + Ope_Ate_Peticiones.Campo_Numero_Interior + " || ' ' || COLONIA." + Cat_Ate_Colonias.Campo_Nombre + " AS DIRECCION"
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Referencia
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Email
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Origen_De_Registro + " AS MODULO_ORIGEN"
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Descripcion_Peticion + " AS PETICION"
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Descripcion_Solucion + " AS SOLUCION"
                + ", PETICION." + Ope_Ate_Peticiones.Campo_Fecha_Solucion_Real
                + " FROM " 
                + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " PETICION LEFT JOIN " 
                + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIA ON PETICION."
                + Ope_Ate_Peticiones.Campo_Colonia_ID + " = COLONIA." 
                + Cat_Ate_Colonias.Campo_Colonia_ID + " WHERE PETICION." 
                + Ope_Ate_Peticiones.Campo_Folio + " = '" + Negocio.P_Folio + "'";
            DataSet data_set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return data_set;
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consultar_Unidad_Responsable_De_Empleado
        ///DESCRIPCIÓN: regresa un datatable con los datos de las dependencias asignadas al empleado que se 
        ///             recibe como parámetro (solo las unidades que tienen peticiones asignadas)
        ///PARÁMETROS:
        /// 		1. Empleado_ID: id del empleado a consultar
        /// 		2. Dependencia_ID_Empleado: id de la dependencia a la que pertenece el empleado
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 29-oct-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Unidad_Responsable_De_Empleado(String Empleado_ID, string Dependencia_ID_Empleado)
        {
            String Mi_SQL = "";
            Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID + ", " +
            Cat_Dependencias.Campo_Clave + "||' '||" + Cat_Dependencias.Campo_Nombre + " AS CLAVE_NOMBRE" +
            " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
            " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " ='" + Dependencia_ID_Empleado + "'" +
            " AND " + Cat_Dependencias.Campo_Dependencia_ID + " IN (SELECT DISTINCT(" 
            + Ope_Ate_Peticiones.Campo_Dependencia_ID + ") FROM " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + ")" +
            " UNION ALL " +
            "SELECT " + Cat_Dependencias.Campo_Dependencia_ID + ", " +
            Cat_Dependencias.Campo_Clave + "||' '||" + Cat_Dependencias.Campo_Nombre + " AS CLAVE_NOMBRE" +
            " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
            " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " IN (SELECT " +
            Cat_Det_Empleado_UR.Campo_Dependencia_ID + " FROM " + Cat_Det_Empleado_UR.Tabla_Cat_Det_Empleado_UR +
            " WHERE " + Cat_Det_Empleado_UR.Campo_Empleado_ID + " ='" + Empleado_ID + "')" +
            " AND " + Cat_Dependencias.Campo_Dependencia_ID + " IN (SELECT DISTINCT("
            + Ope_Ate_Peticiones.Campo_Dependencia_ID + ") FROM " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + ")";

            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            DataTable Tabla = null;
            if (_DataSet.Tables.Count != 0)
            {
                Tabla = _DataSet.Tables[0];
            }
            return Tabla;
        }
    
        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consultar_Dependencias
        ///DESCRIPCIÓN: Genera y ejecuta una consulta a Cat_Dependencias en la base de datos
        ///PARÁMETROS:
        /// 		1. Negocio: instancia de la clase de negocio con los filtros para la consulta
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 17-may-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Dependencias(Cls_Ope_Consulta_Peticiones_Negocio Negocio)
        {
            DataTable Dt_datos = null;
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT "
                    + Cat_Dependencias.Campo_Dependencia_ID
                    + "," + Cat_Dependencias.Campo_Nombre
                    + "," + Cat_Dependencias.Campo_Clave
                    + "," + Cat_Dependencias.Campo_Comentarios
                    +" FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE ";

                // agregar filtros
                if (!String.IsNullOrEmpty(Negocio.P_Comentarios) && !String.IsNullOrEmpty(Negocio.P_Nombre) && !String.IsNullOrEmpty(Negocio.P_Clave)) // buscar en nombre, clave o descripción
                {
                    Mi_SQL += "(UPPER(" + Cat_Dependencias.Campo_Comentarios + ") LIKE UPPER('%" + Negocio.P_Comentarios + "%')"
                        + " OR UPPER(" + Cat_Dependencias.Campo_Nombre + ") LIKE UPPER('%" + Negocio.P_Nombre + "%')"
                        + " OR UPPER(" + Cat_Dependencias.Campo_Clave + ") LIKE UPPER('%" + Negocio.P_Clave + "%')) AND ";
                }
                else
                {
                    if (!String.IsNullOrEmpty(Negocio.P_Comentarios)) // buscar en nombre, clave o descripción
                    {
                        Mi_SQL += "UPPER(" + Cat_Dependencias.Campo_Comentarios + ") LIKE UPPER('%" + Negocio.P_Comentarios + "%') AND ";
                    }
                    if (!String.IsNullOrEmpty(Negocio.P_Clave))
                    {
                        Mi_SQL += " UPPER(" + Cat_Dependencias.Campo_Clave + ") LIKE UPPER('%" + Negocio.P_Clave + "%') AND ";
                    }
                    if (!String.IsNullOrEmpty(Negocio.P_Nombre))
                    {
                        Mi_SQL += "UPPER(" + Cat_Dependencias.Campo_Nombre + ") LIKE UPPER('%" + Negocio.P_Nombre + "%') AND ";
                    }
                }

                if (!String.IsNullOrEmpty(Negocio.P_Estatus))
                {
                    Mi_SQL += Cat_Dependencias.Campo_Estatus + " LIKE '%" + Negocio.P_Estatus + "%' ";
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

                // ejecutar consulta
                Dt_datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Dt_datos;
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consultar_Solicitantes
        ///DESCRIPCIÓN: Genera y ejecuta una consulta a Cat_Dependencias en la base de datos
        ///PARÁMETROS:
        /// 		1. Negocio: instancia de la clase de negocio con los filtros para la consulta
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 17-may-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Solicitantes(Cls_Ope_Consulta_Peticiones_Negocio Negocio)
        {
            DataTable Dt_datos = null;
            String Mi_SQL = "WITH SOLICITANTES AS (SELECT " + Ope_Ate_Peticiones.Campo_Apellido_Paterno
                + " || ' ' || NVL2(" + Ope_Ate_Peticiones.Campo_Apellido_Materno + ", " + Ope_Ate_Peticiones.Campo_Apellido_Materno
                + " || ' ', '') || " + Ope_Ate_Peticiones.Campo_Nombre_Solicitante + " NOMBRE_CIUDADANO" // subconsulta que concatena el nombre y apellidos (con nvl2 se omite el espacio vacío si no hay apellido materno)
                + ", " + Ope_Ate_Peticiones.Campo_Nombre_Solicitante
                + ", " + Ope_Ate_Peticiones.Campo_Apellido_Paterno
                + ", " + Ope_Ate_Peticiones.Campo_Apellido_Materno
                + ", " + Ope_Ate_Peticiones.Campo_Telefono
                + ", " + Ope_Ate_Peticiones.Campo_Email
                + ", " + Ope_Ate_Peticiones.Campo_Contribuyente_ID
                + ", (SELECT " + Cat_Ate_Colonias.Campo_Nombre + " FROM "
                + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " WHERE "
                + Cat_Ate_Colonias.Campo_Colonia_ID + " = "
                + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." + Ope_Ate_Peticiones.Campo_Colonia_ID + ") COLONIA" // subconsulta colonia
                + ", (SELECT " + Cat_Pre_Calles.Campo_Nombre + " FROM "
                + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " WHERE "
                + Cat_Pre_Calles.Campo_Calle_ID + " = "
                + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." + Ope_Ate_Peticiones.Campo_Calle_ID + ") CALLE" // subconsulta calle
                + ", " + Ope_Ate_Peticiones.Campo_Numero_Exterior
                + ", " + Ope_Ate_Peticiones.Campo_Numero_Interior
                + ", " + Ope_Ate_Peticiones.Campo_No_Peticion
                + ", " + Ope_Ate_Peticiones.Campo_Anio_Peticion
                + ", " + Ope_Ate_Peticiones.Campo_Programa_ID
                + ", RANK() OVER (PARTITION BY " + Ope_Ate_Peticiones.Campo_Nombre_Solicitante + ", " 
                + Ope_Ate_Peticiones.Campo_Apellido_Paterno + ", " + Ope_Ate_Peticiones.Campo_Apellido_Materno
                + " ORDER BY " + Ope_Ate_Peticiones.Campo_Fecha_Peticion + " DESC) RANGO" 
                + " FROM "
                + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones
                + ") SELECT * FROM SOLICITANTES WHERE RANGO=1";

            // agregar filtros
            if (!string.IsNullOrEmpty(Negocio.P_Nombre_Solicitante))
            {
                Mi_SQL += " AND UPPER(" + Ope_Ate_Peticiones.Campo_Nombre_Solicitante +
                    ") like UPPER('%" + Negocio.P_Nombre_Solicitante + "%')";
            }
            if (!string.IsNullOrEmpty(Negocio.P_Apellido_Paterno))
            {
                Mi_SQL += " AND UPPER(" + Ope_Ate_Peticiones.Campo_Apellido_Paterno +
                    ") like UPPER('%" + Negocio.P_Apellido_Paterno + "%')";
            }
            if (!string.IsNullOrEmpty(Negocio.P_Apellido_Materno))
            {
                Mi_SQL += " AND UPPER(" + Ope_Ate_Peticiones.Campo_Apellido_Materno +
                    ") like UPPER('%" + Negocio.P_Apellido_Materno + "%')";
            }
            if (!string.IsNullOrEmpty(Negocio.P_Email))
            {
                Mi_SQL += " AND " + Ope_Ate_Peticiones.Campo_Email + " = '" +
                    Negocio.P_Email + "'";
            }
            if (!string.IsNullOrEmpty(Negocio.P_Telefono))
            {
                Mi_SQL += " AND " + Ope_Ate_Peticiones.Campo_Telefono + " = '" +
                    Negocio.P_Telefono + "'";
            }

            Dt_datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_datos;
        }


        #endregion
    }
}
