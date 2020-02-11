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
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using System.Text;
using Presidencia.Orden_Judicial.Negocio;

namespace Presidencia.Orden_Judicial.Datos
{
    public class Cls_Cat_Nom_Tab_Orden_Judicial_Datos
    {
        #region (Métodos)

        #region (Operación)
        /// *************************************************************************************************
        /// Nombre: Alta_Parametro_Orden_Judicial
        /// 
        /// Descripción: Ejecuta el alta de un parametro de orden judicial del empleado.
        /// 
        /// Parámetros: Datos.- Instancia de la clase de negocios con los valores ya establecidos.
        /// 
        /// Usuario creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 5/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *************************************************************************************************
        public static Boolean Alta_Parametro_Orden_Judicial(Cls_Cat_Nom_Tab_Orden_Judicial_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            OracleTransaction Transaccion = null;////Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            OracleConnection Conexion;//Variable para la conexión para la base de datos   
            OracleCommand Comando;//Sirve para la ejecución de las operaciones a la base de datos
            Object ORDEN_JUDICIAL_ID;//Identificador unico de la tabla de bancos.
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
                Mi_SQL.Append("SELECT NVL(MAX(" + Cat_Nom_Tab_Orden_Judicial.Campo_Orden_Judicial_ID + "), '00000') FROM " + Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial);

                //Ejecutar consulta
                Comando.CommandText = Mi_SQL.ToString();
                ORDEN_JUDICIAL_ID = Comando.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(ORDEN_JUDICIAL_ID) == false)
                    Datos.P_Orden_Judicial_ID = String.Format("{0:00000}", Convert.ToInt32(ORDEN_JUDICIAL_ID) + 1);
                else
                    Datos.P_Orden_Judicial_ID = "00001";

                Mi_SQL = new StringBuilder();

                Mi_SQL.Append("INSERT INTO " + Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + " (");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Orden_Judicial_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Empleado_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Beneficiario + ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Sueldo + ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Sueldo + ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Sueldo + ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Aguinaldo+ ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Aguinaldo+ ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Aguinaldo + ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_PV + ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_PV + ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_PV + ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Indemnizacion + ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Indemnizacion + ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Indemnizacion + ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Fecha_Creo + ") VALUES(");
                Mi_SQL.Append("'" + Datos.P_Orden_Judicial_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_Empleado_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_Beneficiario + "', ");
                Mi_SQL.Append("'" + Datos.P_Tipo_Descuento_Orden_Judicial_Sueldo + "', ");
                Mi_SQL.Append("" + Datos.P_Cantidad_Porcentaje_Sueldo + ", ");
                Mi_SQL.Append("'" + Datos.P_Bruto_Neto_Orden_Judicial_Sueldo+ "', ");
                Mi_SQL.Append("'" + Datos.P_Tipo_Descuento_Orden_Judicial_Aguinaldo + "', ");
                Mi_SQL.Append("" + Datos.P_Cantidad_Porcentaje_Aguinaldo + ", ");
                Mi_SQL.Append("'" + Datos.P_Bruto_Neto_Orden_Aguinaldo + "', ");
                Mi_SQL.Append("'" + Datos.P_Tipo_Descuento_Orden_Judicial_Prima_Vacacional + "', ");
                Mi_SQL.Append("" + Datos.P_Cantidad_Porcentaje_Prima_Vacacional + ", ");
                Mi_SQL.Append("'" + Datos.P_Bruto_Neto_Orden_Prima_Vacacional + "', ");
                Mi_SQL.Append("'" + Datos.P_Tipo_Descuento_Orden_Judicial_Indemnizacion + "', ");
                Mi_SQL.Append("" + Datos.P_Cantidad_Porcentaje_Indemnizacion + ", ");
                Mi_SQL.Append("'" + Datos.P_Bruto_Neto_Orden_Indemnizacion + "', ");
                Mi_SQL.Append("'" + Datos.P_Usuario_Creo + "', ");
                Mi_SQL.Append("SYSDATE)");

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
        /// *************************************************************************************************
        /// Nombre: Modificar_Parametro_Orden_Judicial
        /// 
        /// Descripción: Ejecuta la actualización de un parametro de orden judicial del empleado.
        /// 
        /// Parámetros: Datos.- Instancia de la clase de negocios con los valores ya establecidos.
        /// 
        /// Usuario creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 5/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *************************************************************************************************
        public static Boolean Modificar_Parametro_Orden_Judicial(Cls_Cat_Nom_Tab_Orden_Judicial_Negocio Datos)
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
                Mi_SQL.Append("UPDATE " + Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + " SET ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Orden_Judicial_ID + " = '" + Datos.P_Orden_Judicial_ID + "', ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "', ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Beneficiario + " = '" + Datos.P_Beneficiario + "', ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Sueldo + " = '" + Datos.P_Tipo_Descuento_Orden_Judicial_Sueldo + "', ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Sueldo + " = " + Datos.P_Cantidad_Porcentaje_Sueldo + ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Sueldo + " = '" + Datos.P_Bruto_Neto_Orden_Judicial_Sueldo+ "', ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Aguinaldo + " = '" + Datos.P_Tipo_Descuento_Orden_Judicial_Aguinaldo + "', ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Aguinaldo + " = " + Datos.P_Cantidad_Porcentaje_Aguinaldo + ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Aguinaldo + " = '" + Datos.P_Bruto_Neto_Orden_Aguinaldo + "', ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_PV + " = '" + Datos.P_Tipo_Descuento_Orden_Judicial_Prima_Vacacional + "', ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_PV + " = " + Datos.P_Cantidad_Porcentaje_Prima_Vacacional + ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_PV + " = '" + Datos.P_Bruto_Neto_Orden_Prima_Vacacional + "', ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Indemnizacion + " = '" + Datos.P_Tipo_Descuento_Orden_Judicial_Indemnizacion + "', ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Indemnizacion + " = " + Datos.P_Cantidad_Porcentaje_Indemnizacion + ", ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Indemnizacion + " = '" + Datos.P_Bruto_Neto_Orden_Indemnizacion + "', ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Modifico + "', ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Fecha_Creo + " = SYSDATE");
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Orden_Judicial_ID + " = '" + Datos.P_Orden_Judicial_ID + "'");

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
        /// *************************************************************************************************
        /// Nombre: Eliminar_Parametro_Orden_Judicial
        /// 
        /// Descripción: Ejecuta la baja de un parametro de orden judicial del empleado.
        /// 
        /// Parámetros: Datos.- Instancia de la clase de negocios con los valores ya establecidos.
        /// 
        /// Usuario creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 5/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *************************************************************************************************
        public static Boolean Eliminar_Parametro_Orden_Judicial(Cls_Cat_Nom_Tab_Orden_Judicial_Negocio Datos)
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
                Mi_SQL.Append("DELETE FROM " + Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Campo_Orden_Judicial_ID + " = '" + Datos.P_Orden_Judicial_ID + "'");

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

        #region (Consultas)
        /// *************************************************************************************************
        /// Nombre: Consultar_Parametros_Orden_Judicial_Empleado
        /// 
        /// Descripción: Ejecuta la baja de un parametro de orden judicial del empleado.
        /// 
        /// Parámetros: Datos.- Instancia de la clase de negocios con los valores ya establecidos.
        /// 
        /// Usuario creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 5/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *************************************************************************************************
        internal static DataTable Consultar_Parametros_Orden_Judicial_Empleado(Cls_Cat_Nom_Tab_Orden_Judicial_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Parametros_Orden_Judicial = null;

            try
            {
                Mi_SQL.Append("SELECT " + Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + ".*, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append("('[' ||" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " || '] -- ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS EMPLEADO");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial);
                Mi_SQL.Append(" INNER JOIN ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" ON ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + "." + Cat_Nom_Tab_Orden_Judicial.Campo_Empleado_ID);
                Mi_SQL.Append(" = ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                if (!String.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                        Mi_SQL.Append(" AND " + Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + "." + Cat_Nom_Tab_Orden_Judicial.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'");
                    else
                        Mi_SQL.Append(" WHERE " + Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + "." + Cat_Nom_Tab_Orden_Judicial.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_Orden_Judicial_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                        Mi_SQL.Append(" AND " + Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + "." + Cat_Nom_Tab_Orden_Judicial.Campo_Orden_Judicial_ID + "='" + Datos.P_Orden_Judicial_ID + "'");
                    else
                        Mi_SQL.Append(" WHERE " + Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + "." + Cat_Nom_Tab_Orden_Judicial.Campo_Orden_Judicial_ID + "='" + Datos.P_Orden_Judicial_ID + "'");
                }

                Dt_Parametros_Orden_Judicial = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los parámetros de orden judicial del empleado. Error: [" + Ex.Message + "]");
            }
            return Dt_Parametros_Orden_Judicial;
        }
        #endregion

        #endregion
    }
}
