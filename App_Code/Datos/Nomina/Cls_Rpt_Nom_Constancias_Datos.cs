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
using Presidencia.Rpt_Constancias.Negocio;
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using System.Text;

namespace Presidencia.Rpt_Constancias.Datos
{
    public class Cls_Rpt_Nom_Constancias_Datos
    {
        
        #region METODOS

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Empleado
        ///DESCRIPCIÓN          : consulta para obtener los datos de los empleados
        ///PARAMETROS           1 Constancias_Negocio: conexion con la capa de negocios 
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 11/Abril/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static DataTable Consultar_Datos_Empleado(Cls_Rpt_Nom_Constancias_Negocio Constancias_Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS Empleado, ");
                Mi_Sql.Append("NVL(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + ",'') AS RFC, ");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + ", ");
                Mi_Sql.Append("NVL(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_IMSS + ",'') AS IMSS, ");
                Mi_Sql.Append("NVL(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Inicio + ",'') AS Fecha_Inicio, ");
                Mi_Sql.Append("NVL(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Baja + ", '') AS Fecha_Fin, ");
                Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS UR,");
                Mi_Sql.Append("NVL(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario + " *30.42, '0') AS Salario, ");
                Mi_Sql.Append("'Lunes a Viernes' AS Horario, ");
                Mi_Sql.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre + " AS Puesto ");
                Mi_Sql.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias);
                Mi_Sql.Append(" ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_Sql.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Puestos.Tabla_Cat_Puestos);
                Mi_Sql.Append(" ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Puesto_ID);
                Mi_Sql.Append(" = " + Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID);

                //filtro por numero de empleado
                if (!String.IsNullOrEmpty(Constancias_Negocio.P_No_Empleado))
                {
                    if (Mi_Sql.ToString().Contains("WHERE"))
                    {
                        Mi_Sql.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado);
                        Mi_Sql.Append(" = '" + Constancias_Negocio.P_No_Empleado + "'");
                    }
                    else
                    {
                        Mi_Sql.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado);
                        Mi_Sql.Append(" = '" + Constancias_Negocio.P_No_Empleado + "'");
                    }
                }

                //filtro por nombre de empleado
                if (!String.IsNullOrEmpty(Constancias_Negocio.P_Nombre_Empleado))
                {
                    if (Mi_Sql.ToString().Contains("WHERE"))
                    {
                        Mi_Sql.Append(" AND (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                        Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                        Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ")");
                        Mi_Sql.Append(" LIKE '%" + Constancias_Negocio.P_Nombre_Empleado.ToUpper() + "%'");
                        Mi_Sql.Append(" OR (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " || ' ' || ");
                        Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                        Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ")");
                        Mi_Sql.Append(" LIKE '%" + Constancias_Negocio.P_Nombre_Empleado.ToUpper() + "%'");
                    }
                    else
                    {
                        Mi_Sql.Append(" WHERE (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                        Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                        Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ")");
                        Mi_Sql.Append(" LIKE '%" + Constancias_Negocio.P_Nombre_Empleado.ToUpper() + "%'");
                        Mi_Sql.Append(" OR (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " || ' ' || ");
                        Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                        Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ")");
                        Mi_Sql.Append(" LIKE '%" + Constancias_Negocio.P_Nombre_Empleado.ToUpper() + "%'");
                    }
                }

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros de los contratos vencidos. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Empleado_Baja
        ///DESCRIPCIÓN          : consulta para obtener los datos de los empleados
        ///PARAMETROS           1 Constancias_Negocio: conexion con la capa de negocios 
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 11/Abril/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static DataTable Consultar_Datos_Empleado_Baja(Cls_Rpt_Nom_Constancias_Negocio Constancias_Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT " + Cat_Emp_Movimientos_Det.Campo_Motivo_Movimiento);
                Mi_Sql.Append(" FROM " + Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det);
                Mi_Sql.Append(" WHERE " + Cat_Emp_Movimientos_Det.Campo_Empleado_ID + " = '" + Constancias_Negocio.P_No_Empleado + "'");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los datos del empleado. Error: [" + Ex.Message + "]");
            }
        }
        #endregion
    }
}

