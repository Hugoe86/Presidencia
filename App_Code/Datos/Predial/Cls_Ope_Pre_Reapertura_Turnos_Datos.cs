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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Reapertura_Turnos.Negocio;

namespace Presidencia.Reapertura_Turnos.Datos
{
    public class Cls_Ope_Pre_Reapertura_Turnos_Datos
    {
        #region (Métodos)

        #region (Métodos Consulta)
        ///************************************************************************************************
        /// NOMBRE: Consultar_Cierres_Turno_Dia
        ///
        /// DESCRIPCIÓN: Consulta los cierres turno por día por los filtros ingresados.
        /// 
        /// PARÁMETROS: Datos.- Objeto con la información a utilizar en la operación a ejecutar.
        /// 
        /// USUARIO CREO: Juan Alberto Hernández Negrete. 
        /// FECHA CREO: 22/Octubre/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///************************************************************************************************
        public static DataTable Consultar_Cierres_Turno_Dia(Cls_Ope_Pre_Reapertura_Turnos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Cierres_Dia = null;//Variable que almacena los cierres de dia.
            String Aux_Mi_SQL = String.Empty;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia + "." + Ope_Caj_Turnos_Dia.Campo_No_Turno + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia + "." + Ope_Caj_Turnos_Dia.Campo_Fecha_Turno + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia + "." + Ope_Caj_Turnos_Dia.Campo_Hora_Apertura + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia + "." + Ope_Caj_Turnos_Dia.Campo_Hora_Cierre + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia + "." + Ope_Caj_Turnos_Dia.Campo_Estatus + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia + "." + Ope_Caj_Turnos_Dia.Campo_Usuario_Reapertura + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia + "." + Ope_Caj_Turnos_Dia.Campo_Fecha_Reapertura + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia + "." + Ope_Caj_Turnos_Dia.Campo_Fecha_Reapertura_Cierre + ", ");
               
                Mi_SQL.Append("(SELECT (" + 
                    Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + 
                    "|| ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + 
                    "|| ' ' ||" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") FROM " + 
                    Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + 
                    Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "=" +
                    Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia + "." + Ope_Caj_Turnos_Dia.Campo_Usuario_Reapertura + ") AS EMPLEADO ");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia);

                Aux_Mi_SQL = Mi_SQL.ToString();
                Mi_SQL = new StringBuilder();

                if (!String.IsNullOrEmpty(Datos.P_No_Turno_Dia))
                {
                    if (Mi_SQL.ToString().Trim().ToUpper().Contains("WHERE"))
                        Mi_SQL.Append(" AND " + Ope_Caj_Turnos_Dia.Campo_No_Turno + "='" + Datos.P_No_Turno_Dia + "'");
                    else
                        Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos_Dia.Campo_No_Turno + "='" + Datos.P_No_Turno_Dia + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (Mi_SQL.ToString().Trim().ToUpper().Contains("WHERE"))
                        Mi_SQL.Append(" AND " + Ope_Caj_Turnos_Dia.Campo_Estatus + "='" + Datos.P_Estatus + "'");
                    else
                        Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos_Dia.Campo_Estatus + "='" + Datos.P_Estatus + "'");
                }

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Inicio_Busqueda_Cierre_Dias) && !string.IsNullOrEmpty(Datos.P_Fecha_Fin_Busqueda_Cierre_Dias))
                {
                    if (Mi_SQL.ToString().Trim().ToUpper().Contains("WHERE"))
                        Mi_SQL.Append(" AND " + Ope_Caj_Turnos_Dia.Campo_Fecha_Turno + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda_Cierre_Dias + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda_Cierre_Dias + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')");
                    else
                        Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos_Dia.Campo_Fecha_Turno + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda_Cierre_Dias + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                        " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda_Cierre_Dias + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')");
                }

                Mi_SQL = new StringBuilder(Aux_Mi_SQL + Mi_SQL.ToString());

                Dt_Cierres_Dia = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los cierres de turnos. Error: [" + Ex.Message + "]");
            }
            return Dt_Cierres_Dia;
        }
        ///************************************************************************************************
        /// NOMBRE: Rpt_Reapertura_Cierre_Turno_Dia
        ///
        /// DESCRIPCIÓN: Consulta las reaperturas de cierres de turno del día por los filtros ingresados.
        /// 
        /// PARÁMETROS: Datos.- Objeto con la información a utilizar en la operación a ejecutar.
        /// 
        /// USUARIO CREO: Juan Alberto Hernández Negrete. 
        /// FECHA CREO: 23/Octubre/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///************************************************************************************************
        public static DataTable Rpt_Reapertura_Cierre_Turno_Dia(Cls_Ope_Pre_Reapertura_Turnos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Cierres_Dia = null;//Variable que almacena los cierres de dia.
            String Aux_Mi_SQL = String.Empty;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia + "." + Ope_Caj_Turnos_Dia.Campo_No_Turno + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia + "." + Ope_Caj_Turnos_Dia.Campo_Fecha_Turno + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia + "." + Ope_Caj_Turnos_Dia.Campo_Estatus + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia + "." + Ope_Caj_Turnos_Dia.Campo_Fecha_Reapertura + ", ");

                Mi_SQL.Append("(SELECT (" +
                    Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno +
                    "|| ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno +
                    "|| ' ' ||" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") FROM " +
                    Cat_Empleados.Tabla_Cat_Empleados + " WHERE " +
                    Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "=" +
                    Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia + "." + Ope_Caj_Turnos_Dia.Campo_Usuario_Reapertura + ") AS EMPLEADO, ");

                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia + "." + Ope_Caj_Turnos_Dia.Campo_Autorizo_Reapertura + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia + "." + Ope_Caj_Turnos_Dia.Campo_Observaciones);
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia);

                Aux_Mi_SQL = Mi_SQL.ToString();
                Mi_SQL = new StringBuilder();

                if (!String.IsNullOrEmpty(Datos.P_No_Turno_Dia))
                {
                    if (Mi_SQL.ToString().Trim().ToUpper().Contains("WHERE"))
                        Mi_SQL.Append(" AND " + Ope_Caj_Turnos_Dia.Campo_No_Turno + "='" + Datos.P_No_Turno_Dia + "'");
                    else
                        Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos_Dia.Campo_No_Turno + "='" + Datos.P_No_Turno_Dia + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (Mi_SQL.ToString().Trim().ToUpper().Contains("WHERE"))
                        Mi_SQL.Append(" AND " + Ope_Caj_Turnos_Dia.Campo_Estatus + "='" + Datos.P_Estatus + "'");
                    else
                        Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos_Dia.Campo_Estatus + "='" + Datos.P_Estatus + "'");
                }

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Inicio_Busqueda_Cierre_Dias) && !string.IsNullOrEmpty(Datos.P_Fecha_Fin_Busqueda_Cierre_Dias))
                {
                    if (Mi_SQL.ToString().Trim().ToUpper().Contains("WHERE"))
                        Mi_SQL.Append(" AND " + Ope_Caj_Turnos_Dia.Campo_Fecha_Turno + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda_Cierre_Dias + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda_Cierre_Dias + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')");
                    else
                        Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos_Dia.Campo_Fecha_Turno + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda_Cierre_Dias + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                        " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda_Cierre_Dias + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')");
                }

                Mi_SQL = new StringBuilder(Aux_Mi_SQL + Mi_SQL.ToString());

                Dt_Cierres_Dia = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los cierres de turnos. Error: [" + Ex.Message + "]");
            }
            return Dt_Cierres_Dia;
        }
        #endregion

        #region (Métodos Operación)
        ///************************************************************************************************
        /// NOMBRE: Actualiza_Estatus_Cierre_Dia
        ///
        /// DESCRIPCIÓN: Método que abre el cierre del día, cambia el estatus del registro del cierre
        ///              del día.
        /// 
        /// PARÁMETROS: Datos.- Objeto con la información a utilizar en la operación a ejecutar.
        /// 
        /// USUARIO CREO: Juan Alberto Hernández Negrete. 
        /// FECHA CREO: 23/Octubre/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        ///************************************************************************************************
        public static Boolean Actualiza_Estatus_Cierre_Dia(Cls_Ope_Pre_Reapertura_Turnos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variabel que almacenara la consulta.
            Boolean Operacion_Completa = false;//Variable que guarda el estatus de la operacion.

            try
            {
                //Actualizamos el estatus del cierre del día.
                Mi_SQL.Append("UPDATE " + Ope_Caj_Turnos_Dia.Tabla_Ope_Caj_Turnos_Dia + " SET ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Campo_Estatus + "='ABIERTO', ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Campo_Usuario_Reapertura + "='" + Datos.P_Empleado_Reabrio + "', ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Campo_Autorizo_Reapertura + "='" + Datos.P_Autorizo_Reapertura + "', ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Campo_Observaciones + "='" + Datos.P_Observaciones + "', ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Campo_Fecha_Reapertura + "=SYSDATE, ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Campo_Usuario_Modifico + "='" + Datos.P_Empleado_Reabrio + "', ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Campo_Fecha_Modifico + "=SYSDATE");
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Caj_Turnos_Dia.Campo_No_Turno + "='" + Datos.P_No_Turno_Dia + "'");

                //Ejecutamos la actualización del cierre del día.
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al actualizar la información del estatus del cierre de día. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        #endregion

        #endregion
    }
}