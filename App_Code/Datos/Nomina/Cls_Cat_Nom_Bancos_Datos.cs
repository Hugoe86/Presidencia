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
using System.Text;
using Presidencia.Bancos_Nomina.Negocio;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;

namespace Presidencia.Bancos_Nomina.Datos
{
    public class Cls_Cat_Nom_Bancos_Datos
    {
        #region(Métodos)

        #region(Métodos Operación)
        /// ********************************************************************************************************************
        /// NOMBRE: Alta_Banco
        /// 
        /// COMENTARIOS: Esta operación inserta un nuevo registro de banco en la tabla de Cat_Nom_Bancos.
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a insertar en la tabla de Cat_Nom_Bancos.
        /// 
        /// USUARIO CREÓ:Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 16/Febrero/2011 17:52 pm.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Alta_Banco(Cls_Cat_Nom_Bancos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            OracleTransaction Transaccion = null;////Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            OracleConnection Conexion;//Variable para la conexión para la base de datos   
            OracleCommand Comando;//Sirve para la ejecución de las operaciones a la base de datos
            Object Banco_ID;//Identificador unico de la tabla de bancos.
            String Mensaje = String.Empty; //Variable que almacena el mensaje de estado de la operación
            Boolean Operacion_Completa = false;

            Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            Comando = new OracleCommand();
            Conexion.Open();
            Transaccion = Conexion.BeginTransaction();
            Comando.Transaction = Transaccion;
            Comando.Connection = Conexion;

            try
            {
                //Consultas para el ID
                Mi_SQL.Append("SELECT NVL(MAX(" + Cat_Nom_Bancos.Campo_Banco_ID + "), '00000') FROM " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos);

                //Ejecutar consulta
                Comando.CommandText = Mi_SQL.ToString();
                Banco_ID = Comando.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(Banco_ID) == false)
                    Datos.P_Banco_ID = String.Format("{0:00000}", Convert.ToInt32(Banco_ID) + 1);
                else
                    Datos.P_Banco_ID = "00001";

                Mi_SQL = new StringBuilder();

                Mi_SQL.Append("INSERT INTO " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + " (");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_Banco_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_Nombre + ", ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_No_Cuenta + ", ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_Sucursal + ", ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_Referencia + ", ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_Comentarios + ", ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_Fecha_Creo + ", ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_Tipo + ", ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_Plan_Pago + ", ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_No_Meses);
                Mi_SQL.Append(") VALUES(");
                Mi_SQL.Append("'" + Datos.P_Banco_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_Nombre + "', ");
                Mi_SQL.Append("'" + Datos.P_No_Cuenta + "', ");
                Mi_SQL.Append("'" + Datos.P_Sucursal + "', ");
                Mi_SQL.Append("'" + Datos.P_Referencia + "', ");
                Mi_SQL.Append("'" + Datos.P_Comentarios + "', ");
                Mi_SQL.Append("'" + Datos.P_Usuario_Creo + "', ");
                Mi_SQL.Append("SYSDATE, ");
                Mi_SQL.Append("'" + Datos.P_Tipo + "', ");
                Mi_SQL.Append("'" + Datos.P_Plan_Pago + "', ");
                Mi_SQL.Append("'" + Datos.P_No_Meses + "')");

                //Ejecutar consulta
                Comando.CommandText = Mi_SQL.ToString();
                Comando.ExecuteNonQuery();

                //Ejecutar transaccion
                Transaccion.Commit();
                Conexion.Close();
                Operacion_Completa = true;
            }
            catch (OracleException Ex)
            {
                if (Transaccion != null)
                {
                    Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Ex.Message + "]";
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Conexion.Close();
                
                Comando = null;
                Conexion = null;
                Transaccion = null;
            }
            return Operacion_Completa;
        }
        /// ********************************************************************************************************************
        /// NOMBRE: Modificar_Banco
        /// 
        /// COMENTARIOS: Esta operación actualiza un registro de banco en la tabla de Cat_Nom_Bancos.
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a insertar en la tabla de Cat_Nom_Bancos.
        /// 
        /// USUARIO CREÓ:Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 16/Febrero/2011 18:32 pm.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Modificar_Banco(Cls_Cat_Nom_Bancos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            OracleTransaction Transaccion = null;////Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            OracleConnection Conexion;//Variable para la conexión para la base de datos   
            OracleCommand Comando;//Sirve para la ejecución de las operaciones a la base de datos
            String Mensaje = String.Empty; //Variable que almacena el mensaje de estado de la operación
            Boolean Operacion_Completa = false;

            Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            Comando = new OracleCommand();
            Conexion.Open();
            Transaccion = Conexion.BeginTransaction();
            Comando.Transaction = Transaccion;
            Comando.Connection = Conexion;

            try
            {
                Mi_SQL.Append("UPDATE " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + " SET ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_Nombre + "='" + Datos.P_Nombre + "', ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_No_Cuenta + "='" + Datos.P_No_Cuenta + "', ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_Sucursal + "='" + Datos.P_Sucursal + "', ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_Referencia + "='" + Datos.P_Referencia + "', ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_Tipo + "='" + Datos.P_Tipo + "', ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_Plan_Pago + "='" + Datos.P_Plan_Pago + "', ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_No_Meses + "='" + Datos.P_No_Meses + "', ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "', ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_Fecha_Modifico + "=SYSDATE WHERE ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_Banco_ID + "='" + Datos.P_Banco_ID + "'");

                //Ejecutar consulta
                Comando.CommandText = Mi_SQL.ToString();
                Comando.ExecuteNonQuery();

                //Ejecutar transaccion
                Transaccion.Commit();
                Conexion.Close();
                Operacion_Completa = true;
            }
            catch (OracleException Ex)
            {
                if (Transaccion != null)
                {
                    Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Ex.Message + "]";
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Conexion.Close();

                Comando = null;
                Conexion = null;
                Transaccion = null;
            }
            return Operacion_Completa;
        }
        /// ********************************************************************************************************************
        /// NOMBRE: Eliminar_Banco
        /// 
        /// COMENTARIOS: Esta operación elimina un registro de banco en la tabla de Cat_Nom_Bancos.
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a insertar en la tabla de Cat_Nom_Bancos.
        /// 
        /// USUARIO CREÓ:Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 16/Febrero/2011 18:42 pm.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Eliminar_Banco(Cls_Cat_Nom_Bancos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            OracleTransaction Transaccion = null;////Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            OracleConnection Conexion;//Variable para la conexión para la base de datos   
            OracleCommand Comando;//Sirve para la ejecución de las operaciones a la base de datos
            String Mensaje = String.Empty; //Variable que almacena el mensaje de estado de la operación
            Boolean Operacion_Completa = false;

            Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            Comando = new OracleCommand();
            Conexion.Open();
            Transaccion = Conexion.BeginTransaction();
            Comando.Transaction = Transaccion;
            Comando.Connection = Conexion;

            try
            {
                Mi_SQL.Append("DELETE FROM " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + " WHERE ");
                Mi_SQL.Append(Cat_Nom_Bancos.Campo_Banco_ID + "='" + Datos.P_Banco_ID + "'");

                //Ejecutar consulta
                Comando.CommandText = Mi_SQL.ToString();
                Comando.ExecuteNonQuery();

                //Ejecutar transaccion
                Transaccion.Commit();
                Conexion.Close();
                Operacion_Completa = true;
            }
            catch (OracleException Ex)
            {
                if (Transaccion != null)
                {
                    Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Ex.Message + "]";
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Conexion.Close();

                Comando = null;
                Conexion = null;
                Transaccion = null;
            }
            return Operacion_Completa;
        }
        #endregion

        #region(Métodos Consulta)
        /// ********************************************************************************************************************
        /// NOMBRE: Consulta_Bancos
        /// 
        /// COMENTARIOS: Consulta los bancos que existen actualmente en el sistema registrados.
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a insertar en la tabla de Cat_Nom_Bancos.
        /// 
        /// USUARIO CREÓ:Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 16/Febrero/2011 17:52 pm.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consulta_Bancos(Cls_Cat_Nom_Bancos_Negocio Datos)
        {
            DataTable Dt_Bancos = null;//Lista de bancos que existen actualmente en el sistema registrados.
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.

            try
            {
                Mi_SQL.Append("SELECT " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + ".*");
                Mi_SQL.Append(" FROM " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos);
                

                if (!string.IsNullOrEmpty(Datos.P_Banco_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" OR " + Cat_Nom_Bancos.Campo_Banco_ID + "='" + Datos.P_Banco_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Nom_Bancos.Campo_Banco_ID + "='" + Datos.P_Banco_ID + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Nombre))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" OR UPPER(" + Cat_Nom_Bancos.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre + "%')");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE UPPER(" + Cat_Nom_Bancos.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre + "%')");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" OR " + Cat_Nom_Bancos.Campo_Tipo + "='" + Datos.P_Tipo + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Nom_Bancos.Campo_Tipo + "='" + Datos.P_Tipo + "'");
                    }
                }

                Dt_Bancos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los bancos que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Bancos;
        }
        #endregion

        #endregion
    }
}
