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
using Presidencia.Percepciones_Deducciones_Fijas.Negocio;
using System.Text;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;


namespace Presidencia.Percepciones_Deducciones_Fijas.Datos
{
    public class Cls_Ope_Nom_Percepciones_Deducciones_Fijas_Datos
    {
        #region (Metodos)

        #region (Operación)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Registro_Percepciones_Deducciones_Tipo_Nomina_Sindicato
        /// DESCRIPCION : Registra las percepciones o deducciones que aplicaran para él 
        ///               empleado.
        /// 
        /// PARAMETROS  : Dt_Datos.- Percepciones on Deducciones a aplicar al empleado. 
        ///               Empleado_ID.- Empleado al que se le aplicaran las percepciones y/o
        ///                             Deducciones.
        ///               Concepto.- Concepto al que pertenece la percepción o deducción.                 
        /// 
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 05/Enero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Registro_Percepciones_Deducciones_Tipo_Nomina(Cls_Ope_Nom_Percepciones_Deducciones_Fijas_Negocio Datos)
        {
            String Mi_SQL;           //Obtiene la cadena de inserción hacía la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                //Removemos las percepciones deducciones del empleado.
                Mi_SQL = "DELETE FROM " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det +
                         " WHERE " + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";

                Mi_SQL += " AND " + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Concepto + "='" + Datos.P_Concepto + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                Mi_SQL = String.Empty;

                if (Datos.P_Dt_Percepciones_Tipo_Nomina is DataTable)
                {
                    if (Datos.P_Dt_Percepciones_Tipo_Nomina.Rows.Count > 0)
                    {
                        foreach (DataRow PERCEPCION_DEDUCCION in Datos.P_Dt_Percepciones_Tipo_Nomina.Rows)
                        {
                            if (PERCEPCION_DEDUCCION is DataRow)
                            {
                                Mi_SQL = "INSERT INTO " + 
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + 
                                    " (" +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + ", " + 
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + ", " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Concepto + ", " + 
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad + ", " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe + ", " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo + ", " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida + ", " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID + ", " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina +
                                    ") VALUES(" +
                                    "'" + Datos.P_Empleado_ID + "', " +
                                    "'" + PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString() + "', " +
                                    "'" + Datos.P_Concepto + "', " +
                                    Convert.ToDouble((String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString().Trim())) ? "0" : PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString().Trim()) + ", " +
                                    Convert.ToDouble((String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe].ToString().Trim())) ? "0" : PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe].ToString().Trim()) + ", " +
                                    Convert.ToDouble((String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo].ToString().Trim())) ? "0" : PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo].ToString().Trim()) + ", " +
                                    Convert.ToDouble((String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida].ToString().Trim())) ? "0" : PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida].ToString().Trim()) + ", " +
                                    "'" + PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID].ToString() + "', " +
                                    Convert.ToDouble((String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina].ToString().Trim())) ? "0" : PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina].ToString().Trim()) + ")";

                                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos            
                            }
                        }
                    }
                }

                Mi_SQL = String.Empty;

                if (Datos.P_Dt_Deducciones_Tipo_Nomina is DataTable)
                {
                    if (Datos.P_Dt_Deducciones_Tipo_Nomina.Rows.Count > 0)
                    {
                        foreach (DataRow PERCEPCION_DEDUCCION in Datos.P_Dt_Deducciones_Tipo_Nomina.Rows)
                        {
                            if (PERCEPCION_DEDUCCION is DataRow)
                            {
                                Mi_SQL = "INSERT INTO " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det +
                                    " (" +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + ", " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + ", " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Concepto + ", " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad + ", " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe + ", " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo + ", " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida + ", " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID + ", " +
                                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina +
                                    ") VALUES(" +
                                    "'" + Datos.P_Empleado_ID + "', " +
                                    "'" + PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString() + "', " +
                                    "'" + Datos.P_Concepto + "', " +
                                    Convert.ToDouble((String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString().Trim())) ? "0" : PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString().Trim()) + ", " +
                                    Convert.ToDouble((String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe].ToString().Trim())) ? "0" : PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe].ToString().Trim()) + ", " +
                                    Convert.ToDouble((String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo].ToString().Trim())) ? "0" : PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo].ToString().Trim()) + ", " +
                                    Convert.ToDouble((String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida].ToString().Trim())) ? "0" : PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida].ToString().Trim()) + ", " +
                                    "'" + PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID].ToString() + "', " +
                                    Convert.ToDouble((String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina].ToString().Trim())) ? "0" : PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina].ToString().Trim()) + ")";

                                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos            
                            }
                        }
                    }
                }

                Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
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
        #endregion

        #region (Consulta)
        /// *******************************************************************************************************************
        /// NOMBRE: Consultar_Percepciones_Deducciones_Tipo_Nomina
        ///
        /// DESCRIPCIÓN: Consulta las percepciones y deducciones que tiene el empleado asignadas por tipo de nómina.
        /// 
        /// PARÁMETROS: 
        /// 
        /// USUARIO CREÓ:
        /// FECHA CREÓ:
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// *******************************************************************************************************************
        public static DataTable Consultar_Percepciones_Deducciones_Tipo_Nomina(Cls_Ope_Nom_Percepciones_Deducciones_Fijas_Negocio Datos)
        {
            DataTable Dt_Percepciones_Deducciones = null;//Variable que almacenara el listado de conceptos por tipo de nomina.
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det + ".");
                Mi_SQL.Append(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det + ".");
                Mi_SQL.Append(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos);

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "=");
                Mi_SQL.Append(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det + ".");
                Mi_SQL.Append(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID);

                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det + ".");
                Mi_SQL.Append(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Tipo_Nomina_ID + "='" + string.Empty + "'");

                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Campo_Tipo + "='" + string.Empty + "'");

                Dt_Percepciones_Deducciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text , Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las percepciones y deducciones por tipo de nómina. Error: [" + Ex.Message + "]");
            }
            return Dt_Percepciones_Deducciones;
        }
        #endregion

        #endregion
    }
}
