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
using Presidencia.SUA.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;

namespace Presidencia.SUA.Datos
{
    public class Cls_Rpt_Nom_Generacion_Archivo_SUA
    {
        #region (Métodos)
        /// ************************************************************************************************************************
        /// Nombre: Consultar_Empleados
        /// 
        /// Descripción: Método que consulta la información de los empleados.
        /// 
        /// Parámetros: No Aplica.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: Enero/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ************************************************************************************************************************ 
        public static DataTable Consultar_Empleados(Cls_Rpt_Nom_Generacion_Arch_SUA_Negocio Datos)
        {
            String Mi_Oracle = "";//Variable que alamcenara la consulta.
            DataTable Dt_Empleados = null;//Variable que almacenara una lista de empleados.
            try
            {
                Mi_Oracle = "SELECT ";

                Mi_Oracle += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ";

                Mi_Oracle += " (select " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias + "." + Cat_Grupos_Dependencias.Campo_Registro_Patronal;
                Mi_Oracle += " from " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias;
                Mi_Oracle += " where " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias + "." + Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID + "=";
                Mi_Oracle += " (select " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Grupo_Dependencia_ID;
                Mi_Oracle += " from " + Cat_Dependencias.Tabla_Cat_Dependencias;
                Mi_Oracle += " where ";
                Mi_Oracle += Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "=";
                Mi_Oracle += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + ")) AS REGISTRO_PATRONAL, ";

                Mi_Oracle += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + ", ";

                Mi_Oracle += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_IMSS + " AS NO_AFILIACION, ";

                Mi_Oracle = Mi_Oracle + "(" + Cat_Empleados.Campo_Apellido_Paterno;
                Mi_Oracle = Mi_Oracle + "||' '||" + Cat_Empleados.Campo_Apellido_Materno;
                Mi_Oracle = Mi_Oracle + "||' '||" + Cat_Empleados.Campo_Nombre + ") AS Empleado, ";

                Mi_Oracle += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario_Integrado + ", ";

                Mi_Oracle += "(select (" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " || ' ' || ";
                Mi_Oracle += Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") from ";
                Mi_Oracle += Cat_Dependencias.Tabla_Cat_Dependencias + " where ";
                Mi_Oracle += Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID;
                Mi_Oracle += "=" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + ") AS DEPENDENCIA, ";

                Mi_Oracle = Mi_Oracle + " TO_CHAR(" + Cat_Empleados.Campo_Fecha_Inicio + ", 'DD-Mon-YYYY') AS FECHA_ALTA ";

                Mi_Oracle = Mi_Oracle + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;

                if (!string.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_RFC_Empleado))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC_Empleado + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC_Empleado + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Nombre_Empleado))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND UPPER(" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Nombre_Empleado + "%')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE UPPER(" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Nombre_Empleado + "%')";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Unidad_Responsable))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Unidad_Responsable + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Unidad_Responsable + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Registro_Patronal))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Dependencia_ID + " IN (SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + "." +
                            Cat_Dependencias.Campo_Dependencia_ID + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE " +
                            Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Grupo_Dependencia_ID + "=" +
                            " (SELECT " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias + "." + Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID +
                            " FROM " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias + 
                            " WHERE " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias + "." + Cat_Grupos_Dependencias.Campo_Registro_Patronal + "='" + Datos.P_Registro_Patronal + "'))";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Dependencia_ID + " IN (SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + "." +
                            Cat_Dependencias.Campo_Dependencia_ID + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE " +
                            Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Grupo_Dependencia_ID + "=" +
                            " (SELECT " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias + "." + Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID +
                            " FROM " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias + 
                            " WHERE " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias + "." + Cat_Grupos_Dependencias.Campo_Registro_Patronal + "='" + Datos.P_Registro_Patronal + "'))";
                    }
                }

                if (Mi_Oracle.Contains("WHERE"))
                {
                    Mi_Oracle += " AND " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }
                else
                {
                    Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }


                Mi_Oracle += " ORDER BY ";
                Mi_Oracle += "(select " +  Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " from ";
                Mi_Oracle += Cat_Dependencias.Tabla_Cat_Dependencias + " where ";
                Mi_Oracle += Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID;
                Mi_Oracle += "=" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "), ";
                Mi_Oracle += Cat_Empleados.Campo_Apellido_Paterno + " ASC";

                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Empleados;
        }
        #endregion
    }
}
