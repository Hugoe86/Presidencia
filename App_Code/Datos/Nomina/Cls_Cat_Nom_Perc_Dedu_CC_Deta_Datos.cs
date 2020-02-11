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
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using System.Text;
using Presidencia.Relacion_Perc_Dedu_Cuentas_Contables.Negocio;

namespace Presidencia.Relacion_Perc_Dedu_Cuentas_Contables.Datos
{
    public class Cls_Cat_Nom_Perc_Dedu_CC_Deta_Datos
    {
        #region (Metodos)

        #region (Consultas)
        /// ************************************************************************************************************************************************************
        /// Nombre: Consultar_Cuentas_Contables_X_Concepto
        /// 
        /// Descripción: Metodo que consulta las cuentas contables por percepcion y/o deduccion.
        /// 
        /// Parámetros:
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico.
        /// ************************************************************************************************************************************************************
        public static DataTable Consultar_Cuentas_Contables_X_Concepto(Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.

            DataSet Ds_Resultado = new DataSet();//Variable que almacena el resultado.
            DataTable Dt_Resultado = new DataTable();//Variable que almacena el resultado.

            OracleConnection Conexion = new OracleConnection();//Variable que controla la conexion con la base de datos.
            OracleCommand Comando = new OracleCommand();//Variable que ejecuta la consulta.
            OracleDataAdapter Adaptador = new OracleDataAdapter();//Variable que controla las actualizaciones hacia la base de datos.
            OracleTransaction Transaccion = null;//Variable que valida si se ejecuta o rechaza el comando.

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();

            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("select ");

                Mi_SQL.Append("('[' || ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave);
                Mi_SQL.Append(" || '] - ' || ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre);
                Mi_SQL.Append(") as CONCEPTO, ");

                Mi_SQL.Append("('[' || ");
                Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta);
                Mi_SQL.Append(" || '] - ' || ");
                Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Descripcion);
                Mi_SQL.Append(") as CUENTA, ");

                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + ", ");
                Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID);

                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Nom_Perc_Dedu_CC_Deta.Tabla_Cat_Nom_Perc_Dedu_CC_Deta);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " on ");
                Mi_SQL.Append(Cat_Nom_Perc_Dedu_CC_Deta.Tabla_Cat_Nom_Perc_Dedu_CC_Deta + "." + Cat_Nom_Perc_Dedu_CC_Deta.Campo_Percepcion_Deduccion_ID + "=");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);

                Mi_SQL.Append(" left outer join " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + " on ");
                Mi_SQL.Append(Cat_Nom_Perc_Dedu_CC_Deta.Tabla_Cat_Nom_Perc_Dedu_CC_Deta + "." + Cat_Nom_Perc_Dedu_CC_Deta.Campo_Cuenta_Contable_ID + "=");
                Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID);


                if (!String.IsNullOrEmpty(Datos.P_Percepcion_Deduccion_ID))
                {
                    if (Mi_SQL.ToString().Contains("where"))
                    {
                        Mi_SQL.Append(" and ");
                        Mi_SQL.Append(Cat_Nom_Perc_Dedu_CC_Deta.Tabla_Cat_Nom_Perc_Dedu_CC_Deta + "." + Cat_Nom_Perc_Dedu_CC_Deta.Campo_Percepcion_Deduccion_ID + "='" + Datos.P_Percepcion_Deduccion_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" where ");
                        Mi_SQL.Append(Cat_Nom_Perc_Dedu_CC_Deta.Tabla_Cat_Nom_Perc_Dedu_CC_Deta + "." + Cat_Nom_Perc_Dedu_CC_Deta.Campo_Percepcion_Deduccion_ID + "='" + Datos.P_Percepcion_Deduccion_ID + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Datos.P_Cuenta_Contable_ID))
                {
                    if (Mi_SQL.ToString().Contains("where"))
                    {
                        Mi_SQL.Append(" and ");
                        Mi_SQL.Append(Cat_Nom_Perc_Dedu_CC_Deta.Tabla_Cat_Nom_Perc_Dedu_CC_Deta + "." + Cat_Nom_Perc_Dedu_CC_Deta.Campo_Cuenta_Contable_ID + "='" + Datos.P_Cuenta_Contable_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" where ");
                        Mi_SQL.Append(Cat_Nom_Perc_Dedu_CC_Deta.Tabla_Cat_Nom_Perc_Dedu_CC_Deta + "." + Cat_Nom_Perc_Dedu_CC_Deta.Campo_Cuenta_Contable_ID + "='" + Datos.P_Cuenta_Contable_ID + "'");
                    }
                }

                if (String.IsNullOrEmpty(Datos.P_Cuenta_Contable_ID) && String.IsNullOrEmpty(Datos.P_Percepcion_Deduccion_ID))
                {
                    Conexion.Close();
                    return Dt_Resultado;
                }

                Comando.CommandText = Mi_SQL.ToString();
                Adaptador.SelectCommand = Comando;

                Adaptador.Fill(Ds_Resultado);
                Dt_Resultado = Ds_Resultado.Tables[0];

                Transaccion.Commit();
            }
            catch (Exception Ex)
            {
                Transaccion.Rollback();
                throw new Exception(Ex.Message);
            }
            finally { Conexion.Close(); }
            return Dt_Resultado;            
        }
        /// ************************************************************************************************************************************************************
        /// Nombre: Consultar_Cuentas_Contables
        /// 
        /// Descripción: Metodo que consulta las cuentas contables que se existen en el sistema.
        /// 
        /// Parámetros:
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico.
        /// ************************************************************************************************************************************************************
        public static DataTable Consultar_Cuentas_Contables()
        {
            OracleConnection conexion = new OracleConnection();//Variable que almacenara la conexion con la base de datos.
            OracleCommand Comando = new OracleCommand();//Variable que controla la sentencias contra la base de datos.
            OracleDataAdapter Adaptador = new OracleDataAdapter();//Variable que controla el Fill de las tablas con el resultado de la consulta.
            OracleTransaction Transaccion = null;//Variable que controla las transacciones realizadas contra la base de datos.

            DataSet Ds_Resultado = new DataSet();//Variable que almacena el resultado.
            DataTable Dt_Resultado = null;//Variable que almacena el resultado.

            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.

            conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            conexion.Open();

            Transaccion = conexion.BeginTransaction();
            Comando.Connection = conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("select ");
                Mi_SQL.Append("(" + Cat_Con_Cuentas_Contables.Campo_Cuenta + " || ' - ' || " + Cat_Con_Cuentas_Contables.Campo_Descripcion + ") as CUENTA, ");
                Mi_SQL.Append(Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID);
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);

                Comando.CommandText = Mi_SQL.ToString();
                Adaptador.SelectCommand = Comando;

                Adaptador.Fill(Ds_Resultado);
                Dt_Resultado = Ds_Resultado.Tables[0];

                Transaccion.Commit();
            }
            catch (Exception Ex)
            {
                Transaccion.Rollback();
                throw new Exception(Ex.Message);
            }
            finally { conexion.Close(); }
            return Dt_Resultado;
        }
        /// ************************************************************************************************************************************************************
        /// Nombre: Consultar_Conceptos
        /// 
        /// Descripción: Metodo que consulta los conceptos percepciones y/o deducciones que existen actualmente ene le sistema.
        /// 
        /// Parámetros:
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico.
        /// ************************************************************************************************************************************************************
        public static DataTable Consultar_Conceptos()
        {
            OracleConnection conexion = new OracleConnection();//Variable que almacena la conexion con la base de datos.
            OracleCommand Comando = new OracleCommand();//Variable que controla las sentencias que se realizan contra la base de datos.
            OracleDataAdapter Adaptador = new OracleDataAdapter();//Variable que controla el Fill de las tablas con el resultado de la consulta.
            OracleTransaction Transaccion = null;//Variable que controla las transacciones realizadas  contra la base de datos.

            DataSet Ds_Resultado = new DataSet();//Variable que almacena el resultado.
            DataTable Dt_Resultado = null;//Variable que almacena el resultado.

            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.

            conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            conexion.Open();

            Transaccion = conexion.BeginTransaction();
            Comando.Connection = conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("select ");
                Mi_SQL.Append("(" + Cat_Nom_Percepcion_Deduccion.Campo_Clave + " || ' - ' || " + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ") as CONCEPTO, ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion);

                Comando.CommandText = Mi_SQL.ToString();
                Adaptador.SelectCommand = Comando;

                Adaptador.Fill(Ds_Resultado);
                Dt_Resultado = Ds_Resultado.Tables[0];

                Transaccion.Commit();
            }
            catch (Exception Ex)
            {
                Transaccion.Rollback();
                throw new Exception(Ex.Message);
            }
            finally { conexion.Close(); }
            return Dt_Resultado;
        }
        /// ************************************************************************************************************************************************************
        /// Nombre: Consultar_Conceptos_X_Clave
        /// 
        /// Descripción: Metodo que consulta los conceptos percepciones y/o deducciones por la clave ingresada. 
        /// 
        /// Parámetros:
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico.
        /// ************************************************************************************************************************************************************
        public static DataTable Consultar_Conceptos_X_Clave(Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio Datos)
        {
            OracleConnection conexion = new OracleConnection();//Variable que almacena la conexion.
            OracleCommand Comando = new OracleCommand();//Variable que controla las sentencias realizadas contra la base de datos.
            OracleDataAdapter Adaptador = new OracleDataAdapter();//Variable que controla el Fill de las tablas con el resultado de la consulta. 
            OracleTransaction Transaccion = null;//Variable que controla las transacciones realizadas contra la base de datos.

            DataSet Ds_Resultado = new DataSet();//Variable que almacena el resultado.
            DataTable Dt_Resultado = null;//Variable que almacena el resultado.

            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.

            conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            conexion.Open();

            Transaccion = conexion.BeginTransaction();
            Comando.Connection = conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("select ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion);
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Campo_Clave + "= '" + Datos.P_CLave + "'");

                Comando.CommandText = Mi_SQL.ToString();
                Adaptador.SelectCommand = Comando;

                Adaptador.Fill(Ds_Resultado);
                Dt_Resultado = Ds_Resultado.Tables[0];

                Transaccion.Commit();
            }
            catch (Exception Ex)
            {
                Transaccion.Rollback();
                throw new Exception(Ex.Message);
            }
            finally { conexion.Close(); }
            return Dt_Resultado;
        }
        /// ************************************************************************************************************************************************************
        /// Nombre: Consultar_Cuenta_Contable_X_Cuenta
        /// 
        /// Descripción: Metodo que consulta la cuenta contable por la clave ingresada. 
        /// 
        /// Parámetros:
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico.
        /// ************************************************************************************************************************************************************
        public static DataTable Consultar_Cuenta_Contable_X_Cuenta(Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio Datos)
        {
            OracleConnection conexion = new OracleConnection();//Variable que almacena la conexion.
            OracleCommand Comando = new OracleCommand();//Variable que controla las sentencias realizadas contra la base de datos.
            OracleDataAdapter Adaptador = new OracleDataAdapter();//Variable que controla el Fill de las tablas con el resultado de la consulta. 
            OracleTransaction Transaccion = null;//Variable que controla las transacciones realizadas contra la base de datos.

            DataSet Ds_Resultado = new DataSet();//Variable que almacena el resultado.
            DataTable Dt_Resultado = null;//Variable que almacena el resultado.

            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.

            conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            conexion.Open();

            Transaccion = conexion.BeginTransaction();
            Comando.Connection = conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("select ");
                Mi_SQL.Append(Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID);
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Con_Cuentas_Contables.Campo_Cuenta + "= '" + Datos.P_Cuenta + "'");

                Comando.CommandText = Mi_SQL.ToString();
                Adaptador.SelectCommand = Comando;

                Adaptador.Fill(Ds_Resultado);
                Dt_Resultado = Ds_Resultado.Tables[0];

                Transaccion.Commit();
            }
            catch (Exception Ex)
            {
                Transaccion.Rollback();
                throw new Exception(Ex.Message);
            }
            finally { conexion.Close(); }
            return Dt_Resultado;
        }
        #endregion

        #region (Operaciones)
        /// ************************************************************************************************************************************************************
        /// Nombre: Alta_Individual
        /// 
        /// Descripción: Metodo que ejecuta el alta de la relación entre las percepciones y/o deducciones y las cuentas contables.
        /// 
        /// Parámetros: Datos.- Objeto que permite mover todas las proiedades a travez de la capa de usuario a ala de datos.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico.
        /// ************************************************************************************************************************************************************
        public static Boolean Alta_Individual(Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.
            Boolean Estatus = false;//variable que almacena el estatus de la operacion.
            String Percepcion_Deduccion_ID = String.Empty;//identificador de la tabla de Cat_Nom_Percepcion_Deduccion
            String Cuenta_Contable_ID = String.Empty;//identificador de la tabla de Cat_Nom_cuentas_Contables.
            OracleConnection Conexion = new OracleConnection();//Variable que controla la conexion.
            OracleCommand Comando = new OracleCommand();//Variable que controla las operaciones a realizar sobre la base de datos.
            OracleTransaction Transaccion = null;//Variable que controla las transacciones.

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();

            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("insert into ");
                Mi_SQL.Append(Cat_Nom_Perc_Dedu_CC_Deta.Tabla_Cat_Nom_Perc_Dedu_CC_Deta);
                Mi_SQL.Append(" (");
                Mi_SQL.Append(Cat_Nom_Perc_Dedu_CC_Deta.Campo_Percepcion_Deduccion_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Perc_Dedu_CC_Deta.Campo_Cuenta_Contable_ID);
                Mi_SQL.Append(") values(");
                Mi_SQL.Append("'" + Datos.P_Percepcion_Deduccion_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_Cuenta_Contable_ID + "'");
                Mi_SQL.Append(")");

                Comando.CommandText = Mi_SQL.ToString();
                Comando.ExecuteNonQuery();

                Transaccion.Commit();
                Estatus = true;
            }
            catch (Exception Ex)
            {
                Transaccion.Rollback();
                throw new Exception(Ex.Message);
            }
            finally { Conexion.Close(); }
            return Estatus;
        }
        /// ************************************************************************************************************************************************************
        /// Nombre: Alta_Individual
        /// 
        /// Descripción: Metodo que ejecuta el alta de la relación entre las percepciones y/o deducciones y las cuentas contables.
        /// 
        /// Parámetros: Datos.- Objeto que permite mover todas las proiedades a travez de la capa de usuario a ala de datos.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico.
        /// ************************************************************************************************************************************************************
        public static Boolean Delete(Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.
            Boolean Estatus = false;//variable que almacena el estatus de la operacion.
            String Percepcion_Deduccion_ID = String.Empty;//identificador de la tabla de Cat_Nom_Percepcion_Deduccion
            String Cuenta_Contable_ID = String.Empty;//identificador de la tabla de Cat_Nom_cuentas_Contables.
            OracleConnection Conexion = new OracleConnection();//Variable que controla la conexion.
            OracleCommand Comando = new OracleCommand();//Variable que controla las operaciones a realizar sobre la base de datos.
            OracleTransaction Transaccion = null;//Variable que controla las transacciones.

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();

            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("delete from ");
                Mi_SQL.Append(Cat_Nom_Perc_Dedu_CC_Deta.Tabla_Cat_Nom_Perc_Dedu_CC_Deta);
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Nom_Perc_Dedu_CC_Deta.Campo_Percepcion_Deduccion_ID + "='" + Datos.P_Percepcion_Deduccion_ID + "' and ");
                Mi_SQL.Append(Cat_Nom_Perc_Dedu_CC_Deta.Campo_Cuenta_Contable_ID + "='" + Datos.P_Cuenta_Contable_ID + "'");

                Comando.CommandText = Mi_SQL.ToString();
                Comando.ExecuteNonQuery();

                Transaccion.Commit();
                Estatus = true;
            }
            catch (Exception Ex)
            {
                Transaccion.Rollback();
                throw new Exception(Ex.Message);
            }
            finally { Conexion.Close(); }
            return Estatus;
        }
        #endregion

        #endregion
    }
}
