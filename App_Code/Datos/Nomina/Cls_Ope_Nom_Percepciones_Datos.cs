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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Faltas_Empleado.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Vacaciones_Empleado.Negocio;
using Presidencia.Tiempo_Extra.Negocio;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Cat_Parametros_Nomina.Negocio;
using Presidencia.Zona_Economica.Negocios;
using Presidencia.Domingos_Trabajados.Negocios;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Ope_Dias_Festivos.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Ajuste_ISR.Negocio;
using System.Text;
using Presidencia.DateDiff;

namespace Presidencia.Calculo_Percepciones.Datos
{
    public class Cls_Ope_Nom_Percepciones_Datos
    {
        #region (Consulta Incidencias Empleados)
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Percepciones_Var_Empleado_Periodo_Nominal
        /// DESCRIPCION : Consulta las Percepciones Variables que a trabajado el empleado en el periodo seleccionado. 
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.
        ///              Nomina_ID: Identificador de la nómina.
        ///              No_Nomina: Identificador del periodo nominal a consultar las incidencias de tiempo extra.
        ///              
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 23/Febrero/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static DataTable Consultar_Percepciones_Var_Empleado_Periodo_Nominal(String Empleado_ID, String Nomina_ID, Int32 No_Nomina, String Percepcion_Variable_ID)
        {
            DataTable Dt_Percepciones_Variables = null;//Almacenará el listado de horas extra.
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta de horas extra. 

            try
            {
                Mi_SQL.Append(" SELECT ");
                Mi_SQL.Append("SUM(" + Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + "." + Ope_Nom_Perc_Var_Emp_Det.Campo_Cantidad + ") AS CANTIDAD ");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + " INNER JOIN " + Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var);
                Mi_SQL.Append(" ON ");
                Mi_SQL.Append(Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + "." + Ope_Nom_Perc_Var_Emp_Det.Campo_No_Percepcion);
                Mi_SQL.Append(" = ");
                Mi_SQL.Append(Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." + Ope_Nom_Percepciones_Var.Campo_No_Percepcion);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." + Ope_Nom_Percepciones_Var.Campo_Estatus + "='Aceptado'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + "." + Ope_Nom_Perc_Var_Emp_Det.Campo_Estatus + "='Autorizado'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + "." + Ope_Nom_Perc_Var_Emp_Det.Campo_Empleado_ID + "='" + Empleado_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." + Ope_Nom_Percepciones_Var.Campo_Percepcion_Deduccion_ID + "='" + Percepcion_Variable_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append("(" + Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." + Ope_Nom_Percepciones_Var.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." + Ope_Nom_Percepciones_Var.Campo_No_Nomina + "=" + No_Nomina + ")");

                Dt_Percepciones_Variables = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las percepciones variables de los empleados. Error: " + Ex.Message + "]");
            }
            return Dt_Percepciones_Variables;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Tiempo_Extra_Empleado_Periodo_Nominal
        /// DESCRIPCION : Consulta las Tiempo Extra que a trabajado el empleado en el periodo seleccionado. 
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.
        ///              Nomina_ID: Identificador de la nómina.
        ///              No_Nomina: Identificador del periodo nominal a consultar las incidencias de tiempo extra.
        ///              
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 23/Febrero/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static DataTable Consultar_Tiempo_Extra_Empleado_Periodo_Nominal(String Empleado_ID, String Nomina_ID, Int32 No_Nomina)
        {
            DataTable Dt_Tiempo_Extra = null;//Almacenará el listado de horas extra.
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta de horas extra. 

            try
            {
                Mi_SQL.Append(" SELECT ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + ".*, " + Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + ".*");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + " INNER JOIN " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra);
                Mi_SQL.Append(" ON ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_No_Tiempo_Extra);
                Mi_SQL.Append(" = ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_No_Tiempo_Extra);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Estatus + "='Aceptado'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Estatus + "='Autorizado'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID + "='" + Empleado_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append("(" + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_No_Nomina + "=" + No_Nomina + ")");

                Dt_Tiempo_Extra = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el tiempo extra de los empleados. Error: " + Ex.Message + "]");
            }
            return Dt_Tiempo_Extra;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Dias_Festivos_Empleado_Periodo_Nominal
        /// DESCRIPCION : Consulta los dias festivos que a trabajado el empleado en el periodo nominal seleccionado. 
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.
        ///              Nomina_ID: Identificador de la nómina.
        ///              No_Nomina: Identificador del periodo nominal a consultar las incidencias de tiempo extra.
        ///              
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 23/Febrero/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static DataTable Consultar_Dias_Festivos_Empleado_Periodo_Nominal(String Empleado_ID, String Nomina_ID, Int32 No_Nomina)
        {
            DataTable Dt_Dias_Festivos = null;//Almacenará el listado de dias festivos
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta de dias festivos. 

            try
            {
                Mi_SQL.Append(" SELECT ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + ".*, " + Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + ".*");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + " INNER JOIN " + Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos);
                Mi_SQL.Append(" ON ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + "." + Ope_Nom_Dias_Festivo_Emp_Det.Campo_No_Dia_Festivo);
                Mi_SQL.Append(" = ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_No_Dia_Festivo);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_Estatus + "='Aceptado'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + "." + Ope_Nom_Dias_Festivo_Emp_Det.Campo_Estatus + "='Autorizado'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + "." + Ope_Nom_Dias_Festivo_Emp_Det.Campo_Empleado_ID + "='" + Empleado_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append("(" + Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                Mi_SQL.Append("  AND ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_No_Nomina + "=" + No_Nomina + ")");

                Dt_Dias_Festivos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los dias festivos de los empleados. Error: " + Ex.Message + "]");
            }
            return Dt_Dias_Festivos;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Dias_Domingos_Empleado_Periodo_Nominal
        /// DESCRIPCION : Consulta los dias domingos que a trabajado el empleado en el periodo nominal seleccionado. 
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.
        ///              Nomina_ID: Identificador de la nómina.
        ///              No_Nomina: Identificador del periodo nominal a consultar las incidencias de tiempo extra.
        ///              
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 23/Febrero/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static DataTable Consultar_Dias_Domingos_Empleado_Periodo_Nominal(String Empleado_ID, String Nomina_ID, Int32 No_Nomina)
        {
            DataTable Dt_Dias_Domingos = null;//Almacenará el listado de dias festivos
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta de dias festivos. 

            try
            {
                Mi_SQL.Append(" SELECT ");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + ".*, " + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + ".*");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + " INNER JOIN " + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos);
                Mi_SQL.Append(" ON ");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + "." + Ope_Nom_Domingos_Empleado_Detalles.Campo_No_Domingo);
                Mi_SQL.Append(" = ");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_No_Domingo);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append("UPPER(" + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_Estatus + ")=UPPER('ACEPTADO')");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append("UPPER(" + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + "." + Ope_Nom_Domingos_Empleado_Detalles.Campo_Estatus + ")=UPPER('ACEPTADO')");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + "." + Ope_Nom_Domingos_Empleado_Detalles.Campo_Empleado_ID + "='" + Empleado_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append("(" + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_No_Nomina + "=" + No_Nomina + ")");

                Dt_Dias_Domingos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los dias domingos de los empleados. Error: " + Ex.Message + "]");
            }
            return Dt_Dias_Domingos;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Vacaciones_Empleado_Periodo_Nominal
        /// DESCRIPCION : Consulta los dias que ha tomado el empleado de vacaciones en el periodo nominal seleccionado. 
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.
        ///              Nomina_ID: Identificador de la nómina.
        ///              No_Nomina: Identificador del periodo nominal a consultar las incidencias de tiempo extra.
        ///              
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 23/Febrero/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static DataTable Consultar_Vacaciones_Empleado_Periodo_Nominal(String Empleado_ID, String Nomina_ID, Int32 No_Nomina, DateTime Fecha)
        {
            DataTable Dt_Vacaciones = null;//Almacenará el listado de dias festivos
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta de dias festivos. 
            String Mi_SQL_2 = "";
            DataTable Dt_Periodos_Regulares = null;
            Int32 No_Nomina_Anio_Anterior = 0;

            try
            {
                Mi_SQL.Append(" SELECT " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det + ".*");
                Mi_SQL.Append(" FROM " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det);
                Mi_SQL.Append(" WHERE " + Ope_Nom_Vacaciones_Dia_Det.Campo_Estatus + "='Pendiente'");
                Mi_SQL.Append(" AND " + Ope_Nom_Vacaciones_Dia_Det.Campo_Estado + "='Pendiente'");
                Mi_SQL.Append(" AND " + Ope_Nom_Vacaciones_Dia_Det.Campo_Fecha + " <= '" + string.Format("{0:dd/MM/yyyy}", Fecha) + "'");
                Mi_SQL.Append(" AND " + Ope_Nom_Vacaciones_Dia_Det.Campo_No_Vacacion + " IN ");
                Mi_SQL.Append(" (SELECT ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion);
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Campo_Estatus + "='Autorizado'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID + "='" + Empleado_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append("(" + OPE_NOM_VACACIONES_EMPLEADO.Campo_Nomina_ID + "='" + Nomina_ID + "' AND " + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Nomina + "=" + No_Nomina);

                if (No_Nomina > 1)
                {
                    Mi_SQL.Append(" OR ");
                    Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Campo_Nomina_ID + "='" + Nomina_ID + "' AND " + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Nomina + "=" + (No_Nomina - 1));
                }
                else if (No_Nomina == 1)
                {
                    String Nomina_Anterior = String.Format("{0:00000}", (Convert.ToInt32(Nomina_ID) - 1));

                    Mi_SQL_2 = "SELECT * FROM (SELECT * FROM " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles +
                               " WHERE " + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID + "='" + Nomina_Anterior + "'" +
                               " ORDER BY " + Cat_Nom_Nominas_Detalles.Campo_No_Nomina + " DESC)" +
                               " WHERE rownum=1";

                    Dt_Periodos_Regulares = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL_2).Tables[0];

                    if (Dt_Periodos_Regulares is DataTable)
                    {
                        if (Dt_Periodos_Regulares.Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(Dt_Periodos_Regulares.Rows[0][Cat_Nom_Nominas_Detalles.Campo_No_Nomina].ToString()))
                            {
                                No_Nomina_Anio_Anterior = Convert.ToInt32(Dt_Periodos_Regulares.Rows[0][Cat_Nom_Nominas_Detalles.Campo_No_Nomina].ToString());

                                Mi_SQL.Append(" OR ");
                                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Campo_Nomina_ID + "='" + Nomina_Anterior + "' AND " + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Nomina + "=" + No_Nomina_Anio_Anterior);
                            }
                        }
                    }
                }

                Mi_SQL.Append("))");

                Dt_Vacaciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el tiempo extra de los empleados. Error: " + Ex.Message + "]");
            }
            return Dt_Vacaciones;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Faltas_Empleado_Periodo_Nominal
        /// DESCRIPCION : Consulta las faltas que a tenido el empleado en el periodo actual.
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.
        ///              Nomina_ID: Identificador de la nómina.
        ///              No_Nomina: Identificador del periodo nominal a consultar las incidencias de tiempo extra.
        ///              
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 23/Febrero/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static DataTable Consultar_Faltas_Empleado_Periodo_Nominal(String Empleado_ID, String Nomina_ID, Int32 No_Nomina)
        {
            DataTable Dt_Faltas_Empleados = null;//Almacenará el listado de dias festivos
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta de dias festivos. 

            try
            {
                Mi_SQL.Append(" SELECT ");
                Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + ".*");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Campo_Empleado_ID + "='" + Empleado_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Campo_Estatus + "='Autorizado'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append("(" + Ope_Nom_Faltas_Empleado.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Campo_No_Nomina + "=" + No_Nomina + ")");

                Dt_Faltas_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el tiempo extra de los empleados. Error: " + Ex.Message + "]");
            }
            return Dt_Faltas_Empleados;
        }
        ///******************************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Cambiar_Estatus_Vacaciones
        /// 
        /// DESCRIPCION : Consultalos de los detalles de las vacaciones del empleado. Estos detalles tendran la información de los estatus 
        ///               y el estado de las vacaciones.
        /// 
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 11/Febrero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///******************************************************************************************************************************************************
        public static Boolean Cambiar_Estatus_Vacaciones(String No_Dia_Vacacion)
        {
            String Mi_SQL = "";//Variable que almacena la consulta.
            Boolean Operacion_Completa = false;//Guarda el estatus de la configuración.

            try
            {
                Mi_SQL = "UPDATE " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det + " SET " +
                         Ope_Nom_Vacaciones_Dia_Det.Campo_Estado + "='Tomado', " +
                         Ope_Nom_Vacaciones_Dia_Det.Campo_Estatus + "='Pagado'" +
                         " WHERE " + Ope_Nom_Vacaciones_Dia_Det.Campo_No_Dia_Vacacion + "='" + No_Dia_Vacacion + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al cambiar de estado de las vaciones del empleado. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        ///******************************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Dias_Periodo_Nominal
        /// 
        /// DESCRIPCION : Consulta el periodo nominal de generación de nomina actual.
        /// 
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 11/Marzo/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*****************************************************************************************************************************************
        public static Int32 Consultar_Dias_Periodo_Nominal(String Empleado_ID, String Nomina_ID, Int32 Periodo_Nominal)
        {
            Cls_Cat_Empleados_Negocios INF_EMPLEADOS = new Cls_Cat_Empleados_Negocios();
            String Mi_SQL = "";//Variable de conexión con la capa de negocios.
            DataTable Dt_Periodo_Nominal = null;
            DateTime? Fecha_Inicio = null;
            DateTime? Fecha_Final = null;
            Int32 Dias_Periodo_Nominal = 0;
            Int32 Dias_Temp = 0;

            try
            {
                INF_EMPLEADOS = Consultar_Informacion_Empleado(Empleado_ID);

                Mi_SQL = "SELECT " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + ".* ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID + "='" + Nomina_ID + "' ";
                Mi_SQL = Mi_SQL + " AND " + Cat_Nom_Nominas_Detalles.Campo_No_Nomina + "=" + Periodo_Nominal;

                Dt_Periodo_Nominal = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                if (Dt_Periodo_Nominal is DataTable)
                {
                    if (Dt_Periodo_Nominal.Rows.Count > 0)
                    {
                        foreach (DataRow Periodo in Dt_Periodo_Nominal.Rows)
                        {
                            if (Periodo is DataRow)
                            {
                                if (!string.IsNullOrEmpty(Periodo[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString()))
                                    Fecha_Inicio = Convert.ToDateTime(Periodo[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString());
                                if (!string.IsNullOrEmpty(Periodo[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString()))
                                    Fecha_Final = Convert.ToDateTime(Periodo[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString());

                                Dias_Periodo_Nominal = (Int32)Cls_DateAndTime.DateDiff(DateInterval.Day, ((DateTime)Fecha_Inicio), ((DateTime)Fecha_Final)) + 1;

                                if (INF_EMPLEADOS.P_Fecha_Inicio > ((DateTime)Fecha_Inicio)) {
                                    Dias_Temp = (Int32)Cls_DateAndTime.DateDiff(DateInterval.Day, ((DateTime)Fecha_Inicio), INF_EMPLEADOS.P_Fecha_Inicio);
                                    Dias_Periodo_Nominal = Dias_Periodo_Nominal - Dias_Temp;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los dias que tiene el periodo nominal de generación de nómina. Error: [" + Ex.Message + "]");
            }
            return Dias_Periodo_Nominal;
        }
        ///******************************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Incapacidades
        /// 
        /// DESCRIPCION : Consulta las incapacidades que a tenido el empleado en la catorcena actual.
        /// 
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 11/Marzo/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*****************************************************************************************************************************************
        public static DataTable Consultar_Incapacidades(String Nomina_ID, Int32 No_Nomina, String Empleado_ID)
        {
            DataTable Dt_Incapacidades = null;//Almacenará el listado de dias festivos
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta de dias festivos. 

            try
            {
                Mi_SQL.Append(" SELECT ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + ".*");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Empleado_ID + "='" + Empleado_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Estatus + "='Autorizado'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append("(" + Ope_Nom_Incapacidades.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_No_Nomina + "=" + No_Nomina + ")");

                Dt_Incapacidades = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las incapacidades autorizadas para la nómina que actualmente se genera. Error: " + Ex.Message + "]");
            }
            return Dt_Incapacidades;
        }
        #endregion

        #region (Operación)
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Descomprometer_Vacaciones_Empleados
        /// 
        /// DESCRIPCION : Cambia el estado del registro de vacacion del empleado a un  estado de tomado.
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.
        ///              Nomina_ID: Identificador de la nómina.
        ///              No_Nomina: Identificador del periodo nominal a consultar las incidencias de tiempo extra.
        ///              
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : Enero/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static void Descomprometer_Vacaciones_Empleados(String Empleado_ID, String Nomina_ID, Int32 No_Nomina)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.

            try
            {
                Mi_SQL.Append("update ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado);
                Mi_SQL.Append(" set ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Campo_Estado + "='TOMADO'");
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID + "='" + Empleado_ID + "'");
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Nomina + "=" + No_Nomina);

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las vacaciones que le aplican al empleado para el periodo a generar su nómina. Error: [" + Ex.Message + "]");
            }
        }
        #endregion

        #region (Consultas Generales)
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Total_Percepciones_Sindicato
        /// DESCRIPCION : Consulta las percepciones que tienen asignadas el sindicato al que pertenece el empleado.
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.
        ///              Tipo: Tipo [Percepcion o Deduccion]             
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 21/Diciembre/2010 10:27 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static DataTable Consultar_Percepciones_Sindicato_Empleado(String Empleado_ID, String Tipo)
        {
            String Mi_Oracle = "";//Variable que almacenara la consulta.
            try
            {
                Mi_Oracle = " SELECT " + Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad + ", " +
                    Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".*" +
                    " FROM " + Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Sin_Per_Ded_Det + ", " +
                    Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion +
                    " WHERE " + Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Sindicato_ID + " IN " +
                    " (SELECT " + Cat_Empleados.Campo_Sindicato_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                    " WHERE " + Cat_Empleados.Campo_Empleado_ID + " = '" + Empleado_ID + "') AND " +
                    Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Sin_Per_Ded_Det + "." +
                    Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Sindicato_ID + " IN " +
                    " (SELECT " + Cat_Empleados.Campo_Sindicato_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                    " WHERE " + Cat_Empleados.Campo_Empleado_ID + " = '" + Empleado_ID + "') " +
                    " AND " +
                    Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Sin_Per_Ded_Det + "." +
                    Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID + " IN " +
                    " (SELECT " + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + " FROM " +
                    Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " WHERE " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Tipo + "='" + Tipo + "') AND " +
                    Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." +
                    Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "=" +
                    Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Sin_Per_Ded_Det + "." +
                    Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID;

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
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

        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Ajustes_ISR_Empleado
        /// DESCRIPCION : Consulta si el Empleado tiene algún Ajuste de ISR.
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.          
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 13/Enero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static DataTable Consultar_Ajustes_ISR_Empleado_Finiquito(String Empleado_ID, DateTime Fecha_Generar_Nomina)
        {
            String Mi_SQL = "";//Variable que almacenará la consulta. 
            DataTable Dt_Ajustes_ISR_Empelado = null;//Variable que almacenrá los registros de Ajustes de ISR.

            try
            {
                Mi_SQL = " SELECT " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR + ".* " +
                " FROM " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR +
                " WHERE " +
                Ope_Nom_Ajuste_ISR.Campo_Empleado_ID + "='" + Empleado_ID + "'" +
                " AND " +
                Ope_Nom_Ajuste_ISR.Campo_Estatus_Ajuste_ISR + "='Proceso'" +
                " AND " +
                " ('" + string.Format("{0:dd/MM/yyyy}", Fecha_Generar_Nomina) + "' BETWEEN " + Ope_Nom_Ajuste_ISR.Campo_Fecha_Inicio_Pago +
                " AND " + Ope_Nom_Ajuste_ISR.Campo_Fecha_Termino_Pago + ") Or ('" + 
                string.Format("{0:dd/MM/yyyy}", Fecha_Generar_Nomina) + "' <= " + Ope_Nom_Ajuste_ISR.Campo_Fecha_Termino_Pago + ")";


                Dt_Ajustes_ISR_Empelado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado en la clase de datos [Cls_Ope_Nom_Percepciones_Datos] al consultar los ajustes de ISR. Error: [" + Ex.Message + "]");
            }
            return Dt_Ajustes_ISR_Empelado;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Ajustes_ISR_Empleado
        /// DESCRIPCION : Consulta si el Empleado tiene algún Ajuste de ISR.
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           su salario diario.          
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 13/Enero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static DataTable Consultar_Ajustes_ISR_Empleado(String Empleado_ID, DateTime Fecha_Generar_Nomina)
        {
            String Mi_SQL = "";//Variable que almacenará la consulta. 
            DataTable Dt_Ajustes_ISR_Empelado = null;//Variable que almacenrá los registros de Ajustes de ISR.

            try
            {
                Mi_SQL = " SELECT " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR + ".* " +
                " FROM " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR +
                " WHERE " +
                Ope_Nom_Ajuste_ISR.Campo_Empleado_ID + "='" + Empleado_ID + "'" +
                " AND " +
                Ope_Nom_Ajuste_ISR.Campo_Estatus_Ajuste_ISR + "='Proceso'" +
                " AND " +
                " ('" + string.Format("{0:dd/MM/yyyy}", Fecha_Generar_Nomina) + "' >= " + Ope_Nom_Ajuste_ISR.Campo_Fecha_Inicio_Pago +
                " AND " +
                " '" + string.Format("{0:dd/MM/yyyy}", Fecha_Generar_Nomina) + "' <= " + Ope_Nom_Ajuste_ISR.Campo_Fecha_Termino_Pago + ")";

                Dt_Ajustes_ISR_Empelado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado en la clase de datos [Cls_Ope_Nom_Percepciones_Datos] al consultar los ajustes de ISR. Error: [" + Ex.Message + "]");
            }
            return Dt_Ajustes_ISR_Empelado;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Pago_Ajustes_ISR_Empleado
        /// DESCRIPCION : Actualiza los pagos en el Ajuste de ISR.
        /// 
        /// PARAMETROS:  Datos: Almacena los datos que serán actualizados en el ajuste de ISR.        
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 13/Enero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static Boolean Pago_Ajustes_ISR_Empleado(Cls_Ope_Nom_Ajuste_ISR_Negocio Datos)
        {
            String Mi_SQL;    //Obtiene la cadena de inserción hacía la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            Boolean Operacion_Completa = false;                                                 //Sirve para validar si la operación completo.

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_SQL = "UPDATE " + Ope_Nom_Ajuste_ISR.Tabla_Ope_Nom_Ajuste_ISR +
                          " SET " +
                          Ope_Nom_Ajuste_ISR.Campo_Estatus_Ajuste_ISR + "='" + Datos.P_Estatus_Ajuste_ISR + "', " +
                          Ope_Nom_Ajuste_ISR.Campo_No_Pago + "=" + Datos.P_No_Pago + ", " +
                          Ope_Nom_Ajuste_ISR.Campo_Total_ISR_Ajustado + "=" + Datos.P_Total_ISR_Ajustado +
                          " WHERE " +
                          Ope_Nom_Ajuste_ISR.Campo_No_Ajuste_ISR + "='" + Datos.P_No_Ajuste_ISR + "'";

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
                Operacion_Completa = true;
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
            return Operacion_Completa;
        }
        #endregion

        #region (Metodos Generales)
        /// ***********************************************************************************
        /// Nombre: Consultar_Informacion_Empleado
        /// 
        /// Descripción: Consulta la información general del empleado.
        /// 
        /// Parámetros: No_Empleado.- Identificador interno del sistema para las operaciones que
        ///                           se realizan sobre los empelados.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 16/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************
        protected static Cls_Cat_Empleados_Negocios Consultar_Informacion_Empleado(String Empleado_ID)
        {
            Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = new Cls_Cat_Empleados_Negocios();//Variable que almacenara la información del empleado.
            DataTable Dt_Empleado = null;//Variable que almacena el registro búscado del empleado.

            try
            {
                Obj_Empleados.P_Empleado_ID = Empleado_ID;
                Dt_Empleado = Obj_Empleados.Consulta_Empleados_General();//Consultamos la información del empleado.

                if (Dt_Empleado is DataTable)
                {
                    if (Dt_Empleado.Rows.Count > 0)
                    {
                        foreach (DataRow EMPLEADO in Dt_Empleado.Rows)
                        {
                            if (EMPLEADO is DataRow)
                            {
                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString().Trim()))
                                    INF_EMPLEADO.P_Tipo_Nomina_ID = EMPLEADO[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Zona_ID].ToString().Trim()))
                                    INF_EMPLEADO.P_Zona_ID = EMPLEADO[Cat_Empleados.Campo_Zona_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim()))
                                    INF_EMPLEADO.P_Empleado_ID = EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString().Trim()))
                                    INF_EMPLEADO.P_Salario_Diario = Convert.ToDouble(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString().Trim());

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Terceros_ID].ToString().Trim()))
                                    INF_EMPLEADO.P_Terceros_ID = EMPLEADO[Cat_Empleados.Campo_Terceros_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Fecha_Inicio].ToString().Trim()))
                                    INF_EMPLEADO.P_Fecha_Inicio = Convert.ToDateTime(EMPLEADO[Cat_Empleados.Campo_Fecha_Inicio].ToString().Trim());
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las información del empleado. Error: [" + Ex.Message + "]");
            }
            return INF_EMPLEADO;
        }
        #endregion
    }
}