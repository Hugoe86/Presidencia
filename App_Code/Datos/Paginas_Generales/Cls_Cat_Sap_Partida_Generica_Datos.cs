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
using Presidencia.Sap_Partida_Generica.Negocio;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using System.Text;

namespace Presidencia.Sap_Partida_Generica.Datos
{
    public class Cls_Cat_Sap_Partida_Generica_Datos
    {
        #region(Métodos de Operación)
        /// ********************************************************************************************************************
        /// NOMBRE: Alta_Partida_Generica
        /// 
        /// COMENTARIOS: Esta operación inserta un nuevo registro de Partida Generica en la tabla de Cat_Sap_Partida_Generica.
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a insertar en la tabla de Cat_Sap_Partida_Generica.
        /// 
        /// USUARIO CREÓ:Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 25/Febrero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Alta_Partida_Generica(Cls_Cat_Sap_Partida_Generica_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            OracleTransaction Transaccion = null;////Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            OracleConnection Conexion;//Variable para la conexión para la base de datos   
            OracleCommand Comando;//Sirve para la ejecución de las operaciones a la base de datos
            Object Partida_Generica_ID;//Identificador unico de la tabla de bancos.
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
                Mi_SQL.Append("SELECT NVL(MAX(" + Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID + "), '00000') FROM " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas);

                //Ejecutar consulta
                Comando.CommandText = Mi_SQL.ToString();
                Partida_Generica_ID = Comando.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(Partida_Generica_ID) == false)
                    Datos.P_Partida_Generica_ID= String.Format("{0:00000}", Convert.ToInt32(Partida_Generica_ID) + 1);
                else
                    Datos.P_Partida_Generica_ID = "00001";

                Mi_SQL = new StringBuilder();

                Mi_SQL.Append("INSERT INTO " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + " (");
                Mi_SQL.Append(Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID + ", ");
                Mi_SQL.Append(Cat_Sap_Partidas_Genericas.Campo_Clave + ", ");
                Mi_SQL.Append(Cat_Sap_Partidas_Genericas.Campo_Descripcion + ", ");
                Mi_SQL.Append(Cat_Sap_Partidas_Genericas.Campo_Estatus + ", ");
                Mi_SQL.Append(Cat_Sap_Partidas_Genericas.Campo_Concepto_ID + ", ");
                Mi_SQL.Append(Cat_Sap_Partidas_Genericas.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Cat_Sap_Partidas_Genericas.Campo_Fecha_Creo + ") VALUES(");
                Mi_SQL.Append("'" + Datos.P_Partida_Generica_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_Clave + "', ");
                Mi_SQL.Append("'" + Datos.P_Descripcion + "', ");
                Mi_SQL.Append("'" + Datos.P_Estatus + "', ");
                Mi_SQL.Append("'" + Datos.P_Concepto_ID + "', ");
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
        /// ********************************************************************************************************************
        /// NOMBRE: Baja_Partida_Generica
        /// 
        /// COMENTARIOS: Esta operación elimina un registro de Partida Generica en la tabla de Cat_Sap_Partida_Generica.
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a insertar en la tabla de Cat_Sap_Partida_Generica.
        /// 
        /// USUARIO CREÓ:Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 25/Febrero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Baja_Partida_Generica(Cls_Cat_Sap_Partida_Generica_Negocio Datos)
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
                Mi_SQL.Append("DELETE FROM " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + " WHERE ");
                Mi_SQL.Append(Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID + "='" + Datos.P_Partida_Generica_ID + "'");

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
        /// NOMBRE: Modificar_Partida_Generica
        /// 
        /// COMENTARIOS: Esta operación modifica un registro de Partida Generica en la tabla de Cat_Sap_Partida_Generica.
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a insertar en la tabla de Cat_Sap_Partida_Generica.
        /// 
        /// USUARIO CREÓ:Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 25/Febrero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Modificar_Partida_Generica(Cls_Cat_Sap_Partida_Generica_Negocio Datos)
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
                Mi_SQL.Append("UPDATE " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + " SET ");
                Mi_SQL.Append(Cat_Sap_Partidas_Genericas.Campo_Clave + "='" + Datos.P_Clave + "', ");
                Mi_SQL.Append(Cat_Sap_Partidas_Genericas.Campo_Descripcion + "='" + Datos.P_Descripcion + "', ");
                Mi_SQL.Append(Cat_Sap_Partidas_Genericas.Campo_Estatus + "='" + Datos.P_Estatus + "', ");
                Mi_SQL.Append(Cat_Sap_Partidas_Genericas.Campo_Concepto_ID + "='" + Datos.P_Concepto_ID + "', ");
                Mi_SQL.Append(Cat_Sap_Partidas_Genericas.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "', ");
                Mi_SQL.Append(Cat_Sap_Partidas_Genericas.Campo_Fecha_Modifico + "=SYSDATE WHERE ");
                Mi_SQL.Append(Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID + "='" + Datos.P_Partida_Generica_ID + "'");

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

        #region(Métodos de Consulta)
        /// ********************************************************************************************************************
        /// NOMBRE: Consulta_Partidas_Genericas
        /// 
        /// COMENTARIOS: Consulta las partidas genericas que existen actualmente en el sistema registrados.
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a insertar en la tabla de Cat_Sap_Partida_Generica.
        /// 
        /// USUARIO CREÓ:Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 25/Febrero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consulta_Partidas_Genericas(Cls_Cat_Sap_Partida_Generica_Negocio Datos)
        {
            DataTable Dt_Partidas_Genericas = null;//Lista de bancos que existen actualmente en el sistema registrados.
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.

            try
            {
                Mi_SQL.Append("SELECT " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + ".*");
                Mi_SQL.Append(" FROM " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas);


                if (!string.IsNullOrEmpty(Datos.P_Partida_Generica_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" OR " + Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID + "='" + Datos.P_Partida_Generica_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID + "='" + Datos.P_Partida_Generica_ID + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Clave))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" OR " + Cat_Sap_Partidas_Genericas.Campo_Clave + "='" + Datos.P_Clave + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Sap_Partidas_Genericas.Campo_Clave + "='" + Datos.P_Clave + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Descripcion))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" OR " + Cat_Sap_Partidas_Genericas.Campo_Descripcion + " LIKE '%" + Datos.P_Descripcion + "%'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Sap_Partidas_Genericas.Campo_Descripcion + " LIKE '%" + Datos.P_Descripcion + "%'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" OR " + Cat_Sap_Partidas_Genericas.Campo_Estatus + "='" + Datos.P_Estatus + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Sap_Partidas_Genericas.Campo_Estatus + "='" + Datos.P_Estatus + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Concepto_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" OR " + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID + "='" + Datos.P_Concepto_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID + "='" + Datos.P_Concepto_ID + "'");
                    }
                }

                Dt_Partidas_Genericas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las partidas genericas que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Partidas_Genericas;
        }
        /// ********************************************************************************************************************
        /// NOMBRE: Consultar_Capitulo_Concepto
        /// 
        /// COMENTARIOS: Consultar el capitulo al que pertence el concepto seleccionado.
        /// 
        /// USUARIO CREÓ:Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 26/Febrero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Capitulo_Concepto(Cls_Cat_Sap_Partida_Generica_Negocio Datos)
        {
            DataTable Dt_Sap_Capitulo = null;//Lista de bancos que existen actualmente en el sistema registrados.
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.

            try
            {
                Mi_SQL.Append(" SELECT CAT_SAP_CAPITULO.* ");
                Mi_SQL.Append(" FROM CAT_SAP_CAPITULO ");
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(" CAPITULO_ID IN (SELECT CAPITULO_ID FROM CAT_SAP_CONCEPTO WHERE CONCEPTO_ID='" + Datos.P_Concepto_ID + "') ");

                Dt_Sap_Capitulo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los el capitulo del concepto. Error: [" + Ex.Message + "]");
            }
            return Dt_Sap_Capitulo;
        }
        /// ********************************************************************************************************************
        /// NOMBRE: Consultar_Sap_Capitulos
        /// 
        /// COMENTARIOS: Consultar el capitulos registrados actualmente en el sistema.
        /// 
        /// USUARIO CREÓ:Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 26/Febrero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Sap_Capitulos()
        {
            DataTable Dt_Sap_Capitulos = null;//Lista de bancos que existen actualmente en el sistema registrados.
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.

            try
            {
                Mi_SQL.Append(" SELECT CAT_SAP_CAPITULO.* ");
                Mi_SQL.Append(" FROM CAT_SAP_CAPITULO ");

                Dt_Sap_Capitulos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Sap_Capitulos que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Sap_Capitulos;
        }
        /// ********************************************************************************************************************
        /// NOMBRE: Consultar_Conceptos_Pertencen_Capitulo
        /// 
        /// COMENTARIOS: Consulta los conceptos de acuerdo al capitulo seleccionado.
        /// 
        /// USUARIO CREÓ:Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 26/Febrero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Conceptos_Pertencen_Capitulo(Cls_Cat_Sap_Partida_Generica_Negocio Datos) {
            DataTable Dt_Sap_Capitulos = null;//Lista de bancos que existen actualmente en el sistema registrados.
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.

            try
            {
                Mi_SQL.Append(" SELECT CAT_SAP_CONCEPTO.* ");
                Mi_SQL.Append(" FROM CAT_SAP_CONCEPTO ");

                if (!string.IsNullOrEmpty(Datos.P_Capitulo_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" OR CAPITULO_ID='" + Datos.P_Capitulo_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE CAPITULO_ID='" + Datos.P_Capitulo_ID + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Concepto_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" OR CONCEPTO_ID='" + Datos.P_Concepto_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE CONCEPTO_ID='" + Datos.P_Concepto_ID + "'");
                    }
                }

                Dt_Sap_Capitulos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Sap_Capitulos que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Sap_Capitulos;
        }
        #endregion
    }
}