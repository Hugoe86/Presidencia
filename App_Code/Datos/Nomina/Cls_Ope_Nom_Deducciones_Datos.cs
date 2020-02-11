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
using Presidencia.Calculo_Percepciones.Datos;
using Presidencia.DateDiff;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Prestamos.Negocio;
using System.Text;

namespace Presidencia.Calculo_Deducciones.Datos
{
    public class Cls_Ope_Nom_Deducciones_Datos
    {
        #region (Consulta Incidencias Empleados)
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Deducciones_Var_Empleado_Periodo_Nominal
        /// DESCRIPCION : Consulta las Deducciones Variables que a trabajado el empleado en el periodo seleccionado. 
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
        public static DataTable Consultar_Deducciones_Var_Empleado_Periodo_Nominal(String Empleado_ID, String Nomina_ID, Int32 No_Nomina, String Deduccion_Variable_ID)
        {
            DataTable Dt_Deducciones_Variables = null;//Almacenará el listado de horas extra.
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta de horas extra. 

            try
            {
                Mi_SQL.Append(" SELECT ");
                Mi_SQL.Append("SUM(" + Ope_Nom_Deduc_Var_Emp_Det.Tabla_Ope_Nom_Deduc_Var_Emp_Det + "." + Ope_Nom_Deduc_Var_Emp_Det.Campo_Cantidad + ") AS CANTIDAD ");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Deduc_Var_Emp_Det.Tabla_Ope_Nom_Deduc_Var_Emp_Det + " INNER JOIN " + Ope_Nom_Deducciones_Var.Tabla_Ope_Nom_Deducciones_Var);
                Mi_SQL.Append(" ON ");
                Mi_SQL.Append(Ope_Nom_Deduc_Var_Emp_Det.Tabla_Ope_Nom_Deduc_Var_Emp_Det + "." + Ope_Nom_Deduc_Var_Emp_Det.Campo_No_Deduccion);
                Mi_SQL.Append(" = ");
                Mi_SQL.Append(Ope_Nom_Deducciones_Var.Tabla_Ope_Nom_Deducciones_Var + "." + Ope_Nom_Deducciones_Var.Campo_No_Deduccion);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Nom_Deducciones_Var.Tabla_Ope_Nom_Deducciones_Var + "." + Ope_Nom_Deducciones_Var.Campo_Estatus + "='Aceptado'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Deduc_Var_Emp_Det.Tabla_Ope_Nom_Deduc_Var_Emp_Det + "." + Ope_Nom_Deduc_Var_Emp_Det.Campo_Estatus + "='Autorizado'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Deduc_Var_Emp_Det.Tabla_Ope_Nom_Deduc_Var_Emp_Det + "." + Ope_Nom_Deduc_Var_Emp_Det.Campo_Empleado_ID + "='" + Empleado_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Deducciones_Var.Tabla_Ope_Nom_Deducciones_Var + "." + Ope_Nom_Deducciones_Var.Campo_Percepcion_Deduccion_ID + "='" + Deduccion_Variable_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append("(" + Ope_Nom_Deducciones_Var.Tabla_Ope_Nom_Deducciones_Var + "." + Ope_Nom_Deducciones_Var.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Deducciones_Var.Tabla_Ope_Nom_Deducciones_Var + "." + Ope_Nom_Deducciones_Var.Campo_No_Nomina + "=" + No_Nomina + ")");

                Dt_Deducciones_Variables = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las deducciones variables de los empleados. Error: " + Ex.Message + "]");
            }
            return Dt_Deducciones_Variables;
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
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Estatus + "='Autorizado'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID + "='" + Empleado_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append("(" + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + Ope_Nom_Tiempo_Extra.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + Ope_Nom_Tiempo_Extra.Campo_No_Nomina + "=" + No_Nomina + ")");

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
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_Estatus + "='ACEPTADO'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + "." + Ope_Nom_Domingos_Empleado_Detalles.Campo_Estatus + "='ACEPTADO'");
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
        public static DataTable Consultar_Vacaciones_Empleado_Periodo_Nominal(String Empleado_ID, String Nomina_ID, Int32 No_Nomina)
        {
            DataTable Dt_Vacaciones = null;//Almacenará el listado de dias festivos
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta de dias festivos. 

            try
            {
                Mi_SQL.Append(" SELECT ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + ".*");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Campo_Estatus + "='Autorizado'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID + "='" + Empleado_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append("(" + OPE_NOM_VACACIONES_EMPLEADO.Campo_Nomina_ID + "='" + Nomina_ID + "' AND " + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Nomina + "=" + No_Nomina + ")");

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
        public static Boolean Cambiar_Estatus_Vacaciones(String No_Vacacion)
        {
            String Mi_SQL = "";//Variable que almacena la consulta.
            Boolean Operacion_Completa = false;//Guarda el estatus de la configuración.

            try
            {
                Mi_SQL = "UPDATE " + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + " SET " +
                          OPE_NOM_VACACIONES_EMPLEADO.Campo_Estatus + "='Cancelado'" +
                          " WHERE " + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion + "='" + No_Vacacion + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                Mi_SQL = "UPDATE " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det + " SET " +
                         Ope_Nom_Vacaciones_Dia_Det.Campo_Estado + "='Tomado', " +
                         Ope_Nom_Vacaciones_Dia_Det.Campo_Estatus + "='Pagado'" +
                         " WHERE " + Ope_Nom_Vacaciones_Dia_Det.Campo_No_Vacacion + "='" + No_Vacacion + "'";

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
        ///******************************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Deducciones_Var_Empleado_Periodo_Nominal_Proveedores
        /// 
        /// DESCRIPCION : Consulta los prestamos deproveedores externos a presidencia.
        /// 
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 28/Abril/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*****************************************************************************************************************************************
        public static DataTable Consultar_Deducciones_Var_Empleado_Periodo_Nominal_Proveedores(String Empleado_ID, 
            String Nomina_ID, Int32 No_Nomina, String Deduccion_Variable_ID)
        {
            String Mi_SQL = String.Empty;//Variable que almacenara la consulta a la
            DataTable Dt_Detalles_Proveedor = null;

            try
            {
                Mi_SQL = "SELECT " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + ".*";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID + "='" + Deduccion_Variable_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Nom_Proveedores_Detalles.Campo_Empleado_ID + "='" + Empleado_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Nom_Proveedores_Detalles.Campo_Nomina_ID + "='" + Nomina_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Nom_Proveedores_Detalles.Campo_Periodo + "=" + No_Nomina;
                Mi_SQL = Mi_SQL + " AND " + Ope_Nom_Proveedores_Detalles.Campo_Estatus + "='ACEPTADO'";

                Dt_Detalles_Proveedor = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception)
            {
                throw new Exception("Error al consultar ");
            }
            return Dt_Detalles_Proveedor;
        }
        #endregion

        #region (Metodos Consulta)
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Tabulador_ISR
        /// DESCRIPCION : 
        /// 
        /// PARAMETROS:  Total_Gravable_Prima_Vacacional_Aguinaldo.- Se suma el Gravado de la Prima Vacacional y el Aguinaldo.       
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 28/Diciembre/2010 12:30 pm.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static DataTable Consultar_Tabulador_ISR(Double Cantidad_Gravable, Double DIAS_PERIODO)
        {
            StringBuilder Query = new StringBuilder();
            DataTable Dt_Tabulador_ISR = null;//Variable que almacenara el registro de retencion a terceros consultado.

            try
            {
                Query.Append("SELECT ");
                Query.Append(Tab_Nom_ISR.Campo_ISR_ID + ", ");
                Query.Append("(" + Tab_Nom_ISR.Campo_Limite_Inferior + " * " + DIAS_PERIODO + ") AS " + Tab_Nom_ISR.Campo_Limite_Inferior + ", ");
                Query.Append("(" + Tab_Nom_ISR.Campo_Cuota_Fija + " * " + DIAS_PERIODO + ") AS " + Tab_Nom_ISR.Campo_Cuota_Fija + ", ");
                Query.Append("(" + Tab_Nom_ISR.Campo_Porcentaje + " / 100) AS " + Tab_Nom_ISR.Campo_Porcentaje + ", ");
                Query.Append(Tab_Nom_ISR.Campo_Tipo_Nomina + ", ");
                Query.Append(Tab_Nom_ISR.Campo_Comentarios + ", ");
                Query.Append(Tab_Nom_ISR.Campo_Usuario_Creo + ", ");
                Query.Append(Tab_Nom_ISR.Campo_Fecha_Creo + ", ");
                Query.Append(Tab_Nom_ISR.Campo_Usuario_Modifico + ", ");
                Query.Append(Tab_Nom_ISR.Campo_Fecha_Modifico);
                Query.Append(" FROM ");
                Query.Append("(SELECT ");
                Query.Append(Tab_Nom_ISR.Tabla_Tab_Nom_ISR + ".*");
                Query.Append(" FROM ");
                Query.Append(Tab_Nom_ISR.Tabla_Tab_Nom_ISR);
                Query.Append(" WHERE ");
                Query.Append(Tab_Nom_ISR.Campo_Tipo_Nomina + "='CATORCENAL'");
                Query.Append(" AND ");
                Query.Append("(" + Tab_Nom_ISR.Campo_Limite_Inferior + " * " + DIAS_PERIODO +  ") <= " + Cantidad_Gravable);
                Query.Append(" ORDER BY ");
                Query.Append(Tab_Nom_ISR.Campo_Limite_Inferior);
                Query.Append(" DESC)");
                Query.Append(" WHERE rownum=1");

                Dt_Tabulador_ISR = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Query.ToString()).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron consultados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            return Dt_Tabulador_ISR;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Tabulador_Subsidio_Empleado
        /// DESCRIPCION : 
        /// 
        /// PARAMETROS:  Cantidad_Gravable.- Se suma el Gravado de la Prima Vacacional y el Aguinaldo.       
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 28/Diciembre/2010 12:30 pm.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static DataTable Consultar_Tabulador_Subsidio_Empleado(Double Cantidad_Gravable, Double DIAS_PERIODO)
        {
            StringBuilder Query = new StringBuilder();
            DataTable Dt_Tabulador_Subsidio = null;//Variable que almacenara el registro de retencion a terceros consultado.

            try
            {
                Query.Append("SELECT ");
                Query.Append(Tab_Nom_Subsidio.Campo_Subsidio_ID + ", ");
                Query.Append("(" + Tab_Nom_Subsidio.Campo_Limite_Inferior + " * " + DIAS_PERIODO + ") AS " + Tab_Nom_Subsidio.Campo_Limite_Inferior + ", ");
                Query.Append("(" + Tab_Nom_Subsidio.Campo_Subsidio + " * " + DIAS_PERIODO + ") AS " + Tab_Nom_Subsidio.Campo_Subsidio + ", ");
                Query.Append(Tab_Nom_Subsidio.Campo_Tipo_Nomina + ", ");
                Query.Append(Tab_Nom_Subsidio.Campo_Comentarios + ", ");
                Query.Append(Tab_Nom_Subsidio.Campo_Usuario_Creo + ", ");
                Query.Append(Tab_Nom_Subsidio.Campo_Fecha_Creo + ", ");
                Query.Append(Tab_Nom_Subsidio.Campo_Usuario_Modifico + ", ");
                Query.Append(Tab_Nom_Subsidio.Campo_Fecha_Modifico);
                Query.Append(" FROM ");
                Query.Append("(SELECT ");
                Query.Append(Tab_Nom_Subsidio.Tabla_Tab_Nom_Subsidio + ".*");
                Query.Append(" FROM ");
                Query.Append(Tab_Nom_Subsidio.Tabla_Tab_Nom_Subsidio);
                Query.Append(" WHERE ");
                Query.Append(Tab_Nom_Subsidio.Campo_Tipo_Nomina + "='CATORCENAL'");
                Query.Append(" AND ");
                Query.Append("(" + Tab_Nom_Subsidio.Campo_Limite_Inferior + " * " + DIAS_PERIODO + ") <= " + Cantidad_Gravable);
                Query.Append(" ORDER BY ");
                Query.Append(Tab_Nom_Subsidio.Campo_Limite_Inferior);
                Query.Append(" DESC)");
                Query.Append(" WHERE rownum=1");

                Dt_Tabulador_Subsidio = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Query.ToString()).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron consultados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            return Dt_Tabulador_Subsidio;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Parametro_Retencion_Terceros
        /// DESCRIPCION : Consulta el parametro de retencion a terceros.
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           el parámetro de retencion.         
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 28/Diciembre/2010 12:30 pm.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static DataTable Consultar_Parametro_Retencion_Terceros(String Empleado_ID) {
            String Mi_SQL = "";//Variable que almacenará la consulta de parametro de retencion.
            DataTable Dt_Terceros = null;//Variable que almacenara el registro de retencion a terceros consultado.

            try
            {
                Mi_SQL = "SELECT " + Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros + ".* " +
                         " FROM " +
                         Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros +
                         " WHERE " +
                         Cat_Nom_Terceros.Campo_Tercero_ID +
                         " IN " +
                         "(SELECT " + Cat_Empleados.Campo_Terceros_ID +
                         " FROM " +
                         Cat_Empleados.Tabla_Cat_Empleados +
                         " WHERE " +
                         Cat_Empleados.Campo_Empleado_ID + "='" + Empleado_ID + "')";

                Dt_Terceros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];                        
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron consultados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            return Dt_Terceros;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Parametro_Retencion_Fondo_Retiro
        /// DESCRIPCION : Consulta el parametro de retencion de fondo de retiro.
        /// 
        /// PARAMETROS:  Empleado_ID: El identificar del empleado del cual se desea conocer
        ///                           el parámetro de retencion de fondo de retiro.         
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 28/Diciembre/2010 12:30 pm.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static DataTable Consultar_Parametro_Retencion_Fondo_Retiro(String Empleado_ID)
        {
            String Mi_SQL = "";//Variable que almacenará la consulta de parametro de retencion.
            DataTable Dt_Parametros_Nomina = null;//Variable que almacenara el registro de retencion a terceros consultado.

            try
            {
                Mi_SQL = "SELECT " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + ".* " +
                         " FROM " +
                         Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros +
                         " WHERE " +
                         Cat_Nom_Parametros.Campo_Zona_ID +
                         " IN " +
                         "(SELECT " + Cat_Empleados.Campo_Zona_ID +
                         " FROM " +
                         Cat_Empleados.Tabla_Cat_Empleados +
                         " WHERE " +
                         Cat_Empleados.Campo_Empleado_ID + "='" + Empleado_ID + "')";

                Dt_Parametros_Nomina = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron consultados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            return Dt_Parametros_Nomina;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Prestamos_Empleado
        /// DESCRIPCION : Consulta los prestamos autorizados que el empleado tiene actualmente en el sistema.
        /// 
        /// PARAMETROS:  Empleado_ID: Identificador del Empleado. Por el cuál se consultaran los prestamos que tiene dados de alta 
        ///                           en el sistema.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 17/Enero/2010 12:53 pm.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static DataTable Consultar_Prestamos_Empleado_Finiquito(String Empleado_ID, DateTime Fecha_Generar_Nomina)
        {
            String Mi_SQL = "";//Variable que almacenara la consulta de prestamos
            DataTable Dt_Prestamos_Autorizados_Activos_Empleado = null;//Lista de prestamos que debe actualmente el empleado.

            try
            {
                Mi_SQL = "SELECT " +
                         Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + ".* " +
                         " FROM " +
                          Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo +
                         " WHERE " +
                         Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID + " ='" + Empleado_ID + "'" +
                         " AND " +
                         Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Solicitud + " = 'Autorizado'" +
                         " AND " +
                         Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo + "='PROCESO'" +
                         " AND " +
                         " ('" + string.Format("{0:dd/MM/yyyy}", Fecha_Generar_Nomina) + "' BETWEEN FECHA_INICIO_PAGO AND FECHA_TERMINO_PAGO) Or ('" + string.Format("{0:dd/MM/yyyy}", Fecha_Generar_Nomina) + "' <= FECHA_TERMINO_PAGO)";

                Dt_Prestamos_Autorizados_Activos_Empleado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado al consultar los prestamos que tienen un empleado. Error: [" + Ex.Message + "]");
            }
            return Dt_Prestamos_Autorizados_Activos_Empleado;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Prestamos_Empleado
        /// DESCRIPCION : Consulta los prestamos autorizados que el empleado tiene actualmente en el sistema.
        /// 
        /// PARAMETROS:  Empleado_ID: Identificador del Empleado. Por el cuál se consultaran los prestamos que tiene dados de alta 
        ///                           en el sistema.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 17/Enero/2010 12:53 pm.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static DataTable Consultar_Prestamos_Empleado(String Empleado_ID, DateTime Fecha_Generar_Nomina)
        {
            String Mi_SQL = "";//Variable que almacenara la consulta de prestamos
            DataTable Dt_Prestamos_Autorizados_Activos_Empleado = null;//Lista de prestamos que debe actualmente el empleado.

            try
            {
                Mi_SQL = "SELECT " +
                         Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo + ".* " +
                         " FROM " +
                          Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo +
                         " WHERE " +
                         Ope_Nom_Solicitud_Prestamo.Campo_Solicita_Empleado_ID + " ='" + Empleado_ID + "'" +
                         " AND " +
                         Ope_Nom_Solicitud_Prestamo.Campo_Estatus_Solicitud + " = 'Autorizado'" +
                         " AND " +
                         Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo + "='PROCESO'" +
                         " AND " +
                         " ('" + string.Format("{0:dd/MM/yyyy}", Fecha_Generar_Nomina) + "' BETWEEN FECHA_INICIO_PAGO AND FECHA_TERMINO_PAGO)";

                Dt_Prestamos_Autorizados_Activos_Empleado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado al consultar los prestamos que tienen un empleado. Error: [" + Ex.Message + "]");
            }
            return Dt_Prestamos_Autorizados_Activos_Empleado;
        }
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Cambiar_Estado_Prestamo
        /// DESCRIPCION : Cambia a Pagado el Estatus del Prestamo.
        /// 
        /// PARAMETROS:  Empleado_ID: Identificador del Empleado. Por el cuál se consultaran los prestamos que tiene dados de alta 
        ///                           en el sistema.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 17/Enero/2010 12:53 pm.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static Boolean Cambiar_Estado_Prestamo(String No_Solicitud, DateTime Fecha_Generar_Nomina)
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
                Mi_SQL = "UPDATE " + Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo +
                         " SET " +
                         Ope_Nom_Solicitud_Prestamo.Campo_Estado_Prestamo + "='PAGADO', " +
                         Ope_Nom_Solicitud_Prestamo.Campo_Fecha_Finiquito_Prestamo + "='" + string.Format("{0:dd/MM/yyyy}", Fecha_Generar_Nomina) + "'" +
                         " WHERE " +
                         Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud + "='" + No_Solicitud + "'";

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
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Capturar_Pago_Abono_Prestamo_Catorcenal
        /// DESCRIPCION : 
        /// 
        /// PARAMETROS:  
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 17/Enero/2010 12:53 pm.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static Boolean Capturar_Pago_Abono_Prestamo_Catorcenal(Cls_Ope_Nom_Pestamos_Negocio Datos)
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
                Mi_SQL = "UPDATE " + Ope_Nom_Solicitud_Prestamo.Tabla_Ope_Nom_Solicitud_Prestamo +
                         " SET " +
                         Ope_Nom_Solicitud_Prestamo.Campo_Monto_Abonado + "=" + Datos.P_Monto_Abonado + ", " +
                         Ope_Nom_Solicitud_Prestamo.Campo_Saldo_Actual + "=" + Datos.P_Saldo_Actual + ", " +
                         Ope_Nom_Solicitud_Prestamo.Campo_No_Abono + "=" + Datos.P_No_Abono + 
                         " WHERE " +
                         Ope_Nom_Solicitud_Prestamo.Campo_No_Solicitud + "='" + Datos.P_No_Solicitud + "'";

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
        ///************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Salario_Diario_Zona
        /// DESCRIPCION : Consulta el salario diario de la zona. 
        /// 
        /// PARAMETROS:  Empleado_ID.- Identificador único del empleado.
        ///                      
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 12/Febrero/2010 9:33 am.
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***********************************************************************************************************************
        public static Double Consultar_Salario_Diario_Zona(String Empleado_ID)
        {
            StringBuilder Mi_SQL = new StringBuilder();         //Variable que almacenara la consulta.
            OracleConnection Conexion = new OracleConnection(); //Variable que almacena la conexión.
            OracleCommand Comando = new OracleCommand();        //comando que se usara para realizar la consulta.
            OracleTransaction Transaccion;                      //Variable que manejara las transacciones de la consulta.
            OracleDataReader Lector = null;                     //Lector de los registros consultados.
            String Zona_ID = "";                                //Variable que almacena el identificador de la zona economica.
            Double Salario_Diario_Zona = 0.0;                   //Variable que almacena el salario diario de la zona.

            Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;//Obtenemos la cadena d e conexión.
            Conexion.Open();                                    //Abrimos la conexión.
            Transaccion = Conexion.BeginTransaction();          //Obtenemos la instancia para el manejo de transacciones. 
            Comando.Connection = Conexion;                      //Asignamos la coneccion con la que trabajara, el comando.
            Comando.Transaction = Transaccion;                  //Asignamos la transaccion que manejara algun posible error cuando el comando ejecute la consulta.

            try
            {
                //Generamos la consulta.
                Mi_SQL.Append(" SELECT " + Cat_Nom_Zona_Economica.Campo_Salario_Diario);
                Mi_SQL.Append(" FROM " + Cat_Nom_Zona_Economica.Tabla_Cat_Nom_Zona_Economica);
                Mi_SQL.Append(" WHERE " + Cat_Nom_Zona_Economica.Campo_Zona_ID);
                Mi_SQL.Append(" IN ");
                Mi_SQL.Append(" (SELECT " + Cat_Empleados.Campo_Zona_ID);
                Mi_SQL.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" WHERE " + Cat_Empleados.Campo_Empleado_ID + "='" + Empleado_ID + "')");

                Comando.CommandText = Mi_SQL.ToString();//Establecemos la consulta.
                Lector = Comando.ExecuteReader();//Ejecutamos la consulta.
                Transaccion.Commit();//Ejecutamos el comando.

                if (Lector is OracleDataReader)
                {
                    if (Lector.Read())
                    {
                        if (Lector.GetDouble(0) is Double)
                        {
                            Salario_Diario_Zona = Lector.GetDouble(0);
                        }
                    }
                }
            }
            catch (OracleException Ex)
            {
                if (Transaccion != null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion != null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion != null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Conexion.Close();
            }
            return Salario_Diario_Zona;
        }
        #endregion

        #region (Consulta FONACOT)
       
        #endregion
    }
}
