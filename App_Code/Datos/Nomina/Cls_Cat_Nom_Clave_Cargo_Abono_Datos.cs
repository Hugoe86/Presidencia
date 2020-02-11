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
using Presidencia.Claves_Cargo_Abono.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Text;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

namespace Presidencia.Claves_Cargo_Abono.Datos
{
    public class Cls_Cat_Nom_Clave_Cargo_Abono_Datos
    {
        #region (Metodos)

        #region (Consultas)
        /// ************************************************************************************************************************************************************
        /// Nombre: Consultar_Clave_Cargo_Abono
        /// 
        /// Descripción: Metodo que consulta las claves de cargo-abono.
        /// 
        /// Parámetros: No Aplica.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico.
        /// ************************************************************************************************************************************************************
        public static DataTable Consultar_Clave_Cargo_Abono(Cls_Cat_Nom_Clave_Cargo_Abono_Negocio Datos)
        {
            DataTable Dt_Resultado = null;//Variable que listara el resultado de la consulta.
            Boolean Estatus = false;//Variable que guarda el estatus de la operacion.
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena el query.
            OracleConnection Conexion = new OracleConnection();//Variable que almacena la conexion.
            OracleCommand Comando = new OracleCommand();//Variable que almacena el comando que ejecutara las consultas.
            OracleTransaction Transaccion = null;//Variable transaccion que almacena ejecuta o deshace cambios en la base de datos.

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();

            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("select ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Tabla_Cat_Nom_Claves_Cargo_Abono + ".* ");
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Tabla_Cat_Nom_Claves_Cargo_Abono);

                if (!String.IsNullOrEmpty(Datos.P_Cargo_Abono_ID))
                {
                    if (Mi_SQL.ToString().Trim().Contains("where"))
                    {
                        Mi_SQL.Append(" and ");
                        Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Cargo_Abono_ID + "='" + Datos.P_Cargo_Abono_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" where ");
                        Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Cargo_Abono_ID + "='" + Datos.P_Cargo_Abono_ID + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Datos.P_Clave))
                {
                    if (Mi_SQL.ToString().Trim().Contains("where"))
                    {
                        Mi_SQL.Append(" and ");
                        Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Clave + "='" + Datos.P_Clave + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" where ");
                        Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Clave + "='" + Datos.P_Clave + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Datos.P_Descripcion))
                {
                    if (Mi_SQL.ToString().Trim().Contains("where"))
                    {
                        Mi_SQL.Append(" and ");
                        Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Descripcion + "='" + Datos.P_Descripcion + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" where ");
                        Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Descripcion + "='" + Datos.P_Descripcion + "'");
                    }
                }

                Mi_SQL.Append(" order by " + Cat_Nom_Claves_Cargo_Abono.Campo_Clave + " asc ");

                Comando.CommandText = Mi_SQL.ToString();
                //create the DataAdapter & DataSet
                OracleDataAdapter da = new OracleDataAdapter(Comando);
                DataSet ds = new DataSet();

                //fill the DataSet using default values for DataTable names, etc.
                da.Fill(ds);
                Dt_Resultado = ds.Tables[0]; 

                Transaccion.Commit();
                Estatus = true;
            }
            catch (Exception Ex)
            {
                Transaccion.Rollback();
                throw new Exception(Ex.Message);
            }
            finally { Conexion.Close(); }
            return Dt_Resultado;
        }
        #endregion

        #region (Operación)
        /// ************************************************************************************************************************************************************
        /// Nombre: Alta
        /// 
        /// Descripción: Metodo que ejecuta el alta de una clave.
        /// 
        /// Parámetros:
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico.
        /// ************************************************************************************************************************************************************
        public static Boolean Alta(Cls_Cat_Nom_Clave_Cargo_Abono_Negocio Datos)
        {
            Object Clave_Cargo_Abono = null;
            Boolean Estatus = false;//Variable que almacena el estatus de la operacion.
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.
            OracleConnection Conexion = new OracleConnection();//Variable que almacena la conexión.
            OracleCommand comando = new OracleCommand();//Variable que almacena el comando.
            OracleTransaction Transaccion = null;//Variable que controlara las transacciones.

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();

            Transaccion = Conexion.BeginTransaction();
            comando.Connection = Conexion;
            comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("select nvl(max(" + Cat_Nom_Claves_Cargo_Abono.Campo_Cargo_Abono_ID + "), '00000') ");
                Mi_SQL.Append(" from " + Cat_Nom_Claves_Cargo_Abono.Tabla_Cat_Nom_Claves_Cargo_Abono);

                comando.CommandText = Mi_SQL.ToString();
                Clave_Cargo_Abono = comando.ExecuteOracleScalar();

                if (!Convert.IsDBNull(Clave_Cargo_Abono))
                {
                    Datos.P_Cargo_Abono_ID = String.Format("{0:00000}", (Convert.ToInt32(Clave_Cargo_Abono.ToString()) + 1));
                }
                else
                {
                    Datos.P_Cargo_Abono_ID = "00001";
                }

                Mi_SQL.Remove(0, Mi_SQL.Length);

                Mi_SQL.Append("insert into ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Tabla_Cat_Nom_Claves_Cargo_Abono);
                Mi_SQL.Append(" (");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Cargo_Abono_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Clave + ", ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Descripcion + ", ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Fecha_Creo + ") values(");
                Mi_SQL.Append("'" + Datos.P_Cargo_Abono_ID + "', ");
                Mi_SQL.Append(Datos.P_Clave + ", ");
                Mi_SQL.Append("'" + Datos.P_Descripcion + "', ");
                Mi_SQL.Append("'" + Datos.P_Usuario + "', ");
                Mi_SQL.Append("sysdate");
                Mi_SQL.Append(")");

                comando.CommandText = Mi_SQL.ToString();
                comando.ExecuteNonQuery();
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
        /// Nombre: Actualizar
        /// 
        /// Descripción: Metodo que ejecuta la actualización de una clave.
        /// 
        /// Parámetros:
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico.
        /// ************************************************************************************************************************************************************
        public static Boolean Actualizar(Cls_Cat_Nom_Clave_Cargo_Abono_Negocio Datos)
        {
            Boolean Estatus = false;//Variable que almacena el estatus de la operacion.
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.

            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();

            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("update ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Tabla_Cat_Nom_Claves_Cargo_Abono);
                Mi_SQL.Append(" set ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Clave + "=" + Datos.P_Clave + ", ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Descripcion + "='" + Datos.P_Descripcion + "', ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Usuario_Creo + "='" + Datos.P_Usuario + "', ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Fecha_Creo + "=sysdate ");
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Cargo_Abono_ID + "='" + Datos.P_Cargo_Abono_ID + "'");

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
        /// Nombre: Delete
        /// 
        /// Descripción: Metodo que ejecuta elimina una clave.
        /// 
        /// Parámetros:
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico.
        /// ************************************************************************************************************************************************************
        public static Boolean Delete(Cls_Cat_Nom_Clave_Cargo_Abono_Negocio Datos)
        {
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            Boolean Estatus = false;
            StringBuilder Mi_SQL = new StringBuilder();

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();

            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("delete from ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Tabla_Cat_Nom_Claves_Cargo_Abono);
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Cargo_Abono_ID + "='" + Datos.P_Cargo_Abono_ID + "'");

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
