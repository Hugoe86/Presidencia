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
using Presidencia.Reportes_Alta_Isseg.Negocio;

namespace Presidencia.Reportes_Alta_Isseg.Datos
{
    public class Cls_Rpt_Nom_Alta_Isseg_Datos
    {

        #region Consulta
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Alta_Empleado_Isseg
        /// DESCRIPCION : Consulta al personal que se dio de alta con el atributo de isseg
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        :Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 10/Marzo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Alta_Empleado_Isseg(Cls_Rpt_Nom_Alta_Isseg_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta para del Empleado
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL.Append("SELECT " );
                //  para el empleado
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado+ ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_CURP + ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre+ ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Sexo + ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Nacimiento + ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Calle + ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Colonia + ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Ciudad + ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estado+ ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Codigo_Postal + ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Telefono_Casa + ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Codigo_Programatico + ",");

                //  para la dependencia
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " as Clave_Dependencia, ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " as Nombre_Dependencia, ");
                
                //  para le sindicato
                if (!String.IsNullOrEmpty(Datos.P_Sindicato_ID))
                {
                    Mi_SQL.Append("TO_NUMBER(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Sindicato_ID + ") as NUMERO_SINDICATO ,");
                    Mi_SQL.Append(Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + "." + Cat_Nom_Sindicatos.Campo_Nombre + " as SINDICATO ,");
                }

                //  para el tipo de nomina
                Mi_SQL.Append(" " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + ", ");
                Mi_SQL.Append("to_number(" + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ") as Clave_Nomina_ID ");

                Mi_SQL.Append( " from ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados);

                //  para la union con la tabla de dependencia
                Mi_SQL.Append(" left outer join " + Cat_Dependencias.Tabla_Cat_Dependencias + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append("=" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                //  para la union con la tabla de tipos de nomina
                Mi_SQL.Append(" left outer join " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                //  para la union con la tabla sindiacato
                if (!String.IsNullOrEmpty(Datos.P_Sindicato_ID))
                {
                    Mi_SQL.Append(" join " + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + " on ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Sindicato_ID);
                    Mi_SQL.Append("=" + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + "." + Cat_Nom_Sindicatos.Campo_Sindicato_ID);
                }

                Mi_SQL.Append(" Where ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Aplica_ISSEG + "='SI' ");
                Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + "='ACTIVO' ");

                //  para las fechas
                Mi_SQL.Append(" AND TO_DATE(TO_CHAR(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Alta_Isseg + " , 'DD-MM-YY')) > = '" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Alta_Isseg_Inicio) + "' ");
                Mi_SQL.Append(" AND TO_DATE(TO_CHAR(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Alta_Isseg + " , 'DD-MM-YY')) < = '" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Alta_Isseg_Fin) + "' ");
                
                
                //  filtro por dependencia
                if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID))
                    Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "' ");
               

                //  filtro por tipo de nomina
                if (!String.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                    Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "' ");
              
                //filtro por nombre de empleado
                if (!String.IsNullOrEmpty(Datos.P_Nombre_Empleado))
                {
                    Mi_SQL.Append(" AND UPPER(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre);
                    Mi_SQL.Append(") LIKE '%" + Datos.P_Nombre_Empleado.ToUpper() + "%'  ");
                }

                //  para el sindicato
                if (!String.IsNullOrEmpty(Datos.P_Sindicato_ID))
                    Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Sindicato_ID + " = '" + Datos.P_Sindicato_ID + "' ");

                //  ordenar
                Mi_SQL.Append(" ORDER BY ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ", ");
                
                if (!String.IsNullOrEmpty(Datos.P_Sindicato_ID))
                    Mi_SQL.Append("SINDICATO, ");
                
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno+ ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre);

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