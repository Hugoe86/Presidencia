using System;
using System.IO;
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
using System.Text;
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Reportes_nomina_Vacaciones_Empleados.Negocio;

namespace Presidencia.Reportes_nomina_Vacaciones_Empleados.Datos
{
    public class Cls_Rpt_Nom_Vacaciones_Empleados_Datos
    {
        #region Consutas
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Dias_Vacaciones
        /// DESCRIPCION : Consulta todos los datos del Empleado que tenga vacaciones
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        :Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 28/Marzo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Dias_Vacaciones(Cls_Rpt_Nom_Vacaciones_Empleados_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta 
            String Mi_SQL_Comparacion = String.Empty;
            DataTable Dt_Consulta = new DataTable();
            try
            {
               
                Mi_SQL.Append("Select ");                
                // para los datos del empleado
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Aplica_ISSEG + ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Codigo_Programatico + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
               
                //  para el nombre del empleado
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") as Nombre_Empleado,");
                
                //  para los dias tomados de vacaciones del empleado
                Mi_SQL.Append("(Select count(" + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det + "." + Ope_Nom_Vacaciones_Dia_Det.Campo_No_Vacacion + ")" +
                    " From " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det + 
                    " where " + Ope_Nom_Vacaciones_Dia_Det.Campo_Estatus + "='Pendiente' and " +
                    Ope_Nom_Vacaciones_Dia_Det.Campo_Estado + "='Pendiente' and " + 
                    Ope_Nom_Vacaciones_Dia_Det.Campo_Fecha + "<=" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." +
                    Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + " and " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det + "." +
                    Ope_Nom_Vacaciones_Dia_Det.Campo_No_Vacacion + "=" + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." +
                    OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion + ") as Cantidad, ");

                //  para el nombre de la dependencia
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " as Nombre_Dependencia, ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " as Clave_Dependencia, ");
                
                //  para los datos de la nomina
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ",");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + ",");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio + ",");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + ",");
                
                //  para el salario diario y dias de vacaciones
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario + ", ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_Cantidad_Dias + ", ");
                
                //  para los calculos del descuento
                Mi_SQL.Append("(" + Cat_Empleados.Campo_Salario_Diario + " * ");
                Mi_SQL.Append("(Select count(" + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det + "." + Ope_Nom_Vacaciones_Dia_Det.Campo_No_Vacacion + ")" +
                   " From " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det +
                   " where " + Ope_Nom_Vacaciones_Dia_Det.Campo_Estatus + "='Pendiente' and " +
                   Ope_Nom_Vacaciones_Dia_Det.Campo_Estado + "='Pendiente' and " +
                   Ope_Nom_Vacaciones_Dia_Det.Campo_Fecha + "<=" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." +
                   Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + " and " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det + "." +
                   Ope_Nom_Vacaciones_Dia_Det.Campo_No_Vacacion + "=" + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." +
                   OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion + ") ) as Sueldo_Normal_Vacaciones, ");

                Mi_SQL.Append("(((" + Cat_Empleados.Campo_Salario_Diario + " * (select " + Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Prevision_Social_Multiple +
                        " From " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + ")))) as Psm_Diario, ");

                Mi_SQL.Append("(((" + Cat_Empleados.Campo_Salario_Diario + " * (select " + Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Prevision_Social_Multiple +
                        " From " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + ")) + " + Cat_Empleados.Campo_Salario_Diario
                        + ") * " + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." +
                        OPE_NOM_VACACIONES_EMPLEADO.Campo_Cantidad_Dias + ") as Psm_Vacaciones ");


                Mi_SQL.Append(" FROM " + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado);

                Mi_SQL.Append(" left outer join " + Cat_Empleados.Tabla_Cat_Empleados + " on ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID + "=");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" left outer join " + Cat_Dependencias.Tabla_Cat_Dependencias + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append("=" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + " on ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Nomina);
                Mi_SQL.Append("=" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);
  
                Mi_SQL.Append(" WHERE ");

                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_Nomina_ID +
                    "='" + Datos.P_Nomina_id + "' ");

                Mi_SQL.Append(" AND " + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Nomina +
                    "=" + Datos.P_No_Nomina + " ");
                
                //  filtro para el numero del empleado
                if (!String.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    Mi_SQL.Append(" AND " + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID +
                        " in ( SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " +
                        Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "') ");
                }
                //  filtro para el tipo de nomina
                if (!String.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." +
                            Cat_Empleados.Campo_Tipo_Nomina_ID + " in ( SELECT " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID +
                            " FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " WHERE " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID +
                            "='" + Datos.P_Tipo_Nomina_ID + "') ");
                }

                //  filtro por dependencia
                if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    Mi_SQL.Append(" AND " + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." +
                            OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID + " in ( SELECT " + Cat_Empleados.Campo_Empleado_ID +
                            " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Dependencia_ID +
                            "='" + Datos.P_Dependencia_ID + "') ");
                }

                Mi_SQL.Append(" order by " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." +
                        Cat_Nom_Tipos_Nominas.Campo_Nomina + ", Nombre_Dependencia, Nombre_Empleado");

                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                return Dt_Consulta;
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        #endregion
    }
}