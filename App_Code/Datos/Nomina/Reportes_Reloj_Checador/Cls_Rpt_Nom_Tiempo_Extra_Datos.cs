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
using Presidencia.Nomina_Reporte_Tiempo_Extra.Negocio;

namespace Presidencia.Nomina_Reporte_Tiempo_Extra.Datos
{
    public class Cls_Rpt_Nom_Tiempo_Extra_Datos
    {
        #region Consultas
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Tiempo_Extra
        /// DESCRIPCION : Consulta todos los datos del Empleado que tenga horas extras
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        :Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 22/Marzo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Tiempo_Extra(Cls_Rpt_Nom_Tiempo_Extra_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta para del Empleado
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL.Append("Select ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Horas + ", ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Fecha + ", ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario + ", ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") as Nombre_Empleado,");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Aplica_ISSEG + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave+ " as Clave_Dependencia, ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " as Nombre_Dependencia, ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + ", ");
                Mi_SQL.Append("(((" + Cat_Empleados.Campo_Salario_Diario + "/8))) as Salario_Hora,");
                Mi_SQL.Append("(((" + Cat_Empleados.Campo_Salario_Diario + "/8) * " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra +
                            "." + Ope_Nom_Tiempo_Extra.Campo_Horas + " * 2 )) Salario_Normal_Extra,");
                Mi_SQL.Append("(((" + Cat_Empleados.Campo_Salario_Diario + " * ( select " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros +
                           "." + Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Prevision_Social_Multiple + " from " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros +
                           ")))) as Psm_Diario, ");
                Mi_SQL.Append("(((" + Cat_Empleados.Campo_Salario_Diario + " * ( select " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros +
                           "." + Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Prevision_Social_Multiple + " from " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros +
                           "))/8)) as Psm_Hora, ");
                Mi_SQL.Append("(((" + Cat_Empleados.Campo_Salario_Diario + " * ( select " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros +
                          "." + Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Prevision_Social_Multiple + " from " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros +
                          ") + " + Cat_Empleados.Campo_Salario_Diario + ")/8) * " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Horas + " * 2 ) as Psm_Extra ");

                Mi_SQL.Append(" from " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra);
                
                Mi_SQL.Append(" left outer join " + Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + " on ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_No_Tiempo_Extra + "=");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_No_Tiempo_Extra);

                Mi_SQL.Append(" left outer join " + Cat_Empleados.Tabla_Cat_Empleados + " on ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID + "=");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" left outer join " + Cat_Dependencias.Tabla_Cat_Dependencias + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append("=" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                Mi_SQL.Append(" where " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Nomina_ID +
                            "='" + Datos.P_Nomina_id + "' ");

                Mi_SQL.Append(" and " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_No_Nomina +
                            "=" + Datos.P_No_Nomina + " ");

                if(!String.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    Mi_SQL.Append(" and " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "' ");
                }

                Mi_SQL.Append(" order by " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ",");
                Mi_SQL.Append(" Nombre_Dependencia, Nombre_Empleado");
            
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

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Dia_Festivo
        /// DESCRIPCION : Consulta todos los datos del Empleado que tenga dias festivos laborables
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        :Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 23/Marzo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Dia_Festivo(Cls_Rpt_Nom_Tiempo_Extra_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta para del Empleado
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL.Append("Select ");
                Mi_SQL.Append(Tab_Nom_Dias_Festivos.Tabla_Tab_Nom_Dias_Festivos + "." + Tab_Nom_Dias_Festivos.Campo_Fecha + ", ");
                Mi_SQL.Append(Tab_Nom_Dias_Festivos.Tabla_Tab_Nom_Dias_Festivos + "." + Tab_Nom_Dias_Festivos.Campo_Descripcion + ", ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_No_Nomina + ", ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario + ", ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") as Nombre_Empleado,");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Aplica_ISSEG + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " as Clave_Dependencia, ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " as Nombre_Dependencia, ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + ", ");
                Mi_SQL.Append("((" + Cat_Empleados.Campo_Salario_Diario + " * 2 )) as Salario_Dia_Extra,");

                Mi_SQL.Append("(((" + Cat_Empleados.Campo_Salario_Diario + " * ( select " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros +
                           "." + Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Prevision_Social_Multiple + " from " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros +
                           ")))) as Psm_Diario, ");
                
                Mi_SQL.Append("(((" + Cat_Empleados.Campo_Salario_Diario + " * ( select " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros +
                          "." + Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Prevision_Social_Multiple + " from " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros +
                          ") + " + Cat_Empleados.Campo_Salario_Diario + ")  * 2 )) as Psm_Extra ");

                Mi_SQL.Append(" from " + Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos);

                Mi_SQL.Append(" right outer join  " + Tab_Nom_Dias_Festivos.Tabla_Tab_Nom_Dias_Festivos + " on ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_Dia_ID + "=");
                Mi_SQL.Append(Tab_Nom_Dias_Festivos.Tabla_Tab_Nom_Dias_Festivos + "." + Tab_Nom_Dias_Festivos.Campo_Dia_ID);

                Mi_SQL.Append(" right outer join  " + Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + " on ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_No_Dia_Festivo + "=");
                Mi_SQL.Append(Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + "." + Ope_Nom_Dias_Festivo_Emp_Det.Campo_No_Dia_Festivo);

                Mi_SQL.Append(" left outer join " + Cat_Empleados.Tabla_Cat_Empleados + " on ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + "." + Ope_Nom_Dias_Festivo_Emp_Det.Campo_Empleado_ID + "=");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" left outer join " + Cat_Dependencias.Tabla_Cat_Dependencias + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append("=" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                Mi_SQL.Append(" where " + Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_Nomina_ID +
                            "='" + Datos.P_Nomina_id + "' ");

                Mi_SQL.Append(" and " + Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_No_Nomina +
                            "=" + Datos.P_No_Nomina + " ");

                if (!String.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    Mi_SQL.Append(" and " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "' ");
                }

                Mi_SQL.Append(" order by " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ",");
                Mi_SQL.Append(" Nombre_Dependencia, Nombre_Empleado");

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
        
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Prima_Dominical
        /// DESCRIPCION : Consulta todos los datos del Empleado que tenga dias laborables en domingo
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        :Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 23/Marzo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Prima_Dominical(Cls_Rpt_Nom_Tiempo_Extra_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta para del Empleado
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL.Append("Select ");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_Fecha + ", ");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_No_Nomina + ", ");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario + ", ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") as Nombre_Empleado,");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Aplica_ISSEG + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " as Clave_Dependencia, ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " as Nombre_Dependencia, ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + ", ");
               
                Mi_SQL.Append("((" + Cat_Empleados.Campo_Salario_Diario + ") * ( ( ( select " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros +
                            "." + Cat_Nom_Parametros.Campo_Porcentaje_Prima_Dominical +" from " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros +
                            ") / 100 )  ) ) as Salario_Dia_Extra,");

                Mi_SQL.Append("(((" + Cat_Empleados.Campo_Salario_Diario + " * ( select " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros +
                           "." + Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Prevision_Social_Multiple + " from " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros +
                           ")))) as Psm_Diario, ");
                
                Mi_SQL.Append("(((" + Cat_Empleados.Campo_Salario_Diario + " * ( select " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros +
                          "." + Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Prevision_Social_Multiple + " from " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros +
                          ") + " + Cat_Empleados.Campo_Salario_Diario + ") * ( ( ( Select " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros +
                            "." + Cat_Nom_Parametros.Campo_Porcentaje_Prima_Dominical + " from " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros +
                          " ) / 100)  )  ) ) as Psm_Extra ");

                Mi_SQL.Append(" from " + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos);

                Mi_SQL.Append(" right outer join  " + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + " on ");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado_Detalles.Campo_No_Domingo + "=");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + "." + Ope_Nom_Domingos_Empleado_Detalles.Campo_No_Domingo);

                Mi_SQL.Append(" left outer join " + Cat_Empleados.Tabla_Cat_Empleados + " on ");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + "." + Ope_Nom_Domingos_Empleado_Detalles.Campo_Empleado_ID + "=");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" left outer join " + Cat_Dependencias.Tabla_Cat_Dependencias + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append("=" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                Mi_SQL.Append(" where " + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_Nomina_ID +
                            "='" + Datos.P_Nomina_id + "' ");

                Mi_SQL.Append(" and " + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_No_Nomina +
                            "=" + Datos.P_No_Nomina + " ");

                if (!String.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    Mi_SQL.Append(" and " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "' ");
                }

                Mi_SQL.Append(" order by " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ",");
                Mi_SQL.Append(" Nombre_Dependencia, Nombre_Empleado");

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