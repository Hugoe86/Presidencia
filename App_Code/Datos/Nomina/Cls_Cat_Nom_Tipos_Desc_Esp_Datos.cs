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
using Presidencia.Tipos_Descuentos_Especificos.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using System.Text;

namespace Presidencia.Tipos_Descuentos_Especificos.Datos
{
    public class Cls_Cat_Nom_Tipos_Desc_Esp_Datos
    {
        #region (Metodos)

        #region (Consulta)
        /// **********************************************************************************************
        /// Nombre: Consulta_Tipos_Descuentos_Especificos
        /// 
        /// Descripción: Metodo que consulta los tipos de descuentos especificos.
        /// 
        /// Parámetros:
        /// 
        /// Usuario Creo: Juan Alberto Hernandez Negrete.
        /// Fecha Creo: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// **********************************************************************************************
        public static DataTable Consulta_Tipos_Descuentos_Especificos(Cls_Cat_Nom_Tipos_Desc_Esp_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            StringBuilder Mi_SQL_Aux = new StringBuilder();
            DataSet Ds_Resultado = new DataSet();//Variable que almacenara el resultado de la consulta
            DataTable Dt_Resultado = null;//Variable de tipo tabla que almacenara el resultado de la consulta.

            OracleConnection Conexion = new OracleConnection();//Variable que almacena la canexion.
            OracleCommand Comando = new OracleCommand();//Variable que almacena el comando.
            OracleDataAdapter Adaptador = new OracleDataAdapter();//Variable que controlara el Fill del DataSet.
            OracleTransaction Transaccion = null;//Variable que controlara las transacciones.

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();

            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("select ");
                Mi_SQL.Append(Cat_Nom_Tipos_Desc_Esp.Tabla_Cat_Nom_Tipos_Desc_Esp + ".*, ");
                //Subquery for get clave cargo/abono.
                Mi_SQL.Append("(select (");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Tabla_Cat_Nom_Claves_Cargo_Abono + "." + Cat_Nom_Claves_Cargo_Abono.Campo_Clave + " || '-' || ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Tabla_Cat_Nom_Claves_Cargo_Abono + "." + Cat_Nom_Claves_Cargo_Abono.Campo_Descripcion + ") ");
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Tabla_Cat_Nom_Claves_Cargo_Abono);
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Tabla_Cat_Nom_Claves_Cargo_Abono + "." + Cat_Nom_Claves_Cargo_Abono.Campo_Cargo_Abono_ID + "=");
                Mi_SQL.Append(Cat_Nom_Tipos_Desc_Esp.Tabla_Cat_Nom_Tipos_Desc_Esp + "." + Cat_Nom_Tipos_Desc_Esp.Campo_Cargo_Abono_ID);
                Mi_SQL.Append(") as CLAVE_CARGO_ABONO ");

                Mi_SQL.Append(" from "); 
                Mi_SQL.Append(Cat_Nom_Tipos_Desc_Esp.Tabla_Cat_Nom_Tipos_Desc_Esp);

                if (!String.IsNullOrEmpty(Datos.P_Tipo_Desc_Esp_ID))
                {
                    if (Mi_SQL_Aux.ToString().Contains("where"))
                    {
                        Mi_SQL_Aux.Append(" and ");
                        Mi_SQL_Aux.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Tipo_Desc_Esp_ID + "='" + Datos.P_Tipo_Desc_Esp_ID + "'"); 
                    }
                    else
                    {
                        Mi_SQL_Aux.Append(" where ");
                        Mi_SQL_Aux.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Tipo_Desc_Esp_ID + "='" + Datos.P_Tipo_Desc_Esp_ID + "'"); 
                    }
                }

                if (!String.IsNullOrEmpty(Datos.P_Clave))
                {
                    if (Mi_SQL_Aux.ToString().Contains("where"))
                    {
                        Mi_SQL_Aux.Append(" and ");
                        Mi_SQL_Aux.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Clave + "='" + Datos.P_Clave + "'");
                    }
                    else
                    {
                        Mi_SQL_Aux.Append(" where ");
                        Mi_SQL_Aux.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Clave + "='" + Datos.P_Clave + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Datos.P_Descripcion))
                {
                    if (Mi_SQL_Aux.ToString().Contains("where"))
                    {
                        Mi_SQL_Aux.Append(" and ");
                        Mi_SQL_Aux.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Descripcion + "='" + Datos.P_Descripcion + "'"); 
                    }
                    else
                    {
                        Mi_SQL_Aux.Append(" where ");
                        Mi_SQL_Aux.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Descripcion + "='" + Datos.P_Descripcion + "'"); 
                    }
                }

                if (!String.IsNullOrEmpty(Datos.P_Cargo_Abono_ID)) {
                    if (Mi_SQL_Aux.ToString().Contains("where"))
                    {
                        Mi_SQL_Aux.Append(" and ");
                        Mi_SQL_Aux.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Cargo_Abono_ID + "='" + Datos.P_Cargo_Abono_ID + "'"); 
                    }
                    else {
                        Mi_SQL_Aux.Append(" where ");
                        Mi_SQL_Aux.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Cargo_Abono_ID + "='" + Datos.P_Cargo_Abono_ID + "'"); 
                    }
                }

                Comando.CommandText = Mi_SQL.ToString() + Mi_SQL_Aux.ToString();
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
        public static DataTable Consultar_Clave_Cargo_Abono(Cls_Cat_Nom_Tipos_Desc_Esp_Negocio Datos)
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
                Mi_SQL.Append("(");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Clave + "|| '-' ||" + Cat_Nom_Claves_Cargo_Abono.Campo_Descripcion);
                Mi_SQL.Append(") as CLAVE_CARGO_ABONO, ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Cargo_Abono_ID);
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Tabla_Cat_Nom_Claves_Cargo_Abono);
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

        #region (Operacion)
        /// **********************************************************************************************
        /// Nombre: Alta
        /// 
        /// Descripción: Metodo que ejecuta el Alta de Tipo de Descuenta Especifico.
        /// 
        /// Parámetros:
        /// 
        /// Usuario Creo: Juan Alberto Hernandez Negrete.
        /// Fecha Creo: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// **********************************************************************************************
        public static Boolean Alta(Cls_Cat_Nom_Tipos_Desc_Esp_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.
            Boolean Estatus = false;//Variable que almacena el estatus de la operacion.
            Object Aux = null;//Variable auxiliar para la generacion del identificador consecutivo.

            OracleConnection Conexion = new OracleConnection();//Variable que almacena la conexion.
            OracleCommand Comando = new OracleCommand();//Variable que almacena el comando.
            OracleTransaction Transaccion = null;//Variable que almacena la transaccion.

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();

            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("select nvl(max(" + Cat_Nom_Tipos_Desc_Esp.Campo_Tipo_Desc_Esp_ID + "), '00000')");
                Mi_SQL.Append(" from " + Cat_Nom_Tipos_Desc_Esp.Tabla_Cat_Nom_Tipos_Desc_Esp);

                Comando.CommandText = Mi_SQL.ToString();
                Aux = Comando.ExecuteOracleScalar();

                if (!Convert.IsDBNull(Aux))
                {
                    Datos.P_Tipo_Desc_Esp_ID = String.Format("{0:00000}", (Convert.ToInt32(Aux.ToString()) + 1));
                }
                else Datos.P_Tipo_Desc_Esp_ID = "00001";

                Mi_SQL.Remove(0, Mi_SQL.Length);

                Mi_SQL.Append("insert into ");
                Mi_SQL.Append(Cat_Nom_Tipos_Desc_Esp.Tabla_Cat_Nom_Tipos_Desc_Esp);
                Mi_SQL.Append(" (");
                Mi_SQL.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Tipo_Desc_Esp_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Clave + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Descripcion + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Cargo_Abono_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Fecha_Creo);
                Mi_SQL.Append(") values(");
                Mi_SQL.Append("'" + Datos.P_Tipo_Desc_Esp_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_Clave + "', ");
                Mi_SQL.Append("'" + Datos.P_Descripcion + "', ");
                Mi_SQL.Append("'" + Datos.P_Cargo_Abono_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_Usuario + "', ");
                Mi_SQL.Append("sysdate)");

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
        /// **********************************************************************************************
        /// Nombre: Actualizar
        /// 
        /// Descripción: Metodo que ejecuta la actualización del Tipo de Descuenta Especifico.
        /// 
        /// Parámetros:
        /// 
        /// Usuario Creo: Juan Alberto Hernandez Negrete.
        /// Fecha Creo: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// **********************************************************************************************
        public static Boolean Actualizar(Cls_Cat_Nom_Tipos_Desc_Esp_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            Boolean Estatus = false;

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
                Mi_SQL.Append(Cat_Nom_Tipos_Desc_Esp.Tabla_Cat_Nom_Tipos_Desc_Esp);
                Mi_SQL.Append(" set ");
                Mi_SQL.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Clave + "='" + Datos.P_Clave + "', ");
                Mi_SQL.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Descripcion + "='" + Datos.P_Descripcion + "', ");
                Mi_SQL.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Cargo_Abono_ID + "='" + Datos.P_Cargo_Abono_ID + "', ");
                Mi_SQL.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Usuario_Modifico + "='" + Datos.P_Usuario + "', ");
                Mi_SQL.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Fecha_Modifico + "=sysdate ");
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Tipo_Desc_Esp_ID + "='" + Datos.P_Tipo_Desc_Esp_ID + "'");

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
        /// **********************************************************************************************
        /// Nombre: Delete
        /// 
        /// Descripción: Metodo que ejecuta la baja del Tipo de Descuenta Especifico.
        /// 
        /// Parámetros:
        /// 
        /// Usuario Creo: Juan Alberto Hernandez Negrete.
        /// Fecha Creo: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// **********************************************************************************************
        public static Boolean Delete(Cls_Cat_Nom_Tipos_Desc_Esp_Negocio Datos)
        {

            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.
            Boolean Estatus = false;//Variable que alamcena el estatus de la operacion.

            OracleConnection Conexion = new OracleConnection();//Variable que almacena el estatus de la conexion.
            OracleCommand Comando = new OracleCommand();//Variable que almacena el comando.
            OracleTransaction Transaccion = null;//Variable que almacena la transaccion.

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();

            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("delete from ");
                Mi_SQL.Append(Cat_Nom_Tipos_Desc_Esp.Tabla_Cat_Nom_Tipos_Desc_Esp);
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Nom_Tipos_Desc_Esp.Campo_Tipo_Desc_Esp_ID + "='" + Datos.P_Tipo_Desc_Esp_ID + "'");

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
