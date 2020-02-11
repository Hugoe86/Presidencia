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
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Recibos_Empleados.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Archivos_Historial_Nomina_Generada;
using System.Text;

namespace Presidencia.Recibos_Empleados.Datos
{
    public class Cls_Ope_Nom_Recibos_Empleados_Datos
    {
        #region (Métodos Operación)
        ///**************************************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Recibo_Empleado
        /// DESCRIPCION : Ejecuta el alta de un recibo de nómina. Este registro se genera por empleado ademas inserta en la tabla de detalles todos los conceptos
        ///               que aplicaron para el cálculo realizado.
        ///               
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 05/Febrero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///**************************************************************************************************************************************************************
        public static void Alta_Recibo_Empleado(Cls_Ope_Nom_Recibos_Empleados_Negocio Datos)
        {

            String Mi_SQL;           //Obtiene la cadena de inserción hacía la base de datos
            Object No_Recibo;   //Obtiene el ID con la cual se guardo los datos en la base de datos
            Int64 No_Detalle_Recibo = 0;
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Nom_Recibos_Empleados.Campo_No_Recibo + "),'0000000000') ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados;
                No_Recibo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(No_Recibo))
                {
                    Datos.P_No_Recibo= "0000000001";
                }
                else
                {
                    Datos.P_No_Recibo = String.Format("{0:0000000000}", Convert.ToInt32(No_Recibo) + 1);
                }


                Mi_SQL = "INSERT INTO " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + " (" +
                         Ope_Nom_Recibos_Empleados.Campo_No_Recibo + ", " +
                         Ope_Nom_Recibos_Empleados.Campo_Detalle_Nomina_ID + ", " +
                         Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + ", " +
                         Ope_Nom_Recibos_Empleados.Campo_No_Nomina + ", " +
                         Ope_Nom_Recibos_Empleados.Campo_Tipo_Nomina_ID + ", " +
                         Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + ", " +
                         Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID + ", " +
                         Ope_Nom_Recibos_Empleados.Campo_Puesto_ID + ", " +
                         Ope_Nom_Recibos_Empleados.Campo_Dias_Trabajados + ", " +
                         Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones + ", " +
                         Ope_Nom_Recibos_Empleados.Campo_Total_Deducciones + ", " +
                         Ope_Nom_Recibos_Empleados.Campo_Total_Nomina + ", " +
                         Ope_Nom_Recibos_Empleados.Campo_Gravado + ", " +
                         Ope_Nom_Recibos_Empleados.Campo_Exento + ", " +
                         Ope_Nom_Recibos_Empleados.Campo_Salario_Diario + ", " +
                         Ope_Nom_Recibos_Empleados.Campo_Salario_Diario_Integrado + ", " +
                         Ope_Nom_Recibos_Empleados.Campo_Usuario_Creo + ", " +
                         Ope_Nom_Recibos_Empleados.Campo_Fecha_Creo + ", " +
                         Ope_Nom_Recibos_Empleados.Campo_Nomina_Generada +
                         ") VALUES(" +
                         "'" + Datos.P_No_Recibo + "', " +
                         "'" + Datos.P_Detalle_Nomina_ID + "', " +
                         "'" + Datos.P_Nomina_ID + "', " +
                         Datos.P_No_Nomina + ", " +
                         "'" + Datos.P_Tipo_Nomina_ID + "', " +
                         "'" + Datos.P_Empleado_ID + "', " +
                         "'" + Datos.P_Dependencia_ID + "', " +
                         "'" + Datos.P_Puesto_ID + "', " +
                         Datos.P_Dias_Trabajados + ", " +
                         Datos.P_Total_Percepciones + ", " +
                         Datos.P_Total_Deducciones + ", " +
                         Datos.P_Total_Nomina + ", " +
                         Datos.P_Gravado + ", " +
                         Datos.P_Exento + ", " +
                         Datos.P_Salario_Diario + ", " +
                         Datos.P_Salario_Diario_Integrado + ", " +
                         "'" + Datos.P_Usuario_Creo + "', SYSDATE, '" + Datos.P_Nomina_Generada + "')";

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos


                if (Datos.P_Dt_Recibo_Empleado_Detalles != null)
                {
                    No_Detalle_Recibo = Obtener_Consecutivo(Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo_Detalles,
                                        Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det);

                    foreach (DataRow Renglon in Datos.P_Dt_Recibo_Empleado_Detalles.Rows)
                    {
                        if (Renglon is DataRow)
                        {
                            Mi_SQL = "INSERT INTO " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + " (" +
                                     Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo_Detalles + ", " +
                                     Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo + ", " +
                                     Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + ", " +
                                     Ope_Nom_Recibos_Empleados_Det.Campo_Monto + ", " +
                                     Ope_Nom_Recibos_Empleados_Det.Campo_Gravado + ", " +
                                     Ope_Nom_Recibos_Empleados_Det.Campo_Exento + ") VALUES(" +
                                     No_Detalle_Recibo + ", " +
                                     "'" + Datos.P_No_Recibo + "', " +
                                     "'" + Renglon["Percepcion_Deduccion"].ToString() + "', " +
                                     ((string.IsNullOrEmpty(Renglon["Monto"].ToString())) ? "0" : string.Format("{0:0.00}", Convert.ToDouble( Renglon["Monto"].ToString()))) + ", " +
                                     ((string.IsNullOrEmpty(Renglon["Grava"].ToString())) ? "0" : string.Format("{0:0.00}", Convert.ToDouble(Renglon["Grava"].ToString()))) + ", " +
                                     ((string.IsNullOrEmpty(Renglon["Exenta"].ToString())) ? "0" :string.Format("{0:0.00}", Convert.ToDouble( Renglon["Exenta"].ToString()))) + ")";

                            Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                            Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                            No_Detalle_Recibo += 1;
                        }
                    }
                }

                Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
                Cls_Sessiones.Historial_Nomina_Generada.Append("[RECIBOS, " + Datos.P_No_Recibo + "]\r\n");
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
        }


        public void Baja_Recibo_Empleado()
        {

        }
        #endregion

        #region (Métodos Consulta)
        ///**************************************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Recibos_Empleados
        /// DESCRIPCION : Consulta los Recibos de la Nómina de los Empleados.
        ///               
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 08/Febrero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///**************************************************************************************************************************************************************
        public static DataTable Consultar_Recibos_Empleados(Cls_Ope_Nom_Recibos_Empleados_Negocio Datos)
        {
            String Mi_SQL = "";//Variable que almacenará la consulta.
            DataTable Dt_Recibos_Empleado = null;//Variable que almacena el recibo consultado del empleado.

            try
            {
                Mi_SQL = "SELECT " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + ".* " +
                         " FROM " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados;

                if (!string.IsNullOrEmpty(Datos.P_No_Recibo))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Ope_Nom_Recibos_Empleados.Campo_No_Recibo + "='" + Datos.P_No_Recibo + "'";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Ope_Nom_Recibos_Empleados.Campo_No_Recibo + "='" + Datos.P_No_Recibo + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Nomina_ID))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'";
                    }
                }

                Dt_Recibos_Empleado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los recibos de nómina de los empleados. Error: [" + Ex.Message + "]");
            }
            return Dt_Recibos_Empleado;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Consecutivo
        ///DESCRIPCIÓN: Obtiene el numero consecutivo para las tablas ocupadas en esta clase
        ///PARAMETROS: 1.-Campo del cual se obtendra el consecutivo
        ///            2.-Nombre de la tabla
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Obtener_Consecutivo(String Campo_ID, String Tabla)
        {
            int Consecutivo = 0;
            String Mi_Sql;
            Object Obj; //Obtiene el ID con la cual se guardo los datos en la base de datos
            Mi_Sql = "SELECT NVL(MAX (" + Campo_ID + "),0) FROM " + Tabla;
            Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            Consecutivo = (Convert.ToInt32(Obj) + 1);
            return Consecutivo;
        }
        /// ********************************************************************************************************
        /// Nombre: Consultar_Recibos_Con_Nomina_Negativa
        /// 
        /// Descripción: Consulta los empleados con nominas negativas.
        /// 
        /// Parámetros: Datos.- Objeto con todas las propiedades de la clase de negocios.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: 08/Julio/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ********************************************************************************************************
        internal static DataTable Consultar_Recibos_Con_Nomina_Negativa(Cls_Ope_Nom_Recibos_Empleados_Negocio Datos)
        {
            DataTable Dt_Recibos_Saldos_Negativos = null;//Variable que almacenara los recibos con nominas negativas.
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.

            try
            {
                Mi_SQL.Append(" SELECT ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append("(");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
                Mi_SQL.Append(" || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append(" || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre);
                Mi_SQL.Append(") AS EMPLEADO, ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Salario_Diario + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Gravado + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Exento + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Deducciones + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Nomina);

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" LEFT OUTER JOIN ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados);
                Mi_SQL.Append(" ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Nomina + " < 0");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Datos.P_No_Nomina);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Tipo_Nomina_ID + "=" + Datos.P_Tipo_Nomina_ID);

                Dt_Recibos_Saldos_Negativos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los recibos con saldos negativos del empleado. Error: [" + Ex.Message + "]");
            }
            return Dt_Recibos_Saldos_Negativos;
        }
        #endregion
    }
}
